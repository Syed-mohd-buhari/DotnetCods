using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("BuildConstructions")]
    public  class BuildConstruction : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short BuildConstructionId { get; set; }
        [Required]
        [Column("BuildConstruction")]
        public string BuildConstructionDescription { get; set; }
        public int Rule { get; set; }
        public string CloudType { get; set; }
        public bool? IsCluodHostedAsset { get; set; }
        public virtual ICollection<MajorHardwareBuild> MajorHardwareBuilds { get; set; }
    }
}
