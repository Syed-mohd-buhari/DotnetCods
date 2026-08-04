using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.OMC;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public class AssetAsIsSdiInfoMapper
    {
        public static AssetAsIsSdiInfo Get(Assetasissdiinfo model)
        {
            if (model == null)
                return null;
            var result = new AssetAsIsSdiInfo()
            {
                Assetasissdiinfoid = model.Assetasissdiinfoid,
                Datasourcename = model.Datasourcename,
                Datasourcetype = model.Datasourcetype,
                Swversion = model.Swversion,
                Firmwareversion = model.Firmwareversion,
                Manufacturer = model.Manufacturer,
                Model = model.Model,
                Tsrmodel = model.Tsrmodel,
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

        public static Assetasissdiinfo Set(AssetAsIsSdiInfo model)
        {
            if (model == null)
                return null;
            var result = new Assetasissdiinfo()
            {
                Assetasissdiinfoid = model.Assetasissdiinfoid,
                Datasourcename = model.Datasourcename,
                Datasourcetype = model.Datasourcetype,
                Swversion = model.Swversion,
                Firmwareversion = model.Firmwareversion,
                Manufacturer = model.Manufacturer,
                Model = model.Model,
                Tsrmodel = model.Tsrmodel,
                Deletiondate = model.DeletionDate,
                Deleted = model.Deleted,

            };
            return result;
        }
    }
}
