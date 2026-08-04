using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class SystemFunctionMapper
    {
        public static SystemFunction Get(Systemfunctions model)
        {
            if (model == null)
                return null;
            return new SystemFunction()
            {
                SystemFunctionId = model.Systemfunctionid,
                SystemFunctionDescription = model.Systemfunction,
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
        public static Systemfunctions Set(SystemFunction model)
        {
            return new Systemfunctions()
            {
                Systemfunctionid = model.SystemFunctionId,
                Systemfunction = model.SystemFunctionDescription,
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
