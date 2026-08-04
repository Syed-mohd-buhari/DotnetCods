using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.InterVMType
{
    public class InterVmTypeDtoGrid
    {
        [Default]
        public long InterVmTypeId { get; set; }
        [Default]
        public string InterDescription { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }

    }
}
