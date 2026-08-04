using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Vbom
{
    public class VnfHardwareMapper
    {
        public static VnfHardWare GetVnfHardWare(Vnfhardware model)
        {
            if (model == null)
                return null;
            var result = new VnfHardWare()
            {
                VnfHardWareId = model.Vnfhardwareid,
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

        public static Vnfhardware SetVnfHardWare(VnfHardWare model)
        {
            if (model == null)
                return null;
            var result = new Vnfhardware()
            {
                Vnfhardwareid = model.VnfHardWareId,
                Description = model.Description,
            };
            return result;
        }
    }
}
