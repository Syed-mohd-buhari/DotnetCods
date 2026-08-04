using CAM.Enum;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAM.BusinessManager.ExtensionMethod.PlannedActivityResource
{
    public static class PlannedActivityResourceMethod
    {
     
        public static string toPlanningRiskDescription(this CAM.Entities.Models.Lookup.PlannedActivityResource entity, bool forLcm, bool forDesignAspect, bool forAddAsset, bool forEditAsset)
        {
            if (entity.PlannedActivityResourcePlanningRisk == null)
            {
                return null;
            }
            var entities = entity.PlannedActivityResourcePlanningRisk.Where(x => x.ForLcm == forLcm && x.ForDesignAspect == forDesignAspect && x.ForAddAsset == forAddAsset && x.ForEditAsset == forEditAsset).ToList();
            if (entities.Count <= 0)
            {
                return null;
            }
            return String.Join(" | ", entities.Select(x => x.PlanningRisk?.PlanningRiskDescription).Where(x => x != null).ToArray());
        }
        public static List<short> getSelectedPlanningRisks(this CAM.Entities.Models.Lookup.PlannedActivityResource entity, bool forLcm, bool forDesignAspect, bool forAddAsset, bool forEditAsset, bool forServicePlan = false)
        {
            List<short> retVal = new List<short>();
            if (entity.PlannedActivityResourcePlanningRisk != null)
            {
                var entities = entity.PlannedActivityResourcePlanningRisk.Where(x => x.ForLcm == forLcm && x.ForDesignAspect == forDesignAspect && x.ForAddAsset == forAddAsset && x.ForEditAsset == forEditAsset).ToList();
                if (entities.Count > 0)
                {
                    retVal = entities.Select(x => x.PlanningRiskId).ToList();
                }
            }
            return retVal;
        }
        public static string toBenefitDescription(this CAM.Entities.Models.Lookup.PlannedActivityResource entity, bool forLcm, bool forDesignAspect, bool forAddAsset, bool forEditAsset)
        {
            if (entity.PlannedActivityResourceBenefit == null)
            {
                return null;
            }
            var entities = entity.PlannedActivityResourceBenefit.Where(x => x.ForLcm == forLcm && x.ForDesignAspect == forDesignAspect && x.ForAddAsset == forAddAsset && x.ForEditAsset == forEditAsset).ToList();
            if (entities.Count <= 0)
            {
                return null;
            }
            return String.Join(" | ", entities.Select(x => x.Benefit?.BenefitDescription).Where(x => x != null).ToArray());
        }
        public static List<short> getSelectedBenefits(this CAM.Entities.Models.Lookup.PlannedActivityResource entity, bool forLcm, bool forDesignAspect, bool forAddAsset, bool forEditAsset, bool forServicePlan = false)
        {
            List<short> retVal = new List<short>();
            if (entity.PlannedActivityResourceBenefit != null)
            {
                var entities = entity.PlannedActivityResourceBenefit.Where(x => x.ForLcm == forLcm && x.ForDesignAspect == forDesignAspect && x.ForAddAsset == forAddAsset && x.ForEditAsset == forEditAsset).ToList();
                if (entities.Count > 0)
                {
                    retVal = entities.Select(x => x.BenefitId).ToList();
                }
            }
            return retVal;
        }
        public static string toDriverDescription(this CAM.Entities.Models.Lookup.PlannedActivityResource entity, bool forLcm , bool forDesignAspect, bool forAddAsset, bool forEditAsset)
        {
            if (entity.PlannedActivityResourceDriver == null)
            {
                return null;
            }
            var entities = entity.PlannedActivityResourceDriver.Where(x => x.ForLcm == forLcm && x.ForDesignAspect == forDesignAspect && x.ForAddAsset == forAddAsset && x.ForEditAsset == forEditAsset).ToList();
            if (entities.Count <= 0)
            {
                return null;
            }
            return String.Join(" | ", entities.Select(x => x.Driver?.DriverDescription).Where(x => x != null).ToArray());
        }
        public static List<short> getSelectedDrivers(this CAM.Entities.Models.Lookup.PlannedActivityResource entity, bool forLcm , bool forDesignAspect, bool forAddAsset, bool forEditAsset, bool forServicePlan = false)
        {
            List<short> retVal = new List<short>();
            if (entity.PlannedActivityResourceDriver != null )
            {
                var entities = entity.PlannedActivityResourceDriver.Where(x => x.ForLcm == forLcm && x.ForDesignAspect == forDesignAspect && x.ForAddAsset == forAddAsset && x.ForEditAsset == forEditAsset).ToList();
                if (entities.Count > 0)
                {
                    retVal = entities.Select(x => x.DriverId).ToList();
                }
            }
            return retVal;
        } 


        public static string toPlannedActivityTypeIsFor(this CAM.Entities.Models.Settings.SettingsUpdatePlannedActivity entity)
        {
            if (entity.PlannedActivityTypeFor == null)
            {
                return null;
            }
            if (entity.PlannedActivityTypeFor == (short)PlannedActivityTypeForEnum.LcmEngineering)
            {
                return "Lcm Engineering";
            }
            else if (entity.PlannedActivityTypeFor == (short)PlannedActivityTypeForEnum.AddAsset)
            {
                return "Add Asset";
            }
            else if (entity.PlannedActivityTypeFor == (short)PlannedActivityTypeForEnum.EditAsset)
            {
                return "Edit Asset";
            }
            else if (entity.PlannedActivityTypeFor == (short)PlannedActivityTypeForEnum.DesignAspect)
            {
                return "Design Aspect";
            }
            else if (entity.PlannedActivityTypeFor == (short)PlannedActivityTypeForEnum.ServicePlan)
            {
                return "Service Plan";
            }
            else
            {
                return null;
            }
        }
        
    }
}
