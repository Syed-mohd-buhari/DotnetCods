using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Enum;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class MajorHardwareBuildMapper
    {
        public static MajorHardwareBuild GetMajorHardwareBuildMapper(Majorhardwarebuilds model)
        {
            if (model == null)
                return null;
            var result= new MajorHardwareBuild()
            {
                MajorHardwareId = model.Majorhardwareid,
                BuildConstructionId = model.Buildconstructionid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                EndOfMaintenance = model.Endofmaintenance,
                EOMStatus =(EOMEnum)model.Eomstatus,
                EndOfsupport =model.Endofsupport,
                HardwareSolution = model.Hardwaresolution,
                HardwareSolutionReourceId = model.Hardwaresolutionreourceid,
                HardwareType = model.Hardwaretype,
                LastTimeBuyExpansions = model.Lasttimebuyexpansions,
                LastTimeBuyNew = model.Lasttimebuynew,
                LastTimeBuyUpgrades =model.Lasttimebuyupgrades,
                OriginalEquipmentManufacturerId = model.Orgeqpmanufacturerid,
                OtherHardwareInfo = model.Otherhardwareinfo,
                PlatformId = model.Platformid,
                ProprietaryHardware =model.Proprietaryhardware,
                SpareFieldsJson = model.Sparefieldsjson,
                TypeOfProcessor = model.Typeofprocessor,
                VulnerabilityStatus = model.Vulnerabilitystatus,
                OriginalEquipmentManufacturer = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(model.Orgeqpmanufacturer),
                BuildConstruction = BuildConstructionMapper.GetBuildConstructionMapper(model.Buildconstruction),
                HardwareSolutionResource = HardwareSolutionResourceMapper.GetHardwareSolutionResourceMapper(model.Hardwaresolutionreource),
                Platform = PlatFormMapper.GetPlatFormMapper(model.Platform),
                OperatingSystem = model.Operatingsystem,
                GeneraAvailableDate = model.Generaavailabledate,
                Description = model.Description
            };

            result.SystemTypesMajorHardwareBuilds = model.Systemtypesmajorhardwarebuilds.Select(p => SystemTypesMajorHardwareBuildMapper.GetSystemTypesMajorHardwareBuildMapper(p,false)).ToList();
            result.MajorHwBuidlsDesignContacts = model.Majorhwbuildsdesigncontacts.Select(p => MajorHwBuidlsDesignContactsMapper.Get(p, false)).ToList();

            //if (model.Majorhwbuildsdesigncontacts != null)
            //{
            //    foreach (var item in model.Majorhwbuildsdesigncontacts)
            //    {
            //        result.MajorHwBuidlsDesignContacts.Add(MajorHwBuidlsDesignContactsMapper.Get(item,false));
            //    }
            //}
           
            return result;
        }
        public static Majorhardwarebuilds SetMajorHardwareBuildMapper(MajorHardwareBuild model)
        {
            if(model == null)
                return null;

            var result = new Majorhardwarebuilds()
            {
                Majorhardwareid = model.MajorHardwareId,
                Buildconstructionid = model.BuildConstructionId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Endofmaintenance = model.EndOfMaintenance,
                Eomstatus = (short)model.EOMStatus,
                Endofsupport = model.EndOfsupport,
                Hardwaresolution = model.HardwareSolution,
                Hardwaresolutionreourceid = model.HardwareSolutionReourceId,
                Hardwaretype = model.HardwareType,
                Lasttimebuyexpansions = model.LastTimeBuyExpansions,
                Lasttimebuynew = model.LastTimeBuyNew,
                Lasttimebuyupgrades = model.LastTimeBuyUpgrades,
                Orgeqpmanufacturerid= model.OriginalEquipmentManufacturerId,
                Otherhardwareinfo = model.OtherHardwareInfo,
                Platformid = model.PlatformId,
                Proprietaryhardware = model.ProprietaryHardware,
                Sparefieldsjson = model.SpareFieldsJson,
                Typeofprocessor = model.TypeOfProcessor,
                Vulnerabilitystatus = model.VulnerabilityStatus,
                Orgeqpmanufacturer = OriginalEquipmentManufacturerMapper.SetOriginalEquipmentManufacturerMapper(model.OriginalEquipmentManufacturer),
                Buildconstruction = BuildConstructionMapper.SetBuildConstructionMapper(model.BuildConstruction),
                Hardwaresolutionreource = HardwareSolutionResourceMapper.SetHardwareSolutionResourceMapper(model.HardwareSolutionResource),
                Platform = PlatFormMapper.SetPlatFormMapper(model.Platform),
                Generaavailabledate = model.GeneraAvailableDate,
                Description = model.Description
            };

            if (model.MajorHwBuidlsDesignContacts != null)
            {
                foreach(var item in model.MajorHwBuidlsDesignContacts)
                {
                    result.Majorhwbuildsdesigncontacts.Add(MajorHwBuidlsDesignContactsMapper.Set(item));
                }
            }

            return result;
        }
    }
}
