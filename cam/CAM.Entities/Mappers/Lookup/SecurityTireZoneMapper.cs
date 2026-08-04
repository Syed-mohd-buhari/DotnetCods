using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class SecurityTireZoneMapper
    {
        public static SecurityTireZone Get(Securitytirezone model)
        {
            if (model == null)
                return null;
            return new SecurityTireZone()
            {
                SecurityTireZoneId = model.Securitytirezoneid,
                SecurityTireZoneDescription = model.Description,
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
        public static Securitytirezone Set(SecurityTireZone model)
        {
            return new Securitytirezone()
            {
                Securitytirezoneid = model.SecurityTireZoneId,
                Description = model.SecurityTireZoneDescription,
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
