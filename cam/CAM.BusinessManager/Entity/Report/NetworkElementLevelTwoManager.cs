using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.ComponentBag;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.Rules;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Report;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using static CAM.BusinessManager.Rules.LCMEngineeringRulesExtension;

namespace CAM.BusinessManager.Entity.Report
{
    public class NetworkElementLevelTwoManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private CommonManager _commonManager;

        private readonly ICurrentUserService _currentUserService;
        private readonly string _engUpdateTracker = "To be started";
        private static int currenYear = DateTime.Now.Year;
        private static readonly DateTime fronzenDate = new DateTime(currenYear, 6, 1);
        private static readonly DateTime targetDate = new DateTime(currenYear + 1, 6, 1);
        public NetworkElementLevelTwoManager(IEnumerable<IRepositoryWrapper> wrappers,
        GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, CommonManager commonManager,
        IRepositoryWrapper repositoryWrapper
        ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _commonManager = commonManager;



        }

        public QueryResultDto<NetworkLevelTwoSWReportDtoGrid> FindDisaggregatedWithCondition(ReportSoftwareQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyDisaggregatedFilter(buildFilterDto);


            var result = GetDisaggregatedQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetDisaggregatedColumnsMapDB());

            IEnumerable<Networkelementsasplanned> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();

            #region // filters
            if (buildFilterDto?.OutputToLcmSoftware != null && buildFilterDto.OutputToLcmSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.OutputToLcmSoftware.Contains(x.Lcmengineering.Lcmstatussoftware));
            }
            if (buildFilterDto?.LcmStatusEngSoftware != null && buildFilterDto.LcmStatusEngSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusEngSoftware.Contains(x.Lcmengineering.Lcmstatusengsoftware));
            }
            if (buildFilterDto?.LcmStatusOpsSoftware != null && buildFilterDto.LcmStatusOpsSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusOpsSoftware.Contains(x.Lcmengineering.Lcmstatusopssoftware));
            }
            if (buildFilterDto?.OperationsMaintenanceContractLcm != null && buildFilterDto.OperationsMaintenanceContractLcm.Any())
            {
                query = query.Where(x => buildFilterDto.OperationsMaintenanceContractLcm.Contains(x.Lcmengineering.Outputtolcmsoftware));
            }

            if (buildFilterDto?.ENGKPI2 != null && buildFilterDto.ENGKPI2.Any())
            {
                query = query.Where(x => buildFilterDto.ENGKPI2.Contains(GetEngKpi2(x.Lcmengineering.Lcmstatusengsoftware, x.Lcmengineering.Outputtolcmsoftware)));
            }
            if (buildFilterDto?.ExpLCMstatusatendofFY24 != null && buildFilterDto.ExpLCMstatusatendofFY24.Any())
            {
                query = query.Where(x => buildFilterDto.ExpLCMstatusatendofFY24
                             .Contains(GetExpLCMstatusatendofFY24(x.Lcmengineering.Lcmstatussoftware,
                                                                    x.Lcmengineering.Outputtolcmsoftware,
                                                                    x.Lcmengineering.Softwareendofwarrantydate != null ? x.Lcmengineering.Softwareendofwarrantydate : x.Lcmengineering.Softwareendofsupportcontract,
                                                                    x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Plannedcompletion,
                                                                     x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Projectstatus
                                                                    )));
            }

            if (buildFilterDto?.HardwareModel != null && buildFilterDto.HardwareModel.Any())
            {
                query = query.Where(x =>
                x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                   .SingleOrDefault(m =>
                       m.Ismain &&
                       m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                       m.Deleted == false)?.Majorhardware?.Buildconstruction?.Rule == (int)BuildconstructionRuleEnum.VirtualHW ?
                        buildFilterDto.HardwareModel.Contains(ConstantValueFilter.Virtualized) : buildFilterDto.HardwareModel.Contains(x.Designcomponent.Systemtype.toLcmDbExportHardwareName()));
            }
            if (buildFilterDto?.IdentifiedAction != null && buildFilterDto.IdentifiedAction.Any())
            {
                query = query.Where(x => buildFilterDto.IdentifiedAction.Contains(GetIdentificationActionForLcmExport(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software)).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Lcmengineering?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofmaintenance, _repositoryWrapper)));
            }
            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Budgettrackingid)));
            }

            if (buildFilterDto?.AssetServiceFunctionality != null && buildFilterDto.AssetServiceFunctionality.Any())
            {

                query = query.Where(x => x?.Designcomponent?.Designcomponentfamily?.Designaspects?.
                                      FirstOrDefault() != null).Where(x => buildFilterDto.AssetServiceFunctionality
                                 .Contains(string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Designaspects?.
                                      FirstOrDefault()?.Designaspectssupportedsvr?.Select(x => x?.Service?.Description)?.Distinct())));
            }
            if (buildFilterDto?.ExposedEdgeFlag != null && buildFilterDto.ExposedEdgeFlag.Any())
            {
                query = query.ToList().Where(x => x.Lcmengineering.Lcmancillarydata.Any())
                    .Where(x => buildFilterDto.ExposedEdgeFlag.Contains(GetExposedEdgeValue(x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Isexposededge)));
            }

            #region Ticket 465 - LCM export: RAG status field - filter is not working fine
            if (buildFilterDto?.RagStatus != null && buildFilterDto.RagStatus.Any())
            {
                List<List<string>> lcmIdList = new List<List<string>>();
                foreach (var item in buildFilterDto.RagStatus)
                    lcmIdList.Add(LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(query, null, 2, item).Where(x => x.Value != "0").Select(x => x.Text).ToList());

                if (lcmIdList != null && lcmIdList.Count() > 0)
                    query = query.Where(x => lcmIdList.SelectMany(y => y).Contains(x.Lcmengineeringid.ToString()));

            }
            #endregion
            #endregion

            //var totalCount = query.SelectMany(x => x.Identitiesasis.DefaultIfEmpty()).Count();
            var totalCount = query.SelectMany(x => x.Identitiesasis.Where(d => buildFilterDto.InterfaceType == null || !buildFilterDto.InterfaceType.Any() ||
           (x.Identitiesasis != null && d.Interfacetype != null && buildFilterDto.InterfaceType.Contains(d.Interfacetype))).DefaultIfEmpty(), (x, identity) => new { x, identity })
                .SelectMany(y => y.x.Buildbag.Componentsoftwarebuildbags.DefaultIfEmpty()).Count();
            #region // Mapping Caluculated Value
            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != null).Select(x => (long)x.Lcmengineeringid)?.Distinct()?.ToList();

            var AssetIdAndOpcoId = query?.ToList()?.Where(x => x.Networkelementasplannedid != 0)?.DistinctBy(x => x?.Networkelementasplannedid)
                .ToDictionary(x => x.Networkelementasplannedid, x => (long)x.Opcoid);

            var allSubDomain = _commonManager.GetCalculatedAssetSubDomainSpocEntityforReport(AssetIdAndOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedAssetEduSpocEntityforReport(AssetIdAndOpcoId).ToList();

            var allOperationalContract = _commonManager.GetCalculateLCMOperationalContractsForReport(lcmengineeringsEntityId);


            var reportLastUpdateDate = _repositoryWrapper.LcmDBExportUpdateHistory.FindAll().FirstOrDefault();
            #endregion

            bool isLcmDBExportUpdated = false;

            if (reportLastUpdateDate != null && reportLastUpdateDate.Lastswupdatedate.Date == DateTime.Now.Date)
            {
                isLcmDBExportUpdated = true;
            }



            #region // Grid mapping

            

            var reports = query.SelectMany(x => x.Identitiesasis.Where(d => buildFilterDto.InterfaceType == null || !buildFilterDto.InterfaceType.Any() ||
            (x.Identitiesasis != null && d.Interfacetype != null && buildFilterDto.InterfaceType.Contains(d.Interfacetype))).DefaultIfEmpty(), (x, identity) => new { x, identity })
            .SelectMany(y => y.x.Buildbag.Componentsoftwarebuildbags.DefaultIfEmpty(), (y, bag) =>
            {
                var grid = new NetworkLevelTwoSWReportDtoGrid();
                var systemType = y.x?.Designcomponent?.Systemtype;
                var majorSW = systemType?.Majorsoftwarebuilds;


                var hw = systemType?.Systemtypesmajorhardwarebuilds
                      ?.SingleOrDefault(m =>
                          m.Ismain &&
                          m.Systemtypeid == systemType?.Systemtypeid &&
                          m.Deleted == false)?.Majorhardware;

                var plannedActivitySWFilterCheck = y.x?.Lcmengineering.PlannedactivitiesLcmengineering?.AsQueryable().GetPlannedActivityFilteredDB(ConstantValueFilter.Software);

                var FilterPlanneSW = y.x.Lcmengineering?.PlannedactivitiesLcmengineering?.AsQueryable()
                     .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software)).OrderBy(x => x.Plannedcompletion);

                var plannedActivitySW = plannedActivitySWFilterCheck.Plannedactivityresourceid == null ? null : plannedActivitySWFilterCheck;
                #region
                var dcSubnetwork = y.x?.Designcomponent?.Subnetworkboundary;
                var Dcf = y.x?.Designcomponent?.Designcomponentfamily;

                if (!isLcmDBExportUpdated)
                {
                    y.x.Lcmengineering.Outputtolcmsoftware = y.x.Lcmengineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareOutput).Result;
                    y.x.Lcmengineering.Lcmstatussoftware = y.x.Lcmengineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatus).Result;
                    y.x.Lcmengineering.Lcmstatusopssoftware = y.x.Lcmengineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusOps).Result;
                    y.x.Lcmengineering.Lcmstatusengsoftware = y.x.Lcmengineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusEng).Result;
                    _repositoryWrapper.Lcmengineering.Update(y.x.Lcmengineering);
                    _repositoryWrapper.Save();
                    _repositoryWrapper.ClearTracker();
                }

                grid.DesignComponentFamily = y.x.Designcomponent.toDesignComponentFamily();
                grid.SupportedService = string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct() ?? new List<string>());

                grid.LcmStatus = y.x.Lcmengineering.Lcmstatussoftware;
                grid.OperationsMaintenanceContract = y.x.Lcmengineering.Outputtolcmsoftware;
                grid.LcmStatusOpsSoftware = y.x.Lcmengineering.Lcmstatusopssoftware;
                grid.LcmStatusEngSoftware = y.x.Lcmengineering.Lcmstatusengsoftware;

                grid.ReportId = $"{y.x.Lcmengineering.Resourcekey}-{y.x.Swresourcekey}-0";
                grid.LcmEngineeringId = y.x.Lcmengineering.Lcmengineeringid;
                grid.LocalMarket = y.x.Opco?.Opco;
                grid.DesignComponentIndex = y.x.Designcomponentid;

                #region // Bag details
                grid.BagName = _commonManager.GetBuildBagDescription(y.x.Buildbag);
                grid.ComponentName = ComponentBagExtensionMethod.GetComponentBagDescription(_repositoryWrapper, bag?.Componentsoftwarebuildid);
                grid.ComponentResourceKey = ComponentBagExtensionMethod.GetComponentResourcekey(_repositoryWrapper, y.x.Opcoid, grid.ComponentName, y.x.Designcomponent.Designcomponentfamilyid);
                #endregion

                #region code optimize org Table

                grid.VerticalEngineeringTeam =
                    string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == y.x.Networkelementasplannedid && m.VerticalDic != null && y.x.Deleted == false)
                    .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList())
                    ;

                grid.VerticalSubDomain = string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == y.x.Networkelementasplannedid && m.SubdomainresponsiblesDic != null && y.x.Deleted == false)
                    .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList());

                grid.EngineeringContactPoint = _commonManager.GetEngContactPointFromEduAndSubDomainSpoc(allEdu?.Where(m => m.NetWorkElementAsPlannedId == y.x.Networkelementasplannedid && y.x.Deleted == false).Select(t => t?.ContactEmail).ToList(),
                                   allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == y.x.Networkelementasplannedid && y.x.Deleted == false).Select(t => t?.ContactEmail).ToList());

                grid.OperationsContactPoint = string.Join(" | ",
                       allOperationalContract?.Where(m => m.LcmengineeringId == y.x.Lcmengineeringid).Select(x => x?.OperationDescription).Distinct());

                #endregion

                grid.AssetCategory = systemType?.Assetcategory?.Assetcategory;

                grid.AssetClass = majorSW.Productname != null ? majorSW.Productname?.Description : "";

                grid.AssetType = majorSW.Criticalassettype?.Description;

                grid.AssetDescription = dcSubnetwork.Description;

                grid.ProductImportance = y.x.Lcmengineering.Productimportance?.Productimportance;

                grid.Vendor = majorSW.Orgeqpmanufacturer
                       ?.Originalequipmentmanufacturer;


                grid.HardwareModel = hw.Buildconstruction?.Rule == (int)BuildconstructionRuleEnum.VirtualHW ? ConstantValueFilter.Virtualized : systemType?.toLcmDbExportHardwareName();


                grid.NumberOfNodes = 1;



                grid.EOMStatus = majorSW != null ? (EOMEnum)majorSW.Eomstatus
                      : EOMEnum.NotSpecified;

                grid.VendorEndOfMaintenanceDate = majorSW.Endofmaintenance != null
                       ? majorSW.Endofmaintenance?.Date : null;

                grid.VendorEndOfMaintenanceDateValue = majorSW.Endofmaintenance != null
                      ? majorSW.Endofmaintenance?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : grid.EOMStatus == EOMEnum.NotAnnounced ? "Not Announced" : null;



                grid.DescriptionOfPlannedAction = plannedActivitySW
                       ?.Plannedactivityresource?.Plannedactivityresource;
                grid.PlannedSoftwareVersion = plannedActivitySW?.Activitydetails ?? ConstantValueFilter.Na;


                grid.ProjectStatus = plannedActivitySW?.Projectstatus;

                grid.ProjectEndDate = plannedActivitySW?.Plannedcompletion?.Date;

                grid.ProjectEndDateValue = plannedActivitySW?.Plannedcompletion?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                grid.TrackingNumberProjectName = y.x.Lcmengineering.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Software);
                grid.Notes = plannedActivitySW?.Notes;

                grid.SoftwareVersion = majorSW?.Softwareversion;

                #region

                grid.SystemTypeId = y.x.Designcomponent?.Systemtypeid ?? 0;

                grid.DesignComponentId = y.x.Designcomponentid;
                grid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
                grid.MajorHardwareBuildId = y.x.Designcomponent.Systemtype?.Systemtypesmajorhardwarebuilds
                       ?.SingleOrDefault(m =>
                           m.Ismain && m.Systemtypeid == y.x.Designcomponent.Systemtype.Systemtypeid &&
                           m.Deleted == false)?.Majorhardwareid;

                grid.AssetVirtualized =
                   hw?.Buildconstruction?.Iscloudasset == true
                   ? ConstantValueFilter.YES
                   : ConstantValueFilter.NO;

                grid.BundleBudget = GetBundleBudget(plannedActivitySW?.Budgettrackingid);

                grid.BundleId = GetBundleBudget(plannedActivitySW?.Budgettrackingid) == ConstantValueFilter.yes ? plannedActivitySW.Budgettrackingid.Substring(2) : null;

                grid.AssetServiceFunctionality = y.x?.Designcomponent?.Designcomponentfamily?.Designaspects?.FirstOrDefault()?.Designaspectssupportedsvr?.Count() > 0 ?
                    string.Join(" | ", y.x?.Designcomponent?.Designcomponentfamily?.Designaspects?.FirstOrDefault()?.Designaspectssupportedsvr.Select(x => x?.Service?.Description).Distinct()) : string.Empty;

                grid.Platform = y.x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? y.x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description : "";

                grid.OpsMaintenanceConractEnd = y.x.Lcmengineering.Softwareendofwarrantydate != null ? y.x.Lcmengineering.Softwareendofwarrantydate : y.x.Lcmengineering.Softwareendofsupportcontract;
                grid.OpsMaintenanceConractEndValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

                grid.VendorEndOfVulnerabilitySecuritySupportDateValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;



                grid.EngRiskEvaluationNotes = plannedActivitySW
                     ?.Riskengineeringnotes;

                grid.EngRiskEvaluation = _commonManager.GetRiskValue(plannedActivitySW
                      ?.Engineeringrisk?.Description);

                #endregion
                #endregion
                grid.OpsRiskEvaluation = _commonManager.GetRiskValue(plannedActivitySWFilterCheck.Plannedactivityresourceid == null ? null :
                                    y.x.Lcmengineering.PlannedactivitiesLcmengineering?.Where(p => p.Operationalriskid.HasValue && buildFilterDto.OpsRiskEvaluation.Contains(p.Operationalriskid.Value)).GetPlannedActivityFilteredDB(ConstantValueFilter.Software)?.Operationalrisk?.Description); ;

                #region

                grid.OpsRiskEvaluationNotes = plannedActivitySW?.Riskoperationalnotes;


                grid.OverallRiskEvaluation = plannedActivitySW?.Overallriskevaluation;
                grid.PlannedActivityId = plannedActivitySW?.Plannedactivityid;

                grid.BudgetEstimated = y.x.Lcmengineering.PlannedactivitiesLcmengineering.GetPlannedActivityBudgetEstimated(ConstantValueFilter.Software);
                grid.ManagedByGdc = ConstantValueFilter.no;
                grid.DesignComponentFamilyId = y.x.Designcomponent?.Designcomponentfamilyid;
                grid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";

                grid.Archived = y.x.Lcmengineering.Archived;

                grid.MainOrganization = string.Empty;
                grid.DeliveryPlanAvailable = plannedActivitySW?.Deliveryplanavailable != null && plannedActivitySW.Deliveryplanavailable ? ConstantValueFilter.YES : ConstantValueFilter.NO;


                grid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper);
                grid.Criticality = dcSubnetwork?.Criticality;
                grid.GdprRelevant = dcSubnetwork.Gdprrelevant != null ? dcSubnetwork.Gdprrelevant == true ? ConstantValueFilter.YES : ConstantValueFilter.NO : null;

                grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngSoftware, grid.OperationsMaintenanceContract);
                grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus, grid.OperationsMaintenanceContract, grid.OpsMaintenanceConractEnd, grid.ProjectEndDate, grid.ProjectStatus);
                grid.IsExtendedSupportOfferedByVendor = y.x.Lcmengineering.Isextendedsupportofferedbyvendor.HasValue && y.x.Lcmengineering.Isextendedsupportofferedbyvendor.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO;


                #region // Identites details
                grid.IpAddress = y?.identity?.Value;
                grid.Category = y?.identity?.Category?.Description;
                grid.InterfaceType = y?.identity?.Interfacetype;
                #endregion

                grid.Hostname = Convert.ToString(y.x.Elementname).Trim();

                #region Ticket 465 - LCM export: RAG status field - filter is not working fine
                grid.RagStatus = LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(null,
                     query.Where(r => r.Lcmengineering.Lcmengineeringid == y.x.Lcmengineeringid).ToList(), 2
                     ).FirstOrDefault().Text.ToString();
                #endregion

                grid.WbsCode = GetWbsCode(plannedActivitySW?.Deliveryprojectname);
                grid.BptID = plannedActivitySW?.Budgettrackingid;
                grid.PpmID = plannedActivitySW?.Deliveryprojectid;
                grid.MainOrganization = ConstantValueFilter.Nse;
                grid.SerialNumber = y.x.Swresourcekey;


                #region LCM R9 Part - 1
                var lcmAuditAttributes = y.x.Lcmengineering.Lcmancillarydata.FirstOrDefault();
                grid.SecurityRiskPotential = y.x.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames.Where(x => x.Riskcluster != null).Select(x => x.Riskcluster?.Risklevel).FirstOrDefault();
                grid.SecurityRiskOverall = GetSecurityRiskOverAllValue(lcmAuditAttributes != null ? lcmAuditAttributes.Securityriskeffective : string.Empty, grid.SecurityRiskPotential);

                if (lcmAuditAttributes != null)
                {

                    grid.ReasonfornoPlan = lcmAuditAttributes.Reasonfornoplan;
                    grid.CommentonProjectStatus = lcmAuditAttributes.Commentonprojectstatus;
                    grid.SecurityRiskEffective = lcmAuditAttributes.Securityriskeffective;
                    grid.SecurityMitigation = lcmAuditAttributes.Securitymitigation;

                    grid.IncludedinSecurityScanning = lcmAuditAttributes.Includedinsecurityscanning;
                    grid.RaId = lcmAuditAttributes.Raid;
                    grid.RequestID = lcmAuditAttributes.Requestid;
                    grid.LastScanDate = lcmAuditAttributes.Lastscandate?.Date;
                    grid.LastScanDateValue = lcmAuditAttributes.Lastscandate;
                    grid.AssetOutofScopeForReportingPurposes = lcmAuditAttributes.Assetoutofscope;
                    grid.LastUpgradeDate = lcmAuditAttributes.Lastupgradedate?.Date;
                    grid.LastUpgradeDateValue = lcmAuditAttributes.Lastupgradedate;
                    grid.EomControl = lcmAuditAttributes.Eomcontrol;
                    grid.EngUpdateTracker = lcmAuditAttributes.Engupdatetracker;
                    grid.OpsUpdateTracker = lcmAuditAttributes.Opsupdatetracker;
                    grid.ExNetworks = lcmAuditAttributes.Exnetworks;
                    grid.NewopsRiskEvaluation = GetNewOpsRiskEvaluationValue(lcmAuditAttributes.Incidentclass, lcmAuditAttributes.Occurenceprobability);
                    grid.OccurrenceProbability = lcmAuditAttributes.Occurenceprobability;
                    grid.IncidentClass = lcmAuditAttributes.Incidentclass;
                    grid.ProductCode = lcmAuditAttributes.Productcode;
                    grid.HandedOverToOperation = lcmAuditAttributes.Handedovertooperation;

                    grid.ContractRenewalPlan = lcmAuditAttributes.Contractrenewalplan;
                    grid.DataSource = lcmAuditAttributes.Datasource;
                    grid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;


                    grid.OriginalSwLcmId = lcmAuditAttributes.Originalswlcmid;
                    grid.LabSWRelease = "";
                    #region Ticket 551
                    #region //719 Regulatory Fields Changes
                    grid.IsPecn = lcmAuditAttributes.Ispecn;
                    grid.IsPecs = lcmAuditAttributes.Ispecs;
                    grid.IsScf = lcmAuditAttributes.Isscf;
                    grid.IsNof = lcmAuditAttributes.Isnof;
                    #endregion

                    grid.ExternalFacingFlag = lcmAuditAttributes.Externalfacingflag != null ?
                       lcmAuditAttributes.Externalfacingflag.Value.ToString() : "";
                    #endregion
                }
                grid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
      ? lcmAuditAttributes.Handedovertooperation : true;
                grid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
                  ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;

                #endregion
                #endregion
                var Endofmaintenance = majorSW?.Endofmaintenance;

                grid.IdentifiedAction = y.x.Lcmengineering.Archived != true ? GetIdentificationActionForLcmExport(FilterPlanneSW.FirstOrDefault(), Endofmaintenance, _repositoryWrapper) : "";


                grid.Program = FilterPlanneSW.FirstOrDefault()
                     ?.ProgramNavigation?.Programdescription;

                grid.ProjectOwner = FilterPlanneSW.FirstOrDefault()
                      ?.Projectowner;


                return grid;
            })
            .ToList();
            #endregion



            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lastswupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
                _repositoryWrapper.ClearTracker();
            }

            if (!isExport)
            {
                reports = reports.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            else
            {
                reports = reports.ToList();
            }

            var rtn = new QueryResultDto<NetworkLevelTwoSWReportDtoGrid>(new GenerateRenderForGrid<NetworkLevelTwoSWReportDtoGrid>(_columnManager))
            {
                TotalItems = totalCount
            };

            rtn.Items = reports.ToArray();

            return rtn;
        }


        #region Disaggregated

        public List<FilterValueDto> GetDisaggregatedFilter(string propertyName, string propertyFilter,
           ReportSoftwareQueryDto buildFilterDto,bool isAdmin=false)
        {
            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyDisaggregatedFilter(buildFilterDto);
            var query = GetDisaggregatedQuery(predicateResult);

            var lcmAncillaryQuery = query.Where(x => x.Lcmengineering.Lcmancillarydata.Count > 0).
                Select(x => x.Lcmengineering.Lcmancillarydata);

            #region //SPOC and EPO Functions

            var eduSpoc = query.SelectMany(x => x.Lcmengineering.Lcmengineeringeduspoc.Select(y => new FilterValueDto { Text = y.Eduspoc.Email, Value = y.Eduspoc.ToString() })).ToList()
                .Concat(query.Where(x => x.Lcmengineering != null && x.Lcmengineering.Lcmengineeringeduspoc.Count() <= 0)
                       .Select(x =>

                          _commonManager.AddBlankFilterValue()
                       )).ToList();

            var subDomSpoc = query.SelectMany(x => x.Lcmengineering.Lcmengineeringsubdomainspoc.Select(y => new FilterValueDto { Text = y.Subdomainspoc.Email, Value = y.Subdomainspocid.ToString() })).ToList()
                .Concat(query.Where(x => x.Lcmengineering != null && x.Lcmengineering.Lcmengineeringsubdomainspoc.Count() <= 0)
                       .Select(x =>

                           _commonManager.AddBlankFilterValue()
                       )).ToList();

            var comSpocs = eduSpoc.Concat(subDomSpoc).DistinctBy(x => x.Value).ToList();

            #endregion

            var rtn = propertyName switch
            {
                "wbsCode" => query.Select(p => new FilterValueDto
                     (GetWbsCode(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectname))).Distinct().ToList(),
                "bptID" => query
                 .Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                 .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                 .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                 .Projectstatus != null ?
                  p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                 .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                 .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                 .Budgettrackingid : string.Empty)).Distinct().ToList(),
                "ppmID" => query
                    .Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Projectstatus != null ?
                     p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectid : string.Empty)).Distinct().ToList(),
                "mainOrganization" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Nse) },
                "serialNumber" => query.Select(p => new FilterValueDto(p.Swresourcekey)).Distinct().ToList(),
                "reportId" => query.Select(p => new FilterValueDto(p.Lcmengineering.Resourcekey + "-" + p.Swresourcekey + "0")).Distinct().ToList(),

                "ragStatus" => LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(query, null, 2).Distinct().ToList(),


                "designComponentFamily" => query.ToList().Select(y =>
                new FilterValueDto
                {
                    Text = y.Designcomponent.toDesignComponentFamily(),
                    Value = y.Designcomponent.Designcomponentfamilyid.ToString()
                }).Distinct().ToList(),

                "supportedService" => query.ToList().Where(x => x.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr != null).Select(y =>
                new FilterValueDto
                {
                    Text = y.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Select(x => x.Service.Description).FirstOrDefault(),
                    Value = y.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Select(x => x.Service.Id.ToString()).FirstOrDefault()
                }).Distinct().ToList(),

                "ipAddress" => query.Where(x => x.Identitiesasis != null && x.Identitiesasis.Count() > 0).SelectMany(x => x.Identitiesasis.Select(y =>
                new FilterValueDto
                {
                    Text = y.Value.ToString(),
                    Value = y.Value.ToString(),

                })).Distinct().ToList(),

                "category" => query.Where(x => x.Identitiesasis != null && x.Identitiesasis.Count() > 0).SelectMany(x => x.Identitiesasis.Select(y =>
                new FilterValueDto
                {
                    Text = y.Category.Description.ToString(),
                    Value = y.Categoryid.ToString()
                })).Distinct().ToList(),
                "interfaceType" => query.Where(x => x.Identitiesasis != null && x.Identitiesasis.Count() > 0).SelectMany(x => x.Identitiesasis.Select(y =>
                new FilterValueDto
                {
                    Text = y.Interfacetype.ToString(),
                    Value = y.Interfacetype.ToString(),
                })).Distinct().ToList(),

                "hostname" => query.Select(p => new FilterValueDto(p.Elementname)).Distinct().ToList(),
                "engKpi2" => query.Select(p => new FilterValueDto
                    (GetEngKpi2(p.Lcmengineering.Lcmstatusengsoftware, p.Lcmengineering.Outputtolcmsoftware))).ToList().Distinct().ToList(),
                "expLCMstatusatendofFY24" => query.Select(p => new FilterValueDto
                         (GetExpLCMstatusatendofFY24(p.Lcmengineering.Lcmstatussoftware,
                                                    p.Lcmengineering.Outputtolcmsoftware,
                                                    p.Lcmengineering.Softwareendofwarrantydate != null ? p.Lcmengineering.Softwareendofwarrantydate : p.Lcmengineering.Softwareendofsupportcontract,
                                                    p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion,
                                                    p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus
                                                    ))).AsEnumerable().Distinct().ToList(),
                "identifiedAction" => query.Select(x => new FilterValueDto(GetIdentificationActionForLcmExport(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                               .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Lcmengineering.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance, _repositoryWrapper))).ToList().Distinct().ToList(),
                "originalLCMSpreadsheetID" => query.Select(p => new FilterValueDto(p.Previousswresourcekey)).Distinct().ToList(),
                "localMarket" => query.Select(p => new FilterValueDto(p.Opco.Opco)).Distinct().ToList(),
                "designComponentIndex" => query.Select(p => new FilterValueDto(p.Designcomponentid.ToString())).Distinct().ToList(),

                "operationsContactPoint" => query.SelectMany(x => x.Lcmengineering.Lcmoperationalcontracts)
                             .Select(p => new FilterValueDto(p.Operationalcontract.Description)).Distinct().ToList(),
                "assetCategory" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Assetcategory.Assetcategory)).Distinct().ToList(),
                "operationsMaintenanceContract" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Outputtolcmsoftware)).Distinct().ToList(),
                "assetClass" => query.Where(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null).ToList().Select(p => new FilterValueDto
                         (p.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? p.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description : "")).Distinct().ToList(),
                "assetType" => query.Where(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Criticalassettype != null).AsEnumerable()
                   .Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Criticalassettype.Description)).Distinct().ToList(),
                "assetDescription" => query.AsEnumerable()
                    .Select(x => new FilterValueDto(x.Designcomponent?.Designcomponentfamily?.Description)).Distinct().ToList(),
                "productImportance" => _repositoryWrapper.ProductImportance.FindAll().Select(x => new FilterValueDto(x.Productimportance)).ToList(),
                "vendor" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer)).Distinct().ToList(),
                "hardwareModel" => query.ToList().Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                   .SingleOrDefault(m =>
                       m.Ismain &&
                       m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                       m.Deleted == false)?.Majorhardware?.Buildconstruction?.Rule == (int)BuildconstructionRuleEnum.VirtualHW ? ConstantValueFilter.Virtualized :
                                    x.Designcomponent.Systemtype.toLcmDbExportHardwareName())).Distinct().ToList(),
                "plannedAction" => new List<FilterValueDto>() { new FilterValueDto() { Text = ConstantValueFilter.OnHardware, Value = "1" }, new FilterValueDto() { Text = ConstantValueFilter.OnSoftware, Value = "2" } },
                "descriptionOfPlannedAction" => query.Select(p => new FilterValueDto
                (p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource)).Distinct().ToList(),
                "plannedSoftwareVersion" => query.Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Activitydetails)).ToList().Append(new FilterValueDto(ConstantValueFilter.Na)).Distinct().ToList(),
                "typeOfNetworkElement" => query.Where(x => x.Designcomponent.Subnetworkboundary.Lcmpolicy != null)
                                .Select(x => new FilterValueDto
                                {
                                    Text = ((LCMPolicy)x.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString(),
                                    Value = x.Designcomponent.Subnetworkboundary.Lcmpolicy.ToString()
                                }).Distinct().ToList(),
                "trackingNumberProjectName" => query.Select(x => new FilterValueDto
                        (x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname)).Distinct().ToList(),
                "notes" => query.Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes)).Distinct().ToList(),
                "assetVirtualized" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.yes), new FilterValueDto(ConstantValueFilter.no) },
                "softwareVersion" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion)).Distinct().ToList(),
                "budgetEstimated" => query.Select(s => s.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                                        .FirstOrDefault())?.AsEnumerable()
                                                        ?.Select(p => new FilterValueDto(p?.Currency == null ? p?.Budgetvalue?.ToString() : p?.Budgetvalue?.ToString() + p?.Currency))?.Distinct()?.ToList(),
                "bundleBudget" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.yes), new FilterValueDto(ConstantValueFilter.no) },
                "bundleId" => query.AsEnumerable().Where(x => GetBundleBudget(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware))
                                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Budgettrackingid) == ConstantValueFilter.yes)
                                   .Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware))
                                .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid.Substring(2))).Distinct().ToList(),
                "assetServiceFunctionality" => query.Select(p => new FilterValueDto(string.Join(" | ", p.Designcomponent.Designcomponentfamily.Designaspects.
                                                                     FirstOrDefault().Designaspectssupportedsvr.Select(x => x.Service.Description)))).ToList().Distinct().ToList(),
                "platform" => query.Where(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null)
                         .Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description,
                             x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description)).Distinct().ToList(),
                "riskCluster" => query.Where(x => x.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames != null).Select(p => new FilterValueDto(p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                .Select(x => x.Riskcluster.Riskclusterid).FirstOrDefault(),
                 p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                .Select(x => x.Riskcluster.Description).FirstOrDefault())).Distinct().ToList(),
                "criticality" => query.Select(p => new FilterValueDto(p.Designcomponent.Subnetworkboundary.Criticality)).Distinct().ToList(),
                "gdprRelevant" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.yes), new FilterValueDto(ConstantValueFilter.no) },
                "engRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = _commonManager.GetRiskValue(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description),
                    Value = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid.ToString()
                }).Distinct().ToList(),
                "overallRiskEvaluation" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                            .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                           .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation)).Distinct().ToList(),
                "engRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                 .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes)).Distinct().ToList(),
                "opsRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = _commonManager.GetRiskValue(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description),
                    Value = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid.ToString()
                }
                ).Distinct().ToList(),
                "opsRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "outputToLcmSoftware" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatussoftware)).Distinct().ToList(),
                "lcmStatusOpsSoftware" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatusopssoftware)).Distinct().ToList(),
                "lcmStatusEngSoftware" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatusengsoftware)).Distinct().ToList(),
                "isExtendedSupportOfferedByVendor" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.yes), new FilterValueDto(ConstantValueFilter.no) },
                "deliveryPlanAvailable" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.yes), new FilterValueDto(ConstantValueFilter.no) },
                "lcmStatus" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatussoftware)).Distinct().ToList(),
                "program" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                           .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                   .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription)).ToList().Distinct().ToList(),
                "projectOwner" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                       .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectowner)).ToList().Distinct().ToList(),
                "projectStatus" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
              .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
              .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus)).ToList().Distinct().ToList(),

                #region LCM R9 Part - 1
                "reasonfornoPlan" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Reasonfornoplan)).Distinct().ToList(),
                "commentonProjectStatus" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Commentonprojectstatus)).Distinct().ToList(),
                "securityRiskPotential" => query.Where(x => x.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames != null)
                 .Select(p => new FilterValueDto(p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                 .Select(y => y.Riskcluster.Risklevel).FirstOrDefault())).Distinct().ToList(),
                "securityRiskEffective" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Securityriskeffective)).Distinct().ToList(),
                "securityMitigation" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Securitymitigation)).Distinct().ToList(),
                "securityRiskOverall" => query.ToList().Select(p => new FilterValueDto(GetSecurityRiskOverAllValue(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Securityriskeffective,
                p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                .Select(y => y.Riskcluster.Risklevel).FirstOrDefault()))).Distinct().ToList(),
                "includedinSecurityScanning" => query.ToList().Select(p => new FilterValueDto { Text = p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Includedinsecurityscanning == true ? ConstantValueFilter.yes : ConstantValueFilter.no, Value = p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Includedinsecurityscanning.ToString() }).Distinct().ToList(),
                "raId" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Raid)).Distinct().ToList(),
                "requestID" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Requestid)).Distinct().ToList(),
                "lastScanDate" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Lastscandate)).Distinct().ToList(),
                "assetOutofScopeForReportingPurposes" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Assetoutofscope)).Distinct().ToList(),
                "lastUpgradeDate" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Lastupgradedate)).Distinct().ToList(),
                "eomControl" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.
                FirstOrDefault()?.Eomcontrol)).Distinct().ToList(),

                "engUpdateTracker" =>
               query.ToList()
                .Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?
                .Engupdatetracker)).Distinct()
                .ToList(),




                "opsUpdateTracker" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Opsupdatetracker)).Distinct().ToList(),
                "exNetworks" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Exnetworks)).Distinct().ToList(),
                "newopsRiskEvaluation" => query.ToList().Select(p => new FilterValueDto(GetNewOpsRiskEvaluationValue(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Incidentclass, p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Occurenceprobability))).Distinct().ToList(),
                "occurrenceProbability" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Occurenceprobability)).Distinct().ToList(),
                "incidentClass" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Incidentclass)).Distinct().ToList(),
                "productCode" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Productcode)).Distinct().ToList(),
                "handedOverToOperation" => query.ToList().Select(p => new FilterValueDto
                {
                    Text = p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Handedovertooperation == true ? ConstantValueFilter.yes : ConstantValueFilter.no,
                    Value = p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Handedovertooperation.ToString()
                }).Distinct().ToList(),

                "contractRenewalPlan" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Contractrenewalplan)).Distinct().ToList(),
                "dataSource" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Datasource)).Distinct().ToList(),
                "scopeOfSimplification" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Scopeofsimplification)).Distinct().ToList(),
                "originalSwLcmId" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Originalswlcmid)).Distinct().ToList(),
                "labSWRelease" => query.ToList().Select(p => new FilterValueDto("")).Distinct().ToList(),
                #endregion
                #region  Ticket 551
                #region 719 Regulatory fields changes
                "isPecn" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Ispecn != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Ispecn == true ? ConstantValueFilter.yes : ConstantValueFilter.no, Value = p.FirstOrDefault()?.Ispecn.ToString() }).Distinct().ToList(),
                "isPecs" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Ispecs != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Ispecs == true ? ConstantValueFilter.yes : ConstantValueFilter.no, Value = p.FirstOrDefault()?.Ispecs.ToString() }).Distinct().ToList(),
                "isScf" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Isscf != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Isscf == true ? ConstantValueFilter.yes : ConstantValueFilter.no, Value = p.FirstOrDefault()?.Isscf.ToString() }).Distinct().ToList(),
                "isNof" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Isnof != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Isnof == true ? ConstantValueFilter.yes : ConstantValueFilter.no, Value = p.FirstOrDefault()?.Isnof.ToString() }).Distinct().ToList(),
                #endregion

                "infrastructureLocation" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Locationinfrastructure != null))
                    .Select(p => new FilterValueDto(p.FirstOrDefault()?.Locationinfrastructure)).Distinct().ToList(),

                "exposedEdgeFlag" => query.ToList().Select(p => new FilterValueDto(GetExposedEdgeValue(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Isexposededge))).Distinct().ToList(),

                "externalFacingFlag" => lcmAncillaryQuery.ToList().
               Where(x => x.Any(x => x.Externalfacingflag != null))
               .Select(p => new FilterValueDto
               {
                   Text = p.FirstOrDefault()?.Externalfacingflag == true ? ConstantValueFilter.yes : ConstantValueFilter.no,
                   Value = p.FirstOrDefault()?.Externalfacingflag.ToString()
               }).Distinct().ToList(),
                #endregion
                #region SPOC And Vertical filetring based on ORG table
                "verticalEngineeringTeam" => string.IsNullOrEmpty(propertyFilter)
                                ? query.SelectMany(x => x.Lcmengineering.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid)
                                ).SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Verticalresponsible,
                                    Value = p.Verticalresponsibleid.ToString()
                                }).Distinct().ToList()
                                 .Concat(query.Where(x => x.Lcmengineering != null && x.Lcmengineering.Lcmengineeringsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                     _commonManager.AddBlankFilterValue()
                                   )).Distinct().ToList()
                                : query.SelectMany(x => x.Lcmengineering.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid))
                                .SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Verticalresponsible,
                                    Value = p.Verticalresponsibleid.ToString()
                                }).Where(x => x.Text.Contains(propertyFilter)).Distinct().ToList()
                                 .Concat(query.Where(x => x.Lcmengineering != null && x.Lcmengineering.Lcmengineeringeduspoc.Count() <= 0)
                                   .Select(x =>

                                      _commonManager.AddBlankFilterValue()
                                   )).Distinct().ToList(),
                "verticalSubDomain" => string.IsNullOrEmpty(propertyFilter)
                                ? query.SelectMany(x => x.Networkelementasplannedsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid)
                                ).Select(y =>
                                new FilterValueDto
                                {
                                    Text = y.Subdomainspoc.Subdomainresponsible.Subdomainresponsible,
                                    Value = y.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString()
                                }).Distinct().ToList()
                                 .Concat(query.Where(x => x.Networkelementasplannedsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                      _commonManager.AddBlankFilterValue()
                                   ))).Distinct().ToList()
                                : query.SelectMany(x => x.Networkelementasplannedsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid))                               
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsible,
                                    Value = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString()
                                }).Where(x => x.Text.Contains(propertyFilter)).Distinct().ToList()
                                 .Concat(query.Where(x => x.Networkelementasplannedsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                      _commonManager.AddBlankFilterValue()
                                   ))).Distinct().ToList(),

                "engineeringContactPoint" => comSpocs.Select(x => new FilterValueDto { Text = x.Text, Value = x.Value }).Distinct().ToList(),

                #endregion

                #region //Bag Filters
                "bagName" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = _commonManager.GetBuildBagDescription(p.Buildbag), Value = p.Buildbagid.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.Buildbagid.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = _commonManager.GetBuildBagDescription(p.Buildbag), Value = p.Buildbagid.ToString() }).Distinct()
                   .ToList(),
                "componentName" => string.IsNullOrEmpty(propertyFilter)
               ? query.ToList().SelectMany(x => x.Buildbag.Componentsoftwarebuildbags.Select(p => new FilterValueDto
               {
                   Text = ComponentBagExtensionMethod.GetComponentBagDescription(_repositoryWrapper, p.Componentsoftwarebuildid),
                   Value = p.Componentsoftwarebuildid.ToString()
               })).Distinct().ToList()
               : query.ToList()
                   .Where(x => x.Buildbagid.ToString().Contains(propertyFilter)).SelectMany(x => x.Buildbag.Componentsoftwarebuildbags.Select(p => new FilterValueDto
                   {
                       Text = ComponentBagExtensionMethod.GetComponentBagDescription(_repositoryWrapper, p.Componentsoftwarebuildid),
                       Value = p.Componentsoftwarebuildid.ToString()
                   })).Distinct().ToList(),
                "componentResourceKey" => string.IsNullOrEmpty(propertyFilter)
               ? query.ToList().SelectMany(x => x.Buildbag.Componentsoftwarebuildbags.Select(p => new FilterValueDto
               {
                   Text = ComponentBagExtensionMethod.GetComponentResourcekey(_repositoryWrapper, x.Opcoid, ComponentBagExtensionMethod.GetComponentBagDescription(_repositoryWrapper, p.Componentsoftwarebuildid),
                   x.Designcomponent.Designcomponentfamilyid),
                   Value = p.Componentsoftwarebuildid.ToString()
               })).Distinct().ToList()
               : query.ToList()
                   .Where(x => x.Buildbagid.ToString().Contains(propertyFilter)).SelectMany(x => x.Buildbag.Componentsoftwarebuildbags.Select(p => new FilterValueDto
                   {
                       Text = ComponentBagExtensionMethod.GetComponentResourcekey(_repositoryWrapper, x.Opcoid, ComponentBagExtensionMethod.GetComponentBagDescription(_repositoryWrapper, p.Componentsoftwarebuildid),
                       x.Designcomponent.Designcomponentfamilyid),
                       Value = p.Componentsoftwarebuildid.ToString()
                   })).Distinct().ToList()
                   .ToList(),
                #endregion

                _ => new List<FilterValueDto>(),
            };
            if (!isAdmin && (buildFilterDto.VerticalEngineeringTeam != null && buildFilterDto.VerticalEngineeringTeam.Count > 0) && propertyName == "verticalEngineeringTeam")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalEngineeringTeam.Contains(x.Value.ToString())).ToList();
            }



            return rtn;

        }


        private IQueryable<Networkelementsasplanned> GetDisaggregatedQuery(ExpressionStarter<Networkelementsasplanned> predicateResult)
        {
            var lcmDeploymentstatusInServiceId = _commonManager.GetLcmDeploymentStatusId("in-service");
            var environmentId = _commonManager.GetEnvironmentId("production");

            var result = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult).AsNoTracking().AsSplitQuery()
                                .Where(x =>
                                    x.Designcomponent.Systemtype.Deleted == false &&
                                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(s => s.Deleted == false) &&
                                    x.Lcmengineeringid != null
                                   && x.Deploymentstatus.Deploymentstatusid == lcmDeploymentstatusInServiceId
                                    && x.Environment.Environmentid == environmentId
                                )
                                .Include(x => x.Opco)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmancillarydata)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Productimportance)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Criticalassettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetclass)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation).ThenInclude(x => x.Riskclustervodafonenames).ThenInclude(x => x.Riskcluster)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetworksupportedsvr).ThenInclude(x => x.Service)
                                //.Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designaspects).ThenInclude(x => x.Designaspectssupportedsvr).ThenInclude(x => x.Service)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Activitystatus)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Engineeringrisk)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Operationalrisk)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverytrackings)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.ProgramNavigation)
                                //.Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering)
                                .Include(x => x.Identitiesasis).ThenInclude(x => x.Category)
                                .Include(x=>x.Buildbag).ThenInclude(x=>x.Componentsoftwarebuildbags);

            return result;
        }

        public ExpressionStarter<Networkelementsasplanned> ApplyDisaggregatedFilter(ReportSoftwareQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);

            var predicateInner = PredicateBuilder.New<Networkelementsasplanned>(true);
            if (buildFilterDto?.BptID != null && buildFilterDto.BptID.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.BptID)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PpmID != null && buildFilterDto.PpmID.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.PpmID)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SerialNumber != null && buildFilterDto.SerialNumber.Any())
            {
                foreach (var item in buildFilterDto?.SerialNumber)
                    predicateInner.Or(x => x.Swresourcekey == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectStatus != null && buildFilterDto.ProjectStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ProjectStatus)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProgramLcm != null && buildFilterDto.ProgramLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ProgramLcm)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectOwner != null && buildFilterDto.ProjectOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ProjectOwner)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectowner == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.Hostname != null && buildFilterDto.Hostname.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Hostname)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IpAddress != null && buildFilterDto.IpAddress.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.IpAddress)
                    predicateInner.Or(x => x.Identitiesasis.Any(x => x.Value.ToString() == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.Category != null && buildFilterDto.Category.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Category)
                    predicateInner.Or(x => x.Identitiesasis.Any(x => x.Categoryid.ToString() == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.InterfaceType != null && buildFilterDto.InterfaceType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.InterfaceType)
                    predicateInner.Or(x => x.Identitiesasis.Any(x => x.Interfacetype.ToString() == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.RiskCluster != null && buildFilterDto.RiskCluster.Any())
            {
                var entitieswithdata = _repositoryWrapper.RiskClusterVodafoneNamesRepository.FindAll().Include(x => x.Riskcluster).ToList();
                var filterEntities = entitieswithdata.Where(entity =>
                {
                    foreach (var item in buildFilterDto?.RiskCluster)
                    {
                        if (entity.Riskcluster.Riskclusterid == item)
                        {
                            return true;
                        }
                    }
                    return false;
                });

                var filteredEntityIds = filterEntities.Select(x => x.Riskcluster.Riskclusterid).ToList();
                predicateResult.And(x => filteredEntityIds.Contains(x.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames.Select(x => x.Riskcluster.Riskclusterid).FirstOrDefault()));
            }

            if (buildFilterDto?.Criticality != null && buildFilterDto.Criticality.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Criticality)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Criticality == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.GdprRelevant != null && buildFilterDto.GdprRelevant.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.GdprRelevant)
                {
                    if (item == ConstantValueFilter.YES)
                    {
                        predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Gdprrelevant == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Gdprrelevant == false);
                    }
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.TypeOfNetworkElement != null && buildFilterDto.TypeOfNetworkElement.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.TypeOfNetworkElement)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Lcmpolicy == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DeliveryPlanAvailable != null && buildFilterDto.DeliveryPlanAvailable.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.DeliveryPlanAvailable)
                {
                    if (item == ConstantValueFilter.YES)
                    {
                        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmsoftware)
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmsoftware)
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == false);
                    }
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ReportId != null && buildFilterDto.ReportId.Any())
            {
                foreach (var item in buildFilterDto?.ReportId)
                    predicateInner.Or(x => x.Lcmengineering.Resourcekey + "-" + x.Swresourcekey + "0" == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OriginalLCMSpreadsheetID != null && buildFilterDto.OriginalLCMSpreadsheetID.Any())
            {
                foreach (var item in buildFilterDto?.OriginalLCMSpreadsheetID)
                    predicateInner.Or(x => x.Previousswresourcekey == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.LocalMarket != null && buildFilterDto.LocalMarket.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.LocalMarket)
                    predicateInner.Or(x => x.Opco.Opco == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.LocalMarketId != null && buildFilterDto.LocalMarketId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.LocalMarketId)
                    predicateInner.Or(x => x.Opco.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.DesignComponentIndex != null && buildFilterDto.DesignComponentIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.DesignComponentIndex)
                    predicateInner.Or(x => x.Designcomponentid.ToString() == item);
                predicateResult.And(predicateInner);
            }

            #region Vertical and Spocs Filters

            if (buildFilterDto?.VerticalEngineeringTeam != null && buildFilterDto.VerticalEngineeringTeam.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                int count = 0;

                foreach (var item in buildFilterDto?.VerticalEngineeringTeam)
                    if (item == "yes")
                    {
                        count++;

                        predicateInner.Or(x => !x.Networkelementasplannedsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;

                        predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser.Any
                         (m => m.Organisation.Vertical.Verticalresponsibleid.ToString() == item && m.Deleted == false /*&& m.Opcoid == x.Opcoid*/)));
                    }

                if (count > 0) predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.VerticalSubDomain != null && buildFilterDto.VerticalSubDomain.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                int count = 0;

                foreach (var item in buildFilterDto?.VerticalSubDomain)
                    if (item == "yes")
                    {
                        count++;

                        predicateInner.Or(x => !x.Networkelementasplannedsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;

                        predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuseropcosUser.Any
                     (m => m.Deleted == false && m.Opcoid == x.Opcoid))
                     && x.Networkelementasplannedsubdomainspoc.Any(d => d.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString() == item));
                    }

                if (count > 0) predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngineeringContactPoint != null && buildFilterDto.EngineeringContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.EngineeringContactPoint)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => x.Lcmengineering != null && !x.Lcmengineering.Lcmengineeringsubdomainspoc.Any() && !x.Lcmengineering.Lcmengineeringeduspoc.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering != null && x.Lcmengineering.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.Id.ToString() == item) ||
                            x.Lcmengineering != null && x.Lcmengineering.Lcmengineeringeduspoc.Any(d => d.Eduspoc.Id.ToString() == item));
                    }

                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OperationsContactPoint != null && buildFilterDto.OperationsContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OperationsContactPoint)
                    predicateInner.Or(x => x.Lcmengineering.Lcmoperationalcontracts.Any(d => d.Operationalcontract.Description == item));
                predicateResult.And(predicateInner);
            }
            #endregion

            if (buildFilterDto?.AssetCategory != null && buildFilterDto.AssetCategory.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.AssetCategory)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Assetcategory.Assetcategory == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.AssetClass != null && buildFilterDto.AssetClass.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.AssetClass)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description == item : false);

                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.AssetType != null && buildFilterDto.AssetType.Any())
            {

                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.AssetType)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Criticalassettype != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Criticalassettype.Description == item : false);

                predicateResult.And(predicateInner);

            }

            if (buildFilterDto?.AssetDescription != null && buildFilterDto.AssetDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.AssetDescription)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Description == item);

                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProductImportance != null && buildFilterDto.ProductImportance.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ProductImportance)
                    predicateInner.Or(x => x.Lcmengineering.Productimportance.Productimportance == item);

                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Vendor != null && buildFilterDto.Vendor.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Vendor)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.PlannedAction != null && buildFilterDto.PlannedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.PlannedAction)
                    switch (item)
                    {
                        case 1:
                            predicateInner.Or(x => x.Lcmengineering.Onhardware);
                            break;
                        case 2:
                            predicateInner.Or(x => x.Lcmengineering.Onsoftware);
                            break;
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OverallRiskEvaluation != null && buildFilterDto.OverallRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OverallRiskEvaluation)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.DescriptionOfPlannedAction != null && buildFilterDto.DescriptionOfPlannedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.DescriptionOfPlannedAction)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                     .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                     .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PlannedSoftwareVersion != null && buildFilterDto.PlannedSoftwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.PlannedSoftwareVersion)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                     .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Activitydetails == item ||
                        x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                     .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Activitydetails == null && item == ConstantValueFilter.Na);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.TrackingNumberProjectNameLcm != null && buildFilterDto.TrackingNumberProjectNameLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.TrackingNumberProjectNameLcm)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Notes != null && buildFilterDto.Notes.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Notes)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.VendorEndOfMaintenanceDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.VendorEndOfMaintenanceDateValue.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance >= buildFilterDto.VendorEndOfMaintenanceDateValue.StartDate);
                if (buildFilterDto?.VendorEndOfMaintenanceDateValue.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= buildFilterDto.VendorEndOfMaintenanceDateValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IsExtendedSupportOfferedByVendor != null && buildFilterDto.IsExtendedSupportOfferedByVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.IsExtendedSupportOfferedByVendor)
                {
                    if (item.ToLower() == ConstantValueFilter.Yes)
                    {
                        predicateInner.Or(x => x.Lcmengineering.Isextendedsupportofferedbyvendor == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.Isextendedsupportofferedbyvendor == false || !x.Lcmengineering.Isextendedsupportofferedbyvendor.HasValue);
                    }
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectEndDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.ProjectEndDateValue.StartDate != null)
                    predicateInner.And(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion >= buildFilterDto.ProjectEndDateValue.StartDate);
                if (buildFilterDto?.ProjectEndDateValue.EndDate != null)
                    predicateInner.And(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion <= buildFilterDto.ProjectEndDateValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.VendorEndOfVulnerabilitySecuritySupportDateValueLcm != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDateValueLcm.StartDate != null)
                    predicateInner.And(x => (x.Lcmengineering.Softwareendofwarrantydate != null ? x.Lcmengineering.Softwareendofwarrantydate : x.Lcmengineering.Softwareendofsupportcontract) >= buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDateValueLcm.StartDate);

                if (buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDateValueLcm.EndDate != null)
                    predicateInner.And(x => (x.Lcmengineering.Softwareendofwarrantydate != null ? x.Lcmengineering.Softwareendofwarrantydate : x.Lcmengineering.Softwareendofsupportcontract) <= buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDateValueLcm.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetVirtualized != null && buildFilterDto.AssetVirtualized.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.AssetVirtualized)
                {
                    if (item == ConstantValueFilter.YES)
                    {
                        predicateInner.Or(x => x.Lcmengineering.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Lcmengineering.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Buildconstruction.Iscloudasset == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Lcmengineering.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Buildconstruction.Iscloudasset == false);

                    }
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SoftwareVersion != null && buildFilterDto.SoftwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.SoftwareVersion)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OpsMaintenanceConractEndValueLcm != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.OpsMaintenanceConractEndValueLcm.StartDate != null)
                    predicateInner.And(x => (x.Lcmengineering.Softwareendofwarrantydate != null ? x.Lcmengineering.Softwareendofwarrantydate : x.Lcmengineering.Softwareendofsupportcontract) >= buildFilterDto.OpsMaintenanceConractEndValueLcm.StartDate);

                if (buildFilterDto.OpsMaintenanceConractEndValueLcm.EndDate != null)
                    predicateInner.And(x => (x.Lcmengineering.Softwareendofwarrantydate != null ? x.Lcmengineering.Softwareendofwarrantydate : x.Lcmengineering.Softwareendofsupportcontract) <= buildFilterDto.OpsMaintenanceConractEndValueLcm.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.BudgetEstimated != null && buildFilterDto.BudgetEstimated.Any())
            {

                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.BudgetEstimated)
                {
                    var value = decimal.TryParse(item, out _);
                    if (value)
                    {
                        var decimalValue = decimal.Parse(item);
                        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                         .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                         .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgetvalue == decimalValue);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                            .OrderBy(x => x.Plannedcompletion).Select(s => new { s.Budgetvalue, s.Currency }).AsQueryable().Select(x => x.Budgetvalue.ToString() + x.Currency).FirstOrDefault() == item);
                    }
                }

                predicateResult.And(predicateInner);

            }
            if (buildFilterDto?.BundleId != null && buildFilterDto.BundleId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.BundleId)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid.Substring(2) == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Platform != null && buildFilterDto.Platform.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Platform)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null && x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.EngRiskEvaluation != null && buildFilterDto.EngRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.EngRiskEvaluation)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngRiskEvaluationNotes != null && buildFilterDto.EngRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.EngRiskEvaluationNotes)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluation != null && buildFilterDto.OpsRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluation)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OpsRiskEvaluationNotes != null && buildFilterDto.OpsRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluationNotes)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OverallRiskEvaluation != null && buildFilterDto.OverallRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OverallRiskEvaluation)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.IsExtendedSupportOfferedByVendor != null && buildFilterDto.IsExtendedSupportOfferedByVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.IsExtendedSupportOfferedByVendor)
                {
                    if (item.ToLower() == ConstantValueFilter.Yes)
                    {
                        predicateInner.Or(x => x.Lcmengineering.Isextendedsupportofferedbyvendor == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.Isextendedsupportofferedbyvendor == false || !x.Lcmengineering.Isextendedsupportofferedbyvendor.HasValue);
                    }
                }
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.LcmStatus != null && buildFilterDto.LcmStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.LcmStatus)
                    predicateInner.Or(x => x.Lcmengineering.Lcmstatussoftware == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.ReasonfornoPlan != null && buildFilterDto.ReasonfornoPlan.Any())
            {
                foreach (var item in buildFilterDto?.ReasonfornoPlan)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Reasonfornoplan == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.CommentonProjectStatus != null && buildFilterDto.CommentonProjectStatus.Any())
            {
                foreach (var item in buildFilterDto?.CommentonProjectStatus)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Commentonprojectstatus == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SecurityRiskPotential != null && buildFilterDto.SecurityRiskPotential.Any())
            {
                var entitieswithdata = _repositoryWrapper.RiskClusterVodafoneNamesRepository.FindAll().Include(x => x.Riskcluster).ToList();
                var filterEntities = entitieswithdata.Where(entity =>
                {
                    foreach (var item in buildFilterDto?.SecurityRiskPotential)
                    {
                        if (entity.Riskcluster.Risklevel == item)
                        {
                            return true;
                        }
                    }
                    return false;
                });

                var filteredEntityIds = filterEntities.Select(x => x.Riskcluster.Risklevel).ToList();
                predicateResult.And(x => filteredEntityIds.Contains(x.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames.Select(x => x.Riskcluster.Risklevel).FirstOrDefault()));
            }
            if (buildFilterDto?.SecurityRiskEffective != null && buildFilterDto.SecurityRiskEffective.Any())
            {
                foreach (var item in buildFilterDto?.SecurityRiskEffective)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Securityriskeffective == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SecurityMitigation != null && buildFilterDto.SecurityMitigation.Any())
            {
                foreach (var item in buildFilterDto?.SecurityMitigation)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Securitymitigation == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IncludedinSecurityScanning != null && buildFilterDto.IncludedinSecurityScanning.Any())
            {
                foreach (var item in buildFilterDto?.IncludedinSecurityScanning)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Includedinsecurityscanning == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.RaId != null && buildFilterDto.RaId.Any())
            {
                foreach (var item in buildFilterDto?.RaId)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Raid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.RequestIDLcm != null && buildFilterDto.RequestIDLcm.Any())
            {
                foreach (var item in buildFilterDto?.RequestIDLcm)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Requestid == item));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastScanDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.LastScanDateValue.StartDate != null)
                    predicateInner.And(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Lastscandate >= buildFilterDto.LastScanDateValue.StartDate));
                if (buildFilterDto.LastScanDateValue.EndDate != null)
                    predicateInner.And(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Lastscandate <= buildFilterDto.LastScanDateValue.EndDate));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.AssetOutofScopeForReportingPurposes != null && buildFilterDto.AssetOutofScopeForReportingPurposes.Any())
            {
                foreach (var item in buildFilterDto?.AssetOutofScopeForReportingPurposes)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Assetoutofscope == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastUpgradeDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.LastUpgradeDateValue.StartDate != null)
                    predicateInner.And(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Lastupgradedate >= buildFilterDto.LastUpgradeDateValue.StartDate));
                if (buildFilterDto.LastUpgradeDateValue.EndDate != null)
                    predicateInner.And(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Lastupgradedate <= buildFilterDto.LastUpgradeDateValue.EndDate));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.EomControl != null && buildFilterDto.EomControl.Any())
            {
                foreach (var item in buildFilterDto?.EomControl)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Eomcontrol == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngUpdateTracker != null && buildFilterDto.EngUpdateTracker.Any())
            {

                foreach (var item in buildFilterDto?.EngUpdateTracker)
                {
                    if (item.ToLower() == _engUpdateTracker.ToLower())
                    {
                        predicateInner.Or(x => !x.Lcmengineering.Lcmancillarydata.Any());
                        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Engupdatetracker == item));
                    }

                    else
                        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Engupdatetracker == item));
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsUpdateTracker != null && buildFilterDto.OpsUpdateTracker.Any())
            {
                foreach (var item in buildFilterDto?.OpsUpdateTracker)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Opsupdatetracker == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EXNetworks != null && buildFilterDto.EXNetworks.Any())
            {
                foreach (var item in buildFilterDto?.EXNetworks)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Exnetworks == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.NEWOPSRiskEvaluation != null && buildFilterDto.NEWOPSRiskEvaluation.Any())
            {
                if (buildFilterDto?.NEWOPSRiskEvaluation != null && buildFilterDto.NEWOPSRiskEvaluation.Any())
                {
                    var entitieswithdata = _repositoryWrapper.NetworkElementAsPlanned.FindAll().Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmancillarydata).ToList();
                    var filteredEntities = entitieswithdata.Where(entity =>
                    {
                        foreach (var item in buildFilterDto.NEWOPSRiskEvaluation)
                        {
                            if (entity.Lcmengineering.Lcmancillarydata.Any(y =>
                     GetNewOpsRiskEvaluationValue(y.Incidentclass, y.Occurenceprobability) == item))
                            {
                                return true;
                            }
                        }
                        return false;
                    });
                    var filteredEntityIds = filteredEntities.Select(entity => entity.Lcmengineeringid).ToList();
                    predicateResult.And(x => filteredEntityIds.Contains(x.Lcmengineeringid));
                }
            }
            if (buildFilterDto?.OccurrenceProbability != null && buildFilterDto.OccurrenceProbability.Any())
            {
                foreach (var item in buildFilterDto?.OccurrenceProbability)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Occurenceprobability == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IncidentClass != null && buildFilterDto.IncidentClass.Any())
            {
                foreach (var item in buildFilterDto?.IncidentClass)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Incidentclass == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ProductCode != null && buildFilterDto.ProductCode.Any())
            {
                foreach (var item in buildFilterDto?.ProductCode)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Productcode == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.HandedOverToOperation != null && buildFilterDto.HandedOverToOperation.Any())
            {
                foreach (var item in buildFilterDto?.HandedOverToOperation)
                {
                    if (item == true)
                    {
                        predicateInner.Or(x => !x.Lcmengineering.Lcmancillarydata.Any());
                        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Handedovertooperation == item));
                    }
                    else
                        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Handedovertooperation == item));
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ContractRenewalPlan != null && buildFilterDto.ContractRenewalPlan.Any())
            {
                foreach (var item in buildFilterDto?.ContractRenewalPlan)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Contractrenewalplan == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.DataSourceLcm != null && buildFilterDto.DataSourceLcm.Any())
            {
                foreach (var item in buildFilterDto?.DataSourceLcm)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Datasource == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ScopeOfSimplification != null && buildFilterDto.ScopeOfSimplification.Any())
            {
                foreach (var item in buildFilterDto?.ScopeOfSimplification)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Scopeofsimplification == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SecurityRiskOverallLcm != null && buildFilterDto.SecurityRiskOverallLcm.Any())
            {
                var entitieswithdata = _repositoryWrapper.NetworkElementAsPlanned.FindAll().Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmancillarydata)
                   .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation).ThenInclude(x => x.Riskclustervodafonenames).ThenInclude(x => x.Riskcluster).ToList();
                var filteredEntities = entitieswithdata.Where(entity =>
                {
                    foreach (var item in buildFilterDto.SecurityRiskOverallLcm)
                    {
                        if (entity.Lcmengineering.Lcmancillarydata.Any(y =>
                 GetSecurityRiskOverAllValue(y.Securityriskeffective, entity.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames != null ?
                 entity.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames
                 .Select(x => x.Riskcluster.Risklevel).FirstOrDefault() : string.Empty) == item))
                        {
                            return true;
                        }
                    }
                    return false;
                });
                var filteredEntityIds = filteredEntities.Select(entity => entity.Lcmengineeringid).ToList();
                predicateResult.And(x => filteredEntityIds.Contains(x.Lcmengineeringid));
            }
            if (buildFilterDto?.OriginalSwLcmId != null && buildFilterDto.OriginalSwLcmId.Any())
            {
                foreach (var item in buildFilterDto?.OriginalSwLcmId)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Originalswlcmid == item));
                predicateResult.And(predicateInner);
            }
            #region Ticket 551
            #region // 719 Regualtory fields changes
            if (buildFilterDto?.IsPecn != null && buildFilterDto.IsPecn.Any())
            {
                foreach (var item in buildFilterDto?.IsPecn)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Ispecn == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IsPecs != null && buildFilterDto.IsPecs.Any())
            {
                foreach (var item in buildFilterDto?.IsPecs)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Ispecs == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IsNof != null && buildFilterDto.IsNof.Any())
            {
                foreach (var item in buildFilterDto?.IsNof)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Isnof == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IsScf != null && buildFilterDto.IsScf.Any())
            {
                foreach (var item in buildFilterDto?.IsScf)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Isscf == item));
                predicateResult.And(predicateInner);
            }
            #endregion
            if (buildFilterDto?.ExternalFacingFlag != null && buildFilterDto.ExternalFacingFlag.Any())
            {
                foreach (var item in buildFilterDto?.ExternalFacingFlag)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Externalfacingflag == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.InfrastructureLocation != null && buildFilterDto.InfrastructureLocation.Any())
            {
                foreach (var item in buildFilterDto?.InfrastructureLocation)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Locationinfrastructure == item);
                predicateResult.And(predicateInner);
            }

            #endregion
            #region DCF And SS Filtering
            if (buildFilterDto?.DesignComponentFamily != null && buildFilterDto.DesignComponentFamily.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.DesignComponentFamily)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.SupportedService != null && buildFilterDto.SupportedService.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.SupportedService)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Any(m => m.Serviceid == item));
                predicateResult.And(predicateInner);
            }
            #endregion

            #region //Bag Filters
            if (buildFilterDto?.BagName != null && buildFilterDto.BagName.Any())
            {
                foreach (var item in buildFilterDto?.BagName)
                    predicateInner.Or(x => x.Buildbagid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ComponentName != null && buildFilterDto.ComponentName.Any())
            {
                foreach (var item in buildFilterDto?.ComponentName)
                    predicateInner.Or(x => x.Buildbag.Componentsoftwarebuildbags.Any(y => y.Componentsoftwarebuildid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ComponentResourceKey != null && buildFilterDto.ComponentResourceKey.Any())
            {
                foreach (var item in buildFilterDto?.ComponentResourceKey)
                    predicateInner.Or(x => x.Buildbag.Componentsoftwarebuildbags.Any(y => y.Componentsoftwarebuildid == item));
                predicateResult.And(predicateInner);
            }
            #endregion
            return predicateResult;
        }

        private Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]> GetDisaggregatedColumnsMapDB()
        {
            return new Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]>
            {
                ["plannedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Onhardware, p => p.Lcmengineering.Onsoftware },
                ["softwareSheetIndex"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Softwaresheetindex },
                ["localMarket"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Opco.Opco },
                ["assetCategory"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Assetcategory.Assetcategory },
                ["assetClass"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.toAssetClassDescription(_repositoryWrapper) },
                ["assetType"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype.Assettype.Assettype : x.Designcomponent.Systemtype.VodafonenameNavigation != null ? x.Designcomponent.Systemtype.VodafonenameNavigation.Description : "" }, // .AssetTypeIdNavigation.AssetTypeDescription },
                ["assetDescription"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => !string.IsNullOrEmpty(p.Designcomponent.Designcomponentfamily.Description) ? p.Designcomponent.Designcomponentfamily.Description : p.Designcomponent.Designcomponentfamily.Description },
                ["productImportance"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Productimportance.Productimportance },
                ["vendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer },
                ["hardwareModel"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Single(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Platform, p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Single(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Hardwaretype },
                ["numberOfNodes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => 1 },
                ["operationsMaintenanceContract"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Outputtolcmsoftware },
                ["opsMaintenanceConractEnd"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Softwareendofsupportcontract },
                ["vendorEndOfMaintenanceDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance },
                ["lcmStatus"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Constraintlcm },

                ["descriptionOfPlannedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource },

                ["plannedSoftwareRelease"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Activitydetails },
                ["projectStatus"] = new Expression<Func<Networkelementsasplanned, object>>[] { x =>x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().GetPlannedActivityProjectStatus(_repositoryWrapper).Projectstatus },
                ["projectEndDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion },

                ["trackingNumberProjectName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname },

                ["notes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes },


                ["assetVirtualized"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.VodafonenameNavigation != null ? p.Designcomponent.Systemtype.VodafonenameNavigation.Description : "" },

                ["SoftwareVersion"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion },
                ["cloudVersion"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => "cloud version" },
                ["bundleBudget"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgetvalue,p=>p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Currency },

                ["bundleId"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid },

                ["assetServiceFunctionality"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => string.IsNullOrEmpty(p.Designcomponent.Subnetworkboundary.Alias) ? p.Designcomponent.Subnetworkboundary.Description : p.Designcomponent.Subnetworkboundary.Alias },

                ["platform"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Assetclass.Assetclass },
                ["lCMStatusENG"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => "cloud version" },
                ["lCMStatusOPS"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => "cloud version" },
                ["engRiskEvaluation"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description },
                ["engRiskEvaluationNotes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes },
                ["oPSRiskEvaluation"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description },
                ["oPSRiskEvaluationNotes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes },

                ["VendorEndOfVulnerabilitySecuritySupportDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport },
                ["overallRiskEvaluation"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation },

                ["outputToLcmSoftware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatussoftware },
                ["lcmStatusEngSoftware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatusengsoftware },
                ["lcmStatusOpsSoftware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatusopssoftware },
                ["isExtendedSupportofferedByVendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Isextendedsupportofferedbyvendor },
                ["deliveryPlanAvailable"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion },

                #region LCM R9 Part-1

                ["reasonfornoPlan "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Reasonfornoplan },
                ["commentonProjectStatus "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Commentonprojectstatus },
                ["securityRiskEffective "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Securityriskeffective },
                ["securityMitigation "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Securitymitigation },
                ["includedinSecurityScanning "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Includedinsecurityscanning },
                ["raId "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Raid },
                ["requestID "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Requestid },
                ["lastScanDate "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Lastscandate },
                ["assetOutofScopeForReportingPurposes "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Assetoutofscope },
                ["lastUpgradeDate "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Lastupgradedate },
                ["eomControl "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Eomcontrol },
                ["engUpdateTracker "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Engupdatetracker },
                ["opsUpdateTracker "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Opsupdatetracker },
                ["exNetworks "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Exnetworks },
                ["newopsRiskEvaluation "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Exnetworks },
                ["occurrenceProbability "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Occurenceprobability },
                ["incidentClass "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Incidentclass },
                ["productCode "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Productcode },
                ["handedOverToOperation "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Handedovertooperation },
                ["contractRenewalPlan "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Contractrenewalplan },
                ["dataSource "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Datasource },
                ["scopeOfSimplification "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Scopeofsimplification },
                ["labSWRelease"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Scopeofsimplification },
                #endregion
                #region ticket 551

                ["externalFacingFlag"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Externalfacingflag },
                #region // Regulatory fields changes
                ["ispecn"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Ispecn },
                ["isPecs"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Ispecs },
                ["isScf"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Isscf },
                ["isNof"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Isnof },
                #endregion
                #endregion
            };
        }
        #endregion

        #region Async Operations
        public async Task<QueryResultDto<NetworkLevelTwoSWReportDtoGrid>> FindDisaggregatedWithConditionAsync(ReportSoftwareQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyDisaggregatedFilter(buildFilterDto);


            var result = GetDisaggregatedQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetDisaggregatedColumnsMapDB());

            IEnumerable<Networkelementsasplanned> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();

            #region // filters
            if (buildFilterDto?.OutputToLcmSoftware != null && buildFilterDto.OutputToLcmSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.OutputToLcmSoftware.Contains(x.Lcmengineering.Lcmstatussoftware));
            }
            if (buildFilterDto?.LcmStatusEngSoftware != null && buildFilterDto.LcmStatusEngSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusEngSoftware.Contains(x.Lcmengineering.Lcmstatusengsoftware));
            }
            if (buildFilterDto?.LcmStatusOpsSoftware != null && buildFilterDto.LcmStatusOpsSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusOpsSoftware.Contains(x.Lcmengineering.Lcmstatusopssoftware));
            }
            if (buildFilterDto?.OperationsMaintenanceContractLcm != null && buildFilterDto.OperationsMaintenanceContractLcm.Any())
            {
                query = query.Where(x => buildFilterDto.OperationsMaintenanceContractLcm.Contains(x.Lcmengineering.Outputtolcmsoftware));
            }

            if (buildFilterDto?.ENGKPI2 != null && buildFilterDto.ENGKPI2.Any())
            {
                query = query.Where(x => buildFilterDto.ENGKPI2.Contains(GetEngKpi2(x.Lcmengineering.Lcmstatusengsoftware, x.Lcmengineering.Outputtolcmsoftware)));
            }
            if (buildFilterDto?.ExpLCMstatusatendofFY24 != null && buildFilterDto.ExpLCMstatusatendofFY24.Any())
            {
                query = query.Where(x => buildFilterDto.ExpLCMstatusatendofFY24
                             .Contains(GetExpLCMstatusatendofFY24(x.Lcmengineering.Lcmstatussoftware,
                                                                    x.Lcmengineering.Outputtolcmsoftware,
                                                                    x.Lcmengineering.Softwareendofwarrantydate != null ? x.Lcmengineering.Softwareendofwarrantydate : x.Lcmengineering.Softwareendofsupportcontract,
                                                                    x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Plannedcompletion,
                                                                     x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software))
                                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Projectstatus
                                                                    )));
            }

            if (buildFilterDto?.HardwareModel != null && buildFilterDto.HardwareModel.Any())
            {
                query = query.Where(x =>
                x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                   .SingleOrDefault(m =>
                       m.Ismain &&
                       m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                       m.Deleted == false)?.Majorhardware?.Buildconstruction?.Rule == (int)BuildconstructionRuleEnum.VirtualHW ?
                        buildFilterDto.HardwareModel.Contains(ConstantValueFilter.Virtualized) : buildFilterDto.HardwareModel.Contains(x.Designcomponent.Systemtype.toLcmDbExportHardwareName()));
            }
            if (buildFilterDto?.IdentifiedAction != null && buildFilterDto.IdentifiedAction.Any())
            {
                query = query.Where(x => buildFilterDto.IdentifiedAction.Contains(GetIdentificationActionForLcmExport(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software)).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Lcmengineering?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofmaintenance, _repositoryWrapper)));
            }
            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Budgettrackingid)));
            }

            if (buildFilterDto?.AssetServiceFunctionality != null && buildFilterDto.AssetServiceFunctionality.Any())
            {

                query = query.Where(x => x?.Designcomponent?.Designcomponentfamily?.Designaspects?.
                                      FirstOrDefault() != null).Where(x => buildFilterDto.AssetServiceFunctionality
                                 .Contains(string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Designaspects?.
                                      FirstOrDefault()?.Designaspectssupportedsvr?.Select(x => x?.Service?.Description)?.Distinct())));
            }
            if (buildFilterDto?.ExposedEdgeFlag != null && buildFilterDto.ExposedEdgeFlag.Any())
            {
                query = query.ToList().Where(x => x.Lcmengineering.Lcmancillarydata.Any())
                    .Where(x => buildFilterDto.ExposedEdgeFlag.Contains(GetExposedEdgeValue(x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Isexposededge)));
            }

            #region Ticket 465 - LCM export: RAG status field - filter is not working fine
            if (buildFilterDto?.RagStatus != null && buildFilterDto.RagStatus.Any())
            {
                List<List<string>> lcmIdList = new List<List<string>>();
                foreach (var item in buildFilterDto.RagStatus)
                    lcmIdList.Add(LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(query, null, 2, item).Where(x => x.Value != "0").Select(x => x.Text).ToList());

                if (lcmIdList != null && lcmIdList.Count() > 0)
                    query = query.Where(x => lcmIdList.SelectMany(y => y).Contains(x.Lcmengineeringid.ToString()));

            }
            #endregion
            #endregion

            var totalCount = query.SelectMany(x => x.Identitiesasis.DefaultIfEmpty()).Count();




            #region // Mapping Caluculated Value
            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != null).Select(x => (long)x.Lcmengineeringid)?.Distinct()?.ToList();

            var AssetIdAndOpcoId = query?.ToList()?.Where(x => x.Networkelementasplannedid != 0)?.DistinctBy(x => x?.Networkelementasplannedid)
                .ToDictionary(x => x.Networkelementasplannedid, x => (long)x.Opcoid);

            var allSubDomain = _commonManager.GetCalculatedAssetSubDomainSpocEntityforReport(AssetIdAndOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedAssetEduSpocEntityforReport(AssetIdAndOpcoId).ToList();

            var allOperationalContract = _commonManager.GetCalculateLCMOperationalContractsForReport(lcmengineeringsEntityId);


            var reportLastUpdateDate = await _repositoryWrapper.LcmDBExportUpdateHistory.FindAll().FirstOrDefaultAsync();
            #endregion

            bool isLcmDBExportUpdated = false;

            if (reportLastUpdateDate != null && reportLastUpdateDate.Lastswupdatedate.Date == DateTime.Now.Date)
            {
                isLcmDBExportUpdated = true;
            }



            #region // Grid mapping
            var reports = query.SelectMany(x => x.Identitiesasis.Where(d => buildFilterDto.InterfaceType == null || !buildFilterDto.InterfaceType.Any() ||
            (x.Identitiesasis != null && d.Interfacetype != null && buildFilterDto.InterfaceType.Contains(d.Interfacetype))).DefaultIfEmpty(), (x, identity) => new { x, identity })
            .SelectMany(y => y.x.Buildbag.Componentsoftwarebuildbags.DefaultIfEmpty(), (y, bag) =>
            {
               var grid = new NetworkLevelTwoSWReportDtoGrid();
               var systemType = y.x?.Designcomponent?.Systemtype;
               var majorSW = systemType?.Majorsoftwarebuilds;


               var hw = systemType?.Systemtypesmajorhardwarebuilds
                 ?.SingleOrDefault(m =>
                     m.Ismain &&
                     m.Systemtypeid == systemType?.Systemtypeid &&
                     m.Deleted == false)?.Majorhardware;

               var plannedActivitySWFilterCheck = y.x?.Lcmengineering.PlannedactivitiesLcmengineering?.AsQueryable().GetPlannedActivityFilteredDB(ConstantValueFilter.Software);

               var FilterPlanneSW = y.x.Lcmengineering?.PlannedactivitiesLcmengineering?.AsQueryable()
                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software)).OrderBy(x => x.Plannedcompletion);

               var plannedActivitySW = plannedActivitySWFilterCheck.Plannedactivityresourceid == null ? null : plannedActivitySWFilterCheck;
               #region
               var dcSubnetwork = y.x?.Designcomponent?.Subnetworkboundary;
               var Dcf = y.x?.Designcomponent?.Designcomponentfamily;

               if (!isLcmDBExportUpdated)
               {
                   y.x.Lcmengineering.Outputtolcmsoftware = y.x.Lcmengineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareOutput).Result;
                   y.x.Lcmengineering.Lcmstatussoftware = y.x.Lcmengineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatus).Result;
                   y.x.Lcmengineering.Lcmstatusopssoftware = y.x.Lcmengineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusOps).Result;
                   y.x.Lcmengineering.Lcmstatusengsoftware = y.x.Lcmengineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusEng).Result;
                   _repositoryWrapper.Lcmengineering.Update(y.x.Lcmengineering);
                   _repositoryWrapper.Save();
                   _repositoryWrapper.ClearTracker();
               }

               grid.DesignComponentFamily = y.x.Designcomponent.toDesignComponentFamily();
               grid.SupportedService = string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct() ?? new List<string>());
               grid.LcmStatus = y.x.Lcmengineering.Lcmstatussoftware;
               grid.OperationsMaintenanceContract = y.x.Lcmengineering.Outputtolcmsoftware;
               grid.LcmStatusOpsSoftware = y.x.Lcmengineering.Lcmstatusopssoftware;
               grid.LcmStatusEngSoftware = y.x.Lcmengineering.Lcmstatusengsoftware;

               grid.ReportId = $"{y.x.Lcmengineering.Resourcekey}-{y.x.Swresourcekey}-0";
               grid.LcmEngineeringId = y.x.Lcmengineering.Lcmengineeringid;
               grid.LocalMarket = y.x.Opco?.Opco;
               grid.DesignComponentIndex = y.x.Designcomponentid;

                #region // Bag details
                grid.BagName = _commonManager.GetBuildBagDescription(y.x.Buildbag);
                grid.ComponentName = ComponentBagExtensionMethod.GetComponentBagDescription(_repositoryWrapper, bag?.Componentsoftwarebuildid);
                grid.ComponentResourceKey = ComponentBagExtensionMethod.GetComponentResourcekey(_repositoryWrapper, y.x.Opcoid, grid.ComponentName, y.x.Designcomponent.Designcomponentfamilyid);
                #endregion

                #region code optimize org Table

                grid.VerticalEngineeringTeam =
             string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == y.x.Networkelementasplannedid && m.VerticalDic != null && y.x.Deleted == false)
               .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList())
               ;

               grid.VerticalSubDomain = string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == y.x.Networkelementasplannedid && m.SubdomainresponsiblesDic != null && y.x.Deleted == false)
          .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList());

               grid.EngineeringContactPoint = _commonManager.GetEngContactPointFromEduAndSubDomainSpoc(allEdu?.Where(m => m.NetWorkElementAsPlannedId == y.x.Networkelementasplannedid && y.x.Deleted == false).Select(t => t?.ContactEmail).ToList(),
                              allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == y.x.Networkelementasplannedid && y.x.Deleted == false).Select(t => t?.ContactEmail).ToList());

               grid.OperationsContactPoint = string.Join(" | ",
                  allOperationalContract?.Where(m => m.LcmengineeringId == y.x.Lcmengineeringid).Select(x => x?.OperationDescription).Distinct());

               #endregion

               grid.AssetCategory = systemType?.Assetcategory?.Assetcategory;

               grid.AssetClass = majorSW.Productname != null ? majorSW.Productname?.Description : "";

               grid.AssetType = majorSW.Criticalassettype?.Description;

               grid.AssetDescription = dcSubnetwork.Description;

               grid.ProductImportance = y.x.Lcmengineering.Productimportance?.Productimportance;

               grid.Vendor = majorSW.Orgeqpmanufacturer
                  ?.Originalequipmentmanufacturer;


               grid.HardwareModel = hw.Buildconstruction?.Rule == (int)BuildconstructionRuleEnum.VirtualHW ? ConstantValueFilter.Virtualized : systemType?.toLcmDbExportHardwareName();


               grid.NumberOfNodes = 1;



               grid.EOMStatus = majorSW != null ? (EOMEnum)majorSW.Eomstatus
                 : EOMEnum.NotSpecified;

               grid.VendorEndOfMaintenanceDate = majorSW.Endofmaintenance != null
                  ? majorSW.Endofmaintenance?.Date : null;

               grid.VendorEndOfMaintenanceDateValue = majorSW.Endofmaintenance != null
                 ? majorSW.Endofmaintenance?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : grid.EOMStatus == EOMEnum.NotAnnounced ? "Not Announced" : null;



               grid.DescriptionOfPlannedAction = plannedActivitySW
                  ?.Plannedactivityresource?.Plannedactivityresource;
               grid.PlannedSoftwareVersion = plannedActivitySW?.Activitydetails ?? ConstantValueFilter.Na;


               grid.ProjectStatus = plannedActivitySW?.Projectstatus;

               grid.ProjectEndDate = plannedActivitySW?.Plannedcompletion?.Date;

               grid.ProjectEndDateValue = plannedActivitySW?.Plannedcompletion?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


               grid.TrackingNumberProjectName = y.x.Lcmengineering.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Software);
               grid.Notes = plannedActivitySW?.Notes;

               grid.SoftwareVersion = majorSW?.Softwareversion;

               #region

               grid.SystemTypeId = y.x.Designcomponent?.Systemtypeid ?? 0;

               grid.DesignComponentId = y.x.Designcomponentid;
               grid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
               grid.MajorHardwareBuildId = y.x.Designcomponent.Systemtype?.Systemtypesmajorhardwarebuilds
                  ?.SingleOrDefault(m =>
                      m.Ismain && m.Systemtypeid == y.x.Designcomponent.Systemtype.Systemtypeid &&
                      m.Deleted == false)?.Majorhardwareid;

               grid.AssetVirtualized =
              hw?.Buildconstruction?.Iscloudasset == true
              ? ConstantValueFilter.YES
              : ConstantValueFilter.NO;

               grid.BundleBudget = GetBundleBudget(plannedActivitySW?.Budgettrackingid);

               grid.BundleId = GetBundleBudget(plannedActivitySW?.Budgettrackingid) == ConstantValueFilter.yes ? plannedActivitySW.Budgettrackingid.Substring(2) : null;

               grid.AssetServiceFunctionality = dcSubnetwork.Subnetworksupportedsvr?.Count() > 0 ? string.Join(" | ", dcSubnetwork.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct()) : string.Empty;

               grid.Platform = y.x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? y.x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description : "";

               grid.OpsMaintenanceConractEnd = y.x.Lcmengineering.Softwareendofwarrantydate != null ? y.x.Lcmengineering.Softwareendofwarrantydate : y.x.Lcmengineering.Softwareendofsupportcontract;
               grid.OpsMaintenanceConractEndValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

               grid.VendorEndOfVulnerabilitySecuritySupportDateValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;



               grid.EngRiskEvaluationNotes = plannedActivitySW
                ?.Riskengineeringnotes;

               grid.EngRiskEvaluation = _commonManager.GetRiskValue(plannedActivitySW
                 ?.Engineeringrisk?.Description);

               #endregion
               #endregion
               grid.OpsRiskEvaluation = _commonManager.GetRiskValue(plannedActivitySWFilterCheck.Plannedactivityresourceid == null ? null :
                               y.x.Lcmengineering.PlannedactivitiesLcmengineering?.Where(p => p.Operationalriskid.HasValue && buildFilterDto.OpsRiskEvaluation.Contains(p.Operationalriskid.Value)).GetPlannedActivityFilteredDB(ConstantValueFilter.Software)?.Operationalrisk?.Description); ;

               #region

               grid.OpsRiskEvaluationNotes = plannedActivitySW?.Riskoperationalnotes;


               grid.OverallRiskEvaluation = plannedActivitySW?.Overallriskevaluation;
               grid.PlannedActivityId = plannedActivitySW?.Plannedactivityid;

               grid.BudgetEstimated = y.x.Lcmengineering.PlannedactivitiesLcmengineering.GetPlannedActivityBudgetEstimated(ConstantValueFilter.Software);
               grid.ManagedByGdc = ConstantValueFilter.no;
               grid.DesignComponentFamilyId = y.x.Designcomponent?.Designcomponentfamilyid;
               grid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";

               grid.Archived = y.x.Lcmengineering.Archived;

               grid.MainOrganization = string.Empty;
               grid.DeliveryPlanAvailable = plannedActivitySW?.Deliveryplanavailable != null && plannedActivitySW.Deliveryplanavailable ? ConstantValueFilter.YES : ConstantValueFilter.NO;


               //grid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper);
               grid.RiskCluster = systemType?.VodafonenameNavigation?.Riskclustervodafonenames.Where(x => x.Riskcluster != null).Select(x => x.Riskcluster?.Description).FirstOrDefault();
                grid.Criticality = dcSubnetwork?.Criticality;
               grid.GdprRelevant = dcSubnetwork.Gdprrelevant != null ? dcSubnetwork.Gdprrelevant == true ? ConstantValueFilter.YES : ConstantValueFilter.NO : null;

               grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngSoftware, grid.OperationsMaintenanceContract);
               grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus, grid.OperationsMaintenanceContract, grid.OpsMaintenanceConractEnd, grid.ProjectEndDate, grid.ProjectStatus);
               grid.IsExtendedSupportOfferedByVendor = y.x.Lcmengineering.Isextendedsupportofferedbyvendor.HasValue && y.x.Lcmengineering.Isextendedsupportofferedbyvendor.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO;

               grid.IpAddress = y.identity?.Value;
               grid.Category = y?.identity?.Category?.Description;
               grid.InterfaceType = y?.identity?.Interfacetype;


               grid.Hostname = Convert.ToString(y.x.Elementname).Trim();

               #region Ticket 465 - LCM export: RAG status field - filter is not working fine
               grid.RagStatus = LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(null,
                query.Where(s => s.Lcmengineering.Lcmengineeringid == y.x.Lcmengineeringid).ToList(), 2
                ).FirstOrDefault().Text.ToString();
               #endregion

               grid.WbsCode = GetWbsCode(plannedActivitySW?.Deliveryprojectname);
               grid.BptID = plannedActivitySW?.Budgettrackingid;
               grid.PpmID = plannedActivitySW?.Deliveryprojectid;
               grid.MainOrganization = ConstantValueFilter.Nse;
               grid.SerialNumber = y.x.Swresourcekey;


               #region LCM R9 Part - 1
               var lcmAuditAttributes = y.x.Lcmengineering.Lcmancillarydata.FirstOrDefault();
               grid.SecurityRiskPotential = systemType?.VodafonenameNavigation?.Riskclustervodafonenames.Where(x => x.Riskcluster != null).Select(x => x.Riskcluster?.Risklevel).FirstOrDefault();
                grid.SecurityRiskOverall = GetSecurityRiskOverAllValue(lcmAuditAttributes != null ? lcmAuditAttributes.Securityriskeffective : string.Empty, grid.SecurityRiskPotential);

               if (lcmAuditAttributes != null)
               {

                   grid.ReasonfornoPlan = lcmAuditAttributes.Reasonfornoplan;
                   grid.CommentonProjectStatus = lcmAuditAttributes.Commentonprojectstatus;
                   grid.SecurityRiskEffective = lcmAuditAttributes.Securityriskeffective;
                   grid.SecurityMitigation = lcmAuditAttributes.Securitymitigation;

                   grid.IncludedinSecurityScanning = lcmAuditAttributes.Includedinsecurityscanning;
                   grid.RaId = lcmAuditAttributes.Raid;
                   grid.RequestID = lcmAuditAttributes.Requestid;
                   grid.LastScanDate = lcmAuditAttributes.Lastscandate?.Date;
                   grid.LastScanDateValue = lcmAuditAttributes.Lastscandate;
                   grid.AssetOutofScopeForReportingPurposes = lcmAuditAttributes.Assetoutofscope;
                   grid.LastUpgradeDate = lcmAuditAttributes.Lastupgradedate?.Date;
                   grid.LastUpgradeDateValue = lcmAuditAttributes.Lastupgradedate;
                   grid.EomControl = lcmAuditAttributes.Eomcontrol;
                   grid.EngUpdateTracker = lcmAuditAttributes.Engupdatetracker;
                   grid.OpsUpdateTracker = lcmAuditAttributes.Opsupdatetracker;
                   grid.ExNetworks = lcmAuditAttributes.Exnetworks;
                   grid.NewopsRiskEvaluation = GetNewOpsRiskEvaluationValue(lcmAuditAttributes.Incidentclass, lcmAuditAttributes.Occurenceprobability);
                   grid.OccurrenceProbability = lcmAuditAttributes.Occurenceprobability;
                   grid.IncidentClass = lcmAuditAttributes.Incidentclass;
                   grid.ProductCode = lcmAuditAttributes.Productcode;
                   grid.HandedOverToOperation = lcmAuditAttributes.Handedovertooperation;

                   grid.ContractRenewalPlan = lcmAuditAttributes.Contractrenewalplan;
                   grid.DataSource = lcmAuditAttributes.Datasource;
                   grid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;


                   grid.OriginalSwLcmId = lcmAuditAttributes.Originalswlcmid;
                   grid.LabSWRelease = "";
                   #region Ticket 551
                   #region //719 Regulatory Fields Changes
                   grid.IsPecn = lcmAuditAttributes.Ispecn;
                   grid.IsPecs = lcmAuditAttributes.Ispecs;
                   grid.IsScf = lcmAuditAttributes.Isscf;
                   grid.IsNof = lcmAuditAttributes.Isnof;
                   #endregion

                   grid.ExternalFacingFlag = lcmAuditAttributes.Externalfacingflag != null ?
                  lcmAuditAttributes.Externalfacingflag.Value.ToString() : "";
                   #endregion
               }
               grid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
               ? lcmAuditAttributes.Handedovertooperation : true;
               grid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
               ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;

               #endregion
               #endregion
               var Endofmaintenance = majorSW?.Endofmaintenance;

               grid.IdentifiedAction = y.x.Lcmengineering.Archived != true ? GetIdentificationActionForLcmExport(FilterPlanneSW.FirstOrDefault(), Endofmaintenance, _repositoryWrapper) : "";


               grid.Program = FilterPlanneSW.FirstOrDefault()
                ?.ProgramNavigation?.Programdescription;

               grid.ProjectOwner = FilterPlanneSW.FirstOrDefault()
                 ?.Projectowner;


               return grid;
           }).ToList();
            #endregion



            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lastswupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
                await _repositoryWrapper.ClearTracker();
            }

            if (!isExport)
            {
                reports = reports.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            else
            {
                reports = reports.ToList();
            }

            var rtn = new QueryResultDto<NetworkLevelTwoSWReportDtoGrid>(new GenerateRenderForGrid<NetworkLevelTwoSWReportDtoGrid>(_columnManager))
            {
                TotalItems = totalCount
            };

            rtn.Items = reports.ToArray();

            return rtn;
        }


        #endregion

    }
}


