using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.PAT
{
    public class ReleaseModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthYear { get; set; }
        public string ReleaseNumber { get; set; }
    }
}
