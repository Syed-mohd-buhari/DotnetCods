using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.Entity;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class PlannedActivityMapper : Profile
    {
        private IRepositoryWrapper _repositoryWrapper;
        private CommonManager _commonManager;
        public PlannedActivityMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper,CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));

            CreateMap<PlannedActivityDtoUpdate, PlannedActivity>().ReverseMap()
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(x => x.ModificationUserEntity != null ? x.ModificationUserEntity.Email : ""))
                .ForMember(x => x.RiskOpeId, opt => opt.MapFrom(x => x.OperationalRiskId))
                .ForMember(x => x.RiskEngId, opt => opt.MapFrom(x => x.EngineeringRiskId))
                .ForMember(x => x.LastModified, opt => opt.MapFrom(x => x.ModificationDate))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(x => x.StartDate))
                .ForMember(x => x.Archived, opt => opt.MapFrom(x => x.Archived))
                .ForMember(x => x.ForAddAsset, opt => opt.MapFrom(x => x.ForAddAsset))
                .ForMember(x => x.ForEditAsset, opt => opt.MapFrom(x => x.ForEditAsset))
                .ForMember(x => x.ProjectDescription, opt => opt.MapFrom(x => x.ProjectDescription))
                .ForMember(x => x.LcmCategories, opt => opt.MapFrom(src => _commonManager.LcmCategoryStatust(src.Lcmcategories)))
                .ForMember(x => x.OriginalLcmEngineeringId, opt => opt.MapFrom(x => x.OriginalLcmEngineeringId))
                .ForMember(x => x.ProgramId, opt => opt.MapFrom(x => x.ProgramId))
                .ForMember(x => x.PreBaseLineDate, opt => opt.MapFrom(x => x.PreBaseLineDate))
                 .ForMember(dest => dest.BuildBagId, opt => opt.MapFrom(
                    src => src.Buildbagid
                ))
                .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
                src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(x => x.PlannedCompletionValue, s => s.MapFrom(src => src.PlannedCompletion != null ?
                src.PlannedCompletion.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
                 .ForMember(x => x.StartDateValue, s => s.MapFrom(src => src.StartDate != null ?
                src.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))                 
                 .ForMember(x => x.PreBaseLineDateValue, s => s.MapFrom(src => src.PreBaseLineDate != null ?
                src.PreBaseLineDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
                 ;

            _ = CreateMap<PlannedActivity, PlannedActivityDtoGrid>()
                .ForMember(x => x.DesignComponentFamilyName, opt => opt.MapFrom(src => src.toOriginalDesignComponentFamilyDescription(_repositoryWrapper)))
                .ForMember(x => x.PlannedDesignComponentName, opt => opt.MapFrom(src => src.toPlannedDesignComponentDescription(_repositoryWrapper)))
                .ForMember(x => x.OriginalDesignComponent, opt => opt.MapFrom(src => src.toOriginalDesignComponentDescription(_repositoryWrapper)))
                .ForMember(x => x.PlannedDesignComponent, opt => opt.MapFrom(src => src.toPlannedDesignComponentDescription(_repositoryWrapper)))
                .ForMember(x => x.PlannedActivityId, opt => opt.MapFrom(x => x.PlannedActivityId))
                .ForMember(x => x.DesignComponentFamilyIndex, opt => opt.MapFrom(x => x.DesignComponentFamilyId))
                .ForMember(x => x.OriginalDesignComponentIndex, opt => opt.MapFrom(x => x.LcmEngineeringId != null ? x.LcmEngineering.DesignComponentId :
                x.NetworkElementAsPlannedId != null ? x.NetworkElementAsPlanned.DesignComponentId : (long?)null))
                .ForMember(x => x.PlannedDesignComponentIndex, opt => opt.MapFrom(x => x.DesignComponentId))
                .ForMember(x => x.PlanningActivityStatusId, opt => opt.MapFrom(x => x.PlannedActivityResource.RuleLinkedDc == (int)PlannedActivityResourceEnum.No_PlannedActivity ? string.Empty : x.PlanningActivityStatus != null ? x.PlanningActivityStatus.PlanningActivityStatusDescription : string.Empty))
                .ForMember(x => x.ActivityStatusId, opt => opt.MapFrom(x => x.PlannedActivityResource.RuleLinkedDc == (int)PlannedActivityResourceEnum.No_PlannedActivity ? string.Empty: x.ActivityStatus != null?x.ActivityStatus.ActivityStatusDescription: string.Empty))
                .ForMember(x => x.DeliveryStatusId, opt => opt.MapFrom(x => x.DeliveryStatus != null ? x.DeliveryStatus.DeliveryStatusDescription : string.Empty))
                .ForMember(x => x.ResponsibilityPhaseId, opt => opt.MapFrom(x => x.ResponsibilityPhase != null ? x.ResponsibilityPhase.ResponsibilityPhaseDescription: string.Empty))
                .ForMember(x => x.PlannedActivityResourceId, opt => opt.MapFrom(x => x.PlannedActivityResource.PlannedActivityResourceDescription))
                .ForMember(x => x.BudgetValueGrid, opt => opt.MapFrom(x => x.BudgetValue.ToString() + x.Currency))
                .ForMember(x => x.BudgetAvailability, opt => opt.MapFrom(x => x.BudgetAvailability != null ? x.BudgetAvailability.BudgetAvailabilityDescription : string.Empty))
                .ForMember(o => o.OpCo, o => o.MapFrom(s => s.LcmEngineeringId != null ? s.LcmEngineering.OpCo.OpCoDescription : (s.DesignAspectId != null ? s.DesignAspect.OpCo.OpCoDescription : s.NetworkElementAsPlanned!= null ? s.NetworkElementAsPlanned.OpCo.OpCoDescription: s.OpCo != null ? s.OpCo.OpCoDescription : string.Empty)))
                .ForMember(x => x.Archived, opt => opt.MapFrom(x => x.Archived))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
                .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
                src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(x => x.PlannedCompletionValue, s => s.MapFrom(src => src.PlannedCompletion != null ?
                src.PlannedCompletion.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
                 .ForMember(x => x.StartDateValue, s => s.MapFrom(src => src.StartDate != null ?
                src.StartDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
               .ForMember(x => x.PreBaseLineDateValue, s => s.MapFrom(src => src.PreBaseLineDate != null ?
                src.PreBaseLineDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty))
                .ForMember(x => x.StartDate, s => s.MapFrom(src => src.StartDate))
                .ForMember(x => x.PreBaseLineDate, s => s.MapFrom(src => src.PreBaseLineDate))
                .ForMember(x => x.Program, s => s.MapFrom(src => src.ProgramNavigation != null ? src.ProgramNavigation.ProgramDescription:string.Empty))
                .ForMember(x => x.Driver, s => s.MapFrom(src => src.Driver != null ? src.Driver.DriverDescription : string.Empty))
                .ForMember(x => x.Benefits, s => s.MapFrom(src => src.Benefit != null ? src.Benefit.BenefitDescription :string.Empty))
                .ForMember(x => x.ActivityDetails, s => s.MapFrom(src => src.ActivityDetails == null && src.ActivityDetails == "" ? string.Empty: src.ActivityDetails))
                .ForMember(x => x.PlanningRisk, s => s.MapFrom(src => src.PlanningRisk != null ? src.PlanningRisk.PlanningRiskDescription : string.Empty))
                .ForMember(x => x.RiskEngineeringEvaluation, s => s.MapFrom(src => src.EngineeringRisk != null ?src.EngineeringRisk.RiskDescription : string.Empty))
                .ForMember(x => x.RiskOperationalEvaluation, s => s.MapFrom(src => src.OperationalRisk != null ? src.OperationalRisk.RiskDescription : string.Empty))
                .ForMember(x => x.DeliveryTrackingId, opt => opt.MapFrom(src => src.DeliveryTrackingId))
                 .ForMember(x => x.DeliveryProjectPpmId, opt => opt.MapFrom(src => src.DeliveryProjectId))
                 .ForMember(x => x.ProjectDescription, opt => opt.MapFrom(src => src.ProjectDescription))
                 .ForMember(x => x.LcmCategories, opt => opt.MapFrom(src => _commonManager.LcmCategoryStatust(src.Lcmcategories)))
            .ForMember(x => x.PlannedBuildBagDescription, s => s.MapFrom(src => _commonManager.GetBuildBagDescriptionFromEnity(src.Buildbag != null ? src.Buildbag : null)))
              .ForMember(x => x.CurrentBuildBagDescription, s => s.MapFrom(src =>
              (src.LcmEngineering != null) ?
              _commonManager.GetBuildBagDescriptionFromEnity(src.LcmEngineering.BuildBag) : string .Empty 
              ))
          .ForMember(dest => dest.VerticalName, opt => opt.MapFrom(x =>
                        ( x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0) ?
                        string.Join(",", x.VerticalFilterDto.Select(m => m.Value).ToList() ??
                        new List<string>()) : string.Empty))

          .ForMember(dest => dest.VerticalNameId, opt => opt.MapFrom(x =>
                        (x.LcmEngineering != null && x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0) ?
                         x.VerticalFilterDto.Select(m => m.Key.ToString()).Distinct().ToList()
                        :  new List<string>()) 

                 )

                .ForMember(x => x.PlannedActivityTypeFor, opt => opt.MapFrom(src => src.GetPlannedActivityTypeFor()))
            #region Ticket 685 Dev - #674 SettingsUpdatePlanedActivity - Display  Planned Activity Associated/Linked Table Details  - Ex : LCM, DA, Assets

                .ForMember(x => x.ForAddAsset, opt => opt.MapFrom(src => (src.ForAddAsset == null) ? false : src.ForAddAsset
                ))

                 .ForMember(x => x.ForEditAsset, opt => opt.MapFrom(src => (src.ForEditAsset == null) ? false : src.ForEditAsset
                ))
                .ForMember(x => x.ForLcmLink, opt => opt.MapFrom(src => src.LcmEngineeringId != null ? 1 : 0))
                .ForMember(x => x.ForDesignAspectLink, opt => opt.MapFrom(src => src.DesignAspectId != null ? 1 : 0)) //"YES" : "NO"))

                .ForMember(x => x.ForServicePlanLink, opt => opt.MapFrom(src => src.Serviceplanid != null ? 1 : 0));
            #endregion
                ;

            _ = CreateMap<PlannedActivity, ArchivedPlannedActivityDtoGrid>()             
        .ForMember(dest => dest.VerticalName, opt => opt.MapFrom(x =>
                      (x.LcmEngineering != null && x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0) ?
                      string.Join(",", x.VerticalFilterDto.Select(m => m.Value).ToList() ??
                      new List<string>()) : string.Empty))

        .ForMember(dest => dest.VerticalNameId, opt => opt.MapFrom(x =>
                      (x.LcmEngineering != null && x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0) ?
                       x.VerticalFilterDto.Select(m => m.Key.ToString()).Distinct().ToList()
                      : new List<string>()) 
               ) 
              ;
            CreateMap<PlannedActivityDtoCreate, PlannedActivity>()
                .ForMember(r => r.PlannedActivityResource, pot => pot.MapFrom(r => r.PlannedActivityResource.Values))
                .ForMember(r => r.EngineeringRiskId, pot => pot.MapFrom(r => r.RiskEngId))
                .ForMember(r => r.OperationalRiskId, pot => pot.MapFrom(r => r.RiskOpeId));
        }
    }
}
