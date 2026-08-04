using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("ActivityStatuses")]
    public partial class ActivityStatus : AuditableEntity
    {
     //comment
        public ActivityStatus()
        {
            PlannedActivities = new HashSet<PlannedActivity>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short ActivityStatusId { get; set; }

        [Required]
        [Column("ActivityStatus")]
        [StringLength(50)]
        public string ActivityStatusDescription { get; set; }
        public int Rule { get; set; }
        public int ProjectStatusCombinationRule { get; set; }


        [InverseProperty(nameof(PlannedActivity.ActivityStatus))]
        public virtual ICollection<PlannedActivity> PlannedActivities { get; set; }
    }
}