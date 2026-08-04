using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public static class SWConfigMapper
    {
        public static SWConfigFunctionAreas Get(Swconfigfunctionareas model)
        {
            if (model == null)
                return null;
            var result = new SWConfigFunctionAreas()
            {
                SWConfigFunctionAreaId = model.Swconfigfunctionareaid,
                FunctionAreaDescription = model.Functionareadescription,
                FunctionAreaName = model.Functionareaname, 
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                FunctionId = model.Functionid,
                Function = FunctionMapper.Get(model.Function),

            };          
            return result;
        }


        public static Swconfigfunctionareas Set(SWConfigFunctionAreas model)
        {
            var result= new Swconfigfunctionareas()
            {

                Swconfigfunctionareaid = model.SWConfigFunctionAreaId,
                Functionareadescription = model.FunctionAreaDescription,
                Functionareaname = model.FunctionAreaName,
                Functionid = model.FunctionId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Function = FunctionMapper.Set(model.Function),

            };
            
            return result;
        }
    }
}
