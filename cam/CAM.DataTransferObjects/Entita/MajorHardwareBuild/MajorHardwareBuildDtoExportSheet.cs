using CAM.DataAttributes.Grid;
using CAM.Enum;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CAM.DataTransferObjects.Entita.MajorHardwareBuild
{
    public class MajorHardwareBuildDtoExportSheet
    {
        [IgnoreGrid]
        public bool? Deleted { get; set; }
        [IgnoreGrid]
        public bool? Orphan { get; set; }

        [DisplayName("HW Solution")]
        [OrderGrid(Order = 6)]
        [Default]
        public string HardwareSolution { get; set; }


        [OrderGrid(Order = 7)]
        [DisplayName("Other HW Info")]
        public string OtherHardwareInfo { get; set; }

        //date
        [OrderGrid(Order = 9)]
        [DisplayName("GA Date")]
        [Default]
        public string GeneraAvailableDate { get; set; }

        //date
        [OrderGrid(Order = 10)]
        [DisplayName("EOS Date")]
        [Default]
        public string EndOfsupport { get; set; }

        //date
        [OrderGrid(Order = 11)]
        [DisplayName("LTB New Date")]
        public string LastTimeBuyNew { get; set; }

        //date
        [OrderGrid(Order = 12)]
        [DisplayName("LTB Upgrades Date")]
        public string LastTimeBuyUpgrades { get; set; }

        //date
        [OrderGrid(Order = 13)]
        [DisplayName("LTB Expansions Date")]
        public string LastTimeBuyExpansions { get; set; }

        [OrderGrid(Order = 14)]
        [DisplayName("Vulnerability Status")]
        public string VulnerabilityStatus { get; set; }

        //date
        [OrderGrid(Order = 15)]
        [DisplayName("Last Modified Date")]
        public string LastModifiedDate { get; set; }

        [OrderGrid(Order = 16)]
        [MailTo]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }

        [IgnoreGrid]
        [Required(ErrorMessage = "Proprietary Hardware is required")]
        public bool ProprietaryHardware { get; set; }
        [IgnoreGrid]
        public string SpareFieldsJson { get; set; }

        [IgnoreGrid]
        public EOMEnum EOMStatus { get; set; }

        [OrderGrid(Order = 1)]
        [DisplayName("Hardware Index")]
        public long MajorHardwareBuildId { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Equipment Manufacturer")]
        [Default]
        public string OriginalEquipmentManufacturer { get; set; }

        [OrderGrid(Order = 3)]
        [Default]
        public string Platform { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("HW Type")]
        [Default]
        public string HardwareType { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Build Construction")]
        [Default]
        public string BuildConstruction { get; set; }

        //date
        [OrderGrid(Order = 8)]
        [DisplayName("EOM Date")]
        [Default]
        public string EndOfMaintenanceValue { get; set; }
    }
}
