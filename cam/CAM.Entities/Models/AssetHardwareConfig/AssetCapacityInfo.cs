using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models.AssetHardwareConfig
{
    public partial class AssetCapacityInfo : AuditableEntity
    {
        public long AssetCapacityInfoId { get; set; }
        public long AssetHardwareAncillaryId { get; set; }
        public string PhysicalServerHostName { get; set; }
        public string PhysicalServerIpAddress { get; set; }
        public string PhysicalServerSerialNumber { get; set; }
        public long? NoOfInstances { get; set; }
        public long? Vcpu { get; set; }
        public decimal? Memory { get; set; }
        public decimal? Storage { get; set; }

        public long? PhysicalServerHwModelId { get; set; }
        public long? PhysicalServerVendorId { get; set; }
        public string? PhysicalServerHwModel { get; set; }
        public string? PhysicalServerVendor { get; set; }

        public virtual AssetHardwareAncillary AssetHardwareAncillary { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
    }
}
