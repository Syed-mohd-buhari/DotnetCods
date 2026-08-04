using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class VerticalResponsibleQueryDto : QueryObject
    {
        public List<string> VerticalResponsibleDescription { get; set; }
    }
}