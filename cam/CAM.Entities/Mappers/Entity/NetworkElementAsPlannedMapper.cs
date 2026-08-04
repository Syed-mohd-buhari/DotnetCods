using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class NetworkElementAsPlannedMapper
    {
        public static NetworkElementAsPlanned Get (Networkelementsasplanned model , bool Include =true)
        {
            if (model == null)
                return null;
            var result = new NetworkElementAsPlanned()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                AdditionalInformation1 = model.Additionalinformation1,
                AdditionalInformation2 = model.Additionalinformation2,
                AutomatedFeedback = model.Automatedfeedback,
                CapacityPlanReference = model.Capacityplanreference,
                DeploymentStatusId = model.Deploymentstatusid,
                DeploymentTypeId = model.Deploymenttypeid,
                DesignComponentId =model.Designcomponentid,
                DesignComponentFamilyId = model.Designcomponent != null? model.Designcomponent.Designcomponentfamilyid : null,
                ElementName = model.Elementname,
                EnvironmentId = model.Environmentid,
                LocationId=model.Locationid,
                NetworkConstruct = model.Networkconstruct,
                NetworkElementAsPlannedId =model.Networkelementasplannedid,
                NfviBundleIDId = model.Nfvibundleidid,
                OpCoId = model.Opcoid,
                LcmEngineeringId = model.Lcmengineeringid,
                OriginalEquipmentManufacturerId = model.Orgeqpmanufacturerid,
                PlannedAction =model.Plannedactivities.Any(x=>x.Archived != true) && model.Plannedactivities.Count > 0 ? true: false,
                DeploymentStatus = DeploymentStatusMapper.GetDeploymentStatusMapper(model.Deploymentstatus),
                DeploymentType = DeploymentTypeMapper.GetDeploymentTypeMapper(model.Deploymenttype),
                DesignComponent = Include ? DesignComponentMapper.GetDesignComponentMapper(model.Designcomponent) : null,
                Location = LocationMapper.GetLocationMapper(model.Location),
                Environment =EnvironmentMapper.GetEnvironmentMapper(model.Environment),
                NFVIBundleID = NFVIBundleIDMapper.Get(model.Nfvibundleid),
                OpCo=OpCoMapper.GetOpCoMapper(model.Opco),
                LcmEngineering = LCMEngineeringMapper.GetLcmEngineeringDeployStatusMapper(model.Lcmengineering),
                VodafoneName = model.Designcomponent?.Subnetworkboundary?.Vodafonename?.Description,
                HwResourceKey = model.Hwresourcekey,
                PreviousHWResourceKey = model.Previoushwresourcekey,
                SwResourceKey = model.Swresourcekey,
                PreviousSWResourceKey = model.Previousswresourcekey,              
                IsFinalAsset = model.Isfinalasset,                
                NetworkElementEduSpocIdList =
                 Include ? model.Networkelementasplannededuspoc.Select(p => (int?)p?.Eduspocid).ToList() : new List<int?>(),
                PlannedActivityDictonary = model.Plannedactivities.Where(x => x.Deleted == false && x.Archived != true).ToDictionary(x => x.Plannedactivityid,
                            x =>
                              $" {x?.Plannedcompletion.Value.ToString("MMM")} | " +
                              $" {x?.Plannedcompletion.Value.ToString("yyyy")} | " +
                              $" {x?.Plannedactivityresource?.Plannedactivityresource} | " +
                              $" {x?.Deliverystatus?.Deliverystatus} | " ),
                Buildbag = Include ? BuildBagMapper.GetBuildBagDropdownAsync(model.Buildbag) : null,
                Buildbagid = model.Buildbagid,
                IsAssured = model.Isassured,
                ElementDomianName = model.Elementdomianname,
                AssetLiveStatusDate =  model.Assetlivestatusdate != null ? (DateTime)model.Assetlivestatusdate : null,
                AssetDecommissionedDate = model.Assetdecommissioneddate != null ? (DateTime)model.Assetdecommissioneddate : null,
                RfaDate=model.Rfadate,
                HwPoArrivedDate=model.Hwpoarriveddate,
                HwPoRaisedDate=model.Hwporaiseddate,
                BomSubmittedDate=model.Hwporaiseddate,
            };

           

            if (model.Networkelementasplannedsubdomainspoc != null)
            {
                foreach (var item in model.Networkelementasplannedsubdomainspoc)
                {
                    result.NetworkElementAsPlannedSubDomainSpoc.Add(NetworkElementAsPlannedSubDomainSpocMapper.GetSpocId(item));
                }
            }
            return result;
        }

        public static NetworkElementAsPlanned GetEnvMapperFromNTWElement(Networkelementsasplanned model, bool Include = true)
        {
            if (model == null)
                return null;
            var result = new NetworkElementAsPlanned()
            {

                NetworkElementAsPlannedId = model.Networkelementasplannedid,
                Environment = EnvironmentMapper.GetEnvironmentMapper(model.Environment),
                ElementDomianName = model.Elementdomianname,
            };
            return result;
        }


        public static Networkelementsasplanned Set( NetworkElementAsPlanned model)
        {
            var result = new Networkelementsasplanned()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,

                Additionalinformation1 = model.AdditionalInformation1,
                Additionalinformation2 = model.AdditionalInformation2,
                Automatedfeedback = model.AutomatedFeedback,
                Capacityplanreference = model.CapacityPlanReference,
                Deploymentstatusid = model.DeploymentStatusId,
                Deploymenttypeid = model.DeploymentTypeId,
                Designcomponentid = model.DesignComponentId,
                Elementname = model.ElementName,
                Environmentid = model.EnvironmentId,
                Locationid = model.LocationId,
                Networkconstruct = model.NetworkConstruct,
                Networkelementasplannedid = model.NetworkElementAsPlannedId,
                Nfvibundleidid = model.NfviBundleIDId,
                Opcoid = model.OpCoId,
                Orgeqpmanufacturerid = model.OriginalEquipmentManufacturerId,
                Plannedaction = model.PlannedActivities.Any(x => x.Archived != true) && model.PlannedActivities.Count > 0 ? true : false,
                Swresourcekey = model.SwResourceKey,
                Previousswresourcekey = model.PreviousSWResourceKey,
                Hwresourcekey = model.HwResourceKey,
                Previoushwresourcekey = model.PreviousHWResourceKey,
                Lcmengineeringid = model.LcmEngineeringId,            
                Isfinalasset = model.IsFinalAsset,
                Buildbagid = model.Buildbagid,
                Isassured = model.IsAssured,
                Elementdomianname = model.ElementDomianName,
                Assetlivestatusdate = model.AssetLiveStatusDate != null ? model.AssetLiveStatusDate.Value : null,
                Assetdecommissioneddate = model.AssetDecommissionedDate != null ? model.AssetDecommissionedDate.Value : null,
                Assetrfsdate = model.AssetRfsDate != null ? model.AssetRfsDate.Value : null,
                Assetrfodate = model.AssetRfoDate != null ? model.AssetRfoDate.Value : null,
                Rfadate = model.RfaDate,
                Hwporaiseddate = model.HwPoRaisedDate,
                Hwpoarriveddate = model.HwPoArrivedDate,
                Bomsubmitteddate = model.BomSubmittedDate,
            };
            if (model.PlannedActivities != null)
            {
                foreach (var item in model.PlannedActivities)
                {
                    result.Plannedactivities.Add(PlannedActivityMapper.Set(item));
                } 
            }
            if (model.NetworkElementAsPlannedEduSpoc != null)
            {
                foreach (var item in model.NetworkElementAsPlannedEduSpoc)
                {
                    result.Networkelementasplannededuspoc.Add(NetworkElementAsPlannedEduSpocMapper.Set(item));
                }
            }
            if (model.NetworkElementAsPlannedSubDomainSpoc != null)
            {
                foreach (var item in model.NetworkElementAsPlannedSubDomainSpoc)
                {
                    result.Networkelementasplannedsubdomainspoc.Add(NetworkElementAsPlannedSubDomainSpocMapper.Set(item));
                }
            }
            return result;
        }
    }
}
