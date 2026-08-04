using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Enum;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class ComponentSoftwareBuildMapper
    {
        public static ComponentSoftwareBuild GetComponentSoftwareBuildMapper(Componentsoftwarebuilds model )
        {
            if (model == null)
                return null;
            var result = new ComponentSoftwareBuild()
            {
                ComponentSoftwareBuildId = model.Componentsoftwarebuildid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                EndOfMaintenance = model.Endofmaintenance,
                EOMStatus = (EOMEnum)model.Eomstatus,
                EndOfsupport = model.Endofsupport,
                LastTimeBuyExpansions = model.Lasttimebuyexpansions,
                LastTimeBuyNew = model.Lasttimebuynew,
                LastTimeBuyUpgrades = model.Lasttimebuyupgrades,
                Componentmanufacturerid = model.Componentmanufacturerid,
                SpareFieldsJson = model.Sparefieldsjson,
                VulnerabilityStatus = model.Vulnerabilitystatus,
                DeliveryMethod =model.Deliverymethod,
                GeneraAvailableDate = model.Generaavailabledate,
                OperatingSystemId = model.Operatingsystemid,
                SoftwareVersion = model.Softwareversion,              
                CriticalAssetTypeId = model.Criticalassettypeid,
                CriticalAssetType = model.Criticalassettype == null ? null : CriticalAssetTypeMapper.Get(model.Criticalassettype),
                OperatingSystem = OperatingSystemMapper.GetOperatingSystemMapper(model.Operatingsystem),
                ComponentManufacturers = ComponentManufacturersMapper.GetComponentManufacturersMapper(model.Componentmanufacturer),                
                Description = model.Description,
              
                
            };
            if (model.Componentsoftwarebuildsdesigncontacts != null && model.Componentsoftwarebuildsdesigncontacts.Count > 0)
            {
                foreach (var item in model.Componentsoftwarebuildsdesigncontacts)
                {
                    result.ComponentSoftwareBuildsDesignContact.Add(ComponentSoftwareBuildDesignContactsMapper.GetComponentSWDesignContact(item));
                }
            }

            return result;

        }
        public static Componentsoftwarebuilds SetComponentSoftwareBuildMapper(ComponentSoftwareBuild model)
        {
            if (model == null)
                return null;
            var result = new Componentsoftwarebuilds()
            {
                Componentsoftwarebuildid = model.ComponentSoftwareBuildId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Endofmaintenance = model.EndOfMaintenance,
                Eomstatus = (short)model.EOMStatus,
                Endofsupport = model.EndOfsupport,
                Lasttimebuyexpansions = model.LastTimeBuyExpansions,
                Lasttimebuynew = model.LastTimeBuyNew,
                Lasttimebuyupgrades = model.LastTimeBuyUpgrades,
                Componentmanufacturerid = model.Componentmanufacturerid,
                Sparefieldsjson = model.SpareFieldsJson,
                Vulnerabilitystatus = model.VulnerabilityStatus,
                Deliverymethod = model.DeliveryMethod,
                Generaavailabledate = model.GeneraAvailableDate,
                Operatingsystemid = model.OperatingSystemId,
                Softwareversion = model.SoftwareVersion,
                Criticalassettypeid = model.CriticalAssetTypeId,
                Criticalassettype = CriticalAssetTypeMapper.Set(model.CriticalAssetType),
                Operatingsystem = OperatingSystemMapper.SetOperatingSystemMapper(model.OperatingSystem),
                Componentmanufacturer = ComponentManufacturersMapper.SetComponentManufacturersMapper(model.ComponentManufacturers),
                Description = model.Description,

                 
            };
          
            return result;
        }
    }
}
