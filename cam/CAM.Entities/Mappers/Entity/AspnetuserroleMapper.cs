using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public class AspnetuserroleMapper
    {
        public static AspNetUserRoles Get(Aspnetuserroles model, bool include = false)
        {
            if (model == null)
                return null;
            var result = new AspNetUserRoles()
            {
                AspNetUserRoleId = model.Aspnetuserroleid,
                Userid = model.Userid,
                Roleid = model.Roleid,              
                Role = AspNetRoleMapper.GetAspNetRoleMapper(model.Role),
                User = ApplicationUserMapper.GetApplicationUserMapper(model.User, include),
               


            };
            return result;
        }

        public static Aspnetuserroles Set(AspNetUserRoles model)
        {
            return new Aspnetuserroles()
            {
                Aspnetuserroleid = model.AspNetUserRoleId,
                Userid = model.Userid,
                Roleid = model.Roleid,
                Role = AspNetRoleMapper.SetAspNetRoleMapper(model.Role),            

            };
        }


        public static ApplicationUserRole GetUserApplication(Aspnetuserroles model, bool include = false)
        {
            if (model == null)
                return null;
            var result = new ApplicationUserRole()
            {
                UserId = model.Userid,
                RoleId = model.Roleid,
                User = include == true ? ApplicationUserMapper.GetApplicationUserMapper(model.User) : null,
                Role = AspNetRoleMapper.GetApplicationRole(model.Role),


            };
            return result;
        }
    }
}
