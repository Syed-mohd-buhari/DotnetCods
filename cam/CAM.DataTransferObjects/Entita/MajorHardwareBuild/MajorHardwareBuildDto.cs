using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Enum;

namespace CAM.DataTransferObjects.Entita.MajorHardwareBuild
{
    public abstract class MajorHardwareBuildDto : GridDtoBase
    {
        [DisplayName("HW Solution")]
        [OrderGrid(Order = 7)]
        [Default]
        public string HardwareSolution { get; set; }


        [OrderGrid(Order = 8)]
        [DisplayName("Other HW Info")]
        public string OtherHardwareInfo { get; set; }

       [IgnoreGrid]
        public DateTime? GeneraAvailableDate { get; set; }

        [IgnoreGrid]
        public DateTime? EndOfsupport { get; set; }

        [IgnoreGrid]
        public DateTime? LastTimeBuyNew { get; set; }

        [IgnoreGrid]
        public DateTime? LastTimeBuyUpgrades { get; set; }

        [IgnoreGrid]
        public DateTime? LastTimeBuyExpansions { get; set; }
     
        [OrderGrid(Order = 17)]
        [DisplayName("Vulnerability Status")]
        public string VulnerabilityStatus { get; set; }
 
        [IgnoreGrid]
        public override DateTime? LastModified { get; set; }

        [OrderGrid(Order = 16)]
        [MailTo]
        [DisplayName("Last Modified By")]
        public override string LastModifiedBy { get; set; }


        [IgnoreGrid]
        [Required(ErrorMessage = "Proprietary Hardware is required")]
        public bool ProprietaryHardware { get; set; }
        [IgnoreGrid]
        public string SpareFieldsJson { get; set; }

        [IgnoreGrid]
        public DateTime? EndOfMaintenance { get; set; }

        [IgnoreGrid]
        public EOMEnum EOMStatus { get; set; }


    }
}