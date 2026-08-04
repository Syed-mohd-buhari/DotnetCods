using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models
{
    [Table("PlannedActivities")]
    public class PlannedActivity : AuditableEntity
    {
        public PlannedActivity()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PlannedActivityId { get; set; }
        public long? DesignComponentId { get; set; }
        public long? LcmEngineeringId { get; set; }
        public long? NetworkElementAsPlannedId { get; set; }
        public long? OriginalDesignComponentIndex { get; set; }
        public short? RelatesToId { get; set; }
        public short PlannedImplementationYear { get; set; }
        public short ActivityStatusId { get; set; }
        public short PlanningActivityStatusId { get; set; }
        public bool IsNewServiceArchitecture { get; set; }
        public bool IsReplacementExistingSolution { get; set; }
        public long? LinkedToPlannedActivityId { get; set; }

        public short? DriverId { get; set; }
        public short? BenefitId { get; set; }
        public short? PlanningRiskId { get; set; }
        public long? DesignAspectId { get; set; }
        public long? DesignComponentFamilyId { get; set; }

        //Exodus 
        public long? PlannedDesignComponentFamilyId { get; set; }

        public bool? ForAddAsset { get; set; }
        public bool? ForEditAsset { get; set; }

        public virtual DesignAspect DesignAspect { get; set; }

        public virtual DesignComponentFamily DesignComponentFamily { get; set; }



        [Column("PlannedActivity")]
        [StringLength(500)]
        public string PlannedActivityDescription { get; set; }

        public short? PlannedActivityResourceId { get; set; }


        [StringLength(2000)] public string ActivityDetails { get; set; }
        [ForeignKey(nameof(BudgetAvailabilityId))]
        public virtual BudgetAvailability BudgetAvailability { get; set; }
        public virtual short? BudgetAvailabilityId { get; set; }


        [ForeignKey(nameof(OperationalRiskId))]
        public virtual RiskResource OperationalRisk { get; set; }
        public virtual short? OperationalRiskId { get; set; }

        [ForeignKey(nameof(EngineeringRiskId))]
        public virtual RiskResource EngineeringRisk { get; set; }
        public virtual short? EngineeringRiskId { get; set; }

        [StringLength(255)] public string DeliveryProjectName { get; set; }

        [StringLength(50)] public string LocalApproval { get; set; }

        public short? DeliveryStatusId { get; set; }
        public short? ResponsibilityPhaseId { get; set; }
        public DateTime? PlannedCompletion { get; set; }
        public string Notes { get; set; }
        public string RiskEngineeringNotes { get; set; }
        public string ProjectStatus { get; set; }

        public string RiskOperationalNotes { get; set; }
        [Column("SpareFieldsJSON")] public string SpareFieldsJson { get; set; }


        public decimal? BudgetValue { get; set; }
       
        public string? DeliveryProjectId { get; set; }
        public string? BudgetTrackingId { get; set; }

        public DateTime? StartDate { get; set; }

        [ForeignKey(nameof(ActivityStatusId))]
        [InverseProperty("PlannedActivities")]
        public virtual ActivityStatus ActivityStatus { get; set; }

        public long? OriginalLcmEngineeringId { get; set; }

        [ForeignKey(nameof(OriginalLcmEngineeringId))]
        public virtual LcmEngineering Originallcmengineering { get; set; }

        [ForeignKey(nameof(DeliveryStatusId))]
        [InverseProperty("PlannedActivities")]
        public virtual DeliveryStatus DeliveryStatus { get; set; }
        [ForeignKey(nameof(DesignComponentId))]
        [InverseProperty("PlannedActivities")]
        public virtual DesignComponent DesignComponent { get; set; }

        [ForeignKey(nameof(PlanningActivityStatusId))]
        [InverseProperty("PlannedActivities")]
        public virtual PlanningActivityStatus PlanningActivityStatus { get; set; }
        public short? OpCoId { get; set; }


        [ForeignKey(nameof(ResponsibilityPhaseId))]
        [InverseProperty("PlannedActivities")]
        public virtual ResponsibilityPhase ResponsibilityPhase { get; set; }

        [ForeignKey(nameof(LcmEngineeringId))]
        public virtual LcmEngineering LcmEngineering { get; set; }

        [ForeignKey(nameof(NetworkElementAsPlannedId))]
        public virtual NetworkElementAsPlanned NetworkElementAsPlanned { get; set; }

        [ForeignKey(nameof(PlannedActivityResourceId))]
        public virtual PlannedActivityResource PlannedActivityResource { get; set; }

        [ForeignKey(nameof(OpCoId))]
        public virtual OpCo OpCo { get; set; }
        public string Currency { get; set; }
        public string OverallRiskEvaluation { get; set; }


        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; }
        [ForeignKey(nameof(BenefitId))]
        public virtual Benefit Benefit { get; set; }
        [ForeignKey(nameof(PlanningRiskId))]
        public virtual PlanningRisk PlanningRisk { get; set; }
        public bool? Archived { get; set; } = false;
        public bool DeliveryPlanAvailable { get; set; } = false;
        public string ProjectOwner { get; set; }
        public int? DeliveryTrackingId { get; set; }

        public bool? IsPAReleaseDetailUnknown { get; set; }

        public  List<FilterValueDtoKeyValueList> VerticalFilterDto {get;set;}
        public List<int?> DesignContactDto { get; set; }

        public long  Buildbagid { get; set; }
        public   BuildBag Buildbag { get; set; }

        public string Plannedactivityteam { get; set; }
        public string Priority { get; set; }
        public short? Plannedactivitycategoryid { get; set; }
        public long? DesigncComponentFamilyId { get; set; }
        public short? Lcmcategories { get; set; }
        public long? ProgramId { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime? PreBaseLineDate { get; set; }
        public virtual ProgramEntity ProgramNavigation { get; set; }
        public virtual PlannedActivityCategory PlannedactivitycategoryNavigation { get; set; }
        public int? Serviceplanid { get; set; }
        public virtual ServicePlan Serviceplan { get; set; }
        public bool? Isserviceplan { get; set; }


    }
}