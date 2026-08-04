using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;


namespace CAM.Entities.Mappers.Lookup
{
    public class ResourceTypeMapper
    {
        public static ResourceTypes GetResourcetypes(Resourcetypes model)
        {
            if (model == null)
                return null;
            return new ResourceTypes()
            {
                ResourceTypesId=model.Resourcetypesid,
                Name=model.Name,
                Description=model.Description,
                CreationDate=model.Creationdate,
                CreationUser=model.Creationuser,
                ModificationDate=model.Modificationdate,
                ModificationUser=model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),

            };
        }
    }
}
