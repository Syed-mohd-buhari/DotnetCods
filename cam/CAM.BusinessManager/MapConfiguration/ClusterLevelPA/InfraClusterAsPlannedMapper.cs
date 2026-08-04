using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.ClusterLevelPA;
using CAM.Entities.Model.ClusterLevelPA;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration.ClusterLevelPA
{
    public class InfraClusterAsPlannedMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public InfraClusterAsPlannedMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<InfraClusterAsPlanned, InfraClusterAsPlannedDtoGrid>()
             .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationuserNavigation.Email))
               .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.ModificationDate))
                //.ForMember(dest => dest.HardwaretypeId, opt => opt.MapFrom(src => src.HardwareType))
                //.ForMember(dest => dest.HardwaretypeValue, opt => opt.MapFrom(src => src.HardwareTypeValue))
            ;
             

        }
    }

}
