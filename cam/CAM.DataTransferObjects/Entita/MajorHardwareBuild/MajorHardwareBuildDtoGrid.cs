using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.MajorHardwareBuild
{
    public class MajorHardwareBuildDtoGrid : MajorHardwareBuildDto
    {

        [OrderGrid(Order = 1)]
        [DisplayName("Equipment Manufacturer")]
        [Default]
        public string OriginalEquipmentManufacturer { get; set; }

        [OrderGrid(Order = 2)]
        [Default]
        public string Platform { get; set; }

        [OrderGrid(Order = 3)]
        [DisplayName("HW Type")]
        [Default]
        public string HardwareType { get; set; }

        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Design Contact")]

        public string DesignContact { get; set; }
        //[IgnoreGrid]
        [OrderGrid(Order = 5)]
        [DisplayName("Hardware Index")]
        //[Default]
        public long MajorHardwareBuildId { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("Build Construction")]
        [Default]
        public string BuildConstruction { get; set; }

        [OrderGrid(Order = 9)]
        [DisplayName("EOM")]
        [Default]
        [DateRangeGrid]
        public string EndOfMaintenanceValue { get; set; }

        [OrderGrid(Order = 10)]
        [DateRangeGrid]
        [DisplayName("GA Date")]
        [Default]
        public string GeneraAvailableDateValue { get; set; }

        [OrderGrid(Order = 11)]
        [DateRangeGrid]
        [DisplayName("EOS")]
        [Default]
        public string EndOfsupportValue { get; set; }

        [OrderGrid(Order = 15)]
        [DateRangeGrid]
        [Default]
        [DisplayName("Last Modified")]
        public string LastModifiedValue { get; set; }

        [OrderGrid(Order = 12)]
        [DateRangeGrid]
        [DisplayName("LTB New")]
        public string LastTimeBuyNewValue { get; set; }
        [DateRangeGrid]
        [OrderGrid(Order = 13)]
        [DisplayName("LTB Upgrades")]
        public string LastTimeBuyUpgradesValue { get; set; }
        [DateRangeGrid]
        [OrderGrid(Order = 14)]
        [DisplayName("LTB Expansions")]
        public string LastTimeBuyExpansionsValue { get; set; }

        [DisplayName("Description")]
        [OrderGrid(Order = 18)]
        public string Description { get; set; }

    }
}