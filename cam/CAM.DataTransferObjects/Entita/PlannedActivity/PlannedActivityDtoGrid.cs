using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.Entita.ServicePlan;
using CAM.Entities.Models;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public class PlannedActivityDtoGrid : PlannedActivityDto
    {
        [OrderGrid(Order = 1)]
        [DisplayName("OpCo")]
        [Default]
        public string OpCo { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Current Design Component")]
        [Default]
        public string OriginalDesignComponent { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Planned Design Component")]
        [Default]
        public string PlannedDesignComponent { get; set; }



        [OrderGrid(Order = 7)]
        [DisplayName("Planned Activity Index")]
        public long PlannedActivityId { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Design Component Family Index")]
        public long? DesignComponentFamilyIndex { get; set; }

        [OrderGrid(Order = 9)]
        [DisplayName("Original Design Component Index")]
        public long? OriginalDesignComponentIndex { get; set; }

        [OrderGrid(Order = 10)]
        [DisplayName("Planned Design Component Index")]
        public long? PlannedDesignComponentIndex { get; set; }


        [OrderGrid(Order = 24)]
        [DisplayName("Delivery Status")]
        [Default]
        public string DeliveryStatusId { get; set; }


        [OrderGrid(Order = 14)]
        [DisplayName("Activity Status")]
        [Default]
        public string ActivityStatusId { get; set; }

        [OrderGrid(Order = 16)]
        [DisplayName("Planning Status")]
        public string PlanningActivityStatusId { get; set; }

        [OrderGrid(Order = 23)]
        [DisplayName("Responsibility Phase")]
        [Default]
        public string ResponsibilityPhaseId { get; set; }

        [IgnoreGrid]
        [DisplayName("Relates To")]
        public string RelatesToId { get; set; }


        [IgnoreGrid]
        public string LcmEngineering { get; set; }

        [OrderGrid(Order = 15)]
        [DisplayName("Planned Activity")]
        [Default]
        public string PlannedActivityResourceId { get; set; }

        [OrderGrid(Order = 42)]
        public string Driver { get; set; }
        [OrderGrid(Order = 17)]
        public string Benefits { get; set; }
        [OrderGrid(Order = 18)]
        [DisplayName("Planning Risk")]
        public string PlanningRisk { get; set; }

        [OrderGrid(Order = 22)]
        [DisplayName("Budget Value")]
        public string BudgetValueGrid { get; set; }

        [OrderGrid(Order = 25)]
        [DisplayName("Risk Engineering Evaluation")]
        public string RiskEngineeringEvaluation { get; set; }

        [OrderGrid(Order = 29)]
        [DisplayName("Risk Operational Evaluation")]
        public string RiskOperationalEvaluation { get; set; }

        [OrderGrid(Order = 43)]
        [DisplayName("Budget Availability")]
        public string BudgetAvailability { get; set; }

        [OrderGrid(Order = 68)]
        [DisplayName("Vertical Name")]
        public string VerticalName { get; set; }


        [IgnoreGrid]
        public List<string> VerticalNameId { get; set; }

        [OrderGrid(Order = 45)]
        [DisplayName("For LCM")]
        public bool? ForLcmLink { get; set; }

        [OrderGrid(Order = 49)]
        [DisplayName("For DesignAspects")]
        public bool? ForDesignAspectLink { get; set; }

        [OrderGrid(Order = 70)] 
        [DisplayName("For ServicePlan")]
        public bool? ForServicePlanLink { get; set; }

    }

    public class PlannedActivityAtGlanceGridDto
    {
        public long? LcmengineeringId { get; set; }
        public long? DesignAspectId { get; set; }

        public long OpcoId { get; set; }
        public string OpCoDescrption { get; set; }
        public long? PlannedActivityId { get; set; }
        public long? PAResourceId { get; set; }
        public string PAResourceDesc { get; set; }

        public long? PAResourceRuleLinkedId { get; set; }
        public string PAResourceRuleLinkedDesc { get; set; }

        public long? PADelivertyStatusId { get; set; }
        public string PADelivertyStatusDesc { get; set; }

        public long? PADesignComponenetId { get; set; }
        public string PADesignComponenetName { get; set; }
        public string PADCFName { get; set; }
        public string PACurrentDCFName { get; set; }
        public string PACurrentDCName { get; set; }
        public string PAPlannedDCName { get; set; }

        public long? PAActivityStatusId { get; set; }
        public string PAActivityStatusDesc { get; set; }
        public string PAActivityTextDesc { get; set; }

        public string StartDateValue { get; set; }

        public string PlannedCompletion { get; set; }
        public string PlannedImplementationYear { get; set; }

        public string BudgetAvailabilityValue { get; set; }
        public string LocalApprovalValue { get; set; }

        public decimal? BudgetValue { get; set; }
        public string Currency { get; set; }

        public string BudgetTrackingId { get; set; }
        public string Program { get; set; }

        public string ProjectOwner { get; set; }

        public long? ResponsibilityPhaseId { get; set; }
        public string ResponsibilityPhase { get; set; }

        public string DeliveryProjectNameWBSCode { get; set; }
        public string DeliveryProjectIdPPMID { get; set; }
        public string IsLCMReleaseDetailsUnknown { get; set; }

        public string IsPAReleaseDetailsUnknown { get; set; }
        public string compatibilityColorCode { get; set; }
        public string greenCompatibilityPercentage { get; set; }

        public string amberCompatibilityPercentage { get; set; }

        public string redCompatibilityPercentage { get; set; }
        public string TotalCompatibilityPercentage { get; set; }

        public int greenNodeCount { get; set; }

        public int amberNodeCount { get; set; }

        public int redNodeCount { get; set; }
        public long NodesCount { get; set; }

        public long PAPlannedDCNodesCount { get; set; }
        [IgnoreGrid]
        public List<FilterValueDtoKeyValueList> VerticalFilterDto { get; set; }

    }


}