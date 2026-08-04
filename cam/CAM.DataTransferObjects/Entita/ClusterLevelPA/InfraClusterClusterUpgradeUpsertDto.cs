using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.ClusterLevelPA
{
    public class InfraClusterClusterUpgradeUpsertDto
    {
        public InfraClusterClusterUpgradeUpsertDto()
        {           

            infraClusterAsPlannedDtoGrid = new List<InfraClusterAsPlannedDtoGrid>();
        }
        public List<InfraClusterAsPlannedDtoGrid> infraClusterAsPlannedDtoGrid { get; set; }

        public List<KeyValuePairDto> PlatfromResource { get; set; }
        public List<DropdownKeyValueList> DeploymentStatusReosurce { get; set; }
       
        public List<OpcoBasedLocation> LocationReosurce { get; set; }
        public List<KeyValuePairDto> ClusterTypeMswResource { get; set; }
        public  Dictionary<int, string>  VerticalResponsibleResource { get; set; }
        public List<KeyValuePairDto> HardwareMhwResource { get; set; }
        public List<KeyValuePairDto> OpcoReosurce { get; set; }      
        public List<ClusterUpgradeAddUpdateDto> ClusterUpgradeAddUpdateDto { get; set; }
        public long? PlannedHardwareTypeId { get; set; }
        public IDictionary<long, string> PlannedHardwareTypeResources { get; set; }
        public string Site { get; set; }
        public List<NWElementClusterAsPlannedUpSertDto> nwElementClusterAsPlannedUpSertDto { get; set; }
    }

    public class DropdownKeyValueList
    {

        public short Key { get; set; }
        public string Value { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }

    }
}
