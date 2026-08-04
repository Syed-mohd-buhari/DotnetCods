using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.ForeignIndex;
using CAM.Enum;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.ForeignIndex
{
  
    public static class FI_SessionMapper 
    {
        public static FI_Session Get(FiSessions model)
        {
            if (model == null)
                return null;
            return new FI_Session()
            {
                SessionId = model.Sessionid,
                Source = (ForeignIndexSource)model.Source,
                Status = (ForeignIndexStatus)model.Status,
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
        public static FiSessions Set(FI_Session model)
        {
            return new FiSessions()
            {
                Sessionid = model.SessionId,
                Source = (int)model.Source,
                Status = (int)model.Status,
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