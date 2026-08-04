using CAM.Entities.Models;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;

namespace CAM.Entities.Model.ClusterLevelPA
{
    public partial class InfraClusterAsPlanned : AuditableEntity
    {
        public InfraClusterAsPlanned()
        {
            NetworkElementClusterAsPlanned = new HashSet<NetworkElementClusterAsPlanned>();
            ClusterUpGradeStatus = new HashSet<ClusterUpGradeStatus>();
        }

        public long InfraClusterAsPlannedId { get; set; }
        public short? OpCoId { get; set; }
        public string? OpCoValue { get; set; }
        public short? LocationId { get; set; }
        public string? LocationValue { get; set; }
        public string Site { get; set; }
        public short? PlatformId { get; set; }
        public string? PlatformValue { get; set; }

        public long? ClustertypeId { get; set; }
        public string? ClustertypeValue { get; set; }
        public string ClusterName { get; set; }
        public long? HardwaretypeId { get; set; }
        public string? HardwaretypeValue { get; set; }
        public short? DeploymentStatusId { get; set; }
        public string? DeploymentStatusValue { get; set; }

        public int? VerticalResponsibleId { get; set; }
        public string? VerticalResponsibleValue { get; set; }

        public virtual MajorSoftwareBuild Clustertype { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual DeploymentStatus DeploymentStatus { get; set; }
        public virtual MajorHardwareBuild HardwaretypeNavigation { get; set; }
        public virtual Location Location { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual OpCo Opco { get; set; }
        public virtual Platform Platform { get; set; }
        public virtual ICollection<NetworkElementClusterAsPlanned> NetworkElementClusterAsPlanned { get; set; }
        public virtual ICollection<ClusterUpGradeStatus> ClusterUpGradeStatus { get; set; }

        public virtual VerticalResponsible VerticalResponsible { get; set; }
    }
}
