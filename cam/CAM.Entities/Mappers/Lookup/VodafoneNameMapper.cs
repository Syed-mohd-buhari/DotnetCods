using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public class VodafoneNameMapper
    {
        public static Models.Lookup.VodafoneNames GetVodafoneNamesMapper(Vodafonenames vodafonenames,bool include =true)
        {
            if (vodafonenames == null)
                return null;

            var result = new Models.Lookup.VodafoneNames()
            {
                Id = vodafonenames.Id,
                Description = vodafonenames.Description,
                CreationDate = vodafonenames.Creationdate,
                CreationUser = vodafonenames.Creationuser,
                ModificationDate = vodafonenames.Modificationdate,
                ModificationUser = vodafonenames.Modificationuser,
                Deleted = vodafonenames.Deleted.Value,
                DeletionDate = vodafonenames.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(vodafonenames.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(vodafonenames.ModificationuserNavigation),

            };
            if(include && vodafonenames.Productname != null)
            {
                foreach(var item in vodafonenames.Productname)
                {
                    result.ProductNames.Add(ProductNameMapper.GetProductNameMapper(item,false));
                }
            }           

            if(vodafonenames.Riskclustervodafonenames != null)
            {
                foreach(var item in vodafonenames.Riskclustervodafonenames)
                {
                    result.RiskClusterVodafoneNames.Add(RiskClusterVodafoneNameMapper.Get(item,false));
                }
            }
            return result;
        }
        public static Vodafonenames SetVodafoneNameMapper(VodafoneNames vodafoneNames )
        {
            if (vodafoneNames == null)
                return null;

            var result = new Vodafonenames()
            {
                Id = vodafoneNames.Id,
                Description = vodafoneNames.Description,
                Creationdate = vodafoneNames.CreationDate,
                Creationuser = vodafoneNames.CreationUser,
                Modificationdate = vodafoneNames.ModificationDate,
                Modificationuser = vodafoneNames.ModificationUser,
                Deleted = vodafoneNames.Deleted,
                Deletiondate = vodafoneNames.DeletionDate,

            };
            if (vodafoneNames.ProductNames != null)
            {
                foreach (var item in vodafoneNames.ProductNames)
                {
                    result.Productname.Add(ProductNameMapper.SetProductNameMapper(item));
                }
            }

            if (vodafoneNames.RiskClusterVodafoneNames != null)
            {
                foreach (var item in vodafoneNames.RiskClusterVodafoneNames)
                {
                    result.Riskclustervodafonenames.Add(RiskClusterVodafoneNameMapper.Set(item));
                }
            }
            return result;  
        }
    }
}
