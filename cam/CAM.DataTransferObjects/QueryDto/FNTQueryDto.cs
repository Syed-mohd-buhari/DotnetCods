using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.QueryDto
{
    public class FNTQueryDto : QueryObject
    {

        public List<string> Hostname { get; set; }
        public List<string> SerialNumberOfHardwareAsset { get; set; }
        public List<string> LocationOfHardWareAsset { get; set; }
        public List<string> HardwareTypeOfHardwareAsset { get; set; }
        public List<string> HardwareVendorName { get; set; }
        public List<string> SystemNameManagementIpAddress { get; set; }
        public List<string> LocalMarketOwnerShip { get; set; }
        public List<string> VendorHardwareEndOfSupportDate { get; set; }      
        public List<string> HardwareEndOfLifeDate { get; set; }
        public List<string> HardwareEndOfSaleDate { get; set; }       
        public List<string> HardwareModules { get; set; }
        public List<string> VerticalResponsible { get; set; }
        public List<string> SoftwareProductType { get; set; }
        public List<string> SoftwareProductVersion { get; set; }
        public List<string> OperatingSystemName { get; set; }
        public List<string> ApplicationHostedOnSoftware { get; set; }
        public List<string> UuidOrSerialnumberOfSoftware { get; set; }
        public List<string> SoftwareVendorName { get; set; }
        public List<string> ServiceType { get; set; }
        public List<string> SoftwareEndOfLifeDate { get; set; }       
        public List<string> SoftwareEndOfSupportDate { get; set; }
        public List<string> SoftwareEndOfSaleDate { get; set; }
        public List<string> LocationName { get; set; }
        public List<string> CloudHostedAsset { get; set; }
        public List<string> VerticalEngineeringTeam { get; set; }
        public List<string> VerticalSubDomain { get; set; }
        public List<string> Platform { get; set; }
        public List<string> RiskCluster { get; set; }
        public List<string> OperationContactPoint { get; set; }
        public List<string> AssetCategory { get; set; }
        public List<string> AssetClass { get; set; }
        public List<string> AssetType { get; set; }
        public List<string> AssetDescription { get; set; }
        public List<string> ProductImportance { get; set; }
        public List<string> OperationsMaintenanceContract { get; set; }
        public List<string> IdentifiedAction { get; set; }
        public List<string> DescriptionOfPlannedAction { get; set; }
        public List<string> PlannedHwModel { get; set; }
        public List<string> BusinessServiceNames { get; set; }
        public List<string> OpsMaintenanceContractEndDate { get; set; }
        public List<string> IncidentClass { get; set; }
        public List<string> OccurrenceProbability { get; set; }
        public List<string> OrganizationResponsible { get; set; }
        public List<string> AssetStatus { get; set; }
        public List<string> TypeOfNetworkElement { get; set; }
        public List<string> LocalMarket { get; set; }
        public List<string> Application { get; set; }
        public List<string> Cloud { get; set; }
        public List<string> PhysicalServerHostname { get; set; }
        public List<string> PhysicalServerIPAddress { get; set; }
        public List<string> PhysicalServerSerialNumber { get; set; }
        public List<string> PhysicalServerHWModel { get; set; }
        public List<string> PhysicalServerVendor { get; set; }
        public List<string> VirtualServerHostedOn { get; set; }
        public List<string> VirtualServerManufacturer { get; set; }
        public List<string> VirtualServerTypeOfDevice { get; set; }
        public List<string> VirtualMachineType { get; set; }
        public List<string> VirtualServerIPAddress { get; set; }
        public List<string> VirtualServerSerialNumber { get; set; }
        public List<string> VirtualServerType { get; set; }
        public List<string> OsName { get; set; }
        public List<string> OsVersion { get; set; }
        public List<string> OsStartDate { get; set; }       
        public List<string> OsInstallationDate { get; set; }
        public List<string> OsStatus { get; set; }
        public List<string> SoftwareName { get; set; }
        public List<string> Version { get; set; }
        public List<string> Release { get; set; }
        public List<string> Manufacturer { get; set; }
        public List<string> Language { get; set; }
    }
}
