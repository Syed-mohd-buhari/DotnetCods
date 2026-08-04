using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.CBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public class CnfPriorityMapper
    {
        public static CnfPriority GetCnfPriority(Cnfpriority model)
        {
            if (model == null)
                return null;
            var result = new CnfPriority()
            {
                CnfPriorityId = model.Cnfpriorityid,
                Description = model.Description,               
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            }; 
            return result;
        }

        public static Cnfpriority SetCnfPriority(CnfPriority model)
        {
            if (model == null)
                return null;
            var result = new Cnfpriority()
            {
                Cnfpriorityid = model.CnfPriorityId,
                Description = model.Description,
                
            };
            return result;
        }

    }
}
