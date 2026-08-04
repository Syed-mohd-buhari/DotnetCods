using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
namespace CAM.Entities.Models.RBAC
{
    public class AspNetUserOpcos : AuditableEntity
    {
        public int Aspnetuseropcoid { get; set; }
        public int? Userid { get; set; }
        public short? Opcoid { get; set; }
        public bool? Isrestrictedopco { get; set; }
        public bool? Isinusedopcos { get; set; }
        public virtual OpCo Opco { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
