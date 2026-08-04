using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class MajorHwBuidlsDesignContactsMapper
    {
        public static MajorHwBuidlsDesignContact Get(Majorhwbuildsdesigncontacts model, bool Include = true)
        {
            if (model == null)
                return null;
            return new MajorHwBuidlsDesignContact()
            {
                MajorHwBuidlsDesignContactId = model.Majorhwbuildsdesigncontactsid,
                MajorHardwareBuildsId = model.Majorhardwarebuildsid,
                DesignContactId = model.Designcontactid,
               // MajorHardwareBuilds =Include? MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(model.Majorhardwarebuilds):null,
                DesignContact = ApplicationUserMapper.GetApplicationUserMapper(model.Designcontact),
                //CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                //ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),              
            };
        }

        public static Majorhwbuildsdesigncontacts Set(MajorHwBuidlsDesignContact model)
        {
            if (model == null)
                return null;
            return new Majorhwbuildsdesigncontacts()
            {
                Majorhwbuildsdesigncontactsid = model.MajorHwBuidlsDesignContactId,
                Majorhardwarebuildsid = model.MajorHardwareBuildsId ,
                Designcontactid = model.DesignContactId,
                //Majorhardwarebuilds = MajorHardwareBuildMapper.SetMajorHardwareBuildMapper(model.MajorHardwareBuilds),
                //Designcontact = ApplicationUserMapper.SetApplicationUserMapper(model.DesignContact),
                //CreationuserNavigation = ApplicationUserMapper.SetApplicationUserMapper(model.CreationUserEntity),
                //ModificationuserNavigation = ApplicationUserMapper.SetApplicationUserMapper(model.ModificationUserEntity),
            };
        }
    }
}
