using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.LookUp.Class
{
    public class ClassDtoCreate : ClassDtoGrid
    {
        public IDictionary<int, string> CategoryResource { get; set; }
    }
}
