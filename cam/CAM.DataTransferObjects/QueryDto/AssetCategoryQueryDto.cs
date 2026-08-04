using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class AssetCategoryQueryDto : QueryObject
    {
        public List<string> AssetCategoryDescriptionDescription { get; set; }
    }
}