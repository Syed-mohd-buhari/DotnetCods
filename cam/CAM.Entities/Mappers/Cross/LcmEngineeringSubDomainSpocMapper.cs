using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.Cross
{
    public static class LcmEngineeringSubDomainSpocMapper
    {
        public static LcmEngineeringSubDomainSpoc Get(Lcmengineeringsubdomainspoc model)
        {
            if (model == null)
                return null;
            return new LcmEngineeringSubDomainSpoc()
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

                LcmEngineeringSubDomainSpocId = model.Lcmengineeringsubdomainspocid,
                Subdomainspocid = model.Subdomainspocid,
                Subdomainspoc = ApplicationUserMapper.GetApplicationUserMapper(model.Subdomainspoc),
                
                
            };
        }
        public static Lcmengineeringsubdomainspoc Set(LcmEngineeringSubDomainSpoc model)
        {
            return new Lcmengineeringsubdomainspoc()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Lcmengineeringid = model.LcmengineeringId,
                Lcmengineeringsubdomainspocid = model.LcmEngineeringSubDomainSpocId,
                Subdomainspocid = model.Subdomainspocid,
            };
        }
    }
}
