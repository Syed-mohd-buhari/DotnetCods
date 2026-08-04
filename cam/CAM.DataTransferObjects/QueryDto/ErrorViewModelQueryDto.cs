using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ErrorViewModelQueryDto : QueryObject
    {
        public List<string> RequestId { get; set; }
        public List<bool> ShowRequestId { get; set; }
    }
}