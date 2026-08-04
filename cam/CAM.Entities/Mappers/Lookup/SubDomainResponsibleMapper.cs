using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class SubDomainResponsibleMapper
    {
        public static Models.Lookup.SubDomainResponsible GetSubDomainResponsibleMapper(Subdomainresponsibles SubDomainResponsible)
        {

            if (SubDomainResponsible == null)
                return null;
            return new Models.Lookup.SubDomainResponsible()
            {
                SubDomainResponsibleId= SubDomainResponsible.Subdomainresponsibleid,
                SubDomainResponsibleDescription = SubDomainResponsible.Subdomainresponsible,
                CreationDate = SubDomainResponsible.Creationdate,
                CreationUser = SubDomainResponsible.Creationuser,
                ModificationDate = SubDomainResponsible.Modificationdate,
                ModificationUser = SubDomainResponsible.Modificationuser,
                Deleted = SubDomainResponsible.Deleted.Value,
                DeletionDate = SubDomainResponsible.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(SubDomainResponsible.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(SubDomainResponsible.ModificationuserNavigation),
               
            };
        }
        public static Subdomainresponsibles SetSubDomainResponsibleMapper(Models.Lookup.SubDomainResponsible SubDomainResponsible)
        {
            if (SubDomainResponsible == null)
                return null;
            return new Subdomainresponsibles()
            {

                Subdomainresponsibleid = SubDomainResponsible.SubDomainResponsibleId,
                Subdomainresponsible = SubDomainResponsible.SubDomainResponsibleDescription,
                Creationdate = SubDomainResponsible.CreationDate,
                Creationuser = SubDomainResponsible.CreationUser,
                Modificationdate = SubDomainResponsible.ModificationDate,
                Modificationuser = SubDomainResponsible.ModificationUser,
                Deleted = SubDomainResponsible.Deleted,
                Deletiondate = SubDomainResponsible.DeletionDate,
            };
        }

        public static ApplicationSubDomainRes GetApplicationSubDomainResponsible(Subdomainresponsibles SubDomainResponsible)
        {

            if (SubDomainResponsible == null)
                return null;
            return new ApplicationSubDomainRes()
            {
                SubdomainResponsibleId = SubDomainResponsible.Subdomainresponsibleid,
                SubDomainRes = SubDomainResponsible.Subdomainresponsible,

            };
        }
    }
}
