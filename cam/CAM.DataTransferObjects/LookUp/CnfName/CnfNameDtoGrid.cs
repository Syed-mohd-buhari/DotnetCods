using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.CnfName
{
    public class CnfNameDtoGrid
    {
        [Default]
        public long CnfNameId { get; set; }
        [Default]
        public string CnfDescription { get; set; }
        [Default]
        public string Product{ get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }

    }
}
