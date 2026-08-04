using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.LookUp.Location;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.NetworkElementAsPlanned
{
    public class NetworkElementAsPlannedDtoCreate : NetworkElementAsPlannedDto
    {
        public IDictionary<short, string> OriginalEquipmentManufacturerResource { get; set; }
        public short OpCoId { get; set; }
        public short DeploymentStatusId { get; set; }
        public short? DeploymentTypeId { get; set; }
        public IDictionary<long, string> DesignComponentFamilyResource { get; set; }
        public short? LocationId { get; set; }
        public IDictionary<short, string> OpCoReosurce { get; set; }
        public long DesignComponentId { get; set; }
        public IDictionary<long, string> DesignComponentReosurce { get; set; }
       
        public short EnvironmentId { get; set; }
        public IDictionary<int, string> EnvironmentReosurce { get; set; }
      
        public IDictionary<short, DeploymentStatusDto> DeploymentStatusReosurce { get; set; }
        public IDictionary<int, string> DeploymentTypeReosurce { get; set; }
        public IDictionary<int, LocationDto> LocationReosurce { get; set; }
        
        public IDictionary<int, string> NfviBundleIDReosurce { get; set; }
        public List<int?> SubDomainSpocIds { get; set; }
        public List<int?> EduSpocIds { get; set; }
        public IDictionary<int, string>? SubDomainSpocResource { get; set; }

        public IDictionary<int, string>? EduSpocResource { get; set; }

        public short? NfviBundleIDId { get; set; }
        public List<PlannedActivityDtoUpdate> PlannedActivityDto { get; set; }
        
        public bool? IsFinalAsset { get; set; }
        #region SystemOfsystem
        public List<ViewBagandComponenetDto> BuildBagResources { get; set; }
        public long BuildBagId { get; set; }
        #endregion
        #region SystemNames
        public IDictionary<long, string> SystemNames { get; set; }
        #endregion

        public long LcmEngineeringId { get; set; }
    }
}
