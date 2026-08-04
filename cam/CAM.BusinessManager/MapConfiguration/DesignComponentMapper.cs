using AutoMapper;
using CAM.Entities;
using CAM.Entities.Models;
using System.Linq;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.DesignComponent;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CAM.Repository.Helpers;
using System.Security.Claims;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using OracleModels.DBModels;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.BusinessManager.MapConfiguration
{
    public class DesignComponentMapper : Profile
    {

        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager;
        public DesignComponentMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper,CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));



            CreateMap<DesignComponentDtoUpdate, DesignComponent>()
                .ReverseMap()
                //.ForMember(x => x.SubNetworkBoundaryIds, opt => opt.MapFrom(x => x.DesignComponentFamily.SubNetworkBoundaryId))
                .ForMember(x => x.GdprRelevant, opt => opt.MapFrom(x => x.DesignComponentFamily.SubNetworkBoundary.GdprRelevant))
                .ForMember(x => x.LastModified, opt => opt.MapFrom(x => x.ModificationDate))
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(x => x.ModificationUserEntity.Email));

            CreateMap<DesignComponent, DesignComponentDtoGrid>()

                .ForMember(x => x.Orphan, s => s.MapFrom(src => !src.Lcmengineerings.Any() && !src.Networkelementsasplanned.Any() && !src.PlannedActivities.Any()))
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(dest => dest.SystemType, opt => opt.MapFrom(src => Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src.SystemType).toSystemTypeName(_repositoryWrapper)))
                .ForMember(dest => dest.EquipmentManufacturer, opt => opt.MapFrom(src => src.SystemType.MajorSoftwareBuilds.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.SystemType.MajorSoftwareBuilds.ProductName != null ? src.SystemType.MajorSoftwareBuilds.ProductName.Description : ""))
                .ForMember(dest => dest.SubNetworkBoundary, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.SubNetworkBoundary.Alias) ? src.SubNetworkBoundary.Description : src.SubNetworkBoundary.Alias))
                .ForMember(dest => dest.SoftwareVersion, opt => opt.MapFrom(src => src.SystemType.MajorSoftwareBuilds.SoftwareVersion))
                .ForMember(dest => dest.HardwarePlatform, opt => opt.MapFrom(src => src.SystemType.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain) != null ? src.SystemType.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain).MajorHardware.Platform.PlatformDescription : null))
                .ForMember(dest => dest.HardwareSolution, opt => opt.MapFrom(src => src.SystemType.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain) != null ? src.SystemType.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain).MajorHardware.HardwareSolution : null))
                .ForMember(dest => dest.HardwareType, opt => opt.MapFrom(src => src.SystemType.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain) != null ? src.SystemType.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain).MajorHardware.HardwareType : null))
                .ForMember(x => x.LastModified, opt => opt.MapFrom(x => x.ModificationDate))
                .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
                 src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(x => x.SystemTypeId, opt => opt.MapFrom(x => x.SystemTypeId))
                .ForMember(x => x.VodafoneName, opt => opt.MapFrom(x => x.SystemType.VodafoneName.Description))
                .ForMember(x => x.VodafoneNameId, opt => opt.MapFrom(x => x.SystemType.VodafoneName.Id))
                .ForMember(dest => dest.DesignComponent, opt => opt.MapFrom(src => Entities.Mappers.Entity.DesignComponentMapper.SetDesignComponentMapper(src).ToDesignComponentName(_repositoryWrapper)));


            CreateMap<Designcomponents, DesignComponentDtoGrid>()

               .ForMember(x => x.Orphan, s => s.MapFrom(src => !src.Lcmengineering.Any() && !src.Networkelementsasplanned.Any() && !src.Plannedactivities.Any()))
               .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
               .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationuserNavigation.Email))
               .ForMember(dest => dest.SystemType, opt => opt.MapFrom(src => src.Systemtype.toSystemTypeName(_repositoryWrapper)))
               .ForMember(dest => dest.EquipmentManufacturer, opt => opt.MapFrom(src => src.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer))
               .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Systemtype.Majorsoftwarebuilds.Productname != null ? src.Systemtype.Majorsoftwarebuilds.Productname.Description : ""))
               .ForMember(dest => dest.SubNetworkBoundary, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Subnetworkboundary.Alias) ? src.Subnetworkboundary.Description : src.Subnetworkboundary.Alias))
               .ForMember(dest => dest.SoftwareVersion, opt => opt.MapFrom(src => src.Systemtype.Majorsoftwarebuilds.Softwareversion))
               .ForMember(dest => dest.HardwarePlatform, opt => opt.MapFrom(src => src.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain) != null ? src.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware.Platform.Platform : null))
               .ForMember(dest => dest.HardwareSolution, opt => opt.MapFrom(src => src.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain) != null ? src.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware.Hardwaresolution : null))
               .ForMember(dest => dest.HardwareType, opt => opt.MapFrom(src => src.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain) != null ? src.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware.Hardwaretype : null))
               .ForMember(x => x.LastModified, opt => opt.MapFrom(x => x.Modificationdate))
               .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
                src.Modificationdate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
               .ForMember(x => x.SystemTypeId, opt => opt.MapFrom(x => x.Systemtypeid))
               .ForMember(x => x.VodafoneName, opt => opt.MapFrom(x => x.Systemtype.VodafonenameNavigation.Description))
               .ForMember(x => x.VodafoneNameId, opt => opt.MapFrom(x => x.Systemtype.VodafonenameNavigation.Id))
                //.ForMember(
                //    dest => dest.SystemTypeIdBasedVerticalId,
                //    opt => opt.MapFrom(x => x.Systemtype.Verticalresponsible.Verticalresponsibleid)
                //)
                .ForMember(
                    dest => dest.SystemTypeIdBasedVerticalValue,
                    opt => opt.MapFrom(x => _commonManager.GetVerticalResponseForLibary((long)x.Systemtype.Majorsoftwarebuildsid,x.Systemtype.Systemtypesmajorhardwarebuilds.Select(x=>x.Majorhardwareid).FirstOrDefault()))
                )
               .ForMember(dest => dest.DesignComponent, opt => opt.MapFrom(src => src.ToDesignComponentName(_repositoryWrapper)))
               .ForMember(dest => dest.DesignContact, opt => opt.MapFrom(src => _commonManager.GetSubdomainSpocEmailForLibary(src.Systemtype.Majorsoftwarebuilds.Majorsoftwarebuildsid,
               src.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardwareid).FirstOrDefault())))

               ;

            CreateMap<DesignComponentDtoCreate, DesignComponent>();
        }


    }
}
