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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CAM.BusinessManager.LookUp
{
    public class ActivityStatusesManager : GridBaseAsync<ActivityStatus, TipologicaGridDtoCombinationRule, TipologicaQueryDtoCombinationRule, Activitystatuses>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public ActivityStatusesManager(IEnumerable<IRepositoryWrapper> wrappers, 
            GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
   

        public override ExpressionStarter<Activitystatuses> ApplyFilterForOracleModel(TipologicaQueryDtoCombinationRule request)
        {
            var predicateResult = PredicateBuilder.New<Activitystatuses>();
            var predicateInner = PredicateBuilder.New<Activitystatuses>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Activitystatuses>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Activitystatus == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Activitystatuses>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Activitystatusid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Rule != null && request.Rule.Any())
            {
                predicateInner = PredicateBuilder.New<Activitystatuses>();
                foreach (var item in request.Rule)
                    predicateInner.Or(x => x.Rule == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Activitystatuses>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.ProjectStatusCombinationRule != null && request.ProjectStatusCombinationRule.Any())
            {
                predicateInner = PredicateBuilder.New<Activitystatuses>();
                foreach (var item in request.ProjectStatusCombinationRule)
                    predicateInner.Or(x => x.Projectstatuscombinationrule == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Activitystatuses>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Activitystatuses>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override  List<TipologicaGridDtoCombinationRule> CastObjectToDto(IQueryable<ActivityStatus> request)
        {
            var result = request.ToList().Select(dto => new TipologicaGridDtoCombinationRule()
            {
                Id = dto.ActivityStatusId,
                Description = dto.ActivityStatusDescription,
                LastModified = dto.ModificationDate,
                Deleted = dto.Deleted,
                Orphan = dto.PlannedActivities.Any(),
                Rule = dto.Rule,
                ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();

            return result;
        }

        public override Dictionary<string, Expression<Func<ActivityStatus, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ActivityStatus, object>>[]>
            {
                ["description"] = new Expression<Func<ActivityStatus, object>>[] { p => p.ActivityStatusDescription },
                ["id"] = new Expression<Func<ActivityStatus, object>>[] { p => p.ActivityStatusId },
                ["rule"] = new Expression<Func<ActivityStatus, object>>[] { p => p.Rule },
                ["projectStatusCombinationRule"] = new Expression<Func<ActivityStatus, object>>[] { p => p.ProjectStatusCombinationRule },
                ["lastModifiedBy"] = new Expression<Func<ActivityStatus, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<ActivityStatus> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.ActivityStatusDescription))
                : request.Where(x => x.ActivityStatusDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.ActivityStatusDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.ActivityStatusId.ToString()))
                : request.Where(x => x.ActivityStatusId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ActivityStatusId.ToString())),
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

        public override IQueryable<ActivityStatus> PrepareQuery(TipologicaQueryDtoCombinationRule request, ExpressionStarter<ActivityStatus> predicateResult , ExpressionStarter<Activitystatuses> oraclePredicateResult = null)
        {
           
            var query = oraclePredicateResult.IsStarted
              ? _repositoryWrapper.ActivityStatus.FindByCondition(oraclePredicateResult)
              : _repositoryWrapper.ActivityStatus.GetAll();


            return query.Include(m=>m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => ActivityStatusMapper.GetActivityStatusMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDtoCombinationRule dto)
        {
            var entityExists = await _repositoryWrapper.ActivityStatus.FindByCondition(
               x => x.Activitystatusid == dto.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Activitystatusid
                };
            }
            ActivityStatus entity = new ActivityStatus() { ActivityStatusId = dto.Id, ActivityStatusDescription = dto.Description, Rule = dto.ProjectStatusCombinationRule, ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule };
            var savedObject = ActivityStatusMapper.SetActivityActivityStatusMapper(entity);
            _repositoryWrapper.ActivityStatus.Create(savedObject);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDtoCombinationRule dto)
        {
            var entityExists = await _repositoryWrapper.ActivityStatus.FindByCondition(
               x => x.Activitystatusid != dto.Id && x.Activitystatus == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Activitystatusid
                };
            }
            ActivityStatus entity = new ActivityStatus() { ActivityStatusId = dto.Id, ActivityStatusDescription = dto.Description, Rule = dto.Rule, ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule };
            var savedObject = ActivityStatusMapper.SetActivityActivityStatusMapper(entity);
            _repositoryWrapper.ActivityStatus.Update(savedObject);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Activitystatusid == id).SingleAsync();
            _repositoryWrapper.ActivityStatus.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Activitystatusid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Activitystatusid == id).SingleAsync();
            _repositoryWrapper.ActivityStatus.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Activitystatusid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var entity = await _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Activitystatusid == id).SingleAsync();
            var entities = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Activitystatusid == id)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Activitystatus)
                .Include(x => x.Deliverystatus)
                .Select(x =>PlannedActivityMapper.Get(x,true).toLinkedPlannedActivityName())
                .ToArray();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = entities });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Activity Status",
                        RecordName = entity.Activitystatus,
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
            var model = _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Activitystatusid == id)
                .Include(x => x.ModificationuserNavigation).Single();

            var entity = ActivityStatusMapper.GetActivityStatusMapper(model);

            var dto = new TipologicaGridDtoCombinationRule()
            {
                Id = entity.ActivityStatusId, 
                Description = entity.ActivityStatusDescription, 
                LastModified = entity.ModificationDate, Rule = entity.Rule, 
                ProjectStatusCombinationRule= entity.ProjectStatusCombinationRule,
                LastModifiedBy = entity.ModificationUserEntity.Email
            };
            return dto;
        }

       
    }
}
