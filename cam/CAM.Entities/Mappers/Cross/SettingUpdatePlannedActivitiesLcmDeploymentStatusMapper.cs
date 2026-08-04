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
    public static class SettingUpdatePlannedActivitiesLcmDeploymentStatusMapper
    {
        public static SettingUpdatePlannedActivityLcmDeploymentStatus Get(Settingupdateplannedactivitylcmdeploymentstatus model)
        {

            if (model == null)
                return null;
            return new SettingUpdatePlannedActivityLcmDeploymentStatus()
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
                LcmDeploymentStatusId = model.Lcmdeploymentstatusid,
                Id = model.Id,
                LcmDeploymentStatus = LCMDeploymentStatusMapper.GetLCMDeploymentStatusMapper(model.Lcmdeploymentstatus)
            };
        }

        public static Settingupdateplannedactivitylcmdeploymentstatus Set(SettingUpdatePlannedActivityLcmDeploymentStatus model)
        {
            return new Settingupdateplannedactivitylcmdeploymentstatus()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Settingupdateplannedactivityid = model.SettingUpdatePlannedActivityId,
                Id = model.Id,
                Lcmdeploymentstatusid = model.LcmDeploymentStatusId,
                Lcmdeploymentstatus = LCMDeploymentStatusMapper.SetLCMDeploymentStatusMapper(model.LcmDeploymentStatus)
            };
        }
    }
}
