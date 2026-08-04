using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Lookup
{
    public static class AspNetRoleMapper
    {
        public static AspNetRoles GetAspNetRoleMapper(Aspnetroles model)
        {
            if (model == null)
                return null;
            var result = new AspNetRoles()
            {
                Id = model.Id,
                Name = model.Name,
                Normalizedname = model.Normalizedname,
                Concurrencystamp = model.Concurrencystamp,
                Abstractiontaborder = model.Abstractiontaborder,
                Description = model.Description,
            };
            //if(model.Aspnetuserroles != null)
            //{
            //    foreach(var item in model.Aspnetuserroles)
            //    {
            //        result.Aspnetuserroles.Add(AspnetuserroleMapper.Get(item));
            //    }
            //}
            return result;
        }
        public static Aspnetroles SetAspNetRoleMapper(AspNetRoles model)
        {
            if (model == null)
                return null;
            var result = new Aspnetroles()
            {
                Id = model.Id,
                Name = model.Name,
                Normalizedname = model.Normalizedname,
                Concurrencystamp = model.Concurrencystamp,
                Abstractiontaborder = model.Abstractiontaborder,
                Description = model.Description,
            };
            //if (model.Aspnetuserroles != null)
            //{
            //    foreach (var item in model.Aspnetuserroles)
            //    {
            //        result.Aspnetuserroles.Add(AspnetuserroleMapper.Set(item));
            //    }
            //}
            return result;
        }

        public static ApplicationRole GetApplicationRole(Aspnetroles model)
        {
            if (model == null)
                return null;
            var result = new ApplicationRole()
            {
                RoleId = model.Id,
                RoleName = model.Name,
                Description = model.Description,
                
            };
            return result;
        }
    }
}
