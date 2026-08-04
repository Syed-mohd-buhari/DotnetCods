using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Model.ClusterLevelPA;
using CAM.Entities.Models.ComponentSoftware;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.ClusterLevelPA
{
    public static class NetworkElementClusterAsPlannedMapper
    {
        public static NetworkElementClusterAsPlanned GetAppClusterAsPlannedMapper(Networkelementclusterasplanned model, bool include = false )
        {
            if (model == null)
                return null;
            var result = new NetworkElementClusterAsPlanned()
            {
               NetworkElementClusterAsPlannedId = model.Networkelementclusterasplannedid,
               InfraClusterAsPlannedId = model.Infraclusterasplannedid,
               ApplicationId = model.Applicationid,    
               ApplicationName = model.Application.Description,
                AppClusterName = model.Appclustername,
               DeploymentStatusId= model.Deploymentstatusid,
               DeploymentStatus = model.Deploymentstatus!=null?DeploymentStatusMapper.GetDeploymentStatusMapper(model.Deploymentstatus): null,
                CreationDate = model.Creationdate,
                ModificationDate = model.Modificationdate,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),                                          
            };
            

            return result;

        }
        public static Networkelementclusterasplanned SetAppClusterAsPlannedMapper(NetworkElementClusterAsPlanned model)
        {
            if (model == null)
                return null;
            var result = new Networkelementclusterasplanned()
            {
                Networkelementclusterasplannedid = model.NetworkElementClusterAsPlannedId,
                Infraclusterasplannedid = model.InfraClusterAsPlannedId,
                Applicationid = model.ApplicationId,
                Appclustername = model.AppClusterName,
                Deploymentstatusid = model.DeploymentStatusId,
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
