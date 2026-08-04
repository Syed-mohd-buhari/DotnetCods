using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.LcmEngineering
{
    public abstract class LcmEngineeringDto : GridDtoBase
    {

        [IgnoreGrid]
        public bool ElementCount { get; set; }

        [IgnoreGrid]
        public string HardwareSheetIndex { get; set; }


        [IgnoreGrid]
        public string SoftwareSheetIndex { get; set; }



        [IgnoreGrid]
        public bool OnSoftware { get; set; }


        [IgnoreGrid]
        public bool OnHardware { get; set; }


        [OrderGrid(Order = 3)]
        [Default]
        [DisplayName("Resource Key")]
        public string ResourceKey { get; set; }


        [OrderGrid(Order = 8)]
        [DisplayName("N° Nodes In Prod")]
        [Default]
        public int? NumberOfNodes { get; set; }


        [OrderGrid(Order = 9)]
        [DisplayName("N° Nodes In Lab")]
        [Default]
        public int? NumberOfNodesInLab { get; set; }



        [OrderGrid(Order = 10)]
        [DisplayName("SW In Warranty")]
        [Default]
        public bool Warranty { get; set; }

        [OrderGrid(Order = 12)]
        [DisplayName("Comment On Project Status")]
        [Default]
        public string CommentOnProjectStatus { get; set; }

        [OrderGrid(Order = 13)]
        [DisplayName("Reason For NoPlan")]
        [Default]
        public string ReasonForNoPlan { get; set; }

        [OrderGrid(Order = 14)]
        [DateRangeGrid]
        [DisplayName("SW Warranty End Date")]
        [Default]
        public DateTime? SoftwareEndOfWarrantyDate { get; set; }
        
        [OrderGrid(Order = 15)]
        public string SoftwareSupportProvider { get; set; }
        
        [OrderGrid(Order = 16)]
        public string SoftwareSupportType { get; set; }
        

        [OrderGrid(Order = 17)]
        [DateRangeGrid]
        public DateTime? SoftwareEndOfSupportContract { get; set; }


        [OrderGrid(Order = 18)]
        public string HardwareSupportProvider { get; set; }



        [OrderGrid(Order = 19)]
        [DisplayName("HW Sup. Type")]
        public string HardwareSupportType { get; set; }



        [OrderGrid(Order = 20)]
        [DateRangeGrid]
        public DateTime? HardwareEndOfSupportContract { get; set; }

        [IgnoreGrid]
        public bool RenewalInProgress { get; set; }

       [IgnoreGrid]
        public DateTime? LastModified { get; set; }



        [OrderGrid(Order = 28)]
        [DisplayName("Previous Resource Key")]
        public string PreviousResourceKey { get; set; }


        [OrderGrid(Order = 30)]
        [DateRangeGrid]
        [DisplayName("Last Modified")]
        public string LastModifiedValue { get; set; }

        [OrderGrid(Order = 31)]
        [MailTo]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }


        [OrderGrid(Order = 32)]
        [DisplayName("Is Software Extended Support Offered By Vendor")]
        public bool? IsExtendedSupportOfferedByVendor { get; set; }

        [OrderGrid(Order = 33)]
        [DisplayName("Is Hardware Extended Support Offered By Vendor")]
        public bool? HwIsExtendedSupportOfferedByVendor { get; set; }

        [IgnoreGrid]
        public long BuildBagId { get; set; }
    }
}