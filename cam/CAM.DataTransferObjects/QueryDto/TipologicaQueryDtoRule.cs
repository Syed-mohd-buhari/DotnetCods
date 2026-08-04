using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class TipologicaQueryDtoRule : TipologicaQueryDto
    {
        public List<int> Rule { get; set; }
        public List<string> CloudTypeBuild { get; set; }
        public List<string> IsCloudHostedAsset { get; set; }
    }
}