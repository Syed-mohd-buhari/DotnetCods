using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;


namespace CAM.Entities.Mappers.Lookup
{
    public static class ProductImportanceMapper
    {
        public static Models.Lookup.ProductImportance GetProductImportanceMapper(Productimportances ProductImportance)
        {

            if (ProductImportance == null)
                return null;
            return new Models.Lookup.ProductImportance()
            {
                ProductImportanceId = ProductImportance.Productimportanceid,
                ProductImportanceDescription = ProductImportance.Productimportance,
                CreationDate = ProductImportance.Creationdate,
                CreationUser = ProductImportance.Creationuser,
                ModificationDate = ProductImportance.Modificationdate,
                ModificationUser = ProductImportance.Modificationuser,
                Deleted = ProductImportance.Deleted.Value,
                DeletionDate = ProductImportance.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(ProductImportance.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(ProductImportance.ModificationuserNavigation),
                
            };
        }
        public static Productimportances SetProductImportanceMapper(Models.Lookup.ProductImportance ProductImportance)
        {
            if (ProductImportance == null)
                return null;
            return new Productimportances()
            {
                Productimportanceid = ProductImportance.ProductImportanceId,
                Productimportance = ProductImportance.ProductImportanceDescription,
                Creationdate = ProductImportance.CreationDate,
                Creationuser = ProductImportance.CreationUser,
                Modificationdate = ProductImportance.ModificationDate,
                Modificationuser = ProductImportance.ModificationUser,
                Deleted = ProductImportance.Deleted,
                Deletiondate = ProductImportance.DeletionDate,
            };
        }
    }
}
