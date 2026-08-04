using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.OMC
{
    public partial class AssetAsIsSdiInfo : AuditableEntity
    {
        public long Assetasissdiinfoid { get; set; }
        public string Datasourcename { get; set; }
        public string Datasourcetype { get; set; }
        public string Swversion { get; set; }
        public string Firmwareversion { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Tsrmodel { get; set; }
    }
}
