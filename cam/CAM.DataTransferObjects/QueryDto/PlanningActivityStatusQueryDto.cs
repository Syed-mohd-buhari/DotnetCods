using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class PlanningActivityStatusQueryDto : QueryObject
    {
        public List<string> PlanningActivityStatusDescription { get; set; }
    }
}