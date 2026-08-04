using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models
{
    [Table("DesignComponentFamilies")]
    public partial class DesignComponentFamily : AuditableEntity
    {
        public DesignComponentFamily()
        {
            DesignComponents = new List<DesignComponent>();
            Serviceplandcfmapping = new HashSet<ServicePlanDcfMapping>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DesignComponentFamilyId { get; set; }

        public string SystemTypeIdentityName { get; set; }
        public string Description { get; set; }

        [Required]
        public long SubNetworkBoundaryId { get; set; }

        public bool SystemIsShared { get; set; }
        ////Tack 550 
        public short? SecurityTireZoneId { get; set; }
        public short? SharingTypeId { get; set; }

        public short? MajorSoftwareOemId { get; set; }
        public string MajorSoftwareProductName { get; set; }
        [StringLength(50)]
        [Required]
        public string ProductName { get; set; }
        public short? MajorHardwareOemId { get; set; }
        public short? PlatformId { get; set; }

        public bool Implementation { get; set; }

        public CriticalAssetType? CriticalAssetType { get; set; }

        public int? CriticalityRating { get; set; }

        public bool? CountrySpecificCriticality { get; set; }

        public List<FilterValueDtoKeyValueList> VerticalFilterDto { get; set; }
         
        [ForeignKey(nameof(MajorSoftwareOemId))]
        [InverseProperty(nameof(OriginalEquipmentManufacturer.MajorSoftwareDesignComponentFamily))]
        public virtual OriginalEquipmentManufacturer MajorSoftwareOem { get; set; }

        [ForeignKey(nameof(MajorHardwareOemId))]
        [InverseProperty(nameof(OriginalEquipmentManufacturer.MajorHardwareDesignComponentFamily))]
        public virtual OriginalEquipmentManufacturer MajorHardwareOem { get; set; }


        public List<DesignComponentFamilySystemFunction> SystemFunctions { get; set; }


        public List<DesignComponentFamilyCustomerWheel> CustomerWheels { get; set; }

        [InverseProperty(nameof(DesignComponent.DesignComponentFamily))]
        public List<DesignComponent> DesignComponents { get; set; }

        [ForeignKey(nameof(SubNetworkBoundaryId))]
        [InverseProperty(nameof(Lookup.SubNetworkBoundary.DesignComponentFamilies))]
        public virtual SubNetworkBoundary SubNetworkBoundary { get; set; }

        [ForeignKey(nameof(SecurityTireZoneId))]
        [InverseProperty(nameof(Lookup.SecurityTireZone.DesignComponentFamilies))]
        public virtual SecurityTireZone SecurityTireZone { get; set; }

        [ForeignKey(nameof(SharingTypeId))]
        [InverseProperty(nameof(Lookup.SharingType.DesignComponentFamilies))]
        public virtual SharingType SharingType { get; set; }
        public decimal? ProductNameId { get; set; }

        [ForeignKey(nameof(ProductNameId))]
        [InverseProperty(nameof(Lookup.ProductName.DesignComponentFamilies))]
        public virtual ProductName ProductNameNavigation { get; set; }

        public virtual ICollection<ServicePlanDcfMapping> Serviceplandcfmapping { get; set; }

    }
}