using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.CBom;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public class FunctionStandardNameMapper
    {
        public static FunctionStandardName GetFunctionName(Functionstandardname model)
        {
            if (model == null)
                return null;
            var result = new FunctionStandardName()
            {
                FunctionStandardNameId = model.Functionstandardnameid,
                FunctionName = model.Functionname,               
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            }; 
            return result;
        }

        public static Functionstandardname SetFunctionName(FunctionStandardName model)
        {
            if (model == null)
                return null;
            var result = new Functionstandardname()
            {
                Functionstandardnameid = model.FunctionStandardNameId,
                Functionname = model.FunctionName,
                
            };
            return result;
        }

    }
}
