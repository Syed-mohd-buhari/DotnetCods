using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.ComponentSoftware;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public static class ComponentManufacturersMapper
    {
        public static ComponentManufacturers GetComponentManufacturersMapper(Componentmanufacturers model, bool include = false )
        {
            if (model == null)
                return null;
            var result = new ComponentManufacturers()
            {
                Componentmanufacturerid = model.Componentmanufacturerid,
                Componentmanufacturer = model.Componentmanufacturer,
                Componentname = model.Componentname,
                CreationDate = model.Creationdate,
                ModificationDate = model.Modificationdate,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                                          
            };
            

            return result;

        }
        public static Componentmanufacturers SetComponentManufacturersMapper(ComponentManufacturers model)
        {
            if (model == null)
                return null;
            var result = new Componentmanufacturers()
            {
                Componentmanufacturerid = model.Componentmanufacturerid,
                Componentmanufacturer = model.Componentmanufacturer,
                Componentname = model.Componentname,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,                                
            };
          
            return result;
        }
    }
}
