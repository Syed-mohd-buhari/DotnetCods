using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class AssetTypeMapper
    {
        public static AssetType GetAssetTypeMapper(Assettypes AssetType)
        {
            if (AssetType == null)
                return null;
            return new AssetType()
            {
                AssetTypeId = AssetType.Assettypeid,
                AssetCategoryId = AssetType.Assetcategoryid,
                AssetClassId =AssetType.Assetclassid,
                AssetTypeDescription = AssetType.Assettype,
                CreationDate = AssetType.Creationdate,
                CreationUser = AssetType.Creationuser,
                ModificationDate = AssetType.Modificationdate,
                ModificationUser = AssetType.Modificationuser,
                Deleted = AssetType.Deleted.Value,
                DeletionDate = AssetType.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(AssetType.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(AssetType.ModificationuserNavigation),
                AssetCategory =AssetCategoryMapper.GetAssetCategoryMapper(AssetType.Assetcategory),
                
            };
        }
        public static Assettypes SetAssetTypeMapper(AssetType AssetType)
        {
            if (AssetType == null)
                return null;
            return new Assettypes()
            {
                Assettypeid = AssetType.AssetTypeId,
                Assetcategoryid = AssetType.AssetCategoryId,
                Assettype = AssetType.AssetTypeDescription,
                Creationdate = AssetType.CreationDate,
                Creationuser = AssetType.CreationUser,
                Modificationdate = AssetType.ModificationDate,
                Modificationuser = AssetType.ModificationUser,
                Deleted = AssetType.Deleted,
                Deletiondate = AssetType.DeletionDate,
                Assetclassid =AssetType.AssetClassId,

            };
        }
    }
}
