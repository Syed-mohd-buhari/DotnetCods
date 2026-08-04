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
    public class DeliveryStatusManager : GridBaseAsync<DeliveryStatus, TipologicaGridDtoCombinationRule, TipologicaQueryDtoCombinationRule, Deliverystatuses>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public DeliveryStatusManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
  

        public override ExpressionStarter<Deliverystatuses> ApplyFilterForOracleModel(TipologicaQueryDtoCombinationRule request)
        {
            var predicateResult = PredicateBuilder.New<Deliverystatuses>();
            var predicateInner = PredicateBuilder.New<Deliverystatuses>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverystatuses>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Deliverystatus == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverystatuses>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Deliverystatusid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverystatuses>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Rule != null && request.Rule.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverystatuses>();
                foreach (var item in request.Rule)
                    predicateInner.Or(x => x.Rule == item);
                predicateResult.And(predicateInner);
            }
            if (request.ProjectStatusCombinationRule != null && request.ProjectStatusCombinationRule.Any())
            {
                predicateInner = PredicateBuilder.New<Deliverystatuses>();
                foreach (var item in request.ProjectStatusCombinationRule)
                    predicateInner.Or(x => x.Projectstatuscombinationrule == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Deliverystatuses>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Deliverystatuses>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDtoCombinationRule> CastObjectToDto(IQueryable<DeliveryStatus> request)
        {
            return  request.Select(dto => new TipologicaGridDtoCombinationRule()
            {
                Id = dto.DeliveryStatusId,
                Description = dto.DeliveryStatusDescription,
                LastModified = dto.ModificationDate,
                Rule = dto.Rule,
                 ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule,
                 LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<DeliveryStatus, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<DeliveryStatus, object>>[]>
            {
                ["description"] = new Expression<Func<DeliveryStatus, object>>[] { p => p.DeliveryStatusDescription },
                ["id"] = new Expression<Func<DeliveryStatus, object>>[] { p => p.DeliveryStatusId },
                ["rule"] = new Expression<Func<DeliveryStatus, object>>[] { p => p.Rule },
                ["projectStatusCombinationRule"] = new Expression<Func<DeliveryStatus, object>>[] { p => p.ProjectStatusCombinationRule },
                ["lastModifiedBy"] = new Expression<Func<DeliveryStatus, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<DeliveryStatus> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.DeliveryStatusDescription))
                : request.Where(x => x.DeliveryStatusDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.DeliveryStatusDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.DeliveryStatusId.ToString()))
                : request.Where(x => x.DeliveryStatusId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.DeliveryStatusId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "rule" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.Rule.ToString()))
                    : request.Where(x => x.Rule.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Rule.ToString())),
                "projectStatusCombinationRule" => string.IsNullOrEmpty(propertyFilter)
            ? request.Select(x => new FilterValueDto(x.ProjectStatusCombinationRule.ToString()))
            : request.Where(x => x.ProjectStatusCombinationRule.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ProjectStatusCombinationRule.ToString())),
            };
        }

        public override IQueryable<DeliveryStatus> PrepareQuery(TipologicaQueryDtoCombinationRule request, ExpressionStarter<DeliveryStatus> predicateResult , ExpressionStarter<Deliverystatuses> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.DeliveryStatus.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.DeliveryStatus.FindAll();

           return  query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => DeliveryStatusMapper.GetDeliveryStatusMapper(p)).AsQueryable();

     
        }

        public async Task<ResultDto> Add(TipologicaGridDtoCombinationRule dto)
        {
            var entityExists = await _repositoryWrapper.DeliveryStatus.FindByCondition(
               x => x.Deliverystatus.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Deliverystatusid
                };
            }
            DeliveryStatus entity = new DeliveryStatus() { DeliveryStatusId = dto.Id, DeliveryStatusDescription = dto.Description, Rule = dto.Rule, ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule };
            _repositoryWrapper.DeliveryStatus.Create(DeliveryStatusMapper.SetDeliveryStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDtoCombinationRule dto)
        {
            var entityExists = await _repositoryWrapper.DeliveryStatus.FindByCondition(
               x => x.Deliverystatusid != dto.Id 
               && x.Deliverystatus.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Deliverystatusid
                };
            }
            DeliveryStatus entity = new DeliveryStatus() { DeliveryStatusId = dto.Id, DeliveryStatusDescription = dto.Description, Rule = dto.Rule, ProjectStatusCombinationRule = dto.ProjectStatusCombinationRule };
            _repositoryWrapper.DeliveryStatus.Update(DeliveryStatusMapper.SetDeliveryStatusMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatusid == id).SingleAsync();
            _repositoryWrapper.DeliveryStatus.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Deliverystatusid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatusid == id).SingleAsync();
            _repositoryWrapper.DeliveryStatus.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Deliverystatusid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var PlannedActivity = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Deliverystatusid == id)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Activitystatus)
                .Include(x => x.Deliverystatus)
                .Select(x => PlannedActivityMapper.Get(x,true).toLinkedPlannedActivityName())
                .ToArray();
            var SettingsUpdatePlannedActivity = _repositoryWrapper.SettingsUpdatePlannedActivity
                .FindByCondition(x => x.Deliverystatusid == id).Select(x => x.Settingsupdateplnactdes).ToArray();

            var entity = await _repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatusid == id)
               .SingleAsync();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (PlannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = PlannedActivity });
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
                        EntityName = "Delivery Status",
                        RecordName = entity.Deliverystatus,
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
            var model = _repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatusid == id)
                .Include(x => x.ModificationuserNavigation).Single();

            var entity = DeliveryStatusMapper.GetDeliveryStatusMapper(model);
            var dto = new TipologicaGridDtoCombinationRule() { Id = (short)entity.DeliveryStatusId, Description = entity.DeliveryStatusDescription, LastModified = entity.ModificationDate, Rule = entity.Rule, LastModifiedBy = entity.ModificationUserEntity.Email, ProjectStatusCombinationRule = entity.ProjectStatusCombinationRule };
            return dto;
        }

      
    }
}
