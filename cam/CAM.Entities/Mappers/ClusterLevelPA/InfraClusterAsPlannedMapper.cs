
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Model.ClusterLevelPA;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.ClusterLevelPA
{
    public static class InfraClusterAsPlannedMapper
    {
        
        public static InfraClusterAsPlanned GetInfraClusterAsPlanned(Infraclusterasplanned model)
        {

            if (model == null)
                return null;
            var result = new InfraClusterAsPlanned()
            {
                InfraClusterAsPlannedId = model.Infraclusterasplannedid,
                OpCoId = model.Opcoid,
                OpCoValue = model?.Opco?.Opco,
                LocationId = model.Locationid,
                 LocationValue = model?.Location?.Location,
                Site = model.Site,
                PlatformId = model.Platformid,
                PlatformValue = model?.Platform?.Platform,
                ClustertypeId = model.Clustertypeid,
                ClustertypeValue = model?.Clustertype?.Productname?.Description+"-"+model?.Clustertype?.Softwareversion,
                ClusterName = model.Clustername,
                HardwaretypeId = model.Hardwaretype,
                //OEM-Platform-HWModel
                // Text = x.Orgeqpmanufacturer.Originalequipmentmanufacturer+"-"+x.Platform.Platform+"-"+x.Hardwaretype 
                HardwaretypeValue = model?.HardwaretypeNavigation?.Orgeqpmanufacturer?.Originalequipmentmanufacturer+"-" 
                + model?.HardwaretypeNavigation?.Platform?.Platform + "-" + model?.HardwaretypeNavigation?.Hardwaretype,
                DeploymentStatusId = model.Deploymentstatusid,
                DeploymentStatusValue = model?.Deploymentstatus?.Deploymentstatus,
                VerticalResponsibleId = model.Verticalresponsibleid,
                VerticalResponsibleValue = model?.Verticalresponsible?.Verticalresponsible,
                CreationUser = model.Creationuser,
                CreationDate = model.Creationdate,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationuserNavigation = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationuserNavigation = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                NetworkElementClusterAsPlanned = model.Networkelementclusterasplanned.Select(p => NetworkElementClusterAsPlannedMapper.GetAppClusterAsPlannedMapper(p, false)).ToList(),

            };
             
            return result;
        }

      

        public static Infraclusterasplanned SeInfraClusterAsPlanned(InfraClusterAsPlanned model)
        {
            return new Infraclusterasplanned()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Infraclusterasplannedid = model.InfraClusterAsPlannedId,
                Opcoid = model.OpCoId,
                Locationid = model.LocationId,
                Site = model.Site,
                Platformid = model.PlatformId,
                Clustertypeid = model.ClustertypeId,
                Clustername= model.ClusterName ,
                Hardwaretype = model.HardwaretypeId  ,
                Deploymentstatusid = model.DeploymentStatusId
            };
        }
    }
}
