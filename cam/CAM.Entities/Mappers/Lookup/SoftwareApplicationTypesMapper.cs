using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public class ProductNameMapper
    {
        public static Models.Lookup.ProductName GetProductNameMapper(Productname productname, bool includeVodafoneName = true)
        {
            if (productname == null)
                return null;
            var result = new Models.Lookup.ProductName()
            {
                Id = productname.Productnameid,
                Description = productname.Description,
                CreationDate = productname.Creationdate,
                CreationUser = productname.Creationuser,
                ModificationDate = productname.Modificationdate,
                ModificationUser = productname.Modificationuser,
                Deleted = productname.Deleted.Value,
                DeletionDate = productname.Deletiondate,              
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(productname.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(productname.ModificationuserNavigation),
                VodafoneNameId = productname.Vodafonenamesid,
                IsPlatformSoftware = productname.Isplatformsoftware
                
            };
            if(includeVodafoneName && productname.Vodafonenames != null)
            {
                result.VodafoneName = VodafoneNameMapper.GetVodafoneNamesMapper(productname.Vodafonenames, false);
            }
            return result;
        }
        public static Productname SetProductNameMapper(ProductName ProductName)
        {
            if (ProductName == null)
                return null;
            return new Productname()
            {
                Productnameid = ProductName.Id,
                Description = ProductName.Description,
                Creationdate = ProductName.CreationDate,
                Creationuser = ProductName.CreationUser,
                Modificationdate = ProductName.ModificationDate,
                Modificationuser = ProductName.ModificationUser,
                Deleted = ProductName.Deleted,
                Deletiondate = ProductName.DeletionDate,
                Vodafonenamesid = ProductName.VodafoneNameId,
                Isplatformsoftware = ProductName.IsPlatformSoftware
            };
        }
    }
}
