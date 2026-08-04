using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models.Cross
{
    [Table("PlannedActivityResourcePlanningRisk")]
    public partial class PlannedActivityResourcePlanningRisk : AuditableEntity
    {
        [Key]
        public long PlannedActivityResourcePlanningRiskId { get; set; }

        public short PlannedActivityResourceId { get; set; }

        public short PlanningRiskId { get; set; }

        public bool ForLcm { get; set; }

        public bool? ForDesignAspect { get; set; }

        public bool? ForAddAsset { get; set; }
        public bool? ForEditAsset { get; set; }

        [ForeignKey(nameof(PlannedActivityResourceId))]
        public virtual PlannedActivityResource PlannedActivityResource { get; set; }
        [ForeignKey(nameof(PlanningRiskId))]
        public virtual PlanningRisk PlanningRisk { get; set; }
    }
}