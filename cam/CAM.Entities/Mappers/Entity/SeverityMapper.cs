using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class SeverityMapper
    {
        public static SeverityEntity Get(Severity model)
        {
            if (model == null)
                return null;
            return new SeverityEntity()
            {
                SeverityId = model.Severityid,
                SeverityDescription = model.Severitydescription,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationUser = model.Modificationuser,
                ModificationDate = model.Modificationdate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
        }


    }
}
