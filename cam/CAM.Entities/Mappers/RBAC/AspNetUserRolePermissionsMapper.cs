using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.RBAC;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.RBAC
{
    public static class AspNetUserRolePermissionsMapper
    {
        
        public static AspNetUserRolePermissions GetAspNetUserRolePermissions(Aspnetuserrolepermissions model)
        {

            if (model == null)
                return null;
            var result = new AspNetUserRolePermissions()
            {
                AspNetUserRolePermissionId = model.Aspnetuserrolepermissionid,
                RoleId = model.Roleid,
                ModuleId = model?.Moduleid,
                PermissionLevel = model.Permissionlevel,              
              
                CreationUser = model.Creationuser,
                CreationDate = model.Creationdate,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationuserNavigation = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationuserNavigation = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Role= AspNetRoleMapper.GetAspNetRoleMapper(model.Role),
                Module = AspNetModulesMapper.GetAspNetModulesr(model.Module),

            };
             
            return result;
        }

      

        public static Aspnetuserrolepermissions SeAspNetUserRolePermissions(AspNetUserRolePermissions model)
        {
            return new Aspnetuserrolepermissions()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Aspnetuserrolepermissionid = model.AspNetUserRolePermissionId,
                Roleid = model.RoleId,
                Moduleid = model.ModuleId,
                Permissionlevel = model.PermissionLevel,
                
            };
        }
    }
}
