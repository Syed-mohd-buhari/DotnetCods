using AutoMapper;
using CAM.Entities.Models;
using System.Linq;
using CAM.DataTransferObjects.Entita.MajorHardwareBuild;
using CAM.Entities.Models.Cross;
using System.Globalization;
using CAM.Entities.Models.Lookup;
using CAM.BusinessManager.ExtensionMethod.MajorHardwareBuild;
using CAM.Contracts.RepositoryContracts.Base;
using Microsoft.EntityFrameworkCore;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.BusinessManager.MapConfiguration
{
    public class MajorHardwareBuildMapper : Profile
    {
        public CommonManager _commonManager { get; set; }
        public MajorHardwareBuildMapper(CommonManager commonManager)
        {
            _commonManager = commonManager;
            CreateMap<MajorHardwareBuildDtoUpdate, MajorHardwareBuild>()
                .ForMember(x => x.MajorHwBuidlsDesignContacts, opt => opt.MapFrom(
                    src => src.DesignContactIds.Select(x => new MajorHwBuidlsDesignContact
                    {
                        DesignContactId = (short)x,
                        MajorHardwareBuildsId = src.MajorHardwareId
                    }).ToList())).ReverseMap()
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.LastModified, opt => opt.MapFrom(s => s.ModificationDate))
 
                ;

            CreateMap<MajorHardwareBuildDtoGrid, MajorHardwareBuildDtoExportSheet>()
                .ForMember(x => x.EndOfMaintenanceValue, s => s.MapFrom(src => src.EOMStatus == Enum.EOMEnum.Default ?
                (src.EndOfMaintenance.HasValue ? src.EndOfMaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "")
                : (src.EOMStatus == Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified")))
                .ForMember(x => x.GeneraAvailableDate, s => s.MapFrom(src => src.GeneraAvailableDate.HasValue ?
                src.GeneraAvailableDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                .ForMember(x => x.EndOfsupport, s => s.MapFrom(src => src.EndOfsupport.HasValue ?
                src.EndOfsupport.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "Not Announced"))
                .ForMember(x => x.LastModifiedDate, s => s.MapFrom(src => src.LastModified.HasValue ?
                src.LastModified.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                .ForMember(x => x.LastTimeBuyExpansions, s => s.MapFrom(src => src.LastTimeBuyExpansions.HasValue ?
                src.LastTimeBuyExpansions.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                .ForMember(x => x.LastTimeBuyUpgrades, s => s.MapFrom(src => src.LastTimeBuyUpgrades.HasValue ?
                src.LastTimeBuyUpgrades.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                .ForMember(x => x.LastTimeBuyNew, s => s.MapFrom(src => src.LastTimeBuyNew.HasValue ?
                src.LastTimeBuyNew.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                //.ForMember(dest => dest.DesignContact, opt => opt.MapFrom(x => x.MajorSwBuidlsDesignContacts
                //    .Where(x => x.Deleted == false).Select(fx => fx.DesignContact.Email).Distinct()
                //    .Aggregate(
                //        "", (current, next) => current + ", " + next)))
                ;
            
            CreateMap<MajorHardwareBuild, MajorHardwareBuildDtoGrid>()
                .ForMember(x => x.Orphan, s => s.MapFrom(src => !src.SystemTypesMajorHardwareBuilds.Any()))
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                //.ForMember(x => x.DesignContact, s => s.MapFrom(src => src.DesignContactNavigation.Email))
                .ForMember(o => o.OriginalEquipmentManufacturer,
                    o => o.MapFrom(s => s.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription))
                //.ForMember(o => o.HardwareSolution,
                //   o => o.MapFrom(s => s.HardwareSolution ?? (s.HardwareSolutionResource != null ? s.HardwareSolutionResource.HardwareSolutionResourceDescription : "")))

                .ForMember(o => o.BuildConstruction,
                    o => o.MapFrom(s => /*s.BuildConstruction == null && string.IsNullOrEmpty(s.BuildConstruction.BuildConstructionDescription) ? "N/A" : */s.BuildConstruction.BuildConstructionDescription))
                .ForMember(o => o.Platform, o => o.MapFrom(s => s.Platform.PlatformDescription))

                .ForMember(x => x.EndOfMaintenanceValue, o => o.MapFrom(s => s.EOMStatus == Enum.EOMEnum.Default ?
                (s.EndOfMaintenance.HasValue ? s.EndOfMaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "")
                : (s.EOMStatus == Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified")))
                 .ForMember(x => x.GeneraAvailableDateValue, s => s.MapFrom(src => src.GeneraAvailableDate.HasValue ?
                 src.GeneraAvailableDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
                 .ForMember(x => x.EndOfsupportValue, s => s.MapFrom(src => src.EndOfsupport.HasValue ?
                src.EndOfsupport.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "Not Announced"))
                 .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src => 
                src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(o => o.VulnerabilityStatus, o => o.MapFrom(s => s.VulnerabilityStatus))
                .ForMember(x => x.MajorHardwareBuildId, opt => opt.MapFrom(s => s.MajorHardwareId))
                .ForMember(x => x.LastModified, opt => opt.MapFrom(s => s.ModificationDate))
             .ForMember(x => x.LastTimeBuyNewValue, s => s.MapFrom(src => src.LastTimeBuyNew.HasValue ?
                src.LastTimeBuyNew.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
             .ForMember(x => x.LastTimeBuyUpgradesValue, s => s.MapFrom(src => src.LastTimeBuyUpgrades.HasValue ?
               src.LastTimeBuyUpgrades.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))
             .ForMember(x => x.LastTimeBuyExpansionsValue, s => s.MapFrom(src => src.LastTimeBuyExpansions.HasValue ?
               src.LastTimeBuyExpansions.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))

             .ForMember(dest => dest.DesignContact, opt => opt.MapFrom(x => _commonManager.GetMajorHardwareDesignContacts(x.MajorHwBuidlsDesignContacts)));
                //.ForMember(x => x.last, o => o.MapFrom(s => s.EOMStatus == Enum.EOMEnum.Default ?
                //(s.EndOfMaintenance.HasValue ? s.EndOfMaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "")
                //: (s.EOMStatus == Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified")))
                //;
            CreateMap<MajorHardwareBuildDtoCreate, MajorHardwareBuild>()
                .ReverseMap()
                .ForMember(dest => dest.DesignContactIds, opt => opt.MapFrom(
                    src => src.MajorHwBuidlsDesignContacts.Where(x => !x.Deleted).Select(x => x.DesignContactId)
                        .ToList()));
            //CreateMap<MajorHardwareBuild, MajorHardwareBuildDto>().ReverseMap();
        }

    }
}