using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.ComponentSoftware
{
    public class ComponentManufacturersQueryDto : QueryObject
    {
        public List<long> ComponentManufacturerId { get; set; }
        public List<string> ComponentManufacturer { get; set; }
        public List<string> ComponentName { get; set; }

    }
}