using System.ComponentModel;
using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using ClosedXML.Excel;

namespace CAM.DataTransferObjects.Entita.DesignComponentFamily
{
    public class DesignComponentFamilyDtoGrid : DesignComponentFamilyDto
    {
        [DisplayName("DCF Index")]
        [OrderGrid(Order = 1)]
        [Default]
        public long DesignComponentFamilyId { get; set; }

        [DisplayName("DCF Name")]
        [OrderGrid(Order = 2)]
        [Default]
        public string DesignComponentFamilyName { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Vodafone Name")]
        [Default]
        public string VodafoneName { get; set; }

        [DisplayName("SubNetwork Boundary")]
        [OrderGrid(Order = 6)]
        [Default]
        public string SubNetworkBoundary { get; set; }

        [DisplayName("Supported Services")]
        [OrderGrid(Order = 7)]
        [Default]
        public string SupportedServices { get; set; }



        [IgnoreGrid]
        public int? VodafoneNameId { get; set; }

        [DisplayName("Sharing Type")]
        [OrderGrid(Order = 8)]
        public string SharingType { get; set; }


        [DisplayName("Vertical Name")]
        [OrderGrid(Order = 11)]
        [Default]
        public string SystemTypeIdBasedVerticalValue { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order = 12)]
        public int SystemTypeIdBasedVerticalId { get; set; }
    }
}