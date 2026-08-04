using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.PodTypeInfo;
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
    public class PodTypeInfoManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly DropdownDataServiceManager _dropDownManager;


        public PodTypeInfoManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, DropdownDataServiceManager dropownManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _dropDownManager = dropownManager;
        }
        #region UI Member function
        public ExpressionStarter<Podtypeinfo> ApplyFilterForOracleModel(PodTypeInfoQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Podtypeinfo>();

            if (request.Podtypeinfoid != null && request.Podtypeinfoid.Any())
            {
                var predicateInner = PredicateBuilder.New<Podtypeinfo>();
                foreach (var item in request.Podtypeinfoid)
                    predicateInner.Or(x => x.Podtypeinfoid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Podtypeinfoname != null && request.Podtypeinfoname.Any())
            {
                var predicateInner = PredicateBuilder.New<Podtypeinfo>();
                foreach (var item in request.Podtypeinfoname)
                    predicateInner.Or(x => x.Podtypeinfoname == item);
                predicateResult.And(predicateInner);
            }

            if (request.Podroledescription != null && request.Podroledescription.Any())
            {
                var predicateInner = PredicateBuilder.New<Podtypeinfo>();
                foreach (var item in request.Podroledescription)
                    predicateInner.Or(x => x.Podroledescription == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                var predicateInner = PredicateBuilder.New<Podtypeinfo>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                var predicateInner = PredicateBuilder.New<Podtypeinfo>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;

        }

        public async Task<ResultDto> GetEnityGrid(PodTypeInfoQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<PodTypeInfoDtoGrid>(new GenerateRenderForGrid<PodTypeInfoDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(() => PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = MappingDto(data);

            rtn.Items = result.ToArray();
            predicateResult = PredicateBuilder.New<Podtypeinfo>(true);
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

        public List<PodTypeInfoDtoGrid> MappingDto(List<PodTypeInfo> query)
        {
            var result = new List<PodTypeInfoDtoGrid>();
            try
            {
                return result = query.Select(x =>
                {
                    var grid = new PodTypeInfoDtoGrid();

                    grid.PodTypeInfoId = x.PodTypeInfoId;
                    grid.PodRoleDescription = x.PodRoleDescription;
                    grid.PodTypeInfoName = x.PodTypeInfoName;
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


        public Dictionary<string, Expression<Func<PodTypeInfo, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<PodTypeInfo, object>>[]>
            {
                ["podTypeInfoId"] = new Expression<Func<PodTypeInfo, object>>[] { p => p.PodTypeInfoId },
                ["podTypeInfoName"] = new Expression<Func<PodTypeInfo, object>>[] { p => p.PodTypeInfoName },
                ["podRoleDescription"] = new Expression<Func<PodTypeInfo, object>>[] { p => p.PodRoleDescription },
                ["modificationUser"] = new Expression<Func<PodTypeInfo, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, PodTypeInfoQueryDto request)
        {
            var predicateResult = ApplyFilterForOracleModel(request);

            var filteredQuery = await Task.Run(() => PrepareQuery(predicateResult));

            var result = propertyName switch
            {
                "podTypeInfoId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PodTypeInfoId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.PodTypeInfoId))
                    .Distinct()
                    .ToList(),

                "podRoleDescription" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PodRoleDescription.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.PodRoleDescription))
                    .Distinct()
                    .ToList(),

                "podTypeInfoName" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PodTypeInfoName.ToString().Contains(propertyFilter))
                 .Select(x => new FilterValueDto(x.PodTypeInfoName))
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

        public IQueryable<PodTypeInfo> PrepareQuery(ExpressionStarter<Podtypeinfo> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.PodTypeInfoRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.PodTypeInfoRepository.FindAll()
                 .Include(m => m.ModificationuserNavigation)
                 .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => PodTypeInfoMapper.GetPodTypeInfo(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(PodTypeInfoCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.PodTypeInfoRepository.FindByCondition(
               x => x.Podroledescription.ToLower().Trim().Replace(" ", "") == dto.PodRoleDescription.ToLower().Trim().Replace(" ", "") && 
               x.Podtypeinfoname.ToLower().Trim().Replace(" ", "") == dto.PodTypeInfoName.ToLower().Trim().Replace(" ","")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Podtypeinfoid
                };
            }

            Podtypeinfo entity = new Podtypeinfo()
            {
                Podtypeinfoid = dto.PodTypeInfoId,
                Podtypeinfoname = dto.PodTypeInfoName,
                Podroledescription = dto.PodRoleDescription,
            };

            _repositoryWrapper.PodTypeInfoRepository.Create(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public PodTypeInfoCreateDto GetCreatePage()
        {
            var dto = new PodTypeInfoCreateDto();
            return dto;
        }

        public async Task<PodTypeInfoUpdateDto> GetUpdatedPage(long id)
        {

            var Entity = await _repositoryWrapper.PodTypeInfoRepository.FindByCondition(x => x.Podtypeinfoid == id).Include(x => x.ModificationuserNavigation).FirstOrDefaultAsync();
            var dto = new PodTypeInfoUpdateDto();
            if (Entity != null)
            {
                var model = PodTypeInfoMapper.GetPodTypeInfo(Entity);
                if (model != null)
                {
                    dto.PodTypeInfoId = model.PodTypeInfoId;
                    dto.PodTypeInfoName = model.PodTypeInfoName;
                    dto.PodRoleDescription = model.PodRoleDescription;
                    dto.LastModified = model.ModificationDate;
                    dto.LastModifiedBy = model.ModificationUserEntity.Email;
                    return dto;
                }
            }
            return dto;

        }

        public async Task<ResultDto> Update(PodTypeInfoUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.PodTypeInfoRepository.FindByCondition(
                   x => x.Podtypeinfoid != dto.PodTypeInfoId
                   && x.Podtypeinfoname.ToLower().Trim().Replace(" ", "") == dto.PodTypeInfoName.ToLower().Trim().Replace(" ", "")
                   && x.Podroledescription.ToLower().Trim().Replace(" ", "") == dto.PodRoleDescription.ToLower().Trim().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Podtypeinfoid
                };
            }
            Podtypeinfo model = new Podtypeinfo()
            {
                Podroledescription = dto.PodRoleDescription,
                Podtypeinfoname = dto.PodTypeInfoName,
                Podtypeinfoid = dto.PodTypeInfoId,
                Deleted = false,
            };
            _repositoryWrapper.PodTypeInfoRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.PodTypeInfoRepository
            .FindByCondition(x => x.Podtypeinfoid == id)
            .SingleAsync();

            var relatedcnfPodInfo = await _repositoryWrapper.CnfPodInfoRepository.FindByCondition(x => x.Podtypeinfoid == id).ToListAsync();

            if (relatedcnfPodInfo != null && relatedcnfPodInfo.Count > 0)
            {
                if (relatedcnfPodInfo.Any(x => x.Podtypeinfoid != 0))
                {
                    return new ResultDto
                    {
                        Info = $"Podtype Name '{entity.Podtypeinfoname}', is Linked with CNF Pod Info ",
                        Data = entity.Podtypeinfoid,
                        Warning = true
                    };
                }
            }

            _repositoryWrapper.PodTypeInfoRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Podtypeinfoid
            };
        }
        #endregion

    }
}
