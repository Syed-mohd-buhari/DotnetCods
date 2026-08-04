using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class EquipmentStatusMapper
    {
        public static Models.Lookup.EquipmentStatus GetEquipmentStatusMapper(Equipmentstatuses EquipmentStatus)
        {
            if (EquipmentStatus == null)
                return null;
            return new Models.Lookup.EquipmentStatus()
            {
                EquipmentStatusId = EquipmentStatus.Equipmentstatusid,
                EquipmentStatusDescription = EquipmentStatus.Equipmentstatus,
                CreationDate = EquipmentStatus.Creationdate,
                CreationUser = EquipmentStatus.Creationuser,
                ModificationDate = EquipmentStatus.Modificationdate,
                ModificationUser = EquipmentStatus.Modificationuser,
                Deleted = EquipmentStatus.Deleted.Value,
                DeletionDate = EquipmentStatus.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(EquipmentStatus.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(EquipmentStatus.ModificationuserNavigation),
                
            };
        }
        public static Equipmentstatuses SetEquipmentStatusMapper(Models.Lookup.EquipmentStatus EquipmentStatus)
        {
            return new Equipmentstatuses()
            {
                Equipmentstatusid = EquipmentStatus.EquipmentStatusId,
                Equipmentstatus = EquipmentStatus.EquipmentStatusDescription,
                Creationdate = EquipmentStatus.CreationDate,
                Creationuser = EquipmentStatus.CreationUser,
                Modificationdate = EquipmentStatus.ModificationDate,
                Modificationuser = EquipmentStatus.ModificationUser,
                Deleted = EquipmentStatus.Deleted,
                Deletiondate = EquipmentStatus.DeletionDate,
            };
        }
    }
}
