using CAM.Entities.Mappers.Identity;
using CAM.Entities.Models.Lookup;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.Entities.Mappers.Lookup
{
    public static class OperationalContractMapper
    {
        public static OperationalContract GetOperationalContractMapper(Operationalcontracts Operationalcontract)
        {
            if (Operationalcontract == null)
                return null;
            return new OperationalContract()
            {
                Id = Operationalcontract.Id,
                Description = Operationalcontract.Description,
                CreationDate = Operationalcontract.Creationdate,
                CreationUser = Operationalcontract.Creationuser,
                ModificationDate = Operationalcontract.Modificationdate,
                ModificationUser = Operationalcontract.Modificationuser,
                Deleted = Operationalcontract.Deleted.Value,
                DeletionDate = Operationalcontract.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Operationalcontract.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(Operationalcontract.ModificationuserNavigation),

            };
        }

        public static Operationalcontracts SetOperationalContractMapper(OperationalContract Operationalcontract)
        {
            return new Operationalcontracts()
            {
                Id = Operationalcontract.Id,
                Description = Operationalcontract.Description,
                Creationdate = Operationalcontract.CreationDate,
                Creationuser = Operationalcontract.CreationUser,
                Modificationdate = Operationalcontract.ModificationDate,
                Modificationuser = Operationalcontract.ModificationUser,
                Deleted = Operationalcontract.Deleted,
                Deletiondate = Operationalcontract.DeletionDate,
            };
        }
    }
}
