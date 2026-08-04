using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class XmlParseRunMapper
    {
        public static XmlParseRun Get(Xmlparserun model)
        {
            if (model == null)
                return null;
            var result =  new XmlParseRun()
            {
                Xmlparserunid = model.Xmlparserunid,
                Filename = model.Filename,
                FileProcessStartTime = model.Fileprocessstarttime,
                FileprocessEndTime = model.Fileprocessendtime,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                  
            };
            if (model.Nodeparsehistory != null)
            {
                foreach (var item in model.Nodeparsehistory)
                {
                    result.NodeParseHistory.Add(NodeParseHistoryMapper.Get(item));
                }
            }
            return result;
        }      
    }
}
