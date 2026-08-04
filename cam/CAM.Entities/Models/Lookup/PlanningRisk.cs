using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;

namespace CAM.Entities.Models.Lookup
{
    [Table("PlanningRisks")]
    public partial class PlanningRisk : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short PlanningRiskId { get; set; }
        [Column("PlanningRisk")]

        public string PlanningRiskDescription { get; set; }

        public virtual ICollection<PlannedActivityResourcePlanningRisk> PlannedActivityResourcePlanningRisk { get; set; }

    }
}
