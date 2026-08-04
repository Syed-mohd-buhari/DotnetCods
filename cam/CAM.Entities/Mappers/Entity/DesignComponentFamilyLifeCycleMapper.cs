using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using IdentityServer4.Events;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Mappers.Entity
{
    public class DesignComponentFamilyLifeCycleMapper
    {
        public static DesignComponentFamilyLifeCycle Get(Dcflifecycle model)
        {
            if (model == null)
                return null;
            return new DesignComponentFamilyLifeCycle()
            {
                DcfLifeCycleId = model.Dcflifecycleid,
                ResourceKey = model.Resourcekey,
                PreviousResourceKey = model.Previousresourcekey,
                OpCoId = model.Opcoid,
                DcfId = model.Dcfid,
                DcId = model.Dcid,
                EventName = model.EventName,
                EventId = model.EventId,
                Notes = model.Notes,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),               
                CurrentDetails=model.Currentdetails, 
                PlannedDetails = model.Planneddetails,
                OpcoDescription = model.Opcodescription,
                DcfDescription = model.Dcfdescription,
                CategoryType = model.Categorytype,
                BagName = model.Bagname ,
                DcDescription = model.Dcdescription
            };
        }
        public static Dcflifecycle  Set(DesignComponentFamilyLifeCycle model)
        {
            return new Dcflifecycle{
                Dcflifecycleid = model.DcfLifeCycleId,
                Dcid=model.DcId,
                Dcfid=model.DcfId,
                EventName=model.EventName,
                Notes=model.Notes,
                EventId =(short)model.EventId,
                Creationdate= model.CreationDate,
                Creationuser=model.CreationUser,
                Modificationdate=model.ModificationDate,
                Modificationuser=model.ModificationUser,
                Deleted=model.Deleted,
                Deletiondate= model.DeletionDate,
                Currentdetails=model.CurrentDetails,
                Resourcekey=model.ResourceKey,
                Previousresourcekey=model.PreviousResourceKey,
                Opcoid=model.OpCoId,
                Planneddetails = model.PlannedDetails,
                Opcodescription = model.OpcoDescription,
                Dcfdescription = model.DcfDescription,               
                Categorytype = model.CategoryType,
                Bagname = model.BagName,
                Dcdescription = model.DcfDescription

            };
        }
    }
}
