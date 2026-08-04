using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.VBom
{
    public class VmTypeNameQueryDto : QueryObject
    {
        public List<long> Vmtypenameid { get; set; }
        public List<long> Vnfnameid { get; set; }
        public List<string> Vmtypedescription { get; set; }
    }
}
