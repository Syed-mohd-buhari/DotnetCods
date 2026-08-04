using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.FeedBackLoopLog;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class FeedBackLoopLogMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public FeedBackLoopLogMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<FeedBackLoopAudits, FeedBackLoopAuditGridDto>();
            //.ForMember(x => x.ProcessingTime, s => s.MapFrom(src => src.ProcessStartTime - src.ProcessEndTime));
            //.ForMember(x => x.ModificationDate, s => s.MapFrom(src => src.ModificationDate))
            //.ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email))
            //.ForMember(x => x.CreationDate, s => s.MapFrom(src => src.CreationDate));

        }
    }

}
