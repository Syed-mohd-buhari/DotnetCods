using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.ServicePlan;
using CAM.Entities.Models;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class ServicePlanMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager;
        public ServicePlanMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<ServicePlan, ServicePlanPaGridDto>()
            .ForMember(x => x.ServicePlanId, opt => opt.MapFrom(src => src.Serviceplanid))
           .ForMember(x => x.ServiceName, opt => opt.MapFrom(src => src.Servicemaster.Description))
           .ForMember(x => x.DesignComponentFamilyName, opt => opt.MapFrom(src => string.Join(" ; ", _commonManager.GetServicePlanDetails(src.Serviceplanid).Select(r => r.DCFName))))
           .ForMember(dest => dest.PlannedActivity,
                   opt => opt.MapFrom(x =>
                       x.Plannedactivity.Where(x => x.Archived != true && !x.Deleted).ToDictionary(x => x.PlannedActivityId,
                           x => x.GetPlannedAction(_repositoryWrapper))))
           .ForMember(x => x.OpCo, opt => opt.MapFrom(src => src.Opco.OpCoDescription))
           .ForMember(dest => dest.VerticalName, opt => opt.MapFrom(x =>
                        (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0) ?
                        string.Join(",", x.VerticalFilterDto.Select(m => m.Value).ToList() ??
                        new List<string>()) : string.Empty));
           


        }
    }

}
