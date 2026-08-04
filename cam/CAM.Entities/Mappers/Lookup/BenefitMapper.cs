using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class BenefitMapper
    {
        public static Benefit GetBenefitMapper(Benefits Benefit)
        {
            if (Benefit == null)
                return null;
            return new Benefit()
            {
                BenefitId = Benefit.Benefitid,
                BenefitDescription = Benefit.Benefit,
                CreationDate = Benefit.Creationdate,
                CreationUser = Benefit.Creationuser,
                ModificationDate = Benefit.Modificationdate,
                ModificationUser = Benefit.Modificationuser,
                Deleted = Benefit.Deleted.Value,
                DeletionDate = Benefit.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Benefit.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Benefit.ModificationuserNavigation),
                
            };
        }
        public static Benefits SetBenefitMapper(Benefit Benefit)
        {
            return new Benefits()
            {
                Benefitid = Benefit.BenefitId,
                Benefit = Benefit.BenefitDescription,
                Creationdate = Benefit.CreationDate,
                Creationuser = Benefit.CreationUser,
                Modificationdate = Benefit.ModificationDate,
                Modificationuser = Benefit.ModificationUser,
                Deleted = Benefit.Deleted,
                Deletiondate = Benefit.DeletionDate,


            };
        }
    }
}
