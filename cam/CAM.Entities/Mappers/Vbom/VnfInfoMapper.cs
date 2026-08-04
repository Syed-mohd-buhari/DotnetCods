using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.VBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Vbom
{
    public class VnfInfoMapper
    {
        public static VnfInfo GetVnfInfo(Vnfinfo model)
        {
            if (model == null)
                return null;
            var result = new VnfInfo()
            {
                VnfInfoId = model.Vnfinfoid,
                VnfNameId = model.Vnfnameid,
                VnfClusterInfoId = model.Vnfclusterinfoid,
                VnfVmTypeNameId = model.Vnfvmtypenameid,
                Nsxt = model.Nsxt,
                InterVmTypeId = model.Intervmtypeid,
                IntraVmTypeId = model.Intravmtypeid,

                VmWorkLoadTypeId = model.Vmworkloadtypeid,
                VmStorageBlockSize = model.Vmstorageblocksize,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,

                VnfNameDesc = model.Vnfname.Vnfdescription,
                VnfVmTypeNameDesc = model.Vnfvmtypename.Vmtypedescription,
                InterVmTypeDesc = model.Intervmtype.Interdescription,
                IntraVmTypeDesc = model.Intravmtype.Intradescription,
                VmWorkLoadTypeDesc = model.Vmworkloadtype.Description,
                Numa = model.Numa,
                Socket = model.Socket,

                LocationId = model.Vnfclusterinfo.Locationid,
                SiteName = model.Vnfclusterinfo.Location.Shortdescription,
                LocationName = model.Vnfclusterinfo.Location.Location,


            };
            if (model.CreationuserNavigation != null)
                result.CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation);

            if (model.ModificationuserNavigation != null)
                result.ModificationuserNavigation = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation);
           
            if (model.Vnfvmcapacity != null && model.Vnfvmcapacity.Count > 0)
            {
                foreach (var item in model.Vnfvmcapacity)
                {
                 result.VnfVmCapacity.Add(VnfVmCapacityMapper.GetVnfVmCapacity(item, false));
                }
            }
            return result;
        }

        public static Vnfinfo SetVnfInfo(VnfInfo model)
        {
            if (model == null)
                return null;
            var result = new Vnfinfo()
            {
                Vnfinfoid = model.VnfInfoId,
                Vnfnameid = model.VnfNameId,
                Vnfclusterinfoid = model.VnfClusterInfoId,
                Vnfvmtypenameid = model.VnfVmTypeNameId,
                Nsxt = model.Nsxt,
                Intervmtypeid = model.InterVmTypeId,
                Intravmtypeid = model.IntraVmTypeId,

                Vmworkloadtypeid = model.VmWorkLoadTypeId,
                Vmstorageblocksize = model.VmStorageBlockSize,

            };
            return result;
        }
    }
}
