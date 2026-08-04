using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CAM.DataTransferObjects.Entita.DesignAspects
{
    public abstract class DesignAspectDto : GridDtoBase
    {
        [DisplayName("Opco")]
        [OrderGrid(Order = 1)]
        [Default]
        public string OpCoName { get; set; }

        [DisplayName("DCF Name")]
        [OrderGrid(Order = 2)]
        [Default]
        public string DesignComponentFamilyName { get; set; }

        [DisplayName("SubNetwork Boundary")]
        [OrderGrid(Order = 3)]
        [Default]
        public string SubNetworkBoundary { get; set; }
        [Default]
        [DisplayName("Platform Software")]
        [OrderGrid(Order = 4)]
        public bool? PlatformSoftware { get; set; }

        [IgnoreGrid]
        public short? OpCoId { get; set; }

        [DisplayName("DA Index")]
        [OrderGrid(Order = 5)]
       // [Default]
        public long Id { get; set; }

        [DisplayName("Design Component Family Index")]
        [OrderGrid(Order = 6)]
        public long DesignComponentFamilyId { get; set; }


        [DisplayName("Supported Services")]
        [OrderGrid(Order = 7)]
        [Default]
        public string SupportedServices { get; set; }

        [IgnoreGrid]
        public int? AuthenicationTypeId { get; set; }

        [DisplayName("Authenication Type")]
        [OrderGrid(Order = 8)]
        public string AuthenicationTypeName { get; set; }

        [IgnoreGrid]
        public int? SecurityManagerId { get; set; }

        [DisplayName("Security Manager")]
        [OrderGrid(Order = 9)]

        public string SecurityManagerName { get; set; }

        [IgnoreGrid]
        public int? LicenseModelId { get; set; }

        [DisplayName("License Model")]
        [OrderGrid(Order =10)]

        public string LicenseModelName { get; set; }

        [IgnoreGrid]
        public int? ThirdPartyAccessId { get; set; }

        [DisplayName("Third Party Access")]
        [OrderGrid(Order = 11)]

        public string ThirdPartyAccessName { get; set; }

        [IgnoreGrid]
        public int? SiteResilienceId { get; set; }
        [DisplayName("Site Resilienc")]
        [OrderGrid(Order = 12)]

        public string SiteResilienceName { get; set; }

        [IgnoreGrid]
        public int? SWDeliveryLifeCycleId { get; set; }
        [DisplayName("SW Delivery Life Cycle")]
        [OrderGrid(Order = 13)]

        public string SwDeliveryLifeCycleName { get; set; }

        [IgnoreGrid]
        public int? BusinessContinuityMethodId { get; set; }
        [DisplayName("Business Continuity Method")]
        [OrderGrid(Order = 14)]
        public string BusinessContinuityMethodName { get; set; }

        [IgnoreGrid]
        public int? InstanceResilienceId { get; set; }
        [DisplayName("Instance Resilience")]
        [OrderGrid(Order = 15)]

        public string InstanceResilienceName { get; set; }

        [DisplayName("BC")]
        [OrderGrid(Order = 16)]
        [Default]
        public string CriticalNationalInfrastructureName { get; set; }

        [IgnoreGrid]
        public short? SecurityTireZoneId { get; set; }
        [DisplayName("Security Tire Zone")]
        [OrderGrid(Order = 17)]
        [Default]
        public string SecurityTireZoneName { get; set; }

        [OrderGrid(Order = 18)]
        [DisplayName("Archived")]
        public bool? Archived { get; set; } = false;

        [DisplayName("Description")]
        [StringLength(2000)]
        [OrderGrid(Order = 19)]
        [Default]
        public string Description { get; set; }

        [OrderGrid(Order = 20)]
        [DisplayName("Nominal Capacity Limit")]
        public string NominalCapacityLimit { get; set; }


        [OrderGrid(Order = 21)]
        [DisplayName("Max Allowed Loading")]
        public string MaxAllowedLoading { get; set; }


        [OrderGrid(Order = 22)]
        [DisplayName("Designed Capacity Limit")]
        public string DesignedCapacityLimit { get; set; }

        [OrderGrid(Order = 23)]
        [DisplayName("Criticality Rating")]
        public int? CriticalityRating { get; set; }

        [OrderGrid(Order = 24)]
        [DisplayName("Country Specific Criticality")]
        public bool CountrySpecificCriticality { get; set; }


        [DisplayName("Vertical Response")]
        [OrderGrid(Order = 25)]        
        public string VerticalName { get; set; }

        [IgnoreGrid]
        [DisplayName("Vertical Id")]
        [OrderGrid(Order = 26)]
        public long VerticalId { get; set; }

        [IgnoreGrid]
        public new DateTime? LastModified { get; set; }

        [DateRangeGrid]
        [DisplayName("Last Modified")]
        [OrderGrid(Order = 27)]
        //[Default]
        public string LastModifiedValue { get; set; }

        [MailTo]
        [DisplayName("Last Modified By")]
        [OrderGrid(Order = 28)]
        //[Default]

        public new string LastModifiedBy { get; set; }

    }
    public class DesignAspectPlannedActivityDto
    {
        public string DesignComponentName { get; set; }
        public List<long?> DcfId { get; set; }
        public long? DcId { get; set; }
        public short? OpcoId { get; set; }
        public string OpcoName { get; set; }
        public string LcmId { get; set; }
        public string LcmPaId { get; set; }
        public string AssetPaId { get; set; }
        
    }

}
