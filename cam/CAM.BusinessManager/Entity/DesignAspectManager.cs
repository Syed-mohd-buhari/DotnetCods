using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity.ExodusProgram;
using CAM.BusinessManager.Entity.PlannedActivities;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignAspects;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using IdentityServer4.Extensions;
using LinqKit;
using Microsoft.AspNetCore.Http;
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
    public class DesignAspectManager : BaseManager
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly DesignComponentFamilyManager _designComponentFamilyManager;
        private readonly GridCustomColumnManager _manager;
        private readonly PlannedActivityManager _plannedActivityManager;
        private readonly DeliveryTrackingManager _deliveryTrackingManager;
        private readonly CommonManager _commonManager;
        private readonly ILoggerManager _logger;
        private readonly DaMigrationStatusManager _daMigrationStatusManager;
        private readonly DesignAspectPlannedActivityManger _designAspectPlannedActivityManger;
        private readonly PlatformMigrationManager _platformMigrationManager;
        private bool isNoDaPa = false;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;

        //private readonly AuthorizedRoleManager _authorizedRoleManager;
        //private readonly long sessionUserId;
        //private readonly int adminRoleId;
        //private readonly ICurrentUserService _currentUserService;
        //private readonly List<short> _opcoList;
        public DesignAspectManager(IEnumerable<IRepositoryWrapper> wrappers, PlannedActivityManager plannedActivityManager,
            DesignComponentFamilyManager designComponentFamilyManager, IMapper mapper, GridCustomColumnManager manager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, DeliveryTrackingManager deliveryTrackingManager,
            DropdownDataServiceManager dropdownDataServiceManager
            , CommonManager commonManager, ILoggerManager logger, DaMigrationStatusManager  daMigrationStatusManager//, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager
           , DesignAspectPlannedActivityManger designAspectPlannedActivityManger, PlatformMigrationManager platformMigrationManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _mapper = mapper;
            _designComponentFamilyManager = designComponentFamilyManager;
            _plannedActivityManager = plannedActivityManager;
            _deliveryTrackingManager = deliveryTrackingManager;
            _commonManager = commonManager;
            _logger = logger;
            _daMigrationStatusManager = daMigrationStatusManager;
            _designAspectPlannedActivityManger = designAspectPlannedActivityManger;
            _platformMigrationManager = platformMigrationManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            //_currentUserService = currentUserService;
            //_authorizedRoleManager = authorizedRoleManager;
            //this.adminRoleId = _currentUserService.adminRoleId;
            //this.sessionUserId = _currentUserService.UserId;

            //var _roleOpcoList = _authorizedRoleManager.GetUserRoleOpcoList(this.sessionUserId);

            //var _adminRoleCheck = _roleOpcoList.Where(x => x.Role == this.adminRoleId.ToString()).Select(x => x.Role).FirstOrDefault();

            //_opcoList = (_adminRoleCheck != null) ? null :  _roleOpcoList.Select(x => Convert.ToInt16(x.OpCo)).Distinct().ToList();



        }

        public async Task<ResultDto> GetDCFName(long designcomponentfamilyId)
        {
            var dcf = await Task.Run(()  => _repositoryWrapper.DesignComponentFamily
                .FindByCondition(p => p.Designcomponentfamilyid == designcomponentfamilyId)
                .Include(p => p.Subnetworkboundary)
                .SingleOrDefault());
            var name = dcf.DCFName(_repositoryWrapper);
            return new ResultDto
            {
                Warning = false,
                Data = new { DesignComponentName = name }
            };
        }
        public DesignAspectDtoModel GetCreatedPage(List<long> _opcoList, long? dcfId = null)
        {
            var selectedDCF = dcfId.HasValue ? _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == dcfId.Value).Include(p => p.Subnetworkboundary).FirstOrDefault() : null;
            var isSupportedAllServices = dcfId.HasValue ? (selectedDCF.Subnetworkboundary.Default ?? false) : false;
            //var opcoResources = !dcfId.HasValue ? GetImplemenatedOpcos() : _designComponentFamilyManager.GetImplementionOpcos(dcfId.Value);

            var opcoResources = (_opcoList != null && _opcoList.Any() == true) ?
                                 _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco)
                                 : _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            var dcfResources = !dcfId.HasValue ? new List<Designcomponentfamilies>() : new List<Designcomponentfamilies>() { selectedDCF };
            var authenicationResources = _repositoryWrapper.AuthenicationTypeRepository.FindAll();
            var thirdPartyAccessTypeResources = _repositoryWrapper.ThirdPartyAccessTypeRepository.FindAll();
            var geoResilienceResources = _repositoryWrapper.BusinessContinuityMethodRepository.FindAll();
            var instanceResilienceResources = _repositoryWrapper.InstanceResilienceRepository.FindAll();
            var licenseModelResources = _repositoryWrapper.LicenseModelRepository.FindAll();
            var securityManagerResources = _repositoryWrapper.SecurityManagerRepository.FindAll();
            var siteResilienceResources = _repositoryWrapper.SiteResilienceRepository.FindAll();
            var swDeliveryLifeCycleResources = _repositoryWrapper.SWDeliveryLifeCycleRepository.FindAll();
            var securityTireResources = _repositoryWrapper.SecurityTireZone.FindAll();
            var supportedServiceResources = dcfId.HasValue ? _repositoryWrapper.SubnetworkSupportedServiceRepository.FindByCondition(p => p.Subnetworkid == selectedDCF.Subnetworkboundaryid).Include(p => p.Service).Select(p => p.Service).ToList() : new List<Supportedservices>();
            var usedNetworkFunctionResources = dcfId.HasValue ? _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == dcfId.Value)
                .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                .FirstOrDefault()
                .Designcomponents.Select(x => x.Systemtype.Majorsoftwarebuilds).SelectMany(x => x.Majorsoftwarebuildnetworkfunction).Select(x => new { Id = x.Networkfunctionid, Description = x.Networkfunction.Description }).Distinct()
                : null;

            var model = new DesignAspectDtoModel
            {
                OpcosResource = opcoResources,
                DCFsResource = dcfResources.ToDictionary(x => x.Designcomponentfamilyid, x => x.DCFName(_repositoryWrapper)).Where(f => !f.Value.IsNullOrEmpty())
                .ToDictionary(x => x.Key, y => y.Value),
                AuthenicationTypesResource = authenicationResources.ToDictionary(x => x.Id, x => x.Description),
                ThirdPartyAccessResource = thirdPartyAccessTypeResources.ToDictionary(x => x.Id, x => x.Description),
                SiteResilienceMethodsResource = geoResilienceResources.ToDictionary(x => x.Id, x => x.Description),
                InstanseResiliencesResource = instanceResilienceResources.ToDictionary(x => x.Id, x => x.Description),
                SiteResiliencesResource = siteResilienceResources.ToDictionary(x => x.Id, x => x.Description),
                SecurityManagersResource = securityManagerResources.ToDictionary(x => x.Id, x => x.Description),
                SWDeliveryLifeCyclesResource = swDeliveryLifeCycleResources.ToDictionary(x => x.Id, x => x.Description),
                SecurityTireZoneResource = securityTireResources.ToDictionary(x => x.Securitytirezoneid, x => x.Description),
                LicenseModelsResource = licenseModelResources.ToDictionary(x => x.Id, x => x.Description),
                SupportedServicesResource = supportedServiceResources.ToDictionary(x => x.Id, x => x.Description),
                UsedNetworkFunctionsResource = usedNetworkFunctionResources?.ToDictionary(x => x.Id, x => x.Description),
                SubNetworkBoundary = dcfId.HasValue ? (string.IsNullOrEmpty(selectedDCF.Subnetworkboundary.Alias) ? selectedDCF.Subnetworkboundary.Description : selectedDCF.Subnetworkboundary.Alias) : null,
                DesignComponentFamilyName = selectedDCF != null ? selectedDCF.DCFName(_repositoryWrapper) : null,
                DesignComponentFamilyId = dcfId.HasValue ? dcfId.Value : 0,
                isSupportedAllServices = isSupportedAllServices
            };
            return model;
        }

        public async Task< DesignAspectDtoModel> GetUpdatedPage(long id,List<long> _opcoList,List<long> _verticalList)
        {
            var dbModel = _repositoryWrapper.DesignAspectRepository.FindByCondition(p => p.Id == id)
                .Include(p => p.ModificationuserNavigation)
                .Include(p => p.CreationuserNavigation).FirstOrDefault();
            var dcfId = dbModel.Designcomponentfamilyid;
            var model = new DesignAspectDtoModel();
            if (dbModel != null)
            {
                model = Bind(DesignAspectMapper.Get(dbModel));
            }
            var selectedDCF = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == dcfId)
                .Include(p => p.Subnetworkboundary).Include(x => x.Productname).SingleOrDefault();
            var isSupportedAllServices = selectedDCF != null ? (selectedDCF.Subnetworkboundary.Default ?? false) : false;
            //var opcoResources = GetImplemenatedOpcos();
            var opcoResources = (_opcoList != null && _opcoList.Any() == true) ?
                                 _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco)
                                 : _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);

            var dcfResources = new List<Designcomponentfamilies>() { selectedDCF };
            var authenicationResources = _repositoryWrapper.AuthenicationTypeRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
            var thirdPartyAccessTypeResources = _repositoryWrapper.ThirdPartyAccessTypeRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);

            var geoResilienceResources = _repositoryWrapper.BusinessContinuityMethodRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
            var instanceResilienceResources = _repositoryWrapper.InstanceResilienceRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
            var licenseModelResources = _repositoryWrapper.LicenseModelRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
            var securityManagerResources = _repositoryWrapper.SecurityManagerRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
            var siteResilienceResources = _repositoryWrapper.SiteResilienceRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
            var swDeliveryLifeCycleResources = _repositoryWrapper.SWDeliveryLifeCycleRepository.FindAll().ToDictionary(x => x.Id, x => x.Description);
            var securityTireResources = _repositoryWrapper.SecurityTireZone.FindAll().ToDictionary(x => x.Securitytirezoneid, x => x.Description);

            var supportedServiceResources = _repositoryWrapper.SubnetworkSupportedServiceRepository
                .FindByCondition(p => p.Subnetworkid == selectedDCF.Subnetworkboundaryid).Include(p => p.Service).Select(p => p.Service)
                .ToDictionary(x => x.Id, x => x.Description);
            var usedNetworkFunctionResources = _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == dcfId)
                .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                .FirstOrDefault()
                .Designcomponents.Select(x => x.Systemtype.Majorsoftwarebuilds).SelectMany(x => x.Majorsoftwarebuildnetworkfunction)
                .Select(x => new { Id = x.Networkfunctionid, Description = x.Networkfunction.Description }).Distinct().ToList();

            var da_SupportedSvr = _repositoryWrapper.DesignAspectSupportedServiceRepository.FindByCondition(p => p.Designaspectid == model.Id).ToList();

            var da_usedNFs = _repositoryWrapper.DesignAspectNetworkFunctionRepository.FindByCondition(p => p.Designaspectid == model.Id);

            model.OpcosResource = opcoResources;
            model.DCFsResource = dcfResources.ToDictionary(x => x.Designcomponentfamilyid, x => x.DCFName(_repositoryWrapper))
                .Where(f => !f.Value.IsNullOrEmpty()).ToDictionary(k => k.Key, v => v.Value);
            model.AuthenicationTypesResource = authenicationResources;
            model.ThirdPartyAccessResource = thirdPartyAccessTypeResources;
            model.SiteResilienceMethodsResource = geoResilienceResources;
            model.InstanseResiliencesResource = instanceResilienceResources;
            model.SiteResiliencesResource = siteResilienceResources;
            model.SecurityManagersResource = securityManagerResources;
            model.SWDeliveryLifeCyclesResource = swDeliveryLifeCycleResources;
            model.SecurityTireZoneResource = securityTireResources;
            model.LicenseModelsResource = licenseModelResources;
            model.SupportedServicesResource = supportedServiceResources;
            model.UsedNetworkFunctionsResource = usedNetworkFunctionResources?.ToDictionary(x => x.Id, x => x.Description);
            model.SubNetworkBoundary = (string.IsNullOrEmpty(selectedDCF.Subnetworkboundary.Alias) ? selectedDCF.Subnetworkboundary.Description : selectedDCF.Subnetworkboundary.Alias);
            model.DesignComponentFamilyName = selectedDCF != null ? selectedDCF.DCFName(_repositoryWrapper) : null;
            model.SupportedServicesIds = da_SupportedSvr.Select(p => p.Serviceid).AsEnumerable();
            model.UsedNetworkFunctionsIds = da_usedNFs.Select(p => p.Networkfunctionid).AsEnumerable();
            model.DesignComponentFamilyId = dcfId;
            model.isSupportedAllServices = isSupportedAllServices;
            model.NominalCapacityLimit = model.NominalCapacityLimit;
            model.MaxAllowedLoading = model.MaxAllowedLoading;
            model.DesignedCapacityLimit = model.DesignedCapacityLimit;
            model.PlatformSoftware = selectedDCF.Productname.Isplatformsoftware;
            List<short> opcoConvertedList = (_opcoList != null) ?
                _opcoList.Select(x => (short)Convert.ToInt16(x)).ToList() : null;

            List<int> verticalConvertedList = (_verticalList != null) ?
               _verticalList.Select(x => (int)Convert.ToInt16(x)).ToList() : null;

            var plannedActivitiesId = await Task.Run(() => _repositoryWrapper.PlannedActivity.FindByCondition(p => p.Archived != true && p.Designaspectid == id)
            .Select(p => p.Plannedactivityid).ToList());

            model.PlannedActivityDto = new List<PlannedActivityDtoUpdate>();

            foreach (var planned in plannedActivitiesId)
            {
                model.PlannedActivityDto.Add(await _plannedActivityManager.GetUpdatePage(planned, opcoConvertedList, verticalConvertedList,0,model.DesignComponentFamilyId));
            }

            #region exodus
            if (plannedActivitiesId.Count > 0)
            {
                DaMigrationStatusQueryDto daMigrationFilterDto = new DaMigrationStatusQueryDto
                {
                    PlannedActivityId = plannedActivitiesId
                };
                var existingDaMigartion = _daMigrationStatusManager.FindWithConditionAsync(daMigrationFilterDto).Result.Items.ToList();
                model.DaMigrationStatusEntity = existingDaMigartion;


                //DaAssetMigrationQueryDto daAssetMigrationFilter = new DaAssetMigrationQueryDto
                //{
                //    PlannedActivityId = plannedActivitiesId
                //};
                //var existingDaAssetMigration = _platformMigrationManager.FindWithConditionAsync(daAssetMigrationFilter).Result.Items.ToList();
                //model.DaAssetMigrationEntity = existingDaAssetMigration;
            }

            #endregion
            return model;
        }

        public async Task<ResultDto> Add(DesignAspectDtoModel dto)
        {
            var existEntity = EntityExists(dto);
            if (existEntity != null)
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryAddExists,
                    Data = new { id = existEntity.Id, orphanDeleted = true }
                };
            else
            {
                if (dto.PlannedActivityDto != null && dto.PlannedActivityDto.Count > 1 && dto.PlannedActivityDto.Any())
                {
                    var isduplicate = await DuplicateCheckForMultipleDcf(dto.PlannedActivityDto);
                    if (isduplicate == true)
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.PlannedDCFEntryHasLinkedPlannedActivities
                        };
                    }
                }
                var model = DesignAspectMapper.Set(Bind(dto));

                foreach (var item in dto.SupportedServicesIds)
                {
                    model.Designaspectssupportedsvr.Add(new Designaspectssupportedsvr() { Designaspectid = model.Id, Serviceid = item });
                }
                foreach (var item in dto.UsedNetworkFunctionsIds)
                {
                    model.Designaspectsnetworkfunctions.Add(new Designaspectsnetworkfunctions() { Designaspectid = model.Id, Networkfunctionid = item });
                }
                var toupdatedPlannedActivities = await UpdatePlannedActivity(dto);
                model.Archived = false;
                try
                {
                    foreach (var item in toupdatedPlannedActivities)
                    {
                        item.Designcomponentfamilyid = dto.DesignComponentFamilyId;
                        item.Designaspectid = model.Id;
                        if (item.Plannedactivityid == 0)
                        {

                            var plannedActivityExist = PlannedActivityExists(item.Plannedactivityid, model.Opcoid,
                                model.Id, item.Plannedactivityresourceid);
                            if (plannedActivityExist != null)
                            {
                                return new ResultDto
                                {
                                    Warning = true,
                                    Info = ResultMessages.EntryAddExists,
                                    Data = new { id = plannedActivityExist.Plannedactivityid, orphanDeleted = true }
                                };
                            }
                            else
                            {
                                _repositoryWrapper.PlannedActivity.Create(item);
                                await _repositoryWrapper.SaveAsync();
                                if (item.Deliveryplanavailable)
                                {
                                    var mileStoneStatus = await _commonManager.CalculateMSDuration(item, _repositoryWrapper);
                                    if (mileStoneStatus.isSuccess)
                                    {
                                        // We should pass inservice and planned asset count only 
                                        var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                        var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                        // int assetCount = savedData.Deploymentstatusid == getAssetInserviceID && savedData.Environmentid == getAssetInserviceID ? 1 : 0;

                                        var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, item, 0);
                                        await _deliveryTrackingManager.Add(deliveryTracking);
                                    }

                                }
                                var addProjectPlanEntity = await _commonManager.AddProjectPlan(item, _repositoryWrapper);

                                foreach (var plan in addProjectPlanEntity)
                                {
                                    _repositoryWrapper.ProjectPlanRepository.Create(plan);
                                    await _repositoryWrapper.SaveAsync();

                                    await _commonManager.CreateOrUpdateProjectPlanAudit(plan.Projectsplanid, null, item.Plannedcompletion.Value.ToShortDateString(), 1, _repositoryWrapper);
                                }
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                            }
                        }
                        else
                        {
                            var plannedActivityExist = PlannedActivityExists(item.Plannedactivityid, model.Opcoid,
                                model.Id, item.Plannedactivityresourceid);
                            if (plannedActivityExist != null)
                            {
                                return new ResultDto
                                {
                                    Warning = true,
                                    Info = ResultMessages.EntryUpdateExists,
                                    Data = new { id = plannedActivityExist.Plannedactivityid, orphanDeleted = true }
                                };
                            }
                            else
                            {
                                _repositoryWrapper.PlannedActivity.Update(item);

                                if (item.Deliveryplanavailable)
                                {
                                    var mileStoneStatus = await _commonManager.CalculateMSDuration(item, _repositoryWrapper);
                                    if (mileStoneStatus.isSuccess)
                                    {
                                        // We should pass inservice and planned asset count only 
                                        var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                        var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;

                                        var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, item, 0);
                                        await _deliveryTrackingManager.Add(deliveryTracking);
                                    }

                                }
                                var addProjectPlanEntity = await _commonManager.AddProjectPlan(item, _repositoryWrapper);

                                foreach (var plan in addProjectPlanEntity)
                                {
                                    _repositoryWrapper.ProjectPlanRepository.Create(plan);
                                }
                            }
                        }
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        var settingRuleElementCount = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.DesignAspect && x.Plannedactivityresourceid == item.Plannedactivityresourceid && x.Deliverystatusid == item.Deliverystatusid)
                        .Include(x => x.Deliverystatus)?.FirstOrDefault()?.Ruleelementcount;

                        if (settingRuleElementCount == (int)RuleElementCountEnum.RolloutComplete)
                        {
                            item.Archived = true;
                        }
                        _repositoryWrapper.PlannedActivity.Update(item);
                        await _repositoryWrapper.SaveAsync();
  
                        await _commonManager.CreateOrUpdateBptreport(item.Plannedactivityid, 0, 0, model.Designcomponentfamilyid);
                    }

                    _repositoryWrapper.DesignAspectRepository.Create(model);
                    await _repositoryWrapper.SaveAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Add DesignAspect : " + ex);

                }

                return new ResultDto { Info = ResultMessages.EntryAddSuccess };
            }
        }
        public Plannedactivities PlannedActivityExists(long plannedActivityId, short? OpcoId, long designAspectId, short? plannedActivityResourceID)
        {
            var query = _repositoryWrapper.PlannedActivity.FindByCondition(x =>
                    (plannedActivityId == 0 || x.Plannedactivityid != plannedActivityId) &&
                    x.Plannedactivityresourceid == plannedActivityResourceID &&
                    x.Opcoid == OpcoId &&
                    x.Designaspectid == designAspectId &&
                    x.Archived != true)
                .Include(x => x.Designaspect)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Damigrationstatus);

            var uniqueDaPa = query.FirstOrDefault();
            if (uniqueDaPa?.Plannedactivityresource?.Rulelinkeddc == (int)PlannedActivityResourceEnum.Project_Plan)
            {
                return null; 
            }

            return uniqueDaPa;
        }

        public async Task<ResultDto> Edit(DesignAspectDtoModel dto)
        {
            var existEntity = EntityExists(dto);
            if (existEntity != null)
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = new { id = existEntity.Id, orphanDeleted = true }
                };
            else
            {
                if(dto.PlannedActivityDto != null && dto.PlannedActivityDto.Count > 1 && dto.PlannedActivityDto.Any())
                {
                    var isduplicate = await DuplicateCheckForMultipleDcf(dto.PlannedActivityDto);
                    if(isduplicate == true)
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.PlannedDCFEntryHasLinkedPlannedActivities
                        };
                    }
                }


                var model = DesignAspectMapper.Set(Bind(dto));
                var oldUsedNFs = _repositoryWrapper.DesignAspectNetworkFunctionRepository.FindByCondition(p => p.Designaspectid == dto.Id).ToList();

                foreach (var item in oldUsedNFs)
                {
                    _repositoryWrapper.DesignAspectNetworkFunctionRepository.DeleteDeep(item);
                }

                var oldUsedServices = _repositoryWrapper.DesignAspectSupportedServiceRepository.FindByCondition(p => p.Designaspectid == dto.Id).ToList();
                foreach (var item in oldUsedServices)
                {
                    _repositoryWrapper.DesignAspectSupportedServiceRepository.DeleteDeep(item);
                }
                model.Archived = model.Archived == null ? false : model.Archived;
                _repositoryWrapper.DesignAspectRepository.Update(model);
                await _repositoryWrapper.SaveAsync();

                foreach (var item in dto.SupportedServicesIds)
                {
                    _repositoryWrapper.DesignAspectSupportedServiceRepository.Create(new Designaspectssupportedsvr() { Designaspectid = model.Id, Serviceid = item });
                }
                foreach (var item in dto.UsedNetworkFunctionsIds)
                {
                    _repositoryWrapper.DesignAspectNetworkFunctionRepository.Create(new Designaspectsnetworkfunctions() { Designaspectid = model.Id, Networkfunctionid = item });
                }


                await _repositoryWrapper.SaveAsync();

                #region PlannedActivities

                var toupdatedPlannedActivities = await UpdatePlannedActivity(dto);

                var ids = toupdatedPlannedActivities.Select(s => s.Plannedactivityid);

                var plannedActivitiestoDelete = _repositoryWrapper.PlannedActivity.FindByCondition(x =>
                  x.Designaspectid == dto.Id && !ids.Contains(x.Plannedactivityid)
              , false, false).Include(x => x.Budgetprojecttrackers).Include(x => x.Projectsplan).ThenInclude(x => x.Projectplanaudit)
              .Include(x => x.Damigrationstatus).Include(x => x.Daassetmigration).ToList();

                foreach (var plannedActivity in plannedActivitiestoDelete)
                {
                    var deliveryTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == plannedActivity.Plannedactivityid).FirstOrDefault();
                    if(deliveryTracking != null)
                        _repositoryWrapper.DeliveryTrackingRepository.DeleteDeep(deliveryTracking);
                    await _commonManager.GenerateAuditLogEntryForPAHardDeleteEntity(plannedActivity);

                    if (plannedActivity.Budgetprojecttrackers != null && plannedActivity.Budgetprojecttrackers.Count > 0)
                    {
                        if (plannedActivity.Budgetprojecttrackers.FirstOrDefault() != null)
                        {
                            _repositoryWrapper.BudgetProjectTrackersRepository.DeleteDeep(plannedActivity.Budgetprojecttrackers.FirstOrDefault());
                        }

                    }
                    foreach(var deletePp in plannedActivity.Projectsplan)
                    {
                        foreach (var deleteppa in deletePp.Projectplanaudit)
                        {
                            _repositoryWrapper.ProjectPlanAuditRepository.DeleteDeep(deleteppa);
                        }
                        _repositoryWrapper.ProjectPlanRepository.DeleteDeep(deletePp);
                    }
                    await AddOrUpdateMultipleDcf(plannedActivity.Daplannedactivitydcf.Select(x => x.Designcomponentfamilyid).ToList(), plannedActivity.Plannedactivityid,true);

                    foreach (var deleteDaMigration in plannedActivity.Damigrationstatus)
                    {
                        _repositoryWrapper.DaMigrationStatusRepository.DeleteDeep(deleteDaMigration);
                    }
                    if (plannedActivity.Daassetmigration != null && plannedActivity.Daassetmigration.Count >0)
                    {
                        await _platformMigrationManager.DeletePltformMigrationPaNewAsset(plannedActivity.Plannedactivityid);
                    }

                    foreach (var deleteDaAssetMigration in plannedActivity.Daassetmigration)
                    {
                        _repositoryWrapper.DaAssetMigrationRepository.DeleteDeep(deleteDaAssetMigration);
                    }

                    _repositoryWrapper.PlannedActivity.DeleteDeep(plannedActivity);

                    
                }

                try
                {
                    foreach (var item in toupdatedPlannedActivities)
                    {

                        //item.Designcomponentfamilyid =  dto.DesignComponentFamilyId; // Exodus 
                        item.Designaspectid = dto.Id;

                        if (item.Buildbagid == 0 )
                        {
                            var getDummyBag = _commonManager.GetDummyBag();
                            item.Buildbagid = getDummyBag.Buildbagid;
                        }
                        if (item.Plannedactivityid == 0)
                        {

                            var plannedActivityExist = PlannedActivityExists(item.Plannedactivityid, model.Opcoid,
                                model.Id, item.Plannedactivityresourceid);
                            if (plannedActivityExist != null)
                            {
                                return new ResultDto
                                {
                                    Warning = true,
                                    Info = ResultMessages.EntryAddExists,
                                    Data = new { id = plannedActivityExist.Plannedactivityid, orphanDeleted = true }
                                };
                            }
                            else
                            {
                                _repositoryWrapper.PlannedActivity.Create(item);
                                await _repositoryWrapper.SaveAsync();
                                if (item.Deliveryplanavailable && isNoDaPa == false)
                                {
                                    var mileStoneStatus = await _commonManager.CalculateMSDuration(item, _repositoryWrapper);
                                    if (mileStoneStatus.isSuccess)
                                    {
                                        // We should pass inservice and planned asset count only 
                                        var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                        var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                        // int assetCount = savedData.Deploymentstatusid == getAssetInserviceID && savedData.Environmentid == getAssetInserviceID ? 1 : 0;

                                        var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, item, 0);
                                        await _deliveryTrackingManager.Add(deliveryTracking);
                                    }

                                }
                                var addProjectPlanEntity = await _commonManager.AddProjectPlan(item, _repositoryWrapper);

                                foreach (var plan in addProjectPlanEntity)
                                {
                                    _repositoryWrapper.ProjectPlanRepository.Create(plan);
                                    await _repositoryWrapper.SaveAsync();

                                    await _commonManager.CreateOrUpdateProjectPlanAudit(plan.Projectsplanid, null, item.Plannedcompletion.Value.ToShortDateString(), 1, _repositoryWrapper);
                                }
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                            }
                        }
                        else
                        {
                            var plannedActivityExist = PlannedActivityExists(item.Plannedactivityid, model.Opcoid,
                                model.Id, item.Plannedactivityresourceid);
                            if (plannedActivityExist != null)
                            {
                                return new ResultDto
                                {
                                    Warning = true,
                                    Info = ResultMessages.EntryUpdateExists,
                                    Data = new { id = plannedActivityExist.Plannedactivityid, orphanDeleted = true }
                                };
                            }
                            else
                            {
                                _repositoryWrapper.PlannedActivity.Update(item);

                                //ProjectPlanUpdateForPlanCompletionDate
                                var pacompletiondate = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == item.Plannedactivityid).Select(x => x.Plannedcompletion).FirstOrDefault();
                                if (pacompletiondate != item.Plannedcompletion)
                                    await _commonManager.UpdateProjectPlanDateForLastDeliveryStatusOfMS(item, item.Plannedcompletion, (int)MilestoneStatusEnum.MS4,_repositoryWrapper,true);


                                if (item.Deliveryplanavailable && isNoDaPa == false)
                                {
                                    var mileStoneStatus = await _commonManager.CalculateMSDuration(item, _repositoryWrapper);
                                    if (mileStoneStatus.isSuccess)
                                    {
                                        // We should pass inservice and planned asset count only 
                                        var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                        var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                        // int assetCount = savedData.Deploymentstatusid == getAssetInserviceID && savedData.Environmentid == getAssetInserviceID ? 1 : 0;

                                        var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, item, 0);
                                        await _deliveryTrackingManager.Add(deliveryTracking);
                                    }

                                }
                                var addProjectPlanEntity = await _commonManager.AddProjectPlan(item, _repositoryWrapper);

                                foreach (var plan in addProjectPlanEntity)
                                {
                                    _repositoryWrapper.ProjectPlanRepository.Create(plan);
                                }
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                            }
                        }
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        var settingRuleElementCount = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.DesignAspect && x.Plannedactivityresourceid == item.Plannedactivityresourceid && x.Deliverystatusid == item.Deliverystatusid)
                      .Include(x => x.Deliverystatus)?.FirstOrDefault()?.Ruleelementcount;
                        item.Projectstatus = item.GetPlannedActivityProjectStatus(_repositoryWrapper).Projectstatus;
                        if (settingRuleElementCount == (int)RuleElementCountEnum.RolloutComplete)
                        {
                            item.Archived = true;
                        }
                        _repositoryWrapper.PlannedActivity.Update(item);
                        await _repositoryWrapper.SaveAsync();
                        await _commonManager.CreateOrUpdateBptreport(item.Plannedactivityid, 0, 0, (long)item.Designcomponentfamilyid);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError("Update DesignAspect : " + ex);

                }

                #endregion

                return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
            }
        }

        public async Task<ResultDto> Reset(int id)
        {
            var designAspect = _repositoryWrapper.DesignAspectRepository.FindByCondition(p => p.Id == id).Include(p => p.Designcomponentfamily).SingleOrDefault();
            var daNetworkFunctions = _repositoryWrapper.DesignAspectNetworkFunctionRepository.FindByCondition(p => p.Designaspectid == id).ToList();
            var daSupportedServices = _repositoryWrapper.DesignAspectSupportedServiceRepository.FindByCondition(p => p.Designaspectid == id).ToList();
            var supportedServiceResources = _repositoryWrapper.SubnetworkSupportedServiceRepository.FindByCondition(p => p.Subnetworkid == designAspect.Designcomponentfamily.Subnetworkboundaryid).Include(p => p.Service).Select(p => p.Service).ToList();
            var usedNetworkFunctionResources = _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == designAspect.Designcomponentfamilyid)
                .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                .FirstOrDefault()
                .Designcomponents.Select(x => x.Systemtype.Majorsoftwarebuilds).SelectMany(x => x.Majorsoftwarebuildnetworkfunction).Select(x => x.Networkfunction);
            // var usedNetworkFunctionResources = _repositoryWrapper.DesignComponentFamilyNetworkFunction.FindByCondition(p => p.Designcomponentfamilyid == designAspect.Designcomponentfamilyid).Include(p => p.Networkfunction).Select(p => p.Networkfunction).ToList();

            if (designAspect == null)
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryResetNotExists,
                    Data = new { id = id, orphanDeleted = false }
                };
            if (designAspect != null)
            {
                designAspect.Description = null;
                designAspect.Criticalnationalinfrastructure = false;
                designAspect.Instanceresilienceid = null;
                designAspect.Businesscontinuitymethodid = null;
                designAspect.Authenicationtypeid = null;
                designAspect.Securitymanagerid = null;
                designAspect.Securitytirezoneid = null;
                designAspect.Swdeliverylifecycleid = null;
                designAspect.Siteresilienceid = null;
                designAspect.Thirdpartyaccessid = null;
                designAspect.Licensemodelid = null;
                designAspect.Archived = false;
                designAspect.Nominalcapacitylimit = null;
                designAspect.Designedcapacitylimit = null;
                designAspect.Maxallowedloading = null;

            }

            foreach (var item in daNetworkFunctions)
            {
                _repositoryWrapper.DesignAspectNetworkFunctionRepository.DeleteDeep(item);
            }
            foreach (var item in daSupportedServices)
            {
                _repositoryWrapper.DesignAspectSupportedServiceRepository.DeleteDeep(item);
            }
            foreach (var item in usedNetworkFunctionResources)
            {
                _repositoryWrapper.DesignAspectNetworkFunctionRepository.Create(new Designaspectsnetworkfunctions() { Designaspectid = id, Networkfunctionid = item.Id });
            }
            foreach (var item in supportedServiceResources)
            {
                _repositoryWrapper.DesignAspectSupportedServiceRepository.Create(new Designaspectssupportedsvr() { Designaspectid = id, Serviceid = item.Id });
            }
            _repositoryWrapper.DesignAspectRepository.Update(designAspect);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Warning = true,
                Info = ResultMessages.EntryResetSuccess,
                Data = new { id = id, orphanDeleted = false }
            };
        }
        public Designaspects EntityExists(DesignAspectDtoModel dto)
        {
            return dto.Id == 0
                ? _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Designcomponentfamilyid == dto.DesignComponentFamilyId && x.Opcoid == dto.OpCoId && x.Archived != true).SingleOrDefault()
                : _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Id != dto.Id && x.Designcomponentfamilyid == dto.DesignComponentFamilyId && x.Opcoid == dto.OpCoId && x.Archived != true).SingleOrDefault();

        }

        private PlannedActivity BindPlannedActivity(PlannedActivityDtoUpdate dto, long? designAspectId)
        {
            return new PlannedActivity
            {
                DesignAspectId = designAspectId,
                DesignComponentId = dto.DesignComponentId,
                ActivityDetails = dto.ActivityDetails,
                DeliveryProjectName = dto.DeliveryProjectName,
                ActivityStatusId = dto.ActivityStatusId,
                PlannedActivityDescription = dto.PlannedActivityDescription,
                BenefitId = dto.BenefitId,
                RelatesToId = dto.RelatesToId,
                DriverId = dto.DriverId,
                IsNewServiceArchitecture = dto.IsNewServiceArchitecture,
                IsReplacementExistingSolution = dto.IsReplacementExistingSolution,
                BudgetAvailabilityId = dto.BudgetAvailabilityId,
                PlannedCompletion = dto.PlannedCompletion,
                OperationalRiskId = dto.RiskOpeId,
                EngineeringRiskId = dto.RiskEngId,
                DeliveryStatusId = dto.DeliveryStatusId,
                ResponsibilityPhaseId = dto.ResponsibilityPhaseId,
                PlannedActivityResourceId = dto.PlannedActivityResourceId,
                SpareFieldsJson = dto.SpareFieldsJson,
                PlanningActivityStatusId = dto.PlanningActivityStatusId.Value,
                LocalApproval = dto.LocalApproval,
                RiskEngineeringNotes = dto.RiskEngineeringNotes,
                BudgetTrackingId = dto.BudgetTrackingId,
                BudgetValue = dto.BudgetValue,
                Notes = dto.Notes,
                DeliveryProjectId = dto.DeliveryProjectId,
                PlannedActivityId = dto.PlannedActivityId,
                PlannedImplementationYear = dto.PlannedImplementationYear ?? 0,
                PlanningRiskId = dto.PlanningRiskId,
                RiskOperationalNotes = dto.RiskOperationalNotes,
                Currency = dto.Currency,
                StartDate = dto.StartDate,
                DeliveryPlanAvailable = dto.DeliveryPlanAvailable,
                Priority = dto.Priority,
                Plannedactivitycategoryid = dto.PlannedActivityCategoryId,
                Lcmcategories = dto.LcmCategories != null && !dto.LcmCategories.IsNullOrEmpty() ? Convert.ToInt16(dto.LcmCategories) : default(short),
                Plannedactivityteam =dto.PlannedActivityTeam,
                ProjectOwner = dto.ProjectOwner,
                ProgramId = dto.ProgramId,
                ProjectStatus = dto.ProjectStatus,
                ProjectDescription = dto.ProjectDescription,
                PreBaseLineDate = dto.PreBaseLineDate,
                DesignComponentFamilyId = dto.DesignComponentFamilyId,
                PlannedDesignComponentFamilyId = dto.PlannedDesignComponentFamilyId
            };
        }
        private DesignAspect Bind(DesignAspectDtoModel dto)
        {
            return new DesignAspect()
            {
                Id = dto.Id,
                DesignComponentFamilyId = dto.DesignComponentFamilyId,                
                BusinessContinuityMethodId = dto.BusinessContinuityMethodId,
                LicenseModelId = dto.LicenseModelId,
                InstanceResilienceId = dto.InstanceResilienceId,
                Description = dto.Description,
                AuthenicationTypeId = dto.AuthenicationTypeId,
                CriticalNationalInfrastructure = dto.CountrySpecificCriticality,
                OpCoId = dto.OpCoId,
                SecurityManagerId = dto.SecurityManagerId,
                SWDeliveryLifeCycleId = dto.SWDeliveryLifeCycleId,
                SecurityTireZoneId = dto.SecurityTireZoneId,
                SiteResilienceId = dto.SiteResilienceId,
                ThirdPartyAccessId = dto.ThirdPartyAccessId,
                NominalCapacityLimit = dto.NominalCapacityLimit,
                MaxAllowedLoading = dto.MaxAllowedLoading,
                DesignedCapacityLimit = dto.DesignedCapacityLimit,
                CriticalityRating = ComputeCriticalityRating(dto.DesignComponentFamilyId)
            };
        }
        private DesignAspectDtoModel Bind(DesignAspect dto)
        {
            var model = new DesignAspectDtoModel()
            {
                Id = dto.Id,
                DesignComponentFamilyId = dto.DesignComponentFamilyId,
                LicenseModelId = dto.LicenseModelId,
                InstanceResilienceId = dto.InstanceResilienceId,
                Description = dto.Description,
                AuthenicationTypeId = dto.AuthenicationTypeId,
                CountrySpecificCriticality = dto.CriticalNationalInfrastructure,
                OpCoId = dto.OpCoId,
                SecurityManagerId = dto.SecurityManagerId,
                SWDeliveryLifeCycleId = dto.SWDeliveryLifeCycleId,
                SecurityTireZoneId = dto.SecurityTireZoneId,
                BusinessContinuityMethodId = dto.BusinessContinuityMethodId,
                SiteResilienceId = dto.SiteResilienceId,
                ThirdPartyAccessId = dto.ThirdPartyAccessId,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity?.Email,
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                NominalCapacityLimit = dto.NominalCapacityLimit,
                MaxAllowedLoading = dto.MaxAllowedLoading,
                DesignedCapacityLimit = dto.DesignedCapacityLimit,
                CriticalityRating = ComputeCriticalityRating(dto.DesignComponentFamilyId)
            };

            return model;
        }

        private DesignAspectDtoGrid BindGrid(DesignAspect dto, List<PlannedActivity> plannedActivities)
        {
            var result = new DesignAspectDtoGrid()
            {
                Id = dto.Id,
                DesignComponentFamilyId = dto.DesignComponentFamilyId,
                LicenseModelId = dto.LicenseModelId,
                InstanceResilienceId = dto.InstanceResilienceId,
                Description = dto.Description,
                AuthenicationTypeId = dto.AuthenicationTypeId,
                CountrySpecificCriticality = dto.CriticalNationalInfrastructure,
                OpCoId = dto.OpCoId,
                SecurityManagerId = dto.SecurityManagerId,
                SWDeliveryLifeCycleId = dto.SWDeliveryLifeCycleId,
                SecurityTireZoneId = dto.SecurityTireZoneId,
                BusinessContinuityMethodId = dto.BusinessContinuityMethodId,
                SiteResilienceId = dto.SiteResilienceId,
                ThirdPartyAccessId = dto.ThirdPartyAccessId,
                AuthenicationTypeName = dto.AuthenicationType?.Description,
                CriticalNationalInfrastructureName = dto.CriticalNationalInfrastructure == false ? ConstantValueFilter.CamalNo : ConstantValueFilter.CamalYes,
                DesignComponentFamilyName = dto.DesignComponentFamily.DCFName(_repositoryWrapper),
                PlatformSoftware = dto.DesignComponentFamily.ProductNameNavigation.IsPlatformSoftware,
                InstanceResilienceName = dto.InstanceResilience?.Description,
                LicenseModelName = dto.LicenseModel?.Description,
                OpCoName = dto.OpCo.OpCoDescription,
                SecurityManagerName = dto.SecurityManager?.Description,
                SecurityTireZoneName = dto.SecurityTireZone?.SecurityTireZoneDescription,
                SiteResilienceName = dto.SiteResilience?.Description,
                SwDeliveryLifeCycleName = dto.SWDeliveryLifeCycle?.Description,
                ThirdPartyAccessName = dto.ThirdPartyAccessType?.Description,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                BusinessContinuityMethodName = dto.BusinessContinuityMethod?.Description,
                SubNetworkBoundary = string.IsNullOrEmpty(dto.DesignComponentFamily.SubNetworkBoundary.Alias) ? dto.DesignComponentFamily.SubNetworkBoundary.Description : dto.DesignComponentFamily.SubNetworkBoundary.Alias,
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Archived = dto.Archived,
                VodafoneName = dto.DesignComponentFamily?.SubNetworkBoundary?.VodafoneName?.Description,
                NominalCapacityLimit = dto.NominalCapacityLimit,
                MaxAllowedLoading = dto.MaxAllowedLoading,
                DesignedCapacityLimit = dto.DesignedCapacityLimit,
                CriticalityRating = dto.CriticalityRating,
                VerticalName = (dto.VerticalFilterDto != null) ? string.Join(",", dto.VerticalFilterDto.Select(t => t.Value).ToList() ?? new List<string>()) : string.Empty


            };
            result.SupportedServices = string.Join(",", dto.DesignAspectSupportedServices.Select(p => p.SupportedService.Description));
            result.UsedNetworkFunctions = string.Join(",", dto.DesignAspectNetworkFunctions.Select(p => p.NetworkFunction.Description));

            result.PlannedActivity = (plannedActivities != null && plannedActivities.Any()) ? 
                (plannedActivities.Where(x => !x.Deleted && x.Archived != true).ToDictionary(x => (short)x.PlannedActivityId, x => x.GetPlannedAction(_repositoryWrapper))) : new Dictionary<short, string>();
            return result;
        }

        public int? ComputeCriticalityRating(long dcfid)
        {
            // bool? Pcisox, bool? C3C4, int? GDPRClassification, bool? InternetFacing,
            //bool? MissionCritical, bool? SecurityElement, /*bool CountrySpecificCriticality*/
            int CriticalityRating = 0;

            var dcf = _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == dcfid).Include(x => x.Subnetworkboundary).FirstOrDefault();

            var countryspecificcriticality = GetCriticalCountryValue(dcfid);


            if (dcf != null)
            {
                if (dcf.Subnetworkboundary.PciSox == true)
                {
                    CriticalityRating += 1;
                }
                if (dcf.Subnetworkboundary.C3C4 == true)
                {
                    CriticalityRating += 1;
                }
                if (dcf.Subnetworkboundary.Gdprclassification == 2)
                {
                    CriticalityRating += 1;
                }
                if (dcf.Subnetworkboundary.Internetfacing == true)
                {
                    CriticalityRating += 1;
                }
                if (dcf.Subnetworkboundary.Missioncritical == true)
                {
                    CriticalityRating += 1;
                }
                if (dcf.Subnetworkboundary.Securityelement == true)
                {
                    CriticalityRating += 1;
                }
                if (countryspecificcriticality == true)
                {
                    CriticalityRating += 1;
                }
            }




            return CriticalityRating;

        }
        private bool GetCriticalCountryValue(long dcfId)
        {
            var designAspectList = _repositoryWrapper.DesignAspectRepository.FindByCondition(p => p.Designcomponentfamilyid == dcfId).ToList();
            if (!designAspectList.Any())
                return false;

            return designAspectList.Any(p => p.Criticalnationalinfrastructure == true);


        }



        public Dictionary<long, string> GetDCFPerOpco(int opcoId)
        {
            var designComponentFamilies = _repositoryWrapper.DesignComponentFamily.FindAll();
            var dCFs = designComponentFamilies.ToList()
                .Select(x => new { x.Designcomponentfamilyid, Name=x.DCFName(_repositoryWrapper) })
                .Where(x=>!string.IsNullOrEmpty(x.Name)).ToDictionary(y=>y.Designcomponentfamilyid,y=>y.Name);
           
            return dCFs;
        }
        public Dictionary<long, string> ExistingGetDCFPerOpco(int opcoId)
        {
            var selectedOpco = _repositoryWrapper.OpCo.FindByCondition(p => p.Opcoid == opcoId).SingleOrDefault();
            var lCMs = _repositoryWrapper.Lcmengineering.FindByCondition(p => p.Opcoid == opcoId)
                            .Include(p => p.Designcomponent)
                            .ThenInclude(p => p.Designcomponentfamily).AsEnumerable()/*.Where(p => p.CountNodes(false, _repositoryWrapper) > 0)*/.ToList();
            var dcfIds = lCMs.Select(p => p.Designcomponent.Designcomponentfamilyid).Distinct().ToList();
            var designComponentFamilies = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => dcfIds.Contains(p.Designcomponentfamilyid));
            var dCFs = designComponentFamilies.ToDictionary(x => x.Designcomponentfamilyid, y => y.DCFName(_repositoryWrapper));


            return dCFs;
        }
        public List<Opcos> GetImplemenatedOpcos()
        {
            // Changes required otherwise throw column null value error
            var lCMs = _repositoryWrapper.Lcmengineering.FindAll()
                .Include(x => x.Opco)
                .Include(x => x.Designcomponent).AsEnumerable().Where(p => p.CountNodes(false, _repositoryWrapper) > 0);            
            return lCMs.Select(p => p.Opco).DistinctBy(x => x.Opcoid).ToList();
        }

        public DesignAspectDCFModel GetServicesAndNetworks(long dcfId)
        {
            var model = new DesignAspectDCFModel();
            var selectedDCF = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == dcfId).Include(p => p.Subnetworkboundary).Include(x => x.Productname).SingleOrDefault();

            model.PlatformSoftware = selectedDCF.Productname.Isplatformsoftware;

            var isSupportedAllServices = selectedDCF != null ? (selectedDCF.Subnetworkboundary.Default ?? false) : false;
            model.IsSupportedAllServices = isSupportedAllServices;
            var supportedServiceResources = _repositoryWrapper.SubnetworkSupportedServiceRepository.FindByCondition(p => p.Subnetworkid == selectedDCF.Subnetworkboundaryid).Include(p => p.Service).Select(p => p.Service).ToList();
            model.Services = supportedServiceResources.ToDictionary(x => x.Id, y => y.Description);

            var usedNetworkFunctionResources = _repositoryWrapper.MajorSoftwareBuildNetworkFunction.FindByCondition(p => p.Majorsoftwarebuildid == selectedDCF.Designcomponentfamilyid).Include(p => p.Networkfunction).Select(p => p.Networkfunction).ToList();
            model.NetworkFunctions = usedNetworkFunctionResources.ToDictionary(x => x.Id, y => y.Description);

            var opcoResources = _designComponentFamilyManager.GetImplementionOpcos(dcfId);
            model.SubNetworkBoundary = string.IsNullOrEmpty(selectedDCF.Subnetworkboundary.Alias) ? selectedDCF.Subnetworkboundary.Description : selectedDCF.Subnetworkboundary.Alias;

            return model;
        }

        private IQueryable<DesignAspect> PrepareQuery(ExpressionStarter<Designaspects> predicateResult,
          bool includeDeleted)
        {
            var result =   _repositoryWrapper.DesignAspectRepository.FindByCondition(predicateResult, includeDeleted)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                        .Include(x => x.Opco)
                        .Include(x => x.Designaspectsnetworkfunctions).ThenInclude(x => x.Networkfunction)
                        .Include(x => x.Designaspectssupportedsvr).ThenInclude(x => x.Service)
                        .Include(x => x.Plannedactivities).ThenInclude(p => p.Activitystatus)
                        .Include(x => x.Plannedactivities).ThenInclude(p => p.Planningactivitystatus)
                        .Include(x => x.Plannedactivities).ThenInclude(p => p.Deliverystatus)
                        .Include(x => x.Plannedactivities)
                        .Include(x => x.Authenicationtype)
                        .Include(x => x.Siteresilience)
                        .Include(x => x.Businesscontinuitymethod)
                        .Include(x => x.Instanceresilience)
                        .Include(x => x.Securitymanager)
                        .Include(x => x.Securitytirezone)
                        .Include(x => x.Thirdpartyaccess)
                        .Include(x => x.Swdeliverylifecycle)
                        .Include(x => x.Licensemodel)                        
                        .Include(x => x.ModificationuserNavigation)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Vodafonename)
                       .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                       .ThenInclude(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts)
                       .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                       .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware.Majorhwbuildsdesigncontacts)
                       .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Productname) ;

            var data = result.AsEnumerable().Select(p => DesignAspectMapper.Get(p,true,true)).ToList();
 
            return data.AsQueryable();
        }

        public QueryResultDto<DesignAspectDtoGrid> FindWithCondition(
         DesignAspectQueryDto designAspectQueryDto)
        {
            var predicateResult = ApplyFilter(designAspectQueryDto);
            if (designAspectQueryDto.Deleted == true) predicateResult = predicateResult.And(x => x.Deleted.Value);

            var model = PrepareQuery(predicateResult, designAspectQueryDto.Deleted ?? false);


            var query = model.ApplyOrdering(designAspectQueryDto, GetColumnsMap());

            if (designAspectQueryDto.VerticalName?.Any() == true)
            {
               // query = query.Where(x => x.DesignComponentFamily.DesignComponents.Any());
                foreach (var item in query)
                {
                    var designContactList = item.DesignContactList;// _commonManager.GetDesignContactFromMajorSoftwareAndHardWare(item.DesignComponentFamily.DesignComponents);
                    if (designContactList?.Any() == true)
                        item.VerticalFilterDto = _commonManager.GetVerticaleNameDynamicFormat(designContactList, 0)
                                .Select(x => new FilterValueDtoKeyValueList
                                {
                                    Key = Convert.ToInt16(x?.Value),
                                    Value = x?.Text
                                })?.Distinct().ToList();
                }

                var verticalNames = designAspectQueryDto.VerticalName;

                // null case (when "yes" is included)
                var nullVerticals = query;
                if (verticalNames.Contains("yes"))              
                    nullVerticals = query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));



                var otherVerticals = query.Where(x =>
                    x.VerticalFilterDto != null &&
                    x.VerticalFilterDto.Any(v => verticalNames.Where(t => t != "yes").Contains(v.Key.ToString()))
                );

                // combine both
                if (!(verticalNames.Contains("yes")))
                    query = otherVerticals;
                else if (verticalNames.Contains("yes") && verticalNames.Count > 1)
                    query =  nullVerticals.Union(otherVerticals);
                else query = nullVerticals;

            }
     
            var rtn = new QueryResultDto<DesignAspectDtoGrid>(new GenerateRenderForGrid<DesignAspectDtoGrid>(_manager))
            {
                TotalItems = query.Count()
            };

            query = query.ApplyPaging(designAspectQueryDto);

            if (designAspectQueryDto.VerticalName?.Any() != true)
                foreach (var item in query)
                {
                    var designContactList = item.DesignContactList; //_commonManager.GetDesignContactFromMajorSoftwareAndHardWare(item.DesignComponentFamily.DesignComponents);
                    if (designContactList?.Any() == true)
                        item.VerticalFilterDto = _commonManager.GetVerticaleNameDynamicFormat(designContactList, 0)
                                .Select(x => new FilterValueDtoKeyValueList
                                {
                                    Key = Convert.ToInt16(x?.Value),
                                    Value = x?.Text
                                })?.Distinct().ToList();
                }

            List<DesignAspect> data = query.ToList();

            var designAspectsResult = data.Select(p => BindGrid(p, p.PlannedActivities.ToList())).ToList();
            rtn.Items = designAspectsResult.ToArray();
            return rtn;
        }


        private ExpressionStarter<Designaspects> ApplyFilter(DesignAspectQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Designaspects>(true);
            var archivePredicate = PredicateBuilder.New<Designaspects>(true);
            archivePredicate.Or(x => x.Archived == buildFilterDto.Archived);
            predicateResult.And(archivePredicate);
            var predicateInner = PredicateBuilder.New<Designaspects>();

            if (buildFilterDto.Id != null && buildFilterDto.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Description != null && buildFilterDto.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.Description)
                    predicateInner.Or(x => x.Description.Equals(item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.countrySpecificCriticality != null && buildFilterDto.countrySpecificCriticality.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();

                foreach (var item in buildFilterDto.countrySpecificCriticality)
                    predicateInner.Or(x => x.Criticalnationalinfrastructure == item);
                predicateResult.And(predicateInner);
            }
           
            if (buildFilterDto.DesignComponentFamilyName != null && buildFilterDto.DesignComponentFamilyName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.DesignComponentFamilyName)
                {
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponentfamilyid == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VodafoneName != null && buildFilterDto.VodafoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.VodafoneName)
                {
                    predicateInner.Or(x => x.Designcomponentfamily.Subnetworkboundary.Vodafonename != null && x.Designcomponentfamily.Subnetworkboundary.Vodafonename.Id == item);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignComponentFamilyId != null && buildFilterDto.DesignComponentFamilyId.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.DesignComponentFamilyId)
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CriticalityRating != null && buildFilterDto.CriticalityRating.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.CriticalityRating)
                    predicateInner.Or(x => ( x.Criticalityrating == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubNetworkBoundary != null && buildFilterDto.SubNetworkBoundary.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.SubNetworkBoundary)
                    predicateInner.Or(x => (string.IsNullOrEmpty(x.Designcomponentfamily.Subnetworkboundary.Alias) ? x.Designcomponentfamily.Subnetworkboundary.Description : x.Designcomponentfamily.Subnetworkboundary.Alias) == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.UsedNetworkFunctions != null && buildFilterDto.UsedNetworkFunctions.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.UsedNetworkFunctions)
                    predicateInner.Or(x => x.Designaspectsnetworkfunctions.Any(d => d.Networkfunctionid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SupportedServices != null && buildFilterDto.SupportedServices.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.SupportedServices)
                    predicateInner.Or(x => x.Designaspectssupportedsvr.Any(d => d.Serviceid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AuthenicationTypeName != null && buildFilterDto.AuthenicationTypeName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.AuthenicationTypeName)
                    predicateInner.Or(x => x.Authenicationtypeid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SiteResilienceName != null && buildFilterDto.SiteResilienceName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.SiteResilienceName)
                    predicateInner.Or(x => x.Siteresilienceid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LicenseModelName != null && buildFilterDto.LicenseModelName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.LicenseModelName)
                    predicateInner.Or(x => x.Licensemodelid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SwDeliveryLifeCycleName != null && buildFilterDto.SwDeliveryLifeCycleName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.SwDeliveryLifeCycleName)
                    predicateInner.Or(x => x.Swdeliverylifecycleid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BusinessContinuityMethodName != null && buildFilterDto.BusinessContinuityMethodName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.BusinessContinuityMethodName)
                    predicateInner.Or(x => x.Businesscontinuitymethodid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.InstanceResilienceName != null && buildFilterDto.InstanceResilienceName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.InstanceResilienceName)
                    predicateInner.Or(x => x.Instanceresilienceid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SecurityTireZoneName != null && buildFilterDto.SecurityTireZoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.SecurityTireZoneName)
                    predicateInner.Or(x => x.Securitytirezoneid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SecurityManagerName != null && buildFilterDto.SecurityManagerName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.SecurityManagerName)
                    predicateInner.Or(x => x.Securitymanagerid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ThirdPartyAccessName != null && buildFilterDto.ThirdPartyAccessName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.ThirdPartyAccessName)
                    predicateInner.Or(x => x.Thirdpartyaccessid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpCoName != null && buildFilterDto.OpCoName.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.OpCoName)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedActivity != null && buildFilterDto.PlannedActivity.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.PlannedActivity)
                    predicateInner.Or(x => x.Plannedactivities.Any(s => s.Plannedactivityid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NominalCapacityLimit != null && buildFilterDto.NominalCapacityLimit.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.NominalCapacityLimit)
                    predicateInner.Or(x => x.Nominalcapacitylimit == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MaxAllowedLoading != null && buildFilterDto.MaxAllowedLoading.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.MaxAllowedLoading)
                    predicateInner.Or(x => x.Maxallowedloading == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignedCapacityLimit != null && buildFilterDto.DesignedCapacityLimit.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.DesignedCapacityLimit)
                    predicateInner.Or(x => x.Designedcapacitylimit == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OpCoId != null && buildFilterDto.OpCoId.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.OpCoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlatformSoftware != null && buildFilterDto.PlatformSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.PlatformSoftware)
                {
                    predicateInner.Or(x => x.Designcomponentfamily.Productname!= null && x.Designcomponentfamily.Productname.Isplatformsoftware.ToString() == item);
                }
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }


        private Dictionary<string, Expression<Func<DesignAspect, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<DesignAspect, object>>[]>
            {
                ["id"] = new Expression<Func<DesignAspect, object>>[] { p => p.Id },

                ["designComponentFamilyName"] = new Expression<Func<DesignAspect, object>>[]
                    {p => p.DesignComponentFamily.DCFName(_repositoryWrapper)},
                ["designComponentFamilyId"] = new Expression<Func<DesignAspect, object>>[]
                    {p => p.DesignComponentFamilyId},

                ["authenicationTypeName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.AuthenicationType != null ? p.AuthenicationType.Description : default,
                },
                ["siteResilienceName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.SiteResilience != null ? p.SiteResilience.Description : default
                },
                ["opCoName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.OpCo.OpCoDescription
                },
                ["archived"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.Archived
                },
                ["businessContinuityMethodName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.BusinessContinuityMethod != null ? p.BusinessContinuityMethod.Description : default,
                },
                ["instanceResilienceName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.InstanceResilience != null ? p.InstanceResilience.Description : default
                },
                ["securityManagerName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.SecurityManager != null ? p.SecurityManager.Description : default,
                },
                ["criticalNationalInfrastructureName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.CriticalNationalInfrastructure,
                },
                ["licenseModelName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.LicenseModel != null ? p.LicenseModel.Description : default
                },
                ["thirdPartyAccessName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.ThirdPartyAccessType != null ? p.ThirdPartyAccessType.Description : default
                },
                ["swDeliveryLifeCycleName"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.SWDeliveryLifeCycle != null ? p.SWDeliveryLifeCycle.Description : default
                },
                ["supportedServices"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.DesignAspectSupportedServices.FirstOrDefault() != null ?
                            p.DesignAspectSupportedServices.FirstOrDefault().SupportedService.Description : null
                },
                ["subNetworkBoundary"] = new Expression<Func<DesignAspect, object>>[] { p => string.IsNullOrEmpty(p.DesignComponentFamily.SubNetworkBoundary.Alias) ? p.DesignComponentFamily.SubNetworkBoundary.Description : p.DesignComponentFamily.SubNetworkBoundary.Alias },
                ["securityTireZoneName"] = new Expression<Func<DesignAspect, object>>[] { p => p.SecurityTireZone != null ? p.SecurityTireZone.SecurityTireZoneDescription : default },
                ["plannedActivity"] = new Expression<Func<DesignAspect, object>>[] { p => p.PlannedActivities.Where(x => !x.Deleted && x.Archived != true).FirstOrDefault() != null ? p.PlannedActivities.Where(x => !x.Deleted && x.Archived != true).FirstOrDefault().PlannedImplementationYear : default,
                                                                            p => p.PlannedActivities.Where(x => !x.Deleted && x.Archived != true).FirstOrDefault() != null ? p.PlannedActivities.Where(x => !x.Deleted && x.Archived != true).FirstOrDefault().ActivityStatus.ActivityStatusDescription : default,
                                                                            p => p.PlannedActivities.Where(x => !x.Deleted && x.Archived != true).FirstOrDefault() != null ? p.PlannedActivities.Where(x => !x.Deleted && x.Archived != true).FirstOrDefault().PlanningActivityStatus.PlanningActivityStatusDescription : default, },
                ["lastModifiedValue"] = new Expression<Func<DesignAspect, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<DesignAspect, object>>[]
                    {p => p.ModificationUserEntity.Email},
                ["usedNetworkFunctions"] = new Expression<Func<DesignAspect, object>>[] { p => p.DesignAspectNetworkFunctions.FirstOrDefault() != null ? p.DesignAspectNetworkFunctions.FirstOrDefault().NetworkFunction.Description : default },
                ["vodafoneName"] = new Expression<Func<DesignAspect, object>>[] { p => p.DesignComponentFamily != null  && p.DesignComponentFamily.SubNetworkBoundary != null && p.DesignComponentFamily.SubNetworkBoundary.VodafoneName != null ?
                                                                                p.DesignComponentFamily.SubNetworkBoundary.VodafoneName.Description : default },
                ["vodafoneName"] = new Expression<Func<DesignAspect, object>>[] { p => p.DesignComponentFamily != null  && p.DesignComponentFamily.SubNetworkBoundary != null && p.DesignComponentFamily.SubNetworkBoundary.VodafoneName != null ?
                                                                                p.DesignComponentFamily.SubNetworkBoundary.VodafoneName.Description : default },
                ["description"] = new Expression<Func<DesignAspect, object>>[] { p => p.Description },
                ["nominalCapacityLimit"] = new Expression<Func<DesignAspect, object>>[] { p => p.NominalCapacityLimit },
                ["maxAllowedLoading"] = new Expression<Func<DesignAspect, object>>[] { p => p.MaxAllowedLoading },
                ["designedCapacityLimit"] = new Expression<Func<DesignAspect, object>>[] { p => p.DesignedCapacityLimit },
                ["opCoId"] = new Expression<Func<DesignAspect, object>>[]
                {
                    p => p.OpCo.OpCoId
                }
            };
        }

        private async Task<List<Plannedactivities>> UpdatePlannedActivity(DesignAspectDtoModel model)
        {
            var plannedactivities = new List<Plannedactivities>();
            if (model.PlannedActivityDto != null && model.PlannedActivityDto.Count > 0)
            {
                var getDummyBag = _commonManager.GetDummyBag();               

                try
                {                    
                    foreach (var item in model.PlannedActivityDto)
                    {
                        short? FetchNopAResourcesId = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x =>
                              x.Plannedactivityresourceid == item.PlannedActivityResourceId
                             && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity_For_DA).FirstOrDefault()?.Plannedactivityresourceid;

                        if (FetchNopAResourcesId is not null and not 0)
                        {
                            isNoDaPa = true;

                        }

                        var plannedActivity = PlannedActivityMapper.Set(BindPlannedActivity(item, model.Id));

                        if (isNoDaPa)
                        {
                            Dictionary<short, string> activitystatusid = await _dropdownDataServiceManager.GetActivityStatusDic(false, ConstantValueFilter.paActivityStatus);
                            Dictionary<short, string> planningactivitystatusid = await _dropdownDataServiceManager.GetPlanningActivityStatusDic(false, ConstantValueFilter.paPlanningActivityStatus);
                            plannedActivity.Activitystatusid = activitystatusid.Select(x => x.Key).FirstOrDefault();
                            plannedActivity.Planningactivitystatusid = planningactivitystatusid.Select(x => x.Key).FirstOrDefault();
                            plannedActivity.Projectstatus = Outputs.NotRequested;
                        }

                        plannedActivity.Designcomponentid = plannedActivity.Designcomponentid == 0 ? null : plannedActivity.Designcomponentid;
                        plannedActivity.Deliverystatusid =
                            plannedActivity.Deliverystatusid == 0 ? null : plannedActivity.Deliverystatusid;
                        plannedActivity.Responsibilityphaseid =
                            plannedActivity.Responsibilityphaseid == 0 ? null : plannedActivity.Responsibilityphaseid;
                        plannedActivity.Deliverystatus = plannedActivity.Deliverystatusid == 0 ? null : plannedActivity.Deliverystatus;
                        plannedActivity.Designaspectid = model.Id;
                        plannedActivity.Opcoid = model.OpCoId;
                        plannedActivity.Archived = false;
                        plannedActivity.Plannedactivitycategoryid =
                                    plannedActivity.Plannedactivitycategoryid == 0 ? null : plannedActivity.Plannedactivitycategoryid;
                        plannedActivity.Programid =
                                    plannedActivity.Programid == 0 ? null : plannedActivity.Programid;
                        plannedActivity.Buildbagid = getDummyBag.Buildbagid;
                        plannedActivity.Designcomponentfamilyid = item.PlannedDesignComponentFamilyId;//exodus
                      
                        if (plannedActivity.Plannedactivityid == 0)
                        {
                            _repositoryWrapper.PlannedActivity.Create(plannedActivity);
                        }
                        else
                        {
                            _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                        }
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        //multipleDcf                      
                        if (item.DesignComponentFamilyIdList != null && item.DesignComponentFamilyIdList.Count > 0)
                        await AddOrUpdateMultipleDcf(item.DesignComponentFamilyIdList, plannedActivity.Plannedactivityid);

                        #region exodus
                        var settingRuleElementCount = _repositoryWrapper.SettingsUpdatePlannedActivity
                            .FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.DesignAspect
                            && x.Plannedactivityresourceid == plannedActivity.Plannedactivityresourceid && x.Deliverystatusid == plannedActivity.Deliverystatusid)
                            ?.FirstOrDefault();

                        var exodusPaTypeList = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid
                        == plannedActivity.Plannedactivityresourceid
                        && (x.Rulelinkeddc == (int)PlannedActivityResourceEnum.Infra_Readiness
                        || x.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration)).Select(y => y.Rulelinkeddc).ToList();

                        if (exodusPaTypeList != null && exodusPaTypeList.Count > 0)
                        {
                            if (exodusPaTypeList.Any(x => x == (int)PlannedActivityResourceEnum.Infra_Readiness))
                            {
                                var isProjectPlanDeliveryStatus = _repositoryWrapper.SettingsUpdatePlannedActivity.
                                    FindByCondition(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Infra_Readiness)
                                    .Select(x => x.Order).Min();
                                if (isProjectPlanDeliveryStatus == settingRuleElementCount.Order) await _designAspectPlannedActivityManger.AddDaMigrationEntry(plannedActivity);

                            }
                            else if (exodusPaTypeList.Any(x => x == (int)PlannedActivityResourceEnum.Platform_Migration))
                            {
                                var isProjectPlanDeliveryStatus = _repositoryWrapper.SettingsUpdatePlannedActivity.
                                    FindByCondition(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration)
                                    .Select(x => x.Order).Min();

                                if (item.IsRefactorDC == true)
                                {
                                    await _platformMigrationManager.RefactorDcCalculation(item.PlaftformMigrationDcfResources, plannedActivity.Plannedactivityid);
                                }
                                await _platformMigrationManager.AddOrUpdateAssetMigrationAsync(item.PlaftformMigrationDcfResources, plannedActivity.Plannedactivityid);

                            }
                        }

                        #endregion


                        plannedactivities.Add(plannedActivity);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("DesignAspect - PlannedActivity Add/Edit : " + ex.StackTrace);
                    throw;
                }


            }


            return plannedactivities;
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter,
          DesignAspectQueryDto designAspectQueryDto,bool isAdmin)
        {

            var predicateResult = ApplyFilter(designAspectQueryDto);

            var query = PrepareQuery(predicateResult, false);

            if (designAspectQueryDto.VerticalId != null && designAspectQueryDto.VerticalId.Count > 0)
                query = query.Where(x => x.DesignComponentFamily.DesignComponents.Any());

            foreach (var item in query)
            {
                var designContactList = item.DesignContactList;// _commonManager.GetDesignContactFromMajorSoftwareAndHardWare(item.DesignComponentFamily.DesignComponents);
                if (designContactList?.Any() == true)
                    item.VerticalFilterDto = _commonManager.GetVerticaleNameDynamicFormat(designContactList, 0)
                            .Select(x => new FilterValueDtoKeyValueList
                            {
                                Key = Convert.ToInt16(x?.Value),
                                Value = x?.Text
                            })?.Distinct().ToList();
            }

            if (designAspectQueryDto.VerticalName != null && designAspectQueryDto.VerticalName.Any() == true && !designAspectQueryDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Any(v => designAspectQueryDto.VerticalName.Contains(v.Key.ToString())));
            }
            else if (designAspectQueryDto.VerticalName?.Any() == true && designAspectQueryDto.VerticalName.Count() == 1 && designAspectQueryDto.VerticalName.Contains("yes"))
            {
                query = query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));
            }
            else if (designAspectQueryDto.VerticalName?.Any() == true && designAspectQueryDto.VerticalName.Count() > 1 && designAspectQueryDto.VerticalName.Contains("yes"))
            {
                var nullVerticals = designAspectQueryDto.VerticalName.Contains("yes") ?
                    query.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0)) : null;
                var verticalFilter = query
                                    .Where(x => x.VerticalFilterDto != null &&
                                    x.VerticalFilterDto.Any(c => designAspectQueryDto.VerticalName.Where(t => t != "yes").Contains(c.Key.ToString())));
                query = nullVerticals?.Any() == true && verticalFilter?.Any() == true ?
                    nullVerticals.Concat(verticalFilter) : nullVerticals?.Any() == true && verticalFilter
                    ?.Any() == false ? nullVerticals
                    : nullVerticals?.Any() == false && verticalFilter?.Any() == true ? verticalFilter : null;
            }

            var rtn = propertyName switch
            {
                "id" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   {
                       Text = p.Id.ToString(),
                       Value = p.Id.ToString()
                   }).Distinct().ToList()
                   : query.Where(x => x.Id.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto
                               {
                                   Text = p.Id.ToString(),
                                   Value = p.Id.ToString()
                               }).Distinct().ToList(),
                "description" => string.IsNullOrEmpty(propertyFilter)
                              ? query.Select(p => new FilterValueDto
                              {
                                  Text = p.Description,
                                  Value = p.Description
                              }).Distinct().ToList()
                              : query.Where(x => x.Description.Contains(
                                          propertyFilter)).Select(p => new FilterValueDto
                                          {
                                              Text = p.Description,
                                              Value = p.Description
                                          }).Distinct().ToList(),
                "designComponentFamilyName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.DesignComponentFamily.DCFName(_repositoryWrapper),
                        Value = p.DesignComponentFamilyId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.DesignComponentFamily.DCFName(_repositoryWrapper).Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.DesignComponentFamily.DCFName(_repositoryWrapper),
                                    Value = p.DesignComponentFamilyId.ToString()
                                }).Distinct().ToList(),
                "vodafoneName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.DesignComponentFamily.SubNetworkBoundary.VodafoneName != null ? p.DesignComponentFamily.SubNetworkBoundary.VodafoneName.Description : "",
                        Value = p.DesignComponentFamily.SubNetworkBoundary.VodafoneName != null ? p.DesignComponentFamily.SubNetworkBoundary.VodafoneName.Id.ToString() : ""
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                         x.DesignComponentFamily.SubNetworkBoundary.VodafoneName.Description.Contains(
                         propertyFilter)).Select(p => new FilterValueDto
                         {
                             Text = p.DesignComponentFamily.SubNetworkBoundary.VodafoneName != null ? p.DesignComponentFamily.SubNetworkBoundary.VodafoneName.Description : "",
                             Value = p.DesignComponentFamily.SubNetworkBoundary.VodafoneName != null ? p.DesignComponentFamily.SubNetworkBoundary.VodafoneName.Id.ToString() : ""
                         }).Distinct().ToList(),

                "designComponentFamilyId" => string.IsNullOrEmpty(propertyFilter)
                           ? query.Select(p => new FilterValueDto
                           {
                               Text = p.DesignComponentFamily.DesignComponentFamilyId.ToString(),
                               Value = p.DesignComponentFamilyId.ToString()
                           }).Distinct().ToList()
                           : query
                               .Where(x =>
                                   x.DesignComponentFamily.DesignComponentFamilyId.ToString().Contains(
                                       propertyFilter)).Select(p => new FilterValueDto
                                       {
                                           Text = p.DesignComponentFamily.DesignComponentFamilyId.ToString(),
                                           Value = p.DesignComponentFamilyId.ToString()
                                       }).Distinct().ToList(),

                "opCoName" => string.IsNullOrEmpty(propertyFilter)
                        ? query.Select(p => new FilterValueDto
                        {
                            Text = p.OpCo.OpCoDescription.ToString(),
                            Value = p.OpCoId.ToString()
                        }).Distinct().ToList()
                        : query
                            .Where(x =>
                                x.OpCoId.ToString().Contains(
                                    propertyFilter)).Select(p => new FilterValueDto
                                    {
                                        Text = p.OpCo.OpCoDescription.ToString(),
                                        Value = p.OpCoId.ToString()
                                    }).Distinct().ToList(),

                "subNetworkBoundary" => string.IsNullOrEmpty(propertyFilter) ? query.Select(x => new FilterValueDto(string.IsNullOrEmpty(x.DesignComponentFamily.SubNetworkBoundary.Alias) ? x.DesignComponentFamily.SubNetworkBoundary.Description : x.DesignComponentFamily.SubNetworkBoundary.Alias)).Distinct().ToList()
                   : query.Where(x => string.IsNullOrEmpty(x.DesignComponentFamily.SubNetworkBoundary.Alias) ? x.DesignComponentFamily.SubNetworkBoundary.Description.Contains(propertyFilter) : x.DesignComponentFamily.SubNetworkBoundary.Alias.Contains(propertyFilter))
                   .Select(x => new FilterValueDto(string.IsNullOrEmpty(x.DesignComponentFamily.SubNetworkBoundary.Alias) ? x.DesignComponentFamily.SubNetworkBoundary.Description : x.DesignComponentFamily.SubNetworkBoundary.Alias)).Distinct().ToList(),

                "supportedServices" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(p => p.DesignAspectSupportedServices != null && p.DesignAspectSupportedServices.Count > 0).SelectMany(x => x.DesignAspectSupportedServices).Select(p => new FilterValueDto(p.ServiceId.ToString(), p.SupportedService.Description)).Distinct().ToList()
                    : query
                    .Where(x => x.DesignAspectSupportedServices != null && x.DesignAspectSupportedServices.Any(s => s.SupportedService.Description.ToUpper().Contains(propertyFilter.ToUpper())))
                    .SelectMany(x => x.DesignAspectSupportedServices)
                    .Select(p => new FilterValueDto(p.ServiceId.ToString(), p.SupportedService.Description)).Distinct()
                    .ToList(),

                "usedNetworkFunctions" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(p => p.DesignAspectNetworkFunctions != null).SelectMany(x => x.DesignAspectNetworkFunctions).Select(p => new FilterValueDto(p.NetworkFunctionId, p.NetworkFunction.Description)).Distinct().ToList()
                    : query
                        .Where(x => x.DesignAspectNetworkFunctions != null && x.DesignAspectNetworkFunctions.Any(s => s.NetworkFunction.Description.ToUpper().Contains(propertyFilter.ToUpper())))
                        .SelectMany(x => x.DesignAspectNetworkFunctions)
                        .Select(p => new FilterValueDto(p.NetworkFunctionId, p.NetworkFunction.Description)).Distinct()
                        .ToList(),

                "authenicationTypeName" => string.IsNullOrEmpty(propertyFilter)
                                ? query.Where(p => p.AuthenicationTypeId.HasValue).Select(p => new FilterValueDto
                                {
                                    Text = p.AuthenicationType.Description,
                                    Value = p.AuthenicationTypeId.ToString()
                                }).Distinct().ToList()
                            : query
                                .Where(x => x.AuthenicationTypeId.HasValue &&
                                    x.AuthenicationType.Description.Contains(
                                        propertyFilter)).Select(p => new FilterValueDto
                                        {
                                            Text = p.AuthenicationType.Description,
                                            Value = p.AuthenicationTypeId.ToString()
                                        }).Distinct().ToList(),
                "securityManagerName" => string.IsNullOrEmpty(propertyFilter)
                            ? query.Where(p => p.SecurityManagerId.HasValue).Select(p => new FilterValueDto
                            {
                                Text = p.SecurityManager.Description,
                                Value = p.SecurityManagerId.ToString()
                            }).Distinct().ToList()
                            : query
                            .Where(x => x.SecurityManagerId.HasValue && x.SecurityManager.Description.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.SecurityManager.Description,
                                    Value = p.SecurityManagerId.ToString()
                                }).Distinct().ToList(),
                "thirdPartyAccessName" => string.IsNullOrEmpty(propertyFilter)
                             ? query.Where(x => x.ThirdPartyAccessId.HasValue).Select(p => new FilterValueDto
                             {
                                 Text = p.ThirdPartyAccessType.Description,
                                 Value = p.ThirdPartyAccessId.ToString()
                             }).Distinct().ToList()
                             : query
                             .Where(x => x.ThirdPartyAccessId.HasValue && x.ThirdPartyAccessType.Description.Contains(
                                 propertyFilter)).Select(p => new FilterValueDto
                                 {
                                     Text = p.ThirdPartyAccessType.Description,
                                     Value = p.ThirdPartyAccessId.ToString()
                                 }).Distinct().ToList(),
                "siteResilienceName" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Where(x => x.SiteResilienceId.HasValue).Select(p => new FilterValueDto
                     {
                         Text = p.SiteResilience.Description,
                         Value = p.SiteResilienceId.ToString()
                     }).Distinct().ToList()
                     : query
                     .Where(x => x.SiteResilienceId.HasValue && x.SiteResilience.Description.Contains(
                         propertyFilter)).Select(p => new FilterValueDto
                         {
                             Text = p.SiteResilience.Description,
                             Value = p.SiteResilienceId.ToString()
                         }).Distinct().ToList(),
                "swDeliveryLifeCycleName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Where(x => x.SWDeliveryLifeCycleId.HasValue).Select(p => new FilterValueDto
                       {
                           Text = p.SWDeliveryLifeCycle.Description,
                           Value = p.SWDeliveryLifeCycleId.ToString()
                       }).Distinct().ToList()
                       : query
                       .Where(x => x.SWDeliveryLifeCycleId.HasValue && x.SWDeliveryLifeCycle.Description.Contains(
                           propertyFilter)).Select(p => new FilterValueDto
                           {
                               Text = p.SWDeliveryLifeCycle.Description,
                               Value = p.SWDeliveryLifeCycleId.ToString()
                           }).Distinct().ToList(),

                "instanceResilienceName" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(x => x.InstanceResilienceId.HasValue).Select(p => new FilterValueDto
                   {
                       Text = p.InstanceResilience.Description,
                       Value = p.InstanceResilienceId.ToString()
                   }).Distinct().ToList()
                   : query
                   .Where(x => x.InstanceResilienceId.HasValue && x.InstanceResilience.Description.Contains(
                       propertyFilter)).Select(p => new FilterValueDto
                       {
                           Text = p.InstanceResilience.Description,
                           Value = p.InstanceResilienceId.ToString()
                       }).Distinct().ToList(),
                "businessContinuityMethodName" => string.IsNullOrEmpty(propertyFilter)
                 ? query.Where(x => x.BusinessContinuityMethodId.HasValue).Select(p => new FilterValueDto
                 {
                     Text = p.BusinessContinuityMethod.Description,
                     Value = p.BusinessContinuityMethodId.ToString()
                 }).Distinct().ToList()
                 : query
                 .Where(x => x.BusinessContinuityMethodId.HasValue && x.BusinessContinuityMethod.Description.Contains(
                     propertyFilter)).Select(p => new FilterValueDto
                     {
                         Text = p.BusinessContinuityMethod.Description,
                         Value = p.BusinessContinuityMethodId.ToString()
                     }).Distinct().ToList(),
                "licenseModelName" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(x => x.LicenseModelId.HasValue).Select(p => new FilterValueDto
                   {
                       Text = p.LicenseModel.Description,
                       Value = p.LicenseModelId.ToString()
                   }).Distinct().ToList()
                   : query
                   .Where(x => x.LicenseModelId.HasValue && x.LicenseModel.Description.Contains(
                       propertyFilter)).Select(p => new FilterValueDto
                       {
                           Text = p.LicenseModel.Description,
                           Value = p.LicenseModelId.ToString()
                       }).Distinct().ToList(),
                "securityTireZoneName" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(x => x.SecurityTireZoneId.HasValue).Select(p => new FilterValueDto
                   {
                       Text = p.SecurityTireZone.SecurityTireZoneDescription,
                       Value = p.SecurityTireZoneId.ToString()
                   }).Distinct().ToList()
                   : query
                   .Where(x => x.SecurityTireZoneId.HasValue && x.SecurityTireZone.SecurityTireZoneDescription.Contains(
                       propertyFilter)).Select(p => new FilterValueDto
                       {
                           Text = p.SecurityTireZone.SecurityTireZoneDescription,
                           Value = p.SecurityTireZoneId.ToString()
                       }).Distinct().ToList(),
                "criticalNationalInfrastructureName" =>
                   string.IsNullOrEmpty(propertyFilter)
                   ? query
                       .Select(p => new FilterValueDto { Text = p.CriticalNationalInfrastructure ? ConstantValueFilter.YES : ConstantValueFilter.NO, Value = p.CriticalNationalInfrastructure.ToString() }).Distinct().ToList()
                   : query
                       .Where(p => (p.CriticalNationalInfrastructure ? ConstantValueFilter.YES : ConstantValueFilter.NO).Contains(propertyFilter))
                       .Select(p => new FilterValueDto { Text = p.CriticalNationalInfrastructure ? ConstantValueFilter.YES : ConstantValueFilter.NO, Value = p.CriticalNationalInfrastructure.ToString() }).Distinct().ToList(),
                "plannedActivity" => string.IsNullOrEmpty(propertyFilter)
                     ? query.ToList().SelectMany(q => q.PlannedActivities.Select(x => new FilterValueDto
                     {
                         Value = x.PlannedActivityId.ToString(),
                         Text = x.PlannedImplementationYear.ToString() + " | " + " | " + x.ActivityStatus.ActivityStatusDescription + " | " + x.PlanningActivityStatus.PlanningActivityStatusDescription
                     }).ToList()).Distinct().ToList()
                     : query.ToList()
                         .Where(x =>
                             x.PlannedActivities.Any(s => s.PlannedImplementationYear.ToString().Contains(propertyFilter) || s.ActivityStatus.ActivityStatusDescription.Contains(propertyFilter) || s.PlanningActivityStatus.PlanningActivityStatusDescription.Contains(propertyFilter))).ToList()
                         .SelectMany(q => q.PlannedActivities.Select(x => new FilterValueDto
                         {
                             Value = x.PlannedActivityId.ToString(),
                             Text = x.PlannedImplementationYear.ToString() + " | " + x.ActivityStatus.ActivityStatusDescription + " | " + x.PlanningActivityStatus.PlanningActivityStatusDescription
                         }).ToList()).Distinct().ToList(),


                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

                "archived" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.Archived != null)
                    .Select(p => new FilterValueDto { Text = p.Archived == false ? ConstantValueFilter.NO : ConstantValueFilter.YES, Value = p.Archived.ToString() }).Distinct().ToList()
                    : query
                    .Where(p => p.Archived != null && (p.Archived.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO).Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Archived == false ? ConstantValueFilter.NO : ConstantValueFilter.YES, Value = p.Archived.ToString() }).Distinct()
                    .ToList(),
                "nominalCapacityLimit" => string.IsNullOrEmpty(propertyFilter)
                        ? query.Where(x => x.NominalCapacityLimit != null).Select(p => new FilterValueDto
                        {
                            Text = p.NominalCapacityLimit,
                            Value = p.NominalCapacityLimit
                        }).Distinct().ToList()
                        : query.Where(x => x.NominalCapacityLimit != null && x.NominalCapacityLimit.Contains(
                           propertyFilter)).Select(p => new FilterValueDto
                           {
                               Text = p.NominalCapacityLimit,
                               Value = p.NominalCapacityLimit
                           }).Distinct().ToList(),
                "maxAllowedLoading" => string.IsNullOrEmpty(propertyFilter)
                ? query.Where(x => x.MaxAllowedLoading != null).Select(p => new FilterValueDto
                {
                    Text = p.MaxAllowedLoading,
                    Value = p.MaxAllowedLoading
                }).Distinct().ToList()
                : query.Where(x => x.MaxAllowedLoading != null && x.MaxAllowedLoading.Contains(
                  propertyFilter)).Select(p => new FilterValueDto
                  {
                      Text = p.MaxAllowedLoading,
                      Value = p.MaxAllowedLoading
                  }).Distinct().ToList(),
                "designedCapacityLimit" => string.IsNullOrEmpty(propertyFilter)
                        ? query.Where(x => x.DesignedCapacityLimit != null).Select(p => new FilterValueDto
                        {
                            Text = p.DesignedCapacityLimit,
                            Value = p.DesignedCapacityLimit
                        }).Distinct().ToList()
                        : query.Where(x => x.DesignedCapacityLimit != null && x.DesignedCapacityLimit.Contains(
                           propertyFilter)).Select(p => new FilterValueDto
                           {
                               Text = p.DesignedCapacityLimit,
                               Value = p.DesignedCapacityLimit
                           }).Distinct().ToList(),
                "countrySpecificCriticality" => string.IsNullOrEmpty(propertyFilter)
                        ? query.Select(p => new FilterValueDto
                         ( 
                            p.CriticalNationalInfrastructure
                        )).Distinct().ToList()
                        : query .Select(p => new FilterValueDto
                           (
                            p.CriticalNationalInfrastructure
                        )).Distinct().ToList(),
                "criticalityRating" => string.IsNullOrEmpty(propertyFilter)
                        ? query.Where(x => x.CriticalityRating != null).Select(p => new FilterValueDto
                         (
                            p.CriticalityRating
                        )).Distinct().ToList()
                        : query.Where(x => x.CriticalityRating == Convert.ToInt16( propertyFilter)).Select(p => new FilterValueDto
                           (
                            p.CriticalityRating
                        )).Distinct().ToList(),

                "verticalName" => query.ToList().Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                         .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                         { Text = t.Value, Value = t.Key.ToString() }))?.Distinct()?.ToList()
                          .Concat(query.ToList().Where(x =>  (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0))
                            .Select(x => new FilterValueDto
                            {
                                Text = "---",
                                Value = "yes",
                            })).Distinct().ToList(),

                "platformSoftware" =>
                                   string.IsNullOrEmpty(propertyFilter)
                                   ? query
                                       .Select(p => new FilterValueDto { Text = p.DesignComponentFamily.ProductNameNavigation.IsPlatformSoftware.Value? ConstantValueFilter.YES : ConstantValueFilter.NO, Value = p.DesignComponentFamily.ProductNameNavigation.IsPlatformSoftware.ToString() }).Distinct().ToList()
                                   : query
                                       .Where(p => (p.DesignComponentFamily.ProductNameNavigation.IsPlatformSoftware.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO).Contains(propertyFilter))
                                       .Select(p => new FilterValueDto { Text = p.DesignComponentFamily.ProductNameNavigation.IsPlatformSoftware.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                           Value = p.DesignComponentFamily.ProductNameNavigation.IsPlatformSoftware.ToString()
                                       }).Distinct().ToList(),

                _ => new List<FilterValueDto>(),


            } ;

            //if ((_opcoList != null) && (!string.IsNullOrEmpty(propertyName)))
            //    rtn = ("opCoName" == propertyName.ToString()) ?
            //       (rtn.Where(x => _opcoList.Contains(Convert.ToInt16(x.Value.ToString())))).ToList<FilterValueDto>()
            //       : rtn;

            if (!isAdmin && (designAspectQueryDto.VerticalName != null && designAspectQueryDto.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                rtn = rtn.Where(x => designAspectQueryDto.VerticalName.Contains(x.Value.ToString())).ToList();
            }

            return rtn;
        }

        public async Task<ResultDto> GetRelatedRecords(int id)
        {
            string[] plannedActivityLinked = new string[0];
            string[] plannedActivity = new string[0];
            //da completare mancano altre tabelle
            var plannedActivityLinkedIds = _repositoryWrapper.PlannedActivity
                        .FindByCondition(x => x.Designaspectid == id && x.Linkedtoplannedactivityid != null)
                        .Select(x => x.Linkedtoplannedactivityid);

            if (plannedActivityLinkedIds.Count() > 0)
            {
                plannedActivityLinked = _repositoryWrapper.PlannedActivity
                        .FindByCondition(x => plannedActivityLinkedIds.Contains(x.Plannedactivityid))
                        .Include(x => x.Plannedactivityresource)
                        .Include(x => x.Activitystatus)
                        .Include(x => x.Deliverystatus)
                        .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                        .ToArray();
            }
            plannedActivity = _repositoryWrapper.PlannedActivity
                        .FindByCondition(x => x.Designaspectid == id && x.Linkedtoplannedactivityid == null && x.Archived != true)
                        .Include(x => x.Plannedactivityresource)
                        .Include(x => x.Activitystatus)
                        .Include(x => x.Deliverystatus)
                        .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                        .ToArray();


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (plannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = ConstantValueFilter.PlannedActivity, Values = plannedActivity });
            if (plannedActivityLinked.Length > 0)
                rm.Add(new ResultMessageDto() { Table = ConstantValueFilter.LinkedPlannedActivity, Values = plannedActivityLinked });

            var entity = await _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Id == id)
             .Include(x => x.Opco).Include(p => p.Designcomponentfamily).FirstOrDefaultAsync();
            #region Active LCM Check
            if (entity != null)
            {
                var dcList = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == entity.Designcomponentfamilyid)
                                .Select(x => x.Designcomponentid).ToList();

                var dcfAccosiatedLcmList = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Opcoid == entity.Opcoid
                && dcList.Contains(x.Designcomponentid) && x.Archived != true).Select(x => x.Lcmengineeringid.ToString()).Distinct().ToArray();
                if (dcfAccosiatedLcmList.Any())
                {
                    rm.Add(new ResultMessageDto()
                    {
                        Table = ConstantValueFilter.lcmEngineering,
                        Values = dcfAccosiatedLcmList
                    });

                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryDeleteHasActiveLCms,
                        Data = new RelatedRecordsResultDto()
                        {
                            EntityName = ConstantValueFilter.DesignAspect,
                            RecordName = " has related LCM entities ",
                            DataRelatedList = rm
                        }
                    };

                }
            }

            #endregion
            if (plannedActivityLinked.Length > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = ConstantValueFilter.DesignAspect,
                        RecordName = entity.Opco.Opco + " - " + entity.Designcomponentfamily.DCFName(_repositoryWrapper),
                        DataRelatedList = rm
                    }
                };
            }
            else if (plannedActivity.Length > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteHasPlannedActivities,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = ConstantValueFilter.DesignAspect,
                        RecordName = entity.Opco.Opco + " - " + entity.Designcomponentfamily.DCFName(_repositoryWrapper),
                        DataRelatedList = rm
                    }
                };
            }
            else
            {
                return new ResultDto();
            }

        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Id == id).SingleAsync();

            foreach (var toDelete in entity.Plannedactivities)
                _repositoryWrapper.PlannedActivity.Delete(toDelete);

            _repositoryWrapper.DesignAspectRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(long id, bool onlyPlannedActivities)
        {
            Designaspects entity = new Designaspects();
            entity = await _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Id == id)
                .Include(x => x.Designaspectsnetworkfunctions)
                .Include(x => x.Designaspectssupportedsvr)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Budgetprojecttrackers)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Projectsplan).ThenInclude(x => x.Projectplanaudit)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Daassetmigration)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Damigrationstatus)
                .SingleAsync();

            var dcList = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == entity.Designcomponentfamilyid).Select(x => x.Designcomponentid).ToList();
            var dcfAccosiatedLcmList = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Opcoid == entity.Opcoid && dcList.Contains(x.Designcomponentid) && x.Archived != true).ToList();
            if (dcfAccosiatedLcmList.Any())
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteHasActiveLCms,
                    Data = entity.Id,
                    Warning = true
                   
                };
            }


            if (entity.Plannedactivities != null && entity.Plannedactivities.Count > 0)
            {
                //Ticket 897
                var plannedActivityToDelete = entity.Plannedactivities.ToList();
                foreach (var toDelete in plannedActivityToDelete)
                {
                    var deliveryTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == toDelete.Plannedactivityid).FirstOrDefault();
                    if (deliveryTracking != null)
                    {
                        _repositoryWrapper.DeliveryTrackingRepository.DeleteDeep(deliveryTracking);
                    }
                    await _commonManager.GenerateAuditLogEntryForPAHardDeleteEntity(toDelete);
                    if (toDelete.Budgetprojecttrackers != null && toDelete.Budgetprojecttrackers.Count > 0)
                    {
                        if (toDelete.Budgetprojecttrackers.FirstOrDefault() != null)
                        {
                            _repositoryWrapper.BudgetProjectTrackersRepository.DeleteDeep(toDelete.Budgetprojecttrackers.FirstOrDefault());
                        }

                    }
                    foreach (var plan in toDelete.Projectsplan)
                    {
                        foreach (var deletePpa in plan.Projectplanaudit)
                        {
                            _repositoryWrapper.ProjectPlanAuditRepository.DeleteDeep(deletePpa);
                        }
                        _repositoryWrapper.ProjectPlanRepository.DeleteDeep(plan);
                    }

                    if (toDelete.Damigrationstatus != null && toDelete.Damigrationstatus.Count > 0)
                    {
                        foreach (var deletePpa in toDelete.Damigrationstatus)
                        {
                            _repositoryWrapper.DaMigrationStatusRepository.DeleteDeep(deletePpa);
                        }

                    }

                    if (toDelete.Daassetmigration != null && toDelete.Daassetmigration.Count > 0)
                    {
                      await  _platformMigrationManager.DeletePltformMigrationPaNewAsset(toDelete.Plannedactivityid);
                        foreach (var deletePpa in toDelete.Daassetmigration)
                        {
                            _repositoryWrapper.DaAssetMigrationRepository.DeleteDeep(deletePpa);
                        }

                    }
                    _repositoryWrapper.PlannedActivity.DeleteDeep(toDelete);
                }
                await _repositoryWrapper.SaveAsync();
                entity.Plannedactivities = null;
            }

            if (!onlyPlannedActivities)
            {
                if (entity.Designaspectsnetworkfunctions != null && entity.Designaspectsnetworkfunctions.Count > 0)
                {
                    var usedNetworkFunctions = entity.Designaspectsnetworkfunctions.ToList();

                    foreach (var toDelete in usedNetworkFunctions)
                        _repositoryWrapper.DesignAspectNetworkFunctionRepository.DeleteDeep(toDelete);
                }
                if (entity.Designaspectssupportedsvr != null && entity.Designaspectssupportedsvr.Count > 0)
                {
                    var supportedServicesLst = entity.Designaspectssupportedsvr.ToList();

                    foreach (var toDelete in supportedServicesLst)
                        _repositoryWrapper.DesignAspectSupportedServiceRepository.DeleteDeep(toDelete);
                }

                _repositoryWrapper.DesignAspectRepository.DeleteDeep(entity);
            }
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> AddOrUpdateMultipleDcf(List<long?> dcfIds, long paId,bool isDelete = false)
        {
            try
            {

                List<Daplannedactivitydcf> existingMultipleDcfEntry = _repositoryWrapper.DaPlannedActivityDcfRepository
                                                                        .FindByCondition(x => x.Plannedactivityid == paId)
                                                                        .ToList();

                #region Add MultipleDcf
                foreach (long dcfId in dcfIds)
                {

                    if (!existingMultipleDcfEntry.Any(x => x.Designcomponentfamilyid == dcfId))
                    {
                        _repositoryWrapper.DaPlannedActivityDcfRepository.Create(new Daplannedactivitydcf
                        {
                            Designcomponentfamilyid = dcfId,
                            Plannedactivityid = paId,
                            Dcfstatus = false
                        });
                    }
                }
                await _repositoryWrapper.SaveAsync();
                #endregion

                #region Remove Unused DesignComponentFamilies
                List<Daplannedactivitydcf> recordsToDelete = existingMultipleDcfEntry
                    .Where(y => y.Plannedactivityid == paId)
                    .ToList();
                if (isDelete == true)
                {
                    foreach (Daplannedactivitydcf record in recordsToDelete)
                    {
                        _repositoryWrapper.DaPlannedActivityDcfRepository.DeleteDeep(record);
                    }
                }

                if (recordsToDelete.Any())
                {
                    await _repositoryWrapper.SaveAsync();
                }
                #endregion


                await _repositoryWrapper.ClearTracker();

            }
            catch (Exception ex)
            {
                
                _logger.LogError("Design Aspect - Project Plan PA - AddOrUpdateMultipleDcf() : " + ex);
                throw;
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<bool> DuplicateCheckForMultipleDcf(List<PlannedActivityDtoUpdate> items)
        {
            if (items == null || !items.Any() || !items.Any(x => x.PlannedActivityId == 0))
            {
                return false;
            }

            var projectNames = items
                .Select(i => string.IsNullOrEmpty(i.DeliveryProjectName)
                    ? string.Empty
                    : i.DeliveryProjectName.Split('-')[0])
                .Distinct()
                .ToList();

            var opCoId = items.Select(i => i.OpCoId).Distinct().ToList().FirstOrDefault();
            var programIds = items.Select(i => i.ProgramId == 0 ? (long?)null : i.ProgramId).Distinct().ToList();

            var existingPAEntities = await _repositoryWrapper.PlannedActivity
               .FindByCondition(x =>
                   x.Opcoid == opCoId &&
                   (x.Programid == null || programIds.Contains(x.Programid)) && x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Project_Plan)
               .Include(x => x.Plannedactivityresource)
               .Include(x => x.Daplannedactivitydcf).ToListAsync();

            var existingEntities = existingPAEntities
                .Where(x => !string.IsNullOrEmpty(x.Deliveryprojectname) &&
                            projectNames.Any(pn => x.Deliveryprojectname.Split('-')[0].ToLower() == pn.ToLower())).SelectMany(x => x.Daplannedactivitydcf.Select(y => y.Designcomponentfamilyid).ToList());

            var currentMultipleDCFs = items.Where(i => i.PlannedActivityId == 0 && i.DesignComponentFamilyIdList.Any());
            foreach (var item in currentMultipleDCFs)
            {
                var newFamilyIds = item.DesignComponentFamilyIdList.Distinct();

                if (newFamilyIds.Any(id => existingEntities.Contains(id)))
                {
                    return true;
                }
            }

            return false;
        }

    }

}
