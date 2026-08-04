using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.VBom
{
    public class IntraVmTypeQueryDto : QueryObject
    {
        public List<long> Intravmtypeid { get; set; }
        public List<string> Intradescription { get; set; }
    }
}
