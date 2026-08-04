using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class OperatingSystemQueryDto : QueryObject
    {
        public List<short> OperatingSystemId { get; set; }
        public List<string> OperatingSystemName { get; set; }
    }
}
