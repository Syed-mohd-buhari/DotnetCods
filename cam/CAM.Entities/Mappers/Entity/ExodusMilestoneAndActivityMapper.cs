using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class ExodusMilestoneAndActivityMapper
    {
        public static ExodusMilestoneAndActivities Get(Exodusmilestoneandactivities model)
        {
            if (model == null)
                return null;
            var result = new ExodusMilestoneAndActivities()
            {
                ExodusMilestoneAndActivityId = model.Exodusmilestoneandactivityid,
                Activities = model.Activities,
                ActivitiesOrder = model.Activitiesorder,
                Milestones = model.Milestones,
                ActualColumnNames = model.Actualcolumnnames,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                              
            };
            return result;
        }

        public static Exodusmilestoneandactivities Set(ExodusMilestoneAndActivities model)
        {
            return new Exodusmilestoneandactivities()
            {
                Exodusmilestoneandactivityid = model.ExodusMilestoneAndActivityId,
                Activities = model.Activities,
                Activitiesorder = model.ActivitiesOrder,
                Milestones = model.Milestones,
                Actualcolumnnames = model.ActualColumnNames

            };
        }
    }
}
