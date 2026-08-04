using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Repository;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.NewPortal
{
    public class ProductGraphManager : BaseManager
    {
        IRepositoryWrapper _repositoryWrapper;
        AuthorizedRoleManager _authorizedRoleManager;
        private readonly LCMPADapperQueries _lCMPADapperQueries;
        private readonly CommonDapperRepository _commonDapperRepository;
        private readonly CommonManager _commonManager;
        private DateTime currentDate = System.DateTime.Now.Date;
        private static DateTime today = DateTime.Today;
        private DateTime? preferenceDate = null;
        private DateTime? messagingDate = null;
        private readonly ILoggerManager _loggerManager;
        private readonly string dapperDatabaseMode = "normal";
        private readonly DapperCommonManager _dapperCommonManager;

        public ProductGraphManager(IEnumerable<IRepositoryWrapper> wrappers, AuthorizedRoleManager authorizedRoleManager, ILoggerManager loggerManager,
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonDapperRepository commonDapperRepository, LCMPADapperQueries lCMPADapperQueries,
           CommonManager commonManager, DapperCommonManager dapperCommonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _authorizedRoleManager = authorizedRoleManager;
            _lCMPADapperQueries = lCMPADapperQueries;
            _loggerManager = loggerManager;
            _commonDapperRepository = commonDapperRepository;
            _commonManager = commonManager;
            dapperDatabaseMode = GlobalDbMode.DbMode.ContainsKey(CurrentLogLevelConfig._UserName) ? GlobalDbMode.DbMode[CurrentLogLevelConfig._UserName] : "normal";
            _dapperCommonManager = dapperCommonManager;
        }
        
 
        #region Product Complaince for Software Product Owner
        public IQueryable<MajorSoftwareBuild> GetQueryForProduct(ExpressionStarter<Majorsoftwarebuilds> predicateResult)
        {
            var query = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(predicateResult, false)
                            .Include(x => x.Productname)
                            .Include(x => x.Majorswbuildsdesigncontacts)
                            .AsEnumerable().Select(p => MajorSoftwareBuildMapper.GetProductAndDesignContactMapper(p)).AsQueryable();

            return query;
        }

        public ExpressionStarter<Majorsoftwarebuilds> ApplyFilter(ProductComplainceQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Majorsoftwarebuilds>();
            var predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();

            if (buildFilterDto.MajorSoftwareBuildId != null && buildFilterDto.MajorSoftwareBuildId.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.MajorSoftwareBuildId)
                    predicateInner.Or(x => x.Majorsoftwarebuildsid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignContactIds != null && buildFilterDto.DesignContactIds.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.DesignContactIds)
                    predicateInner.Or(x => x.Majorswbuildsdesigncontacts.Any(c => c.Designcontactid == item));
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public string GetProductCompliance(DateTime? eos, DateTime? eom, EOMEnum eomStatus)
        {
            if (eos == null && eom == null)
            {
                return eomStatus == EOMEnum.NotAnnounced ? ConstantValueFilter.Green : ConstantValueFilter.Red;
            }

            if ((eos.HasValue && eos.Value >= currentDate) || (eom.HasValue && eom.Value >= currentDate))
            {
                return ConstantValueFilter.Green;
            }

            return ConstantValueFilter.Red;
        }

        public Task<IEnumerable<ProductComplainceDto>> GetBaseSoftwareRecords(List<MajorSoftwareBuild> SoftwareEntity)
        {
            IEnumerable<ProductComplainceDto> baseSoftwareEntity = SoftwareEntity.Select(x => new ProductComplainceDto()
            {
                ProductId = x.ProductName.Id,
                ProductName = x.ProductName.Description,
                EndOfMaintenance = x.EndOfMaintenance,
                EndOfSupport = x.EndOfsupport,
                EOMValue = x.EOMStatus == (short)Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified",
                CompatibilityColorCode = GetProductCompliance(x.EndOfsupport, x.EndOfMaintenance, x.EOMStatus),
            });

            return Task.FromResult(baseSoftwareEntity);
        }

        public async Task<ResultDto> FindByCondtionForProductComplaince(long userId , AspNetUserRoleRBODto userDetailsList)
        {
            try
            {
                
                var verticalIds = userDetailsList.VerticalDetails != null && userDetailsList.VerticalDetails.Count > 0 ?
                                  userDetailsList.VerticalDetails : new List<int>();

                var opCoIds = userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                              userDetailsList.OpcoDetails : new List<short>();
               
                ProductComplainceQueryDto queryDto = new ProductComplainceQueryDto();

                var designContactIds = _commonManager.GetDesignContactIdsFromOpcoAndVerticalIds(opCoIds, verticalIds);
                if (designContactIds != null && designContactIds.Count > 0)
                    queryDto.DesignContactIds = designContactIds;



                var predicateResult = ApplyFilter(queryDto);

                var queryList = GetQueryForProduct(predicateResult).ToList();
                if (queryList.Count == 0)
                {
                    return new ResultDto();
                }

                var BaseProductRecords = await GetBaseSoftwareRecords(queryList);
                var productWiseCompliance = await GetProductWisePercentage(BaseProductRecords);

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = new
                    {
                        productWiseCompliance,
                        RoleName = "SW Product Owner"
                    }
                };
            }
            catch (Exception ex)
            {

                _loggerManager.LogError(ex, "FindByCondtionForProductComplaince failed");
                throw;
            }
        }

        public Task<ProductComplianceSummaryDto> GetProductWisePercentage(IEnumerable<ProductComplainceDto> productEntities)
        {
            var entityList = productEntities as IList<ProductComplainceDto> ?? productEntities.ToList();

            var products = entityList
                .GroupBy(x => x.ProductName)
                .Select(g =>
                {
                    var greenCount = g.Count(x => x.CompatibilityColorCode == ConstantValueFilter.Green);
                    var redCount = g.Count(x => x.CompatibilityColorCode == ConstantValueFilter.Red);
                    var totalCount = g.Count();

                    return new ProductComplainceDto
                    {
                        ProductId = g.First().ProductId,
                        ProductName = g.Key,
                        CompliantProductCount = greenCount.ToString(),
                        NonCompliantProductCount = redCount.ToString(),
                        TotalProductCount = totalCount.ToString()
                    };
                })
                .OrderByDescending(x => x.ProductName).ToList();

            var distinctProducts = entityList.ToList();
            var totalGreenCount = distinctProducts.Count(x => x.CompatibilityColorCode == ConstantValueFilter.Green);
            var totalRedCount = distinctProducts.Count(x => x.CompatibilityColorCode == ConstantValueFilter.Red);
            var overallProductCount = totalGreenCount + totalRedCount;

            string CalculateProductPercentage(int count, int total) =>
                total > 0 && count > 0 ? ((decimal)count / total * 100).ToString("0.#") : "0";

            return Task.FromResult(new ProductComplianceSummaryDto
            {
                Products = products,
                OverallProductCount = overallProductCount,
                OverallComplaintProductCount = totalGreenCount,
                OverallNonComplaintProductCount = totalRedCount,
                OverallGreenPercentage = CalculateProductPercentage(totalGreenCount, overallProductCount),
                OverallRedPercentage = CalculateProductPercentage(totalRedCount, overallProductCount)
            });
        }

        #endregion
        #region Product Complaince for Hardware Product Owner
        public IQueryable<Majorhardwarebuilds> GetQueryForHardwarePlatform(ExpressionStarter<Majorhardwarebuilds> predicateResult)
        {
            var query = _repositoryWrapper.MajorHardwareBuild.FindByCondition(predicateResult, false)
                            .Include(x => x.Platform)
                            .Include(x => x.Majorhwbuildsdesigncontacts)
                            .AsNoTracking();

            return query;
        }

        public ExpressionStarter<Majorhardwarebuilds> ApplyFilterForHwProductOwner(ProductComplainceQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Majorhardwarebuilds>();
            var predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();

            if (buildFilterDto.MajorSoftwareBuildId != null && buildFilterDto.MajorSoftwareBuildId.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.MajorSoftwareBuildId)
                    predicateInner.Or(x => x.Majorhardwareid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignContactIds != null && buildFilterDto.DesignContactIds.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.DesignContactIds)
                    predicateInner.Or(x => x.Majorhwbuildsdesigncontacts.Any(c => c.Designcontactid == item));
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
         

        public async Task<ResultDto> GetHardwareProductOwnerComplaince(long userId, AspNetUserRoleRBODto userDetailsList)
        {
            try
            {
                
                var verticalIds = userDetailsList.VerticalDetails != null && userDetailsList.VerticalDetails.Count > 0 ?
                                  userDetailsList.VerticalDetails : new List<int>();

                var opCoIds = userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                              userDetailsList.OpcoDetails : new List<short>();
                if (userDetailsList?.RoleRecords?.Any(x =>
                     string.Equals(x.RoleName, "HW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = null
                    };
                }
                ProductComplainceQueryDto queryDto = new ProductComplainceQueryDto();

                var designContactIds = _commonManager.GetDesignContactIdsFromOpcoAndVerticalIds(opCoIds, verticalIds);
                if (designContactIds != null && designContactIds.Count > 0)
                    queryDto.DesignContactIds = designContactIds;



                var predicateResult = ApplyFilterForHwProductOwner(queryDto);

                var queryList = GetQueryForHardwarePlatform(predicateResult).ToList();
                if (queryList.Count == 0)
                {
                    return new ResultDto();
                }

                var BaseProductRecords = queryList.Select(x => new ProductComplainceDto()
                {
                    ProductId = x.Platformid,
                    ProductName = x.Platform.Platform,
                    EndOfMaintenance = x.Endofmaintenance,
                    EndOfSupport = x.Endofsupport,
                    EOMValue = x.Eomstatus == (short)Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified",
                    CompatibilityColorCode = GetProductCompliance(x.Endofsupport, x.Endofmaintenance, (EOMEnum)x.Eomstatus),
                });
                var productWiseCompliance = await GetProductWisePercentage(BaseProductRecords);

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = new
                    {
                        productWiseCompliance,
                        RoleName ="HW Product Owner"
                    }
                };
            }
            catch (Exception ex)
            {

                _loggerManager.LogError(ex, "GetHardwareProductOwnerComplaince Failed");
                throw;
            }
        }
 
        #endregion

    }

}
