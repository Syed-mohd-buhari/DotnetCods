using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class DesignAspectMapper
    {
        public static DesignAspect Get (Designaspects model, bool Include = true,bool excludeDc = false)
        {
            if (model == null)
                return null;
            var result = new DesignAspect()
            {
                Id = model.Id,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                AuthenicationTypeId = model.Authenicationtypeid,
                SecurityTireZoneId = model.Securitytirezoneid,
                CriticalNationalInfrastructure = model.Criticalnationalinfrastructure,
                Description = model.Description,
                BusinessContinuityMethodId = model.Businesscontinuitymethodid,
                InstanceResilienceId = model.Instanceresilienceid,
                LicenseModelId = model.Licensemodelid,
                OpCoId = model.Opcoid,
                SiteResilienceId = model.Siteresilienceid,
                SWDeliveryLifeCycleId = model.Swdeliverylifecycleid,
                ThirdPartyAccessId = model.Thirdpartyaccessid,
                SecurityManagerId = model.Securitymanagerid,
                DesignComponentFamilyId = model.Designcomponentfamilyid,               
                AuthenicationType = AuthenicationTypeMapper.Get(model.Authenicationtype),
                BusinessContinuityMethod = BusinessContinuityMethodMapper.Get(model.Businesscontinuitymethod),
                InstanceResilience = InstanceResilienceMapper.Get(model.Instanceresilience),
                LicenseModel = LicenseModelMapper.Get(model.Licensemodel),
                OpCo = OpCoMapper.GetOpCoMapper(model.Opco),
                SecurityManager = SecurityManagerMapper.Get(model.Securitymanager),
                SecurityTireZone = SecurityTireZoneMapper.Get(model.Securitytirezone),
                SiteResilience = SiteResilienceMapper.Get(model.Siteresilience),
                SWDeliveryLifeCycle = SWDeliveryLifeCycleMapper.Get(model.Swdeliverylifecycle),
                ThirdPartyAccessType = ThirdPartyAccessTypeMapper.Get(model.Thirdpartyaccess),
                Archived = model.Archived,
                VodafoneName = (model.Designcomponentfamily != null && model.Designcomponentfamily.Subnetworkboundary != null) ?  model.Designcomponentfamily.Subnetworkboundary.Vodafonenameid : null,
                MaxAllowedLoading = model.Maxallowedloading,
                NominalCapacityLimit = model.Nominalcapacitylimit,
                DesignedCapacityLimit = model.Designedcapacitylimit,
                CriticalityRating = model.Criticalityrating
            };
            result.DesignAspectSupportedServices = Include ? model.Designaspectssupportedsvr.Select(p => DesignAspectSupportedServiceMapper.Get(p)).ToList() : null;
            result.DesignAspectNetworkFunctions = Include ? model.Designaspectsnetworkfunctions.Select(p => DesignAspectNetworkFunctionMapper.Get(p)).ToList() : null;
            result.PlannedActivities = Include  ? model.Plannedactivities.Select(p => PlannedActivityMapper.Get(p)).ToList() : null;


            result.DesignContactList = model?.Designcomponentfamily?.Designcomponents?.SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
            .Where(x => x.Deleted == false).Select(m => (int?)m.Designcontactid))?.
                  Distinct().ToList();

            var majorHardwareDesignContact = model?.Designcomponentfamily?.Designcomponents?.SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?.SelectMany(m =>
            m.Majorhardware?.Majorhwbuildsdesigncontacts.Where(x => x.Deleted == false).Select(n => (int?)n.Designcontactid))?.
               Distinct().ToList())?.ToList();

            if (result.DesignContactList?.Any() == true && majorHardwareDesignContact?.Any() == true)
                result.DesignContactList = result.DesignContactList.Union(majorHardwareDesignContact).ToList();
            else if (result.DesignContactList?.Any() == false && majorHardwareDesignContact?.Any() == true)
                result.DesignContactList = majorHardwareDesignContact;

            if (excludeDc == true) //Code optimize 
            {
                model.Designcomponentfamily.Designcomponents = null;
                result.DesignComponentFamily = DesignComponentFamilyMapper.Get(model.Designcomponentfamily);
            }
            else 
                result.DesignComponentFamily = DesignComponentFamilyMapper.Get(model.Designcomponentfamily);


            return result;
        }

        public static Designaspects Set(DesignAspect model)
        {
            if (model == null)
                return null;
            return new Designaspects()
            {
                Id = model.Id,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Authenicationtypeid = model.AuthenicationTypeId,
                Securitytirezoneid = model.SecurityTireZoneId,
                Criticalnationalinfrastructure = model.CriticalNationalInfrastructure,
                Description = model.Description,
                Businesscontinuitymethodid = model.BusinessContinuityMethodId,
                Instanceresilienceid = model.InstanceResilienceId,
                Licensemodelid = model.LicenseModelId,
                Opcoid = model.OpCoId,
                Siteresilienceid = model.SiteResilienceId,
                Swdeliverylifecycleid = model.SWDeliveryLifeCycleId,
                Thirdpartyaccessid = model.ThirdPartyAccessId,
                Securitymanagerid = model.SecurityManagerId,
                Designcomponentfamilyid = model.DesignComponentFamilyId,
                Archived = model.Archived,
                Nominalcapacitylimit = model.NominalCapacityLimit,
                Maxallowedloading = model.MaxAllowedLoading,
                Designedcapacitylimit = model.DesignedCapacityLimit,
                Criticalityrating = model.CriticalityRating
            };

        }
    }
}
