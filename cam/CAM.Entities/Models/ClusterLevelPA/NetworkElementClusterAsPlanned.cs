using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;

namespace CAM.Entities.Model.ClusterLevelPA
{
    public partial class NetworkElementClusterAsPlanned : AuditableEntity
    {
        public NetworkElementClusterAsPlanned()
        {
            ClusterUpGradeStatus = new HashSet<ClusterUpGradeStatus>();
        }

        public long NetworkElementClusterAsPlannedId { get; set; }
        public long? InfraClusterAsPlannedId { get; set; }
        public decimal? ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string AppClusterName { get; set; }
        public short? DeploymentStatusId { get; set; }
       
        public virtual ProductName Application { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual DeploymentStatus DeploymentStatus { get; set; }
        public virtual InfraClusterAsPlanned InfraClusterAsPlanned { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<ClusterUpGradeStatus> ClusterUpGradeStatus { get; set; }
    }
}
