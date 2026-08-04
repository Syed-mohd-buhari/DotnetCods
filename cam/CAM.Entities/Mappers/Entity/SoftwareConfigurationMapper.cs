using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public static class SoftwareConfigurationMapper
    {
        public static SoftwareConfiguration GetSoftwareConfiguration(Softwareconfiguration model)
        {
            if (model == null)
                return null;
            var result = new SoftwareConfiguration()
            {
                Softwareconfigurationid = model.Softwareconfigurationid,
                Networkelementid = model.Networkelementid,
                Opco = model.Opco,
                Oem = model.Oem,
                Elementname = model.Elementname,
                creationDate = model.Creationdate,
                creationUser = model.Creationuser,
                modificationDate = model.Modificationdate,
                modificationUser = model.Modificationuser,
                NetworkElement = NetworkElementMapper.Get(model.Networkelement),
                

            };
            //if (model.Function != null)
            //{
            //    foreach (var item in model.Function)
            //    {
            //        result.Function.Add(FunctionMapper.Get(item));
            //    }
            //}
            return result;
        }


        public static Softwareconfiguration Set(SoftwareConfiguration model)
        {
            var result= new Softwareconfiguration()
            {

                Softwareconfigurationid = model.Softwareconfigurationid,
                Networkelementid = model.Networkelementid,
                Opco = model.Opco,
                Elementname = model.Elementname,
                Creationdate = model.creationDate,
                Creationuser = model.creationUser,
                Modificationdate = model.modificationDate,
                Modificationuser = model.modificationUser,
                Networkelement = NetworkElementMapper.Set(model.NetworkElement),

            };
            if (model.Function != null)
            {
                foreach (var item in model.Function)
                {
                    result.Function.Add(FunctionMapper.Set(item));
                }
            }
            return result;
        }
    }
}
