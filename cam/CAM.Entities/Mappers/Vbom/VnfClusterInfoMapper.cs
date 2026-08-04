using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.VBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Vbom
{
    public class VnfClusterInfoMapper
    {
        public static VnfClusterInfo GetVnfClusterInfo(Vnfclusterinfo model)
        {
            if (model == null)
                return null;
            var result = new VnfClusterInfo()
            {
                VnfClusterInfoId = model.Vnfclusterinfoid,
                OpcoId = model.Opcoid,
                LocationId = model.Locationid,
                ClusterNameId = model.Clusternameid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                OpcoIdDescription = model.Opco.Opco,
                LocationShortDescription = model.Location.Shortdescription,
                LocationName = model.Location.Location,
                ClusterName = model.Clustername.Clusterdescription,
                NoOfBlades = model.Noofblades,
                HardwareTypeId = model.Hardwaretypeid,
                HardwareType=model.Hardwaretype.Description,
                FileName = model.Filename,
                Revision = model.Revision,

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

        public static Vnfclusterinfo SetVnfClusterInfo(VnfClusterInfo model)
        {
            if (model == null)
                return null;
            var result = new Vnfclusterinfo()
            {
                Vnfclusterinfoid = model.VnfClusterInfoId,
                Opcoid = model.OpcoId,
                Locationid = model.LocationId,
                Clusternameid = model.ClusterNameId,
                Noofblades = model.NoOfBlades,
                Hardwaretypeid = model.HardwareTypeId,
                Filename = model.FileName,
                Revision = model.Revision,
            };
            return result;
        }
    }
}
