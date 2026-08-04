using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.DesignComponentFamilyLifeCycle;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static CAM.Enum.ResourceTypeEnum;

namespace CAM.BusinessManager.MapConfiguration
{
    public class DesignComponentFamilyLifeCycleMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;

        public DesignComponentFamilyLifeCycleMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<DesignComponentFamilyLifeCycle, DesignComponentFamilyLifeCycleDtoGrid>()
                .ForMember(x => x.ModificationUser, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.ModificationDate, s => s.MapFrom(src => src.ModificationDate))
                .ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email))
                .ForMember(x => x.CreationDate, s => s.MapFrom(src => src.CreationDate))
                .ForMember(x => x.OpCo, s => s.MapFrom(src => src.OpcoDescription))
               // .ForMember(x => x.DesignComponentName, opt => opt.MapFrom(src => Entities.Mappers.Entity.DesignComponentMapper.SetDesignComponentMapper(src.DesignComponents).ToDesignComponentName(_repositoryWrapper)))
                //.ForMember(x => x.DesignComponentFamilyName, s => s.MapFrom(src => src.DesignComponentFamily.toDesignComponentFamilyName(_repositoryWrapper)))
                 .ForMember(x => x.CurrentDetail, s => s.MapFrom(src =>  
                 
                ((int)ResourceTypesKey.Component == src.CategoryType) ? Convert.ToString( src.BagName)+" "+src.CurrentDetails : src.CurrentDetails

                 ))
                 .ForMember(x => x.PlannedDetail, s => s.MapFrom(src => src.PlannedDetails))
                .ForMember(x => x.DesignComponentName, opt => opt.MapFrom(src => src.DcDescription))
                 .ForMember(x => x.DesignComponentFamilyName, s => s.MapFrom(src => src.DcfDescription))
                 .ForMember(x => x.CategoryType, s => s.MapFrom(src => getCategoryType((int)(src.CategoryType ?? 0))
                 ))
                ;

        }

        public string getCategoryType(int  categoryType)
        {
            if ((int)ResourceTypesKey.Component == categoryType) return "Component";           
            else if ((int)ResourceTypesKey.SWAsset == categoryType) return "Asset";
            else if ((int)ResourceTypesKey.HWAsset == categoryType) return "Asset";
            else if ((int)ResourceTypesKey.Identity == categoryType) return "Identity";
            else if ((int)ResourceTypesKey.Lcm == categoryType) return "LCM";

            return "";

        }
    }
}
