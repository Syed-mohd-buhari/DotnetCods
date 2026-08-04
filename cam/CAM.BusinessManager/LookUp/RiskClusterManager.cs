using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.RiskCluster;
using CAM.DataTransferObjects.LookUp.VodafoneName;
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
    public class RiskClusterManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;

       
        public RiskClusterManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, IMapper mapper, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) :base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
        }
        #region UI Member function
        public ExpressionStarter<Riskclusters> ApplyFilterForOracleModel(RiskClusterQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Riskclusters>();
            var predicateInner = PredicateBuilder.New<Riskclusters>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Riskclusters>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.RiskClusterId != null && request.RiskClusterId.Any())
            {
                predicateInner = PredicateBuilder.New<Riskclusters>();
                foreach (var item in request.RiskClusterId)
                    predicateInner.Or(x => x.Riskclusterid == item);
                predicateResult.And(predicateInner);
            }
            if (request.RiskLevel != null && request.RiskLevel.Any())
            {
                predicateInner = PredicateBuilder.New<Riskclusters>();
                foreach (var item in request.RiskLevel)
                    predicateInner.Or(x => x.Risklevel == item);
                predicateResult.And(predicateInner);
            }

            if (request.ModificationUser != null && request.ModificationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Riskclusters>();
                foreach (var item in request.ModificationUser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.ModificationDate != null)
            {
                predicateInner = PredicateBuilder.New<Riskclusters>();
                if (request.ModificationDate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.ModificationDate.StartDate);
                if (request.ModificationDate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.ModificationDate.EndDate);
                predicateResult.And(predicateInner);
            }
                     

            return predicateResult;

        }
      
       
        public async Task<QueryResultDto<RiskClusterDtoGrid>> GetEnityGrid(RiskClusterQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<RiskClusterDtoGrid>(new GenerateRenderForGrid<RiskClusterDtoGrid>(_columnManager))
            {

            };
            var query = PrepareQuery(predicateResult);
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = data.Select(x =>
            {
                var grid = new RiskClusterDtoGrid();

                grid.RiskClusterId = x.Riskclusterid;
                grid.RiskLevel = x.Risklevel;
                grid.Description = x.Description;
                grid.LastModified = x.ModificationDate;
                grid.LastModifiedBy = x.ModificationUserEntity.Email;
                return grid;
            } ).ToList();

            rtn.Items = result.ToArray();
            return rtn;
        }

        public Dictionary<string, Expression<Func<RiskClusters, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<RiskClusters, object>>[]>
            {
                ["riskClusterId"] = new Expression<Func<RiskClusters, object>>[] { p => p.Riskclusterid },
                ["description"] = new Expression<Func<RiskClusters, object>>[] { p => p.Description },
                ["risklevel"] = new Expression<Func<RiskClusters, object>>[] { p => p.Risklevel },
                ["modificationUser"] = new Expression<Func<RiskClusters, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, RiskClusterQueryDto request)
        {           
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = PrepareQuery(predicateResult);

            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();
            return data;
        }
        public async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<RiskClusters> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description)) : request.Where(x =>
                   x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                        : request
                            .Where(x =>
                                x.ModificationUserEntity.Email.Contains(
                                    propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "riskClusterId" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(x => new FilterValueDto(x.Riskclusterid.ToString()))
                        : request.Where(x =>
                            x.Riskclusterid.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Riskclusterid.ToString())),

                
            };
        }
        public IQueryable<RiskClusters> PrepareQuery(ExpressionStarter<Riskclusters> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.RiskClusterRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.RiskClusterRepository.FindAll()
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation);

            return query
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => RiskClusterMapper.Get(p)).AsQueryable();
        }
        #endregion
        public async Task<ResultDto> Add(RiskClusterCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.RiskClusterRepository.FindByCondition(
               x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Riskclusterid
                };
            }

            RiskClusters entity = new RiskClusters()
            {
                Riskclusterid = dto.RiskClusterId,
                Risklevel = dto.RiskLevel,
                Description = dto.Description,
            };

            _repositoryWrapper.RiskClusterRepository.Create(RiskClusterMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public RiskClusterDtoGrid GetCreatePage()
        {

            var dto = new RiskClusterDtoGrid();           

            return dto;

        }

        public RiskClusterDtoGrid GetUpdatedPage(int id)
        {

            var riskCLusterModel = _repositoryWrapper.RiskClusterRepository.FindByCondition(x => x.Riskclusterid == id).FirstOrDefault();
            var dto = new RiskClusterDtoGrid();
            if (riskCLusterModel != null)
            {
                var riskEntity = RiskClusterMapper.Get(riskCLusterModel);
                if(riskEntity != null)
                {
                    dto.RiskClusterId = riskEntity.Riskclusterid;
                    dto.RiskLevel = riskEntity.Risklevel;
                    dto.Description = riskEntity.Description;                    
                    return dto;
                }
            }

            return dto;
           
        }

        public async Task<ResultDto> Update(RiskClusterUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.RiskClusterRepository.FindByCondition(
                   x => x.Riskclusterid != dto.RiskClusterId
                   && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Riskclusterid
                };
            }
            RiskClusters model = new RiskClusters()
            {
                Riskclusterid = dto.RiskClusterId,
                Description = dto.Description,
                Risklevel = dto.RiskLevel,
            };
            var updateRiskEntity = RiskClusterMapper.Set(model);
            _repositoryWrapper.RiskClusterRepository.Update(updateRiskEntity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
                 
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(int id)
        {
            var entity = await _repositoryWrapper.RiskClusterRepository.FindByCondition(x => x.Riskclusterid == id).SingleAsync();
            var CheckRelatonShipForVfName = _repositoryWrapper.RiskClusterVodafoneNamesRepository.FindByCondition(x => x.Riskclusterid == id).ToList();
            if (CheckRelatonShipForVfName != null && CheckRelatonShipForVfName.Count > 0)
            {
                if (CheckRelatonShipForVfName.Any(x => x.Vodafonenameid != null))
                {
                    return new ResultDto
                    {
                        Info = $"{entity.Riskclusterid}, This Id has Linked to Voadafone Name ",
                        Data = entity.Riskclusterid
                    };
                }
            }          
                _repositoryWrapper.RiskClusterRepository.Delete(entity);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Riskclusterid
                };
            
        }

        public async Task<ResultDto> DeleteDeep(int id)
        {
            var entity = await _repositoryWrapper.RiskClusterRepository
            .FindByCondition(x => x.Riskclusterid == id)
            .SingleAsync();           
           
            var relatedRiskClusterVfmap = _repositoryWrapper.RiskClusterVodafoneNamesRepository.FindByCondition(x => x.Riskclusterid == id).ToList();
            if(relatedRiskClusterVfmap != null && relatedRiskClusterVfmap.Count > 0)
            {
                if(relatedRiskClusterVfmap.Any(x=>x.Vodafonenameid != null))
                {
                    return new ResultDto
                    {
                        Info = $"{entity.Riskclusterid}, This Id has Linked to Voadafone Name ",
                        Data = entity.Riskclusterid
                    };
                }
                else
                {
                    foreach (var item in relatedRiskClusterVfmap)
                    {
                        item.Riskclusterid = null;
                        _repositoryWrapper.RiskClusterVodafoneNamesRepository.Update(item);
                    }

                }
               
            }          
            await _repositoryWrapper.SaveAsync();

            _repositoryWrapper.RiskClusterRepository.DeleteDeep(entity);

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Riskclusterid
            };
        }

    }
}
