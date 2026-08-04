using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Enum;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class MajorHardwareBuildAsIsMapper
    {
        public static MajorHardwareBuildAsIs GetMajorHardwareBuildAsIsMapper(Majorhardwarebuildasis model)
        {
            if (model == null)
                return null;
            var result= new MajorHardwareBuildAsIs()
            {
                MajorHardwareBuildAsIsId = model.Majorhardwarebuildasisid,
                BuildConstructionId = model.Buildconstructionid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),               
                HardwareSolution = model.Hardwaresolution,
                HardwareSolutionReourceId = model.Hardwaresolutionreourceid,
                HardwareType = model.Hardwaretype,               
                OriginalEquipmentManufacturerId = model.Orgeqpmanufacturerid,               
                PlatformId = model.Platformid,              
                OriginalEquipmentManufacturer = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(model.Orgeqpmanufacturer),
                BuildConstruction = BuildConstructionMapper.GetBuildConstructionMapper(model.Buildconstruction),
                HardwareSolutionResource = HardwareSolutionResourceMapper.GetHardwareSolutionResourceMapper(model.Hardwaresolutionreource),
                Platform = PlatFormMapper.GetPlatFormMapper(model.Platform),
              
            };
         
           
            return result;
        }
        public static Majorhardwarebuildasis SeMajorHardwareBuildAsIsMapper(MajorHardwareBuildAsIs model)
        {
            if(model == null)
                return null;

            var result = new Majorhardwarebuildasis()
            {
                Majorhardwarebuildasisid = model.MajorHardwareBuildAsIsId,
                Buildconstructionid = model.BuildConstructionId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                
                Hardwaresolution = model.HardwareSolution,
                Hardwaresolutionreourceid = model.HardwareSolutionReourceId,
                Hardwaretype = model.HardwareType,
               
                Orgeqpmanufacturerid= model.OriginalEquipmentManufacturerId,
                
                Platformid = model.PlatformId,
                
                Orgeqpmanufacturer = OriginalEquipmentManufacturerMapper.SetOriginalEquipmentManufacturerMapper(model.OriginalEquipmentManufacturer),
                Buildconstruction = BuildConstructionMapper.SetBuildConstructionMapper(model.BuildConstruction),
                Hardwaresolutionreource = HardwareSolutionResourceMapper.SetHardwareSolutionResourceMapper(model.HardwareSolutionResource),
                Platform = PlatFormMapper.SetPlatFormMapper(model.Platform),
               
            };
 

            return result;
        }
    }
}
