using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration.ComponentSoftware
{
    public class ComponentSwBuildMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager { get; set; }
        public ComponentSwBuildMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<ComponentSoftwareBuildDtoUpdate, ComponentSoftwareBuild>()
             .ForMember(dest => dest.Componentmanufacturerid,
                  opt => opt.MapFrom(src => src.ComponentManufacturerId))
             .ForMember(x => x.ComponentSoftwareBuildsDesignContact, opt => opt.MapFrom(
                  src => src.DesignContactIds.Select(x => new ComponentSoftwareBuildsDesignContact
                  {
                      DesignContactId = (short)x,
                      ComponentSoftwareBuildId = src.ComponentSoftwareBuildId
                  }).ToList()))       
              .ReverseMap()             
              .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationUserEntity.Email))
              .ForMember(x => x.LastModified, opt => opt.MapFrom(s => s.ModificationDate))
              ;


            _ = CreateMap<ComponentSoftwareBuild, ComponentSoftwareBuildDtoGrid>()               
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.Description, opt => opt.MapFrom(s => s.Description))               
                .ForMember(o => o.ComponentManufacturers,
                    o => o.MapFrom(s => $"{s.ComponentManufacturers.Componentmanufacturer}-{s.ComponentManufacturers.Componentname}"))
                .ForMember(x => x.LastModified, opt => opt.MapFrom(s => s.ModificationDate))
                .ForMember(x => x.VulnerabilityStatus, opt => opt.MapFrom(s => s.VulnerabilityStatus))
                .ForMember(x => x.OperatingSystem, opt => opt.MapFrom(s => s.OperatingSystem.OperatingSystemName))
                .ForMember(x => x.CriticalAssetType, opt => opt.MapFrom(s => s.CriticalAssetType.Description))
                .ForMember(x => x.ComponentSoftwareBuildId, opt => opt.MapFrom(s => s.ComponentSoftwareBuildId))
                 .ForMember(x => x.GeneraAvailableDateValue, s => s.MapFrom(src => src.GeneraAvailableDate.HasValue ?
                 src.GeneraAvailableDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : ""))                
                 .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
                src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))

                 .ForMember(x => x.EndOfsupportValue, r => r.MapFrom(s => s.EndOfsupport.HasValue?s.EndOfsupport.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) :"Not Announced")) 

                .ForMember(x => x.EndOfMaintenanceValue, o => o.MapFrom(s => s.EOMStatus == Enum.EOMEnum.Default ? (s.EndOfMaintenance.HasValue ? s.EndOfMaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "") : (s.EOMStatus == Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified")))

                .ForMember(dest => dest.DesignContact, opt => opt.MapFrom(x => _commonManager
                .GetDesignContacts(x.ComponentSoftwareBuildsDesignContact.Where(x=>x.Deleted == false))))

                ;


            CreateMap<ComponentSoftwareBuildDtoCreate, ComponentSoftwareBuild>()
                .ForMember(x => x.Componentmanufacturerid, x => x.MapFrom(s => s.ComponentManufacturerId))
                .ForMember(x => x.ComponentSoftwareBuildId, s => s.MapFrom(s => s.ComponentSoftwareBuildId))

                .ReverseMap();


        }
    }
}