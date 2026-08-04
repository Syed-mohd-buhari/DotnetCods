using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.Entita.NetworkElement;
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
    public class AuditHistoryMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public AuditHistoryMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<AuditHistory, AuditHistoryDtoGrid>()
            .ForMember(x => x.ModificationUser, s => s.MapFrom(src => src.ModificationUserEntity.Email))
            .ForMember(x => x.ModificationDate, s => s.MapFrom(src => src.ModificationDate))
            .ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email))
            .ForMember(x => x.CreationDate, s => s.MapFrom(src => src.CreationDate));
            CreateMap<AuditHistoryCreateDto, AuditHistory>();

        }
    }

}
