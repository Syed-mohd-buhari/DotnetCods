using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.Entita.Reconsiliation;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class ReconciliationMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public ReconciliationMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<AuditHistoryCreateDto, AuditHistory>();
            CreateMap<ReconciliationCreateDto, ReconciliationModel>();

            CreateMap<ReconciliationModel, ReconciliationGridDto>()
           .ForMember(x => x.LcmId, s => s.MapFrom(src => CAM.BusinessManager.Entity.ReconciliationManager.GetLcmRecord(src.AssetId, _repositoryWrapper)))
           .ForMember(x => x.PlannedActivityId, s => s.MapFrom(src => CAM.BusinessManager.Entity.ReconciliationManager.GetPARecord(src.AssetId, _repositoryWrapper,src.Status)));
           //.ForMember(x => x.Creationuser, s => s.MapFrom(src => src.CreationUserEntity.Email))
           //.ForMember(x => x.Creationdate, s => s.MapFrom(src => src.CreationDate));

        }
    }

}
