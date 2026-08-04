using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class PassThroughLcmQueryDto : QueryObject
    {

        public List<long> PassThroughLcmId { get; set; }
        public List<long> NonTemsVertical { get; set; }
        public List<string> ReportId { get; set; }
        public List<string> LocalMarket { get; set; }
        public List<string> VerticalEngineeringTeam { get; set; }
        public List<string> VerticalSubDomain { get; set; }
        public List<string> EngineeringContactPoint { get; set; }
        public List<string> AssetCategory { get; set; }
        public List<string> AssetClass { get; set; }
        public List<string> AssetType { get; set; }
        public List<string> AssetDescription { get; set; }
        public List<string> AssetVirtualized { get; set; }
        public List<string> ProductImportance { get; set; }
        public List<string> HardwareModel { get; set; }
        public List<string> ProductCode { get; set; }
        public List<string> NumberOfNodes { get; set; }
        public List<string> HandedOverToOperation { get; set; }
        public List<string> ContractRenewalPlan { get; set; }
        public List<string> LcmStatus { get; set; }
        public List<string> IdentifiedAction { get; set; }
        public List<string> DescriptionOfPlannedAction { get; set; }
        public List<string> PlannedSoftwareVersion { get; set; }
        public List<string> ProjectStatus { get; set; }
        public List<string> ReasonfornoPlan { get; set; }
        public List<string> CommentonProjectStatus { get; set; }
        public List<string> ProjectEndDate { get; set; }
        public List<string> RagStatus { get; set; }
        public List<string> TrackingNumberProjectName { get; set; }
        public List<string> Program { get; set; }
        public List<string> WbsCode { get; set; }
        public List<string> BptID { get; set; }
        public List<string> PpmID { get; set; }
        public List<string> ScopeOfSimplification { get; set; }
        public List<string> DataSource { get; set; }
        public List<string> ProjectOwner { get; set; }
        public List<string> BudgetEstimated { get; set; }
        public List<string> Notes { get; set; }
        public List<string> BundleBudget { get; set; }
        public List<string> BundleId { get; set; }
        public List<string> AssetServiceFunctionality { get; set; }
        public List<string> Platform { get; set; }
        public List<string> EngRiskEvaluation { get; set; }
        public List<string> EngRiskEvaluationNotes { get; set; }
        public List<string> OpsRiskEvaluation { get; set; }
        public List<string> OpsRiskEvaluationNotes { get; set; }
        public List<string> IncidentClass { get; set; }
        public List<string> OccurrenceProbability { get; set; }
        public List<string> NewopsRiskEvaluation { get; set; }
        public List<string> OverallRiskEvaluation { get; set; }
        public List<string> RiskCluster { get; set; }
        public List<string> SecurityRiskPotential { get; set; }
        public List<string> VulnerabilityScore { get; set; }
        public List<string> Comments { get; set; }
        public List<string> QId { get; set; }
        public List<string> RequestID { get; set; }
        public List<string> VulnerabilityRating { get; set; }
        public List<string> SecurityRiskEffective { get; set; }
        public List<string> SecurityMitigation { get; set; }
        public List<string> SecurityRiskOverall { get; set; }
        public List<string> IncludedinSecurityScanning { get; set; }
        public List<string> RaId { get; set; }
        public List<string> LcmCumulativeRiskId { get; set; }
        public List<string> LcmCumulativeRiskLevel { get; set; }
        public List<string> CyberRiskRequestId { get; set; }
        public List<string> Criticality { get; set; }
        public List<string> AssetStatus { get; set; }
        public List<string> GdprRelevant { get; set; }
        public List<string> LastScanDate { get; set; }
        public List<string> LastUpgradeDate { get; set; }
        public List<string> AssetOutofScopeForReportingPurposes { get; set; }
        public List<string> MainOrganization { get; set; }
        public List<string> IsExtendedSupportOfferedByVendor { get; set; }
        public List<string> EomControl { get; set; }
        public List<string> EngUpdateTracker { get; set; }
        public List<string> OpsUpdateTracker { get; set; }
        public List<string> TypeOfNetworkElement { get; set; }
        public List<string> EngKpi2 { get; set; }
        public List<string> IpAddress { get; set; }
        public List<string> SerialNumber { get; set; }
        public List<string> Hostname { get; set; }
        public List<string> ExNetworks { get; set; }
        public List<string> OriginalLcmId { get; set; }

        #region Software
        public List<string> SoftwareVersion { get; set; }
        public List<string> SWVendor { get; set; }
        public List<string> SWOperationsContactPoint { get; set; }
        public List<string> SWOperationsMaintenanceContract { get; set; }
        public List<string> LcmStatusEngSoftware { get; set; }
        public List<string> LcmStatusOpsSoftware { get; set; }
        public List<string> SWOpsMaintenanceConractEndDate { get; set; }
        public List<string> SWVendorEndOfMaintenanceDate { get; set; }
        public List<string> VendorEndOfVulnerabilitySecuritySupportDate { get; set; }
        public List<string> ApplicationOperatingSys { get; set; }
        public List<string> EofsDate { get; set; }
        public List<string> EoslKpidaily { get; set; }
        public List<string> EoslKpiFrozen { get; set; }
        public List<string> EoslKpiForecast { get; set; }
        public List<string> ProductImportanceHistory2 { get; set; }
        public List<string> SwResourceKeyPassThrough { get; set; }

        #endregion

        #region Hardware            
        public List<string> HWOperationsContactPoint { get; set; }
        public List<string> HWVendor { get; set; }
        public List<string> HWOperationsMaintenanceContract { get; set; }
        public List<string> HWVendorEndOfMaintenanceDate { get; set; }
        public List<string> PlannedHWModel { get; set; }
        public List<string> HWOPSMaintenanceContractEndDate { get; set; }
        public List<string> LcmStatusEngHardware { get; set; }
        public List<string> LcmStatusOpsHardware { get; set; }
        public List<string> HwResourceKeyPassThrough { get; set; }
        #endregion


    }

}
