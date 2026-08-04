using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.LookUp.ServiceMaster
{
    public class ServiceMasterDtoGrid : GridDtoBase
    {
        [Default]
        public int ServiceMasterIndex { get; set; }
        [Default]
        public string Description { get; set; }
    }
}
