using CAM.Entities.Models.AssetHardwareConfig;
using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.VBom
{
    public partial class ClusterName : AuditableEntity
    {
        public ClusterName()
        {
            VnfClusterInfo = new HashSet<VnfClusterInfo>();

            AssetHardwareAncillary = new HashSet<AssetHardwareAncillary>();
        }

        public long ClusterNameId { get; set; }
        public string ClusterDescription { get; set; }
        public short ClusterType { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<VnfClusterInfo> VnfClusterInfo { get; set; }
        public virtual ICollection<AssetHardwareAncillary> AssetHardwareAncillary { get; set; }
    }
}
