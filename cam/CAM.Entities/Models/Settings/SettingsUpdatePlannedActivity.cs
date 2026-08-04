using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.Settings
{
    [Table("SettingsUpdatePlannedActivity")]
    public class SettingsUpdatePlannedActivity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short SettingsUpdatePlannedActivityId { get; set; }
        public string SettingsUpdatePlannedActivityDescription { get; set; }
        public short PlanningActivityStatusId { get; set; }
        public short? PlanningActivityResourceId { get; set; }

        public short? SuccessorPlannedActivityId { get; set; }
        public short BudgetAvailabilityId { get; set; }
        public string LocalApproval { get; set; }
        public short DeliveryStatusId { get; set; }
        public int Order { get; set; }
        public int Rule { get; set; }
        public int RuleElementCount { get; set; }
        public int MaxOrder { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public bool? RuleForSuccessorPlannedActivityCreation { get; set; }
        public bool? SpecifyDC { get; set; }
        public short? PlannedActivityTypeFor { get; set; }

        public bool? NeedPlannedAsset { get; set; }

        public bool? IsRollback { get; set; }
        public int? MSStatus { get; set; }
        public int? MSStatusDuration { get; set; }
        public bool? IsMileStone { get; set; }

        [ForeignKey(nameof(PlanningActivityStatusId))]
        public virtual PlanningActivityStatus PlanningActivityStatus { get; set; }

        [ForeignKey(nameof(PlanningActivityResourceId))]
        public virtual PlannedActivityResource PlannedActivityResource { get; set; }

        [ForeignKey(nameof(SuccessorPlannedActivityId))]
        public virtual PlannedActivityResource SuccessorPlannedActivityTypeResource { get; set; }

        [ForeignKey(nameof(DeliveryStatusId))]
        public virtual DeliveryStatus DeliveryStatus { get; set; }

        [ForeignKey(nameof(BudgetAvailabilityId))]
        public virtual BudgetAvailability BudgetAvailability { get; set; }

        public List<CrossSettingsUpdatePlannedActivity> CrossSettingsIn { get; set; }
        public List<CrossSettingsUpdatePlannedActivity> CrossSettingsOut { get; set; }
        public List<SettingUpdatePlannedActivityLcmDeploymentStatus> SettingUpdatePlannedActivityLcmDeploymentStatus { get; set; }
        public List<SettingUpdatePlannedActivityAssetDeploymentStatus> SettingUpdatePlannedActivityAssetDeploymentStatus { get; set; }

    }
}
