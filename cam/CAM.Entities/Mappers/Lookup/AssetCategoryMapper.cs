using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class AssetCategoryMapper
    {
        public static AssetCategory GetAssetCategoryMapper(Assetcategories AssetCategory)
        {
            if (AssetCategory == null)
                return null;
            return new AssetCategory()
            {
                AssetCategoryId = AssetCategory.Assetcategoryid,
                AssetCategoryDescription = AssetCategory.Assetcategory,
                CreationDate = AssetCategory.Creationdate,
                CreationUser = AssetCategory.Creationuser,
                ModificationDate = AssetCategory.Modificationdate,
                ModificationUser = AssetCategory.Modificationuser,
                Deleted = AssetCategory.Deleted.Value,
                DeletionDate = AssetCategory.Deletiondate,
                AssetClassId = AssetCategory.Assetclassid,
                TakeFromAssetTypeTable = AssetCategory.Takefromassettypetable,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(AssetCategory.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(AssetCategory.ModificationuserNavigation),
                AssetClass = AssetClassMapper.GetAssetClassMapper(AssetCategory.Assetclass),
                
            };
        }
        public static Assetcategories SetAssetCategoryMapper(AssetCategory AssetCategory)
        {
            if (AssetCategory == null)
                return null;
            return new Assetcategories()
            {
                Assetcategoryid = AssetCategory.AssetCategoryId,
                Assetcategory = AssetCategory.AssetCategoryDescription,
                Creationdate = AssetCategory.CreationDate,
                Creationuser = AssetCategory.CreationUser,
                Modificationdate = AssetCategory.ModificationDate,
                Modificationuser = AssetCategory.ModificationUser,
                Deleted = AssetCategory.Deleted,
                Deletiondate = AssetCategory.DeletionDate,
                Assetclassid =AssetCategory.AssetClassId,
                Takefromassettypetable =AssetCategory.TakeFromAssetTypeTable,

            };
        }
    }
}
