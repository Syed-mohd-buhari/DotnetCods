using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class OriginalEquipmentManufacturerQueryDto : QueryObject
    {
        public List<string> OriginalEquipmentManufacturerDescription { get; set; }
    }
}