using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.CBom
{
    public class CnfPriority : AuditableEntity
    {
        public CnfPriority()
        {
            CnfPodInfo = new HashSet<CnfPodInfo>();
        }

        public long CnfPriorityId { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<CnfPodInfo> CnfPodInfo { get; set; }
    }
}
