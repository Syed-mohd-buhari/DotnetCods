using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class SubDomainSpocMapper
    {
        public static Models.Lookup.SubDomainSpoc GetSubDomainSpocMapper(Subdomainspocs SubDomainSpoc)
        {

            if (SubDomainSpoc == null)
                return null;
            return new Models.Lookup.SubDomainSpoc()
            {
                SubDomainSpocId= SubDomainSpoc.Subdomainspocid,
                SubDomainSpocDescription = SubDomainSpoc.Subdomainspoc,
                CreationDate = SubDomainSpoc.Creationdate,
                CreationUser = SubDomainSpoc.Creationuser,
                ModificationDate = SubDomainSpoc.Modificationdate,
                ModificationUser = SubDomainSpoc.Modificationuser,
                Deleted = SubDomainSpoc.Deleted.Value,
                DeletionDate = SubDomainSpoc.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(SubDomainSpoc.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(SubDomainSpoc.ModificationuserNavigation),
                isSubDomain= SubDomainSpoc.Issubdomain,
                isEdu= SubDomainSpoc.Isedu
               
            };
        }
        public static Subdomainspocs SetSubDomainSpocMapper(Models.Lookup.SubDomainSpoc SubDomainSpoc)
        {
            return new Subdomainspocs()
            {

                Subdomainspocid = SubDomainSpoc.SubDomainSpocId,
                Subdomainspoc = SubDomainSpoc.SubDomainSpocDescription,
                Creationdate = SubDomainSpoc.CreationDate,
                Creationuser = SubDomainSpoc.CreationUser,
                Modificationdate = SubDomainSpoc.ModificationDate,
                Modificationuser = SubDomainSpoc.ModificationUser,
                Deleted = SubDomainSpoc.Deleted,
                Deletiondate = SubDomainSpoc.DeletionDate,
                Isedu = SubDomainSpoc.isEdu,
                Issubdomain = SubDomainSpoc.isSubDomain,
            };
        }
    }
}
