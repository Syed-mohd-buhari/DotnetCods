using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.VBom
{
    public partial class VnfName : AuditableEntity
    {
        public VnfName()
        {
            VmTypeName = new HashSet<VmTypeName>();
            VnfInfo = new HashSet<VnfInfo>();
        }

        public long VnfNameId { get; set; }
        public string VnfDescription { get; set; }
        public decimal? ProductId { get; set; }

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ProductName Product { get; set; }
        public virtual ICollection<VmTypeName> VmTypeName { get; set; }
        public virtual ICollection<VnfInfo> VnfInfo { get; set; }
    }
}
