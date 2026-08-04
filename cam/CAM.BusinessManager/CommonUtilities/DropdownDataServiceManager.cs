using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Common;
using CAM.DataTransferObjects.Entita.ComponentSoftware.MappedComponentBuildBag;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.LookUp.AspnetUserRoleModuleMapping;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models.RBAC;
using CAM.Enum;
using CAM.Identity;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CAM.BusinessManager.CommonUtilities
{
    public class DropdownDataServiceManager : BaseManager
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        public readonly CommonManager _commonManager;
        public DropdownDataServiceManager(CommonManager commonManager,IEnumerable<IRepositoryWrapper> wrappers, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _commonManager = commonManager;
        }

        #region // All  opcos drop down

        public async Task<ResultDto> GetOpcos(bool dic = false, bool filterdto = false, bool keyValuePair = false, bool removeZOpco = false ,List<short> opcoList=null)
        {
            var allOpcos = opcoList!=null && opcoList.Count>0?
                await _repositoryWrapper.OpCo.FindByCondition(x=>opcoList.Contains(x.Opcoid)).Select(x => new
                {
                    x.Opco,
                    x.Opcoid
                })?.Distinct().OrderBy(x => x.Opco).ToListAsync()
                : await _repositoryWrapper.OpCo.FindAll().Select(x => new
                {
                    x.Opco,
                    x.Opcoid
                })?.Distinct().OrderBy(x => x.Opco).ToListAsync();

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data =
                dic ? allOpcos.ToDictionary(x => (long)x.Opcoid, x => x.Opco.ToString()).OrderBy(x => x.Value) :

                filterdto ? allOpcos.Select(m => new FilterValueDto { Text = m.Opco, Value = m.Opcoid.ToString() }).OrderBy(x => x.Text) :

                keyValuePair && removeZOpco ? allOpcos.Where(x=>x.Opco.ToLower().Trim() != "zzz").Select(m => new DropdownKeyValueList { Key = m.Opcoid, Value = m.Opco }).OrderBy(x => x.Value) :

                keyValuePair && !removeZOpco ? allOpcos.Select(m => new DropdownKeyValueList { Key = m.Opcoid, Value = m.Opco }).OrderBy(x => x.Value) :

                allOpcos.OrderBy(x => x.Opco)
            };
        }

        public async Task<Dictionary<long, string>> GetAllOpcos(List<short> opcoList=null)
        {
            var allOpcosDictornary = opcoList!=null && opcoList.Count>0 ? await _repositoryWrapper.OpCo.FindByConditionWithDelete(x=>opcoList.Contains(x.Opcoid))
                .ToDictionaryAsync(
                x => (long)x.Opcoid, x => x.Opco.ToString()):

            await _repositoryWrapper.OpCo.FindAll().ToDictionaryAsync(
                x => (long)x.Opcoid, x => x.Opco.ToString());
            return allOpcosDictornary;
        }
        #endregion

        #region // product Filter and drop down

        public async Task<ResultDto> GetAllProducts(bool dic = false, bool filterdto = false, bool keyValuePair = false)
        {
            var allProductsDictornary = await _repositoryWrapper.ProductNameRepository.FindAll().Select(
                x => new
                {
                    x.Productnameid,
                    x.Description
                })?.Distinct().OrderBy(x => x.Description).ToListAsync();
            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data =
                           dic ? allProductsDictornary.ToDictionary(x => (long)x.Productnameid, x => x.Description.ToString()).OrderBy(x => x.Value) :

                           filterdto ? allProductsDictornary.Select(m => new FilterValueDto { Text = m.Description, Value = m.Productnameid.ToString() }).OrderBy(x => x.Text) :

                           keyValuePair ? allProductsDictornary.Select(m => new DropdownKeyValueList { Key = (short)m.Productnameid, Value = m.Description }).OrderBy(x => x.Value) :

                           allProductsDictornary.OrderBy(x => x.Description)
            };
        }

        #endregion

        #region // Environment drop down
        public async Task<List<DropdownKeyValueList>> GetEnvironement(
            bool isDeletedRequired = false,
            bool isIncludeTestAndProd = false)
        {
            var query = _repositoryWrapper.Environment.FindAll(isDeletedRequired);

            //This Condition is for the Exodus Environment dropdown production and test
            if (isIncludeTestAndProd)
            {
                query = query.Where(env =>
                    ConstantValueFilter.ProductionAndTest.Contains(env.Environment.ToLower()));
            }

            return await query
                .Select(x => new DropdownKeyValueList
                {
                    Key = x.Environmentid,
                    Value = x.Environment
                })
                .OrderBy(x => x.Value)
                .ToListAsync();
        }
        #endregion

        #region // Deployment dorp down
        public async Task<List<DropdownKeyValueList>> GetAssetDeploymentStatus(bool isDeletedRequired = false)
        {
            var deploymentStatus = await _repositoryWrapper.DeploymentStatus.FindAll(isDeletedRequired).Select(x =>
            new DropdownKeyValueList { Key = x.Deploymentstatusid, Value = x.Deploymentstatus }).Distinct().OrderBy(x => x.Value).ToListAsync();
            return deploymentStatus;
        }

        #endregion

        #region // Supported service filter and drop down
        public async Task<ResultDto> GetAllSupportedServices(bool dic = false, bool filterdto = false, bool keyValuePair = false)
        {
            var allSupportedServicesDictornary = await _repositoryWrapper.SupportedServiceRepository.FindAll()
                .Select(x => new { x.Id, x.Description })?.Distinct().OrderBy(x => x.Description).ToListAsync();


            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data =
                        dic ? allSupportedServicesDictornary.ToDictionary(x => (long)x.Id, x => x.Description.ToString()).OrderBy(x => x.Value) :

                        filterdto ? allSupportedServicesDictornary.Select(m => new FilterValueDto { Text = m.Description, Value = m.Id.ToString() }).OrderBy(x => x.Text) :

                        keyValuePair ? allSupportedServicesDictornary.Select(m => new DropdownKeyValueList { Key = (short)m.Id, Value = m.Description }).OrderBy(x => x.Value) :

                        allSupportedServicesDictornary.OrderBy(x => x.Description)
            };

        }

        #endregion

        #region // Vertical filter and drop down
        public async Task<ResultDto> GetAllVerticalResponse(bool dic = false, bool filterdto = false, bool keyValuePair = false, bool isPageLoad = false, bool isAdmin = false,List<int> verticalList=null)
        {
            var allVerticalResponse = isPageLoad && isAdmin ? await _repositoryWrapper.VerticalResponsible.FindByCondition
                                                              (y => y.Verticalresponsible.ToLower().Trim() == ConstantValueFilter.CsCoreEnablers
                                                              || y.Verticalresponsible.ToLower().Trim() == ConstantValueFilter.CsIms)
                                                             .Select(x => new { x.Verticalresponsibleid, x.Verticalresponsible })?
                                                             .Distinct()
                                                             .OrderBy(x => x.Verticalresponsible)
                                                             .ToListAsync()
                                    :
                                    !isPageLoad && isAdmin ? await _repositoryWrapper.VerticalResponsible.FindAll()
                                                            .Select(x => new { x.Verticalresponsibleid, x.Verticalresponsible })?
                                                            .Distinct()
                                                            .OrderBy(x => x.Verticalresponsible)
                                                            .ToListAsync()
                                    :
                                    !isAdmin && verticalList != null && verticalList.Count > 0 ? await _repositoryWrapper.VerticalResponsible.FindByCondition(
                                                                                                 y => verticalList.Contains(y.Verticalresponsibleid))
                                                                                                 .Select(x => new { x.Verticalresponsibleid, x.Verticalresponsible })?
                                                                                                 .Distinct()
                                                                                                 .OrderBy(x => x.Verticalresponsible)
                                                                                                 .ToListAsync()
                                    : null;


            
            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data =
                     dic ? allVerticalResponse.ToDictionary(x => (long)x.Verticalresponsibleid, x => x.Verticalresponsible.ToString()).OrderBy(x => x.Value) :

                     filterdto ? allVerticalResponse.Select(m => new FilterValueDto { Text = m.Verticalresponsible, Value = m.Verticalresponsibleid.ToString() }).OrderBy(x => x.Text) :

                     keyValuePair ? allVerticalResponse.Select(m => new DropdownKeyValueList { Key = (short)m.Verticalresponsibleid, Value = m.Verticalresponsible }).OrderBy(x => x.Value) :

                     allVerticalResponse.OrderBy(x => x.Verticalresponsible)
            };
        }
        #endregion

        #region // deliveryststus drop down
        public async Task<Dictionary<short, string>> GetDeliveryStatusDic(bool isDeletedRequired = false)
        {
            var deliveryStatus = await _repositoryWrapper.DeliveryStatus.FindAll(isDeletedRequired).ToDictionaryAsync(x => x.Deliverystatusid, x => x.Deliverystatus);
            return deliveryStatus;
        }
        #endregion

        #region // Responsibility drop down
        public async Task<Dictionary<short, string>> GetResponsibilityPhaseDic(bool isDeletedRequired = false)
        {
            var responsibilityPhase = await _repositoryWrapper.ResponsibilityPhase.FindAll(isDeletedRequired).ToDictionaryAsync(x => x.Responsibilityphaseid, x => x.Responsibilityphase);
            return responsibilityPhase;
        }
        #endregion

        #region // Planned activity status Drop down
        public async Task<Dictionary<short, string>> GetPlanningActivityStatusDic(bool isDeletedRequired = false, string activityDesc = "")
        {
            var planningActivityStatus = await _repositoryWrapper.PlanningActivityStatus.FindAll(isDeletedRequired).ToDictionaryAsync(x => x.Planningactivitystatusid, x => x.Planningactivitystatus);
            if (!string.IsNullOrEmpty(activityDesc) && planningActivityStatus.Count > 0)
                return planningActivityStatus.Where(x => x.Value.Replace(" ", "").ToLower() == activityDesc).ToDictionary(x => x.Key, x => x.Value);

            return planningActivityStatus;
        }
        #endregion

        #region // Activity status Drop down
        public async Task<Dictionary<short, string>> GetActivityStatusDic(bool isDeletedRequired = false, string activityDesc = "")
        {
            var activityStatus = await _repositoryWrapper.ActivityStatus.FindAll(isDeletedRequired)
                .ToDictionaryAsync(x => x.Activitystatusid, x => x.Activitystatus);

            if (!string.IsNullOrEmpty(activityDesc) && activityStatus.Count > 0)
                return activityStatus.Where(x => x.Value.Replace(" ", "").ToLower() == activityDesc).ToDictionary(x => x.Key, x => x.Value);

            return activityStatus;
        }
        #endregion

        #region // BudgetAvailability Drop down
        public async Task<Dictionary<short, string>> GetBudgetAvailabilityDic(bool isDeletedRequired = false)
        {
            var budgetAvailability = await _repositoryWrapper.BudgetAvailability.FindAll(isDeletedRequired).ToDictionaryAsync(x => x.Budgetavailabilityid, x => x.Description);
            return budgetAvailability;
        }
        #endregion

        #region // Risk Drop down
        public async Task<Dictionary<short, string>> GetRiskDic(bool isDeletedRequired = false)
        {
            var risk = await _repositoryWrapper.Risk.FindAll(isDeletedRequired).ToDictionaryAsync(x => x.Riskid, x => x.Description);
            return risk;
        }
        #endregion

        #region // Drive drop down
        public async Task<Dictionary<short, string>> GetDriverDic(bool isDeletedRequired = false)
        {
            var driver = await _repositoryWrapper.Driver.FindAll(isDeletedRequired).ToDictionaryAsync(x => x.Driverid, x => x.Driver);
            return driver;
        }

        #endregion

        #region // Benefit Drop down
        public async Task<Dictionary<short, string>> GetBenefitDic(bool isDeletedRequired = false)
        {
            var benefit = await _repositoryWrapper.Benefit.FindAll(isDeletedRequired).ToDictionaryAsync(x => x.Benefitid, x => x.Benefit);
            return benefit;
        }
        #endregion

        #region // PlanningRisk drop down
        public async Task<Dictionary<short, string>> GetPlanningRiskDic(bool isDeletedRequired = false)
        {
            var planningRisk = await _repositoryWrapper.PlanningRisk.FindAll(isDeletedRequired).ToDictionaryAsync(x => x.Planningriskid, x => x.Planningrisk);
            return planningRisk;
        }
        #endregion

        #region // planned activity Resource List based on plannedActivityTypeFor
        public async Task<ResultDto> GetPlannedActivityType(short plannedActivityTypeFor)
        {
            if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.LcmEngineering)
            {
                var lcmPaList = await Task.Run(() => _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Forlcm == ConstantValueFilter.isTrue));

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = lcmPaList.Select(m => new DropdownKeyValueList { Key = m.Plannedactivityresourceid, Value = m.Plannedactivityresource, Id = m.Rulelinkeddc }).OrderBy(m => m.Value)
                };
            }
            else if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.AddAsset)
            {
                var addAssetPaList = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Foraddasset == ConstantValueFilter.isTrue);
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = addAssetPaList.Select(m => new DropdownKeyValueList { Key = m.Plannedactivityresourceid, Value = m.Plannedactivityresource, Id = m.Rulelinkeddc }).OrderBy(m => m.Value)
                };
            }
            else if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.EditAsset)
            {
                var editAssetPaList = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Foreditasset == ConstantValueFilter.isTrue);
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = editAssetPaList.Select(m => new DropdownKeyValueList { Key = m.Plannedactivityresourceid, Value = m.Plannedactivityresource, Id = m.Rulelinkeddc }).OrderBy(m => m.Value)
                };
            }
            else if (plannedActivityTypeFor == (short)PlannedActivityTypeForEnum.DesignAspect)
            {
                var designAspectPaList = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Fordesignaspect == ConstantValueFilter.isTrue);
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false,
                    Data = designAspectPaList.Select(m => new DropdownKeyValueList { Key = m.Plannedactivityresourceid, Value = m.Plannedactivityresource, Id = m.Rulelinkeddc }).OrderBy(m => m.Value)
                };
            }
            else {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoNoFound,
                    Warning = true
                };
            }
        }
        #endregion
        
        #region // ProductImportance drop down
        public async Task<List<DropdownKeyValueList>> GetProductImportances(bool isDeletedRequired = false)
        {
            var productImportant = await _repositoryWrapper.ProductImportance.FindAll(isDeletedRequired).Select(x =>
            new DropdownKeyValueList { Key = x.Productimportanceid, Value = x.Productimportance }).Distinct().OrderBy(x => x.Value).ToListAsync();

            return productImportant;
        }
        #endregion

        #region // SettingsUpdatePlannedActivity drop down
        public async Task<List<DropdownKeyValueList>> GetSettingPlannedActivityBasedDeliveryStatusForPA(ExpressionStarter<Settingsupdateplannedactivity> predicateResult)
        {
            var deliveryStatusDic = await Task.Run(() => _repositoryWrapper.SettingsUpdatePlannedActivity
     .FindByCondition(predicateResult)
     .OrderBy(x => x.Order)
     .Select(x => new DropdownKeyValueList
     {
         Value = x.Deliverystatus.Deliverystatus,
         Key = x.Deliverystatus.Deliverystatusid,
         Id = x.Plannedactivityresource.Rulelinkeddc
     })
     .AsEnumerable() // Switch to LINQ-to-Objects
     .DistinctBy(x => x.Value) // Apply DistinctBy in-memory
     .ToList());



            return deliveryStatusDic;

        }
        #endregion

        #region // Dc based Designcomponentfamily drop down

        public async Task<ResultDto> GetAllDesignComponentFamilyName(DesignComponentFamilyQueryDto buildFilterDto, bool deleted = false)
        {
            try
            {
                var predicateResult = PredicateBuilder.New<Designcomponents>(true);

                var predicateInner = PredicateBuilder.New<Designcomponents>(true);

                if (buildFilterDto != null && (buildFilterDto.SupportedServices != null && buildFilterDto.SupportedServices.Any()))
                {
                    // predicateInner = PredicateBuilder.New<Designcomponents>();
                    foreach (var item in buildFilterDto.SupportedServices)
                        predicateInner.Or(x => x.Designcomponentfamily.Subnetworkboundary.Subnetworksupportedsvr.Any(m => m.Serviceid == item));
                    predicateResult.And(predicateInner);
                }

                var dcfNamesAndIds = await Task.Run(() => _repositoryWrapper.DesignComponent.FindByCondition(predicateResult, deleted)
                       .Include(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Orgeqpmanufacturer)
                       .Include(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Platform)
                       .Include(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction)
                       .Include(p => p.Systemtype).ThenInclude(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Orgeqpmanufacturer)
                       .Include(p => p.Designcomponentfamily).ThenInclude(p => p.Subnetworkboundary).ThenInclude(p => p.Subnetworksupportedsvr)
                       .Include(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                       .Select(x => new DropdownKeyValueList
                       {
                           Description = x.toDesignComponentFamily(),
                           Id = (int)x.Designcomponentfamilyid
                       }).ToList());

                return new ResultDto
                {
                    Data = dcfNamesAndIds.DistinctBy(x => x.Id),
                };


            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = ex.Message + ResultMessages.GetInfoNoFound,

                };
            }
        }
        #endregion

        #region // Location
        public async Task<Dictionary<short, string>> GetLocationDropDown(short? opcoId)
        {
            var resource = await _repositoryWrapper.Location.FindByCondition(x => x.Opcoid == opcoId).ToListAsync();

            var result = resource.DistinctBy(x => x.Locationid).ToDictionary(
                x => x.Locationid, x => x.Location.ToString());
            return result;
        }
        #endregion


        public async Task<ResultDto> GetVendorResource(bool dic = false, bool filterdto = false, bool keyValuePair = false)
        {
            var allVendorResource = await _repositoryWrapper.OriginalEquipmentManufacturer.FindAll().Select(x => new
            {
                x.Orgeqpmanufacturerid,
                x.Originalequipmentmanufacturer
            }).Distinct().OrderBy(x => x.Orgeqpmanufacturerid).ToListAsync();


            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data =
                dic ? allVendorResource.ToDictionary(x => (long)x.Orgeqpmanufacturerid, y => y.Originalequipmentmanufacturer.ToString()).OrderBy(x => x.Value) :

                filterdto ? allVendorResource.Select(m => new FilterValueDto { Text = m.Originalequipmentmanufacturer, Value = m.Orgeqpmanufacturerid.ToString() }).OrderBy(x => x.Text) :

                keyValuePair ? allVendorResource.Select(m => new DropdownKeyValueList { Key = m.Orgeqpmanufacturerid, Value = m.Originalequipmentmanufacturer }).OrderBy(x => x.Value) :

                allVendorResource.OrderBy(x => x.Originalequipmentmanufacturer)
            };

        }

        public async Task<ResultDto> GetPlaftformAndBuildContructionResource(
            bool dic = false,
            bool filterdto = false,
            bool keyValuePair = false,
            bool isPageLoad = false)
        {
            var allPlatformResource = isPageLoad ? await _repositoryWrapper.Platform
                .FindByCondition(x => !ConstantValueFilter.InfraBuildConstructions.Contains(x.Platform.Replace(" ","").ToUpper()))
                .Select(x => new
                {
                    x.Platformid,
                    x.Platform
                }).OrderBy(x => x.Platformid).ToListAsync() 
                : 
                await _repositoryWrapper.Platform
                .FindByCondition(x => ConstantValueFilter.InfraBuildConstructions.Contains(x.Platform.Replace(" ", "").ToUpper()))
                .Select(x => new
                {
                    x.Platformid,
                    x.Platform
                })
                .OrderBy(x => x.Platformid)
                .ToListAsync();

            //var allBuildConstructionResource = isPageLoad ? await _repositoryWrapper.BuildConstruction
            //    .FindByCondition(x => !ConstantValueFilter.InfraBuildConstructions.Contains(x.Buildconstruction))
            //    .Select(x => new
            //    {
            //        x.Buildconstructionid,
            //        x.Buildconstruction
            //    }).OrderBy(x => x.Buildconstructionid).ToListAsync() 
            //    :
            //    await _repositoryWrapper.BuildConstruction
            //    .FindAll()
            //    .Select(x => new
            //    {
            //        x.Buildconstructionid,
            //        x.Buildconstruction
            //    })
            //    .OrderBy(x => x.Buildconstructionid)
            //    .ToListAsync();


            var combinedResource = (
                from p in allPlatformResource
                select new
                {
                    Id = p.Platformid,
                    DisplayText = $"{p.Platform}"
                })
                .DistinctBy(x => x.DisplayText)
                .OrderBy(x => x.DisplayText)
                .ToList();


            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data =
                    dic
                        ? combinedResource.ToDictionary(
                            x => x.Id,
                            y => y.DisplayText)

                    : filterdto
                        ? combinedResource.Select(m => new FilterValueDto
                        {
                            Text = m.DisplayText,
                            Value = m.Id.ToString()
                        })

                    : keyValuePair
                        ? combinedResource.Select(m => new DropdownKeyValueList
                        {
                            Key = m.Id,
                            Value = m.DisplayText
                        })

                    : combinedResource
            };
        }



        public async Task<ResultDto> GetHWBuildConstruction(bool dic = false, bool filterdto = false, bool keyValuePair = false)
        {
            var allBuildResource = await _repositoryWrapper.BuildConstruction.FindAll().Select(x => new
            {
                x.Buildconstructionid,
                x.Buildconstruction
            }).Distinct().OrderBy(x => x.Buildconstruction).ToListAsync();

            if (dic)
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = allBuildResource.ToDictionary(x => (long)x.Buildconstructionid, y => y.Buildconstruction.ToString()).OrderBy(x => x.Value)
                };
            }
            else if (filterdto)
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = allBuildResource.Select(m => new FilterValueDto { Text = m.Buildconstruction, Value = m.Buildconstructionid.ToString() }).OrderBy(x => x.Text)
                };
            }
            else if (keyValuePair)
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = allBuildResource.Select(m => new DropdownKeyValueList { Key = m.Buildconstructionid, Value = m.Buildconstruction }).OrderBy(x => x.Value)
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = allBuildResource.OrderBy(x => x.Buildconstruction)
                };

            }

        }


        public async Task<ResultDto> GetEnvironmentNode(bool dic = false, bool filterdto = false, bool keyValuePair = false)
        {
            var allEnvironmentResource = await _repositoryWrapper.Environment.FindAll().Select(x => new
            {
                x.Environmentid,
                x.Environment
            }).Distinct().OrderBy(x => x.Environment).ToListAsync();

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data =
                dic ? allEnvironmentResource.ToDictionary(x => (long)x.Environmentid, y => y.Environment.ToString()).OrderBy(x => x.Value) :

                filterdto ? allEnvironmentResource.Select(m => new FilterValueDto { Text = m.Environment, Value = m.Environmentid.ToString() }).OrderBy(x => x.Text) :

                keyValuePair ? allEnvironmentResource.Select(m => new DropdownKeyValueList { Key = m.Environmentid, Value = m.Environment }).OrderBy(x => x.Value) :

                allEnvironmentResource.OrderBy(x => x.Environment)
            };
        }
        public async Task<Dictionary<long, string>> GetHWBuildConstruction()
        {
            var allVendorResource = await _repositoryWrapper.BuildConstruction.FindAll().ToDictionaryAsync(
                x => (long)x.Buildconstructionid, x => x.Buildconstruction.ToString());
            return allVendorResource;
        }

        public async Task<List<KeyValuePairDto>> GetAllCriticalAssetType()
        {
            var allProducts = await _repositoryWrapper.CriticalAssetTypeRepository.FindAll().Select(
                x => new KeyValuePairDto
                {
                    Key =  x.Id,
                    Text = x.Description
                })?.Distinct().OrderBy(x => x.Text).ToListAsync();

            return allProducts;

        }
        public async Task<List<KeyValuePairDto>> GetAllOperatingSystem()
        {
            var allOperatingSystem = await _repositoryWrapper.OperatingSystem.FindAll().Select(
                x => new KeyValuePairDto
                {
                    Key = x.Operatingsystemid,
                    Text = x.Operatingsystemname
                })?.Distinct().OrderBy(x => x.Text).ToListAsync();
            return allOperatingSystem;
        }

        public async Task<List<KeyValuePairDto>> GetAllNetworkFunction()
        {
            var allNetworkFunction = await _repositoryWrapper.NetworkFunctionRepository.FindAll().Select(
                x => new KeyValuePairDto
                {
                    Key = x.Id,
                    Text = x.Description
                })?.Distinct().OrderBy(x => x.Text).ToListAsync();
            return allNetworkFunction;
        }
        public async Task<List<KeyValuePairDto>> GetAllOrganisation()
        {
            var allOrganisation = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Id != 1)
                .Select(
             x => new KeyValuePairDto
             {
                 Key =  x.Id,
                 Text = x.Email
             })?.Distinct().OrderBy(x => x.Text).ToListAsync();
            return allOrganisation;
        }

        public List<KeyValuePairDto> GetFilterValueVendorResource()
        {
            var allVendorResource = _repositoryWrapper.OriginalEquipmentManufacturer.FindAll().Select(x => new KeyValuePairDto
            {
                Key = x.Orgeqpmanufacturerid,
                Text = x.Originalequipmentmanufacturer
            })?.Distinct().OrderBy(x => x.Text).ToList();


            return allVendorResource;
        }
        public List<KeyValuePairDto> GetAllPlatorm()
        {
            var allPlatformResource = _repositoryWrapper.Platform.FindAll().Select(x => new KeyValuePairDto
            {
                Key = x.Platformid,
                Text = x.Platform
            })?.Distinct().OrderBy(x => x.Text).ToList();


            return allPlatformResource;
        }
        public List<KeyValuePairDto> GetAllBuildConstruction()
        {
            var allBcResource = _repositoryWrapper.BuildConstruction.FindAll().Select(x => new KeyValuePairDto
            {
                Key = x.Buildconstructionid,
                Text = x.Buildconstruction
            })?.Distinct().OrderBy(x => x.Text).ToList();


            return allBcResource;
        }
        public List<KeyValuePairDto> GetAllHwSolutionResource()
        {
            var allBcResource = _repositoryWrapper.HardwareSolutionResource.FindAll().Select(x => new KeyValuePairDto
            {
                Key = x.Hardwaresolutionresourceid,
                Text = x.Hardwaresolutionreource
            })?.Distinct().OrderBy(x => x.Text).ToList();


            return allBcResource;
        }
        public async Task<List<KeyValuePairDto>> GetFilterValueComponentManufacturersResource()
        {

            var allVendorResource = await _repositoryWrapper.ComponentManufacturersRepository.FindAll().ToListAsync();
            var result = allVendorResource.Select(x => new KeyValuePairDto
            {
                Key = x.Componentmanufacturerid,
                Text = $"{x.Componentmanufacturer}-{x.Componentname}"
            }).Distinct().OrderBy(x => x.Text).ToList();


            return result;
        }       

        public async Task<List<KeyValuePairDto>> GetAllProductFilterValueDto()
        {
            var allOrganisation = await _repositoryWrapper.ProductNameRepository.FindAll()
                .Select(
             x => new KeyValuePairDto
             {
                 Key =  (long) x.Productnameid,
                 Text = x.Description
             })?.Distinct().OrderBy(x => x.Text).ToListAsync();
            return allOrganisation;
        }
        public async Task<Dictionary<long, string>> GetAllVmwareFromMajorSwBuildDto()
        {
          
            var allVmwareFromMajorSwBuild = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Isvmware == true).ToDictionaryAsync(
             x => x.Majorsoftwarebuildsid,
              x => x.Softwareversion.ToString()
           );

            return allVmwareFromMajorSwBuild;
        }
        public async Task<Dictionary<long, string>> GetAllVmwareFromNfvi()
        {
          

            var allNfviEntity = await _repositoryWrapper.NfviSoftwareCompatibilityRepository.FindAll().Include(x => x.Plaftform)
                .ToListAsync();

            var result = allNfviEntity.DistinctBy(x=>x.Plaftformid).ToDictionary(
              x => x.Plaftformid,
              y => y.Plaftform.Softwareversion.ToString()
                );


            return result;
        }
        public async Task<List<ViewComponentSoftwareBuild>> GetComponenetSoftwareDetails()
        {

            var resultComponenetSwBuild = await Task.Run(() => _repositoryWrapper.ComponentSoftwareBuildRepository.FindAll().Include(t => t.Componentmanufacturer)

                .Select(x => new ViewComponentSoftwareBuild
                {
                    DisplayDescription = $"{x.Componentmanufacturer.Componentmanufacturer}-" +
                                         $"{x.Componentmanufacturer.Componentname}-" +
                                         $"{x.Softwareversion}",
                    ComponentSoftwareBuildId = x.Componentsoftwarebuildid,
                    ComponentManufacturerId = (long)x.Componentmanufacturerid,
                }).ToList());

            return resultComponenetSwBuild;

        }

        public async Task<List<KeyValuePairDto>> GetBuildBagDetails()
        {
            var resultBuildBag = await _repositoryWrapper.BuildBagRepository.FindAll()               
                .Select(x => new KeyValuePairDto
                {
                    Text = $"{x.Bagdescription}-{x.Bagversion}",
                    Key =  x.Buildbagid
                }).ToListAsync();

            return resultBuildBag;

        }
        #region // SOS Drop down for opco and DCF
        public async Task<List<KeyValuePairDto>> GetDCFDetails(long dcfId = 0, bool isSpecficDcf = false)
        {
            var predicateResult = PredicateBuilder.New<Designcomponentfamilies>(true);
            var predicateInner = PredicateBuilder.New<Designcomponentfamilies>(true);

            if(isSpecficDcf && dcfId > 0)
            {
                predicateInner.Or(x => x.Designcomponentfamilyid == dcfId);
                predicateResult.And(predicateInner);
            }

            var dcfEntity = await _repositoryWrapper.DesignComponentFamily.FindByCondition(predicateResult).ToListAsync();
            var result = dcfEntity.ToList()
                .Select(x => new KeyValuePairDto { Key = x.Designcomponentfamilyid, Text = x.DCFName(_repositoryWrapper) })
                .Where(x => !string.IsNullOrEmpty(x.Text)).ToList();
                          
            return result;

        }
        public async Task<List<KeyValuePairDto>> GetOpcoDetails(short opCoId = 0, bool isSpecficOpco = false,List<short> opcoList=null)
        {
            var predicateResult = PredicateBuilder.New<Opcos>(true);
            var predicateInner = PredicateBuilder.New<Opcos>(true);

            if (isSpecficOpco && opCoId > 0)
            {
                predicateInner.Or(x => x.Opcoid == opCoId);
                predicateResult.And(predicateInner);
            }

            var opCoEntity = opcoList!=null && opcoList.Any()==true?
                await _repositoryWrapper.OpCo.FindByCondition(x=>opcoList.Contains(x.Opcoid)).ToListAsync()
                :await _repositoryWrapper.OpCo.FindByCondition(predicateResult).ToListAsync();
            var result = opCoEntity    
            .Select(x => new KeyValuePairDto
                {
                    Text = x.Opco,
                    Key =  x.Opcoid
                }).ToList();

            return result;

        }
        #endregion
        public async Task<DesignComponentDtoCreate>  GetSystemType()
        {
            ///1391 - While creating new DC, displaying unknownDesign Component has to be fixed           
            ///Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
            var systemTypes = await _repositoryWrapper.SystemType.FindByCondition(x => x.Majorsoftwarebuilds.Softwareversion.ToLower() != ConstantValueFilter.Unknown)
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                    .Include(x => x.Majorsoftwarebuilds.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponents.Where(x => x.Deleted == false))
                    .OrderByDescending(x => x.Modificationdate)
                    .Select(x => new DictionaryList { Key = x.Systemtypeid, Value = x.SystemTypeNameForDC(_repositoryWrapper) })
                    .ToListAsync();
            var dto = new DesignComponentDtoCreate
            {

                SystemTypeResource = systemTypes,
            };

            return dto;
        }

        #region V-BOM
        public List<KeyValuePairDto> GetVnfName()
        {


            var allVnfName = _repositoryWrapper.VnfNameRepository.FindAll()
     .GroupBy(x => x.Vnfdescription)
     .Select(g => new KeyValuePairDto
     {
         Key =  g.First().Vnfnameid,
         Text = g.Key
     })
     .OrderBy(x => x.Text)
     .ToList();


            return allVnfName;
        }
        public List<KeyValuePairDto> GetVnfClusterName()
        {

            var allVnfClusterName = _repositoryWrapper.ClusterNameRepository.FindAll()
     .GroupBy(x => x.Clusterdescription)
     .Select(g => new KeyValuePairDto
     {
         Key =  g.First().Clusternameid,
         Text = g.Key
     })
     .OrderBy(x => x.Text)
     .ToList();          

            return allVnfClusterName;
        }

        public List<KeyValuePairDto> GetVOMHardwareType()
        {
            var hardwareTypeResource = _repositoryWrapper.VnfHardwareRepository.FindAll()
   .GroupBy(x => x.Description)
   .Select(g => new KeyValuePairDto
   {
       Key =  g.First().Vnfhardwareid,
       Text = g.First().Description,
   })
   .OrderBy(x => x.Text)
   .ToList();


            return hardwareTypeResource;
        }
        public List<Vmtypename> GetVnfVmTypeDetails()
        {
            
       

            var allVnfVmType = _repositoryWrapper.VmTypeNameRepository.FindAll()
    .AsEnumerable()
   .GroupBy(x => new { x.Vmtypedescription, x.Vnfnameid })
    .Select(g => g.First())
    .OrderBy(x => x.Vmtypedescription).ThenBy(x => x.Vnfnameid)
    .ToList();


            return allVnfVmType;
        }

        public async Task<List<Locations>> GetAllLocations()
        {
            var allLocation = await _repositoryWrapper.Location.FindAll()?.Distinct().OrderBy(x => x.Locationid).ToListAsync();
            return allLocation;
        }
        public async Task<List<OpcoBasedLocation>> GetAllLocationsBasedOpcos(List<short> opcoList = null)
        {

            var allLocation = opcoList != null && opcoList.Count > 0 ?
                 await _repositoryWrapper.OpCo
                 .FindByCondition(x => opcoList.Contains(x.Opcoid) && x.Locations.Any(t => t.Shortdescription != null))
                 .Include(t => t.Locations)
                 .Select(x => new OpcoBasedLocation
                 {
                     OpcoId = x.Opcoid,
                     OpcoDescription = x.Opco,
                     LocationDetails = x.Locations.Any(t => t.Shortdescription != null) ? x.Locations.Where(t => t.Shortdescription != null)
                         .Select(y => new LocationValueDto
                         {
                             Text = y.Location,
                             Value = y.Locationid,
                             ShortDescription = y.Shortdescription
                         })
                         .ToList() : new List<LocationValueDto>()
                 })
                 .ToListAsync()
                 : await _repositoryWrapper.OpCo
                 .FindByCondition(x => x.Locations.Any(t => t.Shortdescription != null))
                 .Include(t => t.Locations)
                 .Select(x => new OpcoBasedLocation
                 {
                     OpcoId = x.Opcoid,
                     OpcoDescription = x.Opco,
                     LocationDetails = x.Locations.Any(t => t.Shortdescription != null) ? x.Locations.Where(t => t.Shortdescription != null)
                         .Select(y => new LocationValueDto
                         {
                             Text = y.Location,
                             Value = y.Locationid,
                             ShortDescription = y.Shortdescription
                         })
                         .ToList() : new List<LocationValueDto>()
                 })
                 .ToListAsync();


            return allLocation;
        }


        public List<FilterValueDto> GetAllIntraTypeVM()
        {

            var allIntraTypeVM = _repositoryWrapper.IntraVmTypeRepository.FindAll().Select
                (
                s => new FilterValueDto
                {
                    Text = s.Intradescription,
                    Value = s.Intravmtypeid.ToString()
                }
                ).OrderBy(x => x.Text).ToList();
            return allIntraTypeVM;
        }
        public List<FilterValueDto> GetAllInterTypeVM()
        {

            var allInterTypeVM = _repositoryWrapper.InterVmTypeRepository.FindAll().Select
                 (
                 s => new FilterValueDto
                 {
                     Text = s.Interdescription,
                     Value = s.Intervmtypeid.ToString()
                 }
                 ).OrderBy(x => x.Text).ToList();

            return allInterTypeVM;
        }
        public List<FilterValueDto> GetAllFinancialVersion()
        {
            var allFinancialVersion = new List<FilterValueDto>()
            {
                new FilterValueDto{
                     Text = "H1",
                     Value = "1"
                 },
                new FilterValueDto{
                     Text = "H2",
                     Value = "2"
                 },
          
            };



            return allFinancialVersion;
        }
        public List<FilterValueDto> GetAllVmWorkLoadType()
        {
            var allVmWorkLoad = _repositoryWrapper.VnfWorkLoadTypeRepository.FindAll()
    .AsEnumerable()  
    .GroupBy(x => x.Description)
    .Select(g => g.First())
    .Select(s => new FilterValueDto
    {
        Text = s.Description,
        Value = s.Vmworkloadtypeid.ToString()
    })
    .OrderBy(x => x.Text)
    .ToList();


            return allVmWorkLoad;
        }

        public async Task<Dictionary<decimal, string>> GetProductNameDropDown()
        {
            var resource = await _repositoryWrapper.ProductNameRepository.FindAll().ToListAsync();

            var result = resource.DistinctBy(x => x.Productnameid).ToDictionary(
                x => x.Productnameid, x => x.Description.ToString());
            return result;
        }


        #endregion


        #region  Cbom

        public async Task<List<KeyValuePairDto>> GetCbomPriority()
        {
            var cnfPriorityEntities = await _repositoryWrapper.CnfPriorityRepository.FindAll().Select(x => new KeyValuePairDto
            {
                Key =  x.Cnfpriorityid,
                Text = x.Description

            }).ToListAsync();

            return cnfPriorityEntities;
        }

        public async Task<List<KeyValuePairDto>> GetCbomHardware()
        {
            var cnfHardwareEntities = await _repositoryWrapper.CnfHardwareRepository.FindAll().Select(x => new KeyValuePairDto
            {
                Key = x.Cnfhardwareid,
                Text = x.Description

            }).ToListAsync();

            return cnfHardwareEntities;
        }
        public List<KeyValuePairDto> GetAllCnfNameResources()
        {
            var allCnfName = _repositoryWrapper.CnfNameRepository.FindAll().Select(x => new KeyValuePairDto
            {
                Key = x.Cnfnameid,
                Text = x.Cnfdescription
            })?.Distinct().OrderBy(x => x.Text).ToList();


            return allCnfName;
        }
        public List<Cnfcluster> GetAllCnfClusterResources()
        {
            var allCnfcluster = _repositoryWrapper.CnfClusterRepository.FindAll() .OrderBy(x => x.Cnfclustername).ToList();

            return allCnfcluster;
        }
        public List<Podtypeinfo> GetAllPodTypeInfoResource()
        {
            var allCnfName = _repositoryWrapper.PodTypeInfoRepository.FindAll().OrderBy(x => x.Podtypeinfoname).ToList();
            return allCnfName;
        }
        public List<Functionstandardname> GetAllFunctionStandardedNameResource()
        {
            var allCnfName = _repositoryWrapper.FunctionStandardNameRepository.FindAll().ToList();


            return allCnfName;
        }

        #endregion

        #region //OEM drop down
        public async Task<Dictionary<short, string>> GetOemFromMajorSwBuildDto()
        {

            var allOemFromMajorSwBuildDto = await _repositoryWrapper.MajorSoftwareBuild.FindAll().Include(x => x.Orgeqpmanufacturer).ToListAsync();
            
            var result = allOemFromMajorSwBuildDto.DistinctBy(x=>x.Orgeqpmanufacturerid).ToDictionary(
             x => x.Orgeqpmanufacturerid,
              y => y.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToString()
           );

            return result;
        }
        public async Task<Dictionary<int, string>> GetVFFromSystemTypeDto()
        {

            var allVFFromSystemTypeDto = await _repositoryWrapper.SystemType.FindAll().Include(x => x.VodafonenameNavigation).ToListAsync();
                
            var result = allVFFromSystemTypeDto.DistinctBy(x=>x.Vodafonename).ToDictionary(
             x => x.Vodafonename.Value,
              y => y.VodafonenameNavigation.Description
           );

            return result;
        }
        public async Task<Dictionary<long, string>> GetPlannedCompletionFromLcmDto()
        {
            var getLcmDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x =>
           ConstantValueFilter.LcmDeploymentStatus.Contains(x.Description.ToLower().Trim().Replace(" ", ""))).Select(r => r.Id).ToList();

            var allPlannedCompletionFromLcmDto = await _repositoryWrapper.Lcmengineering.FindByCondition(x => getLcmDeploymentStatusId.Contains(x.Lcmdeploymentstatusid.Value) &&
                 x.Onsoftware == ConstantValueFilter.isTrue)
                .Include(x => x.PlannedactivitiesLcmengineering).ToListAsync();
             var result = allPlannedCompletionFromLcmDto.DistinctBy(x=>x.PlannedactivitiesLcmengineering.Select(e=>e.Plannedactivityid).FirstOrDefault()).ToDictionary(
             x => x.PlannedactivitiesLcmengineering.Select(r=>r.Plannedactivityid).FirstOrDefault(),
              y => y.PlannedactivitiesLcmengineering.Select(r => r.Plannedcompletion.ToString()).FirstOrDefault()
           );

            return result;
        }
        public async Task<Dictionary<int, string>> GetVerticalFromLCMDto()
        {
            var verticalDropDown = await _repositoryWrapper.VerticalResponsible.FindAll().ToDictionaryAsync(x => x.Verticalresponsibleid, y => y.Verticalresponsible);

            return verticalDropDown;
        }
        #endregion

        #region // Team, CategoryPA, BudgetOwner Drop Down       
        public async Task<Dictionary<short, string>> GetPACategoryDropDown()
        {
            var resource = await _repositoryWrapper.PlannedActivityCategoryRepository.FindAll().ToListAsync();

            var result = resource.DistinctBy(x => x.Plannedactivitycategoryid).ToDictionary(
                x => x.Plannedactivitycategoryid, x => x.Categorydescription.ToString());
            return result;
        }
        #endregion

        #region  Asset Ancillary 
        public List<KeyValuePairDto> GetAssetCluster()
        {

            var allAssetCluster = _repositoryWrapper.AssetClusterRepository.FindAll()
                     .GroupBy(x => x.Description)
                     .Select(g => new KeyValuePairDto
                     {
                         Key =  g.First().Assetclusterid,
                         Text = g.Key
                     })
                     .OrderBy(x => x.Text)
                     .ToList();

            return allAssetCluster;
        }

        public List<KeyValuePairDto> GetAssetClusterType()
        {

            var allAssetCluster = _repositoryWrapper.AssetClusterTypeRepository.FindAll()
                     .GroupBy(x => x.Description)
                     .Select(g => new KeyValuePairDto
                     {
                         Key =  g.First().Assetclustertypeid,
                         Text = g.Key
                     })
                     .OrderBy(x => x.Text)
                     .ToList();

            return allAssetCluster;
        }
        public List<Datacenter> GetDataCenter()
        {

            var allAssetCluster = _repositoryWrapper.DataCenterRepository.FindAll()
                     .GroupBy(x => x.Description)
                     .Select(g => new Datacenter
                     {
                         Datacenterid = (short)g.First().Datacenterid,
                         Description = g.Key,
                         Opcoid = g.First().Opcoid,
                         Environmentzone = g.First().Environmentzone,
                     })
                     .OrderBy(x => x.Description)
                     .ToList();

            return allAssetCluster;
        }

        #endregion


        public async Task<string> GetVerticalResponseName(long nonTemsVerticalId)
        {
            return await _repositoryWrapper.VerticalResponsible.FindByCondition(x => x.Verticalresponsibleid== nonTemsVerticalId).Select(x => x.Verticalresponsible).FirstOrDefaultAsync();
        }

        public async Task<List<KeyValuePairDto>> GetAllVerticalResponsible(List<int> verticalList=null)
        {
            var allDomain = verticalList!=null && verticalList.Count>0 ?
                await _repositoryWrapper.VerticalResponsible.FindByCondition(x=>verticalList.Contains(x.Verticalresponsibleid)).Select(
                x => new KeyValuePairDto
                {
                    Key = x.Verticalresponsibleid,
                    Text = x.Verticalresponsible
                })?.Distinct().OrderBy(x => x.Text).ToListAsync()
                : await _repositoryWrapper.VerticalResponsible.FindAll().Select(
                x => new KeyValuePairDto
                {
                    Key = x.Verticalresponsibleid,
                    Text = x.Verticalresponsible
                })?.Distinct().OrderBy(x => x.Text).ToListAsync();

            return allDomain;

        }
        public async Task<List<DropdownAspnetModuleList>> GetAllAspnetModules()
        {
            var allProducts = await _repositoryWrapper.AspNetModulesRepository.FindAll().Select(
                x => new DropdownAspnetModuleList
                {
                    Key = x.Aspnetmoduleid,
                    Value = x.Module,
                    ModulePath = x.Modulepath,
                    Category = x.Category,
                    Menu = x.Menu,
                })?.Distinct().OrderBy(x => x.Value).ToListAsync();
            return allProducts;
        }
        public async Task<List<KeyValuePairDto>> GetAllAspnetRoles()
        {
            var allProducts = await _repositoryWrapper.RoleRepository.FindAll().Select(
                x => new KeyValuePairDto
                {
                    Key = x.Id,
                    Text = x.Name
                })?.Distinct().OrderBy(x => x.Text).ToListAsync();

            return allProducts;

        }

    }


    public class DropdownKeyValueList
    {

        public short Key { get; set; }
        public string Value { get; set; }

        public int Id { get; set; }
        public string Description { get; set; }

    }
    public class AssetOverviewbyMarketDto
    {
        public DropdownKeyValueList Opco { get; set; }
        public DropdownKeyValueList Vendor { get; set; }
        public DropdownKeyValueList Vertical { get; set; }
        public DropdownKeyValueList HWBuildCons { get; set; }
        public DropdownKeyValueList SupportService { get; set; }
        public DropdownKeyValueList DCF { get; set; }
        public DropdownKeyValueList Environment { get; set; }

        public DropdownKeyValueList SystemType { get; set; }
    }
}
