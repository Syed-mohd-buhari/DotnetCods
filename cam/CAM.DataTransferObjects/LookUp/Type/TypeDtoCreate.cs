using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.Type
{
    public class TypeDtoCreate : TypeDto
    {
        public IDictionary<int, string> CategoryResource { get; set; }
    }
}
