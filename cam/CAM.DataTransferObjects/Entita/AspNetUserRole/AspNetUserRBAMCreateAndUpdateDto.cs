using CAM.DataTransferObjects.AbstractionLayer;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.AspNetUserRole
{
    public class AspNetUserRBAMCreateAndUpdateDto
    {
        public AspNetUserRoleUpdateDto AspNetUserRoleCreateAndUpdateDto { get; set; }
        public List<UserPrefrenceDetails> userPrefrenceDetails { get; set; }
        public int UserId { get; set; }
        public bool? Issubdomainspoc { get; set; }
        public bool? Iseduspoc { get; set; }
        public bool? Isdesigncontact { get; set; }
        public int? Subdomainresponsibleid { get; set; }
    }

    public class AspNetUserPermissionDto
    {
        public int? Roleid { get; set; }
        public string Role { get; set; }
        public int? Moduleid { get; set; }
        public string Module { get; set; }
        public short? Permissionlevel { get; set; }
    }
}
