using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.DesignComponentFamily;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using CAM.Enum;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class DesignComponentFamilyMapper : Profile
    {

        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager;
        public DesignComponentFamilyMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper , CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));



            CreateMap<DesignComponentFamilyDtoUpdate, DesignComponentFamily>()
                .ReverseMap()
                .ForMember(x => x.LastModified, opt => opt.MapFrom(x => x.ModificationDate))
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(x => x.ModificationUserEntity.Email))
                .ForMember(x=>x.ProductNameId, opt=> opt.MapFrom(x=>x.ProductNameId))

                ;

            CreateMap<DesignComponentFamily, DesignComponentFamilyDtoGrid>()
                .ForMember(x => x.Orphan, s => s.MapFrom(src => !src.DesignComponents.Any()))
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                 .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
                src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.SubNetworkBoundary, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.SubNetworkBoundary.Alias) ? src.SubNetworkBoundary.Description : src.SubNetworkBoundary.Alias))
                .ForMember(dest => dest.SharingType, opt => opt.MapFrom(src => src.SharingType.SharingTypeDescription))
                .ForMember(x => x.ProductName, opt => opt.MapFrom(x => x.ProductNameNavigation != null ? x.ProductNameNavigation.Description : null))
                 .ForMember(
                    dest => dest.SupportedServices,
                    opt => opt.MapFrom(x => string.Join(" | ", x.SubNetworkBoundary.SupportedServices.Select(fx => fx.SupportedService.Description).Distinct()))
                )
                .ForMember(
                    dest => dest.VodafoneName,
                    opt => opt.MapFrom(x => string.Join(" | ", x.DesignComponents.Select(fx => fx.SystemType.VodafoneName.Description).Distinct()))
                )
                .ForMember(
                    dest => dest.VodafoneNameId,
                    opt => opt.MapFrom(x =>  x.DesignComponents.Select(fx => fx.SystemType.VodafoneNameId).Distinct().FirstOrDefault())
                ) 
  .ForMember(
                    dest => dest.SystemTypeIdBasedVerticalValue,
                    opt => opt.MapFrom(src => string.Join(",", src.VerticalFilterDto.Select(t => t.Value).ToList() ?? new List<string>())  ))


                .ForMember(x => x.LastModified, opt => opt.MapFrom(x => x.ModificationDate))
                .ForMember(x=>x.DesignComponentFamilyName, opt => opt.MapFrom(src=>src.DCFName(_repositoryWrapper)))
                .ForMember(x => x.DesignContact, s => s.MapFrom(src => 
                _commonManager.GetSubdomainSpocEmailForLibary(src.DesignComponents.FirstOrDefault().SystemType.MajorSoftwareBuilds.MajorSoftwareBuildsId,
                src.DesignComponents.FirstOrDefault().SystemType.SystemTypesMajorHardwareBuilds.Select(x => x.MajorHardware.MajorHardwareId).FirstOrDefault())))
                ;
            
            CreateMap<DesignComponentFamilyDtoCreate, DesignComponentFamily>()
                 .ForMember(x => x.ProductNameId, x => x.MapFrom(s => s.ProductNameId))
               
            ;
        }
    }
}