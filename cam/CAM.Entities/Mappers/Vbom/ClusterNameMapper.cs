using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.VBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Vbom
{
    public class ClusterNameMapper
    {
        public static ClusterName GetClusterName(Clustername model)
        {
            if (model == null)
                return null;
            var result = new ClusterName()
            {
                ClusterNameId = model.Clusternameid,
                ClusterDescription = model.Clusterdescription,
                ClusterType = model.Clustertype,               
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,               
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };
            if (model.Vnfclusterinfo != null && model.Vnfclusterinfo.Count > 0)
            {
                foreach (var item in model.Vnfclusterinfo)
                {
                    result.VnfClusterInfo.Add(VnfClusterInfoMapper.GetVnfClusterInfo(item));
                }
            }
            return result;
        }

        public static Clustername SetClusterName(ClusterName model)
        {
            if (model == null)
                return null;
            var result = new Clustername()
            {
                Clusternameid = model.ClusterNameId,
                Clusterdescription = model.ClusterDescription,
                Clustertype = model.ClusterType,
                  };
            return result;
        }
    }
}
