using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Cross
{
    [Table("SystemTypesMajorHardwareBuilds")]
    public partial class SystemTypesMajorHardwareBuild : AuditableEntity
    {
        [Key]
        public long SystemTypeId { get; set; }
        [Key]
        public long MajorHardwareId { get; set; }

        [ForeignKey(nameof(MajorHardwareId))]
        [InverseProperty(nameof(MajorHardwareBuild.SystemTypesMajorHardwareBuilds))]
        public virtual MajorHardwareBuild MajorHardware { get; set; }
        [ForeignKey(nameof(SystemTypeId))]
        [InverseProperty("SystemTypesMajorHardwareBuilds")]
        public virtual SystemType SystemType { get; set; }

        public bool IsMain { get; set; }
    }
}