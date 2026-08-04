using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
    public class LcmAncillaryDataQueryDto : QueryObject
    {
        public List<long> LcmAncillaryDataId { get; set; }
        public List<long> LcmEngineeringId { get; set; }
        public List<long> OpCo { get; set; }
        public List<long> DesignComponent { get; set; }
        public List<string> ProductCode { get; set; }
        public List<bool> HandedOverToOperation { get; set; }
        public List<string> ContractRenewalPlan { get; set; }
        public List<string> ReasonForNoPlan { get;set; }
        public List<string> CommentOnProjectStatus { get; set; }
        public List<string> ScopeOfSimplication { get; set; }
        public List<string> DataSource { get; set; }
        public List<string> IncidentClass { get; set; }
        public List<string> OccurrenceProbability { get; set; }
        public List<string> NEWOPSRiskEvaluation { get; set; }
        public List<string> SecurityRiskPotential { get; set; }
        public List<string> SecurityRiskEffective { get; set; }
        public List<string> SecurityMitigation { get; set; }
        public List<string> SecurityRiskOverall { get; set; }
        public List<bool> IncludedinSecurityScanning { get; set; }
        public List<string> RaId { get; set; }
        public List<string> RequestIDLcm { get; set; }
        public DateFilter LastScanDate { get; set; }
        public DateFilter LastUpgradeDate { get; set; }
        public List<string> AssetOutofScopeForReportingPurposes { get; set; }
        public List<string> EomControl { get; set; }
        public List<string> EngUpdateTracker { get; set; }
        public List<string> OpsUpdateTracker { get; set; }
        public List<string> Custom2 { get; set; }
        public List<string> KpiStatusService { get; set; }
        public List<string> Custom { get; set; }
        public List<string> Custom1 { get; set; }
        public List<string> Id_New { get; set; }
        public List<string> EXNetworks { get; set; }
        public List<string> ProductImportanceHistory2 { get; set; }
        public List<string> CloudVersion { get; set; }
        public List<string> LabSwRelease { get; set; }      
        public List<string> CertifiedSWReleaseforNFVIbundle { get; set; }
        public List<string> LcmStatus { get; set; }
        public List<string> OriginalLCMID { get; set; }     
        public List<string>CreationUser { get; set; }
        public DateFilter CreationDate { get; set; }
        public List<string>Modificationuser { get; set; }
        public DateFilter ModificationDate { get; set; }
        public List<string> InfrastructureLocation { get; set; }
        public List<string> Vulnerabilityrating { get; set; }
        public List<string> Cyberriskrequestid { get; set; }

        public List<string> LastScanRefNumber { get; set; }
        public List<string> LastPenTestReferenceNumber { get; set; }
        public DateFilter LastPenTestDate { get; set; }
        public string RiskComment { get; set; }
        public string Qid { get; set; }
    }
}
