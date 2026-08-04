using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Models
{
    [Table("Tsrpassthrough")]
    public class TsrPassThrough : AuditableEntity
    {
        public long TsrPassThroughId { get; set; }
        public string AssetId { get; set; }
        public string VodafoneUniqueIdentifier { get; set; }
        public string AssetName { get; set; }
        public string AssetDescriptionOrPurpose { get; set; }
        public string AssetType { get; set; }
        public string BusinessOwner { get; set; }
        public string SupportOwner { get; set; }
        public string SupportTeam { get; set; }
        public string SupportTeamsPlaceInTheOrganisation { get; set; }
        public string AssetFunction { get; set; }
        public string DeploymentOrLifeCycleStatus { get; set; }
        public string RelatedRiskIdsFromRiskRegisters { get; set; }
        public string RegulatoryScope { get; set; }
        public string CountryWhereAssetIsLocated { get; set; }
        public string Geolocation { get; set; }
        public string Infrastructure { get; set; }
        public string UpStreamDependencies { get; set; }
        public string DownStreamDependencies { get; set; }
        public string ChangesToTheassetSinceDeployment { get; set; }
        public string CloudHostedAsset { get; set; }
        public string CloudType { get; set; }
        public string CloudVendor { get; set; }
        public string EquipmentName { get; set; }
        public string HostLocationWithInPhysicalLocation { get; set; }
        public string SoftwareVendorName { get; set; }
        public string Model { get; set; }
        public string FirmwareVersion { get; set; }
        public string FirmwareVersionPatchLevel { get; set; }
        public string MaintenanceSupportSupplier { get; set; }
        public string VendorHardwareEndOfSupportDate { get; set; }
        public string MaintenanceHardwareEndOfSupportDate { get; set; }
        public string DependantHardware { get; set; }
        public string InstanceType { get; set; }
        public string OperatingSystemName { get; set; }
        public string OperatingSystemSwvVersion { get; set; }
        public string OperatingSystemSwVersionPatchLevel { get; set; }
        public string SystemNameDns { get; set; }
        public string SystemNameManagementIpAddress { get; set; }
        public string SystemNameNetBios { get; set; }
        public string SystemNameHostName { get; set; }
        public string DateAssetMovedToLiveStatus { get; set; }
        public string DateAssetDecommissioned { get; set; }
        public string HardwareVendorName { get; set; }
        public string MaintenanceSupportSupplierSecond { get; set; }
        public string VendorSoftwareEndOfSupportDate { get; set; }
        public string MaintenanceSoftwareEndOfSupportDate { get; set; }
        public string DependantSystemSoftware { get; set; }
        public string ResilienceModel { get; set; }
        public string GeographicSiteResilience { get; set; }
        public string LocalSiteResilience { get; set; }
        public string NameOfProductsDependantOnAsset { get; set; }
        public string TechnicalServiceNames { get; set; }
        public string Customer { get; set; }
        public string PrivilegedAccessLogging { get; set; }
        public string BoardOrModuleNameComponentName { get; set; }
        public string BoardOrModuleTypeComponentSubtype { get; set; }
        public string BoardOrModuleTypeComponentVersionNumber { get; set; }
        public string ExposedEdge { get; set; }
        public string ExternallyFacingSystem { get; set; }
        public string ManagementPlane { get; set; }
        public string NetworkOverSightFunction { get; set; }
        public string Pecn { get; set; }
        public string Pecs { get; set; }
        public string SecurityCriticalFunction { get; set; }
        public string ProductImportance { get; set; }
        public string Critical { get; set; }
        public string CriticalityType { get; set; }
        public string SerialNumber { get; set; }
        public string PartNumber { get; set; }
        public string DescriptionOfPlannedaction { get; set; }
        public string IdentifiedAction { get; set; }
        public string LastUpgradeDate { get; set; }
        public string ProdOrLab { get; set; }
        public string LocalMarketOwnerShip { get; set; }
        public string BudgetEstimated { get; set; }
        public string BundleBudget { get; set; }
        public string AssuranceCall { get; set; }
        public string CommentOnProjectStatus { get; set; }
        public string ProjectEndDate { get; set; }
        public string ProjectStatus { get; set; }
        public string ServiceLevel { get; set; }
        public string LastPenTestDate { get; set; }
        public string LastPenTestRefNo { get; set; }
        public string PiData { get; set; }
        public string EncryptedPiData { get; set; }
        public int? RecordClassifier { get; set; }

        public long? NonTemsVertical { get; set; }
        public string ProductName { get; set; }
        public string SoftwareVersion { get; set; }
        public string SubDomainResponsible { get; set; }
        public bool? IsSortingAllowed { get; set; }

        public virtual AppSettingsConfiguration NonTemsVerticalNavigation { get; set; }
    }
}