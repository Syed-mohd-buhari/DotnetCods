using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public class FunctionMapper
    {
        public static Functions Get(Function model)
        {
            if (model == null)
                return null;
            var result = new Functions()
            {
                Functionid= model.Functionid,
                Functionname= model.Functionname,
                Creationdate = model.Creationdate,
                Creationuser = model.Creationuser,
                Modificationdate = model.Modificationdate,
                Modificationuser = model.Modificationuser,
                Softwareconfigurationid =model.Softwareconfigurationid,
                Softwareconfiguration=SoftwareConfigurationMapper.GetSoftwareConfiguration(model.Softwareconfiguration),
            };
            //if (model.Swconfigfunctionareas != null)
            //{
            //    foreach (var item in model.Swconfigfunctionareas)
            //    {
            //        result.SWConfigFunctionAreas.Add(SWConfigMapper.Get(item));
            //    }
            //}
            return result;
        }

        public static Function Set(Functions model)
        {
            if (model == null)
                return null;
            var result = new Function()
            {
                Functionid = model.Functionid,
                Functionname = model.Functionname,
                Creationdate = model.Creationdate,
                Creationuser = model.Creationuser,
                Modificationdate = model.Modificationdate,
                Modificationuser = model.Modificationuser,
                Softwareconfigurationid =model.Softwareconfigurationid,
                Softwareconfiguration = SoftwareConfigurationMapper.Set(model.Softwareconfiguration),
            };
            if (model.SWConfigFunctionAreas != null)
            {
                foreach (var item in model.SWConfigFunctionAreas)
                {
                    result.Swconfigfunctionareas.Add(SWConfigMapper.Set(item));
                }
            }
            return result;
        }
    }
}
