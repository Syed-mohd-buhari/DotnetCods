using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class RiskMapper
    {
        public static RiskResource GetRiskMapper(Risk risk)
        {
            if (risk == null)
                return null;
            return new RiskResource()
            {
                RiskId = risk.Riskid,
                RiskDescription = risk.Description,
                CreationDate = risk.Creationdate,
                CreationUser = risk.Creationuser,
                ModificationDate = risk.Modificationdate,
                ModificationUser = risk.Modificationuser,
                Deleted = risk.Deleted.Value,
                DeletionDate = risk.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(risk.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(risk.ModificationuserNavigation),
                Severity = risk.Severity,
                
            };
        }
        public static Risk SetRiskMapper(RiskResource risk)
        {
            return new Risk()
            {
                Riskid = risk.RiskId,
                Description = risk.RiskDescription,
                Creationdate = risk.CreationDate,
                Creationuser = risk.CreationUser,
                Modificationdate = risk.ModificationDate,
                Modificationuser = risk.ModificationUser,
                Deleted = risk.Deleted,
                Deletiondate = risk.DeletionDate,
                Severity = risk.Severity,
            };
        }
    }
}
