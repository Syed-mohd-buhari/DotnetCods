using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AssetHardwareAncillaryQueryDto : QueryObject
    {

        public List<long> AssetHardwareAncillaryId { get; set; }  

        public List<long> ElementName { get; set; }
     
        public List<long> MajorHardwareName { get; set; } 

        public List<long> DataCeterName { get; set; }
     
        public List<long> ClusterNameDesc { get; set; } 

        public List<long> AssetClusterTypeDesc { get; set; }
        public List<long> AssetClusterDesc { get; set; }
    }
}
