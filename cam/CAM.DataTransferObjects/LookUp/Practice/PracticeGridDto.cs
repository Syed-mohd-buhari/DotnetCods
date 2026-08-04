using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.LookUp.Practice
{
    public class PracticeGridDto 
    {
        [Default]
        public int PracticeId { get; set; }
        [Default]

        public string PracticeDescription { get; set; }
        [Default]
        public string PracticeEmail { get; set; }
        [IgnoreGrid]
        public  DateTime LastModified { get; set; }
        [Default]
        public virtual string LastModifiedBy { get; set; }
        [IgnoreGrid]
        public Dictionary<int,string> PracticeResource { get;set; }
        [IgnoreGrid]
        public int? PracticeEmailId { get; set; }
    }
}
