using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class DesignComponentQueryDto : QueryObject
    {
        public List<long> DesignComponentFamily { get; set; }
        public List<long> DesignComponentFamilyId { get; set; }
        public List<long> SystemTypeId { get; set; }
        public List<long> DesignComponentId { get; set; }
        public List<long> DesignComponent { get; set; }
        public List<short> EquipmentManufacturer { get; set; }

        public List<decimal> ProductName { get; set; }

        public List<string> SoftwareVersion { get; set; }

        public List<short> HardwarePlatform { get; set; }

        public List<string> HardwareSolution { get; set; }
        public List<string> HardwareType { get; set; }
        public List<string> LastModifiedBy { get; set; }
        public List<string> SubNetworkBoundary { get; set; }

        public List<int> VodafoneName { get; set; }

        public DateFilter LastModifiedValue { get; set; }

        public List<int> SystemTypeIdBasedVerticalId { get; set; }

        public List<string> SystemTypeIdBasedVerticalValue { get; set; }
        public List<string> DesignContact { get; set; }
    }
}