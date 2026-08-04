using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.ComponentModel;

namespace CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes
{
    public class ProductNameDtoGrid : ProductNameDto
    {

        [DateRangeGrid]
        [OrderGrid(Order = 4)]
        [Default]
        public DateTime? LastModified { get; set; }

        [OrderGrid(Order = 5)]
        [MailTo]
        [Default]
        public string LastModifiedBy { get; set; }


    }
    public class ProductNameDto : GridDtoBase
    {
        [OrderGrid(Order = 1)]
        [DisplayName("ID")]
        [Default]
        public decimal Id { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Description")]
        [Default]
        public string Description { get; set; }
        [IgnoreGrid]
        public int? VodafoneNameId { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        public string VodafoneName { get;set; }


        [OrderGrid(Order = 4)]
        [DisplayName("IsPlatformSoftware")]
        [Default]
        public string IsPlatformSoftware { get; set; }
    }

}