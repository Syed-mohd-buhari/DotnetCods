using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.ClusterLevelPA
{
    public class InfraClusterAsPlannedQueryDto : QueryObject
    {
        public List<long> InfraClusterAsPlannedId { get; set; }
        public List<string?> OpCoValue { get; set; }
        public List<string?> LocationValue { get; set; }
        public List<string> Site { get; set; }
        public List<string?> PlatformValue { get; set; }
        public List<string?> ClustertypeValue { get; set; }
        public List<string> ClusterName { get; set; }
        public List<string?> HardwareTypeValue { get; set; }
        public List<string?> DeploymentStatusValue { get; set; }

        public List<string?> VerticalResponsibleValue { get; set; }

        public List<short?> OpCoId { get; set; }

        public long PaId { get; set; }

    }
}
