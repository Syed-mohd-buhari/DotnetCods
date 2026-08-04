using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.LookUp.AssetMapInfo
{
    public class AssetMapInfoDtoGrid:GridDtoBase
    {
        [Default]
        public long AssetMapInfoId { get; set; }
        [Default]
        [DisplayName("OMC Asset")]
        public string OmcAssetName { get; set; }
        [Default]
        [DisplayName("TEMS Asset")]
        public string TemsAssetName { get; set; }
        [Default]
        [DisplayName("ENM Asset")]
        public string EnmAssetName { get; set; }
        [Default]

        public string Site { get; set; }
        [Default]
        [DisplayName("Data Source Name")]
        public string DataSourceName { get; set; }
    }
}
