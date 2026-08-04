using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class LCMDeploymentStatusMapper
    {
        public static LCMDeploymentStatus GetLCMDeploymentStatusMapper(Lcmdeploymentstatus model, bool Include = true)
        {
            if (model == null)
                return null;
            return new LCMDeploymentStatus()
            {
                Id = model.Id,
                Description = model.Description,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Lcmengineerings = model.Lcmengineering.Select(p => LCMEngineeringMapper.GetLcmEngineeringMapper(p, false)).ToList(),
            };
        }
        public static Lcmdeploymentstatus SetLCMDeploymentStatusMapper(LCMDeploymentStatus model)
        {
            if (model == null)
                return null;
            return new Lcmdeploymentstatus()
            {
                Id = model.Id,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Description = model.Description,
            };
        }
    }
}
