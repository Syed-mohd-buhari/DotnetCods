using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.TsrLog;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class TsrLogMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public TsrLogMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<TsrLog, TsrLogDtoGrid>()
            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate));

        }
    }

}
