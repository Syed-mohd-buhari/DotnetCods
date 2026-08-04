using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Identity;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class MainOrganisationMapper
    {
        public static MainOrganisation GetMainOrganisationMapper(Mainorganisation model)
        {
            if (model == null)
                return null;
            return new MainOrganisation()
            {
                MainorganisationId = model.Mainorganisationid,
                MainorganisationDescription = model.Mainorganisationdescription,
                CreationDate = model.Creationdate,
                //CreationUser = model.Creationuser,
                //ModificationUser = model.Modificationuser,
                ModificationDate = model.Modificationdate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
        }


        public static Mainorganisation SetMainOrganisationMapper(MainOrganisation model)
        {
            if (model == null)
                return null;
            return new Mainorganisation()
            {
                Mainorganisationid = model.MainorganisationId,
                Mainorganisationdescription = model.MainorganisationDescription,
                Deleted = model.Deleted,
                //CreationDate = model.Creationdate,
                ////CreationUser = model.Creationuser,
                ////ModificationUser = model.Modificationuser,
                //ModificationDate = model.Modificationdate,
                //CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                //ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
        }

        public static ApplicationMainOrganisation GetApplicationMainOrganisationMapper(Mainorganisation model)
        {
            if (model == null)
                return null;
            return new ApplicationMainOrganisation()
            {
                MainorganisationId = model.Mainorganisationid,
                MainorganisationDescription = model.Mainorganisationdescription,
            };
        }

    }
}
