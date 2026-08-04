using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("HardwareSolutionResource")]
    public partial class HardwareSolutionResource : AuditableEntity
    {
      

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short HardwareSolutionResourceId { get; set; }
        [Column("HardwareSolutionReource")]
        public string HardwareSolutionResourceDescription { get; set; }

        [InverseProperty(nameof(MajorHardwareBuild.HardwareSolutionResource))]
        public virtual ICollection<MajorHardwareBuild> MajorHardwareBuilds{ get; set; }
    }
}