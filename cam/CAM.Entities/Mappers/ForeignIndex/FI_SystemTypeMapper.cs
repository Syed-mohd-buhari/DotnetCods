using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.ForeignIndex;
using CAM.Enum;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.ForeignIndex
{

    public static class FI_SystemTypeMapper
    {
        public static FI_SystemTypes Get(FiSystemtypes model)
        {
            if (model == null)
                return null;
            return new FI_SystemTypes()
            {
                SessionId = model.Sessionid,
                Description = model.Description,
                DesignComponentId = model.Designcomponentid,
                FI_SystemTypesId = model.FiSystemtypesid,
                SystemTypesId = model.Systemtypesid,
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
        public static FiSystemtypes Set(FI_SystemTypes model)
        {
            return new FiSystemtypes()
            {
                Sessionid = model.SessionId,
                Description = model.Description,
                Designcomponentid = model.DesignComponentId,
                FiSystemtypesid = model.FI_SystemTypesId,
                Systemtypesid = model.SystemTypesId,
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