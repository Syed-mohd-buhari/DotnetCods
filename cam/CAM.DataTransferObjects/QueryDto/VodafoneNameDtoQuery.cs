using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{

    public class VodafoneNameDtoQuery : QueryObject
    {
        public List<int> Id { get; set; }
        public List<string> Description { get; set; }
        public List<decimal> ProductName { get; set; }

        public DateFilter LastModifiedValue { get; set; }
        public List<string> RiskCluster { get; set; }
        public List<string> RiskLevel { get; set; }
    }
}
