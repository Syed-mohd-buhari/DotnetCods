using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.DataTransferObjects.Entita.Organisation;
using OracleModels.DBModels;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.Aspnetuserrole
{
    public class AspNetUserRoleCreateOrUpdateDto : AspnetuserroleGridDto
    {
        public IDictionary<short, string>? OpCoResource { get; set; }
        public IDictionary<int, string>? VerticalResource { get; set; }
        public IDictionary<int, string>? RoleResource { get; set; }
        public IDictionary<int, string>? RoleDescriptionResource { get; set; }
        public IDictionary<long, string>? VerticalResponcibleResource { get; set; }

        public OrganisationCreateDto OrganisationResourecs { get; set; }
        public List<Aspnetuserrolepermissions> AspnetuserrolepermissionsResources { get; set; }
        public List<OrganisatioDtoGrid> OrganisatioDtoGrids { get; set; }
        public IDictionary<int, string>? SubdomainResbonsibleResource { get; set; }
    }
}
