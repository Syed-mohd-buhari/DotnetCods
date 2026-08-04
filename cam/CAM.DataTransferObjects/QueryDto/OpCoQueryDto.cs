using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class OpCoQueryDto : QueryObject
    {
        public List<string> OpCoDescription { get; set; }
    }
}