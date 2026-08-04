using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Settings;

namespace CAM.Entities.Models.Lookup
{
    [Table("DeliveryStatuses")]
    public partial class DeliveryStatus : AuditableEntity
    {
        public DeliveryStatus()
        {
            PlannedActivities = new HashSet<PlannedActivity>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short DeliveryStatusId { get; set; }
        [Required]
        [Column("DeliveryStatus")]
        [StringLength(150)]
        public string DeliveryStatusDescription { get; set; }

        public int ProjectStatusCombinationRule { get; set; }
        public int Rule { get; set; }

        [InverseProperty(nameof(PlannedActivity.DeliveryStatus))]
        public virtual ICollection<PlannedActivity> PlannedActivities { get; set; }

        public virtual ICollection<SettingsUpdatePlannedActivity> SettingsUpdatePlannedActivities { get; set; }
    }
}