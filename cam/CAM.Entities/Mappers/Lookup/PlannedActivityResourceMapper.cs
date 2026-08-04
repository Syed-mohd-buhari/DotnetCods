using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using OracleModels.DBModels;
namespace CAM.Entities.Mappers.Lookup
{
    public static class PlannedActivityResourceMapper
    {
        public static Models.Lookup.PlannedActivityResource GetPlannedActivityResourceMapper(Plannedactivityresources model)
        {
            if (model == null)
                return null;
            var result = new Models.Lookup.PlannedActivityResource()
            {
                PlannedActivityResourceId = model.Plannedactivityresourceid,
                PlannedActivityResourceDescription = model.Plannedactivityresource,
                CreationDate = model.Creationdate,
                CreationUser = model.Creationuser,
                ModificationDate = model.Modificationdate,
                ModificationUser = model.Modificationuser,
                Deleted = model.Deleted.Value,
                DeletionDate = model.Deletiondate,
                CreationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.CreationuserNavigation),
                ModificationUserEntity = ApplicationUserMapper.GetApplicationUserMapper(model.ModificationuserNavigation),
                ActivityDetailsForVirtualizedAddAsset = model.Actdetailsforvrtaddasset,
                ActivityDetailsForVirtualizedEditAsset = model.Actdetailsforvrteditasset,
                ActivityDetailsAddAsset = model.Activitydetailsaddasset,
                ActivityDetailsEditAsset = model.Activitydetailseditasset,
                BenefitTextAddAsset = model.Benefittextaddasset,
                BenefitTextEditAsset = model.Benefittexteditasset,
                DriverTextAddAsset = model.Drivertextaddasset,
                DriverTextEditAsset = model.Drivertexteditasset,
                Exportable = model.Exportable,
                ForCreateAddAsset = model.Forcreateaddasset,
                ForCreateEditAsset = model.Forcreateeditasset,
                ForEditAddAsset = model.Foreditaddasset,
                ForEditEditAsset = model.Forediteditasset,
                ForLcm = model.Forlcm,
                ForAddAsset = model.Foraddasset,
                ForEditAsset = model.Foreditasset,
                JsonForm = model.Jsonform,
                LcmHardware = model.Lcmhardware,
                LcmLabelHardware = model.Lcmlabelhardware,
                LcmLabelSoftware = model.Lcmlabelsoftware,
                LcmSoftware = model.Lcmsoftware,
                AddAssetHardware = model.Addassethardware,
                AddAssetLabelHardware = model.Addassetlabelhardware,
                AddAssetSoftware = model.Addassetsoftware,
                AddAssetLabelSoftware = model.Addassetlabelsoftware,
                EditAssetHardware = model.Editassethardware,
                EditAssetLabelHardware = model.Editassetlabelhardware,
                EditAssetSoftware = model.Editassetsoftware,
                EditAssetLabelSoftware = model.Editassetlabelsoftware,
                RuleActicvityDetails = model.Ruleacticvitydetails,
                RuleActicvityDetailsAddAsset = model.Ruleactdetailsaddasset,
                RuleActicvityDetailsEditAsset = model.Ruleactdetailseditasset,
                RuleLinkedDc = model.Rulelinkeddc,
                RuleAddAsset = model.Ruleaddasset,
                RuleEditAsset = model.Ruleeditasset,
                ActivityDetailsLcm = model.Activitydetailslcm,
                OnBareMetalAddAsset = model.Onbaremetaladdasset,
                OnBareMetalEditAsset = model.Onbaremetaleditasset,
                OnVirtualizedAddAsset = model.Onbaremetaladdasset,
                OnVirtualizedEditAsset = model.Onvirtualizededitasset,
                PlannedDesignComponentRequiredAddAsset = model.Plandesigncompreqaddasset,
                PlannedDesignComponentRequiredEditAsset = model.Plandesigncompreqeditasset,
                ActivityDetailsDesignAspect = model.Activitydetailsdesignaspect,
                ForDesignAspect = model.Fordesignaspect ?? false,
                DesignAspectLabelHardware = model.Designaspectlabelhardware,
                DesignAspectLabelSoftware = model.Designaspectlabelsoftware,
                DesignAspectHardware = model.Designaspecthardware ?? false,
                DesignAspectSoftware = model.Designaspectsoftware ?? false,
                RuleDesignAspect = model.Ruledesignaspect,
                DesignAspectExportable = model.Designaspectexportable ?? false,
                RuleLinkedDcNavigation = PlannedActivityTypesMapper.GetPlannedActivityTypesRules(model.RulelinkeddcNavigation),
                ForServicePlan = model.Forserviceplan,
            };

            foreach (var item in model.Plannedactivityresourcebenefit)
            {
                result.PlannedActivityResourceBenefit.Add(PlannedActivityResourceBenefitMapper.Get(item));
            }
            foreach (var item in model.Plannedactivityresourcedriver)
            {
                result.PlannedActivityResourceDriver.Add(PlannedActivityResourceDriverMapper.Get(item));
            }
            foreach (var item in model.Plannedactivityresourceplanningrisk)
            {
                result.PlannedActivityResourcePlanningRisk.Add(PlannedActivityResourcePlanningRiskMapper.Get(item));
            }
           
            return result;
        }
        public static Plannedactivityresources SetPlannedActivityResourceMapper(Models.Lookup.PlannedActivityResource model)
        {
            var result = new Plannedactivityresources()
            {

                Plannedactivityresourceid = model.PlannedActivityResourceId,
                Plannedactivityresource = model.PlannedActivityResourceDescription,
                Creationdate = model.CreationDate,
                Creationuser = model.CreationUser,
                Modificationdate = model.ModificationDate,
                Modificationuser = model.ModificationUser,
                Deleted = model.Deleted,
                Deletiondate = model.DeletionDate,
                Actdetailsforvrtaddasset = model.ActivityDetailsForVirtualizedAddAsset,
                Actdetailsforvrteditasset = model.ActivityDetailsForVirtualizedAddAsset,
                Activitydetailsaddasset = model.ActivityDetailsAddAsset,
                Activitydetailseditasset = model.ActivityDetailsEditAsset,
                Benefittextaddasset = model.BenefitTextAddAsset,
                Benefittexteditasset = model.BenefitTextEditAsset,
                Drivertextaddasset = model.DriverTextAddAsset,
                Drivertexteditasset = model.DriverTextEditAsset,
                Exportable = model.Exportable,
                Forcreateaddasset = model.ForCreateAddAsset,
                Forcreateeditasset = model.ForCreateEditAsset,
                Foreditaddasset = model.ForEditAddAsset,
                Forediteditasset = model.ForEditEditAsset,
                Forlcm = model.ForLcm,
                Foraddasset = model.ForAddAsset,
                Foreditasset = model.ForEditAsset,
                Jsonform = model.JsonForm,
                Lcmhardware = model.LcmHardware,
                Lcmlabelhardware = model.LcmLabelHardware,
                Lcmlabelsoftware = model.LcmLabelSoftware,
                Lcmsoftware = model.LcmSoftware,
                Addassethardware = model.AddAssetHardware,
                Addassetlabelhardware = model.AddAssetLabelHardware,
                Addassetsoftware = model.AddAssetSoftware,
                Addassetlabelsoftware = model.AddAssetLabelSoftware,
                Editassethardware = model.EditAssetHardware,
                Editassetlabelhardware = model.EditAssetLabelHardware,
                Editassetsoftware = model.EditAssetSoftware,
                Editassetlabelsoftware = model.EditAssetLabelSoftware,
                Ruleacticvitydetails = model.RuleActicvityDetails,
                Ruleactdetailsaddasset = model.RuleActicvityDetailsAddAsset,
                Ruleactdetailseditasset = model.RuleActicvityDetailsEditAsset,
                Rulelinkeddc = model.RuleLinkedDc,
                Ruleaddasset = model.RuleAddAsset,
                Ruleeditasset = model.RuleEditAsset,
                Activitydetailslcm = model.ActivityDetailsLcm,
                Onbaremetaladdasset = model.OnBareMetalAddAsset,
                Onbaremetaleditasset = model.OnBareMetalEditAsset,
                Onvirtualizedaddasset = model.OnVirtualizedAddAsset,
                Onvirtualizededitasset = model.OnVirtualizedEditAsset,
                Plandesigncompreqaddasset = model.PlannedDesignComponentRequiredAddAsset,
                Plandesigncompreqeditasset = model.PlannedDesignComponentRequiredEditAsset,
                Activitydetailsdesignaspect = model.ActivityDetailsDesignAspect,
                Fordesignaspect = model.ForDesignAspect,
                Designaspectlabelhardware = model.DesignAspectLabelHardware,
                Designaspectlabelsoftware = model.DesignAspectLabelSoftware,
                Designaspecthardware = model.DesignAspectHardware,
                Designaspectsoftware = model.DesignAspectSoftware,
                Ruledesignaspect = model.RuleDesignAspect,
                Designaspectexportable = model.DesignAspectExportable,
                RulelinkeddcNavigation = PlannedActivityTypesMapper.SetPlannedActivityTypesRules(model.RuleLinkedDcNavigation),
                Forserviceplan = model.ForServicePlan,

            };
            foreach (var item in model.PlannedActivityResourceBenefit)
            {
                result.Plannedactivityresourcebenefit.Add(PlannedActivityResourceBenefitMapper.Set(item));
            }
            foreach (var item in model.PlannedActivityResourceDriver)
            {
                result.Plannedactivityresourcedriver.Add(PlannedActivityResourceDriverMapper.Set(item));
            }
            foreach (var item in model.PlannedActivityResourcePlanningRisk)
            {
                result.Plannedactivityresourceplanningrisk.Add(PlannedActivityResourcePlanningRiskMapper.Set(item));
            }          

            return result;
        }

        public static Models.Lookup.PlannedActivityResource GetPlannedActivityResourceMapperForBPT(Plannedactivityresources model)
        {
            if (model == null)
                return null;
            var result = new Models.Lookup.PlannedActivityResource()
            {
                PlannedActivityResourceId = model.Plannedactivityresourceid,
                PlannedActivityResourceDescription = model.Plannedactivityresource,
            }; 
            return result;
        }
    }
}
