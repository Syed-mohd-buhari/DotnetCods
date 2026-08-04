using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class AppConfigurationSeetingsMapper
    {
        public static AppSettingsConfiguration Get(Appsettingsconfiguration model)
        {
            if (model == null)
                return null;
            var result = new AppSettingsConfiguration()
            {
                AppSettingsConfigurationId = model.Appconfigurationsettingid,
                AppSettingsId = model.Appsettingid,
                SettingsValue = model.Settingsvalue,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                AppConfiguration = AppConfigurationMapper.Get(model.Appsetting),
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                              
            };
            return result;
        }

        public static Appsettingsconfiguration Set(AppSettingsConfiguration model)
        {
            return new Appsettingsconfiguration()
            {
                Appconfigurationsettingid = model.AppSettingsConfigurationId,
                Appsettingid = model.AppSettingsId,
                Settingsvalue = model.SettingsValue,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser

            };
        }
    }
}
