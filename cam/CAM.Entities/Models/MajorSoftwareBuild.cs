using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using OperatingSystem = CAM.Entities.Models.Lookup.OperatingSystem;
using CAM.Enum;
using OracleModels.DBModels;
using CAM.Identity;

namespace CAM.Entities.Models
{
    [Table("MajorSoftwareBuilds")]
    public partial class MajorSoftwareBuild : AuditableEntity
    {
        public MajorSoftwareBuild()
        {
           
            SystemTypes = new List<SystemType>();
            SoftwarebuildcompatibilityMajorsoftwarebuild =   new List<SoftwareBuildCompatibility>();
            MajorSwBuidlsDesignContacts = new List<MajorSwBuidlsDesignContact>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MajorSoftwareBuildsId { get; set; }
        [Required]
        public short OriginalEquipmentManufacturerId { get; set; }

        [Required]
        public int? CriticalAssetTypeId { get; set; }

        /// <summary>
        /// Software release number
        /// </summary>
        [StringLength(255)]
        [Required]
        public string SoftwareVersion { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastTimeBuyNew { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastTimeBuyUpgrades { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastTimeBuyExpansions { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EndOfMaintenance { get; set; }

        public EOMEnum EOMStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndOfsupport { get; set; }
        [Column(TypeName = "date")]
        public DateTime? GeneraAvailableDate { get; set; }
        [StringLength(50)]
        public string DeliveryMethod { get; set; }
        public string VulnerabilityStatus { get; set; }

        public short? OperatingSystemId { get; set; }
        [ForeignKey(nameof(OperatingSystemId))]
        [InverseProperty(nameof(Lookup.OperatingSystem.MajorSoftwareBuilds))]
        public virtual OperatingSystem OperatingSystem { get; set; }
        

        [ForeignKey(nameof(CriticalAssetTypeId))]
        //[InverseProperty("MajorSoftwareBuilds")]
        [InverseProperty(nameof(Lookup.CriticalAssetType.MajorSoftwareBuilds))]
        public virtual CriticalAssetType CriticalAssetType { get; set; }

        [Column("SpareFieldsJSON")]
        public string SpareFieldsJson { get; set; }
   

        [ForeignKey(nameof(OriginalEquipmentManufacturerId))]
        [InverseProperty("MajorSoftwareBuilds")]
        public virtual OriginalEquipmentManufacturer OriginalEquipmentManufacturer { get; set; }

        
        [InverseProperty(nameof(SystemType.MajorSoftwareBuilds))]
        public  List<SystemType> SystemTypes { get; set; }
        public decimal? ProductNameId { get; set; }

        [ForeignKey(nameof(ProductNameId))]
        [InverseProperty("MajorSoftwareBuilds")]
        public virtual ProductName ProductName { get; set; }

        public List<MajorSwBuidlsDesignContact> MajorSwBuidlsDesignContacts { get; set; }
        public List<MajorSoftwareBuildFamilyNetworkFunction> NetworkFunctions { get; set; }
       
       
        public   List<SoftwareBuildCompatibility> SoftwarebuildcompatibilityMajorsoftwarebuild { get; set; }
        public bool? Isvmware { get; set; }

    }
}
