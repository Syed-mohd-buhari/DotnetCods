using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class SWDeliveryLifeCycleMapper
    {
        public static SWDeliveryLifeCycle Get(Swdeliverylifecycle model)
        {
            if (model == null)
                return null;
            return new SWDeliveryLifeCycle()
            {
                Id = model.Id,
                Description = model.Description,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),

            };
        }
        public static Swdeliverylifecycle Set(SWDeliveryLifeCycle model)
        {
            return new Swdeliverylifecycle()
            {
                Id = model.Id,
                Description = model.Description,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
            };
        }
    }
}
