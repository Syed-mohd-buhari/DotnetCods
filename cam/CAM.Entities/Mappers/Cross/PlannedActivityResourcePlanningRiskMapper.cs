using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Cross
{
    public static class PlannedActivityResourcePlanningRiskMapper
    {
        public static PlannedActivityResourcePlanningRisk Get (Plannedactivityresourceplanningrisk model)
        {

            if (model == null)
                return null;
            return new PlannedActivityResourcePlanningRisk()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                PlannedActivityResourceId = model.Plannedactivityresourceid,
                PlannedActivityResourcePlanningRiskId = model.Plnactresourceplanningriskid,
                PlanningRiskId = model.Planningriskid,
               // PlannedActivityResource =PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(model.Plannedactivityresource),
                PlanningRisk = PlanningRiskMapper.GetPlanningRiskMapper(model.Planningrisk),
                ForLcm = model.Forlcm,
                ForDesignAspect = model.Fordesignaspect,
                ForAddAsset = model.Foraddasset,
                ForEditAsset = model.Foreditasset
            };
        }

        public static Plannedactivityresourceplanningrisk Set(PlannedActivityResourcePlanningRisk model)
        {
            return new Plannedactivityresourceplanningrisk(){

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Plannedactivityresourceid = model.PlannedActivityResourceId,
                Plnactresourceplanningriskid = model.PlannedActivityResourcePlanningRiskId,
                Planningriskid = model.PlanningRiskId,
                Forlcm = model.ForLcm,
                Fordesignaspect = model.ForDesignAspect,
                Foraddasset = model.ForAddAsset,
                Foreditasset = model.ForEditAsset

            };
        }
    }
}
