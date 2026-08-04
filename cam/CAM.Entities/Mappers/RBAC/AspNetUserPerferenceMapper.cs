using CAM.Entities.Models.RBAC;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.RBAC
{
    public static class AspNetUserPerferenceMapper
    {
        public static AspNetUserPerference GetAspNetModulesr(Aspnetuserpreferences model)
        {
            if (model == null)
                return null;
            var result = new AspNetUserPerference()
            {
                Aspnetuserpreferenceid = model.Aspnetuserpreferenceid,
                Userid = model.Userid,
                Moduleid = model.Aspnetmoduleid,
                Permission = model.Permission,
                Order = model.Order,
                Isuserpreference = model.Isuserpreference,
                Aspnetmodule = AspNetModulesMapper.GetAspNetModulesr(model.Aspnetmodule)
                //CreationDate = model.Creationdate,
                //CreationUser = model.Creationuser,
                //ModificationDate = model.Modificationdate,
                //ModificationUser = model.Modificationuser,
                //Deleted = model.Deleted.Value,
                //DeletionDate = model.Deletiondate,
                //CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                //ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),


            };

            return result;

        }
        public static Aspnetuserpreferences SetAspNetModules(AspNetUserPerference model)
        {
            if (model == null)
                return null;
            var result = new Aspnetuserpreferences()
            {
                Aspnetuserpreferenceid = model.Aspnetuserpreferenceid,
                Userid = model.Userid,
                Aspnetmoduleid = model.Moduleid,
                Permission = model.Permission,
                Order = model.Order,
                Isuserpreference = model.Isuserpreference,
                //Creationdate = model.CreationDate,
                //Creationuser = model.CreationUser,
                //Modificationdate = model.ModificationDate,
                //Modificationuser = model.ModificationUser,
                //Deleted = model.Deleted,
                //Deletiondate = model.DeletionDate,
            };

            return result;
        }
    }
}
