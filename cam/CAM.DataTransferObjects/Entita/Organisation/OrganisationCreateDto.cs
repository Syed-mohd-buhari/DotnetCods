using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.Organisation
{
    public class OrganisationCreateDto : OrganisatioDtoGrid
    {
        public IDictionary<int, string> MainOrganisations { get; set; }

        public IDictionary<int?, string> PracticeContacts { get; set; }

        public IDictionary<int, string> Practices { get; set; }

        public IDictionary<int, string> Contacts { get; set; }

        //public IDictionary<int, string> SubDomainResponsibles { get; set; }

        public IDictionary<int, int?> PracticeMethods { get; set; }
        public IDictionary<int?, string> VerticalResponsible { get; set; }
         

    }
}
