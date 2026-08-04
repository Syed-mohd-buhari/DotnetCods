using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.AssetHardwareConfig
{
    public partial class AssetClusterType : AuditableEntity
    {
        public AssetClusterType()
        {
            AssetHardwareAncillary = new HashSet<AssetHardwareAncillary>();
        }

        public long AssetClusterTypeId { get; set; }
        public string Description { get; set; }
 

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<AssetHardwareAncillary> AssetHardwareAncillary { get; set; }
    }
}
