using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.VBom;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Mappers.Vbom
{
    public class VnfNameMapper
    {
        public static VnfName GetVnfName(Vnfname model, bool Include = true)
        {
            if (model == null)
                return null;
            var result = new VnfName()
            {
                VnfDescription = model.Vnfdescription,
                VnfNameId = model.Vnfnameid,
                ProductId = model.Productid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                Product = ProductNameMapper.GetProductNameMapper(model.Product),
            };

            if (model.Vmtypename != null && model.Vmtypename.Count > 0)
            {
                foreach (var item in model.Vmtypename)
                {
                    result.VmTypeName.Add(VmTypeNameMapper.GetVmTypeName(item, Include));
                }
            }

            //if (model.Vnfinfo != null && model.Vnfinfo.Count > 0)
            //{
            //    foreach (var item in model.Vnfinfo)
            //    {
            //        result.VnfInfo.Add(VnfInfoMapper.GetVnfInfo(item));
            //    }
            //}

            return result;
        }

        public static Vnfname SetVnfName(VnfName model)
        {
            if (model == null)
                return null;
            var result = new Vnfname()
            {
                Vnfdescription = model.VnfDescription,
                Vnfnameid = model.VnfNameId,
                Productid = model.ProductId,
            };
            return result;
        }
    }
}
