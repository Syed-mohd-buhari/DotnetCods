using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class ProblemCategoryMapper
    {
        public static ProblemCategory Get(Problemcategory model)
        {
            if (model == null)
                return null;
            return new ProblemCategory()
            {
                ProblemCategoryId = model.Problemcategoryid,
                ProblemCategoryDescription = model.Problemcategorydescription,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                //Deleted = model.Deleted.Value,
                //DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
        }

        public static Problemcategory Set(ProblemCategory model)
        {
            return new Problemcategory()
            {
                Problemcategoryid = model.ProblemCategoryId,
                Problemcategorydescription = model.ProblemCategoryDescription,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate
            };
        }
    }
}
