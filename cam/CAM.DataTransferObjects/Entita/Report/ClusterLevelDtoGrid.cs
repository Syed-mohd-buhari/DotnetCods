using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.Report
{
    public class ClusterLevelDtoGrid : GridDtoBase
    {
       
        [IgnoreGrid]
        public long InfraClusterAsPlannedId { get; set; }
        [IgnoreGrid]
        public short? OpCoId { get; set; }
        [Default]
        [DisplayName("OpCo")]
        [OrderGrid(Order = 1)]
        public string OpCoValue { get; set; }
        [IgnoreGrid]
        public short? LocationId { get; set; }
        [Default]
        [DisplayName("Location")]
        [OrderGrid(Order = 2)]
        public string LocationValue { get; set; }
        [Default]
        [DisplayName("Site")]
        [OrderGrid(Order = 3)]
        public string Site { get; set; }
        [IgnoreGrid]
        public short? PlatformId { get; set; }
        [Default]
        [DisplayName("Platform")]
        [OrderGrid(Order = 4)]
        public string PlatformValue { get; set; }
        [IgnoreGrid]
        public long? ClustertypeId { get; set; }
        [Default]
        [DisplayName("Cluster Type")]
        [OrderGrid(Order = 5)]
        public string? ClustertypeValue { get; set; }
        [Default]
        [DisplayName("Cluster Name")]
        [OrderGrid(Order = 6)]
        public string ClusterName { get; set; }
        [IgnoreGrid]
        public long? ApplicationId { get; set; }
        [Default]
        [DisplayName("Application")]
        [OrderGrid(Order = 7)]
        public string? ApplicationName { get; set; }
        [Default]
        [DisplayName("App Cluster Name")]
        [OrderGrid(Order = 8)]
        public string? AppClusterName { get; set; }
        [IgnoreGrid]
        public long? HardwaretypeId { get; set; }
        [Default]
        [DisplayName("Hardware Type")]
        [OrderGrid(Order = 9)]
        public string? HardwaretypeValue { get; set; }

        [DisplayName("Infra Cluster Deployment Status")]
        [OrderGrid(Order = 10)]
        public string? InfraClusterAsPlannedDeploymentStatus { get; set; }      

        [IgnoreGrid]
        public int? VerticalResponsibleId { get; set; }
        [Default]
        [DisplayName("Vertical Responsible")]
        [OrderGrid(Order = 11)]
        public string? VerticalResponsibleValue { get; set; }
       
        [IgnoreGrid]
        [DateRangeGrid]
        [DisplayName(" Network Element Cluster Last Modified Date")]
        public DateTime? NetworkElementClusterdLastModified { get; set; }

        [IgnoreGrid]
        [MailTo]
        [DisplayName("Network Element Cluster Last Modified By")]
        public string NetworkElementClusterdLastModifiedBy { get; set; }

        [IgnoreGrid]
        public short? DeploymentStatusId { get; set; }

        [Default]
        [DisplayName("App Cluster Deployment Status")]
        [OrderGrid(Order = 12)]
        public string? DeploymentStatusValue { get; set; }

    }
}
