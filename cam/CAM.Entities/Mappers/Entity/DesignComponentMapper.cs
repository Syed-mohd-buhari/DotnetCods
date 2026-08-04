using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
using System.Linq;

namespace CAM.Entities.Mappers.Entity
{
    public static class DesignComponentMapper
    {
        public static DesignComponent GetDesignComponentMapper(Designcomponents model , bool Include =true)
        {
            if (model == null)
                return null;
            return new DesignComponent()
            {
                DesignComponentId = model.Designcomponentid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                GdprRelevant =model.Gdprrelevant,
                SubNetworkBoundaryId =model.Subnetworkboundaryid,
                SystemTypeId = model.Systemtypeid,
                SubNetworkBoundary = SubNetworkBoundaryMapper.GetSubNetworkBoundaryMapper(model.Subnetworkboundary,false),
                SystemType  = SystemTypeMapper.GetSystemTypeMapper(model.Systemtype , false),
                DesignComponentFamilyId = model.Designcomponentfamilyid,
                DesignComponentFamily = Include? DesignComponentFamilyMapper.Get(model.Designcomponentfamily) : null,
                Lcmengineerings = model.Lcmengineering.Select(p => LCMEngineeringMapper.GetLcmEngineeringMapper(p,false)).ToList(),
                PlannedActivities = model.Plannedactivities.Select(p => PlannedActivityMapper.Get(p,false)).ToList(),
                Networkelementsasplanned = model.Networkelementsasplanned.Select(p => NetworkElementAsPlannedMapper.Get(p, false)).ToList(),
                //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
                VisibleFlag = model.Visibleflag
            };
        }
        public static Designcomponents SetDesignComponentMapper(DesignComponent model)
        {
            if (model == null)
                return null;
            return new Designcomponents()
            {
                Designcomponentid = model.DesignComponentId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Gdprrelevant = model.GdprRelevant,
                Subnetworkboundaryid = model.SubNetworkBoundaryId,
                Systemtypeid = model.SystemTypeId,
                Systemtype = SystemTypeMapper.SetSystemTypeMapper(model.SystemType),                
                Designcomponentfamilyid = model.DesignComponentFamilyId,
                Visibleflag = model.VisibleFlag
            };
        }
        public static Designcomponents SetDcForPlatformMigrationPA(long dcId)
        {
                       
            return new Designcomponents()
            {
                Designcomponentid = dcId

            };
        }
    }
}
