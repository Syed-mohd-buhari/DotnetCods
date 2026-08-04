using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models
{
    public class VnfHardWare : AuditableEntity
    {
        public long VnfHardWareId { get; set; }
        public string Description { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
    }
}
