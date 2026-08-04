using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Model.ClusterLevelPA;
using CAM.Entities.Models;
using CAM.Entities.Models.RBAC;
using CAM.Enum;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.RBAC
{
    public static class AspNetModulesMapper
    {
        public static AspNetModules GetAspNetModulesr(Aspnetmodules model )
        {
            if (model == null)
                return null;
            var result = new AspNetModules()
            {
                AspnetModuleId = model.Aspnetmoduleid,
                Module = model.Module,
                ModulePath = model.Modulepath,
                Category = model.Category,
                IsDefault = model.Isdefault,  
                Menu= model.Menu,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                
            };
            
            return result;

        }
        public static Aspnetmodules SetAspNetModules(AspNetModules model)
        {
            if (model == null)
                return null;
            var result = new Aspnetmodules()
            {
                Aspnetmoduleid = model.AspnetModuleId,
                Module = model.Module,
                Modulepath = model.ModulePath,
                Menu= model.Menu,
                Category = model.Category,
                Isdefault = model.IsDefault,
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
