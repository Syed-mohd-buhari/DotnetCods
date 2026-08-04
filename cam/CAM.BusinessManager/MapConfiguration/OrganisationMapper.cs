using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.Organisation;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class OrganisationMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager;
        public OrganisationMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper,CommonManager commonManager)
        {
            _commonManager = commonManager;

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<OrganisationCreateDto, OrganisationModel>();
            CreateMap<OrganisationUpdateDto, OrganisationModel>();

            CreateMap<OrganisationModel, OrganisationUpdateDto>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationDate));

            CreateMap<OrganisationModel, OrganisatioDtoGrid>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
            //.ForMember(x => x.OpCo, s => s.MapFrom((src,_,_,context) => (bool)context.Items["isRemoveOpcoAndVertical"] ? string.Empty : _commonManager.GetUserOpcosForOrganisationLibary(src.ContactId)))
            //.ForMember(x => x.Contact, s => s.MapFrom(src => src.ContactNavigation.Email))
            .ForMember(x => x.PracticeContact, s => s.MapFrom(src => src.Practice.PracticeEmail.Email))
            .ForMember(x => x.Practice, s => s.MapFrom(src => src.Practice.PracticeDescription))
            .ForMember(x => x.VerticalResponsible, s => s.MapFrom(src => src.VerticalResponsibleName))
           
            //.ForMember(x => x.VerticalResponsible, s => s.MapFrom((src, _, _, context) => (bool)context.Items["isRemoveOpcoAndVertical"] ? string.Empty : _commonManager.GetUserVerticalForOrganisationLibary(src.ContactId)))
            //.ForMember(x => x.SubdomainResponsible, s => s.MapFrom(src => src.SubdomainResponsible.SubDomainResponsibleDescription))
            .ForMember(x => x.MainOrganisation, s => s.MapFrom(src => src.MainOrganisation.MainorganisationDescription))
            ;



        }
    }

}
