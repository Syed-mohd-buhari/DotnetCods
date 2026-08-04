using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.Entities.Mappers.Setting
{
    public static class SettingsUpdatePlannedActivityMapper
    {
        public static SettingsUpdatePlannedActivity Get(Settingsupdateplannedactivity model)
        {
            if (model == null)
                return null;
            var result = new SettingsUpdatePlannedActivity()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                BudgetAvailabilityId = model.Budgetavailabilityid,
                DeliveryStatusId = model.Deliverystatusid,
                LastModified = model.Lastmodified,
                LastModifiedBy = model.Lastmodifiedby,
                LocalApproval = model.Localapproval,
                MaxOrder = model.Maxorder,
                Order = model.Order,
                PlanningActivityStatusId = model.Planningactivitystatusid,
                PlanningActivityResourceId = model.Plannedactivityresourceid != null ? model.Plannedactivityresourceid.Value : (short?)null ,
                SuccessorPlannedActivityId = model.Successorplannedactivityresourceid != null ? model.Successorplannedactivityresourceid.Value :(short?)null ,
                RuleForSuccessorPlannedActivityCreation = model.Ruleforsuccessorplannedactivitycreation,
                Rule = model.Rule,
                SpecifyDC = model.Specifydc ?? false,
                SettingsUpdatePlannedActivityDescription = model.Settingsupdateplnactdes,
                SettingsUpdatePlannedActivityId = model.Settingsupdateplnactid,
                BudgetAvailability = BudgetAvailabilityMapper.GetBudgetAvailabilityMapper(model.Budgetavailability),
                DeliveryStatus = DeliveryStatusMapper.GetDeliveryStatusMapper(model.Deliverystatus),
                PlanningActivityStatus = PlanningActivityStatusMapper.GetPlanningActivityStatusMapper(model.Planningactivitystatus),
                PlannedActivityResource = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(model.Plannedactivityresource),
                SuccessorPlannedActivityTypeResource = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(model.Successorplannedactivityresource),
                RuleElementCount = model.Ruleelementcount,
                PlannedActivityTypeFor = model.Plannedactivitytypefor,
                NeedPlannedAsset = model.Needplannedasset,
                IsRollback = model.Isrollback,
                MSStatus = model.Milestonestatus,
                MSStatusDuration = model.Milestonestatusduration,
                IsMileStone = model.Ismilestone


            };
            result.SettingUpdatePlannedActivityLcmDeploymentStatus = model.Settingupdateplannedactivitylcmdeploymentstatus.Select(p => SettingUpdatePlannedActivitiesLcmDeploymentStatusMapper.Get(p)).ToList();
            result.SettingUpdatePlannedActivityAssetDeploymentStatus = model.Settingupdateplannedactivityassetdeploymentstatus.Select(p => SettingUpdatePlannedActivitiesAssetDeploymentStatusMapper.Get(p)).ToList();

            return result;
        }

        public static Settingsupdateplannedactivity Set(SettingsUpdatePlannedActivity model)
        {
            return new Settingsupdateplannedactivity()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Budgetavailabilityid = model.BudgetAvailabilityId,
                Deliverystatusid = model.DeliveryStatusId,
                Lastmodified = model.LastModified,
                Lastmodifiedby = model.LastModifiedBy,
                Localapproval = model.LocalApproval,
                Maxorder = model.MaxOrder,
                Order = model.Order,
                Planningactivitystatusid = model.PlanningActivityStatusId,
                Plannedactivityresourceid = model.PlanningActivityResourceId,
                Successorplannedactivityresourceid = model.SuccessorPlannedActivityId,
                Ruleforsuccessorplannedactivitycreation = model.RuleForSuccessorPlannedActivityCreation,
                Rule = model.Rule,
                Specifydc = model.SpecifyDC,
                Settingsupdateplnactdes = model.SettingsUpdatePlannedActivityDescription,
                Settingsupdateplnactid = model.SettingsUpdatePlannedActivityId,
                Ruleelementcount = model.RuleElementCount,
                Plannedactivitytypefor = model.PlannedActivityTypeFor,
                Needplannedasset = model.NeedPlannedAsset,
                Isrollback = model.IsRollback,
                Milestonestatus = model.MSStatus,
                Milestonestatusduration = model.MSStatusDuration,
                Ismilestone = model.IsMileStone
                
            };
        }
    }
}
