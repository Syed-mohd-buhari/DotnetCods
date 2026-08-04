using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class SupportedResourceMapper
    {
        public static Models.Lookup.SupportedResource GetSupportedResourceMapper(Supportedresource SupportedResource)
        {

            if (SupportedResource == null)
                return null;
            return new Models.Lookup.SupportedResource()
            {
                Id= SupportedResource.Id,
                Description = SupportedResource.Description,
                CreationDate = SupportedResource.Creationdate,
                CreationUser = SupportedResource.Creationuser,
                ModificationDate = SupportedResource.Modificationdate,
                ModificationUser = SupportedResource.Modificationuser,
                Deleted = SupportedResource.Deleted.Value,
                DeletionDate = SupportedResource.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(SupportedResource.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(SupportedResource.ModificationuserNavigation),
                Rule = SupportedResource.Rule,
               
                
            };
        }
        public static Supportedresource SetSupportedResourceMapper(Models.Lookup.SupportedResource SupportedResource)
        {
            if (SupportedResource == null)
                return null;
            return new Supportedresource()
            {
                Id = SupportedResource.Id,
                Description = SupportedResource.Description,
                Creationdate = SupportedResource.CreationDate,
                Creationuser = SupportedResource.CreationUser,
                Modificationdate = SupportedResource.ModificationDate,
                Modificationuser = SupportedResource.ModificationUser,
                Deleted = SupportedResource.Deleted,
                Deletiondate = SupportedResource.DeletionDate,
                Rule = SupportedResource.Rule,
            };
        }
    }
}
