using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.Enum;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CAM.DataTransferObjects.Entita.MajorSoftwareBuild
{
    public class MajorSoftwareBuildDtoExportSheet
    {
        [OrderGrid(Order = 1)]
        [DisplayName("Software Index")]

        public long MajorSoftwareBuildId { get; set; }
        [OrderGrid(Order = 2)]
        [DisplayName("Equipment Manufacturer")]
        [Default]
        public string OriginalEquipmentManufacturer { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("EOM")]
        [Default]
        [DateRangeGrid]
        public string EndOfMaintenanceValue { get; set; }
        [OrderGrid(Order = 10)]
        [DisplayName("Vulnerability Status")]
        public string VulnerabilityStatus { get; set; }
        [IgnoreGrid]
        [DisplayName("Third Party SW Components")]
        public IDictionary<long, string> ThirdPartySoftwareComponents { get; set; }
        [OrderGrid(Order = 11)]
        [DisplayName("OS")]
        public string OperatingSystem { get; set; }

        [OrderGrid(Order = 12)]
        [DisplayName("Software Family")]
        [Default]
        public string CriticalAssetType { get; set; }
        

        [OrderGrid(Order = 3)]
        [DisplayName("Product Name")]
        [Default]
        public string ProductName { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Software Version")]
        [Required(ErrorMessage = "Software Application is required")]
        [FormatClosetXml(Type = XLDataType.Text)]
        [Format(FormatType = "Text")]
        [Default]
        public string SoftwareVersion { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Delivery Method")]
        [Default]
        public string DeliveryMethod { get; set; }

        [OrderGrid(Order = 7)]
        [DateRangeGrid]
        [DisplayName("GA Date")]
        [Default]
        public DateTime? GeneraAvailableDate { get; set; }

        [OrderGrid(Order = 8)]
        [DateRangeGrid]
        [DisplayName("LTB New")]
        public DateTime? LastTimeBuyNew { get; set; }

        [OrderGrid(Order = 9)]
        [DateRangeGrid]
        [DisplayName("EOS")]
        [Default]
        public DateTime? EndOfsupport { get; set; }


        [OrderGrid(Order = 13)]
        [DateRangeGrid]
        [DisplayName("Last Modified")]
        [Default]
        public new DateTime? LastModified { get; set; }

        [OrderGrid(Order = 14)]
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

        [IgnoreGrid]
        public bool? Deleted { get; set; }
        [IgnoreGrid]
        public bool? Orphan { get; set; }
    }
}
