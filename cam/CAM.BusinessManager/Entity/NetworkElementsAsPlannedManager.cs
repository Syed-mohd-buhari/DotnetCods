using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DeploymentStatus;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsIs;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.PlannedActivityResource;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.Location;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.LookUp.Location;
using CAM.DataTransferObjects.LookUp.PlannedActivityResourceDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Entities.Models.Settings;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using IdentityServer4.Extensions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CAM.Enum.LcmEnum;
using static CAM.Enum.ResourceTypeEnum;

namespace CAM.BusinessManager.Entity
{
    public class NetworkElementsAsPlannedManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        private PlannedActivityManager _plannedActivityManager;
        private IdentityAsIsManager _identityAsIsManager;
        private readonly DeliveryTrackingManager _deliveryTrackingManager;
        private readonly ResourceKeyMasterManager _resourceKeyMasterManager;
        private DesignComponentFamilyLifeCycleManager _designComponentFamilyLifeCycleManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly ILoggerManager _logger;
        public NetworkElementsAsPlannedManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, IdentityAsIsManager identityAsIsManager,
            GridCustomColumnManager manager, PlannedActivityManager plannedActivityManager, DesignComponentFamilyLifeCycleManager designComponentLIfecycleManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, DeliveryTrackingManager deliveryTrackingManager, ResourceKeyMasterManager resourceKeyMasterManager,
            ICurrentUserService currentUserService, CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager, ILoggerManager logger) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _plannedActivityManager = plannedActivityManager;
            _identityAsIsManager = identityAsIsManager;
            _deliveryTrackingManager = deliveryTrackingManager;
            _resourceKeyMasterManager = resourceKeyMasterManager;
            _designComponentFamilyLifeCycleManager = designComponentLIfecycleManager;
            _currentUserService = currentUserService;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _logger = logger;
        }

        public ResultDto<LocationDeploymentTypeRelatedEntity> GetLocationDeploymentTypeRelated(int locationId)
        {
            var location = _repositoryWrapper.Location.FindByCondition(x => x.Locationid == locationId);
            var related = new LocationDeploymentTypeRelatedEntity();
            related.DeploymentTypeRelatedDict = location.Where(x => x.Locationid == locationId)
                .SelectMany(x => x.Locationdeploymenttypes).ToDictionary(x => x.Deploymenttypeid, x => x.Deploymenttype.Deploymenttype);
            return new ResultDto<LocationDeploymentTypeRelatedEntity>()
            {
                Warning = false,
                Data = related,
                Info = ""
            };

        }

        public async Task<ResultDto> Add(NetworkElementAsPlannedDtoCreate dto, bool? forced = false)
        {
            var entityExists = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
                x => x.Opcoid == dto.OpCoId
                    && x.Designcomponentid == dto.DesignComponentId
                    && x.Elementname == dto.ElementName
                , ConstantValueFilter.isTrue)
                .OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                if (entityExists.Deleted == ConstantValueFilter.isTrue)
                {
                    if (forced == true)
                    {
                        return await AddBase(dto);
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryAddExists,
                            Data = new { id = entityExists.Networkelementasplannedid, orphanDeleted = true }
                        };
                    }
                }
                else
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                        Data = entityExists.Networkelementasplannedid
                    };
                }
            }
            return await AddBase(dto);
        }

        public async Task<ResultDto> AddBase(NetworkElementAsPlannedDtoCreate dto)
        {
            var entity = _mapper.Map<NetworkElementAsPlanned>(dto);
            return await AddNetworkElementsEntity(entity);
        }

        public async Task<ResultDto> AddNetworkElementsEntity(NetworkElementAsPlanned entity)
        {
            var existingAssetDeploymentStatusId = entity.DeploymentStatusId;

            entity.LocationId ??= _repositoryWrapper.Location.FindByCondition(x => x.Defaultvalue == ConstantValueFilter.isTrue).Single()
              .Locationid;
            var inServiceId = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.ToLower().Replace(" ", "") == ConstantValueFilter.InService).Select(x => x.Deploymentstatusid).FirstOrDefault();
            var lcmEntity = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Opcoid == entity.OpCoId && x.Designcomponentid == entity.DesignComponentId)
               .Include(x => x.Lcmengineeringeduspoc).Include(x => x.Lcmengineeringsubdomainspoc).FirstOrDefault();

            #region #1658 - Archive LCM - Move Asset PA to Archive
            var assetDeployment = await _dropdownDataServiceManager.GetAssetDeploymentStatus();
            var inservice_Decommission_AssetDeploymentId = assetDeployment
                .Where(x => ConstantValueFilter.assetDeploymentStatusForArchivePa.Any(y => y == x.Value.Replace(" ", "").ToLower()))
                .Select(t => t.Key).ToList();
            #endregion

            string swResourceKey = string.Empty;
            string hwResourceKey = string.Empty;

            var AssetKeys = await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforAssets(entity.DesignComponentId, entity.OpCoId, entity.ElementName, (long)(lcmEntity?.Buildbagid ?? 0));
            if (AssetKeys != null)
            {
                swResourceKey = AssetKeys[(int)ResourceTypesKey.SWAsset];
                hwResourceKey = AssetKeys[(int)ResourceTypesKey.HWAsset];
                entity.SwResourceKey = swResourceKey;
                entity.HwResourceKey = hwResourceKey;
            }

            if (entity.DeploymentStatusId == inServiceId && lcmEntity != null)
            {
                entity.LcmEngineeringId = lcmEntity?.Lcmengineeringid;
            }
            if (lcmEntity != null)
            {
                entity.Buildbagid = lcmEntity.Buildbagid;
                foreach (var plan in entity.PlannedActivities)
                {
                    plan.Buildbagid = entity.Buildbagid;
                }
            }
            var model = NetworkElementAsPlannedMapper.Set(entity);
            if (entity.LcmEngineeringId == 0)
                model.Lcmengineeringid = null;

                _repositoryWrapper.NetworkElementAsPlanned.Create(model);

            await _repositoryWrapper.SaveAsync();

           ///Ticket 1118 - Asset Create  in Asset screen - Copy the LCM Edu and Subdomain to Asset  EDU and Sub Domain Spoc Table
            if (entity?.NetworkElementAsPlannedEduSpoc?.Any() == false) await TransferEduSpocToAsset(model.Networkelementasplannedid, lcmEntity);
            if (entity?.NetworkElementAsPlannedSubDomainSpoc?.Any() == false) await TransferSubDomainSpocToAsset(model.Networkelementasplannedid, lcmEntity);


            foreach (var plannedActivity in model.Plannedactivities)
            {
                plannedActivity.Designcomponentid = plannedActivity.Designcomponentid;
                plannedActivity.Deliverystatusid = plannedActivity.Deliverystatusid == 0 ? null : plannedActivity.Deliverystatusid;
                plannedActivity.Responsibilityphaseid = plannedActivity.Responsibilityphaseid == 0 ? null : plannedActivity.Responsibilityphaseid;
                plannedActivity.Deliverystatusid = plannedActivity.Deliverystatusid == 0 ? null : plannedActivity.Deliverystatusid;
                plannedActivity.Lcmengineeringid = plannedActivity.Lcmengineeringid == 0 ? null : plannedActivity.Lcmengineeringid;
                plannedActivity.Networkelementasplannedid = plannedActivity.Networkelementasplannedid == 0 ? null : plannedActivity.Networkelementasplannedid;
                plannedActivity.Foraddasset = ConstantValueFilter.isTrue;
                plannedActivity.Foreditasset = !ConstantValueFilter.isTrue;
                plannedActivity.Opcoid = entity.OpCoId;
                plannedActivity.Archived = plannedActivity.Archived == null ? !ConstantValueFilter.isTrue : plannedActivity.Archived;
                plannedActivity.Buildbagid = entity.Buildbagid;                
                plannedActivity.Plannedactivitycategoryid =
                            plannedActivity.Plannedactivitycategoryid == 0 ? null : plannedActivity.Plannedactivitycategoryid;
                plannedActivity.Programid =
                            plannedActivity.Programid == 0 ? null : plannedActivity.Programid;
                if (entity.DesignComponentId != 0)
                {
                    var dcfId = _repositoryWrapper.DesignComponent
                        .FindByCondition(p => p.Designcomponentid == entity.DesignComponentId)
                        .Select(p => p.Designcomponentfamilyid).SingleOrDefault();
                    plannedActivity.Designcomponentfamilyid = dcfId;
                }

                if (plannedActivity.Plannedactivityid != 0)
                {
                    var plannedActivityExist = PlannedActivityExists(plannedActivity.Plannedactivityid, plannedActivity.Plannedactivityresourceid, model.Networkelementasplannedid);
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
                        _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                        await _repositoryWrapper.SaveAsync();
                        if (plannedActivity.Deliveryplanavailable)
                        {
                            var mileStoneStatus = await _commonManager.CalculateMSDuration(plannedActivity, _repositoryWrapper);
                            if (mileStoneStatus.isSuccess)
                            {
                                // We should pass inservice and planned asset count only 
                                var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                int assetCount = model.Deploymentstatusid == getAssetInserviceID && model.Environmentid == getProductionEnvrionmentId ? 1 : 0;

                                var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, plannedActivity, assetCount);
                                await _deliveryTrackingManager.Add(deliveryTracking);
                            }

                        }
                        var addProjectPlanEntity = await _commonManager.AddProjectPlan(plannedActivity, _repositoryWrapper);

                        foreach (var plan in addProjectPlanEntity)
                        {
                            _repositoryWrapper.ProjectPlanRepository.Create(plan);
                        }
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                else
                {
                    var plannedActivityExist = PlannedActivityExists(plannedActivity.Plannedactivityid, plannedActivity.Plannedactivityresourceid, model.Networkelementasplannedid);
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
                        _repositoryWrapper.PlannedActivity.Create(plannedActivity);
                        await _repositoryWrapper.SaveAsync();
                        if (plannedActivity.Deliveryplanavailable)
                        {
                            var mileStoneStatus = await _commonManager.CalculateMSDuration(plannedActivity, _repositoryWrapper);
                            if (mileStoneStatus.isSuccess)
                            {
                                // We should pass inservice and planned asset count only 
                                var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                int assetCount = model.Deploymentstatusid == getAssetInserviceID && model.Environmentid == getProductionEnvrionmentId ? 1 : 0;

                                var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, plannedActivity, assetCount);
                                await _deliveryTrackingManager.Add(deliveryTracking);
                            }

                        }
                        var addProjectPlanEntity = await _commonManager.AddProjectPlan(plannedActivity, _repositoryWrapper);

                        foreach (var plan in addProjectPlanEntity)
                        {
                            _repositoryWrapper.ProjectPlanRepository.Create(plan);
                            await _repositoryWrapper.SaveAsync();

                            await _commonManager.CreateOrUpdateProjectPlanAudit(plan.Projectsplanid, null, plannedActivity.Plannedcompletion.Value.ToShortDateString(), 1, _repositoryWrapper);
                        }
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }

                }

                var settingRuleElementCount = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.AddAsset && x.Plannedactivityresourceid == plannedActivity.Plannedactivityresourceid && x.Deliverystatusid == plannedActivity.Deliverystatusid)
                       .Include(x => x.Deliverystatus)?.FirstOrDefault();


                if (settingRuleElementCount?.Ruleelementcount == (int)RuleElementCountEnum.RolloutComplete)
                {
                    plannedActivity.Archived = ConstantValueFilter.isTrue;
                    _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }

                await _commonManager.CreateOrUpdateBptreport(plannedActivity.Plannedactivityid, model.Designcomponentid,(long)plannedActivity.Designcomponentid);
            }

            model = await UpdateFromVNFTransition(model);

            if (model.Plannedactivities.Count > 1)
            {
                var activePAs = model.Plannedactivities.Where(x => x.Archived != ConstantValueFilter.isTrue);

                var earliestPA = activePAs.FirstOrDefault(x => x.Plannedcompletion == activePAs.Min(p => p.Plannedcompletion));

                var earliestSetting = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.AddAsset && x.Plannedactivityresourceid == earliestPA.Plannedactivityresourceid && x.Deliverystatusid == earliestPA.Deliverystatusid)
                      .Include(x => x.Deliverystatus)
                      .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus)?.FirstOrDefault();

                var assetDeploymentStatus = earliestSetting?.Settingupdateplannedactivityassetdeploymentstatus?.FirstOrDefault()?.Assetdeploymentstatusid;

                model.Deploymentstatusid = assetDeploymentStatus != null ? assetDeploymentStatus.Value : model.Deploymentstatusid;


            } 
            _repositoryWrapper.NetworkElementAsPlanned.Update(model);
            await _repositoryWrapper.SaveAsync();
            ///Ticket 1292 - Insert/Update/Delete actions in the Asset screen are not updating the LCM Number of Nodes Count and DCF Implementation Value.
           await _commonManager.UpdateLcmNodeCountsField((long)(model?.Lcmengineeringid ?? 0));
            await _commonManager.SetImplementationFlagInDCF(model.Designcomponentid );

            if (existingAssetDeploymentStatusId != model.Deploymentstatusid &&
                     inservice_Decommission_AssetDeploymentId.Any(x => x == model.Deploymentstatusid))
                await ArchiveAssetAddNodeAndDecommissionPA(model.Networkelementasplannedid);


            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(NetworkElementAsPlannedDtoUpdate dto, bool? forced = false)
        {

            var _LcmEng = _repositoryWrapper.Lcmengineering.FindByCondition(
                        x => x.Opcoid == dto.OpCoId && x.Designcomponentid == dto.DesignComponentId).Select(x => x.Lcmengineeringid).FirstOrDefault();


            if (_LcmEng != 0)
            {
                    dto.LcmEngineeringId = _LcmEng;
            }
                var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
                x => x.Opcoid == dto.OpCoId
                     && x.Designcomponentid == dto.DesignComponentId
                     && x.Elementname == dto.ElementName
                    && x.Networkelementasplannedid != dto.NetworkElementAsPlannedId, ConstantValueFilter.isTrue)
                .OrderByDescending(x => x.Creationdate)
                .FirstOrDefaultAsync();

            ///Start of the LCM R8 - Part 3 requirements to update element Name in Dcf and resourkeymaster page
            var elementName = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
                             x => x.Opcoid == dto.OpCoId
                            && x.Designcomponentid == dto.DesignComponentId
                            && x.Networkelementasplannedid == dto.NetworkElementAsPlannedId).Select(x => x.Elementname).SingleOrDefault();
            if (elementName != null)
            {
                dto.OldElementName = elementName;
                if (elementName != dto.ElementName)
                {
                    _designComponentFamilyLifeCycleManager.UpdateAssetNameOnDcfLifecycle(dto.DesignComponentId, dto.OpCoId, dto.NetworkElementAsPlannedId, dto.ElementName);
                }
            }
            else dto.OldElementName = dto.ElementName;
            ///End of LCM R8 - Part 3 requirements to update element Name in Dcf and resourkeymaster page

            var originalEntityWithSameNaturalKey = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
                    x => x.Opcoid == dto.OpCoId
                         && x.Designcomponentid == dto.DesignComponentId
                         && x.Elementname == dto.ElementName
                        && x.Networkelementasplannedid == dto.NetworkElementAsPlannedId, true)
                    .SingleOrDefaultAsync();
            if (anotherEntityWithSameNaturalKeyExists != null && originalEntityWithSameNaturalKey == null)
            {
                if (anotherEntityWithSameNaturalKeyExists.Deleted == ConstantValueFilter.isTrue)
                {
                    if (forced == true)
                    {
                        return await UpdateBase(dto, forced);
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryUpdateExists,
                            Data = new { id = anotherEntityWithSameNaturalKeyExists.Networkelementasplannedid, orphanDeleted = true }
                        };
                    }
                }
                else
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = anotherEntityWithSameNaturalKeyExists.Deleted.Value ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
                        Data = anotherEntityWithSameNaturalKeyExists.Designcomponentid
                    };
                }
            }


            return await UpdateBase(dto, forced);
        }
        public Plannedactivities PlannedActivityExists(long plannedActivityId, short? plannedActivityResourceID, long? AssetId)
        {
            return plannedActivityId == 0
                ? _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Archived != ConstantValueFilter.isTrue && x.Plannedactivityresourceid == plannedActivityResourceID
                && x.Networkelementasplannedid == AssetId).FirstOrDefault()

                : _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid != plannedActivityId && x.Archived != ConstantValueFilter.isTrue
                && x.Plannedactivityresourceid == plannedActivityResourceID
                && x.Networkelementasplannedid == AssetId).FirstOrDefault();

        }
        private async Task<ResultDto> UpdateBase(NetworkElementAsPlannedDtoUpdate dto, bool? forced)
        {
            var entity = _mapper.Map<NetworkElementAsPlanned>(dto);
            var mappedEntity = NetworkElementAsPlannedMapper.Set(entity);
            var designComponent = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == dto.DesignComponentId).FirstOrDefault();
            entity.LocationId ??= _repositoryWrapper.Location.FindByCondition(x => x.Defaultvalue == ConstantValueFilter.isTrue).Single()
                .Locationid;
            var inServiceId = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.ToLower().Replace(" ", "") == ConstantValueFilter.InService).Select(x => x.Deploymentstatusid).FirstOrDefault();
            var lcmId = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Opcoid == entity.OpCoId && x.Designcomponentid == entity.DesignComponentId)
                                        .Select(x => x.Lcmengineeringid).FirstOrDefault();


            var originalEntityKey = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
               x => x.Networkelementasplannedid == dto.NetworkElementAsPlannedId, ConstantValueFilter.isTrue)
               .SingleOrDefaultAsync();

            #region #1658 - Archive LCM - Move Asset PA to Archive
            var assetDeployment = await _dropdownDataServiceManager.GetAssetDeploymentStatus();
            var inservice_Decommission_AssetDeploymentId = assetDeployment
                .Where(x => ConstantValueFilter.assetDeploymentStatusForArchivePa.Any(y => y == x.Value.Replace(" ", "").ToLower()))
                .Select(t => t.Key).ToList();
            var existingAssetDeploymentStatusId = originalEntityKey?.Deploymentstatusid;
            var currentAssetDeploymentStatusId = entity.DeploymentStatusId;
            #endregion

            ///Start of LCM R8 - Part 3 requirements to update overridden resource keys
            ///Updating the ResourceKey in Resourcekey Master table
            if (originalEntityKey != null)
            {
                if (originalEntityKey.Networkconstruct != entity.NetworkConstruct)
                {
                    var _assetExists = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
                             x => x.Opcoid == dto.OpCoId && x.Networkconstruct == entity.NetworkConstruct
                            && x.Elementname != entity.ElementName).OrderBy(x => x.Modificationdate).FirstOrDefault();

                    if (_assetExists != null)
                    {
                        var _networkConstruct = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == _assetExists.Designcomponentid).FirstOrDefault();
                        /// only if the network construct belongs to same DCFID, the hardware resource key will be asigned to shared assets.
                        if (designComponent.Designcomponentfamilyid == _networkConstruct.Designcomponentfamilyid)
                        {
                            if (entity.HwResourceKey != _assetExists.Hwresourcekey)
                            {
                                entity.PreviousHWResourceKey = entity.HwResourceKey;
                                entity.HwResourceKey = _assetExists.Hwresourcekey;
                            }
                        }
                    }
                }
                else
                {
                    if (entity.HwResourceKey != originalEntityKey.Hwresourcekey)
                    {
                        entity.PreviousHWResourceKey = entity.HwResourceKey;
                    }
                }
                if (entity.SwResourceKey != originalEntityKey.Swresourcekey)
                {
                    entity.PreviousSWResourceKey = entity.SwResourceKey;
                }

                _designComponentFamilyLifeCycleManager.UpdateResoureKeyForAssetInDcfLifecycle(entity);
            }


            if (entity.DeploymentStatusId == inServiceId && lcmId != 0)
            {
                entity.LcmEngineeringId = lcmId;
            }
            else if(entity.LcmEngineeringId == 0)
            {
                entity.LcmEngineeringId = null;
            }

            if (forced == true)
            {
                entity.Deleted = !ConstantValueFilter.isTrue;
                entity.DeletionDate = null;
            }

            var ids = entity.PlannedActivities.Select(s => s.PlannedActivityId);
            var subDomainSpocsRelations =
                _repositoryWrapper.NetworkElementAsPlannedSubDomainSpoc.
                    FindByCondition(x => x.Networkelementasplannedid == dto.NetworkElementAsPlannedId).ToList();

            foreach (var toDelete in subDomainSpocsRelations)
                _repositoryWrapper.NetworkElementAsPlannedSubDomainSpoc.DeleteDeep(toDelete);

            var eduSpocRelations =
                _repositoryWrapper.NetworkElementAsPlannedEduSpoc.FindByCondition(x => x.Networkelementasplannedid == dto.NetworkElementAsPlannedId).ToList();

            foreach (var toDelete in eduSpocRelations)
                _repositoryWrapper.NetworkElementAsPlannedEduSpoc.DeleteDeep(toDelete);

            var plannedActivitiestoDelete = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Archived != true
            && x.Networkelementasplannedid == dto.NetworkElementAsPlannedId
            && !ids.Contains(x.Plannedactivityid)
               , false, false).Include(x => x.Budgetprojecttrackers).Include(x => x.Projectsplan).ThenInclude(x => x.Projectplanaudit).ToList();
            foreach (var toDelete in plannedActivitiestoDelete)
            {
                await _commonManager.GenerateAuditLogEntryForPAHardDeleteEntity(toDelete);
                var deliveryTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == toDelete.Plannedactivityid).FirstOrDefault();
                if (deliveryTracking != null)
                {
                    _repositoryWrapper.DeliveryTrackingRepository.DeleteDeep(deliveryTracking);
                }
                foreach(var deletePP in toDelete.Projectsplan)
                {
                    foreach (var deleteppa in deletePP.Projectplanaudit)
                    {
                        _repositoryWrapper.ProjectPlanAuditRepository.DeleteDeep(deleteppa);
                    }
                    _repositoryWrapper.ProjectPlanRepository.DeleteDeep(deletePP);
                }
                if (toDelete.Budgetprojecttrackers != null && toDelete.Budgetprojecttrackers.Count > 0)
                {
                    if (toDelete.Budgetprojecttrackers.FirstOrDefault() != null)
                    {
                        _repositoryWrapper.BudgetProjectTrackersRepository.DeleteDeep(toDelete.Budgetprojecttrackers.FirstOrDefault());
                    }

                }
                _repositoryWrapper.PlannedActivity.DeleteDeep(toDelete);
            }

            dto.EduSpocIds = dto.EduSpocIds?.Distinct()?.ToList();
            dto.SubDomainSpocIds = dto.SubDomainSpocIds?.Distinct()?.ToList();

            var networkElementAsIsRelations = _repositoryWrapper.NetworkElementAsIs.
                    FindByCondition(x => x.Networkelementasplannedid == dto.NetworkElementAsPlannedId);
            foreach (var toUpdate in networkElementAsIsRelations)
            {
                toUpdate.Elementdeploymentname = dto.ElementName;
                if (designComponent != null)
                    toUpdate.Systemtypeid = designComponent.Systemtypeid;
                if (dto.LocationId.HasValue) toUpdate.Locationid = dto.LocationId.Value;
                _repositoryWrapper.NetworkElementAsIs.Update(toUpdate);
            }
            entity.DeploymentStatusId = dto.DeploymentStatusId;
            entity.IsAssured = false;
            var savedData = NetworkElementAsPlannedMapper.Set(entity);

            foreach (var item in dto.EduSpocIds)
            {
                var newAssetEduSpoc = new Networkelementasplannededuspoc
                {
                    Networkelementasplannedid = savedData.Networkelementasplannedid,
                    Eduspocid = item
                };
                _repositoryWrapper.NetworkElementAsPlannedEduSpoc.Create(newAssetEduSpoc);
            }
            foreach (var item in dto.SubDomainSpocIds)
            {
                var newAssetSubDomain = new Networkelementasplannedsubdomainspoc
                {
                    Networkelementasplannedid = savedData.Networkelementasplannedid,
                    Subdomainspocid = item
                };
                _repositoryWrapper.NetworkElementAsPlannedSubDomainSpoc.Create(newAssetSubDomain);
            }

            _repositoryWrapper.NetworkElementAsPlanned.Update(savedData);
            await _repositoryWrapper.SaveAsync();

            currentAssetDeploymentStatusId = Convert.ToInt16( savedData.Deploymentstatusid);
            _designComponentFamilyLifeCycleManager.setResourceKeyStatus(originalEntityKey.Hwresourcekey, (int)ResourceTypesKey.HWAsset);
            _designComponentFamilyLifeCycleManager.setResourceKeyStatus(originalEntityKey.Swresourcekey, (int)ResourceTypesKey.SWAsset);


            foreach (var plannedActivity in entity.PlannedActivities)
            {
                var bptPaId = plannedActivity.PlannedActivityId;
                plannedActivity.DesignComponentId = plannedActivity.DesignComponentId;
                plannedActivity.DeliveryStatusId =
                    plannedActivity.DeliveryStatusId == 0 ? null : plannedActivity.DeliveryStatusId;
                plannedActivity.ResponsibilityPhaseId =
                    plannedActivity.ResponsibilityPhaseId == 0 ? null : plannedActivity.ResponsibilityPhaseId;
                plannedActivity.DeliveryStatus = plannedActivity.DeliveryStatusId == 0 ? null : plannedActivity.DeliveryStatus;
                plannedActivity.NetworkElementAsPlannedId = dto.NetworkElementAsPlannedId;
                plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                plannedActivity.OpCoId = entity.OpCoId;               
                plannedActivity.Plannedactivitycategoryid =
                            plannedActivity.Plannedactivitycategoryid == 0 ? null : plannedActivity.Plannedactivitycategoryid;
                plannedActivity.ProgramId =
                            plannedActivity.ProgramId == 0 ? null : plannedActivity.ProgramId;
                if (dto.DesignComponentId != 0)
                {
                    var dcfId = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == plannedActivity.DesignComponentId).Select(p => p.Designcomponentfamilyid).SingleOrDefault();
                    plannedActivity.DesignComponentFamilyId = dcfId;
                }

                var settingRuleElementCount = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.EditAsset && x.Plannedactivityresourceid == plannedActivity.PlannedActivityResourceId && x.Deliverystatusid == plannedActivity.DeliveryStatusId)
                    .Include(x => x.Deliverystatus)?.FirstOrDefault()?.Ruleelementcount;

                if (plannedActivity.PlannedActivityId != 0)
                {
                    var plannedActivityExist = PlannedActivityExists(plannedActivity.PlannedActivityId, plannedActivity.PlannedActivityResourceId, mappedEntity.Networkelementasplannedid);

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

                        plannedActivity.NetworkElementAsPlannedId = mappedEntity.Networkelementasplannedid;
                        if (settingRuleElementCount == (int)RuleElementCountEnum.RolloutComplete)
                        {
                            plannedActivity.Archived = ConstantValueFilter.isTrue;
                        }
                        var newPlannedActivity = PlannedActivityMapper.Set(plannedActivity);

                        //ProjectPlanUpdateForPlanCompletionDate
                        var pacompletiondate = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == newPlannedActivity.Plannedactivityid).Select(x => x.Plannedcompletion).FirstOrDefault();
                        if (pacompletiondate != newPlannedActivity.Plannedcompletion)
                            await _commonManager.UpdateProjectPlanDateForLastDeliveryStatusOfMS(newPlannedActivity, newPlannedActivity.Plannedcompletion, (int)MilestoneStatusEnum.MS4, _repositoryWrapper,true);

                        _repositoryWrapper.PlannedActivity.Update(newPlannedActivity);
                        await _repositoryWrapper.SaveAsync();

                        if (newPlannedActivity.Deliveryplanavailable)
                        {
                            var mileStoneStatus = await _commonManager.CalculateMSDuration(newPlannedActivity, _repositoryWrapper);
                            if (mileStoneStatus.isSuccess)
                            {
                                // We should pass inservice and planned asset count only 
                                var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                int assetCount = savedData.Deploymentstatusid == getAssetInserviceID && savedData.Environmentid == getProductionEnvrionmentId ? 1 : 0;

                                var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, newPlannedActivity, assetCount);
                                await _deliveryTrackingManager.Add(deliveryTracking);
                            }

                        }
                        var addProjectPlanEntity = await _commonManager.AddProjectPlan(newPlannedActivity, _repositoryWrapper);

                        foreach (var plan in addProjectPlanEntity)
                        {
                            _repositoryWrapper.ProjectPlanRepository.Create(plan);
                        }
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                else
                {
                    plannedActivity.ForEditAsset = ConstantValueFilter.isTrue;
                    plannedActivity.ForAddAsset = !ConstantValueFilter.isTrue;
                    plannedActivity.NetworkElementAsPlannedId = mappedEntity.Networkelementasplannedid;
                    var plannedActivityExist = PlannedActivityExists(plannedActivity.PlannedActivityId, plannedActivity.PlannedActivityResourceId, mappedEntity.Networkelementasplannedid);
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
                        if (settingRuleElementCount == (int)RuleElementCountEnum.RolloutComplete)
                        {
                            plannedActivity.Archived = true;
                        }
                        var newPlannedActivity = PlannedActivityMapper.Set(plannedActivity);

                        _repositoryWrapper.PlannedActivity.Create(newPlannedActivity);
                        await _repositoryWrapper.SaveAsync();
                          bptPaId = newPlannedActivity.Plannedactivityid;
                        if (newPlannedActivity.Deliveryplanavailable)
                        {
                            var mileStoneStatus = await _commonManager.CalculateMSDuration(newPlannedActivity, _repositoryWrapper);
                            if (mileStoneStatus.isSuccess)
                            {
                                // We should pass inservice and planned asset count only 
                                var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                int assetCount = savedData.Deploymentstatusid == getAssetInserviceID && savedData.Environmentid == getProductionEnvrionmentId ? 1 : 0;

                                var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, newPlannedActivity, assetCount);
                                await _deliveryTrackingManager.Add(deliveryTracking);
                            }

                        }
                        var addProjectPlanEntity = await _commonManager.AddProjectPlan(newPlannedActivity, _repositoryWrapper);

                        foreach (var plan in addProjectPlanEntity)
                        {
                            _repositoryWrapper.ProjectPlanRepository.Create(plan);
                            await _repositoryWrapper.SaveAsync();

                            await _commonManager.CreateOrUpdateProjectPlanAudit(plan.Projectsplanid, null, newPlannedActivity.Plannedcompletion.Value.ToShortDateString(), 1, _repositoryWrapper);
                        }
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
                await _repositoryWrapper.SaveAsync();

                await _repositoryWrapper.ClearTracker();

                await _commonManager.CreateOrUpdateBptreport(bptPaId, entity.DesignComponentId,(long)plannedActivity.DesignComponentId);
            }

           
            if (entity.PlannedActivities.Count >= 1)
            {
                var activePAs = entity.PlannedActivities.Where(x => x.Archived != ConstantValueFilter.isTrue);
                if (activePAs != null && activePAs.Count() > 0)
                {
                    var earliestPA = activePAs.FirstOrDefault(x => x.PlannedCompletion == activePAs.Min(p => p.PlannedCompletion));

                    var earliestSetting = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.EditAsset && x.Plannedactivityresourceid == earliestPA.PlannedActivityResourceId && x.Deliverystatusid == earliestPA.DeliveryStatusId)
                          .Include(x => x.Deliverystatus)
                          .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus)?.FirstOrDefault();

                    var assetDeploymentStatus = earliestSetting?.Settingupdateplannedactivityassetdeploymentstatus?.FirstOrDefault()?.Assetdeploymentstatusid;

                    var asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == entity.NetworkElementAsPlannedId).FirstOrDefault();
                    asset.Deploymentstatusid = assetDeploymentStatus != null ? assetDeploymentStatus.Value : entity.DeploymentStatusId;
                    _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                    await _repositoryWrapper.SaveAsync();

                    currentAssetDeploymentStatusId = Convert.ToInt16(asset.Deploymentstatusid);
                }


            }
            //Ticket 1292 - Insert/Update/Delete actions in the Asset screen are not updating the LCM Number of Nodes Count and DCF Implementation Value.
            await _commonManager.UpdateLcmNodeCountsField(lcmId);
            await _commonManager.SetImplementationFlagInDCF(entity.DesignComponentId);

            if (existingAssetDeploymentStatusId != currentAssetDeploymentStatusId &&
                inservice_Decommission_AssetDeploymentId.Any(x => x == currentAssetDeploymentStatusId))
                await  ArchiveAssetAddNodeAndDecommissionPA(dto.NetworkElementAsPlannedId);


            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.NetworkElementAsPlannedId
            };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == id)
                .Include(x => x.Plannedactivities).SingleAsync();

            if (entity.Plannedactivities.Any())
            {
                foreach (var toDelete in entity.Plannedactivities)
                {
                    var deliveryTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == toDelete.Plannedactivityid).FirstOrDefault();
                    if (deliveryTracking != null)
                    {
                        _repositoryWrapper.DeliveryTrackingRepository.DeleteDeep(deliveryTracking);
                    }
                    // await _commonManager.GenerateAuditLogEntryForPAHardDeleteEntity(toDelete);
                    _repositoryWrapper.PlannedActivity.Delete(toDelete);
                }
                await _repositoryWrapper.SaveAsync();
                entity.Plannedactivities = null;
            }
            _repositoryWrapper.NetworkElementAsPlanned.Delete(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Networkelementasplannedid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id, bool onlyPlannedActivities)
        {
            var entity = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == id)
                .Include(x => x.Networkelementasplannededuspoc)
                .Include(x => x.Networkelementasplannedsubdomainspoc)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Budgetprojecttrackers)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Projectsplan).ThenInclude(x => x.Projectplanaudit)
                .Include(x => x.Assethardwareancillary).ThenInclude(x => x.Assetcapacityinfo)
                 .Include(x => x.Daassetmigration)
                .SingleAsync();

            if (entity.Plannedactivities != null)
            {
                //Ticket 897 
                var plannedActivityToDelete = entity.Plannedactivities.ToList();
                foreach (var toDelete in plannedActivityToDelete)
                {
                    var deliveryTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == toDelete.Plannedactivityid).FirstOrDefault();
                    if (deliveryTracking != null)
                        _repositoryWrapper.DeliveryTrackingRepository.DeleteDeep(deliveryTracking);  //need to be revamp

                    await _commonManager.GenerateAuditLogEntryForPAHardDeleteEntity(toDelete);

                    if (toDelete.Budgetprojecttrackers != null && toDelete.Budgetprojecttrackers.Count > 0)
                    {
                        if (toDelete.Budgetprojecttrackers.FirstOrDefault() != null)
                        {
                            _repositoryWrapper.BudgetProjectTrackersRepository.DeleteDeep(toDelete.Budgetprojecttrackers.FirstOrDefault());
                        }

                    }
                    foreach(var deletePp in toDelete.Projectsplan)
                    {
                        foreach (var deletePpa in deletePp.Projectplanaudit)
                        {
                            _repositoryWrapper.ProjectPlanAuditRepository.DeleteDeep(deletePpa);
                        }
                        _repositoryWrapper.ProjectPlanRepository.DeleteDeep(deletePp);
                    }

                    _repositoryWrapper.PlannedActivity.DeleteDeep(toDelete);  //need to be revamp

                }
                entity.Plannedactivities = null;
            }

            if (!onlyPlannedActivities)
            {
                if (entity.Networkelementasplannededuspoc != null && entity.Networkelementasplannededuspoc.Count > 0)
                {
                    var eduSpocList = entity.Networkelementasplannededuspoc.ToList();
                    foreach (var toDelete in eduSpocList)
                        _repositoryWrapper.NetworkElementAsPlannedEduSpoc.DeleteDeep(toDelete);
                }
                if (entity.Networkelementasplannedsubdomainspoc != null && entity.Networkelementasplannedsubdomainspoc.Count > 0)
                {
                    var subDomainSpocList = entity.Networkelementasplannedsubdomainspoc.ToList();
                    foreach (var toDelete in subDomainSpocList)
                        _repositoryWrapper.NetworkElementAsPlannedSubDomainSpoc.DeleteDeep(toDelete);
                }

                if (entity.Daassetmigration != null && entity.Daassetmigration.Count > 0)
                {
                     
                    foreach (var toDelete in entity.Daassetmigration)
                        _repositoryWrapper.DaAssetMigrationRepository.DeleteDeep(toDelete);
                }

                if (entity.Assethardwareancillary != null && entity.Assethardwareancillary.Count > 0)
                {
                    
                    foreach (var toDelete in entity.Assethardwareancillary)
                    {
                        foreach (var item in toDelete.Assetcapacityinfo)
                        {

                            _repositoryWrapper.AssetCapacityInfoRepository.DeleteDeep(item);
                        }
                        _repositoryWrapper.AssetHardwareAncillaryRepository.DeleteDeep(toDelete);
                    }
                     
                }

                var identities = _repositoryWrapper.IdentityAsIsRepository.FindByCondition(x => x.Assetid == entity.Networkelementasplannedid).ToList();
                foreach (var identity in identities)
                {
                    _repositoryWrapper.IdentityAsIsRepository.DeleteDeep(identity);
                }
                _repositoryWrapper.NetworkElementAsPlanned.DeleteDeep(entity);

            }
            /// LCM Part3 Requirements. if the key is not used i any of the existing entities, set the status of the key to not in use.
            /// the below code block is for Software ResourceKey
            if (entity.Swresourcekey != null)
            {
                _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(entity);
                _designComponentFamilyLifeCycleManager.setResourceKeyStatus(entity.Swresourcekey, (int)ResourceTypesKey.SWAsset);

            }
            /// LCM Part3 Requirements. if the key is not used i any of the existing entities, set the status of the key to not in use.
            /// the below code block is for Hardware ResourceKey
            if (entity.Hwresourcekey != null)
            {
                _designComponentFamilyLifeCycleManager.setResourceKeyStatus(entity.Hwresourcekey, (int)ResourceTypesKey.HWAsset);
            }
            await _repositoryWrapper.SaveAsync();
            //Ticket 1292 - Insert/Update/Delete actions in the Asset screen are not updating the LCM Number of Nodes Count and DCF Implementation Value.
            await _commonManager.UpdateLcmNodeCountsField((long)(entity?.Lcmengineeringid ??0));
            await _commonManager.SetImplementationFlagInDCF(entity.Designcomponentid   );
           
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Networkelementasplannedid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            string[] plannedActivityLinked = new string[0];
            string[] plannedActivity = new string[0];
            string[] networkElementsAsIs = new string[0];


            var plannedActivityLinkedIds = _repositoryWrapper.PlannedActivity
                        .FindByCondition(x => x.Networkelementasplannedid == id && x.Linkedtoplannedactivityid != null)
                        .Select(x => x.Linkedtoplannedactivityid);

            networkElementsAsIs = _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasplannedid == id).Select(x => NetworkElementAsIsMapper.Get(x).toDescription()).ToArray();

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
                        .FindByCondition(x => x.Networkelementasplannedid == id && x.Linkedtoplannedactivityid == null)
                        .Include(x => x.Plannedactivityresource)
                        .Include(x => x.Activitystatus)
                        .Include(x => x.Deliverystatus)
                        .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                        .ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (plannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = ConstantValueFilter.plannedActivity, Values = plannedActivity });
            if (plannedActivityLinked.Length > 0)
                rm.Add(new ResultMessageDto() { Table = ConstantValueFilter.linkedPlannedActivity, Values = plannedActivityLinked });
            if (networkElementsAsIs.Length > 0)
                rm.Add(new ResultMessageDto() { Table = ConstantValueFilter.networkElementAsIs, Values = networkElementsAsIs });

            var entity = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == id)
              .Include(x => x.Opco)
              .Include(x => x.Designcomponent).SingleAsync();

            var oem = await _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == entity.Orgeqpmanufacturerid).SingleAsync();


            if (networkElementsAsIs.Length > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Network Element As Planned",
                        RecordName = entity.Opco.Opco + " - " + oem.Originalequipmentmanufacturer + " - " + entity.Elementname,
                        DataRelatedList = rm
                    }
                };
            }
            else if (plannedActivityLinked.Length > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteHasLinkedPlannedActivities,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Network Element As Planned",
                        RecordName = entity.Opco.Opco + " - " + oem.Originalequipmentmanufacturer + " - " + entity.Elementname,
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
                        EntityName = "Network Element As Planned",
                        RecordName = entity.Opco.Opco + " - " + oem.Originalequipmentmanufacturer + " - " + entity.Elementname,
                        DataRelatedList = rm
                    }
                };
            }
            else
            {
                return new ResultDto();
            }

        }

        public async Task<ResultDto> Restore(long id)
        {

            var entity = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == id, true).SingleAsync();

            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.NetworkElementAsPlanned.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Networkelementasplannedid
            };
        }

        public NetworkElementAsPlannedDto Get(long id)
        {
            var entity = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == id).Single();
            return _mapper.Map<NetworkElementAsPlannedDto>(entity);
        }

        public async Task< NetworkElementAsPlannedDtoCreate> GetCreatePage(List<short> _opcoList, List<int> _verticalList)
        {

            #region    //Ticket 595 -#590 - Vertical Filter to be applied on design component dropdown's in LC, PA, DA etc.,
             
            var designComponentFamilyResource = await Task.Run(() => _repositoryWrapper.DesignComponentFamily.FindAll()
            .ToDictionary(x => x.Designcomponentfamilyid, x => x.DCFName(_repositoryWrapper)).Where(f => !f.Value.IsNullOrEmpty())
            .ToDictionary(k => k.Key, v => v.Value));

            var designComponentResource = await Task.Run(() => DesignComponentTypeExtensionMethod.verticalBasedDesignComponentRecord(_verticalList, _repositoryWrapper));

            #endregion

            var opcoResource = (_opcoList != null && _opcoList.Any() == true) ?
                                 _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco)
                                 : _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            var environmentReosurce = _repositoryWrapper.Environment.FindAll().ToDictionary(x => (int)x.Environmentid, x => x.Environment);
            var deploymentStatusReosurce = _repositoryWrapper.DeploymentStatus.FindAll();
            var locations = _repositoryWrapper.Location.FindAll().ToDictionary(x => (int)x.Locationid, x => _mapper.Map<LocationDto>(LocationMapper.GetLocationMapper(x)));
            var oemResource = _repositoryWrapper.OriginalEquipmentManufacturer.FindAll().ToDictionary(x => x.Orgeqpmanufacturerid,
                    x => x.Originalequipmentmanufacturer);
            var deploymentTypeReosurce = _repositoryWrapper.DeploymentType.FindAll().ToDictionary(x => (int)x.Deploymenttypeid, x => x.Deploymenttype);
            var designContact = (_opcoList != null && _opcoList.Any() == true) ? _repositoryWrapper.UserRepository.FindAll()
                                                                        .Include(y => y.AspnetuseropcosUser)
                                                                                    .Include(y => y.AspnetuserverticalsUser).ThenInclude(y => y.Organisation)
                                                                                    .Where(x =>
                                                                                           x.AspnetuseropcosUser.Any(y => _opcoList.Contains((short)y.Opcoid)) &&
                                                                                           x.AspnetuserverticalsUser.Any(x1 => _verticalList.Contains((int)x1.Organisation.Verticalid))
                                                                                        ).ToList()
                                                                        : _repositoryWrapper.UserRepository.FindAll().ToList();
            var subSpoc = designContact.Where(x => x.Issubdomainspoc == ConstantValueFilter.isTrue).ToList();
            var eduSpoc = designContact.Where(x => x.Iseduspoc == ConstantValueFilter.isTrue).ToList();
            var systemNames = _repositoryWrapper.SystemNamesRepository.FindAll().ToDictionary(x => x.Systemnameid, x => x.Systemnamedescription);
            var nfviBundleIDReosurce = _repositoryWrapper.NFVIBundleID.FindAll().ToDictionary(x => (int)x.Nfvibundleidid, x => x.Nfvibundleid);
            List<ViewBagandComponenetDto> buildBagResources = await _commonManager.GetBagAndComponentDetailsForDropdownAsync(0, true);


            var plannedResource = await Task.Run(() => _repositoryWrapper.PlannedActivityResourceRepository
                   .FindAll()
                   .Include(x => x.Plannedactivityresourceplanningrisk)
                   .Include(x => x.Plannedactivityresourcebenefit)
                   .Include(x => x.Plannedactivityresourcedriver).AsNoTracking());

            var paresourceEntity = new PlannedActivityResource();
            var getSelectedDrivers = _repositoryWrapper.PlannedActivityResourceDriver.FindAll().ToList();
            var getSelectedBenefits = _repositoryWrapper.PlannedActivityResourceBenefit.FindAll().ToList();
            var getSelectedPlanningRisks = _repositoryWrapper.PlannedActivityResourcePlanningRisk.FindAll().ToList();

            var plannedActivityResource = await Task.Run(() => plannedResource.ToDictionary(x => x.Plannedactivityresourceid,                
                x =>
                {
                    return new PlannedActivityResourceDto()
                    {
                        PlannedActivityResourceDescription = x.Plannedactivityresource,
                        LastModified = x.Modificationdate,
                        RuleActicvityDetails = x.Ruleacticvitydetails,
                        RuleLinkedDc = x.Rulelinkeddc,
                        RuleAddAsset = x.Ruleaddasset,
                        RuleEditAsset = x.Ruleeditasset,

                        DriverTextAddAsset = getSelectedDrivers.Where(x => x.Forlcm == false && x.Fordesignaspect == false 
                        && x.Foraddasset == true && x.Foreditasset == false).Select(x => x.Driverid).ToList(),                        
                        BenefitTextAddAsset = getSelectedBenefits.Where(x => x.Forlcm == false && x.Fordesignaspect == false
                        && x.Foraddasset == true && x.Foreditasset == false).Select(x => x.Benefitid).ToList(),                     
                        PlanningRisksAddAsset = getSelectedPlanningRisks.Where(x => x.Forlcm == false && x.Fordesignaspect == false
                        && x.Foraddasset == true && x.Foreditasset == false).Select(x => x.Planningriskid).ToList(),   
                        

                        DriverTextEditAsset = getSelectedDrivers.Where(x => x.Forlcm == false && x.Fordesignaspect == false
                        && x.Foraddasset == false && x.Foreditasset == true).Select(x => x.Driverid).ToList(),
                        BenefitTextEditAsset = getSelectedBenefits.Where(x => x.Forlcm == false && x.Fordesignaspect == false
                        && x.Foraddasset == false && x.Foreditasset == true).Select(x => x.Benefitid).ToList(),
                        PlanningRisksAEditAsset = getSelectedPlanningRisks.Where(x => x.Forlcm == false && x.Fordesignaspect == false
                        && x.Foraddasset == false && x.Foreditasset == true).Select(x => x.Planningriskid).ToList(),


                        DriverTextLcm = getSelectedDrivers.Where(x => x.Forlcm == true && x.Fordesignaspect == false
                        && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Driverid).ToList(),
                        BenefitTextLcm = getSelectedBenefits.Where(x => x.Forlcm == true && x.Fordesignaspect == false
                        && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Benefitid).ToList(),
                        PlanningRisksLcm = getSelectedPlanningRisks.Where(x => x.Forlcm == true && x.Fordesignaspect == false
                        && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Planningriskid).ToList(),

                        DriverTextDesignAspect = getSelectedDrivers.Where(x => x.Forlcm == false && x.Fordesignaspect == true
                         && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Driverid).ToList(),
                        BenefitTextDesignAspect = getSelectedBenefits.Where(x => x.Forlcm == false && x.Fordesignaspect == true
                        && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Benefitid).ToList(),
                        PlanningRisksDesignAspect = getSelectedPlanningRisks.Where(x => x.Forlcm == false && x.Fordesignaspect == true
                        && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Planningriskid).ToList(), 

                        ForCreateAddAsset = x.Forcreateaddasset,
                        ForCreateEditAsset = x.Forcreateeditasset,

                        ForEditAddAsset = x.Foreditaddasset,
                        ForEditEditAsset = x.Forediteditasset,

                        RuleActicvityDetailsAddAsset = x.Ruleactdetailsaddasset,
                        RuleActicvityDetailsEditAsset = x.Ruleactdetailseditasset,

                        Exportable = x.Exportable,

                        LcmLabelHardware = x.Lcmlabelhardware,
                        LcmLabelSoftware = x.Lcmlabelsoftware,
                        LcmHardware = x.Lcmhardware,
                        LcmSoftware = x.Lcmsoftware,

                        AddAssetLabelHardware = x.Addassetlabelhardware,
                        AddAssetLabelSoftware = x.Addassetlabelsoftware,
                        AddAssetHardware = x.Addassethardware,
                        AddAssetSoftware = x.Addassetsoftware,
                        EditAssetHardware = x.Editassethardware,
                        EditAssetLabelHardware = x.Editassetlabelhardware,
                        EditAssetSoftware = x.Editassetsoftware,
                        EditAssetLabelSoftware = x.Editassetlabelsoftware,
                        ForLcm = x.Forlcm,
                        ForAddAsset = x.Foraddasset,
                        ForEditAsset = x.Foreditasset,

                        JsonForm = x.Jsonform,
                        PlannedActivityResourceId = x.Plannedactivityresourceid,
                        ActivityDetailsAddAsset = x.Activitydetailsaddasset,
                        ActivityDetailsEditAsset = x.Activitydetailseditasset,
                        ActivityDetailsLcm = x.Activitydetailslcm,
                        ActivityDetailsForVirtualizedAddAsset = x.Actdetailsforvrtaddasset,
                        ActivityDetailsForVirtualizedEditAsset = x.Actdetailsforvrteditasset
                    };



                }
                
                )) ;
            
            
            
            var model = new NetworkElementAsPlannedDtoCreate()
            {


                DesignComponentReosurce = designComponentResource
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .ToDictionary(x => x.Designcomponentid, x => x.toDesignComponentFamily(_repositoryWrapper)),

                OpCoReosurce = opcoResource ,
                EnvironmentReosurce = environmentReosurce,
                DeploymentStatusReosurce = deploymentStatusReosurce.ToDictionary(x => x.Deploymentstatusid, dto => new DeploymentStatusDto()
                {
                    DeploymentStatusId = dto.Deploymentstatusid,
                    DeploymentStatusDescription = dto.Deploymentstatus,
                    Rule = (int)dto.Rule,
                    PlannedActivityResourceAllowedId = DeploymentStatusMapper.GetDeploymentStatusPAResourceAllowed(dto).toPlannedActivityResourceKeyList(),
                    ReadOnlyPlannedActivity = dto.Readonlyplannedactivity,
                    CheckPlannedActivity = dto.Checkplannedactivity,

                }),

                LocationId = null,
                OriginalEquipmentManufacturerResource = oemResource,
                DeploymentTypeReosurce = deploymentTypeReosurce,
                SubDomainSpocResource = subSpoc?.DistinctBy(x => x.Id).ToDictionary(x => (int)x.Id, x => x.Email),
                EduSpocResource = eduSpoc?.DistinctBy(x => x.Id).ToDictionary(x => (int)x.Id, x => x.Email),
                NfviBundleIDReosurce = nfviBundleIDReosurce,
                LocationReosurce = locations,
                DesignComponentFamilyResource = designComponentFamilyResource,
                BuildBagResources = buildBagResources,
                SystemNames = systemNames,
            };
            return model;
        }


        private Dictionary<short, DeploymentStatusDto> GetDeploymentStatusResource(IQueryable<Settingsupdateplannedactivity> query)
        {
            return query
                .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus)
                .ThenInclude(x => x.Assetdeploymentstatus)
                .SelectMany(x => x.Settingupdateplannedactivityassetdeploymentstatus)
                .Select(x => x.Assetdeploymentstatus)
                .ToDictionary(x => x.Deploymentstatusid, x => new DeploymentStatusDto
                {
                    DeploymentStatusId = x.Deploymentstatusid,
                    DeploymentStatusDescription = x.Deploymentstatus,
                    Rule = (int)x.Rule,
                    PlannedActivityResourceAllowedId = DeploymentStatusMapper.GetDeploymentStatusPAResourceAllowed(x).toPlannedActivityResourceKeyList(),
                    ReadOnlyPlannedActivity = x.Readonlyplannedactivity,
                    CheckPlannedActivity = x.Checkplannedactivity
                });

        }

        private DeploymentStatusDto CreateDeploymentStatusDto(Deploymentstatuses data)
        {
            return new DeploymentStatusDto
            {
                DeploymentStatusId = data.Deploymentstatusid,
                DeploymentStatusDescription = data.Deploymentstatus,
                Rule = (int)data.Rule,
                PlannedActivityResourceAllowedId = DeploymentStatusMapper.GetDeploymentStatusPAResourceAllowed(data).toPlannedActivityResourceKeyList(),
                ReadOnlyPlannedActivity = data.Readonlyplannedactivity,
                CheckPlannedActivity = data.Checkplannedactivity
            };
        }
        public async Task<NetworkElementAsPlannedDtoUpdate> GetUpdatePage(long id, List<short> _opcoList, List<int> _verticalList)
        {
            var entity = await Task.Run( () => _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == id, ConstantValueFilter.isTrue)
                .Include(x => x.Networkelementasplannedsubdomainspoc)
                .Include(x => x.Networkelementasplannededuspoc)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .Include(x => x.Plannedactivities)                
                .SingleOrDefault());

            var networkData = NetworkElementAsPlannedMapper.Get(entity);
           
            var dto = _mapper.Map<NetworkElementAsPlannedDtoUpdate>(networkData);
             
            List<ViewBagandComponenetDto> buildBagResources = await _commonManager.GetBagAndComponentDetailsForDropdownAsync(0, true);

            dto.BuildBagResources = buildBagResources;
            dto.BuildBagId = entity.Buildbagid != 0   ? (long )entity.Buildbagid : 0;
            var ActivePAs = entity.Plannedactivities.Where(x => x.Archived != ConstantValueFilter.isTrue);

            var systemNames = _repositoryWrapper.SystemNamesRepository.FindAll();
            dto.SystemNames = systemNames.ToDictionary(x => x.Systemnameid, x => x.Systemnamedescription);
            dto.ElementDomianName = entity.Elementdomianname;
            dto.AssetLiveStatusDate = entity.Assetlivestatusdate;
            dto.DateAssetDecommissionedAsset = entity.Assetdecommissioneddate;

            #region 
            if (ActivePAs?.Any() == true)
            {
                Plannedactivities selectedPA;

                if (ActivePAs.Count() == 1) 
                    selectedPA = ActivePAs.FirstOrDefault();
                
                else
                {
                    var minPlannedCompletion = ActivePAs.Min(x => x.Plannedcompletion);
                    selectedPA = ActivePAs?.FirstOrDefault(x => x.Plannedcompletion == minPlannedCompletion);
                }

                var paType = ActivePAs.Count() == 1 ? PlannedActivityTypeForEnum.EditAsset : PlannedActivityTypeForEnum.AddAsset;
                var settingsPA = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x =>
                        x.Deliverystatusid == selectedPA.Deliverystatusid &&
                        x.Plannedactivityresourceid == selectedPA.Plannedactivityresourceid &&
                        x.Plannedactivitytypefor == (short)paType
                    );
                dto.DeploymentStatusReosurce = GetDeploymentStatusResource(settingsPA); 

               
                if (dto.DeploymentStatusId != 0 && !dto.DeploymentStatusReosurce.ContainsKey(dto.DeploymentStatusId))
                {
                    var data = _repositoryWrapper.DeploymentStatus
                        .FindByCondition(x => x.Deploymentstatusid == dto.DeploymentStatusId, ConstantValueFilter.isTrue)
                        .FirstOrDefault();

                    if (data != null)
                    {
                        dto.DeploymentStatusReosurce.Add(data.Deploymentstatusid, CreateDeploymentStatusDto(data));
                    }
                }
            }
            else
            {
                var allStatuses = _repositoryWrapper.DeploymentStatus.FindAll().ToList();

                dto.DeploymentStatusReosurce = allStatuses.ToDictionary(
                    x => x.Deploymentstatusid,
                    x => CreateDeploymentStatusDto(x)
                );

                if (dto.DeploymentStatusId != 0 && !dto.DeploymentStatusReosurce.ContainsKey(dto.DeploymentStatusId))
                {
                    var data = _repositoryWrapper.DeploymentStatus
                        .FindByCondition(x => x.Deploymentstatusid == dto.DeploymentStatusId, ConstantValueFilter.isTrue)
                        .SingleOrDefault();

                    if (data != null)
                    {
                        dto.DeploymentStatusReosurce.Add(data.Deploymentstatusid, CreateDeploymentStatusDto(data));
                    }
                }
            }


            #endregion


            #region lookUp

            #region    //Ticket 595 -#590 - Vertical Filter to be applied on design component dropdown's in LC, PA, DA etc.,

            dto.DesignComponentReosurce = DesignComponentTypeExtensionMethod.verticalBasedDesignComponentRecord(_verticalList, _repositoryWrapper)
              .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
               .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
               .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
               .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
               .ToDictionary(x => x.Designcomponentid, x => x.toDesignComponentNameLcm(_repositoryWrapper));

            dto.DesignComponentFamilyResource = _repositoryWrapper.DesignComponentFamily.FindAll().ToDictionary(x => x.Designcomponentfamilyid, x => x.DCFName(_repositoryWrapper))
                .Where(f => !f.Value.IsNullOrEmpty()).ToDictionary(k => k.Key, v => v.Value);


            #endregion
            if (!dto.DesignComponentReosurce.ContainsKey(dto.DesignComponentId))
            {
                var data = _repositoryWrapper.DesignComponent.FindByCondition(
                    x => x.Designcomponentid == dto.DesignComponentId,
                    includeDeleted: ConstantValueFilter.isTrue)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer).Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .SingleOrDefault();

                if (data != null)
                {
                    dto.DesignComponentReosurce.Add(data.Designcomponentid, data.toDesignComponentNameLcm(_repositoryWrapper));
                }
            }            
          
            dto.OpCoReosurce = (_opcoList != null && _opcoList.Any() == true) ?
                                 _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco)
                                 : _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            if (!dto.OpCoReosurce.ContainsKey(dto.OpCoId))
            {
                var data = _repositoryWrapper.OpCo.FindByCondition(
                    x => x.Opcoid == dto.OpCoId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.OpCoReosurce.Add(data.Opcoid, data.Opco);
                }
            }
           
            var aspNetUserIdList = (_opcoList != null && _opcoList.Any() == true) ?
                                                                                _repositoryWrapper.UserRepository.FindAllWithDelete(ConstantValueFilter.isTrue)
                                                                                 .Include(y => y.AspnetuseropcosUser)
                                                                                    .Include(y => y.AspnetuserverticalsUser).ThenInclude(y => y.Organisation)
                                                                                    .Where(x =>
                                                                                           x.AspnetuseropcosUser.Any(y => _opcoList.Contains((short)y.Opcoid)) &&
                                                                                           x.AspnetuserverticalsUser.Any(x1 => _verticalList.Contains((int)x1.Organisation.Verticalid))
                                                                                        ).ToList()
                                                                                : _repositoryWrapper.UserRepository.FindAllWithDelete(ConstantValueFilter.isTrue)
                                                                                .ToList();


            dto.SubDomainSpocIds =  entity.Networkelementasplannedsubdomainspoc.Where(x => x.Deleted == false && x.Subdomainspocid != null)
                         .Select(x =>  x.Subdomainspocid).ToList()  ;

            dto.SubDomainSpocResource =
                aspNetUserIdList.Where(x => x.Issubdomainspoc == true && x.Deleted == !ConstantValueFilter.isTrue).DistinctBy(x => x.Id)
                .ToDictionary(x => x.Id, x => x.Email);
            if (dto.SubDomainSpocIds != null && dto.SubDomainSpocIds.Any())
            {
                foreach (var item in dto.SubDomainSpocIds)
                {
                    if (!dto.SubDomainSpocResource.ContainsKey(Convert.ToInt16( item)))
                    {
                        var data = aspNetUserIdList.Where(x => x.Id == item).FirstOrDefault();
                        if (data != null)
                        {
                            dto.SubDomainSpocResource.Add(data.Id,
                                data.Email);
                        }
                    }
                }
            }

            dto.EduSpocIds = entity?.Networkelementasplannededuspoc.Where(x => x.Deleted == false && x.Eduspocid != null)
                        .Select(x => x.Eduspocid).ToList() ;

            dto.EduSpocResource =
                  aspNetUserIdList.Where(x => x.Iseduspoc == ConstantValueFilter.isTrue && x.Deleted == !ConstantValueFilter.isTrue).DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email);
            if (dto.EduSpocIds != null && dto.EduSpocIds.Any())
            {
                foreach (var item in dto.EduSpocIds)
                {
                    if (!dto.EduSpocResource.ContainsKey(Convert.ToInt16(item)))
                    {
                        var data = aspNetUserIdList.Where(x => x.Id == item).FirstOrDefault();
                        if (data != null)
                        {
                            dto.EduSpocResource.Add(data.Id,
                                data.Email);
                        }
                    }
                }
            }
 
            dto.EnvironmentReosurce =
                _repositoryWrapper.Environment.FindAll().ToDictionary(x => (int)x.Environmentid, x => x.Environment);
            if (!dto.EnvironmentReosurce.ContainsKey(dto.EnvironmentId))
            {
                var data = _repositoryWrapper.Environment.FindByCondition(
                    x => x.Environmentid == dto.EnvironmentId, true).SingleOrDefault();
                if (data != null)
                {
                    dto.EnvironmentReosurce.Add(data.Environmentid, data.Environment);
                }
            }
             
            dto.OriginalEquipmentManufacturerResource = _repositoryWrapper.OriginalEquipmentManufacturer.FindAll().ToDictionary(x => x.Orgeqpmanufacturerid,
                    x => x.Originalequipmentmanufacturer);
            if (!dto.OriginalEquipmentManufacturerResource.ContainsKey(dto.OriginalEquipmentManufacturerId))
            {
                var data = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(
                    x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.OriginalEquipmentManufacturerResource.Add(data.Orgeqpmanufacturerid, data.Originalequipmentmanufacturer);
                }
            }
           
            dto.DeploymentTypeReosurce = _repositoryWrapper.DeploymentType.FindAll().ToDictionary(x => (int)x.Deploymenttypeid, x => x.Deploymenttype);
 

            var plannedResource = _repositoryWrapper.PlannedActivityResourceRepository
                        .FindAll()
                        .Include(x => x.Plannedactivityresourceplanningrisk)
                        .Include(x => x.Plannedactivityresourcebenefit)
                        .Include(x => x.Plannedactivityresourcedriver)
                        ;

            var paresourceEntity = new PlannedActivityResource();
            var getSelectedDrivers = _repositoryWrapper.PlannedActivityResourceDriver.FindAll().ToList();
            var getSelectedBenefits = _repositoryWrapper.PlannedActivityResourceBenefit.FindAll().ToList();
            var getSelectedPlanningRisks = _repositoryWrapper.PlannedActivityResourcePlanningRisk.FindAll().ToList();


            #region
            //var plannedActivityResource = plannedResource.ToDictionary(x => x.Plannedactivityresourceid, x =>
            //new PlannedActivityResourceDto()
            //{
            //    PlannedActivityResourceDescription = x.Plannedactivityresource,
              
            //    LastModified = x.Modificationdate,
            //    RuleActicvityDetails = x.Ruleacticvitydetails,
            //    RuleLinkedDc = x.Rulelinkeddc,
            //    Exportable = x.Exportable,
            //    LcmLabelHardware = x.Lcmlabelhardware,
            //    LcmLabelSoftware = x.Lcmlabelsoftware,
            //    LcmHardware = x.Lcmhardware,
            //    LcmSoftware = x.Lcmsoftware,
            //    AddAssetHardware = x.Addassethardware,
            //    AddAssetLabelHardware = x.Addassetlabelhardware,
            //    AddAssetSoftware = x.Addassetsoftware,
            //    AddAssetLabelSoftware = x.Addassetlabelsoftware,
            //    EditAssetHardware = x.Editassethardware,
            //    EditAssetLabelHardware = x.Editassetlabelhardware,
            //    EditAssetSoftware = x.Editassetsoftware,
            //    EditAssetLabelSoftware = x.Editassetlabelsoftware,

            //    ForLcm = x.Forlcm,
            //    RuleAddAsset = x.Ruleaddasset,
            //    RuleEditAsset = x.Ruleeditasset,
            //    ForAddAsset = x.Foraddasset,
            //    ForEditAsset = x.Foreditasset,

            //    DriverTextAddAsset = getSelectedDrivers.Where(x => x.Forlcm == false && x.Fordesignaspect == false
            //             && x.Foraddasset == true && x.Foreditasset == false).Select(x => x.Driverid).ToList(),
            //    BenefitTextAddAsset = getSelectedBenefits.Where(x => x.Forlcm == false && x.Fordesignaspect == false
            //    && x.Foraddasset == true && x.Foreditasset == false).Select(x => x.Benefitid).ToList(),
            //    PlanningRisksAddAsset = getSelectedPlanningRisks.Where(x => x.Forlcm == false && x.Fordesignaspect == false
            //    && x.Foraddasset == true && x.Foreditasset == false).Select(x => x.Planningriskid).ToList(),


            //    DriverTextEditAsset = getSelectedDrivers.Where(x => x.Forlcm == false && x.Fordesignaspect == false
            //    && x.Foraddasset == false && x.Foreditasset == true).Select(x => x.Driverid).ToList(),
            //    BenefitTextEditAsset = getSelectedBenefits.Where(x => x.Forlcm == false && x.Fordesignaspect == false
            //    && x.Foraddasset == false && x.Foreditasset == true).Select(x => x.Benefitid).ToList(),
            //    PlanningRisksAEditAsset = getSelectedPlanningRisks.Where(x => x.Forlcm == false && x.Fordesignaspect == false
            //    && x.Foraddasset == false && x.Foreditasset == true).Select(x => x.Planningriskid).ToList(),


            //    DriverTextLcm = getSelectedDrivers.Where(x => x.Forlcm == true && x.Fordesignaspect == false
            //    && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Driverid).ToList(),
            //    BenefitTextLcm = getSelectedBenefits.Where(x => x.Forlcm == true && x.Fordesignaspect == false
            //    && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Benefitid).ToList(),
            //    PlanningRisksLcm = getSelectedPlanningRisks.Where(x => x.Forlcm == true && x.Fordesignaspect == false
            //    && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Planningriskid).ToList(),

            //    DriverTextDesignAspect = getSelectedDrivers.Where(x => x.Forlcm == false && x.Fordesignaspect == true
            //     && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Driverid).ToList(),
            //    BenefitTextDesignAspect = getSelectedBenefits.Where(x => x.Forlcm == false && x.Fordesignaspect == true
            //    && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Benefitid).ToList(),
            //    PlanningRisksDesignAspect = getSelectedPlanningRisks.Where(x => x.Forlcm == false && x.Fordesignaspect == true
            //    && x.Foraddasset == false && x.Foreditasset == false).Select(x => x.Planningriskid).ToList(),

            //    ForCreateAddAsset = x.Forcreateaddasset,
            //    ForCreateEditAsset = x.Forcreateeditasset,

            //    ForEditAddAsset = x.Foreditaddasset,
            //    ForEditEditAsset = x.Forediteditasset,

            //    RuleActicvityDetailsAddAsset = x.Ruleactdetailsaddasset,
            //    RuleActicvityDetailsEditAsset = x.Ruleactdetailseditasset,
            //    //JsonFormResource = x.JsonForm,
            //    JsonForm = x.Jsonform,
            //    PlannedActivityResourceId = x.Plannedactivityresourceid,
            //    ActivityDetailsAddAsset = x.Activitydetailsaddasset,
            //    ActivityDetailsEditAsset = x.Activitydetailseditasset,

            //    ActivityDetailsLcm = x.Activitydetailslcm,
            //    ActivityDetailsForVirtualizedAddAsset = x.Actdetailsforvrtaddasset,
            //    ActivityDetailsForVirtualizedEditAsset = x.Actdetailsforvrteditasset,
               

            //});
            #endregion
            dto.NfviBundleIDReosurce =
                _repositoryWrapper.NFVIBundleID.FindAll().ToDictionary(x => (int)x.Nfvibundleidid, x => x.Nfvibundleid);
            if (dto.NfviBundleIDId != null && !dto.NfviBundleIDReosurce.ContainsKey((int)dto.NfviBundleIDId))
            {
                var data = _repositoryWrapper.NFVIBundleID.FindByCondition(
                    x => x.Nfvibundleidid == dto.NfviBundleIDId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.NfviBundleIDReosurce.Add(data.Nfvibundleidid, data.Nfvibundleid);
                }
            }
          
            dto.LocationReosurce = _repositoryWrapper.Location.FindAll().ToDictionary(x => (int)x.Locationid, x => _mapper.Map<LocationDto>(LocationMapper.GetLocationMapper(x)));

            if (dto.LocationId != null && !dto.LocationReosurce.ContainsKey((int)dto.LocationId))
            {
                var data = _repositoryWrapper.Location.FindByCondition(
                    x => x.Locationid == dto.LocationId, ConstantValueFilter.isTrue).SingleOrDefault();
                if (data != null)
                {
                    dto.LocationReosurce.Add(data.Locationid, _mapper.Map<LocationDto>(LocationMapper.GetLocationMapper(data)));
                }
            }
        
            dto.LinkedToNetworkAsIs = _repositoryWrapper.NetworkElementAsIs.FindByCondition(p => p.Networkelementasplannedid == dto.NetworkElementAsPlannedId).Any();
            #endregion

           #region PlannedActivityDto
            var paIds = ActivePAs?.Select(x => x.Plannedactivityid).ToList();
            dto.PlannedActivityDto = new List<PlannedActivityDtoUpdate>();
            foreach (var planned in paIds)
            {
                dto.PlannedActivityDto.Add(await _plannedActivityManager.GetUpdatePage(planned, _opcoList, _verticalList));
            }
             #endregion
            return dto;
        }

        public static ExpressionStarter<Networkelementsasplanned> ApplyFilter(NetworkElementAsPlannedQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);
            var predicateInner = PredicateBuilder.New<Networkelementsasplanned>(true);

            if (buildFilterDto.NodeIndex != null && buildFilterDto.NodeIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.NodeIndex)
                    predicateInner.Or(x => x.Networkelementasplannedid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.OpCo)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Environment != null && buildFilterDto.Environment.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.Environment)
                    predicateInner.Or(x => x.Environmentid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VodafoneName != null && buildFilterDto.VodafoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.VodafoneName)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Vodafonenameid != null && x.Designcomponent.Subnetworkboundary.Vodafonenameid == item);
                predicateResult.And(predicateInner);
            }
          
            if (buildFilterDto?.DesignComponentIndex != null && buildFilterDto.DesignComponentIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.DesignComponentIndex)
                    predicateInner.Or(x => x.Designcomponentid.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.DesignComponentFamilyIndex != null && buildFilterDto.DesignComponentFamilyIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.DesignComponentFamilyIndex)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamilyid.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DeploymentStatus != null && buildFilterDto.DeploymentStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.DeploymentStatus)
                    predicateInner.Or(x => x.Deploymentstatusid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DeploymentType != null && buildFilterDto.DeploymentType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.DeploymentType)
                    predicateInner.Or(x => x.Deploymenttypeid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Location != null && buildFilterDto.Location.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.Location)
                    predicateInner.Or(x => x.Locationid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NfviBundleID != null && buildFilterDto.NfviBundleID.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.NfviBundleID)
                    predicateInner.Or(x => x.Nfvibundleidid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignComponent != null && buildFilterDto.DesignComponent.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.DesignComponent)
                    predicateInner.Or(x => x.Designcomponentid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.PlannedActivity != null && buildFilterDto.PlannedActivity.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.PlannedActivity)
                    predicateInner.Or(x => x.Plannedactivities.Any(s => s.Plannedactivityid == item));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);

                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);

                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ElementName != null && buildFilterDto.ElementName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.ElementName)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedAction != null && buildFilterDto.PlannedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.PlannedAction)
                    if (item == true)
                        predicateInner.Or(x => x.Plannedactivities.Count > 0 && x.Plannedactivities.Any(x => x.Archived != ConstantValueFilter.isTrue));
                    else
                        predicateInner.Or(x => x.Plannedactivities.Count == 0 /*&& x.Plannedactivities.Any(x=>x.Archived == true)*/);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AutomatedFeedback != null && buildFilterDto.AutomatedFeedback.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.AutomatedFeedback)
                    predicateInner.Or(x => x.Automatedfeedback == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NetworkConstruct != null && buildFilterDto.NetworkConstruct.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.NetworkConstruct)
                    predicateInner.Or(x => x.Networkconstruct == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.CapacityPlanReference != null && buildFilterDto.CapacityPlanReference.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.CapacityPlanReference)
                    predicateInner.Or(x => x.Capacityplanreference == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.AdditionalInformation1 != null && buildFilterDto.AdditionalInformation1.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.AdditionalInformation1)
                    predicateInner.Or(x => x.Additionalinformation1 == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.AdditionalInformation2 != null && buildFilterDto.AdditionalInformation2.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.AdditionalInformation2)
                    predicateInner.Or(x => x.Additionalinformation2 == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HwResourceKey != null && buildFilterDto.HwResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.HwResourceKey)
                    predicateInner.Or(x => x.Hwresourcekey == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PreviousHWResourceKey != null && buildFilterDto.PreviousHWResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.PreviousHWResourceKey)
                    predicateInner.Or(x => x.Previoushwresourcekey == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SwResourceKey != null && buildFilterDto.SwResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.SwResourceKey)
                    predicateInner.Or(x => x.Swresourcekey == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PreviousSWResourceKey != null && buildFilterDto.PreviousSWResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.PreviousSWResourceKey)
                    predicateInner.Or(x => x.Previousswresourcekey == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubDomainSpoc != null && buildFilterDto.SubDomainSpoc.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.SubDomainSpoc)
                    if (item == "yes")
                    { 
                        predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc == null || !x.Networkelementasplannedsubdomainspoc.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(d => d.Subdomainspocid.ToString() == item));
                    }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Eduspoc != null && buildFilterDto.Eduspoc.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.Eduspoc)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => x.Networkelementasplannededuspoc == null || !x.Networkelementasplannededuspoc.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Networkelementasplannededuspoc.Any(d => d.Eduspocid.ToString() == item));
                    }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                int count = 0;
                foreach (var item in buildFilterDto.VerticalName)
                    if (item == "yes")
                    {
                        count++;
                        predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc == null || !x.Networkelementasplannedsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;
                        predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser
                    .Any(m => m.Organisation.Vertical.Verticalresponsibleid.ToString() == item && m.Deleted == false /*&& m.Opcoid == x.Opcoid*/)));
                    }
                if (count > 0) predicateResult.And(predicateInner);
            }

            #region      //Ticket 1212 - Asset Pivot Tabel 
            if (buildFilterDto.PaImplementaionYear?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.PaImplementaionYear)
                {
                    if (item == "")
                        predicateInner.Or(x => x.Lcmengineeringid == null ||
                                                     x.Lcmengineeringid != null && !x.Lcmengineering.PlannedactivitiesLcmengineering.Any());
                    else
                        predicateInner.Or(x => x.Lcmengineeringid != null && x.Lcmengineering.PlannedactivitiesLcmengineering
  .Any(d => d.Plannedimplementationyear.ToString() == item));
                }

                predicateResult.And(predicateInner);
            }
            #region - DCF Implementation
            if (buildFilterDto.isDcfImplementation != null && buildFilterDto.isDcfImplementation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();

                foreach (var item in buildFilterDto.isDcfImplementation)
                {
                    if (item == true)
                        predicateInner.Or(x => x.Designcomponent.Lcmengineering.Any(p => p.Numberofnodes > 0));
                    else
                        predicateInner.Or(x => x.Designcomponent.Lcmengineering.All(p => p.Numberofnodes == 0) ||
                        (!x.Designcomponent.Lcmengineering.Any()));
                }


                predicateResult.And(predicateInner);
            }
            #endregion

            if (buildFilterDto.HardwareType != null && buildFilterDto.HardwareType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.HardwareType)
                    if (item == "1") //Virtual Records
                    {
                        predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .Any(n => n.Ismain && (ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction))));
                    }
                    else /// Non Virtual Records
                    {
                        predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                   .Any(n => n.Ismain && !(ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction))));
                    }
                predicateResult.And(predicateInner);
            }
            #endregion

            if (buildFilterDto.BuildBagDescription?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var description in buildFilterDto.BuildBagDescription)
                    descriptionPredicate.Or(x => x.Buildbagid == description);

                predicateResult.And(descriptionPredicate);
            }

            if (buildFilterDto.Assured?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var description in buildFilterDto.Assured)                   
                    descriptionPredicate.Or(x => x.Isassured == description);                    
                
                predicateResult.And(descriptionPredicate);
            }

            if (buildFilterDto.ElementDomianName != null && buildFilterDto.ElementDomianName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.ElementDomianName)
                    predicateInner.Or(x => x.Elementdomianname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetLiveStatusDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.AssetLiveStatusDateValue.StartDate != null)
                    predicateInner.And(x => x.Assetlivestatusdate.Value >= buildFilterDto.AssetLiveStatusDateValue.StartDate);

                if (buildFilterDto.AssetLiveStatusDateValue.EndDate != null)
                    predicateInner.And(x => x.Assetlivestatusdate.Value <= buildFilterDto.AssetLiveStatusDateValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DateAssetDecommissionedAssetValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.DateAssetDecommissionedAssetValue.StartDate != null)
                    predicateInner.And(x => x.Assetdecommissioneddate.Value >= buildFilterDto.DateAssetDecommissionedAssetValue.StartDate);

                if (buildFilterDto.DateAssetDecommissionedAssetValue.EndDate != null)
                    predicateInner.And(x => x.Assetdecommissioneddate.Value <= buildFilterDto.DateAssetDecommissionedAssetValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.IsVirtualizedOrContanarized != null && buildFilterDto.IsVirtualizedOrContanarized.Count>0)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.IsVirtualizedOrContanarized)
                {
                    if(item == true)
                    {
                        predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(r => r.Ismain == true)
                        .Any(t => t.Majorhardware.Buildconstruction.Iscloudasset == true));
                    }
                    else
                    {
                        predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                         .Any(t => t.Majorhardware.Buildconstruction.Iscloudasset == false));
                    }
                }
                   
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public async Task<QueryResultDto<NetworkElementAsPlannedDtoGrid>> FindWithCondition(NetworkElementAsPlannedQueryDto designComponentFilterDto)
        {
            var predicateResult = ApplyFilter(designComponentFilterDto);
            if (designComponentFilterDto.Deleted == ConstantValueFilter.isTrue)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
                  
            var rtn = new QueryResultDto<NetworkElementAsPlannedDtoGrid>(new GenerateRenderForGrid<NetworkElementAsPlannedDtoGrid>(_manager))
            {
            };

            var query = await GetQuery(predicateResult, designComponentFilterDto.Deleted ?? false);
            var data =  query.ApplyOrdering(designComponentFilterDto, GetColumnsMap()).ApplyPaging(designComponentFilterDto).ToList();

            IEnumerable<NetworkElementAsPlannedDtoGrid> lcmEngineeringdResult;
            lcmEngineeringdResult = _mapper.Map<IEnumerable<NetworkElementAsPlannedDtoGrid>>(data);
            rtn.Items = lcmEngineeringdResult.ToArray();
            rtn.TotalItems = query.Count();
            return rtn;
        }

        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, NetworkElementAsPlannedQueryDto buildFilterDto,bool isAdmin)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
            
            var query = await GetQuery(predicateResult, !ConstantValueFilter.isTrue);
 
            var majorhardwareList = (propertyName == "isVirtualizedOrContanarized") ? query?.ToList()?.SelectMany(x => x?.DesignComponent?.SystemType?.SystemTypesMajorHardwareBuilds
            .Where(t => t.IsMain == true)
            .Select(y =>
             y.MajorHardware.BuildConstruction.IsCluodHostedAsset)).ToList() : new List<bool?>();
             
            var rtn = propertyName switch
            {
                "nodeIndex" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = $"TEMS{p.NetworkElementAsPlannedId:000000}", Value = p.NetworkElementAsPlannedId.ToString() }).Distinct().ToList()
                : query.ToList()
                    .Where(x => $"TEMS{x.NetworkElementAsPlannedId:000000}".Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = $"TEMS{p.NetworkElementAsPlannedId:000000}", Value = p.NetworkElementAsPlannedId.ToString() }).Distinct()
                    .ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "vodafoneName" => string.IsNullOrEmpty(propertyFilter)
                          ? query.Select(p => new FilterValueDto
                          {
                              Text = p.DesignComponent.SubNetworkBoundary.VodafoneName != null ? p.DesignComponent.SubNetworkBoundary.VodafoneName.Description : "",
                              Value = p.DesignComponent.SubNetworkBoundary.VodafoneName != null ? p.DesignComponent.SubNetworkBoundary.VodafoneNameId.ToString() : ""
                          }).Distinct().ToList()
                          : query
                          .Where(x =>
                          x.DesignComponent.SubNetworkBoundary.VodafoneName.Description.Contains(
                          propertyFilter)).Select(p => new FilterValueDto
                          {
                              Text = p.DesignComponent.SubNetworkBoundary.VodafoneName != null ? p.DesignComponent.SubNetworkBoundary.VodafoneName.Description : "",
                              Value = p.DesignComponent.SubNetworkBoundary.VodafoneName != null ? p.DesignComponent.SubNetworkBoundary.VodafoneNameId.ToString() : ""
                          }).Distinct().ToList(),
                "elementName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto
                    { Text = p.ElementName, Value = p.ElementName }).Distinct().ToList()
                    : query.ToList()
                        .Where(x => x.ElementName.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.ElementName, Value = p.ElementName }).Distinct()
                        .ToList(),

                "designComponentIndex" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.DesignComponentId.ToString(), Value = p.DesignComponentId.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.DesignComponentId.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.DesignComponentId.ToString(), Value = p.DesignComponentId.ToString() }).Distinct()
                    .ToList(),
                "designComponentFamilyIndex" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.DesignComponentFamilyId.ToString(), Value = p.DesignComponentFamilyId.ToString() }).Distinct().ToList()
                : query
                .Where(x => x.DesignComponentFamilyId.ToString().Contains(propertyFilter)).Select(p =>
                    new FilterValueDto { Text = p.DesignComponentFamilyId.ToString(), Value = p.DesignComponentFamilyId.ToString() }).Distinct()
                .ToList(),
                "networkConstruct" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.NetworkConstruct, Value = p.NetworkConstruct }).Distinct().ToList()
                    : query
                        .Where(x => x.NetworkConstruct.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.NetworkConstruct, Value = p.NetworkConstruct }).Distinct()
                        .ToList(),
                "capacityPlanReference" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.CapacityPlanReference, Value = p.CapacityPlanReference }).Distinct().ToList()
                : query
                    .Where(x => x.CapacityPlanReference.Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.CapacityPlanReference, Value = p.CapacityPlanReference }).Distinct()
                    .ToList(),

                "additionalInformation1" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.AdditionalInformation1, Value = p.AdditionalInformation1 }).Distinct().ToList()
                : query
                    .Where(x => x.AdditionalInformation1.Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.AdditionalInformation1, Value = p.AdditionalInformation1 }).Distinct()
                    .ToList(),

                "additionalInformation2" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.AdditionalInformation2, Value = p.AdditionalInformation2 }).Distinct().ToList()
                : query
                    .Where(x => x.AdditionalInformation2.Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.AdditionalInformation2, Value = p.AdditionalInformation2 }).Distinct()
                    .ToList(),

                "designComponent" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto
                    {
                        Text = DesignComponentMapper.SetDesignComponentMapper(p.DesignComponent).toDesignComponentNameLcm(_repositoryWrapper),
                        Value = p.DesignComponentId.ToString()
                    }).Distinct().ToList()
                    : query.ToList()
                        .Where(x =>
                                DesignComponentMapper.SetDesignComponentMapper(x.DesignComponent).toDesignComponentNameLcm(_repositoryWrapper).ToUpper().Contains(
                                propertyFilter.ToUpper())).Select(p => new FilterValueDto
                                {
                                    Text = DesignComponentMapper.SetDesignComponentMapper(p.DesignComponent).toDesignComponentNameLcm(_repositoryWrapper),
                                    Value = p.DesignComponentId.ToString()
                                }).Distinct().ToList(),
                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.OpCo.OpCoDescription,
                        Value = p.OpCoId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.OpCo.OpCoDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.OpCo.OpCoDescription,
                                    Value = p.OpCoId.ToString()
                                }).Distinct().ToList(),

                "location" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Location.LocationDescription,
                        Value = p.LocationId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Location.LocationDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Location.LocationDescription,
                                    Value = p.LocationId.ToString()
                                }).Distinct().ToList(),
                "nfviBundleID" => query.Where(x => x.NFVIBundleID != null).Select(p => new FilterValueDto
                {
                    Text = p.NFVIBundleID.NFVIBundleIdDescription,
                    Value = p.NfviBundleIDId.ToString()
                }).Distinct().ToList(),
                "environment" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Environment.EnvironmentDescription,
                        Value = p.EnvironmentId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Environment.EnvironmentDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Environment.EnvironmentDescription,
                                    Value = p.EnvironmentId.ToString()
                                }).Distinct().ToList(),

                "deploymentStatus" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.DeploymentStatus.DeploymentStatusDescription,
                        Value = p.DeploymentStatusId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.DeploymentStatus.DeploymentStatusDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.DeploymentStatus.DeploymentStatusDescription,
                                    Value = p.DeploymentStatusId.ToString()
                                }).Distinct().ToList(),
                "deploymentType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(p => p.DeploymentType != null).Select(p => new FilterValueDto
                    {
                        Text = p.DeploymentType.DeploymentTypeDescription,
                        Value = p.DeploymentTypeId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x => x.DeploymentType != null && x.DeploymentType.DeploymentTypeDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.DeploymentType.DeploymentTypeDescription,
                                    Value = p.DeploymentTypeId.ToString()
                                }).Distinct().ToList(),
                "plannedAction" =>
                                 string.IsNullOrEmpty(propertyFilter)
                                   ? query
                                       .Select(p => new FilterValueDto { Text = p.PlannedAction ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(), Value = p.PlannedAction.ToString() }).Distinct().ToList()
                                   : query
                                       .Where(p => (p.PlannedAction ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                                       .Select(p => new FilterValueDto { Text = p.PlannedAction ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(), Value = p.PlannedAction.ToString() }).Distinct().ToList(),
                "automatedFeedback" =>
                 string.IsNullOrEmpty(propertyFilter)
                   ? query
                       .Select(p => new FilterValueDto { Text = p.AutomatedFeedback ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(), Value = p.AutomatedFeedback.ToString() }).Distinct().ToList()
                   : query
                       .Where(p => (p.AutomatedFeedback ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                       .Select(p => new FilterValueDto { Text = p.AutomatedFeedback ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(), Value = p.AutomatedFeedback.ToString() }).Distinct().ToList(),


                "plannedActivity" => query.ToList()
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PlannedActivityDictonary.ToString().Contains(propertyFilter))
                .SelectMany(q => q.PlannedActivityDictonary.Select(x => new FilterValueDto
                    {
                        Value = x.Key.ToString(),
                        Text = x.Value
                    }).ToList()).Distinct().ToList(),                    
                "hwResourceKey" => string.IsNullOrEmpty(propertyFilter)
         ? query.Where(x => x.HwResourceKey != null).Select(p => new FilterValueDto
         {
             Text = p.HwResourceKey,
             Value = p.HwResourceKey
         }).Distinct().ToList()
        : query
          .Where(x => x.HwResourceKey != null && x.HwResourceKey.Contains(propertyFilter)).Select(p => new FilterValueDto
          {
              Text = p.HwResourceKey,
              Value = p.HwResourceKey
          }).ToList().Distinct().ToList(),
                "previousHWResourceKey" => string.IsNullOrEmpty(propertyFilter)
               ? query.Where(x => x.PreviousHWResourceKey != null).Select(p => new FilterValueDto
               {
                   Text = p.PreviousHWResourceKey,
                   Value = p.PreviousHWResourceKey
               }).Distinct().ToList()
              : query
                .Where(x => x.PreviousHWResourceKey != null && x.PreviousHWResourceKey.Contains(propertyFilter)).Select(p => new FilterValueDto
                {
                    Text = p.PreviousHWResourceKey,
                    Value = p.PreviousHWResourceKey
                }).Distinct().ToList(),
                "swResourceKey" => string.IsNullOrEmpty(propertyFilter)
                 ? query.Where(x => x.SwResourceKey != null).Select(p => new FilterValueDto
                 {
                     Text = p.SwResourceKey,
                     Value = p.SwResourceKey
                 }).Distinct().ToList()
                : query
                  .Where(x => x.SwResourceKey != null && x.SwResourceKey.Contains(propertyFilter)).Select(p => new FilterValueDto
                  {
                      Text = p.SwResourceKey,
                      Value = p.SwResourceKey
                  }).ToList().Distinct().ToList(),
                "previousSWResourceKey" => string.IsNullOrEmpty(propertyFilter)
               ? query.Where(x => x.PreviousSWResourceKey != null).Select(p => new FilterValueDto
               {
                   Text = p.PreviousSWResourceKey,
                   Value = p.PreviousSWResourceKey
               }).Distinct().ToList()
              : query
                .Where(x => x.PreviousSWResourceKey != null && x.PreviousSWResourceKey.Contains(propertyFilter)).Select(p => new FilterValueDto
                {
                    Text = p.PreviousSWResourceKey,
                    Value = p.PreviousSWResourceKey
                }).Distinct().ToList(),


                "eduspoc" => (string.IsNullOrEmpty(propertyFilter)
                         ? _commonManager.GetDesignContactUsersFilterDto(query.SelectMany(p => p.NetworkElementEduSpocIdList)?.Distinct()?.ToList(), true, false)
                         : _commonManager.GetDesignContactUsersFilterDto(query.SelectMany(p => p.NetworkElementEduSpocIdList)?.Distinct()?.ToList(), true, false)
                         .Where(y => y.Text.Contains(propertyFilter)))?.Distinct()?.ToList(),

                "subDomainSpoc" => (string.IsNullOrEmpty(propertyFilter)
    ? _commonManager.GetDesignContactUsersFilterDto(query.Where(t => t.NetworkElementAsPlannedSubDomainSpoc.Any()).SelectMany(p => p.NetworkElementAsPlannedSubDomainSpoc.Select(y => y.Subdomainspocid))?.Distinct()?.ToList(), false, true)
    : _commonManager.GetDesignContactUsersFilterDto(query.Where(t => t.NetworkElementAsPlannedSubDomainSpoc.Any()).SelectMany(p => p.NetworkElementAsPlannedSubDomainSpoc.Select(y => y.Subdomainspocid))?.Distinct()?.ToList(), false, true)
    .Where(y => y.Text.Contains(propertyFilter)))?.Distinct()?.ToList(),
                "verticalName" => string.IsNullOrEmpty(propertyFilter)
                                                     ? query.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                                     .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                                                     {
                                                         Text = t.Value,
                                                         Value = t.Key.ToString()
                                                     }))?.Distinct()?.ToList()
                                                     .Concat(query.Where(x => x.NetworkElementAsPlannedSubDomainSpoc != null && x.NetworkElementAsPlannedSubDomainSpoc.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList()
                                                     :
                     query.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                                     .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                                                     {
                                                         Text = t.Value,
                                                         Value = t.Key.ToString()
                                                     })).Where(x => x.Text.Contains(propertyFilter))?.Distinct()?.ToList()
                                                     .Concat(query.Where(x => x.NetworkElementAsPlannedSubDomainSpoc != null && x.NetworkElementAsPlannedSubDomainSpoc.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),

                "buildBagDescription" =>   query
                   .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Buildbag.BagDescription.Contains(propertyFilter))
                   .Select(p => new FilterValueDto { Text = _commonManager.GetBuildBagDescriptionFromEnity(p.Buildbag), Value = p.Buildbagid .ToString()})
                   .Distinct()
                   .ToList (),

                "assured" => query.ToList().Where(x=> string.IsNullOrEmpty(propertyFilter) || x.IsAssured.ToString().Contains(propertyFilter))                                      
                        .Select(p => new FilterValueDto { Text = p.IsAssured == true ? ConstantValueFilter.Assured 
                        : ConstantValueFilter.NotAssured, Value = p.IsAssured.ToString() }).Distinct().ToList(),

                "isVirtualizedOrContanarized" => majorhardwareList?.ToList().Where(x => string.IsNullOrEmpty(propertyFilter) || x.ToString().Contains(propertyFilter))
               .Select(p => new FilterValueDto
               {
                   Text = p == true ? ConstantValueFilter.YES
               : ConstantValueFilter.NO,
                   Value = p.ToString()
               }).Distinct().ToList(),

                "elementDomianName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.ElementDomianName,
                        Value = p.ElementDomianName
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ElementDomianName.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.ElementDomianName,
                                    Value = p.ElementDomianName
                                }).Distinct().ToList(),

                _ => new List<FilterValueDto>()
            };

            if (!isAdmin && (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();
            }

            return rtn;
        }

        public async Task<IQueryable<Networkelementsasplanned>> GetQueryRecords(ExpressionStarter<Networkelementsasplanned> predicateResult, bool includeDeleted, bool isPivotReprt = false)
        {
            var query = await Task.Run(() => _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult, includeDeleted).AsNoTracking()
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                     .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                    .ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                    .ThenInclude(x => x.Productname)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                    .ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Lcmengineering)
                    .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Vodafonename)
                    .Include(x => x.Environment)
                    .Include(x => x.Deploymentstatus)
                    .Include(x => x.Location)
                    .Include(x => x.Opco)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Networkelementasplannedsubdomainspoc).ThenInclude(x=>x.Subdomainspoc).ThenInclude(x=>x.AspnetuserverticalsUser).ThenInclude(x=>x.Organisation).ThenInclude(x=>x.Vertical)
                    .Include(x => x.Buildbag).AsQueryable());

            if (!isPivotReprt)
            {
                query = query
                    .Include(x => x.Networkelementasplannededuspoc)
                    
                    .Include(x => x.Plannedactivities).ThenInclude(x => x.Activitystatus)
                    .Include(x => x.Plannedactivities).ThenInclude(x => x.Planningactivitystatus)
                    .Include(x => x.Plannedactivities).ThenInclude(x => x.Plannedactivityresource)
                    .Include(x => x.Plannedactivities).ThenInclude(x => x.Deliverystatus)
                    .Include(x => x.Nfvibundleid)
                    .Include(x => x.Deploymenttype)
                    .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmdeploymentstatus)
                    ;
            }
            //else
            //{
            //    query = query
            //        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
            //        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction);
            //}
           
            return query;
        }
        public async Task<IQueryable<NetworkElementAsPlanned>> GetQuery(ExpressionStarter<Networkelementsasplanned> predicateResult, bool includeDeleted)
        {
            var query = await GetQueryRecords(predicateResult, includeDeleted);

            var data = query.AsEnumerable().Select(x => NetworkElementAsPlannedMapper.Get(x)).ToList();

            foreach (var item in data)
            {
                if (item.NetworkElementAsPlannedSubDomainSpoc?.Any() == true)
                    item.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(item?.NetworkElementAsPlannedSubDomainSpoc?.Select(x => x?.Subdomainspocid).ToList(),
                        item?.OpCoId, false, true)?.Distinct()?.ToDictionary(m => Convert.ToInt16(m.Value), m => m.Text);

            }

            return data.AsQueryable();
        }

        private Dictionary<string, Expression<Func<NetworkElementAsPlanned, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<NetworkElementAsPlanned, object>>[]>
            {
                ["nodeIndex"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.NetworkElementAsPlannedId },
                ["designComponentIndex"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.DesignComponent.DesignComponentId },
                ["designComponent"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.GetDesignComponentDescriptionOrAlias(p) },
                ["opCo"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.OpCo.OpCoDescription },
                ["designComponentFamilyIndex"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.DesignComponentFamilyId },
                ["plannedActivity"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { x => x.GetPlannedImplementationYear(x), x => x.GetActivityStatusDescription(x), x => x.GetPlanningActivityStatusDescription(x) },
                ["lastModified"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.ModificationDate },
                 ["lastModifiedValue"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.ModificationUserEntity.Email },
                ["networkElementAsPlannedId"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.NetworkElementAsPlannedId },
                ["elementName"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.ElementName },
                ["capacityPlanReference"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.CapacityPlanReference },
                ["additionalInformation1"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.AdditionalInformation1 },
                ["additionalInformation2"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.AdditionalInformation2 },
                ["networkConstruct"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.NetworkConstruct },
                ["deploymentType"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.GetDeploymentTypeDescription(p) },
                ["deploymentStatus"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.DeploymentStatus.DeploymentStatusDescription },
                ["environment"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.Environment.EnvironmentDescription },
                ["location"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.Location.LocationDescription },
                ["nfviBundleID"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.GetNFVIBundleIdDescription(p) },
                ["hwResourceKey"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.HwResourceKey },
                ["previousHWResourceKey"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.PreviousHWResourceKey },
                ["swResourceKey"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.SwResourceKey },
                ["previousSWResourceKey"] = new Expression<Func<NetworkElementAsPlanned, object>>[] { p => p.PreviousSWResourceKey },

            };
        }

        private async Task<Networkelementsasplanned> UpdateFromVNFTransition(Networkelementsasplanned entity)
        {
            var dc = await _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                .FirstOrDefaultAsync();

            if (dc != null)
            {
                var virtualized = dc.Systemtype?.Systemtypesmajorhardwarebuilds?
                    .SingleOrDefault(m =>
                                         m.Ismain &&
                                         m.Systemtypeid == dc.Systemtype?.Systemtypeid &&
                                         m.Deleted == !ConstantValueFilter.isTrue
                    )?.Majorhardware?.Buildconstruction?.Rule == 3 ? ConstantValueFilter.isTrue : !ConstantValueFilter.isTrue;
                if (virtualized)
                {
                    var vnfTransition = await _repositoryWrapper.VNFTransition.FindByCondition(x => x.Opcoid == entity.Opcoid && x.Elementname == entity.Elementname).FirstOrDefaultAsync();
                    if (vnfTransition != null)
                    {
                        entity.Nfvibundleidid = vnfTransition.Nfvibundleidid;
                    }
                }
            }
            return entity;
        }


        public async Task<ResultDto> GetDesignComponentList(short? oemId)
        {
            if (oemId != null)
            {
                var allDC = await _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid == oemId)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .ToListAsync();

                var x = allDC.OrderBy(p => p.Designcomponentid).Distinct();
                var dcNameList = x.ToDictionary(x => x.Designcomponentid, x => x.toDesignComponentNameLcm(_repositoryWrapper));

                if (dcNameList.Any())
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.GetInfoSuccess,
                        Warning = false,
                        Data = dcNameList

                    };
                }
                else
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.NoDesignComponent,
                        Warning = false,
                        Data = dcNameList

                    };
                }
            }
            else
            {
                var designResource = _repositoryWrapper.DesignComponent.FindAll();
                var dcNameList = designResource
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .ToDictionary(x => x.Designcomponentid, x => x.toDesignComponentNameLcm(_repositoryWrapper));

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = dcNameList

                };

            }

        }

        public async Task<ResultDto> GetAssetDeploymentStatusRelatedDeliveryStatusAndPAResource(short plannedActivityResourceId, short deliveryStatusId, bool isAddAsset)
        {
            // IDictionary<short, string> AssetdeploymentstatusDict = new Dictionary<short, string>();
            var AssetdeploymentstatusDict = _repositoryWrapper.SettingsUpdatePlannedActivity
                .FindByCondition(x => x.Plannedactivityresourceid == plannedActivityResourceId
                && x.Deliverystatusid == deliveryStatusId && (isAddAsset == ConstantValueFilter.isTrue ? x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.AddAsset : x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.EditAsset)
                && x.Settingupdateplannedactivityassetdeploymentstatus.Any())
                .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus)
                .ThenInclude(x => x.Assetdeploymentstatus)?
                .Select(x => x.Settingupdateplannedactivityassetdeploymentstatus)
                .FirstOrDefault()?
                .Where(x => x.Assetdeploymentstatus != null)
                .Select(x => x.Assetdeploymentstatus);
            // .ToDictionary(x => x.Deploymentstatusid, x => x.Deploymentstatus);

            var DeploymentStatusReosurce = AssetdeploymentstatusDict?.ToDictionary(x => x.Deploymentstatusid, dto => new DeploymentStatusDto()
            {
                DeploymentStatusId = dto.Deploymentstatusid,
                DeploymentStatusDescription = dto.Deploymentstatus,
                Rule = (int)dto.Rule,
                PlannedActivityResourceAllowedId = DeploymentStatusMapper.GetDeploymentStatusMapper(dto).toPlannedActivityResourceKeyList(),
                ReadOnlyPlannedActivity = dto.Readonlyplannedactivity,
                CheckPlannedActivity = dto.Checkplannedactivity,

            });

            if (DeploymentStatusReosurce != null && DeploymentStatusReosurce.Count >0)
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = DeploymentStatusReosurce

                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.NoDesignComponent,
                    Warning = false

                };
            }
        }

        public ResultDto GetAssetsByOpcoIdAndDcfId(short opcoId, long dcfId)
        {
            return new ResultDto()
            {
                Data = _repositoryWrapper.NetworkElementAsPlanned.GetAssetsByOpcoIdAndDcfId(opcoId, dcfId),
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
            };

        }

        #region  //Ticket 1118 - Asset Create  in Asset screen - Copy the LCM Edu and Subdomain to Asset  EDU and Sub Domain Spoc Table

        public async Task<string> TransferEduSpocToAsset(long assetId, Lcmengineering lcmEntity)
        {
            try
            {
                if (lcmEntity != null)
                {

                    if (lcmEntity?.Lcmengineeringeduspoc?.Any() == true)
                    {

                        var lcmEduList = lcmEntity?.Lcmengineeringeduspoc?
                            .Select(m => m?.Eduspocid)
                            .Distinct()
                            .ToList();

                        if (lcmEduList?.Any() == true)
                        {

                            foreach (var Item in lcmEduList)
                            {
                                _repositoryWrapper.NetworkElementAsPlannedEduSpoc.Create(new Networkelementasplannededuspoc
                                {
                                    Networkelementasplannedid = assetId,
                                    Eduspocid = Item,
                                });

                            }
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();
                        }


                    }
                }

                return ResultMessages.EntryAddSuccess;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        public async Task<string> TransferSubDomainSpocToAsset(long assetId, Lcmengineering lcmEntity)
        {
            try
            {
                if (lcmEntity != null)
                {

                    if (lcmEntity?.Lcmengineeringsubdomainspoc?.Any() == true)
                    {

                        var lcmEduList = lcmEntity?.Lcmengineeringsubdomainspoc?
                            .Select(m => m?.Subdomainspocid)
                            .Distinct()
                            .ToList();

                        if (lcmEduList?.Any() == true)
                        {

                            foreach (var Item in lcmEduList)
                            {
                                _repositoryWrapper.NetworkElementAsPlannedSubDomainSpoc.Create(new Networkelementasplannedsubdomainspoc
                                {
                                    Networkelementasplannedid = assetId,
                                    Subdomainspocid = Item,
                                });

                            }
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();
                        }


                    }
                }

                return ResultMessages.EntryAddSuccess;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        #endregion
        public ResultDto GetLCMEngineeringID([FromBody] NetworkElementAsPlannedGetLCMIDQueryDto dto)
        {
            ResultDto resultDto = null;

            try
            {
                if (dto != null && (dto.OpCo != null && dto.OpCo.Count > 0)
                     && (dto.DesignComponent != null && dto.DesignComponent.Count > 0)
                     && (dto.DeploymentStatus != null && dto.DeploymentStatus.Count > 0))
                {

                    //Get Asset DeployementStatus Value from Deploymentstatuses Table
                    var _astDplyStatus = _repositoryWrapper.DeploymentStatus.FindByCondition
                        (x => x.Deleted == false && x.Deploymentstatusid == dto.DeploymentStatus[0]).Select(x => new
                        {
                            astDeploymentStatusId = x.Deploymentstatusid,
                            astDeploymentStatusValue = x.Deploymentstatus.ToLower().Trim()
                        }).FirstOrDefault();

                    //Get LCM ID from Lcmengineering Table
                    var _LcmEng = _repositoryWrapper.Lcmengineering.FindByCondition(
                        x => x.Opcoid == dto.OpCo[0] && x.Designcomponentid == dto.DesignComponent[0]).
                    Select(x => new
                    {
                        lcmid = x.Lcmengineeringid,
                        statusId = x.Lcmdeploymentstatusid,
                        statusValue = (x.Lcmdeploymentstatus.Description == null) ?
                        "" : x.Lcmdeploymentstatus.Description.ToLower().Trim()
                    }).FirstOrDefault();


                    if (_LcmEng != null)
                    {
                        var lcmRecordStatusValueCheck = _currentUserService.lcmDeploymentStatus.
                        Where(x => x.Contains(Convert.ToString(_LcmEng.statusValue)));

                        // Asset is Planned - LCM must be In-Service/Planned
                        if ((_astDplyStatus.astDeploymentStatusValue == _currentUserService.lcmDeploymentStatus[0].Trim())
                            &&
                            (lcmRecordStatusValueCheck != null && lcmRecordStatusValueCheck.Count() != 0))
                            resultDto = new ResultDto
                            {
                                Info = ResultMessages.EntryLcmIdExists,
                                Data = _LcmEng.lcmid
                            };
                        //Asset is In-Service - LCM must be In-Service
                        else if ((_astDplyStatus.astDeploymentStatusValue == _currentUserService.lcmDeploymentStatus[1].Trim())
                             &&
                              (_LcmEng.statusValue == _currentUserService.lcmDeploymentStatus[1].Trim()))
                            resultDto = new ResultDto
                            {
                                Info = ResultMessages.EntryLcmIdExists,
                                Data = _LcmEng.lcmid
                            };
                        else if ((_astDplyStatus.astDeploymentStatusValue == _currentUserService.lcmDeploymentStatus[2].Trim())
                             &&
                              (_LcmEng.statusValue == _currentUserService.lcmDeploymentStatus[1].Trim()))
                            resultDto = new ResultDto
                            {
                                Info = ResultMessages.EntryLcmIdExists,
                                Data = _LcmEng.lcmid
                            };
                        //Other Status in Asset must be One to One Relation
                        else if (_LcmEng.statusValue == _astDplyStatus.astDeploymentStatusValue)
                            resultDto = new ResultDto
                            {
                                Info = ResultMessages.EntryLcmIdExists,
                                Data = _LcmEng.lcmid
                            };
                        else
                        {
                            resultDto = new ResultDto
                            {
                                Info = ResultMessages.EntryLcmIdExistsWithDifferentStatus,
                                Data = null
                            };
                        }
                    }
                    else
                    {
                        resultDto = new ResultDto
                        {
                            Info = ResultMessages.EntryLcmIdNotExistsForOpcoDesignComponent,
                            Data = null
                        };

                    }


                    return resultDto;
                }
                else
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryLcmIdParameterIssue,
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = ResultMessages.SystemError,
                    Data = null
                };
            }

        }
        #region #1658 - Archive LCM - Move Asset PA to Archive
        public async Task<ResultDto> ArchiveAssetAddNodeAndDecommissionPA(long assetId)
        {
            try
            {

                var includedPaActivity = ConstantValueFilter.assetPaArchivePaType;

                var assetPaEntity = await _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Deleted == false && x.Archived == false && x.Networkelementasplannedid
                == assetId && includedPaActivity.Any(y => y == x.Plannedactivityresource.Plannedactivityresource.Replace(" ", "").ToLower())
                ).Include(x => x.Plannedactivityresource).ThenInclude(x => x.SettingsupdateplannedactivityPlannedactivityresource)
                .Include(x => x.Budgetprojecttrackers)
                .ToListAsync();


                if (assetPaEntity != null && assetPaEntity.Count > 0)
                {

                    Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();

                    foreach (Plannedactivities pa in assetPaEntity)
                    {

                        var getFinalDeliveryStatus = pa?.Plannedactivityresource?.SettingsupdateplannedactivityPlannedactivityresource
                            .OrderByDescending(x => x.Order).FirstOrDefault();

                        if (getFinalDeliveryStatus != null)

                        {
                            if (completedActivityStatus != null) pa.Activitystatusid = completedActivityStatus.Activitystatusid;
                            pa.Archived = true;
                            pa.Deliverystatusid = getFinalDeliveryStatus.Deliverystatusid;
                            _repositoryWrapper.PlannedActivity.Update(pa);
                            var budgetPlanningTrackerEntity = pa?.Budgetprojecttrackers;

                            if (budgetPlanningTrackerEntity != null && budgetPlanningTrackerEntity.Count > 0)
                            {
                                foreach (Budgetprojecttrackers item in budgetPlanningTrackerEntity)
                                {
                                    item.Archive = true;
                                    _repositoryWrapper.BudgetProjectTrackersRepository.Update(item);

                                }

                            }
                        }

                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }

                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = assetPaEntity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Issue happen when try to Archive Asset (ArchiveAssetPa) : " + ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true, Data = ex };
            }

        }

        #endregion
              
    }
}