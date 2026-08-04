using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public class PlannedActivityMigrationsDto
    {
        public long PlannedActivityId { get; set; }
        public long? LinkedPlannedActivityId { get; set; }
        public int NumberOfNodes { get; set; }

        public int NumberOfLabNodes { get; set; }

        public IEnumerable<long> NetworkElementStart { get; set; }
        public IEnumerable<long> NetworkElementEnd { get; set; }
        public long DesignComponentIdStart { get; set; }

        public short? PlannedActivityFor { get; set; }

    }
}
