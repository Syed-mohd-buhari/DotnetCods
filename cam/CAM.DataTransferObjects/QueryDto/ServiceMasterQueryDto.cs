using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ServiceMasterQueryDto : QueryObject
    {
        public List<int> ServiceMasterIndex { get; set; }
        public List<string> Description { get; set; }  
    }
}
