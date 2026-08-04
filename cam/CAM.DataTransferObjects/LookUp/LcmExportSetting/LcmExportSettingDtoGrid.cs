using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.LcmExportSetting
{
    public class LcmExportSettingDtoGrid : LcmExportSettingDto
    {

        [OrderGrid(Order = 3)]
        [DisplayName("LCM Historical Info SW")]
        [Default]
        public string LcmHistoricalInfoSW { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("LCM Historical Info HW")]
        [Default]
        public string LcmHistoricalInfoHW { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Is Historical")]
        [Default]
        public bool? IsHistorical { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("Is Default")]
        [Default]
        public bool? IsDefault { get; set; }

        [IgnoreGrid]
        public int? Reportlevel { get; set; }

        [IgnoreGrid]
        public int? ReportType { get; set; }

        [DateRangeGrid]
        [OrderGrid(Order = 6)]
        [DisplayName("Last Modified Date")]
        [Default]
        public string LastModifiedValue { get; set; }

    }
    public class LcmExportSettingDto : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [DisplayName("ID")]
        [Default]
        public int Id { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Description")]
        [Default]
        public string Description { get; set; }
    }
}