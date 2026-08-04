using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.ClusterLevelPA
{
    public class NWElementClusterAsPlannedUpSertDto 
    {
        public long NetworkElementClusterAsPlannedId { get; set; }
        public long? InfraClusterAsPlannedId { get; set; }
        public decimal? ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string AppClusterName { get; set; }
        public short? DeploymentStatusId { get; set; }
        public string DeploymentStatusValue { get; set; }
        public string ClusterName { get; set; }
        public List<KeyValuePairDto> ApplicationNames { get; set; }
        public List<KeyValuePairDto> DeploymentStatuesResources { get; set; }

    }

    public class NWElementClusterAsPlannedDto
    {
        public string ClusterName { get; set; }
        public long? InfraClusterAsPlannedId { get; set; }
        public List<NWElementClusterAsPlannedUpSertDto> NWElementClusterAsPlannedUpSertDto {  get; set; }
    }

    public class NWElementClusterAsPlannedDtoGrid : GridDtoBase
    {
        public string ClusterName { get; set; }
        public long NetworkElementClusterAsPlannedId { get; set; }
        public long? InfraClusterAsPlannedId { get; set; }
        public decimal? ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string AppClusterName { get; set; }
        public short? DeploymentStatusId { get; set; }
        public string DeploymentStatusValue { get; set; }
    }
}
