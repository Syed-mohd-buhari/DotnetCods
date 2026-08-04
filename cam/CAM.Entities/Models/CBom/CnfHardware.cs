using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.CBom
{
    public class CnfHardware : AuditableEntity
    {
        public CnfHardware()
        {
            CnfClusterInfo = new HashSet<CnfClusterInfo>();
        }

        public long CnfHardwareId { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<CnfClusterInfo> CnfClusterInfo { get; set; }
    }
}
