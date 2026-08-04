using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class PlatFormMapper
    {
        public static Models.Lookup.Platform GetPlatFormMapper(Platforms PlatForm)
        {
            if (PlatForm == null)
                return null;
            return new Models.Lookup.Platform()
            {
                PlatformId = PlatForm.Platformid,
                PlatformDescription = PlatForm.Platform,
                CreationDate = PlatForm.Creationdate,
                CreationUser = PlatForm.Creationuser,
                ModificationDate = PlatForm.Modificationdate,
                ModificationUser = PlatForm.Modificationuser,
                Deleted = PlatForm.Deleted.Value,
                DeletionDate = PlatForm.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(PlatForm.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(PlatForm.ModificationuserNavigation),
                
            };
        }
        public static Platforms SetPlatFormMapper(Models.Lookup.Platform PlatForm)
        {
            if (PlatForm == null)
                return null;
            return new Platforms()
            {
                Platformid = PlatForm.PlatformId,
                Platform = PlatForm.PlatformDescription,
                Creationdate = PlatForm.CreationDate,
                Creationuser = PlatForm.CreationUser,
                Modificationdate = PlatForm.ModificationDate,
                Modificationuser = PlatForm.ModificationUser,
                Deleted = PlatForm.Deleted,
                Deletiondate = PlatForm.DeletionDate,
            };
        }
    }
}
