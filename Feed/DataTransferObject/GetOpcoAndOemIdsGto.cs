using System.Xml.Linq;

namespace TEMS.DataTransferObject
{
    public class GetOpcoAndOemIdsGto : NetworkAsIsInsertionDto
    {
        public string OpCoId { get; set; }
        public string OemId { get; set; }
        public string AssetId { get; set; }
    }
    public class NetworkAsIsInsertionDto
    {
        public string OpCoId { get; set; }
        public string SystemId { get; set; }
        public string OemId { get; set; }
        public string AssetId { get; set; }
        public string LocationId { get; set; }
        public string SoftwareProductNumber { get; set; }
        public string SoftwareProductDate { get; set; }
        public string SoftwareInstallDate { get; set; }
        public string DataAcquisitionDate { get; set; }
        public string ElementName { get; set; }
        public string NodeType { get; set; }
        public string PlatformType { get; set; }
        public string HardwareType { get; set; }
        public string SoftwareReleaseInformation { get; set; }
        public int User { get; set; }
        public string Spare1ossorenm { get; set; }

    }
    public class IdentityAsisCreateDto : GetOpcoAndOemIdsGto
    {
        public string IPAddress { get; set; }
        public string AssetId { get; set; }
        public int User { get; set; }
        public string ResourceKey { get; set; }
        public string CategoryId { get; set; }
        public Dictionary<string, string>? AssetEntity { get; set; }
    }

    public class DcfLifeCycle
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? Currentdetails { get; set; }
        public string? Opcoid { get; set; }
        public string? Dcfid { get; set; }
        public string? Resourcekey { get; set; }
        public int Categorytype { get; set; }
        public string? Dcfdescription { get; set; }
        public string? Opcodescription { get; set; }
        public string? Dcdescription { get; set; }
        public string? DcId { get; set; }
        public string? BagName { get; set; }
    }
}
