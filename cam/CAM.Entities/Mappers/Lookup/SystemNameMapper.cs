using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class SystemNameMapper
    {
        public static SystemNames GetSystemNameMapper(Systemnames systemnames)
        {
            if (systemnames == null)
                return null;
            var result = new SystemNames()
            {
                SystemNameId = systemnames.Systemnameid,
                SystemNameDescription = systemnames.Systemnamedescription,
                CreationDate = systemnames.Creationdate,
                CreationUser = systemnames.Creationuser,
                ModificationDate = systemnames.Modificationdate,
                ModificationUser = systemnames.Modificationuser,
                Deleted = systemnames.Deleted.Value,
                DeletionDate = systemnames.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(systemnames.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(systemnames.ModificationuserNavigation),
            };

            return result;
        }
        public static Systemnames SetSystemNamesMapper(SystemNames systemnames)
        {
            if (systemnames == null)
                return null;
            return new Systemnames()
            {
                Systemnameid = systemnames.SystemNameId,
                Systemnamedescription = systemnames.SystemNameDescription,
                Creationdate = systemnames.CreationDate,
                Creationuser = systemnames.CreationUser,
                Modificationdate = systemnames.ModificationDate,
                Modificationuser = systemnames.ModificationUser,
                Deleted = systemnames.Deleted,
                Deletiondate = systemnames.DeletionDate,
            };
        }
    }
}
