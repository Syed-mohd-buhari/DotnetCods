using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.PodTypeInfo
{
    public class PodTypeInfoDtoGrid
    {
        [Default]
        public long PodTypeInfoId { get; set; }
        [Default]
        public string PodTypeInfoName { get; set; }
        [Default]
        public string PodRoleDescription { get; set; }       
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }

    }
}
