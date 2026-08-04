using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto.XBom.VBom
{
    public class VnfHardwareTypeQueryDto:QueryObject
    {
        public List<long> Vnfhardwareid { get; set; }
        public List<string> Description { get; set; }
    }
}
