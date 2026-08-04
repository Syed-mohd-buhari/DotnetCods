using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class SWConfigQueryDto : QueryObject
    {
        public List<long> SoftwareConfigurationId { get; set; }
        public List<long> NetworkElementId { get; set; }
        public List<string> OpCo { get; set; }
        public List<string> Oem { get; set; }
        public List<string> ElementName { get; set; }
        public List<decimal> FunctionId { get; set; }
        public List<string> FunctionName { get; set; }
        public List<decimal> SWConfigFunctionAreaId { get; set; }
        public List<string> FunctionAreaName { get; set; }
        public List<string> FunctionAreaDescription { get; set; }
        public List<int> SubFunctionId { get; set; }
        public List<string> SubFunctionName { get; set; }
        public List<int> SubFunctioinAreaId { get; set; }
        public List<string> SubFunctionAreaName { get; set; }
        public List<string> SubFunctionAreaDescription { get; set; }  
        public List<string> Creationuser { get; set; }
        public DateFilter Creationdate { get; set; }
        public List<string> Modificationuser { get; set; }
        public DateFilter Modificationdate { get; set; }
    }
}
