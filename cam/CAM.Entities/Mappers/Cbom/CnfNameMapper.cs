using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.CBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public partial class  CnfNameMapper
    {
        public static CnfName GetCnfInfo(Cnfname model)
        {
            if (model == null)
                return null;
            var result = new CnfName()
            {
                CnfNameId = model.Cnfnameid,
                CnfDescription = model.Cnfdescription,
                ProductId = model.Productid,
               
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Product = ProductNameMapper.GetProductNameMapper(model.Product,false)
            };

            return result;
        }

        public static Cnfname SetCnfInfo(CnfName model)
        {
            if (model == null)
                return null;
            var result = new Cnfname()
            {
                Cnfnameid = model.CnfNameId,
                Cnfdescription = model.CnfDescription,
                Productid = model.ProductId,
            };
            return result;
        }
    }
}
