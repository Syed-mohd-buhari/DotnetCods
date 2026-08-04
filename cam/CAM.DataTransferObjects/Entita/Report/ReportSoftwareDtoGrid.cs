using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using CAM.Enum;
using ClosedXML.Excel;

namespace CAM.DataTransferObjects.Entita.Report
{

    public class ReportSoftwareDtoGrid : ReportDto
    {

        [DisplayName("Asset virtualized")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 14)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetVirtualized { get; set; }

        [DisplayName("Product Importance")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 15)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ProductImportance { get; set; }

        [DisplayName("Vendor")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 16)]
        [ColorGrid(Color = "red")]
        [Default]
        public string Vendor { get; set; }

        [DisplayName("Application/Operating SYS")]
        [OrderGrid(Order = 17)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ApplicationOrOperationgSys { get; set; }

        [DisplayName("HW Model")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 18)]
        [ColorGrid(Color = "red")]
        [Default]
        public string HardwareModel { get; set; }

        [DisplayName("Product code")]
        [OrderGrid(Order = 19)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ProductCode { get; set; }

        [DisplayName("SW Release")]
        [Format(FormatType = "Text")]
        [OrderGrid(Order = 20)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SoftwareVersion { get; set; }

        [DisplayName("Number of Nodes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 21)]
        [ColorGrid(Color = "red")]
        [Default]
        public int NumberOfNodesLcm { get; set; }

        [DisplayName("EOX")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [OrderGrid(Order = 22)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OperationsMaintenanceContractLcm { get; set; }

        [IgnoreGrid]
        public DateTime? VendorEndOfVulnerabilitySecuritySupportDate { get; set; }

        [Default]
        [DateRangeGrid]
        [DisplayName("EOFS Date")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 23)]
        [ColorGrid(Color = "red")]
        public string VendorEndOfVulnerabilitySecuritySupportDateValueLcm { get; set; }
        
        [DisplayName("Asset Status FY26")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 24)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string AssetStatusFY26 { get; set; }

        [DisplayName("Asset Status FY27")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [IgnoreGrid]
        public string AssetStatusFY27 { get; set; }

        [DisplayName("Asset Status FY28")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 25)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string AssetStatusFY28 { get; set; }

        [DisplayName("EOSL KPI Frozen")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 26)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string EoslKpiFrozen { get; set; }

        [DisplayName("EOSL KPI Forecast")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 27)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string EoslKpiForecast { get; set; }

        [DisplayName("Exception Flag")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 28)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string ExceptionFlag { get; set; }

        [OrderGrid(Order = 30)]
        [DisplayName("Description of Planned Action")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string DescriptionOfPlannedAction { get; set; }

        [DisplayName("Planned SW Release")]
        [OrderGrid(Order = 31)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string PlannedSoftwareVersion { get; set; }

        [DisplayName("Project Status")]
        [OrderGrid(Order = 32)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ProjectStatus { get; set; }

        [DisplayName("Comment on Project Status")]
        [OrderGrid(Order = 33)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string CommentonProjectStatus { get; set; }

        [IgnoreGrid]
        public DateTime? ProjectEndDate { get; set; }

        [OrderGrid(Order = 34)]
        [Default]
        [DisplayName("Project End Date")]
        [DateRangeGrid]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        public string ProjectEndDateValue { get; set; }

        [DisplayName("Program")]
        [OrderGrid(Order = 35)]
        [Default]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        public string TrackingNumberProjectNameLcm { get; set; }

        [DisplayName("Program2")]
        [OrderGrid(Order = 36)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ProgramLcm { get; set; }

        [DisplayName("WBS Code")]
        [OrderGrid(Order = 37)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string WbsCode { get; set; }

        [DisplayName("BPT ID")]
        [OrderGrid(Order = 38)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string BptID { get; set; }

        [DisplayName("PPM ID")]
        [OrderGrid(Order = 39)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string PpmID { get; set; }

        [DisplayName("Scope of simplification")]
        [OrderGrid(Order = 40)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ScopeOfSimplification { get; set; }

        [DisplayName("Project WBS Owner")]
        [OrderGrid(Order = 41)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string DataSourceLcm { get; set; }

        [DisplayName("Project Owner")]
        [OrderGrid(Order = 42)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ProjectOwner { get; set; }

        [OrderGrid(Order = 43)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [DisplayName("Budget Estimated")]
        [Default]
        public string BudgetEstimated { get; set; }

        [OrderGrid(Order = 44)]
        [DisplayName("Notes")]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Notes { get; set; }

        [OrderGrid(Order = 45)]
        [DisplayName("Platform")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string Platform { get; set; }
        [IgnoreGrid]
        public DateTime? OpsMaintenanceConractEnd { get; set; }

        [OrderGrid(Order = 46)]
        [DisplayName("EOX Date")]
        [DateRangeGrid]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsMaintenanceConractEndValueLcm { get; set; }

        [DisplayName("Incident Class")]
        [OrderGrid(Order = 47)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string IncidentClass { get; set; }

        [DisplayName("Occurrence Probability")]
        [OrderGrid(Order = 48)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OccurrenceProbability { get; set; }

        [OrderGrid(Order = 49)]
        [DisplayName("OPS Risk Evaluation")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string OverallRiskEvaluationLcm { get; set; }

        [DisplayName("Risk Cluster")]
        [OrderGrid(Order = 50)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RiskCluster { get; set; }

        [DisplayName("Security Risk (Potential)")]
        [OrderGrid(Order = 51)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SecurityRiskPotential { get; set; }

        [DisplayName("Comments")]
        [OrderGrid(Order = 53)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [IgnoreGrid]
        [Default]
        public string Comments { get; set; }

        [DisplayName("QID")]
        [OrderGrid(Order = 54)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string QId { get; set; }

        [DisplayName("Vuln. Request ID")]
        [OrderGrid(Order = 55)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RequestIDLcm { get; set; }

        [DisplayName("Vulnerability Rating")]
        [OrderGrid(Order = 56)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string VulnerabilityRating { get; set; }

        [DisplayName("Security Risk (Effective)")]
        [OrderGrid(Order = 57)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SecurityRiskEffective { get; set; }

        [DisplayName("RA ID")]
        [OrderGrid(Order = 59)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RaId { get; set; }

        [DisplayName("Cyber Risk Request ID")]
        [OrderGrid(Order = 62)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string CyberRiskRequestId { get; set; }

        [OrderGrid(Order = 67)]
        [DisplayName("Asset out of scope for reporting purposes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetOutofScopeForReportingPurposes { get; set; }

        [DisplayName("Main Organization")]
        [OrderGrid(Order = 68)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string MainOrganization { get; set; }

        [OrderGrid(Order = 71)]
        [DisplayName("ENG Update Tracker")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngUpdateTracker { get; set; }

        [OrderGrid(Order = 72)]
        [DisplayName("OPS Update Tracker")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsUpdateTracker { get; set; }

        [DisplayName("IP Address")]
        [OrderGrid(Order = 73)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string IpAddress { get; set; }

        [DisplayName("Serial Number")]
        [OrderGrid(Order = 74)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string SerialNumber { get; set; }

        [DisplayName("Hostname")]
        [OrderGrid(Order = 75)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Hostname { get; set; }

        [DisplayName("Ex Networks")]
        [OrderGrid(Order = 76)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ExNetworks { get; set; }

        [DisplayName("Original LCM ID")]
        [OrderGrid(Order = 77)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string OriginalSwLcmId { get; set; }

        [DisplayName("EOSL KPI Target")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 78)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string EoslKpiTarget { get; set; }

        [DisplayName("Risk Comment")]
        [OrderGrid(Order = 79)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RiskComment { get; set; }

        [DisplayName("Handed Over to Operation")]
        [OrderGrid(Order = 80)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public bool? HandedOverToOperation { get; set; }

        [DisplayName("Contract Renewal Plan")]
        [OrderGrid(Order = 81)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string ContractRenewalPlan { get; set; }

        [DisplayName("Vulnerability Score")]
        [OrderGrid(Order = 82)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string VulnerabilityScore { get; set; }

        [DisplayName("Security Final Risk")]
        [OrderGrid(Order = 83)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SecurityRiskOverallLcm { get; set; }

        [DisplayName("LCM Cumulative Risk ID")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [IgnoreGrid]
        [Default]
        public string LcmCumulativeRiskId { get; set; }

        [DisplayName("LCM Cumulative Risk Level")]
        [OrderGrid(Order = 84)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string LcmCumulativeRiskLevel { get; set; }

        [DisplayName("Criticality")]
        [OrderGrid(Order = 85)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Criticality { get; set; }

        [DisplayName("GDPR Relevant")]
        [OrderGrid(Order = 86)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string GdprRelevant { get; set; }

        [IgnoreGrid]
        public DateTime? LastScanDate { get; set; }

        [DisplayName("Last Scan Date")]
        [DateRangeGrid]
        [OrderGrid(Order = 87)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public DateTime? LastScanDateValue { get; set; }


        [IgnoreGrid]
        public DateTime? LastUpgradeDate { get; set; }

        [DisplayName("Last Upgrade Date")]
        [DateRangeGrid]
        [OrderGrid(Order = 89)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public DateTime? LastUpgradeDateValue { get; set; }

        [DisplayName("Extended support option offered by vendor")]
        [OrderGrid(Order = 90)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string IsExtendedSupportOfferedByVendor { get; set; }

        [OrderGrid(Order = 91)]
        [DisplayName("EOM_control")]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string EomControl { get; set; }

        [OrderGrid(Order = 48)]
        [DisplayName("Type")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string Type { get; set; }

        [DisplayName("Status Of The Platform")]
        [OrderGrid(Order = 67)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string StatusOfThePlatform { get; set; }

        [DisplayName("OPS_Sys")]
        [OrderGrid(Order = 68)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string OpsSys { get; set; }

        #region Ignore grid columns


        [DisplayName("LCM Status FY26")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 23)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string LcmStatus { get; set; }

        [IgnoreGrid]
        public DateTime? VendorEndOfMaintenanceDate { get; set; }

        [DisplayName("Vendor End Of Maintenance")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 22)]
        [ColorGrid(Color = "red")]
        [DateRangeGrid]
        [Default]
        [IgnoreGrid]
        public string VendorEndOfMaintenanceDateValue { get; set; }


        [DisplayName("Design Component Family")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 1)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string DesignComponentFamily { get; set; }

        [DisplayName("Supported Service")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 2)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string SupportedService { get; set; }


        //Al momento non sembrano esserci dati presenti solo per i report hardware

        [DisplayName("Identified action")]
        [OrderGrid(Order = 27)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string IdentifiedAction { get; set; }

        [DisplayName("Reason for no plan")]
        [OrderGrid(Order = 31)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string ReasonfornoPlan { get; set; }

        [DisplayName("RAG Status")]
        [OrderGrid(Order = 34)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string RagStatus { get; set; }

        [OrderGrid(Order = 45)]
        [DisplayName("Bundle Budget")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string BundleBudget { get; set; }

        [OrderGrid(Order = 46)]
        [DisplayName("Bundle ID")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string BundleId { get; set; }

        [OrderGrid(Order = 47)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [DisplayName("Business Service Name")]
        [Default]
        [IgnoreGrid]
        public string AssetServiceFunctionality { get; set; }

        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [OrderGrid(Order = 49)]
        [Default]
        [DisplayName("LCM Status ENG")]
        [IgnoreGrid]

        public string LcmStatusEngSoftware { get; set; }

        [OrderGrid(Order = 50)]
        [DisplayName("LCM Status OPS")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string LcmStatusOpsSoftware { get; set; }

        [OrderGrid(Order = 52)]
        [DisplayName("ENG risk evaluation")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string EngRiskEvaluation { get; set; }
        [OrderGrid(Order = 53)]
        [DisplayName("ENG Risk evaluation notes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string EngRiskEvaluationNotes { get; set; }
        [OrderGrid(Order = 54)]
        [DisplayName("OPS risk evaluation")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        [IgnoreGrid]
        public string OpsRiskEvaluation { get; set; }

        [OrderGrid(Order = 55)]
        [DisplayName("OPS Risk evaluation notes")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        [IgnoreGrid]
        public string OpsRiskEvaluationNotes { get; set; }

        [DisplayName("NEW OPS Risk Evaluation")]
        [OrderGrid(Order = 58)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [IgnoreGrid]
        [Default]
        public string NewopsRiskEvaluation { get; set; }

        [DisplayName("Security Mitigation")]
        [OrderGrid(Order = 68)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string SecurityMitigation { get; set; }


        [DisplayName("Included in Security Scanning")]
        [OrderGrid(Order = 70)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public bool? IncludedinSecurityScanning { get; set; }    

        [DisplayName("Asset Status")]
        [OrderGrid(Order = 76)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string AssetStatus { get; set; }

        [DisplayName("Type of Network Element")]
        [OrderGrid(Order = 86)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string TypeOfNetworkElement { get; set; }


        [DisplayName("ENG KPI 2")]
        [OrderGrid(Order = 87)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [IgnoreGrid]
        [Default]
        public string EngKpi2 { get; set; }

        [DisplayName("Custom2")]
        [OrderGrid(Order = 91)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string Custom2 { get; set; }

        [DisplayName("KPI Status Service")]
        [OrderGrid(Order = 92)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string KpiStatusService { get; set; }

        [DisplayName("Custom")]
        [OrderGrid(Order = 93)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string Custom { get; set; }

        [DisplayName("Custom1")]
        [OrderGrid(Order = 94)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string Custom1 { get; set; }

        [DisplayName("Exp. LCM status at end of FY24")]
        [OrderGrid(Order = 95)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string ExpLCMstatusatendofFY24 { get; set; }

        [DisplayName("ID_NEW")]
        [OrderGrid(Order = 96)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string Id_New { get; set; }

        [DisplayName("Product Importance History2")]
        [OrderGrid(Order = 98)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string ProductImportanceHistory2 { get; set; }

        [DisplayName("CLOUD Version")]
        [OrderGrid(Order = 99)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string CloudVersion { get; set; }

        [DisplayName("      ")]
        [OrderGrid(Order = 100)]
        [HeaderColor(BackgroundColor = 0xffffff, FontColor = 0xffffff)]
        [ColorGrid(Color = "white")]
        [Default]
        [IgnoreGrid]
        public string empty1 { get; set; }

        [DisplayName("Lab SW Release")]
        [OrderGrid(Order = 101)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string LabSWRelease { get; set; }


        [DisplayName("Certified SW release for NFVI bundle  3.2.1")]
        [OrderGrid(Order = 102)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string CertifiedSWReleaseforNFVIbundle { get; set; }

        [DisplayName("      ")]
        [OrderGrid(Order = 103)]
        [HeaderColor(BackgroundColor = 0xffffff, FontColor = 0xffffff)]
        [ColorGrid(Color = "white")]
        [IgnoreGrid]
        [Default]
        public string empty2 { get; set; }

        [DisplayName("LCM Status (june 2021)")]
        [OrderGrid(Order = 104)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string LcmStatusJune2021 { get; set; }

        [DisplayName("      ")]
        [OrderGrid(Order = 105)]
        [HeaderColor(BackgroundColor = 0xffffff, FontColor = 0xffffff)]
        [ColorGrid(Color = "white")]
        [Default]
        [IgnoreGrid]
        public string empty3 { get; set; }

        [OrderGrid(Order = 107)]
        [DisplayName("Managed by GDC")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [IgnoreGrid]
        public string ManagedByGdc { get; set; }

        [DisplayName("Delivery Plan Available")]
        [OrderGrid(Order = 108)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [IgnoreGrid]
        public string DeliveryPlanAvailable { get; set; }
        /// ///////////////////////////////////////////

        [IgnoreGrid]
        public long? DesignComponentFamilyId { get; set; }

        [IgnoreGrid]
        public bool? Archived { get; set; }
        #region  Ticket 551
        [DisplayName("IsPecn")]
        [OrderGrid(Order = 109)]
        [Default]
        [IgnoreGrid]
        public bool? IsPecn { get; set; }
        [DisplayName("IsPecs")]
        [OrderGrid(Order = 110)]
        [Default]
        [IgnoreGrid]
        public bool? IsPecs { get; set; }
        [DisplayName("IsScf")]
        [OrderGrid(Order = 111)]
        [Default]
        [IgnoreGrid]
        public bool? IsScf { get; set; }
        [DisplayName("IsNof")]
        [OrderGrid(Order = 112)]
        [Default]
        [IgnoreGrid]
        public bool? IsNof { get; set; }

        [DisplayName("Exposed Edge Flag")]
        [OrderGrid(Order = 113)]
        [Default]
        [IgnoreGrid]
        public string ExposedEdgeFlag { get; set; }

        [DisplayName("External Facing Flag")]
        [OrderGrid(Order = 114)]
        [Default]
        [IgnoreGrid]
        public string ExternalFacingFlag { get; set; }

        [DisplayName("Location Infrastructure")]
        [OrderGrid(Order = 115)]
        [Default]
        [IgnoreGrid]
        public string InfrastructureLocation { get; set; }
        #endregion
        #endregion
    }
}
