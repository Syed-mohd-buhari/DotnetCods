using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public static class NFVIBundleIDMapper
    {
        public static NFVIBundleID  Get(Nfvibundleids model)
        {
            if (model == null)
                return null;
            return new NFVIBundleID()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                NFVIBundleIDId = model.Nfvibundleidid,
                NFVIBundleIdDescription = model.Nfvibundleid,
                Order = model.Order,
                
            
            };
        }
        public static Nfvibundleids Set(NFVIBundleID model)
        {
            return new Nfvibundleids()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Nfvibundleidid = model.NFVIBundleIDId,
                Nfvibundleid = model.NFVIBundleIdDescription,
                Order = model.Order,
            };
        }
    }
}
