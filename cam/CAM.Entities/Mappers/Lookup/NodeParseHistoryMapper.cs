using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class NodeParseHistoryMapper
    {
        public static NodeParseHistory Get(Nodeparsehistory model)
        {
            if (model == null)
                return null;
            return new NodeParseHistory()
            {
                NodeParseHistoryId = model.Nodeparsehistoryid,
                XmlParseRunId = model.Xmlparserunid,
                OpCo = model.Opco,
                Oem= model.Oem,
                ElementName = model.Elementname,
                ParseType = model.Parsetype,
                UpdatedRows = model.Updatedrows,
                Status = model.Status,
                NodeProcessStartTime = model.Nodeprocessstarttime,
                NodeProcessEndTime = model.Nodeprocessendtime,
                NodeType = model.Nodetype,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                XmlParseRun = XmlParseRunMapper.Get(model.Xmlparserun)
            };
        }      
    }
}
