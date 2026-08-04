using AutoMapper;
using System.Linq;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.DataTransferObjects.Entita.NetworkElementAsIs;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsIs;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.BusinessManager.ExtensionMethod.SystemTypesMajorHardwareBuilds;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using CAM.Repository.Helpers;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class NetworkElementAsIsMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;

        public NetworkElementAsIsMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));


            CreateMap<NetworkElementAsIsDtoCreate, NetworkElementAsIs>();
            CreateMap<NetworkElementAsIsDtoUpdate, NetworkElementAsIs>();
            CreateMap<NetworkElementAsIs, NetworkElementAsIsDtoUpdate>()
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
                .ForMember(x => x.SoftwareProductionDateValue, s => s.MapFrom(src => src.SoftwareProductionDate != null ? src.SoftwareProductionDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
                .ForMember(x => x.SoftwareInstallDateValue, s => s.MapFrom(src => src.SoftwareInstallDate != null ? src.SoftwareInstallDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
                .ForMember(x => x.DataAcquisitionDateValue, s => s.MapFrom(src => src.DataAcquisitionDate != null ? src.DataAcquisitionDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty));
            CreateMap<NetworkElementAsIs, NetworkElementAsIsDtoGrid>()
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
                 .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
                src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(x => x.Location, s => s.MapFrom(src => src.Location.LocationDescription))
                .ForMember(x => x.OpCo, s => s.MapFrom(src => src.OpCo.OpCoDescription))
                .ForMember(x=>x.Platform, s=>s.MapFrom(src=>src.PlatformType))
                .ForMember(x => x.SystemType, s => s.MapFrom(src => CAM.Entities.Mappers.Entity.SystemTypeMapper.SetSystemTypeMapper(src.SystemType).toSystemTypeName(_repositoryWrapper)))
                //.ForMember(x => x.Platform, s => s.MapFrom(src => src.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().MajorHardware.Platform.PlatformDescription))
                //.ForMember(x => x.HardwareType, s => s.MapFrom(src => src.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().MajorHardware.HardwareType))
                .ForMember(x => x.HardwareSolution, s => s.MapFrom(src => src.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().MajorHardware.HardwareSolution))
                .ForMember(x => x.OtherHardwareInfo, s => s.MapFrom(src => src.SystemType.SystemTypesMajorHardwareBuilds.Where(x => x.IsMain && x.Deleted == false).SingleOrDefault().MajorHardware.OtherHardwareInfo))
                //.ForMember(x => x.SoftwareReleaseInformation, s => s.MapFrom(src => src.toSoftwareReleaseInformation()))
                 .ForMember(x => x.SoftwareProductionDate, s => s.MapFrom(src => src.SoftwareProductionDate != null ? src.SoftwareProductionDate.Value.Date : src.SoftwareProductionDate))
                 .ForMember(x => x.SoftwareProductionDateValue, s => s.MapFrom(src => src.SoftwareProductionDate != null ?
                src.SoftwareProductionDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
                 .ForMember(x => x.SoftwareInstallDateValue, s => s.MapFrom(src => src.SoftwareInstallDate != null ?
                src.SoftwareInstallDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
                 .ForMember(x => x.DataAcquisitionDateValue, s => s.MapFrom(src => src.DataAcquisitionDate != null ?
                src.DataAcquisitionDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
                 .ForMember(x => x.HardwareInstallDate, s => s.MapFrom(src => src.HardwareInstallDate))
                 .ForMember(dest => dest.VerticalName, opt => opt.MapFrom(x =>
                        (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0) ?
                        string.Join(",", x.VerticalFilterDto.Select(m => m.Value).ToList() ??
                        new List<string>()) : string.Empty))

            ;

        }
    }
}
