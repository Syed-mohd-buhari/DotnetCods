using System.Collections.Generic;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.QueryDto
{
    public class CriticalityQueryDto : QueryObject
    {
        public List<short> AssetCategory { get; set; }
        public List<string> Text { get; set; }

    }
}