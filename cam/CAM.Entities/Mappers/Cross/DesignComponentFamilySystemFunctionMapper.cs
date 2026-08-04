using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Cross
{
    public static class DesignComponentFamilySystemFunctionMapper
    {
        public static DesignComponentFamilySystemFunction Get(Designcomponentfamilysystemfunction model)
        {

            if (model == null)
                return null;
            return new DesignComponentFamilySystemFunction()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                DesignComponentFamilyId = model.Designcomponentfamilyid,
                DesignComponentFamilySystemFunctionId = model.Designcompfamilysysfuncid,
                SystemFunctionId = model.Systemfunctionid,
                //DesignComponentFamily = DesignComponentFamilyMapper.Get(model.Designcomponentfamily),
                SystemFunction = SystemFunctionMapper.Get(model.Systemfunction)

            };
        }

        public static Designcomponentfamilysystemfunction Set(DesignComponentFamilySystemFunction model)
        {
            return new Designcomponentfamilysystemfunction()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Designcomponentfamilyid = model.DesignComponentFamilyId,
                Designcompfamilysysfuncid = model.DesignComponentFamilySystemFunctionId,
                Systemfunctionid = model.SystemFunctionId,
                Designcomponentfamily = DesignComponentFamilyMapper.Set(model.DesignComponentFamily),
                Systemfunction = SystemFunctionMapper.Set(model.SystemFunction)



            };
        }
    }
}
