using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.BPT;
using CAM.Entities.Models;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class BudgetProjectTrackerMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        public BudgetProjectTrackerMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper)
        {

            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));
            CreateMap<BudgetProjectTrackers, BPTGridDto>()

            .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
           // .ForMember(x => x.CurrentTrackingNumber, s => s.MapFrom(src => string.Empty))
            .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
            //.ForMember(x => x.Activity, s => s.MapFrom(src => string.Join("-", src.PlannedActivity.OpCo.OpCoDescription, src.PlannedActivity.DesignAspectId != null ?src.PlannedActivity.DesignAspect.DesignComponentFamily.toDesignComponentFamilyName(_repositoryWrapper):
            //CAM.Entities.Mappers.Entity.DesignComponentMapper.SetDesignComponentMapper(src.PlannedActivity.DesignComponent).toDesignComponentFamily(_repositoryWrapper),
            //src.PlannedActivity.PlannedActivityResource.PlannedActivityResourceDescription)))            
            .ForMember(dest => dest.VerticalName, opt => opt.MapFrom(x =>
                        (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0) ?
                        string.Join(",", x.VerticalFilterDto.Select(m => m.Value).ToList() ??
                        new List<string>()) : string.Empty))
            .ForMember(x => x.Activity, s => s.MapFrom(src => CAM.Entities.Mappers.Entity.PlannedActivityMapper.Set(src.PlannedActivity).GetActvityString(_repositoryWrapper)));

        }
    }

}
