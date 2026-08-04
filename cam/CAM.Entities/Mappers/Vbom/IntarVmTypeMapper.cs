using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.VBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Vbom
{
    public partial class IntarVmTypeMapper
    {
        public static IntraVmType GetIntarVmTypee(Intravmtype model)
        {
            if (model == null)
                return null;
            var result = new IntraVmType()
            {
                IntraDescription = model.Intradescription,
                IntraVmTypeId = model.Intravmtypeid,

                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };

            return result;
        }

        public static Intravmtype SetInterVmType(IntraVmType model)
        {
            if (model == null)
                return null;
            var result = new Intravmtype()
            {
                Intravmtypeid = model.IntraVmTypeId,
                Intradescription = model.IntraDescription,
            };
            return result;
        }
    }
}
