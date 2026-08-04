using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.VBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Vbom
{
    public partial class InterVmTypeMapper
    {
        public static InterVmType GetInterVmType(Intervmtype model)
        {
            if (model == null)
                return null;
            var result = new InterVmType()
            {
                InterVmTypeId = model.Intervmtypeid,
                InterDescription = model.Interdescription,
               
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };
            
            return result;
        }

        public static Intervmtype SetInterVmType(InterVmType model)
        {
            if (model == null)
                return null;
            var result = new Intervmtype()
            {
                Intervmtypeid = model.InterVmTypeId,
                Interdescription = model.InterDescription,
            };
            return result;
        }
    }
}
