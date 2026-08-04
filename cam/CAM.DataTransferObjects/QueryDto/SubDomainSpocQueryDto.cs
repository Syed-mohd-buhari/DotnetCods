using CAM.DataTransferObjects.QueryDto.Base;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{
    public class SubDomainSpocQueryDto: QueryObject
    {
        public List<bool> IsSubDomain { get; set; }
        public List<bool> IsEdu { get; set; }
        public List<short> Id { get; set; }
        public List<string> Description { get; set; }
    }
}
