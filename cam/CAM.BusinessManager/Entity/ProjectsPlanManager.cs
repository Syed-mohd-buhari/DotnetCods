using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ProjectPlan;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class ProjectsPlanManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        private readonly CommonManager _commonManager;
        private readonly ILoggerManager _logger;
        private readonly LookUp.DeliveryTrackingManager _deliveryTrackingManager;
        public static readonly Dictionary<int?, string> MilestoneStatusMap = new Dictionary<int?, string>
                                        {
                                            { 1, "MS1" },
                                            { 2, "MS2" },
                                            { 3, "MS3" },
                                            { 4, "MS4" }
                                        };
        public ProjectsPlanManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, ILoggerManager logger, CommonManager commonManager, LookUp.DeliveryTrackingManager deliveryTrackingManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _logger = logger;
            _commonManager = commonManager;
            _deliveryTrackingManager = deliveryTrackingManager;
        }


        #region UiMemberFunctions

        public async Task<ResultDto> FindWithCondition(long paId)
        {
            var predicateResult = PredicateBuilder.New<Projectsplan>(true);
            var predicateInner = PredicateBuilder.New<Projectsplan>(true);
            try
            {
                if (paId > 0)
                {
                    predicateInner.Or(x => x.Plannedactivityid == paId && x.Plannedactivity.Archived == false);
                    predicateResult.And(predicateInner);
                }

                var projectPlanEntities = await Task.Run(() => GetQuery(predicateResult, false).ToList());

                if (projectPlanEntities != null)
                {
                    var exportProjectPlanEntity = projectPlanEntities.Where(ms => ms.Settingsupdateplannedactivity.Milestonestatus > 0).OrderByDescending(x => x.Settingsupdateplannedactivity.Milestonestatus).Select(t => new ProjectPlanExportDto
                    {
                        PaId = t.Plannedactivityid,
                        ActivityDetail = t.Plannedactivity.GetActvityString(_repositoryWrapper),
                        EduSpoc = GetEduSpocFromPA(t.Plannedactivityid).Result,
                        ColorCoding = GetColorCodeBasedModificationDate(t.Plannedactivityid).Result,
                        #region MileStone 
                        MilestoneId = Convert.ToInt16(t.Settingsupdateplannedactivity.Milestonestatus),
                        MilestoneName = MilestoneStatusMap.ContainsKey(Convert.ToInt16(t.Settingsupdateplannedactivity.Milestonestatus))
                                                                    ? MilestoneStatusMap[Convert.ToInt16(t.Settingsupdateplannedactivity.Milestonestatus)]
                                                                    : "Unknown",
                        MilestoneDuration = t.Settingsupdateplannedactivity.Milestonestatusduration,
                        #endregion

                        #region Delivery status details
                        ProjectsPlanId = t.Projectsplanid,
                        DeliveryStatusId = t.Settingsupdateplannedactivity.Deliverystatusid,
                        DeliveryStatusText = t.Settingsupdateplannedactivity.Deliverystatus?.Deliverystatus,
                        PlanningStartDate = t.Planningstartdate,
                        PlanningEndDate = t.Planningenddate,
                        Progress = t.Progress,
                        PlanDescription = t.Description,
                        order = t.Settingsupdateplannedactivity.Order,
                        IsMileStone = t.Settingsupdateplannedactivity.Ismilestone == ConstantValueFilter.isTrue ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                        BaseLineStartDate = t.Baselinestartdate,
                        BaseLineEndDate = t.Baselineenddate                      
                        #endregion


                    }).ToList();

                    var result = exportProjectPlanEntity
                        .GroupBy(x => new { x.PaId })
                        .Select(group => new ProjectPlanDto
                        {
                            PaId = group.Key.PaId,
                            ActivityDetail = group.FirstOrDefault().ActivityDetail,
                            EduSpoc = group.FirstOrDefault().EduSpoc,
                            ColorCoding = group.FirstOrDefault().ColorCoding,
                            Milestones = group
                                .GroupBy(x => new { x.MilestoneId })//  group milestones
                                .Select(mg => new MilestoneDto
                                {
                                    MilestoneId = mg.Key.MilestoneId,
                                    MilestoneName = mg.FirstOrDefault().MilestoneName,
                                    MilestoneDuration = mg.FirstOrDefault().MilestoneDuration,
                                    Statuses = mg.OrderBy(x=>x.order).Select(item => new PAandDeliveryStatuses
                                    {
                                        ProjectPlanId = item.ProjectsPlanId,
                                        DeliveryStatusId = item.DeliveryStatusId,
                                        DeliveryStatusText = item.DeliveryStatusText,
                                        PlanningStartDate = item.PlanningStartDate,
                                        PlanningEndDate = item.PlanningEndDate,
                                        Progress = item.Progress,
                                        Description = item.PlanDescription,
                                        IsMileStone = item.IsMileStone,
                                        BaseLineStartDate = item.BaseLineStartDate,
                                        BaseLineEndDate = item.BaseLineEndDate
                                    }).ToList()
                                }).ToList()
                        })
                        .ToList();


                    return new ResultDto
                    {
                        Data = exportProjectPlanEntity.Count() > 0 ? result : new List<ProjectPlanDto>(),
                        Info = exportProjectPlanEntity.Count() > 0 ? ResultMessages.GetInfoSuccess : ResultMessages.GetInfoNoFound,
                        Warning = exportProjectPlanEntity.Count() > 0 ? false : true,
                    };
                }
                else
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.GetInfoNoFound
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto
                {
                    Data = $"{ex.Message}-{ex.StackTrace}",
                    Warning = true,
                    Info = ResultMessages.GetInfoNoFound

                };
            }
        }


        public IQueryable<Projectsplan> GetQuery(ExpressionStarter<Projectsplan> predicateResult, bool includeDeleted)
        {
            var projectPlanEntities =
                predicateResult.IsStarted
                ?
                _repositoryWrapper.ProjectPlanRepository.FindByCondition(predicateResult, includeDeleted)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Plannedactivityresource).ThenInclude(x => x.SettingsupdateplannedactivityPlannedactivityresource)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Opco)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                        .Include(x => x.Settingsupdateplannedactivity).ThenInclude(x => x.Deliverystatus)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily)
                        .AsQueryable()
                : _repositoryWrapper.ProjectPlanRepository.FindAll()
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Plannedactivityresource).ThenInclude(x => x.SettingsupdateplannedactivityPlannedactivityresource)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Opco)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                        .Include(x => x.Settingsupdateplannedactivity).ThenInclude(x => x.Deliverystatus)
                        .Include(x => x.Plannedactivity).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily)
                        .AsQueryable();

            return projectPlanEntities;

        }

        #endregion

        #region CRUD
        public async Task<ResultDto> UpdateProjectPlan(ProjectPlanDto projectPlanDto)
        {
            try
            {
                var projectPlanEnityExits = _repositoryWrapper.ProjectPlanRepository.FindByCondition(x => x.Plannedactivityid == projectPlanDto.PaId).Include(x => x.Plannedactivity).ToList();
                if (projectPlanEnityExits != null)
                {
                    foreach (var mileStones in projectPlanDto.Milestones)
                    {
                        foreach (var status in mileStones.Statuses)
                        {
                                var projectPlanEntity = projectPlanEnityExits.FirstOrDefault(p => p.Projectsplanid == status.ProjectPlanId);
                                  
                                 if(projectPlanEntity.Planningenddate != status.PlanningEndDate)
                                     await _commonManager.UpdateProjectPlanDateForLastDeliveryStatusOfMS(projectPlanEntity.Plannedactivity, status.PlanningEndDate, (int)mileStones.MilestoneId, _repositoryWrapper,true, projectPlanEntity.Settingsupdateplannedactivityid);

                                if (projectPlanEntity != null)
                                {
                                    projectPlanEntity.Planningenddate = status.PlanningEndDate;
                                    projectPlanEntity.Planningstartdate = status.PlanningStartDate;
                                    projectPlanEntity.Progress = status.Progress;
                                    projectPlanEntity.Description = status.Description;

                                    _repositoryWrapper.ProjectPlanRepository.Update(projectPlanEntity);
                                }
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();                           
                        }
                    }
                }

                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = string.Join(",", projectPlanEnityExits.Select(x => x.Plannedactivityid).ToList())

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto()
                {
                    Data = ex.Message,
                    Info = ResultMessages.EntryUpdateExists,
                    Warning = true
                };
            }
        }
        #endregion

        #region BulkCreateorUpdate
        public IQueryable<Plannedactivities> GetBulkQuery()
        {
            var plannedEntities = _repositoryWrapper.PlannedActivity.FindAll()
            .Include(x => x.Plannedactivityresource).ThenInclude(x => x.SettingsupdateplannedactivityPlannedactivityresource).ThenInclude(x => x.Deliverystatus)
            .Include(x => x.Opco)
            .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
            .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
            .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
            .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
            .AsQueryable();

            return plannedEntities;
        }



        public async Task<ResultDto> BulkCreation()
        {
            var projectPlanCreationEnity = new List<Projectsplan>();
            var projectPlanUpdateEnity = new List<Projectsplan>();

            try
            {
                var paRelatedData = await Task.Run(() => GetBulkQuery());

                if (paRelatedData != null)
                {
                    foreach (var result in paRelatedData.ToList())
                    {

                        var ppcMappedEntities = await _commonManager.AddProjectPlan(result, _repositoryWrapper);

                        foreach (var calculatePpEntity in ppcMappedEntities)
                        {
                            var projectPlanExist = _repositoryWrapper.ProjectPlanRepository.FindByCondition(x => x.Plannedactivityid == calculatePpEntity.Plannedactivityid && x.Projectsplanid == calculatePpEntity.Projectsplanid).FirstOrDefault();

                            if (projectPlanExist != null)
                            {
                                var isUpdated = await Task.Run(() => ProjectPlanUpdate(calculatePpEntity, projectPlanExist));
                                projectPlanUpdateEnity.Add(isUpdated);
                            }
                            else
                            {
                                projectPlanCreationEnity.Add(calculatePpEntity);
                            }
                        }

                    }
                }

                if (projectPlanUpdateEnity != null && projectPlanUpdateEnity.Count > 0)
                {
                    _repositoryWrapper.ProjectPlanRepository.BulkUpdate(projectPlanUpdateEnity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                else
                {
                    if (projectPlanCreationEnity.Count > 0)
                    {
                        _repositoryWrapper.ProjectPlanRepository.BulkCreate(projectPlanCreationEnity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }

               //var deliveryTrackingDataRefresh = _deliveryTrackingManager.DeliveryTrackingDataRefresh();
 
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = string.Join(",", projectPlanCreationEnity.Select(x => x.Projectsplanid).ToList(), projectPlanUpdateEnity.Select(x => x.Projectsplanid).ToList())
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddUpdateFailed,
                    Data = ex.StackTrace
                };

            }
        }

        public Projectsplan ProjectPlanUpdate(Projectsplan CalculateEntity, Projectsplan existingEntity)
        {
            if ((CalculateEntity != null) && (existingEntity != null))
            {
                existingEntity.Settingsupdateplannedactivityid = CalculateEntity.Settingsupdateplannedactivityid;
                existingEntity.Planningstartdate = CalculateEntity.Planningstartdate;
                existingEntity.Planningenddate = CalculateEntity.Planningenddate;
                existingEntity.Baselinestartdate = CalculateEntity.Baselinestartdate;
                existingEntity.Baselineenddate = CalculateEntity?.Baselineenddate;
                return existingEntity;
            }
            else
            {
                return new Projectsplan();
            }
        }

        #endregion

        
        #region //Export
        public async Task<QueryResultDto<ProjectPlanExportDto>> FindWithConditionForExport(List<long> paId)
        {
            var predicateResult = PredicateBuilder.New<Projectsplan>(true);
            var predicateInner = PredicateBuilder.New<Projectsplan>(true);
            var exportProjectPlanEntity = new List<ProjectPlanExportDto>();
            var projectPlansSheetRender = new QueryResultDto<ProjectPlanExportDto>(new GenerateRenderForGrid<ProjectPlanExportDto>(_manager))
            {

            };
            try
            {
                if (paId.Count > 0)
                {
                    foreach (var id in paId)
                    {
                        predicateInner.Or(x => x.Plannedactivityid == id && x.Plannedactivity.Archived == false);
                    }
                    predicateResult.And(predicateInner);
                }
               
                var projectPlanEntities = await Task.Run(() => GetQuery(predicateResult, false).ToList());

                if (projectPlanEntities != null)
                {
                    exportProjectPlanEntity = projectPlanEntities.Where(ms => ms.Settingsupdateplannedactivity.Milestonestatus > 0).OrderByDescending(x => x.Settingsupdateplannedactivity.Milestonestatus).Select(t => new ProjectPlanExportDto
                    {
                        ProjectsPlanId = t.Projectsplanid,
                        PaId = t.Plannedactivityid,
                        ActivityDetail = $"{t.Plannedactivity.Opco.Opco}-{t.Plannedactivity.Designcomponent.toDesignComponentFamily()}-{t.Plannedactivity.Plannedactivityresource.Plannedactivityresource}",
                        EduSpoc = GetEduSpocFromPA(t.Plannedactivityid).Result,
                        #region MileStone 
                        MilestoneId = Convert.ToInt16(t.Settingsupdateplannedactivity.Milestonestatus),
                        MilestoneName = MilestoneStatusMap.ContainsKey(Convert.ToInt16(t.Settingsupdateplannedactivity.Milestonestatus))
                                                                    ? MilestoneStatusMap[Convert.ToInt16(t.Settingsupdateplannedactivity.Milestonestatus)]
                                                                    : "Unknown",
                        MilestoneDuration = t.Settingsupdateplannedactivity.Milestonestatusduration,
                        #endregion

                        #region Delivery status details
                        DeliveryStatusId = t.Settingsupdateplannedactivity.Deliverystatusid,
                        DeliveryStatusText = t.Settingsupdateplannedactivity.Deliverystatus?.Deliverystatus,
                        PlanningStartDate = t.Planningstartdate,
                        PlanningEndDate = t.Planningenddate,
                        Progress = t.Progress,
                        PlanDescription = t.Description,
                        order = t.Settingsupdateplannedactivity.Order,
                        BaseLineEndDate = t.Baselineenddate,
                        BaseLineStartDate = t.Baselinestartdate
                        #endregion
                        

                    }).ToList();//.OrderBy(x => new { x.MilestoneId, x.DeliveryStatusId });

                    projectPlansSheetRender.Items = exportProjectPlanEntity.OrderBy(x => x.PaId).ThenBy(y => y.order).ToList();
                }
                return projectPlansSheetRender;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return projectPlansSheetRender;
            }
        }
        #endregion


        public async Task<string> GetEduSpocFromPA(long paId)
        {
            string eduSpoc = string.Empty;
            if (paId == 0) return eduSpoc;

            var plannedActivities = await _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == paId)
                                            .Include(x => x.Designaspect).FirstOrDefaultAsync();

            if(plannedActivities != null)
            {
                if(plannedActivities.Lcmengineeringid != null)
                {
                    var lcmEduSpoc = _repositoryWrapper.LcmEngineeringEduSpoc.FindByCondition(x => x.Lcmengineeringid == plannedActivities.Lcmengineeringid 
                                    && x.Deleted == false).Include(x => x.Eduspoc).AsQueryable().Select(x => x.Eduspoc.Email).ToList();
                    eduSpoc = string.Join(',', lcmEduSpoc);
                }
                else if (plannedActivities.Networkelementasplannedid != null)
                {
                    var assetEduSpoc = _repositoryWrapper.NetworkElementAsPlannedEduSpoc.FindByCondition(x => x.Networkelementasplannedid == plannedActivities.Networkelementasplannedid
                                    && x.Deleted == false).Include(x => x.Eduspoc).AsQueryable().Select(x => x.Eduspoc.Email).ToList();
                    eduSpoc = string.Join(',', assetEduSpoc);
                }
                else if (plannedActivities.Designaspectid != null && plannedActivities.Designaspect != null)
                {

                    var dc = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == plannedActivities.Designaspect.Designcomponentfamilyid).Select(x => x.Designcomponentid).ToListAsync();
                    var lcm =  await _repositoryWrapper.Lcmengineering.FindByCondition(x => (dc.Any(d => d == x.Designcomponentid)) && (x.Opcoid == plannedActivities.Opcoid)).Select(x => x.Lcmengineeringid).ToListAsync();
                    var lcmEduSpoc = _repositoryWrapper.LcmEngineeringEduSpoc.FindByCondition(x => lcm.Contains(x.Lcmengineeringid)
                                    && x.Deleted == false).Include(x => x.Eduspoc).AsQueryable().Select(x => x.Eduspoc.Email).ToList();
                    eduSpoc = string.Join(',', lcmEduSpoc);

                }
            }
            return eduSpoc;
        }

        public async Task<string> GetColorCodeBasedModificationDate(long paId)
        {
            var colorCoding = ConstantValueFilter.Red;
            if (paId == 0) return colorCoding;
            var projectPlanEntity = await _repositoryWrapper.ProjectPlanRepository.FindByCondition(x => x.Plannedactivityid == paId).Include(x => x.Projectplanaudit).ToListAsync();

            if (projectPlanEntity != null && projectPlanEntity.Count > 0)
            {
                var pPAuditModificationDate = projectPlanEntity.OrderByDescending(x => x.Modificationdate).FirstOrDefault();
                if (pPAuditModificationDate != null)
                {
                    DateTime now = DateTime.Now;
                    var lastModifiedDate = pPAuditModificationDate.Projectplanaudit.OrderByDescending(x => x.Modificationdate).Select(x => x.Modificationdate).FirstOrDefault();
                    if (now.AddMonths(-1) < lastModifiedDate)
                    {
                        colorCoding = ConstantValueFilter.Green;
                    }
                    else if ((now.AddMonths(-2) <= lastModifiedDate) && (lastModifiedDate >= now.AddMonths(-1)))
                    {
                        colorCoding = ConstantValueFilter.Amber;
                    }
                    else
                    {
                        colorCoding = ConstantValueFilter.Red;
                    }
                }
            }
            return colorCoding;
        }


    }
}
