using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class ClassMapper
    {
        public static Class Get(Classes model)
        {
            if (model == null)
                return null;
            return new Class()
            {
                Id = model.Id,
                Description = model.Description,
                CategoryId = model.Categoryid,
                CreationDate = model.Creationdate.Value,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate.Value,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                Category = CategoryMapper.Get(model.Category),
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),

            };
        }
        public static Classes Set(Class model)
        {
            return new Classes()
            {
                Id = model.Id,
                Description = model.Description,
                Categoryid = model.CategoryId,
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
