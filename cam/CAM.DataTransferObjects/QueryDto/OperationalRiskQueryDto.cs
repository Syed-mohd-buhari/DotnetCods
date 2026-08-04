using System.Collections.Generic;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.QueryDto
{
    public class OperationalRiskQueryDto : QueryObject
    {
        public List<short> Id { get; set; }
        public List<int> Severity { get; set; }
        public List<string> Description { get; set; }

    }
}