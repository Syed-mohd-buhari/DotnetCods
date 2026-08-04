using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AssetPassThrough;
using CAM.Entities.Models.PassThroughData;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class PassThroughMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public PassThroughMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<AssetPassThrough, PassThroughDtoGrid>()
            //.ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            //.ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
            .ForMember(x => x.AssetTypeTsr, s => s.MapFrom(src => src.AssetType))
            .ForMember(x => x.ResourceKeyTsr, s => s.MapFrom(src => src.ResourceKey))
            .ForMember(x => x.LastPenTestDateTsr, s => s.MapFrom(src => src.LastPenTestDate))
            .ForMember(x => x.LastPenTestRefNo, s => s.MapFrom(src => src.LastPenTestRefNo))

             ;

        }
    }

}
