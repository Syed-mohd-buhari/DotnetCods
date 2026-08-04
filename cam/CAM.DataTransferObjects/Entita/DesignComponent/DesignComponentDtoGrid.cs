using System;
using System.ComponentModel;
using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using ClosedXML.Excel;

namespace CAM.DataTransferObjects.Entita.DesignComponent
{
    public class DesignComponentDtoGrid : DesignComponentDto
    {
        [OrderGrid(Order = 1)]
        [DisplayName("DC Index")]
        public long DesignComponentId { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Design Component")]
        [Default]
        public string DesignComponent { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Vodafone Name")]
        [Default]
        public string VodafoneName { get; set; }
        [OrderGrid(Order = 4)]
        [DisplayName("Design Contact")]
        [Default]
        public string DesignContact { get; set; }
        [OrderGrid(Order = 5)]
        [DisplayName("DCF Index")]
        public long DesignComponentFamilyId { get; set; }

        [IgnoreGrid]
        [DisplayName("System solution")]
        public string SystemType { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("System Type Index")]
        public int SystemTypeId { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("SubNetwork boundary")]
        [Default]
        public string SubNetworkBoundary { get; set; }

        [IgnoreGrid]
        public string VodafoneNameId { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Equipment Manufacturer")]
        [Default]
        public string EquipmentManufacturer { get; set; }

        [OrderGrid(Order = 9)]
        [DisplayName("Product Name")]
        [Default]
        public string ProductName { get; set; }

        [OrderGrid(Order = 10)]
        [DisplayName("Software Version")]
        [Format(FormatType = "Text")]
        [Default]
        public string SoftwareVersion{ get; set; }
        [OrderGrid(Order = 11)]
        [DisplayName("Hardware Platform")]
        [Default]
        public string HardwarePlatform { get; set; }

        [OrderGrid(Order = 12)]
        [DisplayName("Hardware Solution")]
        [Default]
        public string HardwareSolution { get; set; }
        [OrderGrid(Order = 13)]
        [DisplayName("Hardware Type")]
        [Default]
        public string HardwareType { get; set; }
     
       [IgnoreGrid]
        public override DateTime? LastModified { get; set; }

        [DateRangeGrid]
        [OrderGrid(Order = 14)]
        [DisplayName("Last Modified")]
        [Default]
        public string LastModifiedValue { get; set; }

        [MailTo]
        [OrderGrid(Order = 15)]
        [DisplayName("Last Modified By")]
        [Default]
        public override string LastModifiedBy { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order = 16)]
        public int SystemTypeIdBasedVerticalId { get; set; }


        [DisplayName("Vertical Name")]
        [OrderGrid(Order = 17)]
        [Default]
        public string SystemTypeIdBasedVerticalValue { get; set; }


    }
}