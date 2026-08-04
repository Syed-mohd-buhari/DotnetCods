using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.LookUp.VnfHardwareType
{
    public class VnfHardwareTypeDtoGrid
    {
        [Default]
        public long VnfHardwareId { get; set; }
        [Default]
        public string Description { get; set; }
        [Default]
        public string LastModifiedBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime LastModified { get; set; }
    }
}
