using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.OMC.AssetAsisSdiSwitchInfo
{
    public class AssetAsIsSdiSwitchInfoGridDto :GridDtoBase
    {
       
        [Default]
        [DisplayName("Data Source Name")]
        [OrderGrid(Order = 1)]
        public string Datasourcename { get; set; }
        [Default]
        [DisplayName("Switch Name")]
        [OrderGrid(Order = 2)]
        public string Switchname { get; set; }
        [Default]
        [DisplayName("Switch Id")]
        [OrderGrid(Order = 3)]
        public string Switchid { get; set; }
        [Default]
        [DisplayName("Switch Admin State")]
        [OrderGrid(Order = 4)]
        public string Switchadminstate { get; set; }
        [Default]
        [DisplayName("Switch Unique Id")]
        [OrderGrid(Order = 5)]
        public string Switchuniqueid { get; set; }
        [Default]
        [DisplayName("Switch Role")]
        [OrderGrid(Order = 6)]
        public string Switchrole { get; set; }
        [Default]
        [DisplayName("Switch Rack")]
        [OrderGrid(Order = 7)]
        public string Switchrack { get; set; }
        [Default]
        [DisplayName("Switch Label")]
        [OrderGrid(Order = 8)]
        public string Switchlabel { get; set; }
        [Default]
        [DisplayName("Switch Serial Number")]
        [OrderGrid(Order = 9)]
        public string Switchserialnumber { get; set; }
        [Default]
        [DisplayName("Switch Ops State")]
        [OrderGrid(Order = 10)]
        public string Switchopsstate { get; set; }
        [Default]
        [DisplayName("Switch Network")]
        [OrderGrid(Order = 11)]
        public string Switchnetwork { get; set; }
        [Default]
        [DisplayName("Switch Manufacturer")]
        [OrderGrid(Order = 12)]
        public string Switchmanufacturer { get; set; }
        [Default]
        [DisplayName("Switch Model")]
        [OrderGrid(Order = 13)]
        public string Switchmodel { get; set; }
        [Default]
        [DisplayName("Switch Ip Address")]
        [OrderGrid(Order = 14)]
        public string Switchipaddress { get; set; }
        [Default]
        [DisplayName("Switch Software Version")]
        [OrderGrid(Order = 15)]
        public string Switchswversion { get; set; }

        [DisplayName("Id Index")]
        [OrderGrid(Order = 16)]
        public long Assetasissdiswitchinfoid { get; set; }
    }
}
