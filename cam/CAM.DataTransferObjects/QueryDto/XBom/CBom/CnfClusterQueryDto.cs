using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.CBom
{
    public class CnfClusterQueryDto : QueryObject
    {
        public List<long> Cnfclusterid { get; set; }
        public List<string> Cnfclustername { get; set; }
        public List<string> Nodepool { get; set; }
        public List<string> Alaisname { get; set; }
        public List<string> Cnfname { get; set; }

    }
}
