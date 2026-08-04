using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CAM.Identity
{
    public class ApplicationRole : IdentityRole<int>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public int RoleId { get; set; }
        public string RoleName { get; set; }  
        public string Description { get; set; }
    }
}
