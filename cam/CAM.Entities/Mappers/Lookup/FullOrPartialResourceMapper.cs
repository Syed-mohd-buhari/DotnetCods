using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class FullOrPartialResourceMapper
    {
        public static Models.Lookup.FullOrPartialResource GetFullOrPartialResourceMapper(Fullorpartialresource fullorpartialresource)
        {
            if (fullorpartialresource == null)
                return null;
            return new Models.Lookup.FullOrPartialResource()
            {
                Id= fullorpartialresource.Id,
                Description = fullorpartialresource.Description,
                CreationDate = fullorpartialresource.Creationdate,
                CreationUser = fullorpartialresource.Creationuser,
                ModificationDate = fullorpartialresource.Modificationdate,
                ModificationUser = fullorpartialresource.Modificationuser,
                Deleted = fullorpartialresource.Deleted.Value,
                DeletionDate = fullorpartialresource.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(fullorpartialresource.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(fullorpartialresource.ModificationuserNavigation),
                
            };
        }
        public static Fullorpartialresource SetfullorpartialresourceMapper(Models.Lookup.FullOrPartialResource fullorpartialresource)
        {
            if (fullorpartialresource == null)
                return null;
            return new Fullorpartialresource()
            {
                Id = fullorpartialresource.Id,
                Description = fullorpartialresource.Description,
                Creationdate = fullorpartialresource.CreationDate,
                Creationuser = fullorpartialresource.CreationUser,
                Modificationdate = fullorpartialresource.ModificationDate,
                Modificationuser = fullorpartialresource.ModificationUser,
                Deleted = fullorpartialresource.Deleted,
                Deletiondate = fullorpartialresource.DeletionDate,
            };
        }
    }
}
