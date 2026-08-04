using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;


namespace CAM.Entities.Mappers.Lookup
{
    public static class NFVIStatusMapper
    {
        public static NFVIStatus GetNFVIStatusMapper(Nfvistatuses model)
        {
            if (model == null)
                return null;
            return new NFVIStatus()
            {
                NFVIStatusId = model.Nfvistatusid,
                NFVIStatusDescription = model.Nfvistatus,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Color = model.Color,
               

            };
        }
        public static Nfvistatuses SetNFVIStatusMapper(NFVIStatus model)
        {
            return new Nfvistatuses()
            {
                Nfvistatusid = model.NFVIStatusId,
                Nfvistatus = model.NFVIStatusDescription,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Color = model.Color,
            };
        }
    }
}
