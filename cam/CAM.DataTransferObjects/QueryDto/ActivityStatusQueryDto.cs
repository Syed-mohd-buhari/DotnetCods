using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ActivityStatusQueryDto : QueryObject
    {
        public List<string> ActivityStatusDescription { get; set; }
    }
}