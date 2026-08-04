using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.VBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Vbom
{
    public class VmTypeNameMapper
    {
        public static VmTypeName GetVmTypeName(Vmtypename model, bool Include = true)
        {
            if (model == null)
                return null;
            var result = new VmTypeName()
            {
                VmtypeNameId = model.Vmtypenameid,
                VmTypeDescription = model.Vmtypedescription,
                VnfNameId = model.Vnfnameid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                VnfName = Include?VnfNameMapper.GetVnfName(model.Vnfname, false):null,
            };
            if (model.Vnfinfo != null && model.Vnfinfo.Count > 0)
            {
                foreach (var item in model.Vnfinfo)
                {
                    result.VnfInfo.Add(VnfInfoMapper.GetVnfInfo(item));
                }
            }
            return result;
        }

        public static Vmtypename SetVmTypeName(VmTypeName model)
        {
            if (model == null)
                return null;
            var result = new Vmtypename()
            {
                Vmtypenameid = model.VmtypeNameId,
                Vmtypedescription = model.VmTypeDescription,
                Vnfnameid = model.VnfNameId,
            };
            return result;
        }
    }
}
