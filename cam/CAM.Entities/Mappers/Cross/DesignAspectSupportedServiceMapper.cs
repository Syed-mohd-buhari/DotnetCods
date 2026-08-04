using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Cross
{
    public static class DesignAspectSupportedServiceMapper
    {
        public static DesignAspectSupportedService Get(Designaspectssupportedsvr model)
        {

            if (model == null)
                return null;
            return new DesignAspectSupportedService()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser.Value,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser.Value,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                DesignAspectId = model.Designaspectid,
                ServiceId = model.Serviceid,
                Id = model.Id,
               // DesignAspect = DesignAspectMapper.Get(model.Designaspect),
                SupportedService = SupportedServiceMapper.Get(model.Service)

            };
        }

        public static Designaspectssupportedsvr Set(DesignAspectSupportedService model)
        {
            return new Designaspectssupportedsvr()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Designaspectid = model.DesignAspectId,
                Id = model.Id,
                Serviceid = model.ServiceId,
                //Designaspect = DesignAspectMapper.Set(model.DesignAspect),
                Service = SupportedServiceMapper.Set(model.SupportedService)



            };
        }
    }
}
