using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public class DaAssetMigrationMapper
    {
        public static DaAssetMigration GetDaAssetMigration(Daassetmigration model)
        {
            if (model == null)
                return null;
            var result = new DaAssetMigration()
            {
                DaAssetMigrationId = model.Daassetmigrationid,
                PlannedActivityId = model.Plannedactivityid,

                NetworkElementAsPlannedId = model.Networkelementasplannedid,
                OldAssetName = model?.Networkelementasplanned != null ? model?.Networkelementasplanned.Elementname : string.Empty,
                OldDeploymentStatus = model?.Networkelementasplanned?.Deploymentstatus?.Deploymentstatus ?? string.Empty,
                OldEnvironment = model?.Networkelementasplanned?.Environment?.Environment ?? string.Empty,
                CurrentDesignComponenetId = model?.Networkelementasplanned?.Designcomponentid ?? 0,

                NewelEmentName = model.Newelementname,
                TargetDesignComponenetId = model.Targetdesigncomponenetid,

                NewEnvironmentId = model.Environmentid,
                NewDeploymentStatusId = model.Deploymentstatusid,
                OpcoId = model.Opcoid,
                NewLocationId = model.Locationid,

                RfoDate = model.Rfodate,
                RfsDate = model.Rfsdate,
                MigrationCompletionDate = model.Migrationcompletiondate,
                TrafficNodePercentage = model.Trafficnodepercentage,

                NewOpco = model?.Opco?.Opco ?? string.Empty,
                NewLocation = model?.Location?.Location ?? string.Empty,
                NewDeploymentStatus = model?.Deploymentstatus?.Deploymentstatus ?? string.Empty,
                NewEnvironment = model?.Environment?.Environment ?? string.Empty,

                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                HwPoArrivedDate = model.Hwpoarriveddate,
                HwPoRaisedDate = model.Hwporaiseddate,
                BomSubmittedDate = model.Bomsubmitteddate,
                RfaDate = model.Rfadate,
                IsDecommissioned = model.Isdecommissioned,
                ProductNameId = model.Productnameid,
                PlatformId = model.Platformid,
                Vecdate = model.Vecdate,
                Startofappintegration = model.Startofappintegration,
                Migrationstart = model.Migrationstart,
                //CurrentSwProductName = model?.Plannedactivity?.Designaspect?.Designcomponentfamily?.Designcomponents?.Select(x => x.Systemtype?.Majorsoftwarebuilds?.Productname?.Description).FirstOrDefault() ?? string.Empty,
                //TargerHwPlatform = model?.Plannedactivity?.Designcomponentfamily?.Designcomponents?.Select(x => x.Systemtype?.Systemtypesmajorhardwarebuilds
                //.Select(x => x.Majorhardware?.Platform?.Platform).FirstOrDefault()).FirstOrDefault() ?? string.Empty,
                

            };

            if(result.ProductNames == null)
            {
                result.ProductNames = ProductNameMapper.GetProductNameMapper(model.Productname);
            }
            if(result.Platforms == null)
            {
                result.Platforms = PlatFormMapper.GetPlatFormMapper(model.Platform);
            }
            if(result.PlannedActivity == null)
            {
                result.PlannedActivity = PlannedActivityMapper.Get(model.Plannedactivity);
            }
            if (result.NetworkElementAsPlanned == null)
            {
                result.NetworkElementAsPlanned = NetworkElementAsPlannedMapper.GetEnvMapperFromNTWElement(model.Networkelementasplanned);
            }
            return result;
        }

        public static DaAssetMigration GetDaAssetMigrationForReports(Daassetmigration model)
        {
            try
            {
                if (model == null)
                    return null;
                var result = new DaAssetMigration()
                {
                    DaAssetMigrationId = model.Daassetmigrationid,
                    PlannedActivityId = model.Plannedactivityid,
                    NetworkElementAsPlannedId = model.Networkelementasplannedid,
                    OldAssetName = model?.Networkelementasplanned != null ? model?.Networkelementasplanned.Elementname : string.Empty,
                    NewelEmentName = model.Newelementname,
                    TargetDesignComponenetId = model.Targetdesigncomponenetid,
                    NewEnvironmentId = model.Environmentid,
                    NewDeploymentStatusId = model.Deploymentstatusid,
                    OpcoId = model.Opcoid,
                    OpcoDescription = model?.Opco?.Opco,
                    EnvironmentDescription = model?.Networkelementasplanned?.Environment?.Environment,
                    EnvironmentId = Convert.ToInt64(model?.Networkelementasplanned?.Environment?.Environmentid),
                    NewLocationId = model.Locationid,
                    RfoDate = model.Rfodate,
                    RfsDate = model.Rfsdate,
                    MigrationCompletionDate = model.Migrationcompletiondate,
                    NewOpco = model?.Opco?.Opco ?? string.Empty,
                    NewLocation = model?.Location?.Location ?? string.Empty,
                    NewDeploymentStatus = model?.Deploymentstatus?.Deploymentstatus ?? string.Empty,
                    HwPoArrivedDate = model.Hwpoarriveddate,
                    HwPoRaisedDate = model.Hwporaiseddate,
                    BomSubmittedDate = model.Bomsubmitteddate,
                    RfaDate = model.Rfadate,
                    ProductNameId = model.Productnameid,
                    PlatformId = model.Platformid,
                    Vecdate = model.Vecdate,
                    Startofappintegration = model.Startofappintegration,
                    Migrationstart = model?.Migrationstart,
                    //CurrentSwProductName = model?.Plannedactivity?.Designaspect?.Designcomponentfamily?.Designcomponents?.Select(x => x.Systemtype?.Majorsoftwarebuilds?.Productname?.Description).FirstOrDefault() ?? string.Empty,
                    //TargerHwPlatform = model?.Plannedactivity?.Designcomponentfamily?.Designcomponents?.Select(x => x.Systemtype?.Systemtypesmajorhardwarebuilds
                    //.Select(x => x.Majorhardware?.Platform?.Platform).FirstOrDefault()).FirstOrDefault() ?? string.Empty,

                };

                if (result.PlannedActivity == null)
                {
                    result.PlannedActivity = PlannedActivityMapper.GetPaForReports(model.Plannedactivity);
                }
                if (result.ProductNames == null)
                {
                    result.ProductNames = ProductNameMapper.GetProductNameMapper(model.Productname);
                }
                if (result.Platforms == null)
                {
                    result.Platforms = PlatFormMapper.GetPlatFormMapper(model.Platform);
                }
                return result;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static Daassetmigration SetDaAssetMigration(DaAssetMigration model)
        {
            if (model == null)
                return null;
            var result = new Daassetmigration()
            {
                Daassetmigrationid = model.DaAssetMigrationId,
                Plannedactivityid = model.PlannedActivityId,
                Networkelementasplannedid = model.NetworkElementAsPlannedId,
                Newelementname = model.NewelEmentName,
                Targetdesigncomponenetid = model.TargetDesignComponenetId,
                Environmentid = model.NewEnvironmentId,
                Deploymentstatusid = model.NewDeploymentStatusId,                
                Opcoid = model.OpcoId,
                Locationid = model.NewLocationId,
                Rfodate = model.RfoDate,
                Rfsdate = model.RfsDate,
                Migrationcompletiondate = model.MigrationCompletionDate,
                Trafficnodepercentage = model.TrafficNodePercentage,
                Platformid = model.PlatformId,
                Productnameid = model.ProductNameId,
                Vecdate = model.Vecdate,
                Startofappintegration = model.Startofappintegration,
                Migrationstart = model.Migrationstart,
            };
            return result;
        }
    }
}
