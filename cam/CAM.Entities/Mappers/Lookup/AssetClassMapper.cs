using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class AssetClassMapper
    {
        public static AssetClass GetAssetClassMapper(Assetclasses AssetClass)
        {
            if (AssetClass == null)
                return null;
            return new AssetClass()
            {
                AssetClassId = AssetClass.Assetclassid,
                AssetCategoryId = AssetClass.Assetcategoryid,
                AssetClassDescription = AssetClass.Assetclass,
                CreationDate = AssetClass.Creationdate,
                CreationUser = AssetClass.Creationuser,
                ModificationDate = AssetClass.Modificationdate,
                ModificationUser = AssetClass.Modificationuser,
                Deleted = AssetClass.Deleted.Value,
                DeletionDate = AssetClass.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(AssetClass.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(AssetClass.ModificationuserNavigation),
                
            };
        }
        public static Assetclasses SetAssetClassMapper(AssetClass AssetClass)
        {
            if (AssetClass == null)
                return null;
            return new Assetclasses()
            {
                Assetclassid = AssetClass.AssetClassId,
                Assetcategoryid = AssetClass.AssetCategoryId,
                Assetclass = AssetClass.AssetClassDescription,
                Creationdate = AssetClass.CreationDate,
                Creationuser = AssetClass.CreationUser,
                Modificationdate = AssetClass.ModificationDate,
                Modificationuser = AssetClass.ModificationUser,
                Deleted = AssetClass.Deleted,
                Deletiondate = AssetClass.DeletionDate,
              

            };
        }
    }
}
