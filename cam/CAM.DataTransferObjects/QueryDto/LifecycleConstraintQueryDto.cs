using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.QueryDto
{
   public class LifecycleConstraintQueryDto
    {
        public DateTime? MsLastTimeBuyNew { get; set; }
        public DateTime? MsLastTimeBuyUpgrades { get; set; }
        public DateTime? MsLastTimeBuyExpansions { get; set; }
        public DateTime? MsEndOfMaintenance { get; set; }
        public DateTime? MsEndOfsupport { get; set; }

        public DateTime? MhLastTimeBuyNew { get; set; }
        public DateTime? MhLastTimeBuyUpgrades { get; set; }
        public DateTime? MhLastTimeBuyExpansions { get; set; }
        public DateTime? MhEndOfMaintenance { get; set; }
        public DateTime? MhEndOfsupport { get; set; }
    }
}
