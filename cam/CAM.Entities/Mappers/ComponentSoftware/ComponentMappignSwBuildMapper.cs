using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Enum;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class ComponentMappignSwBuildMapper
    {
        public static ComponentSoftwareBuildBag GetComponentMappingSwBuilBagdMapper(Componentsoftwarebuildbags model )
        {
            if (model == null)
                return null;
            var result = new ComponentSoftwareBuildBag()
            {
                ComponentSoftwareBuildBagId = model.Componentsoftwarebuildbagid,
                ComponentSoftwareBuildId = model.Componentsoftwarebuildid,
                BuildBagId = model.Buildbagid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                ComponentSoftwareBuilds = ComponentSoftwareBuildMapper.GetComponentSoftwareBuildMapper(model.Componentsoftwarebuild),
                BuildBags = BuildBagMapper.GetBuildBag(model.Buildbag) 
                
            };
            
            return result;

        }
        public static Componentsoftwarebuildbags SetComponentMappingSwBuilBagdMapper(ComponentSoftwareBuildBag model)
        {
            if (model == null)
                return null;
            var result = new Componentsoftwarebuildbags()
            {
                Componentsoftwarebuildbagid = model.ComponentSoftwareBuildBagId,
                Componentsoftwarebuildid     = model.ComponentSoftwareBuildId,
                Buildbagid    = model.BuildBagId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Componentsoftwarebuild  = ComponentSoftwareBuildMapper.SetComponentSoftwareBuildMapper(model.ComponentSoftwareBuilds),
                Buildbag = BuildBagMapper.SetBuildBag(model.BuildBags)


            };
          
            return result;
        }
    }
}
