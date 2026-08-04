using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class DaPlannedActivityDcfMapper
    {
        public static DaPlannedActivityDcf GetDaPlannedActivityDcfMapper(Daplannedactivitydcf daplannedactivitydcf)
        {
            if (daplannedactivitydcf == null)
                return null;
            return new DaPlannedActivityDcf()
            {
                DaPlannedActivityDcfId = daplannedactivitydcf.Daplannedactivitydcfid,
                PlannedActivityId = daplannedactivitydcf.Plannedactivityid,
                DesignComponentFamilyId = daplannedactivitydcf.Designcomponentfamilyid,
                DcfStatus = daplannedactivitydcf.Dcfstatus,
                CreationDate = daplannedactivitydcf.Creationdate,
                CreationUser = daplannedactivitydcf.Creationuser,
                ModificationDate = daplannedactivitydcf.Modificationdate,
                ModificationUser = daplannedactivitydcf.Modificationuser,
                Deleted = daplannedactivitydcf.Deleted.Value,
                DeletionDate = daplannedactivitydcf.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(daplannedactivitydcf.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(daplannedactivitydcf.ModificationuserNavigation),
                DesignComponentFamily = DesignComponentFamilyMapper.Get(daplannedactivitydcf.Designcomponentfamily),
                PlannedActivity = PlannedActivityMapper.Get(daplannedactivitydcf.Plannedactivity)
            };
        }
        public static Daplannedactivitydcf SetDaPlannedActivityDcfMapper(DaPlannedActivityDcf daPlannedActivityDcf)
        {
            return new Daplannedactivitydcf()
            {
                Daplannedactivitydcfid = daPlannedActivityDcf.DaPlannedActivityDcfId,
                Plannedactivityid = daPlannedActivityDcf.PlannedActivityId,
                Designcomponentfamilyid = daPlannedActivityDcf.DaPlannedActivityDcfId,
                Dcfstatus = daPlannedActivityDcf.DcfStatus,
                Creationdate = daPlannedActivityDcf.CreationDate,
                Creationuser = daPlannedActivityDcf.CreationUser,
                Modificationdate = daPlannedActivityDcf.ModificationDate,
                Modificationuser = daPlannedActivityDcf.ModificationUser
            };
        }
    }
}
