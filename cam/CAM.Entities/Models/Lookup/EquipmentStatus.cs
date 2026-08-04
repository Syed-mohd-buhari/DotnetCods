using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("EquipmentStatuses")]
    public class EquipmentStatus : AuditableEntity
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public short EquipmentStatusId { get; set; }
            [Required]
            [Column("EquipmentStatus")]
            public string EquipmentStatusDescription { get; set; }

        [InverseProperty(nameof(VNFTransition.EquipmentStatus))]
        public virtual ICollection<VNFTransition> VNFTransitions { get; set; }

    }
}
