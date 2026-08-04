using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Enum;
using System.Collections.Generic;

namespace CAM.DataTransferObjects.QueryDto.FNT
{
    public class TemsFntReportQueryDto : QueryObject
    {
        public List<long> TemsFntReportId { get; set; }
        public List<string> HostName { get; set; }
        public List<string> SerialNumberOfHardwareAsset { get; set; }
        public List<string> LocationOfHardwareAsset { get; set; }
        public List<string> HardwareTypeOfHardwareAsset { get; set; }
        public List<string> Vendor { get; set; }
        public List<string> IpAddressOfHardwareAsset { get; set; }
        public List<string> Market { get; set; }
        public List<string> HwEndOfLife { get; set; }
        public List<string> HwEndOfSupport { get; set; }
        public List<string> HwEndOfSale { get; set; }
        public List<string> HardwareModules { get; set; }
        public List<string> SoftwareProductType { get; set; }
        public List<string> SoftwareProductVersion { get; set; }
        public List<string> SoftwareIsVirtualized { get; set; }
        public List<string> OperatingSystemOfVirtualMachine { get; set; }
        public List<string> ApplicationHostedOnSoftware { get; set; }
        public List<string> UuidSerialNumberOfSoftware { get; set; }
        public List<string> SoftwareVendor { get; set; }
        public List<string> LocationOfSoftware { get; set; }
        public List<string> ServiceType { get; set; }
        public List<string> SwEndOfLife { get; set; }
        public List<string> SwEndOfSupport { get; set; }
        public List<string> SwEndOfSale { get; set; }
        public List<string> VerticalEngineeringTeam { get; set; }
        public List<string> VerticalSubdomain { get; set; }
        public List<string> Platform { get; set; }
        public List<string> RiskCluster { get; set; }
        public List<string> OperationsContactPoint { get; set; }
        public List<string> AssetCategory { get; set; }
        public List<string> AssetClass { get; set; }
        public List<string> AssetType { get; set; }
        public List<string> AssetDescription { get; set; }
        public List<string> ProductImportance { get; set; }
        public List<string> OperationsMaintenanceContract { get; set; }
        public List<string> VendorEndOfMaintenanceDate { get; set; }
        public List<string> IdentifiedAction { get; set; }
        public List<string> DescriptionOfPlannedAction { get; set; }
        public List<string> Model { get; set; }
        public List<string> BusinessServiceName { get; set; }
        public List<string> OpMaintenanceContractendDate { get; set; }
        public List<string> IncidentClass { get; set; }
        public List<string> OccurrenceProbability { get; set; }
        public List<string> MeverticalResposible { get; set; }
        public List<string> AssetStatus { get; set; }
        public List<string> TypeOfNetworkElement { get; set; }
        public List<string> LocalMarket { get; set; }
        public List<string> Application { get; set; }
        public List<string> Cloud { get; set; }
        //public List<string> DataCenterocation { get; set; }
        public List<string> PhysicalServerHostname { get; set; }
        public List<string> PhysicalServerIpaddress { get; set; }
        public List<string> PhysicalServerSerialNumber { get; set; }
        public List<string> PhysicalServerHwModel { get; set; }
        public List<string> PhysicalServerVendor { get; set; }
        public List<string> VirtualServerHostedOn { get; set; }
        public List<string> VirtualServerManufacturer { get; set; }
        public List<string> VirtualServerTypeOfDevice { get; set; }
        public List<string> VirtualMachineType { get; set; }
        public List<string> VirtualServerIpaddress { get; set; }
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
        public List<string> HwOpsContractStatus { get; set; }
        public List<string> HwOpsContractEndDate { get; set; }
        public List<string> SwOpsContractStatus { get; set; }
        public List<string> SwOpsContractEndDate { get; set; }
        public List<string> HwOperationsContactPoint { get; set; }
        public List<string> SwOperationsContactPoint { get; set; }
        public List<string> PhysicalServerOsName { get; set; }
        public List<string> PhysicalServerOsVersion { get; set; }
        public List<string> PhysicalServerOsStartDate { get; set; }
        public List<string> PhysicalServerOsInstallationDate { get; set; }
        public List<string> PhysicalServerOsStatus { get; set; }
        public List<string> PlannedHwModel { get; set; }
        public List<string> CreationUser { get; set; }
        public DateFilter CreationDate { get; set; }
        public List<string> ModificationUser { get; set; }
        public DateFilter ModificationDate { get; set; }

    }
    public class NonTemsFntReportQueryDto: TemsFntReportQueryDto
    {
        public List<long> PassThroughId { get; set; }
        public List<long> NonTemsVertical { get; set; }
    }
}
