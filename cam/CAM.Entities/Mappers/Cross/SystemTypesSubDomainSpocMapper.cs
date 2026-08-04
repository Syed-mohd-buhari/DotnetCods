using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Cross
{
    public static class SystemTypesSubDomainSpocMapper
    {
        public static SystemTypesSubDomainSpoc GetSystemTypesSubDomainSpocMapper(Systemtypessubdomainspoc model)
        {
            if (model == null)
                return null;
            return new SystemTypesSubDomainSpoc()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                SubDomainSpocId = model.Subdomainspocid,
                SystemTypeId =model.Systemtypeid,
                SystemTypesSubDomainSpocId = model.Systemtypessubdomainspocid,
                SubDomainSpoc = SubDomainSpocMapper.GetSubDomainSpocMapper(model.Subdomainspoc),



            };
        }
        public static Systemtypessubdomainspoc SetSystemTypesSubDomainSpocMapper(SystemTypesSubDomainSpoc model)
        {
            if (model == null)
                return null;
            return new Systemtypessubdomainspoc()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Subdomainspocid = model.SubDomainSpocId,
                Systemtypeid = model.SystemTypeId,
                Systemtypessubdomainspocid = model.SystemTypesSubDomainSpocId,

            };
        }
    }
}
