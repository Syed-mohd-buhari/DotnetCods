using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.RiskCluster;
using CAM.DataTransferObjects.LookUp.ServiceMaster;
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
    public class ServiceMasterManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly IMapper _mapper;

       
        public ServiceMasterManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, IMapper mapper, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) :base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _mapper = mapper;
        }
        #region UI Member function
        public ExpressionStarter<Servicemaster> ApplyFilterForOracleModel(ServiceMasterQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Servicemaster>();
            var predicateInner = PredicateBuilder.New<Servicemaster>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Servicemaster>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.ServiceMasterIndex != null && request.ServiceMasterIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Servicemaster>();
                foreach (var item in request.ServiceMasterIndex)
                    predicateInner.Or(x => x.Servicemasterid == item);
                predicateResult.And(predicateInner);
            }            

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Servicemaster>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Servicemaster>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
                     

            return predicateResult;

        }
            
        public async Task<ResultDto> FindWithCondition(ServiceMasterQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<ServiceMasterDtoGrid>(new GenerateRenderForGrid<ServiceMasterDtoGrid>(_columnManager))
            {

            };
            var query = await PrepareQuery(predicateResult);
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = data.Select(x =>
            {
                var grid = new ServiceMasterDtoGrid();

                grid.ServiceMasterIndex = x.Servicemasterid;
                grid.Description = x.Description;
                grid.LastModified = x.ModificationDate;
                grid.LastModifiedBy = x.ModificationUserEntity.Email;
                return grid;
            } ).ToList();

            rtn.Items = result.ToArray();

            #region Returning all items
            predicateResult = PredicateBuilder.New<Servicemaster>(true);
            var fullQuery = await PrepareQuery(predicateResult);
            var allItems = fullQuery.ToList().Select(x =>
            {
                var grid = new ServiceMasterDtoGrid();
                grid.ServiceMasterIndex = x.Servicemasterid;
                grid.Description = x.Description;
                return grid;
            }).ToList();
            #endregion

            return new ResultDto
            {
                Data = new
                {
                    rtn.GridRender,
                    rtn.Items,
                    rtn.TotalItems,
                    allItems
                }
            };
        }

        public Dictionary<string, Expression<Func<ServiceMaster, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ServiceMaster, object>>[]>
            {
                ["serviceMasterId"] = new Expression<Func<ServiceMaster, object>>[] { p => p.Servicemasterid },
                ["description"] = new Expression<Func<ServiceMaster, object>>[] { p => p.Description },
                ["modificationUser"] = new Expression<Func<ServiceMaster, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, ServiceMasterQueryDto request)
        {           
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = await PrepareQuery(predicateResult);

            var data = GetFilterValueList(query, propertyName, propertyFilter).Distinct().ToList();
            return data;
        }
        public  IEnumerable<FilterValueDto> GetFilterValueList(IQueryable<ServiceMaster> request, string propertyName, string propertyFilter)
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

                "serviceMasterId" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(x => new FilterValueDto(x.Servicemasterid.ToString()))
                        : request.Where(x =>
                            x.Servicemasterid.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Servicemasterid.ToString())),

                _ => new List<FilterValueDto>()
            };
        }
        public async Task<IQueryable<ServiceMaster>> PrepareQuery(ExpressionStarter<Servicemaster> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.ServiceMasterRepository.FindByCondition(predicateResult)
               : _repositoryWrapper.ServiceMasterRepository.FindAll();

            var data = await query.AsNoTracking()
                .Include(x => x.ModificationuserNavigation).ToListAsync();

            return data.Select(p => ServiceMasterMapper.Get(p)).AsQueryable();
        }
        #endregion

        #region Add , Update & Delete
        public async Task<ResultDto> Add(ServiceMasterCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.ServiceMasterRepository.FindByCondition(
               x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Servicemasterid
                };
            }

            ServiceMaster entity = new ServiceMaster()
            {
                Servicemasterid = dto.ServiceMasterIndex,
                Description = dto.Description,
            };

            _repositoryWrapper.ServiceMasterRepository.Create(ServiceMasterMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(ServiceMasterUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.ServiceMasterRepository.FindByCondition(
                   x => x.Servicemasterid != dto.ServiceMasterIndex
                   && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Servicemasterid
                };
            }
            ServiceMaster model = new ServiceMaster()
            {
                Servicemasterid = dto.ServiceMasterIndex,
                Description = dto.Description,
            };
            var updateServiceEntity = ServiceMasterMapper.Set(model);
            _repositoryWrapper.ServiceMasterRepository.Update(updateServiceEntity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(int id)
        {
            var entity = await _repositoryWrapper.ServiceMasterRepository
            .FindByCondition(x => x.Servicemasterid == id)
            .SingleAsync();

            var servicePlanEntity = _repositoryWrapper.ServicePlanRepository.FindByCondition(x => x.Servicemasterid == id).ToList();
            if (servicePlanEntity != null && servicePlanEntity.Count > 0)
            {
                return new ResultDto
                {
                    Info = $"{entity.Servicemasterid}, This Id has Linked to Service Plan ",
                    Data = entity.Servicemasterid
                };

            }
            await _repositoryWrapper.SaveAsync();

            _repositoryWrapper.ServiceMasterRepository.DeleteDeep(entity);

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Servicemasterid
            };
        }
        #endregion

        #region // Creare  & Update Page
        public ServiceMasterDtoGrid GetCreatePage()
        {

            var dto = new ServiceMasterDtoGrid();           

            return dto;

        }

        public async Task<ServiceMasterDtoGrid> GetUpdatedPage(int id)
        {

            var riskCLusterModel = await _repositoryWrapper.ServiceMasterRepository.FindByCondition(x => x.Servicemasterid == id).FirstOrDefaultAsync();
            var dto = new ServiceMasterDtoGrid();
            if (riskCLusterModel != null)
            {
                var riskEntity = ServiceMasterMapper.Get(riskCLusterModel);
                if(riskEntity != null)
                {
                    dto.ServiceMasterIndex = riskEntity.Servicemasterid;
                    dto.Description = riskEntity.Description;                    
                    return dto;
                }
            }

            return dto;
           
        }

        #endregion


    }
}
