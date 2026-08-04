using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class RiskClusterVodafoneNameMapper
    {
        public static RiskClusterVodafoneNames Get (Riskclustervodafonenames model, bool include = true)
        {
            if (model == null)
                return null;
            var result = new RiskClusterVodafoneNames()
            {
                Riskclustervodafonenameid = model.Riskclustervodafonenameid,
                Riskclusterid = model.Riskclusterid,
                Vodafonenameid = model.Vodafonenameid,
                CreationDate  =model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate =model.Modificationdate,
                ModificationUser =model.Modificationuser,
                Deleted =model.Deleted.Value,
                DeletionDate =model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                              
            };
            if(model.Riskcluster != null)
            {
                result.Riskcluster = RiskClusterMapper.Get(model.Riskcluster, false);
            }
            if(include && model.Vodafonename != null)
            {
                result.VodafoneName = VodafoneNameMapper.GetVodafoneNamesMapper(model.Vodafonename,false);
            }
            
            return result;
        }
        public static Riskclustervodafonenames Set(RiskClusterVodafoneNames model)
        {
            return new Riskclustervodafonenames()
            {
                Riskclustervodafonenameid = model.Riskclustervodafonenameid,
                Riskclusterid = model.Riskclusterid,
                Vodafonenameid = model.Vodafonenameid,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,  
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Riskcluster = RiskClusterMapper.Set(model.Riskcluster),
                Vodafonename = VodafoneNameMapper.SetVodafoneNameMapper(model.VodafoneName),
                     
            };
        }
    }
}
