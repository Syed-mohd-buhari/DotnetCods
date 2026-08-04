using CAM.Entities.Models.Base;
using System;

namespace CAM.Entities.Models
{
    public partial class LcmAncillaryData : AuditableEntity
    {
        public long LcmAncillaryDataId { get; set; }
        public long LcmEngineeringId { get; set; }
        public string ProductCode { get; set; }
        public bool? HandedOverToOperation { get; set; }
        public string ContractRenewalPlan { get; set; }
        public string ReasonForNoPlan { get; set; }
        public string CommentOnProjectStatus { get; set; }
        public string ScopeOfSimplification { get; set; }
        public string DataSource { get; set; }
        public string IncidentClass { get; set; }
        public string OccurenceProbability { get; set; }
        public string SecurityRiskEffective { get; set; }
        public string SecurityMitigation { get; set; }
        public string AssetOutOfScope { get; set; }
        public bool? IncludedInSecurityScanning { get; set; }
        public string RaId { get; set; }
        public string RequestId { get; set; }
        public DateTime? LastScanDate { get; set; }
        public DateTime? LastUpgradeDate { get; set; }
        public string EomControl { get; set; }
        public string EngUpdateTracker { get; set; }
        public string OpsUpdateTracker { get; set; }      

        public string ExNetworks { get; set; }
        public string OriginalHwLcmId { get; set; }
        public string OriginalSwLcmId { get; set; }

        public bool? Ispecn { get; set; }
        public bool? Ispecs { get; set; }
        public bool? Isscf { get; set; }
        public bool? Isnof { get; set; }
        public bool? ExposedEdgeFlag { get; set; }
        public bool? ExternalFacingFlag { get; set; }
        public string InfrastructureLocation { get; set; }
        public string Vulnerabilityrating { get; set; }
        public string Cyberriskrequestid { get; set; }
        public string LastScanRefNumber { get; set; }
        public string LastPenTestReferenceNumber { get; set; }
        public DateTime? LastPenTestDate { get; set; }
        public string RiskComment { get; set; }
        public string Qid { get; set; }

        public virtual LcmEngineering LcmEngineering { get; set; }
    }
}
