using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class DeliveryTrackingQueryDto : QueryObject
    {
        public List<int> Id { get; set; }
        public List<long?> PlannedActivityId { get; set; }
        public List<int> Activity { get; set; }
        public List<string> MS1EventType { get; set; }
        public DateFilter MS1BaseLineDate { get; set; }
        public DateFilter MS1LatestPlanningDate { get; set; }
        public List<int?> MS1Status { get; set; }
        public List<string> MS2EventType { get; set; }
        public DateFilter MS2BaseLineDate { get; set; }
        public DateFilter MS2LatestPlanningDate { get; set; }
        public List<int?>    MS2Status { get; set; }
        public List<string?> MS3EventType { get; set; }
        public DateFilter MS3BaseLineDate { get; set; }
        public DateFilter   MS3LatestPlanningDate { get; set; }
        public List<int?> MS3Status { get; set; }
        public List<int?> MS4EventType { get; set; }
        public DateFilter MS4BaseLineDate { get; set; }
        public DateFilter MS4LatestPlanningDate { get; set; }
        public List<int?> MS4Status { get; set; }
        public DateFilter  PPMImportDate { get; set; }
        public List<string> Notes1 { get; set; }
        public List<string> Notes2 { get; set; }
        public List<string> Description { get; set; }
        public List<string> ActivityIndex { get; set; }
        public List<string> PpmID { get; set; }
        public List<string> VerticalName {  get; set; }
        public List<short> OpCo { get; set; }
    }
}