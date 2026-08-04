using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cross
{
    public static class LcmEngineeringEduSpocMapper
    {
        public static LcmEngineeringEduSpoc Get(Lcmengineeringeduspoc model)
        {
            if (model == null)
                return null;
            return new LcmEngineeringEduSpoc()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                LcmengineeringId = model.Lcmengineeringid,
                LcmEngineeringEduSpocId = model.Lcmengineeringeduspocid,              
                Eduspocid = model.Eduspocid,
                Eduspoc = ApplicationUserMapper.GetApplicationUserMapper(model.Eduspoc),

                
            };
        }
        public static Lcmengineeringeduspoc Set(LcmEngineeringEduSpoc model)
        {
            return new Lcmengineeringeduspoc()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,                
                Lcmengineeringid = model.LcmengineeringId,
                Lcmengineeringeduspocid = model.LcmEngineeringEduSpocId,
                Eduspocid = model.Eduspocid,

            };
        }
    }
}
