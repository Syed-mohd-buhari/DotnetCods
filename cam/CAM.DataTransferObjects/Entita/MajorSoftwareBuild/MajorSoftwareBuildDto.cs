using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Enum;
using ClosedXML.Excel;

namespace CAM.DataTransferObjects.Entita.MajorSoftwareBuild
{
    public abstract class MajorSoftwareBuildDto : GridDtoBase
    {
        [OrderGrid(Order = 2)]
        [DisplayName("Product Name")]
        [Default]
        public string ProductName { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("Software Version")]
        [Required(ErrorMessage = "Software Version is required")]
        [FormatClosetXml(Type = XLDataType.Text)]
        [Format(FormatType = "Text")]
        [Default]
        public string SoftwareVersion { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Design Contact")]
        public string DesignContact { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Delivery Method")]
        [Default]
        public string DeliveryMethod { get; set; }

        [IgnoreGrid]
        public DateTime? GeneraAvailableDate { get; set; }

        [OrderGrid(Order = 9)]
        [DateRangeGrid]
        [DisplayName("LTB New")]
        public DateTime? LastTimeBuyNew { get; set; }

        [IgnoreGrid]
        public DateTime? EndOfsupport { get; set; }


        [IgnoreGrid]
        public new DateTime? LastModified { get; set; }

        [OrderGrid(Order = 19)]
        [MailTo]
        [DisplayName("Last Modified By")]
        public new string LastModifiedBy { get; set; }

        [DateRangeGrid]
        [DisplayName("LTB Upgrades")]
        [IgnoreGrid]
        public DateTime? LastTimeBuyUpgrades { get; set; }
        
        [DateRangeGrid]
        [IgnoreGrid]
        [DisplayName("LTB Expansions")]
        public DateTime? LastTimeBuyExpansions { get; set; }

        [IgnoreGrid]
        public DateTime? EndOfMaintenance { get; set; }
        
        [IgnoreGrid]
        public EOMEnum EOMStatus { get; set; }

        [IgnoreGrid]
        public string SpareFieldsJSON { get; set; }

    }
}