using System;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Enum;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public abstract class PlannedActivityDto : GridDtoBase
    {
        [IgnoreGrid]
        public long? LcmEngineeringId { get; set; }
        [IgnoreGrid]
        public long? NetworkElementAsPlannedId { get; set; }
        [IgnoreGrid]
        public long? DesignAspectId { get; set; }

        [DisplayName("Implementation Year")]
        [OrderGrid(Order = 12)]
        [Archive]
        public short? PlannedImplementationYear { get; set; }

        [IgnoreGrid]
        public string PlannedActivityDescription { get; set; }



        [OrderGrid(Order = 21)]
        [DisplayName("Delivery Project Name")]
        [Default]
        public string DeliveryProjectName { get; set; }

        [OrderGrid(Order = 19)]
        [DisplayName("Local Approval")]
        public string LocalApproval { get; set; }

        [IgnoreGrid]
        public DateTime? PlannedCompletion { get; set; }

        [DateRangeGrid]
        [DisplayName("Planned Completion")]
        [OrderGrid(Order = 11)]
        [Default]
        public string PlannedCompletionValue { get; set; }

        [OrderGrid(Order = 30)]
        public string Notes { get; set; }

        [OrderGrid(Order = 26)]
        [DisplayName("Risk Engineering Notes")]
        public string RiskEngineeringNotes { get; set; }

        [OrderGrid(Order = 27)]
        [DisplayName("Risk Operational Notes")]
        public string RiskOperationalNotes { get; set; }

        [IgnoreGrid]
        public string SpareFieldsJson { get; set; }


        [IgnoreGrid]
        public string DeliveryProjectId { get; set; }



        [OrderGrid(Order = 20)]
        [DisplayName("Budget Tracking")]
        public string BudgetTrackingId { get; set; }

        [IgnoreGrid]
        public string Currency { get; set; }
        [OrderGrid(Order = 28)]

        [DisplayName("ProjectStatus")]
        [Default]
        public string ProjectStatus { get; set; }

        [IgnoreGrid]
        public decimal? BudgetValue { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Activity Details")]
        [Default]
        public string ActivityDetails { get; set; }
        [IgnoreGrid]
        public bool IsNewServiceArchitecture { get; set; }
        [IgnoreGrid]
        public bool IsReplacementExistingSolution { get; set; }
        [IgnoreGrid]
        public long DesignComponentFamilyId { get; set; }

        [IgnoreGrid]
        public long? PlannedDesignComponentFamilyId { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("DCF Name")]
        [Default]

        public string DesignComponentFamilyName { get; set; }

        [DisplayName("Planned Design Component Name")]
        [OrderGrid(Order = 36)]
        public string PlannedDesignComponentName { get; set; }

        [IgnoreGrid]
        [Archive]
        public DateTime? StartDate { get; set; }

        [OrderGrid(Order = 31)]
        [DateRangeGrid]
        [DisplayName("Start Date")]
        public string StartDateValue { get; set; }

        [OrderGrid(Order = 37)]
        [DisplayName("For Add Asset")]

        public bool? ForAddAsset { get; set; }

        [OrderGrid(Order = 38)]
        [DisplayName("For Edit Asset")]
        public bool? ForEditAsset { get; set; }


        [IgnoreGrid]
        public new DateTime? LastModified { get; set; }

        [OrderGrid(Order = 33)]
        [DateRangeGrid]
        [DisplayName("Last Modified")]
        public string LastModifiedValue { get; set; }

        [OrderGrid(Order = 34)]
        [MailTo]
        [DisplayName("Last Modified By")]
        public new string LastModifiedBy { get; set; }

        [OrderGrid(Order = 32)]
        [DisplayName("Archived")]
        public bool? Archived { get; set; } = false;

        [OrderGrid(Order = 35)]
        [DisplayName("Delivery Plan Available")]
        public bool DeliveryPlanAvailable { get; set; }

        [OrderGrid(Order = 39)]
        [DisplayName("Program")]
        public string Program { get; set; }

        [OrderGrid(Order = 40)]
        [DisplayName("Project Owner")]
        public string ProjectOwner { get; set; }

        [OrderGrid(Order = 41)]
        [DisplayName("Delivery Tracking Index")]
        public int? DeliveryTrackingId { get; set; }

        [IgnoreGrid]
        public PlannedActivityTypeForEnum? PlannedActivityTypeFor { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order = 44)]

        public bool? IsPAReleaseDetailUnknown { get; set; }

        //Ticket 751 PPM Import

        [OrderGrid(Order = 46)]
        [DisplayName("PPM ID")]
        public string DeliveryProjectPpmId { get; set; }

        public long BuildBagId { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Current Bag Name")]
        public string CurrentBuildBagDescription { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("Planned Bag Name")]
        public string PlannedBuildBagDescription { get; set; }

        [OrderGrid(Order = 50)]
        [DisplayName("Planned Activity Team")]
        public string PlannedActivityTeam { get; set; }

        [OrderGrid(Order = 51)]
        [DisplayName("Priority")]
        public string Priority { get; set; }

        [IgnoreGrid]
        public short PlannedActivityCategoryId { get; set; }

        [OrderGrid(Order = 52)]
        [DisplayName("Planned Activity Category")]
        public string PlannedActivityCategory { get; set; }

        [OrderGrid(Order = 53)]
        [DisplayName("LCM Category")]
        public string LcmCategories { get; set; }

        [IgnoreGrid]
        public long ProgramId { get; set; }

        [OrderGrid(Order = 47)]
        [DisplayName("Project Description")]
        public string ProjectDescription { get; set; }

        [IgnoreGrid]
        [Archive]
        public DateTime? PreBaseLineDate { get; set; }

        [OrderGrid(Order = 48)]
        [DateRangeGrid]
        [DisplayName("Pre-BaseLine End Date")]
        public string PreBaseLineDateValue { get; set; }

        [Default]
        [OrderGrid(Order = 69)]
        [DisplayName("Is Service Plan")]
        public bool? IsServicePlan { get; set; }
        [IgnoreGrid]
        public int ServicePlanid { get; set; }
    }
}
