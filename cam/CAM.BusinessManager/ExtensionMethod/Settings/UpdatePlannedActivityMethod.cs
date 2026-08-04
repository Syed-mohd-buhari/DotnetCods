using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.LookUp.PlannedActivityResourceDto;
using CAM.Entities.Models.Settings;
using CAM.Enum;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.ExtensionMethod.Settings
{
    public static class UpdatePlannedActivityMethod
    {

        public static async Task ArchivePlannedActivity(Plannedactivities plannedActivity, IRepositoryWrapper _repositoryWrapper)
        {
            plannedActivity.Archived = true;
            _repositoryWrapper.PlannedActivity.Update(plannedActivity);
            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();
        }

        public static async Task ArchiveParentAsset(Networkelementsasplanned asset, IRepositoryWrapper _repositoryWrapper)
        {
            asset.Deploymentstatusid = _repositoryWrapper.DeploymentStatus
                .FindByCondition(status => status.Deploymentstatus.ToLower().Replace(" ", "") == "REMOVED".ToLower())
                .Select(status => status.Deploymentstatusid)
                .FirstOrDefault();
            await _repositoryWrapper.SaveAsync();
        }

        public static async Task<short> UpdateAssetdeploymentStatus(Networkelementsasplanned asset, IRepositoryWrapper _repositoryWrapper,string assetDeploymentStatus)
        {
            asset.Deploymentstatusid = _repositoryWrapper.DeploymentStatus
                .FindByCondition(status => status.Deploymentstatus.ToLower().Replace(" ", "") == assetDeploymentStatus.ToLower())
                .Select(status => status.Deploymentstatusid)
                .FirstOrDefault();
            _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
            await _repositoryWrapper.SaveAsync();
            return asset.Deploymentstatusid;

        }

    }
}
