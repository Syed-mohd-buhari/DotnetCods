using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Security.Policy;

namespace CAM.Entities.Models
{
    public partial class BuildBag : AuditableEntity
    {
        public BuildBag()
        {
            ComponentSoftwareBuildBags = new List<ComponentSoftwareBuildBag>();
        }

        public long BuildBagId { get; set; }
        public string BagDescription { get; set; }
        public string BagVersion { get; set; }
        public long? DesignComponentFamilyId { get; set; }
        public short? OpCoId { get; set; }
        public bool VisibleFlag { get; set; }

        public List<FilterValueDtoKeyValueList> MappingComponetSoftwareDescription { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual OpCo OpCo { get; set; }
        public virtual DesignComponentFamily DesignComponentFamily { get; set; }


        public virtual ICollection<ComponentSoftwareBuildBag> ComponentSoftwareBuildBags { get; set; }

        public virtual ICollection<LcmEngineering> LcmEngineering { get; set; }
        public virtual ICollection<NetworkElementAsPlanned> NetworkElementsAsPlanned { get; set; }
        public virtual ICollection<PlannedActivity> PlannedActivities { get; set; }

    }
}

