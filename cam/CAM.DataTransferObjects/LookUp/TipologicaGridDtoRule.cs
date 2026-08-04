using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.LookUp
{
    public class TipologicaGridDtoRule : TipologicaGridDto
    {
        [OrderGrid(Order = 5)]
        [Default]
        public int Rule { get; set; }
 
    }

    public class BuildConstructionRule : TipologicaGridDtoRule
    {
        [OrderGrid(Order = 6)]
        [Default]
        public string IsCloudHostedAsset { get; set; }
        [OrderGrid(Order = 7)]
        [IgnoreGrid]
        public bool IsCloudHostedAssetBool { get; set; }
        [OrderGrid(Order = 8)]
        [Default]
        public string CloudTypeBuild { get; set; }
        [IgnoreGrid]
        public string RuleDescription { get; set; }
    }
}