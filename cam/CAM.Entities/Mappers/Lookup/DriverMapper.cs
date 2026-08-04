using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class DriverMapper
    {
        public static Driver GetDriverMapper(Drivers Driver)
        {
            if (Driver == null)
                return null;
            return new Driver()
            {
                DriverId = Driver.Driverid,
                DriverDescription = Driver.Driver,
                CreationDate = Driver.Creationdate,
                CreationUser = Driver.Creationuser,
                ModificationDate = Driver.Modificationdate,
                ModificationUser = Driver.Modificationuser,
                Deleted = Driver.Deleted.Value,
                DeletionDate = Driver.Deletiondate,
                BptDriverDetails = Driver.Bptdriverdetails,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Driver.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Driver.ModificationuserNavigation),
                 
            };
        }
        public static Drivers SetDriverMapper(Driver Driver)
        {
            return new Drivers()
            {
                Driverid = Driver.DriverId,
                Driver = Driver.DriverDescription,
                Creationdate = Driver.CreationDate,
                Creationuser = Driver.CreationUser,
                Modificationdate = Driver.ModificationDate,
                Modificationuser = Driver.ModificationUser,
                Deleted = Driver.Deleted,
                Deletiondate = Driver.DeletionDate,
                Bptdriverdetails = Driver.BptDriverDetails,
            };
        }
    }
}

