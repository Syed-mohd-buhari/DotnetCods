using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.CnfHardwareType
{
    public class CnfHardwareTypeDtoGrid
    {
        [Default]
        public long CnfHardwareId { get; set; }
        [Default]
        public string Description { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }
    }
}
