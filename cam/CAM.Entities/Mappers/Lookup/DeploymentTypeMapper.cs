using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class DeploymentTypeMapper
    {
        public static DeploymentType GetDeploymentTypeMapper(Deploymenttypes DeploymentType)
        {
            if (DeploymentType == null)
                return null;
            return new DeploymentType()
            {
                DeploymentTypeId = DeploymentType.Deploymenttypeid,
                DeploymentTypeDescription = DeploymentType.Deploymenttype,
                CreationDate = DeploymentType.Creationdate,
                CreationUser = DeploymentType.Creationuser,
                ModificationDate = DeploymentType.Modificationdate,
                ModificationUser = DeploymentType.Modificationuser,
                Deleted = DeploymentType.Deleted.Value,
                DeletionDate = DeploymentType.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(DeploymentType.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(DeploymentType.ModificationuserNavigation),
                Rule = DeploymentType.Rule,
                
                 
            };
        }
        public static Deploymenttypes SetDeploymentTypeMapper(DeploymentType DeploymentType)
        {
            return new Deploymenttypes()
            {
                Deploymenttypeid = DeploymentType.DeploymentTypeId,
                Deploymenttype = DeploymentType.DeploymentTypeDescription,
                Creationdate = DeploymentType.CreationDate,
                Creationuser = DeploymentType.CreationUser,
                Modificationdate = DeploymentType.ModificationDate,
                Modificationuser = DeploymentType.ModificationUser,
                Deleted = DeploymentType.Deleted,
                Deletiondate = DeploymentType.DeletionDate,
         
                Rule = DeploymentType.Rule,
            };
        }
    }
}

