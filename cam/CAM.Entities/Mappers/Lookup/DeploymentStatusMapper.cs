using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class DeploymentStatusMapper
    {
        public static DeploymentStatus GetDeploymentStatusMapper(Deploymentstatuses DeploymentStatus)
        {
            if (DeploymentStatus == null)
                return null;
            return new DeploymentStatus()
            {
                DeploymentStatusId = DeploymentStatus.Deploymentstatusid,
                DeploymentStatusDescription = DeploymentStatus.Deploymentstatus,
                CreationDate = DeploymentStatus.Creationdate,
                CreationUser = DeploymentStatus.Creationuser,
                ModificationDate = DeploymentStatus.Modificationdate,
                ModificationUser = DeploymentStatus.Modificationuser,
                Deleted = DeploymentStatus.Deleted.Value,
                DeletionDate = DeploymentStatus.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(DeploymentStatus.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(DeploymentStatus.ModificationuserNavigation),
                Rule = DeploymentStatus.Rule,
                CheckPlannedActivity = DeploymentStatus.Checkplannedactivity,
                PlannedActivityResourceAllowed = DeploymentStatus.Plannedactivityresourceallowed,
                ReadOnlyPlannedActivity = DeploymentStatus.Readonlyplannedactivity,
                DefaultValue = DeploymentStatus.Defaultvalue,
                
            };
        }
        public static Deploymentstatuses SetDeploymentStatusMapper(DeploymentStatus DeploymentStatus)
        {
            return new Deploymentstatuses()
            {
                Deploymentstatusid = DeploymentStatus.DeploymentStatusId,
                Deploymentstatus = DeploymentStatus.DeploymentStatusDescription,
                Creationdate = DeploymentStatus.CreationDate,
                Creationuser = DeploymentStatus.CreationUser,
                Modificationdate = DeploymentStatus.ModificationDate,
                Modificationuser = DeploymentStatus.ModificationUser,
                Deleted = DeploymentStatus.Deleted,
                Deletiondate = DeploymentStatus.DeletionDate,
                Checkplannedactivity = DeploymentStatus.CheckPlannedActivity,
                Plannedactivityresourceallowed = DeploymentStatus.PlannedActivityResourceAllowed,
                Readonlyplannedactivity = DeploymentStatus.ReadOnlyPlannedActivity,
                Rule = DeploymentStatus.Rule,
                Defaultvalue = DeploymentStatus.DefaultValue,

            };
        }

        public static DeploymentStatus GetDeploymentStatusPAResourceAllowed(Deploymentstatuses DeploymentStatus)
        {
            if (DeploymentStatus == null)
                return null;
            return new DeploymentStatus()
            { 
                PlannedActivityResourceAllowed = DeploymentStatus.Plannedactivityresourceallowed
            };
        }
    }
}

