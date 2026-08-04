using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.DaPlannedActivityDcf
{
    public class DaPlannedActivtyDcfDto
    {
        public long DaPlannedActivityDcfId { get; set; }
        public long? PlannedActivityId { get; set; }
        public long? DesignComponentfamilyId { get; set; }
        public string DcfStatus { get; set; }
        public string DesignComponentFamilyName { get; set; }
    }
}
