using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CAM.Enum;

namespace CAM.DataTransferObjects.Entita.Report
{
    public class ReportSubnetWorkDtoGrid
    {
        [DisplayName("Design Component Family")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 1)]
        [ColorGrid(Color = "red")]
        [Default]
        public string DesignComponentFamily { get; set; }

        [DisplayName("Supported Service")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 2)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SupportedService { get; set; }

        [DisplayName("ID")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 3)]
        [ColorGrid(Color = "blue")]
        [FormatClosetXml(Type = XLDataType.Text)]
        [Default]

        public string ReportId { get; set; }

        [DisplayName("Local Market")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 4)]
        [ColorGrid(Color = "red")]
        [Default]
        public string LocalMarket { get; set; }

        [IgnoreGrid]
        public long DesignComponentIndex { get; set; }


        [DisplayName("Vertical Engineering Team")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 5)]
        [ColorGrid(Color = "red")]
        [Default]
        public string VerticalEngineeringTeam { get; set; }
        [DisplayName("Vertical Sub-Domain")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 6)]
        [ColorGrid(Color = "red")]
        [Default]
        public string VerticalSubDomain { get; set; }
        [DisplayName("Engineering Contact Point")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 7)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngineeringContactPoint { get; set; }

        [DisplayName("Planning Contact Point")]
        [OrderGrid(Order = 8)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [IgnoreGrid]
        public string PlanningContactPoint { get; set; }

        [DisplayName("Operations Contact Point")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [OrderGrid(Order = 9)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OperationsContactPoint { get; set; }

        [DisplayName("Components")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 10)] //add
        [ColorGrid(Color = "red")]
        [Default]
        public string Components { get; set; }
        [DisplayName("Asset Category")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 11)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetCategory { get; set; }
        [DisplayName("Asset Class")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 12)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetClass { get; set; }
        [DisplayName("Asset Type")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 13)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetType { get; set; }

        //[DisplayName("Asset Description")]
        //[HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        //[OrderGrid(Order = 13)]
        //[ColorGrid(Color = "gray")]
        //[Default]
        //public string AssetDescription { get; set; }

        [IgnoreGrid]
        public EOMEnum EOMStatus { get; set; }

        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long SystemTypeId { get; set; }
        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long DesignComponentId { get; set; }
        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long? MajorSoftwareBuildId { get; set; }
        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long? MajorHardwareBuildId { get; set; }
        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long LcmEngineeringId { get; set; }

        [IgnoreGrid]
        [Ignore(Ignore = true)]
        public long? PlannedActivityId { get; set; }

        [DisplayName("Support Date")]
        [OrderGrid(Order = 101)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public DateTime? SupportDate { get; set; }

        [DisplayName("Project Scope")]
        [OrderGrid(Order = 102)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public string ProjectScope { get; set; }

        [DisplayName("New Risk Evaluation")]
        [OrderGrid(Order = 103)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public string NewRiskEvaluation { get; set; }

        [DisplayName("Incident Cluster")]
        [OrderGrid(Order = 104)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public string IncidentCluster { get; set; }

        [DisplayName("Frequency Cluster")]
        [OrderGrid(Order = 105)]
        [HeaderColor(BackgroundColor = 0x0F0D0D, FontColor = 0xffffff)]
        [ColorGrid(Color = "black")]
        [IgnoreGrid]
        public string FrequencyCluster { get; set; }


        [DisplayName("Vertical Engineering Team Id")]
        [IgnoreGrid]
        public int VerticalEngineeringTeamId { get; set; }

       

    }

    public class ReportSubnetWorkSoftWareGrid : ReportSubnetWorkDtoGrid
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
        [DisplayName("HW Model")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 17)]
        [ColorGrid(Color = "red")]
        [Default]
        public string HardwareModel { get; set; }


        [DisplayName("Product code")]
        [OrderGrid(Order = 18)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ProductCode { get; set; }

        [DisplayName("SW Release")]
        [Format(FormatType = "Text")]
        [OrderGrid(Order = 19)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SoftwareVersion { get; set; }


        [DisplayName("N° Nodes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 20)]
        [ColorGrid(Color = "red")]
        [Default]
        public int NumberOfNodes { get; set; }

        [DisplayName("Handed Over to Operation")]
        [OrderGrid(Order = 21)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public bool? HandedOverToOperation { get; set; }

        [DisplayName("Operations Maintenance Contract")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [OrderGrid(Order = 22)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OperationsMaintenanceContract { get; set; }

        [DisplayName("Contract Renewal Plan")]
        [OrderGrid(Order = 23)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string ContractRenewalPlan { get; set; }

        [IgnoreGrid]
        public DateTime? VendorEndOfMaintenanceDate { get; set; }

        [DisplayName("Vendor End of Maintenance Date")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 24)]
        [ColorGrid(Color = "red")]
        [DateRangeGrid]
        [Default]
        public string VendorEndOfMaintenanceDateValue { get; set; }

        [OrderGrid(Order = 25)]
        [Default]
        [DateRangeGrid]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [DisplayName("Vendor End of Vulnerability /Security Support Date")]
        [ColorGrid(Color = "gray")]

        public string VendorEndOfVulnerabilitySecuritySupportDateValue { get; set; }

        //Al momento non sembrano esserci dati presenti solo per i report hardware
        [DisplayName("LCM Status")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [OrderGrid(Order = 26)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string LcmStatus { get; set; }

        [DisplayName("Identified action")]
        [OrderGrid(Order = 27)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string IdentifiedAction { get; set; }

        [OrderGrid(Order = 28)]
        [DisplayName("Description of Planned Action")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string DescriptionOfPlannedAction { get; set; }

        [DisplayName("Planned SW Release")]
        [OrderGrid(Order = 29)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string PlannedSoftwareVersion { get; set; }

        [DisplayName("Project Status")]
        [OrderGrid(Order = 30)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ProjectStatus { get; set; }

        [DisplayName("Reason for no plan")]
        [OrderGrid(Order = 31)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ReasonfornoPlan { get; set; }


        [DisplayName("Comment on Project Status")]
        [OrderGrid(Order = 32)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string CommentonProjectStatus { get; set; }

        [IgnoreGrid]
        public DateTime? ProjectEndDate { get; set; }

        [OrderGrid(Order = 33)]
        [Default]
        [DisplayName("Project End Date")]
        [DateRangeGrid]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        public string ProjectEndDateValue { get; set; }

        [DisplayName("RAG Status")]
        [OrderGrid(Order = 34)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RagStatus { get; set; }


        [DisplayName("Tracking Number / Project Name")]
        [OrderGrid(Order = 35)]
        [Default]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        public string TrackingNumberProjectName { get; set; }

        [DisplayName("Program")]
        [OrderGrid(Order = 36)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Program { get; set; }

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


        [DisplayName("Data Source (Project Code)")]
        [OrderGrid(Order = 41)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string DataSource { get; set; }

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
        [DisplayName("Bundle Budget")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string BundleBudget { get; set; }

        [OrderGrid(Order = 46)]
        [DisplayName("Bundle ID")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string BundleId { get; set; }

        [OrderGrid(Order = 47)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [DisplayName("Business Service Name")]
        [Default]
        public string AssetServiceFunctionality { get; set; }

        [OrderGrid(Order = 48)]
        [DisplayName("Platform")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string Platform { get; set; }

        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [OrderGrid(Order = 49)]
        [Default]
        [DisplayName("LCM Status ENG")]
        public string LcmStatusEngSoftware { get; set; }

        [OrderGrid(Order = 50)]
        [DisplayName("LCM Status OPS")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string LcmStatusOpsSoftware { get; set; }

        [IgnoreGrid]
        public DateTime? OpsMaintenanceConractEnd { get; set; }

        [OrderGrid(Order = 51)]
        [DisplayName("OPS Maintenance contract end date")]
        [DateRangeGrid]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsMaintenanceConractEndValue { get; set; }

        [OrderGrid(Order = 52)]
        [DisplayName("ENG risk evaluation")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngRiskEvaluation { get; set; }
        [OrderGrid(Order = 53)]
        [DisplayName("ENG Risk evaluation notes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngRiskEvaluationNotes { get; set; }
        [OrderGrid(Order = 54)]
        [DisplayName("OPS risk evaluation")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsRiskEvaluation { get; set; }

        [OrderGrid(Order = 55)]
        [DisplayName("OPS Risk evaluation notes")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsRiskEvaluationNotes { get; set; }

        [DisplayName("Incident Class")]
        [OrderGrid(Order = 56)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string IncidentClass { get; set; }

        [DisplayName("Occurrence Probability")]
        [OrderGrid(Order = 57)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OccurrenceProbability { get; set; }

        [DisplayName("NEW OPS Risk Evaluation")]
        [OrderGrid(Order = 58)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string NewopsRiskEvaluation { get; set; }

        [OrderGrid(Order = 59)]
        [DisplayName("Overall risk evaluation")]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string OverallRiskEvaluation { get; set; }

        [DisplayName("Risk Cluster")]
        [OrderGrid(Order = 60)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RiskCluster { get; set; }

        [DisplayName("Security Risk (Potential)")]
        [OrderGrid(Order = 61)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SecurityRiskPotential { get; set; }

        [DisplayName("Security Risk (Effective)")]
        [OrderGrid(Order = 62)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SecurityRiskEffective { get; set; }

        [DisplayName("Security Mitigation")]
        [OrderGrid(Order = 63)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SecurityMitigation { get; set; }


        [DisplayName("Security Risk Overall")]
        [OrderGrid(Order = 64)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SecurityRiskOverall { get; set; }

        [DisplayName("Included in Security Scanning")]
        [OrderGrid(Order = 65)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public bool? IncludedinSecurityScanning { get; set; }

        [DisplayName("RA ID")]
        [OrderGrid(Order = 66)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RaId { get; set; }

        [DisplayName("Request ID")]
        [OrderGrid(Order = 67)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RequestID { get; set; }

        [DisplayName("Criticality")]
        [OrderGrid(Order = 68)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Criticality { get; set; }

        [DisplayName("Asset Status")]
        [OrderGrid(Order = 69)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string AssetStatus { get; set; }

        [DisplayName("GDPR Relevant")]
        [OrderGrid(Order = 70)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string GdprRelevant { get; set; }

        [IgnoreGrid]
        public DateTime? LastScanDate { get; set; }

        [DisplayName("Last Scan Date")]
        [DateRangeGrid]
        [OrderGrid(Order = 71)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public DateTime? LastScanDateValue { get; set; }


        [IgnoreGrid]
        public DateTime? LastUpgradeDate { get; set; }

        [DisplayName("Last Upgrade Date")]
        [DateRangeGrid]
        [OrderGrid(Order = 72)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public DateTime? LastUpgradeDateValue { get; set; }

        [OrderGrid(Order = 73)]
        [DisplayName("Asset out of scope for reporting purposes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetOutofScopeForReportingPurposes { get; set; }

        [DisplayName("Main Organization")]
        [OrderGrid(Order = 74)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]

        public string MainOrganization { get; set; }

        [DisplayName("Extended support option offered by vendor")]
        [OrderGrid(Order = 75)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string IsExtendedSupportOfferedByVendor { get; set; }

        [OrderGrid(Order = 76)]
        [DisplayName("EOM_control")]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string EomControl { get; set; }

        [OrderGrid(Order = 77)]
        [DisplayName("ENG Update Tracker")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngUpdateTracker { get; set; }

        [OrderGrid(Order = 78)]
        [DisplayName("OPS Update Tracker")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsUpdateTracker { get; set; }

        [DisplayName("Type of Network Element")]
        [OrderGrid(Order = 79)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string TypeOfNetworkElement { get; set; }


        [DisplayName("ENG KPI 2")]
        [OrderGrid(Order = 80)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string EngKpi2 { get; set; }

        [DisplayName("IP Address")]
        [OrderGrid(Order = 81)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string IpAddress { get; set; }


        [DisplayName("Serial Number")]
        [OrderGrid(Order = 82)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string SerialNumber { get; set; }

        [DisplayName("Hostname")]
        [OrderGrid(Order = 83)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Hostname { get; set; }

        [DisplayName("Custom2")]
        [OrderGrid(Order = 84)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string Custom2 { get; set; }

        [DisplayName("KPI Status Service")]
        [OrderGrid(Order = 85)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string KpiStatusService { get; set; }

        [DisplayName("Custom")]
        [OrderGrid(Order = 86)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string Custom { get; set; }

        [DisplayName("Custom1")]
        [OrderGrid(Order = 87)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        [IgnoreGrid]
        public string Custom1 { get; set; }

        [DisplayName("Exp. LCM status at end of FY24")]
        [OrderGrid(Order = 88)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string ExpLCMstatusatendofFY24 { get; set; }

        [DisplayName("ID_NEW")]
        [OrderGrid(Order = 89)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string Id_New { get; set; }

        [DisplayName("Ex Networks")]
        [OrderGrid(Order = 90)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ExNetworks { get; set; }

        [DisplayName("Product Importance History2")]
        [OrderGrid(Order = 91)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string ProductImportanceHistory2 { get; set; }

        [DisplayName("CLOUD Version")]
        [OrderGrid(Order = 92)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string CloudVersion { get; set; }

        [DisplayName("      ")]
        [OrderGrid(Order = 93)]
        [HeaderColor(BackgroundColor = 0xffffff, FontColor = 0xffffff)]
        [ColorGrid(Color = "white")]
        [Default]
        [IgnoreGrid]
        public string empty1 { get; set; }

        [DisplayName("Lab SW Release")]
        [OrderGrid(Order = 94)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string LabSWRelease { get; set; }


        [DisplayName("Certified SW release for NFVI bundle  3.2.1")]
        [OrderGrid(Order = 95)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        [IgnoreGrid]
        public string CertifiedSWReleaseforNFVIbundle { get; set; }

        [DisplayName("      ")]
        [OrderGrid(Order = 96)]
        [HeaderColor(BackgroundColor = 0xffffff, FontColor = 0xffffff)]
        [ColorGrid(Color = "white")]
        [IgnoreGrid]
        [Default]
        public string empty2 { get; set; }

        [DisplayName("LCM Status (june 2021)")]
        [OrderGrid(Order = 97)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string LcmStatusJune2021 { get; set; }

        [DisplayName("      ")]
        [OrderGrid(Order = 98)]
        [HeaderColor(BackgroundColor = 0xffffff, FontColor = 0xffffff)]
        [ColorGrid(Color = "white")]
        [Default]
        [IgnoreGrid]
        public string empty3 { get; set; }

        [DisplayName("Original LCM Spreadsheet ID")]
        [OrderGrid(Order = 99)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string OriginalSwLcmId { get; set; }


        [OrderGrid(Order = 100)]
        [DisplayName("Managed by GDC")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [IgnoreGrid]
        public string ManagedByGdc { get; set; }

        [DisplayName("Delivery Plan Available")]
        [OrderGrid(Order = 101)]
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
        [OrderGrid(Order = 102)]
        [Default]
        [IgnoreGrid]
        public bool? IsPecn { get; set; }
        [DisplayName("IsPecs")]
        [OrderGrid(Order = 103)]
        [Default]
        [IgnoreGrid]
        public bool? IsPecs { get; set; }
        [DisplayName("IsScf")]
        [OrderGrid(Order = 104)]
        [Default]
        [IgnoreGrid]
        public bool? IsScf { get; set; }
        [DisplayName("IsNof")]
        [OrderGrid(Order = 105)]
        [Default]
        [IgnoreGrid]
        public bool? IsNof { get; set; }

        [DisplayName("Exposed4Edge Flag")]
        [OrderGrid(Order = 106)]
        [Default]
        [IgnoreGrid]
        public string ExposedEdgeFlag { get; set; }

        [DisplayName("External Facing Flag")]
        [OrderGrid(Order = 107)]
        [Default]
        [IgnoreGrid]
        public string ExternalFacingFlag { get; set; }

        [DisplayName("Location Infrastructure")]
        [OrderGrid(Order = 108)]
        [Default]
        [IgnoreGrid]
        public string InfrastructureLocation { get; set; }
        #endregion
    }
    public class ReportSubnetWorkHardWareGrid : ReportSubnetWorkDtoGrid
    {

        [DisplayName("Product Importance")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 14)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ProductImportance { get; set; }

        [DisplayName("Vendor")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 15)]
        [ColorGrid(Color = "red")]
        [Default]
        public string Vendor { get; set; }
        [DisplayName("HW Model")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 16)]
        [ColorGrid(Color = "red")]
        [Default]
        public string HardwareModel { get; set; }

        [DisplayName("Product code")]
        [OrderGrid(Order = 17)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ProductCode { get; set; }

        [DisplayName("N° Nodes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 18)]
        [ColorGrid(Color = "red")]
        [Default]
        public int NumberOfNodes { get; set; }

        [DisplayName("Handed Over to Operation")]
        [OrderGrid(Order = 19)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public bool? HandedOverToOperation { get; set; }

        [DisplayName("Operations Maintenance Contract")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [OrderGrid(Order = 20)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OperationsMaintenanceContract { get; set; }

        [DisplayName("Contract Renewal Plan")]
        [OrderGrid(Order = 21)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string ContractRenewalPlan { get; set; }

        [IgnoreGrid]
        public DateTime? VendorEndOfMaintenanceDate { get; set; }

        [DisplayName("Vendor End of Maintenance Date")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [OrderGrid(Order = 22)]
        [ColorGrid(Color = "red")]
        [DateRangeGrid]
        [Default]
        public string VendorEndOfMaintenanceDateValue { get; set; }
        //Al momento non sembrano esserci dati presenti solo per i report hardware
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

        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [DisplayName("Planned HW Model")]
        [ColorGrid(Color = "gray")]
        [OrderGrid(Order = 26)]
        [Default]
        public string PlannedHardwareModel { get; set; }
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

        [IgnoreGrid]
        public DateTime? ProjectEndDate { get; set; }

        [OrderGrid(Order = 30)]
        [Default]
        [DisplayName("Project End Date")]
        [DateRangeGrid]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        public string ProjectEndDateValue { get; set; }

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


        /// ////////////////////////////////////
        /// ////////////////////////////////////
        /// 

        [OrderGrid(Order = 45)]
        [DisplayName("Platform")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string Platform { get; set; }
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

        [IgnoreGrid]
        public DateTime? OpsMaintenanceConractEnd { get; set; }

        [OrderGrid(Order = 48)]
        [DisplayName("OPS Maintenance contract end date")]
        [DateRangeGrid]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsMaintenanceConractEndValue { get; set; }

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

        [DisplayName("Security Risk (Effective)")]
        [OrderGrid(Order = 59)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SecurityRiskEffective { get; set; }

        [DisplayName("Security Mitigation")]
        [OrderGrid(Order = 60)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SecurityMitigation { get; set; }


        [DisplayName("Security Risk Overall")]
        [OrderGrid(Order = 61)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SecurityRiskOverall { get; set; }

        [DisplayName("Included in Security Scanning")]
        [OrderGrid(Order = 62)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public bool? IncludedinSecurityScanning { get; set; }

        [DisplayName("RA ID")]
        [OrderGrid(Order = 63)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RaId { get; set; }

        [DisplayName("Request ID")]
        [OrderGrid(Order = 64)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RequestID { get; set; }

        [DisplayName("Criticality")]
        [OrderGrid(Order = 65)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Criticality { get; set; }

        [DisplayName("Asset Status")]
        [OrderGrid(Order = 66)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string AssetStatus { get; set; }

        [DisplayName("GDPR Relevant")]
        [OrderGrid(Order = 67)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string GdprRelevant { get; set; }

        [IgnoreGrid]
        public DateTime? LastScanDate { get; set; }

        [DisplayName("Last Scan Date")]
        [OrderGrid(Order = 68)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [DateRangeGrid]
        [Default]
        public DateTime? LastScanDateValue { get; set; }


        [IgnoreGrid]
        public DateTime? LastUpgradeDate { get; set; }

        [DisplayName("Last Upgrade Date")]
        [OrderGrid(Order = 69)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [DateRangeGrid]
        [Default]
        public DateTime? LastUpgradeDateValue { get; set; }

        [OrderGrid(Order = 70)]
        [DisplayName("Asset out of scope for reporting purposes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetOutofScopeForReportingPurposes { get; set; }

        [DisplayName("Main Organization")]
        [OrderGrid(Order = 71)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]

        public string MainOrganization { get; set; }

        [DisplayName("Extended support option offered by vendor")]
        [OrderGrid(Order = 72)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string HwIsExtendedSupportOfferedByVendor { get; set; }


        [OrderGrid(Order = 73)]
        [DisplayName("EOM_control")]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string EomControl { get; set; }

        [OrderGrid(Order = 74)]
        [DisplayName("ENG Update Tracker")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngUpdateTracker { get; set; }

        [OrderGrid(Order = 75)]
        [DisplayName("OPS Update Tracker")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsUpdateTracker { get; set; }

        [DisplayName("Type of Network Element")]
        [OrderGrid(Order = 76)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string TypeOfNetworkElement { get; set; }


        [DisplayName("ENG KPI 2")]
        [OrderGrid(Order = 77)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]

        public string EngKpi2 { get; set; }

        [DisplayName("IP Address")]
        [OrderGrid(Order = 78)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string IpAddress { get; set; }



        [DisplayName("Serial Number")]
        [OrderGrid(Order = 79)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string SerialNumber { get; set; }

        [DisplayName("Hostname")]
        [OrderGrid(Order = 80)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string Hostname { get; set; }

        [DisplayName("Custom2")]
        [OrderGrid(Order = 81)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [IgnoreGrid]
        [Default]
        public string Custom2 { get; set; }

        [DisplayName("KPI Status Service")]
        [OrderGrid(Order = 82)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string KpiStatusService { get; set; }

        [DisplayName("Custom")]
        [OrderGrid(Order = 83)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [IgnoreGrid]
        [Default]
        public string Custom { get; set; }

        [DisplayName("Custom1")]
        [OrderGrid(Order = 84)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [IgnoreGrid]
        [Default]
        public string Custom1 { get; set; }

        [DisplayName("Exp. LCM status at end of FY24")]
        [OrderGrid(Order = 85)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string ExpLCMstatusatendofFY24 { get; set; }

        [DisplayName("ID_NEW")]
        [OrderGrid(Order = 86)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string Id_New { get; set; }

        [DisplayName("Ex Networks")]
        [OrderGrid(Order = 87)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ExNetworks { get; set; }

        [DisplayName("Product Importance History2")]
        [OrderGrid(Order = 88)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string ProductImportanceHistory2 { get; set; }

        [DisplayName("      ")]
        [OrderGrid(Order = 89)]
        [HeaderColor(BackgroundColor = 0xffffff, FontColor = 0xffffff)]
        [ColorGrid(Color = "white")]
        [Default]
        [IgnoreGrid]
        public string empty1 { get; set; }


        [DisplayName("LCM Status (june 2021)")]
        [OrderGrid(Order = 90)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        [IgnoreGrid]
        public string LcmStatusJune2021 { get; set; }

        [DisplayName("      ")]
        [OrderGrid(Order = 91)]
        [HeaderColor(BackgroundColor = 0xffffff, FontColor = 0xffffff)]
        [ColorGrid(Color = "white")]
        [Default]
        [IgnoreGrid]
        public string empty2 { get; set; }

        [DisplayName("Original LCM Spreadsheet ID")]
        [OrderGrid(Order = 92)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string OriginalHwLcmId { get; set; }

        [OrderGrid(Order = 93)]
        [DisplayName("Managed by GDC")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [IgnoreGrid]
        public string ManagedByGdc { get; set; }

        [DisplayName("Delivery Plan Available")]
        [OrderGrid(Order = 94)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [IgnoreGrid]
        public string DeliveryPlanAvailable { get; set; }

        /// ///////////////////////////////////////////

        [IgnoreGrid]
        public long? DesignComponentFamilyId { get; set; }

        [IgnoreGrid]
        public bool? Archived { get; set; }

        [IgnoreGrid]
        public int VerticalEngineeringTeamId { get; set; }

        #region  Ticket 551
        [DisplayName("IsPecn")]
        [OrderGrid(Order = 95)]
        [Default]
        [IgnoreGrid]
        public bool? IsPecn { get; set; }
        [DisplayName("IsPecs")]
        [OrderGrid(Order = 96)]
        [Default]
        [IgnoreGrid]
        public bool? IsPecs { get; set; }
        [DisplayName("IsScf")]
        [OrderGrid(Order = 97)]
        [Default]
        [IgnoreGrid]
        public bool? IsScf { get; set; }
        [DisplayName("IsNof")]
        [OrderGrid(Order = 98)]
        [Default]
        [IgnoreGrid]
        public bool? IsNof { get; set; }

        [DisplayName("Exposed Edge Flag")]
        [OrderGrid(Order = 99)]
        [Default]
        [IgnoreGrid]
        public string ExposedEdgeFlag { get; set; }

        [DisplayName("External Facing Flag")]
        [OrderGrid(Order = 100)]
        [Default]
        [IgnoreGrid]
        public string ExternalFacingFlag { get; set; }

        [DisplayName("Location Infrastructure")]
        [OrderGrid(Order = 101)]
        [Default]
        [IgnoreGrid]
        public string InfrastructureLocation { get; set; }
        #endregion
    }
}
