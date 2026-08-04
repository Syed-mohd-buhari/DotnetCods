using System.Collections.Generic;

namespace CAM.DataTransferObjects.LookUp.Activity
{
    public abstract class ActivityStatusDto
    {
        public string ActivityStatusDescription { get; set; }
        public IEnumerable<short> PlannedActivitiesIds { get; set; }
        public IEnumerable<string> PlannedActivitiesValues { get; set; }
    }
}