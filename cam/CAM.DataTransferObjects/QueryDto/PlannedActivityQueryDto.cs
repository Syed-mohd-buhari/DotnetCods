using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
using CAM.DataTransferObjects.FunctionalityDto;
using System.Security.Policy;

namespace CAM.DataTransferObjects.QueryDto
{
    public class PlannedActivityQueryDto : QueryObject
    {
        public List<long> PlannedActivityId { get; set; }
        public List<short> PlannedImplementationYear { get; set; }
        public List<short> ActivityStatusId { get; set; }
        public List<short> PlanningActivityStatusId { get; set; }
        public List<short> PlannedActivityResourceId { get; set; }
        public List<long> PlannedDesignComponent { get; set; }
        public List<long> plannedDesignComponentIndex { get; set; }
        public List<long> originalDesignComponentIndex { get; set; }
        public List<long> designComponentFamilyIndex { get; set; }
        
        public List<long> OriginalDesignComponent { get; set; }
        public List<short> Driver { get; set; }
        public List<short> Benefits { get; set; }
        public List<string> PlannedActivityDescription { get; set; }
      
        public List<string> BudgetAvailability { get; set; }
        public List<string> DeliveryProjectName { get; set; }
        public List<string> LocalApproval { get; set; }
        public List<short> DeliveryStatusId { get; set; }
        public List<short> ResponsibilityPhaseId { get; set; }
        public DateFilter PlannedCompletionValue { get; set; }
        public DateFilter StartDateValue { get; set; }

        public List<string> Notes { get; set; }
        public List<long> LcmEngineeringId { get; set; }
        public List<string> RiskEngineeringEvaluation { get; set; }
        public List<string> RiskEngineeringNotes { get; set; }
        public List<string> RiskOperationalEvaluation { get; set; }
        public List<string> RiskOperationalNotes { get; set; }
        public List<short> RelatesToId { get; set; }
        public List<string> BudgetValueGrid { get; set; }
        public List<short> PlanningRisk { get; set; }
        public List<string> DeliveryProjectId { get; set; }
         
        public List<string> BudgetTrackingId { get; set; }
        public List<string> ProjectStatus { get; set; }
        public List<short> OpCo { get; set; }
        public List<string> LastModifiedBy { get; set; }       
        public List<bool>  IsNewServiceArchitecture { get; set; }
        public List<bool> IsReplacementExistingSolution { get; set; }
        public List<int> DesignComponentFamilyName { get; set; }
        public DateFilter LastModifiedValue { get; set; }
        public bool Archived { get; set; }
        public List<string> Program { get; set; }
        public List<string> ProjectOwner { get; set; }
        public List<int> DeliveryTrackingId { get; set; }

        public List<bool> DeliveryPlanAvailable { get; set; }

        public List<string> VerticalName { get; set; }

        public List<int> verticalNameId { get; set; }

        public List<bool?> ForAddAsset { get; set; }
        public List<bool?> ForEditAsset { get; set; }
        public bool? ForNetwork { get; set; }
        public bool? ForLcm { get; set; }


        public bool? ForDesignAspect { get; set; }
        #region Ticket 685 Dev - #674 SettingsUpdatePlanedActivity - Display  Planned Activity Associated/Linked Table Details  - Ex : LCM, DA, Assets
        public List<bool?> ForLcmLink { get; set; }
        

        public List<bool?> ForDesignAspectLink { get; set; }
        public List<bool?> ForServicePlanLink { get; set; }
        #endregion

        ///Ticket 729 Filter Both For Edit And Add Asset Records in PA Grid while click Asset button in Manage Network Menu
        public bool? AddEditAssetFilter { get; set; }

        public List<bool> IsPAReleaseDetailUnknown { get; set; }
        ///Ticket 751 PPM Import

        public List<string> DeliveryProjectPpmId { get; set; }

        public List<long> PlannedBuildBagDescription { get; set; }
        public List<long> CurrentBuildBagDescription { get; set; }
        public List<string> PlannedActivityteam { get; set; }
        public List<string> Priority { get; set; }
        public List<string> PlannedActivityCategory { get; set; }
        public List<short> LcmCategories { get; set; }
        public List<long> ProgramId { get; set; }
        public List<string> ProjectDescription { get; set; }
        public DateFilter PreBaseLineDateValue { get; set; }
        public List<string> ActivityDetails { get; set; }
        public List<bool> IsServicePlan { get; set; }
        public List<int> ServiceName { get; set; }
        public List<string> ProgramName { get; set; }
        public List<int> ServicePlanId { get; set; }
        public List<long> PlannedActivity {  get; set; }
    }
}