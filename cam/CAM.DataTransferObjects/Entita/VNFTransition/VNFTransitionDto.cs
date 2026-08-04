using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.VNFTransition
{
    public  class VNFTransitionDto :  GridDtoBase
    {

        [OrderGrid(Order = 2)]
        [DisplayName("VNF Type")]
        [Required(ErrorMessage = "VNF Type is required")]
        [Default]
        public string VnfType { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Current Release")]
        [Required(ErrorMessage = "Current Release is required")]
        [Default]
        public string CurrentRelease { get; set; }

        [OrderGrid(Order = 9)]
        [DisplayName("Planned Release")]
        [Required(ErrorMessage = "Planned Release is required")]
        public string PlannedRelease { get; set; }

        [OrderGrid(Order = 3)]
        [Required(ErrorMessage = "Element Name is required")]
        [DisplayName("Element Name")]
        [Default]
        public string ElementName { get; set; }

        [DisplayName("Spare 1")]
        [OrderGrid(Order =11)] 
        public string Spare1Json { get; set; }

        [OrderGrid(Order = 6)]
        [Required(ErrorMessage = "Location is required")]
        [Default]
        public string Location { get; set; }

        [Required(ErrorMessage = "NFVI Site Designation is required")]
        [OrderGrid(Order = 7)]
        [DisplayName("NFVI-Site Designation")]
        [Default]
        public string NfviSiteDesignation { get; set; }

        [IgnoreGrid]
        public DateTime? LastModified { get; set; }

        [DateRangeGrid]
        [DisplayName("Last Modified")]
        [OrderGrid(Order = 12)]
        public string LastModifiedValue { get; set; }

        [OrderGrid(Order = 13)]
        [MailTo]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }
    }
}
