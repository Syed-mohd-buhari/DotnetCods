using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.AssetPassThrough
{
    public class PassThroughDtoGrid
    {
        [IgnoreGrid]
        public long PassThroughId { get; set; }

        [IgnoreGrid]
        [Default]
        public long? NonTemsVertical { get; set; }

        [IgnoreGrid]
        public long? RecordClassifier { get; set; }

        [DisplayName("ME_SOURCE_ASSET_ID")]
        [OrderGrid(Order = 1)]
        [ColorGrid(Color = "green")]
        [Default]
        public long VodafoneUniqueIdentifier { get; set; }

        [DisplayName("RESOURCEKEY")]
        [OrderGrid(Order = 2)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string ResourceKeyTsr { get; set; }

        [DisplayName("ME_NAME")]
        [OrderGrid(Order = 3)]
        [ColorGrid(Color = "green")]
        [Default]
        public string AssetName { get; set; }

        [DisplayName("ME_COUNTRY_LOCATED")]
        [OrderGrid(Order = 4)]
        [ColorGrid(Color = "green")]
        [Default]
        public string CountryWhereAssetIsLocated { get; set; }

        [DisplayName("ME_TYPE")]
        [OrderGrid(Order = 5)]
        [ColorGrid(Color = "green")]
        [Default]
        public string AssetTypeTsr { get; set; }

        [DisplayName("ME_DESCRIPTION")]
        [OrderGrid(Order = 6)]
        [ColorGrid(Color = "green")]
        [Default]
        public string AssetDescriptionOrPurpose { get; set; }

        [DisplayName("ME_DEPLOYMENT_STATUS")]
        [OrderGrid(Order = 7)]
        [ColorGrid(Color = "green")]
        [Default]
        public string DeploymentOrLifeCycleStatus { get; set; }

        [DisplayName("ME_LIVE_STATUS_DATE")]
        [OrderGrid(Order = 8)]
        [ColorGrid(Color = "green")]
        [Default]
        public string DateAssetMovedToLiveStatus { get; set; }

        [DisplayName("ME_DECOMMISSIONED_DATE")]
        [OrderGrid(Order = 9)]
        [ColorGrid(Color = "green")]
        [Default]
        public string DateAssetDecommissioned { get; set; }

        [DisplayName("ME_SERIAL_NUMBER")]
        [OrderGrid(Order = 10)]
        [ColorGrid(Color = "green")]
        [Default]
        public string SerialNumberTsr { get; set; }

        [DisplayName("ME_ENVIRONMENT")]
        [OrderGrid(Order = 11)]
        [ColorGrid(Color = "green")]
        [Default]
        public string ProdOrLab { get; set; }

        [DisplayName("Hardwaretype of Hardware Asset")]
        [OrderGrid(Order = 12)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string HardwareTypeofHardwareAsset { get; set; }

        [DisplayName("MANAGEMENT_IP_ADDRESS")]
        [OrderGrid(Order = 13)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string SystemNameManagementIpAddress { get; set; }

        [DisplayName("ORGANISATION_NAME")]
        [OrderGrid(Order = 14)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string LocalMarketOwnerShip { get; set; }

        [DisplayName("HW_END_OF_LIFE")]
        [OrderGrid(Order = 15)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string MaintenanceHardwareEndOfSupportDate { get; set; }

        [DisplayName("HW_END_OF_SALE")]
        [OrderGrid(Order = 16)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string HwEndofSale { get; set; }

        [DisplayName("HW_COMPONENT_NAME")]
        [OrderGrid(Order = 17)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string BoardOrModuleNameComponentName { get; set; }

        [DisplayName("HW_COMPONENT_SUBTYPE")]
        [OrderGrid(Order = 18)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string BoardOrModuleTypeComponentSubtype { get; set; }

        [DisplayName("HW_COMPONENT_VERSION")]
        [OrderGrid(Order = 19)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string BoardOrModuleTypeComponentVersionNumber { get; set; }

        [DisplayName("HW_PART_NUMBER")]
        [OrderGrid(Order = 20)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string PartNumber { get; set; }

        [DisplayName("FIRMWARE_VERSION")]
        [OrderGrid(Order = 21)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string FirmwareVersion { get; set; }

        [DisplayName("FIRMWARE_PATCH_LEVEL")]
        [OrderGrid(Order = 22)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string FirmwareVersionPatchLevel { get; set; }

        [DisplayName("MAINTENANCE_SUPPORT_SUPPLIER(HW)")]
        [OrderGrid(Order = 23)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string MaintenanceSupportSupplier { get; set; }

        [DisplayName("DEPENDENT_HARDWARE")]
        [OrderGrid(Order = 24)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string DependantHardware { get; set; }

        [DisplayName("INSTANCE_TYPE")]
        [OrderGrid(Order = 25)]
        [ColorGrid(Color = "lightgreenv")]
        [Default]
        public string InstanceType { get; set; }

        [DisplayName("Software Product type")]
        [OrderGrid(Order = 26)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string SoftwareProductType { get; set; }

        [DisplayName("IS_CLOUD_HOSTED")]
        [OrderGrid(Order = 27)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string CloudHostedAsset { get; set; }

        [DisplayName("CLOUD_TYPE")]
        [OrderGrid(Order = 28)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string CloudType { get; set; }

        [DisplayName("CLOUD_VENDOR")]
        [OrderGrid(Order = 29)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string CloudVendor { get; set; }

        [DisplayName("Application Hosted on Software")]
        [OrderGrid(Order = 30)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string ApplicationHostedonSoftware { get; set; }

        [DisplayName("UUID/Serialnumber of Software")]
        [OrderGrid(Order = 31)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string UuidorSerialNumberofSoftware { get; set; }

        [DisplayName("location/Datacentre of software in case of virtual machine")]
        [OrderGrid(Order = 32)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string LocationOrDatacentre { get; set; }

        [DisplayName("GEO_LOCATION")]
        [OrderGrid(Order = 33)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string Geolocation { get; set; }

        [DisplayName("ME_SERVICE_TYPE")]
        [OrderGrid(Order = 34)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string AssetFunction { get; set; }

        [DisplayName("SW_END_OF_SALE")]
        [OrderGrid(Order = 35)]
        [ColorGrid(Color = "amber")]
        [Default]
        public string SwEndofSale { get; set; }

        [DisplayName("HW OPS Contract Status")]
        [OrderGrid(Order = 36)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string HwOpsContractStatus { get; set; }

        [DisplayName("SW OPS Contract Status")]
        [OrderGrid(Order = 37)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SwOpsContractStatus { get; set; }

        [DisplayName("Planned HW Model")]
        [OrderGrid(Order = 38)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string PlannedHwModel { get; set; }

        [DisplayName("BUSINESS_OWNER")]
        [OrderGrid(Order = 39)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string BusinessOwner { get; set; }

        [DisplayName("SUPPORT_OWNER")]
        [OrderGrid(Order = 40)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SupportOwner { get; set; }

        [DisplayName("SUPPORT_TEAM")]
        [OrderGrid(Order = 41)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SupportTeam { get; set; }

        [DisplayName("SUPPORT_DOMAIN")]
        [OrderGrid(Order = 42)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SupportTeamsPlaceInTheOrganisation { get; set; }

        [DisplayName("RISK_ID")]
        [OrderGrid(Order = 43)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string RelatedRiskIdsFromRiskRegisters { get; set; }

        [DisplayName("REGULATORY_SCOPE")]
        [OrderGrid(Order = 44)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string RegulatoryScope { get; set; }

        [DisplayName("INFRASTRUCTURE LOCATION")]
        [OrderGrid(Order = 45)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string Infrastructure { get; set; }

        [DisplayName("UPSTREAM_DEPENDENCIES")]
        [OrderGrid(Order = 46)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string UpStreamDependencies { get; set; }

        [DisplayName("DOWNSTREAM_DEPENDENCIES")]
        [OrderGrid(Order = 47)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string DownStreamDependencies { get; set; }

        [DisplayName("CHANGE_DESCRIPTION")]
        [OrderGrid(Order = 48)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string ChangesToTheassetSinceDeployment { get; set; }

        [DisplayName("EQUIPMENT_NAME")]
        [OrderGrid(Order = 49)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string EquipmentName { get; set; }

        [DisplayName("VIRTUAL_PLATFORM_LOCATION")]
        [OrderGrid(Order = 50)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string HostLocationWithInPhysicalLocation { get; set; }

        [DisplayName("OS_PATCH_LEVEL")]
        [OrderGrid(Order = 51)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string OperatingSystemSwVersionPatchLevel { get; set; }

        [DisplayName("SYSTEMNAME")]
        [OrderGrid(Order = 52)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SystemNameDns { get; set; }

        [DisplayName("SYSTEMNAME_NETBIOS")]
        [OrderGrid(Order = 53)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SystemNameNetBios { get; set; }

        [DisplayName("HOSTNAME")]
        [OrderGrid(Order = 54)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SystemNameHostName { get; set; }

        [DisplayName("MAINTENANCE_SUPPORT_SUPPLIER(SW)")]
        [OrderGrid(Order = 55)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string MaintenanceSupportSupplierSecond { get; set; }

        [DisplayName("DEPENDANT_SYSTEM_SW")]
        [OrderGrid(Order = 56)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string DependantSystemSoftware { get; set; }

        [DisplayName("RESILIENCE_MODEL")]
        [OrderGrid(Order = 57)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string ResilienceModel { get; set; }

        [DisplayName("RESILIENCE_GEOGRAPHIC_SITE")]
        [OrderGrid(Order = 58)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string GeographicSiteResilience { get; set; }

        [DisplayName("RESILIENCE_LOCAL_SITE")]
        [OrderGrid(Order = 59)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string LocalSiteResilience { get; set; }

        [DisplayName("DEPENDANT_PRODUCT_NAME")]
        [OrderGrid(Order = 60)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string NameOfProductsDependantOnAsset { get; set; }

        [DisplayName("CUSTOMER")]
        [OrderGrid(Order = 61)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string Customer { get; set; }

        [DisplayName("ME_PRIVILEGED_ACCESS_LOGGING")]
        [OrderGrid(Order = 62)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string PrivilegedAccessLogging { get; set; }

        [DisplayName("EXPOSED EDGE FLAG")]
        [OrderGrid(Order = 63)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string ExposedEdge { get; set; }

        [DisplayName("EXTERNAL FACING FLAG")]
        [OrderGrid(Order = 64)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string ExternallyFacingSystem { get; set; }

        [DisplayName("MANAGEMENT_PLANE")]
        [OrderGrid(Order = 65)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string ManagementPlane { get; set; }

        [DisplayName("NETWORK_OVERSIGHT")]
        [OrderGrid(Order = 66)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string NetworkOverSightFunction { get; set; }

        [DisplayName("PECN FLAG")]
        [OrderGrid(Order = 67)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string Pecn { get; set; }

        [DisplayName("PECS FLAG")]
        [OrderGrid(Order = 68)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string Pecs { get; set; }

        [DisplayName("SECURITY_CRITICAL_FUNCTION")]
        [OrderGrid(Order = 69)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string SecurityCriticalFunction { get; set; }

        [DisplayName("Business Critical")]
        [OrderGrid(Order = 70)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string Critical { get; set; }

        [DisplayName("ASSURANCE_CALL")]
        [OrderGrid(Order = 71)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string AssuranceCall { get; set; }

        [DisplayName("SERVICE_LEVEL")]
        [OrderGrid(Order = 72)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string ServiceLevel { get; set; }

        [DisplayName("LAST_PENTEST_DATE")]
        [OrderGrid(Order = 73)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string LastPenTestDateTsr { get; set; }

        [DisplayName("LAST_PENTEST_REFNO")]
        [OrderGrid(Order = 74)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string LastPenTestRefNo { get; set; }

        [DisplayName("PI_DATA")]
        [OrderGrid(Order = 75)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string PiData { get; set; }

        [DisplayName("ENCRYPTED_PI_DATA")]
        [OrderGrid(Order = 76)]
        [ColorGrid(Color = "blue")]
        [Default]
        public string EncryptedPiData { get; set; }

        [DisplayName("Application")]
        [OrderGrid(Order = 77)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string Application { get; set; }

        [DisplayName("CLOUD")]
        [OrderGrid(Order = 78)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string Cloud { get; set; }

        [DisplayName("Physical Server Hostname")]
        [OrderGrid(Order = 79)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalServerHostName { get; set; }

        [DisplayName("Physical Server IP Address")]
        [OrderGrid(Order = 80)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalServerIpaddress { get; set; }

        [DisplayName("Physical Server Serial Number")]
        [OrderGrid(Order = 81)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalServerSerialNumber { get; set; }

        [DisplayName(" Physical Server HW Model")]
        [OrderGrid(Order = 82)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalServerHwModel { get; set; }

        [DisplayName("Physical Server Vendor")]
        [OrderGrid(Order = 83)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalServerVendor { get; set; }

        [DisplayName("Virtual Server hosted on")]
        [OrderGrid(Order = 84)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string VirtualServerHostedon { get; set; }

        [DisplayName("Virtual Server Manufacturer")]
        [OrderGrid(Order = 85)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string VirtualServerManufacturer { get; set; }

        [DisplayName("Virtual Server Type of device")]
        [OrderGrid(Order = 86)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string VirtualServerTypeofDevice { get; set; }

        [DisplayName("Virtual Machine Type")]
        [OrderGrid(Order = 87)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string VirtualMachineType { get; set; }

        [DisplayName("Virtual Server IP Address")]
        [OrderGrid(Order = 88)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string VirtualServerIPAddress { get; set; }

        [DisplayName("Virtual Server Serial Number")]
        [OrderGrid(Order = 89)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string VirtualServerSerialNumber { get; set; }

        [DisplayName("Virtual Server type")]
        [OrderGrid(Order = 90)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string VirtualServerType { get; set; }

        [DisplayName("Physical Server OS Name")]
        [OrderGrid(Order = 91)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalServerOSName { get; set; }

        [DisplayName("Physical OS Version")]
        [OrderGrid(Order = 92)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalOSVersion { get; set; }

        [DisplayName("Physical Server OS Start Date")]
        [OrderGrid(Order = 93)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalServerOSStartDate { get; set; }

        [DisplayName("Physical Server OS installation date")]
        [OrderGrid(Order = 94)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalServerOSinstallationdate { get; set; }

        [DisplayName("Physical Server OS status")]
        [OrderGrid(Order = 95)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string PhysicalServerOSstatus { get; set; }

        [DisplayName("OS_NAME")]
        [OrderGrid(Order = 96)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string OperatingSystemName { get; set; }

        [DisplayName("OS_SW_VERSION")]
        [OrderGrid(Order = 97)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string OperatingSystemSwvVersion { get; set; }

        [DisplayName("OS Start Date")]
        [OrderGrid(Order = 98)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string OsStartDate { get; set; }

        [DisplayName("OS Installation Date")]
        [OrderGrid(Order = 99)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string OsInstallationDate { get; set; }

        [DisplayName("OS Status")]
        [OrderGrid(Order = 100)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string OsStatus { get; set; }

        [DisplayName("Software Name")]
        [OrderGrid(Order = 101)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string SoftwareName { get; set; }

        [DisplayName("Version")]
        [OrderGrid(Order = 102)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string Version { get; set; }

        [DisplayName("Release")]
        [OrderGrid(Order = 103)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string Release { get; set; }

        [DisplayName("Language")]
        [OrderGrid(Order = 104)]
        [ColorGrid(Color = "yellow")]
        [Default]
        public string Language { get; set; }


        //[DisplayName("HW_MANUFACTURER")]
        //[OrderGrid(Order = 5)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string HardwareVendorName { get; set; }

        //[DisplayName("HW_END_OF_SUPPORT")]
        //[OrderGrid(Order = 8)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string VendorHardwareEndOfSupportDate { get; set; }

        //[DisplayName("Software Product Version")]
        //[OrderGrid(Order = 21)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string SoftwareProductVersion { get; set; }

        //[DisplayName("SW_VENDOR")]
        //[OrderGrid(Order = 27)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string SoftwareVendorName { get; set; }

        //[DisplayName("SW_EOSL_CONTRACT_DATE")]
        //[OrderGrid(Order = 31)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string VendorSoftwareEndOfSupportDate { get; set; }

        //[DisplayName("SW_EEOSL_CONTRACT_DATE")]
        //[OrderGrid(Order = 32)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string MaintenanceSoftwareEndOfSupportDate { get; set; }

        //[DisplayName("Vertical Engineering Team")]
        //[OrderGrid(Order = 34)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string VerticalEngineeringTeam { get; set; }

        //[DisplayName("Vertical Sub-Domain")]
        //[OrderGrid(Order = 35)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string VerticalSubDomain { get; set; }

        //[DisplayName("Platform")]
        //[OrderGrid(Order = 36)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string Platform { get; set; }

        //[DisplayName("Risk Cluster")]
        //[OrderGrid(Order = 37)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string RiskCluster { get; set; }

        //[DisplayName("HW Operations Contact Point")]
        //[OrderGrid(Order = 38)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string HwOperationsContactPoint { get; set; }

        //[DisplayName("SW Operations Contact Point")]
        //[OrderGrid(Order = 39)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string SwOperationsContactPoint { get; set; }

        //[DisplayName("Asset Category")]
        //[OrderGrid(Order = 40)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string AssetCategory { get; set; }

        //[DisplayName("Asset Class")]
        //[OrderGrid(Order = 41)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string AssetClass { get; set; }

        //[DisplayName("PRODUCT_IMPORTANCE")]
        //[OrderGrid(Order = 44)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string ProductImportanceTsr { get; set; }

        //[DisplayName("Vendor End of Maintenance Date")]
        //[OrderGrid(Order = 47)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string VendorEndofMaintenanceDate { get; set; }

        //[DisplayName("Model")]
        //[OrderGrid(Order = 48)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string Model { get; set; }

        //[DisplayName("PLANNED_ACTION_DESCRIPTION")]
        //[OrderGrid(Order = 50)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string DescriptionOfPlannedaction { get; set; }

        //[DisplayName("IDENTIFIED_ACTION")]
        //[OrderGrid(Order = 51)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string IdentifiedActionTsr { get; set; }

        //[DisplayName("BUSINESS_SERVICE_NAME")]
        //[OrderGrid(Order = 52)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string TechnicalServiceNames { get; set; }

        //[DisplayName("HW OPS Contract End Date")]
        //[OrderGrid(Order = 53)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string HwOpsContractEndDate { get; set; }

        //[DisplayName("SW OPS Contract End Date")]
        //[OrderGrid(Order = 54)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string SwOpsContractEndDate { get; set; }

        //[DisplayName("Incident Class")]
        //[OrderGrid(Order = 55)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string IncidentClass { get; set; }

        //[DisplayName("Occurrence Probability")]
        //[OrderGrid(Order = 56)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string OccurrenceProbability { get; set; }

        //[DisplayName("Organization(ME_VERTICAL_RESPONSIBLE)/Person group")]
        //[OrderGrid(Order = 57)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string OrganizationorPersonGroup { get; set; }

        //[DisplayName("Type of Network Element")]
        //[OrderGrid(Order = 59)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string TypeofNetworkElement { get; set; }

        //[DisplayName("CRITICALITY_TYPE")]
        //[OrderGrid(Order = 97)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string Criticalitytype { get; set; }

        //[DisplayName("LAST_UPGRADE_DATE")]
        //[OrderGrid(Order = 99)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string LastUpgradeDateTsr { get; set; }

        //[DisplayName("BUDGET_ESTIMATED")]
        //[OrderGrid(Order = 101)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string BudgetEstimatedTsr { get; set; }

        //[DisplayName("BUNDLE_BUDGET")]
        //[OrderGrid(Order = 102)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string BundleBudgetTsr { get; set; }

        //[DisplayName("COMMENTS_ON_PROJECT_STATUS")]
        //[OrderGrid(Order = 104)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string CommentOnProjectStatusTsr { get; set; }

        //[DisplayName("PROJECT_END_DATE")]
        //[OrderGrid(Order = 105)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string ProjectEndDateTsr { get; set; }

        //[DisplayName("PROJECT_STATUS")]
        //[OrderGrid(Order = 106)]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string ProjectStatusTsr { get; set; }





        //[DisplayName("Operations Contact Point")]
        //[OrderGrid(Order = )]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string OperationsContactPoint { get; set; }

        //[DisplayName("Operations Maintenance Contract")]
        //[OrderGrid(Order = )]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string OperationsMaintenanceContract { get; set; }

        //[DisplayName("OPS Maintenance contract end date")]
        //[OrderGrid(Order = )]
        //[ColorGrid(Color = "Red")]
        //[Default]
        //public string OpsMaintenanceContractEndDate { get; set; }

    }
}
