using AutoMapper;
using CAM.Entities.Models;
using System.Linq;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System.Globalization;
using System;
using CAM.BusinessManager.ExtensionMethod.MajorSoftwareBuild;
using CAM.Contracts.RepositoryContracts.Base;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using CAM.Repository.Helpers;
using CAM.Entities.Models.Lookup;
using CAM.BusinessManager.CommonUtilities;
using CAM.DataTransferObjects;

namespace CAM.BusinessManager.MapConfiguration
{
    public class MajorSoftwareBuildMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager { get; set; }
        public MajorSoftwareBuildMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<MajorSoftwareBuildDtoGrid, MajorSoftwareBuildDtoExportSheet>()
                .ForMember(x => x.GeneraAvailableDate, s => s.MapFrom(src => src.GeneraAvailableDate.HasValue ?
                src.GeneraAvailableDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                .ForMember(x=>x.LastTimeBuyNew, s=>s.MapFrom(src=>src.LastTimeBuyNew.HasValue ? 
                src.LastTimeBuyNew.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                .ForMember(x=>x.EndOfsupport, s=>s.MapFrom(src=>src.EndOfsupport.HasValue ? 
                src.EndOfsupport.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "" ))
                .ForMember(x=>x.LastModified, s=>s.MapFrom(src=>src.LastModified.HasValue ? 
                src.LastModified.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                

                .ForMember(x=>x.LastTimeBuyUpgrades, s=>s.MapFrom(src=>src.LastTimeBuyUpgrades.HasValue ?
                src.LastTimeBuyUpgrades.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                .ForMember(x=>x.LastTimeBuyExpansions, s=>s.MapFrom(src=>src.LastTimeBuyExpansions.HasValue ? 
                src.LastTimeBuyExpansions.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture): ""))
                .ForMember(x => x.EndOfMaintenanceValue, s => s.MapFrom(src => src.EOMStatus == Enum.EOMEnum.Default ? 
                (src.EndOfMaintenance.HasValue ? src.EndOfMaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "") : 
                (src.EOMStatus == Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified")));






            CreateMap<MajorSoftwareBuildDtoUpdate, MajorSoftwareBuild>()
                .ForMember(x => x.ProductName, opt => opt.Ignore())
               .ForMember(dest => dest.OriginalEquipmentManufacturerId,
                    opt => opt.MapFrom(src => src.OriginalEquipmentManufacturerId))
               .ForMember(x => x.MajorSwBuidlsDesignContacts, opt => opt.MapFrom(
                    src => src.DesignContactIds.Select(x => new MajorSwBuidlsDesignContact
                    {
                        DesignContactId = (short)x,
                        MajorsoftwarebuildsId = src.MajorSoftwareBuildId
                    }).ToList()))
             .ForMember(x =>
                 x.NetworkFunctions,
                    opt => opt.MapFrom(
                        src => src.NetworkFunctionsIds.Select(id => new MajorSoftwareBuildFamilyNetworkFunction
                        {
                            NetworkFunctionId = id,
                            MajorSoftwareBuildId = src.MajorSoftwareBuildId
                        }).ToList()))
                .ReverseMap()
                 .ForMember(x => x.NetworkFunctionsIds, opt => opt.MapFrom(src => src.NetworkFunctions.Select(x => x.NetworkFunctionId).ToList()))
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.LastModified, opt => opt.MapFrom(s => s.ModificationDate))
                //.ForMember(x => x.DesignContact, opt => opt.MapFrom(src => src.DesignContactNavigation.Email))
                .ForMember(x => x.NetworkFunctionsIds, opt => opt.MapFrom(src => src.NetworkFunctions.Select(x => x.NetworkFunctionId).ToList()))
                .ForMember(x => x.IsPlatform, opt=> opt.MapFrom(src => src.ProductName.IsPlatformSoftware))

                ;





            _ = CreateMap<MajorSoftwareBuild, MajorSoftwareBuildDtoGrid>()
                .ForMember(x => x.Orphan, s => s.MapFrom(src => !src.SystemTypes.Any()))
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.Description, opt => opt.MapFrom(s => s.Description))
                .ForMember(
                    dest => dest.NetworkFunction,
                    opt => opt.MapFrom(x => string.Join(" | ", x.NetworkFunctions.Select(fx => fx.NetworkFunction.Description).Distinct()))
                )
                .ForMember(o => o.OriginalEquipmentManufacturer,
                    o => o.MapFrom(s => s.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription))
                .ForMember(x => x.LastModified, opt => opt.MapFrom(s => s.ModificationDate))
                .ForMember(x => x.VulnerabilityStatus, opt => opt.MapFrom(s => s.VulnerabilityStatus))
                .ForMember(x => x.OperatingSystem, opt => opt.MapFrom(s => s.OperatingSystem.OperatingSystemName))
                .ForMember(x => x.ProductName, opt => opt.MapFrom(s => s.ProductName != null ? s.ProductName.Description : ""))
                .ForMember(x => x.CriticalAssetType, opt => opt.MapFrom(s => s.CriticalAssetType.Description))
                .ForMember(x => x.MajorSoftwareBuildId, opt => opt.MapFrom(s => s.MajorSoftwareBuildsId))
                .ForMember(x => x.OriginalEquipmentManufacturerId, opt => opt.MapFrom(s => s.OriginalEquipmentManufacturerId))
                .ForMember(x => x.ProductNameId, opt => opt.MapFrom(s => s.ProductNameId))
                 .ForMember(x => x.GeneraAvailableDateValue, s => s.MapFrom(src => src.GeneraAvailableDate.HasValue ?
                 src.GeneraAvailableDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                 // .ForMember(x => x.EndOfsupportValue, s => s.MapFrom(src => src.EndOfsupport.HasValue ?
                 //src.EndOfsupport.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                 .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
                src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))

                 .ForMember(x => x.EndOfsupportValue, r => r.MapFrom(s => s.EndOfsupport.HasValue ? s.EndOfsupport.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "Not Announced"))

                .ForMember(x => x.EndOfMaintenanceValue, o => o.MapFrom(s => s.EOMStatus == Enum.EOMEnum.Default ? (s.EndOfMaintenance.HasValue ? s.EndOfMaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "") : (s.EOMStatus == Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified")))
                //.ForMember(dest => dest.DesignContact, opt => opt.MapFrom(x => _commonManager.GetSubdomainSpoc(x.MajorSoftwareBuildsId,0)))
                .ForMember(dest => dest.DesignContact, opt => opt.MapFrom(x => _commonManager.GetDesignContacts(x.MajorSwBuidlsDesignContacts.Where(x => x.Deleted == false))))
            #region //Ticket 743 NFCI Bundle
                 // Set  TCP and TCI column Null for VMWare Records  - July 24th Client Suggestion
                 .ForMember(x => x.TciBundleVersion, s => s.MapFrom(src =>
                 src.Isvmware == true ? string.Empty :
              Convert.ToString(
                MajorHardwareBuildMethod.BundleVersion((long)src.MajorSoftwareBuildsId, (int)Enum.SoftwareCompatibilityEnum.TCIBundle,
                  false, _repositoryWrapper)
                 )))
              .ForMember(x => x.TcpBundleVersion, s => s.MapFrom(src =>
              src.Isvmware == true ? string.Empty : Convert.ToString(
                MajorHardwareBuildMethod.BundleVersion((long)src.MajorSoftwareBuildsId, (int)Enum.SoftwareCompatibilityEnum.TCPBundle,
                false, _repositoryWrapper)
                 )))
              .ForMember(x => x.IsPlatform, s => s.MapFrom(src => src.Isvmware));
#endregion
 
            CreateMap<MajorSoftwareBuildDtoCreate, MajorSoftwareBuild>()
                .ForMember(x => x.ProductNameId, x => x.MapFrom(s => s.ProductNameId))
                .ForMember(x => x.MajorSoftwareBuildsId, s => s.MapFrom(s => s.MajorSoftwareBuildId))
                  .ForMember(x => x.Isvmware, s => s.MapFrom(src => src.IsPlatform))
                .ForMember(x => x.ProductName, opt => opt.Ignore())
                 .ForMember(dest => dest.NetworkFunctions, opt => opt.MapFrom(
                   src => src.NetworkFunctionsIds.Select(x => new MajorSoftwareBuildFamilyNetworkFunction()
                   {
                       NetworkFunctionId = x,
                   }).ToList()))
                .ReverseMap();

        }
    }
}
