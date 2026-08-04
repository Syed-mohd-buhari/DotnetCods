using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class ServiceMasterMapper
    {
        public static ServiceMaster Get (Servicemaster model, bool include = true)
        {
            if (model == null)
                return null;
            var result = new ServiceMaster()
            {
                Servicemasterid = model.Servicemasterid,
                Description =model.Description,
                CreationDate  =model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate =model.Modificationdate,
                ModificationUser =model.Modificationuser,
                Deleted =model.Deleted.Value,
                DeletionDate =model.Deletiondate,
                CreationUserEntity = include == true ? ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation): null,
                ModificationUserEntity = include == true ? ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation) : null,
            };
            
            return result;
        }
        public static Servicemaster Set(ServiceMaster model)
        {
            return new Servicemaster()
            {
                Servicemasterid = model.Servicemasterid,
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
