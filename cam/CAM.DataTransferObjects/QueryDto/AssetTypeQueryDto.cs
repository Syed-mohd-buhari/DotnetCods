using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AssetTypeQueryDto : QueryObject
    {
        public List<string> AssetTypeDescription { get; set; }
    }
}