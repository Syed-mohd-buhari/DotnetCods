using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.TsrPassThrough;
using CAM.Entities.Models.PassThroughData;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.MapConfiguration.TSRNonTems
{
    public class TsrNonTemMapper:Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager;
        
        public TsrNonTemMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<AssetPassThrough, TsrPassThroughDtoGrid>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
            .ForMember(x => x.TsrPassThroughId, s => s.MapFrom(src => src.PassThroughId))
//.ForMember(x => x.AssetId, s => s.MapFrom(src => src.Ass))
.ForMember(x => x.VodafoneUniqueIdentifier, s => s.MapFrom(src => src.VodafoneUniqueIdentifier))
.ForMember(x => x.AssetName, s => s.MapFrom(src => src.AssetName))
.ForMember(x => x.AssetDescriptionOrPurpose, s => s.MapFrom(src => src.AssetDescriptionOrPurpose))
.ForMember(x => x.AssetTypeTsr, s => s.MapFrom(src => src.AssetType))
.ForMember(x => x.BusinessOwner, s => s.MapFrom(src => src.BusinessOwner))
.ForMember(x => x.SupportOwner, s => s.MapFrom(src => src.SupportOwner))
.ForMember(x => x.SupportTeam, s => s.MapFrom(src => src.SupportTeam))
.ForMember(x => x.SupportTeamsPlaceInTheOrganisation, s => s.MapFrom(src => src.SupportTeamsPlaceInTheOrganisation))
.ForMember(x => x.AssetFunction, s => s.MapFrom(src => src.AssetFunction))
.ForMember(x => x.DeploymentOrLifeCycleStatus, s => s.MapFrom(src => src.DeploymentOrLifeCycleStatus))
.ForMember(x => x.RelatedRiskIdsFromRiskRegisters, s => s.MapFrom(src => src.RelatedRiskIdsFromRiskRegisters))
.ForMember(x => x.RegulatoryScope, s => s.MapFrom(src => src.RegulatoryScope))
.ForMember(x => x.CountryWhereAssetIsLocated, s => s.MapFrom(src => src.CountryWhereAssetIsLocated))
.ForMember(x => x.Geolocation, s => s.MapFrom(src => src.Geolocation))
.ForMember(x => x.Infrastructure, s => s.MapFrom(src => src.Infrastructure))
.ForMember(x => x.UpStreamDependencies, s => s.MapFrom(src => src.UpStreamDependencies))
.ForMember(x => x.DownStreamDependencies, s => s.MapFrom(src => src.DownStreamDependencies))
.ForMember(x => x.ChangesToTheassetSinceDeployment, s => s.MapFrom(src => src.ChangesToTheassetSinceDeployment))
.ForMember(x => x.CloudHostedAsset, s => s.MapFrom(src => src.CloudHostedAsset))
.ForMember(x => x.CloudType, s => s.MapFrom(src => src.CloudType))
.ForMember(x => x.CloudVendor, s => s.MapFrom(src => src.CloudVendor))
.ForMember(x => x.EquipmentName, s => s.MapFrom(src => src.EquipmentName))
.ForMember(x => x.HostLocationWithInPhysicalLocation, s => s.MapFrom(src => src.HostLocationWithInPhysicalLocation))
.ForMember(x => x.SoftwareVendorName, s => s.MapFrom(src => src.ResourceKeyNavigation.SWVendor))
.ForMember(x => x.Model, s => s.MapFrom(src => src.ResourceKeyNavigation.HardwareModel))
.ForMember(x => x.FirmwareVersion, s => s.MapFrom(src => src.FirmwareVersion))
.ForMember(x => x.FirmwareVersionPatchLevel, s => s.MapFrom(src => src.FirmwareVersionPatchLevel))
.ForMember(x => x.MaintenanceSupportSupplier, s => s.MapFrom(src => src.MaintenanceSupportSupplier))
.ForMember(x => x.MaintenanceHardwareEndOfSupportDate, s => s.MapFrom(src => _commonManager.GetHwPassthroughlcmRecords(src.ResourceKeyNavigation.LocalMarket, src.ResourceKeyNavigation.AssetClass, src.ResourceKeyNavigation.HardwareModel).Result.Hwvendorendofmaintenancedate))
.ForMember(x => x.DependantHardware, s => s.MapFrom(src => src.DependantHardware))
.ForMember(x => x.InstanceType, s => s.MapFrom(src => src.InstanceType))
.ForMember(x => x.OperatingSystemName, s => s.MapFrom(src => src.OperatingSystemName))
.ForMember(x => x.OperatingSystemSwvVersion, s => s.MapFrom(src => src.OperatingSystemSwvVersion))
.ForMember(x => x.OperatingSystemSwVersionPatchLevel, s => s.MapFrom(src => src.OperatingSystemSwVersionPatchLevel))
.ForMember(x => x.SystemNameDns, s => s.MapFrom(src => src.SystemNameDns))
.ForMember(x => x.SystemNameManagementIpAddress, s => s.MapFrom(src => src.SystemNameManagementIpAddress))
.ForMember(x => x.SystemNameNetBios, s => s.MapFrom(src => src.SystemNameNetBios))
.ForMember(x => x.SystemNameHostName, s => s.MapFrom(src => src.SystemNameHostName))
.ForMember(x => x.DateAssetMovedToLiveStatus, s => s.MapFrom(src => src.DateAssetMovedToLiveStatus))
.ForMember(x => x.DateAssetDecommissioned, s => s.MapFrom(src => src.DateAssetDecommissioned))
.ForMember(x => x.HardwareVendorName, s => s.MapFrom(src => src.ResourceKeyNavigation.HWVendor))
.ForMember(x => x.VendorSoftwareEndOfSupportDate, s => s.MapFrom(src => src.ResourceKeyNavigation.VendorEndOfVulnerabilitySecuritySupportDate))
.ForMember(x => x.MaintenanceSoftwareEndOfSupportDate, s => s.MapFrom(src => src.ResourceKeyNavigation.SWVendorEndOfMaintenanceDate))
.ForMember(x => x.DependantSystemSoftware, s => s.MapFrom(src => src.DependantSystemSoftware))
.ForMember(x => x.ResilienceModel, s => s.MapFrom(src => src.ResilienceModel))
.ForMember(x => x.GeographicSiteResilience, s => s.MapFrom(src => src.GeographicSiteResilience))
.ForMember(x => x.LocalSiteResilience, s => s.MapFrom(src => src.LocalSiteResilience))
.ForMember(x => x.NameOfProductsDependantOnAsset, s => s.MapFrom(src => src.NameOfProductsDependantOnAsset))
//.ForMember(x => x.TechnicalServiceNames, s => s.MapFrom(src => src.TechnicalServiceNames))
.ForMember(x => x.Customer, s => s.MapFrom(src => src.Customer))
.ForMember(x => x.PrivilegedAccessLogging, s => s.MapFrom(src => src.PrivilegedAccessLogging))
.ForMember(x => x.BoardOrModuleNameComponentName, s => s.MapFrom(src => src.BoardOrModuleNameComponentName))
.ForMember(x => x.BoardOrModuleTypeComponentSubtype, s => s.MapFrom(src => src.BoardOrModuleTypeComponentSubtype))
.ForMember(x => x.BoardOrModuleTypeComponentVersionNumber, s => s.MapFrom(src => src.BoardOrModuleTypeComponentVersionNumber))
.ForMember(x => x.ExposedEdge, s => s.MapFrom(src => src.ExposedEdge))
.ForMember(x => x.ExternallyFacingSystem, s => s.MapFrom(src => src.ExternallyFacingSystem))
.ForMember(x => x.ManagementPlane, s => s.MapFrom(src => src.ManagementPlane))
.ForMember(x => x.NetworkOverSightFunction, s => s.MapFrom(src => src.NetworkOverSightFunction))
.ForMember(x => x.Pecn, s => s.MapFrom(src => src.Pecn))
.ForMember(x => x.Pecs, s => s.MapFrom(src => src.Pecs))
.ForMember(x => x.SecurityCriticalFunction, s => s.MapFrom(src => src.SecurityCriticalFunction))
.ForMember(x => x.ProductImportanceTsr, s => s.MapFrom(src => src.ResourceKeyNavigation.ProductImportance))
.ForMember(x => x.Critical, s => s.MapFrom(src => src.Critical))
//.ForMember(x => x.CriticalityType, s => s.MapFrom(src => src.CriticalityType))
.ForMember(x => x.SerialNumberTsr, s => s.MapFrom(src => src.SerialNumber))
.ForMember(x => x.PartNumber, s => s.MapFrom(src => src.PartNumber))
.ForMember(x => x.DescriptionOfPlannedaction, s => s.MapFrom(src => src.ResourceKeyNavigation.DescriptionOfPlannedAction))
.ForMember(x => x.IdentifiedActionTsr, s => s.MapFrom(src => src.ResourceKeyNavigation.IdentifiedAction))
.ForMember(x => x.LastUpgradeDateTsr, s => s.MapFrom(src => src.ResourceKeyNavigation.LastUpgradeDate))
.ForMember(x => x.ProdOrLab, s => s.MapFrom(src => src.ProdOrLab))
.ForMember(x => x.LocalMarketOwnerShip, s => s.MapFrom(src => src.LocalMarketOwnerShip))
.ForMember(x => x.BudgetEstimatedTsr, s => s.MapFrom(src => src.ResourceKeyNavigation.BudgetEstimated))
.ForMember(x => x.BundleBudgetTsr, s => s.MapFrom(src => src.ResourceKeyNavigation.BundleBudget))
.ForMember(x => x.AssuranceCall, s => s.MapFrom(src => src.AssuranceCall))
.ForMember(x => x.CommentOnProjectStatusTsr, s => s.MapFrom(src => src.ResourceKeyNavigation.CommentonProjectStatus))
.ForMember(x => x.ProjectEndDateTsr, s => s.MapFrom(src => src.ResourceKeyNavigation.ProjectEndDate))
.ForMember(x => x.ProjectStatusTsr, s => s.MapFrom(src => src.ResourceKeyNavigation.ProjectStatus))
.ForMember(x => x.ServiceLevel, s => s.MapFrom(src => src.ServiceLevel))
.ForMember(x => x.LastPenTestDateTsr, s => s.MapFrom(src => src.LastPenTestDate))
.ForMember(x => x.LastPenTestRefNo, s => s.MapFrom(src => src.LastPenTestRefNo))
.ForMember(x => x.PiData, s => s.MapFrom(src => src.PiData))
.ForMember(x => x.EncryptedPiData, s => s.MapFrom(src => src.EncryptedPiData))
.ForMember(x => x.MeProductName, s => s.MapFrom(src => src.ResourceKeyNavigation.AssetClass))
.ForMember(x => x.MeSubDomainResponsible, s => s.MapFrom(src => src.ResourceKeyNavigation.VerticalSubDomain))
.ForMember(x => x.MeSoftwareVersion, s => s.MapFrom(src => src.ResourceKeyNavigation.SoftwareVersion))
.ForMember(x=>x.VendorHardwareEndOfSupportDate,s=>s.MapFrom(src=> _commonManager.GetHwPassthroughlcmRecords(src.ResourceKeyNavigation.LocalMarket, src.ResourceKeyNavigation.AssetClass, src.ResourceKeyNavigation.HardwareModel).Result.Vendorendofvulnerabilitysecuritysupportdate))



                       ;

        }
    }
}
