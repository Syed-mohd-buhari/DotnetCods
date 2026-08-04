using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public class LcmAncillaryDataMapper
    {
        public static LcmAncillaryData Get(Lcmancillarydata model)
        {
            if (model == null)
                return null;
            var result = new LcmAncillaryData()
            {
                LcmAncillaryDataId = model.Lcmancillarydataid,
                LcmEngineeringId =model.Lcmengineeringid,
                ProductCode =model.Productcode,
                HandedOverToOperation = model.Handedovertooperation,
                ContractRenewalPlan = model.Contractrenewalplan,
                ReasonForNoPlan = model.Reasonfornoplan,
                CommentOnProjectStatus = model.Commentonprojectstatus,
                ScopeOfSimplification = model.Scopeofsimplification,
                DataSource = model.Datasource,
                IncidentClass = model.Incidentclass,
                OccurenceProbability =model.Occurenceprobability,
                SecurityRiskEffective = model.Securityriskeffective,
                SecurityMitigation = model.Securitymitigation,
                AssetOutOfScope =model.Assetoutofscope ,
                IncludedInSecurityScanning = model.Includedinsecurityscanning,
                RaId = model.Raid,
                RequestId = model.Requestid,
                LastScanDate = model.Lastscandate,
                LastUpgradeDate = model.Lastupgradedate,
                EomControl = model.Eomcontrol,
                EngUpdateTracker = model.Engupdatetracker,
                OpsUpdateTracker = model.Opsupdatetracker,
               
                ExNetworks = model.Exnetworks,
               
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                OriginalHwLcmId = model.Originalhwlcmid,
                OriginalSwLcmId = model.Originalswlcmid,
                InfrastructureLocation = model.Locationinfrastructure,
                Vulnerabilityrating = model.Vulnerabilityrating,
                Cyberriskrequestid = model.Cyberriskrequestid,
                #region // Dev 719 Regulatory fields changes
                Ispecn =model.Ispecn,
                Ispecs = model.Ispecs,
                Isnof = model.Isnof,
                Isscf = model.Isscf,
                #endregion
                ExternalFacingFlag = model.Externalfacingflag,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                LcmEngineering = LCMEngineeringMapper.GetLcmEngineeringMapper(model.Lcmengineering),

                LastPenTestDate = model.Lastpentestdate != null ? model.Lastpentestdate : null,
                LastPenTestReferenceNumber = model.Lastpentestreferencenumber,
                LastScanRefNumber = model.Lastscanrefnumber,
                ExposedEdgeFlag = model.Isexposededge,
                RiskComment = model.Riskcomment,
                Qid = model.Qid
            };
            return result;
        }

        public static Lcmancillarydata Set(LcmAncillaryData model)
        {
            return new Lcmancillarydata()
            {

                Lcmancillarydataid = model.LcmAncillaryDataId,
                Lcmengineeringid = model.LcmEngineeringId,
                Productcode = model.ProductCode,
                Handedovertooperation = model.HandedOverToOperation,
                Contractrenewalplan = model.ContractRenewalPlan,
                Reasonfornoplan = model.ReasonForNoPlan,
                Commentonprojectstatus = model.CommentOnProjectStatus,
                Scopeofsimplification = model.ScopeOfSimplification,
                Datasource = model.DataSource,
                Incidentclass = model.IncidentClass,
                Occurenceprobability = model.OccurenceProbability,
                Securityriskeffective = model.SecurityRiskEffective,
                Securitymitigation = model.SecurityMitigation,
                Assetoutofscope =model.AssetOutOfScope,
                Includedinsecurityscanning = model.IncludedInSecurityScanning,
                Raid = model.RaId,
                Requestid = model.RequestId,
                Lastscandate = model.LastScanDate,
                Lastupgradedate = model.LastUpgradeDate,
                Eomcontrol = model.EomControl,
                Engupdatetracker = model.EngUpdateTracker,
                Opsupdatetracker = model.OpsUpdateTracker,

                Exnetworks = model.ExNetworks,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,                
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Originalswlcmid= model.OriginalSwLcmId,
                Originalhwlcmid = model.OriginalHwLcmId,
                Vulnerabilityrating = model.Vulnerabilityrating,
                Cyberriskrequestid = model.Cyberriskrequestid,
                Lcmengineering = LCMEngineeringMapper.SetLcmEngineeringMapper(model.LcmEngineering),
                #region // User story 719 Regulatory fields changes
                Ispecn = model.Ispecn,
                Ispecs = model.Ispecs,
                Isnof = model.Isnof,
                Isscf = model.Isscf,
                #endregion
                Externalfacingflag = model.ExternalFacingFlag,
                Locationinfrastructure = model.InfrastructureLocation,
                
                Lastpentestdate = model.LastPenTestDate != null ? model.LastPenTestDate : null,
                Lastpentestreferencenumber = model.LastPenTestReferenceNumber,
                Lastscanrefnumber = model.LastScanRefNumber,
                Isexposededge=model.ExposedEdgeFlag,
                Riskcomment = model.RiskComment,
                Qid = model.Qid


            };
        }
    }
}
