using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.CBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public class CnfClusterMapper
    {
        public static CnfCluster GetCnfCluster(Cnfcluster model)
        {
            if (model == null)
                return null;
            var result = new CnfCluster()
            {
                CnfClusterId = model.Cnfclusterid,
                CnfClusterName = model.Cnfclustername,
                AlaisName = model.Alaisname,
                NodePool = model.Nodepool,
                CnfNameId = model.Cnfnameid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                CnfName = CnfNameMapper.GetCnfInfo(model.Cnfname)

            };

            return result;
        }

        public static Cnfcluster SetCnfCluster(CnfCluster model)
        {
            if (model == null)
                return null;
            var result = new Cnfcluster()
            {
                Cnfclusterid = model.CnfClusterId,
                Cnfclustername = model.CnfClusterName,
                Nodepool = model.NodePool,
                Cnfnameid = model.CnfNameId,
                Alaisname = model.AlaisName,
            };
            return result;
        }
    }
}
