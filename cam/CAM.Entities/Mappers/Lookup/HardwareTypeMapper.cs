using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class HardwareTypeMapper
    {
        public static Models.Lookup.HardwareType GetHardwareTypeMapper(Hardwaretypes HardwareType)
        {
            if (HardwareType == null)
                return null;
            return new Models.Lookup.HardwareType()
            {
                HardwareTypeId= HardwareType.Hardwaretypeid,
                HardwareTypeDescription = HardwareType.Hardwaretype,
                CreationDate = HardwareType.Creationdate,
                CreationUser = HardwareType.Creationuser,
                ModificationDate = HardwareType.Modificationdate,
                ModificationUser = HardwareType.Modificationuser,
                Deleted = HardwareType.Deleted.Value,
                DeletionDate = HardwareType.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(HardwareType.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(HardwareType.ModificationuserNavigation),
                
            };
        }
        public static Hardwaretypes SetHardwareTypeMapper(Models.Lookup.HardwareType HardwareType)
        {
            return new Hardwaretypes()
            {
                Hardwaretypeid = HardwareType.HardwareTypeId,
                Hardwaretype = HardwareType.HardwareTypeDescription,
                Creationdate = HardwareType.CreationDate,
                Creationuser = HardwareType.CreationUser,
                Modificationdate = HardwareType.ModificationDate,
                Modificationuser = HardwareType.ModificationUser,
                Deleted = HardwareType.Deleted,
                Deletiondate = HardwareType.DeletionDate,
            };
        }
    }
}
