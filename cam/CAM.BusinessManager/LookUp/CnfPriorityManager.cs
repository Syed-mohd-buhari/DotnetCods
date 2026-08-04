using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.CnfPriority;
using CAM.DataTransferObjects.QueryDto.XBom.CBom;
using CAM.Entities.Mappers.Cbom;
using CAM.Entities.Models.CBom;
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
    public class CnfPriorityManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly DropdownDataServiceManager _dropDownManager;


        public CnfPriorityManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, DropdownDataServiceManager dropownManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _dropDownManager = dropownManager;
        }
        #region UI Member function
        public ExpressionStarter<Cnfpriority> ApplyFilterForOracleModel(CnfPriorityQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Cnfpriority>();

            if (request.Cnfpriorityid != null && request.Cnfpriorityid.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfpriority>();
                foreach (var item in request.Cnfpriorityid)
                    predicateInner.Or(x => x.Cnfpriorityid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Description != null && request.Description.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfpriority>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }            

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Cnfpriority>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Cnfpriority>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(CnfPriorityQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<CnfPriorityDtoGrid>(new GenerateRenderForGrid<CnfPriorityDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Cnfpriority>(true);
            var fullQuery = await Task.Run(() => PrepareQuery(predicateResult));
            var allItems = MappingDto(fullQuery.ToList());

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

        public List<CnfPriorityDtoGrid> MappingDto(List<CnfPriority> query)
        {
            var result = new List<CnfPriorityDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new CnfPriorityDtoGrid();

                    grid.CnfPriorityId = x.CnfPriorityId;
                    grid.Description = x.Description;
                    grid.LastModified = x.ModificationDate;
                    grid.LastModifiedBy = x.ModificationUserEntity.Email;
                    return grid;
                }).ToList();
            }
            catch
            {
                return result;
            }

        }


        public Dictionary<string, Expression<Func<CnfPriority, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<CnfPriority, object>>[]>
            {
                ["CnfHardwareId"] = new Expression<Func<CnfPriority, object>>[] { p => p.CnfPriorityId },
                ["vnfDescription"] = new Expression<Func<CnfPriority, object>>[] { p => p.Description },
                ["modificationUser"] = new Expression<Func<CnfPriority, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, CnfPriorityQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "cnfPriorityId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfPriorityId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.CnfPriorityId))
                    .Distinct()
                    .ToList(),

                "description" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Description.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.Description))
                    .Distinct()
                    .ToList(),               

                "lastModifiedBy" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationUserEntity.Email.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.ModificationUserEntity.Email))
                    .Distinct()
                    .ToList(),

                _ => new List<FilterValueDto>()
            };
            return result;
        }

        public IQueryable<CnfPriority> PrepareQuery(ExpressionStarter<Cnfpriority> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.CnfPriorityRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.CnfPriorityRepository.FindAll()
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => CnfPriorityMapper.GetCnfPriority(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(CnfPriorityCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.CnfPriorityRepository.FindByCondition(
               x => x.Description.ToLower().Trim().Replace(" ", "") == dto.Description.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Cnfpriorityid
                };
            }

            Cnfpriority entity = new Cnfpriority()
            {
                Cnfpriorityid = dto.CnfPriorityId,
                Description = dto.Description,
            };

            _repositoryWrapper.CnfPriorityRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public CnfPriorityCreateDto GetCreatePage()
        {
            
            var dto = new CnfPriorityCreateDto();
            return dto;

        }

        public async Task<CnfPriorityUpdateDto> GetUpdatedPage(long id)
        {

            var Entity = await _repositoryWrapper.CnfPriorityRepository.FindByCondition(x => x.Cnfpriorityid == id).Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new CnfPriorityUpdateDto();
            if (Entity != null)
            {
                var model = CnfPriorityMapper.GetCnfPriority(Entity);
                if (model != null)
                {
                    dto.CnfPriorityId = model.CnfPriorityId;
                    dto.Description = model.Description;
                    dto.LastModified = model.ModificationDate;
                    dto.LastModifiedBy = model.ModificationUserEntity.Email;
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(CnfPriorityUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.CnfPriorityRepository.FindByCondition(
                   x => x.Cnfpriorityid != dto.CnfPriorityId
                   && x.Description.ToLower().Trim().Replace(" ", "") == dto.Description.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Cnfpriorityid
                };
            }
            Cnfpriority model = new Cnfpriority()
            {
                Description = dto.Description,
                Cnfpriorityid = dto.CnfPriorityId,
                Deleted = false,
            };
            _repositoryWrapper.CnfPriorityRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.CnfPriorityRepository
            .FindByCondition(x => x.Cnfpriorityid == id)
            .SingleAsync();

            var relatedcnfPodInfo = await _repositoryWrapper.CnfPodInfoRepository.FindByCondition(x => x.Priorityid == id).ToListAsync();

            if (relatedcnfPodInfo != null && relatedcnfPodInfo.Count > 0)
            {
                if (relatedcnfPodInfo.Any(x => x.Priorityid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"Priority Name '{entity.Description}', is Linked with CNF Pod Info ",
                        Data = entity.Cnfpriorityid,
                        Warning = true
                    };
                }
            }

            _repositoryWrapper.CnfPriorityRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Cnfpriorityid
            };
        }
        #endregion

    }
}
