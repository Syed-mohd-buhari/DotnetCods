using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.SoftwareConfiguration;
using CAM.DataTransferObjects.LookUp.Component;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.MapConfiguration
{
    public class SoftwareConfigurationMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public SoftwareConfigurationMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));


            CreateMap<SoftwareConfigurationFamily, SoftwareConfigurationDtoGrid>()
                .ForMember(x => x.ModificationUser, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.ModificationDate, s => s.MapFrom(src => src.ModificationDate))
                .ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email))
                .ForMember(x => x.CreationDate, s => s.MapFrom(src => src.CreationDate));
            CreateMap<SubFunctionArea, SoftwareConfigurationDtoGrid>();
                //.ForMember(x => x.ModificationUser, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                //.ForMember(x => x.ModificationDate, s => s.MapFrom(src => src.ModificationDate))
                //.ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email))
                //.ForMember(x => x.CreationDate, s => s.MapFrom(src => src.CreationDate));
            //CreateMap<SoftwareConfiguration, SoftwareConfigurationDto>();
            //.ForMember(dest => dest.Function,
            //    opt => opt.MapFrom(x =>
            //        x.Function.Where(x => !x.Deleted).ToDictionary(x => x.Functionid,
            //            x =>
            //              $"{x.Functionname} | {x.Creationdate} | {x.Creationuser}"))
            //    );
            //.ForMember(x=>x.softwareComponentId,opt=>opt.MapFrom(src=>src.Softwareconfigurationid));

        }
    }

}
