using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public class SiteMapper
    {
        public static SiteEntity Get(Sites model)
        {
            if (model == null)
                return null;
            return new SiteEntity()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted != null ? model.Deleted.Value : false,
                DeletionDate = model.Deletiondate,
                SiteId = model.Siteid,
                SiteCategory = model.Sitecategory,
                SiteCode = model.Sitecode,
                LocationId = model.Locationid,
                Region = model.Region,
                Location = LocationMapper.GetLocationMapper(model.Location),
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };
        }
        public static Sites Set(SiteEntity model)
        {
            return new Sites()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Siteid = model.SiteId,
                Sitecategory = model.SiteCategory,
                Sitecode = model.SiteCode,
                Region = model.Region,
                Locationid = model.LocationId,
            };
        }
    }
}
