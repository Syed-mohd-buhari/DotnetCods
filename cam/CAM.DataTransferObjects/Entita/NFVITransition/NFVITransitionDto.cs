using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.NFVITransition
{
    public class NFVITransitionDto : GridDtoBase
    {
       
        [Required(ErrorMessage = "NFVI Site Designation is required")]
        [OrderGrid(Order = 2)]
        [DisplayName("NFVI-Site Designation")]
        [Default]
        public string NfviSiteDesignation { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Next Step")]
        [Default]
        public string NextStep { get; set; }

        //[OrderGrid(Order = 7)]
        //[DisplayName("Status: 12K Switch")]
        //public string Status12KSwitch { get; set; }

        [DisplayName("Spare 1")]
        [OrderGrid(Order = 9)] public string Spare1Json { get; set; }

        [IgnoreGrid]
        public DateTime? LastModified { get; set; }

        [DateRangeGrid]
        [DisplayName("Last Modified")]
        [OrderGrid(Order = 10)]
        public string LastModifiedValue { get; set; }

        [OrderGrid(Order = 11)]
        [MailTo]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }
    }
}
