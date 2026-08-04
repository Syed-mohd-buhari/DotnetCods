using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class OriginalEquipmentManufacturerMapper
    {
        public static Models.Lookup.OriginalEquipmentManufacturer GetOriginalEquipmentManufacturerMapper(Originalequipmentmanufacturers OriginalEquipmentManufacturer)
        {
            if (OriginalEquipmentManufacturer == null)
                return null;
            return new Models.Lookup.OriginalEquipmentManufacturer()
            {
                OriginalEquipmentManufacturerId= OriginalEquipmentManufacturer.Orgeqpmanufacturerid,
                OriginalEquipmentManufacturerDescription = OriginalEquipmentManufacturer.Originalequipmentmanufacturer,
                CreationDate = OriginalEquipmentManufacturer.Creationdate,
                CreationUser = OriginalEquipmentManufacturer.Creationuser,
                ModificationDate = OriginalEquipmentManufacturer.Modificationdate,
                ModificationUser = OriginalEquipmentManufacturer.Modificationuser,
                Deleted = OriginalEquipmentManufacturer.Deleted.Value,
                DeletionDate = OriginalEquipmentManufacturer.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(OriginalEquipmentManufacturer.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(OriginalEquipmentManufacturer.ModificationuserNavigation),
               
            };
        }
        public static Originalequipmentmanufacturers SetOriginalEquipmentManufacturerMapper(Models.Lookup.OriginalEquipmentManufacturer OriginalEquipmentManufacturer)
        {
            if (OriginalEquipmentManufacturer == null)
                return null;
            return new Originalequipmentmanufacturers()
            {

                Orgeqpmanufacturerid = OriginalEquipmentManufacturer.OriginalEquipmentManufacturerId,
                Originalequipmentmanufacturer = OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription,
                Creationdate = OriginalEquipmentManufacturer.CreationDate,
                Creationuser = OriginalEquipmentManufacturer.CreationUser,
                Modificationdate = OriginalEquipmentManufacturer.ModificationDate,
                Modificationuser = OriginalEquipmentManufacturer.ModificationUser,
                Deleted = OriginalEquipmentManufacturer.Deleted,
                Deletiondate = OriginalEquipmentManufacturer.DeletionDate,
            };
        }
    }
}
