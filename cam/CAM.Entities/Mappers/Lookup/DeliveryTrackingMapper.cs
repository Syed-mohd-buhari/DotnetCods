using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class DeliveryTrackingMapper
    {
        public static DeliveryTracking GetDeliveryTrackingMapper(Deliverytrackings model)
        {
            if (model == null)
                return null;
            var result = new DeliveryTracking()
            {
                Id = model.Id,
                MS1LatestPlanningDate = model.Ms1latestplanningdate,
                MS1Status = model.Ms1status,
                MS1EventType = model.Ms1eventtype,
                MS1BaseLineDate = model.Ms1baselinedate,
                MS2LatestPlanningDate = model.Ms2latestplanningdate,
                MS2Status = model.Ms2status,
                MS2EventType = model.Ms2eventtype,
                MS2BaseLineDate = model.Ms2baselinedate,
                MS3LatestPlanningDate = model.Ms3latestplanningdate,
                MS3Status = model.Ms3status,
                MS3EventType = model.Ms3eventtype,
                MS3BaseLineDate = model.Ms3baselinedate,
                MS4LatestPlanningDate = model.Ms4latestplanningdate,
                MS4Status = model.Ms4status,
                MS4EventType = model.Ms4eventtype,
                MS4BaseLineDate = model.Ms4baselinedate,
                PlannedActivityId = model.Plannedactivityid,
                PPMImportDate = model.Ppmimportdate,
                Notes1 = model.Notes1,
                Notes2 = model.Notes2,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                PlannedActivity = PlannedActivityMapper.Get(model.Plannedactivity),
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                OpcoId = model.Plannedactivity?.Opcoid,
                OpcoDescription = model.Plannedactivity?.Opco?.Opco
            };
            if (model.Plannedactivity != null && model.Plannedactivity?.Lcmengineering!=null)
            {
                result.LcmEngineeringId = model.Plannedactivity?.Lcmengineeringid;
                result.LcmEngineeringSubdomainSpoc = model.Plannedactivity?.Lcmengineering?.Lcmengineeringsubdomainspoc!=null && model.Plannedactivity?.Lcmengineering?.Lcmengineeringsubdomainspoc.Count>0 ?
                                                     model.Plannedactivity?.Lcmengineering?.Lcmengineeringsubdomainspoc.Select(x=>x.Subdomainspocid).ToList() : null;
            }
            if (model.Plannedactivity != null && model.Plannedactivity?.Networkelementasplanned != null)
            {
                result.AssetId = model.Plannedactivity?.Networkelementasplannedid;
                result.AssetSubdomainSpoc = model.Plannedactivity?.Networkelementasplanned?.Networkelementasplannedsubdomainspoc.Count > 0 ?
                                                     model.Plannedactivity?.Networkelementasplanned?.Networkelementasplannedsubdomainspoc.Select(x => x.Subdomainspocid).ToList() : null;
            }
            if (model.Plannedactivity != null && model.Plannedactivity.Designaspect != null)
            {
                result.DesignAspectId= model.Plannedactivity?.Designaspect?.Id;
                result.DesignContactDto = model?.Plannedactivity?.Designaspect != null ?
                                          model?.Plannedactivity?.Designaspect?.Designcomponentfamily?.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                                         .Where(x => x.Deleted == false)
                                         .Select(m => (int?)m.Designcontactid))?.
                                          Distinct().ToList() : null;

                var majorHardwareDesignContact = model?.Plannedactivity?.Designaspect != null ?
                                          model?.Plannedactivity?.Designaspect?.Designcomponentfamily?.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                          .SelectMany(m => m.Majorhardware?.Majorhwbuildsdesigncontacts.
                                          Where(x => x.Deleted == false)
                                          .Select(n => (int?)n.Designcontactid))?.
                                          Distinct().ToList())?.ToList() : null;

                if (result.DesignContactDto?.Any() == true && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = result.DesignContactDto.Union(majorHardwareDesignContact).ToList();
                else if (result.DesignContactDto?.Any() == false && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = majorHardwareDesignContact;
            }
            if (model.Plannedactivity != null && result.PlannedActivity.Serviceplan != null)
            {
                result.ServiceInfoId = model.Plannedactivity?.Serviceplanid;
                result.DesignContactDto = model?.Plannedactivity?.Serviceplan != null ?
                                          model?.Plannedactivity?.Serviceplan?.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                                         .Where(x => x.Deleted == false)
                                         .Select(m => (int?)m.Designcontactid)))?.
                                          Distinct().ToList() : null;
                var majorHardwareDesignContact = model?.Plannedactivity?.Serviceplan != null ?
                                          model?.Plannedactivity?.Serviceplan?.Serviceplandcfmappings?.SelectMany(x => x.Dcf.Designcomponents?
                                          .SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                          .SelectMany(m => m.Majorhardware?.Majorhwbuildsdesigncontacts.
                                          Where(x => x.Deleted == false)
                                          .Select(n => (int?)n.Designcontactid))?.
                                          Distinct().ToList()))?.ToList() : null;
                if (result.DesignContactDto?.Any() == true && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = result.DesignContactDto.Union(majorHardwareDesignContact).ToList();
                else if (result.DesignContactDto?.Any() == false && majorHardwareDesignContact?.Any() == true)
                    result.DesignContactDto = majorHardwareDesignContact;
            }
            return result;
        }
        public static Deliverytrackings SetDeliveryTrackingMapper(DeliveryTracking model)
        {
            if (model == null) return null;
            return new Deliverytrackings()
            {
                Id = model.Id,
                Ms1latestplanningdate = model.MS1LatestPlanningDate,
                Ms1status = model.MS1Status,
                Ms1eventtype = model.MS1EventType,
                Ms1baselinedate = model.MS1BaseLineDate,
                Ms2latestplanningdate = model.MS2LatestPlanningDate,
                Ms2status = model.MS2Status,
                Ms2eventtype = model.MS2EventType,
                Ms2baselinedate = model.MS2BaseLineDate,
                Ms3latestplanningdate = model.MS3LatestPlanningDate,
                Ms3status = model.MS3Status,
                Ms3eventtype = model.MS3EventType,
                Ms3baselinedate = model.MS3BaseLineDate,
                Ms4latestplanningdate = model.MS4LatestPlanningDate,
                Ms4status = model.MS4Status,
                Ms4eventtype = model.MS4EventType,
                Ms4baselinedate = model.MS4BaseLineDate,
                Plannedactivityid = model.PlannedActivityId,
                Ppmimportdate = model.PPMImportDate,
                Notes1 = model.Notes1,
                Notes2 = model.Notes2,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
            };
        }
    }
}

