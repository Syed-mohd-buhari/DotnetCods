using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace CAM.Identity
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public short? OpCoId { get; set; }
        public int? VerticalResId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual ApplicationOpco OpCo {get; set;}
        public virtual ApplicationVerticalRes VerticalRes { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
       
    }
}
