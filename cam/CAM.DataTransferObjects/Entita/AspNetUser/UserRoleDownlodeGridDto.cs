using CAM.DataAttributes.Grid;
using System;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.AspNetUser
{
    public class UserRoleDownlodeGridDto
    {
        [Default]
        [DisplayName("User Index")]
        public int UserId { get; set; }
        [Default]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Default]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Default]
        [DisplayName("Active")]
        public bool Active { get; set; }
        [DisplayName("OpCo")]
        [Default]
        public string OpCo { get; set; }
        [Default]
        [DisplayName("Role")]
        public string Role { get; set; }
        [Default]
        [DisplayName("Vertical")]
        public string Vertical { get; set; }
        [Default]
        [DisplayName("Vertical Responsible")]
        public string VerticalResponsible { get; set; }
        [Default]
        [DisplayName("Main Organisation")]
        public string MainOrganisation { get; set; }
        [Default]
        [DisplayName("Practice")]
        public string Practice { get; set; }
        [Default]
        [DisplayName("Subdomain Responsible")]
        public string SubdomainResponsible { get; set; }
        [Default]
        [DisplayName("Practice Contact")]
        public string PracticeContact { get; set; }
        [Default]
        [DisplayName("Is Subdomain Spoc")]
        public bool? IsSubDomainSpoc { get; set; }
        [Default]
        [DisplayName("Is Edu Spoc")]
        public bool? IsEduSpoc { get; set; }
        [Default]
        [DisplayName("Is DesignContact")]
        public bool? IsDesigncontact { get; set; }

        [DisplayName("Last Modified Date")]
        [Default]
        public virtual DateTime? LastModified { get; set; }
      
        [Default]
        [DisplayName("Last Modified By")]
        public virtual string LastModifiedBy { get; set; }

    }
}
