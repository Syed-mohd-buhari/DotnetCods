using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.CBom
{
    public class PodTypeInfoQueryDto : QueryObject
    {
        public List<long> Podtypeinfoid { get; set; }
        public List<string> Podtypeinfoname { get; set; }
        public List<string> Podroledescription { get; set; }

    }
}
