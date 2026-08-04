using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models.Settings
{
    [Table("CrossSettingsUpdatePlannedActivity")]
    public class CrossSettingsUpdatePlannedActivity : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }
        public short SettingsUpdatePlannedActivityInId { get; set; }
        public short SettingsUpdatePlannedActivityOutId { get; set; }

        public int CrossSettingsRule { get; set; }
        [ForeignKey(nameof(SettingsUpdatePlannedActivityInId))]
        public virtual SettingsUpdatePlannedActivity SettingsUpdatePlannedActivityIn { get; set; }
        [ForeignKey(nameof(SettingsUpdatePlannedActivityOutId))]
        public virtual SettingsUpdatePlannedActivity SettingsUpdatePlannedActivityOut { get; set; }

    }
}
