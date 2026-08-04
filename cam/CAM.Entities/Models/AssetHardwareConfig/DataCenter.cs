using CAM.Entities.Models.Base;
using CAM.Identity;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;

namespace CAM.Entities.Models.AssetHardwareConfig
{
    public partial class DataCenter : AuditableEntity
    {
        public DataCenter()
        {
            AssetHardwareAncillary = new HashSet<AssetHardwareAncillary>();
        }

        public long DataCenterId { get; set; }
        public string Description { get; set; }
        public short OpcoId { get; set; }
        public bool? EnvironmentZone { get; set; }
      
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual OpCo Opco { get; set; }
        public virtual ICollection<AssetHardwareAncillary> AssetHardwareAncillary { get; set; }
    }
}
