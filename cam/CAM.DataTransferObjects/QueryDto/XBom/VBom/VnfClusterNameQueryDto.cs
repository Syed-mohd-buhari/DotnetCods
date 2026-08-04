using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.VBom
{
    public class VnfClusterNameQueryDto : QueryObject
    {
        public List<long> Clusternameid { get; set; }
        public List<string> Clusterdescription { get; set; }
        public List<short> Clustertype { get; set; }

    }
}
