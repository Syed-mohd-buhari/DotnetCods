using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class PlanningActivityStatusMapper
    {
        public static Models.Lookup.PlanningActivityStatus GetPlanningActivityStatusMapper(Planningactivitystatuses PlanningActivityStatus)
        {
            if (PlanningActivityStatus == null)
                return null;
            return new Models.Lookup.PlanningActivityStatus()
            {
                PlanningActivityStatusId= PlanningActivityStatus.Planningactivitystatusid,
                PlanningActivityStatusDescription = PlanningActivityStatus.Planningactivitystatus,
                CreationDate = PlanningActivityStatus.Creationdate,
                CreationUser = PlanningActivityStatus.Creationuser,
                ModificationDate = PlanningActivityStatus.Modificationdate,
                ModificationUser = PlanningActivityStatus.Modificationuser,
                Deleted = PlanningActivityStatus.Deleted.Value,
                DeletionDate = PlanningActivityStatus.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(PlanningActivityStatus.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(PlanningActivityStatus.ModificationuserNavigation),
                ProjectStatusCombinationRule = PlanningActivityStatus.Projectstatuscombinationrule,
                Default = PlanningActivityStatus.Default,
               
            };
        }
        public static Planningactivitystatuses SetPlanningActivityStatusMapper(Models.Lookup.PlanningActivityStatus PlanningActivityStatus)
        {
            return new Planningactivitystatuses()
            {

                Planningactivitystatusid = PlanningActivityStatus.PlanningActivityStatusId,
                Planningactivitystatus = PlanningActivityStatus.PlanningActivityStatusDescription,
                Creationdate = PlanningActivityStatus.CreationDate,
                Creationuser = PlanningActivityStatus.CreationUser,
                Modificationdate = PlanningActivityStatus.ModificationDate,
                Modificationuser = PlanningActivityStatus.ModificationUser,
                Deleted = PlanningActivityStatus.Deleted,
                Deletiondate = PlanningActivityStatus.DeletionDate,
                Projectstatuscombinationrule = PlanningActivityStatus.ProjectStatusCombinationRule,
                Default = PlanningActivityStatus.Default,

            };
        }
    }
}
