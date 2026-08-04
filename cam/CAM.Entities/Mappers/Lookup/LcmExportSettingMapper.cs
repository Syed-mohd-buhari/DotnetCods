using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class LcmExportSettingMapper
    {
        public static LcmExportSetting Get(Lcmexportsettings model)
        {
            if (model == null)
                return null;
            return new LcmExportSetting()
            {
                Id = model.Id,
                Description = model.Description,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                LcmHistoricalInfoSW = model.Lcmhistoricalinfosw,
                LcmHistoricalInfoHW = model.Lcmhistoricalinfohw,
                IsHistorical = model.Ishistorical,
                ReportLevel = model.Reportlevel,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                IsDefault = model.Isdefault
            };
        }
        public static Lcmexportsettings Set(LcmExportSetting model)
        {
            return new Lcmexportsettings()
            {
                Id = model.Id,
                Description = model.Description,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Lcmhistoricalinfosw = model.LcmHistoricalInfoSW,
                Lcmhistoricalinfohw = model.LcmHistoricalInfoHW,
                Ishistorical = model.IsHistorical,
                Reportlevel = model.ReportLevel,
                Isdefault = model.IsDefault
            };
        }
    }
}
