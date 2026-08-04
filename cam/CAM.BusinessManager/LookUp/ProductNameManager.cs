using AutoMapper;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.LookUp
{
    public class ProductNameManager : BaseManager,IProductNameManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly DesignComponentManager _dcManger;
        private readonly IMapper _mapper;

        #region Customized_Grid_Methods
        public ProductNameManager(IEnumerable<IRepositoryWrapper> wrappers, DesignComponentManager dcManager, GridCustomColumnManager columnManager, IMapper mapper, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) :base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
            _dcManger = dcManager;
        }
        public ExpressionStarter<Productname> ApplyFilterForOracleModel(ProductNameDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Productname>();
            var predicateInner = PredicateBuilder.New<Productname>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Productname>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Productname>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Productnameid == item);
                predicateResult.And(predicateInner);
            }
            if (request.IsPlatformSoftware != null && request.IsPlatformSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Productname>();
                foreach (var item in request.IsPlatformSoftware)
                    if (item.ToLower() == ConstantValueFilter.yes.ToLower())
                    {
                        predicateInner.Or(x => x.Isplatformsoftware == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Isplatformsoftware == false);
                    }
                predicateResult.And(predicateInner);

            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Productname>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Productname>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Productname>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            if (request.Orphan == true)
            {
                /// dependent entities
            }
            if (request.VodafoneName != null && request.VodafoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Productname>();
                foreach (var item in request.VodafoneName)
                    predicateInner.Or(x => x.Vodafonenames.Description == item);
                predicateResult.And(predicateInner);
            }

            return predicateResult;

        }
        public List<ProductNameDtoGrid> CastObjectToDto(IQueryable<ProductName> request)
        {
            var model = request.Select(dto => new ProductNameDtoGrid()
            {
                Id = dto.Id,
                Description = dto.Description,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                Deleted = dto.Deleted,
                VodafoneName = dto.VodafoneName != null ? dto.VodafoneName.Description:null,
                VodafoneNameId = dto.VodafoneNameId,
                IsPlatformSoftware = dto.IsPlatformSoftware == true ? ConstantValueFilter.Yes : ConstantValueFilter.No
            }).ToList();

            return model;
        }
        public Dictionary<string, Expression<Func<ProductName, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ProductName, object>>[]>
            {
                ["Description"] = new Expression<Func<ProductName, object>>[] { p => p.Description },
                ["Id"] = new Expression<Func<ProductName, object>>[] { p => p.Id },
                ["lastModifiedBy"] = new Expression<Func<ProductName, object>>[] { p => p.ModificationUserEntity.Email },
                ["vodafoneName"] =new Expression<Func<ProductName, object>>[] {p=>p.VodafoneName.Description},
            };
        }
        public async Task<QueryResultDto<ProductNameDtoGrid>> GetEnityGrid(ProductNameDtoQuery request)
        { 
            var predicateResult = ApplyFilterForOracleModel(request);

            var rtn = new QueryResultDto<ProductNameDtoGrid>(new GenerateRenderForGrid<ProductNameDtoGrid>(_columnManager))
            {

            };
            var query = PrepareQuery(request, null, predicateResult);
            int numberOfElements = query.Count();
            rtn.TotalItems = numberOfElements;
            query = query.ToList().AsQueryable().ApplyOrdering(request, GetColumnsMap()).ApplyPaging(request);
            var dataResult = CastObjectToDto(query);
            rtn.Items= dataResult;
            rtn.GridRender.Render.Find(x => x.PropertyName.ToLower().Trim() == "vodafonename").Show = true;
            return rtn;
            
        }
        public ProductNameDtoGrid GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == id)
                 .Include(x => x.ModificationuserNavigation).Include(x=>x.Vodafonenames)
                 .Single();
            var vodafoneNameResource = _repositoryWrapper.VodafoneNameRepository.FindAll().ToDictionary(x => x.Id, y => y.Description);
            var entity = ProductNameMapper.GetProductNameMapper(model);


            var dto = new ProductNameDtoUpdate()
            {
                Id = entity.Id,
                Description = entity.Description,
                IsPlatformSoftware =  entity.IsPlatformSoftware ==true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email.ToString(),
                VodafoneName = entity.VodafoneName != null ?entity.VodafoneName.Description:null,
                VodafoneNameId = entity.VodafoneName !=null ?entity.VodafoneName.Id : null,
            };
            dto.VodafoneNameResource = new Dictionary<int, string>();
            dto.VodafoneNameResource = vodafoneNameResource;

            return dto;
        }
        public async Task<ResultDto> GetRelatedRecords(short id)
        {

            var majorSoftwareBuilds = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Productnameid == id)
                .Include(x => x.Orgeqpmanufacturer)
                .Include(x=>x.Productname)
                .Select(x => x.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " +
                x.Productname.Description != null ? x.Productname.Description:"" + " - " + x.Softwareversion).ToArray();
            
            var vodafoneNames = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == id)
                .Include(x => x.Vodafonenames).Select(x => x.Vodafonenames.Description).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (majorSoftwareBuilds.Count(x => x != null) > 0)
                rm.Add(new ResultMessageDto() { Table = "Major Software Builds", Values = majorSoftwareBuilds });
            if (vodafoneNames.Count(x => x != null) > 0)
                rm.Add(new ResultMessageDto() { Table = "Vodafone Names", Values = vodafoneNames });

            var entity = await _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Product Name",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, ProductNameDtoQuery request)
        {
            request.PageSize = 0;
            request.Page = 1;
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = PrepareQuery(request, null, predicateResult);

            var data =  await Task.Run(()=> GetFilterValueList(query, propertyName, propertyFilter))    ;
            return data.ToList();
        }
        public async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<ProductName> request, string propertyName, string propertyFilter)
        {
             return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description)) : request.Where(x =>
                   x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                        : request
                            .Where(x =>
                                x.ModificationUserEntity.Email.Contains(
                                    propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                        ? request.ToList().Select(x => new FilterValueDto(x.Id.ToString()))
                        : request.Where(x =>
                            x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),

                "vodafoneName" => string.IsNullOrEmpty(propertyFilter) ? request.ToList().Where(x => x.VodafoneName != null).Select(x => new FilterValueDto(x.VodafoneName.Description)) : request.Where(x =>
                                     x.VodafoneName.Description.Contains(propertyFilter)).Where(x => x.VodafoneName != null).Select(x => new FilterValueDto(x.VodafoneName.Description)),

                "isPlatformSoftware" => string.IsNullOrEmpty(propertyFilter)
         ? request.ToList().Select(p => new FilterValueDto(
              p.IsPlatformSoftware == true ? ConstantValueFilter.Yes : ConstantValueFilter.No))
               .Distinct()
               .ToList()
         : request.ToList().Where(x => x.IsPlatformSoftware.ToString().Contains(propertyFilter))
               .Select(p => new FilterValueDto(
              p.IsPlatformSoftware == true ? ConstantValueFilter.Yes : ConstantValueFilter.No))
               .Distinct()
               .ToList(),
            } ; 
        }
        public IQueryable<ProductName> PrepareQuery(ProductNameDtoQuery request, ExpressionStarter<ProductName> predicateResult, ExpressionStarter<Productname> oracleObject = null)
        {
            var query = oracleObject.IsStarted
               ? _repositoryWrapper.ProductNameRepository.FindByCondition(oracleObject).Include(x=>x.Vodafonenames)
               : _repositoryWrapper.ProductNameRepository.FindAll().Include(x => x.Vodafonenames);

            return query
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => ProductNameMapper.GetProductNameMapper(p)).AsQueryable();
        }
        #endregion
        public async Task<ResultDto> Add(ProductNameDto dto)
        {
            var entityExists = await _repositoryWrapper.ProductNameRepository.FindByCondition(
               x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Productnameid
                };
            }

            ProductName entity = new ProductName()
            {
                Id = dto.Id,
                Description = dto.Description,
                VodafoneNameId = dto.VodafoneNameId,
                IsPlatformSoftware = dto.IsPlatformSoftware == null ? false : (dto.IsPlatformSoftware == ConstantValueFilter.False) ? false : true 
            };

            _repositoryWrapper.ProductNameRepository.Create(ProductNameMapper.SetProductNameMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }
        public async Task<ResultDto> Update(ProductNameDto dto)
        {
            var entityExists = await _repositoryWrapper.ProductNameRepository.FindByCondition(
               x => x.Productnameid != dto.Id
               && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               && !x.Deleted.Value).Include(x=>x.Vodafonenames).FirstOrDefaultAsync();

            var vfId = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == dto.Id).FirstOrDefault().Vodafonenamesid;
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Productnameid
                };
            }
            ProductName entity = new ProductName()
            {
                Id = dto.Id,
                Description = dto.Description,
                VodafoneNameId = dto.VodafoneNameId,
                IsPlatformSoftware = dto.IsPlatformSoftware == null ? false : (dto.IsPlatformSoftware == ConstantValueFilter.False) ? false : true,
            };
            _repositoryWrapper.ProductNameRepository.Update(ProductNameMapper.SetProductNameMapper(entity));
           

            // update related VF id to system type
            await CreateNewDcForNewVFnameUpdateOnProductName(dto.VodafoneNameId, dto.Id, vfId);

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public ProductNameDtoCreate GetCreatePage()
        {           
            var dto = new ProductNameDtoCreate();
            dto.VodafoneNameResource = new Dictionary<int, string>();
            dto.VodafoneNameResource = _repositoryWrapper.VodafoneNameRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
  
            
            return dto;
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == id).SingleAsync();
            _repositoryWrapper.ProductNameRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Productnameid
            };
        }
        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.ProductNameRepository
                .FindByCondition(x => x.Productnameid == id)
                .SingleAsync();
            _repositoryWrapper.ProductNameRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Productnameid
            };
        }

        /// <summary>
        /// This fucntion is used for when we update the Vfname for any productname, we are going to create a DC. for that vfname it has a subnetworkboundry we are going to check that subnetworkboundry has dc or not if it is there we don't create a dc if it
        /// not there we will create a Dc for that subnetworkboundry.
        /// </summary>      
        public async Task<ResultDto> CreateNewDcForNewVFnameUpdateOnProductName(int? vodafoneId, decimal productId, int? existingVfId)
        {
            var getSystemType = _repositoryWrapper.SystemType.FindByCondition(x => x.Vodafonename == existingVfId && x.Majorsoftwarebuilds.Productnameid == productId).ToList();
  
            var getUpdateVfRelatedsubnetworkBoundry = _repositoryWrapper.SubNetworkBoundaries.FindByCondition(x => x.Vodafonenameid == vodafoneId).ToList();

            if (getSystemType != null && getSystemType.Count() > 0)
            {
                foreach (var item in getSystemType)
                {
                    var getSystemRelatedDC = _repositoryWrapper.DesignComponent.FindByCondition(x=>x.Systemtypeid == item.Systemtypeid).Include(x=>x.Subnetworkboundary).ToList();
                    foreach(var sub in getUpdateVfRelatedsubnetworkBoundry)
                    {
                        if(!(getSystemRelatedDC.Any(x=>x.Subnetworkboundary.Alias == sub.Alias)))
                        {
                            var designDto = new DesignComponentDtoCreate();
                            if(sub.Alias.ToLower().Trim() == "allsupportedservices")
                            {
                                designDto.VisibleFlag = true;
                            }
                            else
                            {
                                designDto.VisibleFlag = false;
                            }
                            designDto.SystemTypeId = item.Systemtypeid;
                            designDto.SubNetworkBoundaryIds = new List<long>();
                            designDto.SubNetworkBoundaryIds.Add(sub.Id);
                            await _dcManger.Add(designDto);
                        }
                    }
                    if (item.Vodafonename != vodafoneId)
                    {
                        item.Vodafonename = vodafoneId;
                        _repositoryWrapper.SystemType.Update(item);
                    }
                }
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();
            }
 
            return new ResultDto
            {

            };
        }
        

    }
}
