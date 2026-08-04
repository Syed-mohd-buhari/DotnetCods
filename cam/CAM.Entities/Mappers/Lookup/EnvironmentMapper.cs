using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class EnvironmentMapper
    {
        public static Models.Lookup.Environment GetEnvironmentMapper(Environments Environment)
        {
            if (Environment == null)
                return null;
            return new Models.Lookup.Environment()
            {
                EnvironmentId = Environment.Environmentid,
                EnvironmentDescription = Environment.Environment,
                CreationDate = Environment.Creationdate,
                CreationUser = Environment.Creationuser,
                ModificationDate = Environment.Modificationdate,
                ModificationUser = Environment.Modificationuser,
                Deleted = Environment.Deleted.Value,
                DeletionDate = Environment.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Environment.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Environment.ModificationuserNavigation),
                
            };
        }
        public static Environments SetEnvironmentMapper(Models.Lookup.Environment Environment)
        {
            return new Environments()
            {
                Environmentid = Environment.EnvironmentId,
                Environment = Environment.EnvironmentDescription,
                Creationdate = Environment.CreationDate,
                Creationuser = Environment.CreationUser,
                Modificationdate = Environment.ModificationDate,
                Modificationuser = Environment.ModificationUser,
                Deleted = Environment.Deleted,
                Deletiondate = Environment.DeletionDate,
            };
        }
    }
}
