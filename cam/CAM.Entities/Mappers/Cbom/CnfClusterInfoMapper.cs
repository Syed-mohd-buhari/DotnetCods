using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.CBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public partial class CnfClusterInfoMapper
    {
        public static CnfClusterInfo GetCnfClusterInfo(Cnfclusterinfo model)
        {
            if (model == null)
                return null;
            var result = new CnfClusterInfo()
            {
                CnfClusterInfoId = model.Cnfclusterinfoid,
                CnfNameId = model.Cnfnameid,
                CnfClusterId = model.Cnfclusterid,
                CnfHardwareId = model.Cnfhardwareid,

                OpcoId = model.Opcoid,
                SiteId = model.Siteid,
                
                NodePoolBreakup = model.Nodepoolbreakup,
                SpecialRequirements = model.Specialrequirements,
                Hyperthreading = model.Hyperthreading,
                OverProvisioning = model.Overprovisioning,
                WorkerNodeConfiguration = model.Workernodeconfiguration,
                HardwareDescription = model.Hardware,
                CpuKubelet = model.Cpukubelet,
                MemKubelet = model.Memkubelet,
                CpuSystem = model.Cpusystem,
                MemSystem = model.Memsystem,
                VerticalResponsibleId = model.Verticalresponsibleid,
                Comments = model.Comments,
                Notes = model.Notes,
                FileName = model.Filename,
                Revision = model.Revision,
                AggregateImageClusterSize = model.Aggregateimageclustersize,
                CnfClusterNodePoolId = model.Cnfclusternodepoolid,

                HardwareTypeDescription = model.Cnfhardware.Description,
                
                OpcoIdName = model.Opco?.Opco,
                SiteName = model.Site?.Shortdescription,
                LocationName = model.Site.Location,
                CnfClusterIdName = model.Cnfcluster != null ? model.Cnfcluster.Cnfclustername : string.Empty,
                K8ClusterAliasName = model.Cnfcluster != null ? model.Cnfcluster.Alaisname : string.Empty,
                NodePoolName = model.Cnfclusternodepool != null ? model.Cnfclusternodepool.Nodepool : string.Empty,
                CnfNameDescription = model.Cnfcluster != null ? model.Cnfname.Cnfdescription : string.Empty,
                VerticalResponsibleName = model.Cnfcluster != null ? model.Verticalresponsible.Verticalresponsible : string.Empty,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,

            };

            if (model.CreationuserNavigation != null)
                result.CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation);

            if (model.ModificationuserNavigation != null)
                result.ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation);

            if (model.Cnfpodinfo != null && model.Cnfpodinfo.Count > 0)
            {
                foreach (var item in model.Cnfpodinfo)
                {
                    result.CnfPodInfo.Add(CnfPodInfoMapper.GetCnfPodInfo(item));
                }
            }

            return result;
        }

        public static Cnfclusterinfo SetCnfClusterInfo(CnfClusterInfo model)
        {
            if (model == null)
                return null;
            var result = new Cnfclusterinfo()
            {
                Cnfclusterinfoid = model.CnfClusterInfoId,

                Cnfnameid = model.CnfNameId,
                Cnfclusterid = model.CnfClusterId,
                Cnfhardwareid = model.CnfHardwareId,
                Opcoid = model.OpcoId,
                Siteid = model.SiteId,
                
                Nodepoolbreakup = model.NodePoolBreakup,
                Specialrequirements = model.SpecialRequirements,
                Hyperthreading = model.Hyperthreading,
                Overprovisioning = model.OverProvisioning,
                Workernodeconfiguration = model.WorkerNodeConfiguration,
                Hardware = model.HardwareDescription,
                Cpukubelet = model.CpuKubelet,
                Memkubelet = model.MemKubelet,
                Cpusystem = model.CpuSystem,
                Memsystem = model.MemSystem,
                Verticalresponsibleid = model.VerticalResponsibleId,
                Comments = model.Comments,
                Notes = model.Notes,
                Filename = model.FileName,
                Revision = model.Revision,
                Aggregateimageclustersize = model.AggregateImageClusterSize,
                Cnfclusternodepoolid = model.CnfClusterNodePoolId,
            };
            return result;
        }
    }
}
