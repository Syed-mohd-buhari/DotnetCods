using CAM.DataAttributes.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.FNT_Report
{
    public class FNTReportDtoGrid
    {
        [DisplayName("ME_NAME")]
        [OrderGrid(Order = 1)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string Hostname { get; set; }

        [DisplayName("ME_SOURCE_ASSET_ID")]
        [OrderGrid(Order = 2)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SerialNumberOfHardwareAsset { get; set; }


        [DisplayName("ME_COUNTRY_LOCATED")]
        [OrderGrid(Order = 3)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string LocationOfHardWareAsset { get; set; }

        [DisplayName("HARDWARE_TYPE")]
        [OrderGrid(Order = 4)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string HardwareTypeOfHardwareAsset { get; set; }

        [DisplayName("HW_MANUFACTURER")]
        [OrderGrid(Order = 5)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string HardwareVendorName { get; set; }

        [DisplayName("MANAGEMENT_IP_ADDRESS")]
        [OrderGrid(Order = 6)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SystemNameManagementIpAddress { get; set; }

        [DisplayName("ORGANISATION_NAME")]
        [OrderGrid(Order = 7)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string LocalMarketOwnerShip { get; set; }

        [DisplayName("HW_END_OF_SUPPORT")]
        [OrderGrid(Order = 8)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string VendorHardwareEndOfSupportDate { get; set; }

        [DisplayName("HW_END_OF_LIFE")]
        [OrderGrid(Order = 9)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string HardwareEndOfLifeDate { get; set; }

        [DisplayName("HW_END_OF_SALE")]
        [OrderGrid(Order = 10)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string HardwareEndOfSaleDate { get; set; }

        [DisplayName("HARDWARE_MODULES")]
        [OrderGrid(Order = 11)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string HardwareModules { get; set; }

        [DisplayName("ME_VERTICAL_RESPONSIBLE")]
        [OrderGrid(Order = 12)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string VerticalResponsible { get; set; }

        [DisplayName("Software Product Name")]
        [OrderGrid(Order = 13)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SoftwareProductType { get; set; }

        [DisplayName("ME_SW_VERSION")]
        [OrderGrid(Order = 14)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SoftwareProductVersion { get; set; }

        [DisplayName("OS_NAME")]
        [OrderGrid(Order = 15)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string OperatingSystemName { get; set; }

        [DisplayName("Application hosted on Software")]
        [OrderGrid(Order = 16)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ApplicationHostedOnSoftware { get; set; }

        [DisplayName("UUID/Serialnumber of Software")]
        [OrderGrid(Order = 17)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string UuidOrSerialnumberOfSoftware { get; set; }

        [DisplayName("SW_VENDOR")]
        [OrderGrid(Order = 18)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SoftwareVendorName { get; set; }

        [DisplayName("ME_SERVICE_TYPE")]
        [OrderGrid(Order = 19)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ServiceType { get; set; }

        [DisplayName("SW_END_OF_LIFE")]
        [OrderGrid(Order = 20)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SoftwareEndOfLifeDate { get; set; }

        [DisplayName("SW_END_OF_SUPPORT")]
        [OrderGrid(Order = 21)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SoftwareEndOfSupportDate { get; set; }

        [DisplayName("SW_END_OF_SALE")]
        [OrderGrid(Order = 22)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string SoftwareEndOfSaleDate { get; set; }

        [DisplayName("LOCATION_NAME")]
        [OrderGrid(Order = 23)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string LocationName { get; set; }

        [DisplayName("IS_CLOUD_HOSTED")]
        [OrderGrid(Order = 24)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string CloudHostedAsset { get; set; }

        [DisplayName("Vertical Engineering Team")]
        [OrderGrid(Order = 25)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string VerticalEngineeringTeam { get; set; }

        [DisplayName("Vertical Sub-Domain")]
        [OrderGrid(Order = 26)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string VerticalSubDomain { get; set; }

        [DisplayName("Platform")]
        [OrderGrid(Order = 27)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string Platform { get; set; }

        [DisplayName("Risk Cluster")]
        [OrderGrid(Order = 28)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string RiskCluster { get; set; }

        [DisplayName("Operation Contact Point")]
        [OrderGrid(Order = 29)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string OperationContactPoint { get; set; }

        [DisplayName("Asset Category")]
        [OrderGrid(Order = 30)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string AssetCategory { get; set; }

        [DisplayName("Asset Class")]
        [OrderGrid(Order = 31)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string AssetClass { get; set; }

        [DisplayName("Asset Type")]
        [OrderGrid(Order = 32)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string AssetType { get; set; }

        [DisplayName("Asset Description")]
        [OrderGrid(Order = 33)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string AssetDescription { get; set; }

        [DisplayName("PRODUCT_IMPORTANCE")]
        [OrderGrid(Order = 34)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string ProductImportance { get; set; }

        [DisplayName("Operations Maintenance Contract")]
        [OrderGrid(Order = 35)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string OperationsMaintenanceContract { get; set; }

        [DisplayName("IDENTIFIED_ACTION")]
        [OrderGrid(Order = 36)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string IdentifiedAction { get; set; }

        [DisplayName("PLANNED_ACTION_DESCRIPTION")]
        [OrderGrid(Order = 37)]
        [ColorGrid(Color = "Red")]
        [Default]
        public string DescriptionOfPlannedAction { get; set; }

        [DisplayName("MODEL")]
        [OrderGrid(Order = 38)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string PlannedHwModel { get; set; }

        [DisplayName("BUSINESS_SERVICE_NAME")]
        [OrderGrid(Order = 39)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string BusinessServiceNames { get; set; }

        [DisplayName("OPS Maintenance contract end date")]
        [OrderGrid(Order = 40)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OpsMaintenanceContractEndDate { get; set; }

        [DisplayName("Incident Class")]
        [OrderGrid(Order = 41)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string IncidentClass { get; set; }

        [DisplayName("Occurrence Probability")]
        [OrderGrid(Order = 42)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OccurrenceProbability { get; set; }

        [DisplayName("Organization(ME_VERTICAL_RESPONSIBLE)/Person group")]
        [OrderGrid(Order = 43)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OrganizationResponsible { get; set; }

        [DisplayName("Asset Status")]
        [OrderGrid(Order = 44)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string AssetStatus { get; set; }
         
        [DisplayName("Type Of Network Element")]
        [OrderGrid(Order = 45)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string TypeOfNetworkElement { get; set; }
         
        [DisplayName("ORGANISATION_NAME")]
        [OrderGrid(Order = 46)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string LocalMarket { get; set; }

        [DisplayName("Application")]
        [OrderGrid(Order = 47)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Application { get; set; }

        [DisplayName("CLOUD")]
        [OrderGrid(Order = 48)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Cloud { get; set; }

        [DisplayName("Physical Server Hostname")]
        [OrderGrid(Order = 49)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string PhysicalServerHostname { get; set; }

        [DisplayName("Physical Server IP Address")]
        [OrderGrid(Order = 50)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string PhysicalServerIPAddress { get; set; }

        [DisplayName("Physical Server Serial Number")]
        [OrderGrid(Order = 51)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string PhysicalServerSerialNumber { get; set; }

        [DisplayName("Physical Server HW Model")]
        [OrderGrid(Order = 52)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string PhysicalServerHWModel { get; set; }

        [DisplayName("Physical Server Vendor")]
        [OrderGrid(Order = 53)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string PhysicalServerVendor { get; set; }

        [DisplayName("Virtual Server hosted on")]
        [OrderGrid(Order = 54)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string VirtualServerHostedOn { get; set; }

        [DisplayName("Virtual Server Manufacturer")]
        [OrderGrid(Order = 55)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string VirtualServerManufacturer { get; set; }

        [DisplayName("Virtual Server Type of device")]
        [OrderGrid(Order = 56)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string VirtualServerTypeOfDevice { get; set; }

        [DisplayName("Virtual Machine Type")]
        [OrderGrid(Order = 57)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string VirtualMachineType { get; set; }

        [DisplayName("Virtual Server IP Address")]
        [OrderGrid(Order = 58)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string VirtualServerIPAddress { get; set; }

        [DisplayName("Virtual Server Serial Number")]
        [OrderGrid(Order = 59)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string VirtualServerSerialNumber { get; set; }

        [DisplayName("VirtualServerType")]
        [OrderGrid(Order = 60)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string VirtualServerType { get; set; }

        [DisplayName("OS NAME")]
        [OrderGrid(Order = 61)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OsName { get; set; }

        [DisplayName("OS Version")]
        [OrderGrid(Order = 62)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OsVersion { get; set; }

        [DisplayName("OS Start Date")]
        [OrderGrid(Order = 63)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OsStartDate { get; set; }

        [DisplayName("OS Installation Date")]
        [OrderGrid(Order = 64)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OsInstallationDate { get; set; }

        [DisplayName("OS Status")]
        [OrderGrid(Order = 65)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string OsStatus { get; set; }

        [DisplayName("Software Name")]
        [OrderGrid(Order = 66)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string SoftwareName { get; set; }

        [DisplayName("Version")]
        [OrderGrid(Order = 67)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Version { get; set; }

        [DisplayName("Release")]
        [OrderGrid(Order = 68)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Release { get; set; }

        [DisplayName("Manufacturer")]
        [OrderGrid(Order = 69)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Manufacturer { get; set; }

        [DisplayName("Language")]
        [OrderGrid(Order = 70)]
        [ColorGrid(Color = "Grey")]
        [Default]
        public string Language { get; set; }

        [DateRangeGrid]
        [DisplayName("Last Modified Date")]
        [OrderGrid(Order = 71)]
        public DateTime? LastModified { get; set; }

        [MailTo]
        [OrderGrid(Order = 72)]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }
    }
}
