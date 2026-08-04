using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("Risk")]
    public partial class RiskResource : AuditableEntity
    {
     //comment
        public RiskResource()
        {
            PlannedActivities = new HashSet<PlannedActivity>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short RiskId { get; set; }

        [Required]
        [Column("Risk")]
        public string RiskDescription { get; set; }


        public int Severity { get; set; }


        [InverseProperty(nameof(PlannedActivity.OperationalRisk))]
        public virtual ICollection<PlannedActivity> PlannedActivities { get; set; }
    }
}