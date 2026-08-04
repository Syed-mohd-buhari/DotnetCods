using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("ResponsibilityPhases")]
    public partial class ResponsibilityPhase : AuditableEntity
    {
        public ResponsibilityPhase()
        {
            PlannedActivities = new HashSet<PlannedActivity>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short ResponsibilityPhaseId { get; set; }
        [Column("ResponsibilityPhase")]
        [StringLength(50)]
        public string ResponsibilityPhaseDescription { get; set; }

        public int Rule { get; set; }


        [InverseProperty(nameof(PlannedActivity.ResponsibilityPhase))]
        public virtual ICollection<PlannedActivity> PlannedActivities { get; set; }
    }
}