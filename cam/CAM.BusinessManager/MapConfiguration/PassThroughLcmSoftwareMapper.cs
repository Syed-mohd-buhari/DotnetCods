using AutoMapper;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AssetPassThrough;
using CAM.Entities.Models.PassThroughData;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class PassThroughLcmSoftwareMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public PassThroughLcmSoftwareMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<SwPassThroughLcm, PassThroughLcmSoftwareDtoGrid>()
                .ForMember(x => x.SwVendor, s => s.MapFrom(src => src.SWVendor))
                .ForMember(x => x.SwResourceKeyPassThrough, s => s.MapFrom(src => src.SwResourceKey))
                .ForMember(x => x.SwVendorEndOfMaintenanceDate, s => s.MapFrom(src => src.SWVendorEndOfMaintenanceDate))
                .ForMember(x => x.SwOperationsContactPoint, s => s.MapFrom(src => src.SWOperationsContactPoint))
                .ForMember(x => x.SwOpsMaintenanceConractEndDate, s => s.MapFrom(src => src.SWOpsMaintenanceConractEndDate))
                .ForMember(x => x.LcmStatusEngSoftware, s => s.MapFrom(src => src.LcmStatusEngSoftware))
                .ForMember(x => x.LcmStatusOpsSoftware, s => s.MapFrom(src => src.LcmStatusOpsSoftware))
                .ForMember(x => x.SoftwareVersion, s => s.MapFrom(src => src.SoftwareVersion))
                .ForMember(x => x.PlannedSoftwareVersion, s => s.MapFrom(src => src.PlannedSoftwareVersion))
             ;

        }
    }

}
