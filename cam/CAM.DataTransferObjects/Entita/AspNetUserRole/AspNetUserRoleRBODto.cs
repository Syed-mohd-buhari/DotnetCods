using CAM.DataTransferObjects.FunctionalityDto;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.AspNetUserRole
{
    public class AspNetUserRoleRBODto
    {
        public List<short> OpcoDetails { get; set; }
        public List<int> VerticalDetails { get; set; }
        public int? SubdomainDetails { get; set; }
        public List<int> UserRoleId { get; set; }
        public List<RoleDto> RoleRecords { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsManager { get; set; } = false;
        public bool IsKpiAdmin { get; set; } = false;
        public bool IsKpiUser { get; set; } = false;
        public List<string> OpcoDescription { get; set; }
    }

    public class OrganisatioinSpocsDto
    {
        public bool? IsEduSpoc { get; set; }
        public bool? IsSubdomainSpoc { get; set; }

        public bool IsUserExistInOrg { get; set; } = false;
    }
    public class RoleDto
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long? PortalRoleId { get; set; }
    }
}
