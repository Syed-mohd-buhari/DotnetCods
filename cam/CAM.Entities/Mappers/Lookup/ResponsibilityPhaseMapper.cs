using CAM.Entities.Mappers.Identity;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class ResponsibilityPhaseMapper
    {
        public static Models.Lookup.ResponsibilityPhase GetResponsibilityPhaseMapper(Responsibilityphases ResponsibilityPhase)
        {

            if (ResponsibilityPhase == null)
                return null;
            return new Models.Lookup.ResponsibilityPhase()
            {
                ResponsibilityPhaseId = ResponsibilityPhase.Responsibilityphaseid,
                ResponsibilityPhaseDescription = ResponsibilityPhase.Responsibilityphase,
                CreationDate = ResponsibilityPhase.Creationdate,
                CreationUser = ResponsibilityPhase.Creationuser,
                ModificationDate = ResponsibilityPhase.Modificationdate,
                ModificationUser = ResponsibilityPhase.Modificationuser,
                Deleted = ResponsibilityPhase.Deleted.Value,
                DeletionDate = ResponsibilityPhase.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(ResponsibilityPhase.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(ResponsibilityPhase.ModificationuserNavigation),
                Rule = ResponsibilityPhase.Rule,
                
            };
        }
        public static Responsibilityphases SetResponsibilityPhaseMapper(Models.Lookup.ResponsibilityPhase ResponsibilityPhase)
        {
            return new Responsibilityphases()
            {
                Responsibilityphaseid = ResponsibilityPhase.ResponsibilityPhaseId,
                Responsibilityphase = ResponsibilityPhase.ResponsibilityPhaseDescription,
                Creationdate = ResponsibilityPhase.CreationDate,
                Creationuser = ResponsibilityPhase.CreationUser,
                Modificationdate = ResponsibilityPhase.ModificationDate,
                Modificationuser = ResponsibilityPhase.ModificationUser,
                Deleted = ResponsibilityPhase.Deleted,
                Deletiondate = ResponsibilityPhase.DeletionDate,
                Rule = ResponsibilityPhase.Rule,
                
            };
        }
    }
}
