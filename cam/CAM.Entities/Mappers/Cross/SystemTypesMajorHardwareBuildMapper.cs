using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cross
{
    public static class SystemTypesMajorHardwareBuildMapper
    {
        public static SystemTypesMajorHardwareBuild GetSystemTypesMajorHardwareBuildMapper(Systemtypesmajorhardwarebuilds model , bool include = true)
        {
            if (model == null)
                return null;
            return new SystemTypesMajorHardwareBuild()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                IsMain = model.Ismain,
                MajorHardwareId =model.Majorhardwareid,
                SystemTypeId = model.Systemtypeid,
                //SystemType = SystemTypeMapper.GetSystemTypeMapper(model.Systemtype),
                MajorHardware =include ? MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(model.Majorhardware) : null,
                

            };
        }
        public static Systemtypesmajorhardwarebuilds SetSystemTypesMajorHardwareBuildMapper(SystemTypesMajorHardwareBuild model)
        {
            if (model == null)
                return null;
            return new Systemtypesmajorhardwarebuilds()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Ismain = model.IsMain,
                Majorhardwareid = model.MajorHardwareId,
                Systemtypeid = model.SystemTypeId
            };
        }
    }
}
