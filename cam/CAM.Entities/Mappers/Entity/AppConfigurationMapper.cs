using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class AppConfigurationMapper
    {
        public static AppSetting Get(Appsettings model)
        {
            if (model == null)
                return null;
            var result = new AppSetting()
            {
                AppSettingId = model.Appsettingid,
                Description = model.Description,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                              
            };
            return result;
        }

        public static Appsettings Set(AppSetting model)
        {
            return new Appsettings()
            {
                Appsettingid = model.AppSettingId,
                Description = model.Description,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser

            };
        }
    }
}
