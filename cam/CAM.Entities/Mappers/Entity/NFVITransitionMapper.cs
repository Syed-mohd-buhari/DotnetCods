using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;


namespace CAM.Entities.Mappers.Entity
{
    public static class NFVITransitionMapper
    {
        public static NFVITransition GetNFVITransitionMapper(Nfvitransitions model)
        {
            if (model == null)
                return null;
            return new NFVITransition()
            {
                NFVITransitionId = model.Nfvitransitionid,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                NextStep = model.Nextstep,
                NFVISiteDesignation = model.Nfvisitedesignation,
                OpCoId = model.Opcoid,
                Spare1Json = model.Spare1json,
                Status12KSwitchId = model.Status12kswitchid,
                StatusLabMCId = model.Statuslabmcid,
                StatusLabSCId =model.Statuslabscid,
                StatusLiveMCId = model.Statuslivemcid,
                StatusLiveSCId = model.Statuslivescid,
                NFVIStatuses12KSwitch = NFVIStatusMapper.GetNFVIStatusMapper(model.Status12kswitch),
                NFVIStatusesLabMC = NFVIStatusMapper.GetNFVIStatusMapper(model.Statuslabmc),
                NFVIStatusesLabSC = NFVIStatusMapper.GetNFVIStatusMapper(model.Statuslabsc),
                NFVIStatusesLiveMC = NFVIStatusMapper.GetNFVIStatusMapper(model.Statuslivemc),
                NFVIStatusesLiveSC = NFVIStatusMapper.GetNFVIStatusMapper(model.Statuslivesc),
                OpCo = OpCoMapper.GetOpCoMapper(model.Opco),
                
            };
        }
        public static Nfvitransitions SetNFVITransitionMapper(NFVITransition model)
        {
            return new Nfvitransitions()
            {
                Nfvitransitionid = model.NFVITransitionId,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Nextstep = model.NextStep,
                Nfvisitedesignation = model.NFVISiteDesignation,
                Opcoid = model.OpCoId,
                Spare1json = model.Spare1Json,
                Status12kswitchid = model.Status12KSwitchId,
                Statuslabmcid = model.StatusLabMCId,
                Statuslabscid = model.StatusLabSCId,
                Statuslivemcid = model.StatusLiveMCId,
                Statuslivescid = model.StatusLiveSCId,

            };
        }
    }
}
