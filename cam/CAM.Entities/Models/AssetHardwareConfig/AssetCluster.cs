using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.AssetHardwareConfig

{
    public partial class AssetCluster : AuditableEntity
    {
        public AssetCluster()
        {
            AssetHardwareAncillary = new HashSet<AssetHardwareAncillary>();
        }

        public long AssetClusterId { get; set; }
        public string Description { get; set; }
      

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<AssetHardwareAncillary> AssetHardwareAncillary { get; set; }
    }
}
