using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AspnetuserroleQueryDto : QueryObject
    {
        public List<decimal> AspNetUserRoleId { get; set; }
        public List<int> UserId { get; set; }
        public List<int> RoleId { get; set; }
        public List<string> UserName { get; set; }
        public List<string> Email { get; set; }
        public List<bool> Active { get; set; }

        public List<int> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<int> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }
        public List<short> OpcoId { get; set; }
        public List<int> VerticalResponsibleid { get; set; }
        public List<string> Opco { get; set; }
        public List<string> Role { get; set; }
        public List<string> Vertical { get; set; }
        public List<string> VerticalResponsible { get; set; }

        public List<int> MainOrganisation {  get; set; }
        public List<int> Practice { get; set; }
        public List<int> PracticeContact { get; set; }
        public List<int> SubdomainResponsible { get; set; }

        public List<bool> IsSubDomainSpoc { get; set; }

        public List<bool> IsEduSpoc { get; set; }

        public List<bool> IsDesigncontact { get; set; }

    }
}
