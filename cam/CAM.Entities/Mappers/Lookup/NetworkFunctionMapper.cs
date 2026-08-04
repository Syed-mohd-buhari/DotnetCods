using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class NetworkFunctionMapper
    {
        public static NetworkFunction Get(Networkfunctions NetworkFunction)
        {
            if (NetworkFunction == null)
                return null;
            return new NetworkFunction()
            {
                Id = NetworkFunction.Id,
                Description = NetworkFunction.Description,
                CreationDate = NetworkFunction.Creationdate,
                CreationUser = NetworkFunction.Creationuser,
                ModificationDate = NetworkFunction.Modificationdate,
                ModificationUser = NetworkFunction.Modificationuser,
                Deleted = NetworkFunction.Deleted.Value,
                DeletionDate = NetworkFunction.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(NetworkFunction.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(NetworkFunction.ModificationuserNavigation),

            };
        }
        public static Networkfunctions Set(NetworkFunction NetworkFunction)
        {
            return new Networkfunctions()
            {
                Id = NetworkFunction.Id,
                Description = NetworkFunction.Description,
                Creationdate = NetworkFunction.CreationDate,
                Creationuser = NetworkFunction.CreationUser,
                Modificationdate = NetworkFunction.ModificationDate,
                Modificationuser = NetworkFunction.ModificationUser,
                Deleted = NetworkFunction.Deleted,
                Deletiondate = NetworkFunction.DeletionDate,
            };
        }
    }
}
