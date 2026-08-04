using System.Collections.Generic;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.PlannedActivity;

namespace CAM.DataTransferObjects.Entita.DesignComponent
{
    public class ImpactSubNetworkBoundaryChanged
    {
        public List<LcmEngineeringDtoGrid> EngineeringDtoGrids { get; set; }
        public List<PlannedActivityDtoGrid> PlannedActivityDtoGrids { get; set; }
        public List<NetworkElementAsPlannedDtoGrid> NetworkElementAsPlannedDtoGrids { get; set; }
    }
}