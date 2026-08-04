using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity.ExodusProgram;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.Entita.DaMigrationStatus;
using CAM.DataTransferObjects.Entita.DesignAspects;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.LookUp.DaPlannedActivityDcf;
using CAM.Enum;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.PlannedActivities
{
    public class DesignAspectPlannedActivityManger : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly CommonManager _commonManager;
        private readonly ILoggerManager _logger;
        private readonly  DaMigrationStatusManager _daMigrationStatusManager;
        private readonly PlatformMigrationManager _PlatformMigrationManager;
        public DesignAspectPlannedActivityManger(IHttpContextAccessor contextAccessor, IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, 
            CommonManager commonManager, ILoggerManager logger, DaMigrationStatusManager daMigrationStatusManager, PlatformMigrationManager PlatformMigrationManager)
            : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _commonManager = commonManager;
            _logger = logger;
            _daMigrationStatusManager =  daMigrationStatusManager;
            _PlatformMigrationManager = PlatformMigrationManager;
        }

        public async Task<ResultDto> AddDaMigrationEntry(Plannedactivities plannedActivityEntity )
        {
            var locationEntity = _repositoryWrapper.Location.FindByCondition(x => x.Opcoid == plannedActivityEntity.Opcoid).Select(x => x.Locationid).ToList();
             DaMigrationStatusAddUpdateDto  DaMigrationStatusEntity = new  DaMigrationStatusAddUpdateDto ();

             try
            {
                var existingDaMigrationEntity = _repositoryWrapper.DaMigrationStatusRepository.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid).ToList();

                foreach (var item in locationEntity)
                {
                    var isCheckDuplicateRecord = false;
                    if (existingDaMigrationEntity != null && existingDaMigrationEntity.Count >0)
                    {
                          isCheckDuplicateRecord = existingDaMigrationEntity.Where(x => x.Opcoid == plannedActivityEntity.Opcoid && x.Locationid == item).Any();
                       
                    }
                    if(!isCheckDuplicateRecord)
                    DaMigrationStatusEntity.daMigrationStatusDtoGrids.Add(new DaMigrationStatusDtoGrid
                    {
                        PlannedActivityId = plannedActivityEntity.Plannedactivityid,
                        OpcoId = (short)plannedActivityEntity.Opcoid,
                        LocationId = item,
                        StatusId = (short)ConstantValueFilter.daMigrationStatusCode.Where(x => x.Value == "Planned").FirstOrDefault().Key
                    });
                }

                await _daMigrationStatusManager.AddOrUpdateLocationAsync(DaMigrationStatusEntity);
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess 
                   
                };

            }
            catch(Exception ex)
            {
                _logger.LogError("Issue happen when try to Adding Design Aspect Location Migration  : " + ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }

        #region Infra Ready
        public async Task<ResultDto> ArchivePaWhenDaMigrationComplete(long PaId, short settingsPaId)
        {

            var daMigrationEntity = _repositoryWrapper.DaMigrationStatusRepository
                .FindByCondition(x => x.Plannedactivityid == PaId
                && x.Statusid != 3).Any();
            

            try
            {
                 if(!daMigrationEntity)
                {
                    var plannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == PaId, true, false)
                       .Include(x => x.Designaspect).FirstOrDefault();

                    var settingUpdatePa = _repositoryWrapper.SettingsUpdatePlannedActivity.
                               FindByCondition(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Infra_Readiness)
                              .ToList();

                    var infraReadyDeliveryStatusOrderId = settingUpdatePa
                                .Select(x => x.Order).Max();
                    var paOrder = settingUpdatePa.Where(x => x.Settingsupdateplnactid == settingsPaId).Select(x => x.Order).FirstOrDefault();

                    if(infraReadyDeliveryStatusOrderId == paOrder)
                    {
                        Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();

                        plannedActivity.Archived = true;
                        plannedActivity.Deliverystatusid = settingUpdatePa.Where(x => x.Settingsupdateplnactid == settingsPaId).Select(x => x.Deliverystatusid).FirstOrDefault();
                        plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;

                        await _commonManager.setArchiveStatusForBpt(plannedActivity);
                        _repositoryWrapper.PlannedActivity.Update(plannedActivity);

                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                   
                }
             
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess

                };

            }
            catch (Exception ex)
            {
                _logger.LogError("Issue happen when try to Archive Design Aspect Location Migration - Infra ready status : " + ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }

        #endregion

        #region Exodus 
        public async Task<ResultDto> ArchivePaWhenDaAssetMigrationComplete([FromBody] DaAssetMigrationAddUpdateDto dto, long paId, short settingsPaId)
        {

            try
            {
                
                var addUpdateAsset = await _PlatformMigrationManager.AddOrUpdateAssetMigrationAsync(dto, paId);
 
                return new ResultDto
                {
                    //unwanted UI sisplay

                };
          

        }
            catch (Exception ex)
            {
                _logger.LogError("Issue happen when try to Archive Design Aspect Asset Migration - Plaftform Migration status : " + ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }

        #endregion
        public async Task<ResultDto> ApplySettingUpdateRulesToDesignAspectPA(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, short? plannedActivityTypeFor,
            List<DaPlannedActivtyDcfDto> daPlannedActivtyDcfEntities = null, DaAssetMigrationAddUpdateDto _daAssetMigrationDto = null)
        {
            try
            {
                var prevDeliveryStatusIdInfraReady = plannedActivityEntity.Deliverystatusid;
                var prevArchivedInfraReady = plannedActivityEntity.Archived;

                if (plannedActivityEntity.Designaspectid != null || plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.DesignAspect)
                {

                    var plannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid, true, false)
                        .Include(x => x.Designaspect).FirstOrDefault();

                    var designAspectEntity = _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Id == plannedActivityEntity.Designaspectid).FirstOrDefault();

                    plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                    plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;


                    if (settingNew.Ruleelementcount != (int)ArchivingRuleEnum.ArchivePlannedActivityAndParentEntry && settingNew.Ruleelementcount != (int)ArchivingRuleEnum.ArchivePlannedActivityOnly)
                    {
                        _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                    else if ((settingNew.Ruleelementcount == (int)ArchivingRuleEnum.ArchivePlannedActivityOnly))
                    {
                        Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();
                        if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Project_Plan)
                        {
                            bool setArchivalStatus = false;
                            if (daPlannedActivtyDcfEntities != null && daPlannedActivtyDcfEntities.Any())
                            {
                                foreach (var daPlannedActivtyDcf in daPlannedActivtyDcfEntities)
                                {
                                    var singleDcfUpdate = _repositoryWrapper.DaPlannedActivityDcfRepository
                                                            .FindByCondition(x => x.Daplannedactivitydcfid == daPlannedActivtyDcf.DaPlannedActivityDcfId)
                                                            .FirstOrDefault();

                                    if (singleDcfUpdate != null)
                                    {
                                        singleDcfUpdate.Dcfstatus = daPlannedActivtyDcf.DcfStatus == ConstantValueFilter.completed;
                                        _repositoryWrapper.DaPlannedActivityDcfRepository.Update(singleDcfUpdate);
                                    }
                                }
                                await _repositoryWrapper.SaveAsync();
                                var dcfIds = daPlannedActivtyDcfEntities.Select(da => da.DaPlannedActivityDcfId).ToList();

                                var dcfStatusForMultipleDcf = _repositoryWrapper.DaPlannedActivityDcfRepository
                                                                .FindByCondition(x => dcfIds.Contains(x.Daplannedactivitydcfid))
                                                                .ToList();

                                setArchivalStatus = dcfStatusForMultipleDcf.All(x => x.Dcfstatus == true);

                                if (setArchivalStatus)
                                {
                                    plannedActivity.Archived = true;
                                    plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                                    plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;
                                    await _commonManager.setArchiveStatusForBpt(plannedActivity);
                                }
                                else
                                {
                                    plannedActivity.Deliverystatusid = plannedActivity.Deliverystatusid;
                                }
                            }
                        }
                        else if(plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Infra_Readiness   )
                        {
                            var settingUpdatePlannedList = _repositoryWrapper.SettingsUpdatePlannedActivity.
                                FindByCondition(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Infra_Readiness)
                                .Select(x => x.Order).ToList();

                            var minimumDeliveryOrder = settingUpdatePlannedList.Min();
                            var maxDeliveryOrder = settingUpdatePlannedList.Max();

                            if (minimumDeliveryOrder == settingNew.Order) await AddDaMigrationEntry(plannedActivityEntity);
                            else if (maxDeliveryOrder == settingNew.Order)
                            {
                                var daMigrationEntity = _repositoryWrapper.DaMigrationStatusRepository
                       .FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid
                                          && x.Statusid != 3).Any();
                                if (daMigrationEntity)
                                {
                                    plannedActivity.Deliverystatusid = prevDeliveryStatusIdInfraReady;
                                    plannedActivity.Archived = prevArchivedInfraReady;
                                }

                            }
                        }
                        else if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration)
                        {
                            var settingUpdatePlannedList = _repositoryWrapper.SettingsUpdatePlannedActivity.
                                FindByCondition(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration)
                                .Select(x => x.Order).ToList();

                            var minimumDeliveryOrder = settingUpdatePlannedList.Min();
                            var maxDeliveryOrder = settingUpdatePlannedList.Max();

                            var asset_DeploymentStatus = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.ToLower()
                            == ConstantValueFilter.inService || x.Deploymentstatus.ToLower() == ConstantValueFilter.Removed).Select(x => new
                            {
                                x.Deploymentstatus, x.Deploymentstatusid
                            }).ToList();

                            var asset_Inservice_DeploymentStatusId = asset_DeploymentStatus.Where(x => x.Deploymentstatus.ToLower()
                           == ConstantValueFilter.inService).FirstOrDefault().Deploymentstatusid;

                            var asset_Removed_DeploymentStatusId = asset_DeploymentStatus.Where(x => x.Deploymentstatus.ToLower()
                         == ConstantValueFilter.Removed).FirstOrDefault().Deploymentstatusid;


                            //await _PlatformMigrationManager.AddOrUpdateAssetMigrationAsync(_daAssetMigrationDto,plannedActivity.Plannedactivityid);

                            if (maxDeliveryOrder == settingNew.Order)
                            {
                                var daMigrationEntity = _repositoryWrapper.DaAssetMigrationRepository
                       .FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid  &&
                                (x.Deploymentstatusid != asset_Inservice_DeploymentStatusId // && x.Deploymentstatusid != asset_Removed_DeploymentStatusId )
                                && x.Newelementname != null)).Any(); 

                                if (!daMigrationEntity)
                                {
                                    plannedActivity.Archived = true;
                                    plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                                    plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;

                                    await _commonManager.setArchiveStatusForBpt(plannedActivity);
                                }
                                else
                                {

                                    return new ResultDto
                                    {
                                        Info = ResultMessages.DaAssetMustBeInservice,
                                        Data = plannedActivity,
                                        Warning = true
                                        
                                    };
                                }

                            }
                        }
                        else
                        {
                            plannedActivity.Archived = true;
                            plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                            plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;

                            await _commonManager.setArchiveStatusForBpt(plannedActivity);
                        }
                        _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        //Project Plan Archival
                    }

                    //Below update is needed since we update plannedactivty the user should able to see the DA entry at top of the page
                    _repositoryWrapper.DesignAspectRepository.Update(designAspectEntity);
                    await _repositoryWrapper.SaveAsync();

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                        Data = plannedActivity
                    };
                }
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }

        }

        public async Task<ResultDto> GetDcfAssociatedEntities(DesignAspectPlannedActivityDto dto)
        {
            try
            {
                if (dto.DcfId.Count == 0)
                    return new ResultDto();

                var dcList = _repositoryWrapper.DesignComponent
                    .FindByCondition(x => dto.DcfId.Contains(x.Designcomponentfamilyid)).ToDictionary(x => x.Designcomponentid, x => x.toDesignComponentNameLcm(_repositoryWrapper));

                var lcmsIds = await Task.Run(() => _repositoryWrapper.Lcmengineering.FindByCondition(x => dcList.Keys.Contains(x.Designcomponentid) && x.Opcoid == dto.OpcoId).Select(x => x.Lcmengineeringid).ToList());
                var AssetIds = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => dcList.Keys.Contains(x.Designcomponentid) && x.Opcoid == dto.OpcoId).Select(x => x.Networkelementasplannedid).ToList();

                var jsonData = new List<DesignAspectPlannedActivityDto>();
                if (dcList != null && dcList.Count > 0)
                {

                    var dcfAssociatedLcmList = _repositoryWrapper.Lcmengineering.FindByCondition(x => lcmsIds.Contains(x.Lcmengineeringid))
                                                 .Include(x => x.Opco)
                                                 .Include(x => x.PlannedactivitiesLcmengineering)
                                                 .AsEnumerable().ToList();

                    var dcfAssociatedAssetList = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => AssetIds.Contains(x.Networkelementasplannedid))
                                                 .Include(x => x.Opco)
                                                 .Include(x => x.Plannedactivities)
                                                 .AsEnumerable().ToList();


                    if ((dcfAssociatedLcmList != null && dcfAssociatedLcmList.Count > 0) || (dcfAssociatedAssetList != null && dcfAssociatedAssetList.Count > 0))
                    {

                        jsonData = dcList
                            .SelectMany(dc =>
                            {
                                var results = new List<DesignAspectPlannedActivityDto>();

                                var dcRecords = dcfAssociatedLcmList
                                    .Where(l => l.Designcomponentid == dc.Key && l.Archived != true)
                                    .ToList();
                                var dcAssetRecords = dcfAssociatedAssetList
                                    .Where(l => l.Designcomponentid == dc.Key)
                                    .ToList();

                                var lcmPaIds = dcRecords
                                    .SelectMany(l => l.PlannedactivitiesLcmengineering).Where(x => x.Archived != true)
                                    .Select(a => a.Plannedactivityid)
                                    .ToList();

                                var assetPaIds = dcAssetRecords
                                    .SelectMany(ne => ne.Plannedactivities).Where(x => x.Archived != true)
                                    .Select(a => a.Plannedactivityid)
                                    .ToList();

                                var dcId = dcRecords.Select(l => l.Designcomponentid).FirstOrDefault();
                                var opco = dcRecords.Select(l => l.Opco?.Opco).FirstOrDefault();
                                var opcoId = dcRecords.Select(l => l.Opcoid).FirstOrDefault();

                                if (lcmPaIds.Any())
                                {
                                    results.Add(new DesignAspectPlannedActivityDto
                                    {
                                        DesignComponentName = $"LCM - {dc.Value}",
                                        DcId = dcId,
                                        OpcoName = opco,
                                        OpcoId = opcoId,
                                        LcmPaId = string.Join(",", lcmPaIds),
                                        AssetPaId = string.Empty
                                    });
                                }

                                if (assetPaIds.Any())
                                {
                                    results.Add(new DesignAspectPlannedActivityDto
                                    {
                                        DesignComponentName = $"Asset - {dc.Value}",
                                        DcId = dcId,
                                        OpcoName = opco,
                                        OpcoId = opcoId,
                                        LcmPaId = string.Empty,
                                        AssetPaId = string.Join(",", assetPaIds)
                                    });
                                }

                                return results;
                            })
                            .Where(x => !string.IsNullOrEmpty(x.LcmPaId) || !string.IsNullOrEmpty(x.AssetPaId))
                            .ToList();


                    }
                }

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = jsonData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoNoFound,
                    Warning = true,
                    Data = ""

                };
            }

        }

        public async Task<ResultDto> GetMultipleDcfEntities(PlannedActivityDtoUpdate dto)
        {
            var MultipleDcfEntities = await Task.Run(() => _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == dto.OpCoId && x.Programid == dto.ProgramId && x.Deliveryprojectname == dto.DeliveryProjectName)
                                        .Include(x => x.Opco)
                                        .Include(x => x.ProgramNavigation)
                                        .Include(x => x.Daplannedactivitydcf).ThenInclude(x => x.Designcomponentfamily)
                                        .ToList());

            if(MultipleDcfEntities.Count > 0 && MultipleDcfEntities.Any())
            {
                var Daplannedactivitydcf = MultipleDcfEntities.SelectMany(x => x.Daplannedactivitydcf).ToDictionary(x => x.Designcomponentfamilyid, x => x.Designcomponentfamily.DCFName(_repositoryWrapper)); 
                if(Daplannedactivitydcf.Count > 0)
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.PlannedDCFEntryHasLinkedPlannedActivities,
                        Warning = true
                    };
                }
                else
                {
                    return new ResultDto
                    {
                        Info = "",
                    };
                }
            }
            return new ResultDto
            {
                Info = "",
            };

        }
    }
}
