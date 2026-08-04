using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ProblemCategory;
using CAM.DataTransferObjects.Entita.SystemVerificationProblem;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
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

namespace CAM.BusinessManager.Entity
{
    public class ProblemCategoryManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public ProblemCategoryManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<ProblemCategoryDto> FindWithCondition(ProblemCategoryQueryDto problemCategoryQueryDto)
        {
            var predicateResult = ApplyFilter(problemCategoryQueryDto);
            var rtn = new QueryResultDto<ProblemCategoryDto>(new GenerateRenderForGrid<ProblemCategoryDto>(_manager))
            {

            };
            var query = GetQuery(predicateResult, problemCategoryQueryDto.Deleted ?? false).ApplyOrdering(problemCategoryQueryDto, GetColumnsMap());
            rtn.TotalItems = query.Count();
            query = query.ApplyPaging(problemCategoryQueryDto);
            var data = query.ToList();

            IEnumerable<ProblemCategoryDto> ProblemCategoryDto;

            ProblemCategoryDto = _mapper.Map<IEnumerable<ProblemCategoryDto>>(data);

            rtn.Items = ProblemCategoryDto.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Problemcategory> ApplyFilter(ProblemCategoryQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Problemcategory>();
            var predicateInner = PredicateBuilder.New<Problemcategory>();

            if (buildFilterDto.ProblemCategoryId != null && buildFilterDto.ProblemCategoryId.Any())
            {
                predicateInner = PredicateBuilder.New<Problemcategory>();
                foreach (var item in buildFilterDto.ProblemCategoryId)
                    predicateInner.Or(x => x.Problemcategoryid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProblemCategoryDescription != null && buildFilterDto.ProblemCategoryDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Problemcategory>();
                foreach (var item in buildFilterDto.ProblemCategoryDescription)
                    predicateInner.Or(x => x.Problemcategorydescription == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Problemcategory>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Problemcategory>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        private IQueryable<ProblemCategory> GetQuery(ExpressionStarter<Problemcategory> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.ProblemCategoryRepository.FindByCondition(predicateResult, includeDeleted)
                       .Include(x => x.CreationuserNavigation)
                       .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.ProblemCategoryRepository.FindAll()
                      .Include(x => x.CreationuserNavigation)
                      .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => ProblemCategoryMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<ProblemCategory, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ProblemCategory, object>>[]>
            {
                ["problemCategoryId"] = new Expression<Func<ProblemCategory, object>>[] { p => p.ProblemCategoryId },
                ["problemCategoryDescription"] = new Expression<Func<ProblemCategory, object>>[] { p => p.ProblemCategoryDescription },
                ["lastModifiedBy"] = new Expression<Func<ProblemCategory, object>>[] { p => p.ModificationUserEntity.Email },
                ["lastModifiedValue"] = new Expression<Func<ProblemCategory, object>>[] { p => p.ModificationDate },

            };
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, ProblemCategoryQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "problemCategoryId" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.ProblemCategoryId.ToString(), Value = p.ProblemCategoryId.ToString() }).Distinct().ToList()
                      : query
                           .Where(x => x.ProblemCategoryId.ToString().Contains(propertyFilter)).Select(p =>
                                new FilterValueDto { Text = p.ProblemCategoryId.ToString(), Value = p.ProblemCategoryId.ToString() }).Distinct()
                               .ToList(),
                "problemCategoryDescription" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.ProblemCategoryDescription, Value = p.ProblemCategoryDescription }).Distinct().ToList()
                     : query
                          .Where(x => x.ProblemCategoryDescription.Contains(propertyFilter)).Select(p =>
                             new FilterValueDto { Text = p.ProblemCategoryDescription, Value = p.ProblemCategoryDescription }).Distinct()
                            .ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email, Value = p.ModificationUserEntity.Email }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.ModificationUserEntity.Email.Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email, Value = p.ModificationUserEntity.Email }).Distinct().ToList(),


                _ => new List<FilterValueDto>()
            };

            return rtn;

            

        }
        #endregion

        #region CRUD Operations
        public async Task<ResultDto> Add(ProblemCategoryCreateDto dto)
        {
            var entity = _mapper.Map<ProblemCategory>(dto);
            _repositoryWrapper.ProblemCategoryRepository.Create(ProblemCategoryMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        public async Task<ResultDto> Update(ProblemCategoryCreateDto dto)
        {


            var ProblemCategoryModel = _repositoryWrapper.ProblemCategoryRepository.FindByCondition(x => x.Problemcategoryid == dto.ProblemCategoryId).FirstOrDefault();
            var ProblemCategoryEntity = ProblemCategoryMapper.Get(ProblemCategoryModel);

            if (ProblemCategoryEntity != null)
            {
                ProblemCategoryEntity.ProblemCategoryId = dto.ProblemCategoryId;
                ProblemCategoryEntity.ProblemCategoryDescription = dto.ProblemCategoryDescription;
                var ProblemCategorySetEntity = ProblemCategoryMapper.Set(ProblemCategoryEntity);
                _repositoryWrapper.ProblemCategoryRepository.Update(ProblemCategorySetEntity);
                await _repositoryWrapper.SaveAsync();
            }


            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
                Data = ProblemCategoryEntity.ProblemCategoryId
            };
        }

        public ProblemCategoryCreateDto GetCraetePage()
        {
            return new ProblemCategoryCreateDto()
            {
                Deleted = false,
            };
        }
        public ProblemCategoryCreateDto GetUpdatePage(long id)
        {
            var ProblemCategoryModel = _repositoryWrapper.ProblemCategoryRepository.FindByCondition(x => x.Problemcategoryid == id).Include(x=>x.ModificationuserNavigation).FirstOrDefault();
            var dto = new ProblemCategoryCreateDto();
            if (ProblemCategoryModel != null)
            {
                dto.ProblemCategoryId = ProblemCategoryModel.Problemcategoryid;
                dto.ProblemCategoryDescription = ProblemCategoryModel.Problemcategorydescription;
                dto.LastModified = ProblemCategoryModel.Modificationdate;
                dto.LastModifiedBy = ProblemCategoryModel.ModificationuserNavigation.Email;
            }
            return dto;
        } 

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.ProblemCategoryRepository
                .FindByCondition(x => x.Problemcategoryid == id).SingleAsync();

            if (entity != null)
            {
                _repositoryWrapper.ProblemCategoryRepository.Delete(entity);
                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Problemcategoryid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = id
                };
            }
        }
        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.ProblemCategoryRepository
               .FindByConditionWithDelete(x => x.Problemcategoryid == id).SingleAsync();
            if (entity != null)
            {
                _repositoryWrapper.ProblemCategoryRepository.DeleteDeep(entity);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Problemcategoryid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = id
                };
            }
        }
        #endregion
    }

}

