using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.VmWorkloadType
{
    public class VmWorkloadTypeDtoGrid
    {
        [Default]
        public long VmWorkLoadTypeId { get; set; }
        [Default]
        public string Description { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }

    }
}
