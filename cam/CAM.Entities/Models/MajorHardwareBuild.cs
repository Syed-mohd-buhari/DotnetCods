using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    [Table("MajorHardwareBuilds")]
    public partial class MajorHardwareBuild : AuditableEntity
    {
     
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MajorHardwareId { get; set; }
        public short? HardwareSolutionReourceId { get; set; }
        public short OriginalEquipmentManufacturerId { get; set; }
        [StringLength(255)]
        public string HardwareSolution { get; set; }
        public short PlatformId { get; set; }
        [Required]
        [StringLength(255)]
        public string HardwareType { get; set; }
        [StringLength(255)]
        public string OtherHardwareInfo { get; set; }
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
        public string VulnerabilityStatus { get; set; }
        [Column("SpareFieldsJSON")]
        public string SpareFieldsJson { get; set; }

        public bool ProprietaryHardware { get; set; }

        [ForeignKey(nameof(HardwareSolutionReourceId))]
        [InverseProperty(nameof(Lookup.HardwareSolutionResource.MajorHardwareBuilds))]
        public virtual HardwareSolutionResource HardwareSolutionResource { get; set; }
        [ForeignKey(nameof(OriginalEquipmentManufacturerId))]
        [InverseProperty("MajorHardwareBuilds")]
        public virtual OriginalEquipmentManufacturer OriginalEquipmentManufacturer { get; set; }
        [InverseProperty(nameof(SystemTypesMajorHardwareBuild.MajorHardware))]
        public List<SystemTypesMajorHardwareBuild> SystemTypesMajorHardwareBuilds { get; set; }
        [ForeignKey(nameof(PlatformId))]
        public virtual Platform Platform { get; set; }
        public short? BuildConstructionId { get; set; }
        [ForeignKey(nameof(BuildConstructionId))]
        public BuildConstruction BuildConstruction { get; set; }
        public string TypeOfProcessor { get; set; }
        public string OperatingSystem { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GeneraAvailableDate { get; set; }

        public string Description { get; set; }
        public int? DesignContactId { get; set; }
        public ApplicationUser DesignContactNavigation { get; set; }

        public virtual ICollection<MajorHwBuidlsDesignContact> MajorHwBuidlsDesignContacts { get; set; }
    }
}