using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
   public class PassThroughQueryDto : QueryObject
   {

        public List<long> PassThroughId { get; set; }
        public List<long> NonTemsVertical { get; set; }
        public List<string> VodafoneUniqueIdentifier { get; set; }
        public List<string> AssetName { get; set; }
        public List<string> AssetDescriptionOrPurpose { get; set; }
        public List<string> AssetTypeTsr { get; set; }
        public List<string> BusinessOwner { get; set; }
        public List<string> SupportOwner { get; set; }
        public List<string> SupportTeam { get; set; }
        public List<string> SupportTeamsPlaceInTheOrganisation { get; set; }
        public List<string> AssetFunction { get; set; }
        public List<string> DeploymentOrLifeCycleStatus { get; set; }
        public List<string> RelatedRiskIdsFromRiskRegisters { get; set; }
        public List<string> RegulatoryScope { get; set; }
        public List<string> CountryWhereAssetIsLocated { get; set; }
        public List<string> Geolocation { get; set; }
        public List<string> Infrastructure { get; set; }
        public List<string> UpStreamDependencies { get; set; }
        public List<string> DownStreamDependencies { get; set; }
        public List<string> ChangesToTheassetSinceDeployment { get; set; }
        public List<string> CloudHostedAsset { get; set; }
        public List<string> CloudType { get; set; }
        public List<string> CloudVendor { get; set; }
        public List<string> EquipmentName { get; set; }
        public List<string> HostLocationWithInPhysicalLocation { get; set; }
        public List<string> FirmwareVersionPatchLevel { get; set; }
        public List<string> MaintenanceSupportSupplier { get; set; }
        public List<string> DependantHardware { get; set; }
        public List<string> InstanceType { get; set; }
        public List<string> OperatingSystemName { get; set; }
        public List<string> OperatingSystemSwvVersion { get; set; }
        public List<string> OperatingSystemSwVersionPatchLevel { get; set; }
        public List<string> SystemNameDns { get; set; }
        public List<string> SystemNameManagementIpAddress { get; set; }
        public List<string> SystemNameNetBios { get; set; }
        public List<string> SystemNameHostName { get; set; }
        public List<string> DependantSystemSoftware { get; set; }
        public List<string> ResilienceModel { get; set; }
        public List<string> GeographicSiteResilience { get; set; }
        public List<string> LocalSiteResilience { get; set; }
        public List<string> NameOfProductsDependantOnAsset { get; set; }
        public List<string> Customer { get; set; }
        public List<string> PrivilegedAccessLogging { get; set; }
        public List<string> BoardOrModuleNameComponentName { get; set; }
        public List<string> ExposedEdge { get; set; }
        public List<string> ExternallyFacingSystem { get; set; }
        public List<string> ManagementPlane { get; set; }
        public List<string> NetworkOverSightFunction { get; set; }
        public List<string> Pecn { get; set; }
        public List<string> Pecs { get; set; }
        public List<string> SecurityCriticalFunction { get; set; }
        public List<string> Critical { get; set; }
        public List<string> PartNumber { get; set; }
        public List<string> ProdOrLab { get; set; }
        public List<string> LocalMarketOwnerShip { get; set; }
        public List<string> AssuranceCall { get; set; }
        public List<string> ServiceLevel { get; set; }
        public List<string> LastPenTestRefNo { get; set; }
        public List<string> PiData { get; set; }
        public List<string> EncryptedPiData { get; set; }
        public List<string> MaintenanceHardwareEndOfSupportDate { get; set; }
        public List<string> DateAssetMovedToLiveStatus { get; set; }
        public List<string> DateAssetDecommissioned { get; set; }
        public List<string> LastPenTestDateTsr { get; set; }
        public List<string> FirmwareVersion { get; set; }
        public List<string> BoardOrModuleTypeComponentSubtype { get; set; }
        public List<string> BoardOrModuleTypeComponentVersionNumber { get; set; }
        public List<string> SerialNumberTsr { get; set; }
        public List<string> HardwareTypeofHardwareAsset { get; set; }
        public List<string> HwEndofSale { get; set; }
        public List<string> SoftwareProductType { get; set; }
        public List<string> ApplicationHostedonSoftware { get; set; }
        public List<string> UuidorSerialNumberofSoftware { get; set; }
        public List<string> SwEndofSale { get; set; }
        public List<string> VerticalEngineeringTeam { get; set; }
        public List<string> VendorEndofMaintenanceDate { get; set; }
        public List<string> Application { get; set; }
        public List<string> PhysicalServerHostName { get; set; }
        public List<string> PhysicalServerIpaddress { get; set; }
        public List<string> PhysicalServerSerialNumber { get; set; }
        public List<string> PhysicalServerHwModel { get; set; }
        public List<string> PhysicalServerVendor { get; set; }
        public List<string> VirtualServerHostedon { get; set; }
        public List<string> VirtualServerManufacturer { get; set; }
        public List<string> VirtualServerTypeofDevice { get; set; }
        public List<string> VirtualMachineType { get; set; }
        public List<string> VirtualServerSerialNumber { get; set; }
        public List<string> VirtualServerType { get; set; }
        public List<string> OsStartDate { get; set; }
        public List<string> OsInstallationDate { get; set; }
        public List<string> OsStatus { get; set; }
        public List<string> SoftwareName { get; set; }
        public List<string> Version { get; set; }
        public List<string> Release { get; set; }
        public List<string> Language { get; set; }
        public List<string> HwOpsContractStatus { get; set; }
        public List<string> HwOpsContractEndDate { get; set; }
        public List<string> SwOpsContractStatus { get; set; }
        public List<string> PlannedHwModel { get; set; }
        public List<string> LocationOrDatacentre { get; set; }
        public List<string> PhysicalServerOSName { get; set; }
        public List<string> PhysicalOSVersion { get; set; }
        public List<string> PhysicalServerOSStartDate { get; set; }
        public List<string> PhysicalServerOSinstallationdate { get; set; }
        public List<string> PhysicalServerOSstatus { get; set; }
        public List<string> Cloud { get;set; }
        public List<string> ResourceKey { get;set; }
    }
}
