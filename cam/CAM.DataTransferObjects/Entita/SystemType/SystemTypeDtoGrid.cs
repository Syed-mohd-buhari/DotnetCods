using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.SystemType
{
    public class SystemTypeDtoGrid : SystemTypeDto
    {
        [TabGrid(TabName = "Export")]
        [DisplayName("Major HW")]
        [OrderGrid(Order = 1)]
        [Default]
        public IDictionary<long, string> MajorHardwareBuild { get; set; }


        [TabGrid(TabName = "Export")]
        [DisplayName("Major SW")]
        [OrderGrid(Order = 2)]
        [Default]
        public string MajorSoftwareBuild { get; set; }



        [TabGrid(TabName = "Export")]
        [DisplayName("Hardware Index")]
        [OrderGrid(Order = 11)]
        public long? MajorHardwareBuildId { get; set; }


        [TabGrid(TabName = "Export")]
        [DisplayName("Software Index")]
        [OrderGrid(Order = 12)]
        public long? MajorSoftwareBuildId { get; set; }


        [TabGrid(TabName = "Export")]
        [DisplayName("Product Importance")]
        [OrderGrid(Order = 16)]
        [Default]
        public string ProductImportance { get; set; }



        [TabGrid(TabName = "Export")]
        [DisplayName("Asset Class")]
        [OrderGrid(Order = 17)]
        [IgnoreGrid]
        public string AssetClass { get; set; }

        [TabGrid(TabName = "Export")]
        [DisplayName("Asset Category")]
        [OrderGrid(Order = 18)]
        [Default]
        public string AssetCategory { get; set; }


        [TabGrid(TabName = "Export")]
        [DisplayName("System Type Index")]
        [OrderGrid(Order = 20)]
        public long SystemTypeId { get; set; }

        [DateRangeGrid]
        [TabGrid(TabName = "Export")]
        [DisplayName("Last Modified")]
        [OrderGrid(Order = 25)]
        public string LastModifiedValue { get; set; }


        //[TabGrid(TabName = "Export")]
        //[DisplayName("Vertical Res.")]
        //[OrderGrid(Order = 18)]
        //[Default]
        //public string VerticalResponsible { get; set; }

        //[TabGrid(TabName = "Export")]
        //[DisplayName("Sub-Domain Resp.")]
        //[OrderGrid(Order = 14)]
        //[Default]
        //public string SubDomainResponsible { get; set; }


        [TabGrid(TabName = "Export")]
        [DisplayName("Oem SW")]
        [IgnoreGrid]
        public string SoftwareOem { get; set; }

        [TabGrid(TabName = "Export")]
        [DisplayName("Oem HW")]
        [IgnoreGrid]
        public string HardwareOem { get; set; }

        [IgnoreGrid]
        public string MajorHardwareBuildWithoutOem { get; set; }


        [IgnoreGrid]
        public string EndOfMaintenanceValue { get; set; }

    }
}