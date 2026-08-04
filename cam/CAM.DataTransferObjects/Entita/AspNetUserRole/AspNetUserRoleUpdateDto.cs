using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.AspNetUserRole
{
    public class AspNetUserRoleUpdateDto :AspnetuserroleGridDto
    {
        public  long AspNetUserRoleId { get; set; }
        public short? OpcoId { get; set; }
        public int? VerticalResponsibleId { get; set; }
        public int RoleId { get; set; }

        public string OpCoIds { get; set; }
        public string VerticalResponsibleIds { get; set; }
        public string RoleIds { get; set; }
        public string OrganisationIds { get; set; }
        public string RestrictedOpcoIds { get; set; }
    }

    public class OrgAndVeicalDto
    {
        public int? VerticalResponsibleId { get; set; }
        public long OrganisationId { get; set; }
    }

    public class UserDoaminAndRoleDetails
    {
        public List<short> OpcoIds { get; set;}
        public List<long> OrgVerticalIds { get; set; }
        public List<int> RoleIds { get; set; }
        public List<short> RestrictedOpcoIds { get; set;}
        public List<long> VerticalResponcibleIds { get; set; }
    }
}
