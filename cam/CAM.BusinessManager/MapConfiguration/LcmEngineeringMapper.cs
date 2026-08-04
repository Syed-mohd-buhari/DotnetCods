using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.LcmEngineering;
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
    public class LcmEngineeringMapper : Profile
    {
        private const short MAX_CHAR_ACTIVITIES = 80;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly CommonManager _commonManger;


        public LcmEngineeringMapper(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, CommonManager commonManager)
        {
            _commonManger = commonManager;
            string email = contextAccessor.HttpContext.User.Identity is ClaimsIdentity authenticatedUser ? authenticatedUser?.FindFirst("email")?.Value : null;
            _repositoryWrapper = string.IsNullOrEmpty(email) ? wrappers.First() : (GlobalDbMode.DbMode.ContainsKey(email) ? ((GlobalDbMode.DbMode.Count != 0 && GlobalDbMode.DbMode[email] == "training") ? wrappers.Last() : wrappers.First()) : wrappers.First());


            _ = CreateMap<LcmEngineeringDtoUpdate, LcmEngineering>()
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
                        RelatesToId = x.RelatesToId,
                        DriverId = x.DriverId,
                        IsNewServiceArchitecture = x.IsNewServiceArchitecture,
                        IsReplacementExistingSolution = x.IsReplacementExistingSolution,
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
                        StartDate = x.StartDate,
                        OriginalLcmEngineeringId = x.OriginalLcmEngineeringId,
                        DeliveryPlanAvailable = x.DeliveryPlanAvailable,
                        ProjectOwner = x.ProjectOwner,
                        IsPAReleaseDetailUnknown = x.IsPAReleaseDetailUnknown,
                        Buildbagid = x.BuildBagId,
                        Plannedactivitycategoryid = x.PlannedActivityCategoryId,
                        ProgramId = x.ProgramId,
                        //Plannedactivitycategory = x.PlannedActivityCategory,
                        Plannedactivityteam = x.PlannedActivityTeam,
                        Priority = x.Priority,                        
                        Lcmcategories = x.LcmCategories != null && !x.LcmCategories.IsNullOrEmpty()? Convert.ToInt16(x.LcmCategories):default(short),
                        PreBaseLineDate = x.PreBaseLineDate
                    }).ToList()))
                .ForMember(x => x.CheckboxResourceLcmEngineeringHardwares, s => s.MapFrom(r =>
                    r.CheckboxResourceLcmEngineeringHardwares.Select(t =>
                        new ReasonCheckboxResourceLcmEngineeringHardware()
                        {
                            ReasonCheckboxResourceId = (short)t,
                        })))
                .ForMember(x => x.CheckboxResourceLcmEngineeringSoftwares, s => s.MapFrom(r =>
                    r.CheckboxResourceLcmEngineeringSoftwares.Select(t =>
                        new ReasonCheckboxResourceLcmEngineeringSoftware()
                        {
                            ReasonCheckboxResourceId = (short)t,
                        })))
                .ForMember(dest => dest.NumberOfNodes, opt => opt.MapFrom(x => x.CountNetworkElementReleated(false, _repositoryWrapper)))
                .ForMember(dest => dest.NumberOfNodesInLab, opt => opt.MapFrom(x => x.CountNetworkElementReleated(true, _repositoryWrapper)))
                .ForMember(dest => dest.LcmengineeringId, opt => opt.MapFrom(x => x.LcmEngineeringId))
                .ForMember(dest => dest.BuildBagId, opt => opt.MapFrom(x => x.BuildBagId))
                .ForMember(dest => dest.LcmEngineeringSubDomainSpoc, opt => opt.MapFrom(
                    src => src.SubDomainSpocIds.Select(x => new LcmEngineeringSubDomainSpoc
                    {
                        Subdomainspocid = x,
                        LcmengineeringId = src.LcmEngineeringId
                    }).ToList()))
                 .ForMember(dest => dest.LCMOperationContracts, opt => opt.MapFrom(
                    src => src.OperationContractsIds.Select(x => new LcmOperationalContracts
                    {
                        OperationalContractId = (short)x,
                        LcmId = src.LcmEngineeringId
                    }).ToList()))
                .ForMember(dest => dest.LcmEngineeringEduSpoc, opt => opt.MapFrom(
                    src => src.EduSpocIds.Select(x => new LcmEngineeringEduSpoc
                    {
                        Eduspocid = x,
                        LcmengineeringId = src.LcmEngineeringId
                    }).ToList()))

                .ReverseMap()
                .ForMember(dest => dest.NumberOfNodes, opt => opt.MapFrom(x => x.CountNetworkElementReleated(false, _repositoryWrapper)))
                .ForMember(dest => dest.NumberOfNodesInLab, opt => opt.MapFrom(x => x.CountNetworkElementReleated(true, _repositoryWrapper)))

                .ForMember(dest => dest.EduSpocIds, opt => opt.MapFrom(
                    src => src.LcmEngineeringEduSpoc.Where(x => !x.Deleted).Select(x => x.SubDomainSpocId)
                        .ToList()))

                 .ForMember(dest => dest.OperationContractsIds, opt => opt.MapFrom(
                    src => src.LCMOperationContracts.Where(x => !x.Deleted).Select(x => x.OperationalContractId)
                        .ToList()))
                .ForMember(dest => dest.CheckboxResourceLcmEngineeringHardwares, opt => opt.MapFrom(
                    src => src.CheckboxResourceLcmEngineeringHardwares.Select(x => x.ReasonCheckboxResourceId)
                        .ToList()))
                .ForMember(dest => dest.CheckboxResourceLcmEngineeringSoftwares, opt => opt.MapFrom(
                    src => src.CheckboxResourceLcmEngineeringSoftwares.Select(x => x.ReasonCheckboxResourceId)
                        .ToList()))

                .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.ModificationUserEntity.Email))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.ModificationDate))
                .ForMember(dest => dest.LcmEngineeringId, opt => opt.MapFrom(x => x.LcmengineeringId))
                .ForMember(x => x.PlannedActivityDto, opt => opt.MapFrom(x => x.PlannedActivities
                    .Where(x => x.Deleted == false).Select(x => new PlannedActivityDtoUpdate
                    {
                        LcmEngineeringId = x.LcmEngineeringId,
                        DesignComponentId = x.DesignComponentId,
                        ActivityDetails = x.ActivityDetails,
                        DeliveryProjectName = x.DeliveryProjectName,
                        ActivityStatusId = x.ActivityStatusId,
                        PlannedActivityDescription = x.PlannedActivityDescription,
                        BenefitId = x.BenefitId,
                        RelatesToId = x.RelatesToId,
                        DriverId = x.DriverId,
                        BudgetAvailabilityId = x.BudgetAvailabilityId,
                        PlannedCompletion = x.PlannedCompletion,
                        RiskEngId = x.EngineeringRiskId,
                        IsNewServiceArchitecture = x.IsNewServiceArchitecture,
                        IsReplacementExistingSolution = x.IsReplacementExistingSolution,
                        RiskOpeId = x.OperationalRiskId,
                        DeliveryStatusId = x.DeliveryStatusId,
                        ResponsibilityPhaseId = x.ResponsibilityPhaseId,
                        SpareFieldsJson = x.SpareFieldsJson,
                        PlanningActivityStatusId = x.PlanningActivityStatusId,
                        LocalApproval = x.LocalApproval,
                        RiskEngineeringNotes = x.RiskEngineeringNotes,
                        BudgetTrackingId = x.BudgetTrackingId,
                        BudgetValue = x.BudgetValue,
                        Notes = x.Notes,
                        DeliveryProjectId = x.DeliveryProjectId,
                        PlannedActivityId = x.PlannedActivityId,
                        PlannedImplementationYear = x.PlannedImplementationYear,
                        PlanningRiskId = x.PlanningRiskId,
                        RiskOperationalNotes = x.RiskOperationalNotes,
                        PlannedActivityResourceId = x.PlannedActivityResourceId,
                        LastModified = x.ModificationDate,
                        Currency = x.Currency,
                        StartDate = x.StartDate,
                        ProjectOwner = x.ProjectOwner,
                        IsPAReleaseDetailUnknown = x.IsPAReleaseDetailUnknown,
                        BuildBagId = x.Buildbagid,
                        Priority = x.Priority,
                        //PlannedActivityCategory = x.PlannedactivitycategoryNavigation.Categorydescription,
                        PlannedActivityCategoryId = x.Plannedactivitycategoryid == null ? default(short):x.Plannedactivitycategoryid.Value,
                        ProgramId = x.ProgramId == null ? default(long):x.ProgramId.Value,
                        LcmCategories = x.Lcmcategories == null? string.Empty: x.Lcmcategories.ToString(),
                        PlannedActivityTeam = x.Plannedactivityteam,
                        PreBaseLineDate = x.PreBaseLineDate

                    }).ToList()))
                ;

            _ = CreateMap<LcmEngineering, LcmEngineeringDtoGrid>()
               .ForMember(x => x.Orphan, s => s.MapFrom(src => src.NumberOfNodes == 0))
               .ForMember(x => x.Deleted, s => s.MapFrom(src => src.Deleted))
               .ForMember(x => x.LastModifiedBy, s => s.MapFrom(src => src.ModificationUserEntity.Email))
               .ForMember(x => x.LastModified, s => s.MapFrom(src => src.ModificationDate))
               .ForMember(x => x.LastModifiedValue, s => s.MapFrom(src =>
               src.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
               .ForMember(x => x.ProductImportanceId, s => s.MapFrom(src => src.ProductImportanceRel.ProductImportanceDescription))
               .ForMember(x => x.Warranty, s => s.MapFrom(src => src.Warranty))
               .ForMember(x => x.Archived, s => s.MapFrom(src => src.Archived))
               .ForMember(x => x.LcmDeploymentStatus, s => s.MapFrom(src => src.LCMDeploymentStatus != null ? src.LCMDeploymentStatus.Description : ""))
               .ForMember(dest => dest.DesignComponent, opt => opt.MapFrom(src => CAM.Entities.Mappers.Entity.DesignComponentMapper.SetDesignComponentMapper(src.DesignComponent).toDesignComponentNameLcm(_repositoryWrapper)))
               .ForMember(dest => dest.OpCo, opt => opt.MapFrom(x => x.OpCo.OpCoDescription))
               .ForMember(dest => dest.NumberOfNodes, opt => opt.MapFrom(x => x.CountNetworkElementReleated(false, _repositoryWrapper)))
               .ForMember(dest => dest.NumberOfNodesInLab, opt => opt.MapFrom(x => x.CountNetworkElementReleated(true, _repositoryWrapper)))
               .ForMember(dest => dest.DesignComponentFamilyId, opt => opt.MapFrom(src => src.DesignComponentFamilyId))
               .ForMember(dest => dest.LcmEngineeringId, opt => opt.MapFrom(src => src.LcmengineeringId))
               .ForMember(dest => dest.SubDomainSpoc, opt => opt.MapFrom(x =>
               _commonManger.GetEduAndSubDomainSpocUserEmail(x.LcmEngineeringSubDomainSpoc.Select(x => x.Subdomainspocid).ToList(), false, true)))

               .ForMember(dest => dest.Eduspoc, opt => opt.MapFrom(x =>
                 _commonManger.GetEduAndSubDomainSpocUserEmail(x.LcmEngineeringEduSpoc.Select(x => x.Eduspocid).ToList(), true, false)))

                 .ForMember(
                   dest => dest.OperationalContact,
                   opt => opt.MapFrom(x => string.Join(" | ", x.LCMOperationContracts.Select(fx => fx.OperationalContract.Description).Distinct()))
                   )
               .ForMember(dest => dest.PlannedActivity,
                   opt => opt.MapFrom(x =>
                       x.PlannedActivities.Where(x => !x.Deleted).ToDictionary(x => x.PlannedActivityId,
                           x => x.GetPlannedAction(_repositoryWrapper)))
                   )
                .ForMember(dest => dest.VerticalName, opt => opt.MapFrom(x => _commonManger.GetVerticaleNameRes(x.LcmEngineeringSubDomainSpoc.
                Select(x => x.Subdomainspocid).ToList(), x.OpCoId, false, true)))

               .ForMember(dest => dest.OriginalLcm,
                   opt => opt.MapFrom(x =>
                     x.PlannedActivities.Where(x => !x.Deleted && x.OriginalLcmEngineeringId.HasValue && x.OriginalLcmEngineeringId != 0).Select(x =>
                     new
                     {
                         x.OriginalLcmEngineeringId,
                         DC = x.Originallcmengineering.DesignComponent.toDesignComponentNameLcm(_repositoryWrapper)
                     }).Distinct()
                     .ToDictionary(x => x.OriginalLcmEngineeringId.Value, x => x.DC)))
               .ForMember(des => des.HwIsExtendedSupportOfferedByVendor, opt => opt.MapFrom(src => src.HwIsExtendedSupportOfferedByVendor == true))
               .ForMember(des => des.IsLcmAncillaryData, opt => opt.MapFrom(src => src.LcmAncillaryData.Count > 0))
               .ForMember(x => x.ReasonForNoPlan, s => s.MapFrom(src => src.PlannedActivities.GetAncillaryDataForNoPa(_repositoryWrapper, true)))
               .ForMember(x => x.CommentOnProjectStatus, s => s.MapFrom(src => src.PlannedActivities.GetAncillaryDataForNoPa(_repositoryWrapper, false)))
               .ForMember(x => x.BuildBagDescription, s => s.MapFrom(src => _commonManger.GetBuildBagDescriptionFromEnity(src.BuildBag)))
               .ForMember(dest => dest.BuildBagId, opt => opt.MapFrom(src => src.BuildBagId))
               ;

            _ = CreateMap<LcmEngineeringDtoCreate, LcmEngineering>()
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
                        RelatesToId = x.RelatesToId,
                        DriverId = x.DriverId,
                        IsNewServiceArchitecture = x.IsNewServiceArchitecture,
                        IsReplacementExistingSolution = x.IsReplacementExistingSolution,
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
                        StartDate = x.StartDate,
                        ProjectOwner = x.ProjectOwner,
                        IsPAReleaseDetailUnknown = x.IsPAReleaseDetailUnknown,
                        Buildbagid = x.BuildBagId,
                        Priority = x.Priority,
                        //Plannedactivitycategory = x.PlannedActivityCategory,
                        Plannedactivitycategoryid = x.PlannedActivityCategoryId == null ? default(short) : x.PlannedActivityCategoryId,
                        ProgramId = x.ProgramId == null ? default(long) : x.ProgramId,
                        Lcmcategories = x.LcmCategories != null && !x.LcmCategories.IsNullOrEmpty() ? Convert.ToInt16(x.LcmCategories) : default(short),
                        Plannedactivityteam = x.PlannedActivityTeam,
                        PreBaseLineDate = x.PreBaseLineDate
                    }).ToList()))
                .ForMember(x => x.CheckboxResourceLcmEngineeringHardwares, s => s.MapFrom(r => r.CheckboxResourceLcmEngineeringHardwares.Select(t => new ReasonCheckboxResourceLcmEngineeringHardware()
                {
                    ReasonCheckboxResourceId = (short)t,

                })))
                .ForMember(x => x.CheckboxResourceLcmEngineeringSoftwares, s => s.MapFrom(r => r.CheckboxResourceLcmEngineeringSoftwares.Select(t => new ReasonCheckboxResourceLcmEngineeringSoftware()
                {

                    ReasonCheckboxResourceId = (short)t,

                })))
                .ForMember(dest => dest.LcmEngineeringSubDomainSpoc, opt => opt.MapFrom(
                    src => src.SubDomainSpocIds.Select(x => new LcmEngineeringSubDomainSpoc()
                    {
                        Subdomainspocid = x,
                    }).ToList()
                ))

                .ForMember(dest => dest.LCMOperationContracts, opt => opt.MapFrom(
                    src => src.OperationContractsIds.Select(x => new LcmOperationalContracts()
                    {
                        OperationalContractId = (short)x,
                    }).ToList()
                ))
                .ForMember(dest => dest.BuildBagId, opt => opt.MapFrom(
                    src =>  src.BuildBagId
                ))
                .ForMember(dest => dest.LCMDeploymentStatusId, opt => opt.MapFrom(
                    src => src.LCMDeploymentStatusId))
                   .ForMember(dest => dest.LcmEngineeringEduSpoc, opt => opt.MapFrom(
                    src => src.EduSpocIds.Select(x => new LcmEngineeringEduSpoc()
                    {
                        Eduspocid = (short)x,
                    }).ToList()
                ));

        }
    }
}