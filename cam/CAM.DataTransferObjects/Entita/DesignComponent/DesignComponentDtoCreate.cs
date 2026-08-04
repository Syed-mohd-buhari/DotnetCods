using CAM.DataTransferObjects.LookUp;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.DesignComponent
{
    public class DesignComponentDtoCreate : DesignComponentDto
    {
        public List<DictionaryList> SystemTypeResource { get; set; }
        public List<SystemAndSubNetworkBoundary> SubNetworkBoundaryResource { get; set; }

        public IDictionary<int, string> SubNetworkSupportedServices { get; set; }

        public long SystemTypeId { get; set; }
        public List<long> SubNetworkBoundaryIds { get; set; }

        public List<long> SupportedServiceIds { get; set; }

        public bool SupportedAllServices { get; set; }
        public long? DesignComponentFamilyId { get; set; }
        public long? PlatformId { get; set; }

        //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
        public bool? VisibleFlag { get; set; }
    }
}