using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class DeliveryStatusQueryDto : QueryObject
    {
        public List<string> DeliveryStatusDescription { get; set; }
    }
}