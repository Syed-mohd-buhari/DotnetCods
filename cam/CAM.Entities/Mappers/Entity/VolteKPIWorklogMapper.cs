using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;

namespace CAM.Entities.Mappers.Entity
{
    public static class VolteKPIWorklogMapper
    {
        public static VolteKPIWorklog Get(Voltekpiworklog model)
        {
            if (model == null)
                return null;
            return new VolteKPIWorklog()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                OpCoId = model.Opcoid,
                Month = model.Month,
                VolteKPIId = model.Voltekpiid,
                Year = model.Year,
                ActualMonthlyValueNew = model.Actualmonthlyvaluenew,
                ActualMonthlyValueOld = model.Actualmonthlyvalueold,
                ActualNumberOfProvisionedNew = model.Actualnumberofprovisionednew,
                ActualNumberOfProvisionedOld = model.Actualnumberofprovisionedold,
                ActualNumberOfRegisteredNew = model.Actualnumberofregisterednew,
                ActualNumberOfRegisteredOld = model.Actualnumberofregisteredold,
                Approved = model.Approved,
                Comments = model.Comments,
                EoyTargetNew = model.Eoytargetnew,
                EoyTargetOld = model.Eoytargetold,
                IsStored = model.Isstored,
                TargetMonthlyValueNew = model.Targetmonthlyvaluenew,
                TargetMonthlyValueOld = model.Targetmonthlyvalueold,
                TargetMonthlyValueProposed = model.Targetmonthlyvalueproposed,
                VolteKPIType = model.Voltekpitype,
                VolteKPIWorklogId = model.Voltekpiworklogid,
              
                OpCo = OpCoMapper.GetOpCoMapper(model.Opco),


            };
        }
        public static Voltekpiworklog Set(VolteKPIWorklog model)
        {
            return new Voltekpiworklog()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Opcoid = model.OpCoId,
                Month = model.Month,
                Voltekpiid = model.VolteKPIId,
                Year = model.Year,
                Actualmonthlyvaluenew = model.ActualMonthlyValueNew,
                Actualmonthlyvalueold = model.ActualMonthlyValueOld,
                Actualnumberofprovisionednew = model.ActualNumberOfProvisionedNew,
                Actualnumberofprovisionedold = model.ActualNumberOfProvisionedOld,
                Actualnumberofregisterednew = model.ActualNumberOfRegisteredNew,
                Actualnumberofregisteredold = model.ActualNumberOfRegisteredOld,
                Approved = model.Approved,
                Comments = model.Comments,
                Eoytargetnew = model.EoyTargetNew,
                Eoytargetold = model.EoyTargetOld,
                Isstored = model.IsStored,
                Targetmonthlyvaluenew = model.TargetMonthlyValueNew,
                Targetmonthlyvalueold = model.TargetMonthlyValueOld,
                Targetmonthlyvalueproposed = model.TargetMonthlyValueProposed,
                Voltekpitype = model.VolteKPIType,
                Voltekpiworklogid = model.VolteKPIWorklogId,

            };
        }
    }
}
