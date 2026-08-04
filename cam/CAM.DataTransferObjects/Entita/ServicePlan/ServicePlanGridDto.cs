using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.ServicePlan
{
    public class ServicePlanGridDto : GridDtoBase
    {
        [Default]
        public int ServicePlanId { get; set; }
        [IgnoreGrid]

        public int ServiceMasterId { get; set; }
        [Default]

        public string ServiceMaster {  get; set; }
        [IgnoreGrid]

        public short? OpCoId { get; set; }
        [Default]

        public string OpCo { get; set; }
        [IgnoreGrid]
        public long? DCFId { get; set; }
        [Default]

        public string DCFName { get; set; }
        [Default]

        public string Program {  get; set; }
        [Default]
        public string Status { get; set; }
        [IgnoreGrid]
        public Dictionary<int,string> DcfStatus {  get; set; }
        [IgnoreGrid]
        public long PlannedActivityId { get; set; }
        [IgnoreGrid]
        public int ServicePlanDcfMappingId { get; set; }
    }
}
