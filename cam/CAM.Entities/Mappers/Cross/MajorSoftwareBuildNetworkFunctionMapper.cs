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
    public static class MajorSoftwareBuildNetworkFunctionMapper
    {

        public static MajorSoftwareBuildFamilyNetworkFunction Get(Majorsoftwarebuildnetworkfunction model)
        {

            if (model == null)
                return null;
            return new MajorSoftwareBuildFamilyNetworkFunction()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                MajorSoftwareBuildId = model.Majorsoftwarebuildid,
                NetworkFunctionId = model.Networkfunctionid,

                Id = model.Id,
                //DesignComponentFamily = DesignComponentFamilyMapper.Get(model.Designcomponentfamily),
                NetworkFunction = NetworkFunctionMapper.Get(model.Networkfunction)

            };
        }

        public static Majorsoftwarebuildnetworkfunction Set(MajorSoftwareBuildFamilyNetworkFunction model)
        {
            return new Majorsoftwarebuildnetworkfunction()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Majorsoftwarebuildid = model.MajorSoftwareBuildId,
                Id = model.Id,
                Networkfunctionid = model.NetworkFunctionId,
                Majorsoftwarebuild = MajorSoftwareBuildMapper.SetMajorSoftwareBuildMapper(model.MajorSoftwareBuild),
                Networkfunction = NetworkFunctionMapper.Set(model.NetworkFunction)



            };
        }
    }
}
