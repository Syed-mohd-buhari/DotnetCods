using CAM.DataTransferObjects.Entita.ClusterLevelPA;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.ServicePlan;
using CAM.DataTransferObjects.LookUp.DaPlannedActivityDcf;
using CAM.DataTransferObjects.Settings;
using CAM.DataTransferObjects.Settings.SettingsUpdatePlannedActivity;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
  public  class UpdatePlannedActivityStatusDto
    {
        public short OpCoId { get; set; }
        public IDictionary<short, string> OpCoResource { get; set; }
        public long? DesignComponentId { get; set; }
        public IDictionary<long, string>? DesignComponentResource { get; set; }
        public long? DesignComponentFamilyId { get; set; }
        public IDictionary<long?, string>? DesignComponentFamilyResource { get; set; }
        public long? PlanningActivityDetailsResourceId { get; set; }
        public IDictionary<long, string>? PlanningActivityDetailsResource { get; set; }
        public short? PlannedActivityTypeId { get; set; }
        public IDictionary<short, string>? PlannedActivityTypeResource { get; set; }

        public short? SettingsUpdatePlannedActivityId { get; set; }
        //public long? PlanningActivityDetailsResourceId { get; set; }
       // public IDictionary<long, string> PlanningActivityDetailsResource { get; set; }
        public IDictionary<short, SettingsUpdatePlannedActivityDtoUpdate> SettingsUpdatePlannedActivityResource { get; set; }

        public bool InEngineeringPhase { get; set; }
        public bool ElementCount { get; set; }
        public List<NetworkElementAssociated> StartNodesInProd { get; set; }
        public List<NetworkElementAssociated> StartNodesInLab { get; set; }
        public List<NetworkElementAssociated> EndNodesInProd { get; set; }
        public List<NetworkElementAssociated> EndNodesInLab { get; set; }
        public int RuleElementCount { get; set; }

        public int NumberOfNodesInput { get; set; }
        public int NumberOfNodesOutput { get; set; }
        public int NumberOfNodesInLabInput { get; set; }
        public int NumberOfNodesInLabOutput { get; set; }
        public bool IsCrossSettings { get; set; }
        public IDictionary<short, CrossSettingsDto>? CrossSettingscResource { get; set; }

        public short? PlannedActivityTypeFor { get; set; }

        public DateTime? AssetLiveStatusDate { get; set; }
        public DateTime? AssetDecommissionedDate { get; set; }
        public long RuleLinkedDc { get; set; }

        public List<DaPlannedActivtyDcfDto> DaPlannedActivtyDcfDto { get; set; }
        public List<ServicePlanGridDto> ServicePlanDcfDto { get; set; }

        public InfraClusterClusterUpgradeUpsertDto infraClusterClusterUpgradeUpsertDto { get; set; }
        public List<ClusterUpgradeAddUpdateDto> clusterUpgradeAddUpdateDto { get; set; }
        public long? PlannedHardwareTypeId { get; set; }
        public IDictionary<long, string>? PlannedHardwareTypeResource { get; set; }
    }
}
