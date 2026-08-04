using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Enum;
using ClosedXML.Excel;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.AssetPassThrough
{
    public class PassThroughLcmDtoGrid
    {
        [DisplayName("ID")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 1)]
        [ColorGrid(Color = "blue")]
        [FormatClosetXml(Type = XLDataType.Text)]
        [Default]
        public string ReportId { get; set; }

        [DisplayName("Local Market")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 2)]
        [ColorGrid(Color = "red")]
        [Default]
        public string LocalMarket { get; set; }

        [DisplayName("Vertical Engineering Team")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 3)]
        [ColorGrid(Color = "red")]
        [Default]
        public string VerticalEngineeringTeam { get; set; }
        [DisplayName("Vertical Sub-Domain")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 4)]
        [ColorGrid(Color = "red")]
        [Default]
        public string VerticalSubDomain { get; set; }

        [DisplayName("Engineering Contact Point")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 5)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngineeringContactPoint { get; set; }

        [DisplayName("Asset Category")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 7)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetCategory { get; set; }

        [DisplayName("Asset Class")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 8)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetClass { get; set; }

        [DisplayName("Asset Type")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 9)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetType { get; set; }

        [DisplayName("Asset Description")]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [OrderGrid(Order = 10)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string AssetDescription { get; set; }

        [DisplayName("Product Importance")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 12)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ProductImportance { get; set; }

        [DisplayName("HW Model")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 14)]
        [ColorGrid(Color = "red")]
        [Default]
        public string HardwareModel { get; set; }

        [DisplayName("Product code")]
        [OrderGrid(Order = 15)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ProductCode { get; set; }

        [DisplayName("N° Nodes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 17)]
        [ColorGrid(Color = "red")]
        [Default]
        public string NumberOfNodes { get; set; }

        [DisplayName("Handed Over to Operation")]
        [OrderGrid(Order = 18)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string HandedOverToOperation { get; set; }

        [DisplayName("Contract Renewal Plan")]
        [OrderGrid(Order = 20)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string ContractRenewalPlan { get; set; }

        [DisplayName("LCM Status")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 23)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string LcmStatus { get; set; }

        [DisplayName("Identified action")]
        [OrderGrid(Order = 24)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string IdentifiedAction { get; set; }

        [OrderGrid(Order = 25)]
        [DisplayName("Description of Planned Action")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string DescriptionOfPlannedAction { get; set; }

        [DisplayName("Project Status")]
        [OrderGrid(Order = 27)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ProjectStatus { get; set; }

        [DisplayName("Reason for no plan")]
        [OrderGrid(Order = 28)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ReasonfornoPlan { get; set; }


        [DisplayName("Comment on Project Status")]
        [OrderGrid(Order = 29)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string CommentonProjectStatus { get; set; }

        [OrderGrid(Order = 30)]
        [Default]
        [DisplayName("Project End Date")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        public string ProjectEndDate { get; set; }

        [DisplayName("RAG Status")]
        [OrderGrid(Order = 31)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RagStatus { get; set; }

        [DisplayName("Tracking Number / Project Name")]
        [OrderGrid(Order = 32)]
        [Default]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        public string TrackingNumberProjectName { get; set; }

        [DisplayName("Program")]
        [OrderGrid(Order = 33)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Program { get; set; }

        [DisplayName("WBS Code")]
        [OrderGrid(Order = 34)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string WbsCode { get; set; }

        [DisplayName("BPT ID")]
        [OrderGrid(Order = 35)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string BptID { get; set; }

        [DisplayName("PPM ID")]
        [OrderGrid(Order = 36)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string PpmID { get; set; }

        [DisplayName("Scope of simplification")]
        [OrderGrid(Order = 37)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ScopeOfSimplification { get; set; }


        [DisplayName("Data Source (Project Code)")]
        [OrderGrid(Order = 38)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string DataSource { get; set; }

        [DisplayName("Project Owner")]
        [OrderGrid(Order = 39)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ProjectOwner { get; set; }

        [OrderGrid(Order = 40)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [DisplayName("Budget Estimated")]
        [Default]
        public string BudgetEstimated { get; set; }

        [OrderGrid(Order = 41)]
        [DisplayName("Notes")]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Notes { get; set; }

        [OrderGrid(Order = 42)]
        [DisplayName("Bundle Budget")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string BundleBudget { get; set; }

        [OrderGrid(Order = 43)]
        [DisplayName("Bundle ID")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string BundleId { get; set; }

        [OrderGrid(Order = 44)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [DisplayName("Business Service Name")]
        [Default]
        public string AssetServiceFunctionality { get; set; }

        [OrderGrid(Order = 45)]
        [DisplayName("Platform")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string Platform { get; set; }

        [OrderGrid(Order = 49)]
        [DisplayName("ENG risk evaluation")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngRiskEvaluation { get; set; }
        [OrderGrid(Order = 50)]
        [DisplayName("ENG Risk evaluation notes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngRiskEvaluationNotes { get; set; }
        [OrderGrid(Order = 51)]
        [DisplayName("OPS risk evaluation")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsRiskEvaluation { get; set; }

        [OrderGrid(Order = 52)]
        [DisplayName("OPS Risk evaluation notes")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsRiskEvaluationNotes { get; set; }

        [DisplayName("Incident Class")]
        [OrderGrid(Order = 53)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string IncidentClass { get; set; }

        [DisplayName("Occurrence Probability")]
        [OrderGrid(Order = 54)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OccurrenceProbability { get; set; }

        [DisplayName("NEW OPS Risk Evaluation")]
        [OrderGrid(Order = 55)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string NewopsRiskEvaluation { get; set; }

        [OrderGrid(Order = 56)]
        [DisplayName("Overall risk evaluation")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string OverallRiskEvaluation { get; set; }

        [DisplayName("Risk Cluster")]
        [OrderGrid(Order = 57)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RiskCluster { get; set; }

        [DisplayName("Security Risk (Potential)")]
        [OrderGrid(Order = 58)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SecurityRiskPotential { get; set; }

        [DisplayName("Vulnerability Score")]
        [OrderGrid(Order = 59)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string VulnerabilityScore { get; set; }

        [DisplayName("Comments")]
        [OrderGrid(Order = 60)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Comments { get; set; }

        [DisplayName("QID")]
        [OrderGrid(Order = 61)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string QId { get; set; }

        [DisplayName("Request ID")]
        [OrderGrid(Order = 62)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RequestID { get; set; }

        [DisplayName("Vulnerability Rating")]
        [OrderGrid(Order = 63)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string VulnerabilityRating { get; set; }

        [DisplayName("Security Risk (Effective)")]
        [OrderGrid(Order = 64)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SecurityRiskEffective { get; set; }

        [DisplayName("Security Mitigation")]
        [OrderGrid(Order = 65)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string SecurityMitigation { get; set; }


        [DisplayName("Security Risk Overall")]
        [OrderGrid(Order = 66)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SecurityRiskOverall { get; set; }

        [DisplayName("Included in Security Scanning")]
        [OrderGrid(Order = 67)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string IncludedinSecurityScanning { get; set; }

        [DisplayName("RA ID")]
        [OrderGrid(Order = 68)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RaId { get; set; }

        [DisplayName("LCM Cumulative Risk ID")]
        [OrderGrid(Order = 69)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string LcmCumulativeRiskId { get; set; }

        [DisplayName("LCM Cumulative Risk Level")]
        [OrderGrid(Order = 70)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string LcmCumulativeRiskLevel { get; set; }

        [DisplayName("Cyber Risk Request ID")]
        [OrderGrid(Order = 71)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string CyberRiskRequestId { get; set; }

        [DisplayName("Criticality")]
        [OrderGrid(Order = 72)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Criticality { get; set; }

        [DisplayName("Asset Status")]
        [OrderGrid(Order = 73)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string AssetStatus { get; set; }

        [DisplayName("GDPR Relevant")]
        [OrderGrid(Order = 74)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string GdprRelevant { get; set; }

        [DisplayName("Last Scan Date")]
        [OrderGrid(Order = 75)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string LastScanDate { get; set; }

        [DisplayName("Last Upgrade Date")]
        [DateRangeGrid]
        [OrderGrid(Order = 76)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string LastUpgradeDate { get; set; }

        [OrderGrid(Order = 77)]
        [DisplayName("Asset out of scope for reporting purposes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetOutofScopeForReportingPurposes { get; set; }

        [DisplayName("Main Organization")]
        [OrderGrid(Order = 78)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]

        public string MainOrganization { get; set; }

        [DisplayName("Extended support option offered by vendor")]
        [OrderGrid(Order = 79)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string IsExtendedSupportOfferedByVendor { get; set; }

        [OrderGrid(Order = 80)]
        [DisplayName("EOM_control")]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string EomControl { get; set; }

        [OrderGrid(Order = 81)]
        [DisplayName("ENG Update Tracker")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngUpdateTracker { get; set; }

        [OrderGrid(Order = 82)]
        [DisplayName("OPS Update Tracker")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsUpdateTracker { get; set; }

        [DisplayName("Type of Network Element")]
        [OrderGrid(Order = 83)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string TypeOfNetworkElement { get; set; }


        [DisplayName("ENG KPI 2")]
        [OrderGrid(Order = 84)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string EngKpi2 { get; set; }

        [DisplayName("IP Address")]
        [OrderGrid(Order = 85)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string IpAddress { get; set; }


        [DisplayName("Serial Number")]
        [OrderGrid(Order = 86)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string SerialNumber { get; set; }

        [DisplayName("Hostname")]
        [OrderGrid(Order = 87)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Hostname { get; set; }        

        [DisplayName("Ex Networks")]
        [OrderGrid(Order = 88)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ExNetworks { get; set; }

        [DisplayName("Original LCM Spreadsheet ID")]
        [OrderGrid(Order = 89)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string OriginalLcmId { get; set; }
    }

    public class PassThroughLcmSoftwareDtoGrid : PassThroughLcmDtoGrid
    {
        [DisplayName("SwResourceKey")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 96)]
        [ColorGrid(Color = "blue")]
        [FormatClosetXml(Type = XLDataType.Text)]
        [IgnoreGrid]
        public string SwResourceKeyPassThrough { get; set; }

        [DisplayName("SW Operations Contact Point")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [OrderGrid(Order = 6)]
        [ColorGrid(Color = "green")]
        [Default]
        public string SwOperationsContactPoint { get; set; }

        [DisplayName("Asset virtualized")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 11)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetVirtualized { get; set; }

        [DisplayName("SW Vendor")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 13)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SwVendor { get; set; }

        [DisplayName("SW Release")]
        [Format(FormatType = "Text")]
        [OrderGrid(Order = 16)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SoftwareVersion { get; set; }

        [DisplayName("SW Operations Maintenance Contract")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [OrderGrid(Order = 19)]
        [ColorGrid(Color = "green")]
        [Default]
        public string SwOperationsMaintenanceContract { get; set; }

        [DisplayName("SW Vendor End of Maintenance Date")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 21)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SwVendorEndOfMaintenanceDate { get; set; }

        [OrderGrid(Order = 22)]
        [Default]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [DisplayName("Vendor End of Vulnerability /Security Support Date")]
        [ColorGrid(Color = "gray")]
        public string VendorEndOfVulnerabilitySecuritySupportDate { get; set; }

        [DisplayName("Planned SW Release")]
        [OrderGrid(Order = 26)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string PlannedSoftwareVersion { get; set; }

        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [OrderGrid(Order = 46)]
        [Default]
        [DisplayName("LCM Status ENG")]
        public string LcmStatusEngSoftware { get; set; }


        [OrderGrid(Order = 47)]
        [DisplayName("LCM Status OPS")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string LcmStatusOpsSoftware { get; set; }

        [OrderGrid(Order = 48)]
        [DisplayName("SW OPS Maintenance contract end date")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string SwOpsMaintenanceConractEndDate { get; set; }
        [OrderGrid(Order = 90)]
        [DisplayName("Application/Operating SYS")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ApplicationOperatingSys { get; set; }
        [OrderGrid(Order = 91)]
        [DisplayName("EOFS Date")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EofsDate { get; set; }
        [OrderGrid(Order = 92)]
        [DisplayName("EOSL KPI Daily")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EoslKpidaily { get; set; }
        [OrderGrid(Order = 93)]
        [DisplayName("EOSL KPI Frozen")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EoslKpiFrozen { get; set; }
        [OrderGrid(Order = 94)]
        [DisplayName("EOSL KPI Forecast")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EoslKpiForecast { get; set; }
        [OrderGrid(Order = 95)]
        [DisplayName("Product Importance History2")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ProductImportanceHistory2 { get; set; }
    }

    public class PassThroughLcmHardwareDtoGrid : PassThroughLcmDtoGrid
    {
        [DisplayName("ID")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 1)]
        [ColorGrid(Color = "blue")]
        [FormatClosetXml(Type = XLDataType.Text)]
        [IgnoreGrid]
        public string HwResourceKeyPassThrough { get; set; }

        [DisplayName("HW Operations Contact Point")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [OrderGrid(Order = 6)]
        [ColorGrid(Color = "green")]
        [Default]
        public string HwOperationsContactPoint { get; set; }

        [DisplayName("HW Vendor")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 13)]
        [ColorGrid(Color = "red")]
        [Default]
        public string HwVendor { get; set; }

        [DisplayName("HW Operations Maintenance Contract")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [OrderGrid(Order = 19)]
        [ColorGrid(Color = "green")]
        [Default]
        public string HwOperationsMaintenanceContract { get; set; }

        [DisplayName("HW Vendor End of Maintenance Date")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 21)]
        [ColorGrid(Color = "red")]
        [Default]
        public string HwVendorEndOfMaintenanceDate { get; set; }

        [DisplayName("Planned HW Model")]
        [OrderGrid(Order = 26)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string PlannedHWModel { get; set; }

        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [OrderGrid(Order = 46)]
        [Default]
        [DisplayName("LCM Status ENG")]
        public string LcmStatusEngHardware { get; set; }


        [OrderGrid(Order = 47)]
        [DisplayName("LCM Status OPS")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string LcmStatusOpsHardware { get; set; }

        [OrderGrid(Order = 48)]
        [DisplayName("HW OPS Maintenance contract end date")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string HwOpsMaintenanceConractEndDate { get; set; }
    }
}
