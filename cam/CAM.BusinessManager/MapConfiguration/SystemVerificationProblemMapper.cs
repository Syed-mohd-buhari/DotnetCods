using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.Entita.AuditLog;
using CAM.DataTransferObjects.Entita.NetworkElement;
using CAM.DataTransferObjects.Entita.NetworkElementAsIs;
using CAM.DataTransferObjects.Entita.SystemVerificationProblem;
using CAM.Entities.Models;
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
    public class SystemVerificationProblemMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public SystemVerificationProblemMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<SystemVerificationProblemCreateDto, SystemVerificationProblems>();
            CreateMap<SystemVerificationProblemUpdateDto, SystemVerificationProblems>();

            CreateMap<SystemVerificationProblems, SystemVerificationProblemUpdateDto>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationDate));

            CreateMap<SystemVerificationProblems, SystemVerificationProblemDtoGrid>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
            .ForMember(x => x.OpCoName, s => s.MapFrom(src => src.OpCo.OpCoDescription))
            .ForMember(x => x.ProblemCategory, s => s.MapFrom(src => src.ProblemCategory.ProblemCategoryDescription))
            .ForMember(x => x.Environment, s => s.MapFrom(src => src.Environment.EnvironmentDescription))
            .ForMember(x => x.SeverityDescription, s => s.MapFrom(src => src.SeverityEntity.SeverityDescription));



        }
    }

}
