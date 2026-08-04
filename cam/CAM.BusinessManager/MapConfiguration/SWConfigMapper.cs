using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.SoftwareConfiguration;
using CAM.Entities.Models.Lookup;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class SWConfigMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public SWConfigMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<SWConfigFunctionAreas, SWConfigGridDto>()
            .ForMember(x => x.OpCo, s => s.MapFrom(src => src.Function.Softwareconfiguration.Opco))
            .ForMember(x => x.Oem, s => s.MapFrom(src => src.Function.Softwareconfiguration.Oem))
            .ForMember(x => x.ElementName, s => s.MapFrom(src => src.Function.Softwareconfiguration.Elementname))
            .ForMember(x => x.FunctionName, s => s.MapFrom(src => src.Function.Functionname))
            .ForMember(x=>x.SoftwareConfigurationId, s=>s.MapFrom(src=>src.Function.Softwareconfiguration.Softwareconfigurationid));
            
           
           

        }
    }

}
