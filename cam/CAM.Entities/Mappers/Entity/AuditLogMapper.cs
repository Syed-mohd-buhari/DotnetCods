using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class AuditLogMapper
    {
        public static AuditLogs Get(Auditlogs model)
        {
            if (model == null)
                return null;
            var result = new AuditLogs()
            {
                AuditLogId = model.Auditlogid,
                EntityField =model.Entityfield,
                EntityId = model.Entityid,
                EntityName =model.Entityname,
                EntityState = model.Entitystate,
                OldValue = model.Oldvalue,
                NewValue = model.Newvalue,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                              
            };
            return result;
        }
    }
}
