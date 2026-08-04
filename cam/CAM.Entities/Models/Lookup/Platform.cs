using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("Platforms")]
  public  class Platform : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short PlatformId { get; set; }
        [Required]
        [Column("Platform")]
        public string PlatformDescription { get; set; }

        [InverseProperty(nameof(MajorHardwareBuild.Platform))]
        public virtual ICollection<MajorHardwareBuild> MajorHardwareBuilds { get; set; }
    }
}
