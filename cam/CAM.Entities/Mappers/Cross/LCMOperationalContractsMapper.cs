using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Cross;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Cross
{
    public static class LCMOperationalContractsMapper
    {
        public static LcmOperationalContracts Get(Lcmoperationalcontracts model)
        {

            if (model == null)
                return null;
            return new LcmOperationalContracts()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                LcmId = model.Lcmid,
                OperationalContractId = model.Operationalcontractid,              
                Id = model.Id,
               // LcmEngineering = LCMEngineeringMapper.GetLcmEngineeringMapper(model.Lcm),
                OperationalContract = OperationalContractMapper.GetOperationalContractMapper(model.Operationalcontract),

            };
        }

        public static Lcmoperationalcontracts Set(LcmOperationalContracts model)
        {
            return new Lcmoperationalcontracts()
            {

                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Operationalcontractid = model.OperationalContractId,
                Lcmid = model.LcmId,
                Id = model.Id,

            };
        }
    }
}
