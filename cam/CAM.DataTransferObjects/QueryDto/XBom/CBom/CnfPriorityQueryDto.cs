using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.CBom
{
    public class CnfPriorityQueryDto : QueryObject
    {
        public List<long> Cnfpriorityid { get; set; }
        public List<string> Description { get; set; }
    }
}
