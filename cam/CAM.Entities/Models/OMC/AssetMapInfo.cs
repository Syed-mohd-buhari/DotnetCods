using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.OMC
{
    public partial class AssetMapInfo: AuditableEntity
    {
        public long Assetmapinfoid { get; set; }
        public string Omcassetname { get; set; }
        public string Temsassetname { get; set; }
        public string Enmassetname { get; set; }
        public string Site { get; set; }
        public string Datasourcename { get; set; }
    }
}
