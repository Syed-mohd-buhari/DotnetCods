using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.CnfCluster
{
    public class CnfClusterDtoGrid
    {
        [Default]
        public long CnfClusterId { get; set; }
        [Default]
        public string CnfClusterName { get; set; }
        [Default]
        public string NodePool { get; set; }
        [Default]
        public string AlaisName { get; set; }
        [Default]
        public string CnfName { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }

    }
}
