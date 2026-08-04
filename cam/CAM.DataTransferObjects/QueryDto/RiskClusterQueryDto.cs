using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class RiskClusterQueryDto : QueryObject
    {
        public List<int> RiskClusterId { get; set; }
        public List<string> Description { get; set; }
        public List<string> RiskLevel { get; set; }
        public List<string> CreationUser { get; set; }
        public DateFilter CreationDate { get; set; }
        public List<string> ModificationUser { get; set; }     
        public DateFilter ModificationDate { get; set; }
    }
}
