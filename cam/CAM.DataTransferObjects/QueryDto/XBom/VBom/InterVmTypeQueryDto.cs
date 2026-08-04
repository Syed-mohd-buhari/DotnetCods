using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.VBom
{
    public class InterVmTypeQueryDto : QueryObject
    {
        public List<long> Intervmtypeid { get; set; }
        public List<string> Interdescription { get; set; }
    }
}
