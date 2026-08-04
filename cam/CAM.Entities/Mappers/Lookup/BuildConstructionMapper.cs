using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class BuildConstructionMapper
    {
        public static BuildConstruction GetBuildConstructionMapper(Buildconstructions BuildConstruction)
        {
            if (BuildConstruction == null)
                return null;
            var result = new BuildConstruction()
            {
                BuildConstructionId = BuildConstruction.Buildconstructionid,
                BuildConstructionDescription = BuildConstruction.Buildconstruction,
                CreationDate = BuildConstruction.Creationdate,
                CreationUser = BuildConstruction.Creationuser,
                ModificationDate = BuildConstruction.Modificationdate,
                ModificationUser = BuildConstruction.Modificationuser,
                Deleted = BuildConstruction.Deleted.Value,
                DeletionDate = BuildConstruction.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(BuildConstruction.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(BuildConstruction.ModificationuserNavigation),
                Rule = BuildConstruction.Rule,
                CloudType = BuildConstruction.Cloudtype,
                IsCluodHostedAsset = BuildConstruction.Iscloudasset,
            };

            return result;
        }
        public static Buildconstructions SetBuildConstructionMapper(BuildConstruction BuildConstruction)
        {
            if (BuildConstruction == null)
                return null;
            return new Buildconstructions()
            {
                Buildconstructionid = BuildConstruction.BuildConstructionId,
                Buildconstruction = BuildConstruction.BuildConstructionDescription,
                Creationdate = BuildConstruction.CreationDate,
                Creationuser = BuildConstruction.CreationUser,
                Modificationdate = BuildConstruction.ModificationDate,
                Modificationuser = BuildConstruction.ModificationUser,
                Deleted = BuildConstruction.Deleted,
                Deletiondate = BuildConstruction.DeletionDate,
                Rule = BuildConstruction.Rule,
                Cloudtype = BuildConstruction.CloudType,
                Iscloudasset = BuildConstruction.IsCluodHostedAsset,
            };
        }
    }
}
