using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.VBom
{
    public class VmWorkloadTypeQueryDto : QueryObject
    {
        public List<long> Vmworkloadtypeid { get; set; }
        public List<string> Description { get; set; }
    }
}
