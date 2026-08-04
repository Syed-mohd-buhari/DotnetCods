using CAM.Entities.Models;
using CAM.Entities.Models.Base;
using CAM.Identity;
using System;

namespace CAM.Entities.Model.ClusterLevelPA
{
    public partial class ClusterUpGradeStatus : AuditableEntity
    {
        public long ClusterUpGradeStatusId { get; set; }
        public long? InfraClusterAsPlannedId { get; set; }
        public long? NetworkElementClusterAsPlannedId { get; set; }
        public long? PlannedActivityId { get; set; }
        public short? StatusId { get; set; } 

        public virtual NetworkElementClusterAsPlanned NetworkElementClusterAsPlanned { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual InfraClusterAsPlanned InfraClusterAsPlanned { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual PlannedActivity PlannedActivity { get; set; }
    }
}
