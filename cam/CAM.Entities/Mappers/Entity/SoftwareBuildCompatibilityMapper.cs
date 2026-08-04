using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Mappers.Entity
{
    public static class SoftwareBuildCompatibilityMapper
    {
        public static SoftwareBuildCompatibility Get(Softwarebuildcompatibility model)
        {
            if (model == null)
                return null;
            return new SoftwareBuildCompatibility()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),

                SoftwareBuildCompatibilityId = model.Softwarebuildcompatibilityid,
                MajorSoftwareBuildId = model.Majorsoftwarebuildid,
                BundleMajorSoftwareBuildId = model .Bundlemajorsoftwarebuildid,
                BundleType = model.Bundletype ,
                //BundleMajorSoftwareBuildNavigation = MajorSoftwareBuildMapper
                //.GetMajorSoftwareBuildMapper(model.Bundlemajorsoftwarebuild.SoftwarebuildcompatibilityBundlemajorsoftwarebuild)



            };
        }

        public static Softwarebuildcompatibility Set(SoftwareBuildCompatibility model)
        {
            if (model == null)
                return null;
            return new Softwarebuildcompatibility()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                 
                Softwarebuildcompatibilityid = model.SoftwareBuildCompatibilityId,
                Majorsoftwarebuildid = model.MajorSoftwareBuildId,
                Bundlemajorsoftwarebuildid = model.BundleMajorSoftwareBuildId,
                Bundletype = model.BundleType,
            };
        }
    }
}
