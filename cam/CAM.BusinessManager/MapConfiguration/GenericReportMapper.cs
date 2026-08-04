using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.GenericReportDto;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class GenericReportMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;

        public GenericReportMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<CAM.Entities.Models.DynamicReports, GenericReportGridCreateDto>()
                .ForMember(x => x.ModificationUser, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.ModificationDate, s => s.MapFrom(src => src.ModificationDate))
                .ForMember(x => x.CreationUser, s => s.MapFrom(src => src.CreationUserEntity.Email))
                .ForMember(x => x.CreationDate, s => s.MapFrom(src => src.CreationDate))
                .ForMember(x => x.ExportType, s => s.MapFrom(src =>
                (string.IsNullOrEmpty(Convert.ToString(src.ExportType ))) ? null :
                   (( src.ExportType  == 1) ? "Aggregate"
                    : ( src.ExportType == 2) ? "DisAggregate" : null)

                ))
                .ForMember(x => x.ScheduledType, s => s.MapFrom(src =>
                (string.IsNullOrEmpty(Convert.ToString(src.ScheduledType))) ? null :
                   ((src.ScheduledType == 1) ? "Monthly"
                    : (src.ScheduledType == 2) ? "Weekly" : null)

                ))
                .ForMember(x => x.Published, s => s.MapFrom(src => src.Published == true ? "Published" : src.Published == null ? null : "Not Published"))
                //.ForMember(x=>x.UserId,s => s.MapFrom(src => src.Users.Email))


                ;

        }
    }
}
