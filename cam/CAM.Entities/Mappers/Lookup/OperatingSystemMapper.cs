using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class OperatingSystemMapper
    {
        public static Models.Lookup.OperatingSystem GetOperatingSystemMapper(Operatingsystems OperatingSystem)
        {
            if (OperatingSystem == null)
                return null;
            return new Models.Lookup.OperatingSystem()
            {
                OperatingSystemId= OperatingSystem.Operatingsystemid,
                OperatingSystemName = OperatingSystem.Operatingsystemname,
                CreationDate = OperatingSystem.Creationdate,
                CreationUser = OperatingSystem.Creationuser,
                ModificationDate = OperatingSystem.Modificationdate,
                ModificationUser = OperatingSystem.Modificationuser,
                Deleted = OperatingSystem.Deleted.Value,
                DeletionDate = OperatingSystem.Deletiondate,
                OperatingSystemVersion = OperatingSystem.Operatingsystemversion,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(OperatingSystem.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(OperatingSystem.ModificationuserNavigation),
               
            };
        }
        public static Operatingsystems SetOperatingSystemMapper(Models.Lookup.OperatingSystem OperatingSystem)
        {
            if (OperatingSystem == null)
                return null;
            return new Operatingsystems()
            {

                Operatingsystemid = OperatingSystem.OperatingSystemId,
                Operatingsystemname = OperatingSystem.OperatingSystemName,
                Creationdate = OperatingSystem.CreationDate,
                Creationuser = OperatingSystem.CreationUser,
                Modificationdate = OperatingSystem.ModificationDate,
                Modificationuser = OperatingSystem.ModificationUser,
                Deleted = OperatingSystem.Deleted,
                Deletiondate = OperatingSystem.DeletionDate,
                Operatingsystemversion = OperatingSystem.OperatingSystemVersion
            };
        }
    }
}
