using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.VMTypeName
{
    public class VmTypeNameDtoGrid
    {
        [Default]
        public long VmTypeNameId { get; set; }
        [Default]
        public string VmTypeDescription { get; set; }
        [Default]
        public string VnfName { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }

    }
}
