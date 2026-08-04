
using CAM.DataTransferObjects.Entita.PlannedActivity;

namespace CAM.DataTransferObjects.Entita.ServicePlan
{
    public class ServicePlanUpdateDto : ServicePlanCreateDto
    {
        public PlannedActivityDtoUpdate PlannedActivityUpdateDto { get; set; }
    }
}
