 
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class NfviSoftwareCompatibilityQueryDto : QueryObject
    {
        public List<long> NfviSoftwareCompatibilityId { get; set; }
        public List<string> MinimumSupportedVersion { get; set; }
        public List<string> plaftFormVersion { get; set; }
        public List<string> productName { get; set; }
        public List<string> vendor { get; set; }
        public List<long> VodafoneName { get; set; }

    }
}
