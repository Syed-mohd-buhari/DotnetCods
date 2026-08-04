using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;

namespace CAM.Entities.Mappers.Entity
{
    public class IdentityMapper
    {
        public static NodeIdentity Get(Identities model)
        {
            if (model == null)
                return null;
            return new NodeIdentity()
            {
                Identityid= model.Identitiesid,
                NetworkelementId = model.Networkelementid,
                oem = model.Oem,             
                Opco = model.Opco,
                Elementname = model.Elementname,
                Apnodeaipaddress = model.Apnodeaipaddress,
                Apnodebipaddress = model.Apnodebipaddress,
                Ipaddress = model.Ipaddress,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation)

            };
        }

        public static Identities Set(NodeIdentity model)
        {
            return new Identities()
            {

                Identitiesid = model.Identityid,
                Networkelementid = model.NetworkelementId,
                Oem =model.oem,
                Opco = model.Opco,
                Elementname = model.Elementname,
                Apnodeaipaddress = model.Apnodeaipaddress,
                Apnodebipaddress = model.Apnodebipaddress,
                Ipaddress = model.Ipaddress,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                
            };
        }
    }
}
