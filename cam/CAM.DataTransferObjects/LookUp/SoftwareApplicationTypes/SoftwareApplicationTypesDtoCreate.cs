using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes
{
    public class ProductNameDtoCreate: ProductNameDtoGrid
    {
        public Dictionary<int, string> VodafoneNameResource { get; set; }
    }
}
