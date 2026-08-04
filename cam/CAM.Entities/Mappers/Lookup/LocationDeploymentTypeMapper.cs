using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public class LocationDeploymentTypeMapper
    {
        public static LocationDeploymentTypes Get(Locationdeploymenttypes model)
        {
            if (model == null)
                return null;
            return new LocationDeploymentTypes()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted != null ? model.Deleted.Value : false,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                LocationId = model.Locationid.Value,
                Id = model.Id,
                DeploymentTypeId = model.Deploymenttypeid.Value,
                DeploymentType = DeploymentTypeMapper.GetDeploymentTypeMapper(model.Deploymenttype),
            };
        }
        public static Locationdeploymenttypes Set(LocationDeploymentTypes model)
        {
            return new Locationdeploymenttypes()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Locationid = model.LocationId,
                Id = model.Id,
                Deploymenttypeid = model.DeploymentTypeId,
                Location = LocationMapper.SetLocationMapper(model.Location),
                Deploymenttype = DeploymentTypeMapper.SetDeploymentTypeMapper(model.DeploymentType),
            };
        }
    }
}
