using CAM.DataTransferObjects.Entita.ClusterLevelPA;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.LookUp.PlannedActivityResourceDto;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public class PlannedActivityDtoCreate : PlannedActivityDto
    {
        public short? DeliveryStatusId { get; set; }
        public string DeliveryStatusName { get; set; }
        public short ActivityStatusId { get; set; }
        public string ActivityStatusName { get; set; }
        public short? PlanningActivityStatusId { get; set; }
        public short? ResponsibilityPhaseId { get; set; }
        public short? PlannedActivityResourceId { get; set; }
        public short? RelatesToId { get; set; }
        public long? DesignComponentId { get; set; }
        public short? BudgetAvailabilityId { get; set; }
        public short? RiskEngId { get; set; }
        public short? OpCoId { get; set; }
        public short? BenefitId { get; set; }
        public short? DriverId { get; set; }
        public short? PlanningRiskId { get; set; }
        public short? RiskOpeId { get; set; }
        public long? LinkedToPlannedActivityId { get; set; }

        #region Resource
        public IDictionary<short, PlannedActivityResourceDto> PlannedActivityResource { get; set; }
        public IDictionary<short, string> DeliveryStatusResource { get; set; }
        public IDictionary<short, string> ActivityStatusResource { get; set; }
        public IDictionary<short, string> PlanningActivityStatusResource { get; set; }
        public IDictionary<short, string> ResponsibilityPhaseResource { get; set; }
       
        public IDictionary<long, bool> DesignComponentIsVirtualizedResource { get; set; }
        public IDictionary<short, string> BudgetAvaibilityResource { get; set; }
        public IDictionary<short, string>RiskResource { get; set; }
        public IDictionary<short, string> OpCoResource { get; set; }
        public IDictionary<short, string> DriverResource { get; set; }
        public IDictionary<short, string> PlanningRiskResource { get; set; }
        public IDictionary<short, string> BenefitResource { get; set; }

        ///Ticket 603 - #503 :  Analysis - Software Upgrade Utility
        public IDictionary<long, string> TransientDesignComponentResource { get; set; }
        #endregion

        public List<KeyValuePair<long, string>> DesignComponentResource { get; set; }
        
        /// <summary>
        /// Ticket 1025 - Enhancements On Modernizes Workflow - Type2
        /// </summary>
        public bool settingUpdatePaNeedPlannedAsset { get; set; }
        #region SystemOfsystem
        public List<ViewBagandComponenetDto> BuildBagResources { get; set; }
       
        #endregion

       
        public IDictionary<short, string> PlannedActivityCategoryResource { get; set; }
        public IDictionary<short, string> LcmCategoryResource { get; set; }
        public IDictionary<string, string> PriorityResource { get; set; }
        public IDictionary<long, string> ProgramResource { get; set; }
        public List<long?> DesignComponentFamilyIdList { get; set; }
        public List<KeyValuePair<long, string>> DesignComponentFamilyResource { get; set; }

        public List<KeyValuePair<long, string>> PlannedDcfResource { get; set; }
        #region ClusterLevel PA
        public InfraClusterClusterUpgradeUpsertDto infraClusterClusterUpgradeUpsertDto { get; set; }

        #endregion


    }


}