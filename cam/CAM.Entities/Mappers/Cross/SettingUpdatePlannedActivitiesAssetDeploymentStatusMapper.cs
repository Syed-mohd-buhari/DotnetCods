using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Cross
{
    public static class SettingUpdatePlannedActivitiesAssetDeploymentStatusMapper
    {
        public static SettingUpdatePlannedActivityAssetDeploymentStatus Get(Settingupdateplannedactivityassetdeploymentstatus model)
        {

            if (model == null)
                return null;
            return new SettingUpdatePlannedActivityAssetDeploymentStatus()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                SettingUpdatePlannedActivityId = model.Settingupdateplannedactivityid,
                AssetDeploymentStatusId = model.Assetdeploymentstatusid,
                Id = model.Id,
                AssetDeploymentStatus = DeploymentStatusMapper.GetDeploymentStatusMapper(model.Assetdeploymentstatus)
            };
        }

        public static Settingupdateplannedactivityassetdeploymentstatus Set(SettingUpdatePlannedActivityAssetDeploymentStatus model)
        {
            return new Settingupdateplannedactivityassetdeploymentstatus()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Settingupdateplannedactivityid = model.SettingUpdatePlannedActivityId,
                Id = model.Id,
                Assetdeploymentstatusid = model.AssetDeploymentStatusId,
                Assetdeploymentstatus = DeploymentStatusMapper.SetDeploymentStatusMapper(model.AssetDeploymentStatus)
            };
        }
    }
}
