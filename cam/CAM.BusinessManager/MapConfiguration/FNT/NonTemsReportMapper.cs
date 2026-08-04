using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.FNT_Report;
using CAM.Entities.Models.PassThroughData;
using CAM.Repository.Helpers;
using DocumentFormat.OpenXml.Office.Word;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.MapConfiguration.FNT
{
    public class NonTemsReportMapper:Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager;
        public NonTemsReportMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            _commonManager = commonManager; 
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<AssetPassThrough, NonTemsFntReportDtoGrid>()
            .ForMember(x => x.PassThroughId, s => s.MapFrom(src => src.PassThroughId))
            .ForMember(x => x.HostName, s => s.MapFrom(src => src.AssetName))
            .ForMember(x => x.SerialNumberOfHardwareAsset, s => s.MapFrom(src => src.VodafoneUniqueIdentifier))
            .ForMember(x => x.LocationOfHardwareAsset, s => s.MapFrom(src => src.CountryWhereAssetIsLocated))
            .ForMember(x => x.HardwareTypeOfHardwareAsset, s => s.MapFrom(src => src.HardwareTypeofHardwareAsset))
            .ForMember(x => x.VendorFnt, s => s.MapFrom(src => _commonManager.GetHwPassthroughlcmRecords(src.ResourceKeyNavigation.LocalMarket, src.ResourceKeyNavigation.AssetClass, src.ResourceKeyNavigation.HardwareModel).Result.Hwvendor))
            .ForMember(x => x.IpAddressOfHardwareAsset, s => s.MapFrom(src => src.SystemNameManagementIpAddress))
            .ForMember(x => x.Market, s => s.MapFrom(src => src.LocalMarketOwnerShip))
            .ForMember(x => x.HwEndOfLife, s => s.MapFrom(src => src.MaintenanceHardwareEndOfSupportDate))
            .ForMember(x => x.HwEndOfSupport, s => s.MapFrom(src => src.VendorHardwareEndOfSupportDate))
            .ForMember(x => x.HwEndOfSale, s => s.MapFrom(src => src.HwEndofSale))
            .ForMember(x => x.HardwareModules, s => s.MapFrom(src => src.BoardOrModuleNameComponentName))
            .ForMember(x => x.SoftwareProductType, s => s.MapFrom(src => src.SoftwareProductType))
            .ForMember(x => x.SoftwareProductVersion, s => s.MapFrom(src => src.ResourceKeyNavigation.SoftwareVersion))
            .ForMember(x => x.SoftwareIsVirtualized, s => s.MapFrom(src => src.CloudHostedAsset))
            .ForMember(x => x.OperatingSystemOfVirtualMachine, s => s.MapFrom(src => src.OperatingSystemName))
            .ForMember(x => x.ApplicationHostedOnSoftware, s => s.MapFrom(src => src.ApplicationHostedonSoftware))
            .ForMember(x => x.UuidSerialNumberOfSoftware, s => s.MapFrom(src => src.UuidorSerialNumberofSoftware))
            .ForMember(x => x.SoftwareVendor, s => s.MapFrom(src => src.ResourceKeyNavigation.SWVendor))
            .ForMember(x => x.LocationOfSoftware, s => s.MapFrom(src => src.Geolocation))
            .ForMember(x => x.ServiceType, s => s.MapFrom(src => src.AssetFunction))
            .ForMember(x => x.SwEndOfLife, s => s.MapFrom(src => src.ResourceKeyNavigation.SWVendorEndOfMaintenanceDate))
            .ForMember(x => x.SwEndOfSupport, s => s.MapFrom(src => src.ResourceKeyNavigation.VendorEndOfVulnerabilitySecuritySupportDate))
            .ForMember(x => x.SwEndOfSale, s => s.MapFrom(src => src.SwEndofSale))
            .ForMember(x => x.VerticalEngineeringTeam, s => s.MapFrom(src => src.ResourceKeyNavigation.VerticalEngineeringTeam))
            .ForMember(x => x.VerticalSubdomain, s => s.MapFrom(src => src.ResourceKeyNavigation.VerticalSubDomain))
            .ForMember(x => x.Platform, s => s.MapFrom(src => src.ResourceKeyNavigation.Platform))
            .ForMember(x => x.RiskCluster, s => s.MapFrom(src => src.ResourceKeyNavigation.RiskCluster))
            //.ForMember(x => x.OperationsContactPoint, s => s.MapFrom(src => src.OperationsContactPoint))
            .ForMember(x => x.AssetCategory, s => s.MapFrom(src => src.AssetFunction))
            .ForMember(x => x.AssetClass, s => s.MapFrom(src => src.ResourceKeyNavigation.AssetClass))
            .ForMember(x => x.AssetTypeFnt, s => s.MapFrom(src => src.AssetType))
            .ForMember(x => x.AssetDescriptionFnt, s => s.MapFrom(src => src.AssetDescriptionOrPurpose))
            .ForMember(x => x.ProductImportance, s => s.MapFrom(src => src.ResourceKeyNavigation.ProductImportance))
            //.ForMember(x => x.OperationsMaintenanceContract, s => s.MapFrom(src => src.OperationsMaintenanceContract))
            .ForMember(x => x.VendorEndOfMaintenanceDateFnt, s => s.MapFrom(src => _commonManager.GetHwPassthroughlcmRecords(src.ResourceKeyNavigation.LocalMarket, src.ResourceKeyNavigation.AssetClass, src.ResourceKeyNavigation.HardwareModel).Result.Hwvendorendofmaintenancedate))
            .ForMember(x => x.IdentifiedActionFnt, s => s.MapFrom(src => src.ResourceKeyNavigation.IdentifiedAction))
            .ForMember(x => x.DescriptionOfPlannedActionFnt, s => s.MapFrom(src => src.ResourceKeyNavigation.DescriptionOfPlannedAction))
            .ForMember(x => x.Model, s => s.MapFrom(src => _commonManager.GetHwPassthroughlcmRecords(src.ResourceKeyNavigation.LocalMarket, src.ResourceKeyNavigation.AssetClass, src.ResourceKeyNavigation.HardwareModel).Result.Hardwaremodel))
            .ForMember(x => x.BusinessServiceName, s => s.MapFrom(src => src.BusinessOwner))
            //.ForMember(x => x.OpMaintenanceContractendDate, s => s.MapFrom(src => src.OpsMaintenanceContractEndDate))
            .ForMember(x => x.IncidentClass, s => s.MapFrom(src => src.ResourceKeyNavigation.IncidentClass))
            .ForMember(x => x.OccurrenceProbability, s => s.MapFrom(src => src.ResourceKeyNavigation.OccurrenceProbability))
            .ForMember(x => x.MeverticalResposible, s => s.MapFrom(src => src.ResourceKeyNavigation.VerticalEngineeringTeam))
            .ForMember(x => x.AssetStatus, s => s.MapFrom(src => src.DeploymentOrLifeCycleStatus))
            .ForMember(x => x.TypeOfNetworkElement, s => s.MapFrom(src => src.ResourceKeyNavigation.TypeOfNetworkElement))
            .ForMember(x => x.LocalMarket, s => s.MapFrom(src => src.LocalMarketOwnerShip))
            .ForMember(x => x.Application, s => s.MapFrom(src => src.Application))
            .ForMember(x => x.Cloud, s => s.MapFrom(src => src.CloudHostedAsset))
            //.ForMember(x => x.Data, s => s.MapFrom(src => src.Geolocation))
            .ForMember(x => x.PhysicalServerHostname, s => s.MapFrom(src => src.PhysicalServerHostName))
            .ForMember(x => x.PhysicalServerIpaddress, s => s.MapFrom(src => src.PhysicalServerIpaddress))
            .ForMember(x => x.PhysicalServerSerialNumber, s => s.MapFrom(src => src.PhysicalServerSerialNumber))
            .ForMember(x => x.PhysicalServerHwModel, s => s.MapFrom(src => src.PhysicalServerHwModel))
            .ForMember(x => x.PhysicalServerVendor, s => s.MapFrom(src => src.PhysicalServerVendor))
            .ForMember(x => x.VirtualServerHostedOn, s => s.MapFrom(src => src.VirtualServerHostedon))
            .ForMember(x => x.VirtualServerManufacturer, s => s.MapFrom(src => src.VirtualServerManufacturer))
            .ForMember(x => x.VirtualServerTypeOfDevice, s => s.MapFrom(src => src.VirtualServerTypeofDevice))
            .ForMember(x => x.VirtualMachineType, s => s.MapFrom(src => src.VirtualServerType))
            .ForMember(x => x.VirtualServerIpaddress, s => s.MapFrom(src => src.SystemNameManagementIpAddress))
            .ForMember(x => x.VirtualServerSerialNumber, s => s.MapFrom(src => src.VirtualServerSerialNumber))
            .ForMember(x => x.VirtualServerType, s => s.MapFrom(src => src.VirtualServerType))
            .ForMember(x => x.OsName, s => s.MapFrom(src => src.OperatingSystemName))
            .ForMember(x => x.OsVersion, s => s.MapFrom(src => src.OperatingSystemSwvVersion))
            .ForMember(x => x.OsStartDate, s => s.MapFrom(src => src.OsStartDate))
            .ForMember(x => x.OsInstallationDate, s => s.MapFrom(src => src.OsInstallationDate))
            .ForMember(x => x.OsStatus, s => s.MapFrom(src => src.OsStatus))
            .ForMember(x => x.SoftwareName, s => s.MapFrom(src => src.SoftwareName))
            .ForMember(x => x.Version, s => s.MapFrom(src => src.Version))
            .ForMember(x => x.Release, s => s.MapFrom(src => src.Release))
            .ForMember(x => x.Manufacturer, s => s.MapFrom(src => src.ResourceKeyNavigation.SWVendor))
            .ForMember(x => x.Language, s => s.MapFrom(src => src.Language))

            .ForMember(x => x.HwOpsContractStatus, s => s.MapFrom(src => src.HwOpsContractStatus))
            .ForMember(x => x.HwOpsContractEndDate, s => s.MapFrom(src => _commonManager.GetHwPassthroughlcmRecords(src.ResourceKeyNavigation.LocalMarket, src.ResourceKeyNavigation.AssetClass, src.ResourceKeyNavigation.HardwareModel).Result.Hwopsmaintenancecontractenddate))
            .ForMember(x => x.HwOperationsContactPoint, s => s.MapFrom(src => _commonManager.GetHwPassthroughlcmRecords(src.ResourceKeyNavigation.LocalMarket, src.ResourceKeyNavigation.AssetClass, src.ResourceKeyNavigation.HardwareModel).Result.Hwoperationsmaintenancecontract))
            .ForMember(x => x.SwOperationsContactPoint, s => s.MapFrom(src => src.ResourceKeyNavigation.SWOperationsContactPoint))
            .ForMember(x => x.SwOpsContractEndDate, s => s.MapFrom(src => src.ResourceKeyNavigation.SWOpsMaintenanceConractEndDate))
            .ForMember(x => x.SwOpsContractStatus, s => s.MapFrom(src => src.SwOpsContractStatus))

            .ForMember(x => x.ModificationUser, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email));


        }

    }
}
