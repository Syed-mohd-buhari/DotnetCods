using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.LookUp.VodafoneName
{
    public class VodafoneNameDtoGrid:VodafoneNameDto
    {
        [DateRangeGrid]
        [OrderGrid(Order = 6)]
        [DisplayName("Last Modified Date")]
        [Default]
        public string LastModifiedValue { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order = 7)]
        public DateTime? LastModified { get; set; }
        [OrderGrid(Order = 4)]
        public string RiskCluster { get; set; }

        [OrderGrid(Order = 5)]
        public string RiskLevel { get; set; }

        [IgnoreGrid]
        public Dictionary<int,string>RiskClusterResource { get; set; }
        [IgnoreGrid]
        public Dictionary<int, string> RiskClusterSeverityResource { get; set; }

    }
    public class VodafoneNameDto : GridDtoBase
    {
        [OrderGrid(Order = 1)]  
        [DisplayName("ID")]
        [Default]
        public int Id { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Description")]
        [Default]
        public string Description { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Product Name")]
        [Default]
        public string ProductName { get; set; }
        [IgnoreGrid]
        [Ignore]
        public List<decimal> ProductNameIds { get; set; }
        [IgnoreGrid]
        public int RiskClusterId { get; set; }
        [IgnoreGrid]
        public int RiskClusterVodafoneNameMapId { get; set; }

    }
  
}
