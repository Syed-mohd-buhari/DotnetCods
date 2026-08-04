using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.Cross
{
    public static class NetworkElementAsPlannedSubDomainSpocMapper
    {
        public static NetworkElementAsPlannedSubDomainSpoc Get(Networkelementasplannedsubdomainspoc model)
        {
            if (model == null)
                return null;
            return new NetworkElementAsPlannedSubDomainSpoc()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                NetworkElementAsPlannedId = model.Networkelementasplannedid,
                NetworkElementAsPlannedSubDomainSpocId = model.Ntkelementasplnsubdomainspocid,              
                NetworkElementAsPlanned = NetworkElementAsPlannedMapper.Get(model.Networkelementasplanned),               
                Subdomainspoc = ApplicationUserMapper.GetApplicationUserMapper(model.Subdomainspoc),
                Subdomainspocid = model.Subdomainspocid != null ? (int)model.Subdomainspocid : null,

            };
        }

        public static NetworkElementAsPlannedSubDomainSpoc GetSpocId(Networkelementasplannedsubdomainspoc model)
        {
            if (model == null)
                return null;
            return new NetworkElementAsPlannedSubDomainSpoc()
            {                
                NetworkElementAsPlannedId = model.Networkelementasplannedid,
                Subdomainspocid = model.Subdomainspocid != null ? (int)model.Subdomainspocid : null,

            };
        }
        public static Networkelementasplannedsubdomainspoc Set(NetworkElementAsPlannedSubDomainSpoc model)
        {
            return new Networkelementasplannedsubdomainspoc()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                //Subdomainspocid = model.SubDomainSpocId,
                Ntkelementasplnsubdomainspocid = model.NetworkElementAsPlannedSubDomainSpocId,
                Networkelementasplannedid = model.NetworkElementAsPlannedId,
                Subdomainspocid = model.Subdomainspocid
            };
        }
    }
}
