using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class VerticalResponsibleMapper
    {
        public static Models.Lookup.VerticalResponsible GetVerticalResponsibleMapper(Verticalresponsibles VerticalResponsible)
        {
            if (VerticalResponsible == null)
                return null;
            return new Models.Lookup.VerticalResponsible()
            {
                VerticalResponsibleId= VerticalResponsible.Verticalresponsibleid,
                VerticalResponsibleDescription = VerticalResponsible.Verticalresponsible,
                CreationDate = VerticalResponsible.Creationdate,
                CreationUser = VerticalResponsible.Creationuser,
                ModificationDate = VerticalResponsible.Modificationdate,
                ModificationUser = VerticalResponsible.Modificationuser,
                Deleted = VerticalResponsible.Deleted.Value,
                DeletionDate = VerticalResponsible.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(VerticalResponsible.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(VerticalResponsible.ModificationuserNavigation),
               
            };
        }
        public static Verticalresponsibles SetVerticalResponsibleMapper(Models.Lookup.VerticalResponsible VerticalResponsible)
        {
            if (VerticalResponsible == null)
                return null;
            return new Verticalresponsibles()
            {

                Verticalresponsibleid = VerticalResponsible.VerticalResponsibleId,
                Verticalresponsible = VerticalResponsible.VerticalResponsibleDescription,
                Creationdate = VerticalResponsible.CreationDate,
                Creationuser = VerticalResponsible.CreationUser,
                Modificationdate = VerticalResponsible.ModificationDate,
                Modificationuser = VerticalResponsible.ModificationUser,
                Deleted = VerticalResponsible.Deleted,
                Deletiondate = VerticalResponsible.DeletionDate,
            };
        }


        public static ApplicationVerticalRes GetApplicationVerticalResponsible(Verticalresponsibles VerticalResponsible)
        {
            if (VerticalResponsible == null)
                return null;
            return new ApplicationVerticalRes()
            {
                VerticalResId = VerticalResponsible.Verticalresponsibleid,
                VerticalRes = VerticalResponsible.Verticalresponsible,
                
            };
        }
    }
}
