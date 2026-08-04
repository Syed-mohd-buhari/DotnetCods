using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Cross
{
    public static class DesignAspectNetworkFunctionMapper
    {
        public static DesignAspectNetworkFunction Get(Designaspectsnetworkfunctions model)
        {

            if (model == null)
                return null;
            return new DesignAspectNetworkFunction()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser.Value,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser.Value,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                DesignAspectId = model.Designaspectid,
                NetworkFunctionId = model.Networkfunctionid,
                Id = model.Id,
                //DesignAspect = DesignAspectMapper.Get(model.Designaspect),
                NetworkFunction = NetworkFunctionMapper.Get(model.Networkfunction)

            };
        }

        public static Designaspectsnetworkfunctions Set(DesignAspectNetworkFunction model)
        {
            return new Designaspectsnetworkfunctions()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Designaspectid = model.DesignAspectId,
                Id = model.Id,
                Networkfunctionid = model.NetworkFunctionId,
               // Designaspect = DesignAspectMapper.Set(model.DesignAspect),
                Networkfunction = NetworkFunctionMapper.Set(model.NetworkFunction)



            };
        }
    }
}
