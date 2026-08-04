using CAM.DataAttributes.Grid;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CAM.DataTransferObjects.Entita.TsrPassThrough
{
    public class TsrPassThroughDtoGrid 
    {
        [IgnoreGrid]
        [OrderGrid(Order = 1)]
        public long TsrPassThroughId { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order = 2)]        
        [Default]
        public string AssetId { get; set; }


        [DisplayName("ME_SOURCE_ASSET_ID")]
        [OrderGrid(Order = 3)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string VodafoneUniqueIdentifier { get; set; }

        [DisplayName("ME_NAME")]
        [OrderGrid(Order = 4)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string AssetName { get; set; }

        [DisplayName("ME_DESCRIPTION")]
        [OrderGrid(Order = 5)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string AssetDescriptionOrPurpose { get; set; }

        [DisplayName("ME_TYPE")]
        [OrderGrid(Order = 6)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string AssetTypeTsr { get; set; }

        [DisplayName("BUSINESS_OWNER")]
        [OrderGrid(Order = 7)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string BusinessOwner { get; set; }

        [DisplayName("SUPPORT_OWNER")]
        [OrderGrid(Order = 8)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string SupportOwner { get; set; }

        [DisplayName("SUPPORT_TEAM")]
        [OrderGrid(Order = 9)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SupportTeam { get; set; }

        [DisplayName("SUPPORT_DOMAIN")]
        [OrderGrid(Order = 10)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SupportTeamsPlaceInTheOrganisation { get; set; }

        [DisplayName("ME_SERVICE_TYPE")]
        [OrderGrid(Order = 11)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string AssetFunction { get; set; }

        [DisplayName("ME_DEPLOYMENT_STATUS")]
        [OrderGrid(Order = 12)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string DeploymentOrLifeCycleStatus { get; set; }

        [DisplayName("RISK_ID")]
        [OrderGrid(Order = 13)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string RelatedRiskIdsFromRiskRegisters { get; set; }

        [DisplayName("REGULATORY_SCOPE")]
        [OrderGrid(Order = 14)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string RegulatoryScope { get; set; }

        [DisplayName("ME_COUNTRY_LOCATED")]
        [OrderGrid(Order = 15)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string CountryWhereAssetIsLocated { get; set; }

        [DisplayName("GEO_LOCATION")]
        [OrderGrid(Order = 16)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string Geolocation { get; set; }

        [DisplayName("INFRASTRUCTURE LOCATION")]
        [OrderGrid(Order = 17)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string Infrastructure { get; set; }

        [DisplayName("UPSTREAM_DEPENDENCIES")]
        [OrderGrid(Order = 18)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string UpStreamDependencies { get; set; }

        [DisplayName("DOWNSTREAM_DEPENDENCIES")]
        [OrderGrid(Order = 19)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string DownStreamDependencies { get; set; }

        [DisplayName("CHANGE_DESCRIPTION")]
        [OrderGrid(Order = 20)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string ChangesToTheassetSinceDeployment { get; set; }

        [DisplayName("IS_CLOUD_HOSTED")]
        [OrderGrid(Order = 21)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string CloudHostedAsset { get; set; }

        [DisplayName("CLOUD_TYPE")]
        [OrderGrid(Order = 22)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string CloudType { get; set; }

        [DisplayName("CLOUD_VENDOR")]
        [OrderGrid(Order = 23)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string CloudVendor { get; set; }

        [DisplayName("EQUIPMENT_NAME")]
        [OrderGrid(Order = 24)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string EquipmentName { get; set; }

        [DisplayName("VIRTUAL_PLATFORM_LOCATION")]
        [OrderGrid(Order = 25)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string HostLocationWithInPhysicalLocation { get; set; }

        [DisplayName("ME_SW_VENDOR")]
        [OrderGrid(Order = 26)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SoftwareVendorName { get; set; }

        [DisplayName("Model")]
        [OrderGrid(Order = 27)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Model { get; set; }

        [DisplayName("FIRMWARE_VERSION")]
        [OrderGrid(Order = 28)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string FirmwareVersion { get; set; }

        [DisplayName("FIRMWARE_PATCH_LEVEL")]
        [OrderGrid(Order = 29)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string FirmwareVersionPatchLevel { get; set; }

        [DisplayName("MAINTENANCE_SUPPORT_SUPPLIER(HW)")]
        [OrderGrid(Order = 30)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string MaintenanceSupportSupplier { get; set; }

        [DisplayName("HW_END_OF_SUPPORT")]
        [OrderGrid(Order = 31)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string VendorHardwareEndOfSupportDate { get; set; }

        [DisplayName("HW_END_OF_LIFE")]
        [OrderGrid(Order = 32)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string MaintenanceHardwareEndOfSupportDate { get; set; }

        [DisplayName("DEPENDENT_HARDWARE")]
        [OrderGrid(Order = 33)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string DependantHardware { get; set; }

        [DisplayName("INSTANCE_TYPE")]
        [OrderGrid(Order = 34)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string InstanceType { get; set; }

        [DisplayName("OS_NAME")]
        [OrderGrid(Order = 35)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string OperatingSystemName { get; set; }

        [DisplayName("OS_SW_VERSION")]
        [OrderGrid(Order = 36)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OperatingSystemSwvVersion { get; set; }

        [DisplayName("OS_PATCH_LEVEL")]
        [OrderGrid(Order = 37)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OperatingSystemSwVersionPatchLevel { get; set; }

        [DisplayName("SYSTEMNAME")]
        [OrderGrid(Order = 38)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string SystemNameDns { get; set; }

        [DisplayName("MANAGEMENT_IP_ADDRESS")]
        [OrderGrid(Order = 39)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SystemNameManagementIpAddress { get; set; }

        [DisplayName("SYSTEMNAME_NETBIOS")]
        [OrderGrid(Order = 40)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string SystemNameNetBios { get; set; }

        [DisplayName("HOSTNAME")]
        [OrderGrid(Order = 41)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SystemNameHostName { get; set; }

        [DisplayName("ME_LIVE_STATUS_DATE")]
        [OrderGrid(Order = 42)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string DateAssetMovedToLiveStatus { get; set; }

        [DisplayName("ME_DECOMMISSIONED_DATE")]
        [OrderGrid(Order = 43)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string DateAssetDecommissioned { get; set; }

        [DisplayName("HW_MANUFACTURER")]
        [OrderGrid(Order = 44)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string HardwareVendorName { get; set; }

        [DisplayName("MAINTENANCE_SUPPORT_SUPPLIER(SW)")]
        [OrderGrid(Order = 45)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string MaintenanceSupportSupplierSecond { get; set; }

        [DisplayName("SW_EOSL_CONTRACT_DATE")]
        [OrderGrid(Order = 46)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string VendorSoftwareEndOfSupportDate { get; set; }

        [DisplayName("SW_EEOSL_CONTRACT_DATE")]
        [OrderGrid(Order = 47)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string MaintenanceSoftwareEndOfSupportDate { get; set; }

        [DisplayName("DEPENDANT_SYSTEM_SW")]
        [OrderGrid(Order = 48)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string DependantSystemSoftware { get; set; }

        [DisplayName("RESILIENCE_MODEL")]
        [OrderGrid(Order = 49)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ResilienceModel { get; set; }

        [DisplayName("RESILIENCE_GEOGRAPHIC_SITE")]
        [OrderGrid(Order = 50)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string GeographicSiteResilience { get; set; }

        [DisplayName("RESILIENCE_LOCAL_SITE")]
        [OrderGrid(Order = 51)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string LocalSiteResilience { get; set; }

        [DisplayName("DEPENDANT_PRODUCT_NAME")]
        [OrderGrid(Order = 52)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string NameOfProductsDependantOnAsset { get; set; }

        [DisplayName("BUSINESS_SERVICE_NAME")]
        [OrderGrid(Order = 53)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string TechnicalServiceNames { get; set; }

        [DisplayName("CUSTOMER")]
        [OrderGrid(Order = 54)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Customer { get; set; }

        [DisplayName("ME_PRIVILEGED_ACCESS_LOGGING")]
        [OrderGrid(Order = 55)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string PrivilegedAccessLogging { get; set; }

        [DisplayName("HW_COMPONENT_NAME")]
        [OrderGrid(Order = 56)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string BoardOrModuleNameComponentName { get; set; }

        [DisplayName("HW_COMPONENT_SUBTYPE")]
        [OrderGrid(Order = 57)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string BoardOrModuleTypeComponentSubtype { get; set; }

        [DisplayName("HW_COMPONENT_VERSION")]
        [OrderGrid(Order = 58)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string BoardOrModuleTypeComponentVersionNumber { get; set; }

        [DisplayName("EXPOSED EDGE FLAG")]
        [OrderGrid(Order = 59)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ExposedEdge { get; set; }

        [DisplayName("EXTERNAL FACING FLAG")]
        [OrderGrid(Order = 60)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ExternallyFacingSystem { get; set; }

        [DisplayName("MANAGEMENT_PLANE")]
        [OrderGrid(Order = 61)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string ManagementPlane { get; set; }

        [DisplayName("NETWORK_OVERSIGHT")]
        [OrderGrid(Order = 62)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string NetworkOverSightFunction { get; set; }

        [DisplayName("PECN FLAG")]
        [OrderGrid(Order = 63)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string Pecn { get; set; }

        [DisplayName("PECS FLAG")]
        [OrderGrid(Order = 64)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string Pecs { get; set; }

        [DisplayName("SECURITY_CRITICAL_FUNCTION")]
        [OrderGrid(Order = 65)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SecurityCriticalFunction { get; set; }

        [DisplayName("PRODUCT_IMPORTANCE")]
        [OrderGrid(Order = 66)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ProductImportanceTsr { get; set; }

        [DisplayName("Business Critical")]
        [OrderGrid(Order = 67)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Critical { get; set; }

        [DisplayName("CRITICALITY_TYPE")]
        [OrderGrid(Order = 68)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Criticalitytype { get; set; }

        [DisplayName("ME_SERIAL_NUMBER")]
        [OrderGrid(Order = 69)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string SerialNumberTsr { get; set; }

        [DisplayName("HW_PART_NUMBER")]
        [OrderGrid(Order = 70)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string PartNumber { get; set; }

        [DisplayName("PLANNED_ACTION_DESCRIPTION")]
        [OrderGrid(Order = 71)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string DescriptionOfPlannedaction { get; set; }

        [DisplayName("IDENTIFIED_ACTION")]
        [OrderGrid(Order = 72)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string IdentifiedActionTsr { get; set; }

        [DisplayName("LAST_UPGRADE_DATE")]
        [OrderGrid(Order = 73)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string LastUpgradeDateTsr { get; set; }

        [DisplayName("ME_ENVIRONMENT")]
        [OrderGrid(Order = 74)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ProdOrLab { get; set; }

        [DisplayName("ORGANISATION_NAME")]
        [OrderGrid(Order = 75)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string LocalMarketOwnerShip { get; set; }

        [DisplayName("BUDGET_ESTIMATED")]
        [OrderGrid(Order = 76)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string BudgetEstimatedTsr { get; set; }

        [DisplayName("BUNDLE_BUDGET")]
        [OrderGrid(Order = 77)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string BundleBudgetTsr { get; set; }

        [DisplayName("ASSURANCE_CALL")]
        [OrderGrid(Order = 78)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string AssuranceCall { get; set; }

        [DisplayName("COMMENTS_ON_PROJECT_STATUS")]
        [OrderGrid(Order = 79)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string CommentOnProjectStatusTsr { get; set; }

        [DisplayName("PROJECT_END_DATE")]
        [OrderGrid(Order = 80)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ProjectEndDateTsr { get; set; }

        [DisplayName("PROJECT_STATUS")]
        [OrderGrid(Order = 81)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ProjectStatusTsr { get; set; }

        [DisplayName("SERVICE_LEVEL")]
        [OrderGrid(Order = 82)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string ServiceLevel { get; set; }

        [DisplayName("LAST_PENTEST_DATE")]
        [OrderGrid(Order = 83)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string LastPenTestDateTsr { get; set; }

        [DisplayName("LAST_PENTEST_REFNO")]
        [OrderGrid(Order = 84)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string LastPenTestRefNo { get; set; }

        [DisplayName("PI_DATA")]
        [OrderGrid(Order = 85)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string PiData { get; set; }

        [DisplayName("ENCRYPTED_PI_DATA")]
        [OrderGrid(Order = 86)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string EncryptedPiData { get; set; }

        [DisplayName("ME_PRODUCT_NAME")]
        [OrderGrid(Order = 87)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string MeProductName { get; set; }

        [DisplayName("ME_SW_VERSION")]
        [OrderGrid(Order = 88)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string MeSoftwareVersion { get; set; }

        [DisplayName("ME_SUB_DOMAIN_RESPONSIBLE")]
        [OrderGrid(Order = 89)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string MeSubDomainResponsible { get; set; }

        [IgnoreGrid]
        public long? NonTemsVertical { get; set; }

        [DisplayName("Vertical Name")]
        [OrderGrid(Order = 90)]
        public string VerticalName { get; set; }

        [IgnoreGrid]
        public long? RecordClassifier { get; set; }

        [DateRangeGrid]
        [DisplayName("Last Modified Date")]
        [OrderGrid(Order = 91)]
        public DateTime? LastModified { get; set; }

        [MailTo]
        [OrderGrid(Order = 92)]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }
    }


    public class TsrDictionaryGlossaryItemsDto  
    {
        public int GlossaryItemsId { get; set; }

        [Default]
        [OrderGrid(Order = 1)]
        [DisplayName("Cloumn Name")]
        [StringLength(50)]
        public string Header { get; set; }

        [Default]
        [OrderGrid(Order = 2)]
        [DisplayName("Description")]
        [StringLength(2000)]
        public string Description { get; set; }
 
    }
}
