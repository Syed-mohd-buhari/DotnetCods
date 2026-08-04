using CAM.Entities.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models
{
    [Table("VFNDesignComponents")]
    public class VNFDesignComponent : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short VNFDesignComponentId { get; set; }
        [Required]
        [Column("DesignComponent")]
        public string VNFDesignComponentDescription { get; set; }

        [InverseProperty(nameof(VNFTransition.VNFDesignComponent))]
        public virtual ICollection<VNFTransition> VNFTransitions { get; set; }
    }
}
