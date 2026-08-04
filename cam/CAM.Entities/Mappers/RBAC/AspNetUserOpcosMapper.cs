using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.RBAC;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.RBAC
{
    public static class AspNetUserOpcosMapper
    {
        public static AspNetUserOpcos GetAspNetUserOpco(Aspnetuseropcos model, bool include = false)
        {
            if (model == null)
                return null;
            var result = new AspNetUserOpcos()
            {       
                Aspnetuseropcoid = model.Aspnetuseropcoid,
                Userid = model.Userid,
                Opcoid = model.Opcoid,
                Isrestrictedopco = model.Isrestrictedopco,
                Isinusedopcos = model.Isinusedopcos,
                Opco = OpCoMapper.GetOpCoMapper(model.Opco),
                User = ApplicationUserMapper.GetApplicationUserMapper(model.User,include),
                Deleted = model.Deleted.Value,
            };
            
            return result;

        }
        public static Aspnetuseropcos SetAspNetUserOpco(AspNetUserOpcos model)
        {
            if (model == null)
                return null;
            var result = new Aspnetuseropcos()
            {
                Aspnetuseropcoid = model.Aspnetuseropcoid,
                Userid = model.Userid,
                Opcoid = model.Opcoid,
                Deleted = model.Deleted,
                Isrestrictedopco= model.Isrestrictedopco,
            };
          
            return result;
        }

        public static ApplicationUserOpco GetAspNetUserOpcoApplication(Aspnetuseropcos model, bool include = false)
        {
            if (model == null)
                return null;
            var result = new ApplicationUserOpco()
            {
                Aspnetuseropcoid = model.Aspnetuseropcoid,
                Userid = model.Userid,
                Opcoid = model.Opcoid,
                Isrestrictedopco = model.Isrestrictedopco,
                Isinusedopcos = model.Isinusedopcos,
                ApplicationOpco = OpCoMapper.GetApplicationOpco(model.Opco),
                //User = ApplicationUserMapper.GetApplicationUserMapper(model.User, include),
            };

            return result;

        }
    }
}
