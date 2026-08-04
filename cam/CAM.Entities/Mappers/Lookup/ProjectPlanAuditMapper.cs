using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class ProjectPlanAuditMapper
    {
        public static ProjectPlanAudit GetProgramMapper(Projectplanaudit model)
        {
            if (model == null)
                return null;
            return new ProjectPlanAudit()
            {
                ProjectPlanAuditId = model.Projectplanauditid,
                ProjectsPlanId = model.Projectsplanid,
                OldValue = model.Oldvalue,
                NewValue = model.Newvalue,
                ProcessType = model.Processtype,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                 
            };
        }
        public static Projectplanaudit SetProgramMapper(ProjectPlanAudit model)
        {
            return new Projectplanaudit()
            {
                Projectplanauditid = model.ProjectPlanAuditId,
                Projectsplanid = model.ProjectsPlanId,
                Oldvalue = model.OldValue,
                Newvalue = model.NewValue,
                Processtype = model.ProcessType,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
            };
        }
    }
}

