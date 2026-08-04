using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Enum;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class MajorSoftwareBuildMapper
    {
        public static MajorSoftwareBuild GetMajorSoftwareBuildMapper(Majorsoftwarebuilds model , bool Include = true)
        {
            if (model == null)
                return null;
            var result = new MajorSoftwareBuild()
            {
                MajorSoftwareBuildsId = model.Majorsoftwarebuildsid,
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
                OriginalEquipmentManufacturerId = model.Orgeqpmanufacturerid,
                SpareFieldsJson = model.Sparefieldsjson,
                VulnerabilityStatus = model.Vulnerabilitystatus,
                DeliveryMethod =model.Deliverymethod,
                GeneraAvailableDate = model.Generaavailabledate,
                OperatingSystemId = model.Operatingsystemid,
                SoftwareVersion = model.Softwareversion,
                ProductNameId = model.Productnameid,
                ProductName = ProductNameMapper.GetProductNameMapper(model.Productname),
                CriticalAssetTypeId = model.Criticalassettypeid,
                CriticalAssetType = model.Criticalassettype == null ? null : CriticalAssetTypeMapper.Get(model.Criticalassettype),
                OperatingSystem = OperatingSystemMapper.GetOperatingSystemMapper(model.Operatingsystem),
                OriginalEquipmentManufacturer = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(model.Orgeqpmanufacturer),
                SystemTypes = Include ? model.Systemtypes.Select(p=>SystemTypeMapper.GetSystemTypeMapper(p ,false) ).ToList() : null,
                NetworkFunctions = model.Majorsoftwarebuildnetworkfunction.Select(p => MajorSoftwareBuildNetworkFunctionMapper.Get(p)).ToList(),
                Description = model.Description,
                Isvmware = model.Isvmware,   
                SoftwarebuildcompatibilityMajorsoftwarebuild = model.SoftwarebuildcompatibilityMajorsoftwarebuild
                .Select(p => SoftwareBuildCompatibilityMapper.Get(p)).ToList(),
                
            };
            if (model.Majorswbuildsdesigncontacts != null && model.Majorswbuildsdesigncontacts.Count >0)
            {
                foreach (var item in model.Majorswbuildsdesigncontacts)
                {
                    result.MajorSwBuidlsDesignContacts.Add(MajorSwBuidlsDesignContactsMapper.Get(item));
                }
            }

            return result;

        }

        public static MajorSoftwareBuild GetProductAndDesignContactMapper(Majorsoftwarebuilds model, bool Include = true)
        {
            if (model == null)
                return null;
            var result = new MajorSoftwareBuild()
            {
                MajorSoftwareBuildsId = model.Majorsoftwarebuildsid,
                EndOfMaintenance = model.Endofmaintenance,
                EOMStatus = (EOMEnum)model.Eomstatus,
                EndOfsupport = model.Endofsupport,
                ProductNameId = model.Productnameid,
                ProductName = ProductNameMapper.GetProductNameMapper(model.Productname)               
            };
            if (model.Majorswbuildsdesigncontacts != null && model.Majorswbuildsdesigncontacts.Count > 0)
            {
                foreach (var item in model.Majorswbuildsdesigncontacts)
                {
                    //result.MajorSwBuidlsDesignContacts.Add(MajorSwBuidlsDesignContactsMapper.Get(item));
                    result.MajorSwBuidlsDesignContacts.Add( new MajorSwBuidlsDesignContact{ DesignContactId = item.Designcontactid});
                }
            }

            return result;

        }
        public static Majorsoftwarebuilds SetMajorSoftwareBuildMapper(MajorSoftwareBuild model)
        {
            if (model == null)
                return null;
            var result = new Majorsoftwarebuilds()
            {
                Majorsoftwarebuildsid = model.MajorSoftwareBuildsId,
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
                Orgeqpmanufacturerid = model.OriginalEquipmentManufacturerId,
                Sparefieldsjson = model.SpareFieldsJson,
                Vulnerabilitystatus = model.VulnerabilityStatus,
                Deliverymethod = model.DeliveryMethod,
                Generaavailabledate = model.GeneraAvailableDate,
                Operatingsystemid = model.OperatingSystemId,
                Softwareversion = model.SoftwareVersion,
                Productnameid = model.ProductNameId,
                Isvmware = model.Isvmware,  
                Productname = ProductNameMapper.SetProductNameMapper(model.ProductName),
                Criticalassettypeid = model.CriticalAssetTypeId,
                Criticalassettype = CriticalAssetTypeMapper.Set(model.CriticalAssetType),
                Operatingsystem = OperatingSystemMapper.SetOperatingSystemMapper(model.OperatingSystem),
                Orgeqpmanufacturer = OriginalEquipmentManufacturerMapper.SetOriginalEquipmentManufacturerMapper(model.OriginalEquipmentManufacturer),
                Description = model.Description,

                 
            };            
            return result;
        }
    }
}
