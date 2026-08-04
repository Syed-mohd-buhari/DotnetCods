using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.BusinessManager.ExtensionMethod.PlannedActivityResource;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.Settings.SettingsUpdatePlannedActivity;
using CAM.Entities.Mappers.Setting;
using CAM.Entities.Models.Settings;
using CAM.Enum;
using CAM.Infrastucture;
using IdentityServer4.Extensions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Settings
{
    public class SettingsUpdatePlannedActivityManager : GridBaseAsync<SettingsUpdatePlannedActivity, SettingsUpdatePlannedActivityDtoGrid, SettingsUpdatePlannedActivityQueryDto, Settingsupdateplannedactivity>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        private readonly GridCustomColumnManager _columnManager;

        public SettingsUpdatePlannedActivityManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Settingsupdateplannedactivity> ApplyFilterForOracleModel(SettingsUpdatePlannedActivityQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Settingsupdateplannedactivity>();
            var predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();

            if (request.BudgetAvailability != null && request.BudgetAvailability.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.BudgetAvailability)
                    predicateInner.Or(x => x.Budgetavailability.Budgetavailabilityid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LcmDeploymentStatus != null && request.LcmDeploymentStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.LcmDeploymentStatus)
                    predicateInner.Or(x => x.Settingupdateplannedactivitylcmdeploymentstatus.Any(d => d.Lcmdeploymentstatusid == item));
                predicateResult.And(predicateInner);
            }
            if (request.AssetDeploymentStatus != null && request.AssetDeploymentStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.AssetDeploymentStatus)
                    predicateInner.Or(x => x.Settingupdateplannedactivityassetdeploymentstatus.Any(d => d.Assetdeploymentstatusid == item));
                predicateResult.And(predicateInner);
            }
            if (request.LocalApproval != null && request.LocalApproval.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.LocalApproval)
                    predicateInner.Or(x => x.Localapproval == item);
                predicateResult.And(predicateInner);
            }
            if (request.SettingsUpdatePlannedActivityDescription != null && request.SettingsUpdatePlannedActivityDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.SettingsUpdatePlannedActivityDescription)
                    predicateInner.Or(x => x.Settingsupdateplnactdes == item);
                predicateResult.And(predicateInner);
            }
            if (request.Rule != null && request.Rule.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.Rule)
                    predicateInner.Or(x => x.Rule == item);
                predicateResult.And(predicateInner);
            }
            if (request.RuleElementCount != null && request.RuleElementCount.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.RuleElementCount)
                    predicateInner.Or(x => x.Ruleelementcount == item);
                predicateResult.And(predicateInner);
            }
            if (request.Order != null && request.Order.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.Order)
                    predicateInner.Or(x => x.Order == item);
                predicateResult.And(predicateInner);
            }
            if (request.MaxOrder != null && request.MaxOrder.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.MaxOrder)
                    predicateInner.Or(x => x.Maxorder == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                if (request.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModifiedValue.StartDate);
                if (request.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            if (request.PlanningActivityResource != null && request.PlanningActivityResource.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.PlanningActivityResource)
                    predicateInner.Or(x => x.Plannedactivityresource.Plannedactivityresourceid == item);
                predicateResult.And(predicateInner);
            }

            if (request.CrossSetting != null && request.CrossSetting.Any())
            {
                var Settings = _repositoryWrapper.SettingsUpdatePlannedActivity
                    .FindAll()
                    .Include(x => x.CrosssettingsupdateplannedactivitySettingsupdateplnactin)
                    .ThenInclude(x => x.Settingsupdateplnactout)
                    .ToList(); // Retrieve data from the database

                var crossSettingIds = request.CrossSetting
                    .SelectMany(item => Settings
                        .Where(entity =>
                            entity.CrosssettingsupdateplannedactivitySettingsupdateplnactin.Any(inItem =>
                                inItem.Settingsupdateplnactout.Settingsupdateplnactdes
                                    .Split(new[] { " ; " }, StringSplitOptions.RemoveEmptyEntries)
                                    .Contains(item)))
                        .Select(entity => entity.Settingsupdateplnactid))
                    .Distinct()
                    .ToList();

                predicateResult.And(x => crossSettingIds.Contains(x.Settingsupdateplnactid));
            }
            if (request.SuccessorPlannedActivityTypeResource != null && request.SuccessorPlannedActivityTypeResource.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.SuccessorPlannedActivityTypeResource)
                    predicateInner.Or(x => x.Successorplannedactivityresource.Plannedactivityresourceid == item);
                predicateResult.And(predicateInner);
            }
            if (request.RuleforSuccessorPlannedActivityCreation != null && request.RuleforSuccessorPlannedActivityCreation.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();

                foreach (var item in request.RuleforSuccessorPlannedActivityCreation)
                    predicateInner.Or(x => x.Ruleforsuccessorplannedactivitycreation == item);
                predicateResult.And(predicateInner);
            }
            if (request.PlanningActivityStatus != null && request.PlanningActivityStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.PlanningActivityStatus)
                    predicateInner.Or(x => x.Planningactivitystatus.Planningactivitystatusid == item);
                predicateResult.And(predicateInner);
            }
            if (request.DeliveryStatus != null && request.DeliveryStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.DeliveryStatus)
                    predicateInner.Or(x => x.Deliverystatus.Deliverystatusid == short.Parse(item));
                predicateResult.And(predicateInner);
            }
            if (request.PlannedActivityTypeFor != null && request.PlannedActivityTypeFor.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.PlannedActivityTypeFor)
                    predicateInner.Or(x => x.Plannedactivitytypefor == item);
                predicateResult.And(predicateInner);
            }
            if (request.PlannedActivityTypeDescription != null && request.PlannedActivityTypeDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.PlannedActivityTypeDescription)
                    predicateInner.Or(x => x.Plannedactivityresource.RulelinkeddcNavigation.Plannedactivitytypedescription == item);
                predicateResult.And(predicateInner);
            }
            if (request.SpecifyDC != null && request.SpecifyDC.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.SpecifyDC)
                    predicateInner.Or(x => x.Specifydc == item);
                predicateResult.And(predicateInner);
            }
            if (request.NeedPlannedAsset != null && request.NeedPlannedAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.NeedPlannedAsset)
                    predicateInner.Or(x => x.Needplannedasset == item);
                predicateResult.And(predicateInner);
            }
            if (request.IsRollback != null && request.IsRollback.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.IsRollback)
                    predicateInner.Or(x => x.Isrollback == item);
                predicateResult.And(predicateInner);
            }
            if (request.MSStatus != null && request.MSStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.MSStatus)
                    predicateInner.Or(x => x.Milestonestatus == item);
                predicateResult.And(predicateInner);
            }
            if (request.MSStatusDuration != null && request.MSStatusDuration.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.MSStatusDuration)
                    predicateInner.Or(x => x.Milestonestatusduration == item);
                predicateResult.And(predicateInner);
            }
            if (request.IsMileStone != null && request.IsMileStone.Any())
            {
                predicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in request.IsMileStone)
                    predicateInner.Or(x => x.Ismilestone == Convert.ToBoolean(item));
                predicateResult.And(predicateInner);
            }
            return predicateResult;

        }

        public override List<SettingsUpdatePlannedActivityDtoGrid> CastObjectToDto(IQueryable<SettingsUpdatePlannedActivity> request)
        {
            return request.Select(dto => new SettingsUpdatePlannedActivityDtoGrid()
            {
                SettingsUpdatePlannedActivityId = dto.SettingsUpdatePlannedActivityId,
                SettingsUpdatePlannedActivityDescription = dto.SettingsUpdatePlannedActivityDescription,
                LastModified = dto.ModificationDate,
                LocalApproval = dto.LocalApproval,
                DeliveryStatus = dto.DeliveryStatus.DeliveryStatusDescription,
                PlanningActivityStatus = dto.PlanningActivityStatus.PlanningActivityStatusDescription,
                PlanningActivityResource = dto.PlannedActivityResource != null ? dto.PlannedActivityResource.PlannedActivityResourceDescription : "",
                SuccessorPlannedActivityTypeResource = dto.SuccessorPlannedActivityTypeResource != null ? dto.SuccessorPlannedActivityTypeResource.PlannedActivityResourceDescription : "",
                RuleforSuccessorPlannedActivityCreation = dto.RuleForSuccessorPlannedActivityCreation,
                BudgetAvailability = dto.BudgetAvailability.BudgetAvailabilityDescription,
                LcmDeploymentStatus = string.Join(",", dto.SettingUpdatePlannedActivityLcmDeploymentStatus.Select(p => p.LcmDeploymentStatus.Description)),
                AssetDeploymentStatus = string.Join(",", dto.SettingUpdatePlannedActivityAssetDeploymentStatus.Select(p => p.AssetDeploymentStatus.DeploymentStatusDescription)),
                Order = dto.Order,
                RuleElementCount = dto.RuleElementCount,
                MaxOrder = dto.MaxOrder,
                Rule = dto.Rule,
                SpecifyDC = (bool)dto.SpecifyDC,
                NeedPlannedAsset = (dto.NeedPlannedAsset == null) ? false : (bool)dto.NeedPlannedAsset,
                PlannedActivityTypeDescription = dto.PlannedActivityResource.RuleLinkedDcNavigation.PlannedActivityTypeDescription,
                PlannedActivityTypeForValue = dto.toPlannedActivityTypeIsFor(),
                LastModifiedBy = dto.ModificationUserEntity.Email,
                CrossSetting = (dto.CrossSettingsIn.Count() > 0) ? dto.CrossSettingsIn.Where(x => !x.Deleted).Select(x => x.SettingsUpdatePlannedActivityOut)
                  .Select(y => y.SettingsUpdatePlannedActivityDescription).Aggregate(
                      "", (current, next) => current + " ; " + next) : "",
                IsRollback = (bool)dto.IsRollback,
                MsStatus = MSStatusToText(dto.MSStatus),
                MsStatusDuration = dto.MSStatusDuration,
                IsMileStone = dto.IsMileStone.Value == ConstantValueFilter.isTrue ? ConstantValueFilter.Yes : ConstantValueFilter.No,
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<SettingsUpdatePlannedActivity, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SettingsUpdatePlannedActivity, object>>[]>
            {
                ["settingsUpdatePlannedActivityDescription"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.SettingsUpdatePlannedActivityDescription },
                ["settingsUpdatePlannedActivityId"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.DeliveryStatus },
                ["ruleElementCount"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.RuleElementCount },
                ["localApproval"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.LocalApproval },
                ["planningActivityStatusId"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.PlanningActivityStatus.PlanningActivityStatusDescription },
                ["planningActivityResourceId"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.PlannedActivityResource.PlannedActivityResourceDescription },
                ["successorPlannedActivityId"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.SuccessorPlannedActivityTypeResource.PlannedActivityResourceDescription },
                ["deliveryStatusId"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.DeliveryStatus.DeliveryStatusDescription },
                ["budgetAvailability"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.BudgetAvailability.BudgetAvailabilityDescription },
                ["lastModifiedBy"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.ModificationUserEntity.Email },
                ["order"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.Order },
                ["rule"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.Rule },
                ["specifyDC"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.SpecifyDC },
                ["maxOrder"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.MaxOrder },
                ["plannedActivityTypeDescription"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.PlannedActivityResource.RuleLinkedDcNavigation.PlannedActivityTypeDescription },
                ["isRollback"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.IsRollback },
                ["isMileStone"] = new Expression<Func<SettingsUpdatePlannedActivity, object>>[] { p => p.IsMileStone },
            };
        }

        public override IQueryable<SettingsUpdatePlannedActivity> PrepareQuery(SettingsUpdatePlannedActivityQueryDto request, ExpressionStarter<SettingsUpdatePlannedActivity> predicateResult, ExpressionStarter<Settingsupdateplannedactivity> oracleObject = null)
        {
            var query = oracleObject.IsStarted
               ? _repositoryWrapper.SettingsUpdatePlannedActivity
                   .FindByCondition(oracleObject)
                   .Include(x => x.CrosssettingsupdateplannedactivitySettingsupdateplnactin)
                   .ThenInclude(x => x.Settingsupdateplnactout)
                   .Include(x => x.CrosssettingsupdateplannedactivitySettingsupdateplnactout)
                   .Include(x => x.Plannedactivityresource).ThenInclude(x => x.RulelinkeddcNavigation)
                   .Include(x => x.Deliverystatus)
                   .Include(x => x.Budgetavailability)
                   .Include(x => x.Planningactivitystatus)
                   .Include(x => x.Plannedactivityresource)
                   .Include(x => x.Successorplannedactivityresource)
                   .Include(x => x.ModificationuserNavigation)
                   .Include(x => x.Settingupdateplannedactivitylcmdeploymentstatus)
                   .ThenInclude(x => x.Lcmdeploymentstatus)
                   .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus)
                   .ThenInclude(x => x.Assetdeploymentstatus)
                   .AsQueryable()
               : _repositoryWrapper.SettingsUpdatePlannedActivity.FindAll()
                   .Include(x => x.CrosssettingsupdateplannedactivitySettingsupdateplnactin)
                   .ThenInclude(x => x.Settingsupdateplnactout)
                   .Include(x => x.CrosssettingsupdateplannedactivitySettingsupdateplnactout)
                   .Include(x => x.Plannedactivityresource).ThenInclude(x => x.RulelinkeddcNavigation)
                   .Include(x => x.Deliverystatus)
                   .Include(x => x.Budgetavailability)
                   .Include(x => x.Planningactivitystatus)
                   .Include(x => x.Plannedactivityresource)
                   .Include(x => x.Successorplannedactivityresource)
                   .Include(x => x.ModificationuserNavigation)
                   .Include(x => x.Settingupdateplannedactivitylcmdeploymentstatus)
                   .ThenInclude(x => x.Lcmdeploymentstatus)
                   .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus)
                   .ThenInclude(x => x.Assetdeploymentstatus)
                   .AsQueryable();
            var kk = query.ToList();


            var result = query.Select(p => SettingsUpdatePlannedActivityMapper.Get(p)).ToList();

            foreach (var item in result)
            {
                item.CrossSettingsIn = query.FirstOrDefault(p => p.Settingsupdateplnactid == item.SettingsUpdatePlannedActivityId).CrosssettingsupdateplannedactivitySettingsupdateplnactin.Select(p => CrossSettingsUpdatePlannedActivityMapper.Get(p)).ToList();
                item.CrossSettingsOut = query.FirstOrDefault(p => p.Settingsupdateplnactid == item.SettingsUpdatePlannedActivityId).CrosssettingsupdateplannedactivitySettingsupdateplnactout.Select(p => CrossSettingsUpdatePlannedActivityMapper.Get(p)).ToList();
            }
            return result.AsQueryable();
        }

        public async Task<ResultDto> Add(SettingsUpdatePlannedActivityDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(
                x => x.Deliverystatusid == dto.DeliveryStatusId && x.Plannedactivityresourceid == dto.PlanningActivityResourceId && x.Plannedactivitytypefor == dto.PlannedActivityTypeFor
                , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Settingsupdateplnactid
                };
            }
            var order = _repositoryWrapper.SettingsUpdatePlannedActivity.FindAll().Count();

            SettingsUpdatePlannedActivity entity = new SettingsUpdatePlannedActivity()
            {
                SettingsUpdatePlannedActivityDescription = dto.SettingsUpdatePlannedActivityDescription,
                PlanningActivityStatusId = dto.PlanningActivityStatusId,
                PlanningActivityResourceId = dto.PlanningActivityResourceId,
                SuccessorPlannedActivityId = dto.SuccessorPlannedActivityId,
                RuleForSuccessorPlannedActivityCreation = dto.RuleforSuccessorPlannedActivityCreation,
                BudgetAvailabilityId = dto.BudgetAvailabilityId,
                LocalApproval = dto.LocalApproval,
                DeliveryStatusId = dto.DeliveryStatusId,
                Rule = dto.Rule,
                RuleElementCount = dto.RuleElementCount,
                Order = order + 1,
                SpecifyDC = dto.SpecifyDC,
                MaxOrder = (dto.MaxOrder <= 0) ? 1 : dto.MaxOrder,
                PlannedActivityTypeFor = dto.PlannedActivityTypeFor,
                NeedPlannedAsset = dto.NeedPlannedAsset,
                IsRollback = dto.IsRollback != null ? true : false,
                MSStatus = Convert.ToInt32(dto.MsStatus),
                MSStatusDuration = dto.MsStatusDuration,
                IsMileStone = dto.IsMileStone == ConstantValueFilter.Yes ? true : false,
            };
            var model = SettingsUpdatePlannedActivityMapper.Set(entity);


            _repositoryWrapper.SettingsUpdatePlannedActivity.Create(model);
            await _repositoryWrapper.SaveAsync();
            if (dto.LcmDeploymentStatusIds != null)
            {
                foreach (var item in dto.LcmDeploymentStatusIds)
                {
                    model.Settingupdateplannedactivitylcmdeploymentstatus.Add(new Settingupdateplannedactivitylcmdeploymentstatus() { Lcmdeploymentstatusid = item, Settingupdateplannedactivityid = model.Settingsupdateplnactid });
                }
            }

            if (dto.AssetDeploymentStatusIds != null)
            {
                foreach (var item in dto.AssetDeploymentStatusIds)
                {
                    model.Settingupdateplannedactivityassetdeploymentstatus.Add(new Settingupdateplannedactivityassetdeploymentstatus() { Assetdeploymentstatusid = item, Settingupdateplannedactivityid = model.Settingsupdateplnactid });
                }
            }

            await _repositoryWrapper.SaveAsync();
            await InsertCrossSettings(dto.CrossSettingsOutIds, entity.SettingsUpdatePlannedActivityId);
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        private async Task InsertCrossSettings(IEnumerable<RelatedResource> crossIds, short id)
        {
            if (crossIds != null)
            {
                foreach (var crossId in crossIds)
                {
                    CrossSettingsUpdatePlannedActivity cross = new CrossSettingsUpdatePlannedActivity();
                    cross.SettingsUpdatePlannedActivityInId = id;
                    cross.SettingsUpdatePlannedActivityOutId = Convert.ToInt16(crossId.Id);
                    cross.CrossSettingsRule = Convert.ToInt16(crossId.Value);
                    _repositoryWrapper.CrossSettingsUpdatePlannedActivity.Create(CrossSettingsUpdatePlannedActivityMapper.Set(cross));
                    await _repositoryWrapper.SaveAsync();
                }
            }
        }

        public async Task<ResultDto> Update(SettingsUpdatePlannedActivityDtoUpdate dto)
        {
            var anotherEntityWithSameKeyExists = await _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(
                x => x.Deliverystatusid == dto.DeliveryStatusId
                && x.Plannedactivityresourceid == dto.PlanningActivityResourceId && x.Plannedactivitytypefor == dto.PlannedActivityTypeFor
                   && x.Settingsupdateplnactid != dto.SettingsUpdatePlannedActivityId, true).FirstOrDefaultAsync();

            if (anotherEntityWithSameKeyExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = anotherEntityWithSameKeyExists.Deleted.Value ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
                    Data = anotherEntityWithSameKeyExists.Settingsupdateplnactid
                };
            }

            SettingsUpdatePlannedActivity entity = new SettingsUpdatePlannedActivity()
            {
                SettingsUpdatePlannedActivityId = dto.SettingsUpdatePlannedActivityId,
                SettingsUpdatePlannedActivityDescription = dto.SettingsUpdatePlannedActivityDescription,
                PlanningActivityStatusId = dto.PlanningActivityStatusId,
                PlanningActivityResourceId = dto.PlanningActivityResourceId,
                SuccessorPlannedActivityId = dto.SuccessorPlannedActivityId,
                RuleForSuccessorPlannedActivityCreation = dto.RuleforSuccessorPlannedActivityCreation,
                BudgetAvailabilityId = dto.BudgetAvailabilityId,
                LocalApproval = dto.LocalApproval,
                DeliveryStatusId = dto.DeliveryStatusId,
                Order = dto.Order,
                Rule = dto.Rule,
                SpecifyDC = dto.SpecifyDC,
                RuleElementCount = dto.RuleElementCount,
                MaxOrder = (dto.MaxOrder <= 0) ? 1 : dto.MaxOrder,
                PlannedActivityTypeFor = dto.PlannedActivityTypeFor,
                NeedPlannedAsset = dto.NeedPlannedAsset,
                IsRollback = dto.IsRollback,
                MSStatus = Convert.ToInt32(dto.MsStatus),
                MSStatusDuration = dto.MsStatusDuration,
                IsMileStone = dto.IsMileStone == ConstantValueFilter.Yes ? true : false,
            };

            var oldLcmDeploymentStatus = _repositoryWrapper.SettingUpdatePlannedActivityLcmDeploymentStatusRepository.FindByCondition(p => p.Settingupdateplannedactivityid == dto.SettingsUpdatePlannedActivityId).ToList();
            if (oldLcmDeploymentStatus != null && oldLcmDeploymentStatus.Count() > 0)
            {
                foreach (var item in oldLcmDeploymentStatus)
                {
                    _repositoryWrapper.SettingUpdatePlannedActivityLcmDeploymentStatusRepository.DeleteDeep(item);
                }
                _repositoryWrapper.Save();
            }
            if (dto.LcmDeploymentStatusIds != null && dto.LcmDeploymentStatusIds.Count() > 0)
            {
                foreach (var item in dto.LcmDeploymentStatusIds)
                {
                    _repositoryWrapper.SettingUpdatePlannedActivityLcmDeploymentStatusRepository.Create(new Settingupdateplannedactivitylcmdeploymentstatus() { Settingupdateplannedactivityid = dto.SettingsUpdatePlannedActivityId, Lcmdeploymentstatusid = item });
                }
                _repositoryWrapper.Save();
            }

            var oldAssetDeploymentStatus = _repositoryWrapper.SettingUpdatePlannedActivityAssetDeploymentStatusRepository.FindByCondition(p => p.Settingupdateplannedactivityid == dto.SettingsUpdatePlannedActivityId).ToList();
            if (oldAssetDeploymentStatus != null && oldAssetDeploymentStatus.Count() > 0)
            {
                foreach (var item in oldAssetDeploymentStatus)
                {
                    _repositoryWrapper.SettingUpdatePlannedActivityAssetDeploymentStatusRepository.DeleteDeep(item);
                }
                _repositoryWrapper.Save();
            }
            if (dto.AssetDeploymentStatusIds != null && dto.AssetDeploymentStatusIds.Count() > 0)
            {
                foreach (var item in dto.AssetDeploymentStatusIds)
                {
                    _repositoryWrapper.SettingUpdatePlannedActivityAssetDeploymentStatusRepository.Create(new Settingupdateplannedactivityassetdeploymentstatus() { Settingupdateplannedactivityid = dto.SettingsUpdatePlannedActivityId, Assetdeploymentstatusid = item });
                }
                _repositoryWrapper.Save();
            }

            _repositoryWrapper.SettingsUpdatePlannedActivity.Update(SettingsUpdatePlannedActivityMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();

            var crossSetting = _repositoryWrapper.CrossSettingsUpdatePlannedActivity
                .FindByCondition(x => x.Settingsupdateplnactinid == dto.SettingsUpdatePlannedActivityId)
                .ToList();
            if (dto.CrossSettingsOutIds != null)
            {
                foreach (var item in crossSetting)
                {
                    _repositoryWrapper.CrossSettingsUpdatePlannedActivity.DeleteDeep(item);
                    _repositoryWrapper.Save();
                }
                await InsertCrossSettings(dto.CrossSettingsOutIds, entity.SettingsUpdatePlannedActivityId);
            }
            else
            {
                foreach (var item in crossSetting)
                {
                    _repositoryWrapper.CrossSettingsUpdatePlannedActivity.DeleteDeep(item);
                    _repositoryWrapper.Save();
                }
            }

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.SettingsUpdatePlannedActivityId
            };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Settingsupdateplnactid == id).SingleAsync();

            _repositoryWrapper.SettingsUpdatePlannedActivity.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Settingsupdateplnactid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.SettingsUpdatePlannedActivity
                .FindByCondition(x => x.Settingsupdateplnactid == id)
                .Include(x => x.Settingupdateplannedactivitylcmdeploymentstatus).ThenInclude(x => x.Lcmdeploymentstatus)
                .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus).ThenInclude(x => x.Assetdeploymentstatus)
                .SingleAsync();
            if (entity.Settingupdateplannedactivitylcmdeploymentstatus != null && entity.Settingupdateplannedactivitylcmdeploymentstatus.Count > 0)
            {
                var lcmDeploymentStatusLst = entity.Settingupdateplannedactivitylcmdeploymentstatus.ToList();

                foreach (var toDelete in lcmDeploymentStatusLst)
                    _repositoryWrapper.SettingUpdatePlannedActivityLcmDeploymentStatusRepository.DeleteDeep(toDelete);
            }

            if (entity.Settingupdateplannedactivityassetdeploymentstatus != null && entity.Settingupdateplannedactivityassetdeploymentstatus.Count > 0)
            {
                var assetDeploymentStatusLst = entity.Settingupdateplannedactivityassetdeploymentstatus.ToList();

                foreach (var toDelete in assetDeploymentStatusLst)
                    _repositoryWrapper.SettingUpdatePlannedActivityAssetDeploymentStatusRepository.DeleteDeep(toDelete);
            }

            _repositoryWrapper.SettingsUpdatePlannedActivity.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Settingsupdateplnactid
            };
        }


        public async Task<ResultDto> GetRelatedRecords(long id)
        {

            var settingPA = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Settingsupdateplnactid == id).FirstOrDefault();
            var entities = new List<string>();

            if (settingPA.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering)
            {
                entities = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid != null && x.Plannedactivityresourceid == settingPA.Plannedactivityresourceid && x.Deliverystatusid == settingPA.Deliverystatusid)
                .Include(x => x.Deliverystatus)
                .Include(x => x.Plannedactivityresource)
                .Select(x => x.Plannedactivityresource.Plannedactivityresource + ConstantValueFilter.paResourceLCM).Distinct().ToList();
            }
            else if (settingPA.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.AddAsset)
            {
                entities = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityresourceid == settingPA.Plannedactivityresourceid && x.Deliverystatusid == settingPA.Deliverystatusid)
                 .Include(x => x.Deliverystatus)
                .Include(x => x.Plannedactivityresource)
                .Select(x => x.Plannedactivityresource.Plannedactivityresource + ConstantValueFilter.paResourceAddAsset).ToList();
            }
            else if (settingPA.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.EditAsset)
            {
                entities = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Networkelementasplannedid != null && x.Foreditasset == true && x.Plannedactivityresourceid == settingPA.Plannedactivityresourceid && x.Deliverystatusid == settingPA.Deliverystatusid)
                .Include(x => x.Plannedactivityresource)
                .Select(x => x.Plannedactivityresource.Plannedactivityresource + ConstantValueFilter.paResoureEditAsset).ToList(); ;
            }
            else if (settingPA.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.DesignAspect)
            {
                entities = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Designaspectid != null && x.Plannedactivityresourceid == settingPA.Plannedactivityresourceid && x.Deliverystatusid == settingPA.Deliverystatusid)
                 .Include(x => x.Deliverystatus)
                .Include(x => x.Plannedactivityresource)
                .Select(x => x.Plannedactivityresource.Plannedactivityresource + ConstantValueFilter.paResoureDCF).ToList();
            }

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Count > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = entities.ToArray() });

            var entity = await _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Settingsupdateplnactid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Setting Update Planned Activity Status",
                        RecordName = entity.Settingsupdateplnactdes,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public IDictionary<short, string> GetCrossSettings(short plannedActivityFor, int plannedActivityTypeId, int currentDeliveryStatus)
        {
            var CrossSettingsList = new Dictionary<short, string>();
            var crossSettings = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition
                (x => x.Plannedactivitytypefor == plannedActivityFor && x.Plannedactivityresourceid == plannedActivityTypeId && x.Deliverystatusid != currentDeliveryStatus);
            CrossSettingsList = crossSettings.ToDictionary(x => x.Settingsupdateplnactid, x => x.Settingsupdateplnactdes);
            return CrossSettingsList;
        }
        public SettingsUpdatePlannedActivityDtoCreate GetCreatePage()
        {
            var deliveryStatusResource = _repositoryWrapper.DeliveryStatus.FindAll();
            var planningActivityStatusResource = _repositoryWrapper.PlanningActivityStatus.FindAll();
            //var planningActivityResource = _repositoryWrapper.PlannedActivityResourceRepository
            //    .FindByCondition(x => x.Foraddasset == true || x.Foreditasset == true || x.Forlcm == true);

            var successorPlannedActivityTypeResource = _repositoryWrapper.PlannedActivityResourceRepository
                .FindByCondition(x => x.Forlcm == true);

            var budgetAvaibilityResource = _repositoryWrapper.BudgetAvailability.FindAll();
            var crossSettings = _repositoryWrapper.SettingsUpdatePlannedActivity.FindAll();
            var lcmDeploymentStatus = _repositoryWrapper.LcmDeploymentStatusRepository.FindAll();
            var model = new SettingsUpdatePlannedActivityDtoCreate
            {
                DeliveryStatusResource = deliveryStatusResource.ToDictionary(x => x.Deliverystatusid,
                    x => x.Deliverystatus),
                PlanningActivityStatusResource = planningActivityStatusResource.ToDictionary(x => x.Planningactivitystatusid,
                    x => x.Planningactivitystatus),
                //PlanningActivityResource = planningActivityResource.ToDictionary(x => x.Plannedactivityresourceid,
                //    x => x.Plannedactivityresource),
                SuccessorPlannedActivityTypeResource = successorPlannedActivityTypeResource.ToDictionary(x => x.Plannedactivityresourceid,
                    x => x.Plannedactivityresource),
                BudgetAvaibilityResource = budgetAvaibilityResource.ToDictionary(x => x.Budgetavailabilityid,
                    x => x.Description),
                CrossSettingscResource = null,//crossSettings.ToDictionary(x => x.Settingsupdateplnactid, x => x.Settingsupdateplnactdes),
                // LcmDeploymentStatusResource = lcmDeploymentStatus.ToDictionary(x => x.Id, x => x.Description),
                PlannedActivityTypeFor = null

            };
            return model;
        }

        public SettingsUpdatePlannedActivityDtoUpdate GetUpdatePage(long id)
        {
            var result = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Settingsupdateplnactid == id)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.CrosssettingsupdateplannedactivitySettingsupdateplnactin).ThenInclude(x => x.Settingsupdateplnactout)
                .Include(x => x.CrosssettingsupdateplannedactivitySettingsupdateplnactout)
                .Include(x => x.Settingupdateplannedactivitylcmdeploymentstatus).ThenInclude(x => x.Lcmdeploymentstatus)
                .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus).ThenInclude(x => x.Assetdeploymentstatus)
                .FirstOrDefault();

            var entity = SettingsUpdatePlannedActivityMapper.Get(result);
            entity.CrossSettingsIn = result.CrosssettingsupdateplannedactivitySettingsupdateplnactin.Select(p => CrossSettingsUpdatePlannedActivityMapper.Get(p)).ToList();
            entity.CrossSettingsOut = result.CrosssettingsupdateplannedactivitySettingsupdateplnactout.Select(p => CrossSettingsUpdatePlannedActivityMapper.Get(p)).ToList();
            var lcmDeploymentStatus = _repositoryWrapper.SettingUpdatePlannedActivityLcmDeploymentStatusRepository.FindByCondition(p => p.Settingupdateplannedactivityid == id).Include(p => p.Lcmdeploymentstatus).Select(p => p.Lcmdeploymentstatus).ToList();
            var assetDeploymentStatus = _repositoryWrapper.SettingUpdatePlannedActivityAssetDeploymentStatusRepository.FindByCondition(p => p.Settingupdateplannedactivityid == id).Include(p => p.Assetdeploymentstatus).Select(p => p.Assetdeploymentstatusid).ToList();

            var successorPlannedActivityTypeResource = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Forlcm == true);
            var dto = new SettingsUpdatePlannedActivityDtoUpdate
            {
                SettingsUpdatePlannedActivityId = entity.SettingsUpdatePlannedActivityId,
                SettingsUpdatePlannedActivityDescription = entity.SettingsUpdatePlannedActivityDescription,
                PlanningActivityStatusId = entity.PlanningActivityStatusId,
                PlanningActivityResourceId = entity.PlanningActivityResourceId != null ? entity.PlanningActivityResourceId.Value : (short?)null,
                SuccessorPlannedActivityId = entity.SuccessorPlannedActivityId != null ? entity.SuccessorPlannedActivityId.Value : (short?)null,
                BudgetAvailabilityId = entity.BudgetAvailabilityId,
                LocalApproval = entity.LocalApproval,
                DeliveryStatusId = entity.DeliveryStatusId,
                Order = entity.Order,
                RuleforSuccessorPlannedActivityCreation = entity.RuleForSuccessorPlannedActivityCreation,
                Rule = entity.Rule,
                RuleElementCount = entity.RuleElementCount,
                MaxOrder = entity.MaxOrder,
                LastModified = entity.ModificationDate,
                PlannedActivityTypeFor = entity.PlannedActivityTypeFor,
                SpecifyDC = (bool)entity.SpecifyDC,
                NeedPlannedAsset = (entity.NeedPlannedAsset == null) ? false : (bool)entity.NeedPlannedAsset,
                LastModifiedBy = entity.ModificationUserEntity.Email,
                CrossSettingsOutIds = (entity.CrossSettingsIn.Any()) ? entity.CrossSettingsIn.Where(x => !x.Deleted).Select(x =>
                    new RelatedResource()
                    {
                        Id = x.SettingsUpdatePlannedActivityOutId.ToString(),
                        Value = x.CrossSettingsRule.ToString(),
                    }) : null,
                IsRollback = (bool)entity.IsRollback,
                MsStatus = entity.MSStatus,
                MsStatusDuration = entity.MSStatusDuration,
                IsMileStone = entity.IsMileStone.Value == ConstantValueFilter.isTrue ? ConstantValueFilter.Yes : ConstantValueFilter.No,
            };


            #region lookUp
            var deliveryStatusResource = _repositoryWrapper.DeliveryStatus.FindAll();

            dto.DeliveryStatusResource = deliveryStatusResource.ToDictionary(x => x.Deliverystatusid,
                x => x.Deliverystatus);

            if (!dto.DeliveryStatusResource.ContainsKey(dto.DeliveryStatusId))
            {
                var ds = _repositoryWrapper.DeliveryStatus.FindByCondition(
                    x => x.Deliverystatusid == dto.DeliveryStatusId,
                    includeDeleted: true).SingleOrDefault();
                if (ds != null)
                {
                    dto.DeliveryStatusResource.Add(ds.Deliverystatusid, ds.Deliverystatus);
                }

            }

            var budgetAv = _repositoryWrapper.BudgetAvailability.FindAll();
            dto.BudgetAvaibilityResource =
                budgetAv.ToDictionary(x => x.Budgetavailabilityid, x => x.Description);
            if (dto.BudgetAvailabilityId != 0 && !dto.BudgetAvaibilityResource.ContainsKey(dto.BudgetAvailabilityId))
            {
                var data = _repositoryWrapper.BudgetAvailability.FindByCondition(
                    x => x.Budgetavailabilityid == dto.BudgetAvailabilityId,
                    includeDeleted: true).SingleOrDefault();
                if (data != null)
                {
                    dto.BudgetAvaibilityResource.Add(data.Budgetavailabilityid, data.Description);
                }
            }




            var planningActivityStatusResource = _repositoryWrapper.PlanningActivityStatus.FindAll();
            dto.PlanningActivityStatusResource =
                planningActivityStatusResource.ToDictionary(x => x.Planningactivitystatusid,
                    x => x.Planningactivitystatus);

            var settingUpdatePAs_DS = _repositoryWrapper.SettingUpdatePlannedActivityLcmDeploymentStatusRepository
                .FindByCondition(p => p.Settingupdateplannedactivityid == id)
                .ToList();

            var lcmDeploymentStatusResource = _repositoryWrapper.LcmDeploymentStatusRepository.FindAll();
            dto.LcmDeploymentStatusIds = settingUpdatePAs_DS.Select(p => p.Lcmdeploymentstatusid).AsEnumerable();
            // dto.LcmDeploymentStatusResource = lcmDeploymentStatusResource.ToDictionary(x => x.Id, x => x.Description);

            var settingUpdateAssetsPAs_DS = _repositoryWrapper.SettingUpdatePlannedActivityAssetDeploymentStatusRepository
              .FindByCondition(p => p.Settingupdateplannedactivityid == id)
              .ToList();

            var assetDeploymentStatusResource = _repositoryWrapper.DeploymentStatus.FindAll();
            dto.AssetDeploymentStatusIds = settingUpdateAssetsPAs_DS.Select(p => p.Assetdeploymentstatusid).AsEnumerable();

            if (!dto.PlanningActivityStatusResource.ContainsKey(dto.PlanningActivityStatusId))
            {
                var data = _repositoryWrapper.PlanningActivityStatus.FindByCondition(
                    x => x.Planningactivitystatusid == dto.PlanningActivityStatusId,
                    includeDeleted: ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.PlanningActivityStatusResource.Add(data.Planningactivitystatusid, data.Planningactivitystatus);
                }
            }

            var planningActivityResource = _repositoryWrapper.PlannedActivityResourceRepository
                .FindByCondition(x => x.Foraddasset == ConstantValueFilter.isTrue || x.Foreditasset == ConstantValueFilter.isTrue || x.Forlcm == ConstantValueFilter.isTrue);

            dto.PlanningActivityResource =
                planningActivityResource.ToDictionary(x => x.Plannedactivityresourceid,
                    x => x.Plannedactivityresource);
            if (dto.PlanningActivityResourceId != null && !dto.PlanningActivityResource.ContainsKey(dto.PlanningActivityResourceId.Value))
            {
                var data = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(
                    x => x.Plannedactivityresourceid == dto.PlanningActivityResourceId,
                    includeDeleted: ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.PlanningActivityResource.Add(data.Plannedactivityresourceid, data.Plannedactivityresource);
                }
            }

            dto.SuccessorPlannedActivityTypeResource = successorPlannedActivityTypeResource.ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource);
            if (dto.SuccessorPlannedActivityId != null && !dto.SuccessorPlannedActivityTypeResource.ContainsKey(dto.SuccessorPlannedActivityId.Value))
            {
                var data = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(
                    x => x.Plannedactivityresourceid == dto.SuccessorPlannedActivityId,
                    includeDeleted: ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.SuccessorPlannedActivityTypeResource.Add(data.Plannedactivityresourceid, data.Plannedactivityresource);
                }
            }

            var crossSettings = _repositoryWrapper.SettingsUpdatePlannedActivity.FindAll().Select(p => SettingsUpdatePlannedActivityMapper.Get(p)).ToList();
            dto.CrossSettingscResource = crossSettings.Where(x => x.SettingsUpdatePlannedActivityId != id && x.DeliveryStatusId != entity.DeliveryStatusId && x.PlannedActivityTypeFor == entity.PlannedActivityTypeFor && x.PlanningActivityResourceId == entity.PlanningActivityResourceId)
                .ToDictionary(x => x.SettingsUpdatePlannedActivityId, x => x.SettingsUpdatePlannedActivityDescription);
            if (dto.CrossSettingsOutIds != null && dto.CrossSettingsOutIds.Any())
            {
                foreach (var idsds in dto.CrossSettingsOutIds)
                {
                    if (!dto.CrossSettingscResource.ContainsKey(Convert.ToInt16(idsds.Value)))
                    {
                        var data = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(
                            x => x.Settingsupdateplnactid == Convert.ToInt16(idsds.Value), ConstantValueFilter.isTrue)
                            .Select(p => SettingsUpdatePlannedActivityMapper.Get(p)).SingleOrDefault();
                        if (data != null)
                        {
                            dto.CrossSettingscResource.Add(data.SettingsUpdatePlannedActivityId,
                                data.SettingsUpdatePlannedActivityDescription);
                        }
                    }
                }
            }
            #endregion

            return dto;
        }

        public async Task<ResultDto> ChangeGridOrderSettingsUpdatePlannedActivity(List<ChangeGridOrderDto> lista)
        {
            var allEntityExist = await _repositoryWrapper.SettingsUpdatePlannedActivity.FindAll().ToListAsync();
            foreach (var item in lista)
            {
                SettingsUpdatePlannedActivity resource = SettingsUpdatePlannedActivityMapper.Get(allEntityExist.Single(x => x.Settingsupdateplnactid == item.Id));
                resource.Order = item.Order;
                _repositoryWrapper.SettingsUpdatePlannedActivity.Update(SettingsUpdatePlannedActivityMapper.Set(resource));

            }
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, SettingsUpdatePlannedActivityQueryDto request)
        {
            request.PageSize = 0;
            request.Page = 1;
            var predicateResult = ApplyFilterForOracleModel(request);

            var query = await Task.Run(() => PrepareQuery(request, null, predicateResult));

            var data = (await GetFilterValueList(query, propertyName, propertyFilter)).Distinct().ToList();
            return data;
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<SettingsUpdatePlannedActivity> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "settingsUpdatePlannedActivityDescription" => string.IsNullOrEmpty(propertyFilter) ?
                        request.Select(x => new FilterValueDto(x.SettingsUpdatePlannedActivityDescription))
                        : request.Where(x =>
                           x.SettingsUpdatePlannedActivityDescription.Contains(propertyFilter))
                        .Select(x => new FilterValueDto(x.SettingsUpdatePlannedActivityDescription)),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                        : request
                            .Where(x =>
                                x.ModificationUserEntity.Email.Contains(
                                    propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

                "planningActivityStatus" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto
                  {
                      Text = p.PlanningActivityStatus.PlanningActivityStatusDescription,
                      Value = p.PlanningActivityStatusId.ToString()
                  }).Distinct().ToList()
                  : request
                  .Where(x => x.PlanningActivityStatusId.ToString().IsNullOrEmpty() && x.PlanningActivityStatus.PlanningActivityStatusDescription.Contains(
                      propertyFilter)).Select(p => new FilterValueDto
                      {
                          Text = p.PlanningActivityStatus.PlanningActivityStatusDescription,
                          Value = p.PlanningActivityStatusId.ToString()
                      }).Distinct().ToList(),

                "lcmDeploymentStatus" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(p => p.SettingUpdatePlannedActivityLcmDeploymentStatus != null && p.SettingUpdatePlannedActivityLcmDeploymentStatus.Count > 0).SelectMany(x => x.SettingUpdatePlannedActivityLcmDeploymentStatus).Select(p => new FilterValueDto(p.LcmDeploymentStatusId.ToString(), p.LcmDeploymentStatus.Description)).Distinct().ToList()
                    : request
                    .Where(x => x.SettingUpdatePlannedActivityLcmDeploymentStatus != null && x.SettingUpdatePlannedActivityLcmDeploymentStatus.Any(s => s.LcmDeploymentStatus.Description.ToUpper().Contains(propertyFilter.ToUpper())))
                    .SelectMany(x => x.SettingUpdatePlannedActivityLcmDeploymentStatus)
                    .Select(p => new FilterValueDto(p.LcmDeploymentStatusId.ToString(), p.LcmDeploymentStatus.Description)).Distinct()
                    .ToList(),

                "assetDeploymentStatus" => string.IsNullOrEmpty(propertyFilter)
                ? request.Where(p => p.SettingUpdatePlannedActivityAssetDeploymentStatus != null && p.SettingUpdatePlannedActivityAssetDeploymentStatus.Count > 0).SelectMany(x => x.SettingUpdatePlannedActivityAssetDeploymentStatus).Select(p => new FilterValueDto(p.AssetDeploymentStatusId.ToString(), p.AssetDeploymentStatus.DeploymentStatusDescription)).Distinct().ToList()
                : request
                .Where(x => x.SettingUpdatePlannedActivityAssetDeploymentStatus != null && x.SettingUpdatePlannedActivityAssetDeploymentStatus.Any(s => s.AssetDeploymentStatus.DeploymentStatusDescription.ToUpper().Contains(propertyFilter.ToUpper())))
                .SelectMany(x => x.SettingUpdatePlannedActivityAssetDeploymentStatus)
                .Select(p => new FilterValueDto(p.AssetDeploymentStatusId.ToString(), p.AssetDeploymentStatus.DeploymentStatusDescription)).Distinct()
                .ToList(),

                "planningActivityResource" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Where(x => x.PlanningActivityResourceId != null).Select(p => new FilterValueDto
                  {
                      Text = p.PlannedActivityResource != null ? p.PlannedActivityResource.PlannedActivityResourceDescription : "",
                      Value = p.PlanningActivityResourceId.ToString()
                  }).Distinct().ToList()
                  : request
                  .Where(x => x.PlanningActivityResourceId != null && x.PlanningActivityResourceId.ToString().IsNullOrEmpty() && x.PlannedActivityResource.PlannedActivityResourceDescription.Contains(
                      propertyFilter)).Select(p => new FilterValueDto
                      {
                          Text = p.PlannedActivityResource != null ? p.PlannedActivityResource.PlannedActivityResourceDescription : "",
                          Value = p.PlanningActivityResourceId.ToString()
                      }).Distinct().ToList(),


                "successorPlannedActivityTypeResource" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(p => new FilterValueDto
                        {
                            Text = p.SuccessorPlannedActivityTypeResource != null ? p.SuccessorPlannedActivityTypeResource.PlannedActivityResourceDescription : "",
                            Value = p.SuccessorPlannedActivityId.ToString()
                        }).Distinct().ToList()
                        : request
                        .Where(x => x.SuccessorPlannedActivityId.ToString().IsNullOrEmpty() && x.SuccessorPlannedActivityTypeResource.PlannedActivityResourceDescription.Contains(
                            propertyFilter)).Select(p => new FilterValueDto
                            {
                                Text = p.SuccessorPlannedActivityTypeResource != null ? p.SuccessorPlannedActivityTypeResource.PlannedActivityResourceDescription : "",
                                Value = p.SuccessorPlannedActivityId.ToString()
                            }).Distinct().ToList(),

                "budgetAvailability" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto
                  {
                      Text = p.BudgetAvailability.BudgetAvailabilityDescription,
                      Value = p.BudgetAvailabilityId.ToString()
                  }).Distinct().ToList()
                  : request
                  .Where(x => x.BudgetAvailabilityId.ToString().IsNullOrEmpty() && x.BudgetAvailability.BudgetAvailabilityDescription.Contains(
                      propertyFilter)).Select(p => new FilterValueDto
                      {
                          Text = p.BudgetAvailability.BudgetAvailabilityDescription,
                          Value = p.BudgetAvailabilityId.ToString()
                      }).Distinct().ToList(),
                "crossSetting" =>
                string.IsNullOrEmpty(propertyFilter) ?

                    await Task.Run(() => request.SelectMany(p => p.CrossSettingsIn)
                        .Where(x => !x.Deleted)
                        .Select(x => x.SettingsUpdatePlannedActivityOut.SettingsUpdatePlannedActivityDescription)
                        .Distinct()
                        .Select(desc => new FilterValueDto { Text = desc, Value = desc })
                        .ToList())
                :
                request
                        .Where(x => x.DeliveryStatusId.ToString().IsNullOrEmpty() && x.DeliveryStatus.DeliveryStatusDescription.Contains(propertyFilter))
                        .SelectMany(p => p.CrossSettingsIn)
                        .Where(x => !x.Deleted)
                        .Select(x => x.SettingsUpdatePlannedActivityOut.SettingsUpdatePlannedActivityDescription)
                        .Distinct()
                        .Select(desc => new FilterValueDto { Text = desc, Value = desc })
                        .ToList(),
                "deliveryStatus" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto
                  {
                      Text = p.DeliveryStatus.DeliveryStatusDescription,
                      Value = p.DeliveryStatusId.ToString()
                  }).Distinct().ToList()
                  : request
                  .Where(x => x.DeliveryStatusId.ToString().IsNullOrEmpty() && x.DeliveryStatus.DeliveryStatusDescription.Contains(
                      propertyFilter)).Select(p => new FilterValueDto
                      {
                          Text = p.DeliveryStatus.DeliveryStatusDescription,
                          Value = p.DeliveryStatusId.ToString()
                      }).Distinct().ToList(),


                "ruleElementCount" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto
                  {
                      Text = p.RuleElementCount == 1 ? ConstantValueFilter.paArchivePlannedActivityAndParentyEntry : p.RuleElementCount == 2 ? ConstantValueFilter.paArchivePlannedActivityOnly : ConstantValueFilter.paNoRule,
                      Value = p.RuleElementCount.ToString()
                  }).Distinct().ToList()
                  : request
                  .Where(x => x.Rule.ToString().IsNullOrEmpty()).Select(p => new FilterValueDto
                  {
                      Text = p.RuleElementCount == 1 ? ConstantValueFilter.paArchivePlannedActivityAndParentyEntry : p.RuleElementCount == 2 ? ConstantValueFilter.paArchivePlannedActivityOnly : ConstantValueFilter.paNoRule,
                      Value = p.RuleElementCount.ToString()
                  }).Distinct().ToList(),

                "ruleforSuccessorPlannedActivityCreation" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto
                  {
                      Text = p.RuleForSuccessorPlannedActivityCreation == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(),
                      Value = p.RuleForSuccessorPlannedActivityCreation == true ? p.RuleForSuccessorPlannedActivityCreation.ToString() : ConstantValueFilter.No.ToUpper()
                  }).Distinct().ToList()
                  : request
                  .Where(x => x.RuleForSuccessorPlannedActivityCreation.ToString().IsNullOrEmpty()).Select(p => new FilterValueDto
                  {
                      Text = p.RuleForSuccessorPlannedActivityCreation == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(),
                      Value = p.RuleForSuccessorPlannedActivityCreation == true ? p.RuleForSuccessorPlannedActivityCreation.ToString() : ConstantValueFilter.No.ToUpper()
                  }).Distinct().ToList(),


                "specifyDC" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    {
                        Text = p.SpecifyDC == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(),
                        Value = p.SpecifyDC.ToString()
                    }).Distinct().ToList()
                    : request
                        .Where(x => x.SpecifyDC.ToString().IsNullOrEmpty()).Select(p => new FilterValueDto
                        {
                            Text = p.SpecifyDC == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(),
                            Value = p.SpecifyDC.ToString()
                        }).Distinct().ToList(),
                "needPlannedAsset" => string.IsNullOrEmpty(propertyFilter)
                                  ? request.Select(p => new FilterValueDto
                                  {
                                      Text = p.NeedPlannedAsset == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(),
                                      Value = p.NeedPlannedAsset.ToString()
                                  }).Distinct().ToList()
                                  : request
                                      .Where(x => x.NeedPlannedAsset.ToString().IsNullOrEmpty()).Select(p => new FilterValueDto
                                      {
                                          Text = p.NeedPlannedAsset == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(),
                                          Value = p.NeedPlannedAsset.ToString()
                                      }).Distinct().ToList(),
                "order" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(x => new FilterValueDto(x.Order.ToString()))
                        : request.Where(x =>
                            x.Order.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Order.ToString())),

                "maxOrder" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(x => new FilterValueDto(x.MaxOrder.ToString()))
                        : request.Where(x =>
                            x.MaxOrder.ToString() == propertyFilter).Select(x => new FilterValueDto(x.MaxOrder.ToString())),
                "localApproval" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(x => new FilterValueDto(x.LocalApproval.ToString()))
                        : request.Where(x =>
                            x.LocalApproval.ToString() == propertyFilter).Select(x => new FilterValueDto(x.LocalApproval.ToString())),
                //ruleLinkedDcPlannedActivityTypesDescription
                "plannedActivityTypeDescription" => string.IsNullOrEmpty(propertyFilter)
            ? request.Select(x => new FilterValueDto(x.PlannedActivityResource.RuleLinkedDcNavigation.PlannedActivityTypeDescription.ToString()))
            : request.Where(x =>
                x.PlannedActivityResource.RuleLinkedDcNavigation.PlannedActivityTypeDescription
                .ToString() == propertyFilter).
                Select(x => new FilterValueDto(x.PlannedActivityResource.RuleLinkedDcNavigation.PlannedActivityTypeDescription.ToString())),


                "plannedActivityTypeFor" => string.IsNullOrEmpty(propertyFilter)
                     ? request.Where(x => x.PlannedActivityTypeFor != null).Select(p => new FilterValueDto
                     {
                         Text = p.toPlannedActivityTypeIsFor(),
                         Value = p.PlannedActivityTypeFor.ToString()
                     }).Distinct().ToList()
                     : request
                     .Where(x => x.PlannedActivityTypeFor != null && x.DeliveryStatusId.ToString().IsNullOrEmpty() && x.toPlannedActivityTypeIsFor().Contains(
                         propertyFilter)).Select(p => new FilterValueDto
                         {
                             Text = p.toPlannedActivityTypeIsFor(),
                             Value = p.PlannedActivityTypeFor.ToString()
                         }).Distinct().ToList(),

                "rule" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    {
                        Text = CreateRuleToText(p.Rule),
                        Value = p.Rule.ToString()
                    }).Distinct().ToList()
                    : request
                        .Where(x => x.Rule.ToString().IsNullOrEmpty()).Select(p => new FilterValueDto
                        {
                            Text = CreateRuleToText(p.Rule),
                            Value = p.Rule.ToString()
                        }).Distinct().ToList(),

                "isRollback" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(p => new FilterValueDto
                        {
                            Text = p.IsRollback == true ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                            Value = p.IsRollback.ToString()
                        }).Distinct().ToList()
                        : request
                            .Where(x => x.IsRollback.ToString().IsNullOrEmpty()).Select(p => new FilterValueDto
                            {
                                Text = p.IsRollback == true ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                Value = p.IsRollback.ToString()
                            }).Distinct().ToList(),
                "msStatus" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    {
                        Text = MSStatusToText(p.MSStatus),
                        Value = p.MSStatus.ToString()
                    }).Distinct().ToList()
                    : request
                        .Where(x => x.Rule.ToString().IsNullOrEmpty()).Select(p => new FilterValueDto
                        {
                            Text = MSStatusToText(p.MSStatus),
                            Value = p.MSStatus.ToString()
                        }).Distinct().ToList(),

                "msStatusDuration" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(p => new FilterValueDto
                        {
                            Text = p.MSStatusDuration.ToString(),
                            Value = p.MSStatusDuration.ToString()
                        }).Distinct().ToList()
                        : request
                            .Where(x => x.MSStatusDuration.ToString().IsNullOrEmpty()).Select(p => new FilterValueDto
                            {
                                Text = p.MSStatusDuration.ToString(),
                                Value = p.MSStatusDuration.ToString()
                            }).Distinct().ToList(),

                "isMileStone" => string.IsNullOrEmpty(propertyFilter)
                        ? request.Select(p => new FilterValueDto
                        {
                            Text = p.IsMileStone == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                            Value = p.IsMileStone.ToString()
                        }).Distinct().ToList()
                        : request
                            .Where(x => x.IsMileStone.ToString().IsNullOrEmpty()).Select(p => new FilterValueDto
                            {
                                Text = p.IsMileStone == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                                Value = p.IsMileStone.ToString()
                            }).Distinct().ToList(),

                _ => new List<FilterValueDto>(),
            };

        }

        /// <summary>
        /// Get Planned activty types and deployment status based on select PA type for 
        /// </summary>
        /// <param name="plannedActivityTypeFor"></param>
        /// <returns></returns>
        public async Task<ResultDto> GetPATypeAndDeploymentStatus(short plannedActivityTypeFor)
        {
            var paTypes = new Dictionary<short, string>();

            var deploymentStatus = new Dictionary<short, string>();
            var assetDeploymentStatus = new Dictionary<short, string>();

            if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.LcmEngineering)
            {
                paTypes = await Task.Run(() => _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Forlcm == ConstantValueFilter.isTrue).ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource));

            }
            else if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.AddAsset)
            {
                paTypes = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Foraddasset == ConstantValueFilter.isTrue).ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource);

            }
            else if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.EditAsset)
            {
                paTypes = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Foreditasset == ConstantValueFilter.isTrue).ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource);

            }
            else if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.DesignAspect)
            {
                paTypes = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Fordesignaspect == ConstantValueFilter.isTrue).ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource);
            }
            else if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.ServicePlan)
            {
                paTypes = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Forserviceplan == ConstantValueFilter.isTrue).ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource);
            }

            if (plannedActivityTypeFor != (short)PlannedActivityTypeForEnum.DesignAspect)
            {
                deploymentStatus = _repositoryWrapper.LcmDeploymentStatusRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
                assetDeploymentStatus = _repositoryWrapper.DeploymentStatus.FindAll().ToDictionary(x => x.Deploymentstatusid, x => x.Deploymentstatus);
            }

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = new { plannedActivityTypes = paTypes, deploymentStatus = deploymentStatus, assetDeploymentStatus = assetDeploymentStatus }

            };
        }

        private static string CreateRuleToText(int? rule)
        {
            switch (rule)
            {
                case 0:
                    return "Do not Create LCM";
                case 1:
                    return "Create LCM";
                case 2:
                    return "Create And Do Not Copy Operational Data";
                default:
                    return "Unknown Rule";
            }
        }
        private static string MSStatusToText(int? status)
        {
            switch (status)
            {
                case 1:
                    return "MS1";
                case 2:
                    return "MS2";
                case 3:
                    return "MS3";
                case 4:
                    return "MS4";
                default:
                    return string.Empty;
            }
        }

    }
}
