using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using CAM.Repository.Helpers;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace CAM.BusinessManager.MapConfiguration
{
    public class NetworkElementsAsPlannedMapper : Profile
    {
        private const short MAX_CHAR_ACTIVITIES = 80;
        private IRepositoryWrapper _repositoryWrapper;
        CommonManager _commonManager;

        public NetworkElementsAsPlannedMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper,CommonManager commonManager)
        {
            _commonManager = commonManager;
            var authenticatedUser = contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            string email = authenticatedUser != null ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : (wrappers.First()));


            #region Update
            CreateMap<NetworkElementAsPlannedDtoUpdate, NetworkElementAsPlanned>()
                .ForMember(x => x.PlannedActivities, opt => opt.MapFrom(x => x.PlannedActivityDto.Select(x =>
                    new PlannedActivity
                    {
                        LcmEngineeringId = x.LcmEngineeringId,
                        DesignComponentId = x.DesignComponentId,
                        ActivityDetails = x.ActivityDetails,
                        DeliveryProjectName = x.DeliveryProjectName,
                        ActivityStatusId = x.ActivityStatusId,
                        PlannedActivityDescription = x.PlannedActivityDescription,
                        BenefitId = x.BenefitId,
                        IsNewServiceArchitecture = x.IsNewServiceArchitecture,
                        IsReplacementExistingSolution = x.IsReplacementExistingSolution,
                        RelatesToId = x.RelatesToId,
                        DriverId = x.DriverId,
                        BudgetAvailabilityId = x.BudgetAvailabilityId,
                        PlannedCompletion = x.PlannedCompletion,
                        OperationalRiskId = x.RiskOpeId,
                        EngineeringRiskId = x.RiskEngId,
                        DeliveryStatusId = x.DeliveryStatusId,
                        ResponsibilityPhaseId = x.ResponsibilityPhaseId,
                        PlannedActivityResourceId = x.PlannedActivityResourceId,
                        SpareFieldsJson = x.SpareFieldsJson,
                        PlanningActivityStatusId = x.PlanningActivityStatusId.Value,
                        LocalApproval = x.LocalApproval,
                        RiskEngineeringNotes = x.RiskEngineeringNotes,
                        BudgetTrackingId = x.BudgetTrackingId,
                        BudgetValue = x.BudgetValue,
                        Notes = x.Notes,
                        DeliveryProjectId = x.DeliveryProjectId,
                        PlannedActivityId = x.PlannedActivityId,
                        PlannedImplementationYear = x.PlannedImplementationYear ?? 0,
                        PlanningRiskId = x.PlanningRiskId,
                        RiskOperationalNotes = x.RiskOperationalNotes,
                        Currency = x.Currency,
                        OpCoId = x.OpCoId,
                        StartDate = x.StartDate,
                        DeliveryPlanAvailable = x.DeliveryPlanAvailable,
                        Buildbagid = x.BuildBagId,                       
                        Plannedactivitycategoryid = x.PlannedActivityCategoryId,
                        ProgramId = x.ProgramId == 0 ? null : x.ProgramId,
                        //Plannedactivitycategory = x.PlannedActivityCategory,
                        Plannedactivityteam = x.PlannedActivityTeam,
                        Priority = x.Priority,                        
                        Lcmcategories = x.LcmCategories != null && !x.LcmCategories.IsNullOrEmpty() ? Convert.ToInt16(x.LcmCategories) : default(short),
                        ProjectOwner = x.ProjectOwner,
                        PreBaseLineDate = x.PreBaseLineDate
                    }).ToList()))

                 .ForMember(dest => dest.Buildbagid, opt => opt.MapFrom(
                    src => src.BuildBagId
                ))
                 .ForMember(dest => dest.ElementDomianName, opt => opt.MapFrom(src => src.ElementDomianName))
                 .ForMember(dest => dest.AssetLiveStatusDate, opt => opt.MapFrom(src => src.AssetLiveStatusDate))
                 .ForMember(dest => dest.AssetDecommissionedDate, opt => opt.MapFrom(src => src.DateAssetDecommissionedAsset))
                  .ForMember(dest => dest.AssetRfoDate, opt => opt.MapFrom(src => src.AssetRfoDate))
                   .ForMember(dest => dest.AssetRfsDate, opt => opt.MapFrom(src => src.AssetRfsDate))

                .ReverseMap()
                .ForMember(x => x.NodeIndex, s => s.MapFrom(src => $"TEMS{src.NetworkElementAsPlannedId:000000}"))

                .ForMember(dest => dest.OriginalEquipmentManufacturerId, opt => opt.MapFrom(src =>
                src.DesignComponent.SystemType.MajorSoftwareBuilds.OriginalEquipmentManufacturerId))             

                .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.ModificationDate))
                //.ForMember(x => x.PlannedActivityDto, opt => opt.MapFrom(x => x.PlannedActivities
                //    .Where(x => x.Deleted == false).Select(x => new PlannedActivityDtoUpdate
                //    {
                //        LcmEngineeringId = x.LcmEngineeringId,
                //        DesignComponentId = x.DesignComponentId,
                //        ActivityDetails = x.ActivityDetails,
                //        DeliveryProjectName = x.DeliveryProjectName,
                //        ActivityStatusId = x.ActivityStatusId,
                //        PlannedActivityDescription = x.PlannedActivityDescription,
                //        BenefitId = x.BenefitId,
                //        RelatesToId = x.RelatesToId,
                //        DriverId = x.DriverId,
                //        BudgetAvailabilityId = x.BudgetAvailabilityId,
                //        PlannedCompletion = x.PlannedCompletion,
                //        RiskEngId = x.EngineeringRiskId,
                //        IsNewServiceArchitecture = x.IsNewServiceArchitecture,
                //        IsReplacementExistingSolution = x.IsReplacementExistingSolution,
                //        RiskOpeId = x.OperationalRiskId,
                //        DeliveryStatusId = x.DeliveryStatusId,
                //        ResponsibilityPhaseId = x.ResponsibilityPhaseId,
                //        SpareFieldsJson = x.SpareFieldsJson,
                //        PlanningActivityStatusId = x.PlanningActivityStatusId,
                //        LocalApproval = x.LocalApproval,
                //        RiskEngineeringNotes = x.RiskEngineeringNotes,
                //        BudgetTrackingId = x.BudgetTrackingId,
                //        BudgetValue = x.BudgetValue,
                //        Notes = x.Notes,
                //        DeliveryProjectId = x.DeliveryProjectId,
                //        PlannedActivityId = x.PlannedActivityId,
                //        PlannedImplementationYear = x.PlannedImplementationYear,
                //        PlanningRiskId = x.PlanningRiskId,
                //        RiskOperationalNotes = x.RiskOperationalNotes,
                //        PlannedActivityResourceId = x.PlannedActivityResourceId,
                //        LastModified = x.ModificationDate,
                //        Currency = x.Currency, 
                //        OpCoId = x.OpCoId,
                //        StartDate = x.StartDate,
                //        BuildBagId =   x.Buildbagid ,
                //        Priority = x.Priority,
                //        //PlannedActivityCategory = x.Plannedactivitycategory,
                //        PlannedActivityCategoryId = x.Plannedactivitycategoryid == null? default(short):x.Plannedactivitycategoryid.Value,
                //        ProgramId = x.ProgramId == null? default(long):x.ProgramId.Value,
                //        LcmCategories = x.Lcmcategories == null? string.Empty : x.Lcmcategories.ToString(),
                //        PlannedActivityTeam = x.Plannedactivityteam,
                //        ProjectOwner = x.ProjectOwner,
                //    }).ToList()))
                ;
#endregion

            #region Grid
            CreateMap<NetworkElementAsPlanned, NetworkElementAsPlannedDtoGrid>()
                .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
                .ForMember(x => x.NodeIndex, s => s.MapFrom(src => $"TEMS{src.NetworkElementAsPlannedId:000000}"))
                .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
                .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
                src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
               .ForMember(dest => dest.DesignComponent, opt => opt.MapFrom(src => CAM.Entities.Mappers.Entity.DesignComponentMapper.SetDesignComponentMapper(src.DesignComponent).toDesignComponentNameLcm(_repositoryWrapper)))
               .ForMember(dest => dest.DesignComponentIndex, opt => opt.MapFrom(src => src.DesignComponent.DesignComponentId))
               .ForMember(dest => dest.DesignComponentFamilyIndex, opt => opt.MapFrom(src => src.DesignComponentFamilyId))
                .ForMember(dest => dest.OpCo, opt => opt.MapFrom(x => x.OpCo.OpCoDescription))
                .ForMember(dest => dest.OpCoId, opt => opt.MapFrom(x => x.OpCoId))
              .ForMember(dest => dest.Environment,
                    opt => opt.MapFrom(x => x.Environment.EnvironmentDescription))
                .ForMember(dest => dest.DeploymentStatus,
                    opt => opt.MapFrom(x => x.DeploymentStatus.DeploymentStatusDescription))
                .ForMember(dest => dest.DeploymentType,
                    opt => opt.MapFrom(x => x.DeploymentType.DeploymentTypeDescription))
                .ForMember(dest => dest.Location,
                    opt => opt.MapFrom(x => x.Location != null ? x.Location.LocationDescription : "Unplanned"))
                .ForMember(dest => dest.NfviBundleID,
                    opt => opt.MapFrom(x => x.NFVIBundleID.NFVIBundleIdDescription))                 
                     .ForMember(dest => dest.PlannedActivity,
                    opt => opt.MapFrom(x => x.PlannedActivityDictonary))
 
                        .ForMember(dest => dest.VerticalName, opt => opt.MapFrom(x =>
                        ( x.VerticalFilterDto !=null && x.VerticalFilterDto.Count() >0 ) ?
                        string.Join(",", x.VerticalFilterDto.Select(m => m.Value).ToList() ??
                        new List<string>()): string.Empty))

    .ForMember(dest => dest.SubDomainSpoc, opt => opt.MapFrom(x =>
                _commonManager.GetEduAndSubDomainSpocUserEmail(x.NetworkElementAsPlannedSubDomainSpoc.Select(x => x.Subdomainspocid).ToList(),false,true)))

                .ForMember(dest => dest.Eduspoc, opt => opt.MapFrom(x =>
                _commonManager.GetEduAndSubDomainSpocUserEmail(x.NetworkElementEduSpocIdList.ToList(), true, false)))

                 .ForMember(dest => dest.LCMDeployementstatus, opt => opt.MapFrom(
                     src => (src.LcmEngineering != null) ? src.LcmEngineering.LCMDeploymentStatusValue  : string.Empty))
  .ForMember(x => x.BuildBagDescription, s => s.MapFrom(src => _commonManager.GetBuildBagDescriptionFromEnity(src.Buildbag))) 
  .ForMember(x=>x.Assured, s => s.MapFrom(src => src.IsAssured == true? ConstantValueFilter.Assured : ConstantValueFilter.NotAssured))
  .ForMember(x => x.ElementDomianName, s => s.MapFrom(src => src.ElementDomianName))
  .ForMember(x => x.AssetLiveStatusDate, s => s.MapFrom(src => src.AssetLiveStatusDate))
  .ForMember(x => x.DateAssetDecommissionedAsset, s => s.MapFrom(src => src.AssetDecommissionedDate))
                 .ForMember(x => x.AssetLiveStatusDateValue, s => s.MapFrom(src =>
                src.AssetLiveStatusDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(x => x.DateAssetDecommissionedAssetValue, s => s.MapFrom(src =>
                src.AssetDecommissionedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))

                  .ForMember(x => x.IsVirtualizedOrContanarized, s => s.MapFrom(src =>
                (bool) (src.DesignComponent.SystemType.SystemTypesMajorHardwareBuilds
                .Where(t => t.IsMain == true).Select(x => x.MajorHardware.BuildConstruction.IsCluodHostedAsset).FirstOrDefault() ?? false)
                
                ))

                 ;

            #endregion

            #region create Dto
            CreateMap<NetworkElementAsPlannedDtoCreate, NetworkElementAsPlanned>()
               .ForMember(dest => dest.PlannedActivities, opt => opt.MapFrom(src => src.PlannedActivityDto.Select(x =>
                    new PlannedActivity
                    {
                        LcmEngineeringId = x.LcmEngineeringId,
                        DesignComponentId = x.DesignComponentId,
                        ActivityDetails = x.ActivityDetails,
                        DeliveryProjectName = x.DeliveryProjectName,
                        ActivityStatusId = x.ActivityStatusId,
                        PlannedActivityDescription = x.PlannedActivityDescription,
                        BenefitId = x.BenefitId,
                        IsNewServiceArchitecture = x.IsNewServiceArchitecture,
                        IsReplacementExistingSolution = x.IsReplacementExistingSolution,
                        RelatesToId = x.RelatesToId,
                        DriverId = x.DriverId,
                        BudgetAvailabilityId = x.BudgetAvailabilityId,
                        PlannedCompletion = x.PlannedCompletion,
                        OperationalRiskId = x.RiskOpeId,
                        EngineeringRiskId = x.RiskEngId,
                        DeliveryStatusId = x.DeliveryStatusId,
                        ResponsibilityPhaseId = x.ResponsibilityPhaseId,
                        SpareFieldsJson = x.SpareFieldsJson,
                        PlanningActivityStatusId = x.PlanningActivityStatusId.Value,
                        LocalApproval = x.LocalApproval,
                        RiskEngineeringNotes = x.RiskEngineeringNotes,
                        BudgetTrackingId = x.BudgetTrackingId,
                        BudgetValue = x.BudgetValue,
                        Notes = x.Notes,
                        DeliveryProjectId = x.DeliveryProjectId,
                        PlannedActivityId = x.PlannedActivityId,
                        PlannedImplementationYear = x.PlannedImplementationYear ?? 0,
                        PlanningRiskId = x.PlanningRiskId,
                        RiskOperationalNotes = x.RiskOperationalNotes,
                        PlannedActivityResourceId = x.PlannedActivityResourceId,
                        Currency = x.Currency,                      
                        OpCoId = x.OpCoId,
                        StartDate = x.StartDate,
                        Buildbagid =  x.BuildBagId ,
                        Priority = x.Priority,
                        //Plannedactivitycategory = x.PlannedActivityCategory,
                        Plannedactivitycategoryid = x.PlannedActivityCategoryId == null? default(short):x.PlannedActivityCategoryId,
                        ProgramId = x.ProgramId == 0? default(long):x.ProgramId,
                        Lcmcategories = x.LcmCategories != null && !x.LcmCategories.IsNullOrEmpty() ? Convert.ToInt16(x.LcmCategories) : default(short),
                        Plannedactivityteam = x.PlannedActivityTeam,
                        ProjectOwner = x.ProjectOwner,
                        PreBaseLineDate = x.PreBaseLineDate
                    }).ToList()))
              
                .ForMember(dest => dest.NetworkElementAsPlannedSubDomainSpoc, opt => opt.MapFrom(
                    src => src.SubDomainSpocIds.Select(x => new NetworkElementAsPlannedSubDomainSpoc()
                    {
                        Subdomainspocid = x,
                    }).ToList()
                ))
                   .ForMember(dest => dest.NetworkElementAsPlannedEduSpoc, opt => opt.MapFrom(
                    src => src.EduSpocIds.Select(x => new NetworkElementAsPlannedEduSpoc()
                    {
                        Eduspocid = x,
                    }).ToList()
                ));
            #endregion

            #region nweAssociated
            CreateMap<NetworkElementAsPlanned, NetworkElementAssociated>()
                .ForMember(x => x.ElementName, s => s.MapFrom(x => x.ElementName))
                .ForMember(x => x.Enviroment, s => s.MapFrom(x => x.Environment.EnvironmentDescription))
                .ForMember(x => x.EnviromentId, s => s.MapFrom(x => x.EnvironmentId))
                .ForMember(x => x.Id, s => s.MapFrom(x => x.NetworkElementAsPlannedId))
                .ForMember(x => x.Location, s => s.MapFrom(x => x.Location.LocationDescription))
                .ForMember(x => x.AssetsStatusId, s => s.MapFrom(x => x.DeploymentStatus.DeploymentStatusId))
                .ForMember(x => x.LocationId, s => s.MapFrom(x => x.LocationId))
                .ForMember(x => x.AssetsStatus, s => s.MapFrom(x => x.DeploymentStatus.DeploymentStatusDescription));
            #endregion
        }


    }
}