using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Settings;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Setting
{
    public static class CrossSettingsUpdatePlannedActivityMapper
    {
        public static CrossSettingsUpdatePlannedActivity Get (Crosssettingsupdateplannedactivity model)
        {
            if (model == null)
                return null;
            return new CrossSettingsUpdatePlannedActivity()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Id = model.Id,
                CrossSettingsRule = model.Crosssettingsrule,
                SettingsUpdatePlannedActivityInId = model.Settingsupdateplnactinid,
                SettingsUpdatePlannedActivityOutId = model.Settingsupdateplnactoutid,
                SettingsUpdatePlannedActivityIn = SettingsUpdatePlannedActivityMapper.Get(model.Settingsupdateplnactin),
                SettingsUpdatePlannedActivityOut = SettingsUpdatePlannedActivityMapper.Get(model.Settingsupdateplnactout),
                
            };
        }

        public static Crosssettingsupdateplannedactivity  Set(CrossSettingsUpdatePlannedActivity model)
        {
            return new Crosssettingsupdateplannedactivity()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Id = model.Id,
                Crosssettingsrule = model.CrossSettingsRule,
                Settingsupdateplnactinid = model.SettingsUpdatePlannedActivityInId,
                Settingsupdateplnactoutid = model.SettingsUpdatePlannedActivityOutId,

            };
        }
    }
}
