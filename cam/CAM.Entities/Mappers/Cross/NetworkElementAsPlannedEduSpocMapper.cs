using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.Cross
{
    public static class NetworkElementAsPlannedEduSpocMapper
    {
        public static NetworkElementAsPlannedEduSpoc Get(Networkelementasplannededuspoc model)
        {
            if (model == null)
                return null;
            return new NetworkElementAsPlannedEduSpoc()
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
                NetworkElementAsPlannedEduSpocId = model.Ntkelementasplneduspocid,                
                NetworkElementAsPlanned = NetworkElementAsPlannedMapper.Get(model.Networkelementasplanned),
                Eduspocid = model.Eduspocid != null ? (int)model.Eduspocid : null,
                Eduspoc = model.Eduspoc != null ? ApplicationUserMapper.GetApplicationUserMapper(model.Eduspoc) : null,



            };
        }
        public static Networkelementasplannededuspoc Set(NetworkElementAsPlannedEduSpoc model)
        {
            return new Networkelementasplannededuspoc()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Networkelementasplannedid = model.NetworkElementAsPlannedId,
             
                Ntkelementasplneduspocid  = model.NetworkElementAsPlannedEduSpocId,
                Eduspocid = model.Eduspocid
                
            };
        }
    }
}
