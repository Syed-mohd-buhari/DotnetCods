using CAM.Entities.Models.Base;
using CAM.Entities.Models.ComponentSoftware;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OperatingSystem = CAM.Entities.Models.Lookup.OperatingSystem;

namespace CAM.Entities.Models
{
    public partial class ComponentSoftwareBuild : AuditableEntity
    {
        public ComponentSoftwareBuild()
        {
            ComponentSoftwareBuildBag = new List<ComponentSoftwareBuildBag>();
            ComponentSoftwareBuildsDesignContact = new List<ComponentSoftwareBuildsDesignContact>();
           
        }

        public long ComponentSoftwareBuildId { get; set; }
        [Required]
        public long? Componentmanufacturerid { get; set; }


        [Required]
        public int? CriticalAssetTypeId { get; set; }
 
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
        [InverseProperty(nameof( Lookup.OperatingSystem.ComponentSoftwareBuilds))]
        public virtual OperatingSystem OperatingSystem { get; set; } 

        [ForeignKey(nameof(CriticalAssetTypeId))]        
        [InverseProperty(nameof(Lookup.CriticalAssetType.ComponentSoftwareBuilds))]
        public virtual CriticalAssetType CriticalAssetType { get; set; }

        [Column("SpareFieldsJSON")]
        public string SpareFieldsJson { get; set; }

       
         
        [InverseProperty(nameof(SystemType.MajorSoftwareBuilds))]
        public List<SystemType> SystemTypes { get; set; }

        [ForeignKey(nameof(Componentmanufacturerid))]
        [InverseProperty("ComponentSoftwareBuilds")]
        public virtual ComponentManufacturers ComponentManufacturers { get; set; }
  
        public   List<ComponentSoftwareBuildBag> ComponentSoftwareBuildBag { get; set; }
        public   List<ComponentSoftwareBuildsDesignContact> ComponentSoftwareBuildsDesignContact { get; set; }
        

    }
}
