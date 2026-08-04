using CAM.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.ClusterLevelPA
{
    public class ClusterUpgradeAddUpdateDto
    {
        public long ClusterUpGradeStatusId { get; set; }
        public long? InfraClusterAsPlannedId { get; set; }
        public long? NetworkElementClusterAsPlannedId { get; set; }
        public long? PlannedActivityId { get; set; }
        public short? StatusId { get; set; }
        public long? PlannedHardwareTypeId { get; set; }

    }
}
