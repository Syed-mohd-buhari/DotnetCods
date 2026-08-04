using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.ClusterLevelPA
{
    public class NWElementClusterAsPlannedQueryDto : QueryObject
    {
        public List<long> NetworkElementClusterAsPlannedId { get; set; }
        public List<long> InfraClusterAsPlannedId { get; set; }
        public List<decimal> ApplicationId { get; set; }
        public List<string> AppClusterName { get; set; }
        public List<short> DeploymentStatusId { get; set; }
        public long PaId { get; set; }
        public List<short> OpCoId { get; set; }

    }
}
