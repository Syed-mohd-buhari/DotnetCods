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
    public static class DesignComponentFamilyCustomerWheelMapper
    {
        public static DesignComponentFamilyCustomerWheel Get(Designcomponentfamilycustomerwheel model)
        {

            if (model == null)
                return null;
            return new DesignComponentFamilyCustomerWheel()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                DesignComponentFamilyId = model.Designcomponentfamilyid.Value,
                CustomerWheelId = model.Customerwheelid.Value,

                Id = model.Id,
                //DesignComponentFamily = DesignComponentFamilyMapper.Get(model.Designcomponentfamily),
                CustomerWheel = CustomerWheelMapper.Get(model.Customerwheel)

            };
        }

        public static Designcomponentfamilycustomerwheel Set(DesignComponentFamilyCustomerWheel model)
        {
            return new Designcomponentfamilycustomerwheel()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Designcomponentfamilyid = model.DesignComponentFamilyId,
                Id = model.Id,
                Customerwheelid = model.CustomerWheelId,
                Designcomponentfamily = DesignComponentFamilyMapper.Set(model.DesignComponentFamily),
                Customerwheel = CustomerWheelMapper.Set(model.CustomerWheel)



            };
        }
    }
}
