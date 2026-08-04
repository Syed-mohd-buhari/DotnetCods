using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;
using Type = CAM.Entities.Models.Lookup.Type;

namespace CAM.Entities.Mappers.Lookup
{
    public static class TypeMapper
    {
        public static Type Get(Types model)
        {
            if (model == null)
                return null;
            return new Type()
            {
                Id = model.Id,
                Description = model.Description,
                ClassId = model.Classid,
                CreationDate = model.Creationdate.Value,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate.Value,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                Class = ClassMapper.Get(model.Class),
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),

            };
        }
        public static Types Set(Type model)
        {
            return new Types()
            {
                Id = model.Id,
                Description = model.Description,
                Classid = model.ClassId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
            };
        }
    }
}
