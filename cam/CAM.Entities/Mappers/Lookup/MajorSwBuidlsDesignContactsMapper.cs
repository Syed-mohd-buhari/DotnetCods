using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class MajorSwBuidlsDesignContactsMapper
    {
        public static MajorSwBuidlsDesignContact Get(Majorswbuildsdesigncontacts model)
        {
            if (model == null)
                return null;
            return new MajorSwBuidlsDesignContact()
            {
                MajorSwBuidlsDesignContactId = model.Majorswbuidlsdesigncontactid,
                MajorsoftwarebuildsId = model.Majorsoftwarebuildsid,
                DesignContactId = model.Designcontactid,
                //MajorsoftwareBuilds = MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(model.Majorsoftwarebuilds),
                 DesignContact = ApplicationUserMapper.GetApplicationUserMapper(model.Designcontact),
               // CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                //ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),              
            };
        }

        public static Majorswbuildsdesigncontacts Set(MajorSwBuidlsDesignContact model)
        {
            if (model == null)
                return null;
            return new Majorswbuildsdesigncontacts()
            {
                Majorswbuidlsdesigncontactid = model.MajorSwBuidlsDesignContactId,
                Majorsoftwarebuildsid = model.MajorsoftwarebuildsId ,
                //Designcontactid = model.DesignContactId,
                //Majorsoftwarebuilds = MajorSoftwareBuildMapper.SetMajorSoftwareBuildMapper(model.MajorsoftwareBuilds),
                //Designcontact = ApplicationUserMapper.SetApplicationUserMapper(model.DesignContact),
                //CreationuserNavigation = ApplicationUserMapper.SetApplicationUserMapper(model.CreationUserEntity),
                //ModificationuserNavigation = ApplicationUserMapper.SetApplicationUserMapper(model.ModificationUserEntity),
            };
        }
    }
}
