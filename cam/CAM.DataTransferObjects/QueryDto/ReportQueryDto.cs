using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Enum;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public abstract class ReportQueryDto : QueryObject
    {
        public List<string> ReportId { get; set; }
        public List<string> PreviousReportId { get; set; }

        public string Name { get; set; }
        public List<string> LocalMarket { get; set; }
        public List<short> LocalMarketId { get; set; }
        public List<string> DesignComponentIndex { get; set; }
        public List<string> VerticalEngineeringTeam { get; set; }

        public List<string> VerticalSubDomain { get; set; }
        public List<string> EngineeringContactPoint { get; set; }
        // public List<short> OperationsContactPoint { get; set; }
        public List<string> OperationsContactPoint { get; set; }
        public List<string> AssetCategory { get; set; }
        public List<string> AssetClass { get; set; }
        public List<string> AssetType { get; set; }
        public List<string> AssetDescription { get; set; }
        public List<string> ProductImportance { get; set; }
        public List<string> Vendor { get; set; }
        public List<string> HardwareModel { get; set; }
        public List<int> NumberOfNodesLcm { get; set; }
        public List<string> OperationsMaintenanceContractLcm { get; set; }
        public DateFilter VendorEndOfMaintenanceDateValue { get; set; }
        public List<string> LcmStatus { get; set; }
        public List<int> PlannedAction { get; set; }
        public List<string> DescriptionOfPlannedAction { get; set; }
        public List<string> PlannedSoftwareVersion { get; set; }

        public List<string> PlannedHardwareModel { get; set; }

        public List<string> ProjectStatus { get; set; }
        public DateFilter ProjectEndDateValue { get; set; }
        public List<string> TrackingNumberProjectNameLcm { get; set; }
        public List<string> Notes { get; set; }

        public List<string> BundleBudget { get; set; }
        public List<string> BundleId { get; set; }
        public List<string> AssetServiceFunctionality { get; set; }
        public List<string> Platform { get; set; }
        //public List<string> LcmStatusEng { get; set; }
        //public List<string> LcmStatusOps { get; set; }
        public DateFilter OpsMaintenanceConractEndValueLcm { get; set; }
        public List<int> EngRiskEvaluation { get; set; }
        public List<string> EngRiskEvaluationNotes { get; set; }
        public List<int> OpsRiskEvaluation { get; set; }
        public List<string> OpsRiskEvaluationNotes { get; set; }

        public List<string> OverallRiskEvaluation { get; set; }
        public List<string> IdentifiedAction { get; set; }
        public List<string> BudgetEstimated { get; set; }

        public List<string> ManagedByGdc { get; set; }
        public DateFilter ExtendedSupportOptionOfferedByVendor { get; set; }


        public List<string> ENGKPI2 { get; set; }

        public List<string> ExpLCMstatusatendofFY24 { get; set; }


        #region LCM R8 Phase 1  
        public List<int> RiskCluster { get; set; }

        public List<string> Criticality { get; set; }

        public List<string> GdprRelevant { get; set; }

        public List<string> DeliveryPlanAvailable { get; set; }

        public List<string> Hostname { get; set; }

        public List<string> IpAddress { get; set; }

        public List<string> RagStatus { get; set; }

        public List<string> WbsCode { get; set; }

        public List<string> BptID { get; set; }

        public List<string> PpmID { get; set; }

        public List<string> SerialNumber { get; set; }

        public List<string> AssetStatus { get; set; }
        public List<string> ProgramLcm { get; set; }
        public List<string> ProjectOwner { get; set; }

        #endregion

        #region R9 Part - 1
        //public List<string> Custom { get; set; }
        //public List<string> Custom1 { get; set; }
        //public List<string> Custom2 { get; set; }
        //public List<string> KpiStatusService { get; set; }
        public List<string> ReasonfornoPlan { get; set; }
        public List<string> CommentonProjectStatus { get; set; }
        public List<string> SecurityRiskPotential { get; set; }
        public List<string> SecurityRiskEffective { get; set; }
        public List<string> SecurityMitigation { get; set; }
        public List<string> SecurityRiskOverallLcm { get; set; }
        public List<bool> IncludedinSecurityScanning { get; set; }
        public List<string> RaId { get; set; }
        public List<string> RequestIDLcm { get; set; }
        //public List<string> Id_New { get; set; }
        //public List<string> ProductImportanceHistory2 { get; set; }
        // public List<string> LcmStatusJune2021 { get; set; }
        public DateFilter LastScanDate { get; set; }
        public DateFilter LastScanDateValue { get; set; }
        public List<string> AssetOutofScopeForReportingPurposes { get; set; }
        public DateFilter LastUpgradeDate { get; set; }
        public DateFilter LastUpgradeDateValue { get; set; }
        public List<string> EomControl { get; set; }
        public List<string> EngUpdateTracker { get; set; }
        public List<string> OpsUpdateTracker { get; set; }
        public List<string> EXNetworks { get; set; }
        public List<string> NEWOPSRiskEvaluation { get; set; }
        public List<string> OccurrenceProbability { get; set; }
        public List<string> IncidentClass { get; set; }
        public List<string> ProductCode { get; set; }
        public List<bool> HandedOverToOperation { get; set; }
        public List<string> ContractRenewalPlan { get; set; }
        public List<string> DataSourceLcm { get; set; }
        public List<string> ScopeOfSimplification { get; set; }       



        #endregion
        public ReportViewMode ViewMode { get; set; }
        public string LcmExportDescription { get; set; }
        public bool IsHistorical { get; set; }
        public bool IsCurrent { get; set; }
        public List<int> VerticalEngineeringTeamId { get; set; }
        public List<string> VulnerabilityScore { get; set; }
        public List<string> Comments { get; set; }
        public List<string> QID { get; set; }
        public List<string> VulnerabilityRating { get; set; }
        public List<string> LcmCumulativeRiskId { get; set; }
        public List<string> LcmCumulativeRiskLevel { get; set; }
        public List<string> CyberRiskRequestId { get; set; }

        #region Ticket 551
        #region // 719 Regulatory Fiels changes
        public List<bool?> IsPecn { get; set; }
        public List<bool?> IsPecs { get; set; }
        public List<bool?> IsScf { get; set; }
        public List<bool?> IsNof { get; set; }
        #endregion
        public List<string> ExposedEdgeFlag { get; set; }
        public List<bool> ExternalFacingFlag { get; set; }
        public List<string> InfrastructureLocation { get; set; }

        #endregion

        #region // LCM level Two 
        public List<string> Category { get; set; }
        public List<string> InterfaceType { get; set; }
        #endregion

        #region Dcf and Supported Service filtering
        public List<int> DesignComponentFamily { get; set; }

        public List<int> SupportedService { get; set; }
        #endregion

        #region //Bag Details
        public List<long> BagName { get; set; }
        public List<long> ComponentName { get; set; }
        public List<long> ComponentResourceKey { get; set; }


        #endregion

        #region LCM R11
        public List<string> AssetStatusFY26 { get; set; }
        public List<string> AssetStatusFY28 { get; set; }
        public List<string> EoslKpiFrozen { get; set; }
        public List<string> EoslKpiForecast { get; set; }
        public List<string> ExceptionFlag { get; set; }
        public List<string> RiskComment { get; set; }
        public List<string> EoslKpiTarget { get; set; }
        #endregion
    }
}
