using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
namespace CAM.Entities.Mappers.Cross
{
    public static class PlannedActivityResourceBenefitMapper
    {
        public static PlannedActivityResourceBenefit Get(Plannedactivityresourcebenefit model)
        {

            if (model == null)
                return null;
            return new PlannedActivityResourceBenefit()
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
              //  PlannedActivityResource = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(model.Plannedactivityresource),
                BenefitId = model.Benefitid,
                ForLcm = model.Forlcm,
                PlannedActivityResourceBenefitId = model.Planactivityresbenefitid,
                Benefit = BenefitMapper.GetBenefitMapper(model.Benefit),
                ForDesignAspect =model.Fordesignaspect,
                ForAddAsset = model.Foraddasset,
                ForEditAsset = model.Foreditasset
            };
        }

        public static Plannedactivityresourcebenefit Set(PlannedActivityResourceBenefit model)
        {
            return new Plannedactivityresourcebenefit()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Plannedactivityresourceid = model.PlannedActivityResourceId,
                Benefitid = model.BenefitId,
                Forlcm =model.ForLcm,
                Planactivityresbenefitid = model.PlannedActivityResourceBenefitId,
                Fordesignaspect = model.ForDesignAspect,
                Foraddasset = model.ForAddAsset,
                Foreditasset = model.ForEditAsset
            };
        }
    }
}
