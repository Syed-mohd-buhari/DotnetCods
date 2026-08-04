using CAM.Entities.Models.Base;
using CAM.Identity;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.PassThroughData
{
    [Table("Swpassthroughlcm")]
    public class SwPassThroughLcm : AuditableEntity
    {
        public long PassThroughLcmId { get; set; }
        public long? NonTemsVertical { get; set; }
        public string ReportId { get; set; }
        public string LocalMarket { get; set; }
        public string VerticalEngineeringTeam { get; set; }
        public string VerticalSubDomain { get; set; }
        public string EngineeringContactPoint { get; set; }
        public string SWOperationsContactPoint { get; set; }
        public string AssetCategory { get; set; }
        public string AssetClass { get; set; }
        public string AssetType { get; set; }
        public string AssetDescription { get; set; }
        public string AssetVirtualized { get; set; }
        public string ProductImportance { get; set; }
        public string SWVendor { get; set; }
        public string HardwareModel { get; set; }
        public string ProductCode { get; set; }
        public string SoftwareVersion { get; set; }
        public string NumberOfNodes { get; set; }
        public string HandedOverToOperation { get; set; }
        public string SWOperationsMaintenanceContract { get; set; }
        public string ContractRenewalPlan { get; set; }
        public string SWVendorEndOfMaintenanceDate { get; set; }
        public string VendorEndOfVulnerabilitySecuritySupportDate { get; set; }
        public string LcmStatus { get; set; }
        public string IdentifiedAction { get; set; }
        public string DescriptionOfPlannedAction { get; set; }
        public string PlannedSoftwareVersion { get; set; }
        public string ProjectStatus { get; set; }
        public string ReasonfornoPlan { get; set; }
        public string CommentonProjectStatus { get; set; }
        public string ProjectEndDate { get; set; }
        public string RagStatus { get; set; }
        public string TrackingNumberProjectName { get; set; }
        public string Program { get; set; }
        public string WbsCode { get; set; }
        public string BptID { get; set; }
        public string PpmID { get; set; }
        public string ScopeOfSimplification { get; set; }
        public string DataSource { get; set; }
        public string ProjectOwner { get; set; }
        public string BudgetEstimated { get; set; }
        public string Notes { get; set; }
        public string BundleBudget { get; set; }
        public string BundleId { get; set; }
        public string AssetServiceFunctionality { get; set; }
        public string Platform { get; set; }
        public string LcmStatusEngSoftware { get; set; }
        public string LcmStatusOpsSoftware { get; set; }
        public string SWOpsMaintenanceConractEndDate { get; set; }
        public string EngRiskEvaluation { get; set; }
        public string EngRiskEvaluationNotes { get; set; }
        public string OpsRiskEvaluation { get; set; }
        public string OpsRiskEvaluationNotes { get; set; }
        public string IncidentClass { get; set; }
        public string OccurrenceProbability { get; set; }
        public string NewopsRiskEvaluation { get; set; }
        public string OverallRiskEvaluation { get; set; }
        public string RiskCluster { get; set; }
        public string SecurityRiskPotential { get; set; }
        public string VulnerabilityScore { get; set; }
        public string Comments { get; set; }
        public string QId { get; set; }
        public string RequestID { get; set; }
        public string VulnerabilityRating { get; set; }
        public string SecurityRiskEffective { get; set; }
        public string SecurityMitigation { get; set; }
        public string SecurityRiskOverall { get; set; }
        public string IncludedinSecurityScanning { get; set; }
        public string RaId { get; set; }
        public string LcmCumulativeRiskId { get; set; }
        public string LcmCumulativeRiskLevel { get; set; }
        public string CyberRiskRequestId { get; set; }
        public string Criticality { get; set; }
        public string AssetStatus { get; set; }
        public string GdprRelevant { get; set; }
        public string LastScanDate { get; set; }
        public string LastUpgradeDate { get; set; }
        public string AssetOutofScopeForReportingPurposes { get; set; }
        public string MainOrganization { get; set; }
        public string IsExtendedSupportOfferedByVendor { get; set; }
        public string EomControl { get; set; }
        public string EngUpdateTracker { get; set; }
        public string OpsUpdateTracker { get; set; }
        public string TypeOfNetworkElement { get; set; }
        public string EngKpi2 { get; set; }
        public string IpAddress { get; set; }
        public string SerialNumber { get; set; }
        public string Hostname { get; set; }
        public string ExNetworks { get; set; }
        public string OriginalLcmId { get; set; }
        public string HWOperationsContactPoint { get; set; }
        public string HWVendor { get; set; }
        public string HWOperationsMaintenanceContract { get; set; }
        public string HWVendorEndOfMaintenanceDate { get; set; }
        public string PlannedHWModel { get; set; }
        public string HWOPSMaintenanceContractEndDate { get; set; }
        public string LcmStatusEngHardware { get; set; }
        public string LcmStatusOpsHardware { get; set; }
        public string ApplicationOperatingSys { get; set; }
        public string EofsDate { get; set; }
        public string EoslKpiDaily { get; set; }
        public string EoslKpiFrozen { get; set; }
        public string EoslKpiForecast { get; set; }
        public string ProductImportanceHistory2 { get; set; }
        public string SwResourceKey { get; set; }

        public virtual AppSettingsConfiguration NonTemsVerticalNavigation { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<Assetpassthrough> Assetpassthrough { get; set; }

    }
}