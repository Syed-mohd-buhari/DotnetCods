using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.LookUp;
using CAM.Entities.Models.Lookup;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class BudgetAvailabilityManager : GridBaseAsync<BudgetAvailability, TipologicaGridDtoCombinationRule, TipologicaQueryDtoCombinationRule, Budgetavailability>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public BudgetAvailabilityManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
  

        public override ExpressionStarter<Budgetavailability> ApplyFilterForOracleModel(TipologicaQueryDtoCombinationRule request)
        {
            var predicateResult = PredicateBuilder.New<Budgetavailability>();
            var predicateInner = PredicateBuilder.New<Budgetavailability>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetavailability>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetavailability>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Budgetavailabilityid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetavailability>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Rule != null && request.Rule.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetavailability>();
                foreach (var item in request.Rule)
                    predicateInner.Or(x => x.Rule == item);
                predicateResult.And(predicateInner);
            }
            if (request.ProjectStatusCombinationRule != null && request.ProjectStatusCombinationRule.Any())
            {
                predicateInner = PredicateBuilder.New<Budgetavailability>();
                foreach (var item in request.ProjectStatusCombinationRule)
                    predicateInner.Or(x => x.Projectstatuscombinationrule == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Budgetavailability>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Budgetavailability>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDtoCombinationRule> CastObjectToDto(IQueryable<BudgetAvailability> request)
        {
            return  request.Select(dto => new TipologicaGridDtoCombinationRule()
            {
                Id = dto.BudgetAvailabilityId,
                Description = dto.BudgetAvailabilityDescription,
                LastModified = dto.ModificationDate,
                Deleted = dto.Deleted,
                Orphan = dto.PlannedActivities.Any(),
                Rule = dto.Rule,
                ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<BudgetAvailability, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<BudgetAvailability, object>>[]>
            {
                ["description"] = new Expression<Func<BudgetAvailability, object>>[] { p => p.BudgetAvailabilityDescription },
                ["id"] = new Expression<Func<BudgetAvailability, object>>[] { p => p.BudgetAvailabilityId },
                ["rule"] = new Expression<Func<BudgetAvailability, object>>[] { p => p.Rule },
                ["projectStatusCombinationRule"] = new Expression<Func<BudgetAvailability, object>>[] { p => p.ProjectStatusCombinationRule },
                ["lastModifiedBy"] = new Expression<Func<BudgetAvailability, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<BudgetAvailability> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.BudgetAvailabilityDescription))
                : request.Where(x => x.BudgetAvailabilityDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.BudgetAvailabilityDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.BudgetAvailabilityId.ToString()))
                : request.Where(x => x.BudgetAvailabilityId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.BudgetAvailabilityId.ToString())),
                "rule" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.Rule.ToString()))
                    : request.Where(x => x.Rule.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Rule.ToString())),
                "projectStatusCombinationRule" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.ProjectStatusCombinationRule.ToString()))
                    : request.Where(x => x.ProjectStatusCombinationRule.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ProjectStatusCombinationRule.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<BudgetAvailability> PrepareQuery(TipologicaQueryDtoCombinationRule request, ExpressionStarter<BudgetAvailability> predicateResult , ExpressionStarter<Budgetavailability> oraclePredicateResult = null)
        {
          

            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.BudgetAvailability.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.BudgetAvailability.FindAll();

           return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable()
                .Select(p => BudgetAvailabilityMapper.GetBudgetAvailabilityMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDtoCombinationRule dto)
        {
            var entityExists = await _repositoryWrapper.BudgetAvailability.FindByCondition(
               x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Budgetavailabilityid
                };
            }
            BudgetAvailability entity = new BudgetAvailability() { BudgetAvailabilityId = dto.Id, BudgetAvailabilityDescription = dto.Description, Rule = dto.Rule, ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule };
            _repositoryWrapper.BudgetAvailability.Create(BudgetAvailabilityMapper.SetBudgetAvailabilityMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDtoCombinationRule dto)
        {
            var entityExists = await _repositoryWrapper.BudgetAvailability.FindByCondition(
               x => x.Budgetavailabilityid != dto.Id && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Budgetavailabilityid
                };
            }
            BudgetAvailability entity = new BudgetAvailability() { BudgetAvailabilityId = dto.Id, BudgetAvailabilityDescription = dto.Description, Rule = dto.Rule, ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule };
            _repositoryWrapper.BudgetAvailability.Update(BudgetAvailabilityMapper.SetBudgetAvailabilityMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.BudgetAvailability.FindByCondition(x => x.Budgetavailabilityid == id).SingleAsync();
            _repositoryWrapper.BudgetAvailability.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Budgetavailabilityid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.BudgetAvailability.FindByCondition(x => x.Budgetavailabilityid == id).SingleAsync();
            _repositoryWrapper.BudgetAvailability.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Budgetavailabilityid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var plannedActivity = _repositoryWrapper.PlannedActivity
                                    .FindByCondition(x => x.Budgetavailabilityid == id)
                                    .Include(x => x.Plannedactivityresource)
                                    .Include(x => x.Activitystatus)
                                    .Include(x => x.Deliverystatus)
                                    .Select(x => PlannedActivityMapper.Get(x,true).toLinkedPlannedActivityName())
                                    .ToArray();
            var entity = await _repositoryWrapper.BudgetAvailability.FindByCondition(x => x.Budgetavailabilityid == id)
              .SingleAsync();

            var SettingsUpdatePlannedActivity = _repositoryWrapper.SettingsUpdatePlannedActivity
                .FindByCondition(x => x.Budgetavailabilityid== id).Select(x => x.Settingsupdateplnactdes).ToArray();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (plannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivity });
            if (SettingsUpdatePlannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Settings Update Planned Activity", Values = SettingsUpdatePlannedActivity });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Budget Availability",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();

        }

        public TipologicaGridDtoCombinationRule GetCreatePage()
        {
            var dto = new TipologicaGridDtoCombinationRule();
            return dto;
        }

        public TipologicaGridDtoCombinationRule GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.BudgetAvailability.FindByCondition(x => x.Budgetavailabilityid == id)
                .Include(x => x.ModificationuserNavigation).Single();

            var entity = BudgetAvailabilityMapper.GetBudgetAvailabilityMapper(model);
            var dto = new TipologicaGridDtoCombinationRule() { 
                Id = entity.BudgetAvailabilityId, 
                Description = entity.BudgetAvailabilityDescription, 
                LastModified = entity.ModificationDate, 
                Rule = entity.Rule, 
                ProjectStatusCombinationRule = entity.ProjectStatusCombinationRule, 
                LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

    
    }
}
