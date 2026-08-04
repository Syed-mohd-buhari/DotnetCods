using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.OMC;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public class AssetAsIsSdiSwitchInfoMapper
    {
        public static AssetAsIsSdiSwitchInfo Get(Assetasissdiswitchinfo model)
        {
            if (model == null)
                return null;
            var result = new AssetAsIsSdiSwitchInfo()
            {
                Assetasissdiswitchinfoid = model.Assetasissdiswitchinfoid,
                Datasourcename = model.Datasourcename,
                Switchname = model.Switchname,
                Switchid = model.Switchid,
                Switchadminstate = model.Switchadminstate,
                Switchuniqueid = model.Switchuniqueid,
                Switchrole = model.Switchrole,
                Switchrack = model.Switchrack,
                Switchlabel = model.Switchlabel,
                Switchserialnumber = model.Switchserialnumber,
                Switchopsstate = model.Switchopsstate,
                Switchnetwork = model.Switchnetwork,
                Switchmanufacturer = model.Switchmanufacturer,
                Switchmodel = model.Switchmodel,
                Switchipaddress = model.Switchipaddress,
                Switchswversion = model.Switchswversion,
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

        public static Assetasissdiswitchinfo Set(AssetAsIsSdiSwitchInfo model)
        {
            if (model == null)
                return null;
            var result = new Assetasissdiswitchinfo()
            {
                Assetasissdiswitchinfoid = model.Assetasissdiswitchinfoid,
                Datasourcename = model.Datasourcename,
                Switchname = model.Switchname,
                Switchid = model.Switchid,
                Switchadminstate = model.Switchadminstate,
                Switchuniqueid = model.Switchuniqueid,
                Switchrole = model.Switchrole,
                Switchrack = model.Switchrack,
                Switchlabel = model.Switchlabel,
                Switchserialnumber = model.Switchserialnumber,
                Switchopsstate = model.Switchopsstate,
                Switchnetwork = model.Switchnetwork,
                Switchmanufacturer = model.Switchmanufacturer,
                Switchmodel = model.Switchmodel,
                Switchipaddress = model.Switchipaddress,
                Switchswversion = model.Switchswversion,
                Deletiondate = model.DeletionDate,
                Deleted = model.Deleted,

            };
            return result;
        }
    }
}
