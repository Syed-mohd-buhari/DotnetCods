using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.LookUp.SubNetworkBoundary;
using CAM.DataTransferObjects.LookUp.VodafoneName;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using System.Transactions;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using System.Collections.Immutable;
using DocumentFormat.OpenXml.Office2010.Excel;
using AutoMapper.Configuration.Annotations;
using CAM.Entities.Models;
using IdentityServer4.Extensions;
using System.Globalization;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CAM.BusinessManager.LookUp
{
    public class VodafoneNameManager : BaseManager,IVodafoneNameManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;

        public VodafoneNameManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IMapper mapper, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) :base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
        }
        public async Task<ResultDto> Add(VodafoneNameDto dto)
        {
            var entityExists = new Vodafonenames();
            try
            {
                entityExists = await _repositoryWrapper.VodafoneNameRepository.FindByCondition(
                  x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
                  , true).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }

            VodafoneNames entity = new VodafoneNames()
            {
                Id = dto.Id,
                Description = dto.Description,

            };
            var vodafoneName = VodafoneNameMapper.SetVodafoneNameMapper(entity);
            var relatedProductNames = new Productname(); ;
            _repositoryWrapper.VodafoneNameRepository.Create(vodafoneName);
            await _repositoryWrapper.SaveAsync();

            foreach (var productNameId in dto.ProductNameIds)
            {

                relatedProductNames = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == productNameId).FirstOrDefault();
                relatedProductNames.Vodafonenamesid = vodafoneName.Id;
                _repositoryWrapper.ProductNameRepository.Update(relatedProductNames);
            }
            await _repositoryWrapper.SaveAsync();

            // 754 Link the RiskClusterIDs
            var resultriskClusterVfmapp = await CreateRiskClusterIdforVf(dto.Description,dto.RiskClusterId);
            if (resultriskClusterVfmapp.Warning)
            {
                return new ResultDto { Info = "Some error throw while Linked the Risk Cluster on VF Name", Warning = true};
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }
        public async Task<ResultDto> Update(VodafoneNameDto dto)
        {
            var entityExists = await _repositoryWrapper.VodafoneNameRepository.FindByCondition(
                   x => x.Id != dto.Id
                   && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
                   && !x.Deleted.Value).Include(x => x.Productname).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            VodafoneNames entity = new VodafoneNames()
            {
                Id = dto.Id,
                Description = dto.Description,
            };
            var updatedVodafoneName = VodafoneNameMapper.SetVodafoneNameMapper(entity);
            _repositoryWrapper.VodafoneNameRepository.Update(updatedVodafoneName);
            _repositoryWrapper.Save();

            //754 Update the riskCluster for related VF
            var updatdRiskClusterVFmapp = await UpdateRiskClusterIdforVf(dto.RiskClusterId,dto.RiskClusterVodafoneNameMapId,dto.Id);
            if (updatdRiskClusterVFmapp.Warning)
            {
                return new ResultDto() { Info = "Some error throw while update the Riskcluster" };
            }


            var oldSoftawreApplicationTypes = await _repositoryWrapper.ProductNameRepository
                .FindByCondition(x => x.Vodafonenamesid == dto.Id)
                .Include(x => x.Vodafonenames).ToListAsync();


            if (oldSoftawreApplicationTypes != null && oldSoftawreApplicationTypes.Count != 0)
            {
                foreach (var item in oldSoftawreApplicationTypes)
                {
                    item.Vodafonenamesid = null;
                    _repositoryWrapper.ProductNameRepository.Update(item);

                }
            }

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            if (dto.ProductNameIds != null && dto.ProductNameIds.Count != 0)
            {
                foreach (var productNameId in dto.ProductNameIds)
                {
                    var relatedProductNames = await _repositoryWrapper.ProductNameRepository
                        .FindByCondition(x => x.Productnameid == productNameId)
                        .Include(x => x.Vodafonenames).FirstOrDefaultAsync();

                    relatedProductNames.Vodafonenamesid = dto.Id;
                    _repositoryWrapper.ProductNameRepository.Update(relatedProductNames);

                }
            }

            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public ExpressionStarter<Vodafonenames> ApplyFilterForOracleModel(VodafoneNameDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Vodafonenames>();
            var predicateInner = PredicateBuilder.New<Vodafonenames>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Vodafonenames>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }
            if (request.RiskCluster != null && request.RiskCluster.Any())
            {
                predicateInner = PredicateBuilder.New<Vodafonenames>();
                foreach (var item in request.RiskCluster)
                    predicateInner.Or(x => x.Riskclustervodafonenames.Where(x=>x.Riskcluster != null).Any(x=>x.Riskcluster.Description == item));
                predicateResult.And(predicateInner);
            }
            if (request.RiskLevel != null && request.RiskLevel.Any())
            {
                predicateInner = PredicateBuilder.New<Vodafonenames>();
                foreach (var item in request.RiskLevel)
                    predicateInner.Or(x => x.Riskclustervodafonenames.Where(x => x.Riskcluster != null).Any(x => x.Riskcluster.Risklevel == item));
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Vodafonenames>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Vodafonenames>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Vodafonenames>();
                if (request.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModifiedValue.StartDate);
                if (request.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Vodafonenames>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            if (request.ProductName != null && request.ProductName.Any())
            {
                predicateInner = PredicateBuilder.New<Vodafonenames>();
                foreach (var item in request.ProductName)
                    predicateInner.Or(x => x.Productname.Any(d => d.Productnameid == item));
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public List<VodafoneNameDtoGrid> CastObjectToDto(IQueryable<VodafoneNames> request)
        {

            var test = _repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Id == request.FirstOrDefault().Id).SelectMany(x=>x.Systemtypes);
            var model = request.ToList().Select(dto => new VodafoneNameDtoGrid()
            {
                Id = dto.Id,
                Description = dto.Description,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                Deleted = dto.Deleted,
                Orphan = !_repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Id == dto.Id).SelectMany(x => x.Systemtypes).Any() || !_repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Id == dto.Id).SelectMany(x => x.Subnetworkboundaries).Any(),
                ProductName = dto.ProductNames != null ? string.Join(" | ", dto.ProductNames.Select(fx => fx.Description).Distinct()) : "",
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                RiskCluster = dto.RiskClusterVodafoneNames.Where(x=>x.Riskcluster!=null).Select(y=>y.Riskcluster.Description).FirstOrDefault(),
                RiskLevel = dto.RiskClusterVodafoneNames.Where(x => x.Riskcluster != null).Select(y => y.Riskcluster.Risklevel).FirstOrDefault()
            }).ToList();

            return model;
        }
        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.VodafoneNameRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.VodafoneNameRepository
            .FindByCondition(x => x.Id == id)
            .SingleAsync();

            var relatedProductName = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Vodafonenamesid == id).ToList();
            foreach (var item in relatedProductName)
            {
                item.Vodafonenamesid = null;
                _repositoryWrapper.ProductNameRepository.Update(item);
            }
            var relatedSubNetworkBoundaries = _repositoryWrapper.SubNetworkBoundaries.FindByCondition(x => x.Vodafonenameid == id).ToList();
            foreach (var item in relatedSubNetworkBoundaries)
            {
                item.Vodafonenameid = null;
                _repositoryWrapper.SubNetworkBoundaries.Update(item);
            }
            var relatedSystemTypes= _repositoryWrapper.SystemType.FindByCondition(x => x.Vodafonename == id,true).ToList();
            foreach (var item in relatedSystemTypes)
            {
                item.Vodafonename = null;
                _repositoryWrapper.SystemType.Update(item);
            }
            var relatedRiskClusterVfmap = _repositoryWrapper.RiskClusterVodafoneNamesRepository.FindByCondition(x => x.Vodafonenameid == id).ToList();
            foreach(var item in relatedRiskClusterVfmap)
            {
                item.Vodafonenameid = null;
                _repositoryWrapper.RiskClusterVodafoneNamesRepository.Update( item);
            }
            await _repositoryWrapper.SaveAsync();

            _repositoryWrapper.VodafoneNameRepository.DeleteDeep(entity);

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public Dictionary<string, Expression<Func<VodafoneNames, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VodafoneNames, object>>[]>
            {
                ["Description"] = new Expression<Func<VodafoneNames, object>>[] { p => p.Description },
                ["Id"] = new Expression<Func<VodafoneNames, object>>[] { p => p.Id },
                ["lastModifiedBy"] = new Expression<Func<VodafoneNames, object>>[] { p => p.ModificationUserEntity.Email },
                ["productNames"] = new Expression<Func<VodafoneNames, object>>[] { p => p.ProductNames },
            };
        }

        public QueryResultDto<VodafoneNameDtoGrid> GetEnityGrid(VodafoneNameDtoQuery request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            if (request.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            if (request.Orphan == true)
            {
                predicateResult.And(x => !x.Systemtypes.Any() && !x.Subnetworkboundaries.Any());
            }

            var query = PrepareQuery(request, null, predicateResult);

            int numberOfElements = query.Count();
            query = query.ToList().AsQueryable().ApplyOrdering(request, GetColumnsMap()).ApplyPaging(request);
            var dataResult = CastObjectToDto(query);
            var rtn = new QueryResultDto<VodafoneNameDtoGrid>(new GenerateRenderForGrid<VodafoneNameDtoGrid>(_columnManager))
            {
                Items = dataResult,
                TotalItems = numberOfElements,
            };
           // rtn.GridRender.Render.Find(x => x.PropertyName.ToLower().Trim() == "riskcluster").Show = true;
            return rtn;
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, VodafoneNameDtoQuery request)
        {
            request.PageSize = 0;
            request.Page = 1;
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = PrepareQuery(request, null, predicateResult);

            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();
            return data;
        }
        public async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<VodafoneNames> request, string propertyName, string propertyFilter)
        {
            var tt = request.ToList().Where(x => x.RiskClusterVodafoneNames != null ).Select(x => x.RiskClusterVodafoneNames.Where
            (x => x.Riskcluster !=null).FirstOrDefault ()?.Riskcluster?.Risklevel);

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
                        ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                        : request.Where(x =>
                            x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),

                "productName" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Where(p => p.ProductNames != null && p.ProductNames.Count > 0)
                        .SelectMany(x => x.ProductNames).Select(p => new FilterValueDto(p.Id.ToString(), p.Description)).Distinct().ToList()
                        : request.Where(x => x.ProductNames != null && x.ProductNames.Any(s => s.Description.ToUpper()
                        .Contains(propertyFilter.ToUpper())))
                        .SelectMany(x => x.ProductNames)
                        .Select(p => new FilterValueDto(p.Id.ToString(), p.Description)).Distinct()
                        .ToList(),

                "riskCluster" =>
                 request.ToList().Where(x => x.RiskClusterVodafoneNames != null).Select(x => new FilterValueDto(x.RiskClusterVodafoneNames.Where
          (x => x.Riskcluster != null)
          .Select (x => string .IsNullOrWhiteSpace(  Convert.ToString(x.Riskcluster?.Description)) ? null :
          x.Riskcluster?.Description)
          .FirstOrDefault())).Distinct().ToList(),

                "riskLevel" =>
                 request.ToList().Where(x => x.RiskClusterVodafoneNames != null).Select(x => new FilterValueDto(x.RiskClusterVodafoneNames.Where
          (x => x.Riskcluster != null)
          .Select(x => string.IsNullOrWhiteSpace(Convert.ToString(x.Riskcluster?.Risklevel)) ? null :
          x.Riskcluster?.Risklevel)
          .FirstOrDefault())).Distinct().ToList(),

            };
        }
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var subnetworkBoundariesRelated = _repositoryWrapper.VodafoneNameRepository
               .FindByCondition(x => x.Id == id).Include(x=>x.Subnetworkboundaries)
               .SelectMany(x => x.Subnetworkboundaries.Select(x=> !string.IsNullOrEmpty(x.Alias) ? x.Alias :x.Description)).ToArray();

            var systemTypesRelated = _repositoryWrapper.SystemType.FindByCondition(x => x.Vodafonename == id).ToArray();

            List<string> systemTypesRelatedStrings = new List<string>();
            if (systemTypesRelated !=null && systemTypesRelated.Length>0)
            {
                foreach (var item in systemTypesRelated)
                {
                    systemTypesRelatedStrings.Add(item.SystemTypeName(_repositoryWrapper));
                }
            }

            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            if (systemTypesRelated != null && systemTypesRelated.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "System Types Related ", Values = systemTypesRelatedStrings.ToArray() });

            if (subnetworkBoundariesRelated != null && subnetworkBoundariesRelated.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "SubnetworkBoundaries Related", Values = subnetworkBoundariesRelated });

            var entity = await _repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Id == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Vodafone Name",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public VodafoneNameDtoGrid GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Id == id)
                        .Include(x => x.ModificationuserNavigation)
                        .Include(x => x.Productname)
                        .Include(x=>x.Riskclustervodafonenames).ThenInclude(x=>x.Riskcluster)
                        .FirstOrDefault();
            var riskClusterResource = _repositoryWrapper.RiskClusterRepository.FindAll().ToDictionary(x => x.Riskclusterid, y => y.Description);
            var riskClusterSeverityResource = _repositoryWrapper.RiskClusterRepository.FindAll().ToDictionary(x => x.Riskclusterid, y => y.Risklevel);

            var entity = VodafoneNameMapper.GetVodafoneNamesMapper(model);

            var dto = new VodafoneNameDtoUpdate()
            {
                Id = entity.Id,
                Description = entity.Description,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email.ToString(),
                ProductNameIds = entity.ProductNames != null ? entity.ProductNames.Select(p => p.Id).ToList() : null,
            };
            var ProductNamesResource = _repositoryWrapper.ProductNameRepository.Count() != 0
                ? _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Vodafonenamesid == null)
                .ToDictionary(k => k.Productnameid, v => v.Description) : null;
            dto.LinkedProductNames = new Dictionary<decimal, string>();
            dto.ProductNameResource = new List<ProductNameResourceModel>();
            dto.NotLinkedProductNames = new Dictionary<decimal, string>();
            #region//794 add Risk cluster fields.



            dto.RiskClusterId = entity.RiskClusterVodafoneNames.Where(x=>x.Riskcluster!=null).Select(y => y.Riskcluster.Riskclusterid).FirstOrDefault();
            dto.RiskClusterVodafoneNameMapId = entity.RiskClusterVodafoneNames.Where(x => x.Riskcluster != null).Select(y => y.Riskclustervodafonenameid).FirstOrDefault();
            dto.RiskClusterResource = riskClusterResource;
            dto.RiskClusterSeverityResource = riskClusterSeverityResource;



            #endregion
            foreach (var item in ProductNamesResource)
            {
                dto.ProductNameResource.Add(new ProductNameResourceModel
                {
                    Id = item.Key,
                    Description = item.Value,
                    EnableDelete = true,
                });

                dto.NotLinkedProductNames.Add(item.Key,item.Value);
            }
            foreach (var item in model.Productname)
            {
                var canDelete = !_repositoryWrapper.SystemType.FindByCondition(x => x.Majorsoftwarebuilds.Productnameid == item.Productnameid)
                    .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname).ToList().Any();
                dto.ProductNameResource.Add(new ProductNameResourceModel
                {
                    Id = item.Productnameid,
                    Description = item.Description,
                    EnableDelete = canDelete
                });

                dto.LinkedProductNames.Add(item.Productnameid,item.Description);
            }


            return dto;
        }

        public VodafoneNameDtoGrid GetCreatePage()
        {
            var productNameResource = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Vodafonenamesid == null);
            var riskCluster = _repositoryWrapper.RiskClusterRepository.FindAll();
            var riskClusterSeverityResource = _repositoryWrapper.RiskClusterRepository.FindAll().ToDictionary(x => x.Riskclusterid, y => y.Risklevel);


            var dto = new VodafoneNameDtoCreate();
            dto.ProductNameResource = new List<ProductNameResourceModel>();
            dto.RiskClusterResource = new Dictionary<int, string>();
            dto.RiskClusterResource = riskCluster.ToDictionary(x => x.Riskclusterid, f => f.Description);
            dto.RiskClusterSeverityResource = riskClusterSeverityResource;
            foreach (var item in productNameResource)
            {
                dto.ProductNameResource.Add(new ProductNameResourceModel
                {
                    Id = item.Productnameid,
                    Description = item.Description,
                    EnableDelete = true,
                });
            }
            return dto;
        }
        public IQueryable<VodafoneNames> PrepareQuery(VodafoneNameDtoQuery request, ExpressionStarter<VodafoneNames> predicateResult, ExpressionStarter<Vodafonenames> oracleObject = null)
        {
            var query = oracleObject.IsStarted
               ? _repositoryWrapper.VodafoneNameRepository.FindByCondition(oracleObject)
               .Include(x => x.Productname)
               .Include(x => x.Systemtypes)
               .Include(m => m.ModificationuserNavigation)
               .Include(m => m.CreationuserNavigation)
               .Include(x=>x.Riskclustervodafonenames).ThenInclude(x=>x.Riskcluster)
               : _repositoryWrapper.VodafoneNameRepository.FindAll()
               .Include(x => x.Productname)
               .Include(x => x.Systemtypes)
               .Include(m => m.ModificationuserNavigation)
               .Include(m => m.CreationuserNavigation)
               .Include(x => x.Riskclustervodafonenames).ThenInclude(x=>x.Riskcluster);

            return query.AsEnumerable().Select(p => VodafoneNameMapper.GetVodafoneNamesMapper(p)).AsQueryable();
        }

        /// <summary>
        /// This fuction is used to link the riskcluster in Vf name
        /// </summary>
        /// <param name="vfName"></param>
        /// <param name="riskClusterIds"></param>
        /// <returns></returns>
        public async Task<ResultDto> CreateRiskClusterIdforVf(string vfName, int riskClusterIds)
        {
            try
            {        
                var vfId = _repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Description.ToLower().Trim() == vfName.ToLower().Trim()).FirstOrDefault().Id;
                   
                if (vfId != null && vfId > 0)
                {
                    Riskclustervodafonenames entity = new Riskclustervodafonenames()
                    {
                        Vodafonenameid = vfId,
                        Riskclusterid = riskClusterIds>0? riskClusterIds:null,
                    };
                    //var riskClusterVfMapping = RiskClusterVodafoneNameMapper.Set(entity);
                    _repositoryWrapper.RiskClusterVodafoneNamesRepository.Create(entity);
                    _repositoryWrapper.Save();
                    await _repositoryWrapper.ClearTracker();
                }                   
                return new ResultDto()
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Warning = false,
                };
                              
            }
            catch (Exception ex)
            {
                return new ResultDto()
                {
                    Info = ex.Message,
                    Warning = true,
                };
            }
        }


        /// <summary>
        /// This function is used to delete the unliked Riskcluster record and if the new record is came we are going to insert that record.
        /// </summary>
        /// <param name="vfId"></param>
        /// <param name="riskClusterIds"></param>
        /// <returns></returns>
        public async Task<ResultDto> UpdateRiskClusterIdforVf(int riskClusterId ,int riskClustervfnamemapId,int vfId)
        {
            try
            {
                if (riskClustervfnamemapId != 0)
                {
                    var getRiskVFMappeRecord = _repositoryWrapper.RiskClusterVodafoneNamesRepository.FindByCondition(x => x.Riskclustervodafonenameid == riskClustervfnamemapId).FirstOrDefault();
                    if (getRiskVFMappeRecord != null)
                    {
                        getRiskVFMappeRecord.Riskclusterid = riskClusterId;
                        _repositoryWrapper.RiskClusterVodafoneNamesRepository.Update(getRiskVFMappeRecord);

                        _repositoryWrapper.Save();
                        await _repositoryWrapper.ClearTracker();
                    }

                    return new ResultDto() { Warning = false, };
                }
                else
                {
                    Riskclustervodafonenames entity = new Riskclustervodafonenames()
                    {
                        Vodafonenameid = vfId,
                        Riskclusterid = riskClusterId,
                    };
                    _repositoryWrapper.RiskClusterVodafoneNamesRepository.Create(entity);
                    _repositoryWrapper.Save();
                    await _repositoryWrapper.ClearTracker();
                    return new ResultDto() { Warning = false, };

                }
            }

            catch (Exception ex)
            {
                return new ResultDto() { Warning = true, };
            }
        }

    }
}
