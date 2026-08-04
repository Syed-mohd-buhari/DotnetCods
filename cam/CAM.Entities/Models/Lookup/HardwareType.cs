using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("HardwareTypes")]
 public   class HardwareType : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short HardwareTypeId { get; set; }
        [Required]
        [Column("HardwareType")]
        public string HardwareTypeDescription { get; set; }

        //[InverseProperty(nameof(MajorHardwareBuild.HardwareType))]
        //public virtual ICollection<MajorHardwareBuild> MajorHardwareBuilds { get; set; }
    }
}
