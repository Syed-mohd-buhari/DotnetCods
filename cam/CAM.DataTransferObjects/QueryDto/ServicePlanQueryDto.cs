using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ServicePlanQueryDto : QueryObject
    {
        public List<int> ServicePlanId { get; set; }
        public List<int> ServiceMasterId { get; set; }  
        public List<int> OpCoId { get; set; }
        public List<long> DcfId { get; set; }
        public List<string> Program {  get; set; }
        public List<int> Status { get; set; }
    }
}
