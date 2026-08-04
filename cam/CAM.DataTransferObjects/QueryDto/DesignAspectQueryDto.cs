using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{
    public class DesignAspectQueryDto : QueryObject
    {
        public List<int> Id { get; set; }
        public List<int> DesignComponentFamilyName { get; set; }
        public List<int> DesignComponentFamilyId { get; set; }
        public List<int> VodafoneName { get; set; }
        public List<string> SubNetworkBoundary { get; set; }
        public List<string> Description { get; set; }
        public List<int> AuthenicationTypeName { get; set; }
        public List<long> OpCoName { get; set; }
        public List<int> SecurityManagerName { get; set; }
        public List<int> ThirdPartyAccessName { get; set; }
        public List<int> SiteResilienceName { get; set; }
        public List<int> SwDeliveryLifeCycleName { get; set; }

        public List<int> LicenseModelName { get; set; }
        public List<int> BusinessContinuityMethodName { get; set; }
        public List<int> InstanceResilienceName { get; set; }
        public List<int> SecurityTireZoneName { get; set; }
        public List<short> SupportedServices { get; set; }
        public List<short> UsedNetworkFunctions { get; set; }
        public List<long> PlannedActivity { get; set; }
        public List<bool> countrySpecificCriticality { get; set; }

        public List<int> CriticalityRating { get; set; }
        public DateFilter LastModifiedValue { get; set; }
        public bool Archived { get; set; } = false;
        public List<string> NominalCapacityLimit { get; set; }
        public List<string> MaxAllowedLoading { get; set; }
        public List<string> DesignedCapacityLimit { get; set; }

        public List<long> OpCoId { get; set; }

        public List<long> VerticalId { get; set; }

        public List<string> VerticalName { get; set; }
        public List<string> PlatformSoftware { get; set; }
    }
}
