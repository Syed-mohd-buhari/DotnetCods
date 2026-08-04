using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.Entita.DaMigrationStatus;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.DesignAspects
{

    public class DesignAspectDCFModel
    {
        public Dictionary<int, string> Services { get; set; }
        public Dictionary<int, string> NetworkFunctions { get; set; }
        public string SubNetworkBoundary { get; set; }
        public bool IsSupportedAllServices { get; set; }
        public bool? PlatformSoftware { get; set; }
    }
    public class DesignAspectDtoModel : DesignAspectDto
    {
        public IDictionary<int, string> SupportedServicesResource { get; set; }

        public IDictionary<int, string> UsedNetworkFunctionsResource { get; set; }

        public IDictionary<int, string> AuthenicationTypesResource { get; set; }

        public IDictionary<int, string> SiteResiliencesResource { get; set; }

        public IDictionary<int, string> SiteResilienceMethodsResource { get; set; }

        public IDictionary<int, string> InstanseResiliencesResource { get; set; }

        public IDictionary<short, string> OpcosResource { get; set; }

        public IDictionary<long, string> DCFsResource { get; set; }

        public IDictionary<int, string> SecurityManagersResource { get; set; }

        public IDictionary<int, string> SWDeliveryLifeCyclesResource { get; set; }

        public IDictionary<int, string> LicenseModelsResource { get; set; }
        public IDictionary<int, string> ThirdPartyAccessResource { get; set; }
        public IDictionary<short, string> SecurityTireZoneResource { get; set; }

        public IEnumerable<int> SupportedServicesIds { get; set; }
        public IEnumerable<int> UsedNetworkFunctionsIds { get; set; }
        public List<PlannedActivityDtoUpdate> PlannedActivityDto { get; set; }

        public bool isSupportedAllServices { get; set; }
        
        public List<DaMigrationStatusDtoGrid> DaMigrationStatusEntity { get; set; }

        public List<DaAssetMigrationDtoGrid> DaAssetMigrationEntity { get; set; }
   


    }
}
