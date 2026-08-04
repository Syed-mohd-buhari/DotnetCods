using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.ComponentSoftware
{
    public class ComponentMappingSWBuildBagQueryDto : QueryObject
    {
        public List<long> ComponentSoftwarebBuildId { get; set; }       
        public List<string> BuildBagDescription { get; set; }

        public List<string> ComponetSwBuilDescription { get; set; }
         
        public List<string> LastModifiedBy { get; set; }

        public DateFilter LastModifiedValue { get; set; }
         
    }
}