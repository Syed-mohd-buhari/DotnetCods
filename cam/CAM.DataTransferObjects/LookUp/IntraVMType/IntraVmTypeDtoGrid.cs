using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.IntraVMType
{
    public class IntraVmTypeDtoGrid
    {
        [Default]
        public long IntraVmTypeId { get; set; }
        [Default]
        public string IntraDescription { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }

    }
}
