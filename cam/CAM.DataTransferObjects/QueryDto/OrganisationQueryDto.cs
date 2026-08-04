using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class OrganisationQueryDto : QueryObject
    {
        public List<long> OrganisationId { get; set; }
        public List<int> MainOrganisationId { get; set; }
        public List<string> MainOrganisation { get; set; }
        public List<string> Practice { get; set; }
        public List<string> PracticeContact { get; set; }
        //public List<short> OpCo { get; set; }
        public List<int> VerticalResponsible { get; set; }
        //public List<int> SubdomainResponsibleId { get; set; }
        //public List<int> SubdomainResponsible { get; set; }
        //public List<int> ContactId { get; set; }
        //public List<string> Contact { get; set; }

        //public List<bool> IsSubDomainSpoc { get; set; }

        //public List<bool> IsEduSpoc { get; set; }
    }
}
