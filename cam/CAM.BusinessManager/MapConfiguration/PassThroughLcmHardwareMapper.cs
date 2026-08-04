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
    public class PassThroughLcmHardwareMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public PassThroughLcmHardwareMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<HwPassThroughLcm, PassThroughLcmHardwareDtoGrid>()
                .ForMember(x => x.HwVendor, s => s.MapFrom(src => src.HWVendor))
                .ForMember(x => x.HwResourceKeyPassThrough, s => s.MapFrom(src => src.HwResourceKey))
                .ForMember(x => x.HwVendorEndOfMaintenanceDate, s => s.MapFrom(src => src.HWVendorEndOfMaintenanceDate))
                .ForMember(x => x.HwOperationsContactPoint, s => s.MapFrom(src => src.HWOperationsContactPoint))
                .ForMember(x => x.HwOpsMaintenanceConractEndDate, s => s.MapFrom(src => src.HWOPSMaintenanceContractEndDate))
                .ForMember(x => x.LcmStatusOpsHardware, s => s.MapFrom(src => src.LcmStatusOpsHardware))
                .ForMember(x => x.LcmStatusEngHardware, s => s.MapFrom(src => src.LcmStatusEngHardware))
                .ForMember(x => x.PlannedHWModel, s => s.MapFrom(src => src.PlannedHWModel))


             ;

        }
    }

}
