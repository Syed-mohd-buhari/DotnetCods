using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.VodafoneName
{
    public class VodafoneNameDtoCreate : VodafoneNameDtoGrid
    {
        //public IDictionary<decimal, string>? SoftwareApplicationTypesResource { get; set; }
        public List<ProductNameResourceModel>? ProductNameResource { get; set; }
        public Dictionary<decimal,string> LinkedProductNames { get; set; }
        public Dictionary<decimal,string> NotLinkedProductNames { get; set; }   
    }
}
