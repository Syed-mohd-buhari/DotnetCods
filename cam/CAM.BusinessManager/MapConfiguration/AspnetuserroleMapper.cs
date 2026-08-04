using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.Entities.Models;
using CAM.Identity;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class AspnetuserroleMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public AspnetuserroleMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<AspNetUserRoles, AspnetuserroleGridDto>()   
                .ForMember(x => x.UserName, s => s.MapFrom(src => src.User.UserName))
                .ForMember(x => x.Email, s => s.MapFrom(src => src.User.Email))
               .ForMember(x => x.Active, s => s.MapFrom(src => src.User.Active));


            CreateMap<ApplicationUser, AspNetUserROVGridDto>()
                //.ForMember(x => x.AspNetUserRoleId, s => s.MapFrom(src => src.Select(x => x.AspNetUserRoleId).FirstOrDefault()))
                .ForMember(x => x.OpCo, s => s.MapFrom(src => string.Join(",", src.Opcos.Where(f => f.Isinusedopcos == true).Select(r => r.ApplicationOpco.OpCoName).ToList())))
                .ForMember(x => x.Role, s => s.MapFrom(src => string.Join(",", src.UserRoles.Select(r => r.Role.RoleName).ToList())))
                .ForMember(x => x.RoleId, s => s.MapFrom(src => string.Join(",", src.UserRoles.Select(r => r.Role.RoleId).ToList())))
                //.ForMember(x => x.RoleDescription, s => s.MapFrom(src => string.Join(",",src.UserRoles.Select(r => $"{{{r.Role.RoleId}:{r.Role.Description}}}"))))
                .ForMember(x => x.Vertical, s => s.MapFrom(src =>
                string.Join(",", src.ApplicationOrgAndVerticals.Where(f => f.Isvertical == true).Select(r => r.ApplicationOrganisation.VerticalRes.VerticalRes).ToList())))
                .ForMember(x => x.VerticalId, s => s.MapFrom(src =>
                string.Join(",", src.ApplicationOrgAndVerticals.Where(f => f.Isvertical == true).Select(r => r.ApplicationOrganisation.VerticalRes.VerticalResId).ToList())))
                .ForMember(x => x.VerticalResponsible, s => s.MapFrom(src =>
                string.Join(",", src.ApplicationOrgAndVerticals.Where(f => f.Isverticalresponcible == true).Select(r => r.ApplicationOrganisation.VerticalRes.VerticalRes).ToList())))
                .ForMember(x => x.VerticalResponsibleId, s => s.MapFrom(src =>
                string.Join(",", src.ApplicationOrgAndVerticals.Where(f => f.Isverticalresponcible == true).Select(r => r.ApplicationOrganisation.VerticalRes.VerticalResId).ToList())))
                .ForMember(x => x.SubdomainResponsibleId, s => s.MapFrom(src => src.Subdomainresponsibleid))
                .ForMember(x => x.Issubdomainspoc, s => s.MapFrom(src => src.Issubdomainspoc))
                .ForMember(x => x.Iseduspoc, s => s.MapFrom(src => src.Iseduspoc))
                .ForMember(x => x.Isdesigncontact, s => s.MapFrom(src => src.Isdesigncontact))
                .ForMember(x => x.RestrictedOpCoIds, s => s.MapFrom(src => string.Join(",", src.Opcos.Where(f => f.Isrestrictedopco == true).Select(r => r.ApplicationOpco.OpCoName).ToList())));
        }
    }

}
