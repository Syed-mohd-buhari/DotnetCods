using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.BundleUpgradeInitiative
{
    public  class BundleUpgradeInitiativeDto: GridDtoBase
    {
        //[IgnoreGrid]
        //public long BundleUpgradeInitiativeId { get; set; }
        //[OrderGrid(Order = 3)]
        //public short OriginalEquipmentManufacturerId { get; set; }
        [Required(ErrorMessage = "VNF Type is required")]
        [OrderGrid(Order = 4)]
        [DisplayName("VNF Type")]
        [Default]
        public string VnfType { get; set; }

        [Required(ErrorMessage = "Vertical Owner is required")]
        [OrderGrid(Order = 1)]
        [DisplayName("Vertical Owner")]
        [Default]
        public string VerticalOwner { get; set; }
        [OrderGrid(Order = 3)]
        [DisplayName("OEM Certified Release")]
        [Default]
        public string OemCertifiedRelease { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        public string Remarks { get; set; }
        [OrderGrid(Order = 6)]
        [DisplayName("Spare 1")]

        public string Spare1Json { get; set; }

       [IgnoreGrid]
        public DateTime? LastModified { get; set; }

        [DateRangeGrid]
        [OrderGrid(Order = 7)]
        [DisplayName("Last Modified")]
        [Default]

        public string LastModifiedValue { get; set; }

        [OrderGrid(Order = 8)]
        [MailTo]
        [DisplayName("Last Modified By")]
        [Default]
        public string LastModifiedBy { get; set; }
    }
}
