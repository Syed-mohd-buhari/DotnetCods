using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.CBom
{
    public class CnfNameQueryDto : QueryObject
    {
        public List<long> Cnfnameid { get; set; }
        public List<string> Cnfdescription { get; set; }
        public List<string> Product { get; set; }

    }
}
