using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.OMC
{
    public class AssetAsisSdiSwitchInfoQueryDto:QueryObject
    {
        public List<long> Assetasissdiswitchinfoid { get; set; }
        public List<string> Datasourcename { get; set; }
        public List<string> Switchname { get; set; }
        public List<string> Switchid { get; set; }
        public List<string> Switchadminstate { get; set; }
        public List<string> Switchuniqueid { get; set; }
        public List<string> Switchrole { get; set; }
        public List<string> Switchrack { get; set; }
        public List<string> Switchlabel { get; set; }
        public List<string> Switchserialnumber { get; set; }
        public List<string> Switchopsstate { get; set; }
        public List<string> Switchnetwork { get; set; }
        public List<string> Switchmanufacturer { get; set; }
        public List<string> Switchmodel { get; set; }
        public List<string> Switchipaddress { get; set; }
        public List<string> Switchswversion { get; set; }
    }
}
