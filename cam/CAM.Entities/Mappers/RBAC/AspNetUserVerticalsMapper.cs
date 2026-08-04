using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.RBAC;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.RBAC
{
    public static class AspNetUserVerticalsMapper
    {
        public static AspNetUserVerticals Get(Aspnetuserverticals model, bool include = false)
        {
            if (model == null)
                return null;
            var result = new AspNetUserVerticals()
            {
               
                Aspnetuserverticalid = model.Aspnetuserverticalid,
                Userid = model.Userid,
                Organisationid = model.Organisationid,
                User = ApplicationUserMapper.GetApplicationUserMapper(model.User, include),
                Organisation  = OrganisationMapper.GetOrganisationMapper(model.Organisation),
                Deleted = model.Deleted.Value,
                Isvertical = model.Isvertical,
                Isverticalresponcible = model.Isverticalresponcible,
            };
            
            return result;

        }
        public static Aspnetuserverticals Set(AspNetUserVerticals model)
        {
            if (model == null)
                return null;
            var result = new Aspnetuserverticals()
            {               
                Aspnetuserverticalid = model.Aspnetuserverticalid,
                Userid = model.Userid,
                Organisationid = model.Organisationid,
                Deleted = model.Deleted,
                Isvertical = model.Isvertical,
                Isverticalresponcible = model.Isverticalresponcible,
            };
          
            return result;
        }

        public static ApplicationOrgandVertical GetOrgVerticalApplication(Aspnetuserverticals model, bool include = false)
        {
            if (model == null)
                return null;
            var result = new ApplicationOrgandVertical()
            {

                Aspnetuserverticalid = model.Aspnetuserverticalid,
                Userid = model.Userid,
                Organisationid = model.Organisationid,
                Isvertical = model.Isvertical,
                Isverticalresponcible = model.Isverticalresponcible,
                //ApplicationUser = ApplicationUserMapper.GetApplicationUserMapper(model.User, include),
                ApplicationOrganisation = OrganisationMapper.GetApplicationionOrganisation(model.Organisation)
            };

            return result;

        }
    }
}
