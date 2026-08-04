using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ThirdPartySoftwareComponentQueryDto : QueryObject
    {
        public List<string> SoftwareComponentType { get; set; }
        public List<string> SoftwareComponent { get; set; }
        public List<string> SoftwareReleaseInformation { get; set; }
        public List<short> VulnerabilityStatusId { get; set; }
        public DateFilter EndOfSale { get; set; }
        public DateFilter EndOfSupport { get; set; }
        public List<short> OriginalEquipmentManufacturerId { get; set; }
    }
}