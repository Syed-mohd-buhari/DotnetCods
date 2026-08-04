using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ReportHardwareConfigurationQueryDto : ReportQueryDto
    {
        public List<string> LcmStatusEngHardware { get; set; }
        public List<string> LCMStatusOpsHardware { get; set; }
        public List<string> OutputToLcmHardware { get; set; }

        public List<short> TypeOfNetworkElement { get; set; }
        public List<string> OriginalLCMSpreadsheetID { get; set; }
        public List<string> HwIsExtendedSupportOfferedByVendor { get; set; }
        public List<string> OriginalHwLcmId { get; set; }

        public List<string> HardwareProfile { get;set; }    
    }
}
