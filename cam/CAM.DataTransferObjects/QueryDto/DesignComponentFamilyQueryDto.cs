using System;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
using CAM.DataTransferObjects.FunctionalityDto;

namespace CAM.DataTransferObjects.QueryDto
{
    public class DesignComponentFamilyQueryDto : QueryObject
    {
        public List<string> SystemTypeIdentityName { get; set; }
        public List<int> DesignComponentFamilyName { get; set; }
        public List<string> Description { get; set; }
        public List<string> SubNetworkBoundary { get; set; }
        public List<long> SubNetworkBoundaryId { get; set; }
        public List<short> NetworkFunction { get; set; }
        public List<short> SupportedServices { get; set; }
        public List<long> VodafoneName { get; set; }
        public List<long> DesignComponentFamilyId { get; set; }
        public List<bool> Implementation { get; set; }
        public List<bool> SystemIsShared { get; set; }
        public List<int> CriticalityRating { get; set; }
        public new List<string> LastModifiedBy { get; set; }
        public List<short> SharingType { get; set; }
        public List<bool> CountrySpecificCriticality { get; set; }
        public List<decimal> ProductName { get; set; }
        public new DateFilter LastModifiedValue { get; set; }
        public List<int> SystemTypeIdBasedVerticalId { get; set; }
        public List<string> DesignContact { get; set; }

        public List<string> SystemTypeIdBasedVerticalValue { get; set; }

    }
}