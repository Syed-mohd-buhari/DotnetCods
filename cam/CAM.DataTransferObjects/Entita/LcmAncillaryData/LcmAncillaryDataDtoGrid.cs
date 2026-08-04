using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CAM.DataAttributes.Export;
using CAM.DataAttributes.Grid;

namespace CAM.DataTransferObjects.Entita.LcmAncillaryData
{
    public class LcmAncillaryDataDtoGrid
    {
        [DisplayName("Lcm Ancillary Data Id")]
        [OrderGrid(Order = 1)]
        [Default]
        [IgnoreGrid]
        public long LcmAncillaryDataId { get; set; }


        [DisplayName("Lcm Engineering Id")]
        [OrderGrid(Order = 1)]
        [Default]
        public long LcmEngineeringId { get; set; }

        [DisplayName("DesignComponent Index")]
        [OrderGrid(Order = 2)]
        [Default]
        public long DesignComponentIndex { get; set; }

        [DisplayName("Opco")]
        [OrderGrid(Order = 3)]
        [Default]
        public string OpCo { get; set; }

        [DisplayName("DesignComponent")]
        [OrderGrid(Order = 4)]
        [Default]
        public string DesignComponent { get; set; }

        [DisplayName("Product Code")]
        [OrderGrid(Order = 5)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ProductCode { get; set; }

        [DisplayName("Handed over to Operation")]
        [OrderGrid(Order = 6)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public bool HandedOverToOperation { get; set; }



        [DisplayName("Contract Renewal Plan")]
        [OrderGrid(Order = 7)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string ContractRenewalPlan { get; set; }


        [DisplayName("Reason For No Plan")]
        [OrderGrid(Order = 8)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ReasonForNoPlan { get; set; }

        [DisplayName("Comment On Project Status")]
        [OrderGrid(Order = 9)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string CommentOnProjectStatus { get; set; }

        [DisplayName("Scope Of Simplication")]
        [OrderGrid(Order = 10)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string ScopeOfSimplification { get; set; }

        [DisplayName("Data Source(Project Code)")]
        [OrderGrid(Order = 11)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string DataSource { get; set; }

        [DisplayName("Incident Class")]
        [OrderGrid(Order = 12)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string IncidentClass { get; set; }

        [DisplayName("Occurrence Probability")]
        [OrderGrid(Order = 13)]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OccurenceProbability { get; set; }

        [DisplayName("NEW OPS Risk Evaluation")]
        [OrderGrid(Order = 14)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string NewopsRiskEvaluation { get; set; }

        [DisplayName("Security Risk (Potential)")]
        [OrderGrid(Order = 15)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [IgnoreGrid]
        [Default]
        public string SecurityRiskPotential { get; set; }

        [DisplayName("Security Risk (Effective)")]
        [OrderGrid(Order = 16)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SecurityRiskEffective { get; set; }

        [DisplayName("Security Mitigation")]
        [OrderGrid(Order = 17)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string SecurityMitigation { get; set; }


        //[DisplayName("Security Risk Overall")]
        //[OrderGrid(Order = 18)]
        //[HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        //[ColorGrid(Color = "blue")]
        //[Default]
        //public string SecurityRiskOverall { get; set; }

        [DisplayName("Included in Security Scanning")]
        [OrderGrid(Order = 19)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public bool IncludedInSecurityScanning { get; set; }

        [DisplayName("RA ID")]
        [OrderGrid(Order = 20)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RaId { get; set; }

        [DisplayName("Vuln. Request ID")]
        [OrderGrid(Order = 21)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string RequestId { get; set; }

        [DisplayName("Last Scan Date")]
        [OrderGrid(Order = 22)]
        [DateRangeGrid]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public DateTime? LastScanDate { get; set; }

        [DisplayName("Last Upgrade Date")]
        [OrderGrid(Order = 23)]
        [DateRangeGrid]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public DateTime? LastUpgradeDate { get; set; }

        [OrderGrid(Order = 24)]
        [DisplayName("Asset out of scope for reporting purposes")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string AssetOutOfScope { get; set; }

        [OrderGrid(Order = 25)]
        [DisplayName("EOM_control")]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string EomControl { get; set; }

        [OrderGrid(Order = 26)]
        [DisplayName("ENG Update Tracker")]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string EngUpdateTracker { get; set; }

        [OrderGrid(Order = 27)]
        [DisplayName("OPS Update Tracker")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string OpsUpdateTracker { get; set; }

        [DisplayName("Custom2")]
        [OrderGrid(Order = 28)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [IgnoreGrid]

        public string Custom2 { get; set; }

        [DisplayName("KPI Status Service")]
        [OrderGrid(Order = 29)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [IgnoreGrid]
        public string KpiStatusService { get; set; }

        [DisplayName("Custom")]
        [OrderGrid(Order = 30)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [IgnoreGrid]
        public string Custom { get; set; }

        [DisplayName("Custom1")]
        [OrderGrid(Order = 31)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [IgnoreGrid]
        public string Custom1 { get; set; }


        [DisplayName("ID_NEW")]
        [OrderGrid(Order = 32)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [IgnoreGrid]
        public string IdNew { get; set; }

        [DisplayName("EX Networks")]
        [OrderGrid(Order = 33)]
        [HeaderColor(BackgroundColor = 0xcccccc, FontColor = 0xffffff)]
        [ColorGrid(Color = "gray")]
        [Default]
        public string ExNetworks { get; set; }

        [DisplayName("Product Importance History2")]
        [OrderGrid(Order = 34)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [IgnoreGrid]
        public string ProductImportanceHistory2 { get; set; }

        [DisplayName("Cloud Version")]
        [OrderGrid(Order = 35)]
        [IgnoreGrid]
        public string CloudVersion { get; set; }

        [DisplayName("Lab SW Release")]
        [OrderGrid(Order = 36)]
        [Default]
        public string LabSwRelease { get; set; }

        [DisplayName("Certified SW release for NFVI bundle  3.2.1")]
        [OrderGrid(Order = 37)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [IgnoreGrid]
        public string CertifiedSWRealeseForNfviBundle { get; set; }

        [DisplayName("LCM Status (june 2021)")]
        [OrderGrid(Order = 38)]
        [HeaderColor(BackgroundColor = 0x2F75B5, FontColor = 0xffffff)]
        [ColorGrid(Color = "blue")]
        [IgnoreGrid]
        public string LcmStatus { get; set; }


        [DisplayName("Original HW LCM ID")]
        [OrderGrid(Order = 39)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string OriginalHwLcmId { get; set; }


        [DisplayName("Original SW LCM ID")]
        [OrderGrid(Order = 40)]
        [HeaderColor(BackgroundColor = 0xe60000, FontColor = 0xffffff)]
        [ColorGrid(Color = "red")]
        [Default]
        public string OriginalSwLcmId { get; set; }

        [OrderGrid(Order = 41)]
        [Default]
        public string ModificationUser { get; set; }
        [OrderGrid(Order = 42)]
        [Default]
        public string CreationUser { get; set; }
        [OrderGrid(Order = 43)]
        [Default]
        [DateRangeGrid]
        public DateTime ModificationDate { get; set; }
        [OrderGrid(Order = 44)]
        [Default]
        [DateRangeGrid]
        public DateTime CreationDate { get; set; }


        [OrderGrid(Order = 45)]
        [DisplayName("Security Risk Overall")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string SecurityRiskOverall { get; set; }

        [OrderGrid(Order = 46)]
        [DisplayName("Resource Key")]
        [HeaderColor(BackgroundColor = 0x00c300, FontColor = 0xffffff)]
        [ColorGrid(Color = "green")]
        [Default]
        public string ResourceKey { get; set; }

        [DisplayName("Regulatory Fields")]
        [OrderGrid(Order = 47)]
        [Default]
        public Dictionary<string,bool> RegulatoryFields { get; set; }

        [DisplayName("Exposed Edge Flag")]
        [OrderGrid(Order = 48)]
        [IgnoreGrid]
        public bool ExposedEdgeFlag { get; set; }

        [DisplayName("External Facing Flag")]
        [OrderGrid(Order = 49)]
        [Default]
        public bool ExternalFacingFlag { get; set; }

        [DisplayName("Infrastructure Location")]
        [OrderGrid(Order = 50)]
        [Default]
        public string InfrastructureLocation { get; set; }

        [DisplayName("Vulnerability Rating")]
        [OrderGrid(Order = 51)]
        [Default]
        public string VulnerabilityRating { get; set; }
        [DisplayName("Cyber Risk Request Id")]
        [OrderGrid(Order = 52)]
        [Default]
        public string CyberRiskRequestId { get; set; }
        [DisplayName("Last Scan Ref Number")]
        [OrderGrid(Order = 53)]
        [Default]
        public string LastScanRefNumber { get; set; }
        [DisplayName("Last Pen Test Reference Number")]
        [OrderGrid(Order = 54)]
        [Default]
        public string LastPenTestReferenceNumber { get; set; }
        [DisplayName("Last Pen Test Date")]
        [OrderGrid(Order = 55)]
        [DateRangeGrid]
        [Default]
        public DateTime? LastPenTestDate { get; set; }

        [DisplayName("Risk Comment")]
        [OrderGrid(Order = 56)]
        [Default]
        public string RiskComment { get; set; }

        [DisplayName("QID")]
        [OrderGrid(Order = 57)]
        [Default]
        public string QId { get; set; }
    }
}
