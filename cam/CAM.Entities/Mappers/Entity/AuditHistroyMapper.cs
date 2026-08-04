using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class AuditHistroyMapper
    {
        public static AuditHistory Get(Audithistory model)
        {
            if (model == null)
                return null;
            var result = new AuditHistory()
            {
                Audithistoryid = model.Audithistoryid,
                Opco =model.Opco,
                Oem =model.Oem,
                Elementname = model.Elementname,
                Primarykey = model.Primarykey,
                Tablename = model.Tablename,
                Columnname = model.Columnname,
                Oldvalue = model.Oldvalue,
                Newvalue = model.Newvalue,
                Status = model.Status,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                              
            };
            return result;
        }

        public static Audithistory Set(AuditHistory model)
        {
            return new Audithistory()
            {

                Audithistoryid = model.Audithistoryid,
                Opco = model.Opco,
                Oem = model.Oem,
                Elementname = model.Elementname,
                Primarykey = model.Primarykey,
                Tablename = model.Tablename,
                Columnname = model.Columnname,
                Oldvalue = model.Oldvalue,
                Newvalue = model.Newvalue,
                Status = model.Status,
                Creationdate = model.Creationdate,
                Creationuser = model.Creationuser,
                Modificationdate = model.Modificationdate,
                Modificationuser = model.Modificationuser

            };
        }
    }
}
