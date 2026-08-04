using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.CBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public partial class PodTypeInfoMapper
    {
        public static PodTypeInfo GetPodTypeInfo(Podtypeinfo model)
        {
            if (model == null)
                return null;
            var result = new PodTypeInfo()
            {
                PodTypeInfoId = model.Podtypeinfoid,
                PodTypeInfoName = model.Podtypeinfoname,              
                PodRoleDescription = model.Podroledescription,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };

            return result;
        }

        public static Podtypeinfo SetPodTypeInfo(PodTypeInfo model)
        {
            if (model == null)
                return null;
            var result = new Podtypeinfo()
            {
                Podtypeinfoid = model.PodTypeInfoId,
                Podtypeinfoname = model.PodTypeInfoName,
            };
            return result;
        }
    }
}
