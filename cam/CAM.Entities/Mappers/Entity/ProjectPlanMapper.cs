using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class ProjectPlanMapper
    {
        public static ProjectPlan Get(Projectsplan model)
        {
            if (model == null)
                return null;
            var result = new ProjectPlan()
            {
                ProjectsPlanId = model.Projectsplanid,
                PlannedActivityId = model.Plannedactivityid,
                SettingsUpdatePlannedActivityId = model.Settingsupdateplannedactivityid,
                PlanningStartDate = model.Planningstartdate,
                PlanningEndDate = model.Planningenddate,
                Description = model.Description,
                Progress = model.Progress,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                              
            };
            return result;
        }

        public static Projectsplan Set(ProjectPlan model)
        {
            return new Projectsplan()
            {
                Projectsplanid = model.ProjectsPlanId,
                Plannedactivityid = model.PlannedActivityId,
                Settingsupdateplannedactivityid = model.SettingsUpdatePlannedActivityId,
                Planningstartdate = model.PlanningStartDate,
                Planningenddate = model.PlanningEndDate,
                Description = model.Description,
                Progress = model.Progress,
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
