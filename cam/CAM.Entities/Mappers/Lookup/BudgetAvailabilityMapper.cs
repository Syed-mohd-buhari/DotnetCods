using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class BudgetAvailabilityMapper
    {
        public static BudgetAvailability GetBudgetAvailabilityMapper(Budgetavailability BudgetAvailability)
        {
            if (BudgetAvailability == null)
                return null;
            return new BudgetAvailability()
            {
                BudgetAvailabilityId = BudgetAvailability.Budgetavailabilityid,
                BudgetAvailabilityDescription = BudgetAvailability.Description,
                CreationDate = BudgetAvailability.Creationdate,
                CreationUser = BudgetAvailability.Creationuser,
                ModificationDate = BudgetAvailability.Modificationdate,
                ModificationUser = BudgetAvailability.Modificationuser,
                Deleted = BudgetAvailability.Deleted.Value,
                DeletionDate = BudgetAvailability.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(BudgetAvailability.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(BudgetAvailability.ModificationuserNavigation),
                ProjectStatusCombinationRule = BudgetAvailability.Projectstatuscombinationrule,
                Rule = BudgetAvailability.Rule,
               
            };
        }
        public static Budgetavailability SetBudgetAvailabilityMapper(BudgetAvailability BudgetAvailability)
        {
            return new Budgetavailability()
            {
                Budgetavailabilityid = BudgetAvailability.BudgetAvailabilityId,
                Description = BudgetAvailability.BudgetAvailabilityDescription,
                Creationdate = BudgetAvailability.CreationDate,
                Creationuser = BudgetAvailability.CreationUser,
                Modificationdate = BudgetAvailability.ModificationDate,
                Modificationuser = BudgetAvailability.ModificationUser,
                Deleted = BudgetAvailability.Deleted,
                Deletiondate = BudgetAvailability.DeletionDate,
                Projectstatuscombinationrule = BudgetAvailability.ProjectStatusCombinationRule,
                Rule = BudgetAvailability.Rule,
            };
        }
    }
}
