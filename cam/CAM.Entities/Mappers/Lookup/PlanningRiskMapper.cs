using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class PlanningRiskMapper
    {
        public static Models.Lookup.PlanningRisk GetPlanningRiskMapper(Planningrisks PlanningRisk)
        {
            if (PlanningRisk == null)
                return null;
            return new Models.Lookup.PlanningRisk()
            {
                PlanningRiskId= PlanningRisk.Planningriskid,
                PlanningRiskDescription = PlanningRisk.Planningrisk,
                CreationDate = PlanningRisk.Creationdate,
                CreationUser = PlanningRisk.Creationuser,
                ModificationDate = PlanningRisk.Modificationdate,
                ModificationUser = PlanningRisk.Modificationuser,
                Deleted = PlanningRisk.Deleted.Value,
                DeletionDate = PlanningRisk.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(PlanningRisk.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(PlanningRisk.ModificationuserNavigation),
           
            };
        }
        public static Planningrisks SetPlanningRiskMapper(Models.Lookup.PlanningRisk PlanningRisk)
        {
            return new Planningrisks()
            {

                Planningriskid = PlanningRisk.PlanningRiskId,
                Planningrisk = PlanningRisk.PlanningRiskDescription,
                Creationdate = PlanningRisk.CreationDate,
                Creationuser = PlanningRisk.CreationUser,
                Modificationdate = PlanningRisk.ModificationDate,
                Modificationuser = PlanningRisk.ModificationUser,
                Deleted = PlanningRisk.Deleted,
                Deletiondate = PlanningRisk.DeletionDate,
            };
        }
    }
}
