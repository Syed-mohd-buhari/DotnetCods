using CAM.DataAttributes.Grid;
using System;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.FNT_Report
{
    public class TemsFntReportDtoGrid
    {
        [IgnoreGrid]
        [Default]
        [DisplayName("TEMS_FNT_REPORT_ID")]
        public long TemsFntReportId { get; set; }
        [Default]
        [DisplayName("ME_NAME")]
        public string HostName { get; set; }
        [Default]
        [DisplayName("ME_SOURCE_ASSET_ID")]
        public string SerialNumberOfHardwareAsset { get; set; }
        [Default]
        [DisplayName("ME_COUNTRY_LOCATED")]
        public string LocationOfHardwareAsset { get; set; }
        [Default]
        [DisplayName("HARDWARETYPE_OF_HARDWARE_ASSET")]
        public string HardwareTypeOfHardwareAsset { get; set; }
        [Default]
        [DisplayName("HW_MANUFACTURER")]
        public string VendorFnt { get; set; }
        [Default]
        [DisplayName("MANAGEMENT_IP_ADDRESS")]
        public string IpAddressOfHardwareAsset { get; set; }
        [Default]
        [DisplayName("ORGANISATION_NAME")]
        public string Market { get; set; }
        [Default]
        [DisplayName("HW_END_OF_LIFE")]
        public string HwEndOfLife { get; set; }
        [Default]
        [DisplayName("HW_END_OF_SUPPORT")]
        public string HwEndOfSupport { get; set; }
        [Default]
        [DisplayName("HW_END_OF_SALE")]
        public string HwEndOfSale { get; set; }
        [Default]
        [DisplayName("HARDWARE_MODULES")]
        public string HardwareModules { get; set; }
        [Default]
        [DisplayName("SOFTWARE_PRODUCT_TYPE")]
        public string SoftwareProductType { get; set; }
        [Default]
        [DisplayName("ME_SW_VERSION")]
        public string SoftwareProductVersion { get; set; }
        [Default]
        [DisplayName("IS_CLOUD_HOSTED")]
        public string SoftwareIsVirtualized { get; set; }
        [Default]
        [DisplayName("OS_NAME")]
        public string OperatingSystemOfVirtualMachine { get; set; }
        [Default]
        [DisplayName("Application hosted on Software")]
        public string ApplicationHostedOnSoftware { get; set; }
        [Default]
        [DisplayName("UUID_SERIALNUMBER_OF_SOFTWARE")]
        public string UuidSerialNumberOfSoftware { get; set; }
        [Default]
        [DisplayName("SW_VENDOR")]
        public string SoftwareVendor { get; set; }
        [Default]
        [DisplayName("Datacenter/location")]
        public string LocationOfSoftware { get; set; }
        [Default]
        [DisplayName("ME_SERVICE_TYPE")]
        public string ServiceType { get; set; }
        [Default]
        [DisplayName("SW_EEOSL_CONTRACT_DATE")]
        public string SwEndOfLife { get; set; }
        [Default]
        [DisplayName("SW_EOSL_CONTRACT_DATE")]
        public string SwEndOfSupport { get; set; }
        [Default]
        [DisplayName("SW_END_OF_SALE")]
        public string SwEndOfSale { get; set; }
        [Default]
        [DisplayName("Vertical Engineering Team")]
        public string VerticalEngineeringTeam { get; set; }
        [Default]
        [DisplayName("Vertical Sub-Domain")]
        public string VerticalSubdomain { get; set; }
        [Default]
        [DisplayName("Platform")]
        public string Platform { get; set; }
        [Default]
        [DisplayName("Risk Cluster")]
        public string RiskCluster { get; set; }
        [Default]
        [DisplayName("Asset Category")]
        public string AssetCategory { get; set; }
        [Default]
        [DisplayName("Asset Class")]
        public string AssetClass { get; set; }
        [Default]
        [DisplayName("ME_TYPE")]
        public string AssetTypeFnt { get; set; }
        [Default]
        [DisplayName("ME_DESCRIPTION")]
        public string AssetDescriptionFnt { get; set; }
        [Default]
        [DisplayName("PRODUCT_IMPORTANCE")]
        public string ProductImportance { get; set; }
        //[Default]
        //[DisplayName("OPERATIONS_MAINTENANCE_CONTRACT")]
        //public string OperationsMaintenanceContract { get; set; }
        [Default]
        [DisplayName("VENDOR_END_OF_MAINTENANCE_DATE")]
        public string VendorEndOfMaintenanceDateFnt { get; set; }
        [Default]
        [DisplayName("IDENTIFIED_ACTION")]
        public string IdentifiedActionFnt { get; set; }
        [Default]
        [DisplayName("PLANNED_ACTION_DESCRIPTION")]
        public string DescriptionOfPlannedActionFnt { get; set; }
        [Default]
        [DisplayName("HW Model")]
        public string Model { get; set; }
        [Default]
        [DisplayName("Business Service Name")]
        public string BusinessServiceName { get; set; }
        //[Default]
        //[DisplayName("OPS_MAINTENANCE_CONTRACT_END_DATE")]
        //public string OpMaintenanceContractendDate { get; set; }
        [Default]
        [DisplayName("Incident Class")]
        public string IncidentClass { get; set; }
        [Default]
        [DisplayName("Occurrence Probability")]
        public string OccurrenceProbability { get; set; }
        [Default]
        [DisplayName("ME_VERTICAL_RESPONSIBLE")]
        public string MeverticalResposible { get; set; }
        [Default]
        [DisplayName("Asset Status")]
        public string AssetStatus { get; set; }
        [Default]
        [DisplayName("Type of Network Element")]
        public string TypeOfNetworkElement { get; set; }
        [Default]
        [DisplayName("Local Market")]
        public string LocalMarket { get; set; }
        [Default]
        [DisplayName("Application")]
        public string Application { get; set; }
        [Default]
        [DisplayName("CLOUD(Assuming this as Private Cloud)")]
        public string Cloud { get; set; }
        [Default]
        [DisplayName("Physical Server Hostname")]
        public string PhysicalServerHostname { get; set; }
        [Default]
        [DisplayName("Physical Server IP Address")]
        public string PhysicalServerIpaddress { get; set; }
        [Default]
        [DisplayName("Physical Server Serial Number")]
        public string PhysicalServerSerialNumber { get; set; }
        [Default]
        [DisplayName("Physical Server HW Model")]
        public string PhysicalServerHwModel { get; set; }
        [Default]
        [DisplayName("Physical Server Vendor")]
        public string PhysicalServerVendor { get; set; }
        [Default]
        [DisplayName("Virtual Server hosted on")]
        public string VirtualServerHostedOn { get; set; }
        [Default]
        [DisplayName("Virtual Server Manufacturer")]
        public string VirtualServerManufacturer { get; set; }
        [Default]
        [DisplayName("Virtual Server Type of device")]
        public string VirtualServerTypeOfDevice { get; set; }
        [Default]
        [DisplayName("Virtual Machine Type")]
        public string VirtualMachineType { get; set; }
        [Default]
        [DisplayName("Virtual Server IP Address")]
        public string VirtualServerIpaddress { get; set; }
        [Default]
        [DisplayName("Virtual Server Serial Number")]
        public string VirtualServerSerialNumber { get; set; }
        [Default]
        [DisplayName("Virtual Server type")]
        public string VirtualServerType { get; set; }
        [Default]
        [DisplayName("OS name")]
        public string OsName { get; set; }
        [Default]
        [DisplayName("OS Version")]
        public string OsVersion { get; set; }
        [Default]
        [DisplayName("OS_START_DATE")]
        public string OsStartDate { get; set; }
        [Default]
        [DisplayName("OS_INSTALLATION_DATE")]
        public string OsInstallationDate { get; set; }
        [Default]
        [DisplayName("OS_STATUS")]
        public string OsStatus { get; set; }
        [Default]
        [DisplayName("Software Name")]
        public string SoftwareName { get; set; }
        [Default]
        [DisplayName("Version")]
        public string Version { get; set; }
        [Default]
        [DisplayName("Release")]
        public string Release { get; set; }
        [Default]
        [DisplayName("Manufacturer")]
        public string Manufacturer { get; set; }
        [Default]
        [DisplayName("LANGUAGE")]
        public string Language { get; set; }
        [Default]
        [DisplayName("HW OPS Contract Status")]
        public string HwOpsContractStatus { get; set; }
        [Default]
        [DisplayName("HW OPS Contract End Date")]
        public string HwOpsContractEndDate { get; set; }
        [Default]
        [DisplayName("SW OPS Contract Status")]
        public string SwOpsContractStatus { get; set; }
        [Default]
        [DisplayName("SW OPS Contract End Date")]
        public string SwOpsContractEndDate { get; set; }
        [Default]
        [DisplayName("HW Operations Contact Point")]
        public string HwOperationsContactPoint { get; set; }
        [Default]
        [DisplayName("SW Operations Contact Point")]
        public string SwOperationsContactPoint { get; set; }
        [Default]
        [DisplayName("Physical Server OS name")]
        public string PhysicalServerOsName { get; set; }
        [Default]
        [DisplayName("Physical Server OS Version")]
        public string PhysicalServerOsVersion { get; set; }
        [Default]
        [DisplayName("Physical Server OS Start Date")]
        public string PhysicalServerOsStartDate { get; set; }
        [Default]
        [DisplayName("Physical Server OS installation date")]
        public string PhysicalServerOsInstallationDate { get; set; }
        [Default]
        [DisplayName("Physical Server OS status")]
        public string PhysicalServerOsStatus { get; set; }
        [Default]
        [DisplayName("Planned HW Model")]
        public string PlannedHwModel { get; set; }

        [Default]
        [IgnoreGrid]
        public string CreationUser { get; set; }
        [Default]
        [IgnoreGrid]

        public DateTime CreationDate { get; set; }
        [Default]
        [IgnoreGrid]

        public string ModificationUser { get; set; }
        [Default]
        [IgnoreGrid]

        public DateTime ModificationDate { get; set; }


    }
    public class NonTemsFntReportDtoGrid : TemsFntReportDtoGrid
    {
        [IgnoreGrid]
        public long PassThroughId { get; set; }

    }

}
