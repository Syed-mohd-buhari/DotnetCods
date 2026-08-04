using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.QueryDto
{
    public class ReportSubnetWorkQueryDto : ReportSoftwareQueryDto
    {
        public List<string> LcmStatusEngHardware { get; set; }
        public List<string> LCMStatusOpsHardware { get; set; }
        public List<string> OutputToLcmHardware { get; set; }

        //public List<short> TypeOfNetworkElement { get; set; }
        //public List<string> OriginalLCMSpreadsheetID { get; set; }
        public List<string> HwIsExtendedSupportOfferedByVendor { get; set; }
        public List<string> OriginalHwLcmId { get; set; }

        public List<int> DesignComponentFamily { get; set; }

        public List<int> SupportedService { get; set; }

    }
}
