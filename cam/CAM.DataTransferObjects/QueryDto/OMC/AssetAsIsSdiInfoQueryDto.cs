using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.OMC
{
    public class AssetAsIsSdiInfoQueryDto:QueryObject
    {
        public List<long> Assetasissdiinfoid { get; set; }
        public List<string> Datasourcename { get; set; }
        public List<string> Datasourcetype { get; set; }
        public List<string> Swversion { get; set; }
        public List<string> Firmwareversion { get; set; }
        public List<string> Manufacturer { get; set; }
        public List<string> Model { get; set; }
        public List<string> Tsrmodel { get; set; }
    }
}
