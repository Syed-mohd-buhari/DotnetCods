using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.ExodusAssetLevelReport
{
    public class ExodusAssetLevelReportDtoGrid : ExodusFynancialYearDto
    {
        [Default]
        public string OpCo { get; set; }
        [Default]
        public string DcfName { get; set; }
        [Default]
        [IgnoreGrid]
        public string Site { get; set; }
        [Default]

        public string SiteName { get; set; }
        [Default]

        public string Vendor { get; set; }
        [Default]

        public string VnfCnf { get; set; }
        [Default]

        public string VendorNf { get; set; }
        [Default]

        public string XnfInstance { get; set; }
        [Default]

        public string UsageOptional { get; set; }
        [Default]

        public string XnfSizeCore { get; set; }
        [Default]

        public string Environment { get; set; }
        [Default]

        public string Status { get; set; }
        [Default]

        public string LaasCaasInfraStackInitialGoLive { get; set; }
        [Default]

        public string StackNameInitialGoLive { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime? RfoReqdBy { get; set; }
        [DateRangeGrid]
        [Default]

        public DateTime? RfsReqdBy { get; set; }
        [Default]

        public string PoRaised { get; set; }
        [Default]

        public string PoReqdByIfNotNa { get; set; }
        [Default]

        public string ClusterName { get; set; }
        [Default]

        public string HardwareTypeLive { get; set; }
        [Default]

        public string XnfSizeVcpu { get; set; }
        [Default]

        public string TargetXnfInstance { get; set; }
        [Default]

        public string LaasCaasInfraStackTarget { get; set; }
        [Default]

        public string StackNameTarget { get; set; }
        [Default]

        public string PoReqdBy { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime? BomSubmittedDate { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime? HwPoRaisedDate { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime? HwPoArrivedDate { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime? VecDate { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime? StartRfo { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime? StartOfAppIntegration { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime? RfaDate { get; set; }
       
        [Default]
        [DateRangeGrid]

        public DateTime? Rfs { get; set; }
        [Default]
        [DateRangeGrid]
        public DateTime? MigrationStart { get; set; }
        [Default]
        [DateRangeGrid]

        public DateTime? MigrationCompletionDate { get; set; }
        [Default]

        public string BroadcomRelease { get; set; }
        [Default]

        public string HardwareTypeTarget { get; set; }

        [IgnoreGrid]
        public string HardwareTypeTargetId { get; set; }
        [Default]

        public string NfSizeExpansionDcekpiValue { get; set; }
        [Default]

        public string NfSizeDcekpiSauGbps { get; set; }
        [Default]

        public string AciAvailable { get; set; }
        [Default]

        public string Traffic { get; set; }
        [Default]

        public string Power { get; set; }

    }
    public class ExodusFynancialYearDto
    {
        [IgnoreGrid]
        public long? OpCoId { get; set; }
        [IgnoreGrid]
        public long? DCFId { get; set; }
        [IgnoreGrid]
        public long? LocationId { get; set; }
        [IgnoreGrid]
        public long? OemId { get; set; }
        [IgnoreGrid]
        public string? LivePlatformId { get; set; }
        [IgnoreGrid]
        public decimal? ProductNameId { get; set; }
        [IgnoreGrid]
        public long? StausId { get; set; }
        [IgnoreGrid]
        public string? TargetPlatformId { get; set; }
        [IgnoreGrid]
        public long? EnvironmentId { get; set; }
        [IgnoreGrid]
        public string FyQ1_24_25 { get; set; }
        [IgnoreGrid]
        public string FyQ2_24_25 { get; set; }
        [IgnoreGrid]
        public string FyQ3_24_25 { get; set; }
        [IgnoreGrid]
        public string FyQ4_24_25 { get; set; }
        [IgnoreGrid]
        public string FyQ1_25_26 { get; set; }
        [IgnoreGrid]
        public string FyQ2_25_26 { get; set; }
        [IgnoreGrid]
        public string FyQ3_25_26 { get; set; }
        [IgnoreGrid]
        public string FyQ4_25_26 { get; set; }
        [IgnoreGrid]
        public string FyQ1_26_27 { get; set; }
        [IgnoreGrid]
        public string FyQ2_26_27 { get; set; }
        [IgnoreGrid]
        public string FyQ3_26_27 { get; set; }
        [IgnoreGrid]
        public string FyQ4_26_27 { get; set; }
        [IgnoreGrid]
        public string FyQ1_27_28 { get; set; }
        [IgnoreGrid]
        public string FyQ2_27_28 { get; set; }
        [IgnoreGrid]
        public string FyQ3_27_28 { get; set; }
        [IgnoreGrid]
        public string FyQ4_27_28 { get; set; }
        [IgnoreGrid]
        public string FyQ1_28_29 { get; set; }
        [IgnoreGrid]
        public string FyQ2_28_29 { get; set; }
        [IgnoreGrid]
        public string FyQ3_28_29 { get; set; }
        [IgnoreGrid]
        public string FyQ4_28_29 { get; set; }
        [IgnoreGrid]
        public string FyQ1_29_30 { get; set; }
        [IgnoreGrid]
        public string FyQ2_29_30 { get; set; }
        [IgnoreGrid]
        public string FyQ3_29_30 { get; set; }
        [IgnoreGrid]
        public string FyQ4_29_30 { get; set; }
        [IgnoreGrid]
        public string VerticalName { get; set; }
        [IgnoreGrid]
        public Dictionary<short, string> VerticalFilterDto { get; set; }

    }
}