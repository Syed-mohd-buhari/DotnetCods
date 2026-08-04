using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.RBAC
{
    public  class AspNetModules : AuditableEntity
    {
        public AspNetModules()
        {
            AspNetUserRolePermissions = new HashSet<AspNetUserRolePermissions>();
        }

        public int AspnetModuleId { get; set; }
        public string Module { get; set; } 
        public string ModulePath { get; set; }
        public string Category { get; set; }
        public bool? IsDefault { get; set; }
        public string Menu { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<AspNetUserRolePermissions> AspNetUserRolePermissions { get; set; }
    }
}
