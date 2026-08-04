using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.VNFCluster
{
    public class VnfCluserNameDtoGrid
    {
        [Default]
        public long ClusterNameId { get; set; }
        [Default]
        public string ClusterDescription { get; set; }
        [Default]
        public short ClusterType { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }

    }
}
