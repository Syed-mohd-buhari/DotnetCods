using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Engine;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class GridCustomColumnMapper
    {
        public static GridCustomColumn Get(Gridcustomcolumn model)
        {
            if (model == null)
                return null;
            return new GridCustomColumn()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                ClassName = model.Classname,
                GridCustomColumnId = model.Gridcustomcolumnid,
                JsonGridCustomizationData = model.Jsongridcustomizationdata,
                UserId = model.Userid,
                Preferencedate = model.Preferencedate,
                MessagingDate = model.Messagingdate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                User = ApplicationUserMapper.GetApplicationUserMapper(model.User),
                
            };
        }

        public static Gridcustomcolumn Set(GridCustomColumn model)
        {
            return new Gridcustomcolumn()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Classname = model.ClassName,
                Gridcustomcolumnid = model.GridCustomColumnId,
                Jsongridcustomizationdata = model.JsonGridCustomizationData,
                Userid = model.UserId,
                Preferencedate = model.Preferencedate,
                Messagingdate= model.MessagingDate
              
            };
        }


    }
}
