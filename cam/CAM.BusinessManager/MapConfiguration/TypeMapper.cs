using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.IdentityAsIs;
using CAM.DataTransferObjects.LookUp.Type;
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
    public class TypeMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;


        public TypeMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<Entities.Models.Lookup.Type, TypeDtoGrid>()
                .ForMember(des => des.CategoryDescription, opt => opt.MapFrom(src => src.Class.Category.Description))
                .ForMember(des => des.ClassDescription, opt => opt.MapFrom(src => src.Class.Description))
                .ForMember(des => des.LastModifiedValue, opt => opt.MapFrom(src => src.ModificationDate.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture)))
                .ForMember(des => des.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationUserEntity.Email));
                
        }
    }
}
