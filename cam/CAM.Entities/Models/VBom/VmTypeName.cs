using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.VBom
{
    public partial class VmTypeName : AuditableEntity
    {
        public VmTypeName()
        {
            VnfInfo = new HashSet<VnfInfo>();
        }

        public long VmtypeNameId { get; set; }
        public long VnfNameId { get; set; }
        public string VmTypeDescription { get; set; }

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual VnfName VnfName { get; set; }
        public virtual ICollection<VnfInfo> VnfInfo { get; set; }
    }
}
