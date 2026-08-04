using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.DataTransferObjects.LookUp.SubNetworkBoundary
{
    public class SubNetworkBoundaryGridDto : GridDtoBase
    {
        [DisplayName("SubNetwork Boundary Index")]
        [OrderGrid(Order = 1)]
        [Default]
        public long SubNetworkBoundaryId { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Vodafone Name")]
        [Default]
        public string VodafoneName { get; set; }

        [IgnoreGrid]
        public uint? VodafoneNameId { get; set; }


        [DisplayName("SubNetwork Boundary Name")]
        [OrderGrid(Order = 3)]
        [Default]
        public string SubNetworkBoundaryDescription { get; set; }

        [DisplayName("Alias")]
        [OrderGrid(Order = 4)]
        [Default]
        public string Alias { get; set; }

        [DisplayName("Supported Services")]
        [OrderGrid(Order = 5)]
        [Default]
        public string AllSupportedServices { get; set; }

        [DisplayName("GDPR Relevant")]
        [OrderGrid(Order = 6)]
        [Default]
        public bool? GdprRelevant { get; set; }

        [DisplayName("Internet Facing")]
        [OrderGrid(Order = 7)]
        [Default]
        public bool? InternetFacing { get; set; }

        //[IgnoreGrid]
        //public short? LcmPolicy { get; set; }

        [DisplayName("LCM Policy")]
        [OrderGrid(Order = 8)]
        [Default]
        public string LcmPolicy { get; set; }

        [DisplayName("Criticality")]
        [OrderGrid(Order = 9)]
        [Default]
        public string Criticality { get; set; }


        [DisplayName("GDPR Classification")]
        [OrderGrid(Order = 10)]
        [Default]
        public string GdrpClassificationValue { get; set; }


        [DisplayName("PCI/SOX")]
        [OrderGrid(Order = 11)]
        [Default]
        public bool? Pcisox { get; set; }


        [DisplayName("C3/C4")]
        [OrderGrid(Order = 12)]
        [Default]
        public bool? C3C4 { get; set; }



        [DisplayName("Mission Critical")]
        [OrderGrid(Order = 13)]
        [Default]
        public bool? MissionCritical { get; set; }

        //[DisplayName("Critical AssetType ")]
        //[OrderGrid(Order = 14)]
        //[Default]
        [IgnoreGrid]
        public int? CriticalAssetTypeId { get; set; }

        [DisplayName("System Function")]
        [OrderGrid(Order = 14)]
        [Default]
        public string SystemFunction { get; set; }

        [DisplayName("Customer Wheel")]
        [OrderGrid(Order = 15)]
        [Default]
        public string CustomerWheel { get; set; }

        [DisplayName("Security Element")]
        [OrderGrid(Order = 16)]
        //[Default]
        public bool? SecurityElement { get; set; }

        [IgnoreGrid]
        public new DateTime? LastModified { get; set; }

        [DisplayName("Last Modifiedt")]
        [OrderGrid(Order = 17)]
        [Default]
        [DateRangeGrid]
        public string LastModifiedValue { get; set; }

        //[DisplayName("GDPR Classification")]
        //[OrderGrid(Order = 17)]
        [IgnoreGrid]
        public int? GDPRClassification { get; set; }

        [IgnoreGrid]
        public List<int> SupportedServicesIDs { get; set; }

        [IgnoreGrid]
        public List<SupportedService> SupportedServices { get; set; }

        [IgnoreGrid]
        public List<DesignComponentFamily> DesignComponentFamilies { get; set; }

        [IgnoreGrid]
        public int? Order { get; set; }

        [IgnoreGrid]
        public bool Default { get; set; }

        [IgnoreGrid]
        public IEnumerable<short> CustomerWheelsIds { get; set; }

        [IgnoreGrid]
        public IEnumerable<short> SystemFunctionsIds { get; set; }

    }
}
