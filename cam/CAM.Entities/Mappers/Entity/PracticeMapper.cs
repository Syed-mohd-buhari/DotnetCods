using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class PracticeMapper
    {
        public static PracticeModel GetPracticeMapper(Practice model)
        {
            if (model == null)
                return null;
            return new PracticeModel()
            {
                PracticeId = model.Practiceid,
                PracticeDescription = model.Practicedescription,
                PracticeEmailId = model.Practiceemailid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationUser = model.Modificationuser,
                ModificationDate = model.Modificationdate,
                PracticeEmail = ApplicationUserMapper.GetApplicationUserMapper(model.Practiceemail),
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
        }

        public static Practice SetPracticeMapper(PracticeModel model)
        {
            if (model == null)
                return null;
            return new Practice()
            {
                Practiceid = model.PracticeId,
                Practicedescription = model.PracticeDescription,
                Practiceemailid = model.PracticeEmailId,
                Deleted = model.Deleted
                //Creationdate = model.CreationDate,               
                //Modificationdate = model.ModificationDate,
                //PracticeEmail = ApplicationUserMapper.GetApplicationUserMapper(model.Practiceemail),
                //CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                //ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
        }

        public static ApplicationPracticeModel GetApplicationPracticeMapper(Practice model)
        {
            if (model == null)
                return null;
            return new ApplicationPracticeModel()
            {
                PracticeId = model.Practiceid,
                PracticeDescription = model.Practicedescription,
                PracticeEmailId = model.Practiceemailid,
                PracticeEmail = ApplicationUserMapper.GetApplicationUserMapper(model.Practiceemail),
            };
        }
    }
}
