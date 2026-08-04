using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.Entity
{
    public static class VolteKPIMapper
    {
        public static VolteKPI Get(Voltekpi model)
        {
            if (model == null)
                return null;
            return new VolteKPI()
            {
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                KPIFourActualMonthly = model.Kpifouractualmonthly,
                KPIFourComment = model.Kpifourcomment,
                OpCoId = model.Opcoid,
                KPIFourFinalTarget = model.Kpifourfinaltarget,
                KPIFourTargetDateMonth = model.Kpifourtargetdatemonth,
                KPIFourTargetDateYear = model.Kpifourtargetdateyear,
                KPIFourTargetMonthly = model.Kpifourtargetmonthly,
                KPIOneActualValue = model.Kpioneactualvalue,
                KPIOneComment = model.Kpionecomment,
                KPIThreeComment = model.Kpithreecomment,
                KPIOneEoYTarget= model.Kpioneeoytarget,
                KPIOneMonthlyTarget = model.Kpionemonthlytarget,
                KPIThreeActualValue = model.Kpithreeactualvalue,
                KPIThreeEoYTarget =model.Kpithreeeoytarget,
                KPIThreeMonthlyTarget =model.Kpithreemonthlytarget,
                KPITwoComment = model.Kpitwocomment,
                Month = model.Month,
                VolteKPIId = model.Voltekpiid,
                Year = model.Year,
                KPIFourTargetValueChangeProposal = model.Kpi4targetvluchngproposal,
                KPIOneTargetValueChangeProposal=model.Kpi1targetcluchngproposal,
                KPIThreeTargetValueChangeProposal= model.Kpi3targetvluchngproposal,
                KPITwoActualNumberOfProvisionedSubscriber = model.Kpi2actualnoofprovisionedsubsc,
                KPITwoActualNumberOfRegisteredSubscribers =model.Kpi2actualnoofregsubsc,
                OpCo = OpCoMapper.GetOpCoMapper(model.Opco),
                
                

            };
        }
        public static Voltekpi Set(VolteKPI model)
        {
            return new Voltekpi()
            {
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Kpifouractualmonthly = model.KPIFourActualMonthly,
                Kpifourcomment = model.KPIFourComment,
                Opcoid = model.OpCoId,
                Kpifourfinaltarget = model.KPIFourFinalTarget,
                Kpifourtargetdatemonth = model.KPIFourTargetDateMonth,
                Kpifourtargetdateyear = model.KPIFourTargetDateYear,
                Kpifourtargetmonthly = model.KPIFourTargetMonthly,
                Kpioneactualvalue = model.KPIOneActualValue,
                Kpionecomment = model.KPIOneComment,
                Kpithreecomment = model.KPIThreeComment,
                Kpioneeoytarget = model.KPIOneEoYTarget,
                Kpionemonthlytarget = model.KPIOneMonthlyTarget,
                Kpithreeactualvalue = model.KPIThreeActualValue,
                Kpithreeeoytarget = model.KPIThreeEoYTarget,
                Kpithreemonthlytarget = model.KPIThreeMonthlyTarget,
                Kpitwocomment = model.KPITwoComment,
                Month = model.Month,
                Voltekpiid = model.VolteKPIId,
                Year = model.Year,
                Kpi4targetvluchngproposal = model.KPIFourTargetValueChangeProposal,
                Kpi1targetcluchngproposal = model.KPIOneTargetValueChangeProposal,
                Kpi3targetvluchngproposal = model.KPIThreeTargetValueChangeProposal,
                Kpi2actualnoofprovisionedsubsc = model.KPITwoActualNumberOfProvisionedSubscriber,
                Kpi2actualnoofregsubsc = model.KPITwoActualNumberOfRegisteredSubscribers,


            };
        }
    }
}
