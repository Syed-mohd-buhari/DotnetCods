using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Entities.Mappers.Lookup;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using CAM.DataTransferObjects.LookUp.Category;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CAM.BusinessManager.LookUp
{
    public class CategoryManager : GridBaseAsync<Category, CategoryDtoGrid, CategoryDtoQuery, Categories>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public CategoryManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }


        public override ExpressionStarter<Categories> ApplyFilterForOracleModel(CategoryDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Categories>();
            var predicateInner = PredicateBuilder.New<Categories>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Categories>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Categories>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Categories>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Categories>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Categories>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<CategoryDtoGrid> CastObjectToDto(IQueryable<Category> request)
        {
            return request.Select(dto => new CategoryDtoGrid()
            {
                Id = (short)dto.Id,
                Description = dto.Description,
                LastModified = dto.ModificationDate,
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<Category, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Category, object>>[]>
            {
                ["description"] = new Expression<Func<Category, object>>[] { p => p.Description },
                ["id"] = new Expression<Func<Category, object>>[] { p => p.Id },
                ["lastModifiedBy"] = new Expression<Func<Category, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<Category> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description))
                    : request.Where(x => x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                    : request.Where(x => x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<Category> PrepareQuery(CategoryDtoQuery request, ExpressionStarter<Category> predicateResult, ExpressionStarter<Categories> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.CategoryRepository.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.CategoryRepository.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => CategoryMapper.Get(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(CategoryDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.CategoryRepository.FindByCondition(
                x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            Category entity = new Category() { Id = dto.Id, Description = dto.Description };
            _repositoryWrapper.CategoryRepository.Create(CategoryMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(CategoryDtoUpdate dto)
        {
            var entityExists = await _repositoryWrapper.CategoryRepository.FindByCondition(
                x => x.Id != dto.Id && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            Category entity = new Category() { Id = dto.Id, Description = dto.Description };

            _repositoryWrapper.CategoryRepository.Update(CategoryMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.CategoryRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.CategoryRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.CategoryRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.CategoryRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var classes = _repositoryWrapper.ClassRepository.FindByCondition(x => x.Categoryid == id)
                                    .Select(c => c.Description).ToArray();
            var Identitiesasis = _repositoryWrapper.IdentityAsIsRepository.FindByCondition(x => x.Categoryid == id)
                                   .Select(c => c.Value).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (classes.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Class ", Values = classes });
            if (Identitiesasis.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Identity ", Values = Identitiesasis });
            var entity = await _repositoryWrapper.CategoryRepository.FindByCondition(x => x.Id == id)
                .SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Category",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public CategoryDtoCreate GetCreatePage()
        {
            return new CategoryDtoCreate();
        }

        public CategoryDtoUpdate GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.CategoryRepository.FindByCondition(x => x.Id == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = CategoryMapper.Get(model);
            return new CategoryDtoUpdate()
            {
                Id = (short)entity.Id,
                Description = entity.Description,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email
            };
        }

        }
    }

