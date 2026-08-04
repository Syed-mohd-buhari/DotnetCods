using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Settings;

namespace CAM.Entities.Models.Lookup
{
    [Table("BudgetAvailability")]
    public partial class BudgetAvailability : AuditableEntity
    {
     //comment
        public BudgetAvailability()
        {
            PlannedActivities = new HashSet<PlannedActivity>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short BudgetAvailabilityId { get; set; }

        [Required]
        [Column("BudgetAvailability")]
        public string BudgetAvailabilityDescription { get; set; }
        public int ProjectStatusCombinationRule { get; set; }
        public int Rule { get; set; }
        [InverseProperty(nameof(PlannedActivity.BudgetAvailability))]
        public virtual ICollection<PlannedActivity> PlannedActivities { get; set; }
        public virtual ICollection<SettingsUpdatePlannedActivity> SettingsUpdatePlannedActivities { get; set; }
    }
}