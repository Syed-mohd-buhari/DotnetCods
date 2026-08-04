using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.OMC
{
    public partial class AssetAsIsSdiSwitchInfo : AuditableEntity
    {
        public long Assetasissdiswitchinfoid { get; set; }
        public string Datasourcename { get; set; }
        public string Switchname { get; set; }
        public string Switchid { get; set; }
        public string Switchadminstate { get; set; }
        public string Switchuniqueid { get; set; }
        public string Switchrole { get; set; }
        public string Switchrack { get; set; }
        public string Switchlabel { get; set; }
        public string Switchserialnumber { get; set; }
        public string Switchopsstate { get; set; }
        public string Switchnetwork { get; set; }
        public string Switchmanufacturer { get; set; }
        public string Switchmodel { get; set; }
        public string Switchipaddress { get; set; }
        public string Switchswversion { get; set; }
    }
}
