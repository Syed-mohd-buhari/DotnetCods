using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto
{
   public class TsrPassThroughQueryDto : QueryObject
   {

        public List<long> TsrPassThroughId { get; set; }
        public List<string> AssetId { get; set; }
        public List<long> VodafoneUniqueIdentifier { get; set; }
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
        public List<string> SoftwareVendorName { get; set; }
        public List<string> Model { get; set; }
        public List<string> FirmwareVersion { get; set; }
        public List<string> FirmwareVersionPatchLevel { get; set; }
        public List<string> MaintenanceSupportSupplier { get; set; }
        public List<string> VendorHardwareEndOfSupportDate { get; set; }
        public List<string> MaintenanceHardwareEndOfSupportDate { get; set; }
        public List<string> DependantHardware { get; set; }
        public List<string> InstanceType { get; set; }
        public List<string> OperatingSystemName { get; set; }
        public List<string> OperatingSystemSwvVersion { get; set; }
        public List<string> OperatingSystemSwVersionPatchLevel { get; set; }
        public List<string> SystemNameDns { get; set; }
        public List<string> SystemNameManagementIpAddress { get; set; }
        public List<string> SystemNameNetBios { get; set; }
        public List<string> SystemNameHostName { get; set; }
        public List<string> DateAssetMovedToLiveStatus { get; set; }
        public List<string> DateAssetDecommissioned { get; set; }
        public List<string> HardwareVendorName { get; set; }
        public List<string> VendorSoftwareEndOfSupportDate { get; set; }
        public List<string> MaintenanceSoftwareEndOfSupportDate { get; set; }
        public List<string> DependantSystemSoftware { get; set; }
        public List<string> ResilienceModel { get; set; }
        public List<string> GeographicSiteResilience { get; set; }
        public List<string> LocalSiteResilience { get; set; }
        public List<string> NameOfProductsDependantOnAsset { get; set; }
        public List<string> TechnicalServiceNames { get; set; }
        public List<string> Customer { get; set; }
        public List<string> PrivilegedAccessLogging { get; set; }
        public List<string> BoardOrModuleNameComponentName { get; set; }
        public List<string> BoardOrModuleTypeComponentSubtype { get; set; }
        public List<string> BoardOrModuleTypeComponentVersionNumber { get; set; }
        public List<string> ExposedEdge { get; set; }
        public List<string> ExternallyFacingSystem { get; set; }
        public List<string> ManagementPlane { get; set; }
        public List<string> NetworkOverSightFunction { get; set; }
        public List<string> Pecn { get; set; }
        public List<string> Pecs { get; set; }
        public List<string> SecurityCriticalFunction { get; set; }
        public List<string> ProductImportanceTsr { get; set; }
        public List<string> Critical { get; set; }
        public List<string> CriticalityType { get; set; }
        public List<string> SerialNumberTsr { get; set; }
        public List<string> PartNumber { get; set; }
        public List<string> DescriptionOfPlannedaction { get; set; }
        public List<string> IdentifiedActionTsr { get; set; }
        public List<string> LastUpgradeDateTsr { get; set; }
        public List<string> ProdOrLab { get; set; }
        public List<string> LocalMarketOwnerShip { get; set; }
        public List<string> BudgetEstimatedTsr { get; set; }
        public List<string> BundleBudgetTsr { get; set; }
        public List<string> AssuranceCall { get; set; }
        public List<string> CommentOnProjectStatusTsr { get; set; }
        public List<string> ProjectEndDateTsr { get; set; }
        public List<string> ProjectStatusTsr { get; set; }
        public List<string> ServiceLevel { get; set; }
        public List<string> LastPenTestDateTsr { get; set; }
        public List<string> LastPenTestRefNo { get; set; }
        public List<string> PiData { get; set; }
        public List<string> EncryptedPiData { get; set; }
        public List<int> RecordClassifier { get; set; }
        public string ActiveTab { get; set; }
        public List<long> NonTemsVertical { get; set; }
        public List<string> MeProductName { get; set; }
        public List<string> MeSoftwareVersion { get; set; }
        public List<string> MeSubDomainResponsible { get; set; }
        public bool IsGlossary { get; set; }
        public List<int> VerticalName { get; set; }

    }
}
