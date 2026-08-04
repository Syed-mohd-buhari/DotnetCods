using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models.RBAC
{
    public  class AspNetUserRolePermissions : AuditableEntity
    {
        public int AspNetUserRolePermissionId { get; set; }
        public int? RoleId { get; set; }
        public int? ModuleId { get; set; }
        public short? PermissionLevel { get; set; }
      
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual AspNetModules Module { get; set; }
        public virtual AspNetRoles Role { get; set; }
    }
}
