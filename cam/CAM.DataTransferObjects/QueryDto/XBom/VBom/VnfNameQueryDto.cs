using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.XBom.VBom
{
    public class VnfNameQueryDto : QueryObject
    {
        public List<long> Vnfnameid { get; set; }
        public List<string> Vnfdescription { get; set; }
        public List<decimal> Productid { get; set; }
    }
}
