using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Lookup
{
    public static class FeedBackLoopAuditMapper
    {
        public static FeedBackLoopAudits Get(Feedbackloopaudits model)
        {
            if (model == null)
                return null;
            var result = new FeedBackLoopAudits()
            {
                FeedBackLoopAuditId = model.Feedbackloopauditid,
                OpCo = model.Opco,
                Oem= model.Oem,
                ProcessStartTime = model.Processstarttime,
                ProcessEndTime = model.Processendtime,
                FileCount = model.Filecount,
                NodeType = model.Nodetype,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                //CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                //ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
            };
            if(model.Xmlparserun != null)
            {
                foreach(var item in model.Xmlparserun)
                {
                    result.XmlParseRun.Add(XmlParseRunMapper.Get(item));
                }
            }

            return result;
        }      
    }
}
