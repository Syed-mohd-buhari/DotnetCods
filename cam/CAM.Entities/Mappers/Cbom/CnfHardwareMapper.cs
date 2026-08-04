using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.CBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public class CnfHardwareMapper
    {
        public static CnfHardware GetCnfHardware(Cnfhardware model)
        {
            if (model == null)
                return null;
            var result = new CnfHardware()
            {
                CnfHardwareId = model.Cnfhardwareid,
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

        public static Cnfhardware SetCnfHardware(CnfHardware model)
        {
            if (model == null)
                return null;
            var result = new Cnfhardware()
            {
                Cnfhardwareid = model.CnfHardwareId,
                Description = model.Description,
                
            };
            return result;
        }

    }
}
