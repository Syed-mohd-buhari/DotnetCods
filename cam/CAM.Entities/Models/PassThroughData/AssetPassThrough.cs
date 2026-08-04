using CAM.Entities.Models.Base;
using CAM.Identity;
using OracleModels.DBModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.PassThroughData
{
    [Table("Assetpassthrough")]
    public class AssetPassThrough : AuditableEntity
    {
        public long PassThroughId { get; set; }
        public long? NonTemsVertical { get; set; }
        public long? VodafoneUniqueIdentifier { get; set; }
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
        public string MaintenanceSupportSupplierSecond { get; set; }
        public string DependantSystemSoftware { get; set; }
        public string ResilienceModel { get; set; }
        public string GeographicSiteResilience { get; set; }
        public string LocalSiteResilience { get; set; }
        public string NameOfProductsDependantOnAsset { get; set; }
        public string Customer { get; set; }
        public string PrivilegedAccessLogging { get; set; }
        public string BoardOrModuleNameComponentName { get; set; }
        public string ExposedEdge { get; set; }
        public string ExternallyFacingSystem { get; set; }
        public string ManagementPlane { get; set; }
        public string NetworkOverSightFunction { get; set; }
        public string Pecn { get; set; }
        public string Pecs { get; set; }
        public string SecurityCriticalFunction { get; set; }
        public string Critical { get; set; }
        public string PartNumber { get; set; }
        public string ProdOrLab { get; set; }
        public string LocalMarketOwnerShip { get; set; }
        public string AssuranceCall { get; set; }
        public string ServiceLevel { get; set; }
        public string LastPenTestRefNo { get; set; }
        public string PiData { get; set; }
        public string EncryptedPiData { get; set; }
        public string DateAssetMovedToLiveStatus { get; set; }
        public string DateAssetDecommissioned { get; set; }
        public string VendorSoftwareEndofSupportDate { get; set; }
        public string MaintenanceSoftwareEndofSupportDate { get; set; }
        public string LastPenTestDate { get; set; }
        public string FirmwareVersion { get; set; }
        public string BoardOrModuleTypeComponentSubtype { get; set; }
        public string BoardOrModuleTypeComponentVersionNumber { get; set; }
        public string SerialNumber { get; set; }
        public string HardwareTypeofHardwareAsset { get; set; }
        public string HwEndofSale { get; set; }
        public string SoftwareProductType { get; set; }
        public string ApplicationHostedonSoftware { get; set; }
        public string UuidorSerialNumberofSoftware { get; set; }
        public string SwEndofSale { get; set; }
        public string Application { get; set; }
        public string PhysicalServerHostName { get; set; }
        public string PhysicalServerIpaddress { get; set; }
        public string PhysicalServerSerialNumber { get; set; }
        public string PhysicalServerHwModel { get; set; }
        public string PhysicalServerVendor { get; set; }
        public string VirtualServerHostedon { get; set; }
        public string VirtualServerManufacturer { get; set; }
        public string VirtualServerTypeofDevice { get; set; }
        public string VirtualMachineType { get; set; }
        public string VirtualServerSerialNumber { get; set; }
        public string VirtualServerType { get; set; }
        public string OsStartDate { get; set; }
        public string OsInstallationDate { get; set; }
        public string OsStatus { get; set; }
        public string SoftwareName { get; set; }
        public string Version { get; set; }
        public string Release { get; set; }
        public string Language { get; set; }
        public string HwOpsContractStatus { get; set; }
        public string SwOpsContractStatus { get; set; }
        public string PlannedHwModel { get; set; }
        public string LocationOrDatacentre { get; set; }
        public string PhysicalServerOSName { get; set; }
        public string PhysicalOSVersion { get; set; }
        public string PhysicalServerOSStartDate { get; set; }
        public string PhysicalServerOSinstallationdate { get; set; }
        public string PhysicalServerOSstatus { get; set; }
        public string Cloud { get; set; }
        public string ResourceKey { get; set; }
        public string VerticalName { get; set; }    

        public virtual AppSettingsConfiguration NonTemsVerticalNavigation { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual SwPassThroughLcm ResourceKeyNavigation { get; set; }

    }
}