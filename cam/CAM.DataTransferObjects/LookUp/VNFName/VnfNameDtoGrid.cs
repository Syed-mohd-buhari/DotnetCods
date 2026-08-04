using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.VNFName
{
    public class VnfNameDtoGrid
    {
        [Default]
        public long VnfNameId { get; set; }
        [Default]
        public string VnfDescription { get; set; }
        [Default]
        public string Product{ get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }

    }
}
