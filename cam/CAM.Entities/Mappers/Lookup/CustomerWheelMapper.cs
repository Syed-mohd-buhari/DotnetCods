using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class CustomerWheelMapper
    {
        public static CustomerWheel Get(Customerwheels CustomerWheel)
        {
            if (CustomerWheel == null)
                return null;
            return new CustomerWheel()
            {
                Id = CustomerWheel.Id,
                Description = CustomerWheel.Description,
                CreationDate = CustomerWheel.Creationdate,
                CreationUser = CustomerWheel.Creationuser,
                ModificationDate = CustomerWheel.Modificationdate,
                ModificationUser = CustomerWheel.Modificationuser,
                Deleted = CustomerWheel.Deleted.Value,
                DeletionDate = CustomerWheel.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(CustomerWheel.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(CustomerWheel.ModificationuserNavigation),

            };
        }
        public static Customerwheels Set(CustomerWheel CustomerWheel)
        {
            return new Customerwheels()
            {
                Id = CustomerWheel.Id,
                Description = CustomerWheel.Description,
                Creationdate = CustomerWheel.CreationDate,
                Creationuser = CustomerWheel.CreationUser,
                Modificationdate = CustomerWheel.ModificationDate,
                Modificationuser = CustomerWheel.ModificationUser,
                Deleted = CustomerWheel.Deleted,
                Deletiondate = CustomerWheel.DeletionDate,
            };
        }
    }
}
