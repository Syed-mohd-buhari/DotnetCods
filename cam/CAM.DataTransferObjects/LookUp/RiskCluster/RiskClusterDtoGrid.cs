using CAM.DataAttributes.Grid;
using CAM.Entities.Models.Base;
using System;

namespace CAM.DataTransferObjects.LookUp.RiskCluster
{
    public class RiskClusterDtoGrid 
    {
        [Default]
        public int RiskClusterId { get; set; }
        [Default]
        public string Description { get; set; }
        [Default]
        public string RiskLevel { get; set; }
        [IgnoreGrid]
        public string CreationUser { get; set; }
        [DateRangeGrid]
        [IgnoreGrid]
        public DateTime CreationDate { get; set; }
        [Default]

        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }
       
    }
}
