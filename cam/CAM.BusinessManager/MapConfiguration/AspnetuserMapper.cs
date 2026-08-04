using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AspNetUser;
using CAM.Entities.Models;
using CAM.Identity;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class AspnetuserMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public AspnetuserMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<ApplicationUser, AspnetuserGridDto>()
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEmail))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.Modificationdate))
                .ForMember(x => x.UserId, s => s.MapFrom(src => src.Id))
                .ForMember(x => x.MainOrganisationId, s => s.MapFrom(src => src.ApplicationOrgAndVerticals != null && src.ApplicationOrgAndVerticals.Count > 0 ?
                 src.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.MainOranisationId : 0))
                .ForMember(x => x.PracticeId, s => s.MapFrom(src => src.ApplicationOrgAndVerticals != null && src.ApplicationOrgAndVerticals.Count > 0 ?
                 src.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.PracticeId : 0))
                .ForMember(x => x.PracticeContactId, s => s.MapFrom(src => src.ApplicationOrgAndVerticals != null && src.ApplicationOrgAndVerticals.Count > 0 ?
                 src.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.Practice.PracticeEmailId : 0))
                .ForMember(x => x.MainOrganisation, s => s.MapFrom(src => src.ApplicationOrgAndVerticals != null && src.ApplicationOrgAndVerticals.Count > 0 ?
                 src.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.MainOrganisation.MainorganisationDescription : string.Empty))
                .ForMember(x => x.Practice, s => s.MapFrom(src => src.ApplicationOrgAndVerticals != null && src.ApplicationOrgAndVerticals.Count > 0 ?
                 src.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.Practice.PracticeDescription : string.Empty))
                .ForMember(x => x.PracticeContact, s => s.MapFrom(src => src.ApplicationOrgAndVerticals != null && src.ApplicationOrgAndVerticals.Count > 0 ?
                 src.ApplicationOrgAndVerticals.FirstOrDefault().ApplicationOrganisation.Practice.PracticeEmail.Email : string.Empty))
                .ForMember(x => x.SubdomainResponsible, s => s.MapFrom(src => src.ApplicationSubDomainRes.SubDomainRes))
                .ForMember(x => x.IsSubDomainSpoc, s => s.MapFrom(src => src.Issubdomainspoc))
                .ForMember(x => x.IsEduSpoc, s => s.MapFrom(src => src.Iseduspoc))
                .ForMember(x => x.IsDesigncontact, s => s.MapFrom(src => src.Isdesigncontact))
                .ForMember(x => x.OpCo, s => s.MapFrom(src => src.Opcos != null && src.Opcos.Count > 0 ? string.Join(",", src.Opcos.Where(f => f.Isinusedopcos == true)
                .Select(x => x.ApplicationOpco.OpCoName).ToList()) : string.Empty))
                .ForMember(x => x.Vertical, s => s.MapFrom(src => src.ApplicationOrgAndVerticals != null && src.ApplicationOrgAndVerticals.Count > 0 ?
                string.Join(",", src.ApplicationOrgAndVerticals.Where(f => f.Isvertical == true).Select(x => x.ApplicationOrganisation.VerticalRes.VerticalRes).ToList()) : string.Empty))
                .ForMember(x => x.VerticalResponsible, s => s.MapFrom(src => src.ApplicationOrgAndVerticals != null && src.ApplicationOrgAndVerticals.Count > 0 ?
                string.Join(",", src.ApplicationOrgAndVerticals.Where(f => f.Isverticalresponcible == true).Select(x => x.ApplicationOrganisation.VerticalRes.VerticalRes).ToList()) : string.Empty))
                .ForMember(x => x.Role, s => s.MapFrom(src => src.UserRoles != null && src.UserRoles.Count > 0 ? string.Join(",", src.UserRoles.Select(x => x.Role.RoleName).ToList()) : string.Empty));

            CreateMap<IGrouping<AspNetMapper, AspNetUserRoles>, UserRoleDownlodeGridDto>()
               .ForMember(x=>x.UserId,s=>s.MapFrom(src => src.Key.Userid))
               .ForMember(x => x.UserName, s => s.MapFrom(src => src.First().User.UserName))
               .ForMember(x => x.Email, s => s.MapFrom(src => src.First().User.Email))
               .ForMember(x => x.Active, s => s.MapFrom(src => src.First().User.Active))
              .ForMember(x => x.OpCo, s => s.MapFrom(src => string.Join(",", src.First().User.Opcos.SelectMany(x => x.ApplicationOpco.OpCoName)).Distinct().ToList()))
              .ForMember(x => x.Role, s => s.MapFrom(src => string.Join(",", src.Select(r => r.Role.Name).Distinct().ToList())))
              .ForMember(x => x.VerticalResponsible, s => s.MapFrom(src => string.Join(",", src.First().User.ApplicationOrgAndVerticals.SelectMany(r => r.ApplicationOrganisation.VerticalRes.VerticalRes).Distinct().ToList())));

        }
    }

}
