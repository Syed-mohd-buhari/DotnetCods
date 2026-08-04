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
    public class PlanningActivityStatusManager : GridBaseAsync<PlanningActivityStatus, TipologicaGridDtoProjectStatusCombinationRule, TipologicaQueryDtoProjectStatusCombinationRule, Planningactivitystatuses>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public PlanningActivityStatusManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }


        public override ExpressionStarter<Planningactivitystatuses> ApplyFilterForOracleModel(TipologicaQueryDtoProjectStatusCombinationRule request)
        {
            var predicateResult = PredicateBuilder.New<Planningactivitystatuses>();
            var predicateInner = PredicateBuilder.New<Planningactivitystatuses>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Planningactivitystatuses>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Planningactivitystatus == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Planningactivitystatuses>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Planningactivitystatuses>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Planningactivitystatusid == item);
                predicateResult.And(predicateInner);
            }
            if (request.ProjectStatusCombinationRule != null && request.ProjectStatusCombinationRule.Any())
            {
                predicateInner = PredicateBuilder.New<Planningactivitystatuses>();
                foreach (var item in request.ProjectStatusCombinationRule)
                    predicateInner.Or(x => x.Projectstatuscombinationrule == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Planningactivitystatuses>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Planningactivitystatuses>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDtoProjectStatusCombinationRule> CastObjectToDto(IQueryable<PlanningActivityStatus> request)
        {
            return request.Select(dto => new TipologicaGridDtoProjectStatusCombinationRule()
            {
                Id = dto.PlanningActivityStatusId,
                Description = dto.PlanningActivityStatusDescription,
                LastModified = dto.ModificationDate,
                Default = dto.Default,
                ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule,
                LastModifiedBy = dto.ModificationUserEntity.Email

            }).ToList();
        }

        public override Dictionary<string, Expression<Func<PlanningActivityStatus, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<PlanningActivityStatus, object>>[]>
            {
                ["description"] = new Expression<Func<PlanningActivityStatus, object>>[] { p => p.PlanningActivityStatusDescription },
                ["default"] = new Expression<Func<PlanningActivityStatus, object>>[] { p => p.Default },
                ["lastModifiedBy"] = new Expression<Func<PlanningActivityStatus, object>>[] { p => p.ModificationUserEntity.Email },
                ["id"] = new Expression<Func<PlanningActivityStatus, object>>[] { p => p.PlanningActivityStatusId },
                ["projectStatusCombinationRule"] = new Expression<Func<PlanningActivityStatus, object>>[] { p => p.ProjectStatusCombinationRule }
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<PlanningActivityStatus> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.PlanningActivityStatusDescription))
                : request.Where(x => x.PlanningActivityStatusDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.PlanningActivityStatusDescription)),
                "default" => string.IsNullOrEmpty(propertyFilter)
                  ? request
                      .Select(p => new FilterValueDto { Text = p.Default ? "YES" : "NO", Value = p.Default.ToString() }).Distinct()
                  : request
                      .Where(x => x.Default == false)
                      .Select(p => new FilterValueDto { Text = p.Default ? "YES" : "NO", Value = p.Default.ToString() }).Distinct(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.PlanningActivityStatusId.ToString()))
                : request.Where(x => x.PlanningActivityStatusId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.PlanningActivityStatusId.ToString())),
                "projectStatusCombinationRule" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.ProjectStatusCombinationRule.ToString()))
                : request.Where(x => x.ProjectStatusCombinationRule.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ProjectStatusCombinationRule.ToString())),
            };
        }

        public override IQueryable<PlanningActivityStatus> PrepareQuery(TipologicaQueryDtoProjectStatusCombinationRule request, ExpressionStarter<PlanningActivityStatus> predicateResult, ExpressionStarter<Planningactivitystatuses> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.PlanningActivityStatus.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.PlanningActivityStatus.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => PlanningActivityStatusMapper.GetPlanningActivityStatusMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDtoProjectStatusCombinationRule dto)
        {
            var entityExists = await _repositoryWrapper.PlanningActivityStatus.FindByCondition(
               x => x.Planningactivitystatus.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Planningactivitystatusid
                };
            }
            if (dto.Default == true)
            {
                var defaults = await _repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Default == true).ToListAsync();

                foreach (var def in defaults)
                {
                    def.Default = false;
                    _repositoryWrapper.PlanningActivityStatus.Update(def);
                    await _repositoryWrapper.SaveAsync();

                }
            }
            PlanningActivityStatus entity = new PlanningActivityStatus() { PlanningActivityStatusId = dto.Id, PlanningActivityStatusDescription = dto.Description, ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule };
            _repositoryWrapper.PlanningActivityStatus.Create(PlanningActivityStatusMapper.SetPlanningActivityStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDtoProjectStatusCombinationRule dto)
        {
            var entityExists = await _repositoryWrapper.PlanningActivityStatus.FindByCondition(
               x => x.Planningactivitystatusid != dto.Id
               && x.Planningactivitystatus.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Planningactivitystatusid
                };
            }
            if (dto.Default == true)
            {
                var defaults = await _repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Default == true).ToListAsync();

                foreach (var def in defaults)
                {
                    def.Default = false;
                    _repositoryWrapper.PlanningActivityStatus.Update(def);
                    await _repositoryWrapper.SaveAsync();

                }
            }
            PlanningActivityStatus entity = new PlanningActivityStatus() { PlanningActivityStatusId = dto.Id, PlanningActivityStatusDescription = dto.Description, ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule };
            _repositoryWrapper.PlanningActivityStatus.Update(PlanningActivityStatusMapper.SetPlanningActivityStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            //TODO: verificare mapping relazioni (manca settingsupdateplannedactivities)
            var entity = await _repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Planningactivitystatusid == id).SingleAsync();
            _repositoryWrapper.PlanningActivityStatus.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Planningactivitystatusid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Planningactivitystatusid == id).SingleAsync();
            _repositoryWrapper.PlanningActivityStatus.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Planningactivitystatusid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {

            var plannedActivity = _repositoryWrapper.PlannedActivity
                                    .FindByCondition(x => x.Planningactivitystatusid == id)
                                    .Include(x => x.Plannedactivityresource)
                                    .Include(x => x.Activitystatus)
                                    .Include(x => x.Deliverystatus)
                                    .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                                    .ToArray();
            var entity = await _repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Planningactivitystatusid == id).SingleAsync();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (plannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivity });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Planned Activity Status",
                        RecordName = entity.Planningactivitystatus,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public TipologicaGridDtoProjectStatusCombinationRule GetCreatePage()
        {
            var dto = new TipologicaGridDtoProjectStatusCombinationRule();
            return dto;
        }

        public TipologicaGridDtoProjectStatusCombinationRule GetUpdatePage(short id)
        {
            var entity = PlanningActivityStatusMapper.GetPlanningActivityStatusMapper(_repositoryWrapper.PlanningActivityStatus
                .FindByCondition(x => x.Planningactivitystatusid == id)
                .Include(x => x.ModificationuserNavigation)
                .Single());
            var dto = new TipologicaGridDtoProjectStatusCombinationRule() { Id = (short)entity.PlanningActivityStatusId, LastModifiedBy = entity.ModificationUserEntity.Email, Description = entity.PlanningActivityStatusDescription, LastModified = entity.ModificationDate, ProjectStatusCombinationRule = entity.ProjectStatusCombinationRule, Default = entity.Default };
            return dto;
        }


    }
}
