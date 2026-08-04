using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class HardwareSolutionResourceMapper
    {
        public static Models.Lookup.HardwareSolutionResource GetHardwareSolutionResourceMapper(Hardwaresolutionresource HardwareSolutionResource)
        {
            if (HardwareSolutionResource == null)
                return null;
            return new Models.Lookup.HardwareSolutionResource()
            {
                HardwareSolutionResourceId= HardwareSolutionResource.Hardwaresolutionresourceid,
                HardwareSolutionResourceDescription = HardwareSolutionResource.Hardwaresolutionreource,
                CreationDate = HardwareSolutionResource.Creationdate,
                CreationUser = HardwareSolutionResource.Creationuser,
                ModificationDate = HardwareSolutionResource.Modificationdate,
                ModificationUser = HardwareSolutionResource.Modificationuser,
                Deleted = HardwareSolutionResource.Deleted.Value,
                DeletionDate = HardwareSolutionResource.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(HardwareSolutionResource.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(HardwareSolutionResource.ModificationuserNavigation),
                
            };
        }
        public static Hardwaresolutionresource SetHardwareSolutionResourceMapper(Models.Lookup.HardwareSolutionResource HardwareSolutionResource)
        {
            if (HardwareSolutionResource == null)
                return null;
            return new Hardwaresolutionresource()
            {
                Hardwaresolutionresourceid = HardwareSolutionResource.HardwareSolutionResourceId,
                Hardwaresolutionreource = HardwareSolutionResource.HardwareSolutionResourceDescription,
                Creationdate = HardwareSolutionResource.CreationDate,
                Creationuser = HardwareSolutionResource.CreationUser,
                Modificationdate = HardwareSolutionResource.ModificationDate,
                Modificationuser = HardwareSolutionResource.ModificationUser,
                Deleted = HardwareSolutionResource.Deleted,
                Deletiondate = HardwareSolutionResource.DeletionDate,
            };
        }
    }
}
