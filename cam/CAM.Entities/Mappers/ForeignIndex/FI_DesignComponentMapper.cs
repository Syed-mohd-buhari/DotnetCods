using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.ForeignIndex;
using CAM.Enum;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.ForeignIndex
{

    public static class FI_DesignComponentMapper
    {
        public static FI_DesignComponent Get(FiDesigncomponents model)
        {
            if (model == null)
                return null;
            return new FI_DesignComponent()
            {
                SessionId = model.Sessionid,
                Description = model.Description,
                DesignComponentId = model.Designcomponentid,
                FI_DesignComponentId = model.FiDesigncomponentid,
                ToDelete = model.Todelete,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),




            };
        }
        public static FiDesigncomponents Set(FI_DesignComponent model)
        {
            return new FiDesigncomponents()
            {
                Sessionid = model.SessionId,
                Description = model.Description,
                Designcomponentid = model.DesignComponentId,
                FiDesigncomponentid = model.FI_DesignComponentId,
                Todelete = model.ToDelete,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
            };
        }
    }
}