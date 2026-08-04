using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CAM.Identity
{
    public class ApplicationVerticalRes : IdentityUserRole<int>
    {        
        public int? VerticalResId { get; set; }
        public string VerticalRes { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRole { get; set; }
       
    }
}
