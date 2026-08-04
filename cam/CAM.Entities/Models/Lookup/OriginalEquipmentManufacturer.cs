using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("OriginalEquipmentManufacturers")]
    public partial class OriginalEquipmentManufacturer : AuditableEntity
    {
        public OriginalEquipmentManufacturer()
        {
            MajorHardwareBuilds = new HashSet<MajorHardwareBuild>();
            MajorSoftwareBuilds = new HashSet<MajorSoftwareBuild>();
            BundleUpgradeInitiatives = new HashSet<BundleUpgradeInitiative>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short OriginalEquipmentManufacturerId { get; set; }
        [Required]
        [Column("OriginalEquipmentManufacturer")]
        [StringLength(50)]
        public string OriginalEquipmentManufacturerDescription { get; set; }


        [InverseProperty(nameof(BundleUpgradeInitiative.OriginalEquipmentManufacturer))]
        public virtual ICollection<BundleUpgradeInitiative> BundleUpgradeInitiatives { get; set; }

        [InverseProperty(nameof(MajorHardwareBuild.OriginalEquipmentManufacturer))]
        public virtual ICollection<MajorHardwareBuild> MajorHardwareBuilds { get; set; }
        [InverseProperty(nameof(MajorSoftwareBuild.OriginalEquipmentManufacturer))]
        public virtual ICollection<MajorSoftwareBuild> MajorSoftwareBuilds { get; set; }

        [InverseProperty(nameof(NetworkElementAsIs.OriginalEquipmentManufacturer))]
        public virtual ICollection<NetworkElementAsIs> Networkelementsasis { get; set; }


        [InverseProperty(nameof(DesignComponentFamily.MajorSoftwareOem))]
        public virtual ICollection<DesignComponentFamily> MajorSoftwareDesignComponentFamily { get; set; }

        [InverseProperty(nameof(DesignComponentFamily.MajorHardwareOem))]
        public virtual ICollection<DesignComponentFamily> MajorHardwareDesignComponentFamily { get; set; }

  

    }
}