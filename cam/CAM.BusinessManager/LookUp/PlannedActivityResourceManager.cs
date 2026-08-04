using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Entities.Models;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.LookUp.PlannedActivityResourceDto;
using CAM.Entities.Models.Lookup;
using CAM.DataTransferObjects.LookUp;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.PlannedActivityResource;
using CAM.Entities.Models.Cross;
using CAM.Entities.Mappers.Lookup;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Entity;
using CAM.BusinessManager.ExtensionMethod.DeploymentStatus;
using CAM.BusinessManager.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CAM.DataTransferObjects.LookUp.DeliveryTracking;

namespace CAM.BusinessManager.LookUp
{
    public class PlannedActivityResourceManager : GridBaseAsync<PlannedActivityResource, PlannedActivityResourceDtoGrid, PlannedActivityResourceQuery, Plannedactivityresources>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private readonly PlannedActivityManager _plannedActivityManager;
        private readonly DeliveryTrackingManager _deliveryTrackingManager;

        public PlannedActivityResourceManager(IEnumerable<IRepositoryWrapper> wrappers,
            GridCustomColumnManager columnManager, PlannedActivityManager pmanager, IHttpContextAccessor contextAccessor, 
            IRepositoryWrapper repositoryWrapper, DeliveryTrackingManager deliveryTrackingManager) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _plannedActivityManager = pmanager;
            _deliveryTrackingManager = deliveryTrackingManager;
        }


        public override ExpressionStarter<Plannedactivityresources> ApplyFilterForOracleModel(PlannedActivityResourceQuery request)
        {
            var predicateResult = PredicateBuilder.New<Plannedactivityresources>();
            var predicateInner = PredicateBuilder.New<Plannedactivityresources>();

            if (request.PlannedActivityResourceDescription != null && request.PlannedActivityResourceDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.PlannedActivityResourceDescription)
                    predicateInner.Or(x => x.Plannedactivityresource == item);
                predicateResult.And(predicateInner);
            }

            if (request.PlannedActivityResourceId != null && request.PlannedActivityResourceId.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.PlannedActivityResourceId)
                    predicateInner.Or(x => x.Plannedactivityresourceid == item);
                predicateResult.And(predicateInner);
            }


            if (request.ActivityDetailsAddAsset != null && request.ActivityDetailsAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ActivityDetailsAddAsset)
                    predicateInner.Or(x => x.Activitydetailsaddasset == item);
                predicateResult.And(predicateInner);
            }
            if (request.ActivityDetailsEditAsset != null && request.ActivityDetailsEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ActivityDetailsEditAsset)
                    predicateInner.Or(x => x.Activitydetailseditasset == item);
                predicateResult.And(predicateInner);
            }
            if (request.ActivityDetailsLcm != null && request.ActivityDetailsLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ActivityDetailsLcm)
                    predicateInner.Or(x => x.Activitydetailslcm == item);
                predicateResult.And(predicateInner);
            }
            if (request.ActivityDetailsDesignAspect != null && request.ActivityDetailsDesignAspect.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ActivityDetailsDesignAspect)
                    predicateInner.Or(x => x.Activitydetailsdesignaspect == item);
                predicateResult.And(predicateInner);
            }
            if (request.ActivityDetailsForVirtualizedAddAsset != null && request.ActivityDetailsForVirtualizedAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ActivityDetailsForVirtualizedAddAsset)
                    predicateInner.Or(x => x.Actdetailsforvrtaddasset == item);
                predicateResult.And(predicateInner);
            }
            if (request.ActivityDetailsForVirtualizedEditAsset != null && request.ActivityDetailsForVirtualizedEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ActivityDetailsForVirtualizedEditAsset)
                    predicateInner.Or(x => x.Actdetailsforvrteditasset == item);
                predicateResult.And(predicateInner);
            }

            if (request.RuleActicvityDetails != null && request.RuleActicvityDetails.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.RuleActicvityDetails)
                    predicateInner.Or(x => x.Ruleacticvitydetails == item);
                predicateResult.And(predicateInner);
            }
            if (request.RuleDesignAspect != null && request.RuleDesignAspect.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.RuleDesignAspect)
                    predicateInner.Or(x => x.Ruledesignaspect == item);
                predicateResult.And(predicateInner);
            }
            if (request.RuleLinkedDc != null && request.RuleLinkedDc.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.RuleLinkedDc)
                    predicateInner.Or(x => x.Rulelinkeddc == item);
                predicateResult.And(predicateInner);
            }
            if (request.RuleAddAsset != null && request.RuleAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.RuleAddAsset)
                    predicateInner.Or(x => x.Ruleaddasset == item);
                predicateResult.And(predicateInner);
            }
            if (request.RuleEditAsset != null && request.RuleEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.RuleEditAsset)
                    predicateInner.Or(x => x.Ruleeditasset == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.JsonFormResource != null && request.JsonFormResource.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.JsonFormResource)
                    predicateInner.Or(x => x.Jsonform == item);
                predicateResult.And(predicateInner);
            }

            if (request.Exportable != null && request.Exportable.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.Exportable)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Exportable == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Exportable == false);
                    }
                }
            }
            if (request.DesignAspectExportable != null && request.DesignAspectExportable.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.DesignAspectExportable)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Designaspectexportable == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Designaspectexportable == false);
                    }
                }
            }
            if (request.PlannedDesignComponentRequiredAddAsset != null && request.PlannedDesignComponentRequiredAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.PlannedDesignComponentRequiredAddAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Plandesigncompreqaddasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Plandesigncompreqaddasset == false);
                    }
                }
            }
            if (request.PlannedDesignComponentRequiredEditAsset != null && request.PlannedDesignComponentRequiredEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.PlannedDesignComponentRequiredEditAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Plandesigncompreqeditasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Plandesigncompreqeditasset == false);
                    }
                }
            }

            if (request.LcmHardware != null && request.LcmHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.LcmHardware)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Lcmhardware == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Lcmhardware == false);
                    }
                }
            }
            if (request.LcmSoftware != null && request.LcmSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.LcmSoftware)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Lcmsoftware == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Lcmsoftware == false);
                    }
                }
            }
            if (request.LcmLabelHardware != null && request.LcmLabelHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.LcmLabelHardware)
                    predicateInner.Or(x => x.Lcmlabelhardware == item);
                predicateResult.And(predicateInner);
            }
            if (request.LcmLabelSoftware != null && request.LcmLabelSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.LcmLabelSoftware)
                    predicateInner.Or(x => x.Lcmlabelsoftware == item);
                predicateResult.And(predicateInner);
            }

            if (request.DesignAspectHardware != null && request.DesignAspectHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.DesignAspectHardware)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Designaspecthardware == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Designaspecthardware == false);
                    }
                }
            }
            if (request.DesignAspectSoftware != null && request.DesignAspectSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.DesignAspectSoftware)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Designaspectsoftware == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Designaspectsoftware == false);
                    }
                }
            }

            if (request.DesignAspectLabelHardware != null && request.DesignAspectLabelHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.DesignAspectLabelHardware)
                    predicateInner.Or(x => x.Designaspectlabelhardware == item);
                predicateResult.And(predicateInner);
            }
            if (request.DesignAspectLabelSoftware != null && request.DesignAspectLabelSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.DesignAspectLabelSoftware)
                    predicateInner.Or(x => x.Designaspectlabelsoftware == item);
                predicateResult.And(predicateInner);
            }
            if (request.AddAssetHardware != null && request.AddAssetHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.AddAssetHardware)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Addassethardware == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Addassethardware == false);
                    }
                }
            }
            if (request.EditAssetHardware != null && request.EditAssetHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.EditAssetHardware)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Editassethardware == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Editassethardware == false);
                    }
                }
            }
            if (request.AddAssetSoftware != null && request.AddAssetSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.AddAssetSoftware)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Addassetsoftware == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Addassetsoftware == false);
                    }
                }
            }
            if (request.EditAssetSoftware != null && request.EditAssetSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.EditAssetSoftware)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Editassetsoftware == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Editassetsoftware == false);
                    }
                }
            }
            if (request.AddAssetLabelHardware != null && request.AddAssetLabelHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.AddAssetLabelHardware)
                    predicateInner.Or(x => x.Addassetlabelhardware == item);
                predicateResult.And(predicateInner);
            }
            if (request.EditAssetLabelHardware != null && request.EditAssetLabelHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.EditAssetLabelHardware)
                    predicateInner.Or(x => x.Editassetlabelhardware == item);
                predicateResult.And(predicateInner);
            }
            if (request.AddAssetLabelSoftware != null && request.AddAssetLabelSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.AddAssetLabelSoftware)
                    predicateInner.Or(x => x.Addassetlabelsoftware == item);
                predicateResult.And(predicateInner);
            }
            if (request.EditAssetLabelSoftware != null && request.EditAssetLabelSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.EditAssetLabelSoftware)
                    predicateInner.Or(x => x.Editassetlabelsoftware == item);
                predicateResult.And(predicateInner);
            }

            if (request.ForCreateAddAsset != null && request.ForCreateAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ForCreateAddAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Forcreateaddasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Forcreateaddasset == false);
                    }
                }
            }

            if (request.ForCreateEditAsset != null && request.ForCreateEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ForCreateEditAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Forcreateeditasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Forcreateeditasset == false);
                    }
                }
            }

            if (request.ForEditAddAsset != null && request.ForEditAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ForEditAddAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Foreditaddasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Foreditaddasset == false);
                    }
                }
            }

            if (request.ForEditEditAsset != null && request.ForEditEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ForEditEditAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Forediteditasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Forediteditasset == false);
                    }
                }
            }
            if (request.ForLcm != null && request.ForLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ForLcm)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Forlcm == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Forlcm == false);
                    }
                }
            }
            if (request.ForDesignAspect != null && request.ForDesignAspect.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ForDesignAspect)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Fordesignaspect == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Fordesignaspect == false);
                    }
                }
            }
            if (request.ForAddAsset != null && request.ForAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ForAddAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Foraddasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Foraddasset == false);
                    }
                }
            }

            if (request.ForEditAsset != null && request.ForEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ForEditAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Foreditasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Foreditasset == false);
                    }
                }
            }

            if (request.RuleActicvityDetailsAddAsset != null && request.RuleActicvityDetailsAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.RuleActicvityDetailsAddAsset)
                    predicateInner.Or(x => x.Ruleactdetailsaddasset == item);
                predicateResult.And(predicateInner);
            }
            if (request.RuleActicvityDetailsEditAsset != null && request.RuleActicvityDetailsEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.RuleActicvityDetailsEditAsset)
                    predicateInner.Or(x => x.Ruleactdetailseditasset == item);
                predicateResult.And(predicateInner);
            }
            if (request.DriverTextAddAsset != null && request.DriverTextAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.DriverTextAddAsset)
                    predicateInner.Or(x => x.Plannedactivityresourcedriver.Any(d => d.Driverid == item && x.Forlcm == false));
                predicateResult.And(predicateInner);
            }
            if (request.DriverTextEditAsset != null && request.DriverTextEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.DriverTextEditAsset)
                    predicateInner.Or(x => x.Plannedactivityresourcedriver.Any(d => d.Driverid == item && x.Forlcm == false));
                predicateResult.And(predicateInner);
            }
            if (request.BenefitTextAddAsset != null && request.BenefitTextAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.BenefitTextAddAsset)
                    predicateInner.Or(x => x.Plannedactivityresourcebenefit.Any(d => d.Benefitid == item && x.Forlcm == false));
                predicateResult.And(predicateInner);
            }
            if (request.BenefitTextEditAsset != null && request.BenefitTextEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.BenefitTextEditAsset)
                    predicateInner.Or(x => x.Plannedactivityresourcebenefit.Any(d => d.Benefitid == item && x.Forlcm == false));
                predicateResult.And(predicateInner);
            }
            if (request.PlanningRisksAddAsset != null && request.PlanningRisksAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.PlanningRisksAddAsset)
                    predicateInner.Or(x => x.Plannedactivityresourceplanningrisk.Any(d => d.Planningriskid == item && x.Forlcm == false));
                predicateResult.And(predicateInner);
            }
            if (request.PlanningRisksEditAsset != null && request.PlanningRisksAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.PlanningRisksAddAsset)
                    predicateInner.Or(x => x.Plannedactivityresourceplanningrisk.Any(d => d.Planningriskid == item && x.Forlcm == false));
                predicateResult.And(predicateInner);
            }
            if (request.DriverTextDesignAspect != null && request.DriverTextDesignAspect.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.DriverTextDesignAspect)
                    predicateInner.Or(x => x.Plannedactivityresourcedriver.Any(d => d.Driverid == item && x.Fordesignaspect == true));
                predicateResult.And(predicateInner);
            }
            if (request.BenefitTextDesignAspect != null && request.BenefitTextDesignAspect.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.BenefitTextDesignAspect)
                    predicateInner.Or(x => x.Plannedactivityresourcebenefit.Any(d => d.Benefitid == item && x.Fordesignaspect == true));
                predicateResult.And(predicateInner);
            }
            if (request.PlanningRisksDesignAspect != null && request.PlanningRisksDesignAspect.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.PlanningRisksDesignAspect)
                    predicateInner.Or(x => x.Plannedactivityresourceplanningrisk.Any(d => d.Planningriskid == item && x.Fordesignaspect == true));
                predicateResult.And(predicateInner);
            }
            if (request.DriverTextLcm != null && request.DriverTextLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.DriverTextLcm)
                    predicateInner.Or(x => x.Plannedactivityresourcedriver.Any(d => d.Driverid == item && x.Forlcm == true));
                predicateResult.And(predicateInner);
            }
            if (request.BenefitTextLcm != null && request.BenefitTextLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.BenefitTextLcm)
                    predicateInner.Or(x => x.Plannedactivityresourcebenefit.Any(d => d.Benefitid == item && x.Forlcm == true));
                predicateResult.And(predicateInner);
            }
            if (request.PlanningRisksLcm != null && request.PlanningRisksLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.PlanningRisksLcm)
                    predicateInner.Or(x => x.Plannedactivityresourceplanningrisk.Any(d => d.Planningriskid == item && x.Forlcm == true));
                predicateResult.And(predicateInner);
            }

            if (request.OnBareMetalAddAsset != null && request.OnBareMetalAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.OnBareMetalAddAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Onbaremetaladdasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Onbaremetaladdasset == false);
                    }
                }
            }
            if (request.OnBareMetalEditAsset != null && request.OnBareMetalEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.OnBareMetalEditAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Onbaremetaleditasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Onbaremetaleditasset == false);
                    }
                }
            }
            if (request.OnVirtualizedAddAsset != null && request.OnVirtualizedAddAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.OnVirtualizedAddAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Onvirtualizedaddasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Onvirtualizedaddasset == false);
                    }
                }
            }
            if (request.OnVirtualizedEditAsset != null && request.OnVirtualizedEditAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.OnVirtualizedEditAsset)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Onvirtualizededitasset == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Onvirtualizededitasset == false);
                    }
                }
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            if (request.RuleLinkedDcPlannedActivityTypeDescription != null && request.RuleLinkedDcPlannedActivityTypeDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.RuleLinkedDcPlannedActivityTypeDescription)
                    predicateInner.Or(x => x.RulelinkeddcNavigation.Plannedactivitytypedescription == item);
                predicateResult.And(predicateInner);
            }
            if (request.ForServiePlan != null && request.ForServiePlan.Any())
            {
                predicateInner = PredicateBuilder.New<Plannedactivityresources>();
                foreach (var item in request.ForServiePlan)
                    predicateInner.Or(x => x.Forserviceplan.ToString() == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public async Task<IDictionary<short, PlannedActivityResourceDto>> GetResourceForDropdown()
        {
            var plannedActivityResource = _repositoryWrapper.PlannedActivityResourceRepository.FindAll()
              .Include(x => x.Plannedactivityresourceplanningrisk)
                .Include(x => x.Plannedactivityresourcebenefit)
                .Include(x => x.Plannedactivityresourcedriver);


            var plannedActivityResourceForDto = plannedActivityResource.ToDictionary(x => x.Plannedactivityresourceid, x => new PlannedActivityResourceDto()
            {
                PlannedActivityResourceDescription = x.Plannedactivityresource,
                //Deleted = x.Deleted,
                LastModified = x.Modificationdate,
                RuleActicvityDetails = x.Ruleacticvitydetails,
                RuleLinkedDc = x.Rulelinkeddc,
                Exportable = x.Exportable,
                DesignAspectExportable = x.Designaspectexportable ?? false,
                LcmLabelHardware = x.Lcmlabelhardware,
                LcmLabelSoftware = x.Lcmlabelsoftware,
                LcmHardware = x.Lcmhardware,
                LcmSoftware = x.Lcmsoftware,

                AddAssetHardware = x.Addassethardware,
                AddAssetLabelHardware = x.Addassetlabelhardware,
                AddAssetSoftware = x.Addassetsoftware,
                AddAssetLabelSoftware = x.Addassetlabelsoftware,
                EditAssetHardware = x.Editassethardware,
                EditAssetLabelHardware = x.Editassetlabelhardware,
                EditAssetSoftware = x.Editassetsoftware,
                EditAssetLabelSoftware = x.Editassetlabelsoftware,


                PlannedDesignComponentRequiredAddAsset = x.Plandesigncompreqaddasset,
                PlannedDesignComponentRequiredEditAsset = x.Plandesigncompreqeditasset,

                ForLcm = x.Forlcm,
                RuleAddAsset = x.Ruleaddasset,
                RuleEditAsset = x.Ruleeditasset,
                DriverTextAddAsset = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedDrivers(false, false, true, false),
                DriverTextEditAsset = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedDrivers(false, false, false, true),

                BenefitTextAddAsset = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedBenefits(false, false, true, false),
                BenefitTextEditAsset = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedBenefits(false, false, false, true),

                PlanningRisksAddAsset = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedPlanningRisks(false, false, true, false),
                PlanningRisksAEditAsset = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedPlanningRisks(false, false, false, true),

                DriverTextLcm = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedDrivers(true, false, false, false),
                BenefitTextLcm = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedBenefits(true, false, false, false),
                PlanningRisksLcm = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedPlanningRisks(true, false, false, false),

                OnBareMetalAddAsset = x.Onbaremetaladdasset,
                OnBareMetalEditAsset = x.Onbaremetaleditasset,
                OnVirtualizedAddAsset = x.Onvirtualizedaddasset,
                OnVirtualizedEditAsset = x.Onvirtualizededitasset,


                ForCreateAddAsset = x.Forcreateaddasset,
                ForCreateEditAsset = x.Forcreateeditasset,

                ForEditAddAsset = x.Foreditaddasset,
                ForEditEditAsset = x.Forediteditasset,

                ForAddAsset = x.Foraddasset,
                ForEditAsset = x.Foreditasset,

                RuleActicvityDetailsAddAsset = x.Ruleactdetailsaddasset,
                RuleActicvityDetailsEditAsset = x.Ruleactdetailseditasset,
                //JsonFormResource = x.JsonForm,
                JsonForm = x.Jsonform,
                PlannedActivityResourceId = x.Plannedactivityresourceid,
                ActivityDetailsAddAsset = x.Activitydetailsaddasset,
                ActivityDetailsEditAsset = x.Activitydetailseditasset,
                ActivityDetailsLcm = x.Activitydetailslcm,
                ActivityDetailsForVirtualizedAddAsset = x.Actdetailsforvrtaddasset,
                ActivityDetailsForVirtualizedEditAsset = x.Actdetailsforvrteditasset,
                PlanningRisksDesignAspect = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedPlanningRisks(false, true, false, false),
                BenefitTextDesignAspect = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedBenefits(false, true, false, false),
                DriverTextDesignAspect = PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(x).getSelectedDrivers(false, true, false, false),
                ForDesignAspect = x.Fordesignaspect,
                ActivityDetailsDesignAspect = x.Activitydetailsdesignaspect,
                DesignAspectLabelHardware = x.Designaspectlabelhardware,
                DesignAspectLabelSoftware = x.Designaspectlabelsoftware,
                 DesignAspectHardware = x.Designaspecthardware,
                DesignAspectSoftware = x.Designaspectsoftware,
                RuleDesignAspect = x.Ruledesignaspect,
                Forserviceplan = x.Forserviceplan,
            });
            return plannedActivityResourceForDto;

        }
        public override List<PlannedActivityResourceDtoGrid> CastObjectToDto(IQueryable<PlannedActivityResource> request)
        {

            return request.Select(dto => new PlannedActivityResourceDtoGrid()
            {
                PlannedActivityResourceId = dto.PlannedActivityResourceId,
                PlannedActivityResourceDescription = dto.PlannedActivityResourceDescription,
                LastModified = dto.ModificationDate,
                Exportable = dto.Exportable,
                DesignAspectExportable = dto.DesignAspectExportable,
                ForLcm = dto.ForLcm,
                ForDesignAspect = dto.ForDesignAspect,
                DesignAspectHardware = dto.DesignAspectHardware,
                DesignAspectSoftware = dto.DesignAspectSoftware,
                RuleDesignAspect = dto.RuleDesignAspect,
                ActivityDetailsDesignAspect = dto.ActivityDetailsDesignAspect,
                DesignAspectLabelHardware = dto.DesignAspectLabelHardware,
                DesignAspectLabelSoftware = dto.DesignAspectLabelSoftware,
                RuleAddAsset = dto.RuleAddAsset,
                RuleEditAsset = dto.RuleEditAsset,
                RuleActicvityDetailsAddAsset = dto.RuleActicvityDetailsAddAsset,
                RuleActicvityDetailsEditAsset = dto.RuleActicvityDetailsEditAsset,
                PlannedDesignComponentRequiredAddAsset = dto.PlannedDesignComponentRequiredAddAsset,
                PlannedDesignComponentRequiredEditAsset = dto.PlannedDesignComponentRequiredEditAsset,

                DriverTextAddAsset = dto.toDriverDescription(false, false, true, false),
                DriverTextEditAsset = dto.toDriverDescription(false, false, false, true),

                BenefitTextAddAsset = dto.toBenefitDescription(false, false, true, false),
                BenefitTextEditAsset = dto.toBenefitDescription(false, false, false, true),

                PlanningRisksAddAsset = dto.toPlanningRiskDescription(false, false, true, false),
                PlanningRisksEditAsset = dto.toPlanningRiskDescription(false, false, false, true),


                DriverTextLcm = dto.toDriverDescription(true, false, false, false),
                BenefitTextLcm = dto.toBenefitDescription(true, false, false, false),
                PlanningRisksLcm = dto.toPlanningRiskDescription(true, false, false, false),
                DriverTextDesignAspect = dto.toDriverDescription(false, true, false, false),
                BenefitTextDesignAspect = dto.toBenefitDescription(false, true, false, false),
                PlanningRisksDesignAspect = dto.toPlanningRiskDescription(false, true, false, false),
                ForEditAddAsset = dto.ForEditAddAsset,
                ForEditEditAsset = dto.ForEditEditAsset,

                ForAddAsset = dto.ForAddAsset,
                ForEditAsset = dto.ForEditAsset,
                JsonFormResource = dto.JsonForm,
                ActivityDetailsAddAsset = dto.ActivityDetailsAddAsset,
                ActivityDetailsEditAsset = dto.ActivityDetailsEditAsset,
                ActivityDetailsLcm = dto.ActivityDetailsLcm,
                ActivityDetailsForVirtualizedAddAsset = dto.ActivityDetailsForVirtualizedAddAsset,
                ActivityDetailsForVirtualizedEditAsset = dto.ActivityDetailsForVirtualizedEditAsset,

                LcmLabelHardware = dto.LcmLabelHardware,
                LcmLabelSoftware = dto.LcmLabelSoftware,
                LcmHardware = dto.LcmHardware,
                LcmSoftware = dto.LcmSoftware,

                AddAssetHardware = dto.AddAssetHardware,
                AddAssetLabelHardware = dto.AddAssetLabelHardware,
                AddAssetSoftware = dto.AddAssetSoftware,
                AddAssetLabelSoftware = dto.AddAssetLabelSoftware,
                EditAssetHardware = dto.EditAssetHardware,
                EditAssetLabelHardware = dto.EditAssetLabelHardware,
                EditAssetSoftware = dto.EditAssetSoftware,
                EditAssetLabelSoftware = dto.EditAssetLabelSoftware,

                OnBareMetalAddAsset = dto.OnBareMetalAddAsset,
                OnBareMetalEditAsset = dto.OnBareMetalEditAsset,
                OnVirtualizedAddAsset = dto.OnVirtualizedAddAsset,
                OnVirtualizedEditAsset = dto.OnVirtualizedEditAsset,
               
                RuleActicvityDetails = dto.RuleActicvityDetails,
                RuleLinkedDc = dto.RuleLinkedDc,
                ForServicePlan = dto.ForServicePlan,
                RuleLinkedDcPlannedActivityTypeDescription = dto.RuleLinkedDcNavigation.PlannedActivityTypeDescription,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<PlannedActivityResource, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<PlannedActivityResource, object>>[]>
            {
                ["plannedActivityResourceDescription"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.PlannedActivityResourceDescription },
                ["plannedActivityResourceId"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.PlannedActivityResourceId },
                ["exportable"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.Exportable },
                ["forDesignAspect"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ForDesignAspect },
                ["designAspectExportable"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.DesignAspectExportable },
                ["designAspectHardware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.DesignAspectHardware },
                ["designAspectSoftware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.DesignAspectSoftware },
                ["designAspectLabelHardware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.DesignAspectLabelHardware },
                ["designAspectLabelSoftware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.DesignAspectLabelSoftware },
                ["activityDetailsDesignAspect"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ActivityDetailsDesignAspect },
                ["driverTextDesignAspect"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toDriverDescription(false, true, false, false) },
                ["benefitTextDesignAspect"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toBenefitDescription(false, true, false, false) },
                ["planningRisksDesignAspect"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toPlanningRiskDescription(false, true, false, false) },

                ["jsonFormResource"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.JsonForm },
                ["activityDetailsAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ActivityDetailsAddAsset },
                ["activityDetailsForVirtualizedAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ActivityDetailsForVirtualizedAddAsset },
                ["forAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ForAddAsset },
                ["plannedDesignComponentRequiredAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.PlannedDesignComponentRequiredAddAsset },
                ["addAssetHardware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.AddAssetHardware },
                ["addAssetSoftware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.AddAssetSoftware },
                ["addAssetLabelHardware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.AddAssetLabelHardware },
                ["addAssetLabelSoftware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.AddAssetLabelSoftware },
                ["ruleAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.RuleAddAsset },
                ["driverTextAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toDriverDescription(false, false, true, false) },
                ["benefitTextAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toBenefitDescription(false, false, true, false) },
                ["planningRisksAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toPlanningRiskDescription(false, false, true, false) },
                ["onBareMetalAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.OnBareMetalAddAsset },
                ["onVirtualizedAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.OnVirtualizedAddAsset },
                ["forCreateAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ForCreateAddAsset },
                ["forEditAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ForEditAddAsset },
                ["ruleActicvityDetailsAddAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.RuleActicvityDetailsAddAsset },


                ["activityDetailsEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ActivityDetailsEditAsset },
                ["activityDetailsForVirtualizedEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ActivityDetailsForVirtualizedEditAsset },
                ["forEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ForEditAsset },
                ["plannedDesignComponentRequiredEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.PlannedDesignComponentRequiredEditAsset },
                ["editAssetHardware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.EditAssetHardware },
                ["editAssetSoftware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.EditAssetSoftware },
                ["editAssetLabelHardware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.EditAssetLabelHardware },
                ["editAssetLabelSoftware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.EditAssetLabelSoftware },
                ["ruleEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.RuleEditAsset },
                ["driverTextEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toDriverDescription(false, false, false, true) },
                ["benefitTextEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toBenefitDescription(false, false, false, true) },
                ["planningRisksEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toPlanningRiskDescription(false, false, false, true) },
                ["onBareMetalEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.OnBareMetalEditAsset },
                ["onVirtualizedEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.OnVirtualizedEditAsset },
                ["forCreateEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ForCreateEditAsset },
                ["forEditEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ForEditEditAsset },
                ["ruleActicvityDetailsEditAsset"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.RuleActicvityDetailsEditAsset },

                ["activityDetailsLcm"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ActivityDetailsLcm },
                ["forLcm"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ForLcm },
                ["lcmHardware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.LcmHardware },
                ["lcmSoftware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.LcmSoftware },
                ["lcmLabelHardware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.LcmLabelHardware },
                ["lcmLabelSoftware"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.LcmLabelSoftware },

                ["ruleActicvityDetails"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.RuleActicvityDetails },
                ["ruleLinkedDc"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.RuleLinkedDc },
              
                ["driverTextLcm"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toDriverDescription(true, false, false, false) },
                ["benefitTextLcm"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toBenefitDescription(true, false, false, false) },
                ["planningRisksLcm"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.toPlanningRiskDescription(true, false, false, false) },

                ["lastModifiedBy"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ModificationUserEntity.Email },
                ["lastModifiedBy"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.ModificationUserEntity.Email },
                ["ruleLinkedDcPlannedActivityTypeDescription"] = new Expression<Func<PlannedActivityResource, object>>[] { p => p.RuleLinkedDcNavigation.PlannedActivityTypeDescription},
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<PlannedActivityResource> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "activityDetailsDesignAspect" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.ActivityDetailsDesignAspect))
                 : request.Where(x => x.ActivityDetailsDesignAspect.Contains(propertyFilter)).Select(x => new FilterValueDto(x.ActivityDetailsDesignAspect)),

                "plannedActivityResourceDescription" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.PlannedActivityResourceDescription))
                : request.Where(x => x.PlannedActivityResourceDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.PlannedActivityResourceDescription)),
                "plannedActivityResourceId" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.PlannedActivityResourceId.ToString()))
                : request.Where(x => x.PlannedActivityResourceId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.PlannedActivityResourceId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

                "driverTextAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForAddAsset == true).SelectMany(x => x.PlannedActivityResourceDriver).Select(d => new FilterValueDto(d.DriverId, d.Driver.DriverDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toDriverDescription(false, false, true, false).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForAddAsset == true)
                        .SelectMany(x => x.PlannedActivityResourceDriver).Select(d => new FilterValueDto(d.DriverId, d.Driver.DriverDescription)).Distinct().ToList(),
                "benefitTextAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForAddAsset == true).SelectMany(x => x.PlannedActivityResourceBenefit).Select(d => new FilterValueDto(d.BenefitId, d.Benefit.BenefitDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toBenefitDescription(false, false, true, false).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForAddAsset == true)
                        .SelectMany(x => x.PlannedActivityResourceBenefit).Select(d => new FilterValueDto(d.BenefitId, d.Benefit.BenefitDescription)).Distinct().ToList(),
                "planningRisksAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForAddAsset == true).SelectMany(x => x.PlannedActivityResourcePlanningRisk).Select(d => new FilterValueDto(d.PlanningRiskId, d.PlanningRisk.PlanningRiskDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toPlanningRiskDescription(false, false, true, false).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForAddAsset == true)
                        .SelectMany(x => x.PlannedActivityResourcePlanningRisk).Select(d => new FilterValueDto(d.PlanningRiskId, d.PlanningRisk.PlanningRiskDescription)).Distinct().ToList(),


                "driverTextEditAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForEditAsset == true).SelectMany(x => x.PlannedActivityResourceDriver).Select(d => new FilterValueDto(d.DriverId, d.Driver.DriverDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toDriverDescription(false, false, false, true).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForEditAsset == true)
                        .SelectMany(x => x.PlannedActivityResourceDriver).Select(d => new FilterValueDto(d.DriverId, d.Driver.DriverDescription)).Distinct().ToList(),
                "benefitTextEditAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForEditAsset == true).SelectMany(x => x.PlannedActivityResourceBenefit).Select(d => new FilterValueDto(d.BenefitId, d.Benefit.BenefitDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toBenefitDescription(false, false, false, true).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForEditAsset == true)
                        .SelectMany(x => x.PlannedActivityResourceBenefit).Select(d => new FilterValueDto(d.BenefitId, d.Benefit.BenefitDescription)).Distinct().ToList(),
                "planningRisksEditAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForEditAsset == true).SelectMany(x => x.PlannedActivityResourcePlanningRisk).Select(d => new FilterValueDto(d.PlanningRiskId, d.PlanningRisk.PlanningRiskDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toPlanningRiskDescription(false, false, false, true).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForEditAsset == true)
                        .SelectMany(x => x.PlannedActivityResourcePlanningRisk).Select(d => new FilterValueDto(d.PlanningRiskId, d.PlanningRisk.PlanningRiskDescription)).Distinct().ToList(),

                "driverTextLcm" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForLcm == true).SelectMany(x => x.PlannedActivityResourceDriver).Select(d => new FilterValueDto(d.DriverId, d.Driver.DriverDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toDriverDescription(true, false, false, false).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForLcm == true)
                        .SelectMany(x => x.PlannedActivityResourceDriver).Select(d => new FilterValueDto(d.DriverId, d.Driver.DriverDescription)).Distinct().ToList(),
                "benefitTextLcm" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForLcm == true).SelectMany(x => x.PlannedActivityResourceBenefit).Select(d => new FilterValueDto(d.BenefitId, d.Benefit.BenefitDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toBenefitDescription(true, false, false, false).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForLcm == true)
                        .SelectMany(x => x.PlannedActivityResourceBenefit).Select(d => new FilterValueDto(d.BenefitId, d.Benefit.BenefitDescription)).Distinct().ToList(),
                "planningRisksLcm" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForLcm == true).SelectMany(x => x.PlannedActivityResourcePlanningRisk).Select(d => new FilterValueDto(d.PlanningRiskId, d.PlanningRisk.PlanningRiskDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toPlanningRiskDescription(true, false, false, false).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForLcm == true)
                        .SelectMany(x => x.PlannedActivityResourcePlanningRisk).Select(d => new FilterValueDto(d.PlanningRiskId, d.PlanningRisk.PlanningRiskDescription)).Distinct().ToList(),

                "driverTextDesignAspect" => string.IsNullOrEmpty(propertyFilter)
                      ? request.ToList().Where(x => x.ForDesignAspect == true).SelectMany(x => x.PlannedActivityResourceDriver).Select(d => new FilterValueDto(d.DriverId, d.Driver.DriverDescription)).Distinct().ToList()
                      : request.ToList()
                          .Where(x => x.toDriverDescription(false, true, false, false).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForDesignAspect == true)
                          .SelectMany(x => x.PlannedActivityResourceDriver).Select(d => new FilterValueDto(d.DriverId, d.Driver.DriverDescription)).Distinct().ToList(),

                "benefitTextDesignAspect" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForDesignAspect == true).SelectMany(x => x.PlannedActivityResourceBenefit).Select(d => new FilterValueDto(d.BenefitId, d.Benefit.BenefitDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toBenefitDescription(false, true, false, false).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForDesignAspect == true)
                        .SelectMany(x => x.PlannedActivityResourceBenefit).Select(d => new FilterValueDto(d.BenefitId, d.Benefit.BenefitDescription)).Distinct().ToList(),

                "planningRisksDesignAspect" => string.IsNullOrEmpty(propertyFilter)
                    ? request.ToList().Where(x => x.ForDesignAspect == true).SelectMany(x => x.PlannedActivityResourcePlanningRisk).Select(d => new FilterValueDto(d.PlanningRiskId, d.PlanningRisk.PlanningRiskDescription)).Distinct().ToList()
                    : request.ToList()
                        .Where(x => x.toPlanningRiskDescription(false, true, false, false).ToUpper().Contains(propertyFilter.ToUpper()) && x.ForDesignAspect == true)
                        .SelectMany(x => x.PlannedActivityResourcePlanningRisk).Select(d => new FilterValueDto(d.PlanningRiskId, d.PlanningRisk.PlanningRiskDescription)).Distinct().ToList(),


                "activityDetailsAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.ActivityDetailsAddAsset))
                    : request.Where(x => x.ActivityDetailsAddAsset.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ActivityDetailsAddAsset)),
                
                "activityDetailsLcm" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.ActivityDetailsLcm))
                    : request.Where(x => x.ActivityDetailsLcm.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ActivityDetailsLcm)),
                
                "activityDetailsForVirtualizedAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.ActivityDetailsForVirtualizedAddAsset))
                    : request.Where(x => x.ActivityDetailsForVirtualizedAddAsset.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ActivityDetailsForVirtualizedAddAsset)),

                //"ruleActicvityDetailsAddAsset" => string.IsNullOrEmpty(propertyFilter)
                //    ? request.Select(x => new FilterValueDto(x.RuleActicvityDetailsAddAsset))
                //    : request.Where(x => x.RuleActicvityDetailsAddAsset.ToString() == propertyFilter).Select(x => new FilterValueDto(x.RuleActicvityDetailsAddAsset)),

                "activityDetailsEditAsset" => string.IsNullOrEmpty(propertyFilter)
             ? request.Select(x => new FilterValueDto(x.ActivityDetailsEditAsset))
             : request.Where(x => x.ActivityDetailsEditAsset.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ActivityDetailsEditAsset)),

                "activityDetailsForVirtualizedEditAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.ActivityDetailsForVirtualizedEditAsset))
                    : request.Where(x => x.ActivityDetailsForVirtualizedEditAsset.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ActivityDetailsForVirtualizedEditAsset)),

                "lcmHardware" => string.IsNullOrEmpty(propertyFilter)
                    ? request
                        .Select(p => new FilterValueDto { Text = p.LcmHardware ? "YES" : "NO", Value = p.LcmHardware.ToString() }).Distinct()
                    : request
                        .Where(x => x.LcmHardware == false)
                        .Select(p => new FilterValueDto { Text = p.LcmHardware ? "YES" : "NO", Value = p.LcmHardware.ToString() }).Distinct(),
                "plannedDesignComponentRequiredAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.PlannedDesignComponentRequiredAddAsset != null)
                        .Select(p => new FilterValueDto { Text = p.PlannedDesignComponentRequiredAddAsset.Value ? "YES" : "NO", Value = p.PlannedDesignComponentRequiredAddAsset.ToString() }).Distinct()
                    : request.Where(x => x.PlannedDesignComponentRequiredAddAsset != null)
                        .Where(x => x.PlannedDesignComponentRequiredAddAsset == false)
                        .Select(p => new FilterValueDto { Text = p.PlannedDesignComponentRequiredAddAsset.Value ? "YES" : "NO", Value = p.PlannedDesignComponentRequiredAddAsset.ToString() }).Distinct(),

                "plannedDesignComponentRequiredEditAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.PlannedDesignComponentRequiredEditAsset != null)
                        .Select(p => new FilterValueDto { Text = p.PlannedDesignComponentRequiredEditAsset.Value ? "YES" : "NO", Value = p.PlannedDesignComponentRequiredEditAsset.ToString() }).Distinct()
                    : request.Where(x => x.PlannedDesignComponentRequiredEditAsset != null)
                        .Where(x => x.PlannedDesignComponentRequiredEditAsset == false)
                        .Select(p => new FilterValueDto { Text = p.PlannedDesignComponentRequiredEditAsset.Value ? "YES" : "NO", Value = p.PlannedDesignComponentRequiredEditAsset.ToString() }).Distinct(),
                "lcmSoftware" => string.IsNullOrEmpty(propertyFilter)
                    ? request
                        .Select(p => new FilterValueDto { Text = p.LcmSoftware ? "YES" : "NO", Value = p.LcmSoftware.ToString() }).Distinct()
                    : request
                        .Where(x => x.LcmSoftware == false)
                        .Select(p => new FilterValueDto { Text = p.LcmSoftware ? "YES" : "NO", Value = p.LcmSoftware.ToString() }).Distinct(),

                "lcmLabelSoftware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.LcmLabelSoftware))
                    : request.Where(x => x.LcmLabelSoftware.ToString() == propertyFilter).Select(x => new FilterValueDto(x.LcmLabelSoftware)),
                "lcmLabelHardware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.LcmLabelHardware))
                    : request.Where(x => x.LcmLabelHardware.ToString() == propertyFilter).Select(x => new FilterValueDto(x.LcmLabelHardware)),


                "designAspectSoftware" => string.IsNullOrEmpty(propertyFilter)
                    ? request
                        .Select(p => new FilterValueDto { Text = p.DesignAspectSoftware ? "YES" : "NO", Value = p.DesignAspectSoftware.ToString() }).Distinct()
                    : request
                        .Where(x => x.LcmSoftware == false)
                        .Select(p => new FilterValueDto { Text = p.DesignAspectSoftware ? "YES" : "NO", Value = p.DesignAspectSoftware.ToString() }).Distinct(),

                "designAspectHardware" => string.IsNullOrEmpty(propertyFilter)
                                  ? request
                                      .Select(p => new FilterValueDto { Text = p.DesignAspectHardware ? "YES" : "NO", Value = p.DesignAspectHardware.ToString() }).Distinct()
                                  : request
                                      .Where(x => x.LcmSoftware == false)
                                      .Select(p => new FilterValueDto { Text = p.DesignAspectHardware ? "YES" : "NO", Value = p.DesignAspectHardware.ToString() }).Distinct(),


                "designAspectLabelSoftware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.DesignAspectLabelSoftware))
                    : request.Where(x => x.DesignAspectLabelSoftware.ToString() == propertyFilter).Select(x => new FilterValueDto(x.DesignAspectLabelSoftware)),

                "designAspectLabelHardware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.DesignAspectLabelHardware))
                    : request.Where(x => x.DesignAspectLabelHardware.ToString() == propertyFilter).Select(x => new FilterValueDto(x.DesignAspectLabelHardware)),

                "addAssetHardware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.AddAssetHardware != null)
                        .Select(p => new FilterValueDto { Text = p.AddAssetHardware.Value ? "YES" : "NO", Value = p.AddAssetHardware.ToString() }).Distinct()
                    : request.Where(x => x.AddAssetHardware != null)
                        .Where(x => x.AddAssetHardware == false)
                        .Select(p => new FilterValueDto { Text = p.AddAssetHardware.Value ? "YES" : "NO", Value = p.AddAssetHardware.ToString() }).Distinct(),
                "addAssetSoftware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.AddAssetSoftware != null)
                        .Select(p => new FilterValueDto { Text = p.AddAssetSoftware.Value ? "YES" : "NO", Value = p.AddAssetSoftware.ToString() }).Distinct()
                    : request.Where(x => x.AddAssetSoftware != null)
                        .Where(x => x.AddAssetSoftware == false)
                        .Select(p => new FilterValueDto { Text = p.AddAssetSoftware.Value ? "YES" : "NO", Value = p.AddAssetSoftware.ToString() }).Distinct(),
                "addAssetLabelSoftware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.AddAssetLabelSoftware != null).Select(x => new FilterValueDto(x.AddAssetLabelSoftware))
                    : request.Where(x => x.AddAssetLabelSoftware != null).Where(x => x.AddAssetLabelSoftware.ToString() == propertyFilter).Select(x => new FilterValueDto(x.AddAssetLabelSoftware)),
                "addAssetLabelHardware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.AddAssetLabelHardware != null).Select(x => new FilterValueDto(x.AddAssetLabelHardware))
                    : request.Where(x => x.AddAssetLabelHardware != null).Where(x => x.AddAssetLabelHardware.ToString() == propertyFilter).Select(x => new FilterValueDto(x.AddAssetLabelHardware)),



                "editAssetHardware" => string.IsNullOrEmpty(propertyFilter)
                   ? request.Where(x => x.EditAssetHardware != null)
                       .Select(p => new FilterValueDto { Text = p.EditAssetHardware.Value ? "YES" : "NO", Value = p.EditAssetHardware.ToString() }).Distinct()
                   : request.Where(x => x.EditAssetHardware != null)
                       .Where(x => x.EditAssetHardware == false)
                       .Select(p => new FilterValueDto { Text = p.EditAssetHardware.Value ? "YES" : "NO", Value = p.EditAssetHardware.ToString() }).Distinct(),
                "editAssetSoftware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.EditAssetSoftware != null)
                        .Select(p => new FilterValueDto { Text = p.EditAssetSoftware.Value ? "YES" : "NO", Value = p.EditAssetSoftware.ToString() }).Distinct()
                    : request.Where(x => x.EditAssetSoftware != null)
                        .Where(x => x.EditAssetSoftware == false)
                        .Select(p => new FilterValueDto { Text = p.EditAssetSoftware.Value ? "YES" : "NO", Value = p.EditAssetSoftware.ToString() }).Distinct(),
                "editAssetLabelSoftware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.EditAssetLabelSoftware != null).Select(x => new FilterValueDto(x.EditAssetLabelSoftware))
                    : request.Where(x => x.EditAssetLabelSoftware != null).Where(x => x.EditAssetLabelSoftware.ToString() == propertyFilter).Select(x => new FilterValueDto(x.EditAssetLabelSoftware)),
                "editAssetLabelHardware" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.EditAssetLabelHardware != null).Select(x => new FilterValueDto(x.EditAssetLabelHardware))
                    : request.Where(x => x.EditAssetLabelHardware != null).Where(x => x.EditAssetLabelHardware.ToString() == propertyFilter).Select(x => new FilterValueDto(x.EditAssetLabelHardware)),


                "onBareMetalAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.OnBareMetalAddAsset != null)
                        .Select(p => new FilterValueDto { Text = p.OnBareMetalAddAsset.Value ? "YES" : "NO", Value = p.OnBareMetalAddAsset.ToString() }).Distinct()
                    : request.Where(x => x.EditAssetLabelSoftware != null)
                        .Where(x => x.OnBareMetalAddAsset == false)
                        .Select(p => new FilterValueDto { Text = p.OnBareMetalAddAsset.Value ? "YES" : "NO", Value = p.OnBareMetalAddAsset.ToString() }).Distinct(),
                "onVirtualizedAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.OnVirtualizedAddAsset != null)
                        .Select(p => new FilterValueDto { Text = p.OnVirtualizedAddAsset.Value ? "YES" : "NO", Value = p.OnVirtualizedAddAsset.ToString() }).Distinct()
                    : request.Where(x => x.OnVirtualizedAddAsset != null)
                        .Where(x => x.OnVirtualizedAddAsset == false)
                        .Select(p => new FilterValueDto { Text = p.OnVirtualizedAddAsset.Value ? "YES" : "NO", Value = p.OnVirtualizedAddAsset.ToString() }).Distinct(),

                "onBareMetalEditAsset" => string.IsNullOrEmpty(propertyFilter)
          ? request.Where(x => x.OnBareMetalEditAsset != null)
              .Select(p => new FilterValueDto { Text = p.OnBareMetalEditAsset.Value ? "YES" : "NO", Value = p.OnBareMetalEditAsset.ToString() }).Distinct()
          : request.Where(x => x.OnBareMetalEditAsset != null)
              .Where(x => x.OnBareMetalEditAsset == false)
              .Select(p => new FilterValueDto { Text = p.OnBareMetalEditAsset.Value ? "YES" : "NO", Value = p.OnBareMetalEditAsset.ToString() }).Distinct(),
                "onVirtualizedEditAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.OnVirtualizedEditAsset != null)
                        .Select(p => new FilterValueDto { Text = p.OnVirtualizedEditAsset.Value ? "YES" : "NO", Value = p.OnVirtualizedEditAsset.ToString() }).Distinct()
                    : request.Where(x => x.OnVirtualizedAddAsset != null)
                        .Where(x => x.OnVirtualizedEditAsset == false)
                        .Select(p => new FilterValueDto { Text = p.OnVirtualizedEditAsset.Value ? "YES" : "NO", Value = p.OnVirtualizedEditAsset.ToString() }).Distinct(),


                "exportable" => string.IsNullOrEmpty(propertyFilter)
                               ? request.Select(p => new FilterValueDto
                               { Text = p.Exportable ? "YES" : "NO", Value = p.Exportable.ToString() }).Distinct()
                               : request
                               .Where(x => x.Exportable == false).Select(p =>
                                new FilterValueDto { Text = p.Exportable ? "YES" : "NO", Value = p.Exportable.ToString() }).Distinct(),

                "designAspectExportable" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    { Text = p.DesignAspectExportable ? "YES" : "NO", Value = p.DesignAspectExportable.ToString() }).Distinct()
                    : request
                    .Where(x => x.DesignAspectExportable == false).Select(p =>
                     new FilterValueDto { Text = p.DesignAspectExportable ? "YES" : "NO", Value = p.DesignAspectExportable.ToString() }).Distinct(),

                "forCreateAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.ForCreateAddAsset != null).Select(p => new FilterValueDto
                    { Text = p.ForCreateAddAsset.Value ? "YES" : "NO", Value = p.ForCreateAddAsset.ToString() }).Distinct()
                    : request
                        .Where(x => x.ForCreateAddAsset != null && x.ForCreateAddAsset == false).Select(p =>
                            new FilterValueDto { Text = p.ForCreateAddAsset.Value ? "YES" : "NO", Value = p.ForCreateAddAsset.ToString() }).Distinct(),
                "forEditAddAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.ForEditAddAsset != null).Select(p => new FilterValueDto
                    { Text = p.ForEditAddAsset.Value ? "YES" : "NO", Value = p.ForEditAddAsset.ToString() }).Distinct()
                    : request
                        .Where(x => x.ForEditAddAsset != null && x.ForEditAddAsset == false).Select(p =>
                            new FilterValueDto { Text = p.ForEditAddAsset.Value ? "YES" : "NO", Value = p.ForEditAddAsset.ToString() }).Distinct(),


                "forCreateEditAsset" => string.IsNullOrEmpty(propertyFilter)
        ? request.Where(x => x.ForCreateEditAsset != null).Select(p => new FilterValueDto
        { Text = p.ForCreateEditAsset.Value ? "YES" : "NO", Value = p.ForCreateEditAsset.ToString() }).Distinct()
        : request
            .Where(x => x.ForCreateEditAsset != null && x.ForCreateEditAsset == false).Select(p =>
                new FilterValueDto { Text = p.ForCreateEditAsset.Value ? "YES" : "NO", Value = p.ForCreateEditAsset.ToString() }).Distinct(),
                "forEditEditAsset" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Where(x => x.ForEditEditAsset != null).Select(p => new FilterValueDto
                    { Text = p.ForEditEditAsset.Value ? "YES" : "NO", Value = p.ForEditEditAsset.ToString() }).Distinct()
                    : request
                        .Where(x => x.ForEditEditAsset != null && x.ForEditEditAsset == false).Select(p =>
                            new FilterValueDto { Text = p.ForEditEditAsset.Value ? "YES" : "NO", Value = p.ForEditEditAsset.ToString() }).Distinct(),


                "forAddAsset" => string.IsNullOrEmpty(propertyFilter)
      ? request.Where(x => x.ForAddAsset != null).Select(p => new FilterValueDto
      { Text = p.ForAddAsset.Value ? "YES" : "NO", Value = p.ForAddAsset.ToString() }).Distinct()
      : request
          .Where(x => x.ForAddAsset != null && x.ForAddAsset == false).Select(p =>
              new FilterValueDto { Text = p.ForAddAsset.Value ? "YES" : "NO", Value = p.ForAddAsset.ToString() }).Distinct(),

                "forEditAsset" => string.IsNullOrEmpty(propertyFilter)
       ? request.Where(x => x.ForEditAsset != null).Select(p => new FilterValueDto
       { Text = p.ForEditAsset.Value ? "YES" : "NO", Value = p.ForEditAsset.ToString() }).Distinct()
       : request
           .Where(x => x.ForEditAsset != null && x.ForEditAsset == false).Select(p =>
               new FilterValueDto { Text = p.ForEditAsset.Value ? "YES" : "NO", Value = p.ForEditAsset.ToString() }).Distinct(),


                "forLcm" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto { Text = p.ForLcm ? "YES" : "NO", Value = p.ForLcm.ToString() }).Distinct()
                    : request
                        .Where(x => x.ForLcm == false).Select(p =>
                            new FilterValueDto { Text = p.ForLcm ? "YES" : "NO", Value = p.ForLcm.ToString() }).Distinct(),


                "forDesignAspect" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto { Text = p.ForDesignAspect ? "YES" : "NO", Value = p.ForDesignAspect.ToString() }).Distinct()
                    : request
                        .Where(x => x.ForDesignAspect == false).Select(p =>
                            new FilterValueDto { Text = p.ForDesignAspect ? "YES" : "NO", Value = p.ForDesignAspect.ToString() }).Distinct(),
                "ruleDesignAspect" => string.IsNullOrEmpty(propertyFilter)
                                ? request.Where(p => p.RuleDesignAspect != null).Select(p => new FilterValueDto { Text = p.RuleDesignAspect.Value.ToString(), Value = p.RuleDesignAspect.Value.ToString() }).Distinct()
                                : request.Where(p => p.RuleDesignAspect != null && p.RuleDesignAspect.ToString().Contains(propertyFilter)).Select(p =>
                                        new FilterValueDto { Text = p.RuleDesignAspect.Value.ToString(), Value = p.RuleDesignAspect.Value.ToString() }).Distinct(),

                "jsonFormResource" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.JsonForm))
                    : request.Where(x => x.JsonForm.ToString() == propertyFilter).Select(x => new FilterValueDto(x.JsonForm)),

                "ruleLinkedDcPlannedActivityTypeDescription" => string.IsNullOrEmpty(propertyFilter)
              ? request.Select(x => new FilterValueDto(x.RuleLinkedDcNavigation.PlannedActivityTypeDescription))
              : request.Where(x => x.RuleLinkedDcNavigation.PlannedActivityTypeDescription.ToString()
              == propertyFilter).Select(x => new FilterValueDto(x.RuleLinkedDcNavigation.PlannedActivityTypeDescription)),

                "forServicePlan" =>
                                  string.IsNullOrEmpty(propertyFilter)
                                    ? request
                                        .Select(p => new FilterValueDto
                                        {
                                            Text = p.ForServicePlan.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                            Value = p.ForServicePlan.ToString()
                                        }).Distinct().ToList()
                                    : request
                                        .Where(p => p.ForServicePlan.ToString().Contains(propertyFilter))
                                        .Select(p => new FilterValueDto
                                        {
                                            Text = p.ForServicePlan.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                                            Value = p.ForServicePlan.ToString()
                                        }).Distinct().ToList(),


            };

        }


        public override IQueryable<PlannedActivityResource> PrepareQuery(PlannedActivityResourceQuery request, ExpressionStarter<PlannedActivityResource> predicateResult, ExpressionStarter<Plannedactivityresources> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(oraclePredicateResult)

                : _repositoryWrapper.PlannedActivityResourceRepository.FindAll();

            return query
                .Include(m => m.CreationuserNavigation)
                .Include(m => m.ModificationuserNavigation)
                .Include(x => x.RulelinkeddcNavigation)
                        .Include(x => x.Plannedactivityresourcebenefit).ThenInclude(x => x.Benefit)
                    .Include(x => x.Plannedactivityresourcedriver).ThenInclude(x => x.Driver)
                    .Include(x => x.Plannedactivityresourceplanningrisk).ThenInclude(x => x.Planningrisk)
                .AsEnumerable().Select(p => PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(PlannedActivityResourceDto dto)
        {
            var entityExists = await _repositoryWrapper.PlannedActivityResourceRepository
                .FindByCondition(x =>
                    x.Plannedactivityresource == dto.PlannedActivityResourceDescription &&
                    !x.Deleted.Value
                ).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Plannedactivityresourceid
                };
            }
            PlannedActivityResource entity = new PlannedActivityResource()
            {
                PlannedActivityResourceId = dto.PlannedActivityResourceId,
                PlannedActivityResourceDescription = dto.PlannedActivityResourceDescription,
                Exportable = dto.Exportable,
                DesignAspectExportable = dto.DesignAspectExportable,

                JsonForm = dto.JsonForm,
                ActivityDetailsAddAsset = dto.ActivityDetailsAddAsset,
                ActivityDetailsEditAsset = dto.ActivityDetailsEditAsset,
                ActivityDetailsLcm = dto.ActivityDetailsLcm,
                ActivityDetailsForVirtualizedAddAsset=dto.ActivityDetailsForVirtualizedAddAsset,
                ActivityDetailsForVirtualizedEditAsset=dto.ActivityDetailsForVirtualizedEditAsset,
                ForLcm = dto.ForLcm,

                RuleAddAsset = dto.RuleAddAsset,
                RuleEditAsset = dto.RuleEditAsset,
                RuleActicvityDetailsAddAsset = dto.RuleActicvityDetailsAddAsset,
                RuleActicvityDetailsEditAsset = dto.RuleActicvityDetailsEditAsset,
                PlannedDesignComponentRequiredAddAsset = dto.PlannedDesignComponentRequiredAddAsset,
                PlannedDesignComponentRequiredEditAsset = dto.PlannedDesignComponentRequiredEditAsset,
                ForCreateAddAsset = dto.ForCreateAddAsset,
                ForCreateEditAsset = dto.ForCreateEditAsset,

                ForEditAddAsset = dto.ForEditAddAsset,
                ForEditEditAsset = dto.ForEditEditAsset,

                LcmSoftware = dto.LcmSoftware,
                LcmHardware = dto.LcmHardware,
                LcmLabelSoftware = dto.LcmLabelSoftware,
                LcmLabelHardware = dto.LcmLabelHardware,

                AddAssetHardware = dto.AddAssetHardware,
                AddAssetLabelHardware = dto.AddAssetLabelHardware,
                AddAssetSoftware = dto.AddAssetSoftware,
                AddAssetLabelSoftware = dto.AddAssetLabelSoftware,
                EditAssetHardware = dto.EditAssetHardware,
                EditAssetLabelHardware = dto.EditAssetLabelHardware,
                EditAssetSoftware = dto.EditAssetSoftware,
                EditAssetLabelSoftware = dto.EditAssetLabelSoftware,

                OnBareMetalAddAsset = dto.OnBareMetalAddAsset,
                OnBareMetalEditAsset = dto.OnBareMetalEditAsset,

                OnVirtualizedAddAsset = dto.OnVirtualizedAddAsset,
                OnVirtualizedEditAsset = dto.OnVirtualizedEditAsset,

                RuleActicvityDetails = dto.RuleActicvityDetails,
                RuleLinkedDc = dto.RuleLinkedDc,
                ForDesignAspect = dto.ForDesignAspect ?? false,
                DesignAspectLabelSoftware = dto.DesignAspectLabelSoftware,
                DesignAspectLabelHardware = dto.DesignAspectLabelHardware,
                ActivityDetailsDesignAspect = dto.ActivityDetailsDesignAspect,
                DesignAspectHardware = dto.DesignAspectHardware ?? false,
                DesignAspectSoftware = dto.DesignAspectSoftware ?? false,
                RuleDesignAspect = dto.RuleDesignAspect,
                ForAddAsset = dto.ForAddAsset,
                ForEditAsset = dto.ForEditAsset,
                ForServicePlan = dto.Forserviceplan,
            };
            var plannedActivityResourcePlanningRisk = new List<PlannedActivityResourcePlanningRisk>();
            var plannedActivityResourceBenefit = new List<PlannedActivityResourceBenefit>();
            var plannedActivityResourceDriver = new List<PlannedActivityResourceDriver>();
            if (dto.PlanningRisksAddAsset != null && dto.PlanningRisksAddAsset.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRisksAddAsset.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    PlanningRiskId = x,
                    ForDesignAspect = false,
                    ForEditAsset =false,
                    ForAddAsset =true
                }).ToList());
            }
            if (dto.PlanningRisksAEditAsset != null && dto.PlanningRisksAEditAsset.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRisksAEditAsset.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    PlanningRiskId = x,
                    ForDesignAspect = false,
                    ForEditAsset = true,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.BenefitTextAddAsset != null && dto.BenefitTextAddAsset.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextAddAsset.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    BenefitId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = true
                }).ToList());
            }
            if (dto.BenefitTextEditAsset != null && dto.BenefitTextEditAsset.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextEditAsset.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    BenefitId = x,
                    ForDesignAspect = false,
                    ForEditAsset = true,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.DriverTextAddAsset != null && dto.DriverTextAddAsset.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextAddAsset.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    DriverId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = true
                }).ToList());
            }
            if (dto.DriverTextEditAsset != null && dto.DriverTextEditAsset.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextEditAsset.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    DriverId = x,
                    ForDesignAspect = false,
                    ForEditAsset = true,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.PlanningRisksLcm != null && dto.PlanningRisksLcm.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRisksLcm.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = true,
                    PlanningRiskId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.BenefitTextLcm != null && dto.BenefitTextLcm.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextLcm.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = true,
                    BenefitId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.DriverTextLcm != null && dto.DriverTextLcm.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextLcm.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = true,
                    DriverId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }

            #region Design-Aspects
            if (dto.PlanningRisksDesignAspect != null && dto.PlanningRisksDesignAspect.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRisksDesignAspect.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    PlanningRiskId = x,
                    ForDesignAspect = true,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.BenefitTextDesignAspect != null && dto.BenefitTextDesignAspect.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextDesignAspect.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    BenefitId = x,
                    ForDesignAspect = true,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.DriverTextDesignAspect != null && dto.DriverTextDesignAspect.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextDesignAspect.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = true,
                    DriverId = x,
                    ForDesignAspect = true,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            #endregion
            #region // Serviceplan
            if (dto.PlanningRiskServicePlan != null && dto.PlanningRiskServicePlan.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRiskServicePlan.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    PlanningRiskId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.BenefitTextServicePlan != null && dto.BenefitTextServicePlan.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextServicePlan.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    BenefitId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.DriverTextServicePlan != null && dto.DriverTextServicePlan.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextServicePlan.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    DriverId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            #endregion


            entity.PlannedActivityResourcePlanningRisk = plannedActivityResourcePlanningRisk;
            entity.PlannedActivityResourceDriver = plannedActivityResourceDriver;
            entity.PlannedActivityResourceBenefit = plannedActivityResourceBenefit;
            _repositoryWrapper.PlannedActivityResourceRepository.Create(PlannedActivityResourceMapper.SetPlannedActivityResourceMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(PlannedActivityResourceDto dto)
        {
            PlannedActivityResource entity = new PlannedActivityResource()
            {
                PlannedActivityResourceId = dto.PlannedActivityResourceId,
                PlannedActivityResourceDescription = dto.PlannedActivityResourceDescription,
                Exportable = dto.Exportable,
                DesignAspectExportable = dto.DesignAspectExportable,
                JsonForm = dto.JsonForm,
                ActivityDetailsAddAsset = dto.ActivityDetailsAddAsset,
                ActivityDetailsEditAsset = dto.ActivityDetailsEditAsset,
                ActivityDetailsLcm = dto.ActivityDetailsLcm,
                ActivityDetailsForVirtualizedAddAsset = dto.ActivityDetailsForVirtualizedAddAsset,
                ActivityDetailsForVirtualizedEditAsset = dto.ActivityDetailsForVirtualizedEditAsset,
                PlannedDesignComponentRequiredAddAsset = dto.PlannedDesignComponentRequiredAddAsset,
                PlannedDesignComponentRequiredEditAsset = dto.PlannedDesignComponentRequiredEditAsset,

                RuleAddAsset = dto.RuleAddAsset,
                RuleEditAsset = dto.RuleEditAsset,
                RuleActicvityDetailsAddAsset = dto.RuleActicvityDetailsAddAsset,
                RuleActicvityDetailsEditAsset = dto.RuleActicvityDetailsEditAsset,

                ForCreateAddAsset = dto.ForCreateAddAsset,
                ForCreateEditAsset = dto.ForCreateEditAsset,

                ForEditAddAsset = dto.ForEditAddAsset,
                ForEditEditAsset = dto.ForEditEditAsset,
                ForLcm = dto.ForLcm,
                LcmSoftware = dto.LcmSoftware,
                LcmHardware = dto.LcmHardware,
                LcmLabelSoftware = dto.LcmLabelSoftware,
                LcmLabelHardware = dto.LcmLabelHardware,

                AddAssetHardware = dto.AddAssetHardware,
                AddAssetLabelHardware = dto.AddAssetLabelHardware,
                AddAssetSoftware = dto.AddAssetSoftware,
                AddAssetLabelSoftware = dto.AddAssetLabelSoftware,
                EditAssetHardware = dto.EditAssetHardware,
                EditAssetLabelHardware = dto.EditAssetLabelHardware,
                EditAssetSoftware = dto.EditAssetSoftware,
                EditAssetLabelSoftware = dto.EditAssetLabelSoftware,

                OnBareMetalAddAsset = dto.OnBareMetalAddAsset,
                OnBareMetalEditAsset = dto.OnBareMetalEditAsset,

                OnVirtualizedAddAsset = dto.OnVirtualizedAddAsset,
                OnVirtualizedEditAsset = dto.OnVirtualizedEditAsset,
                RuleActicvityDetails = dto.RuleActicvityDetails,
                RuleLinkedDc = dto.RuleLinkedDc,
                ForDesignAspect = dto.ForDesignAspect ?? false,
                DesignAspectLabelSoftware = dto.DesignAspectLabelSoftware,
                DesignAspectLabelHardware = dto.DesignAspectLabelHardware,
                ActivityDetailsDesignAspect = dto.ActivityDetailsDesignAspect,
                DesignAspectHardware = dto.DesignAspectHardware ?? false,
                DesignAspectSoftware = dto.DesignAspectSoftware ?? false,
                RuleDesignAspect = dto.RuleDesignAspect,
                ForAddAsset = dto.ForAddAsset,
                ForEditAsset = dto.ForEditAsset,
                ForServicePlan = dto.Forserviceplan,
            };
            var plannedActivityResourcePlanningRiskRelations = _repositoryWrapper.PlannedActivityResourcePlanningRisk.FindByCondition(x => x.Plannedactivityresourceid == dto.PlannedActivityResourceId).ToList();
            foreach (var toDelete in plannedActivityResourcePlanningRiskRelations) _repositoryWrapper.PlannedActivityResourcePlanningRisk.DeleteDeep(toDelete);

            var plannedActivityResourceBenefitRelations = _repositoryWrapper.PlannedActivityResourceBenefit.FindByCondition(x => x.Plannedactivityresourceid == dto.PlannedActivityResourceId).ToList();
            foreach (var toDelete in plannedActivityResourceBenefitRelations) _repositoryWrapper.PlannedActivityResourceBenefit.DeleteDeep(toDelete);

            var plannedActivityResourceDriverRelations = _repositoryWrapper.PlannedActivityResourceDriver.FindByCondition(x => x.Plannedactivityresourceid == dto.PlannedActivityResourceId).ToList();
            foreach (var toDelete in plannedActivityResourceDriverRelations) _repositoryWrapper.PlannedActivityResourceDriver.DeleteDeep(toDelete);

            var plannedActivityResourcePlanningRisk = new List<PlannedActivityResourcePlanningRisk>();
            var plannedActivityResourceBenefit = new List<PlannedActivityResourceBenefit>();
            var plannedActivityResourceDriver = new List<PlannedActivityResourceDriver>();

            //PlanningRisk
            if (dto.PlanningRisksAddAsset != null && dto.PlanningRisksAddAsset.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRisksAddAsset.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    PlanningRiskId = x,
                    ForDesignAspect = false,
                    ForAddAsset = true,
                    ForEditAsset = false
                }).ToList());
            }
            if (dto.PlanningRisksAEditAsset != null && dto.PlanningRisksAEditAsset.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRisksAEditAsset.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    PlanningRiskId = x,
                    ForDesignAspect = false,
                    ForAddAsset = false,
                    ForEditAsset = true
                }).ToList());
            }
            //Benefit
            if (dto.BenefitTextAddAsset != null && dto.BenefitTextAddAsset.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextAddAsset.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    BenefitId = x,
                    ForDesignAspect = false,
                    ForAddAsset = true,
                    ForEditAsset = false
                }).ToList());
            }
            if (dto.BenefitTextEditAsset != null && dto.BenefitTextEditAsset.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextEditAsset.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    BenefitId = x,
                    ForDesignAspect = false,
                    ForAddAsset = false,
                    ForEditAsset = true
                }).ToList());
            }
            //Driver
            if (dto.DriverTextAddAsset != null && dto.DriverTextAddAsset.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextAddAsset.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    DriverId = x,
                    ForDesignAspect = false,
                    ForAddAsset = true,
                    ForEditAsset = false
                }).ToList());
            }
            if (dto.DriverTextEditAsset != null && dto.DriverTextEditAsset.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextEditAsset.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    DriverId = x,
                    ForDesignAspect = false,
                    ForAddAsset= false,
                    ForEditAsset = true
                }).ToList());
            }
            //PlanningRisk
            if (dto.PlanningRisksLcm != null && dto.PlanningRisksLcm.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRisksLcm.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = true,
                    PlanningRiskId = x,
                    ForDesignAspect = false,
                    ForAddAsset = false,
                    ForEditAsset = false
                }).ToList());
            }
            //Benefit
            if (dto.BenefitTextLcm != null && dto.BenefitTextLcm.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextLcm.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = true,
                    BenefitId = x,
                    ForDesignAspect = false,
                    ForAddAsset = false,
                    ForEditAsset = false
                }).ToList());
            }
            //Driver
            if (dto.DriverTextLcm != null && dto.DriverTextLcm.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextLcm.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = true,
                    DriverId = x,
                    ForDesignAspect = false,
                    ForAddAsset = false,
                    ForEditAsset = false
                }).ToList());
            }


            #region Design-Aspects
            if (dto.PlanningRisksDesignAspect != null && dto.PlanningRisksDesignAspect.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRisksDesignAspect.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    PlanningRiskId = x,
                    ForDesignAspect = true,
                    ForAddAsset = false,
                    ForEditAsset = false
                }).ToList());
            }
            if (dto.BenefitTextDesignAspect != null && dto.BenefitTextDesignAspect.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextDesignAspect.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    BenefitId = x,
                    ForDesignAspect = true,
                    ForAddAsset=false,
                    ForEditAsset=false
                }).ToList());
            }
            if (dto.DriverTextDesignAspect != null && dto.DriverTextDesignAspect.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextDesignAspect.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    DriverId = x,
                    ForDesignAspect = true,
                    ForAddAsset = false,
                    ForEditAsset = false
                }).ToList());
            }
            #endregion

            #region // Serviceplan
            if (dto.PlanningRiskServicePlan != null && dto.PlanningRiskServicePlan.Count > 0)
            {
                plannedActivityResourcePlanningRisk.AddRange(dto.PlanningRiskServicePlan.Select(x => new PlannedActivityResourcePlanningRisk
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    PlanningRiskId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.BenefitTextServicePlan != null && dto.BenefitTextServicePlan.Count > 0)
            {
                plannedActivityResourceBenefit.AddRange(dto.BenefitTextServicePlan.Select(x => new PlannedActivityResourceBenefit
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    BenefitId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            if (dto.DriverTextServicePlan != null && dto.DriverTextServicePlan.Count > 0)
            {
                plannedActivityResourceDriver.AddRange(dto.DriverTextServicePlan.Select(x => new PlannedActivityResourceDriver
                {
                    PlannedActivityResourceId = dto.PlannedActivityResourceId,
                    ForLcm = false,
                    DriverId = x,
                    ForDesignAspect = false,
                    ForEditAsset = false,
                    ForAddAsset = false
                }).ToList());
            }
            #endregion

            entity.PlannedActivityResourcePlanningRisk = plannedActivityResourcePlanningRisk;
            entity.PlannedActivityResourceDriver = plannedActivityResourceDriver;
            entity.PlannedActivityResourceBenefit = plannedActivityResourceBenefit;
            var model = PlannedActivityResourceMapper.SetPlannedActivityResourceMapper(entity);
            _repositoryWrapper.PlannedActivityResourceRepository.Update(model);
            await _repositoryWrapper.SaveAsync();
            foreach (var item in model.Plannedactivityresourcebenefit)
            {
                _repositoryWrapper.PlannedActivityResourceBenefit.Create(item);
            }
            foreach (var item in model.Plannedactivityresourcedriver)
            {
                _repositoryWrapper.PlannedActivityResourceDriver.Create(item);
            }
            foreach (var item in model.Plannedactivityresourceplanningrisk)
            {
                _repositoryWrapper.PlannedActivityResourcePlanningRisk.Create(item);
            }            
            var PAs = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityresourceid == dto.PlannedActivityResourceId)
                .Include(x => x.Designcomponent)
                .Include(x => x.Plannedactivityresource);

            foreach (var plannedActivity in PAs)
            {
                if (plannedActivity?.Plannedactivityresource?.Ruleacticvitydetails != null && plannedActivity?.Designcomponent != null)
                {
                    var result = await _plannedActivityManager.GetActivityDetailsFromLcmRule(plannedActivity.Designcomponent.Designcomponentid, plannedActivity.Plannedactivityresource.Ruleacticvitydetails);
                    plannedActivity.Activitydetails = result;                    
                    plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                    _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                    if (plannedActivity.Deliveryplanavailable)
                    {
                        var deliveryTracking = new DeliveryTrackingDtoCreate()
                        {
                            PlannedActivityId = plannedActivity.Plannedactivityid
                        };
                        await _deliveryTrackingManager.Add(deliveryTracking);
                    }
                }
            }
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == id).SingleAsync();
            _repositoryWrapper.PlannedActivityResourceRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Plannedactivityresourceid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == id)
                 .Include(x => x.Plannedactivityresourceplanningrisk)
                 .Include(x => x.Plannedactivityresourcebenefit)
                 .Include(x => x.Plannedactivityresourcedriver)
                 .SingleAsync();

            var plannedActivityResourcePlanningRiskToDelete = entity.Plannedactivityresourceplanningrisk.ToList();
            foreach (var plannedActivityResourcePlanningRisk in plannedActivityResourcePlanningRiskToDelete)
            {
                _repositoryWrapper.PlannedActivityResourcePlanningRisk.DeleteDeep(plannedActivityResourcePlanningRisk);
            }
            var plannedActivityResourceDriverToDelete = entity.Plannedactivityresourcedriver.ToList();
            foreach (var plannedActivityResourceDriver in plannedActivityResourceDriverToDelete)
            {
                _repositoryWrapper.PlannedActivityResourceDriver.DeleteDeep(plannedActivityResourceDriver);
            }
            var plannedActivityResourceBenefitToDelete = entity.Plannedactivityresourcebenefit.ToList();
            foreach (var plannedActivityResourceBenefit in plannedActivityResourceBenefitToDelete)
            {
                _repositoryWrapper.PlannedActivityResourceBenefit.DeleteDeep(plannedActivityResourceBenefit);
            }
            _repositoryWrapper.PlannedActivityResourceRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Plannedactivityresourceid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {

            var plannedActivity = _repositoryWrapper.PlannedActivity
                                    .FindByCondition(x => x.Plannedactivityresourceid == id)
                                    .Include(x => x.Plannedactivityresource)
                                    .Include(x => x.Activitystatus)
                                    .Include(x => x.Deliverystatus)
                                    .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                                    .ToArray();

            var deploymentStatus = _repositoryWrapper.DeploymentStatus
                .FindAll().ToList()
                .Where(x => DeploymentStatusMapper.GetDeploymentStatusMapper(x).toPlannedActivityResourceKeyList() != null && DeploymentStatusMapper.GetDeploymentStatusMapper(x).toPlannedActivityResourceKeyList().Contains(id))
                .Select(x => x.Deploymentstatus).ToArray();


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (plannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivity });

            if (deploymentStatus.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Deployment Status", Values = deploymentStatus });

            var entity = await _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Plannedactivityresourceid == id).SingleAsync();


            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Planned Activity Resource",
                        RecordName = entity.Plannedactivityresource,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public PlannedActivityResourceDto GetCreatePage()
        {
            var dto = new PlannedActivityResourceDto()
            {
                ActivityDetailsResource = _repositoryWrapper.ActivityDetails.FindAll()
                .Select(p => ActiviyDetailsMapper.GetActivityDetailsMapper(p)).ToDictionary(x => x.ActivityDetailsId, x => new TipologicaGridDtoForVirtualized()
                {
                    Id = (short)x.ActivityDetailsId,
                    Description = x.ActivityDetailsDescription,
                    ForVirtualized = x.ForVirtualized
                }),
                BenefitResource = _repositoryWrapper.Benefit.FindAll().ToDictionary(x => x.Benefitid, x => x.Benefit),
                DriverResource = _repositoryWrapper.Driver.FindAll().ToDictionary(x => x.Driverid, x => x.Driver),
                PlanningRiskResource = _repositoryWrapper.PlanningRisk.FindAll().ToDictionary(x => x.Planningriskid, x => x.Planningrisk),

            };
            return dto;
        }

        public PlannedActivityResourceDto GetUpdatePage(short id)
        {
            var entity = _repositoryWrapper.PlannedActivityResourceRepository
                    .FindByCondition(x => x.Plannedactivityresourceid == id)
                    .Include(x => x.Plannedactivityresourceplanningrisk).ThenInclude(x => x.Planningrisk)
                    .Include(x => x.Plannedactivityresourcebenefit).ThenInclude(x => x.Benefit)
                    .Include(x => x.Plannedactivityresourcedriver).ThenInclude(x => x.Driver)
                    .Include(x => x.ModificationuserNavigation)
                    .Select(p => PlannedActivityResourceMapper.GetPlannedActivityResourceMapper(p))
                    .Single();



            var dto = new PlannedActivityResourceDto()
            {
                PlannedActivityResourceId = (short)entity.PlannedActivityResourceId,
                PlannedActivityResourceDescription = entity.PlannedActivityResourceDescription,
                LastModified = entity.ModificationDate,
                Exportable = entity.Exportable,
                DesignAspectExportable = entity.Exportable,
                JsonForm = entity.JsonForm,
                ActivityDetailsAddAsset = entity.ActivityDetailsAddAsset,
                ActivityDetailsEditAsset = entity.ActivityDetailsEditAsset,

                ActivityDetailsLcm = entity.ActivityDetailsLcm == null ? entity.ActivityDetailsForVirtualizedAddAsset : entity.ActivityDetailsLcm,
                ActivityDetailsForVirtualizedAddAsset = entity.ActivityDetailsForVirtualizedAddAsset,
                ActivityDetailsForVirtualizedEditAsset = entity.ActivityDetailsForVirtualizedEditAsset,
                PlannedDesignComponentRequiredAddAsset = entity.PlannedDesignComponentRequiredAddAsset,
                PlannedDesignComponentRequiredEditAsset = entity.PlannedDesignComponentRequiredEditAsset,

                ForLcm = entity.ForLcm,
                LcmSoftware = entity.LcmSoftware,
                LcmHardware = entity.LcmHardware,
                LcmLabelHardware = entity.LcmLabelHardware,
                LcmLabelSoftware = entity.LcmLabelSoftware,

                AddAssetHardware = entity.AddAssetHardware,
                AddAssetLabelHardware = entity.AddAssetLabelHardware,
                AddAssetSoftware = entity.AddAssetSoftware,
                AddAssetLabelSoftware = entity.AddAssetLabelSoftware,
                EditAssetHardware = entity.EditAssetHardware,
                EditAssetLabelHardware = entity.EditAssetLabelHardware,
                EditAssetSoftware = entity.EditAssetSoftware,
                EditAssetLabelSoftware = entity.EditAssetLabelSoftware,

                RuleAddAsset = entity.RuleAddAsset,
                RuleEditAsset = entity.RuleEditAsset,

                RuleActicvityDetailsAddAsset = entity.RuleActicvityDetailsAddAsset,
                RuleActicvityDetailsEditAsset = entity.RuleActicvityDetailsEditAsset,
                
                DriverTextAddAsset = entity.getSelectedDrivers(false, false, true, false),
                BenefitTextAddAsset = entity.getSelectedBenefits(false, false, true, false),
                PlanningRisksAddAsset = entity.getSelectedPlanningRisks(false, false, true, false),

                DriverTextEditAsset = entity.getSelectedDrivers(false, false, false, true),
                BenefitTextEditAsset = entity.getSelectedBenefits(false, false, false, true),
                PlanningRisksAEditAsset = entity.getSelectedPlanningRisks(false, false, false, true),

                DriverTextLcm = entity.getSelectedDrivers(true, false, false, false),
                BenefitTextLcm = entity.getSelectedBenefits(true, false, false, false),
                PlanningRisksLcm = entity.getSelectedPlanningRisks(true, false, false, false),

                DriverTextDesignAspect = entity.getSelectedDrivers(false, true, false, false),
                BenefitTextDesignAspect = entity.getSelectedBenefits(false, true, false, false),
                PlanningRisksDesignAspect = entity.getSelectedPlanningRisks(false, true, false, false),

                DriverTextServicePlan = entity.getSelectedDrivers(false, false, false, false,true),
                BenefitTextServicePlan = entity.getSelectedBenefits(false, false, false, false, true),
                PlanningRiskServicePlan = entity.getSelectedPlanningRisks(false, false, false, false, true),

                ForCreateAddAsset = entity.ForCreateAddAsset,
                ForCreateEditAsset = entity.ForCreateEditAsset,

                ForEditAddAsset = entity.ForEditAddAsset,
                ForEditEditAsset = entity.ForEditEditAsset,

                OnBareMetalAddAsset = entity.OnBareMetalAddAsset,
                OnBareMetalEditAsset = entity.OnBareMetalEditAsset,

                OnVirtualizedAddAsset = entity.OnVirtualizedAddAsset,
                OnVirtualizedEditAsset = entity.OnVirtualizedEditAsset,
                RuleActicvityDetails = entity.RuleActicvityDetails,
                RuleLinkedDc = entity.RuleLinkedDc,
                LastModifiedBy = entity.ModificationUserEntity.Email,
                ForDesignAspect = entity.ForDesignAspect,
                DesignAspectLabelSoftware = entity.DesignAspectLabelSoftware,
                DesignAspectLabelHardware = entity.DesignAspectLabelHardware,
                ActivityDetailsDesignAspect = entity.ActivityDetailsDesignAspect,
                DesignAspectHardware = entity.DesignAspectHardware,
                DesignAspectSoftware = entity.DesignAspectSoftware,
                RuleDesignAspect = entity.RuleDesignAspect,
                ActivityDetailsResource = _repositoryWrapper.ActivityDetails.FindAll().ToDictionary(x => x.Activitydetailsid, x => new TipologicaGridDtoForVirtualized()
                {
                    Id = (short)x.Activitydetailsid,
                    Description = x.Description,
                    ForVirtualized = x.Forvirtualized
                }),
                BenefitResource = _repositoryWrapper.Benefit.FindAll().ToDictionary(x => x.Benefitid, x => x.Benefit),
                DriverResource = _repositoryWrapper.Driver.FindAll().ToDictionary(x => x.Driverid, x => x.Driver),
                PlanningRiskResource = _repositoryWrapper.PlanningRisk.FindAll().ToDictionary(x => x.Planningriskid, x => x.Planningrisk),
                ForAddAsset = entity.ForAddAsset,
                ForEditAsset = entity.ForEditAsset,
                Forserviceplan = entity.ForServicePlan,
            };

            return dto;
        }


    }
}
