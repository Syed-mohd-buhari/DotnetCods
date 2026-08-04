using CAM.DataAttributes.Grid;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.ServicePlan
{
    public class ServicePlanPaGridDto
    {
        [Default]
        [DisplayName("OpCo")]
        public string OpCo { get; set; }
        [Default]
        [DisplayName("Service Name")]
        public string ServiceName { get; set; }
        [Default]
        [DisplayName("DCF Name")]
        public string DesignComponentFamilyName { get; set; }
        [IgnoreGrid]
        [DisplayName("Is Service Plan")]
        public int ServicePlanId { get; set; }

        [DisplayName("Planned Action")]
        [Default]
        public IDictionary<short, string> PlannedActivity { get; set; }

        [DisplayName("Vertical Name")]
        public string VerticalName { get; set; }

    }
}
