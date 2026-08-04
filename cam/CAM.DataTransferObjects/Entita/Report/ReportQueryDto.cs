#nullable enable
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.VIA;

namespace CAM.DataTransferObjects.Entita.Report
{
    public class ReportQueryAllDto
    {
        public ReportHardwareQueryDto? QueryHardware { get; set; }
        public ReportSoftwareQueryDto? QuerySoftware { get; set; }

        public ReportSoftwareQueryDto? QuerySoftwareLevelTwo { get; set; }
        public ReportSubnetWorkQueryDto? QuerySubnetworkHardware { get; set; }
        public ReportSubnetWorkQueryDto? QuerySubnetworkSoftware { get; set; }
        public ReportHardwareConfigurationQueryDto? QueryHardwareConfiguration { get; set; }

        public string ActiveTab { get; set; }
        public string LcmExportDescription { get; set; }
    }    
    public class ReportViaQueryAllDto
    {
        public ViaExportQuery? QueryHardware { get; set; }
        public ViaExportQuery? QuerySoftware { get; set; }
    }
}
