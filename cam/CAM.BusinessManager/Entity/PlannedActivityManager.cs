using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity.ClusterLevelPA;
using CAM.BusinessManager.Entity.ExodusProgram;
using CAM.BusinessManager.Entity.PlannedActivities;
using CAM.BusinessManager.ExtensionMethod.ComponentBag;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.PlannedActivityResource;
using CAM.BusinessManager.ExtensionMethod.Settings;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.LookUp;
using CAM.BusinessManager.Settings;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ClusterLevelPA;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.Entita.DaMigrationStatus;
using CAM.DataTransferObjects.Entita.GenericReportDto;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.DeliveryTracking;
using CAM.DataTransferObjects.LookUp.PlannedActivityResourceDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Bibliography;
using IdentityServer4.Extensions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CAM.BusinessManager.Rules.LCMEngineeringRulesExtension;

namespace CAM.BusinessManager.Entity
{
    public class PlannedActivityManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly NetworkElementNodeCountManager.NetworkElementNodeCountManager _networkElementsAsPlannedManager;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly DeliveryTrackingManager _deliveryTrackingManager;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly DaMigrationStatusManager _daMigrationStatusManager;
        private readonly DesignAspectPlannedActivityManger _designAspectPlannedActivityManger;
        private readonly PlatformMigrationManager _platformMigrationManager;
        private readonly Lazy<UpdatePlannedActivityManager> _updatePlannedActivityManager;
        private readonly ILoggerManager _logger;
        private readonly Lazy<InfraClusterAsPlannedManager> _infraClusterManager;
        public
            PlannedActivityManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager,
            NetworkElementNodeCountManager.NetworkElementNodeCountManager networkElementsAsPlannedManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, DeliveryTrackingManager deliveryTrackingManager
           , CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager, DaMigrationStatusManager daMigrationStatusManager
            , DesignAspectPlannedActivityManger designAspectPlannedActivityManger, PlatformMigrationManager platformMigrationManager
            , Lazy<UpdatePlannedActivityManager> updatePlannedActivityManager, Lazy<InfraClusterAsPlannedManager> infraClusterManager
            , ILoggerManager logger
            ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
            _networkElementsAsPlannedManager = networkElementsAsPlannedManager;
            _deliveryTrackingManager = deliveryTrackingManager;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _daMigrationStatusManager = daMigrationStatusManager;
            _designAspectPlannedActivityManger = designAspectPlannedActivityManger;
            _platformMigrationManager = platformMigrationManager;
            _updatePlannedActivityManager = updatePlannedActivityManager;
            _infraClusterManager = infraClusterManager;
            _logger = logger;
        }

        public async Task<ResultDto> Add(PlannedActivityDtoCreate dto, bool isServicePlanPa = false)
        {
            var entityExists = new Plannedactivities();
            try
            {
                if(isServicePlanPa == true)
                {
                    entityExists = await _repositoryWrapper.PlannedActivity.FindByCondition(
                    x => x.Opcoid == dto.OpCoId
                    && x.Plannedactivityresourceid == dto.PlannedActivityResourceId
                    && x.Archived != true
                    && x.Deliveryprojectname == dto.DeliveryProjectName)
                     .FirstOrDefaultAsync();
                }
                else
                {
                    entityExists = await _repositoryWrapper.PlannedActivity.FindByCondition(
                                        x => x.Opcoid == dto.OpCoId
                                        && x.Designcomponentid == dto.DesignComponentId
                                        && x.Plannedactivityresourceid == dto.PlannedActivityResourceId && x.Archived != true)
                                         .FirstOrDefaultAsync();
                }


                if (entityExists != null)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryUpdateExists,
                        Data = entityExists.Plannedactivityid
                    };
                }

                var entity = _mapper.Map<PlannedActivity>(dto);
                var model = SetPlannedActivityValue(PlannedActivityMapper.Set(entity));
                model.Archived = false;
                _repositoryWrapper.PlannedActivity.Create(model);
                await _repositoryWrapper.SaveAsync();
                if (dto.DeliveryPlanAvailable)
                {
                    var mileStoneStatus = await _commonManager.CalculateMSDuration(model, _repositoryWrapper);
                    if (mileStoneStatus.isSuccess)
                    {
                        // We should pass inservice and planned asset count only 
                        var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                        var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;

                        var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, model, 0);
                        await _deliveryTrackingManager.Add(deliveryTracking);
                    }

                }
                var addProjectPlanEntity = await _commonManager.AddProjectPlan(model, _repositoryWrapper);

                foreach (var plan in addProjectPlanEntity)
                {
                    _repositoryWrapper.ProjectPlanRepository.Create(plan);
                }
                _repositoryWrapper.Save();
                await _repositoryWrapper.ClearTracker();


                await _repositoryWrapper.SaveAsync();

                #region ClusterLevel LCM PA
                if (dto.infraClusterClusterUpgradeUpsertDto?.infraClusterAsPlannedDtoGrid != null && dto.infraClusterClusterUpgradeUpsertDto?.infraClusterAsPlannedDtoGrid.Count() > 0)
                {
                var clusterLevelPA = await _infraClusterManager.Value.AddOrUpdateInfraClusterAsync(dto.infraClusterClusterUpgradeUpsertDto, entity.PlannedActivityId,false,entity.PlannedActivityResource.RuleLinkedDc);
                }
                #endregion


                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = model.Plannedactivityid };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto { Info = ResultMessages.EntryNotAdd, Warning = true};

            }
        }

        public async Task<ResultDto> Update(PlannedActivityDtoUpdate dto, bool isServicePlanPa = false, int servicePlanId = 0)
        {
            var entityExists = new Plannedactivities();
            try
            {
                if(isServicePlanPa == true)
                {
                    entityExists = await _repositoryWrapper.PlannedActivity.FindByCondition(
                      x => x.Plannedimplementationyear == dto.PlannedImplementationYear
                      && x.Plannedcompletion == dto.PlannedCompletion
                      && x.Plannedactivityid != dto.PlannedActivityId
                      && x.Plannedactivityresourceid == dto.PlannedActivityResourceId && x.Archived != true
                      && x.Isserviceplan == isServicePlanPa && x.Serviceplanid != servicePlanId && x.Opcoid == dto.OpCoId && x.Deliveryprojectname == dto.DeliveryProjectName)
                       .FirstOrDefaultAsync();
                }
                else
                {
                    entityExists = await _repositoryWrapper.PlannedActivity.FindByCondition(
                    x => x.Plannedimplementationyear == dto.PlannedImplementationYear
                    && x.Plannedcompletion == dto.PlannedCompletion
                    && x.Plannedactivityid != dto.PlannedActivityId
                    && x.Plannedactivityresourceid == dto.PlannedActivityResourceId && x.Archived != true)
                     .FirstOrDefaultAsync();
                }


                if (entityExists != null)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryUpdateExists,
                        Data = entityExists.Plannedactivityid
                    };
                }
                var entity = _mapper.Map<PlannedActivity>(dto);
                var model = SetPlannedActivityValue(PlannedActivityMapper.Set(entity));
                model.Operationalriskid = model.Operationalriskid != null ? model.Operationalriskid : dto.RiskOpeId;
                model.Engineeringriskid = model.Engineeringriskid != null ? model.Engineeringriskid : dto.RiskEngId;
                model.Archived = model.Archived == null ? false : model.Archived;
                _repositoryWrapper.PlannedActivity.Update(model);

                var deliveryTrackingExists = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == model.Plannedactivityid).Any();

                if (dto.DeliveryPlanAvailable && !deliveryTrackingExists)
                {
                    var deliveryTracking = new DeliveryTrackingDtoCreate()
                    {
                        PlannedActivityId = model.Plannedactivityid
                    };
                    await _deliveryTrackingManager.Add(deliveryTracking);
                }

                await _repositoryWrapper.SaveAsync();

                #region ClusterLevel LCM PA
                if (dto.infraClusterClusterUpgradeUpsertDto?.infraClusterAsPlannedDtoGrid != null && dto.infraClusterClusterUpgradeUpsertDto?.infraClusterAsPlannedDtoGrid.Count() > 0)
                {
                var clusterLevelPA = await _infraClusterManager.Value.AddOrUpdateInfraClusterAsync(dto.infraClusterClusterUpgradeUpsertDto, entity.PlannedActivityId,false,entity.PlannedActivityResource.RuleLinkedDc);
                }
                #endregion

                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = entity.PlannedActivityId
                };
            }
            catch(Exception Ex)
            {
                _logger.LogError(Ex.StackTrace);
                return new ResultDto
                {
                    Info = Ex.Message,
                    Warning = true,
                };

            }
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == id).SingleAsync();
            _repositoryWrapper.PlannedActivity.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Plannedactivityid
            };
        }


        public async Task<ResultDto> DeleteDeep(long id, bool isServicePlan = false)
        {
            try
            {
                var entity = new Plannedactivities();
                if(isServicePlan == true)
                {
                    entity = await _repositoryWrapper.PlannedActivity
                                        .FindByCondition(x => x.Serviceplanid == id).Include(x => x.Budgetprojecttrackers).Include(x => x.Projectsplan)
                                        .Include(x => x.Damigrationstatus).Include(x => x.Daassetmigration).FirstOrDefaultAsync();
                }
                else
                {
                    entity = await _repositoryWrapper.PlannedActivity
                    .FindByCondition(x => x.Plannedactivityid == id).Include(x => x.Budgetprojecttrackers).Include(x => x.Projectsplan)
                    .Include(x => x.Damigrationstatus).Include(x => x.Daassetmigration).FirstOrDefaultAsync();
                }

                if (entity != null)
                {
                    //Ticket 897 
                    var deliveryTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == entity.Plannedactivityid).FirstOrDefault();
                    if (deliveryTracking != null)
                        _repositoryWrapper.DeliveryTrackingRepository.DeleteDeep(deliveryTracking);

                    await _commonManager.GenerateAuditLogEntryForPAHardDeleteEntity(entity);

                    if (entity.Budgetprojecttrackers != null && entity.Budgetprojecttrackers.Count > 0)
                    {
                        if (entity.Budgetprojecttrackers.FirstOrDefault() != null)
                        {
                            _repositoryWrapper.BudgetProjectTrackersRepository.DeleteDeep(entity.Budgetprojecttrackers.FirstOrDefault());
                        }

                    }
                    // add delte fun for project plan

                    foreach (var deletePp in entity.Projectsplan)
                    {
                        _repositoryWrapper.ProjectPlanRepository.DeleteDeep(deletePp);
                    }

                    if (entity.Daassetmigration != null && entity.Daassetmigration.Count > 0)
                        await _platformMigrationManager.DeletePltformMigrationPaNewAsset(entity.Plannedactivityid);

                    foreach (var item in entity.Daassetmigration)
                    {
                        _repositoryWrapper.DaAssetMigrationRepository.DeleteDeep(item);
                    }
                    foreach (var item in entity.Damigrationstatus)
                    {
                        _repositoryWrapper.DaMigrationStatusRepository.DeleteDeep(item);
                    }

                    _repositoryWrapper.PlannedActivity.DeleteDeep(entity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity != null ? entity.Plannedactivityid : null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotDeleted,
                    Warning = true,
                };
            }

        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {

            var target = await Task.Run(() => _repositoryWrapper.PlannedActivity
              .FindByCondition(x => x.Plannedactivityid == id)
              .Include(x => x.Activitystatus)
              .Include(x => x.Planningactivitystatus).SingleOrDefault());

            var linkedPlanndeActivity = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Linkedtoplannedactivityid == id)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Activitystatus)
                .Include(x => x.Deliverystatus)
                .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                .ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            if (linkedPlanndeActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = ConstantValueFilter.LinkedPlannedActivity, Values = linkedPlanndeActivity });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto
                    {
                        EntityName = ConstantValueFilter.PlannedActivity,
                        RecordName = target.Plannedimplementationyear + " | " + target.Activitystatus.Activitystatus + " " + target.Planningactivitystatus.Planningactivitystatus,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public PlannedActivityDto Get(long id)
        {
            var entity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == id).Single();
            return _mapper.Map<PlannedActivityDto>(PlannedActivityMapper.Get(entity));
        }

        public async Task<PlannedActivityDtoCreate> GetCreatePage(List<short> _opcoList, List<int> _verticalList, long DcId, long DcfId = 0, bool isServicePa = false)
        {
            #region    //Ticket 595 -#590 - Vertical Filter to be applied on design component dropdown's in LC, PA, DA etc.,         

            //Ticket 805 LCM -PA: Planned Design components dropdown should be grouped based on DCF
            // and Tranisent Records are merged with DC dropdown    
            #endregion

            var activityStatus = _repositoryWrapper.ActivityStatus.FindAll();
            var planningActivityStatusResource = _repositoryWrapper.PlanningActivityStatus.FindAll();

            //var getIsPlaftformSwProductName = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == ProductId).Select(x => x.Description.ToLower()).FirstOrDefault();


            var responsibilityPhaseResource = _repositoryWrapper.ResponsibilityPhase.FindAll();

            #region #exodus   && ClusterLevel PA
            var dcEntity =  _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == DcId  || x.Designcomponentfamilyid == DcfId)
                                 .Include(x => x.Systemtype.Majorsoftwarebuilds.Productname)
                                 .Include(x => x.Systemtype.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware.Buildconstruction)
                                .FirstOrDefault()  ;



            #region ClusterLevel PA
            var isClusterLevelPA = (dcEntity?.Systemtype?.Majorsoftwarebuilds?.Isvmware == true &&
               Convert.ToString( dcEntity?.Systemtype?.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain == true)
                .FirstOrDefault()?.Majorhardware?.Buildconstruction?.Buildconstruction)?.ToLower() == ConstantValueFilter.Infrastructure.ToLower()  ) ? true : false;
            #endregion
            var isExodusPa = (DcfId == 0) ? false : dcEntity?.Systemtype?.Majorsoftwarebuilds?.Isvmware ?? false;

            var plannedActivityResource = _repositoryWrapper.PlannedActivityResourceRepository
                           .FindAll()
                          .Include(x => x.Plannedactivityresourceplanningrisk)
                          .Include(x => x.Plannedactivityresourcebenefit)
                          .Include(x => x.Plannedactivityresourcedriver)
                          .AsNoTracking();

            if(!isExodusPa)       
                plannedActivityResource = plannedActivityResource.Where(x => x.Rulelinkeddc != (int)PlannedActivityResourceEnum.Infra_Readiness);

            if (!isClusterLevelPA)
                plannedActivityResource = plannedActivityResource.Where(x => x.Rulelinkeddc != (int)PlannedActivityResourceEnum.AddNewCluster
                                      && x.Rulelinkeddc != (int)PlannedActivityResourceEnum.Add_Remove_Application_from_Cluster
                                      && x.Rulelinkeddc != (int)PlannedActivityResourceEnum.Upgrade_HardwareTypes);

            #endregion

            var budgetAv = _repositoryWrapper.BudgetAvailability.FindAll();
            var risk = _repositoryWrapper.Risk.FindAll();
            var opocResource = (_opcoList != null && _opcoList.Any() == true) ?
                                 _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco)
                                 : _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            // var LocationResource = _dropdownDataServiceManager.GetAllLocations().Result;
            var driverResource = _repositoryWrapper.Driver.FindAll();
            var benefitResource = _repositoryWrapper.Benefit.FindAll();
            var planningRiskResource = _repositoryWrapper.PlanningRisk.FindAll();
            var programResource = _repositoryWrapper.ProgramRepository.FindAll().ToDictionary(x => x.Programid, x => x.Programdescription);
            var defaultPlanningStatus = planningActivityStatusResource.Where(x => x.Default == true)?
                .SingleOrDefault()?.Planningactivitystatusid;

            List<ViewBagandComponenetDto> buildBagResources = await _commonManager.GetBagAndComponentDetailsForDropdownAsync(0, true);
            #region//Ticket 805 LCM -PA: Planned Design components dropdown should be grouped based on DCF  and Tranisent Records are merged with DC dropdown           
            var designComponentResource = DesignComponentTypeExtensionMethod.GetDesignComponentResource(_verticalList, _repositoryWrapper, ConstantValueFilter.All);
            var designComponentResourcePairs = DesignComponentTypeExtensionMethod.GetDCDropdownRecord(designComponentResource, DcId, _repositoryWrapper);

            #endregion

            #region // Team,BudgetOwner,CategoryPA Resource
            var plannedActivityCategoryResource = await _dropdownDataServiceManager.GetPACategoryDropDown();
            #endregion

            var model = new PlannedActivityDtoCreate()
            {

                ActivityStatusResource = activityStatus.ToDictionary(x => x.Activitystatusid, x => x.Activitystatus),
                PlanningActivityStatusResource = planningActivityStatusResource.ToDictionary(x => x.Planningactivitystatusid,
                    x => x.Planningactivitystatus),

                OpCoResource = opocResource,
                ResponsibilityPhaseResource = responsibilityPhaseResource.ToDictionary(x => x.Responsibilityphaseid, x => x.Responsibilityphase),
                PlanningActivityStatusId = defaultPlanningStatus,

                #region //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
                ///Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'  -- July 3 -24
                ///Ticket 805 LCM -PA: Planned Design components dropdown should be grouped based on DCF  and Tranisent Records are merged with DC dropdown
                DesignComponentResource = designComponentResourcePairs.ToList()?.ToList(),


                DesignComponentIsVirtualizedResource = designComponentResource.Where(x => x.Visibleflag == true)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                    .ToDictionary(x => x.Designcomponentid, x =>
                    {
                        return x.Systemtype?.Systemtypesmajorhardwarebuilds?.SingleOrDefault(m =>
                                        m.Ismain &&
                                        m.Systemtypeid == x.Systemtype?.Systemtypeid &&
                                        m.Deleted == false)?.Majorhardware?.Buildconstruction?.Rule == 3
                                        ? true
                                        : false;
                    }),
                #endregion
                DeliveryPlanAvailable = true,
                RiskResource = risk.ToDictionary(x => x.Riskid, x => x.Description),
                PlannedActivityResource = plannedActivityResource.ToDictionary(
                            x => x.Plannedactivityresourceid,
                            x =>
                            {
                                var mapper = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x);

                                return new PlannedActivityResourceDto
                                {
                                    PlannedActivityResourceDescription = x.Plannedactivityresource,
                                    LastModified = x.Modificationdate,
                                    RuleActicvityDetails = x.Ruleacticvitydetails,
                                    RuleLinkedDc = x.Rulelinkeddc,
                                    Exportable = x.Exportable,
                                    LcmLabelHardware = x.Lcmlabelhardware,
                                    LcmLabelSoftware = x.Lcmlabelsoftware,
                                    LcmHardware = x.Lcmhardware,
                                    LcmSoftware = x.Lcmsoftware,

                                    AddAssetHardware = x.Addassethardware,
                                    AddAssetLabelHardware = x.Addassetlabelhardware,
                                    AddAssetSoftware = x.Addassetsoftware,
                                    AddAssetLabelSoftware = x.Addassetlabelsoftware,

                                    EditAssetHardware = x.Editassethardware,
                                    EditAssetLabelHardware = x.Editassetlabelhardware,
                                    EditAssetSoftware = x.Editassetsoftware,
                                    EditAssetLabelSoftware = x.Editassetlabelsoftware,

                                    ForLcm = x.Forlcm,
                                    ForDesignAspect = x.Fordesignaspect,
                                    DesignAspectHardware = x.Designaspecthardware,
                                    DesignAspectSoftware = x.Designaspectsoftware,
                                    ActivityDetailsDesignAspect = x.Activitydetailsdesignaspect,

                                    DriverTextDesignAspect = mapper.getSelectedDrivers(false, true, false, false),
                                    BenefitTextDesignAspect = mapper.getSelectedBenefits(false, true, false, false),
                                    PlanningRisksDesignAspect = mapper.getSelectedPlanningRisks(false, true, false, false),

                                    DesignAspectLabelHardware = x.Designaspectlabelhardware,
                                    DesignAspectLabelSoftware = x.Designaspectlabelsoftware,
                                    RuleDesignAspect = x.Ruledesignaspect,
                                    RuleAddAsset = x.Ruleaddasset,
                                    RuleEditAsset = x.Ruleeditasset,

                                    DriverTextAddAsset = mapper.getSelectedDrivers(false, false, true, false),
                                    DriverTextEditAsset = mapper.getSelectedDrivers(false, false, false, true),

                                    BenefitTextAddAsset = mapper.getSelectedBenefits(false, false, true, false),
                                    BenefitTextEditAsset = mapper.getSelectedBenefits(false, false, false, true),

                                    PlanningRisksAddAsset = mapper.getSelectedPlanningRisks(false, false, true, false),
                                    PlanningRisksAEditAsset = mapper.getSelectedPlanningRisks(false, false, false, true),

                                    PlannedDesignComponentRequiredAddAsset = x.Plandesigncompreqaddasset,
                                    PlannedDesignComponentRequiredEditAsset = x.Plandesigncompreqeditasset,

                                    DriverTextLcm = mapper.getSelectedDrivers(true, false, false, false),
                                    BenefitTextLcm = mapper.getSelectedBenefits(true, false, false, false),
                                    PlanningRisksLcm = mapper.getSelectedPlanningRisks(true, false, false, false),

                                    DriverTextServicePlan = mapper.getSelectedDrivers(false, false, false, false, true),
                                    BenefitTextServicePlan = mapper.getSelectedBenefits(false, false, false, false, true),
                                    PlanningRiskServicePlan = mapper.getSelectedPlanningRisks(false, false, false, false, true),

                                    ForCreateAddAsset = x.Forcreateaddasset,
                                    ForCreateEditAsset = x.Forcreateeditasset,

                                    ForEditAddAsset = x.Foreditaddasset,
                                    ForEditEditAsset = x.Forediteditasset,

                                    RuleActicvityDetailsAddAsset = x.Ruleactdetailsaddasset,
                                    RuleActicvityDetailsEditAsset = x.Ruleactdetailseditasset,

                                    JsonForm = x.Jsonform,
                                    PlannedActivityResourceId = x.Plannedactivityresourceid,
                                    ActivityDetailsLcm = x.Activitydetailslcm,

                                    OnBareMetalAddAsset = x.Onbaremetaladdasset,
                                    OnBareMetalEditAsset = x.Onbaremetaleditasset,
                                    OnVirtualizedAddAsset = x.Onvirtualizedaddasset,
                                    OnVirtualizedEditAsset = x.Onvirtualizededitasset,

                                    ActivityDetailsAddAsset = x.Activitydetailsaddasset,
                                    ActivityDetailsEditAsset = x.Activitydetailseditasset,

                                    ActivityDetailsForVirtualizedAddAsset = x.Actdetailsforvrtaddasset,
                                    ActivityDetailsForVirtualizedEditAsset = x.Actdetailsforvrteditasset,

                                    ForEditAsset = x.Foreditasset,
                                    ForAddAsset = x.Foraddasset,
                                    Forserviceplan = x.Forserviceplan,
                                };
                            }),

                DriverResource = driverResource.ToDictionary(x => x.Driverid, x => x.Driver),
                BenefitResource = benefitResource.ToDictionary(x => x.Benefitid, x => x.Benefit),
                PlanningRiskResource = planningRiskResource.ToDictionary(x => x.Planningriskid, x => x.Planningrisk),
                BudgetAvaibilityResource = budgetAv.ToDictionary(x => x.Budgetavailabilityid, x => x.Description),
                IsPAReleaseDetailUnknown = false,
                //IsServicePlan = 
                #region  //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
                ///Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'  -- July 3 -24
                TransientDesignComponentResource = designComponentResource.Where(x => x.Visibleflag == false).toDesignComponentResource(_repositoryWrapper)
                              .Where(
                    x => !x.Value.ToLower().Contains(ConstantValueFilter.Unknown)).ToDictionary(x => x.Key, x => x.Value),

                #endregion
                BuildBagResources = buildBagResources,
                PlannedActivityCategoryResource = plannedActivityCategoryResource,
                PlannedActivityTeam = ConstantValueFilter.PlannedActivityTeam,
                LcmCategoryResource = _commonManager.LcmCategoryStatusResource(),
                PriorityResource = _commonManager.PriorityStatusResource(),
                ProgramResource = programResource,

                DesignComponentFamilyResource = designComponentResource.ToList().Select(x => x.Designcomponentfamily).DistinctBy(y => y.Designcomponentfamilyid).ToDictionary(x => x.Designcomponentfamilyid,
                x => x.DCFName(_repositoryWrapper)).Where(f => !f.Value.IsNullOrEmpty()).ToDictionary(k => k.Key, v => v.Value).ToList(),

                #region  exodus
                //LocationResource = LocationResource,

                //daMigrationStatusKeyValue = ConstantValueFilter.daMigrationStatusCode,

                #endregion

            };

            #region InfraReady for Da Pa
            var plannedDcfIdForInfraReady = designComponentResource.Where(x => x.Systemtype.Majorsoftwarebuilds.Isvmware == true).Select(x => x.Designcomponentfamilyid)?.Distinct()?.ToList();
            if (plannedDcfIdForInfraReady != null && plannedDcfIdForInfraReady.Count > 0 && (model.DesignComponentFamilyResource != null && model?.DesignComponentFamilyResource.Count > 0))
            {
                model.PlannedDcfResource = model.DesignComponentFamilyResource.Where(x => plannedDcfIdForInfraReady.Contains(x.Key)).ToList();

            }
            #endregion
            return model;
        }
        public List<KeyValuePair<short, string>> GetPlannedActivityRelatedDeliveryStatus_Old(short plannedActivityResourceId, short plannedActivityTypeFor)
        {
            List<KeyValuePair<short, string>> deliveryStatusDictionar = new List<KeyValuePair<short, string>>();
            var result = _repositoryWrapper.SettingsUpdatePlannedActivity
                .FindByCondition(x => x.Plannedactivityresourceid == plannedActivityResourceId && x.Plannedactivitytypefor == plannedActivityTypeFor)
                .OrderBy(x => x.Order)
                .Select(x => new { x.Order, x.Deliverystatus.Deliverystatus, x.Deliverystatus.Deliverystatusid });

            foreach (var item in result)
            {
                deliveryStatusDictionar.Add(new KeyValuePair<short, string>
                     (item.Deliverystatusid, item.Deliverystatus));

            }

            return deliveryStatusDictionar;

        }

        public List<PlannedActivityDeliveryStatusDropDownDto> GetPlannedActivityRelatedDeliveryStatus(short plannedActivityResourceId, short plannedActivityTypeFor)
        {
            List<PlannedActivityDeliveryStatusDropDownDto> deliveryStatusDic =
                _repositoryWrapper.SettingsUpdatePlannedActivity
                .FindByCondition(x => x.Plannedactivityresourceid == plannedActivityResourceId && x.Plannedactivitytypefor == plannedActivityTypeFor)
                .OrderBy(x => x.Order)
                .Select(x => new PlannedActivityDeliveryStatusDropDownDto
                { Value = x.Deliverystatus.Deliverystatus, Key = (short)x.Deliverystatus.Deliverystatusid, NeedPlannedAsset = (bool)x.Needplannedasset })
                .ToList<PlannedActivityDeliveryStatusDropDownDto>();

            return deliveryStatusDic;

        }
        public bool GetConfrontoHardwareTypeFromDesignComponent(long designComponentId, long plannedDesignComponent)
        {
            var dc = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == designComponentId)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .Select(x => x.Systemtype.Systemtypesmajorhardwarebuilds.Single(x => x.Ismain).Majorhardware.Hardwaretype).SingleOrDefault();

            var plannedDc = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == plannedDesignComponent)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .Select(x => x.Systemtype.Systemtypesmajorhardwarebuilds.Single(x => x.Ismain).Majorhardware.Hardwaretype).SingleOrDefault();

            return dc == plannedDc;
        }
        public bool CheckFiscalYear(int fiscalYear)
        {
            var today = DateTime.Today;
            var monthToday = today.Month;
            var yearToday = today.Year;

            if (fiscalYear == yearToday)
            {
                return monthToday > 3;
            }
            else if (fiscalYear == (yearToday - 1))
            {
                return monthToday <= 3;
            }
            else
            {
                return false;
            }
        }

        public async Task<PlannedActivityDtoUpdate> GetUpdatePage(long id, List<short> _opcoList, List<int> _verticalList, short plannedActivityTypeFor = 0, long currentDcfId = 0, bool isServicePA = false)
        {
            var entity = new Plannedactivities();
            if (isServicePA)
            {
                entity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Serviceplanid == id && x.Archived != true, true)
               .Include(x => x.Buildbag).Include(x => x.Damigrationstatus)
               .Include(x => x.Daassetmigration).ThenInclude(x => x.Deploymentstatus)
               .Include(x => x.Designaspect)
               .Include(x => x.ModificationuserNavigation).Include(p => p.Deliverystatus).Include(p => p.Activitystatus).SingleOrDefault();
            }
            else
            {
                entity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == id, true)
                .Include(x => x.Buildbag).Include(x => x.Damigrationstatus)
                .Include(x => x.Daassetmigration).ThenInclude(x => x.Deploymentstatus)
                .Include(x => x.Designaspect)
                .Include(x => x.ModificationuserNavigation).Include(p => p.Deliverystatus).Include(p => p.Activitystatus).SingleOrDefault();
            }
            var model = PlannedActivityMapper.Get(entity);
            var dto = _mapper.Map<PlannedActivityDtoUpdate>(model);

            if (entity != null)
            {
                #region LookUp     

                List<ViewBagandComponenetDto> buildBagResources = await _commonManager.GetBagAndComponentDetailsForDropdownAsync(0, false, true, Convert.ToInt32(dto?.DesignComponentId), (short)dto.OpCoId, 0);
                if (buildBagResources.Any(x => x.BuildBagId == dto.BuildBagId) == false)
                {
                    buildBagResources = await _commonManager.GetBagAndComponentDetailsForDropdownAsync(dto.BuildBagId, false);
                }
                #region //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
                #region //Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'  -- July 3 -24
                #region //Ticket 805 LCM -PA: Planned Design components dropdown should be grouped based on DCF  and Tranisent Records are merged with DC dropdown

                #region//Ticket 805 LCM -PA: Planned Design components dropdown should be grouped based on DCF  and Tranisent Records are merged with DC dropdown

                var dcResource = DesignComponentTypeExtensionMethod.GetDesignComponentResource(_verticalList, _repositoryWrapper, ConstantValueFilter.All);
                dto.DesignComponentResource = DesignComponentTypeExtensionMethod.GetDCDropdownRecord(dcResource, Convert.ToInt64(entity?.Designcomponentid), _repositoryWrapper);

                #endregion
                #endregion
                dto.TransientDesignComponentResource = dcResource.ToList().Where(x => x.Visibleflag == false).toDesignComponentResource(_repositoryWrapper)
                              .Where(
                    x => !x.Value.ToLower().Contains(ConstantValueFilter.Unknown)).ToDictionary(x => x.Key, x => x.Value);
                #endregion
                #endregion
                #region //Ticket 805 LCM -PA: Planned Design components dropdown should be grouped based on DCF  and Tranisent Records are merged with DC dropdown

                if (dto.DesignComponentId != null && !(dto.DesignComponentResource.Any(x => x.Key == dto.DesignComponentId.Value)))
                {
                    var data = _repositoryWrapper.DesignComponent.FindByCondition(
                            x => x.Designcomponentid == dto.DesignComponentId,
                            includeDeleted: true)
                        .Include(x => x.Systemtype)
                        .ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                        .SingleOrDefault();
                    if (data != null)
                    {
                        dto.DesignComponentResource.Add(new KeyValuePair<long, string>(data.Designcomponentid,
                            data.toDesignComponentNameLcm(_repositoryWrapper)
                      ));
                    }
                }
                #endregion
                dto.DesignComponentIsVirtualizedResource = ((dcResource
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                ).ToList().Where(x => x.Visibleflag == true))
                .ToDictionary(x => x.Designcomponentid, x =>
                {
                    return x.Systemtype?.Systemtypesmajorhardwarebuilds?.SingleOrDefault(m =>
                                    m.Ismain &&
                                    m.Systemtypeid == x.Systemtype?.Systemtypeid &&
                                    m.Deleted == false)?.Majorhardware?.Buildconstruction?.Rule == 3
                                    ? true
                                    : false;
                }).Where(f => !f.Value.ToString().IsNullOrEmpty()).ToDictionary(k => k.Key, v => v.Value);

                var opco = (_opcoList != null && _opcoList.Any() == true) ?
                                 _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco)
                                 : _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
                dto.OpCoResource = opco;
                if (dto.OpCoId.HasValue && !dto.OpCoResource.ContainsKey(dto.OpCoId.Value))
                {
                    var data = _repositoryWrapper.OpCo.FindByCondition(
                        x => x.Opcoid == dto.OpCoId,
                        includeDeleted: true).SingleOrDefault();
                    if (data != null)
                    {
                        dto.OpCoResource.Add(data.Opcoid, data.Opco);
                    }
                }

                var risk = _repositoryWrapper.Risk.FindAll();
                dto.RiskResource = risk.ToDictionary(x => x.Riskid, x => x.Description);
                if (dto.RiskOpeId.HasValue && !dto.RiskResource.ContainsKey(dto.RiskOpeId.Value))
                {
                    var data = _repositoryWrapper.Risk.FindByCondition(
                        x => x.Riskid == dto.RiskOpeId,
                        includeDeleted: true).SingleOrDefault();
                    if (data != null)
                    {
                        dto.RiskResource.Add(data.Riskid, data.Description);
                    }
                }
                if (dto.RiskEngId.HasValue && !dto.RiskResource.ContainsKey(dto.RiskEngId.Value))
                {
                    var data = _repositoryWrapper.Risk.FindByCondition(
                        x => x.Riskid == dto.RiskEngId,
                        includeDeleted: true).SingleOrDefault();
                    if (data != null)
                    {
                        dto.RiskResource.Add(data.Riskid, data.Description);
                    }
                }

                var budgetAv = _repositoryWrapper.BudgetAvailability.FindAll();
                dto.BudgetAvaibilityResource =
                    budgetAv.ToDictionary(x => x.Budgetavailabilityid, x => x.Description);
                if (dto.BudgetAvailabilityId.HasValue && !dto.BudgetAvaibilityResource.ContainsKey(dto.BudgetAvailabilityId.Value))
                {
                    var data = _repositoryWrapper.BudgetAvailability.FindByCondition(
                        x => x.Budgetavailabilityid == dto.BudgetAvailabilityId,
                        includeDeleted: true).SingleOrDefault();
                    if (data != null)
                    {
                        dto.BudgetAvaibilityResource.Add(data.Budgetavailabilityid, data.Description);
                    }
                }
                var deliveryStatusResource = _repositoryWrapper.DeliveryStatus.FindAll();
                dto.DeliveryStatusResource =
                    deliveryStatusResource.ToDictionary(x => x.Deliverystatusid, x => x.Deliverystatus);
                if (dto.DeliveryStatusId.HasValue && !dto.DeliveryStatusResource.ContainsKey(dto.DeliveryStatusId.Value))
                {
                    var data = _repositoryWrapper.DeliveryStatus.FindByCondition(
                        x => x.Deliverystatusid == dto.DeliveryStatusId,
                        includeDeleted: true).SingleOrDefault();
                    if (data != null)
                    {
                        dto.DeliveryStatusResource.Add(data.Deliverystatusid, data.Deliverystatus);
                    }
                }

                var activityStatus = _repositoryWrapper.ActivityStatus.FindAll();
                dto.ActivityStatusResource =
                    activityStatus.ToDictionary(x => x.Activitystatusid, x => x.Activitystatus);
                if (!dto.ActivityStatusResource.ContainsKey(dto.ActivityStatusId))
                {
                    var data = _repositoryWrapper.ActivityStatus.FindByCondition(
                        x => x.Activitystatusid == dto.ActivityStatusId,
                        includeDeleted: true).SingleOrDefault();
                    if (data != null)
                    {
                        dto.ActivityStatusResource.Add(data.Activitystatusid, data.Activitystatus);
                    }
                }
                var planningActivityStatusResource = _repositoryWrapper.PlanningActivityStatus.FindAll();
                dto.PlanningActivityStatusResource =
                    planningActivityStatusResource.ToDictionary(x => x.Planningactivitystatusid,
                        x => x.Planningactivitystatus);
                if (!dto.PlanningActivityStatusResource.ContainsKey(dto.PlanningActivityStatusId.Value))
                {
                    var data = _repositoryWrapper.PlanningActivityStatus.FindByCondition(
                        x => x.Planningactivitystatusid == dto.PlanningActivityStatusId,
                        includeDeleted: true).SingleOrDefault();
                    if (data != null)
                    {
                        dto.PlanningActivityStatusResource.Add(data.Planningactivitystatusid, data.Planningactivitystatus);
                    }
                }
                var responsibilityPhaseResource = _repositoryWrapper.ResponsibilityPhase.FindAll();
                dto.ResponsibilityPhaseResource = responsibilityPhaseResource.ToDictionary(x => x.Responsibilityphaseid, x => x.Responsibilityphase);
                if (dto.ResponsibilityPhaseId.HasValue && !dto.ResponsibilityPhaseResource.ContainsKey(dto.ResponsibilityPhaseId.Value))
                {
                    var data = _repositoryWrapper.ResponsibilityPhase.FindByCondition(
                        x => x.Responsibilityphaseid == dto.ResponsibilityPhaseId,
                        includeDeleted: true).SingleOrDefault();
                    if (data != null)
                    {
                        dto.ResponsibilityPhaseResource.Add(data.Responsibilityphaseid, data.Responsibilityphase);
                    }
                }

                #region  Exodus
                var isClusterLevelPA = false;
                if (entity.Lcmengineeringid != null)
                {
                    var dcEntity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == id).Include(x => x.Lcmengineering)
                        .ThenInclude(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware.Buildconstruction)
                        .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname).FirstOrDefault();


                    isClusterLevelPA = (dcEntity?.Lcmengineering?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Isvmware == true &&
             Convert.ToString(dcEntity?.Lcmengineering?.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain == true)
              .FirstOrDefault()?.Majorhardware?.Buildconstruction?.Buildconstruction)?.ToLower() == ConstantValueFilter.Infrastructure.ToLower()) ? true : false;

                }


                var isExodusPa = entity.Designaspectid != null ? _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Id == entity.Designaspectid)
                                 .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype.Majorsoftwarebuilds)
                                 .SelectMany(x => x.Designcomponentfamily.Designcomponents.Select(t => t.Systemtype.Majorsoftwarebuilds.Isvmware)).Where(t => t == true)
                                 .FirstOrDefault() ?? false : false;

                #endregion
                var plannedActivityResource = _repositoryWrapper.PlannedActivityResourceRepository
                   .FindAll()
                  .Include(x => x.Plannedactivityresourceplanningrisk)
                  .Include(x => x.Plannedactivityresourcebenefit)
                  .Include(x => x.Plannedactivityresourcedriver)
                  .AsNoTracking();

                if (!isExodusPa)
                    plannedActivityResource = plannedActivityResource.Where(x => x.Rulelinkeddc != (int)PlannedActivityResourceEnum.Infra_Readiness);

                if (!isClusterLevelPA)
                    plannedActivityResource = plannedActivityResource.Where(x => x.Rulelinkeddc != (int)PlannedActivityResourceEnum.AddNewCluster
                                          && x.Rulelinkeddc != (int)PlannedActivityResourceEnum.Add_Remove_Application_from_Cluster
                                          && x.Rulelinkeddc != (int)PlannedActivityResourceEnum.Upgrade_HardwareTypes);


                var plannedHardwareTypeId = _repositoryWrapper.ClusterUpGradeStatusRepository.FindByCondition(x => x.Plannedactivityid == id).FirstOrDefault();


                if (dto.infraClusterClusterUpgradeUpsertDto == null)
                    dto.infraClusterClusterUpgradeUpsertDto = new InfraClusterClusterUpgradeUpsertDto();

                if (plannedHardwareTypeId != null)
                    dto.infraClusterClusterUpgradeUpsertDto.PlannedHardwareTypeId = plannedHardwareTypeId?.Hardwaretype;

                var currentLcmBagName = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == entity.Lcmengineeringid)
                    .Include(x => x.Buildbag).FirstOrDefault()?.Buildbag;

                var programResource = _repositoryWrapper.ProgramRepository.FindAll().ToDictionary(x => x.Programid, x => x.Programdescription);
                var daMultipleDcfEntities = _repositoryWrapper.DaPlannedActivityDcfRepository.FindByCondition(x => x.Plannedactivityid == entity.Plannedactivityid).Select(x => x.Designcomponentfamilyid).ToList();

                dto.PlannedActivityResource = await Task.Run(() =>
           plannedActivityResource.ToDictionary(
               x => x.Plannedactivityresourceid,
               x =>
               {
                   var mapper = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x);

                   return new PlannedActivityResourceDto
                   {
                       PlannedActivityResourceId = x.Plannedactivityresourceid,
                       PlannedActivityResourceDescription = x.Plannedactivityresource,
                       LastModified = x.Modificationdate,
                       RuleActicvityDetails = x.Ruleacticvitydetails,
                       RuleLinkedDc = x.Rulelinkeddc,
                       Exportable = x.Exportable,

                       // LCM
                       LcmLabelHardware = x.Lcmlabelhardware,
                       LcmLabelSoftware = x.Lcmlabelsoftware,
                       LcmHardware = x.Lcmhardware,
                       LcmSoftware = x.Lcmsoftware,
                       ForLcm = x.Forlcm,
                       DriverTextLcm = mapper.getSelectedDrivers(true, false, false, false),
                       BenefitTextLcm = mapper.getSelectedBenefits(true, false, false, false),
                       PlanningRisksLcm = mapper.getSelectedPlanningRisks(true, false, false, false),
                       ActivityDetailsLcm = x.Activitydetailslcm,

                       // Add Asset
                       AddAssetHardware = x.Addassethardware,
                       AddAssetLabelHardware = x.Addassetlabelhardware,
                       AddAssetSoftware = x.Addassetsoftware,
                       AddAssetLabelSoftware = x.Addassetlabelsoftware,
                       DriverTextAddAsset = mapper.getSelectedDrivers(false, false, true, false),
                       BenefitTextAddAsset = mapper.getSelectedBenefits(false, false, true, false),
                       PlanningRisksAddAsset = mapper.getSelectedPlanningRisks(false, false, true, false),
                       PlannedDesignComponentRequiredAddAsset = x.Plandesigncompreqaddasset,
                       RuleAddAsset = x.Ruleaddasset,
                       ForCreateAddAsset = x.Forcreateaddasset,
                       ForEditAddAsset = x.Foreditaddasset,
                       ActivityDetailsAddAsset = x.Activitydetailsaddasset,
                       OnBareMetalAddAsset = x.Onbaremetaladdasset,
                       OnVirtualizedAddAsset = x.Onvirtualizedaddasset,
                       ActivityDetailsForVirtualizedAddAsset = x.Actdetailsforvrtaddasset,
                       ForAddAsset = x.Foraddasset,

                       // Edit Asset
                       EditAssetHardware = x.Editassethardware,
                       EditAssetLabelHardware = x.Editassetlabelhardware,
                       EditAssetSoftware = x.Editassetsoftware,
                       EditAssetLabelSoftware = x.Editassetlabelsoftware,
                       DriverTextEditAsset = mapper.getSelectedDrivers(false, false, false, true),
                       BenefitTextEditAsset = mapper.getSelectedBenefits(false, false, false, true),
                       PlanningRisksAEditAsset = mapper.getSelectedPlanningRisks(false, false, false, true),
                       PlannedDesignComponentRequiredEditAsset = x.Plandesigncompreqeditasset,
                       RuleEditAsset = x.Ruleeditasset,
                       ForCreateEditAsset = x.Forcreateeditasset,
                       ForEditEditAsset = x.Forediteditasset,
                       ActivityDetailsEditAsset = x.Activitydetailseditasset,
                       OnBareMetalEditAsset = x.Onbaremetaleditasset,
                       OnVirtualizedEditAsset = x.Onvirtualizededitasset,
                       ActivityDetailsForVirtualizedEditAsset = x.Actdetailsforvrteditasset,
                       ForEditAsset = x.Foreditasset,

                       // Design Aspect
                       DesignAspectExportable = x.Designaspectexportable ?? false,
                       ForDesignAspect = x.Fordesignaspect,
                       DesignAspectHardware = x.Designaspecthardware,
                       DesignAspectSoftware = x.Designaspectsoftware,
                       DesignAspectLabelHardware = x.Designaspectlabelhardware,
                       DesignAspectLabelSoftware = x.Designaspectlabelsoftware,
                       ActivityDetailsDesignAspect = x.Activitydetailsdesignaspect,
                       DriverTextDesignAspect = mapper.getSelectedDrivers(false, true, false, false),
                       BenefitTextDesignAspect = mapper.getSelectedBenefits(false, true, false, false),
                       PlanningRisksDesignAspect = mapper.getSelectedPlanningRisks(false, true, false, false),
                       RuleDesignAspect = x.Ruledesignaspect,

                       // Misc
                       RuleActicvityDetailsAddAsset = x.Ruleactdetailsaddasset,
                       RuleActicvityDetailsEditAsset = x.Ruleactdetailseditasset,
                       JsonForm = x.Jsonform,
                       Forserviceplan = x.Forserviceplan,

                       DriverTextServicePlan = mapper.getSelectedDrivers(false, false, false, false, true),
                       BenefitTextServicePlan = mapper.getSelectedBenefits(false, false, false, false, true),
                       PlanningRiskServicePlan = mapper.getSelectedPlanningRisks(false, false, false, false, true),
                   };
               })
       );
                var driverResource = _repositoryWrapper.Driver.FindAll();
                dto.DriverResource = driverResource.ToDictionary(x => x.Driverid, x => x.Driver);

                var benefitResource = _repositoryWrapper.Benefit.FindAll();
                var planningRiskResource = _repositoryWrapper.PlanningRisk.FindAll();

                #region // Team,BudgetOwner,CategoryPA Resource
                var plannedActivityCategoryResource = await _dropdownDataServiceManager.GetPACategoryDropDown();
                #endregion

                dto.BenefitResource = benefitResource.ToDictionary(x => x.Benefitid, x => x.Benefit);
                dto.PlanningRiskResource = planningRiskResource.ToDictionary(x => x.Planningriskid, x => x.Planningrisk);

                dto.DesignComponentFamilyName = model.toOriginalDesignComponentFamilyDescription(_repositoryWrapper);
                dto.PlannedDesignComponentName = model.toPlannedDesignComponentDescription(_repositoryWrapper);
                dto.DeliveryStatusName = entity.Deliverystatus?.Deliverystatus;
                dto.ActivityStatusName = entity.Activitystatus?.Activitystatus;
                dto.IsPAReleaseDetailUnknown = entity.Ispareleasedetailunknown;
                dto.BuildBagResources = buildBagResources;
                dto.CurrentBuildBagDescription = _commonManager.GetBuildBagDescription(currentLcmBagName);
                dto.PlannedBuildBagDescription = _commonManager.GetBuildBagDescription(entity.Buildbag);
                dto.PlannedActivityCategoryResource = plannedActivityCategoryResource;
                dto.PlannedActivityTeam = ConstantValueFilter.PlannedActivityTeam;
                dto.LcmCategoryResource = _commonManager.LcmCategoryStatusResource();
                dto.PriorityResource = _commonManager.PriorityStatusResource();
                dto.LcmCategories = _commonManager.LcmCategoryStatus(dto.LcmCategories);
                dto.ProgramId = Convert.ToInt64(entity.Programid);
                dto.ProgramResource = programResource;
                dto.DesignComponentFamilyResource = DesignComponentTypeExtensionMethod.GetDCFDropdownRecord(_repositoryWrapper);
                dto.DesignComponentFamilyIdList = daMultipleDcfEntities.Any() ? daMultipleDcfEntities : null;

                if (entity.Damigrationstatus != null && entity.Damigrationstatus.Count > 1)
                {
                    dto.PlannedActivityName =
                                    $"Planned : {entity.Damigrationstatus.Count(x => x.Statusid == 1)} | In-Progress : {entity.Damigrationstatus.Count(x => x.Statusid == 2)} | Completed : {entity.Damigrationstatus.Count(x => x.Statusid == 3)}"
                                   ;
                }
                else if (entity.Daassetmigration != null && entity.Daassetmigration.Count > 1)
                {
                    dto.PlannedActivityName = entity.GetDaAssetDepoymentStatusCount(_repositoryWrapper, entity.Daassetmigration.ToList());

                }


                #endregion

                #region Platform Migration
                if (currentDcfId != 0) dto.DesignComponentFamilyId = currentDcfId;
                var isPlaftFormMigration = plannedActivityResource.Any(x => x.Plannedactivityresourceid == model.PlannedActivityResourceId
                    && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration);
                if (isPlaftFormMigration)
                {

                    var dcId = entity?.Designaspect?.Designcomponentfamilyid;//3970;
                    var selectedDcfName = dto.DesignComponentFamilyResource.Where(x => x.Key == dcId).FirstOrDefault().Value;
                    PlatformPlannedDcfDto _platformPlannedDcfDto = new PlatformPlannedDcfDto();
                    _platformPlannedDcfDto.compareValue = selectedDcfName;
                    _platformPlannedDcfDto.dcfResource = dto.DesignComponentFamilyResource.ToDictionary(x => x.Key, y => y.Value);

                    dto.PlannedDcfResource = await _platformMigrationManager.GetPlannedDcfResourcesForPlatformMigration
                        (_platformPlannedDcfDto);
                    var daAssetMigration = await _platformMigrationManager.GetAssetPlatformDropdown((long)model.OpCoId,
                        (long)model.DesignComponentFamilyId, (long)model?.PlannedDesignComponentFamilyId);
                    dto.PlaftformMigrationDcfResources = daAssetMigration;
                }
                else
                {
                    var isInfra_Readiness = plannedActivityResource.Any(x => x.Plannedactivityresourceid == model.PlannedActivityResourceId
                   && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.Infra_Readiness);
                    if (isInfra_Readiness)
                    {
                        var plannedDcfIdForInfraReady = dcResource.Where(x => x.Systemtype.Majorsoftwarebuilds.Isvmware == true).Select(x => x.Designcomponentfamilyid)?.Distinct()?.ToList();
                        if (plannedDcfIdForInfraReady != null && plannedDcfIdForInfraReady.Count > 0 && (dto?.DesignComponentFamilyResource != null && dto?.DesignComponentFamilyResource.Count > 0))
                        {
                            dto.PlannedDcfResource = dto.DesignComponentFamilyResource.Where(x => plannedDcfIdForInfraReady.Contains(x.Key)).ToList();

                        }
                    }
                }

                #endregion
            }

            return dto;
        }

        private static ExpressionStarter<Plannedactivities> ApplyFilter(PlannedActivityQueryDto buildFilterDto, bool isServicePlan = false)
        {
            var predicateResult = PredicateBuilder.New<Plannedactivities>();
            var predicateInner = PredicateBuilder.New<Plannedactivities>();

            if(buildFilterDto.Archived == true)
            {
                predicateResult.And(x => x.Archived == true);
            }
            if (buildFilterDto.PlannedDesignComponent != null && buildFilterDto.PlannedDesignComponent.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedDesignComponent)
                {
                    predicateInner.Or(x => x.Designcomponentid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OriginalDesignComponent != null && buildFilterDto.OriginalDesignComponent.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.OriginalDesignComponent)
                {
                    predicateInner.Or(x =>
                        (x.Lcmengineering != null && x.Lcmengineering.Designcomponentid == item) ||
                        (x.Networkelementasplanned != null && x.Networkelementasplanned.Designcomponentid == item)
                    );
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignComponentFamilyName != null && buildFilterDto.DesignComponentFamilyName.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();


                foreach (var item in buildFilterDto.DesignComponentFamilyName)
                {
                    if (isServicePlan != true)
                    {
                        predicateInner.Or(x =>
                              (x.Lcmengineering != null && x.Lcmengineering.Designcomponent.Designcomponentfamilyid == item) ||
                              (x.Networkelementasplanned != null && x.Networkelementasplanned.Designcomponent.Designcomponentfamilyid == item) ||
                              (x.Designaspect != null && x.Designaspect.Designcomponentfamilyid == item)
                          );
                    }
                    else
                    {
                        predicateInner.Or(x => x.Serviceplan.Serviceplandcfmappings.Any(f => f.Dcfid == item));
                    }
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.OpCo)
                {
                    predicateInner.Or(x => x.Opcoid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedActivityResourceId != null && buildFilterDto.PlannedActivityResourceId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedActivityResourceId)
                {
                    predicateInner.Or(x => x.Plannedactivityresourceid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedActivityId != null && buildFilterDto.PlannedActivityId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedActivityId)
                {
                    predicateInner.Or(x => x.Plannedactivityid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.plannedDesignComponentIndex != null && buildFilterDto.plannedDesignComponentIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.plannedDesignComponentIndex)
                {
                    predicateInner.Or(x => x.Designcomponentid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.originalDesignComponentIndex != null && buildFilterDto.originalDesignComponentIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.originalDesignComponentIndex)
                {
                    // predicateInner.Or(x => x.Lcmengineering.Designcomponentid != 0 ? x.Lcmengineering.Designcomponentid == item : x.Networkelementasplanned.Designcomponentid == item);
                    predicateInner.Or(x => (x.Lcmengineeringid != null ? x.Lcmengineering.Designcomponentid : x.Networkelementasplannedid != null ?
                    x.Networkelementasplanned.Designcomponentid : (long?)null) == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.designComponentFamilyIndex != null && buildFilterDto.designComponentFamilyIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.designComponentFamilyIndex)
                {
                    predicateInner.Or(x => x.Designcomponentfamilyid == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RelatesToId != null && buildFilterDto.RelatesToId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.RelatesToId)
                {
                    predicateInner.Or(x => x.Relatestoid == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlanningActivityStatusId != null && buildFilterDto.PlanningActivityStatusId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlanningActivityStatusId)
                {
                    predicateInner.Or(x => x.Planningactivitystatusid == item);
                }
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.LcmEngineeringId != null && buildFilterDto.LcmEngineeringId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.LcmEngineeringId)
                {
                    predicateInner.Or(x => x.Lcmengineeringid == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedActivityDescription != null && buildFilterDto.PlannedActivityDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedActivityDescription)
                {
                    predicateInner.Or(x => x.Plannedactivity.Contains(item));
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ActivityStatusId != null && buildFilterDto.ActivityStatusId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ActivityStatusId)
                {
                    predicateInner.Or(x => x.Activitystatusid == item);
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DeliveryProjectName != null && buildFilterDto.DeliveryProjectName.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.DeliveryProjectName)
                {
                    predicateInner.Or(x => x.Deliveryprojectname == item);
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Benefits != null && buildFilterDto.Benefits.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.Benefits)
                {
                    predicateInner.Or(x => x.Benefit.Benefitid == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Driver != null && buildFilterDto.Driver.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.Driver)
                {
                    predicateInner.Or(x => x.Driver.Driverid == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BudgetAvailability != null && buildFilterDto.BudgetAvailability.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.BudgetAvailability)
                {
                    predicateInner.Or(x => x.Budgetavailability.Description == item);
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ResponsibilityPhaseId != null && buildFilterDto.ResponsibilityPhaseId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ResponsibilityPhaseId)
                {
                    predicateInner.Or(x => x.Responsibilityphaseid == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RiskEngineeringEvaluation != null && buildFilterDto.RiskEngineeringEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.RiskEngineeringEvaluation)
                {
                    predicateInner.Or(x => x.Engineeringrisk.Description == item);
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.RiskEngineeringNotes != null && buildFilterDto.RiskEngineeringNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.RiskEngineeringNotes)
                {
                    predicateInner.Or(x => x.Riskengineeringnotes == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RiskOperationalEvaluation != null && buildFilterDto.RiskOperationalEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.RiskOperationalEvaluation)
                {
                    predicateInner.Or(x => x.Engineeringrisk.Description == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalApproval != null && buildFilterDto.LocalApproval.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.LocalApproval)
                {
                    predicateInner.Or(x => x.Localapproval == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BudgetValueGrid != null && buildFilterDto.BudgetValueGrid.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.BudgetValueGrid)
                {
                    var value = decimal.TryParse(item, out var test);
                    if (value)
                    {
                        var decimalValue = decimal.Parse(item);

                        predicateInner.Or(x => x.Budgetvalue == decimalValue);
                    }
                    else
                    {
                        predicateInner.Or(x => (x.Budgetvalue.ToString() + x.Currency) == item);
                    }



                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlanningRisk != null && buildFilterDto.PlanningRisk.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlanningRisk)
                {
                    predicateInner.Or(x => x.PlanningriskNavigation.Planningriskid == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProjectStatus != null && buildFilterDto.ProjectStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ProjectStatus)
                {
                    predicateInner.Or(x => x.Projectstatus == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DeliveryProjectId != null && buildFilterDto.DeliveryProjectId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.DeliveryProjectId)
                {
                    predicateInner.Or(x => x.Deliveryprojectid == item);
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.BudgetTrackingId != null && buildFilterDto.BudgetTrackingId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.BudgetTrackingId)
                {
                    predicateInner.Or(x => x.Budgettrackingid == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedImplementationYear != null && buildFilterDto.PlannedImplementationYear.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedImplementationYear)
                {
                    predicateInner.Or(x => x.Plannedimplementationyear == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DeliveryStatusId != null && buildFilterDto.DeliveryStatusId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.DeliveryStatusId)
                {
                    predicateInner.Or(x => x.Deliverystatusid == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RiskOperationalNotes != null && buildFilterDto.RiskOperationalNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.RiskOperationalNotes)
                {
                    predicateInner.Or(x => x.Riskoperationalnotes == item);
                }
                predicateResult.And(predicateInner);
            }



            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.PlannedCompletionValue != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                if (buildFilterDto.PlannedCompletionValue.StartDate != null)
                    predicateInner.And(x => x.Plannedcompletion >= buildFilterDto.PlannedCompletionValue.StartDate);
                if (buildFilterDto.PlannedCompletionValue.EndDate != null)
                    predicateInner.And(x => x.Plannedcompletion <= buildFilterDto.PlannedCompletionValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.StartDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                if (buildFilterDto.StartDateValue.StartDate != null)
                    predicateInner.And(x => x.Startdate >= buildFilterDto.StartDateValue.StartDate);
                if (buildFilterDto.StartDateValue.EndDate != null)
                    predicateInner.And(x => x.Startdate <= buildFilterDto.StartDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Program != null && buildFilterDto.Program.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.Program)
                    predicateInner.Or(x => x.ProgramNavigation.Programdescription == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ProjectOwner != null && buildFilterDto.ProjectOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ProjectOwner)
                    predicateInner.Or(x => x.Projectowner == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DeliveryTrackingId != null && buildFilterDto.DeliveryTrackingId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.DeliveryTrackingId)
                    predicateInner.Or(x => x.Deliverytrackings.FirstOrDefault() != null && x.Deliverytrackings.FirstOrDefault().Id == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DeliveryPlanAvailable != null && buildFilterDto.DeliveryPlanAvailable.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.DeliveryPlanAvailable)
                    predicateInner.Or(x => x.Deliveryplanavailable == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IsPAReleaseDetailUnknown != null && buildFilterDto.IsPAReleaseDetailUnknown.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.IsPAReleaseDetailUnknown)
                    predicateInner.Or(x => x.Ispareleasedetailunknown == item);
                predicateResult.And(predicateInner);
            }

            #region Ticket 685 Dev - #674 SettingsUpdatePlanedActivity - Display  Planned Activity Associated/Linked Table Details  - Ex : LCM, DA, Assets

            if (buildFilterDto.ForAddAsset != null && buildFilterDto.ForAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ForAddAsset)
                {
                    if (item.Value.ToString().ToLower() == ConstantValueFilter.False)
                        predicateInner.Or(x => x.Foraddasset == null);


                    predicateInner.Or(x => x.Foraddasset == item.Value);
                }

                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ForEditAsset != null && buildFilterDto.ForEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ForEditAsset)
                {
                    if (item.Value.ToString().ToLower() == ConstantValueFilter.False)
                        predicateInner.Or(x => x.Foreditasset == null);

                    predicateInner.Or(x => x.Foreditasset == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ForLcmLink != null && buildFilterDto.ForLcmLink.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ForLcmLink)
                    predicateInner.Or(x => (item.Value.ToString().ToLower() == ConstantValueFilter.False)
                    ? x.Lcmengineeringid == null : x.Lcmengineeringid != null);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ForDesignAspectLink != null && buildFilterDto.ForDesignAspectLink.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ForDesignAspectLink)
                    predicateInner.Or(x => (item.Value.ToString().ToLower() == ConstantValueFilter.False) ? x.Designaspectid == null : x.Designaspectid != null);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ForServicePlanLink != null && buildFilterDto.ForServicePlanLink.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ForServicePlanLink)
                    predicateInner.Or(x => (item.Value.ToString().ToLower() == ConstantValueFilter.False) ? x.Serviceplanid == null : x.Serviceplanid != null);
                predicateResult.And(predicateInner);
            }
            #endregion

            #region Ticket 751 PPM Import

            if (buildFilterDto.DeliveryProjectPpmId != null && buildFilterDto.DeliveryProjectPpmId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.DeliveryProjectPpmId)
                {
                    predicateInner.Or(x => x.Deliveryprojectid == item);
                }
                predicateResult.And(predicateInner);
            }

            #endregion

            if (buildFilterDto.PlannedBuildBagDescription?.Any() == true)
            {
                ExpressionStarter<Plannedactivities> descriptionPredicate = PredicateBuilder.New<Plannedactivities>();
                foreach (long description in buildFilterDto.PlannedBuildBagDescription)
                {
                    _ = descriptionPredicate.Or(x => x.Buildbag.Buildbagid == description);
                }

                _ = predicateResult.And(descriptionPredicate);
            }
            if (buildFilterDto.CurrentBuildBagDescription?.Any() == true)
            {
                ExpressionStarter<Plannedactivities> descriptionPredicate = PredicateBuilder.New<Plannedactivities>();
                foreach (long description in buildFilterDto.CurrentBuildBagDescription)
                {
                    _ = descriptionPredicate.Or(x => x.Lcmengineering.Buildbag.Buildbagid == description);
                }

                _ = predicateResult.And(descriptionPredicate);
            }
            if (buildFilterDto.PlannedActivityResourceId != null && buildFilterDto.PlannedActivityResourceId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedActivityResourceId)
                {
                    predicateInner.Or(x => x.Plannedactivityresourceid == item);
                }

                predicateResult.And(predicateInner);
            }

            #region //BGT Newly Columns

            //if (buildFilterDto.PlannedActivityCategory != null && buildFilterDto.PlannedActivityCategory.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Plannedactivities>();
            //    foreach (var item in buildFilterDto.PlannedActivityCategory)
            //    {
            //        predicateInner.Or(x => x.Plannedactivitycategory == item);
            //    }

            //    predicateResult.And(predicateInner);
            //}
            if (buildFilterDto.PlannedActivityteam != null && buildFilterDto.PlannedActivityteam.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedActivityteam)
                {
                    predicateInner.Or(x => x.Plannedactivityteam == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Priority != null && buildFilterDto.Priority.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.Priority)
                {
                    predicateInner.Or(x => x.Priority == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LcmCategories != null && buildFilterDto.LcmCategories.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.LcmCategories)
                {
                    predicateInner.Or(x => x.Lcmcategories == item);
                }

                predicateResult.And(predicateInner);
            }

            #endregion
            if (buildFilterDto.ProjectDescription != null && buildFilterDto.ProjectDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ProjectDescription)
                {
                    predicateInner.Or(x => x.Projectdescription == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PreBaseLineDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                if (buildFilterDto.PreBaseLineDateValue.StartDate != null)
                    predicateInner.And(x => x.Prebaselinedate >= buildFilterDto.PreBaseLineDateValue.StartDate);
                if (buildFilterDto.PreBaseLineDateValue.EndDate != null)
                    predicateInner.And(x => x.Prebaselinedate <= buildFilterDto.PreBaseLineDateValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ActivityDetails != null && buildFilterDto.ActivityDetails.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ActivityDetails)
                {
                    predicateInner.Or(x => x.Activitydetails == item);
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.IsServicePlan != null && buildFilterDto.IsServicePlan.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.IsServicePlan)
                {
                    predicateInner.Or(x => x.Isserviceplan == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProgramName != null && buildFilterDto.ProgramName.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ProgramName)
                {
                    predicateInner.Or(x => x.Deliveryprojectname == item);
                }
                predicateResult.And(predicateInner);
            }
            //if (buildFilterDto.ServiceMaster != null && buildFilterDto.ServiceMaster.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Plannedactivities>();
            //    foreach (var item in buildFilterDto.ServiceMaster)
            //    {
            //        predicateInner.Or(x => x.Serviceplan.Servicemasterid == item);
            //    }
            //    predicateResult.And(predicateInner);
            //}



            return predicateResult;
        }
        private static ExpressionStarter<Plannedactivities> ApplyArchivedFilter(ArchivedPlannedActivityQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Plannedactivities>();

            var predicateInner = PredicateBuilder.New<Plannedactivities>();
            if (buildFilterDto.ForLcmLink != null && buildFilterDto.ForLcmLink.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ForLcmLink)
                    predicateInner.Or(x => (item.Value.ToString().ToLower() == ConstantValueFilter.False)
                    ? x.Lcmengineeringid == null : x.Lcmengineeringid != null);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ForDesignAspectLink != null && buildFilterDto.ForDesignAspectLink.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ForDesignAspectLink)
                    predicateInner.Or(x => (item.Value.ToString().ToLower() == ConstantValueFilter.False) ? x.Designaspectid == null : x.Designaspectid != null);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ForServicePlanLink != null && buildFilterDto.ForServicePlanLink.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ForServicePlanLink)
                    predicateInner.Or(x => (item.Value.ToString().ToLower() == ConstantValueFilter.False) ? x.Serviceplanid == null : x.Serviceplanid != null);
                predicateResult.And(predicateInner);
            }
            if(buildFilterDto.AddEditAssetFilter != null && buildFilterDto.AddEditAssetFilter ==true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                predicateInner.Or(x => x.Foraddasset==true || x.Foreditasset == true);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedActivityId != null && buildFilterDto.PlannedActivityId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedActivityId)
                    predicateInner.Or(x => x.Plannedactivityid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedActivityResourceId != null && buildFilterDto.PlannedActivityResourceId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedActivityResourceId)
                    predicateInner.Or(x => x.Plannedactivityresourceid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ProjectOwner != null && buildFilterDto.ProjectOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ProjectOwner)
                    predicateInner.Or(x => x.Projectowner == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.Program != null && buildFilterDto.Program.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.Program)
                    predicateInner.Or(x => x.ProgramNavigation.Programdescription == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OriginalDesignComponent != null && buildFilterDto.OriginalDesignComponent.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.OriginalDesignComponent)
                {
                    predicateInner.Or(x =>
                        (x.Lcmengineering != null && x.Lcmengineering.Designcomponentid == item) ||
                        (x.Networkelementasplanned != null && x.Networkelementasplanned.Designcomponentid == item)
                    );
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignComponentFamilyName != null && buildFilterDto.DesignComponentFamilyName.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.DesignComponentFamilyName)
                {
                    predicateInner.Or(x =>
                          (x.Lcmengineering != null && x.Lcmengineering.Designcomponent.Designcomponentfamilyid == item) ||
                          (x.Networkelementasplanned != null && x.Networkelementasplanned.Designcomponent.Designcomponentfamilyid == item) ||
                          (x.Designaspect != null && x.Designaspect.Designcomponentfamilyid == item)
                      );
                }

                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.PlannedActivityDesignComponentId != null && buildFilterDto.PlannedActivityDesignComponentId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedActivityDesignComponentId)
                {
                    predicateInner.Or(x => x.Designcomponentid == item);
                }

                predicateResult.And(predicateInner);
            }



            if (buildFilterDto.PlannedImplementationYear != null && buildFilterDto.PlannedImplementationYear.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedImplementationYear)
                {
                    predicateInner.Or(x => x.Plannedimplementationyear == item);
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.StartDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                if (buildFilterDto.StartDateValue.StartDate != null)
                    predicateInner.And(x => x.Startdate >= buildFilterDto.StartDateValue.StartDate);
                if (buildFilterDto.StartDateValue.EndDate != null)
                    predicateInner.And(x => x.Startdate <= buildFilterDto.StartDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.PlannedCompletion != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                if (buildFilterDto.PlannedCompletion.StartDate != null)
                    predicateInner.And(x => x.Plannedcompletion >= buildFilterDto.PlannedCompletion.StartDate);
                if (buildFilterDto.PlannedCompletion.EndDate != null)
                    predicateInner.And(x => x.Plannedcompletion <= buildFilterDto.PlannedCompletion.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            #region //Ticket 793 PPM ID value should present for Archived PAs too in LCM export
            if (buildFilterDto.DeliveryProjectPpmId != null && buildFilterDto.DeliveryProjectPpmId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.DeliveryProjectPpmId)
                {
                    predicateInner.Or(x => x.Deliveryprojectid == item);
                }
                predicateResult.And(predicateInner);
            }
            #endregion

            if (buildFilterDto.PlannedBuildBagDescription?.Any() == true)
            {
                ExpressionStarter<Plannedactivities> descriptionPredicate = PredicateBuilder.New<Plannedactivities>();
                foreach (long description in buildFilterDto.PlannedBuildBagDescription)
                {
                    _ = descriptionPredicate.Or(x => x.Buildbag.Buildbagid == description);
                }

                _ = predicateResult.And(descriptionPredicate);
            }
            if (buildFilterDto.CurrentBuildBagDescription?.Any() == true)
            {
                ExpressionStarter<Plannedactivities> descriptionPredicate = PredicateBuilder.New<Plannedactivities>();
                foreach (long description in buildFilterDto.CurrentBuildBagDescription)
                {
                    _ = descriptionPredicate.Or(x => x.Lcmengineering.Buildbag.Buildbagid == description);
                }

                _ = predicateResult.And(descriptionPredicate);
            }
            if (buildFilterDto.ModificationDate != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                if (buildFilterDto.ModificationDate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.ModificationDate.StartDate);
                if (buildFilterDto.ModificationDate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.ModificationDate.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public QueryResultDto<ArchivedPlannedActivityDtoGrid> FindArchivedWithCondition(ArchivedPlannedActivityQueryDto plannedActivityFilterDto)
        {
            var predicateResult = ApplyArchivedFilter(plannedActivityFilterDto);

            var archivePredicate = PredicateBuilder.New<Plannedactivities>();
            predicateResult.And(x => x.Archived == true);

            var items = GetQueryForArchivedPAs(predicateResult, plannedActivityFilterDto.Deleted ?? false, plannedActivityFilterDto);


            var rtn = new QueryResultDto<ArchivedPlannedActivityDtoGrid>(new GenerateRenderForGrid<ArchivedPlannedActivityDtoGrid>(_columnManager)) { };

            IEnumerable<Plannedactivities> data = items.OrderByDescending(x => x.Modificationdate).ToList();
            IEnumerable<ArchivedPlannedActivityDtoGrid> plannedActivitiesResult;
            plannedActivitiesResult = data.Select(s =>
           new ArchivedPlannedActivityDtoGrid
           {
               OpCo = s.Opco.Opco,
               PlannedActivityResourceId = s.Plannedactivityresource.Plannedactivityresource,
               PlannedActivityId = s.Plannedactivityid,
               DesignComponentFamilyName = s.toOriginalDesignComponentFamilyDescriptionFromArchivedPAs(_repositoryWrapper),
               OriginalDesignComponent = s.toOriginalDesignComponentDescriptionToArchivedPAs(_repositoryWrapper),
               ActivityDetails = s.Activitydetails == null && s.Activitydetails == "" ? string.Empty : s.Activitydetails,
               PlannedActivityDesignComponentId = s.Designcomponent != null ? CAM.Entities.Mappers.Entity.DesignComponentMapper.GetDesignComponentMapper(s.Designcomponent) != null ? CAM.Entities.Mappers.Entity.DesignComponentMapper.GetDesignComponentMapper(s.Designcomponent).toDesignComponentNameLcm(_repositoryWrapper) : "" : null,
               StartDateValue = s.Startdate != null ? s.Startdate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty,
               PlannedCompletion = s.Plannedcompletion != null ? s.Plannedcompletion.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty,
               PlannedImplementationYear = string.IsNullOrEmpty(s.Plannedimplementationyear.ToString()) ? string.Empty : s.Plannedimplementationyear.ToString(),
               Archived = s.Archived,
               VerticalFilterDto = (s.Networkelementasplannedid != null && s.Networkelementasplanned?.Networkelementasplannedsubdomainspoc != null) ?
                                  GetVerticalDetails(s?.Networkelementasplanned?.Networkelementasplannedsubdomainspoc?.Select(x => x?.Subdomainspocid).ToList(),
                                  s?.Networkelementasplanned?.Opcoid, null)
                                  : (s.Lcmengineeringid != null && s.Lcmengineering?.Lcmengineeringsubdomainspoc != null) ?
                                  GetVerticalDetails(s?.Lcmengineering?.Lcmengineeringsubdomainspoc?.Select(x => x?.Subdomainspocid).ToList(),
                                  s?.Lcmengineering?.Opcoid, null)
                                  : (s.Designaspectid != null && s.Designaspect?.Designcomponentfamily?.Designcomponents != null) ?
                                  GetVerticalDetails(new List<int?>(), s?.Designaspect?.Opcoid, s.Designaspect)
                                  : s.Serviceplanid != null ?
                                 GetVerticalDetails(new List<int?>(), 0,null,s.Serviceplan):null,

               ///Ticket 793 PPM ID value should present for Archived PAs too in LCM export
               DeliveryProjectPpmId = s.Deliveryprojectid,
               ModificationDate = s.Modificationdate,
               PlannedBuildBagDescription = _commonManager.GetBuildBagDescription(s.Buildbag),
               CurrentBuildBagDescription = (s?.Lcmengineering != null) ? _commonManager.GetBuildBagDescription(s.Lcmengineering.Buildbag) : string.Empty,
           })?.ToList();

            foreach (var item in plannedActivitiesResult)
            {
                if (item.VerticalFilterDto != null && item.VerticalFilterDto.Count() > 0)
                {
                    item.VerticalName = string.Join(",", item.VerticalFilterDto.Select(x => x.Value)?.Distinct().ToList() ?? new List<string>());
                }

            }

            if (plannedActivityFilterDto.VerticalName?.Any() == true && !plannedActivityFilterDto.VerticalName.Contains("yes"))
            {
                plannedActivitiesResult = plannedActivitiesResult.Where(x =>
                x.VerticalFilterDto != null && x.VerticalFilterDto.Any(c => x.VerticalFilterDto != null && plannedActivityFilterDto.VerticalName.Contains(c.Key.ToString())));
            }
            else if (plannedActivityFilterDto.VerticalName?.Any() == true && plannedActivityFilterDto.VerticalName.Count() == 1
                && plannedActivityFilterDto.VerticalName.Contains("yes"))
            {
                plannedActivitiesResult = plannedActivitiesResult.Where(x => x.VerticalFilterDto==null|| (x.VerticalFilterDto!= null && x.VerticalFilterDto.Count() == 0));
            }
            else if (plannedActivityFilterDto.VerticalName?.Any() == true && plannedActivityFilterDto.VerticalName.Count() > 1 && plannedActivityFilterDto.VerticalName.Contains("yes"))
            {
                var nullVerticals = plannedActivityFilterDto.VerticalName.Contains("yes") ?
                    plannedActivitiesResult.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0)) : null;
                var verticalFilter = plannedActivitiesResult
                                    .Where(x => x.VerticalFilterDto != null &&
                                    x.VerticalFilterDto.Any(c => plannedActivityFilterDto.VerticalName.Where(t => t != "yes").Contains(c.Key.ToString())));
                plannedActivitiesResult = nullVerticals?.Any() == true && verticalFilter?.Any() == true ?
                    nullVerticals.Concat(verticalFilter) : nullVerticals?.Any() == true && verticalFilter
                    ?.Any() == false ? nullVerticals
                    : nullVerticals?.Any() == false && verticalFilter?.Any() == true ? verticalFilter : null;
            }

            rtn.TotalItems = plannedActivitiesResult.Count();

            if (plannedActivityFilterDto.PageSize == 0)
            {
                plannedActivityFilterDto.PageSize = rtn.TotalItems;
                plannedActivityFilterDto.Page = 1;
            }

            plannedActivitiesResult = plannedActivitiesResult.OrderByDescending(x => x.ModificationDate).Skip((plannedActivityFilterDto.Page - 1) * plannedActivityFilterDto.PageSize)
                .Take(plannedActivityFilterDto.PageSize);

            rtn.Items = plannedActivitiesResult.ToArray();

            if (plannedActivityFilterDto.ForServicePlanLink != null && plannedActivityFilterDto.ForServicePlanLink.Any(x => x == true))
            {
                var filtersToHide = new HashSet<string>
                {
                    ConstantValueFilter.OriginalDesignComponent.ToLower(),
                    ConstantValueFilter.PlannedActivityDesignComponentId.ToLower(),
                    ConstantValueFilter.DesignComponentFamilyName.ToLower(),
                };

                // Remove items  
                rtn.GridRender.Render = rtn.GridRender.Render
                    .Where(c => !filtersToHide.Contains(c.PropertyName.ToLower()))
                    .ToList();
            }
            return rtn;
        }
        public List<FilterValueDto> ArchivedPlannedActivityFilter(string propertyName, string propertyFilter, ArchivedPlannedActivityQueryDto buildFilterDto,bool isAdmin)
        {
            var predicateResult = ApplyArchivedFilter(buildFilterDto);

            var archivePredicate = PredicateBuilder.New<Plannedactivities>();
            predicateResult.And(x => x.Archived == true);

            var query = GetQueryForArchivedPAs(predicateResult, buildFilterDto.Deleted ?? false, buildFilterDto);

            if (propertyName == "verticalName")
            {
                var verticalFilterDto = new List<FilterValueDto>();
                var defaultItem = new FilterValueDto { Value = "yes", Text = "---" };
                foreach (var item in query)
                {
                    List<FilterValueDto> tempList = null;
                    if (item.Networkelementasplannedid != null)
                    {
                        tempList=GetVerticalDetails(item?.Networkelementasplanned?.Networkelementasplannedsubdomainspoc?
                                    .Select(x => x?.Subdomainspocid).ToList(),
                                    item?.Networkelementasplanned?.Opcoid)?.Select(t => new FilterValueDto
                                    {
                                        Text = t.Value.ToString(),
                                        Value = t.Key.ToString()
                                    })
                                    ?.Distinct()
                                    ?.ToList();
                    }
                    else if (item.Lcmengineeringid != null)
                    {
                        tempList = GetVerticalDetails(item?.Lcmengineering?.Lcmengineeringsubdomainspoc?
                                    .Select(x => x?.Subdomainspocid).ToList(),
                                    item?.Networkelementasplanned?.Opcoid)?.Select(t => new FilterValueDto
                                    {
                                        Text = t.Value.ToString(),
                                        Value = t.Key.ToString()
                                    })
                                    ?.Distinct()
                                    ?.ToList();
                    }
                    else if (item.Designaspectid != null)
                    {
                        tempList = GetVerticalDetails(new List<int?>(),
                                    item?.Designaspect?.Opcoid,item.Designaspect)?.Select(t => new FilterValueDto
                                    {
                                        Text = t.Value.ToString(),
                                        Value = t.Key.ToString()
                                    })
                                    ?.Distinct()
                                    ?.ToList();
                    }
                    else if (item.Serviceplanid != null)
                    {
                        tempList = GetVerticalDetails(new List<int?>(), 0,null,item?.Serviceplan)?.Select(t => new FilterValueDto
                        {
                            Text = t.Value.ToString(),
                            Value = t.Key.ToString()
                        })?.Distinct()?.ToList();
                    }
                    if (tempList != null && tempList.Any()==true)
                    {
                        var secTempList = verticalFilterDto?.Any() == true ? tempList.Where(x => !verticalFilterDto.Any(y => y.Value == x.Value)): tempList;
                        verticalFilterDto.AddRange(secTempList);
                    }
                    else if (tempList == null || tempList != null && tempList.Count() == 0)
                    {
                        if (verticalFilterDto !=null && !verticalFilterDto.Any(x => x.Value == defaultItem.Value))
                        {
                            verticalFilterDto.Add(defaultItem);
                        }
                    }
                }

                if (!isAdmin && (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count > 0) && propertyName == "verticalName")
                {
                    verticalFilterDto = verticalFilterDto.Where(x => buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();
                }
                return verticalFilterDto;



            }

            var rtn = propertyName switch
            {
                "plannedActivityId" => query.Select(x => new FilterValueDto { Text = x.Plannedactivityid.ToString(), Value = x.Plannedactivityid.ToString() }).Distinct().ToList(),
                "archived" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(p => p.Archived != null)
                        .Select(p => new FilterValueDto { Text = p.Archived.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO, Value = p.Archived.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.Archived != null && p.Archived.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO).Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Archived.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO, Value = p.Archived.ToString() }).Distinct().ToList(),

                "plannedActivityDesignComponentId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Where(x => x.Designcomponent != null)
                    .Select(p => new FilterValueDto(p.Designcomponentid, DesignComponentMapper.GetDesignComponentMapper(p.Designcomponent).toDesignComponentNameLcm(_repositoryWrapper))).Distinct().ToList()
                    : query
                        .ToList()
                        .Where(x => x.Designcomponent != null &&
                           DesignComponentMapper.GetDesignComponentMapper(x.Designcomponent).toDesignComponentNameLcm(_repositoryWrapper).ToUpper().Contains(propertyFilter.ToUpper())
                        ).Select(p => new FilterValueDto(p.Designcomponentid, DesignComponentMapper.GetDesignComponentMapper(p.Designcomponent).toDesignComponentNameLcm(_repositoryWrapper)))
                        .Distinct().ToList(),

                "designComponentFamilyName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.Designcomponentfamily != null || x.Designcomponent != null)
                     .Select(p => new FilterValueDto(p.Designcomponentfamily != null ? p.Designcomponentfamilyid : p.Designcomponent.Designcomponentfamilyid, p.Designcomponentfamily != null ? p.Designcomponentfamily.DCFName(_repositoryWrapper) : p.Designcomponent.Designcomponentfamily.DCFName(_repositoryWrapper))).Distinct().ToList()
                    : query
                        .Where(x => (x.Designcomponentfamily != null && x.Designcomponentfamilyid.ToString().Contains(propertyFilter.ToUpper()))
                        || (x.Designcomponent != null && x.Designcomponent.Designcomponentfamilyid.ToString().Contains(propertyFilter.ToUpper())))
                        .Select(p => new FilterValueDto(p.Designcomponentfamily != null ? p.Designcomponentfamilyid.ToString() : p.Designcomponent.Designcomponentfamilyid.ToString(), p.Designcomponentfamily != null ? p.Designcomponentfamily.DCFName(_repositoryWrapper) : p.Designcomponent.Designcomponentfamily.DCFName(_repositoryWrapper))).Distinct().ToList(),

                "originalDesignComponent" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Where(p => p.Networkelementasplanned != null || p.Lcmengineering != null)
                    .Select(p => new FilterValueDto(
                        (p.Networkelementasplanned != null) ? p.Networkelementasplanned.Designcomponentid : p.Lcmengineering.Designcomponentid,
                        p.toOriginalDesignComponentDescriptionToArchivedPAs(_repositoryWrapper)
                    )).Distinct().ToList()
                    : query
                        .ToList()
                        .Where(x => x.Designcomponent != null &&
                            x.toOriginalDesignComponentDescriptionToArchivedPAs(_repositoryWrapper).ToUpper().Contains(propertyFilter.ToUpper()) &&
                            (x.Networkelementasplanned != null || x.Lcmengineering != null)
                        )
                    .Select(p => new FilterValueDto(
                        (p.Networkelementasplanned != null) ? p.Networkelementasplanned.Designcomponentid : p.Lcmengineering.Designcomponentid,
                        p.toOriginalDesignComponentDescriptionToArchivedPAs(_repositoryWrapper)
                    )).Distinct().ToList(),
                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(p => p.Opco != null).Select(p => new FilterValueDto
                    {
                        Text = p.Opco.Opco,
                        Value = p.Opcoid.ToString()
                    }).Distinct().ToList()
                    : query.ToList().Where(x => x.Opco != null &&
                            x.Opco.Opco.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Opco.Opco,
                        Value = p.Opcoid.ToString()
                    }).Distinct().ToList(),
                "activityDetails" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Activitydetails, p.Activitydetails)).Distinct().ToList()
                    : query.Where(x => x.Activitydetails.Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.Activitydetails)).Distinct().ToList(),
                "plannedActivityResourceId" => string.IsNullOrEmpty(propertyFilter)
                  ? query.Select(p => new FilterValueDto(p.Plannedactivityresourceid, p.Plannedactivityresource.Plannedactivityresource)).Distinct().ToList()
                  : query.Where(x => x.Plannedactivityresource.Plannedactivityresource.Contains(propertyFilter))
                  .Select(p => new FilterValueDto(p.Plannedactivityresourceid, p.Plannedactivityresource.Plannedactivityresource)).Distinct().ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList()
                    : query.Where(x => x.ModificationuserNavigation.Email.Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList(),
                "plannedImplementationYear" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto(p.Plannedimplementationyear)).Distinct().ToList()
               : query.Where(x => x.Plannedimplementationyear.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.Plannedimplementationyear)).Distinct().ToList(),
                "program" => query.Where(p => p.ProgramNavigation.Programdescription != null).Select(p => new FilterValueDto(p.ProgramNavigation.Programdescription)).Distinct().ToList(),
                "projectOwner" => query.Where(p => p.Projectowner != null).Select(p => new FilterValueDto(p.Projectowner)).Distinct().ToList(),

                "deliveryProjectPpmId" => string.IsNullOrEmpty(propertyFilter)
                                   ? query.Select(p => new FilterValueDto(p.Deliveryprojectid)).Distinct().ToList()
                                  : query.Where(x => x.Deliveryprojectid.Contains(propertyFilter)).Select(p =>
                                   new FilterValueDto(p.Deliveryprojectid)).Distinct().ToList(),

                "plannedBuildBagDescription" => query
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Buildbag.Bagdescription.Contains(propertyFilter))
                 .Select(p => new FilterValueDto { Text = _commonManager.GetBuildBagDescription(p.Buildbag), Value = p.Buildbagid.ToString() })
                 .Distinct()
                 .ToList(),
                "currentBuildBagDescription" => query
               .Where(x => x.Lcmengineering != null && (string.IsNullOrEmpty(propertyFilter) || x.Lcmengineering.Buildbag.Bagdescription.Contains(propertyFilter)))
               .Select(p => new FilterValueDto { Text = _commonManager.GetBuildBagDescription(p.Lcmengineering.Buildbag), Value = p.Lcmengineering.Buildbagid.ToString() })
               .Distinct()
               .ToList(),
                _ => new List<FilterValueDto>()
            };
            return rtn;
        }

        public async Task<QueryResultDto<PlannedActivityDtoGrid>> FindWithCondition(PlannedActivityQueryDto plannedActivityFilterDto, bool isServicePlanPa = false)
        {
            var predicateResult = ApplyFilter(plannedActivityFilterDto, isServicePlanPa);

            if (predicateResult.ToString() == "f => False")
            {
                if (plannedActivityFilterDto.ForLcm == true)
                    predicateResult = predicateResult.And(x => x.Lcmengineeringid != null);
                if (plannedActivityFilterDto.ForNetwork == true)
                    predicateResult = predicateResult.Or(x => x.Networkelementasplannedid != null);
                if (plannedActivityFilterDto.ForDesignAspect == true)
                    predicateResult = predicateResult.Or(x => x.Designaspectid != null);
            }

            var archivePredicate = PredicateBuilder.New<Plannedactivities>();
            predicateResult.And(x => x.Archived == plannedActivityFilterDto.Archived);

            var query = await GetQuery(predicateResult, plannedActivityFilterDto,isServicePlanPa);

            #region Ticket 729 Filter Both For Edit And Add Asset Records in PA Grid while click Asset button in Manage Network Menu
            if (plannedActivityFilterDto.AddEditAssetFilter == true)
                query = query.Where(x => x.ForAddAsset == true || x.ForEditAsset == true)
               .ApplyOrdering(plannedActivityFilterDto, GetColumnsMap());
            else
                query = query.ApplyOrdering(plannedActivityFilterDto, GetColumnsMap()); ;

            #endregion

            if (plannedActivityFilterDto.VerticalName?.Any() == true && !plannedActivityFilterDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Any(c => x.VerticalFilterDto!=null && plannedActivityFilterDto.VerticalName.Contains(c.Key.ToString())));
            }
            else if (plannedActivityFilterDto.VerticalName?.Any() == true && plannedActivityFilterDto.VerticalName.Count() == 1 && plannedActivityFilterDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));
            }
            else if (plannedActivityFilterDto.VerticalName?.Any() == true && plannedActivityFilterDto.VerticalName.Count() >1 && plannedActivityFilterDto.VerticalName.Contains("yes"))
            {
                var nullVerticals = plannedActivityFilterDto.VerticalName.Contains("yes")?
                    query.Where(x => x.VerticalFilterDto == null ||(x.VerticalFilterDto != null && x.VerticalFilterDto.Count()==0)) :null;
                var verticalFilter = query
                                    .Where(x => x.VerticalFilterDto != null &&
                                    x.VerticalFilterDto.Any(c => plannedActivityFilterDto.VerticalName.Where(t => t != "yes").Contains(c.Key.ToString())));
                query = nullVerticals?.Any() == true && verticalFilter?.Any() == true ?
                    nullVerticals.Concat(verticalFilter) : nullVerticals?.Any() == true && verticalFilter
                    ?.Any() == false ? nullVerticals
                    : nullVerticals?.Any() == false && verticalFilter?.Any() == true ? verticalFilter : null;
            }

            if (plannedActivityFilterDto.originalDesignComponentIndex != null && plannedActivityFilterDto.originalDesignComponentIndex.Any())
            {
                query = query.Where(x => plannedActivityFilterDto.originalDesignComponentIndex.Contains(x.LcmEngineeringId != null ? x.LcmEngineering.DesignComponentId
                    : x.NetworkElementAsPlannedId != null ? x.NetworkElementAsPlanned.DesignComponentId : 0));
            }

            var rtn = new QueryResultDto<PlannedActivityDtoGrid>(new GenerateRenderForGrid<PlannedActivityDtoGrid>(_columnManager))
            {
                TotalItems = query.ToList().Count()
            };
            var data = (List<PlannedActivity>)query.ApplyPaging(plannedActivityFilterDto).ToList();

            var plannedActivitydResult = _mapper.Map<IEnumerable<PlannedActivityDtoGrid>>(data);

            rtn.Items = plannedActivitydResult.ToArray();
            if (plannedActivityFilterDto.ForServicePlanLink !=null && plannedActivityFilterDto.ForServicePlanLink.Any(x=>x==true))
            {
                var filtersToHide = new HashSet<string>
                {
                    ConstantValueFilter.OriginalDesignComponent.ToLower(),
                    ConstantValueFilter.OriginalDesignComponentIndex.ToLower(),
                    ConstantValueFilter.PlannedDesignComponent.ToLower(),
                    ConstantValueFilter.PlannedDesignComponentIndex.ToLower(),
                    ConstantValueFilter.DesignComponentFamilyIndex.ToLower(),
                    ConstantValueFilter.DesignComponentFamilyName.ToLower(),
                    ConstantValueFilter.PlannedDesignComponentName.ToLower(),
                };

                // Remove items  
                rtn.GridRender.Render = rtn.GridRender.Render
                    .Where(c => !filtersToHide.Contains(c.PropertyName.ToLower()))
                    .ToList();
            }


            return rtn;
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, PlannedActivityQueryDto buildFilterDto, bool isServicePlan = false,bool isAdmin=false)

        {
            var predicateResult = ApplyFilter(buildFilterDto, isServicePlan);


            if (predicateResult.ToString() == "f => False")
            {
                if (buildFilterDto.ForLcm == true)
                    predicateResult = predicateResult.And(x => x.Lcmengineeringid != null);
                if (buildFilterDto.ForNetwork == true)
                    predicateResult = predicateResult.Or(x => x.Networkelementasplannedid != null);
                if (buildFilterDto.ForDesignAspect == true)
                    predicateResult = predicateResult.Or(x => x.Designaspectid != null);
            }

            var archivePredicate = PredicateBuilder.New<Plannedactivities>();
            predicateResult.And(x => x.Archived == buildFilterDto.Archived);

            var query = await GetQuery(predicateResult, buildFilterDto,isServicePlan);

            if (buildFilterDto.VerticalName?.Any() == true && !buildFilterDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Any(c => x.VerticalFilterDto != null && buildFilterDto.VerticalName.Contains(c.Key.ToString())));
            }
            else if (buildFilterDto.VerticalName?.Any() == true && buildFilterDto.VerticalName.Count() == 1 && buildFilterDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));
            }
            else if (buildFilterDto.VerticalName?.Any() == true && buildFilterDto.VerticalName.Count() > 1 && buildFilterDto.VerticalName.Contains("yes"))
            {
                var nullVerticals = buildFilterDto.VerticalName.Contains("yes") ?
                    query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0)) : null;
                var verticalFilter = query
                                    .Where(x => x.VerticalFilterDto != null &&
                                    x.VerticalFilterDto.Any(c => buildFilterDto.VerticalName.Where(t => t != "yes").Contains(c.Key.ToString())));
                query = nullVerticals?.Any() == true && verticalFilter?.Any() == true ?
                    nullVerticals.Concat(verticalFilter) : nullVerticals?.Any() == true && verticalFilter
                    ?.Any() == false ? nullVerticals
                    : nullVerticals?.Any() == false && verticalFilter?.Any() == true ? verticalFilter : null;
            }

            var rtn = propertyName switch
            {
                "lcmEngineeringId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.LcmEngineeringId, p.LcmEngineering.LcmengineeringId)).Distinct().ToList()
                    : query.Where(x => x.LcmEngineeringId.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.LcmEngineeringId)).Distinct().ToList(),

                "plannedDesignComponentIndex" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.DesignComponentId, p.DesignComponentId)).Distinct().ToList()
                    : query.Where(x => x.DesignComponentId.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.DesignComponentId)).Distinct().ToList(),

                "originalDesignComponentIndex" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.LcmEngineeringId != null ? p.LcmEngineering.DesignComponentId :
                    p.NetworkElementAsPlannedId != null ? p.NetworkElementAsPlanned.DesignComponentId : (long?)null,
                    p.LcmEngineeringId != null ? p.LcmEngineering.DesignComponentId :
                    p.NetworkElementAsPlannedId != null ? p.NetworkElementAsPlanned.DesignComponentId : (long?)0)).Distinct().ToList()
                    : query.Where(x => x.LcmEngineeringId != null ? x.LcmEngineering.DesignComponentId.ToString().Contains(propertyFilter)
                    : (x.NetworkElementAsPlannedId != null ? x.NetworkElementAsPlanned.DesignComponentId : (long?)null).ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.LcmEngineeringId != null ? p.LcmEngineering.DesignComponentId :
                    p.NetworkElementAsPlannedId != null ? p.NetworkElementAsPlanned.DesignComponentId : (long?)null,
                    p.LcmEngineeringId != null ? p.LcmEngineering.DesignComponentId :
                    p.NetworkElementAsPlannedId != null ? p.NetworkElementAsPlanned.DesignComponentId : (long?)0)).Distinct().ToList(),

                "designComponentFamilyIndex" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.DesignComponentFamilyId, p.DesignComponentFamilyId)).Distinct().ToList()
                    : query.Where(x => x.DesignComponentFamilyId.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.DesignComponentFamilyId)).Distinct().ToList(),
                "verticalName" => query.AsEnumerable().Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0).SelectMany(p => p.VerticalFilterDto.Select(m =>
                new FilterValueDto
                {
                    Value = m.Key.ToString(),
                    Text = m.Value
                }))?.Distinct()?.ToList()
                .Concat(
                    (query.AsEnumerable().Where(x=>x.VerticalFilterDto!=null && x.VerticalFilterDto.Count==0))
                    .Select(x=>
                            _commonManager.AddBlankFilterValue()
                    )
                )
                .Distinct().ToList(),

                "plannedDesignComponent" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Where(x => x.DesignComponent != null)
                    .Select(p => new FilterValueDto(p.DesignComponentId, DesignComponentMapper.SetDesignComponentMapper(p.DesignComponent).toDesignComponentNameLcm(_repositoryWrapper))).Distinct().ToList()
                    : query
                        .ToList()
                        .Where(x => x.DesignComponent != null &&
                           DesignComponentMapper.SetDesignComponentMapper(x.DesignComponent).toDesignComponentNameLcm(_repositoryWrapper).ToUpper().Contains(propertyFilter.ToUpper())
                        ).Select(p => new FilterValueDto(p.DesignComponentId, DesignComponentMapper.SetDesignComponentMapper(p.DesignComponent).toDesignComponentNameLcm(_repositoryWrapper)))
                        .Distinct().ToList(),
                "archived" =>
                   string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(x => x.Archived != null)
                       .Select(p => new FilterValueDto { Text = p.Archived == false ? ConstantValueFilter.NO : ConstantValueFilter.YES, Value = p.Archived.ToString() }).Distinct().ToList()
                   : query
                       .Where(p => p.Archived != null && (p.Archived.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO).Contains(propertyFilter))
                       .Select(p => new FilterValueDto { Text = p.Archived == false ? ConstantValueFilter.NO : ConstantValueFilter.YES, Value = p.Archived.ToString() }).Distinct()
                       .ToList(),

                "designComponentFamilyName" => isServicePlan != true ?
                (string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.DesignComponentFamily != null || x.DesignComponent != null)
                     .Select(p => new FilterValueDto(p.DesignComponentFamily != null ? p.DesignComponentFamilyId :
                     p.DesignComponent.DesignComponentFamilyId, p.DesignComponentFamily != null ?
                     p.DesignComponentFamily.DCFName(_repositoryWrapper) : p.DesignComponent.DesignComponentFamily.DCFName(_repositoryWrapper))).Distinct().ToList()
                    : query
                        .Where(x => (x.DesignComponentFamily != null && x.DesignComponentFamilyId.ToString().Contains(propertyFilter.ToUpper()))
                        || (x.DesignComponent != null && x.DesignComponent.DesignComponentFamilyId.ToString().Contains(propertyFilter.ToUpper())))
                        .Select(p => new FilterValueDto(p.DesignComponentFamily != null ?
                        p.DesignComponentFamilyId : p.DesignComponent.DesignComponentFamilyId, p.DesignComponentFamily != null ?
                        p.DesignComponentFamily.DCFName(_repositoryWrapper) : p.DesignComponent.DesignComponentFamily.DCFName(_repositoryWrapper))).Distinct().ToList())
                        :
                        (string.IsNullOrEmpty(propertyFilter)?query.Where(x => x.Serviceplan != null && x.Serviceplan.Serviceplandcfmappings != null && x.Serviceplan.Serviceplandcfmappings.Count > 0)
                                               .SelectMany(p => p.Serviceplan.Serviceplandcfmappings)
                                               .Select(c =>
                                                    new FilterValueDto
                                                    {
                                                        Value = c.Dcf.DesignComponentFamilyId.ToString(),
                                                        Text =  c.Dcf.DCFName(_repositoryWrapper)
                                                    }).Distinct().ToList()
                                               : query.Where(a => a.Serviceplan != null && a.Serviceplan.Serviceplandcfmappings != null && a.Serviceplan.Serviceplandcfmappings.Count > 0
                                               && a.Serviceplan.Serviceplandcfmappings.Any(x => x.Dcfid.ToString().Contains(propertyFilter.ToUpper())))
                                               .SelectMany(b => b.Serviceplan.Serviceplandcfmappings)
                                               .Select(c => new FilterValueDto
                                               {
                                                   Value = c.Dcf.DesignComponentFamilyId.ToString(),
                                                   Text = c.Dcf.DCFName(_repositoryWrapper)
                                               }).Distinct().ToList()),

                //"designComponentFamilyName" => isServicePlan != true ?
                //                               query.Where(x => x.DesignComponentFamily != null || x.DesignComponent != null)
                //                               .Select(p =>
                //                                    new FilterValueDto(
                //                                        p.DesignComponentFamily != null ? p.DesignComponentFamilyId : p.DesignComponent.DesignComponentFamilyId
                //                                        , p.DesignComponentFamily != null ? p.DesignComponentFamily.DCFName(_repositoryWrapper) : p.DesignComponent.DesignComponentFamily.DCFName(_repositoryWrapper)
                //                                    )
                //                               ).Distinct().ToList()
                //                               : query.Where(a => a.Serviceplan != null)
                //                               .SelectMany(b => b.Serviceplan.Serviceplandcfmappings)
                //                               .Select(c => new FilterValueDto
                //                               {
                //                                   Value = c.Dcf != null ? c.Dcf.DesignComponentFamilyId.ToString() : c.Designcomponentfamilyid.ToString(),
                //                                   Text = c.Designcomponentfamily != null ? c.Designcomponentfamily.DCFName(_repositoryWrapper) : string.Empty
                //                               }).Distinct().ToList(),

                "originalDesignComponent" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Where(p => p.NetworkElementAsPlanned != null || p.LcmEngineering != null)
                    .Select(p => new FilterValueDto(
                        (p.NetworkElementAsPlanned != null) ? p.NetworkElementAsPlanned.DesignComponentId : p.LcmEngineering.DesignComponentId,
                        p.toOriginalDesignComponentDescription(_repositoryWrapper)
                    )).Distinct().ToList()
                    : query
                        .ToList()
                        .Where(x => x.DesignComponent != null &&
                            x.toOriginalDesignComponentDescription(_repositoryWrapper).ToUpper().Contains(propertyFilter.ToUpper()) &&
                            (x.NetworkElementAsPlanned != null || x.LcmEngineering != null)
                        )
                    .Select(p => new FilterValueDto(
                        (p.NetworkElementAsPlanned != null) ? p.NetworkElementAsPlanned.DesignComponentId : p.LcmEngineering.DesignComponentId,
                        p.toOriginalDesignComponentDescription(_repositoryWrapper)
                    )).Distinct().ToList(),
                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(p => p.OpCo != null).Select(p => new FilterValueDto
                    {
                        Text = p.OpCo.OpCoDescription,
                        Value = p.OpCoId.ToString()
                    }).Distinct().ToList()
                    : query.ToList().Where(x => x.OpCo != null &&
                            x.OpCo.OpCoDescription.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto
                            {
                                Text = p.OpCo.OpCoDescription,
                                Value = p.OpCoId.ToString()
                            }).Distinct().ToList(),

                "deliveryProjectName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.DeliveryProjectName)).Distinct().ToList()
                    : query.Where(x => x.DeliveryProjectName.Contains(propertyFilter)).Select(p => new FilterValueDto(p.DeliveryProjectName)).Distinct().ToList(),
                "activityStatusId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ActivityStatusId, p.ActivityStatus.ActivityStatusDescription)).Distinct().ToList()
                    : query.Where(x => x.ActivityStatus.ActivityStatusDescription.Contains(propertyFilter)).Select(p => new FilterValueDto(p.ActivityStatusId, p.ActivityStatus.ActivityStatusDescription)).Distinct().ToList(),
                "plannedActivityDescription" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.PlannedActivityDescription)).Distinct().ToList()
                    : query.Where(x => x.PlannedActivityDescription.Contains(propertyFilter)).Select(p => new FilterValueDto(p.PlannedActivityDescription)).Distinct().ToList(),
                "benefits" => string.IsNullOrEmpty(propertyFilter)
                 ? query.Where(x => x.Benefit != null).Select(p => new FilterValueDto(p.BenefitId, p.Benefit.BenefitDescription)).Distinct().ToList()
                 : query.Where(x => x.Benefit.BenefitDescription.ToUpper().Contains(propertyFilter.ToUpper())).Select(p => new FilterValueDto(p.BenefitId, p.Benefit.BenefitDescription)).Distinct().ToList(),
                "driver" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.Driver != null).Select(p => new FilterValueDto(p.DriverId, p.Driver.DriverDescription)).Distinct().ToList()
                    : query.Where(x => x.Driver.DriverDescription.ToUpper().Contains(propertyFilter.ToUpper())).Select(p => new FilterValueDto(p.DriverId, p.Driver.DriverDescription)).Distinct().ToList(),
                "budgetAvailability" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.BudgetAvailability != null).Select(p => new FilterValueDto(p.BudgetAvailability.BudgetAvailabilityDescription)).Distinct().ToList()
                    : query.Where(x => x.BudgetAvailability.BudgetAvailabilityDescription.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.BudgetAvailability.BudgetAvailabilityDescription)).Distinct().ToList(),
                "riskEngineeringEvaluation" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.EngineeringRisk != null).Select(p => new FilterValueDto(p.EngineeringRisk.RiskDescription)).Distinct().ToList()
                    : query.Where(x => x.EngineeringRisk != null).Where(x => x.EngineeringRisk.RiskDescription.Contains(propertyFilter)).Select(p => new FilterValueDto(p.EngineeringRisk.RiskDescription)).Distinct().ToList(),
                "riskOperationalEvaluation" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.OperationalRisk != null).Select(p => new FilterValueDto(p.OperationalRisk.RiskDescription)).Distinct().ToList()
                    : query.Where(x => x.OperationalRisk != null && x.OperationalRisk.RiskDescription.Contains(propertyFilter)).Select(p => new FilterValueDto(p.OperationalRisk.RiskDescription)).Distinct().ToList(),
                "deliveryStatusId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.DeliveryStatusId, p.DeliveryStatus.DeliveryStatusDescription)).Distinct().ToList()
                    : query.Where(x => x.DeliveryStatus.DeliveryStatusDescription.Contains(propertyFilter)).Select(p => new FilterValueDto(p.DeliveryStatusId.ToString(), p.DeliveryStatus.DeliveryStatusDescription)).Distinct().ToList(),
                "responsibilityPhaseId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ResponsibilityPhaseId, p.ResponsibilityPhase.ResponsibilityPhaseDescription)).Distinct().ToList()
                    : query.Where(x => x.ResponsibilityPhase.ResponsibilityPhaseDescription.Contains(propertyFilter)).Select(p => new FilterValueDto(p.ResponsibilityPhaseId, p.ResponsibilityPhase.ResponsibilityPhaseDescription)).Distinct().ToList(),
                "planningActivityStatusId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.PlanningActivityStatusId, p.PlanningActivityStatus.PlanningActivityStatusDescription)).Distinct().ToList()
                    : query.Where(x => x.PlanningActivityStatus.PlanningActivityStatusDescription.Contains(propertyFilter)).Select(p => new FilterValueDto(p.PlanningActivityStatusId, p.PlanningActivityStatus.PlanningActivityStatusDescription)).Distinct().ToList(),
                "localApproval" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.LocalApproval)).Distinct().ToList()
                    : query.Where(x => x.LocalApproval.Contains(propertyFilter)).Select(p => new FilterValueDto(p.LocalApproval)).Distinct().ToList(),
                "riskEngineeringNotes" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.RiskEngineeringNotes)).Distinct().ToList()
                    : query.Where(x => x.RiskEngineeringNotes.Contains(propertyFilter)).Select(p => new FilterValueDto(p.RiskEngineeringNotes)).Distinct().ToList(),
                "budgetTrackingId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.BudgetTrackingId)).Distinct().ToList()
                    : query.Where(x => x.BudgetTrackingId.Contains(propertyFilter)).Select(p => new FilterValueDto(p.BudgetTrackingId)).Distinct().ToList(),
                "budgetValueGrid" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new { budget = x.BudgetValue, currency = x.Currency }).Select(p => new FilterValueDto(p.currency == null ? p.budget.ToString() : p.budget.ToString() + p.currency)).Distinct().ToList()
                    : query.Where(x =>
                    (x.BudgetValue.ToString() + x.Currency).Contains(propertyFilter)).Select(p => new FilterValueDto(p.BudgetValue + p.Currency)).Distinct().ToList(),
                "notes" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Notes)).Distinct().ToList()
                    : query.Where(x => x.Notes.Contains(propertyFilter)).Select(p => new FilterValueDto(p.Notes)).Distinct().ToList(),
                "deliveryProjectId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto(p.DeliveryProjectId)).Distinct().ToList()
                : query.Where(x => x.DeliveryProjectId.Contains(propertyFilter)).Select(p => new FilterValueDto(p.DeliveryProjectId)).Distinct().ToList(),
                "plannedActivityId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.PlannedActivityId)).Distinct().ToList()
                    : query.Where(x => x.PlannedActivityId.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.PlannedActivityId)).Distinct().ToList(),
                "plannedImplementationYear" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.PlannedImplementationYear)).Distinct().ToList()
                    : query.Where(x => x.PlannedImplementationYear.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.PlannedImplementationYear)).Distinct().ToList(),
                "planningRisk" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.PlanningRisk != null).Select(p => new FilterValueDto(p.PlanningRiskId, p.PlanningRisk.PlanningRiskDescription)).Distinct().ToList()
                    : query.Where(x => x.PlanningRisk.PlanningRiskDescription.ToUpper().Contains(propertyFilter.ToUpper())).Select(p => new FilterValueDto(p.PlanningRiskId, p.PlanningRisk.PlanningRiskDescription)).Distinct().ToList(),
                "riskOperationalNotes" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.RiskOperationalNotes)).Distinct().ToList()
                    : query.Where(x => x.RiskOperationalNotes.Contains(propertyFilter)).Select(p => new FilterValueDto(p.RiskOperationalNotes)).Distinct().ToList(),
                "projectStatus" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ProjectStatus)).Distinct().ToList()
                    : query.Where(x => x.ProjectStatus.Contains(propertyFilter)).Select(p => new FilterValueDto(p.ProjectStatus)).Distinct().ToList(),
                "plannedActivityResourceId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.PlannedActivityResourceId, p.PlannedActivityResource.PlannedActivityResourceDescription)).Distinct().ToList()
                    : query.Where(x => x.PlannedActivityResource.PlannedActivityResourceDescription.Contains(propertyFilter)).Select(p => new FilterValueDto(p.PlannedActivityResourceId, p.PlannedActivityResource.PlannedActivityResourceDescription)).Distinct().ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "program" => query.Where(p => p.ProgramNavigation.ProgramDescription != null).Select(p => new FilterValueDto(p.ProgramNavigation.ProgramDescription)).Distinct().ToList(),
                "projectOwner" => query.Where(p => p.ProjectOwner != null).Select(p => new FilterValueDto(p.ProjectOwner)).Distinct().ToList(),
                "deliveryTrackingId" => query.Select(p => new FilterValueDto(p.DeliveryTrackingId)).Distinct().ToList(),
                "deliveryPlanAvailable" =>
                  query.Select(p => new FilterValueDto { Text = p.DeliveryPlanAvailable ? ConstantValueFilter.YES : ConstantValueFilter.NO, Value = p.DeliveryPlanAvailable.ToString() }).Distinct().ToList(),
                "isPAReleaseDetailUnknown" =>
                  query.Select(p => new FilterValueDto { Text = p.IsPAReleaseDetailUnknown.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO, Value = p.IsPAReleaseDetailUnknown.ToString() }).Distinct().ToList(),
                //Ticket 751 PPM Import

                "deliveryProjectPpmId" => string.IsNullOrEmpty(propertyFilter)
                                   ? query.Select(p => new FilterValueDto(p.DeliveryProjectId)).Distinct().ToList()
                                   : query.Where(x => x.DeliveryProjectId.Contains(propertyFilter)).Select(p =>
                          new FilterValueDto(p.DeliveryProjectId)).Distinct().ToList(),

                #region Ticket 685 Dev - #674 SettingsUpdatePlanedActivity - Display  Planned Activity Associated/Linked Table Details  - Ex : LCM, DA, Assets

                "forAddAsset" =>
                 string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(p => p.ForAddAsset != null)
                       .Select(p => new FilterValueDto
                       {
                           Text = p.ForAddAsset.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                           Value = p.ForAddAsset.ToString()
                       }).Distinct().ToList()
                   : query
                       .Where(p => p.ForEditAsset != null && (p.ForAddAsset.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO).Contains(propertyFilter))
                       .Select(p => new FilterValueDto
                       {
                           Text = p.ForAddAsset.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                           Value = p.ForAddAsset.ToString()
                       }).Distinct().ToList(),

                "forEditAsset" =>
                 string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(p => p.ForEditAsset != null)
                       .Select(p => new FilterValueDto { Text = p.ForEditAsset.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO, Value = p.ForEditAsset.ToString() }).Distinct().ToList()
                   : query
                       .Where(p => p.ForEditAsset != null && (p.ForEditAsset.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO).Contains(propertyFilter))
                       .Select(p => new FilterValueDto { Text = p.ForEditAsset.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO, Value = p.ForEditAsset.ToString() }).Distinct().ToList(),


                "forLcmLink" =>
               string.IsNullOrEmpty(propertyFilter)
                 ? query
                     .Select(p => new FilterValueDto
                     {
                         Text = p.LcmEngineeringId != null ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                         Value = p.LcmEngineeringId != null ? ConstantValueFilter.CamalTrue : ConstantValueFilter.CamalFalse
                     }).Distinct().ToList()
                 : query
                     .Select(p => new FilterValueDto
                     {
                         Text = p.LcmEngineeringId != null ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                         Value = p.LcmEngineeringId != null ? ConstantValueFilter.CamalTrue : ConstantValueFilter.CamalFalse
                     }).Where(p => p.Text.Contains(propertyFilter)).Distinct().ToList(),
                "forDesignAspectLink" =>
                              string.IsNullOrEmpty(propertyFilter)
                                ? query
                                    .Select(p => new FilterValueDto
                                    {
                                        Text = p.DesignAspectId != null ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                        Value = p.DesignAspectId != null ? ConstantValueFilter.CamalTrue : ConstantValueFilter.CamalFalse
                                    }).Distinct().ToList()
                                : query
                                    .Select(p => new FilterValueDto
                                    {
                                        Text = p.DesignAspectId != null ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                        Value = p.DesignAspectId != null ? ConstantValueFilter.CamalTrue : ConstantValueFilter.CamalFalse
                                    }).Where(p => p.Text.Contains(propertyFilter)).Distinct().ToList(),

                "plannedBuildBagDescription" => query
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Buildbag.BagDescription.Contains(propertyFilter))
                 .Select(p => new FilterValueDto { Text = _commonManager.GetBuildBagDescriptionFromEnity(p.Buildbag), Value = p.Buildbagid.ToString() })
                 .Distinct()
                 .ToList(),
                "currentBuildBagDescription" => query
               .Where(x => x.LcmEngineering != null && (string.IsNullOrEmpty(propertyFilter) || x.LcmEngineering.BuildBag.BagDescription.Contains(propertyFilter)))
               .Select(p => new FilterValueDto { Text = _commonManager.GetBuildBagDescriptionFromEnity(p.LcmEngineering.BuildBag), Value = p.LcmEngineering.BuildBagId.ToString() })
               .Distinct()
               .ToList(),
                #endregion

                #region // BGT Newly Added Column
                "plannedActivityTeam" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.PlannedActivityTeam) },



                "priority" => string.IsNullOrEmpty(propertyFilter)
                                    ? query.Select(p => new FilterValueDto { Text = p.Priority, Value = p.Priority }).Distinct().ToList()
                                    : query.Where(x => x.Priority.Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.Priority, Value = p.Priority }).Distinct().ToList(),

                "plannedActivityCategory" => string.IsNullOrEmpty(propertyFilter)
                         ? query.Select(p => new FilterValueDto(p.PlannedactivitycategoryNavigation.Categorydescription)).Distinct().ToList()
                         : query.Where(x => x.PlannedactivitycategoryNavigation.Categorydescription.Contains(propertyFilter)).Select(p =>
                new FilterValueDto(p.PlannedactivitycategoryNavigation.Categorydescription)).Distinct().ToList(),

                "lcmCategories" => string.IsNullOrEmpty(propertyFilter)
                                    ? query.Select(p => new FilterValueDto { Text = _commonManager.LcmCategoryStatust(p.Lcmcategories), Value = p.Lcmcategories.ToString() }).Distinct().ToList()
                                    : query.Where(x => x.Lcmcategories.ToString().Contains(propertyFilter)).Select(p =>
                           new FilterValueDto(p.Lcmcategories)).Distinct().ToList(),
                #endregion
                "projectDescription" => string.IsNullOrEmpty(propertyFilter)
                                    ? query.Select(p => new FilterValueDto { Text = p.ProjectDescription, Value = p.ProjectDescription.ToString() }).Distinct().ToList()
                                    : query.Where(x => x.ProjectDescription.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.ProjectDescription)).Distinct().ToList(),

                "activityDetails" => string.IsNullOrEmpty(propertyFilter)
                                    ? query.Select(p => new FilterValueDto { Text = p.ActivityDetails, Value = p.ActivityDetails }).Distinct().ToList()
                                    : query.Where(x => x.ActivityDetails.Contains(propertyFilter)).Select(p => new FilterValueDto(p.ActivityDetails)).Distinct().ToList(),

                "isServicePlan" =>
                                string.IsNullOrEmpty(propertyFilter)
                                  ? query.Where(p => p.Isserviceplan != null)
                                      .Select(p => new FilterValueDto
                                      {
                                          Text = p.Isserviceplan.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                          Value = p.Isserviceplan.ToString()
                                      }).Distinct().ToList()
                                  : query
                                      .Where(p => p.Isserviceplan.ToString().Contains(propertyFilter))
                                      .Select(p => new FilterValueDto
                                      {
                                          Text = p.Isserviceplan.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                          Value = p.Isserviceplan.ToString()
                                      }).Distinct().ToList(),


                "programName" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto(p.DeliveryProjectName)).Distinct().ToList()
                     : query.Where(x => x.DeliveryProjectName.Contains(propertyFilter)).Select(p => new FilterValueDto(p.DeliveryProjectName)).Distinct().ToList(),

                "serviceMaster" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto { Text = p.Serviceplan.Servicemaster.Description, Value = p.Serviceplan.Servicemasterid.ToString() }).Distinct().ToList()
               : query.Where(x => x.Serviceplan.Servicemaster.Description.Contains(propertyFilter)).Select(p => new FilterValueDto
               { Text = p.Serviceplan.Servicemaster.Description, Value = p.Serviceplan.Servicemasterid.ToString() }).Distinct().ToList(),
                _ => new List<FilterValueDto>()
            };

            if (!isAdmin && (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();
            }
            return rtn;
        }

        private Dictionary<string, Expression<Func<PlannedActivity, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<PlannedActivity, object>>[]>
            {
                ["plannedActivityId"] = new Expression<Func<PlannedActivity, object>>[] { p => p.PlannedActivityId },
                ["originalDesignComponentIndex"] = new Expression<Func<PlannedActivity, object>>[] { p => p.LcmEngineeringId != null ? p.LcmEngineering.DesignComponentId : p.NetworkElementAsPlannedId },
                ["designComponentFamilyIndex"] = new Expression<Func<PlannedActivity, object>>[] { p => p.DesignComponentFamilyId },
                ["plannedDesignComponentIndex"] = new Expression<Func<PlannedActivity, object>>[] { p => p.DesignComponentId },
                ["opCo"] = new Expression<Func<PlannedActivity, object>>[] { p => p.LcmEngineering != null && p.LcmEngineering.OpCo != null ? p.LcmEngineering.OpCo.OpCoDescription : (p.DesignAspect != null && p.DesignAspect.OpCo != null ? p.DesignAspect.OpCo.OpCoDescription : (p.NetworkElementAsPlanned != null && p.NetworkElementAsPlanned.OpCo != null ? p.NetworkElementAsPlanned.OpCo.OpCoDescription : default)) },
                ["plannedCompletionValue"] = new Expression<Func<PlannedActivity, object>>[] { p => p.PlannedCompletion },
                ["originalDesignComponent"] = new Expression<Func<PlannedActivity, object>>[] { p => p.toOriginalDesignComponentDescription(_repositoryWrapper) },
                ["plannedDesignComponent"] = new Expression<Func<PlannedActivity, object>>[] { p => p.DesignComponent.toDesignComponentNameLcm(_repositoryWrapper) },

                ["plannedImplementationYear"] = new Expression<Func<PlannedActivity, object>>[] { p => p.PlannedImplementationYear },
                ["designComponentFamilyName"] = new Expression<Func<PlannedActivity, object>>[] { p => p.toOriginalDesignComponentFamilyDescription(_repositoryWrapper) },
                ["activityStatusId"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ActivityStatus.ActivityStatusDescription },
                ["plannedActivityResourceId"] = new Expression<Func<PlannedActivity, object>>[] { p => p.PlannedActivityResource.PlannedActivityResourceDescription },
                ["planningActivityStatusId"] = new Expression<Func<PlannedActivity, object>>[] { p => p.PlanningActivityStatus.PlanningActivityStatusDescription },
                ["benefits"] = new Expression<Func<PlannedActivity, object>>[] { p => p.Benefit != null ? p.Benefit.BenefitDescription : default },
                ["localApproval"] = new Expression<Func<PlannedActivity, object>>[] { p => p.LocalApproval },
                ["budgetValueGrid"] = new Expression<Func<PlannedActivity, object>>[] { p => p.BudgetValue, p => p.Currency },
                ["budgetTrackingId"] = new Expression<Func<PlannedActivity, object>>[] { p => p.BudgetTrackingId },
                ["deliveryProjectName"] = new Expression<Func<PlannedActivity, object>>[] { p => p.DeliveryProjectName },
                ["responsibilityPhaseId"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ResponsibilityPhase.ResponsibilityPhaseDescription },
                ["deliveryStatusId"] = new Expression<Func<PlannedActivity, object>>[] { p => p.DeliveryStatus.DeliveryStatusDescription },
                ["riskEngineeringEvaluation"] = new Expression<Func<PlannedActivity, object>>[] { p => p.EngineeringRisk != null ? p.EngineeringRisk.RiskDescription : default },
                ["riskEngineeringNotes"] = new Expression<Func<PlannedActivity, object>>[] { p => p.RiskEngineeringNotes },
                ["riskOperationalEvaluation"] = new Expression<Func<PlannedActivity, object>>[] { p => p.OperationalRisk != null ? p.OperationalRisk.RiskDescription : default },
                ["riskOperationalNotes"] = new Expression<Func<PlannedActivity, object>>[] { p => p.RiskOperationalNotes },
                ["projectStatus"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ProjectStatus },
                ["notes"] = new Expression<Func<PlannedActivity, object>>[] { p => p.Notes },
                ["startDateValue"] = new Expression<Func<PlannedActivity, object>>[] { p => p.StartDate },
                ["archived"] = new Expression<Func<PlannedActivity, object>>[] { p => p.Archived },
                ["lastModifiedValue"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ModificationUserEntity.Email },
                ["deliveryPlanAvailable"] = new Expression<Func<PlannedActivity, object>>[] { p => p.DeliveryPlanAvailable },
                ["budgetAvailability"] = new Expression<Func<PlannedActivity, object>>[] { p => p.BudgetAvailability != null ? p.BudgetAvailability.BudgetAvailabilityDescription : default },
                ["plannedDesignComponentName"] = new Expression<Func<PlannedActivity, object>>[] { p => p.toPlannedDesignComponentDescription(_repositoryWrapper) },
                ["forAddAsset"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ForAddAsset },
                ["forEditAsset"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ForEditAsset },
                ["program"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ProgramNavigation.ProgramDescription },
                ["projectOwner"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ProjectOwner },
                ["deliveryTrackingId"] = new Expression<Func<PlannedActivity, object>>[] { p => p.DeliveryTrackingId },

                ["driver"] = new Expression<Func<PlannedActivity, object>>[] { p => p.Driver != null ? p.Driver.DriverDescription : default },
                ["forLcm"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ForAddAsset },
                ["forDesignAspects"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ForEditAsset },
                //Ticket 751 PPM Import
                ["deliveryProjectPpmId"] = new Expression<Func<PlannedActivity, object>>[] { p => p.DeliveryProjectId },
                ["projectDescription"] = new Expression<Func<PlannedActivity, object>>[] { p => p.ProjectDescription },
            };
        }
        private async Task<IQueryable<PlannedActivity>> GetQuery(ExpressionStarter<Plannedactivities> predicateResult, PlannedActivityQueryDto plannedActivityFilterDto, bool isServicePlan = false)
        {
            ///Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
            var result = _repositoryWrapper.PlannedActivity.FindByCondition(predicateResult).AsNoTracking()
                      .Include(x => x.Serviceplan).ThenInclude(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                      .Include(x => x.Serviceplan).ThenInclude(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                      .Include(x => x.Serviceplan).ThenInclude(x => x.Servicemaster)
                      .Include(x => x.Activitystatus)
                      .Include(x => x.Planningactivitystatus)
                      .Include(x => x.Deliverystatus)
                      .Include(x => x.ModificationuserNavigation)
                      .Include(x => x.Responsibilityphase)
                      .Include(x => x.Opco)
                      .Include(x => x.Plannedactivityresource)
                      .Include(x => x.Driver)
                      .Include(x => x.Benefit)
                      .Include(x => x.PlanningriskNavigation)
                      .Include(x => x.Operationalrisk)
                      .Include(x => x.Engineeringrisk)
                      .Include(x => x.Deliverytrackings)
                      .Include(x => x.Budgetavailability)
                      .Include(x => x.Buildbag)
                      .Include(x => x.ProgramNavigation).AsQueryable();

            if (!isServicePlan)
            {
                result = result
                       .Include(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary)
                       .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                       .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                       .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                       .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                       .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Opco)
                       .Include(x => x.Networkelementasplanned).ThenInclude(x=>x.Networkelementasplannedsubdomainspoc)
                       .Include(x => x.Designaspect).ThenInclude(x => x.Opco)
                       .Include(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                             .ThenInclude(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts)
                       .Include(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                             .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware.Majorhwbuildsdesigncontacts)
                       .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                       .Include(x => x.Lcmengineering).ThenInclude(x => x.Buildbag)
                       .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc);


            }

            var asyncdata = await result.ToListAsync();

            var data = asyncdata.AsEnumerable().Select(p => PlannedActivityMapper.Get(p)).ToList();

            foreach (var item in data)
            {
                if (item.NetworkElementAsPlannedId != null)
                {
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.NetworkElementAsPlanned?.NetworkElementAsPlannedSubDomainSpoc?
                       .Select(x => x?.Subdomainspocid).ToList(),
                       item?.NetworkElementAsPlanned?.OpCoId)?.Select(t => new FilterValueDtoKeyValueList
                       {
                           Key = Convert.ToInt16(t.Value),
                           Value = t.Text
                       })?.Distinct()?.ToList();
                }
                else if (item.LcmEngineeringId != null)
                {
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.LcmEngineering?.LcmEngineeringSubDomainSpoc?
                         .Select(x => x?.Subdomainspocid).ToList(),
                         item?.LcmEngineering?.OpCoId)?.Select(t => new FilterValueDtoKeyValueList
                         {
                             Key = Convert.ToInt16(t.Value),
                             Value = t.Text
                         })?.Distinct()?.ToList();
                }

                else if (item.DesignAspectId != null)
                {
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.DesignContactDto,
                       item?.DesignAspect?.OpCoId)?.Select(t => new FilterValueDtoKeyValueList
                       {
                           Key = Convert.ToInt16(t.Value),
                           Value = t.Text
                       })?.Distinct()?.ToList();
                }
                else if (item.Serviceplanid != null)
                {
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.DesignContactDto,
                       item?.DesignAspect?.OpCoId)?.Select(t => new FilterValueDtoKeyValueList
                       {
                           Key = Convert.ToInt16(t.Value),
                           Value = t.Text
                       })?.Distinct()?.ToList();
                }
            }

            return data.AsQueryable();
        }
        private IQueryable<Plannedactivities> GetQueryForArchivedPAs(ExpressionStarter<Plannedactivities> predicateResult, bool includeDeleted, ArchivedPlannedActivityQueryDto plannedActivityFilterDto)
        {
            var result = _repositoryWrapper.PlannedActivity.FindByCondition(predicateResult) //check we need to include service plan entity
                    .Include(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Opco)
                    .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Networkelementasplannedsubdomainspoc)
                    .Include(x => x.Designaspect).ThenInclude(x => x.Opco)
                    .Include(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                             .ThenInclude(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts)
                    .Include(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                                                             .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware.Majorhwbuildsdesigncontacts)
                    .Include(x => x.Activitystatus)
                    .Include(x => x.Planningactivitystatus)
                    .Include(x => x.Deliverystatus)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Responsibilityphase)
                    .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                    .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                    .Include(x => x.Lcmengineering).ThenInclude(x => x.Buildbag)
                    .Include(x => x.Opco)
                    .Include(x => x.Plannedactivityresource)
                    .Include(x => x.Driver)
                    .Include(x => x.Benefit)
                    .Include(x => x.PlanningriskNavigation)
                    .Include(x => x.Operationalrisk)
                    .Include(x => x.Engineeringrisk)
                    .Include(x => x.Budgetavailability)
                    .Include(x => x.Buildbag)
                    .Include(x => x.ProgramNavigation)
                    .Include(x => x.Serviceplan).ThenInclude(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents)
                                        .ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                    .Include(x => x.Serviceplan).ThenInclude(x => x.Serviceplandcfmappings).ThenInclude(x => x.Dcf).ThenInclude(x => x.Designcomponents)
                                        .ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts);
            return result;
        }

        public async Task<string> GetMajorHwBuild(long id)
        {
            var dc = (await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == id)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform).SingleAsync())
                .Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;
            var hardwareSolution = $"{dc?.Platform.Platform} " + $"{dc?.Hardwaretype}";
            return $"{ConstantValueFilter.Solution}: " + hardwareSolution;

        }

        public async Task<string> GetSystemTypeOem(long id)
        {
            var dc = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == id)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Systemtype)
                .ThenInclude(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Orgeqpmanufacturer).SingleOrDefaultAsync();
            var oem = $"{ConstantValueFilter.OEMSW} :{dc?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer}" +
                   $" - {ConstantValueFilter.OEMHW} :{dc?.Systemtype?.Systemtypesmajorhardwarebuilds?.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware?.Orgeqpmanufacturer?.Originalequipmentmanufacturer}";
            return oem;
        }


        public async Task<string> GetMajorSwBuild(long id)
        {
            var dc = await Task.Run(() => _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == id)
                       .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).SingleOrDefault());
            var softwareVersion = dc?.Systemtype?.Majorsoftwarebuilds?.Softwareversion;
            return $"{ConstantValueFilter.MAJORRELEASE}: " + softwareVersion;
        }

        public async Task<string> GetVirtualizeSystem(long id)
        {
            var dc = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == id)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform).SingleAsync();

            var majorHardware = dc.Systemtype.Systemtypesmajorhardwarebuilds
                .FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;

            var virtualizeSystem = $"{majorHardware?.Platform?.Platform} " +
                                   $"{majorHardware?.Hardwaretype} " +
                                   $"{dc?.Systemtype?.Majorsoftwarebuilds?.Softwareversion}";

            return $"{ConstantValueFilter.VIRTUALIZESYSTEM}: " + virtualizeSystem;
        }


        public async Task<ResultDto> GetLinkedDesignComponent(long designComponentId, int rule)
        {
            var entityDC = await _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == designComponentId)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .SingleAsync();

            var _rule = _repositoryWrapper.PlannedActivityTypesRepository.
                     FindByCondition(x => x.Plannedactivitytypesid == rule && x.Linkeddcrule == true && x.Deleted == false).
                     FirstOrDefault();

            var productNameId = entityDC.Systemtype.Majorsoftwarebuilds.Productnameid;
            var softwareVersion = entityDC.Systemtype.Majorsoftwarebuilds.Softwareversion;
            var oemSw = entityDC.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer;
            var majorHwId = entityDC.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware.Majorhardwareid;
            var oemHw = entityDC.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer;
            var platform = entityDC.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware.Platform.Platform;
            var HwSolution = entityDC.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware.Hardwaresolution;

            //Ticket 799 - RuleLinkDC - Color Coding logic on Planned Design Component - revert -
            var _designComponentData = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Visibleflag == true);

            if (_rule != null)
            {
                if ((bool)_rule.Hwoem)
                {
                    _designComponentData = _designComponentData.Where(x => x.Systemtype.Systemtypesmajorhardwarebuilds.
                    FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemHw);
                }
                if ((bool)_rule.Swoem)
                {
                    _designComponentData = _designComponentData.Where(x => x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemSw);
                }
                if ((bool)_rule.Swproductname)
                {
                    _designComponentData = _designComponentData.Where(x => x.Systemtype.Majorsoftwarebuilds.Productnameid == productNameId);
                }
                if ((bool)_rule.Swversion)
                {
                    _designComponentData = _designComponentData.Where(x => x.Systemtype.Majorsoftwarebuilds.Softwareversion == softwareVersion);
                }
                if ((bool)_rule.Subnetworkservice)
                {
                    _designComponentData = _designComponentData.Where(x => x.Designcomponentfamily.Subnetworkboundary.Id == entityDC.Designcomponentfamily.Subnetworkboundary.Id);
                }
                if ((bool)_rule.Hwplatform)
                {
                    _designComponentData = _designComponentData.Where(x => x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Platform.Platform == platform);
                }
                if ((bool)_rule.Hwsolution)
                {
                    _designComponentData = _designComponentData.Where(x => x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Hardwaresolution == HwSolution);
                }
                /*if ((bool)Patr.Hwsolution)
                {
                    temp = temp.Where(x => x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Majorhardwareid == majorHwId);
                }*/

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = (_designComponentData != null && _designComponentData.Count() > 0) ?
                   _designComponentData.Select(x => x.Designcomponentid).ToList() : null
                };

            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = null
                };
            }
            #region //commonds
            //var productNameId = entityDC.Systemtype.Majorsoftwarebuilds.Productnameid;
            //var softwareVersion = entityDC.Systemtype.Majorsoftwarebuilds.Softwareversion;
            //var oemSw = entityDC.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer;
            //var majorHwId = entityDC.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware.Majorhardwareid;
            //var oemHw = entityDC.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer;
            //var platform = entityDC.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value)?.Majorhardware.Platform.Platform;

            ////note:start from here
            //var listDc = rule switch
            //{
            //    (int)RuleEnum.SwArchitectureUpgradeSwMajorRelease =>
            //        _repositoryWrapper.DesignComponent
            //            .FindByCondition(x =>
            //                   x.Designcomponentfamily.Subnetworkboundary.Id == entityDC.Designcomponentfamily.Subnetworkboundary.Id
            //                && x.Systemtype.Majorsoftwarebuilds.Productnameid == productNameId
            //                && x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemSw
            //                && x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Majorhardwareid == majorHwId
            //            ).Select(x => x.Designcomponentid).ToListAsync(),
            //    (int)RuleEnum.HwRefresh =>
            //        _repositoryWrapper.DesignComponent
            //            .FindByCondition(x =>
            //                   x.Designcomponentfamily.Subnetworkboundary.Id == entityDC.Designcomponentfamily.Subnetworkboundary.Id
            //                && x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemSw
            //                && x.Systemtype.Majorsoftwarebuilds.Productnameid == productNameId
            //                && x.Systemtype.Majorsoftwarebuilds.Softwareversion == softwareVersion
            //                && x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemHw
            //                && x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Platform.Platform != platform
            //            ).Select(x => x.Designcomponentid).ToListAsync(),
            //    (int)RuleEnum.HwReplace =>
            //        _repositoryWrapper.DesignComponent
            //            .FindByCondition(x =>
            //                   x.Designcomponentfamily.Subnetworkboundary.Id == entityDC.Designcomponentfamily.Subnetworkboundary.Id
            //                && x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemSw
            //                && x.Systemtype.Majorsoftwarebuilds.Productnameid == productNameId
            //                && x.Systemtype.Majorsoftwarebuilds.Softwareversion == softwareVersion
            //                && x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer != oemHw
            //            ).Select(x => x.Designcomponentid).ToListAsync(),
            //    (int)RuleEnum.SystemReplace =>
            //        null,
            //    (int)RuleEnum.SystemRefresh =>
            //        _repositoryWrapper.DesignComponent
            //            .FindByCondition(x =>
            //                x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemSw
            //                && x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemHw
            //            ).Select(x => x.Designcomponentid).ToListAsync(),
            //    (int)RuleEnum.HardwareUpgrade =>
            //        _repositoryWrapper.DesignComponent
            //            .FindByCondition(x =>
            //                 x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Platform.Platform == platform
            //                && x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && !x.Deleted.Value).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemHw
            //                && x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer == oemSw
            //                && x.Systemtype.Majorsoftwarebuilds.Productnameid == productNameId
            //                && x.Systemtype.Majorsoftwarebuilds.Softwareversion == softwareVersion
            //            ).Select(x => x.Designcomponentid).ToListAsync(),
            //    (int)RuleEnum.NoRule =>
            //        null,
            //    _ =>
            //        null
            //};

            //return new ResultDto
            //{
            //    Info = ResultMessages.GetInfoSuccess,
            //    Data = listDc?.Result
            //};
            #endregion
        }


        public async Task<List<short>> ActivityStatusLogics(int? responsibilityPhaseId, int? deliveryId, int? budgetAvId, bool? localApproval)
        {
            localApproval = localApproval ?? false;
            var del = await _repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatusid == deliveryId).SingleOrDefaultAsync();
            var bud = await _repositoryWrapper.BudgetAvailability.FindByCondition(x => x.Budgetavailabilityid == budgetAvId).SingleOrDefaultAsync();
            var res = await _repositoryWrapper.ResponsibilityPhase.FindByCondition(x => x.Responsibilityphaseid == responsibilityPhaseId).SingleOrDefaultAsync();

            // deliveryid = 2
            // budgetavid = 1
            // reponsibiltyphase = 1
            // local approval = true
            if (bud != null && bud.Rule == 1)
            {
                //In planning
                return await _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Rule == 1)
                    .Select(x => x.Activitystatusid).ToListAsync();
            }

            if (bud != null && localApproval != null)
            {
                //In Mobilisation
                if (bud.Rule == 2 && !localApproval.Value)
                {
                    return await _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Rule == 2)
                        .Select(x => x.Activitystatusid).ToListAsync();
                }
            }

            if (bud != null && del != null && res != null && localApproval != null)
            {
                //In Delivery Engineering
                if (bud.Rule == 2 && localApproval.Value && res.Rule == 1 && del.Rule != 1)
                {
                    return await _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Rule == 3)
                        .Select(x => x.Activitystatusid).ToListAsync();
                }
            }
            if (bud != null && del != null && res != null && localApproval != null)
            {
                //In Delivery Operations
                if (bud.Rule == 2 && localApproval.Value && res.Rule == 2 && del.Rule != 1)
                {
                    return await _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Rule == 4)
                        .Select(x => x.Activitystatusid).ToListAsync();
                }
            }
            if (del != null && bud != null && localApproval != null)
            {
                // Complete 
                if (del.Rule == 1 && localApproval.Value && bud.Rule == 2)
                {
                    return await _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Rule == 5).Select(x => x.Activitystatusid).ToListAsync();
                }
            }
            return new List<short>();
        }


        public async Task<string> GetActivityDetailsFromLcmRule(long id, int rule)
        {
            return rule switch
            {
                1 => await GetMajorHwBuild(id),
                2 => await GetSystemTypeOem(id),
                3 => await GetMajorSwBuild(id),
                4 => await GetVirtualizeSystem(id),
                _ => ""
            };
        }
        public Plannedactivities SetPlannedActivityValue(Plannedactivities entity)
        {

            if (entity != null)
            {
                var activityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Activitystatusid == entity.Activitystatusid).SingleOrDefault();//_repositoryWrapper.ActivityStatus.FindByCondition(x => x.Activitystatusid == entity.Activitystatusid).FirstOrDefault();
                var projectstatusActivityStatus = (activityStatus)?.Projectstatuscombinationrule;

                var budgetAvailability = _repositoryWrapper.BudgetAvailability.FindByCondition(x => x.Budgetavailabilityid == entity.Budgetavailabilityid).SingleOrDefault();//_repositoryWrapper.BudgetAvailability.FindByCondition(x => x.Budgetavailabilityid == entity.Budgetavailabilityid).FirstOrDefault();
                var projectstatusBudgetAvailability = (budgetAvailability)?.Projectstatuscombinationrule;

                var deliveryStatus = _repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatusid == entity.Deliverystatusid).SingleOrDefault();//_repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatusid == entity.Deliverystatusid).FirstOrDefault();

                var planningStatus = _repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Planningactivitystatusid == entity.Planningactivitystatusid).SingleOrDefault();//_repositoryWrapper.PlanningActivityStatus.FindByCondition(x => x.Planningactivitystatusid == entity.Planningactivitystatusid).FirstOrDefault();


                if (entity.Plannedimplementationyear > DateTime.Now.Year &&
                    activityStatus?.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.InPlanning &&
                    deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != ConstantValueFilter.RolloutComplete &&
                    planningStatus?.Planningactivitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.Planned &&
                    budgetAvailability?.Description.ToLower().Replace(" ", "") == ConstantValueFilter.No.ToLower())// old code
                {
                    entity.Projectstatus = Outputs.ProjectStartedBudgetNotNecessary;
                }
                else if (entity.Plannedimplementationyear >= DateTime.Now.Year &&
                  (activityStatus?.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.InMobilisation) &&
                   deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != ConstantValueFilter.RolloutComplete &&
                    budgetAvailability?.Description.ToLower().Replace(" ", "") == ConstantValueFilter.Yes.ToLower())// old codes
                {
                    entity.Projectstatus = Outputs.ProjectStartedBudgetInLRP;
                }
                else if (entity.Plannedimplementationyear >= DateTime.Now.Year &&
                  (activityStatus?.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.InMobilisation) &&
                   deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != ConstantValueFilter.RolloutComplete &&
                   entity.Localapproval.ToLower().Replace(" ", "") == ConstantValueFilter.Yes.ToLower())
                {

                    entity.Projectstatus = Outputs.ProjectStartedBudgetInDB;
                }
                else if (
                    (activityStatus?.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.InDeliveryEngineering
                    || activityStatus?.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.InDeliveryOperations) &&
                  deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != ConstantValueFilter.RolloutComplete &&
                    planningStatus?.Planningactivitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.Confirmed &&
                  budgetAvailability?.Description.ToLower().Replace(" ", "") == ConstantValueFilter.Yes.ToLower())// old code
                {
                    entity.Projectstatus = Outputs.ProjectOngoingBudgetInDB;
                }
                else if (
                   (activityStatus?.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.InDeliveryEngineering
                   || activityStatus?.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.InDeliveryOperations) &&
                 deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != ConstantValueFilter.RolloutComplete &&
                   planningStatus?.Planningactivitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.Confirmed &&
                 budgetAvailability?.Description.ToLower().Replace(" ", "") == ConstantValueFilter.No.ToLower())// old code
                {
                    entity.Projectstatus = Outputs.ProjectOngoingBudgetNotNecessary;
                }
                else if (
                   activityStatus?.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.InPlanning &&
                 deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") != ConstantValueFilter.RolloutComplete &&
                   planningStatus?.Planningactivitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.Rejected &&
                 budgetAvailability?.Description.ToLower().Replace(" ", "") == ConstantValueFilter.No.ToLower())// old code
                {
                    entity.Projectstatus = Outputs.RequestedButRefused;
                }
                else if (
                   activityStatus?.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.Completed &&
                 deliveryStatus?.Deliverystatus.ToLower().Replace(" ", "") == ConstantValueFilter.RolloutComplete)
                {
                    entity.Projectstatus = Outputs.ProjectCompleted;
                }
                else
                {
                    // Set Default Value April 16 2024
                    entity.Projectstatus = Outputs.ProjectStartedBudgetInDB;
                }

                var engineeringEvaluation = (_repositoryWrapper.Risk.FindByCondition(x => x.Riskid == entity.Engineeringriskid).SingleOrDefault());
                var operationalRisk = (_repositoryWrapper.Risk.FindByCondition(x => x.Riskid == entity.Operationalriskid).SingleOrDefault());
                //EngineeringEvaluation e Risk - Operational Evaluation
                if (engineeringEvaluation != null || operationalRisk != null)
                {
                    if (engineeringEvaluation == null)
                    {
                        entity.Overallriskevaluation = operationalRisk.Description;
                    }

                    if (operationalRisk == null)
                    {
                        entity.Overallriskevaluation = engineeringEvaluation.Description;
                    }

                    if (engineeringEvaluation?.Severity >= operationalRisk?.Severity)
                    {
                        entity.Overallriskevaluation = engineeringEvaluation.Description;
                    }
                    else
                    {
                        entity.Overallriskevaluation = operationalRisk?.Description;
                    }

                }
            }
            return entity;

        }


        public async Task<ResultDto> GetOpCoList(List<short> _opcoList)
        {
            var opco = await Task.Run(() => _repositoryWrapper.OpCo.FindByCondition(x => x.Lcmengineering.Count > 0 
                                            && x.Lcmengineering.Any(s => s.PlannedactivitiesLcmengineering.Any(x => x.Designcomponentid != null)),true).ToList());

            var res = ( (_opcoList != null && _opcoList.Count>0) ? 
                       opco.Where(x => _opcoList.Contains(x.Opcoid)) : 
                       opco).ToDictionary(x => x.Opcoid, x => x.Opco);

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = res
            };

        }

        public async Task<ResultDto> GetDesignComponentList(short opCoId, List<int> _verticalList)
        {
            #region    //Ticket 595 -#590 - Vertical Filter to be applied on design component dropdown's in LC, PA, DA etc.,
            var fetchOpcoBasedLCM = _repositoryWrapper.Lcmengineering
                .FindByCondition(x => x.Archived != true && x.Opcoid == opCoId && x.Designcomponent != null
                && x.Designcomponent.Deleted == false //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
                && x.PlannedactivitiesLcmengineering.Any(x => x.Lcmengineeringid != null))
                 .Include(x => x.PlannedactivitiesLcmengineering)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                 ;

            var lcm = fetchOpcoBasedLCM.ToList();

            #endregion
            var lista = new Dictionary<long, string>();
            foreach (var dc in lcm)
            {
                if (!lista.ContainsKey(dc.Designcomponentid))
                    lista.Add(dc.Designcomponentid, dc.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper));
            }

            var asset = await Task.Run(() => _repositoryWrapper.NetworkElementAsPlanned
               .FindByCondition(x => x.Opcoid == opCoId && x.Designcomponent != null
               && x.Designcomponent.Deleted == false //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
               && x.Plannedactivities.Any(x => x.Networkelementasplannedid != null))
                 .Include(x => x.Plannedactivities)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
               .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
               .ToList());

            foreach (var node in asset)
            {
                if (!lista.ContainsKey(node.Designcomponentid))
                    lista.Add(node.Designcomponentid, node.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper));
            }
            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = lista

            };

        }

        public async Task<ResultDto<PlannedActivityForLinkDto>> GetPlannedActivityForLink(long id)
        {
            var deliveryStatusIds = _repositoryWrapper.DeliveryStatus.FindByCondition(p => p.Deliverystatus.ToLower().Replace(" ", "") == ConstantValueFilter.RFSAchieved
            || p.Deliverystatus.ToLower().Replace(" ", "") == ConstantValueFilter.FSIAchieved || p.Deliverystatus.ToLower().Replace(" ", "") == ConstantValueFilter.RolloutComplete).Select(p => p.Deliverystatusid).ToList();

            var dbData = _repositoryWrapper.PlannedActivity
                 .FindByCondition(x => x.Plannedactivityid == id && deliveryStatusIds.Contains(x.Deliverystatusid.Value))
                 .Include(x => x.Lcmengineering)
                 .Include(x => x.Activitystatus)
                 .Include(x => x.Planningactivitystatus)
                 .Include(x => x.Deliverystatus)
                 .Include(x => x.Plannedactivityresource)
                 .FirstOrDefault();
            if (dbData == null)
                return new ResultDto<PlannedActivityForLinkDto>()
                {
                    Info = "The status of selected planned activity not reached to RFS Achieved",
                    Warning = true
                };
            var entity = PlannedActivityMapper.Get(dbData);

            PlannedActivityForLinkDto dto = new PlannedActivityForLinkDto();
            if (entity.LcmEngineering.ElementCount)
            {
                dto.ElementCount = entity.LcmEngineering.ElementCount;
                dto.StartNetworkElementAssociateds =
                  await _networkElementsAsPlannedManager.GetNetworkElementassociated(
                        entity.LcmEngineering.DesignComponentId, entity.OpCoId.Value);
                dto.EndNetworkElementAssociateds = await
                    _networkElementsAsPlannedManager.GetNetworkElementassociated(
                        entity.DesignComponentId.Value, entity.LcmEngineering.OpCoId.Value);
            }

            //Selected DesignComponent 
            dto.DesignComponentId = entity.LcmEngineering.DesignComponentId;
            IDictionary<long, string> dcResource = _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == dto.DesignComponentId)
                .Include(p => p.Systemtype)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)

                .toDesignComponentResource(_repositoryWrapper);

            var name = dcResource[dto.DesignComponentId];
            dto.DesignComponentResource = new Dictionary<long, string>();
            dto.DesignComponentResource.Add(entity.LcmEngineering.DesignComponentId, name);

            //Selected OpCo
            dto.OpCoId = entity.LcmEngineering.OpCoId ?? 0;
            var opocResource = _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            dto.OpCoResource = new Dictionary<short, string>();
            dto.OpCoResource.Add(dto.OpCoId, opocResource[dto.OpCoId]);

            //Numero Nodi LcmEngineering Vecchia
            dto.NumberOfNodes = entity.LcmEngineering.NumberOfNodes;
            dto.NumberOfLabNodes = entity.LcmEngineering.NumberOfNodesInLab;
            //Planned activity Selected
            dto.PlannedActivityId = entity.PlannedActivityId;
            dto.PlannedActivityResource = new Dictionary<long, PlannedActivityToConnect>();
            var plannedToConnect = new PlannedActivityToConnect();

            //Aggiunge i linked DesignComponent associati alla PlannedActivity
            if (entity.DesignComponentId != null)
            {
                if (entity.DesignComponentId != entity.LcmEngineering.DesignComponentId)
                {
                    var allDesignCompononets = _repositoryWrapper.DesignComponent.FindAll();
                    var dc = allDesignCompononets.Where(x => x.Designcomponentid == entity.DesignComponentId);
                    plannedToConnect.LinkedDesignComponent = dc
                        .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                        .ToDictionary(x => x.Designcomponentid, x => x.toDesignComponentNameLcm(_repositoryWrapper));
                }
                else
                {
                    return new ResultDto<PlannedActivityForLinkDto>()
                    {
                        Info = "The selected record has linked design components equal to lcmenginnering designcomponent",
                        Warning = true,
                        Data = dto
                    };
                }
            }
            else
            {
                return new ResultDto<PlannedActivityForLinkDto>()
                {
                    Info = "The selected record has no linked design components",
                    Warning = true,
                    Data = dto
                };
            }

            var linkedLcmEngineering = _repositoryWrapper.Lcmengineering
                .FindByCondition(x =>
                    x.Designcomponentid == entity.DesignComponentId &&
                    x.Opcoid == entity.LcmEngineering.OpCoId &&
                    x.Lcmengineeringid != entity.LcmEngineeringId
                ).FirstOrDefault();

            if (linkedLcmEngineering == null)
            {
                return new ResultDto<PlannedActivityForLinkDto>()
                {
                    Info = "The linked design components has no LcmEngineering for selected OpCo",
                    Warning = true,
                    Data = dto
                };
            }

            var plannedActivities = _repositoryWrapper.Lcmengineering
                .FindByCondition(x =>
                    x.Designcomponentid == entity.DesignComponentId &&
                    x.Opcoid == entity.LcmEngineering.OpCoId &&
                    x.Lcmengineeringid != entity.LcmEngineeringId
                )
                .SelectMany(x => x.PlannedactivitiesLcmengineering)
                .Include(x => x.Lcmengineering)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Deliverystatus)
                .Include(x => x.Activitystatus)
                .Include(x => x.Planningactivitystatus);

            plannedToConnect.LinkedToPlannedActivity = plannedActivities
                .Where(x => x.Plannedactivityid != entity.PlannedActivityId)
                .ToDictionary(
                    x => x.Plannedactivityid,
                    x =>
                         new PlannedActivityToConnectData()
                         {
                             Name = PlannedActivityMapper.Get(x).toLinkedPlannedActivityName(),
                             NumberOfNode = x.Lcmengineering.Numberofnodes
                         }
            );

            if (entity.LinkedToPlannedActivityId.HasValue && !plannedToConnect.LinkedToPlannedActivity.ContainsKey(entity.LinkedToPlannedActivityId.Value))
            {
                var linkedPlanned = PlannedActivityMapper.Get(_repositoryWrapper.PlannedActivity
                    .FindByCondition(x => x.Plannedactivityid == entity.LinkedToPlannedActivityId)
                    .Include(x => x.Lcmengineering)
                    .Include(x => x.Plannedactivityresource)
                    .Include(x => x.Deliverystatus)
                    .Include(x => x.Activitystatus)
                    .Include(x => x.Planningactivitystatus).Single());

                plannedToConnect.LinkedToPlannedActivity.Add(entity.LinkedToPlannedActivityId ?? 0, new PlannedActivityToConnectData()
                {
                    Name = linkedPlanned.toLinkedPlannedActivityName(),
                    NumberOfNode = linkedPlanned.LcmEngineering.NumberOfNodes,
                });
            }

            plannedToConnect.PlannedActivityName = entity.toLinkedPlannedActivityName();
            plannedToConnect.NumberOfNodes = linkedLcmEngineering.Numberofnodes;
            plannedToConnect.NumberOfLabNodes = linkedLcmEngineering.Numberofnodesinlab;


            dto.PlannedActivityResource.Add(entity.PlannedActivityId, plannedToConnect);
            return new ResultDto<PlannedActivityForLinkDto>()
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = dto
            };
        }
        public async Task<ResultDto<PlannedActivityForLinkDto>> GetPlannedActivityForLinkOld(long id)
        {
            var entity = PlannedActivityMapper.Get(_repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == id)
                .Include(x => x.Lcmengineering)
                .Include(x => x.Deliverystatus)
                .Include(x => x.Activitystatus)
                .Include(x => x.Planningactivitystatus)
                .Include(x => x.Plannedactivityresource)
               .Single());

            var dto = new PlannedActivityForLinkDto();

            dto.DesignComponentId = entity.LcmEngineering.DesignComponentId;
            var dcResource = _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == dto.DesignComponentId)
                //.Include(x => x.Serviceboundary)
                .Include(p => p.Systemtype)
                .toDesignComponentResource(_repositoryWrapper);

            var name = dcResource[dto.DesignComponentId];
            dto.DesignComponentResource = new Dictionary<long, string>();
            dto.DesignComponentResource.Add(entity.LcmEngineering.DesignComponentId, name);

            if (dto?.DesignComponentId != null && !dto.DesignComponentResource.ContainsKey(dto.DesignComponentId))
            {
                var data = await Task.Run(() => _repositoryWrapper.DesignComponent.FindByCondition(
                        x => x.Designcomponentid == dto.DesignComponentId,
                        includeDeleted: true)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .SingleOrDefault());
                if (data != null)
                {
                    dto.DesignComponentResource.Add(data.Designcomponentid, data.toDesignComponentNameLcm(_repositoryWrapper
                    ));
                }
            }

            dto.OpCoId = entity.LcmEngineering.OpCoId ?? 0;
            var opocResource = _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            dto.OpCoResource = new Dictionary<short, string>();
            dto.OpCoResource.Add(dto.OpCoId, opocResource[dto.OpCoId]);
            if (!dto.OpCoResource.ContainsKey(dto.OpCoId))
            {
                var data = _repositoryWrapper.OpCo.FindByCondition(
                    x => x.Opcoid == dto.OpCoId, true).SingleOrDefault();
                if (data != null)
                {
                    dto.OpCoResource.Add(data.Opcoid, data.Opco);
                }
            }

            dto.NumberOfNodes = entity.LcmEngineering.NumberOfNodes;
            dto.NumberOfLabNodes = entity.LcmEngineering.NumberOfNodesInLab;

            dto.PlannedActivityId = entity.PlannedActivityId;
            dto.PlannedActivityResource = new Dictionary<long, PlannedActivityToConnect>();
            var planned = new PlannedActivityToConnect();
            if (entity.DesignComponentId != null)
            {
                var resource = _repositoryWrapper.DesignComponent.FindAll();
                var dc = resource.Where(x => x.Designcomponentid == entity.DesignComponentId);
                planned.LinkedDesignComponent = dc
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .ToDictionary(x => x.Designcomponentid, x => x.toDesignComponentNameLcm(_repositoryWrapper));
            }
            else
            {
                return new ResultDto<PlannedActivityForLinkDto>()
                {
                    Info = "The selected record has no linked design components",
                    Warning = true,
                    Data = dto
                };
            }

            if (entity.LinkedToPlannedActivityId != null)
            {
                var linkedPlanned = PlannedActivityMapper.Get(_repositoryWrapper.PlannedActivity
                    .FindByCondition(x => x.Plannedactivityid == entity.LinkedToPlannedActivityId, true)
                    .Include(x => x.Lcmengineering)
                    .Include(x => x.Plannedactivityresource)
                    .Include(x => x.Deliverystatus)
                    .Include(x => x.Activitystatus)
                    .Include(x => x.Planningactivitystatus).Single());


                planned.LinkedToPlannedActivity = new Dictionary<long, PlannedActivityToConnectData>();
                planned.LinkedToPlannedActivity.Add(entity.LinkedToPlannedActivityId ?? 0, new PlannedActivityToConnectData()
                {
                    Name = linkedPlanned.toLinkedPlannedActivityName(),
                    NumberOfNode = linkedPlanned.LcmEngineering.NumberOfNodes,
                });

            }
            else
            {
                var plannedActivities = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x =>
                        x.Designcomponentid == entity.DesignComponentId &&
                        x.Opcoid == entity.LcmEngineering.OpCoId
                    ).SelectMany(x => x.PlannedactivitiesLcmengineering)
                    .Include(x => x.Lcmengineering)
                    .Include(x => x.Plannedactivityresource)
                    .Include(x => x.Deliverystatus)
                    .Include(x => x.Activitystatus)
                    .Include(x => x.Planningactivitystatus);

                planned.LinkedToPlannedActivity = plannedActivities
                    .Where(x => x.Plannedactivityid != entity.PlannedActivityId)
                    .ToDictionary(
                        x => x.Plannedactivityid,
                        x =>
                             new PlannedActivityToConnectData()
                             {
                                 Name = PlannedActivityMapper.Get(x).toLinkedPlannedActivityName(),
                                 NumberOfNode = x.Lcmengineering.Numberofnodes
                             }
                    );
                if (!planned.LinkedToPlannedActivity.Any())
                {
                    return new ResultDto<PlannedActivityForLinkDto>()
                    {
                        Info = "The selected record has no linked plannedActivity",
                        Warning = true,
                        Data = dto
                    };
                }
            }

            planned.PlannedActivityName = entity.toLinkedPlannedActivityName();

            dto.PlannedActivityResource.Add(entity.PlannedActivityId, planned);
            return new ResultDto<PlannedActivityForLinkDto>()
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = dto
            };
        }

        public async Task<ResultDto<Dictionary<long, PlannedActivityToConnectData>>> GetLcmEngineeringPlannedActivity(long designComponentId, long opCoId)
        {
            var plannedActivities = _repositoryWrapper.Lcmengineering
                .FindByCondition(x => x.Designcomponentid == designComponentId && x.Opcoid == opCoId)
                .SelectMany(x => x.PlannedactivitiesLcmengineering)
                     .Include(x => x.Lcmengineering)
                    .Include(x => x.Plannedactivityresource)
                    .Include(x => x.Deliverystatus)
                    .Include(x => x.Activitystatus)
                    .Include(x => x.Planningactivitystatus);


            var linkedToPlannedActivity = await plannedActivities.ToDictionaryAsync(x => x.Plannedactivityid, x =>
                 new PlannedActivityToConnectData()
                 {
                     Name = PlannedActivityMapper.Get(x).toLinkedPlannedActivityName(),
                     NumberOfNode = x.Lcmengineering.Numberofnodes
                 });

            if (!linkedToPlannedActivity.Any())
            {
                return new ResultDto<Dictionary<long, PlannedActivityToConnectData>>()
                {
                    Info = "The selected record has no linked plannedActivity",
                    Warning = true,
                    Data = linkedToPlannedActivity
                };
            }

            return new ResultDto<Dictionary<long, PlannedActivityToConnectData>>()
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = linkedToPlannedActivity
            };
        }

        public async Task<ResultDto> PlannedActivityMigrations(PlannedActivityMigrationsDto data)
        {
            var plannedActivitiEntity = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Plannedactivityid == data.PlannedActivityId, false, false)
                .Include(x => x.Lcmengineering)
                .Single();
            var lcmEnd = await _repositoryWrapper.Lcmengineering.FindByCondition(x =>
                x.Opcoid == plannedActivitiEntity.Opcoid && x.Designcomponentid == plannedActivitiEntity.Designcomponentid, false, false).SingleOrDefaultAsync();
            var lcmStart = await _repositoryWrapper.Lcmengineering.FindByCondition(x =>
                 x.Opcoid == lcmEnd.Opcoid && x.Designcomponentid == data.DesignComponentIdStart, false, false).SingleOrDefaultAsync();

            lcmEnd.Elementcount = lcmStart.Elementcount;

            if (lcmStart.Elementcount == false)
            {
                lcmEnd.Numberofnodes += data.NumberOfNodes;
                lcmStart.Numberofnodes -= data.NumberOfNodes;

                if (lcmStart.Numberofnodes < 0) lcmStart.Numberofnodes = 0;

                lcmEnd.Numberofnodesinlab += data.NumberOfLabNodes;
                lcmStart.Numberofnodesinlab -= data.NumberOfLabNodes;

                if (lcmStart.Numberofnodesinlab < 0) lcmStart.Numberofnodesinlab = 0;

            }
            _repositoryWrapper.Lcmengineering.Update(lcmStart);
            _repositoryWrapper.Lcmengineering.Update(lcmEnd);
            await _repositoryWrapper.SaveAsync();

            if (lcmStart.Elementcount)
                await _networkElementsAsPlannedManager.MigrateNetworkElement(data, PlannedActivityMapper.Get(plannedActivitiEntity));


            await _repositoryWrapper.ClearTracker();

            if ((lcmStart.Numberofnodes == 0 && lcmStart.Numberofnodesinlab == 0) || (data.NetworkElementStart?.Count() == 0 && data.NetworkElementEnd?.Count() > 0))
            {
                var deliveryStatus = _repositoryWrapper.SettingsUpdatePlannedActivity
                    .FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering && x.Plannedactivityresourceid == plannedActivitiEntity.Plannedactivityresourceid && x.Deliverystatusid == plannedActivitiEntity.Deliverystatusid
                    && x.Ruleelementcount == (int)RuleElementCountEnum.RolloutComplete)
                       .Include(x => x.Deliverystatus)?.FirstOrDefault()?.Deliverystatusid;

                plannedActivitiEntity.Deliverystatusid = deliveryStatus;
                var completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == "COMPLETE").FirstOrDefault();
                plannedActivitiEntity.Activitystatusid = completedActivityStatus.Activitystatusid;
                plannedActivitiEntity.Archived = plannedActivitiEntity.Archived == null ? false : plannedActivitiEntity.Archived;
                plannedActivitiEntity = SetPlannedActivityValue(plannedActivitiEntity);
                _repositoryWrapper.PlannedActivity.Update(plannedActivitiEntity);
                await _repositoryWrapper.SaveAsync();
            }


            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
            };
        }


        public async Task<ResultDto> GetPlannedActivityListForMigration(long dcId, short opcoId)
        {
            var deliveryStatusIds = _repositoryWrapper.DeliveryStatus.FindByCondition(p => p.Deliverystatus.ToLower().Replace(" ", "") == ConstantValueFilter.RFSAchieved
          || p.Deliverystatus.ToLower().Replace(" ", "") == ConstantValueFilter.FSIAchieved || p.Deliverystatus.ToLower().Replace(" ", "") == ConstantValueFilter.RolloutComplete)
                .Select(p => p.Deliverystatusid).ToList();

            var lcm = await Task.Run(() => _repositoryWrapper.Lcmengineering
                .FindByCondition(x => x.Designcomponentid == dcId && x.Opcoid == opcoId)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Activitystatus)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource)
                .ToList());
            var lista = new Dictionary<long, PlannedActivityToConnect>();
            foreach (var item in lcm)
            {
                if (item.PlannedactivitiesLcmengineering.Count() > 0)
                {
                    lista = item.PlannedactivitiesLcmengineering.Where(x => x.Deleted == false && deliveryStatusIds.Contains(x.Deliverystatusid.Value))
                        .ToDictionary(x => x.Plannedactivityid, x =>
                       new PlannedActivityToConnect()
                       {
                           PlannedActivityName = PlannedActivityMapper.Get(x).toLinkedPlannedActivityName(),

                       });
                }
            }
            if (!lista.Any())
            {
                return new ResultDto
                {
                    Info = ResultMessages.NoPlannedActivityManageMig,
                    Warning = true,
                };
            }

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = lista
            };

        }

        #region  Ticket 837 - UpdatePlannedActivityManager - code improvements
        public async Task<Plannedactivities> GetPlannedActivityForUpdatePA(long planningActivityDetailsResourceId)
        {
            var paEntity = await Task.Run(() => _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == planningActivityDetailsResourceId, true, false)
                 .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringeduspoc)
                 .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                 .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmoperationalcontracts)
                 .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmancillarydata)
                 .Include(x => x.Lcmengineering).ThenInclude(x => x.Reasoncheckboxresourcelcmengineeringhardware)
                 .Include(x => x.Lcmengineering).ThenInclude(x => x.Reasoncheckboxresourcelcmengineeringsoftware)
                .Include(x => x.Plannedactivityresource).ThenInclude(x => x.SettingsupdateplannedactivityPlannedactivityresource)
                .FirstOrDefault());
            return paEntity;

        }

        public async Task<Plannedactivities> UpdateSpecifyDcForIsPAUnknownReleaseDetail(Plannedactivities plannedActivityEntity, bool settingPASpecifyDcRule, long specifyDcId, long originalLcmDcId)
        {
            if (settingPASpecifyDcRule == true && plannedActivityEntity.Ispareleasedetailunknown == true)
            {
                #region Ticket 792 Check PA Already Exists while Specify the DC for UnKnown PA , IF exists throw Error Mgs otherwise allow to set Dc
                var plannedActivityExist = LcmEngineeringExtensionMethod.PlannedActivityExists
                (plannedActivityEntity.Plannedactivityid, specifyDcId, plannedActivityEntity.Opcoid, originalLcmDcId,
                   plannedActivityEntity.Plannedactivityresourceid, _repositoryWrapper);

                if (plannedActivityExist != null)
                {
                    return plannedActivityExist;
                }
                #endregion 
                else
                {
                    plannedActivityEntity.Designcomponentid = specifyDcId;
                    plannedActivityEntity.Ispareleasedetailunknown = false;
                    var dcEntry = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == specifyDcId).SingleOrDefault();
                    plannedActivityEntity.Designcomponentfamilyid = dcEntry?.Designcomponentfamilyid;

                    #region  //Ticket 603 - #503 :  Analysis - Software Upgrade Utility 
                    if (dcEntry?.Visibleflag == false)
                    {
                        dcEntry.Visibleflag = true;
                        _repositoryWrapper.DesignComponent.Update(dcEntry);
                        _repositoryWrapper.Save();
                        await _repositoryWrapper.ClearTracker();
                    }
                    #endregion
                    var result = await GetActivityDetailsFromLcmRule((long)specifyDcId, plannedActivityEntity.Plannedactivityresource.Ruleacticvitydetails);
                    plannedActivityEntity.Activitydetails = result;

                    _repositoryWrapper.PlannedActivity.Update(plannedActivityEntity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    return plannedActivityEntity;

                }

            }
            return plannedActivityEntity;

        }
        public async Task TransferRemainingPaEntityToExistingLCM(long plannedactivityid, long currentLcmId, long existingLcmId)
        {
            var transferPAEntities = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid == currentLcmId &&
                                         x.Plannedactivityid != plannedactivityid).ToList();
            foreach (var paItem in transferPAEntities)
            {
                paItem.Lcmengineeringid = existingLcmId;
                _repositoryWrapper.PlannedActivity.Update(paItem);
            }
            _repositoryWrapper.Save();
            await _repositoryWrapper.ClearTracker();
        }

        public async Task<ResultDto> ApplyArchivingRulesToUpdatePAandAssets(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, short? plannedActivityTypeFor)
        {
            if (plannedActivityEntity.Networkelementasplannedid != null || plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.AddAsset || plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.EditAsset)
            {

                var plannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid, true, false)
                    .Include(x => x.Networkelementasplanned).FirstOrDefault();
                plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                if (settingNew.Ruleelementcount != (int)ArchivingRuleEnum.ArchivePlannedActivityAndParentEntry && settingNew.Ruleelementcount != (int)ArchivingRuleEnum.ArchivePlannedActivityOnly)
                {
                    _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }


                var asset = plannedActivity.Networkelementasplanned;
                bool isAssetMigrate = false;
                if (settingNew.Ruleelementcount == (int)ArchivingRuleEnum.ArchivePlannedActivityAndParentEntry)
                {
                    isAssetMigrate = true;
                    plannedActivity.Archived = true;
                    _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                    await _repositoryWrapper.SaveAsync();
                    asset.Deploymentstatusid = (short)UpdatePlannedActivityMethod.UpdateAssetdeploymentStatus(asset, _repositoryWrapper,
                        ConstantValueFilter.Removed).Result;

                    await _commonManager.setArchiveStatusForBpt(plannedActivity);

                }
                else if (settingNew.Ruleelementcount == (int)ArchivingRuleEnum.ArchivePlannedActivityOnly)
                {
                    plannedActivity.Archived = true;
                    _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                    await _repositoryWrapper.SaveAsync();
                    asset.Deploymentstatusid = (short)UpdatePlannedActivityMethod.UpdateAssetdeploymentStatus(asset, _repositoryWrapper,
                        ConstantValueFilter.InService).Result;

                    await _commonManager.setArchiveStatusForBpt(plannedActivity);

                }


                var activePAs = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Archived != true && x.Networkelementasplannedid == asset.Networkelementasplannedid);
                if (activePAs != null && activePAs.Count() > 0)
                {
                    var earliestPA = activePAs.FirstOrDefault(x => x.Plannedcompletion == activePAs.Min(p => p.Plannedcompletion));

                    var earliestSetting = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.EditAsset && x.Plannedactivityresourceid == earliestPA.Plannedactivityresourceid && x.Deliverystatusid == earliestPA.Deliverystatusid)
                          .Include(x => x.Deliverystatus)
                          .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus)?.FirstOrDefault();

                    var assetDeploymentStatus = earliestSetting?.Settingupdateplannedactivityassetdeploymentstatus?.FirstOrDefault()?.Assetdeploymentstatusid;

                    asset.Deploymentstatusid = assetDeploymentStatus != null ? assetDeploymentStatus.Value : asset.Deploymentstatusid;
                }
                else
                {
                    asset.Deploymentstatusid = (short)((settingNew.Settingupdateplannedactivityassetdeploymentstatus != null && settingNew.Settingupdateplannedactivityassetdeploymentstatus.Count > 0) ? settingNew.Settingupdateplannedactivityassetdeploymentstatus.FirstOrDefault()?.Assetdeploymentstatusid : asset.Deploymentstatusid);
                    asset.Plannedaction = false;
                }

                _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                await _repositoryWrapper.SaveAsync();



                return new ResultDto
                {
                    Info = (isAssetMigrate) ? ConstantValueFilter.assetMigrated : ResultMessages.EntryUpdateSuccess,
                    Data = plannedActivity
                };
            }
            return new ResultDto
            {
                Info = "",
                Data = ""
            };

        }
        #endregion

        #region Exodus

        public async Task<ResultDto> ArchivePAWithDaMigration(DaMigrationStatusAddUpdateDto dtoDaMigrateRecords, long PaId, short settingsPaId)
        {
            try
            {
                await _daMigrationStatusManager.AddOrUpdateLocationAsync(dtoDaMigrateRecords);
                await _updatePlannedActivityManager.Value.LcmCreationForInfraReadyDesignAspectPa(PaId);
                await _designAspectPlannedActivityManger.ArchivePaWhenDaMigrationComplete(PaId, settingsPaId);
                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess

                };
            }
            catch
            {
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        public async Task<ResultDto> ArchivePaWhenDaAssetMigrationComplete([FromBody] DaAssetMigrationAddUpdateDto dto, long PaId, short settingsPaId)
        {
            try
            {

                await _designAspectPlannedActivityManger.ArchivePaWhenDaAssetMigrationComplete(dto, PaId, settingsPaId);
                return new ResultDto
                {
                    //Info = ResultMessages.EntryUpdateSuccess  // unwantedly display in ui

                };
            }
            catch
            {
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        //
        public async Task<ResultDto> GetUpdatePlannedActivityStatusForExodus(long id, PlannedActivityTypeForEnum plannedActivityTypeFor, DateTime? Migrationcompletiondate, bool isDaAssetMigration)
        {

            var UpdateResultDto = await _updatePlannedActivityManager.Value.GetUpdatePlannedActivityStatus(id, plannedActivityTypeFor, isDaAssetMigration);
            var UpdatePlannedActivityStatusDto = UpdateResultDto.Data;

            if (UpdatePlannedActivityStatusDto != null)
            {
                UpdatePlannedActivityStatusDto.AssetLiveStatusDate = Migrationcompletiondate;

                var archiveLcmPaResult = await _updatePlannedActivityManager.Value.SaveUpdatePlannedActivityStatus(UpdatePlannedActivityStatusDto);

                var tt = archiveLcmPaResult;
            }

            return new ResultDto();

        }


        #endregion
        #region ClusterLevel PA

        public async Task<ResultDto> ApplySettingUpdateRulesToClusterLevelPA(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, InfraClusterClusterUpgradeUpsertDto infraClusterClusterUpgradeUpsertDto)
        {
          return  await _infraClusterManager.Value.ApplySettingUpdateRulesToClusterLevelPA(plannedActivityEntity, settingNew, infraClusterClusterUpgradeUpsertDto);
        }

        public async Task<ResultDto> AddOrUpdateClusterLevelData(InfraClusterClusterUpgradeUpsertDto dtoInfraClusterRecords, long paId,int? ruleLinkedDc = 0 )
        {
            var clusterLevelPA = await _infraClusterManager.Value.AddOrUpdateInfraClusterAsync(dtoInfraClusterRecords, paId,false, ruleLinkedDc);
            return clusterLevelPA;
        }
        public async Task<ResultDto> ApplySettingUpdateRulesForUpgradeCluster(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, InfraClusterClusterUpgradeUpsertDto dtoInfraClusterRecords)
        {
            var clusterLevelPA = await _infraClusterManager.Value.ApplySettingUpdateRulesForUpgradeCluster(plannedActivityEntity, settingNew, dtoInfraClusterRecords);
            return clusterLevelPA;
        }
        public async Task<ResultDto> ApplySettingUpdateRulesForAddorRemoveCluster(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, InfraClusterClusterUpgradeUpsertDto dtoInfraClusterRecords)
        {
            var clusterLevelPA = await _infraClusterManager.Value.ApplySettingUpdateRulesForAddorRemoveCluster(plannedActivityEntity, settingNew, dtoInfraClusterRecords);
            return clusterLevelPA;
        }


        public Dictionary<long,string> GetHardwareMhwResource(short opCoId, short dcId)
        {
            var clusterLevelPA =  _infraClusterManager.Value.GetAddPage(opCoId, dcId).Result.HardwareMhwResource.ToDictionary(x => x.Key , x => x.Text);
            return clusterLevelPA;
        }

        public List<FilterValueDtoKeyValueList> GetVerticalDetails(List<int?> aspnetId, long? lcmOpcoId,Designaspects designAspects=null,Serviceplan serviceInfo=null)
        {
            if (designAspects != null)
            {
                var mjSwDesignContact = designAspects?.Designcomponentfamily?.Designcomponents?
                                      .SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                                     .Where(x => x.Deleted == false)
                                     .Select(m => (int?)m.Designcontactid))?.
                                      Distinct().ToList();

                var majorHardwareDesignContact = designAspects?.Designcomponentfamily?.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                          .SelectMany(m => m.Majorhardware?.Majorhwbuildsdesigncontacts.
                                          Where(x => x.Deleted == false)
                                          .Select(n => (int?)n.Designcontactid))?.
                                          Distinct().ToList())?.ToList();

                if (mjSwDesignContact?.Any() == true && majorHardwareDesignContact?.Any() == true)
                    aspnetId = mjSwDesignContact.Union(majorHardwareDesignContact).ToList();
                else if (mjSwDesignContact?.Any() == false && majorHardwareDesignContact?.Any() == true)
                    aspnetId = majorHardwareDesignContact;
            }
            if (serviceInfo != null)
            {

                aspnetId =    serviceInfo !=null?
                              serviceInfo.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                              .SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                             .Where(x => x.Deleted == false)
                             .Select(m => (int?)m.Designcontactid)))?.
                              Distinct().ToList() : null;
                var majorHardwareDesignContact = serviceInfo != null  ?
                                          serviceInfo.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                          .SelectMany(m => m.Majorhardware?.Majorhwbuildsdesigncontacts.
                                          Where(x => x.Deleted == false)
                                          .Select(n => (int?)n.Designcontactid))?.
                                          Distinct().ToList()))?.ToList() : null;
                if (aspnetId?.Any() == true && majorHardwareDesignContact?.Any() == true)
                    aspnetId = aspnetId.Union(majorHardwareDesignContact).ToList();
                else if (aspnetId?.Any() == false && majorHardwareDesignContact?.Any() == true)
                    aspnetId = majorHardwareDesignContact;
            }

            return _commonManager.GetVerticaleFilterDto(aspnetId, lcmOpcoId)?
            .Select(t => new FilterValueDtoKeyValueList
            {
                Key = Convert.ToInt16(t.Value),
                Value = t.Text
            })?.Distinct()?.ToList();
        }


        #endregion

    }
}
