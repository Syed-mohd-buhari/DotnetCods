using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.ResourceKeyMaster;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class ResourceKeyMasterMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        
        public ResourceKeyMasterMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            Majorhardwarebuilds mh=new Majorhardwarebuilds();
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<ResourceKeyMaster, ResourceKeyMasterDtoGrid>()
                .ForMember(x => x.ModificationUser, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.ModificationDate, s => s.MapFrom(src => src.ModificationDate))
                .ForMember(x => x.CreationDate, s => s.MapFrom(src => src.CreationDate))
                .ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email))
                .ForMember(x => x.OpCo, s => s.MapFrom(src => src.Opco.OpCoDescription))
                .ForMember(x => x.ResourceType, s => s.MapFrom(src => src.ResourceTypes.Name))
                .ForMember(x => x.KeyStatus, s => s.MapFrom(src => src.KeyStatus == true ? "In Use" : src.KeyStatus == null ? null : "Not In Use"))
                .ForMember(x => x.DesignComponentFamilyName, s => s.MapFrom(src => src.DesignComponentFamily.DCFName(_repositoryWrapper)))
                .ForMember(x => x.BagName, s => s.MapFrom(r => commonManager.GetBuildBagDescription(r.BuildBagId)))
                ;
                //toDesignComponentFamilyName(_repositoryWrapper)));
                //.ForMember(dest => dest.DesignComponent, opt => opt.MapFrom(src => Entities.Mappers.Entity.DesignComponentMapper.SetDesignComponentMapper(src).ToDesignComponentName(_repositoryWrapper)));
            //.ForMember(x => x.DesignComponentFamilyName, s => s.MapFrom(src => src.DesignComponent.toDesignComponentFamily()));
            //.ForMember(x => x.DesignComponentFamilyName, s => s.MapFrom(src => src.DesignComponentFamily.toDesignComponentFamilyNameForResourceKey(repositoryWrapper).Distinct()));
        }
    }
}

