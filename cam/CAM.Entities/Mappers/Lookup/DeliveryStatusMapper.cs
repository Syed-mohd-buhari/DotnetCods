using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class DeliveryStatusMapper
    {
        public static DeliveryStatus GetDeliveryStatusMapper(Deliverystatuses DeliveryStatus)
        {
            if (DeliveryStatus == null)
                return null;
            return new DeliveryStatus()
            {
                DeliveryStatusId = DeliveryStatus.Deliverystatusid,
                DeliveryStatusDescription = DeliveryStatus.Deliverystatus,
                CreationDate = DeliveryStatus.Creationdate,
                CreationUser = DeliveryStatus.Creationuser,
                ModificationDate = DeliveryStatus.Modificationdate,
                ModificationUser = DeliveryStatus.Modificationuser,
                Deleted = DeliveryStatus.Deleted.Value,
                DeletionDate = DeliveryStatus.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(DeliveryStatus.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(DeliveryStatus.ModificationuserNavigation),
                ProjectStatusCombinationRule = DeliveryStatus.Projectstatuscombinationrule,
                Rule = DeliveryStatus.Rule,
                
            };
        }
        public static Deliverystatuses SetDeliveryStatusMapper(DeliveryStatus DeliveryStatus)
        {
            return new Deliverystatuses()
            {
                Deliverystatusid = DeliveryStatus.DeliveryStatusId,
                Deliverystatus = DeliveryStatus.DeliveryStatusDescription,
                Creationdate = DeliveryStatus.CreationDate,
                Creationuser = DeliveryStatus.CreationUser,
                Modificationdate = DeliveryStatus.ModificationDate,
                Modificationuser = DeliveryStatus.ModificationUser,
                Deleted = DeliveryStatus.Deleted,
                Deletiondate = DeliveryStatus.DeletionDate,
                Projectstatuscombinationrule = DeliveryStatus.ProjectStatusCombinationRule,
                Rule = DeliveryStatus.Rule,
            };
        }
    }
}

