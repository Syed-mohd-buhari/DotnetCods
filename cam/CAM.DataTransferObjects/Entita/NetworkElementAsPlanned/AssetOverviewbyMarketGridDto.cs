using CAM.DataAttributes.Grid;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.NetworkElementAsPlanned
{
    public class AssetOverviewbyMarketMappingDto
    {       
        [OrderGrid(Order = 1)]
        public string CeFunction { get; set; }
        
        [OrderGrid(Order = 2)]
        public string Vendor { get; set; }

        [OrderGrid(Order = 3)]
        public string NeInstances { get; set; }  

        [OrderGrid(Order = 4)]
        public int NetworkElementCount { get; set; }

    }
    public class AssetOverviewbyMarketGridDto
    {
        [IgnoreGrid]
        public short SubDomainResponseCeFunctionId { get; set; }

        [OrderGrid(Order = 1)]
        public string SubDomainResponseCeFunction { get; set; }

        [IgnoreGrid]
        public short OemVendorId { get; set; }
        [OrderGrid(Order = 2)]
        [DisplayName("Vendor")]
        public string OemVendor { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Product Name")]
        public string ProductNameNeInstances { get; set; }

        [IgnoreGrid]
        public long NetworkElementsAsPlannedId { get; set; }

        [OrderGrid(Order = 4)]
        public string NetworkElementsPlannedName { get; set; }

        [IgnoreGrid]
        public short OpcoId { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Opco")]
        public string OpCoDescrption { get; set; }
        [IgnoreGrid]
        public short VerticalId { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("Vertical")]
        public string VerticalDescrption { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("Nodes")]
        public int NetworkElementCount { get; set; }

        [IgnoreGrid]
        public long HWBuildConsId { get; set; }
        [OrderGrid(Order = 8)]
        [DisplayName("Build Construction")]
        public string HWBuildCons { get; set; }
        [IgnoreGrid]
        public long SupportServiceId { get; set; }
        [OrderGrid(Order = 9)]
        [DisplayName("Supported Services")]
        public string SupportService { get; set; }
        [IgnoreGrid]
        public long? DCFId { get; set; }
        [OrderGrid(Order = 10)]
        [DisplayName("DCF Name")]
        public string DCFDescription { get; set; }
        [IgnoreGrid]
        public long Environmentid { get; set; }
        [OrderGrid(Order = 11)]
        [DisplayName("Environment")]
        public string Environment { get; set; }

        [IgnoreGrid]
        public long SystemTypeId{ get; set; }
        [OrderGrid(Order = 12)]
        [DisplayName("System Type")]
        public string SystemTypeDescription { get; set; }
    }    
}
