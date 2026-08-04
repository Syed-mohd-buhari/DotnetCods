using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.CBom
{
    public class CnfHardwareTypeQueryDto : QueryObject
    {
        public List<long> Cnfhardwareid { get; set; }
        public List<string> Description { get; set; }
    }
}
