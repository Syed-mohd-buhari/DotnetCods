using System;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Entita.DesignComponentFamily
{
    public abstract class DesignComponentFamilyDto : GridDtoBase
    {
        [DisplayName("Subnetwork Boundary Index")]
        [OrderGrid(Order = 17)]        
        public long SubNetworkBoundaryId { get; set; }

        [DisplayName("System Type Identity Name")]
        [OrderGrid(Order = 5)]
        [Default]
        public string SystemTypeIdentityName { get; set; }

        [DisplayName("Implementation")]
        [OrderGrid(Order = 9)]
        [Default]
        public bool Implementation { get; set; }

        [OrderGrid(Order =10)]
        public string Description { get; set; }

        [DisplayName("System Shared")]
        [OrderGrid(Order = 11)]
        public bool SystemIsShared { get; set; }


        //[DisplayName("Criticality Rating")]
        //[OrderGrid(Order = 12)]
        ////[Default]
        //public int? CriticalityRating { get; set; }

        //[DisplayName("Country Specific Criticality")]
        //[OrderGrid(Order = 13)]
        ////[Default]
        //public bool CountrySpecificCriticality { get; set; }
        [IgnoreGrid]
        public short? SharingTypeId { get; set; }

        [IgnoreGrid]
        public short? MajorSoftwareOemId { get; set; }
        //[IgnoreGrid]
        //public string MajorSoftwareApplicationType { get; set; }


        [OrderGrid(Order = 14)]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [IgnoreGrid]
        public short? MajorHardwareOemId { get; set; }
        [IgnoreGrid]
        public short? PlatformId { get; set; }

        [IgnoreGrid]
        public new DateTime? LastModified { get; set; }

        [DateRangeGrid]
        [DisplayName("Last Modified")]
        [OrderGrid(Order = 15)]
        [Default]
        public string LastModifiedValue { get; set; }

        [MailTo]
        [DisplayName("Last Modified By")]
        [OrderGrid(Order = 16)]
        [Default]
        public new string LastModifiedBy { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Design Contact")]
        public string DesignContact { get; set; }
    }
}