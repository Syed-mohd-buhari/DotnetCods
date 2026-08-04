using System.Collections.Generic;

namespace CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes
{
    public class ProductNameDtoUpdate: ProductNameDtoGrid
    {
        public Dictionary<int,string> VodafoneNameResource { get;set; }
    }
}
