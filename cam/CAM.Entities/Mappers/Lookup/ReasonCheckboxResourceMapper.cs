using CAM.Entities.Mappers.Identity;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class ReasonCheckboxResourceMapper
    {
        public static Models.Lookup.ReasonCheckboxResource GetReasonCheckboxResourceMapper(Reasoncheckboxresources ReasonCheckboxResource)
        {

            if (ReasonCheckboxResource == null)
                return null;
            return new Models.Lookup.ReasonCheckboxResource()
            {
                Id = ReasonCheckboxResource.Id,
                Description = ReasonCheckboxResource.Description,
                CreationDate = ReasonCheckboxResource.Creationdate,
                CreationUser = ReasonCheckboxResource.Creationuser,
                ModificationDate = ReasonCheckboxResource.Modificationdate,
                ModificationUser = ReasonCheckboxResource.Modificationuser,
                Deleted = ReasonCheckboxResource.Deleted.Value,
                DeletionDate = ReasonCheckboxResource.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(ReasonCheckboxResource.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(ReasonCheckboxResource.ModificationuserNavigation),
                IsHardware = ReasonCheckboxResource.Ishardware,
                IsSoftware = ReasonCheckboxResource.Issoftware,
                

            };
        }
        public static Reasoncheckboxresources SetReasonCheckboxResourceMapper(Models.Lookup.ReasonCheckboxResource ReasonCheckboxResource)
        {
            return new Reasoncheckboxresources()
            {
                Id = ReasonCheckboxResource.Id,
                Description = ReasonCheckboxResource.Description,
                Creationdate = ReasonCheckboxResource.CreationDate,
                Creationuser = ReasonCheckboxResource.CreationUser,
                Modificationdate = ReasonCheckboxResource.ModificationDate,
                Modificationuser = ReasonCheckboxResource.ModificationUser,
                Deleted = ReasonCheckboxResource.Deleted,
                Deletiondate = ReasonCheckboxResource.DeletionDate,
                Ishardware = ReasonCheckboxResource.IsHardware,
                Issoftware = ReasonCheckboxResource.IsSoftware,
            };
        }
    }
}
