using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public class UserLoggingLevelMapper
    {
        public static UsersLoggingLevels GetUsersLoggingLevelsMapper(Userslogginglevels userslogginglevels)
        {
            if (userslogginglevels == null)
                return null;

            var result = new UsersLoggingLevels()
            {
                LogLevel = userslogginglevels.Loglevel,
                CreationDate = userslogginglevels.Creationdate,
                CreationUser = userslogginglevels.Creationuser,
                ModificationDate = userslogginglevels.Modificationdate,
                ModificationUser = userslogginglevels.Modificationuser,
                Deleted = userslogginglevels.Deleted.Value,
                DeletionDate = userslogginglevels.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(userslogginglevels.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(userslogginglevels.ModificationuserNavigation),
            };

            return result;
        }
        public static Userslogginglevels SetUsersLoggingLevelsMapper(UsersLoggingLevels usersLoggingLevels)
        {
            if (usersLoggingLevels == null)
                return null;

            var result = new Userslogginglevels()
            {

                Loglevel = usersLoggingLevels.LogLevel,
                Creationdate = usersLoggingLevels.CreationDate,
                Creationuser = usersLoggingLevels.CreationUser,
                Modificationdate = usersLoggingLevels.ModificationDate,
                Modificationuser = usersLoggingLevels.ModificationUser,
                Deleted = usersLoggingLevels.Deleted,
                Deletiondate = usersLoggingLevels.DeletionDate,
            };

            return result;
        }
    }
}
