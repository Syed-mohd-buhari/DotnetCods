using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.ComponentSoftware;
using CAM.BusinessManager.Entity.PlannedActivities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.LookUp;
using CAM.BusinessManager.Rules;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ClusterLevelPA;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.Entita.ServicePlan;
using CAM.DataTransferObjects.LookUp.DaPlannedActivityDcf;
using CAM.DataTransferObjects.LookUp.DeliveryTracking;
using CAM.DataTransferObjects.Settings;
using CAM.DataTransferObjects.Settings.SettingsUpdatePlannedActivity;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Setting;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CAM.Enum.ResourceTypeEnum;

namespace CAM.BusinessManager.Settings
{
    public class UpdatePlannedActivityManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly LcmEngineeringManager _lcmEngineeringManager;
        private readonly PlannedActivityManager _plannedActivityManager;
        private DesignComponentFamilyLifeCycleManager _designComponentFamilyLifeCycleManager;
        private ResourceKeyMasterManager _resourceKeyMasterManager;
        private readonly DeliveryTrackingManager _deliveryTrackingManager;
        private readonly CommonManager _commonManager;
        private readonly BuildBagManager _buildBagManager;
        private readonly ComponentSoftwareBuildBagMappingManager _componentSoftwareBuildMappingManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly NetworkElementsAsPlannedManager _networkElementsAsPlannedManager;
        private readonly DesignAspectPlannedActivityManger _designAspectPlannedActivityManger;         
        private readonly ServicePlanManager _servicePlanManager;

        private Plannedactivities getExistPAEntityIfLCMUnknown = new Plannedactivities();
        private bool isExistPAEntityIfLCMUnknow = false;
        bool isModernizeSuccessorFlow = false;
        private Lcmengineering currentLCMinDB = new Lcmengineering();
        public List<FilterValueDtoKeyValueList> duplicateAssetNamesInLCM = new List<FilterValueDtoKeyValueList>();
        public List<NetworkElementAssociated> selectedMigratedNodeEntity = new List<NetworkElementAssociated>();


        public UpdatePlannedActivityManager(IEnumerable<IRepositoryWrapper> wrappers, LcmEngineeringManager lcmEngineeringManager,
            IMapper mapper, GridCustomColumnManager columnManager, DeliveryTrackingManager deliveryTrackingManager, PlannedActivityManager plannedActivityManager, ResourceKeyMasterManager resourceKeyMasterManager, DesignComponentFamilyLifeCycleManager designComponentLIfecycleManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager, 
            BuildBagManager buildBagManager, ComponentSoftwareBuildBagMappingManager componentSoftwareBuildMappingManager
            ,DropdownDataServiceManager  dropdownDataServiceManager , NetworkElementsAsPlannedManager  networkElementsAsPlannedManager,
            DesignAspectPlannedActivityManger designAspectPlannedActivityManger, ServicePlanManager servicePlanManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
            _lcmEngineeringManager = lcmEngineeringManager;
            _plannedActivityManager = plannedActivityManager;
            _designComponentFamilyLifeCycleManager = designComponentLIfecycleManager;
            _resourceKeyMasterManager = resourceKeyMasterManager;
            _deliveryTrackingManager = deliveryTrackingManager;
            _commonManager = commonManager;
            _buildBagManager = buildBagManager;
            _componentSoftwareBuildMappingManager = componentSoftwareBuildMappingManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _networkElementsAsPlannedManager = networkElementsAsPlannedManager;
            _designAspectPlannedActivityManger = designAspectPlannedActivityManger;
            _servicePlanManager = servicePlanManager;
        }

        public async Task<ResultDto> SaveUpdatePlannedActivityStatus(UpdatePlannedActivityStatusDto data)
        {
            short? plannedActivityTypeFor = data.PlannedActivityTypeFor;

            var settingNew = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Settingsupdateplnactid == data.SettingsUpdatePlannedActivityId)
                .Include(x => x.Deliverystatus)
                .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus).FirstOrDefault();

            var plannedActivityEntity = await _plannedActivityManager.GetPlannedActivityForUpdatePA((long)data.PlanningActivityDetailsResourceId);

            if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Component_Upgrade)
            {
                return await ComponentUpgrade(plannedActivityEntity, settingNew.Deliverystatusid);
            }
            else if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Infra_Readiness)
            {
                return await _designAspectPlannedActivityManger.ApplySettingUpdateRulesToDesignAspectPA(plannedActivityEntity, settingNew, plannedActivityTypeFor, data.DaPlannedActivtyDcfDto);
            }
            else if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration)
            {
                return await _designAspectPlannedActivityManger.ApplySettingUpdateRulesToDesignAspectPA(plannedActivityEntity, settingNew, plannedActivityTypeFor, data.DaPlannedActivtyDcfDto);
            }
            else if ((plannedActivityEntity.Lcmengineering == null && plannedActivityEntity.Designaspectid != null) || plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.DesignAspect)
            {
                return await _designAspectPlannedActivityManger.ApplySettingUpdateRulesToDesignAspectPA(plannedActivityEntity, settingNew, plannedActivityTypeFor, data.DaPlannedActivtyDcfDto);
            }
            if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.AddNewCluster)
            {
                return await _plannedActivityManager.ApplySettingUpdateRulesToClusterLevelPA(plannedActivityEntity, settingNew, data.infraClusterClusterUpgradeUpsertDto);
            }
            if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Upgrade_HardwareTypes)
            {
                return await _plannedActivityManager.ApplySettingUpdateRulesForUpgradeCluster(plannedActivityEntity, settingNew, data.infraClusterClusterUpgradeUpsertDto);
            }
            if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Add_Remove_Application_from_Cluster)
            {
                return await _plannedActivityManager.ApplySettingUpdateRulesForAddorRemoveCluster(plannedActivityEntity, settingNew, data.infraClusterClusterUpgradeUpsertDto);
            }
            else if(plannedActivityTypeFor == (short)PlannedActivityResourceEnum.Service_Planned)
            {
                return await _servicePlanManager.ApplySettingUpdateRulesForServicePlanPA(plannedActivityEntity, settingNew, plannedActivityTypeFor, data.ServicePlanDcfDto);
            }


                isModernizeSuccessorFlow = false;
            #region - Ticket 1094  Get Cross Setting Rule for Node Selection
            var inputPreviousDeliveryState = plannedActivityEntity.Plannedactivityresource.SettingsupdateplannedactivityPlannedactivityresource.FirstOrDefault(x =>
                 x.Deliverystatusid == plannedActivityEntity.Deliverystatusid
                 && x.Plannedactivityresourceid == plannedActivityEntity.Plannedactivityresourceid
                  )?.Settingsupdateplnactid;
            //inputPrevious  - Previous SettingPA Details  && Output - Currently selected Delivery Status Id 
            var moveSelectedNodesToNewLCM = (data.CrossSettingscResource.Select(t => t.Value).ToList()
                .FirstOrDefault(x => x.Input == inputPreviousDeliveryState && 
                x.Output == data.SettingsUpdatePlannedActivityId && x.Rule == ConstantValueFilter.NodeSelectionAskToUser) != null
                  ) ? true : false;
            #endregion

            #region Ticket 918 
            if ((data.EndNodesInLab != null && data.EndNodesInLab.Count() > 0) || (data.EndNodesInProd != null && data.EndNodesInProd.Count() > 0))
            {

                if (selectedMigratedNodeEntity != null && data.EndNodesInProd != null && data.EndNodesInProd.Count > 0)
                    selectedMigratedNodeEntity.AddRange(data.EndNodesInProd);
                if (selectedMigratedNodeEntity != null && data.EndNodesInLab != null && data.EndNodesInLab.Count > 0)
                    selectedMigratedNodeEntity.AddRange(data.EndNodesInLab);

                if (selectedMigratedNodeEntity != null)
                {
                    ///#1409 Software upgrade: FSI achieved : Even though we select 1 node, all the nodes are getting moved to new LCM  - 
                    var existsNodes = selectedMigratedNodeEntity.Select(x => x.Id.ToString()).ToList();
                    if (data.StartNodesInProd != null)
                    {
                        var filteredNodes = data.StartNodesInProd
                            .Where(x => !existsNodes.Contains(x.Id.ToString()) && x.Id > 0)
                            .Select(y => new NetworkElementAssociated
                            {
                                Id = y.Id,
                                IsFinalAsset = false,
                                ElementName = y.ElementName
                            })
                            .ToList();


                        if (filteredNodes != null && filteredNodes.Count > 0) selectedMigratedNodeEntity.AddRange(filteredNodes);
                    }

                    if (data.StartNodesInLab != null)
                    {
                        var filteredLabNodes = data.StartNodesInLab
                            .Where(x => !existsNodes.Contains(x.Id.ToString()) && x.Id > 0)
                            .Select(y => new NetworkElementAssociated
                            {
                                Id = y.Id,
                                IsFinalAsset = false,
                                ElementName = y.ElementName
                            })
                            .ToList();

                        if (filteredLabNodes != null && filteredLabNodes.Count > 0) selectedMigratedNodeEntity.AddRange(filteredLabNodes);
                    }

                }
                foreach (var x in selectedMigratedNodeEntity)
                {
                    if (x.Id != 0)
                    {
                        var asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(y => y.Networkelementasplannedid == x.Id).FirstOrDefault();
                        if (asset != null)
                        {
                            asset.Isfinalasset = x.IsFinalAsset;
                            _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                            _repositoryWrapper.Save();
                            await _repositoryWrapper.ClearTracker();
                        }
                    }
                }
            }
            #endregion 

            #region Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow

            int modernizeRuleLinkedDcId = 0;
            short modernizeAssetStatusInserviceId = 0;
            int modernizeSettingPaRule = 0; //This is used to Split the Asset without Status Change

            if (plannedActivityEntity != null)
            {
                isModernizeSuccessorFlow = (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Modernize_Solution_Successor_Network);
                modernizeRuleLinkedDcId = plannedActivityEntity.Plannedactivityresource.Rulelinkeddc;
                modernizeSettingPaRule = Convert.ToInt16(settingNew?.Rule);
            }

            modernizeAssetStatusInserviceId = (short)LCMEngineeringRulesExtension.GetAssetDeploymentStatusBasedOnParameter(ConstantValueFilter.InService, _repositoryWrapper)?.FirstOrDefault().Key;
            #endregion

            var originalLcm = plannedActivityEntity.Lcmengineering;
            #region Ticket 1025 Modernize Enhancement Flow 2 - New Asset from UpdateSettingPlanned Screen
            try
            {
                if ((isModernizeSuccessorFlow == true) && ((data.StartNodesInLab != null && data.StartNodesInLab.Count() > 0) || (data.StartNodesInProd != null && data.StartNodesInProd.Count() > 0)))
                {
                    LcmEngineeringDtoCreate dtoAssetInsert = new LcmEngineeringDtoCreate();
                    dtoAssetInsert.OpCoId = data.OpCoId;
                    dtoAssetInsert.IsReleaseDetailUnKnown = (bool)originalLcm.Isreleasedetailunknown;
                    dtoAssetInsert.DesignComponentId = originalLcm.Designcomponentid;
                    dtoAssetInsert.DesignComponentFamilyid = (long)originalLcm.Designcomponentfamilyid;
                    dtoAssetInsert.SubDomainSpocIds = originalLcm?.Lcmengineeringsubdomainspoc?.Select(x =>  x.Subdomainspocid)?.ToList();
                    dtoAssetInsert.EduSpocIds = originalLcm?.Lcmengineeringeduspoc?.Select(x => x.Eduspocid)?.ToList();

                    var assetElementEntity = data.StartNodesInLab.Where(x => x.Id == 0).ToList();
                    if (assetElementEntity != null && data.StartNodesInProd != null) assetElementEntity.AddRange(data.StartNodesInProd.Where(x => x.Id == 0).ToList());
                    else if (assetElementEntity == null && data.StartNodesInProd != null) assetElementEntity = data.StartNodesInProd.Where(x => x.Id == 0).ToList();

                    dtoAssetInsert.NetworkElementAssociateds = assetElementEntity;
                    if (assetElementEntity != null && assetElementEntity.Count() > 0)
                    {
                        await _lcmEngineeringManager.GenerateNetworkElementAssociated(dtoAssetInsert, false);
                        var newlyInsertedAsset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(t => t.Lcmengineeringid == null
                        && t.Opcoid == originalLcm.Opcoid && t.Designcomponentid == originalLcm.Designcomponentid).ToList();
                        foreach (var assetitem in newlyInsertedAsset)
                        {
                            assetitem.Lcmengineeringid = originalLcm.Lcmengineeringid;
                            assetitem.Buildbagid = originalLcm.Buildbagid;
                            _repositoryWrapper.NetworkElementAsPlanned.Update(assetitem);
                        }
                        await _repositoryWrapper.SaveAsync();
                    }



                }
            }
            catch (Exception e)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = e.Message
                };
            }
            #endregion

            var archivingRuleApplyToAssetsAndPA = new ResultDto();

            /// -- Automatically Created PA based on Rollout completed in Node Selection
            if ((plannedActivityEntity.Lcmengineering == null && plannedActivityEntity.Networkelementasplannedid != null) || plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.AddAsset || plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.EditAsset)
            {
                archivingRuleApplyToAssetsAndPA = await _plannedActivityManager.ApplyArchivingRulesToUpdatePAandAssets(plannedActivityEntity, settingNew, plannedActivityTypeFor);

                if (!string.IsNullOrEmpty(archivingRuleApplyToAssetsAndPA.Info.ToString()))
                {
                    if (archivingRuleApplyToAssetsAndPA.Info.ToString() == ConstantValueFilter.assetMigrated)
                    {
                        var plannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid, true, false)
                        .Include(x => x.Networkelementasplanned).FirstOrDefault();
                        _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(plannedActivity.Networkelementasplanned, ConstantValueFilter.assetMigrated);
                    }
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                        Data = archivingRuleApplyToAssetsAndPA.Data
                    }; ;
                }
            }


            #region Ticket 772 - #771 - In Update Planned Activity Status Screen & In PA Utility Screen the Specify DC Rule Should be governed using the Settings Planned activity Rules.
                var getUpdatedSpecifyDcForIsPAUnknown = _plannedActivityManager.UpdateSpecifyDcForIsPAUnknownReleaseDetail(plannedActivityEntity, (bool)settingNew.Specifydc,
            (long)data.DesignComponentId, originalLcm.Designcomponentid);

            if (getUpdatedSpecifyDcForIsPAUnknown.Result != null && (getUpdatedSpecifyDcForIsPAUnknown.Result.Plannedactivityid == plannedActivityEntity.Plannedactivityid))
                plannedActivityEntity = getUpdatedSpecifyDcForIsPAUnknown.Result;
            else
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = new { id = getUpdatedSpecifyDcForIsPAUnknown.Result.Plannedactivityid, orphanDeleted = true }
                };
            }


            #endregion
            var updateSpecifyDcToPAandLCM = UpdateLCMandPAOnSpecifiyDC(originalLcm, plannedActivityEntity, data, modernizeAssetStatusInserviceId);

            #region // Asset status #412 Decommission Flow Implementation  - Decommission Flow April 17 2024
            string settingsPaAssetDeploymentStatusId = Convert.ToString(settingNew.Settingupdateplannedactivityassetdeploymentstatus != null ?
                settingNew.Settingupdateplannedactivityassetdeploymentstatus.Where(x => x.Deleted == false).FirstOrDefault()?
                .Assetdeploymentstatusid.ToString() : "");
            await UpdateAssetDeploymentStatusOnDecommission(plannedActivityEntity, settingsPaAssetDeploymentStatusId, modernizeAssetStatusInserviceId, modernizeSettingPaRule, isModernizeSuccessorFlow, data);

            #endregion
            if (settingNew.Ruleelementcount != (int)ArchivingRuleEnum.NoRule)
            {
                var completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus.ToUpper()).FirstOrDefault();
                plannedActivityEntity.Activitystatusid = completedActivityStatus.Activitystatusid;

            }
            #region Ticket 797 - Release details unknown-LCM level_multiple PAs: 2 LCMs with same Opco & DC getting created when we specify same DC while rolling out the Planned activities
            var applyProjectAndFinanceTabForPAEntity = await UpdateProjectAndFinanceTabForPAEntity(plannedActivityEntity, settingNew, data.InEngineeringPhase);
            plannedActivityEntity = applyProjectAndFinanceTabForPAEntity;

            #endregion
            var lcmDeploymentStatusId = FetchLcmDeploymentStatusIdByMinPlannedCompletion(originalLcm.Lcmengineeringid).Result;

            originalLcm.Lcmdeploymentstatusid = lcmDeploymentStatusId != 0 ? lcmDeploymentStatusId : originalLcm.Lcmdeploymentstatusid;
            if (getExistPAEntityIfLCMUnknown != null) originalLcm.Archived = ConstantValueFilter.isTrue;
            _repositoryWrapper.Lcmengineering.Update(originalLcm);
            await _repositoryWrapper.SaveAsync();

            var inServiceStatus = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Description.ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault()?.Id;
            var assets = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Lcmengineeringid == originalLcm.Lcmengineeringid).ToList();

            if (data.AssetLiveStatusDate != null || data.AssetDecommissionedDate != null)
            {
                foreach (var asset in assets)
                {
                    asset.Assetlivestatusdate = data.AssetLiveStatusDate;
                    asset.Assetdecommissioneddate = data.AssetDecommissionedDate;
                    _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                    await _repositoryWrapper.SaveAsync();

                }
                await _repositoryWrapper.ClearTracker();
            }
     
            if (settingNew.Ruleelementcount == (int)ArchivingRuleEnum.ArchivePlannedActivityOnly)
            {
                plannedActivityEntity.Archived = ConstantValueFilter.isTrue;
                if (getExistPAEntityIfLCMUnknown != null)
                    plannedActivityEntity.Lcmengineeringid = getExistPAEntityIfLCMUnknown.Lcmengineeringid;
                _repositoryWrapper.PlannedActivity.Update(plannedActivityEntity);
                await _repositoryWrapper.SaveAsync();

                await _commonManager.setArchiveStatusForBpt(plannedActivityEntity);
                /// LCM Part 3 Requirements
                if (originalLcm != null)
                {
                    originalLcm.Resourcekey = _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforNewSolution(originalLcm, plannedActivityEntity).Result.Data.ToString();

                }
                originalLcm.Lcmdeploymentstatusid = inServiceStatus;
                _repositoryWrapper.Lcmengineering.Update(originalLcm);
                await _repositoryWrapper.SaveAsync();

                var plannedId = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.ToUpper().Replace(" ", "") == ConstantValueFilter.Planned.ToUpper()).Select(x => x.Deploymentstatusid).FirstOrDefault();
                
                var deploymentStatusId = modernizeAssetStatusInserviceId;

                #region #1658 - Archive LCM - Move Asset PA to Archive
                var assetDeploymentStatus = await _dropdownDataServiceManager.GetAssetDeploymentStatus();
                var inservice_Decommission_AssetDeploymentId = assetDeploymentStatus
                    .Where(x => ConstantValueFilter.assetDeploymentStatusForArchivePa.Any(y => y == x.Value.Replace(" ", "").ToLower()))
                    .Select(t => t.Key).ToList();
                #endregion
                foreach (var asset in assets)
                {
                    var existingAssetDeploymentStatusId = asset.Deploymentstatusid;
                    /// add node selection function
                    if (originalLcm.Lcmdeploymentstatusid == inServiceStatus && asset.Isfinalasset.Value)
                    {
                        asset.Deploymentstatusid = (short)deploymentStatusId;
                        _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                        await _repositoryWrapper.SaveAsync();

                        _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(asset, ConstantValueFilter.assetMigrated);
                        if (inservice_Decommission_AssetDeploymentId.Any(x => x == asset.Deploymentstatusid))
                            await _networkElementsAsPlannedManager.ArchiveAssetAddNodeAndDecommissionPA(asset.Networkelementasplannedid);
                    }
                    else
                    {
                        //This call required no need to check  asset Deployment status  - New Intial PA created
                        await _networkElementsAsPlannedManager.ArchiveAssetAddNodeAndDecommissionPA(asset.Networkelementasplannedid); 
                        var result = NodeSelectionMethod(asset, plannedActivityEntity, asset.Designcomponentid);

                    }
                }

            }


            else if (originalLcm.Isreleasedetailunknown != true && getExistPAEntityIfLCMUnknown == null && isExistPAEntityIfLCMUnknow == false)
            {
                var networkElement = data.EndNodesInLab;
                networkElement ??= new List<NetworkElementAssociated>();
                if (data.EndNodesInProd != null && data.EndNodesInProd.Count > 0) networkElement.AddRange(data.EndNodesInProd);

                var lcmExists = await _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Designcomponentid == plannedActivityEntity.Designcomponentid && x.Archived != true
                                          && x.Opcoid == plannedActivityEntity.Lcmengineering.Opcoid, false, false)
                    .OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();

                if (lcmExists == null)
                {
                    var newLCM = CreateLcm(data.NumberOfNodesOutput, data.NumberOfNodesInLabOutput, plannedActivityEntity).Result;

                    var lcmToUprate = plannedActivityEntity.Lcmengineering;

                    if (settingNew.Ruleelementcount == (int)ArchivingRuleEnum.ArchivePlannedActivityAndParentEntry
                      || (newLCM != null && moveSelectedNodesToNewLCM))
                    {
                        //lcmToUprate.Numberofnodesinlab = 0;
                        //lcmToUprate.Numberofnodes = 0;
                        if (newLCM != null) /// Asset status #412 Decommission Flow Implementation  /// For decommission Flow new LCM not create only LCM will Archived
                        {
                            await UpdateAssetResourceKeylcmIdAndDcfId(lcmToUprate, newLCM, plannedActivityEntity, moveSelectedNodesToNewLCM);
                            if (lcmToUprate.Elementcount)
                            {
                                newLCM.Numberofnodes = LCMEngineeringMapper.GetLcmEngineeringMapper(newLCM).CountNetworkElementReleated(false, _repositoryWrapper);
                                newLCM.Numberofnodesinlab = LCMEngineeringMapper.GetLcmEngineeringMapper(newLCM).CountNetworkElementReleated(true, _repositoryWrapper);
                            }
                            else
                            {
                                newLCM.Numberofnodes = data.NumberOfNodesInLabOutput;
                                newLCM.Numberofnodesinlab = data.NumberOfNodesOutput;
                            }

                            _repositoryWrapper.Lcmengineering.Update(newLCM);

                            lcmToUprate.Numberofnodesinlab -= data.NumberOfNodesInLabOutput;
                            lcmToUprate.Numberofnodes -= data.NumberOfNodesOutput;
                        }
                        await _repositoryWrapper.SaveAsync();
                    }
                    else
                    {
                        if (networkElement.Any()) await UpdateNetworkElement(networkElement, plannedActivityEntity.Designcomponentid.Value);
                        lcmToUprate.Numberofnodesinlab -= data.NumberOfNodesInLabOutput;
                        lcmToUprate.Numberofnodes -= data.NumberOfNodesOutput;
                    }
                    _repositoryWrapper.Lcmengineering.Update(lcmToUprate);
                    await _repositoryWrapper.SaveAsync();
                    ///Ticket 1292 - Insert/Update/Delete actions in the Asset screen are not updating the LCM Number of Nodes Count and DCF Implementation Value.
                    await _commonManager.SetImplementationFlagInDCF(lcmToUprate.Designcomponentid);
                }
                else
                {
                    // need to update the exist ancillary to new ancillary
                    if (lcmExists != null)
                    {
                        await _lcmEngineeringManager.InsertUpdateAncillaryDatas(originalLcm.Lcmengineeringid, lcmExists.Lcmengineeringid, false, null, false, originalLcm.Lcmdeploymentstatusid.Value);
                    }
                    _lcmEngineeringManager.AddDesignAspectAndPAForLCM(lcmExists, plannedActivityEntity);// to add DA if not exist and PA if required from seeting PA table

                    var lcmToUprate = plannedActivityEntity.Lcmengineering;
                    if (settingNew.Ruleelementcount == (int)ArchivingRuleEnum.ArchivePlannedActivityAndParentEntry)
                    {
                        lcmToUprate.Numberofnodesinlab = 0;
                        lcmToUprate.Numberofnodes = 0;

                        if (lcmExists.Elementcount)
                        {
                            await UpdateAssetResourceKeylcmIdAndDcfId(lcmToUprate, lcmExists, plannedActivityEntity, moveSelectedNodesToNewLCM);
                            lcmExists.Numberofnodes = LCMEngineeringMapper.GetLcmEngineeringMapper(lcmExists).CountNetworkElementReleated(false, _repositoryWrapper);
                            lcmExists.Numberofnodesinlab = LCMEngineeringMapper.GetLcmEngineeringMapper(lcmExists).CountNetworkElementReleated(true, _repositoryWrapper);

                        }
                        else
                        {
                            lcmExists.Numberofnodes += data.NumberOfNodesInput;
                            lcmExists.Numberofnodesinlab += data.NumberOfNodesInLabInput;
                        }

                    }
                    else
                    {
                        if (networkElement.Any()) await UpdateNetworkElement(networkElement, plannedActivityEntity.Designcomponentid.Value);

                        #region Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
                        if (settingNew.Rule == 1 && isModernizeSuccessorFlow)
                        {
                            var asset = _repositoryWrapper.NetworkElementAsPlanned.
                            FindByCondition(x => x.Lcmengineeringid == lcmToUprate.Lcmengineeringid).ToList();
                            await _repositoryWrapper.ClearTracker();

                            if (!string.IsNullOrEmpty(settingsPaAssetDeploymentStatusId))
                            {
                                foreach (var item in asset)
                                {
                                    item.Deploymentstatusid =
                                          Convert.ToInt16(settingsPaAssetDeploymentStatusId);
                                    _repositoryWrapper.NetworkElementAsPlanned.Update(item);

                                }
                                _repositoryWrapper.Save();
                                await _repositoryWrapper.ClearTracker();
                            }


                        }
                        #endregion

                        if (!lcmExists.Elementcount)
                        {
                            lcmExists.Numberofnodes += data.NumberOfNodesOutput;
                            lcmExists.Numberofnodesinlab += data.NumberOfNodesInLabOutput;
                        }
                        else
                        {
                            lcmExists.Numberofnodes = LCMEngineeringMapper.GetLcmEngineeringMapper(lcmExists).CountNetworkElementReleated(false, _repositoryWrapper);
                            lcmExists.Numberofnodesinlab = LCMEngineeringMapper.GetLcmEngineeringMapper(lcmExists).CountNetworkElementReleated(true, _repositoryWrapper);

                        }
                        lcmToUprate.Numberofnodesinlab -= data.NumberOfNodesInLabOutput;
                        lcmToUprate.Numberofnodes -= data.NumberOfNodesOutput;
                    }

                    _repositoryWrapper.Lcmengineering.Update(lcmToUprate);
                    _repositoryWrapper.Lcmengineering.Detach();
                    _repositoryWrapper.Lcmengineering.Update(lcmExists);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();


                    ///Ticket 1292 - Insert/Update/Delete actions in the Asset screen are not updating the LCM Number of Nodes Count and DCF Implementation Value.
                    await _commonManager.SetImplementationFlagInDCF(lcmToUprate.Designcomponentid);
                    await _commonManager.SetImplementationFlagInDCF(lcmExists.Designcomponentid);

                }

                var numberOfNodesOutput = originalLcm.CountNetworkElementReleated(false, _repositoryWrapper);
                var numberOfNodesInLabOutput = originalLcm.CountNetworkElementReleated(true, _repositoryWrapper);

                var settingRuleElementCount = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering && x.Plannedactivityresourceid == plannedActivityEntity.Plannedactivityresourceid && x.Deliverystatusid == plannedActivityEntity.Deliverystatusid)
                           .Include(x => x.Deliverystatus)?.FirstOrDefault()?.Ruleelementcount;

                if (settingRuleElementCount == (int)ArchivingRuleEnum.ArchivePlannedActivityAndParentEntry || (numberOfNodesOutput == 0 && numberOfNodesInLabOutput == 0))
                {
                    plannedActivityEntity.Archived = true;
                    plannedActivityEntity.Originallcmengineeringid = originalLcm.Lcmengineeringid;
                    _repositoryWrapper.PlannedActivity.Update(plannedActivityEntity);

                    var RemovedDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository
                  .FindByCondition(x => x.Description.ToLower().Replace(" ", "") == ConstantValueFilter.Removed).Select(x => x.Id).FirstOrDefault();

                    originalLcm.Archived = true;
                    originalLcm.Lcmdeploymentstatusid = RemovedDeploymentStatusId;
                    string[] resourceKey = originalLcm.Resourcekey.Split("_");
                    originalLcm.Resourcekey = originalLcm.Resourcekey;

                    _repositoryWrapper.Lcmengineering.Update(originalLcm);

                    await _repositoryWrapper.SaveAsync();

                    await _commonManager.setArchiveStatusForBpt(plannedActivityEntity);
                    if (originalLcm.Archived == true)
                    {
                        await _lcmEngineeringManager.InsertUpdateAncillaryDatas(originalLcm.Lcmengineeringid, 0, false, null, false, originalLcm.Lcmdeploymentstatusid.Value, true);
                    }

                    #region Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow and  #412 Decommission Flow Implementation
                    if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Decommission_Service_Node || isModernizeSuccessorFlow)
                    {
                        await _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforNewSolution(originalLcm, plannedActivityEntity, true);
                         #region// Change DCF Asset record activityDescription 
                        string dcfAssetActivitydescription = ConstantValueFilter.assetRemovedThroughModernized;
                        if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Decommission_Service_Node)
                            dcfAssetActivitydescription = ConstantValueFilter.assetDecommissioned;
                        #endregion

                        foreach (var asset in assets)
                            _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(asset, dcfAssetActivitydescription);
                           
                        }
                    #endregion            
                    await _lcmEngineeringManager.ArchiveDesginAspectCasePARolledout(originalLcm, plannedActivityEntity);
                    await _lcmEngineeringManager.MoveActivePAs(originalLcm.Lcmengineeringid, plannedActivityEntity.Designcomponentid.Value);
                }



            }
            /////--------------------------
            if (moveSelectedNodesToNewLCM) await updateAssetIsFinalAssetFalseForNodeSection(selectedMigratedNodeEntity);

            var formattedDuplicateAsset = duplicateAssetNamesInLCM.GroupBy(x => x.Key)
             .Select(g => $"{g.Key} : {string.Join(", ", g.Select(x => x.Value))}").ToList();


            return new ResultDto
            {

                Info = (duplicateAssetNamesInLCM != null && duplicateAssetNamesInLCM.Count > 0) ? ResultMessages.EntryUpdateSuccessWithDuplicateAsset + " " +
               string.Join(",", formattedDuplicateAsset ?? new List<string>())
             : ResultMessages.EntryUpdateSuccess

            }; 
        }

        #region //add node selection function
        private async Task<ResultDto> NodeSelectionMethod(Networkelementsasplanned asset, Plannedactivities plannedActivityEntity, long currentDcId )
        {
            bool isValid = false;
            try
            {
                if (asset != null && plannedActivityEntity != null)
                {
                    var plannedActivityResource = _repositoryWrapper.PlannedActivityResourceRepository
                        .FindByCondition(x => x.Plannedactivityresource.Replace(" ", "").ToLower() == ConstantValueFilter.initialNetworkDeployment).FirstOrDefault();
                    var settingUpdatPlannedActivity = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivityresourceid == plannedActivityResource.Plannedactivityresourceid).OrderBy(x => x.Order).FirstOrDefault();

                    var deliveryStatus = _repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatus.ToLower().Replace(" ", "") == settingUpdatPlannedActivity.Settingsupdateplnactdes.ToLower().Replace(" ", "")).FirstOrDefault().Deliverystatusid;

                    var activityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(x => x.Activitystatus.ToLower().Replace(" ", "") == ConstantValueFilter.InPlanning).FirstOrDefault().Activitystatusid;

                    var newPlannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid).FirstOrDefault();
                    if (newPlannedActivity != null)
                    {
                        newPlannedActivity.Plannedactivityid = 0;
                        newPlannedActivity.Plannedactivityresourceid = plannedActivityResource.Plannedactivityresourceid;
                        newPlannedActivity.Deliverystatusid = deliveryStatus;
                        newPlannedActivity.Responsibilityphaseid = plannedActivityEntity.Responsibilityphaseid;
                        newPlannedActivity.Deliverystatus = plannedActivityEntity.Deliverystatusid == 0 ? null : plannedActivityEntity.Deliverystatus;
                        newPlannedActivity.Networkelementasplannedid = asset.Networkelementasplannedid;
                        newPlannedActivity.Archived = false;
                        newPlannedActivity.Opcoid = plannedActivityEntity.Opcoid;
                        newPlannedActivity.Designcomponentid = plannedActivityEntity.Designcomponentid;
                        newPlannedActivity.Designcomponentfamilyid = plannedActivityEntity.Designcomponentfamilyid;
                        newPlannedActivity.Lcmengineeringid = null;
                        newPlannedActivity.Foreditasset = true;
                        newPlannedActivity.Foraddasset = false;
                        newPlannedActivity.Deliveryplanavailable = plannedActivityEntity.Deliveryplanavailable;
                        newPlannedActivity.Activitydetails = null;
                        newPlannedActivity.Archived = false;
                        newPlannedActivity.Activitystatusid = activityStatus;
                        newPlannedActivity.Budgetavailabilityid = plannedActivityEntity.Budgetavailabilityid;
                        newPlannedActivity.Localapproval = plannedActivityEntity.Localapproval;
                        newPlannedActivity.Responsibilityphaseid = plannedActivityEntity.Responsibilityphaseid;
                        newPlannedActivity.Engineeringriskid = plannedActivityEntity.Engineeringriskid;
                        newPlannedActivity.Operationalriskid = plannedActivityEntity.Operationalriskid;
                        newPlannedActivity.Plannedcompletion = plannedActivityEntity.Plannedcompletion;
                        newPlannedActivity.Plannedimplementationyear = plannedActivityEntity.Plannedimplementationyear;
                        newPlannedActivity.Plannedactivitycategoryid = plannedActivityEntity.Plannedactivitycategoryid == 0 ? null : plannedActivityEntity.Plannedactivitycategoryid;
                        newPlannedActivity.Programid = plannedActivityEntity.Programid == 0 ? null : plannedActivityEntity.Programid;

                        _repositoryWrapper.PlannedActivity.Create(newPlannedActivity);
                        await _repositoryWrapper.SaveAsync();
                        if (newPlannedActivity.Deliveryplanavailable)
                        {
                            var deliveryTracking = new DeliveryTrackingDtoCreate()
                            {
                                PlannedActivityId = newPlannedActivity.Plannedactivityid

                            };
                            await _deliveryTrackingManager.Add(deliveryTracking);
                        }
                        await _commonManager.CreateOrUpdateBptreport(newPlannedActivity.Plannedactivityid,
                           currentDcId, (long)newPlannedActivity.Designcomponentid);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                    asset.Plannedactivities.Add(newPlannedActivity);

                    _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                }
            }
            catch
            {
                isValid = true;
            }

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = isValid
            };
        }
        #endregion


        public async Task<Networkelementsasplanned> UpdateAssuredStatus (Networkelementsasplanned asset)
        {
            var assetVersion = string.Empty;
            var assetSoftwareDetails = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == asset.Designcomponentid)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).FirstOrDefaultAsync();

            if(assetSoftwareDetails != null)
            {
                assetVersion = assetSoftwareDetails?.Systemtype?.Majorsoftwarebuilds?.Softwareversion;
            }

            var asIsAsset = await _repositoryWrapper.NetworkElementAsIs.FindByCondition(x => x.Networkelementasplannedid == asset.Networkelementasplannedid).FirstOrDefaultAsync();

            if (asIsAsset != null)
            {
                if (assetVersion == asIsAsset?.Softwarereleaseinformation)
                {
                    asset.Isassured = true;
                }
                return asset;
            }
            return asset;
        }

        private async Task ArchiveParentLcmEngineering(Lcmengineering lcmEngineering, Plannedactivities plannedActivity)
        {

            var RemovedDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository
          .FindByCondition(x => x.Description.ToLower().Replace(" ", "") == ConstantValueFilter.Removed).Select(x => x.Id).FirstOrDefault();

            lcmEngineering.Archived = ConstantValueFilter.isTrue;
            lcmEngineering.Lcmdeploymentstatusid = RemovedDeploymentStatusId;

            _repositoryWrapper.Lcmengineering.Update(lcmEngineering);

            await _repositoryWrapper.SaveAsync();

            await _lcmEngineeringManager.ArchiveDesginAspectCasePARolledout(lcmEngineering, plannedActivity);

            await _lcmEngineeringManager.MoveActivePAs(lcmEngineering.Lcmengineeringid, plannedActivity.Designcomponentid.Value);

        }

        public async Task<Lcmengineering> CreateLcm(int numberOfNodes, int numberOfNodesInLab, Plannedactivities plannedActivity)
        {
            return await _lcmEngineeringManager.CreateLcmAndPA(numberOfNodes, numberOfNodesInLab, plannedActivity);
        }


        public async Task UpdateAssetResourceKeylcmIdAndDcfId(Lcmengineering lcmengineering, Lcmengineering newLCM, Plannedactivities plannedActivityEntity, bool moveSelectedNodesToNewLCM = false)
        {
            bool isDuplicateElementMigrate = false;
            if (newLCM != null)
            {
                var nodes = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(p => p.Designcomponentid == lcmengineering.Designcomponentid
                && p.Opcoid == lcmengineering.Opcoid)
                    .Include(x => x.Deploymentstatus).ToList();
                if (moveSelectedNodesToNewLCM)
                    nodes = nodes.Where(x => x.Isfinalasset == true).ToList();

                var currentDCF = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == lcmengineering.Designcomponentid).Select(x => x.Designcomponentfamilyid).FirstOrDefault();
                var plannedDCF = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == newLCM.Designcomponentid).Select(x => x.Designcomponentfamilyid).FirstOrDefault();

                foreach (var networkElementAssociated in nodes)
                {
                    isDuplicateElementMigrate = false;
                    ///(LCM R8 Part 3 requirements)
                    ///Check for refactor use cases
                    if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Refactor)
                    {
                        await _resourceKeyMasterManager.UpdateResourcekeyMasterForRefactor(networkElementAssociated.Swresourcekey, plannedDCF, newLCM.Opcoid);
                        await _resourceKeyMasterManager.UpdateResourcekeyMasterForRefactor(networkElementAssociated.Hwresourcekey, plannedDCF, newLCM.Opcoid);
                    }
                    else if (currentDCF != plannedDCF)
                    {
                        string swResourceKey = string.Empty;
                        string hwResourceKey = string.Empty;

                        ///Passing the new LCM Entity design component will validate against the existing entry
                        ///and there will not be an entry, if the DCF changes so a new resourcekey will be assigned.
                        var AssetKeys = await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforAssets(newLCM.Designcomponentid, networkElementAssociated.Opcoid,
                            networkElementAssociated.Elementname, newLCM.Buildbagid);
                        if (AssetKeys != null)
                        {
                            swResourceKey = AssetKeys[(int)ResourceTypesKey.SWAsset];
                            hwResourceKey = AssetKeys[(int)ResourceTypesKey.HWAsset];
                            networkElementAssociated.Swresourcekey = swResourceKey;
                            networkElementAssociated.Hwresourcekey = hwResourceKey;
                        }
                    }
                    ///End of (LCM R8 Part 3 requirements)
                    networkElementAssociated.Designcomponentid = newLCM.Designcomponentid;
                    networkElementAssociated.Lcmengineeringid = newLCM?.Lcmengineeringid;

                    #region // Assured Logic 
                    
                        // Implementing IsAssured Logic
                        await UpdateAssuredStatus(networkElementAssociated);
                    
                    #endregion

                    _repositoryWrapper.NetworkElementAsPlanned.Update(networkElementAssociated);

                    string dcfActivityDescription = ConstantValueFilter.assetMigrated;
                    if (isModernizeSuccessorFlow) dcfActivityDescription = string.Empty;
                    _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(networkElementAssociated, dcfActivityDescription);



                }
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();
            }

        }

        public async Task UpdateNetworkElement(List<NetworkElementAssociated> networkElement, long designComponentId)
        {

            var ids = networkElement.Select(x => x.Id);
            foreach (var networkElementAssociated in await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => ids.Contains(x.Networkelementasplannedid)).ToListAsync())
            {
                var lcm = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == designComponentId && x.Opcoid == networkElementAssociated.Opcoid && x.Archived != true).FirstOrDefault();
                bool isDuplicateElementMigrate = false;
                networkElementAssociated.Designcomponentid = designComponentId;
                if (lcm != null)
                {
                    networkElementAssociated.Lcmengineeringid = lcm?.Lcmengineeringid;
                    networkElementAssociated.Buildbagid = lcm.Buildbagid;
                    #region #1268 After Migration to new LCM, if Asset/Element Name is same  in both Previous and New LCM. We need to display error to User.
                    var duplicatedElementNameAssociatedExistsLCM = await _commonManager.CheckDuplicatesAssetExistsForMigration(networkElementAssociated.Opcoid,
                        designComponentId, networkElementAssociated.Elementname);
                    if (duplicatedElementNameAssociatedExistsLCM != 0)
                    {
                        duplicateAssetNamesInLCM.Add(new FilterValueDtoKeyValueList
                        {
                            Key = (int)duplicatedElementNameAssociatedExistsLCM,
                            Value = networkElementAssociated.Elementname
                        });

                        isDuplicateElementMigrate = true;
                        var removedDeployStatusId = (short)LCMEngineeringRulesExtension.GetAssetDeploymentStatusBasedOnParameter
                       (ConstantValueFilter.Removed, _repositoryWrapper)?.FirstOrDefault().Key;
                        networkElementAssociated.Deploymentstatusid = removedDeployStatusId;
                    }
                    #endregion
                }

                #region // Assured Logic 

                // Implementing IsAssured Logic
                await UpdateAssuredStatus(networkElementAssociated);

                #endregion

                _repositoryWrapper.NetworkElementAsPlanned.Update(networkElementAssociated);

                string dcfActivityDescription = ConstantValueFilter.assetMigrated;
                if (isDuplicateElementMigrate) dcfActivityDescription = ConstantValueFilter.duplicateAssetMigrated;


                _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(networkElementAssociated, dcfActivityDescription);

            }

            await _repositoryWrapper.SaveAsync();
        }
        public async Task<ResultDto<UpdatePlannedActivityStatusDto>> GetUpdatePlannedActivityStatus(long id, PlannedActivityTypeForEnum plannedActivityTypeFor , bool isDaAssetMigration = false)
        {
            var data = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == id)
                .Include(x => x.Lcmengineering)
                .Include(x => x.Networkelementasplanned)
                .Include(x => x.Designaspect)
                .Include(x => x.Activitystatus)
                .Include(x => x.Planningactivitystatus)
                .Include(x => x.Deliverystatus).Include(x => x.Plannedactivityresource)
                .Include(x => x.Responsibilityphase)

                .FirstOrDefault();

            var entity = PlannedActivityMapper.Get(data);
            var lista = new Dictionary<long, string>();

            var dto = new UpdatePlannedActivityStatusDto();



            dto.DesignComponentResource = new Dictionary<long, string>();
            dto.DesignComponentFamilyResource = new Dictionary<long?, string>();

            dto.RuleLinkedDc = data.Plannedactivityresource.Rulelinkeddc;


            if (entity.ResponsibilityPhase != null && entity.ResponsibilityPhase.ResponsibilityPhaseDescription == ConstantValueFilter.engineering)
            {
                dto.InEngineeringPhase = ConstantValueFilter.isTrue;
            }
            ///ELEMENT COUNT
            if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.LcmEngineering)
            {
                dto.DesignComponentId = entity.LcmEngineering.DesignComponentId;

                var dcResource = _repositoryWrapper.DesignComponent
                  .FindByCondition(x => x.Designcomponentid == dto.DesignComponentId, ConstantValueFilter.isTrue)
                  .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                  .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                  .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                  .toDesignComponentResource(_repositoryWrapper);

                var name = dcResource[(long)dto.DesignComponentId];
                if (data.Lcmengineering.Isreleasedetailunknown == ConstantValueFilter.isTrue)
                {
                    ///Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'  -- July 3 -24
                    dto.DesignComponentResource =
                        (_repositoryWrapper.DesignComponent
                        .FindByCondition(x => x.Designcomponentfamilyid == data.Lcmengineering.Designcomponentfamilyid)
                        .ToDictionary(x => x.Designcomponentid, x => x.ToDesignComponentName(_repositoryWrapper))
                         ).Where(
                    x => !x.Value.ToLower().Contains(ConstantValueFilter.Unknown)).ToDictionary(x => x.Key, x => x.Value);


                }
                else if (data.Ispareleasedetailunknown.Value)
                {
                    dto.DesignComponentResource = (_repositoryWrapper.DesignComponent
                        .FindByCondition(x => x.Designcomponentfamilyid == data.Lcmengineering.Designcomponentfamilyid && x.Designcomponentid != data.Lcmengineering.Designcomponentid)
                        .ToDictionary(x => x.Designcomponentid, x => x.ToDesignComponentName(_repositoryWrapper))
                         ).Where(
                    x => !x.Value.ToLower().Contains(ConstantValueFilter.Unknown)).ToDictionary(x => x.Key, x => x.Value);
                }
                else
                {
                    dto.DesignComponentResource.Add(entity.LcmEngineering.DesignComponentId, name);
                }

                ///1480 Hide  Removed Assets  in Node selection /Update Settings Planned Activity Popup
                dto.ElementCount = entity.LcmEngineering.ElementCount;
                var nodes = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x =>
              x.Opcoid == entity.LcmEngineering.OpCoId &&
              x.Designcomponentid == entity.LcmEngineering.DesignComponentId && x.Deploymentstatus.Deploymentstatus.ToLower() != ConstantValueFilter.Removed)
              .Include(x => x.Environment)
              .Include(x => x.Location)
              .Include(x => x.Deploymentstatus)
              .ToListAsync();

                dto.StartNodesInProd = nodes.Where(x => x.Environment.Environment.ToLower() == ConstantValueFilter.production
                )
                    .Select(p => NetworkElementAsPlannedMapper.Get(p))
                    .Select(x => new NetworkElementAssociated
                    {
                        Id = x.NetworkElementAsPlannedId,
                        LocationId = x.Location.LocationId,
                        Location = x.Location.LocationDescription,
                        Enviroment = x.Environment.EnvironmentDescription,
                        EnviromentId = x.Environment.EnvironmentId,
                        ElementName = x.ElementName,
                        AssetsStatus = x.DeploymentStatus.DeploymentStatusDescription,
                        AssetsStatusId = x.DeploymentStatus.DeploymentStatusId,
                        IsFinalAsset = x.IsFinalAsset
                    }).ToList();

                dto.StartNodesInLab = nodes.Where(x => x.Environment.Environment.ToLower() != ConstantValueFilter.production)
                    .Select(p => NetworkElementAsPlannedMapper.Get(p))
                    .Select(x => new NetworkElementAssociated
                    {
                        Id = x.NetworkElementAsPlannedId,
                        LocationId = x.Location.LocationId,
                        Location = x.Location.LocationDescription,
                        Enviroment = x.Environment.EnvironmentDescription,
                        EnviromentId = x.Environment.EnvironmentId,
                        ElementName = x.ElementName,
                        AssetsStatus = x.DeploymentStatus.DeploymentStatusDescription,
                        AssetsStatusId = x.DeploymentStatus.DeploymentStatusId,
                        IsFinalAsset = x.IsFinalAsset
                    }).ToList();
                var lcm = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Archived != true && x.Designcomponentid == entity.LcmEngineering.DesignComponentId && x.Opcoid == (entity.LcmEngineering.OpCoId ?? 0))
                    .Include(x => x.PlannedactivitiesLcmengineering)
                    .ThenInclude(x => x.Activitystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource).ToList();

                if (dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.New_NFxI_Solution || dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.New_System_HW_SW_Solution
                    || dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.Decommission_Service_Node || dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.Modernize_Solution_Successor_Network)
                {
                    dto.AssetLiveStatusDate = nodes.Select(x => x.Assetlivestatusdate).FirstOrDefault();
                    dto.AssetDecommissionedDate = nodes.Select(x => x.Assetdecommissioneddate).FirstOrDefault();
                }

                foreach (var item in lcm)
                {
                    if (item.PlannedactivitiesLcmengineering.Count() > 0)
                    {
                        lista = item.PlannedactivitiesLcmengineering.Where(y => y.Deleted == false).ToDictionary(x => x.Plannedactivityid, x =>
                            $"{x.Plannedimplementationyear} | {x.Plannedactivityresource?.Plannedactivityresource} | {x.Activitydetails} | {x.Activitystatus?.Activitystatus} | {x.Deliverystatus?.Deliverystatus}");
                    }

                }
                dto.OpCoId = entity.LcmEngineering.OpCoId ?? 0;

                dto.NumberOfNodesInput = entity.LcmEngineering.NumberOfNodes;
                dto.NumberOfNodesInLabInput = entity.LcmEngineering.NumberOfNodesInLab;
                if(dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.Upgrade_HardwareTypes)
                {
                    dto.PlannedHardwareTypeId = _repositoryWrapper.ClusterUpGradeStatusRepository.FindByCondition(x => x.Plannedactivityid == id).FirstOrDefault().Hardwaretype;
                    dto.PlannedHardwareTypeResource = _plannedActivityManager.GetHardwareMhwResource(dto.OpCoId, (short)dto.DesignComponentId);
                }

            }
            else if (plannedActivityTypeFor == PlannedActivityTypeForEnum.AddAsset || plannedActivityTypeFor == PlannedActivityTypeForEnum.EditAsset)
            {
                dto.DesignComponentId = entity?.NetworkElementAsPlanned?.DesignComponentId;
                var dcResource = _repositoryWrapper.DesignComponent
                  .FindByCondition(x => x.Designcomponentid == dto.DesignComponentId)
                  .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                  .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                  .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                  .toDesignComponentResource(_repositoryWrapper);

                var name = dcResource[(long)dto.DesignComponentId];

                dto.DesignComponentResource.Add(entity.NetworkElementAsPlanned.DesignComponentId, name);
                var assets = _repositoryWrapper.NetworkElementAsPlanned
                  .FindByCondition(x => x.Designcomponentid == entity.NetworkElementAsPlanned.DesignComponentId && x.Opcoid == entity.NetworkElementAsPlanned.OpCoId)
                  .Include(x => x.Plannedactivities)
                  .ThenInclude(x => x.Activitystatus)
                  .Include(x => x.Plannedactivities).ThenInclude(x => x.Planningactivitystatus)
                  .Include(x => x.Plannedactivities).ThenInclude(x => x.Deliverystatus)
                  .Include(x => x.Plannedactivities).ThenInclude(x => x.Plannedactivityresource).ToList();


                foreach (var item in assets)
                {
                    if (item.Plannedactivities.Count() > 0)
                    {
                        lista = item.Plannedactivities.Where(y => y.Deleted == false).ToDictionary(x => x.Plannedactivityid, x =>
                            $"{x.Plannedimplementationyear} | {x.Plannedactivityresource?.Plannedactivityresource} | {x.Activitydetails} | {x.Activitystatus?.Activitystatus} | {x.Deliverystatus?.Deliverystatus}");
                    }

                }
                dto.OpCoId = entity.NetworkElementAsPlanned.OpCoId;
            }            
            else if (plannedActivityTypeFor == PlannedActivityTypeForEnum.DesignAspect)
            {
                dto.DesignComponentFamilyId = entity?.DesignAspect?.DesignComponentFamilyId;
                var dcfResource = _repositoryWrapper.DesignComponent
                  .FindByCondition(x => x.Designcomponentfamilyid == dto.DesignComponentFamilyId)
                  .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                  .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                  .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                  .toDesignComponentFamilyResource(_repositoryWrapper);

                var name = dcfResource[(long)dto.DesignComponentFamilyId];

                dto.DesignComponentFamilyResource.Add(entity.DesignAspect.DesignComponentFamilyId, name);

                var designAspects = _repositoryWrapper.DesignAspectRepository
                  .FindByCondition(x => x.Designcomponentfamilyid == entity.DesignAspect.DesignComponentFamilyId && x.Opcoid == entity.DesignAspect.OpCoId)
                  .Include(x => x.Plannedactivities)
                  .ThenInclude(x => x.Activitystatus)
                  .Include(x => x.Plannedactivities).ThenInclude(x => x.Planningactivitystatus)
                  .Include(x => x.Plannedactivities).ThenInclude(x => x.Deliverystatus)
                  .Include(x => x.Plannedactivities).ThenInclude(x => x.Plannedactivityresource).ToList();


                foreach (var item in designAspects)
                {
                    if (item.Plannedactivities.Count() > 0)
                    {
                        lista = item.Plannedactivities.Where(y => y.Deleted == false).ToDictionary(x => x.Plannedactivityid, x =>
                            $"{x.Plannedimplementationyear} | {x.Plannedactivityresource?.Plannedactivityresource} | {x.Activitydetails} | {x.Activitystatus?.Activitystatus} | {x.Deliverystatus?.Deliverystatus}");
                    }

                }
                dto.OpCoId = (short)entity.DesignAspect.OpCoId;
            }
            else
            {

            }


            if (dto?.DesignComponentId != null && !dto.DesignComponentResource.ContainsKey((long)dto.DesignComponentId))
            {
                var dc = _repositoryWrapper.DesignComponent.FindByCondition(
                        x => x.Designcomponentid == dto.DesignComponentId,
                        includeDeleted: true)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).SingleOrDefault();
                if (dc != null)
                {
                    if (data.Lcmengineering.Isreleasedetailunknown == true)
                    {
                        ///Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'  -- July 3 -24
                        dto.DesignComponentResource = (_repositoryWrapper.DesignComponent
                            .FindByCondition(x => x.Designcomponentfamilyid == data.Lcmengineering.Designcomponentfamilyid)
                            .ToDictionary(x => x.Designcomponentid, x => x.ToDesignComponentName(_repositoryWrapper))
                            ).Where(
                    x => !x.Value.ToLower().Contains(ConstantValueFilter.Unknown)).ToDictionary(x => x.Key, x => x.Value);


                    }
                    else if (data.Ispareleasedetailunknown.Value)
                    {
                        dto.DesignComponentResource = (_repositoryWrapper.DesignComponent
                            .FindByCondition(x => x.Designcomponentfamilyid == data.Lcmengineering.Designcomponentfamilyid && x.Designcomponentid != data.Lcmengineering.Designcomponentid)
                            .ToDictionary(x => x.Designcomponentid, x => x.ToDesignComponentName(_repositoryWrapper))
                             ).Where(
                        x => !x.Value.ToLower().Contains(ConstantValueFilter.Unknown)).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else
                    {
                        dto.DesignComponentResource.Add(dc.Designcomponentid, dc.toDesignComponentNameLcm(_repositoryWrapper));
                    }

                }
            }
            else if (dto?.DesignComponentFamilyId != null && !dto.DesignComponentFamilyResource.ContainsKey((long)dto.DesignComponentFamilyId))
            {
                var dcf = _repositoryWrapper.DesignComponent.FindByCondition(
                            x => x.Designcomponentfamilyid == dto.DesignComponentFamilyId,
                            includeDeleted: true)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).SingleOrDefault();

                if (dcf != null)
                {
                    dto.DesignComponentFamilyResource.Add(dcf.Designcomponentfamilyid, dcf.toDesignComponentFamily(_repositoryWrapper));
                }

            }
            else
            {

            }

            var opocResource = _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            dto.OpCoResource = new Dictionary<short, string>();
            if (dto.OpCoId != 0) dto.OpCoResource.Add(dto.OpCoId, opocResource[dto.OpCoId]);
            if (!dto.OpCoResource.ContainsKey(dto.OpCoId))
            {
                var opCo = _repositoryWrapper.OpCo.FindByCondition(
                    x => x.Opcoid == dto.OpCoId, true).SingleOrDefault();
                if (opCo != null)
                {
                    dto.OpCoResource.Add(opCo.Opcoid, opCo.Opco);
                }
            }

            dto.PlanningActivityDetailsResourceId = entity.PlannedActivityId;

            if (entity.DesignComponentId != null)
            {
                var resource = _repositoryWrapper.DesignComponent.FindAll();
                var dc = resource.Where(x => x.Designcomponentid == entity.DesignComponentId);
            }

            dto.PlanningActivityDetailsResource = lista;

            var allSettings = _repositoryWrapper.SettingsUpdatePlannedActivity
                                .FindByCondition(x => x.Plannedactivitytypefor == (short)plannedActivityTypeFor && x.Plannedactivityresourceid == data.Plannedactivityresourceid)
                                .Include(x => x.ModificationuserNavigation)
                                .Include(x => x.Settingupdateplannedactivitylcmdeploymentstatus)
                                .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus)
                                .Select(p => SettingsUpdatePlannedActivityMapper.Get(p))
                                .ToList();
            var settid = allSettings.SingleOrDefault(x => x.PlannedActivityTypeFor == (short)plannedActivityTypeFor &&
                                                            x.DeliveryStatusId == entity.DeliveryStatusId &&
                                                            x.PlanningActivityResourceId == data.Plannedactivityresourceid);

            if(isDaAssetMigration)
            {
                var deliveryLastStageId = allSettings.OrderBy(x => x.Order) .LastOrDefault();
                dto.SettingsUpdatePlannedActivityId = (deliveryLastStageId != null) ? deliveryLastStageId.SettingsUpdatePlannedActivityId : (short)0;
            } 
            else
                dto.SettingsUpdatePlannedActivityId = (settid != null) ? settid.SettingsUpdatePlannedActivityId : (short)0;


            var deliveryStatusResource = _repositoryWrapper.DeliveryStatus.FindAll();
            var planningActivityStatusResource = _repositoryWrapper.PlanningActivityStatus.FindAll();
            var budgetAv = _repositoryWrapper.BudgetAvailability.FindAll();
            dto.SettingsUpdatePlannedActivityResource = new Dictionary<short, SettingsUpdatePlannedActivityDtoUpdate>();
            long lcmInServiceId = 0;
            int order = 0;
            long trafficId = 0;
            long assetInserviceId = 0;
            if (dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.New_NFxI_Solution || dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.New_System_HW_SW_Solution)
            {
                order = allSettings.OrderBy(x => x.Order).Select(x => x.Order).LastOrDefault();
            }

            if (dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.Decommission_Service_Node || dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.Modernize_Solution_Successor_Network)
            {
                trafficId = _commonManager.GetLcmDeploymentStatusId(ConstantValueFilter.trafficFree);
                assetInserviceId = _commonManager.GetLcmDeploymentStatusId(ConstantValueFilter.inService);
                lcmInServiceId = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Description.ToLower().Replace(" ", "") == ConstantValueFilter.inService).FirstOrDefault().Id;

            }

            if(dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.Project_Plan)
            {
                var MultipleDcfEntities = await Task.Run(() => _repositoryWrapper.DaPlannedActivityDcfRepository.FindByCondition(x => x.Plannedactivityid == entity.PlannedActivityId).Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ToList());

                if (MultipleDcfEntities != null && MultipleDcfEntities.Any())
                {
                    dto.DaPlannedActivtyDcfDto = MultipleDcfEntities.Select(da => new DaPlannedActivtyDcfDto
                    {
                        DaPlannedActivityDcfId = da.Daplannedactivitydcfid,
                        DcfStatus = da.Dcfstatus == true ? ConstantValueFilter.completed : ConstantValueFilter.InProgress,
                        PlannedActivityId = da.Plannedactivityid,
                        DesignComponentfamilyId = da.Designcomponentfamilyid,
                        DesignComponentFamilyName = da.Designcomponentfamily.DCFName(_repositoryWrapper)
                    }).ToList();
                }
            }

            if(dto.RuleLinkedDc == (long)PlannedActivityResourceEnum.Service_Planned)
            {
                var servicePlanEntity = await _repositoryWrapper.ServicePlanRepository.FindByCondition(x => x.Serviceplanid == entity.Serviceplanid).FirstOrDefaultAsync();
                if (servicePlanEntity != null)
                {
                    var serivePlnaDcfEntities = await _repositoryWrapper.ServicePlanDcfMappingRepository.FindByCondition(f => f.Serviceplanid == servicePlanEntity.Serviceplanid).Include(x => x.Dcf).ToListAsync();
                    if (serivePlnaDcfEntities != null && serivePlnaDcfEntities.Count > 0)
                    {
                        dto.ServicePlanDcfDto = serivePlnaDcfEntities.Select(sp => new ServicePlanGridDto
                        {
                            ServicePlanId = sp.Serviceplanid.Value,
                            ServicePlanDcfMappingId = sp.Serviceplandcfmappingid,
                            DCFId = sp.Dcfid,
                            DCFName = sp?.Dcf.DCFName(_repositoryWrapper),
                            Program = entity.DeliveryProjectName,
                            Status = sp.Status.ToString(),
                            DcfStatus = ConstantValueFilter.daMigrationStatusCode,
                            PlannedActivityId = entity.PlannedActivityId,

                        }).ToList();
                    }
                }
            }

            foreach (var settings in allSettings)
            {
                var settingDto = new SettingsUpdatePlannedActivityDtoUpdate()
                {
                    SettingsUpdatePlannedActivityId = settings.SettingsUpdatePlannedActivityId,
                    SettingsUpdatePlannedActivityDescription = settings.SettingsUpdatePlannedActivityDescription,
                    PlanningActivityStatusId = entity.PlanningActivityStatusId,
                    BudgetAvailabilityId = settings.BudgetAvailabilityId,
                    LocalApproval = settings.LocalApproval,
                    DeliveryStatusId = settings.DeliveryStatusId,
                    Order = settings.Order,
                    MaxOrder = settings.MaxOrder,
                    RuleElementCount = settings.RuleElementCount,
                    LastModified = settings.ModificationDate,
                    LastModifiedBy = settings.ModificationUserEntity.Email,
                    DeliveryStatusResource = deliveryStatusResource.ToDictionary(x => x.Deliverystatusid,
                    x => x.Deliverystatus),
                    BudgetAvaibilityResource = budgetAv.ToDictionary(x => x.Budgetavailabilityid, x => x.Description),
                    PlanningActivityStatusResource =
                    planningActivityStatusResource.ToDictionary(x => x.Planningactivitystatusid,
                    x => x.Planningactivitystatus),
                    SpecifyDC = (bool)settings.SpecifyDC,
                    IsReleaseDetailsUnknown = data.Lcmengineering != null ? (bool)data.Lcmengineering.Isreleasedetailunknown : !ConstantValueFilter.isTrue,
                    IsPAReleaseDetailsUnknown = data.Ispareleasedetailunknown != null ? data.Ispareleasedetailunknown.Value : false,
                    NeedPlannedAsset = settings.NeedPlannedAsset == null ? false : (bool)settings.NeedPlannedAsset,
                    IsRollback = (bool)settings.IsRollback,
                    IsLiveStatusDateAvailable = (settings.Order == order || (settings?.SettingUpdatePlannedActivityLcmDeploymentStatus?.FirstOrDefault()?.LcmDeploymentStatusId == lcmInServiceId &&
                                                 settings?.SettingUpdatePlannedActivityAssetDeploymentStatus?.Count == 0)) ? true : false,
                    IsDecommissionedDateAvailable = (settings?.SettingUpdatePlannedActivityLcmDeploymentStatus?.FirstOrDefault()?.LcmDeploymentStatusId == lcmInServiceId &&
                                                     settings?.SettingUpdatePlannedActivityAssetDeploymentStatus?.FirstOrDefault()?.AssetDeploymentStatusId == trafficId) ? true : false,

                };
                if (!settingDto.DeliveryStatusResource.ContainsKey(settingDto.DeliveryStatusId))
                {
                    var ds = _repositoryWrapper.DeliveryStatus.FindByCondition(
                        x => x.Deliverystatusid == settingDto.DeliveryStatusId,
                        includeDeleted: ConstantValueFilter.isTrue).SingleOrDefault();
                    if (ds != null)
                    {
                        settingDto.DeliveryStatusResource.Add(ds.Deliverystatusid, ds.Deliverystatus);
                    }
                }

                if (settingDto.BudgetAvailabilityId != 0 && !settingDto.BudgetAvaibilityResource.ContainsKey(settingDto.BudgetAvailabilityId))
                {
                    var ba = _repositoryWrapper.BudgetAvailability.FindByCondition(
                        x => x.Budgetavailabilityid == settingDto.BudgetAvailabilityId,
                        includeDeleted: ConstantValueFilter.isTrue).SingleOrDefault();
                    if (ba != null)
                    {
                        settingDto.BudgetAvaibilityResource.Add(ba.Budgetavailabilityid, ba.Description);
                    }
                }

                if (!settingDto.PlanningActivityStatusResource.ContainsKey(settingDto.PlanningActivityStatusId))
                {
                    var pla = _repositoryWrapper.PlanningActivityStatus.FindByCondition(
                        x => x.Planningactivitystatusid == settingDto.PlanningActivityStatusId,
                        includeDeleted: ConstantValueFilter.isTrue).SingleOrDefault();
                    if (pla != null)
                    {
                        settingDto.PlanningActivityStatusResource.Add(pla.Planningactivitystatusid, pla.Planningactivitystatus);
                    }
                }

                dto.SettingsUpdatePlannedActivityResource.Add(settings.SettingsUpdatePlannedActivityId, settingDto);
                var crossSettings = _repositoryWrapper.CrossSettingsUpdatePlannedActivity.FindAll().Select(p => CrossSettingsUpdatePlannedActivityMapper.Get(p)).ToList();
                dto.CrossSettingscResource = crossSettings.Where(x => x.Deleted == false).ToDictionary(x => x.Id, x =>
                        new CrossSettingsDto()
                        {
                            Input = x.SettingsUpdatePlannedActivityInId,
                            Output = x.SettingsUpdatePlannedActivityOutId,
                            Rule = x.CrossSettingsRule,
                        });
            }

            return new ResultDto<UpdatePlannedActivityStatusDto>()
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = dto
            };

        }

        public async Task<ResultDto> GetPlannedActivityListForUpdatePlannedActivityStatus(long dcId, short opcoId, short plannedActivityTypeId, short? plannedActivityTypeFor = null)
        {
            if (dcId != 0 && opcoId != 0 && plannedActivityTypeId != 0)
            {
                var lcmList = new Dictionary<long, string>();
                var result = new Dictionary<long, string>();

                var assetList = new Dictionary<long, string>();
                if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.LcmEngineering)
                {
                    var lcmPlannedActivities = await _repositoryWrapper.PlannedActivity
                   .FindByCondition(x => x.Archived != true && x.Lcmengineeringid != null && x.Lcmengineering.Designcomponentid == dcId
                   && x.Lcmengineering.Opcoid == opcoId && x.Plannedactivityresourceid == plannedActivityTypeId && x.Deleted == false)
                   .Include(x => x.Lcmengineering)
                   .Include(x => x.Plannedactivityresource)
                   .Include(x => x.Activitystatus)
                    .Include(x => x.Deliverystatus)
                   .ToListAsync();

                    lcmList = (lcmPlannedActivities != null && lcmPlannedActivities.Count() > 0) ? (lcmPlannedActivities.ToDictionary(x => x.Plannedactivityid, x =>
                           $"{((DateTime)x.Plannedcompletion).ToString("MMM")} - {((DateTime)x.Plannedcompletion).ToString("yyyy")} | {x.Plannedactivityresource?.Plannedactivityresource} | {x.Activitydetails} | {x.Activitystatus?.Activitystatus} | {x.Deliverystatus?.Deliverystatus}")) : null;

                    result = lcmList;
                }

                else
                {

                    var assetsPlannedActivities = _repositoryWrapper.PlannedActivity
                   .FindByCondition(x => x.Archived != true && x.Networkelementasplannedid != null && x.Networkelementasplanned.Designcomponentid == dcId
                   && x.Networkelementasplanned.Opcoid == opcoId && x.Plannedactivityresourceid == plannedActivityTypeId && x.Deleted == false)
                   .Include(x => x.Networkelementasplanned)
                   .Include(x => x.Plannedactivityresource)
                   .Include(x => x.Activitystatus)
                    .Include(x => x.Deliverystatus)
                   .ToList();


                    assetList = (assetsPlannedActivities != null && assetsPlannedActivities.Count() > 0) ? (assetsPlannedActivities.ToDictionary(x => x.Plannedactivityid, x =>
                               $"{x.Plannedimplementationyear} | {x.Plannedactivityresource?.Plannedactivityresource} | {x.Activitydetails} | {x.Activitystatus?.Activitystatus} | {x.Deliverystatus?.Deliverystatus}")) : null;

                    result = assetList;
                }

                if (result == null && (assetList == null || assetList.Count == 0))
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.NoPlannedActivity,
                        Warning = true,
                    };
                }

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = result

                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.NoPlannedActivity,
                    Warning = true,
                };
            }

        }

        public async Task<ResultDto> GetSettingUpdatePlannedActivityResource(short plannedActivityResourceId, short? plannedActivityTypeFor = null)
        {
            if (plannedActivityResourceId != 0)
            {
                if (plannedActivityTypeFor.HasValue)
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.GetInfoSuccess,
                        Warning = false,
                        Data = await Task.Run(() => _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivityresourceid == plannedActivityResourceId && x.Plannedactivitytypefor == plannedActivityTypeFor)
                          .ToDictionary(p => p.Settingsupdateplnactid, p => SettingsUpdatePlannedActivityMapper.Get(p)))
                    };
                }
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = await Task.Run(() => _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivityresourceid == plannedActivityResourceId)
                           .ToDictionary(p => p.Settingsupdateplnactid, p => SettingsUpdatePlannedActivityMapper.Get(p)))
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.NoPlannedActivity,
                    Warning = true,
                };
            }

        }

        public async Task<UpdatePlannedActivityStatusDto> GetCreateUpdatePlannedActivityStatus(List<short> _opcoList)
        {
            var opocResource = await Task.Run(() => _opcoList!=null && _opcoList.Count > 0? _repositoryWrapper.OpCo.FindByCondition(x=>_opcoList.Contains(x.Opcoid)):_repositoryWrapper.OpCo.FindAll());
            var settings = _repositoryWrapper.SettingsUpdatePlannedActivity.FindAll().Select(p => SettingsUpdatePlannedActivityMapper.Get(p));
            var deliveryStatusResource = _repositoryWrapper.DeliveryStatus.FindAll();
            var planningActivityStatusResource = _repositoryWrapper.PlanningActivityStatus.FindAll();
            var budgetAvaibilityResource = _repositoryWrapper.BudgetAvailability.FindAll();
            var crossSettings = _repositoryWrapper.CrossSettingsUpdatePlannedActivity.FindAll().ToList();
            var plannedActivityResource = _repositoryWrapper.PlannedActivityResourceRepository
                .FindByCondition(x => x.Forlcm == true || x.Foraddasset == true || x.Foreditasset == true);
            var model = new UpdatePlannedActivityStatusDto()
            {
                OpCoResource = opocResource.ToDictionary(x => x.Opcoid, x => x.Opco),
                PlannedActivityTypeResource = plannedActivityResource.ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource),
                SettingsUpdatePlannedActivityResource = null
            };

            return model;

        }

        public async Task<ResultDto> GetPlannedActivityTypeswithDcIdAndOpcoId(long dcId, short opcoId)
        {
            if (dcId != 0 && opcoId != 0)
            {
                var lcmDic = new Dictionary<short, string>();

                var assetDic = new Dictionary<short, string>();

                var result = new Dictionary<short, string>();

                var lcmplannedActivities = await Task.Run(() => _repositoryWrapper.PlannedActivity
                    .FindByCondition(x => x.Archived != true && x.Lcmengineeringid != null && x.Lcmengineering.Designcomponentid == dcId && x.Lcmengineering.Opcoid == opcoId &&
                    x.Deleted == false)
                    .Include(x => x.Lcmengineering)
                    .Include(x => x.Plannedactivityresource)
                    .ToList());

                var assetsplannedActivities = _repositoryWrapper.PlannedActivity
                    .FindByCondition(x => x.Archived != true && x.Networkelementasplannedid != null && x.Networkelementasplanned.Designcomponentid == dcId && x.Networkelementasplanned.Opcoid == opcoId &&
                    x.Deleted == false)
                    .Include(x => x.Networkelementasplanned)
                    .Include(x => x.Plannedactivityresource)
                    .ToList();

                if (lcmplannedActivities != null && lcmplannedActivities.Count() > 0)
                {
                    lcmDic = lcmplannedActivities.Select(x => new { x.Plannedactivityresource.Plannedactivityresourceid, x.Plannedactivityresource.Plannedactivityresource }).ToList().Distinct().ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource);
                }

                if (assetsplannedActivities != null && assetsplannedActivities.Count() > 0)
                {
                    assetDic = assetsplannedActivities.Select(x => new { x.Plannedactivityresource.Plannedactivityresourceid, x.Plannedactivityresource.Plannedactivityresource }).ToList().Distinct().ToDictionary(x => x.Plannedactivityresourceid, x => x.Plannedactivityresource);
                }

                result = lcmDic.Union(assetDic).ToDictionary(kvp1 => kvp1.Key, kvp1 => kvp1.Value);

                if (result != null && !result.Any())
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.NoPlannedActivity,
                        Warning = true,
                    };
                }


                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = result

                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.NoPlannedActivity,
                    Warning = true,
                };
            }

        }

        public async Task<ResultDto> CheckPlannedActivityTypefor(long plannedActivityTypeId, long designComponentId, long opCoId)
        {

            var result = new Dictionary<short, string>();
            var plannedActivities = await Task.Run(() => _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Archived != true && x.Plannedactivityresourceid == plannedActivityTypeId)
                .Include(x => x.Lcmengineering)
                .Include(x => x.Networkelementasplanned));
            var lcmPA = plannedActivities.Any(x => x.Lcmengineeringid != null && x.Lcmengineering.Designcomponentid == designComponentId
                   && x.Lcmengineering.Opcoid == opCoId);
            if (lcmPA)
            {
                result.Add((short)PlannedActivityTypeForEnum.LcmEngineering, ConstantValueFilter.lcmEngineering);
            }

            var assetPA = plannedActivities.Any(x => x.Networkelementasplannedid != null && x.Networkelementasplanned.Designcomponentid == designComponentId
                   && x.Networkelementasplanned.Opcoid == opCoId);
            if (assetPA)
            {
                result.Add((short)PlannedActivityTypeForEnum.EditAsset, ConstantValueFilter.asset);
            }

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = result
            };
        }

        #region Ticket 837 - UpdatePlannedActivityManager - code improvements
        public async Task UpdateLCMandPAOnSpecifiyDC(Lcmengineering originalLcm, Plannedactivities plannedActivityEntity, UpdatePlannedActivityStatusDto data, short assetInserivceId)
        {
            getExistPAEntityIfLCMUnknown = null;
            if (originalLcm != null)
            {
                currentLCMinDB = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == originalLcm.Lcmengineeringid).FirstOrDefault();


                var getUnknownDc = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == originalLcm.Designcomponentid).FirstOrDefault();
                ///Ticket 722 Create New LCM - Using Existing Unknown Resource LCM When User Specify the DC and LCM Have More than one PA 
                var newUnknowResourceLCM = new Lcmengineering();

                if (data.DesignComponentId != currentLCMinDB.Designcomponentid)
                {
                    if (originalLcm.Isreleasedetailunknown.Value)
                    {

                        #region Ticket 722 Create New LCM - Using Existing Unknown Resource LCM When User Specify the DC and LCM Have More than one PA                       
                        #region Ticket 797 - Release details unknown-LCM level_multiple PAs: 2 LCMs with same Opco & DC getting created when we specify same DC while rolling out the Planned activities
                        getExistPAEntityIfLCMUnknown = _lcmEngineeringManager.PlannedActivityExists(plannedActivityEntity.Plannedactivityid, data.DesignComponentId.Value,
                          currentLCMinDB.Opcoid, data.DesignComponentId.Value, plannedActivityEntity.Plannedactivityresourceid, true);

                        long dcfIdFromExistPAEntityForIsUnknowLcm = 0;

                        var movetoNewLCMPlannedActivityList = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid == plannedActivityEntity.Lcmengineeringid &&
             x.Plannedactivityid != plannedActivityEntity.Plannedactivityid).ToList();

                        if (getExistPAEntityIfLCMUnknown != null && (getExistPAEntityIfLCMUnknown?.Lcmengineering?.Archived == true)) getExistPAEntityIfLCMUnknown = null;

                        if (movetoNewLCMPlannedActivityList != null && movetoNewLCMPlannedActivityList.Count() > 0)
                            newUnknowResourceLCM = await _lcmEngineeringManager.CreateLCMAndRealtedTableRecordsForResourceUnknowLCM(plannedActivityEntity);


                        if (getExistPAEntityIfLCMUnknown != null) // When specifyDC for UnKnowLCM                             
                            await _plannedActivityManager.TransferRemainingPaEntityToExistingLCM(plannedActivityEntity.Plannedactivityid, (long)plannedActivityEntity.Lcmengineeringid, (long)getExistPAEntityIfLCMUnknown.Lcmengineeringid);

                        #endregion
                        if (getUnknownDc != null)
                        {
                            var getUnknownRelatedDCFLifeCycleEntity = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByCondition(x => x.Dcid == originalLcm.Designcomponentid
                            && x.Opcoid == originalLcm.Opcoid).FirstOrDefault();

                            if (getUnknownRelatedDCFLifeCycleEntity != null)
                            {
                                getUnknownRelatedDCFLifeCycleEntity.Dcid = data.DesignComponentId;
                                _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.Update(getUnknownRelatedDCFLifeCycleEntity);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();
                            }
                        }

                        if (getExistPAEntityIfLCMUnknown != null)
                        {
                            isExistPAEntityIfLCMUnknow = true;
                            originalLcm.Elementcount = false;
                            originalLcm.Numberofnodes = 0;
                            originalLcm.Numberofnodesinlab = 0;

                            plannedActivityEntity.Lcmengineeringid = getExistPAEntityIfLCMUnknown.Lcmengineeringid;
                            dcfIdFromExistPAEntityForIsUnknowLcm = (long)_repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid ==
                            getExistPAEntityIfLCMUnknown.Designcomponentid).FirstOrDefault().Designcomponentfamilyid;

                        }

                        var releasedetailUnkownrelatedassets = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Lcmengineeringid == originalLcm.Lcmengineeringid
                        ).ToList();



                        string getExistPAEntityLCMEntity = (getExistPAEntityIfLCMUnknown != null) ? Convert.ToString(_repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid
                        == getExistPAEntityIfLCMUnknown.Lcmengineeringid
                        && x.Lcmdeploymentstatus.Description.ToLower().Replace(" ", "") == ConstantValueFilter.InService)
                            .Include(x => x.Lcmdeploymentstatus).FirstOrDefault()?.Lcmdeploymentstatusid) : string.Empty;

                        #region #1658 - Archive LCM - Move Asset PA to Archive
                        var assetDeploymentStatus = await _dropdownDataServiceManager.GetAssetDeploymentStatus();
                        var inservice_Decommission_AssetDeploymentId = assetDeploymentStatus
                            .Where(x => ConstantValueFilter.assetDeploymentStatusForArchivePa.Any(y => y == x.Value.Replace(" ", "").ToLower()))
                            .Select(t => t.Key).ToList();
                        #endregion

                        foreach (var asset in releasedetailUnkownrelatedassets)
                        {
                            var existingAssetDeploymentStatusId = asset.Deploymentstatusid;
                            bool isExistPA = false;
                            bool isDuplicateElementMigrate = false;
                            if (getExistPAEntityIfLCMUnknown != null && isExistPAEntityIfLCMUnknow == ConstantValueFilter.isTrue
                                && asset.Isfinalasset == ConstantValueFilter.isTrue)
                            {
                                #region #1268 After Migration to new LCM, if Asset/Element Name is same  in both Previous and New LCM. We need to display error to User.
                                var duplicatedElementNameAssociatedExistsLCM = await _commonManager.CheckDuplicatesAssetExistsForMigration(asset.Opcoid,
                                    (long)getExistPAEntityIfLCMUnknown.Designcomponentid, asset.Elementname);
                                if (duplicatedElementNameAssociatedExistsLCM != 0)
                                {
                                    duplicateAssetNamesInLCM.Add(new FilterValueDtoKeyValueList
                                    {
                                        Key = (int)duplicatedElementNameAssociatedExistsLCM,
                                        Value = asset.Elementname
                                    });

                                    isDuplicateElementMigrate = true;
                                    var removedDeployStatusId = (short)LCMEngineeringRulesExtension.GetAssetDeploymentStatusBasedOnParameter
                                   (ConstantValueFilter.Removed, _repositoryWrapper)?.FirstOrDefault().Key;
                                    asset.Deploymentstatusid = removedDeployStatusId;
                                }
                                if (!string.IsNullOrEmpty(getExistPAEntityLCMEntity) && !isDuplicateElementMigrate)
                                    asset.Deploymentstatusid = Convert.ToInt16(assetInserivceId);
                                #endregion

                                isExistPA = true;
                                asset.Designcomponentid = (long)getExistPAEntityIfLCMUnknown.Designcomponentid;
                                if (dcfIdFromExistPAEntityForIsUnknowLcm != 0) asset.Designcomponentfamilyid = dcfIdFromExistPAEntityForIsUnknowLcm;
                                asset.Lcmengineeringid = (long)getExistPAEntityIfLCMUnknown.Lcmengineeringid;
                            }
                            else
                            {
                                asset.Designcomponentid = data.DesignComponentId.Value;
                            }

                            _repositoryWrapper.NetworkElementAsPlanned.Update(asset);

                            if (existingAssetDeploymentStatusId != asset.Deploymentstatusid &&
                       inservice_Decommission_AssetDeploymentId.Any(x => x == asset.Deploymentstatusid))
                                await _networkElementsAsPlannedManager.ArchiveAssetAddNodeAndDecommissionPA(asset.Networkelementasplannedid);

                            if (isExistPA == true && dcfIdFromExistPAEntityForIsUnknowLcm != 0)
                            {
                                string dcfActivityDescription = ConstantValueFilter.assetMigrated;
                                if (isDuplicateElementMigrate) dcfActivityDescription = ConstantValueFilter.duplicateAssetMigrated;

                                _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(asset, dcfActivityDescription);
                                if (asset.Isfinalasset == false)
                                {
                                    //This call required no need to check  asset Deployment status  - New Intial PA created
                                    await _networkElementsAsPlannedManager.ArchiveAssetAddNodeAndDecommissionPA(asset.Networkelementasplannedid);
                                    await NodeSelectionMethod(asset, plannedActivityEntity, asset.Designcomponentid);

                                }
                            }

                        }
                        await _repositoryWrapper.SaveAsync();
                        /// Update Numberofnode count details for assigned Old LCM
                        if (getExistPAEntityIfLCMUnknown != null && isExistPAEntityIfLCMUnknow == ConstantValueFilter.isTrue)
                        {
                            var assingedExistLCMEntity = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid
                            == getExistPAEntityIfLCMUnknown.Lcmengineeringid).FirstOrDefault();
                            if (assingedExistLCMEntity != null && (releasedetailUnkownrelatedassets != null && releasedetailUnkownrelatedassets.Count() > 0))
                            {
                                assingedExistLCMEntity.Elementcount = ConstantValueFilter.isTrue;
                                assingedExistLCMEntity.Numberofnodes = LCMEngineeringMapper.GetLcmEngineeringMapper(assingedExistLCMEntity).CountNetworkElementReleated(!ConstantValueFilter.isTrue, _repositoryWrapper);
                                assingedExistLCMEntity.Numberofnodesinlab = LCMEngineeringMapper.GetLcmEngineeringMapper(assingedExistLCMEntity).CountNetworkElementReleated(ConstantValueFilter.isTrue, _repositoryWrapper);

                                _repositoryWrapper.Lcmengineering.Update(assingedExistLCMEntity);
                                _repositoryWrapper.Save();

                            }
                        }
                        else
                        {
                            originalLcm.Numberofnodes = originalLcm.CountNetworkElementReleated(!ConstantValueFilter.isTrue, _repositoryWrapper);
                            originalLcm.Numberofnodesinlab = originalLcm.CountNetworkElementReleated(ConstantValueFilter.isTrue, _repositoryWrapper);
                            if (originalLcm.Numberofnodes > 0 || originalLcm.Numberofnodesinlab > 0)
                                originalLcm.Elementcount = ConstantValueFilter.isTrue;
                            else originalLcm.Elementcount = !ConstantValueFilter.isTrue;

                            originalLcm.Designcomponentid = data.DesignComponentId.Value;
                            originalLcm.Isreleasedetailunknown = !ConstantValueFilter.isTrue;
                            string newLCMResourceKey = _designComponentFamilyLifeCycleManager.CreateDCFLifecycleForIsReleaseDetailsUnknown(originalLcm).Result.Data.ToString();
                            originalLcm.Resourcekey = newLCMResourceKey;
                            _repositoryWrapper.Lcmengineering.Update(originalLcm);
                            _repositoryWrapper.Save();
                        }

                        plannedActivityEntity.Designcomponentid = data.DesignComponentId;
                        _repositoryWrapper.PlannedActivity.Update(plannedActivityEntity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        #endregion

                    }

                }
                try
                {
                    ///Ticket 722 Create New LCM - Using Existing Unknown Resource LCM When User Specify the DC and LCM Have More than one PA 
                    if ((getUnknownDc != null) && (getUnknownDc.Designcomponentid != originalLcm.Designcomponentid) && (newUnknowResourceLCM?.Lcmengineeringid == 0))
                    {
                        ///Ticket 712 Update PA  not working properly throw error while clicking An Operational radio button
                        _repositoryWrapper.DesignComponent.Delete(getUnknownDc);
                        _repositoryWrapper.Save();
                    }
                }
                catch
                {
                }
            }

        }

        public async Task UpdateAssetDeploymentStatusOnDecommission(Plannedactivities plannedActivityEntity, string settingsPaAssetDeploymentStatusId, short modernizeAssetStatusInserviceId, int modernizeSettingPaRule, bool isModernizeSuccessorFlow, UpdatePlannedActivityStatusDto updatePlannedActivityStatusDto = null)
        {
            #region // Asset status #412 Decommission Flow Implementation  - Decommission Flow April 17 2024

            var assetsStatusUpdateForDecommission = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Lcmengineeringid ==
            plannedActivityEntity.Lcmengineeringid).ToListAsync();

            if (!string.IsNullOrEmpty(settingsPaAssetDeploymentStatusId)  && (assetsStatusUpdateForDecommission != null && assetsStatusUpdateForDecommission.Count >0))
            {

                #region #1658 - Archive LCM - Move Asset PA to Archive
                var assetDeploymentStatus = await _dropdownDataServiceManager.GetAssetDeploymentStatus();
                var inservice_Decommission_AssetDeploymentId = assetDeploymentStatus
                    .Where(x => ConstantValueFilter.assetDeploymentStatusForArchivePa.Any(y => y == x.Value.Replace(" ", "").ToLower()))
                    .Select(t => t.Key).ToList();
                #endregion

                foreach (var asset in assetsStatusUpdateForDecommission)
                {
                    var existingAssetDeploymentStatusId = asset.Deploymentstatusid;
                    if (isModernizeSuccessorFlow) ///Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
                    {
                        if (asset.Deploymentstatusid != modernizeAssetStatusInserviceId && modernizeSettingPaRule == 1)
                        {
                            asset.Deploymentstatusid = asset.Deploymentstatusid;
                            //asset.Assetdecommissioneddate = updatePlannedActivityStatusDto.AssetDecommissionedDate != null ? updatePlannedActivityStatusDto.AssetDecommissionedDate : null;
                        }
                        else if (modernizeSettingPaRule == 0)
                            asset.Deploymentstatusid = Convert.ToInt16(settingsPaAssetDeploymentStatusId);
                    }
                    else
                    {
                        asset.Deploymentstatusid = Convert.ToInt16(settingsPaAssetDeploymentStatusId);
                        //asset.Assetlivestatusdate = updatePlannedActivityStatusDto.AssetLiveStatusDate != null ? updatePlannedActivityStatusDto.AssetLiveStatusDate : null;

                    }


                    _repositoryWrapper.NetworkElementAsPlanned.Update(asset);

                    if (existingAssetDeploymentStatusId != asset.Deploymentstatusid &&
                        inservice_Decommission_AssetDeploymentId.Any(x => x == asset.Deploymentstatusid))
                        await _networkElementsAsPlannedManager.ArchiveAssetAddNodeAndDecommissionPA(asset.Networkelementasplannedid);
                }
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();
            }

            #endregion
        }
        public async Task<Plannedactivities> UpdateProjectAndFinanceTabForPAEntity(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, bool InEngineeringPhase)
        {
            if (getExistPAEntityIfLCMUnknown == null && isExistPAEntityIfLCMUnknow != true)
            {
                plannedActivityEntity.Deliverystatusid = settingNew.Deliverystatusid;
                plannedActivityEntity.Planningactivitystatusid = settingNew.Planningactivitystatusid;
                plannedActivityEntity.Budgetavailabilityid = settingNew.Budgetavailabilityid;
                plannedActivityEntity.Localapproval = settingNew.Localapproval;

                var responsibilityPhase = _repositoryWrapper.ResponsibilityPhase.FindAll();
                if (InEngineeringPhase == true)
                {
                    plannedActivityEntity.Responsibilityphaseid =
                        responsibilityPhase.Single(x => x.Responsibilityphase == ConstantValueFilter.engineering).Responsibilityphaseid;
                }
                else
                {
                    plannedActivityEntity.Responsibilityphaseid =
                        responsibilityPhase.Single(x => x.Responsibilityphase == ConstantValueFilter.Operations).Responsibilityphaseid;
                }
                plannedActivityEntity = _plannedActivityManager.SetPlannedActivityValue(plannedActivityEntity);
                _repositoryWrapper.PlannedActivity.Update(plannedActivityEntity);
                await _repositoryWrapper.SaveAsync();
            }

            return plannedActivityEntity;
        }

        public async Task<short> FetchLcmDeploymentStatusIdByMinPlannedCompletion(long originalLcmId)
        {
            var activeLCMPAs = await Task.Run(() => _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Archived != true && x.Lcmengineeringid == originalLcmId));
            if (activeLCMPAs != null && activeLCMPAs.Count() > 0)
            {
                var earliestLCMPA = activeLCMPAs.FirstOrDefault(x => x.Plannedcompletion == activeLCMPAs.Min(p => p.Plannedcompletion));
                if (earliestLCMPA != null)
                {
                    var earliestLCMPASetting = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor ==
                    (short)PlannedActivityTypeForEnum.LcmEngineering && x.Plannedactivityresourceid == earliestLCMPA.Plannedactivityresourceid
                    && x.Deliverystatusid == earliestLCMPA.Deliverystatusid)
                      .Include(x => x.Deliverystatus)
                      .Include(x => x.Settingupdateplannedactivitylcmdeploymentstatus)?.FirstOrDefault();

                    var statusId = earliestLCMPASetting?.Settingupdateplannedactivitylcmdeploymentstatus?.FirstOrDefault()?.Lcmdeploymentstatusid;
                    short lcmDeploymentStatus = (short)((statusId == null) ? 0 : statusId);
                    return lcmDeploymentStatus;
                }
                return 0;
            }
            return 0;

        }


        #endregion

        #region  update isFinalAsset = false after NodeSelectin

        public async Task updateAssetIsFinalAssetFalseForNodeSection(List<NetworkElementAssociated> networkElementEntity)
        {

            foreach (var x in networkElementEntity)
            {
                if (x.Id != 0)
                {
                    var asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(y => y.Networkelementasplannedid == x.Id).FirstOrDefault();
                    if (asset != null)
                    {
                        asset.Isfinalasset = x.IsFinalAsset;
                        _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                        _repositoryWrapper.Save();
                        await _repositoryWrapper.ClearTracker();
                    }
                }
            }

        }

        #endregion

        #region ComponentUpgrade
        public async Task<ResultDto> ComponentUpgradeOld(Plannedactivities plannedactivities)
        {
            var buildBagEntity = _repositoryWrapper.BuildBagRepository.FindByCondition(x => x.Buildbagid == plannedactivities.Buildbagid).Include(x => x.Componentsoftwarebuildbags).FirstOrDefault();

            var buildBagVersion = await _buildBagManager.GenerateBagVersion(buildBagEntity.Bagdescription);

            var dcfId = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == plannedactivities.Designcomponentid).FirstOrDefault().Designcomponentfamilyid;

            Buildbags buildBag = new Buildbags
            {
                Bagdescription = buildBagEntity.Bagdescription,
                Bagversion = buildBagVersion,
                Opcoid = plannedactivities.Lcmengineering.Opcoid,
                Designcomponentfamilyid = dcfId,
            };
            _repositoryWrapper.BuildBagRepository.Create(buildBag);
            await _repositoryWrapper.SaveAsync();

            if (buildBag.Buildbagid != 0)
            {
                var componentSoftwareId = buildBagEntity.Componentsoftwarebuildbags.Select(x => x.Componentsoftwarebuildid).ToList();
                await _componentSoftwareBuildMappingManager.AddOrUpdateComponenetSoftwareBuildBagAsync(componentSoftwareId, buildBag.Buildbagid);

                await _designComponentFamilyLifeCycleManager.GenerateDCFEntryForComponenet((long)plannedactivities.Lcmengineering.Opcoid, (long)plannedactivities.Designcomponentid, (long)dcfId, plannedactivities.Buildbagid,
                (long)plannedactivities.Plannedactivityresourceid, ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 4).Text, false, true);

                await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforComponent((long)plannedactivities.Lcmengineering.Opcoid, (long)plannedactivities.Designcomponentid, buildBag.Buildbagid, true);

            }

            if (plannedactivities != null)
            {
                plannedactivities.Archived = ConstantValueFilter.isTrue;
                _repositoryWrapper.PlannedActivity.Update(plannedactivities);
                await _repositoryWrapper.SaveAsync();
            }



            return new ResultDto
            {
                Data = buildBag.Buildbagid,
                Info = ResultMessages.EntryUpdateSuccess
            };
        }

        public async Task<ResultDto> ComponentUpgrade(Plannedactivities plannedactivities, short deliveryStatusId)
        {
            try
            {
                var lcmEntity = plannedactivities.Lcmengineering;
                var existingBuildBagId = lcmEntity.Buildbagid;

                var dcfId = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == plannedactivities.Designcomponentid).FirstOrDefault().Designcomponentfamilyid;

                lcmEntity.Buildbagid = plannedactivities.Buildbagid;
                _repositoryWrapper.Lcmengineering.Update(lcmEntity);
                await _repositoryWrapper.SaveAsync();

                if (lcmEntity.Buildbagid != 0)
                {

                    await _designComponentFamilyLifeCycleManager.setResourceKeyNotInUseAndAddDcfLifeCycleEntry((long)lcmEntity.Opcoid, (long)plannedactivities.Designcomponentid, (long)dcfId, (long)plannedactivities.Plannedactivityresourceid, existingBuildBagId);
                    await _repositoryWrapper.ClearTracker();
                    await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforComponent((long)plannedactivities.Lcmengineering.Opcoid, (long)plannedactivities.Designcomponentid, plannedactivities.Buildbagid, true);

                }

                if (plannedactivities != null)
                {
                    plannedactivities.Deliverystatusid = deliveryStatusId;
                    plannedactivities.Archived = ConstantValueFilter.isTrue;
                    _repositoryWrapper.PlannedActivity.Update(plannedactivities);
                    await _repositoryWrapper.SaveAsync();
                }



                return new ResultDto
                {
                    Data = plannedactivities.Buildbagid,
                    Info = ResultMessages.EntryUpdateSuccess
                };
            }
            catch
            {
                return new ResultDto
                {
                    Data = plannedactivities.Buildbagid,
                    Info = ResultMessages.EntryAddUpdateFailed
                };
            }
        }


        #endregion

        #region DeliveryTracking Ms4Status update

        public async Task<ResultDto> UpdateMs4Status(Plannedactivities plannedactivities, Settingsupdateplannedactivity settingsupdateplannedactivity)
        {
            var paHasDeliveryTracking = _repositoryWrapper.DeliveryTrackingRepository
                                        .FindByCondition(x => x.Plannedactivityid == plannedactivities.Plannedactivityid).FirstOrDefault();

            var mileStoneStatus = await _commonManager.CalculateMSDuration(plannedactivities, _repositoryWrapper);
            if (paHasDeliveryTracking != null)
            {

                var msStatuses = settingsupdateplannedactivity.Milestonestatus;

                if (msStatuses == (int)MilestoneStatusEnum.MS1)
                {
                    paHasDeliveryTracking.Ms1baselinedate = plannedactivities.Startdate.Value.AddDays(mileStoneStatus.MS1Status * 7);
                }
                else if (msStatuses == (int)MilestoneStatusEnum.MS2)
                {
                    paHasDeliveryTracking.Ms2baselinedate = plannedactivities.Startdate.Value.AddDays(mileStoneStatus.MS2Status * 7);
                }
                else if (msStatuses == (int)MilestoneStatusEnum.MS3)
                {
                    paHasDeliveryTracking.Ms3baselinedate = plannedactivities.Startdate.Value.AddDays(mileStoneStatus.MS3Status * 7);
                }
                else if (msStatuses == (int)MilestoneStatusEnum.MS4)
                {
                    paHasDeliveryTracking.Ms1baselinedate = plannedactivities.Startdate.Value.AddDays(mileStoneStatus.MS4Status * 7);
                }
                else
                {

                }

                _repositoryWrapper.DeliveryTrackingRepository.Update(paHasDeliveryTracking);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
            }
            else
                return new ResultDto { Info = ResultMessages.EntryNotUpdate };

        }

        #endregion

        #region Infra Ready - Da PA LCM Creation
        public async Task<ResultDto> LcmCreationForInfraReadyDesignAspectPa(long paId)
        {
            return await _lcmEngineeringManager.CreateLcmForInfraReadyPA( paId);
        }

        #endregion
    }
}
