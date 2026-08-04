using CAM.Entities.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models
{
    [Table("VNFTransitions")]
    public class VNFTransition : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VNFTransitionId { get; set; }
        public short OpCoId { get; set; }
        public short? VNFDesignComponentId { get; set; }
        [Required]
        public string VNFType { get; set; }
        public string CurrentRelease { get; set; }
        public string PlannedRelease { get; set; }
        [Required]
        public string ElementName { get; set; }
        [Column("Spare1Json")]
        public string Spare1Json { get; set; }
        public string Location { get; set; }
        public short? NFVIBundleIDId { get; set; }
        [Required]
        public string NFVISiteDesignation { get; set; }
        public short? EquipmentStatusId { get; set; }

        [ForeignKey(nameof(OpCoId))]

        public virtual OpCo OpCo { get; set; }
        [ForeignKey(nameof(VNFDesignComponentId))]
        public virtual VNFDesignComponent VNFDesignComponent { get; set; }
        [ForeignKey(nameof(EquipmentStatusId))]

        public virtual EquipmentStatus EquipmentStatus { get; set; }

        
        [ForeignKey(nameof(NFVIBundleIDId))]
        public virtual NFVIBundleID NFVIBundleID { get; set; }
        
    }
}

