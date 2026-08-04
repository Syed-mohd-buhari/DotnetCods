using System;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.Entita.LcmAncillaryData
{
    public class LcmAncillaryDataCRUDDto
    {
        public long LcmAncillaryDataId { get; set; }
        public long LcmEngineeringId { get; set; }
        public string ProductCode { get; set; }
        public bool HandedOverToOperation { get; set; }
        public string ContractRenewalPlan { get; set; }
        public string ReasonForNoPlan { get; set; }
        public string CommentOnProjectStatus { get; set; }
        public string ScopeOfSimplification { get; set; }
        public string DataSource { get; set; }
        public string IncidentClass { get; set; }
        public string OccurenceProbability { get; set; }              
        public string SecurityRiskPotential { get; set; }
        public string SecurityRiskEffective { get; set; }
        public string SecurityMitigation { get; set; }
        public string AssetOutOfScope { get; set; }
        public bool IncludedInSecurityScanning { get; set; }
        public string RaId { get; set; }
        public string RequestId { get; set; }
        public DateTime? LastScanDate { get; set; }
        public DateTime? LastUpgradeDate { get; set; }
        public string EomControl { get; set; }
        public string EngUpdateTracker { get; set; }
        public string OpsUpdateTracker { get; set; }
        //public string Custom2 { get; set; }
        //public string KpiStatusService { get; set; }
        //public string Custom { get; set; }
        //public string Custom1 { get; set; }
        //public string IdNew { get; set; }
        public string ExNetworks { get; set; }
        //public string ProductImportanceHistory2 { get; set; }
        //public string CloudVersion { get; set; }
        //public string CertifiedSWRealeseForNfviBundle { get; set; }
        public string OriginalHwLcmId { get;set; }
        public string OriginalSwLcmId { get; set; }
        //public string LcmStatus { get; set; }
        public string ModificationUser { get; set; }
        public string CreationUser { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime CreationDate { get; set; }

        public Dictionary<string,bool> RegulatoryFields { get; set; }
        public bool  ExposedEdgeFlag { get; set; }
        public bool  ExternalFacingFlag { get; set; }
        public string InfrastructureLocation { get; set; }
        public string Vulnerabilityrating { get; set; }
        public string Cyberriskrequestid { get; set; }

        public string LastScanRefNumber { get; set; }
        public string LastPenTestReferenceNumber { get; set; }
        public DateTime? LastPenTestDate { get; set; }
        public string RiskComment { get; set; }
        public string QId { get; set; }
    }
}
