using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class ActivityStatusMapper
    {
        public static ActivityStatus GetActivityStatusMapper(Activitystatuses activitystatus)
        {
            if (activitystatus == null)
                return null;
            return new ActivityStatus()
            {
                ActivityStatusId = activitystatus.Activitystatusid,
                ActivityStatusDescription = activitystatus.Activitystatus,
                CreationDate = activitystatus.Creationdate,
                CreationUser = activitystatus.Creationuser,
                ModificationDate = activitystatus.Modificationdate,
                ModificationUser = activitystatus.Modificationuser,
                Deleted = activitystatus.Deleted.Value,
                DeletionDate = activitystatus.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(activitystatus.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(activitystatus.ModificationuserNavigation),
                ProjectStatusCombinationRule = activitystatus.Projectstatuscombinationrule,
                Rule = activitystatus.Rule,
            };
        }
        public static Activitystatuses SetActivityActivityStatusMapper(ActivityStatus activitystatus)
        {
            return new Activitystatuses()
            {
                Activitystatusid = activitystatus.ActivityStatusId,
                Activitystatus = activitystatus.ActivityStatusDescription,
                Creationdate = activitystatus.CreationDate,
                Creationuser = activitystatus.CreationUser,
                Modificationdate = activitystatus.ModificationDate,
                Modificationuser = activitystatus.ModificationUser,
                Deleted = activitystatus.Deleted,
                Deletiondate = activitystatus.DeletionDate,
                Rule = activitystatus.Rule,
                Projectstatuscombinationrule = activitystatus.ProjectStatusCombinationRule,
            };
        }
    }
}
