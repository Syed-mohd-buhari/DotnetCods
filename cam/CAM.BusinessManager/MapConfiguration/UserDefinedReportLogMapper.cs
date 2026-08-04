using AutoMapper;
using CAM.BusinessManager.Entity;
using CAM.Contracts.RepositoryContracts.Base;
 
using CAM.DataTransferObjects.Entita.UserDefinedReportLog;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace CAM.BusinessManager.MapConfiguration
{
    public class UserDefinedReportLogMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public UserDefinedReportLogMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<UserDefinedReportsLogs, UserDefinedReportLogDto>()
             .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationUserEntity.Email))
             .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.ModificationDate))
            .ForMember(des => des.ReportName, opt => opt.MapFrom(src => src.ReportName));



        }
    }

}
