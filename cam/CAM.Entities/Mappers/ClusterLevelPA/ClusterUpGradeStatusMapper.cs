using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Model.ClusterLevelPA;
using CAM.Entities.Models;
using CAM.Enum;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.ClusterLevelPA
{
    public static class ClusterUpGradeStatusMapper
    {
        public static ClusterUpGradeStatus GetClusterUpGradeStatusMapper(Clusterupgradestatus model )
        {
            if (model == null)
                return null;
            var result = new ClusterUpGradeStatus()
            {
                ClusterUpGradeStatusId = model.Clusterupgradestatusid,
                InfraClusterAsPlannedId = model.Infraclusterasplannedid,
                NetworkElementClusterAsPlannedId = model.Networkelementclusterasplannedid,
                PlannedActivityId = model.Plannedactivityid,
                StatusId = model.Statusid,
              
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                
            };
            
            return result;

        }
        public static Clusterupgradestatus SetClusterUpGradeStatusMapper(ClusterUpGradeStatus model)
        {
            if (model == null)
                return null;
            var result = new Clusterupgradestatus()
            {
                Clusterupgradestatusid = model.ClusterUpGradeStatusId,
                Infraclusterasplannedid = model.InfraClusterAsPlannedId,
                Networkelementclusterasplannedid = model.NetworkElementClusterAsPlannedId,
                Plannedactivityid = model.PlannedActivityId,
                Statusid = model.StatusId,
              
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                

            };
          
            return result;
        }
    }
}
