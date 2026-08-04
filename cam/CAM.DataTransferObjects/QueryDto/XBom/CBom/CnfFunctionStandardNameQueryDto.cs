using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.CBom
{
    public class CnfFunctionStandardNameQueryDto : QueryObject
    {
        public List<long> Functionstandardnameid { get; set; }
        public List<string> Functionname { get; set; }
    }
}
