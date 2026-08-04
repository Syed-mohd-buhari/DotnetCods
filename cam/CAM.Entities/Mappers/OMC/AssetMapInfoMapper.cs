using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.OMC;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.OMC
{
    public class AssetMapInfoMapper
    {
        public static AssetMapInfo Get(Assetmapinfo model)
        {
            if (model == null)
                return null;
            var result = new AssetMapInfo()
            {
                Assetmapinfoid = model.Assetmapinfoid,
                Omcassetname = model.Omcassetname,
                Temsassetname = model.Temsassetname,
                Enmassetname = model.Enmassetname,
                Site = model.Site,
                Datasourcename = model.Datasourcename,
                DeletionDate = model.Deletiondate,
                Deleted = model.Deleted.Value,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };

            return result;
        }

        public static Assetmapinfo Set(AssetMapInfo model)
        {
            if (model == null)
                return null;
            var result = new Assetmapinfo()
            {
                Assetmapinfoid = model.Assetmapinfoid,
                Omcassetname = model.Omcassetname,
                Temsassetname = model.Temsassetname,
                Enmassetname = model.Enmassetname,
                Site = model.Site,
                Datasourcename = model.Datasourcename,
                Deletiondate = model.DeletionDate,
                Deleted = model.Deleted,

            };
            return result;
        }
    }
}
