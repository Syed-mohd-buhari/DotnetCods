using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.NfviSoftwareCompatibility
{

    public class NfviSoftwareCompatibilityDtoGrid : GridDtoBase
    {
        [Default]
        public short NfviSoftwareCompatibilityId { get; set; }

        [Default]
        public string Vendor { get; set; }

        [IgnoreGrid]
        public short VendorId { get; set; }

        [DisplayName("PlatformVersion")]
        [Default]
        public string PlaftFormVersion { get; set; }
        [IgnoreGrid]
        public long PlaftFormVersionId { get; set; }

        [Default]
        public string ProductName { get; set; }
        [IgnoreGrid]
        public long ProductNameId { get; set; }

        [Default]
        public string VodafoneName { get; set; }
        [IgnoreGrid]
        public long VodafoneNameId { get; set; }

        [Default]
        public string MinimumSupportedVersion { get; set; }
       
    }
   
}
