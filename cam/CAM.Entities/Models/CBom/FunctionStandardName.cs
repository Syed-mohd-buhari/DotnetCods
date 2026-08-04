using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.CBom
{
    public partial class FunctionStandardName : AuditableEntity
    {
        public FunctionStandardName()
        {
            CnfPodInfo = new HashSet<CnfPodInfo>();
        }

        public long FunctionStandardNameId { get; set; }
        public string FunctionName { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<CnfPodInfo> CnfPodInfo { get; set; }
    }
}
