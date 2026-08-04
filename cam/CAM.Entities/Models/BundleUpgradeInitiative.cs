using CAM.Entities.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models
{
    [Table("BundleUpgradeInitiatives")]
    public class BundleUpgradeInitiative : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long BundleUpgradeInitiativeId { get; set; }
        public short OriginalEquipmentManufacturerId { get; set; }
        [Required]
        public string VNFType { get; set; }
        public string VerticalOwner { get; set; }
        public string OEMCertifiedRelease { get; set; }
        public string Remarks { get; set; }  
        [Column("Spare1Json")]
        public string Spare1Json { get; set; }

        [ForeignKey(nameof(OriginalEquipmentManufacturerId))]
        [InverseProperty("BundleUpgradeInitiatives")]
        public virtual OriginalEquipmentManufacturer OriginalEquipmentManufacturer { get; set; }
    }
}
