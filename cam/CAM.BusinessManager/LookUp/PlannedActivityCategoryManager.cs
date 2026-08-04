using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.PlannedActivityCategory;
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
    public class PlannedActivityCategoryManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

       
        public PlannedActivityCategoryManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) :base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
        #region UI Member function
        public ExpressionStarter<Plannedactivitycategory> ApplyFilterForOracleModel(PlannedActivityCategoryQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Plannedactivitycategory>();
            var predicateInner = PredicateBuilder.New<Plannedactivitycategory>();

            if (request.Plannedactivitycategoryid != null && request.Plannedactivitycategoryid.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitycategory>();
                foreach (var item in request.Plannedactivitycategoryid)
                    predicateInner.Or(x => x.Plannedactivitycategoryid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Categorydescription != null && request.Categorydescription.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitycategory>();
                foreach (var item in request.Categorydescription)
                    predicateInner.Or(x => x.Categorydescription == item);
                predicateResult.And(predicateInner);
            }          

            if (request.ModificationUser != null && request.ModificationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivitycategory>();
                foreach (var item in request.ModificationUser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.ModificationDate != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivitycategory>();
                if (request.ModificationDate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.ModificationDate.StartDate);
                if (request.ModificationDate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.ModificationDate.EndDate);
                predicateResult.And(predicateInner);
            }
                     

            return predicateResult;

        }
      
       
        public async Task<QueryResultDto<PlannedActivityCategoryDtoGrid>> GetEnityGrid(PlannedActivityCategoryQueryDto dto)
        {
            var predicateResult = ApplyFilterForOracleModel(dto);
            var rtn = new QueryResultDto<PlannedActivityCategoryDtoGrid>(new GenerateRenderForGrid<PlannedActivityCategoryDtoGrid>(_columnManager))
            {

            };
            var query = await Task.Run(()=>PrepareQuery(predicateResult));
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(dto, GetColumnsMap()).ApplyPaging(dto);
            var data = query.ToList();
            var result = data.Select(x =>
            {
                var grid = new PlannedActivityCategoryDtoGrid();

                grid.PlannedActivityCategoryId = x.Plannedactivitycategoryid;
                grid.CategoryDescription = x.Categorydescription;
                grid.LastModified = x.ModificationDate;
                grid.LastModifiedBy = x.ModificationUserEntity.Email;
                return grid;
            } ).ToList();

            rtn.Items = result.ToArray();
            return rtn;
        }

        public Dictionary<string, Expression<Func<PlannedActivityCategory, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<PlannedActivityCategory, object>>[]>
            {
                ["plannedactivitycategoryid"] = new Expression<Func<PlannedActivityCategory, object>>[] { p => p.Plannedactivitycategoryid },
                ["categorydescription"] = new Expression<Func<PlannedActivityCategory, object>>[] { p => p.Categorydescription },
                ["modificationUser"] = new Expression<Func<PlannedActivityCategory, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, PlannedActivityCategoryQueryDto request)
        {           
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = PrepareQuery(predicateResult);

            var result = propertyName switch
            {
                "plannedActivityCategoryId" => await query
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Plannedactivitycategoryid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Plannedactivitycategoryid))
                    .Distinct()
                    .ToListAsync(),

                "categoryDescription" => await query
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Categorydescription.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Categorydescription))
                    .Distinct()
                    .ToListAsync(),

                _ => new List<FilterValueDto>(),
            };
            return result;
        }
        
        public IQueryable<PlannedActivityCategory> PrepareQuery(ExpressionStarter<Plannedactivitycategory> predicateResult)
        {
            var query = predicateResult.IsStarted
               ? _repositoryWrapper.PlannedActivityCategoryRepository.FindByCondition(predicateResult)
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
               : _repositoryWrapper.PlannedActivityCategoryRepository.FindAll()
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation);

            return query.AsEnumerable().Select(p => PlannedActivityCategoryMapper.Get(p)).AsQueryable();
        }
        #endregion

        #region // CRUD
        public async Task<ResultDto> Add(PlannedActivityCategoryCreateDto dto)
        {
            var entityExists = await _repositoryWrapper.PlannedActivityCategoryRepository.FindByCondition(
               x => x.Categorydescription.ToLower().Replace(" ", "") == dto.CategoryDescription.ToLower().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Plannedactivitycategoryid.ToString()
                };
            }

            PlannedActivityCategory entity = new PlannedActivityCategory()
            {
                Plannedactivitycategoryid = dto.PlannedActivityCategoryId,
                Categorydescription = dto.CategoryDescription,
            };

            _repositoryWrapper.PlannedActivityCategoryRepository.Create(PlannedActivityCategoryMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }      

        public async Task<ResultDto> Update(PlannedActivityCategoryUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.PlannedActivityCategoryRepository.FindByCondition(
                   x => x.Plannedactivitycategoryid != dto.PlannedActivityCategoryId
                   && x.Categorydescription.ToLower().Replace(" ", "") == dto.CategoryDescription.ToLower().Replace(" ", "")
                   && !x.Deleted.Value).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Plannedactivitycategoryid.ToString()
                };
            }
            PlannedActivityCategory model = new PlannedActivityCategory()
            {
                Plannedactivitycategoryid = dto.PlannedActivityCategoryId,
                Categorydescription = dto.CategoryDescription,
            };
            var updateCategoryEntity = PlannedActivityCategoryMapper.Set(model);
            _repositoryWrapper.PlannedActivityCategoryRepository.Update(updateCategoryEntity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
                 
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.PlannedActivityCategoryRepository
            .FindByCondition(x => x.Plannedactivitycategoryid == id)
            .SingleAsync();           
           
            var relatedBpt = _repositoryWrapper.BudgetProjectTrackersRepository.FindByCondition(x => x.Categoryid == id).ToList();
            var paRelation = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivitycategoryid == id).ToList();

            if((relatedBpt != null && relatedBpt.Count > 0) || (paRelation != null && paRelation.Count>0))
            {

                return new ResultDto
                {
                    Info = ((relatedBpt != null && relatedBpt.Count()>0) && (paRelation!= null && paRelation.Count>0))? 
                    $"{entity.Plannedactivitycategoryid}, This Id has Linked to Planned Activity and Budget Project Tracker ":
                    relatedBpt != null && relatedBpt.Count() > 0 ?
                    $"{entity.Plannedactivitycategoryid}, This Id has Linked to Budget Project Tracker ":
                    paRelation != null && paRelation.Count > 0 ?
                    $"{entity.Plannedactivitycategoryid}, This Id has Linked to Budget Project Tracker ":$"There is no relation",
                    Data = entity.Plannedactivitycategoryid
                };
                
            }          
            await _repositoryWrapper.SaveAsync();

            _repositoryWrapper.PlannedActivityCategoryRepository.DeleteDeep(entity);

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Plannedactivitycategoryid
            };
        }
        #endregion

        #region //Get and Update page

        public PlannedActivityCategoryDtoGrid GetCreatePage()
        {

            var dto = new PlannedActivityCategoryDtoGrid();

            return dto;

        }

        public PlannedActivityCategoryDtoGrid GetUpdatedPage(short id)
        {

            var categoryModel = _repositoryWrapper.PlannedActivityCategoryRepository.FindByCondition(x => x.Plannedactivitycategoryid == id).FirstOrDefault();
            var dto = new PlannedActivityCategoryDtoGrid();
            if (categoryModel != null)
            {
                var categoryEntity = PlannedActivityCategoryMapper.Get(categoryModel);
                if (categoryEntity != null)
                {
                    dto.PlannedActivityCategoryId = categoryEntity.Plannedactivitycategoryid;
                    dto.CategoryDescription = categoryEntity.Categorydescription;
                    return dto;
                }
            }

            return dto;

        }
        #endregion
    }
}
