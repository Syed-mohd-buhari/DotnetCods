using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.Entities.Models;
using CAM.Entities.Models.ComponentSoftware;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration.ComponentSoftware
{
    public class ComponentManufacturersMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public ComponentManufacturersMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            


              CreateMap<ComponentManufacturers, ComponentManufacturersGridDto>()               
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))               
                .ForMember(x => x.LastModified, opt => opt.MapFrom(s => s.ModificationDate));


            //CreateMap<ComponentManufacturersUpdateGridDto, ComponentManufacturers>()
            //    .ForMember(x => x.ModificationUserEntity.Email, x => x.MapFrom(s => s.LastModifiedBy))
            //    .ForMember(x => x.ModificationDate, s => s.MapFrom(s => s.LastModified));
            //.ReverseMap();


        }
    }
}