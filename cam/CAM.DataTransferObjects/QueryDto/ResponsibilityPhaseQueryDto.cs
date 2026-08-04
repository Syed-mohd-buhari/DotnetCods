using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ResponsibilityPhaseQueryDto : QueryObject
    {
        public List<string> ResponsibilityPhaseDescription { get; set; }
    }
}