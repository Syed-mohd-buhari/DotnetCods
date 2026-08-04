using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.PAT
{
    public class PlannedActivityReleaseModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string NextMajorRelease { get; set; }
        public string CurrentMajorRelease { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public DateTime? CurrentEndofMaintenance { get; set; }
        public DateTime? NextEndofMaintenance { get; set; }
    }
}
