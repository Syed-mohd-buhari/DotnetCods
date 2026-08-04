using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AssetClassQueryDto : QueryObject
    {
        public List<string> AssetClassDescription { get; set; }
        public List<bool> IsHw { get; set; }
        public List<bool> IsSw { get; set; }
    }
}