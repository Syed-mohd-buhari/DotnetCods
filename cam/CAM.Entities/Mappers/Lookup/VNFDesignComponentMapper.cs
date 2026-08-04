using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;


namespace CAM.Entities.Mappers.Lookup
{
    public static class VNFDesignComponentMapper
    {
        public static VNFDesignComponent GetVNFDesignComponentMapper(Vfndesigncomponents model)
        {
            if (model == null)
                return null;
            return new VNFDesignComponent()
            {
                VNFDesignComponentId = model.Vnfdesigncomponentid,
                VNFDesignComponentDescription = model.Designcomponent,
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
        public static Vfndesigncomponents SetVNFDesignComponentMapper(VNFDesignComponent model)
        {
            return new Vfndesigncomponents()
            {
                Vnfdesigncomponentid = model.VNFDesignComponentId,
                Designcomponent = model.VNFDesignComponentDescription,
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
