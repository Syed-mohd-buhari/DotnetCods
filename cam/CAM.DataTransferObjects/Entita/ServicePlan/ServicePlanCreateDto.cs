using CAM.DataTransferObjects.Entita.PlannedActivity;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.ServicePlan
{
    public class ServicePlanCreateDto : ServicePlanGridDto
    {
        public Dictionary<int,string> ServiceMasterResource {  get; set; }
        public PlannedActivityDtoCreate PlannedActivityCreateDto { get; set; }
        public List<ServicePlanGridDto> ServicePlanDcfDetails { get; set; }
        public ServicePlanGridDto ServicePlanDetails { get; set; }


        public List<long?> DcfIdList { get; set; }
    }
}
