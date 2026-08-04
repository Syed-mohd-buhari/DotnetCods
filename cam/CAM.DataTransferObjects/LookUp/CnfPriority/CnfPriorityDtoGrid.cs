using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.CnfPriority
{
    public class CnfPriorityDtoGrid
    {
        [Default]
        public long CnfPriorityId { get; set; }
        [Default]
        public string Description { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }
    }
}
