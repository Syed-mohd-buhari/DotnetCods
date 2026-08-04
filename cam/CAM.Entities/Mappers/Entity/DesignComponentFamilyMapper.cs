using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class DesignComponentFamilyMapper
    {
        public static DesignComponentFamily Get(Designcomponentfamilies model , bool include =true)
        {
            if (model == null)
                return null;
            var result = new DesignComponentFamily()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = include == true ? ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation) : null,
                ModificationUserEntity = include == true ? ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation) : null,
                SubNetworkBoundaryId = model.Subnetworkboundaryid,
                Description = model.Description,
                DesignComponentFamilyId = model.Designcomponentfamilyid,

                MajorHardwareOemId = model.Majorhardwareoemid,    
                ProductNameId = model.Productnameid,
                ProductNameNavigation = ProductNameMapper.GetProductNameMapper(model.Productname),
                MajorSoftwareOemId = model.Majorsoftwareoemid,
                PlatformId =model.Platformid,          
                SharingTypeId = model.Sharingtypeid,
                SystemIsShared =model.Systemisshared,
                SystemTypeIdentityName = model.Systemtypeidentityname,
                MajorHardwareOem = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(model.Majorhardwareoem),
                MajorSoftwareOem = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(model.Majorsoftwareoem),           
                SubNetworkBoundary = include? SubNetworkBoundaryMapper.GetSubNetworkBoundaryMapper(model.Subnetworkboundary) :null,
                SharingType =ShareTypeMapper.Get(model.Sharingtype),
                DesignComponents = model?.Designcomponents != null ? model?.Designcomponents.Select(p=> DesignComponentMapper.GetDesignComponentMapper(p,false)).ToList() : null,
                Serviceplandcfmapping = model?.Serviceplandcfmappings != null && model?.Serviceplandcfmappings.Count > 0 && include == true ?
                model?.Serviceplandcfmappings.Select(r => ServicePlanDcfMappingMapper.Get(r, false)).ToList() : null,
                Implementation = model.Implementation ?? false,
             
            };
            return result;
        }

        public static DesignComponentFamily GetDcfForReports(Designcomponentfamilies model, bool include = true)
        {
            if (model == null)
                return null;
            var result = new DesignComponentFamily()
            {
                DesignComponentFamilyId = model.Designcomponentfamilyid,
            };
            return result;
        }
        public static DesignComponentFamily GetWithCountrySpecificCirticality(Designcomponentfamilies model, bool include = true)
        {
            if (model == null)
                return null;
            var result = new DesignComponentFamily()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                SubNetworkBoundaryId = model.Subnetworkboundaryid,
                Description = model.Description,
                DesignComponentFamilyId = model.Designcomponentfamilyid,
                ProductNameId = model.Productnameid,
                ProductNameNavigation = ProductNameMapper.GetProductNameMapper(model.Productname),
                MajorHardwareOemId = model.Majorhardwareoemid,

                MajorSoftwareOemId = model.Majorsoftwareoemid,
                PlatformId = model.Platformid,
                // SecurityTireZoneId = model.Securitytirezoneid,
                SharingTypeId = model.Sharingtypeid,
                SystemIsShared = model.Systemisshared,
                SystemTypeIdentityName = model.Systemtypeidentityname,

                MajorHardwareOem = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(model.Majorhardwareoem),
                MajorSoftwareOem = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(model.Majorsoftwareoem),
                
                SubNetworkBoundary = include ? SubNetworkBoundaryMapper.GetSubNetworkBoundaryMapper(model.Subnetworkboundary) : null,
                SharingType = ShareTypeMapper.Get(model.Sharingtype),
                DesignComponents = model.Designcomponents.Select(p => DesignComponentMapper.GetDesignComponentMapper(p, false)).ToList(),
                Implementation = model.Implementation ?? false,
                 
            };
           
            //var swMajorDesingContact = model?.Designcomponents?.SelectMany(t => t.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts)
            //    ?.DistinctBy(x => x.Designcontactid)?.ToList();

            //if (swMajorDesingContact?.Any() == true)
            //{
            //    foreach (var item in swMajorDesingContact)
            //    {
            //        var tt = MajorSwBuidlsDesignContactsMapper.Get(item);
            //        result.MajorSoftWareBuidlsDesignContacts.Add(MajorSwBuidlsDesignContactsMapper.Get(item));
            //    }
            //}

            //var hardwareDesingContact = model?.Designcomponents?.SelectMany(t => t.Systemtype.Systemtypesmajorhardwarebuilds?.SelectMany(
            //    m => m.Majorhardware.Majorhwbuildsdesigncontacts)?.DistinctBy(x => x.Designcontactid)?.ToList())
            //    ?.DistinctBy(x => x.Designcontactid)?.ToList();


            //if (hardwareDesingContact?.Any() == true)
            //{
            //    foreach (var item in hardwareDesingContact)
            //    {
            //        result.MajorHardWareBuidlsDesignContacts.Add(MajorHwBuidlsDesignContactsMapper.Get(item));
            //    }
            //}
            return result;
        }
        public static int? ComputeCriticalityRating(bool? Pcisox, bool? C3C4, int? GDPRClassification, bool? InternetFacing,
           bool? MissionCritical, bool? SecurityElement, bool? CountrySpecificCriticality)
        {
            int CriticalityRating = 0;

            if (Pcisox == true)
            {
                CriticalityRating += 1;
            }
            if (C3C4 == true)
            {
                CriticalityRating += 1;
            }
            if (GDPRClassification == 2)
            {
                CriticalityRating += 1;
            }
            if (InternetFacing == true)
            {
                CriticalityRating += 1;
            }
            if (MissionCritical == true)
            {
                CriticalityRating += 1;
            }
            if (SecurityElement == true)
            {
                CriticalityRating += 1;
            }
            if (CountrySpecificCriticality == true)
            {
                CriticalityRating += 1;
            }

            return CriticalityRating;

        }
        public static Designcomponentfamilies Set(DesignComponentFamily model)
        {
            if (model == null)
                return null;
            var result = new Designcomponentfamilies()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Subnetworkboundaryid = model.SubNetworkBoundaryId,
                Description = model.Description,
                Designcomponentfamilyid = model.DesignComponentFamilyId,
                Majorhardwareoemid = model.MajorHardwareOemId,
                Majorsoftwareoemid = model.MajorSoftwareOemId,
                Platformid = model.PlatformId,
                //Securitytirezoneid = model.SecurityTireZoneId,
                Sharingtypeid = model.SharingTypeId,
                Systemisshared = model.SystemIsShared,
                Systemtypeidentityname = model.SystemTypeIdentityName,
                Implementation = model.Implementation,
               // Criticalassettype = CriticalAssetTypeMapper.Set(model.CriticalAssetType),
                Productnameid = model.ProductNameId,
                Productname = ProductNameMapper.SetProductNameMapper(model.ProductNameNavigation),
            };
            return result;
        }
    }
}
