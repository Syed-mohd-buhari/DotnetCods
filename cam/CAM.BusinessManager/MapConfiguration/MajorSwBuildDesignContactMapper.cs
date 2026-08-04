using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.Entita.AuditLog;
using CAM.DataTransferObjects.Entita.NetworkElement;
using CAM.DataTransferObjects.Entita.NetworkElementAsIs;
using CAM.DataTransferObjects.Entita.ProblemCategory;
using CAM.DataTransferObjects.Entita.SystemVerificationProblem;
using CAM.DataTransferObjects.LookUp.MajorSwBuildsDesignContact;
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
    public class MajorSwBuildDesignContactMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public MajorSwBuildDesignContactMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<MajorSwBuildsDesignContactCreateDto, MajorSwBuidlsDesignContact>();

            CreateMap<MajorSwBuidlsDesignContact, MajorSwBuildsDesignContactUpdatedto>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationDate))
            .ForMember(x => x.DesignContact, s => s.MapFrom(src => src.DesignContact.Email))
            ;
            

            CreateMap<MajorSwBuidlsDesignContact, MajorSwBuildsDesignContactDto>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
            .ForMember(x => x.DesignContact, s => s.MapFrom(src => src.DesignContact.Email));



        }
    }

}
