using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ThirdPartyHardwareComponentQueryDto : QueryObject
    {
        public List<string> HardwareComponentType { get; set; }
        public List<string> HardwareComponent { get; set; }
        public List<string> HardwareReleaseInformation { get; set; }
        public List<short> VulnerabilityStatusId { get; set; }
        public List<short> MajorHardwareBuildId { get; set; }
        public DateFilter EndOfSale { get; set; }
        public DateFilter EndOfSupport { get; set; }
        public List<short> OriginalEquipmentManufacturerId { get; set; }
    }
}