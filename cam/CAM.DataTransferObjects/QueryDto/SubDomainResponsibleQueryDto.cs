using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class SubDomainResponsibleQueryDto : QueryObject
    {
        public List<string> SubDomainResponsibleDescription { get; set; }
    }
}