using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.ClusterLevelPA
{
    public class InfraClusterAsPlannedDtoGrid : GridDtoBase
    {
        public long InfraClusterAsPlannedId { get; set; }
        public short? OpCoId { get; set; }

        public string OpCoValue { get; set; }
        public short? LocationId { get; set; }

        public string LocationValue { get; set; }
        public string Site { get; set; }
        public short? PlatformId { get; set; }
        public string PlatformValue { get; set; }
        public long? ClustertypeId { get; set; }

        public string? ClustertypeValue { get; set; }
        public string ClusterName { get; set; }

        public long? HardwaretypeId { get; set; }
        public string? HardwaretypeValue { get; set; }

        public short? DeploymentStatusId { get; set; }
        public string? DeploymentStatusValue { get; set; }

        public int? VerticalResponsibleId { get; set; }
        public string? VerticalResponsibleValue { get; set; }
    }

    public class SiteLevelClusterDto
    {
        public string Site { get; set; }
        public List<InfraClusterAsPlannedDtoGrid> infraClusterAsPlannedDtoGrid { get; set; }
    }



}
