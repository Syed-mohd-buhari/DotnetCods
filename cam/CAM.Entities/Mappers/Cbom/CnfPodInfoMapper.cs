using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.CBom;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cbom
{
    public partial class CnfPodInfoMapper
    {
        public static CnfPodInfo GetCnfPodInfo(Cnfpodinfo model)
        {
            if (model == null)
                return null;
            var result = new CnfPodInfo()
            {
                CnfPodInfoId = model.Cnfpodinfoid,
                CnfClusterInfoId = model.Cnfclusterinfoid,
                PodTypeInfoId = model.Podtypeinfoid,
                FunctionStandardId = model.Functionstandardid ,
                PriorityId = model.Priorityid,               
                DaemonSetPod = model.Daemonsetpod,
                IntraPodRules = model.Intrapodrules,
                InterPodRules = model.Interpodrules,
                IsEnhancedHa = model.Isenhancedha,
                PodTypeQos = model.Podtypeqos,
                IsPersistanceStorageFlag = model.Ispersistancestorageflag,
                IsProdHpaEnable = model.Isprodhpaenable,   
                PodroleDescriptionId = model.Podroledescriptionid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                PodTypeInfoName = model.Podtypeinfo != null ? model.Podtypeinfo.Podtypeinfoname : null,
                PodroleDescription = model.Podroledescription != null ? model.Podroledescription.Podroledescription : null,
                PriorityName = model.Priority != null ? model.Priority.Description : null,
                FunctionStandardName = model.Functionstandard != null ? model.Functionstandard.Functionname : null,

            };

            if (model.CreationuserNavigation != null)
                result.CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation);

            if (model.ModificationuserNavigation != null)
                result.ModificationuserNavigation = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation);
           
            if (model.Cnfcapacity != null && model.Cnfcapacity.Count > 0)
            {
                foreach (var item in model.Cnfcapacity)
                {
                    result.CnfCapacity.Add(CnfCapacityMapper.GetCnfCapacityMapper(item));
                }
            }
            return result;
        }

        public static Cnfpodinfo SetCnfPodInfo(CnfPodInfo model)
        {
            if (model == null)
                return null;
            var result = new Cnfpodinfo()
            {
                Cnfpodinfoid = model.CnfPodInfoId,
                Cnfclusterinfoid = model.CnfClusterInfoId,
                Podtypeinfoid = model.PodTypeInfoId,
                Functionstandardid = model.FunctionStandardId,
                Priorityid = model.PriorityId,                
                Daemonsetpod = model.DaemonSetPod,
                Intrapodrules = model.IntraPodRules,
                Interpodrules = model.InterPodRules,
                Isenhancedha = model.IsEnhancedHa,
                Podtypeqos = model.PodTypeQos,
                Ispersistancestorageflag = model.IsPersistanceStorageFlag,
                Isprodhpaenable = model.IsProdHpaEnable,
                Podroledescriptionid = model.PodRoleDescriptionId,
            };
            return result;
        }
    }
}
