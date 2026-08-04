using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class SystemSolutionQueryDto : QueryObject
    {
        public List<string> SystemSolutionDescription { get; set; }
    }
}