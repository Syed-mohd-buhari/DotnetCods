using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;

namespace CAM.Entities.Models
{
    public class DesignAspect : AuditableEntity
    {

        public long Id { get; set; }
        public long DesignComponentFamilyId { get; set; }
        public int? VodafoneName { get; set; }
        public short? OpCoId { get; set; }
        public string Description { get; set; }
        public int? AuthenicationTypeId { get; set; }
        public int? SecurityManagerId { get; set; }
        public int? LicenseModelId { get; set; }
        public int? ThirdPartyAccessId { get; set; }
        public int? SiteResilienceId { get; set; }
        public int? SWDeliveryLifeCycleId { get; set; }
        public int? InstanceResilienceId { get; set; } 
        public int? BusinessContinuityMethodId { get; set; }
        public bool CriticalNationalInfrastructure { get; set; }
        public short? SecurityTireZoneId { get; set; }
        public bool? Archived { get; set; }
        public string NominalCapacityLimit { get; set; }
        public string MaxAllowedLoading { get; set; }
        public string DesignedCapacityLimit { get; set; }
        public int? CriticalityRating { get; set; }

        public virtual AuthenicationType AuthenicationType { get; set; }
        public virtual DesignComponentFamily DesignComponentFamily { get; set; }
        public virtual BusinessContinuityMethod BusinessContinuityMethod { get; set; }
        public virtual InstanceResilience InstanceResilience { get; set; }
        public virtual LicenseModel LicenseModel { get; set; }
        public virtual OpCo OpCo { get; set; }
        public virtual SecurityManager SecurityManager { get; set; }
        public virtual SecurityTireZone SecurityTireZone { get; set; }
        public virtual SiteResilience SiteResilience { get; set; }
        public virtual SWDeliveryLifeCycle SWDeliveryLifeCycle { get; set; }
        public virtual ThirdPartyAccessType ThirdPartyAccessType { get; set; }
        public virtual ICollection<DesignAspectNetworkFunction> DesignAspectNetworkFunctions { get; set; }
        public virtual ICollection<DesignAspectSupportedService> DesignAspectSupportedServices { get; set; }

        public virtual ICollection<PlannedActivity> PlannedActivities { get; set; }
        public List<FilterValueDtoKeyValueList> VerticalFilterDto { get; set; }

        public List<int?> DesignContactList { get; set; }
    }


}
