using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.VBom
{
    public partial class VmWorkLoadType : AuditableEntity
    {
        public VmWorkLoadType()
        {
            VnfInfo = new HashSet<VnfInfo>();
        }

        public long VmworkLoadTypeId { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<VnfInfo> VnfInfo { get; set; }
    }
}
