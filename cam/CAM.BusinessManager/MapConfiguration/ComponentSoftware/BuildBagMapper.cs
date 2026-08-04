using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration.ComponentSoftware
{
    public class BuildBagMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager { get; set; }
        public BuildBagMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));


            _ = CreateMap<BuildBag, BuildBagDtoGrid>()
                 //.ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                 //.ForMember(x => x.BuildBagDescription, s => s.MapFrom(src => src.ComponentBagDescription))
                //.ForMember(x => x.LastModifiedValue, opt => opt.MapFrom(s => s.ModificationDate)); 
                ;
            _ = CreateMap<Buildbags, BuildBagEditPageDto>();

            CreateMap<BuildBagCreatePageDto, BuildBag>()
               .ForMember(x => x.BagDescription, s => s.MapFrom(src => src.BuildBagDescription));

            CreateMap<BuildBagUpdateDto, BuildBag>()
                  .ForMember(x => x.BagDescription, s => s.MapFrom(src => src.BuildBagDescription));
                 
        }
    }
}