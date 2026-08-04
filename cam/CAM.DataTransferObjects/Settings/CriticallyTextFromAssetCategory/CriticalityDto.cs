using System.ComponentModel;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.Settings.CriticallyTextFromAssetCategory
{
    public class CriticalityDto : GridDtoBase
    {
        public short AssetCategoryId { get; set; }
        [OrderGrid(Order = 1)]
        [DisplayName("Text")]
        public string Text { get; set; }
    }
}
