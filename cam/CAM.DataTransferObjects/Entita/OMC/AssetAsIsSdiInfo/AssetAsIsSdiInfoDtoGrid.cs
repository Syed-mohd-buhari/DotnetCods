using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.OMC.AssetAsIsSdiInfo
{
    public class AssetAsIsSdiInfoDtoGrid:GridDtoBase
    {
       
        [Default]
        [DisplayName("Data Source Name")]
        [OrderGrid(Order =1)]
        public string Datasourcename { get; set; }
        [Default]
        [DisplayName("Data Source Type")]
        [OrderGrid(Order = 2)]
        public string Datasourcetype { get; set; }
        [Default]
        [DisplayName("Software Version")]
        [OrderGrid(Order = 3)]
        public string Swversion { get; set; }
        [Default]
        [DisplayName("Firmware Version")]
        [OrderGrid(Order = 4)]
        public string Firmwareversion { get; set; }
        [Default]
        [DisplayName("Manufacturer")]
        [OrderGrid(Order = 5)]
        public string Manufacturer { get; set; }
        [Default]
        [DisplayName("Model")]
        [OrderGrid(Order = 6)]
        public string Model { get; set; }
        [Default]
        [DisplayName("Tsr Model")]
        [OrderGrid(Order = 7)]
        public string Tsrmodel { get; set; }
        [DisplayName("Id Index")]
        [OrderGrid(Order = 8)]
        public long Assetasissdiinfoid { get; set; }
    }
}
