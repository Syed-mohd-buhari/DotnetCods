using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class RiskClusterMapper
    {
        public static RiskClusters Get (Riskclusters model,bool include= true)
        {
            if (model == null)
                return null;
            var result = new RiskClusters()
            {
                Riskclusterid = model.Riskclusterid,
                Description =model.Description,
                Risklevel = model.Risklevel,
                CreationDate  =model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate =model.Modificationdate,
                ModificationUser =model.Modificationuser,
                Deleted =model.Deleted.Value,
                DeletionDate =model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };
            if (include && model.Riskclustervodafonenames != null)
            {
                foreach (var item in model.Riskclustervodafonenames)
                {
                    result.RiskClusterVodafoneNames.Add(RiskClusterVodafoneNameMapper.Get(item,false));
                }
            }
            return result;
        }
        public static Riskclusters Set(RiskClusters model)
        {
            return new Riskclusters()
            {
                Riskclusterid = model.Riskclusterid,
                Description = model.Description,
                Risklevel = model.Risklevel,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,  
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
               
       
            };
        }
    }
}
