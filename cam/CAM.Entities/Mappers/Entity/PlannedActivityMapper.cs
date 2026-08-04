using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public class PlannedActivityMapper
    {
        public static PlannedActivity Get(Plannedactivities model , bool Include = true)
        {
            if (model == null)
                return null;
            PlannedActivity result= new PlannedActivity()
            {
                PlannedActivityId = model.Plannedactivityid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = Include == true ? ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation) : null,
                ModificationUserEntity = Include == true ? ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation) : null,

                ActivityDetails = model.Activitydetails,
                ActivityStatusId = model.Activitystatusid,

                BudgetAvailabilityId = model.Budgetavailabilityid,
                BudgetTrackingId = model.Budgettrackingid,
                BudgetValue = model.Budgetvalue,
                Currency = model.Currency,
                DeliveryProjectName = model.Deliveryprojectname,
                DeliveryStatusId = model.Deliverystatusid,

                EngineeringRiskId = model.Engineeringriskid,
                IsNewServiceArchitecture = model.Isnewservicearchitecture,
                IsReplacementExistingSolution = model.Isreplacementexistingsolution,
                LcmEngineeringId = model.Lcmengineeringid,
                LinkedToPlannedActivityId = model.Linkedtoplannedactivityid,
                LocalApproval = model.Localapproval,
                NetworkElementAsPlannedId = model.Networkelementasplannedid,
                Notes = model.Notes,
                OpCoId = model.Opcoid,
                OperationalRiskId = model.Operationalriskid,
                OverallRiskEvaluation = model.Overallriskevaluation,
                DeliveryProjectId = model.Deliveryprojectid,
                PlannedActivityDescription = model.Plannedactivity,
                PlannedActivityResourceId = model.Plannedactivityresourceid,
                PlannedCompletion = model.Plannedcompletion,
                PlannedImplementationYear = model.Plannedimplementationyear,
                PlanningActivityStatusId = model.Planningactivitystatusid,

                ProjectStatus = model.Projectstatus,
                RelatesToId = model.Relatestoid,
                ResponsibilityPhaseId = model.Responsibilityphaseid,
                RiskEngineeringNotes = model.Riskengineeringnotes,
                RiskOperationalNotes = model.Riskoperationalnotes,
                SpareFieldsJson = model.Sparefieldsjson,
                OpCo = OpCoMapper.GetOpCoMapper(model.Opco),

                ActivityStatus = ActivityStatusMapper.GetActivityStatusMapper(model.Activitystatus),
                BudgetAvailability = BudgetAvailabilityMapper.GetBudgetAvailabilityMapper(model.Budgetavailability),
                DeliveryStatus = DeliveryStatusMapper.GetDeliveryStatusMapper(model.Deliverystatus),
                DesignComponent = Include ? DesignComponentMapper.GetDesignComponentMapper(model.Designcomponent) : null,
                EngineeringRisk = RiskMapper.GetRiskMapper(model.Engineeringrisk),
                LcmEngineering = LCMEngineeringMapper.GetLcmEngineeringMapper(model.Lcmengineering),
                NetworkElementAsPlanned = NetworkElementAsPlannedMapper.Get(model.Networkelementasplanned),
                OperationalRisk = RiskMapper.GetRiskMapper(model.Operationalrisk),
                PlannedActivityResource = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(model.Plannedactivityresource),
                PlanningActivityStatus = PlanningActivityStatusMapper.GetPlanningActivityStatusMapper(model.Planningactivitystatus),
                ResponsibilityPhase = ResponsibilityPhaseMapper.GetResponsibilityPhaseMapper(model.Responsibilityphase),

                BenefitId = model.Benefitid,
                DriverId = model.Driverid,
                PlanningRiskId = model.Planningriskid,
                Benefit = BenefitMapper.GetBenefitMapper(model.Benefit),
                Driver = DriverMapper.GetDriverMapper(model.Driver),
                PlanningRisk = PlanningRiskMapper.GetPlanningRiskMapper(model.PlanningriskNavigation),
                DesignAspectId = model.Designaspectid,
                DesignAspect = DesignAspectMapper.Get(model.Designaspect, false),
                DesignComponentFamilyId = model.Designcomponentfamilyid,
                PlannedDesignComponentFamilyId = model.Designcomponentfamilyid,
                OriginalDesignComponentIndex = model.Networkelementasplanned != null ? model.Networkelementasplanned.Designcomponentid : (long?)null,
                DesignComponentId = model.Designcomponentid,
                DesignComponentFamily = DesignComponentFamilyMapper.Get(model.Designcomponentfamily, false),
                StartDate = model.Startdate,
                ForAddAsset = model.Foraddasset,
                ForEditAsset = model.Foreditasset,
                OriginalLcmEngineeringId = model.Originallcmengineeringid,
                Originallcmengineering = LCMEngineeringMapper.GetLcmEngineeringMapper(model.Originallcmengineering),
                DeliveryPlanAvailable = model.Deliveryplanavailable,
                ProjectOwner = model.Projectowner,
                DeliveryTrackingId = model.Deliverytrackings.FirstOrDefault() != null ? model.Deliverytrackings.FirstOrDefault().Id : null,
                Archived = model.Archived,
                IsPAReleaseDetailUnknown = model.Ispareleasedetailunknown,
                Buildbag = Include ? BuildBagMapper.GetBuildBag(model.Buildbag) : null,
                Buildbagid = model.Buildbagid,
                Plannedactivityteam = model.Plannedactivityteam,
                //Plannedactivitycategory = model.Plannedactivitycategory,
                Plannedactivitycategoryid = model.Plannedactivitycategoryid,
                Priority = model.Priority,
                Lcmcategories = model.Lcmcategories,
                ProgramId = model.Programid,
                ProjectDescription = model.Projectdescription,
                ProgramNavigation = ProgramMapper.GetProgramMapper(model.ProgramNavigation),
                PlannedactivitycategoryNavigation = Include ? PlannedActivityCategoryMapper.Get(model.Plannedactivitycategory, false) : null,
                PreBaseLineDate = model.Prebaselinedate,

                DesigncComponentFamilyId = model.Designcomponentfamilyid != null && model.Designcomponentfamilyid == 0 ? 
                model.Designcomponentfamilyid : null,
                Serviceplanid = model.Serviceplanid,
                Serviceplan = model?.Serviceplan != null && Include == true ? ServicePlanMapper.Get(model.Serviceplan,false) : null,
                Isserviceplan = model.Isserviceplan,
            };
            if(result.DesignAspectId!=null)
            {
                result.DesignContactDto = model?.Designaspect != null ?
                                          model?.Designaspect?.Designcomponentfamily?.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                                         .Where(x => x.Deleted == false)
                                         .Select(m => (int?)m.Designcontactid))?.
                                          Distinct().ToList() : null;

                var majorHardwareDesignContact = model?.Designaspect != null ? 
                                          model?.Designaspect ?.Designcomponentfamily?.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                          .SelectMany(m => m.Majorhardware?.Majorhwbuildsdesigncontacts.
                                          Where(x => x.Deleted == false)
                                          .Select(n => (int?)n.Designcontactid))?.
                                          Distinct().ToList())?.ToList():null;

                if (result.DesignContactDto?.Any() == true && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = result.DesignContactDto.Union(majorHardwareDesignContact).ToList();
                else if (result.DesignContactDto?.Any() == false && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = majorHardwareDesignContact;
            }
            if (result.Serviceplanid != null)
            {
                result.DesignContactDto = model?.Serviceplan != null ?
                                          model?.Serviceplan?.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                                         .Where(x => x.Deleted == false)
                                         .Select(m => (int?)m.Designcontactid)))?.
                                          Distinct().ToList() : null;
                var majorHardwareDesignContact = model?.Serviceplan != null ?
                                          model?.Serviceplan?.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                          .SelectMany(m => m.Majorhardware?.Majorhwbuildsdesigncontacts.
                                          Where(x => x.Deleted == false)
                                          .Select(n => (int?)n.Designcontactid))?.
                                          Distinct().ToList()))?.ToList() : null;
                if (result.DesignContactDto?.Any() == true && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = result.DesignContactDto.Union(majorHardwareDesignContact).ToList();
                else if (result.DesignContactDto?.Any() == false && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = majorHardwareDesignContact;
            }

            return result;
        }

        public static PlannedActivity GetPaForReports(Plannedactivities model, bool Include = true)
        {
            if (model == null)
                return null;
            PlannedActivity result = new PlannedActivity()
            {
                PlannedActivityId = model.Plannedactivityid,  
                DesignComponentFamilyId = model.Designcomponentfamilyid,
                PlannedDesignComponentFamilyId = model.Designcomponentfamilyid,
                DesignComponentFamily = new DesignComponentFamily { DesignComponentFamilyId = (long)model?.Designcomponentfamilyid },
                StartDate = model.Startdate,
                DesigncComponentFamilyId = model.Designcomponentfamilyid != null && model.Designcomponentfamilyid == 0 ?
                model.Designcomponentfamilyid : null,
                PlannedCompletion = model.Plannedcompletion
               
            };

            

            return result;
        }
        public static Plannedactivities Set(PlannedActivity model)
        {
            return new Plannedactivities()
            {
                Plannedactivityid = model.PlannedActivityId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Activitydetails = model.ActivityDetails,
                Activitystatusid = model.ActivityStatusId,               
                Budgetavailabilityid = model.BudgetAvailabilityId,
                Budgettrackingid = model.BudgetTrackingId,
                Budgetvalue = model.BudgetValue,
                Currency = model.Currency,
                Deliveryprojectname = model.DeliveryProjectName,
                Deliverystatusid = model.DeliveryStatusId,
                Designcomponentid = model.DesignComponentId,
                Engineeringriskid = model.EngineeringRiskId,
                Isnewservicearchitecture = model.IsNewServiceArchitecture,
                Isreplacementexistingsolution = model.IsReplacementExistingSolution,
                Lcmengineeringid = model.LcmEngineeringId,
                Linkedtoplannedactivityid = model.LinkedToPlannedActivityId,
                Localapproval = model.LocalApproval,
                Networkelementasplannedid = model.NetworkElementAsPlannedId,
                Notes = model.Notes,
                Opcoid = model.OpCoId,
                Operationalriskid = model.OperationalRiskId,
                Overallriskevaluation = model.OverallRiskEvaluation,
                Deliveryprojectid = model.DeliveryProjectId,
                Plannedactivity = model.PlannedActivityDescription,
                Plannedactivityresourceid = model.PlannedActivityResourceId,
                Plannedcompletion = model.PlannedCompletion,
                Plannedimplementationyear = model.PlannedImplementationYear,
                Planningactivitystatusid = model.PlanningActivityStatusId,
                Projectstatus = model.ProjectStatus,
                Relatestoid = model.RelatesToId,
                Responsibilityphaseid = model.ResponsibilityPhaseId,
                Riskengineeringnotes = model.RiskEngineeringNotes,
                Riskoperationalnotes = model.RiskOperationalNotes,
                Sparefieldsjson = model.SpareFieldsJson,

                Benefitid = model.BenefitId,
                Driverid = model.DriverId,
                Planningriskid = model.PlanningRiskId,
                Designaspectid = model.DesignAspectId,
               // Designcomponentfamilyid =model.DesignComponentFamilyId,
                Designcomponentfamilyid = model.PlannedDesignComponentFamilyId,
                Startdate = model.StartDate,

                Foraddasset = model.ForAddAsset,
                Foreditasset = model.ForEditAsset,
                Originallcmengineeringid = model.OriginalLcmEngineeringId,
                Deliveryplanavailable = model.DeliveryPlanAvailable,
                Projectowner = model.ProjectOwner,
                Archived = model.Archived,
                Ispareleasedetailunknown = model.IsPAReleaseDetailUnknown,
                Buildbagid =  model.Buildbagid,
                Plannedactivityteam = model.Plannedactivityteam,
                //Plannedactivitycategory = model.Plannedactivitycategory,
                Plannedactivitycategoryid = model.Plannedactivitycategoryid == 0 ? null : model.Plannedactivitycategoryid,
                Priority = model.Priority,
                Lcmcategories = model.Lcmcategories,
                Programid = model.ProgramId == 0 ? null : model.ProgramId,
                Projectdescription = model.ProjectDescription,
                Prebaselinedate = model.PreBaseLineDate,
                Isserviceplan = model.Isserviceplan,
                Serviceplanid = model.Serviceplanid,
            };
        }

        public static PlannedActivity GetForBpt(Plannedactivities model, bool Include = true)
        {
            if (model == null)
                return null;
            return new PlannedActivity()
            {
                PlannedActivityId = model.Plannedactivityid, 
                OpCo = OpCoMapper.GetOpCoMapperForBPT(model.Opco),                 
                DesignComponent = Include ? DesignComponentMapper.GetDesignComponentMapper(model.Designcomponent) : null,               
                PlannedActivityResource = PlannedActivityResourceMapper.GetPlannedActivityResourceMapperForBPT(model.Plannedactivityresource),
                DesignAspectId = model.Designaspectid,
                DesignAspect = DesignAspectMapper.Get(model.Designaspect)
               
            };
        }
    }
}
