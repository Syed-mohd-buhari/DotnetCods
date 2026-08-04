using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.CnfFunctionstandardname;
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
    public class CnfFunctionStandardNameManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;


        public CnfFunctionStandardNameManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
        #region UI Member function
        public ExpressionStarter<Functionstandardname> ApplyFilterForOracleModel(CnfFunctionStandardNameQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Functionstandardname>();

            if (request.Functionstandardnameid != null && request.Functionstandardnameid.Any())
            {
                var predicateInner = PredicateBuilder.New<Functionstandardname>();
                foreach (var item in request.Functionstandardnameid)
                    predicateInner.Or(x => x.Functionstandardnameid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Functionname != null && request.Functionname.Any())
            {
                var predicateInner = PredicateBuilder.New<Functionstandardname>();
                foreach (var item in request.Functionname)
                    predicateInner.Or(x => x.Functionname == item);
                predicateResult.And(predicateInner);
            }            

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Functionstandardname>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Functionstandardname>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(CnfFunctionStandardNameQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<CnfFunctionStandardNameDtoGrid>(new GenerateRenderForGrid<CnfFunctionStandardNameDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Functionstandardname>(true);
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

        public List<CnfFunctionStandardNameDtoGrid> MappingDto(List<FunctionStandardName> query)
        {
            var result = new List<CnfFunctionStandardNameDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new CnfFunctionStandardNameDtoGrid();

                    grid.FunctionStandardNameId = x.FunctionStandardNameId;
                    grid.FunctionName = x.FunctionName;
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


        public Dictionary<string, Expression<Func<FunctionStandardName, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<FunctionStandardName, object>>[]>
            {
                ["functionStandardNameId"] = new Expression<Func<FunctionStandardName, object>>[] { p => p.FunctionStandardNameId },
                ["functionName"] = new Expression<Func<FunctionStandardName, object>>[] { p => p.FunctionName },
                ["modificationUser"] = new Expression<Func<FunctionStandardName, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, CnfFunctionStandardNameQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "functionStandardNameId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.FunctionStandardNameId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.FunctionStandardNameId))
                    .Distinct()
                    .ToList(),

                "functionName" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.FunctionName.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.FunctionName))
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

        public IQueryable<FunctionStandardName> PrepareQuery(ExpressionStarter<Functionstandardname> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.FunctionStandardNameRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.FunctionStandardNameRepository.FindAll()
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => FunctionStandardNameMapper.GetFunctionName(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(CnfFunctionStandardNameCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.FunctionStandardNameRepository.FindByCondition(
               x => x.Functionname.ToLower().Trim().Replace(" ", "") == dto.FunctionName.ToLower().Trim().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Functionstandardnameid,
                };
            }

            Functionstandardname entity = new Functionstandardname()
            {
                Functionstandardnameid = dto.FunctionStandardNameId,
                Functionname = dto.FunctionName,
            };

            _repositoryWrapper.FunctionStandardNameRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public CnfFunctionStandardNameCreateDto GetCreatePage()
        {
            
            var dto = new CnfFunctionStandardNameCreateDto();
            return dto;

        }

        public async Task<CnfFunctionStandardNameUpdateDto> GetUpdatedPage(long id)
        {

            var Entity = await _repositoryWrapper.FunctionStandardNameRepository.FindByCondition(x => x.Functionstandardnameid == id).Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new CnfFunctionStandardNameUpdateDto();
            if (Entity != null)
            {
                var model = FunctionStandardNameMapper.GetFunctionName(Entity);
                if (model != null)
                {
                    dto.FunctionStandardNameId = model.FunctionStandardNameId;
                    dto.FunctionName = model.FunctionName;
                    dto.LastModified = model.ModificationDate;
                    dto.LastModifiedBy = model.ModificationUserEntity.Email;
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(CnfFunctionStandardNameUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.FunctionStandardNameRepository.FindByCondition(
                   x => x.Functionstandardnameid != dto.FunctionStandardNameId
                   && x.Functionname.ToLower().Trim().Replace(" ", "") == dto.FunctionName.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Functionstandardnameid
                };
            }
            Functionstandardname model = new Functionstandardname()
            {
                Functionstandardnameid = dto.FunctionStandardNameId,
                Functionname = dto.FunctionName,
                Deleted = false,
            };
            _repositoryWrapper.FunctionStandardNameRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.FunctionStandardNameRepository
            .FindByCondition(x => x.Functionstandardnameid == id)
            .SingleAsync();

            var relatedcnfPodInfo = await _repositoryWrapper.CnfPodInfoRepository.FindByCondition(x => x.Functionstandardid == id).ToListAsync();

            if (relatedcnfPodInfo != null && relatedcnfPodInfo.Count > 0)
            {
                if (relatedcnfPodInfo.Any(x => x.Priorityid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"Funtion Standard Name '{entity.Functionname}', is Linked with CNF Pod Info ",
                        Data = entity.Functionstandardnameid,
                        Warning = true
                    };
                }
            }

            _repositoryWrapper.FunctionStandardNameRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Functionstandardnameid
            };
        }
        #endregion

    }
}
