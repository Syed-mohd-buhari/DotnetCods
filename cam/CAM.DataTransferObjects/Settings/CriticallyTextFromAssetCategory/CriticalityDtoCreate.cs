using System.Collections.Generic;

namespace CAM.DataTransferObjects.Settings.CriticallyTextFromAssetCategory
{
    public class CriticalityDtoCreate : CriticalityDto
    {
        public Dictionary<int, string> AssetCategoryResource { get; set; }
    } 
    public class CriticalityDtoGrid : CriticalityDto
    {
    }
    
}