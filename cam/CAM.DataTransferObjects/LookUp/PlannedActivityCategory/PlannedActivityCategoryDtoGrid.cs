using CAM.DataAttributes.Grid;
using System;

namespace CAM.DataTransferObjects.LookUp.PlannedActivityCategory
{
    public class PlannedActivityCategoryDtoGrid
    {
        [Default]
        public short PlannedActivityCategoryId { get; set; }
        [Default]
        public string CategoryDescription { get; set; }

        [IgnoreGrid]
        public string CreationUser { get; set; }
        [DateRangeGrid]
        [IgnoreGrid]
        public DateTime CreationDate { get; set; }
        [Default]

        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }
       
    }
}
