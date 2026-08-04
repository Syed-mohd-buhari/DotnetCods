using AutoMapper.Internal;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.Rules;
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using static CAM.BusinessManager.Rules.LCMEngineeringRulesExtension;
namespace CAM.BusinessManager.Entity.Report
{
    public class ReportHardwareManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private static int currenYear = DateTime.Now.Year;
        private static readonly DateTime fronzenDate = new DateTime(currenYear, 6, 1);
        private static readonly DateTime targetDate = new DateTime(currenYear + 1, 6, 1);
        private CommonManager _commonManager;
        //private readonly AuthorizedRoleManager _authorizedRoleManager;
        //private readonly long sessionUserId;
        //private readonly int adminRoleId;
        //private readonly string _adminRoleCheck;
        //private readonly List<short> _opcoList;
        //private readonly ICurrentUserService _currentUserService;

        private readonly string _engUpdateTracker = "To be started";
        public ReportHardwareManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, CommonManager commonManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper
            //, AuthorizedRoleManager authorizedRoleManager  , ICurrentUserService currentUserService
            ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _commonManager = commonManager;
            //_currentUserService = currentUserService;
            //_authorizedRoleManager = authorizedRoleManager;
            //this.adminRoleId = _currentUserService.adminRoleId;
            //this.sessionUserId = _currentUserService.UserId;

            //var _roleOpcoList = _authorizedRoleManager.GetUserRoleOpcoList(this.sessionUserId);

            //_adminRoleCheck = _roleOpcoList.Where(x => x.Role == this.adminRoleId.ToString()).Select(x => x.Role).FirstOrDefault();

            //_opcoList = (_adminRoleCheck != null) ? null : _roleOpcoList.Select(x => Convert.ToInt16(x.OpCo)).Distinct().ToList();

        }

        public QueryResultDto<ReportHardwareDtoGrid> FindWithCondition(ReportHardwareQueryDto buildFilterDto, bool isExport = false)
        {
            //if (_opcoList != null && buildFilterDto.LocalMarket.Count() == 0)
            //    buildFilterDto.LocalMarketId = _opcoList;

            if (buildFilterDto != null && buildFilterDto.ViewMode == ReportViewMode.Disaggregated)
            {
                return FindDisaggregatedWithCondition(buildFilterDto, isExport);
            }
            else
            {
                return FindAggregatedWithCondition(buildFilterDto, isExport);
            }
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter,
         ReportHardwareQueryDto buildFilterDto,bool isAdmin=false)
        {
            //if (_adminRoleCheck == null && buildFilterDto.LocalMarket.Count() == 0)
            //    buildFilterDto.LocalMarketId = _opcoList;

            if (buildFilterDto.ViewMode == ReportViewMode.Disaggregated)
            {
                return GetDisaggregatedFilter(propertyName, propertyFilter, buildFilterDto);
            }
            else
            {
                return GetAggregatedFilter(propertyName, propertyFilter, buildFilterDto, isAdmin);
            }
        }


        public List<FilterValueDto> GetAggregatedFilter(string propertyName, string propertyFilter,
            ReportHardwareQueryDto buildFilterDto,bool isAdmin=false)
        {

            ExpressionStarter<Lcmengineering> predicateResult = ApplyAggregatedFilter(buildFilterDto);
            var query = GetQuery(predicateResult);

            var lcmAncillaryQuery = query.Where(x => x.Lcmancillarydata.Count > 0).
                Select(x => x.Lcmancillarydata);

            var eduSpoc = query.SelectMany(x => x.Lcmengineeringeduspoc.Select(y => new FilterValueDto { Text = y.Eduspoc.Email, Value = y.Eduspocid.ToString() })).ToList()
                .Concat(query.Where(x => x.Lcmengineeringeduspoc != null && x.Lcmengineeringeduspoc.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).ToList();

            var subDomSpoc = query.SelectMany(x => x.Lcmengineeringsubdomainspoc.Select(y => new FilterValueDto { Text = y.Subdomainspoc.Email, Value = y.Subdomainspocid.ToString() })).ToList()
                   .Concat(query.Where(x => x.Lcmengineeringsubdomainspoc != null && x.Lcmengineeringsubdomainspoc.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).ToList();

            var comSpocs = eduSpoc.Concat(subDomSpoc).DistinctBy(x => x.Value).ToList();

            var rtn = propertyName switch
            {
                "wbsCode" => query
                    .Select(p => new FilterValueDto
                     (GetWbsCode(p.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectname))).Distinct().ToList(),
                "bptID" => query
                .Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Projectstatus != null ?
                 p.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Budgettrackingid : string.Empty)).Distinct().ToList(),
                "ppmID" => query
                    .Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Projectstatus != null ?
                     p.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectid : string.Empty)).Distinct().ToList(),
                "mainOrganization" => new List<FilterValueDto> { new FilterValueDto() { Text = ConstantValueFilter.Nse.ToUpper(), Value = ConstantValueFilter.Nse.ToUpper() } },
                "serialNumber" => query.Select(p => new FilterValueDto
                { Text = p.Resourcekey, Value = p.Resourcekey }).Distinct().ToList(),

                "ipAddress" => query.SelectMany(x => x.Networkelementsasplanned)?.SelectMany(x => x.Identitiesasis)?.Where(p => p.Interfacetype == ConstantValueFilter.Management && p.Category.Description.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress)
                          .Select(p => new FilterValueDto(p.Value)).Distinct().ToList(),
                "engKpi2" => query.Select(p => new FilterValueDto
                (GetEngKpi2(p.Lcmstatusenghardware, p.Outputtolcmhardware))
                ).ToList().Distinct().ToList(),

                "expLCMstatusatendofFY24" => query.Select(p => new FilterValueDto
                       (GetExpLCMstatusatendofFY24(p.Lcmstatushardware,
                                                  p.Outputtolcmhardware,
                                                  p.Hardwareendofsupportcontract != null ? p.Hardwareendofsupportcontract : p.Softwareendofwarrantydate,
                                                  p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                                                  .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion,
                                                   p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                                                  .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus
                                                  ))).AsEnumerable().Distinct().ToList(),

                "reportId" => query.Select(p => new FilterValueDto(p.Resourcekey)).Distinct().ToList(),

                "previousReportId" => query.Select(p => new FilterValueDto(p.Previousresourcekey)).Distinct().ToList(),
                "originalLCMSpreadsheetID" =>
                query.Select(p => new FilterValueDto(p.Previousresourcekey)).Distinct().ToList(),

                "localMarket" => query.Select(p => new FilterValueDto(p.Opco.Opco)).Distinct().ToList(),

                "designComponentIndex" => query.Select(p => new FilterValueDto(p.Designcomponentid.ToString())).Distinct().ToList(),
                "assetCategory" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Assetcategory.Assetcategory)).Distinct().ToList(),
                "assetType" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware?.Buildconstruction.Buildconstruction)).Distinct().ToList(),
                "assetClass" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware?.Hardwaresolution)).Distinct().ToList(),
                "assetDescription" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent?.Designcomponentfamily?.Description)).Distinct().ToList(),

                "productImportance" => query.ToList().Where(x => x.Lcmancillarydata != null)
                .Select(p => new FilterValueDto(
                    p.Lcmancillarydata.FirstOrDefault(x => x.Assetoutofscope != null) != null ?
                     p.Lcmancillarydata.FirstOrDefault(x => x.Assetoutofscope != null).Assetoutofscope == "Asset in planned dismission, no replacement" ? "Under Dismission" 
                    : p.Productimportance?.Productimportance : p.Productimportance?.Productimportance
                    )).Distinct().ToList(),

                "vendor" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                        m.Deleted == false && m.Deletiondate == null).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer)).Distinct().ToList(),
                "hardwareModel" => query.AsEnumerable().Select(x => new FilterValueDto(x.Designcomponent.Systemtype.toLcmDbExportHardwareName())).Distinct().ToList(),
                "numberOfNodesLcm" => query.Select(p => new FilterValueDto(p.Numberofnodes.ToString())).Distinct().ToList(),
                "plannedAction" => new List<FilterValueDto>() { new FilterValueDto() { Text = ConstantValueFilter.OnHardware, Value = "1" }, new FilterValueDto() { Text = ConstantValueFilter.OnSoftware, Value = "2" } },
                "descriptionOfPlannedAction" => query.Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable()
                                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                                 .Plannedactivityresource.Plannedactivityresource)).Distinct().ToList(),
                "plannedHardwareModel" => query.Where(p => p.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                        .Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.HardwareUpgrade).ToList()
                       .Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Designcomponent?.Systemtype?.toLcmDbExportHardwareName())).Distinct().ToList(),

                "trackingNumberProjectNameLcm" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname)).Distinct().ToList(),
                "notes" => query.Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes)).Distinct().ToList(),

                #region Ticket 465 - LCM export: RAG status field - filter is not working fine

                "ragStatus" => LcmEngineeringExtensionMethod.aggregatedRagStatusFilterRecord(query, null, 1).Distinct().ToList(),
                #endregion

                "operationsContactPoint" => query.SelectMany(x => x.Lcmoperationalcontracts)
                                .Select(p => new FilterValueDto(p.Operationalcontract.Description)).Distinct().ToList(),
                "identifiedAction" => query.Select(x => new FilterValueDto(GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofmaintenance, _repositoryWrapper))).ToList().Distinct().ToList(),
                "softwareRelease" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion)).Distinct().ToList(),
                "budgetEstimated" => query.Select(s => s.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                                          .FirstOrDefault())?.AsEnumerable()
                                          ?.Select(p => new FilterValueDto(p?.Currency == null ? p?.Budgetvalue?.ToString() : p?.Budgetvalue?.ToString() + p?.Currency))?.Distinct()?.ToList(),
                "bundleBudget" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No) },
                "bundleId" => query.Where(x => GetBundleBudget(x.PlannedactivitiesLcmengineering.AsQueryable()
                             .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower()))
                             .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid) == ConstantValueFilter.Yes)
                                .Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                             .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower()))
                             .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid.Substring(2))).Distinct().ToList(),
                "assetServiceFunctionality" => query.Select(p => new FilterValueDto(string.Join(" | ", p.Designcomponent.Designcomponentfamily.Designaspects.
                                      FirstOrDefault().Designaspectssupportedsvr.Select(x => x.Service.Description)))).ToList().Distinct().ToList(),
                "platform" => query.Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m => m.Ismain && m.Deleted == false).Majorhardware.Hardwaresolution)).Distinct().ToList(),
                //LCM R8 Phase 1  
                "engRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = GetRiskValue(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description),
                    Value = x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid.ToString()
                }).Distinct().ToList(),
                "engRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes)).Distinct().ToList(),
                //LCM R8 Phase 1  
                "opsRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = GetRiskValue(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description),
                    Value = x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid.ToString()
                }).Distinct().ToList(),
                "opsRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "overallRiskEvaluationLcm" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "outputToLcmSoftware" => query.ToList().Select(p => new FilterValueDto(p.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatus).Result)).Distinct().ToList(),
                "operationsMaintenanceContractLcm" => query.ToList().Select(p => new FilterValueDto(p.Outputtolcmhardware)).Distinct().ToList(),
                "typeOfNetworkElement" => query.Where(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Lcmpolicy != null)
                                                        .Select(x => new FilterValueDto
                                                        {
                                                            Text = ((LCMPolicy)x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Lcmpolicy).ToString(),
                                                            Value = x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Lcmpolicy.ToString()
                                                        }).Distinct().ToList(),
                "lcmStatusOpsHardware" => query.ToList().Select(p => new FilterValueDto(p.Lcmstatusopshardware)).Distinct().ToList(),
                "lcmStatusEngHardware" => query.ToList().Select(p => new FilterValueDto(p.Lcmstatusenghardware)).Distinct().ToList(),
                "lcmStatus" => query.ToList().Select(p => new FilterValueDto(p.Lcmstatushardware)).Distinct().ToList(),
                //LCM R8 Phase 1  
                "riskCluster" => query.Where(x => x.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames != null).Select(p => new FilterValueDto(p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                .Select(x => x.Riskcluster.Riskclusterid).FirstOrDefault(),
                 p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                .Select(x => x.Riskcluster.Description).FirstOrDefault())).Distinct().ToList(),

                "criticality" => query.Select(p => new FilterValueDto(p.Designcomponent.Subnetworkboundary.Criticality)).Distinct().ToList(),
                "gdprRelevant" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No) },
                "hwIsExtendedSupportOfferedByVendor" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No) },
                "deliveryPlanAvailable" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No) },

                "assetStatus" => query.ToList().Where(x => x.Lcmancillarydata.Any())
                .Select(x => new FilterValueDto(GetAssetStatusBasedOnOriginalHwAndSw(
                    x.Lcmancillarydata.FirstOrDefault() != null ?
                    x.Lcmancillarydata.FirstOrDefault().Originalhwlcmid : string.Empty

                ))).Distinct().ToList(),

                "programLcm" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                           .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription)).ToList().Distinct().ToList(),
                //"projectOwner" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                //       .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                //         .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectowner)).ToList().Distinct().ToList(),
                "projectStatus" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus)).ToList().Distinct().ToList(),

                #region LCM R9 Part - 1
                //"custom " => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Custom)).Distinct().ToList(),
                //"custom1 " => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Custom1)).Distinct().ToList(),
                //"custom2 " => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Custom2)).Distinct().ToList(),
                //"kpiStatusService" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Kpistatusservice)).Distinct().ToList(),
                "reasonfornoPlan" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Reasonfornoplan)).Distinct().ToList(),
                "commentonProjectStatus" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Commentonprojectstatus)).Distinct().ToList(),
                "securityRiskPotential" => query.Where(x => x.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames != null)
                .Select(p => new FilterValueDto(p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                .Select(y => y.Riskcluster.Risklevel).FirstOrDefault())).Distinct().ToList(),
                "securityRiskEffective" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Securityriskeffective)).Distinct().ToList(),
                "securityMitigation" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Securitymitigation)).Distinct().ToList(),
                "securityRiskOverall" => query.ToList().Select(p => new FilterValueDto(GetSecurityRiskOverAllValue(p.Lcmancillarydata.FirstOrDefault()?.Securityriskeffective,
                p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                .Select(y => y.Riskcluster.Risklevel).FirstOrDefault()))).Distinct().ToList(),
                "includedinSecurityScanning" => query.ToList().Select(p => new FilterValueDto
                {
                    Text = p.Lcmancillarydata.FirstOrDefault()?.Includedinsecurityscanning == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                    Value = p.Lcmancillarydata.FirstOrDefault()?.Includedinsecurityscanning.ToString()
                }).Distinct().ToList(),
                "raId" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Raid)).Distinct().ToList(),
                "requestIDLcm" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Requestid)).Distinct().ToList(),
                //"id_New" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Idnew)).Distinct().ToList(),
                //"productImportanceHistory2" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Productimportancehistory2)).Distinct().ToList(),
                //"lcmStatusJune2021" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Lcmstatus)).Distinct().ToList(),
                "lastScanDate" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Lastscandate)).Distinct().ToList(),
                "assetOutofScopeForReportingPurposes" => query.ToList().Select(p => new FilterValueDto
                {
                    Text = _commonManager.GetAssetOutofScope(p.Lcmancillarydata.FirstOrDefault()?.Assetoutofscope),
                    Value = p.Lcmancillarydata.FirstOrDefault()?.Assetoutofscope
                })?.DistinctBy(x => x.Text)?.ToList(),
                "lastUpgradeDate" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Lastupgradedate)).Distinct().ToList(),
                "eomControl" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Eomcontrol)).Distinct().ToList(),
                "engUpdateTracker" =>
                query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.
                Engupdatetracker)).Distinct()
                //.Append(new FilterValueDto(new FilterValueDto
                //{
                //    Text = _engUpdateTracker,
                //    Value = _engUpdateTracker
                //})).Distinct()
                .ToList(),
                "opsUpdateTracker" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Opsupdatetracker)).Distinct().ToList(),
                "exNetworks" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Exnetworks)).Distinct().ToList(),
                "occurrenceProbability" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Occurenceprobability)).Distinct().ToList(),
                "incidentClass" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Incidentclass)).Distinct().ToList(),
                "newopsRiskEvaluation" => query.ToList().Select(p => new FilterValueDto(GetNewOpsRiskEvaluationValue(p.Lcmancillarydata.FirstOrDefault()?.Incidentclass, p.Lcmancillarydata.FirstOrDefault()?.Occurenceprobability))).Distinct().ToList(),
                "productCode" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Productcode)).Distinct().ToList(),
                "handedOverToOperation" => query.ToList().Select(p => new FilterValueDto { Text = p.Lcmancillarydata.FirstOrDefault()?.Handedovertooperation == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.Lcmancillarydata.FirstOrDefault()?.Handedovertooperation.ToString() }).Distinct().ToList(),
                "contractRenewalPlan" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Contractrenewalplan)).Distinct().ToList(),
                "dataSourceLcm" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Datasource)).Distinct().ToList(),
                "scopeOfSimplification" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Scopeofsimplification)).Distinct().ToList(),
                //"cloudVersion" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault().Cloudversion)).Distinct().ToList(),
                //"certifiedSWReleaseforNFVIbundle" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault().Certifiedswrealesefornfvibundle)).Distinct().ToList(),
                "labSWRelease" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Productcode)).Distinct().ToList(),
                "originalHwLcmId" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Originalhwlcmid)).Distinct().ToList(),
                //"vulnerabilityRating" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Vulnerabilityrating)).Distinct().ToList(),
                //"cyberRiskRequestId" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Cyberriskrequestid)).Distinct().ToList(),

                #endregion

                #region Ticket 551
                #region 719 Regulatory fields changes
                "isPecn" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Ispecn != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Ispecn == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.FirstOrDefault()?.Ispecn.ToString() }).Distinct().ToList(),
                "isPecs" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Ispecs != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Ispecs == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.FirstOrDefault()?.Ispecs.ToString() }).Distinct().ToList(),
                "isScf" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Isscf != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Isscf == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.FirstOrDefault()?.Isscf.ToString() }).Distinct().ToList(),
                "isNof" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Isnof != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Isnof == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.FirstOrDefault()?.Isnof.ToString() }).Distinct().ToList(),
                #endregion

                "infrastructureLocation" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Locationinfrastructure != null))
                    .Select(p => new FilterValueDto(p.FirstOrDefault()?.Locationinfrastructure)).Distinct().ToList(),

                "exposedEdgeFlag" => query.ToList().Select(p => new FilterValueDto(GetExposedEdgeValue(p.Lcmancillarydata.FirstOrDefault()?.Isexposededge))).Distinct().ToList(),


                "externalFacingFlag" => lcmAncillaryQuery.ToList().
               Where(x => x.Any(x => x.Externalfacingflag != null))
               .Select(p => new FilterValueDto
               {
                   Text = p.FirstOrDefault()?.Externalfacingflag == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                   Value = p.FirstOrDefault()?.Externalfacingflag.ToString()
               }).Distinct().ToList(),
                "vulnerabilityRating" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Vulnerabilityrating)).Distinct().ToList(),
                "cyberRiskRequestId" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Cyberriskrequestid)).Distinct().ToList(),
                

                #endregion

                #region SPOC And Vertical filetring based on ORG table
                "verticalEngineeringTeam" => string.IsNullOrEmpty(propertyFilter)
                                ? query.SelectMany(x => x.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid)
                                ).SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Verticalresponsible,
                                    Value = p.Verticalresponsibleid.ToString()
                                }).Distinct().ToList()
                                 .Concat(query.Where(x => x.Lcmengineeringsubdomainspoc != null && x.Lcmengineeringsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   )).Distinct().ToList()
                                : query.SelectMany(x => x.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid))
                                .SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Verticalresponsible,
                                    Value = p.Verticalresponsibleid.ToString()
                                }).Where(x => x.Text.Contains(propertyFilter)).Distinct().ToList()
                                 .Concat(query.Where(x => x.Lcmengineeringeduspoc != null && x.Lcmengineeringeduspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   )).Distinct().ToList(),
                "verticalSubDomain" => string.IsNullOrEmpty(propertyFilter)
                                ? query.SelectMany(x => x.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid)
                                )
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsible,
                                    Value = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString()
                                }).Distinct().ToList()
                                 .Concat(query.Where(x => x.Lcmengineeringsubdomainspoc != null && x.Lcmengineeringsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   ))).Distinct().ToList()
                                : query.SelectMany(x => x.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid))
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsible,
                                    Value = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString()
                                }).Where(x => x.Text.Contains(propertyFilter)).Distinct().ToList()
                                 .Concat(query.Where(x => x.Lcmengineeringeduspoc != null && x.Lcmengineeringeduspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   ))).Distinct().ToList(),

                "engineeringContactPoint" => comSpocs.Select(x => new FilterValueDto { Text = x.Text, Value = x.Value }).Distinct().ToList(),

                #endregion
                #region LCM R11 
                "assetStatusFY26" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofsupport != null ? GetAssetStatus(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofsupport.Value, 2026) : string.Empty)).Distinct().ToList(),
                "assetStatusFY28" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofsupport != null ? GetAssetStatus(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofsupport.Value,2028) : string.Empty)).Distinct().ToList(),
                "eoslKpiForecast" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofsupport != null ? GetEoslKpiForeCast(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2027, p.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Plannedcompletion, GetAssetStatus(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2027),
                   p.Softwareendofwarrantydate != null ? p.Softwareendofwarrantydate : p.Softwareendofsupportcontract) : string.Empty)).Distinct().ToList(),
                "eoslKpiFrozen" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofsupport != null ? GetEoslKpiFrozen(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2026, p.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Plannedcompletion, GetAssetStatus(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2026),
                   p.Softwareendofwarrantydate != null ? p.Softwareendofwarrantydate : p.Softwareendofsupportcontract) : string.Empty)).Distinct().ToList(),
                "eoslKpiTarget" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofsupport != null ? GetEoslKpiTarget(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2028, p.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Plannedcompletion, GetAssetStatus(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2028),
                   p.Softwareendofwarrantydate != null ? p.Softwareendofwarrantydate : p.Softwareendofsupportcontract) : string.Empty)).Distinct().ToList(),

                "riskComment" => string.IsNullOrEmpty(propertyFilter) ?
                                query.ToList().Where(x => x.Lcmancillarydata != null)
                                .SelectMany(x => x.Lcmancillarydata)
                                .Select(y => new FilterValueDto(y.Riskcomment))
                                .Distinct().ToList()
                                : query.ToList()
                                .Where(x => x.Lcmancillarydata != null && x.Lcmancillarydata.Any(x => x.Riskcomment != null && x.Riskcomment.Contains(propertyFilter)))
                                .SelectMany(x => x.Lcmancillarydata)
                                .Select(y => new FilterValueDto(y.Riskcomment))
                                .Distinct().ToList(),

                "qId" => string.IsNullOrEmpty(propertyFilter) ?
                        query.ToList().Where(x => x.Lcmancillarydata != null)
                        .SelectMany(x => x.Lcmancillarydata)
                        .Select(y => new FilterValueDto(y.Qid))
                        .Distinct().ToList()
                        : query.ToList()
                        .Where(x => x.Lcmancillarydata != null && x.Lcmancillarydata.Any(x => x.Qid != null && x.Qid.Contains(propertyFilter)))
                        .SelectMany(x => x.Lcmancillarydata)
                        .Select(y => new FilterValueDto(y.Qid))
                        .Distinct().ToList(),

                "physicalLocation" => string.IsNullOrEmpty(propertyFilter) ?
                                    query.ToList().Where(x => x.Networkelementsasplanned != null)
                                    .SelectMany(x => x.Networkelementsasplanned)
                                    .Select(y => new FilterValueDto { Text=y.Locationid.ToString(),Value=y.Location.Location})
                                    .Distinct().ToList()
                                    :
                                    query.ToList()
                                    .Where(x => x.Networkelementsasplanned != null && x.Networkelementsasplanned.Any(x1=>x1.Locationid.ToString().Contains(propertyFilter)))
                                    .SelectMany(x => x.Networkelementsasplanned)
                                    .Select(y => new FilterValueDto { Text = y.Locationid.ToString(), Value = y.Location.Location })
                                    .Distinct().ToList(),

                #endregion

                _ => new List<FilterValueDto>(),

            };
            
                if (!isAdmin && (buildFilterDto.VerticalEngineeringTeam != null && buildFilterDto.VerticalEngineeringTeam.Count > 0) && propertyName == "verticalEngineeringTeam")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalEngineeringTeam.Contains(x.Value.ToString())).ToList();
            }
            return rtn;
        }

        public List<FilterValueDto> GetDisaggregatedFilter(string propertyName, string propertyFilter,
           ReportHardwareQueryDto buildFilterDto)
        {

            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyDisaggregatedFilter(buildFilterDto);
            var query = GetDisaggregatedQuery(predicateResult);

            var lcmAncillaryQuery = query.Where(x => x.Lcmengineering.Lcmancillarydata.Count > 0).
                Select(x => x.Lcmengineering.Lcmancillarydata);

            var eduSpoc = query.SelectMany(x => x.Lcmengineering.Lcmengineeringeduspoc.Select(y => new FilterValueDto { Text = y.Eduspoc.Email, Value = y.Eduspocid.ToString() })).ToList()
                .Concat(query.Where(x => x.Lcmengineering != null && x.Lcmengineering.Lcmengineeringeduspoc.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).ToList();


            var subDomSpoc = query.SelectMany(x => x.Lcmengineering.Lcmengineeringsubdomainspoc.Select(y => new FilterValueDto { Text = y.Subdomainspoc.Email, Value = y.Subdomainspocid.ToString() })).ToList()
                .Concat(query.Where(x => x.Lcmengineering != null && x.Lcmengineering.Lcmengineeringsubdomainspoc.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).ToList();

            var comSpocs = eduSpoc.Concat(subDomSpoc).DistinctBy(x => x.Value).ToList();

            var rtn = propertyName switch
            {
                "wbsCode" => query.Select(p => new FilterValueDto
                     (GetWbsCode(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectname))).Distinct().ToList(),
                "bptID" => query
                .Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Projectstatus != null ?
                 p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Budgettrackingid : string.Empty)).Distinct().ToList(),
                "ppmID" => query
                    .Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Projectstatus != null ?
                     p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectid : string.Empty)).Distinct().ToList(),
                "mainOrganization" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Nse.ToUpper()) },
                "serialNumber" => query.Select(p => new FilterValueDto(p.Hwresourcekey)).Distinct().ToList(),
                "reportId" => query.Select(p => new FilterValueDto(p.Lcmengineering.Resourcekey + "-" + p.Hwresourcekey + "0")).Distinct().ToList(),
                "ipAddress" => query.SelectMany(x => x.Identitiesasis)?.Where(p => p.Interfacetype == ConstantValueFilter.Management && p.Category.Description.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress)
                      .Select(p => new FilterValueDto(p.Value)).Distinct().ToList(),
                "hostname" => query.Select(p => new FilterValueDto
                { Text = p.Elementname, Value = p.Elementname }).Distinct().ToList(),
                "originalLCMSpreadsheetID" => query.Select(p => new FilterValueDto(p.Previoushwresourcekey)).Distinct().ToList(),
                "engKpi2" => query.Select(p => new FilterValueDto(GetEngKpi2(p.Lcmengineering.Lcmstatusenghardware, p.Lcmengineering.Outputtolcmhardware))).ToList().Distinct().ToList(),
                "expLCMstatusatendofFY24" => query.Select(p => new FilterValueDto(
                                        GetExpLCMstatusatendofFY24(p.Lcmengineering.Lcmstatushardware,
                                                                   p.Lcmengineering.Outputtolcmhardware,
                                                                   p.Lcmengineering.Hardwareendofsupportcontract != null ? p.Lcmengineering.Hardwareendofsupportcontract : p.Lcmengineering.Softwareendofwarrantydate,
                                                                   p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                                                                  .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion,
                                                                    p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                                                                  .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus
                                                                   ))).ToList().Distinct().ToList(),
                "lcmSpreadSheetHwId" => query.Select(p => new FilterValueDto
                { Text = p.Lcmengineering.Lcmspreadsheethwid, Value = p.Lcmengineering.Lcmspreadsheethwid }).Distinct().ToList(),
                "localMarket" => query.Select(p => new FilterValueDto(p.Opco.Opco)).Distinct().ToList(),
                "designComponentIndex" => query.Select(p => new FilterValueDto(p.Designcomponentid.ToString())).Distinct().ToList(),
                "assetCategory" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Assetcategory.Assetcategory)).Distinct().ToList(),
                "assetType" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware?.Buildconstruction.Buildconstruction)).Distinct().ToList(),
                "assetClass" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware?.Hardwaresolution)).Distinct().ToList(),
                "assetDescription" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent?.Designcomponentfamily?.Description)).Distinct().ToList(),
                "productImportance" => _repositoryWrapper.ProductImportance.FindAll().Select(x => new FilterValueDto(x.Productimportance)).ToList(),
                "vendor" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                        m.Deleted == false && m.Deletiondate == null).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer)).Distinct().ToList(),
                "hardwareModel" => query.ToList().Select(x => new FilterValueDto(x.Designcomponent.Systemtype.toLcmDbExportHardwareName())).Distinct().ToList(),
                "plannedHardwareModel" => query.Where(p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                      .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                      .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                      .Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.HardwareUpgrade).ToList()
                     .Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                      .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                      .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Designcomponent?.Systemtype.toLcmDbExportHardwareName())).Distinct().ToList(),
                "numberOfNodes" => query.Select(p => new FilterValueDto
                { Text = "1", Value = "1" }).Distinct().ToList(),
                "plannedAction" => new List<FilterValueDto>() { new FilterValueDto() { Text = ConstantValueFilter.OnHardware, Value = "1" }, new FilterValueDto() { Text = ConstantValueFilter.OnSoftware, Value = "2" } },
                "descriptionOfPlannedAction" => query.Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                        .Plannedactivityresource.Plannedactivityresource)).Distinct().ToList(),
                "trackingNumberProjectName" => query.Select(x => new FilterValueDto
                    (x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname)).Distinct().ToList(),
                "notes" => query.Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes)).Distinct().ToList(),
                #region Ticket 465 - LCM export: RAG status field - filter is not working fine

                "ragStatus" => LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(query, null, 1).Distinct().ToList(),
                #endregion

                "operationsContactPoint" => query.SelectMany(x => x.Lcmengineering.Lcmoperationalcontracts)
                                        .Select(p => new FilterValueDto(p.Operationalcontract.Description)).Distinct().ToList(),
                "identifiedAction" => query.Select(x => new FilterValueDto(GetIdentificationActionForLcmExport(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofmaintenance, _repositoryWrapper))).ToList().Distinct().ToList(),
                "softwareRelease" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion)).Distinct().ToList(),
                "cloudVersion" => query.Select(p => new FilterValueDto(ConstantValueFilter.Cloudversion)).Distinct().ToList(),
                "budgetEstimated" => query.Select(s => s.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                                          .FirstOrDefault())?.AsEnumerable()
                                          ?.Select(p => new FilterValueDto(p?.Currency == null ? p?.Budgetvalue?.ToString() : p?.Budgetvalue?.ToString() + p?.Currency))?.Distinct()?.ToList(),
                "bundleBudget" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No) },
                "assetServiceFunctionality" => query.Select(p => new FilterValueDto(string.Join(" | ", p.Designcomponent.Designcomponentfamily.Designaspects.
                                                     FirstOrDefault().Designaspectssupportedsvr.Select(x => x.Service.Description)))).ToList().Distinct().ToList(),
                "platform" => query.Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                   .SingleOrDefault(m => m.Ismain && m.Deleted == false).Majorhardware.Hardwaresolution)).Distinct().ToList(),
                //LCM R8 Phase 1  
                "engRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = GetRiskValue(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description),
                    Value = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid.ToString()
                }).Distinct().ToList(),
                "engRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes)).Distinct().ToList(),
                //LCM R8 Phase 1  
                "opsRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = GetRiskValue(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description),
                    Value = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid.ToString()
                }
                ).Distinct().ToList(),
                "opsRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "overallRiskEvaluationLcm" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "outputToLcmSoftware" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatus).Result)).Distinct().ToList(),
                "operationsMaintenanceContract" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Outputtolcmhardware)).Distinct().ToList(),
                "typeOfNetworkElement" => query.Where(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Lcmpolicy != null)
                  .Select(x => new FilterValueDto { Text = ((LCMPolicy)x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Lcmpolicy).ToString(), Value = x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Lcmpolicy.ToString() }).Distinct().ToList(),
                "lcmStatusOpsHardware" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatusopshardware)).Distinct().ToList(),
                "lcmStatusEngHardware" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatusenghardware)).Distinct().ToList(),
                "lcmStatus" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatushardware)).Distinct().ToList(),
                "bundleId" => query.AsEnumerable().Where(x => GetBundleBudget(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower()))
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Budgettrackingid) == ConstantValueFilter.Yes)
                               .Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower()))
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid.Substring(2))).Distinct().ToList(),
                //LCM R8 Phase 1  
                "riskCluster" => query.Where(x => x.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames != null).Select(p => new FilterValueDto(p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
               .Select(x => x.Riskcluster.Riskclusterid).FirstOrDefault(),
                p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
               .Select(x => x.Riskcluster.Description).FirstOrDefault())).Distinct().ToList(),

                "criticality" => query.Select(p => new FilterValueDto(p.Designcomponent.Subnetworkboundary.Criticality)).Distinct().ToList(),
                "gdprRelevant" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes.ToUpper()), new FilterValueDto(ConstantValueFilter.No.ToUpper()) },
                "hwIsExtendedSupportOfferedByVendor" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes.ToUpper()), new FilterValueDto(ConstantValueFilter.No.ToUpper()) },
                "deliveryPlanAvailable" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes.ToUpper()), new FilterValueDto(ConstantValueFilter.No.ToUpper()) },
                "program" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                             .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                                     .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription)).ToList().Distinct().ToList(),
                "projectOwner" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                       .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectowner)).ToList().Distinct().ToList(),
                "projectStatus" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus)).ToList().Distinct().ToList(),

                #region LCM R9 Part - 1
                //"custom " => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Custom)).Distinct().ToList(),
                //"custom1 " => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Custom1)).Distinct().ToList(),
                //"custom2 " => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Custom2)).Distinct().ToList(),
                //"kpiStatusService" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Kpistatusservice)).Distinct().ToList(),
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
                "includedinSecurityScanning" => query.ToList().Select(p => new FilterValueDto
                {
                    Text = p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Includedinsecurityscanning == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                    Value = p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Includedinsecurityscanning.ToString()
                }).Distinct().ToList(),
                "raId" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Raid)).Distinct().ToList(),
                "requestID" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Requestid)).Distinct().ToList(),
                //"id_New" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Idnew)).Distinct().ToList(),
                //"productImportanceHistory2" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Productimportancehistory2)).Distinct().ToList(),
                //"lcmStatusJune2021" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Lcmstatus)).Distinct().ToList(),
                "lastScanDate" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Lastscandate)).Distinct().ToList(),
                "assetOutofScopeForReportingPurposes" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Assetoutofscope)).Distinct().ToList(),
                "lastUpgradeDate" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Lastupgradedate)).Distinct().ToList(),
                "eomControl" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Eomcontrol)).Distinct().ToList(),
                "engUpdateTracker" =>
               query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.
                FirstOrDefault()?.Engupdatetracker)).Distinct().ToList(),
                //.Append(new FilterValueDto(new FilterValueDto
                //{
                //    Text = _engUpdateTracker,
                //    Value = _engUpdateTracker
                //})).Distinct()


                "opsUpdateTracker" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Opsupdatetracker)).Distinct().ToList(),
                "exNetworks" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Exnetworks)).Distinct().ToList(),
                "occurrenceProbability" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Occurenceprobability)).Distinct().ToList(),
                "incidentClass" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Incidentclass)).Distinct().ToList(),
                "nEWOPSRiskEvaluation" => query.ToList().Select(p => new FilterValueDto(GetNewOpsRiskEvaluationValue(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Incidentclass, p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Occurenceprobability))).Distinct().ToList(),
                "productCode" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Productcode)).Distinct().ToList(),
                "handedOverToOperation" => query.ToList().Select(p => new FilterValueDto
                {
                    Text = p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Handedovertooperation == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                    Value = p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Handedovertooperation.ToString()
                }).Distinct().ToList(),
                "contractRenewalPlan" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Contractrenewalplan)).Distinct().ToList(),
                "dataSource" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Datasource)).Distinct().ToList(),
                "scopeOfSimplification" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Scopeofsimplification)).Distinct().ToList(),
                //"cloudVersion" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Cloudversion)).Distinct().ToList(),
                // "certifiedSWReleaseforNFVIbundle" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Certifiedswrealesefornfvibundle)).Distinct().ToList(),
                "labSWRelease" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Productcode)).Distinct().ToList(),
                "originalHwLcmId" => query.ToList().Select(p => new FilterValueDto
                (p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Originalhwlcmid)).Distinct().ToList(),
                "vulnerabilityRating" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Vulnerabilityrating)).Distinct().ToList(),
                "cyberRiskRequestId" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Cyberriskrequestid)).Distinct().ToList(),
                #endregion
                #region Ticket 551
                #region 719 Regulatory fields changes
                "isPecn" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Ispecn != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Ispecn == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.FirstOrDefault()?.Ispecn.ToString() }).Distinct().ToList(),
                "isPecs" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Ispecs != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Ispecs == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.FirstOrDefault()?.Ispecs.ToString() }).Distinct().ToList(),
                "isScf" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Isscf != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Isscf == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.FirstOrDefault()?.Isscf.ToString() }).Distinct().ToList(),
                "isNof" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Isnof != null)).Select(p => new FilterValueDto { Text = p.FirstOrDefault()?.Isnof == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.FirstOrDefault()?.Isnof.ToString() }).Distinct().ToList(),
                #endregion

                "infrastructureLocation" => lcmAncillaryQuery.ToList().
                    Where(x => x.Any(x => x.Locationinfrastructure != null))
                    .Select(p => new FilterValueDto(p.FirstOrDefault()?.Locationinfrastructure)).Distinct().ToList(),

                "exposedEdgeFlag" => query.ToList().Select(p => new FilterValueDto(GetExposedEdgeValue(p.Lcmengineering.Lcmancillarydata.FirstOrDefault()?.Isexposededge))).Distinct().ToList(),


                "externalFacingFlag" => lcmAncillaryQuery.ToList().
               Where(x => x.Any(x => x.Externalfacingflag != null))
               .Select(p => new FilterValueDto
               {
                   Text = p.FirstOrDefault()?.Externalfacingflag == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                   Value = p.FirstOrDefault()?.Externalfacingflag.ToString()
               }).Distinct().ToList(),


                #endregion
                #region SPOC And Vertical filetring based on ORG table
                "verticalEngineeringTeam" => string.IsNullOrEmpty(propertyFilter)
                                ? query.SelectMany(x => x.Networkelementasplannedsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid)
                                ).SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Verticalresponsible,
                                    Value = p.Verticalresponsibleid.ToString()
                                }).Distinct().ToList()
                                .Concat(query.Where(x => x.Networkelementasplannedsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   )).Distinct().ToList()
                                : query.SelectMany(x => x.Networkelementasplannedsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid))
                                .SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Verticalresponsible,
                                    Value = p.Verticalresponsibleid.ToString()
                                }).Where(x => x.Text.Contains(propertyFilter)).Distinct().ToList()
                                .Concat(query.Where(x => x.Lcmengineering != null && x.Lcmengineering.Lcmengineeringeduspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   )).Distinct().ToList(),
                "verticalSubDomain" => string.IsNullOrEmpty(propertyFilter)
                                ? query.SelectMany(x => x.Networkelementasplannedsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid)
                                )
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsible,
                                    Value = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString()
                                }).Distinct().ToList()
                                 .Concat(query.Where(x => x.Networkelementasplannedsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   ))).Distinct().ToList()
                                : query.SelectMany(x => x.Networkelementasplannedsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid))
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsible,
                                    Value = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString()
                                }).Where(x => x.Text.Contains(propertyFilter)).Distinct().ToList()
                                 .Concat(query.Where(x => x.Networkelementasplannedsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   ))).Distinct().ToList(),

                "engineeringContactPoint" => comSpocs.Select(x => new FilterValueDto { Text = x.Text, Value = x.Value }).Distinct().ToList(),

                #endregion
                _ => new List<FilterValueDto>(),


            };
            return rtn;
        }

        private QueryResultDto<ReportHardwareDtoGrid> FindAggregatedWithCondition(ReportHardwareQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Lcmengineering> predicateResult = ApplyAggregatedFilter(buildFilterDto);


            var result = GetQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetColumnsMapDB(), ConstantValueFilter.Modificationdate);

            IEnumerable<Lcmengineering> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();

            var rtn = new QueryResultDto<ReportHardwareDtoGrid>(new GenerateRenderForGrid<ReportHardwareDtoGrid>(_columnManager))
            {

            };

            if (buildFilterDto.IsHistorical == true)
            {
                var pageSizeGridData = new List<ReportHardwareDtoGrid>();
                var lcmHistoricalInfo = _repositoryWrapper.LcmExportSettingRepository.FindByCondition(x => x.Description == buildFilterDto.LcmExportDescription && x.Ishistorical == true).FirstOrDefault().Lcmhistoricalinfohw;

                if (!(string.IsNullOrEmpty(lcmHistoricalInfo)))
                {
                    var gridData = JsonSerializer.Deserialize<List<ReportHardwareDtoGrid>>(lcmHistoricalInfo);

                    if (!isExport)
                    {
                        pageSizeGridData = gridData.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
                    }
                    else
                    {
                        pageSizeGridData = gridData.ToList();
                    }

                    rtn.TotalItems = gridData.Count;
                    rtn.Items = pageSizeGridData.ToArray();

                    return rtn;
                }
            }
            if (buildFilterDto?.LcmStatus != null && buildFilterDto.LcmStatus.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatus.Contains(x.Lcmstatushardware));
            }
            if (buildFilterDto?.LcmStatusEngHardware != null && buildFilterDto.LcmStatusEngHardware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusEngHardware.Contains(x.Lcmstatusenghardware));
            }
            if (buildFilterDto?.LCMStatusOpsHardware != null && buildFilterDto.LCMStatusOpsHardware.Any())
            {
                query = query.Where(x => buildFilterDto.LCMStatusOpsHardware.Contains(x.Lcmstatusopshardware));
            }
            if (buildFilterDto?.OperationsMaintenanceContractLcm != null && buildFilterDto.OperationsMaintenanceContractLcm.Any())
            {
                query = query.Where(x => buildFilterDto.OperationsMaintenanceContractLcm.Contains(x.Outputtolcmhardware));
            }
            if (buildFilterDto?.HardwareModel != null && buildFilterDto.HardwareModel.Any())
            {
                query = query.Where(x => buildFilterDto.HardwareModel.Contains(x.Designcomponent.Systemtype.toLcmDbExportHardwareName()));
            }
            if (buildFilterDto?.ENGKPI2 != null && buildFilterDto.ENGKPI2.Any())
            {
                query = query.Where(x => buildFilterDto.ENGKPI2.Contains(GetEngKpi2(x.Lcmstatusenghardware, x.Outputtolcmhardware)));
            }

            if (buildFilterDto?.ExpLCMstatusatendofFY24 != null && buildFilterDto.ExpLCMstatusatendofFY24.Any())
            {
                query = query.Where(x => buildFilterDto.ExpLCMstatusatendofFY24.Contains(
                                GetExpLCMstatusatendofFY24(x.Lcmstatushardware,
                                                            x.Outputtolcmhardware,
                                                            x.Hardwareendofsupportcontract != null ? x.Hardwareendofsupportcontract : x.Softwareendofwarrantydate,
                                                            x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower()))
                                                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Plannedcompletion,
                                                            x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower()))
                                                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Projectstatus
                                                            )));
            }

            if (buildFilterDto?.IdentifiedAction != null && buildFilterDto.IdentifiedAction.Any())
            {
                query = query.Where(x => x.Archived != true && buildFilterDto.IdentifiedAction.Contains(GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofmaintenance, _repositoryWrapper)));
            }

            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Budgettrackingid)));
            }

            if (buildFilterDto?.PlannedHardwareModel != null && buildFilterDto.PlannedHardwareModel.Any())
            {
                query = query.Where(x => buildFilterDto.PlannedHardwareModel.Contains(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false &&
                        a.Plannedactivityresource.Lcmhardware && a.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.HardwareUpgrade)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Designcomponent?.Systemtype?.toLcmDbExportHardwareName()));
            }

            if (buildFilterDto?.AssetServiceFunctionality != null && buildFilterDto.AssetServiceFunctionality.Any())
            {

                query = query.Where(x => x?.Designcomponent?.Designcomponentfamily?.Designaspects?.
                                      FirstOrDefault() != null).Where(x => buildFilterDto.AssetServiceFunctionality
                                 .Contains(string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Designaspects?.
                                      FirstOrDefault()?.Designaspectssupportedsvr?.Select(x => x?.Service?.Description)?.Distinct())));
            }
            if (buildFilterDto?.AssetStatus != null && buildFilterDto.AssetStatus.Any())
            {

                query = query.ToList().Where(x => x.Lcmancillarydata.Any())
                    .Where(x => buildFilterDto.AssetStatus.Contains(
                   GetAssetStatusBasedOnOriginalHwAndSw(

                       x.Lcmancillarydata.FirstOrDefault() != null ?
                   x.Lcmancillarydata.FirstOrDefault().Originalhwlcmid : string.Empty

                   )));
            }
            if (buildFilterDto?.ExposedEdgeFlag != null && buildFilterDto.ExposedEdgeFlag.Any())
            {
                query = query.ToList().Where(x => x.Lcmancillarydata.Any())
                    .Where(x => buildFilterDto.ExposedEdgeFlag.Contains(GetExposedEdgeValue(x.Lcmancillarydata.FirstOrDefault().Isexposededge)));
            }
            if (buildFilterDto?.OpsUpdateTracker != null && buildFilterDto.OpsUpdateTracker.Any())
            {
                query = query.ToList().Where(x => x.Lcmancillarydata.Any()).Where(x => buildFilterDto.OpsUpdateTracker.Contains(x.Lcmancillarydata.FirstOrDefault().Opsupdatetracker));
            }
            if (buildFilterDto?.EngUpdateTracker != null && buildFilterDto.EngUpdateTracker.Any())
            {
                if (buildFilterDto.EngUpdateTracker.Contains(_engUpdateTracker))
                {
                    query = query.ToList().Where(x => (x.Lcmancillarydata?.Select(eng => eng.Engupdatetracker).FirstOrDefault() == _engUpdateTracker) 
                                            || string.IsNullOrEmpty(x.Lcmancillarydata?.Select(eng => eng.Engupdatetracker).FirstOrDefault()) );
                }
                else
                {
                    query = query.ToList().Where(x => x.Lcmancillarydata.Any()).Where(x => buildFilterDto.EngUpdateTracker.Contains(x.Lcmancillarydata.FirstOrDefault().Engupdatetracker));
                }
            }
            if (buildFilterDto?.AssetStatusFY26 != null && buildFilterDto.AssetStatusFY26.Any())
            {
                query = query.Where(x => x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport != null && buildFilterDto.AssetStatusFY26.Contains(GetAssetStatus(x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2026)));
            }
            if (buildFilterDto?.AssetStatusFY28 != null && buildFilterDto.AssetStatusFY28.Any())
            {
                query = query.Where(x => x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport != null && buildFilterDto.AssetStatusFY28.Contains(GetAssetStatus(x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2028)));
            }
            if (buildFilterDto?.EoslKpiForecast != null && buildFilterDto.EoslKpiForecast.Any())
            {
                query = query.Where(x => x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport != null && buildFilterDto.EoslKpiForecast.Contains(GetEoslKpiForeCast(x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2027, x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Plannedcompletion, GetAssetStatus(x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2027),
                   x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract)));
            }
            if (buildFilterDto?.EoslKpiFrozen != null && buildFilterDto.EoslKpiFrozen.Any())
            {
                query = query.Where(x => x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport != null && buildFilterDto.EoslKpiFrozen.Contains(GetEoslKpiFrozen(x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2026, x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Plannedcompletion, GetAssetStatus(x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2026),
                   x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract)));
            }
            if (buildFilterDto?.EoslKpiTarget != null && buildFilterDto.EoslKpiTarget.Any())
            {
                query = query.Where(x => x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport != null && buildFilterDto.EoslKpiTarget.Contains(GetEoslKpiTarget(x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2028, x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware)).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Plannedcompletion, GetAssetStatus(x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofsupport, 2028),
                   x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract)));
            }
            #region Ticket 465 - LCM export: RAG status field - filter is not working fine
            if (buildFilterDto?.RagStatus != null && buildFilterDto.RagStatus.Any())
            {
                List<List<string>> lcmIdList = new List<List<string>>();
                foreach (var item in buildFilterDto.RagStatus)
                    lcmIdList.Add(LcmEngineeringExtensionMethod.aggregatedRagStatusFilterRecord(query, null, 1, item).Where(x => x.Value != "0").Select(x => x.Text).ToList());

                if (lcmIdList != null && lcmIdList.Count() > 0)
                    query = query.Where(x => lcmIdList.SelectMany(y => y).Contains(x.Lcmengineeringid.ToString()));

            }
            #endregion
            var totalCount = query.Count();

            if (buildFilterDto.SortBy == ConstantValueFilter.productImportance)
            { }


            if (!isExport)
            {
                query = query.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            else
            {
                query = query.ToList();
            }

            var data = query;
            #region Code Optimization
            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != 0).Select(x => x.Lcmengineeringid)?.Distinct()?.ToList();

            var lcmIdAndLcmOpcoId = query?.ToList()?.Where(x => x.Lcmengineeringid != 0)?.DistinctBy(x => x?.Lcmengineeringid)
                .ToDictionary(x => x.Lcmengineeringid, x => (long)x.Opcoid);

            var allSubDomain = _commonManager.GetCalculatedLcmSubDomainSpocEntityForReport(lcmIdAndLcmOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedLcmEduSpocEntityForReport(lcmIdAndLcmOpcoId).ToList();
            var allOperationalContract = _commonManager.GetCalculateLCMOperationalContractsForReport(lcmengineeringsEntityId);

            #endregion

            var reportLastUpdateDate = _repositoryWrapper.LcmDBExportUpdateHistory.FindAll().FirstOrDefault();
            bool isLcmDBExportUpdated = false;
            if (reportLastUpdateDate != null && reportLastUpdateDate.Lasthwupdatedate.Date == DateTime.Now.Date)
            {
                isLcmDBExportUpdated = true;
            }

            var reports = data.Select(x =>
            {
                var grid = new ReportHardwareDtoGrid();
                var dcSubnetwork = x.Designcomponent?.Subnetworkboundary;

                var systemType = x.Designcomponent?.Systemtype;
                var hardWare = systemType?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware;
                var Dcf = x.Designcomponent?.Designcomponentfamily;
                var plannedActivityHW = x.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Hardware.ToUpper()).Plannedactivityresourceid == null ? null : x.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Hardware.ToUpper());

                if (!isLcmDBExportUpdated)
                {
                    x.Outputtolcmhardware = x.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareOutput).Result;
                    x.Lcmstatushardware = x.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatus).Result;
                    x.Lcmstatusopshardware = x.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatusOps).Result;
                    x.Lcmstatusenghardware = x.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatusEng).Result;
                    _repositoryWrapper.Lcmengineering.Update(x);
                    _repositoryWrapper.Save();
                    _repositoryWrapper.ClearTracker();

                }

                #region daily updated report

                grid.OperationsMaintenanceContractLcm = x.Outputtolcmhardware;
                //grid.LcmStatus = x.Lcmstatushardware;
                grid.LcmStatusOpsHardware = x.Lcmstatusopshardware;
                grid.LcmStatusEngHardware = x.Lcmstatusenghardware;

                #endregion

                grid.ReportId = x.Resourcekey;


                grid.LcmEngineeringId = x.Lcmengineeringid;
                grid.DesignComponentIndex = x.Designcomponentid;
                grid.LocalMarket = x.Opco?.Opco;

                #region code optimize org Table
                grid.VerticalEngineeringTeam = string.Join(",", allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && m.VerticalDic != null && x.Deleted == false)
       .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList());

                grid.VerticalSubDomain = string.Join(",", allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && m.SubdomainresponsiblesDic != null && x.Deleted == false)
            .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList());

                grid.EngineeringContactPoint = _commonManager.GetEngContactPointFromEduAndSubDomainSpoc(allEdu?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && x.Deleted == false).Select(t => t?.ContactEmail).ToList(),
                                allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && x.Deleted == false).Select(t => t?.ContactEmail).ToList());

                grid.OperationsContactPoint = string.Join(" | ",
                    allOperationalContract?.Where(m => m.LcmengineeringId == x.Lcmengineeringid).Select(x => x?.OperationDescription).Distinct());

                #endregion
                grid.AssetCategory = systemType?.Assetcategory?.Assetcategory;

                grid.AssetClass = hardWare?.Hardwaresolution;

                grid.AssetType = GetAssetType(hardWare?.Buildconstruction?.Buildconstruction);

                grid.AssetDescription = Dcf.Description;

                grid.Vendor = hardWare?.Orgeqpmanufacturer
                    ?.Originalequipmentmanufacturer;
                grid.HardwareModel = systemType?.toLcmDbExportHardwareName();
                grid.NumberOfNodesLcm = x.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result; // April 18 #435 Ticket

                grid.EOMStatus = hardWare != null
                           ? (EOMEnum)hardWare.Eomstatus : EOMEnum.NotSpecified;

               grid.VendorEndOfVulnerabilitySecuritySupportDate = hardWare?.Endofsupport;

               grid.VendorEndOfVulnerabilitySecuritySupportDateValueLcm = hardWare?.Endofsupport != null ? hardWare?.Endofsupport.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : grid.EOMStatus == EOMEnum.NotAnnounced ? ConstantValueFilter.NotAnnounced : null;

                grid.TrackingNumberProjectNameLcm = x.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());
                grid.Notes = plannedActivityHW?.Notes;
                grid.SystemTypeId = x.Designcomponent.Systemtypeid;
                grid.DesignComponentId = x.Designcomponentid;
                grid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
                grid.MajorHardwareBuildId = hardWare?.Majorhardwareid;
                grid.BundleBudget = GetBundleBudget(plannedActivityHW?.Budgettrackingid);
                grid.BundleId = GetBundleBudget(plannedActivityHW?.Budgettrackingid) == ConstantValueFilter.Yes ? plannedActivityHW.Budgettrackingid.Substring(2) : null;
                grid.AssetServiceFunctionality = x?.Designcomponent?.Designcomponentfamily?.Designaspects?.FirstOrDefault()?.Designaspectssupportedsvr?.Count() > 0 ? string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Designaspects?.FirstOrDefault()?.Designaspectssupportedsvr.Select(x => x?.Service?.Description).Distinct()) : string.Empty;
                grid.Platform = hardWare?.Hardwaresolution;

                grid.OpsMaintenanceConractEnd = x.Hardwareendofsupportcontract != null ? x.Hardwareendofsupportcontract : x.Softwareendofwarrantydate;
                grid.OpsMaintenanceConractEndValueLcm = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

                grid.OpsRiskEvaluation = GetRiskValue(plannedActivityHW
                    ?.Operationalrisk?.Description);

                grid.OverallRiskEvaluationLcm = plannedActivityHW
                    ?.Overallriskevaluation;

                grid.EngRiskEvaluationNotes = plannedActivityHW
                    ?.Riskengineeringnotes;

                grid.EngRiskEvaluation = GetRiskValue(plannedActivityHW
                  ?.Engineeringrisk?.Description);

                grid.OpsRiskEvaluationNotes = plannedActivityHW
                    ?.Riskoperationalnotes;
                grid.DescriptionOfPlannedAction = plannedActivityHW
                    ?.Plannedactivityresource?.Plannedactivityresource;

                grid.PlannedHardwareModel = plannedActivityHW?.Plannedactivityresource?.Plannedactivityresource?.ToLower().Replace(" ", "") == ConstantValueFilter.HardwareUpgrade
                ? plannedActivityHW?.Designcomponent?.Systemtype?.toLcmDbExportHardwareName() : string.Empty;


                grid.ProjectStatus = plannedActivityHW?.Projectstatus;

                grid.ProjectEndDate = plannedActivityHW?.Plannedcompletion
                    ?.Date;

                grid.ProjectEndDateValue = plannedActivityHW?.Plannedcompletion
                   ?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                grid.PlannedActivityId = plannedActivityHW?.Plannedactivityid;

                grid.GdprRelevant = dcSubnetwork.Gdprrelevant != null ? dcSubnetwork.Gdprrelevant == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper() : null;

                grid.BudgetEstimated = x.PlannedactivitiesLcmengineering?.GetPlannedActivityBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());
                grid.ManagedByGdc = ConstantValueFilter.No;

                grid.DesignComponentFamilyId = x.Designcomponent?.Designcomponentfamilyid;
                grid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";

                grid.Archived = x.Archived;


                #region LCM-R8-Phase1
                grid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper);//systemType?.VodafonenameNavigation?.Description;
                grid.Criticality = dcSubnetwork?.Criticality;

                #region Ticket 465 - LCM export: RAG status field - filter is not working fine
                grid.RagStatus = LcmEngineeringExtensionMethod.aggregatedRagStatusFilterRecord(null,
                  data.Where(y => y.Lcmengineeringid == x.Lcmengineeringid).ToList(), 1
                  ).FirstOrDefault().Text.ToString();
                #endregion
                grid.AssetStatus = GetAssetStatusBasedOnOriginalHwAndSw(
                    x.Lcmancillarydata.FirstOrDefault() != null ?
                    x.Lcmancillarydata.FirstOrDefault().Originalhwlcmid : string.Empty
                    );
                grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngHardware, grid.OperationsMaintenanceContractLcm);
                grid.HwIsExtendedSupportOfferedByVendor = x.Hwisextendedsupportofferedbyvendor.HasValue && x.Hwisextendedsupportofferedbyvendor.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();
                ///grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus, grid.OperationsMaintenanceContractLcm, grid.OpsMaintenanceConractEnd, grid.ProjectEndDate, grid.ProjectStatus);




                var ipIdentities = x.Networkelementsasplanned?.SelectMany(p => p?.Identitiesasis).Where(p => p.Interfacetype == ConstantValueFilter.Management && p?.Category?.Description?.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress);
                grid.IpAddress = ipIdentities != null ? string.Join(" | ", ipIdentities.Select(fx => fx.Value).Distinct()) : string.Empty;

                grid.Hostname = string.Empty;//x.Networkelementsasplanned != null ? string.Join(" | ", x.Networkelementsasplanned.Select(fx => fx.Elementname).Distinct()) : string.Empty;

                grid.DeliveryPlanAvailable = plannedActivityHW?.Deliveryplanavailable != null && plannedActivityHW.Deliveryplanavailable ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();

                grid.WbsCode = GetWbsCode(plannedActivityHW?.Deliveryprojectname);
                grid.BptID = plannedActivityHW?.Budgettrackingid;
                grid.PpmID = plannedActivityHW?.Deliveryprojectid;
                grid.MainOrganization = ConstantValueFilter.Nse.ToUpper();
                grid.SerialNumber = x.Resourcekey;


                #endregion
                #region LCM R9 part-1
                grid.SecurityRiskPotential = x.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames.Where(x => x.Riskcluster != null).Select(x => x.Riskcluster.Risklevel).FirstOrDefault();


                var lcmAuditAttributes = x.Lcmancillarydata?.FirstOrDefault();
                grid.SecurityRiskOverallLcm = GetSecurityRiskOverAllValue(lcmAuditAttributes != null ? lcmAuditAttributes.Securityriskeffective : string.Empty, grid.SecurityRiskPotential);
                if (lcmAuditAttributes != null)
                {
                    grid.ReasonfornoPlan = lcmAuditAttributes.Reasonfornoplan;
                    grid.CommentonProjectStatus = lcmAuditAttributes.Commentonprojectstatus;
                    grid.SecurityRiskEffective = lcmAuditAttributes.Securityriskeffective;
                    grid.SecurityMitigation = lcmAuditAttributes.Securitymitigation;//calculated filesd
                    grid.IncludedinSecurityScanning = lcmAuditAttributes.Includedinsecurityscanning;
                    grid.RaId = lcmAuditAttributes.Raid;
                    grid.RequestIDLcm = lcmAuditAttributes.Requestid;
                    grid.LastScanDate = lcmAuditAttributes.Lastscandate?.Date;
                    grid.LastScanDateValue = lcmAuditAttributes.Lastscandate;//?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    grid.AssetOutofScopeForReportingPurposes = _commonManager.GetAssetOutofScope( lcmAuditAttributes.Assetoutofscope);//calculated filesd


                    grid.LastUpgradeDate = lcmAuditAttributes.Lastupgradedate?.Date;
                    grid.LastUpgradeDateValue = lcmAuditAttributes.Lastupgradedate;//?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    grid.EomControl = lcmAuditAttributes.Eomcontrol;
                    grid.EngUpdateTracker = lcmAuditAttributes.Engupdatetracker;
                    grid.OpsUpdateTracker = lcmAuditAttributes.Opsupdatetracker;
                    grid.ExNetworks = lcmAuditAttributes.Exnetworks;
                    grid.NewopsRiskEvaluation = GetNewOpsRiskEvaluationValue(lcmAuditAttributes.Incidentclass, lcmAuditAttributes.Occurenceprobability);
                    grid.OccurrenceProbability = lcmAuditAttributes.Occurenceprobability;
                    grid.IncidentClass = lcmAuditAttributes.Incidentclass;
                    grid.ProductCode = lcmAuditAttributes.Productcode;
                    grid.ContractRenewalPlan = lcmAuditAttributes.Contractrenewalplan;
                    grid.DataSourceLcm = lcmAuditAttributes.Datasource;
                    grid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;
                    grid.OriginalHwLcmId = lcmAuditAttributes.Originalhwlcmid;
                    grid.CyberRiskRequestId = lcmAuditAttributes.Cyberriskrequestid;
                    grid.VulnerabilityRating = lcmAuditAttributes.Vulnerabilityrating;
                    #region Ticket 551
                    #region //719 Regulatory Fields Changes
                    grid.IsPecn = lcmAuditAttributes.Ispecn;
                    grid.IsPecs = lcmAuditAttributes.Ispecs;
                    grid.IsScf = lcmAuditAttributes.Isscf;
                    grid.IsNof = lcmAuditAttributes.Isnof;
                    #endregion                    
                    grid.ExternalFacingFlag = lcmAuditAttributes.Externalfacingflag != null ?
                    lcmAuditAttributes.Externalfacingflag.Value.ToString() : "";
                    grid.InfrastructureLocation = lcmAuditAttributes.Locationinfrastructure;
                    grid.ExposedEdgeFlag = GetExposedEdgeValue(lcmAuditAttributes.Isexposededge);

                    #endregion
                    grid.CyberRiskRequestId = lcmAuditAttributes.Cyberriskrequestid;
                    grid.VulnerabilityRating = lcmAuditAttributes.Vulnerabilityrating;
                }
                #region Apr 03 requirement
                grid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
? lcmAuditAttributes.Handedovertooperation : true;
                grid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
               ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;
                #endregion
                grid.VulnerabilityScore = string.Empty;
                grid.LcmCumulativeRiskLevel = string.Empty;
                grid.LcmCumulativeRiskId = string.Empty;
                grid.Comments = string.Empty;
                #endregion
                #region LCM R11 Phase-1

                grid.ProductImportance = lcmAuditAttributes?.Assetoutofscope != null ? lcmAuditAttributes?.Assetoutofscope == "Asset in planned dismission, no replacement" ? "Under Dismission" : x.Productimportance?.Productimportance: x.Productimportance?.Productimportance;
                grid.AssetStatusFY26 = grid.VendorEndOfVulnerabilitySecuritySupportDate != null ? GetAssetStatus(grid.VendorEndOfVulnerabilitySecuritySupportDate.Value, 2026) : grid.EOMStatus == EOMEnum.NotAnnounced ? Outputs.onSupport : string.Empty;
                grid.AssetStatusFY27 = grid.VendorEndOfVulnerabilitySecuritySupportDate != null ? GetAssetStatus(grid.VendorEndOfVulnerabilitySecuritySupportDate.Value, 2027) : grid.EOMStatus == EOMEnum.NotAnnounced ? Outputs.onSupport : string.Empty;
                grid.AssetStatusFY28 = grid.VendorEndOfVulnerabilitySecuritySupportDate != null ? GetAssetStatus(grid.VendorEndOfVulnerabilitySecuritySupportDate.Value, 2028) : grid.EOMStatus == EOMEnum.NotAnnounced ? Outputs.onSupport : string.Empty;
                grid.EoslKpiForecast = grid.VendorEndOfVulnerabilitySecuritySupportDate != null ? GetEoslKpiForeCast(grid.VendorEndOfVulnerabilitySecuritySupportDate.Value, 2027, grid.ProjectEndDate, grid.AssetStatusFY27,grid.OpsMaintenanceConractEnd) : grid.EOMStatus == EOMEnum.NotAnnounced ? Outputs.Compliant : string.Empty;
                grid.EoslKpiFrozen = grid.VendorEndOfVulnerabilitySecuritySupportDate != null ? GetEoslKpiFrozen(grid.VendorEndOfVulnerabilitySecuritySupportDate.Value, 2026, grid.ProjectEndDate, grid.AssetStatusFY26, grid.OpsMaintenanceConractEnd) : grid.EOMStatus == EOMEnum.NotAnnounced ? Outputs.Compliant : string.Empty;
                grid.EoslKpiTarget = grid.VendorEndOfVulnerabilitySecuritySupportDate != null ? GetEoslKpiTarget(grid.VendorEndOfVulnerabilitySecuritySupportDate.Value, 2028, grid.ProjectEndDate, grid.AssetStatusFY28, grid.OpsMaintenanceConractEnd) : grid.EOMStatus == EOMEnum.NotAnnounced ? Outputs.Compliant : string.Empty;
                grid.QId = lcmAuditAttributes?.Qid;
                grid.RiskComment = lcmAuditAttributes?.Riskcomment; //need to this column in anicillary table
                grid.PhysicalLocation = x.Networkelementsasplanned.Any()
                                            ? string.Join("; ",
                                                x.Networkelementsasplanned
                                                    .Select(y => y.Location.Location) 
                                                    .Distinct())
                                            : string.Empty;
                grid.ProjectOwner = string.Empty;
                #endregion

                grid.VulnerabilityScore = string.Empty;

                grid.LcmCumulativeRiskLevel = string.Empty;
                grid.LcmCumulativeRiskId = string.Empty;
                grid.Comments = string.Empty;

                grid.IdentifiedAction = x.Archived != true ? GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), hardWare.Endofmaintenance, _repositoryWrapper) : "";

                grid.ProgramLcm = x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.ProgramNavigation?.Programdescription;

                //grid.ProjectOwner = x.PlannedactivitiesLcmengineering.AsQueryable()
                //   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                //   ?.Projectowner;

                return grid;
            }).ToList();

            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lasthwupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
            }

            if (buildFilterDto.ViewMode == ReportViewMode.Aggregated)
            {
                var filtersToHide = new HashSet<string>
                {
                    //ConstantValueFilter.DCF.ToLower(),
                    //ConstantValueFilter.SupportService.ToLower(),
                    ConstantValueFilter.Bagname.ToLower(),
                    ConstantValueFilter.Componentname.ToLower(),
                    ConstantValueFilter.Componentresourcekey.ToLower(),
                    ConstantValueFilter.Components.ToLower(),
                };

                // Update 'Show' property where needed
                foreach (var item in rtn.GridRender.Render)
                {
                    if (filtersToHide.Contains(item.PropertyName.ToLower()))
                    {
                        item.Show = false;
                    }
                }

                // Remove items  
                rtn.GridRender.Render = rtn.GridRender.Render
                    .Where(c => !filtersToHide.Contains(c.PropertyName.ToLower()))
                    .ToList();


            }

            rtn.TotalItems = totalCount;
            rtn.Items = reports.ToArray();

            return rtn;
        }

        private IQueryable<Lcmengineering> GetQuery(ExpressionStarter<Lcmengineering> predicateResult)
        {
            return _repositoryWrapper.Lcmengineering.FindByCondition(predicateResult).Where(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                            .SingleOrDefault(m =>
                                m.Ismain &&
                                m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                                m.Deleted == false).Majorhardware.Buildconstruction.Iscloudasset == false)
                                .Where(x => x.Designcomponent.Systemtype.Deleted == false
                                && x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(s => s.Deleted == false))
                                .Include(x => x.Opco)
                                .Include(x => x.Networkelementsasplanned)
                                .Include(x => x.Productimportance)
                                .Include(x => x.Lcmdeploymentstatus)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                                .Include(x => x.Lcmancillarydata)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation).ThenInclude(x => x.Riskclustervodafonenames).ThenInclude(x => x.Riskcluster)                               
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetclass)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)                               
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designaspects).ThenInclude(x => x.Designaspectssupportedsvr)
                                            .ThenInclude(x => x.Service)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverytrackings)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Activitystatus)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Engineeringrisk)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Operationalrisk)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.ProgramNavigation)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Networkelementsasplanned).ThenInclude(x => x.Identitiesasis).ThenInclude(x => x.Category)
                                .Include(x => x.Networkelementsasplanned).ThenInclude(x => x.Location) 
                                ;
        }

        public ExpressionStarter<Lcmengineering> ApplyAggregatedFilter(ReportHardwareQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);

            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);
            if (buildFilterDto?.BptID != null && buildFilterDto.BptID.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.BptID)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PpmID != null && buildFilterDto.PpmID.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.PpmID)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SerialNumber != null && buildFilterDto.SerialNumber.Any())
            {
                foreach (var item in buildFilterDto?.SerialNumber)
                    predicateInner.Or(x => x.Resourcekey == item);
                predicateResult.And(predicateInner);
            }
            #region Ticket 465 - LCM export: RAG status field - filter is not working fine
            //if (buildFilterDto?.RagStatus != null && buildFilterDto.RagStatus.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Lcmengineering>();
            //    foreach (var item in buildFilterDto?.RagStatus)
            //        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere("HARDWARE"))
            //           .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliverytrackings.FirstOrDefault().Ms2status.ToString() == item);
            //    predicateResult.And(predicateInner);
            //}
            #endregion
            if (buildFilterDto?.ProgramLcm != null && buildFilterDto.ProgramLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.ProgramLcm)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectOwner != null && buildFilterDto.ProjectOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.ProjectOwner)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectowner == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.IpAddress != null && buildFilterDto.IpAddress.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.IpAddress)
                    predicateInner.Or(x => x.Networkelementsasplanned.SelectMany(p => p.Identitiesasis).Any(p => p.Interfacetype == ConstantValueFilter.Management && p.Category.Description.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress && p.Value == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.TypeOfNetworkElement != null && buildFilterDto.TypeOfNetworkElement.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.TypeOfNetworkElement)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Lcmpolicy == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ReportId != null && buildFilterDto.ReportId.Any())
            {
                foreach (var item in buildFilterDto?.ReportId)
                    predicateInner.Or(x => x.Resourcekey == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PreviousReportId != null && buildFilterDto.PreviousReportId.Any())
            {
                foreach (var item in buildFilterDto?.PreviousReportId)
                    predicateInner.Or(x => x.Previousresourcekey == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OriginalLCMSpreadsheetID != null && buildFilterDto.OriginalLCMSpreadsheetID.Any())
            {
                foreach (var item in buildFilterDto?.OriginalLCMSpreadsheetID)
                    predicateInner.Or(x => x.Previousresourcekey == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.DesignComponentIndex != null && buildFilterDto.DesignComponentIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.DesignComponentIndex)
                    predicateInner.Or(x => x.Designcomponentid.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.LocalMarket != null && buildFilterDto.LocalMarket.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.LocalMarket)
                    predicateInner.Or(x => x.Opco.Opco == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.LocalMarketId != null && buildFilterDto.LocalMarketId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.LocalMarketId)
                    predicateInner.Or(x => x.Opco.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            #region Vertical and Spocs Filters

            if (buildFilterDto?.VerticalEngineeringTeam != null && buildFilterDto.VerticalEngineeringTeam.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                int count = 0;

                foreach (var item in buildFilterDto?.VerticalEngineeringTeam)
                    if (item == "yes")
                    {
                        count++;

                        predicateInner.Or(x => x.Lcmengineeringsubdomainspoc != null && !x.Lcmengineeringsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;

                        predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser.Any
                        (m => m.Organisation.Vertical.Verticalresponsibleid.ToString() == item && m.Deleted == false /*&& m.Opcoid == x.Opcoid*/)));
                    }

                if (count > 0) predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.VerticalSubDomain != null && buildFilterDto.VerticalSubDomain.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                int count = 0;

                foreach (var item in buildFilterDto?.VerticalSubDomain)
                    if (item == "yes")
                    {
                        count++;

                        predicateInner.Or(x => x.Lcmengineeringsubdomainspoc != null && !x.Lcmengineeringsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;

                        predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuseropcosUser.Any
                    (m => m.Deleted == false && m.Opcoid == x.Opcoid))
                    && x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.Subdomainresponsibleid.ToString() == item));
                    }
                if (count > 0) predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngineeringContactPoint != null && buildFilterDto.EngineeringContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.EngineeringContactPoint)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => x.Lcmengineeringeduspoc != null && !x.Lcmengineeringeduspoc.Any() && !x.Lcmengineeringsubdomainspoc.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.Id.ToString() == item) ||
                                            x.Lcmengineeringeduspoc.Any(d => d.Eduspoc.Id.ToString() == item));
                    }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OperationsContactPoint != null && buildFilterDto.OperationsContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OperationsContactPoint)
                    predicateInner.Or(x => x.Lcmoperationalcontracts.Any(d => d.Operationalcontract.Description == item));
                predicateResult.And(predicateInner);
            }
            #endregion

            if (buildFilterDto?.AssetCategory != null && buildFilterDto.AssetCategory.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.AssetCategory)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Assetcategory.Assetcategory == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.AssetClass != null && buildFilterDto.AssetClass.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.AssetClass)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware.Hardwaresolution == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.AssetType != null && buildFilterDto.AssetType.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.AssetType)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware.Buildconstruction.Buildconstruction == item);
                predicateResult.And(predicateInner);

            }
            if (buildFilterDto?.AssetDescription != null && buildFilterDto.AssetDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.AssetDescription)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Description == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProductImportance != null && buildFilterDto.ProductImportance.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.ProductImportance)
                {
                    if(item == "Under Dismission")
                        predicateInner.Or(x => x.Lcmancillarydata.Any(m => m.Assetoutofscope == "Asset in planned dismission, no replacement") );
                    else
                                predicateInner.Or(x => x.Productimportance.Productimportance == item);
                }                
                
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Vendor != null && buildFilterDto.Vendor.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Vendor)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m =>
                        m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                        m.Deleted == false && m.Deletiondate == null).Majorhardware.Orgeqpmanufacturer
                    .Originalequipmentmanufacturer == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.NumberOfNodesLcm != null && buildFilterDto.NumberOfNodesLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.NumberOfNodesLcm)
                    predicateInner.Or(x => x.Numberofnodes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PlannedAction != null && buildFilterDto.PlannedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.PlannedAction)
                    switch (item)
                    {
                        case 1:
                            predicateInner.Or(x => x.Onhardware);
                            break;
                        case 2:
                            predicateInner.Or(x => x.Onsoftware);
                            break;
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.DescriptionOfPlannedAction != null && buildFilterDto.DescriptionOfPlannedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.DescriptionOfPlannedAction)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmhardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource == item);
                predicateResult.And(predicateInner);
            }
            //LCM R8 Phase 1  
            if (buildFilterDto?.ProjectStatus != null && buildFilterDto.ProjectStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.ProjectStatus)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.TrackingNumberProjectNameLcm != null && buildFilterDto.TrackingNumberProjectNameLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.TrackingNumberProjectNameLcm)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Notes != null && buildFilterDto.Notes.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Notes)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VendorEndOfMaintenanceDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.VendorEndOfMaintenanceDateValue.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Endofmaintenance >= buildFilterDto.VendorEndOfMaintenanceDateValue.StartDate);
                if (buildFilterDto.VendorEndOfMaintenanceDateValue.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Endofmaintenance <= buildFilterDto.VendorEndOfMaintenanceDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectEndDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto?.ProjectEndDateValue.StartDate != null)
                    predicateInner.And(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion >= buildFilterDto.ProjectEndDateValue.StartDate);
                if (buildFilterDto?.ProjectEndDateValue.EndDate != null)
                    predicateInner.And(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion <= buildFilterDto.ProjectEndDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OpsMaintenanceConractEndValueLcm != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.OpsMaintenanceConractEndValueLcm.StartDate != null)
                    predicateInner.And(x => x.Hardwareendofsupportcontract >= buildFilterDto.OpsMaintenanceConractEndValueLcm.StartDate);

                if (buildFilterDto.OpsMaintenanceConractEndValueLcm.EndDate != null)
                    predicateInner.And(x => x.Hardwareendofsupportcontract <= buildFilterDto.OpsMaintenanceConractEndValueLcm.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.BudgetEstimated != null && buildFilterDto.BudgetEstimated.Any())
            {

                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.BudgetEstimated)
                {
                    var value = decimal.TryParse(item, out _);
                    if (value)
                    {
                        var decimalValue = decimal.Parse(item);
                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                         .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                         .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgetvalue == decimalValue);
                    }
                    else
                    {
                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                            .OrderBy(x => x.Plannedcompletion).Select(s => new { s.Budgetvalue, s.Currency }).AsQueryable().Select(x => x.Budgetvalue.ToString() + x.Currency).FirstOrDefault() == item);
                    }
                }

                predicateResult.And(predicateInner);

            }

            if (buildFilterDto?.BundleId != null && buildFilterDto.BundleId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.BundleId)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid.Substring(2) == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Platform != null && buildFilterDto.Platform.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Platform)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                   .SingleOrDefault(m =>
                       m.Ismain &&
                       m.Deleted == false).Majorhardware.Hardwaresolution == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.EngRiskEvaluation != null && buildFilterDto.EngRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.EngRiskEvaluation)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngRiskEvaluationNotes != null && buildFilterDto.EngRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.EngRiskEvaluationNotes)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluation != null && buildFilterDto.OpsRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluation)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluationNotes != null && buildFilterDto.OpsRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluationNotes)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OverallRiskEvaluation != null && buildFilterDto.OverallRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OverallRiskEvaluation)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DeliveryPlanAvailable != null && buildFilterDto.DeliveryPlanAvailable.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.DeliveryPlanAvailable)
                {
                    if (item == "YES")
                    {
                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                       .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == false);
                    }
                }
                predicateResult.And(predicateInner);
            }
            #region LCM R8 Phase 1  
            if (buildFilterDto.GdprRelevant != null && buildFilterDto.GdprRelevant.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.GdprRelevant)
                {
                    if (item.ToUpper() == ConstantValueFilter.Yes.ToUpper())
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

            if (buildFilterDto?.OutputToLcmHardware != null && buildFilterDto.OutputToLcmHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OutputToLcmHardware)
                    predicateInner.Or(x => x.Lcmstatushardware == item);
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
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Criticality)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Criticality == item);
                predicateResult.And(predicateInner);
            }

            #endregion
            if (buildFilterDto.HwIsExtendedSupportOfferedByVendor != null && buildFilterDto.HwIsExtendedSupportOfferedByVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.HwIsExtendedSupportOfferedByVendor)
                {
                    if (item.ToLower() == ConstantValueFilter.Yes.ToLower())
                    {
                        predicateInner.Or(x => x.Hwisextendedsupportofferedbyvendor == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Hwisextendedsupportofferedbyvendor == false || !x.Hwisextendedsupportofferedbyvendor.HasValue);
                    }
                }
                predicateResult.And(predicateInner);
            }

            #region LCM R9 Part - 1
            if (buildFilterDto?.VulnerabilityRating != null && buildFilterDto.VulnerabilityRating.Any())
            {
                foreach (var item in buildFilterDto?.VulnerabilityRating)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Vulnerabilityrating == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.CyberRiskRequestId != null && buildFilterDto.CyberRiskRequestId.Any())
            {
                foreach (var item in buildFilterDto?.CyberRiskRequestId)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Cyberriskrequestid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ReasonfornoPlan != null && buildFilterDto.ReasonfornoPlan.Any())
            {
                foreach (var item in buildFilterDto?.ReasonfornoPlan)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Reasonfornoplan == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.CommentonProjectStatus != null && buildFilterDto.CommentonProjectStatus.Any())
            {
                foreach (var item in buildFilterDto?.CommentonProjectStatus)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Commentonprojectstatus == item);
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
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Securityriskeffective == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SecurityRiskOverallLcm != null && buildFilterDto.SecurityRiskOverallLcm.Any())
            {
                var entitieswithdata = _repositoryWrapper.Lcmengineering.FindAll().Include(x => x.Lcmancillarydata).Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                    .ThenInclude(x => x.VodafonenameNavigation).ThenInclude(x => x.Riskclustervodafonenames).ThenInclude(x => x.Riskcluster).ToList();
                var filteredEntities = entitieswithdata.Where(entity =>
                {
                    foreach (var item in buildFilterDto.SecurityRiskOverallLcm)
                    {
                        if (entity.Lcmancillarydata.Any(y =>
                 GetSecurityRiskOverAllValue(y.Securityriskeffective, entity.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames != null ?
                 entity.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames.Select(x => x.Riskcluster.Risklevel).FirstOrDefault()
                 : string.Empty) == item))
                        {
                            return true;
                        }
                    }
                    return false;
                });
                var filteredEntityIds = filteredEntities.Select(entity => entity.Lcmengineeringid).ToList();
                predicateResult.And(x => filteredEntityIds.Contains(x.Lcmengineeringid));
            }
            if (buildFilterDto?.SecurityMitigation != null && buildFilterDto.SecurityMitigation.Any())
            {
                foreach (var item in buildFilterDto?.SecurityMitigation)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Securitymitigation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IncludedinSecurityScanning != null && buildFilterDto.IncludedinSecurityScanning.Any())
            {
                foreach (var item in buildFilterDto?.IncludedinSecurityScanning)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Includedinsecurityscanning == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.RaId != null && buildFilterDto.RaId.Any())
            {
                foreach (var item in buildFilterDto?.RaId)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Raid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.RequestIDLcm != null && buildFilterDto.RequestIDLcm.Any())
            {
                foreach (var item in buildFilterDto?.RequestIDLcm)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Requestid == item);
                predicateResult.And(predicateInner);
            }
            //if (buildFilterDto?.Id_New != null && buildFilterDto.Id_New.Any())
            //{
            //    foreach (var item in buildFilterDto?.Id_New)
            //        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Idnew == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.ProductImportanceHistory2 != null && buildFilterDto.ProductImportanceHistory2.Any())
            //{
            //    foreach (var item in buildFilterDto?.ProductImportanceHistory2)
            //        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Productimportancehistory2 == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.LcmStatusJune2021 != null && buildFilterDto.LcmStatusJune2021.Any())
            //{
            //    foreach (var item in buildFilterDto?.LcmStatusJune2021)
            //        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Lcmstatus == item);
            //    predicateResult.And(predicateInner);
            //}
            if (buildFilterDto.LastScanDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.LastScanDateValue.StartDate != null)
                    predicateInner.And(x => x.Lcmancillarydata.FirstOrDefault().Lastscandate >= buildFilterDto.LastScanDateValue.StartDate);
                if (buildFilterDto.LastScanDateValue.EndDate != null)
                    predicateInner.And(x => x.Lcmancillarydata.FirstOrDefault().Lastscandate <= buildFilterDto.LastScanDateValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.AssetOutofScopeForReportingPurposes != null && buildFilterDto.AssetOutofScopeForReportingPurposes.Any())
            {
                foreach (var item in buildFilterDto?.AssetOutofScopeForReportingPurposes)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Assetoutofscope == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastUpgradeDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.LastUpgradeDateValue.StartDate != null)
                    predicateInner.And(x => x.Lcmancillarydata.FirstOrDefault().Lastupgradedate >= buildFilterDto.LastUpgradeDateValue.StartDate);
                if (buildFilterDto.LastUpgradeDateValue.EndDate != null)
                    predicateInner.And(x => x.Lcmancillarydata.FirstOrDefault().Lastupgradedate <= buildFilterDto.LastUpgradeDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.EomControl != null && buildFilterDto.EomControl.Any())
            {
                foreach (var item in buildFilterDto?.EomControl)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Eomcontrol == item);
                predicateResult.And(predicateInner);
            }
            //if (buildFilterDto?.EngUpdateTracker != null && buildFilterDto.EngUpdateTracker.Any())
            //{
            //    foreach (var item in buildFilterDto?.EngUpdateTracker)
            //    {
            //        if (item.ToLower() == _engUpdateTracker.ToLower())
            //        {
            //            predicateInner.Or(x => !x.Lcmancillarydata.Any());
            //            predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Engupdatetracker == item);
            //        }

            //        else
            //            predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Engupdatetracker == item);
            //    }

            //    predicateResult.And(predicateInner);
            //}
            if (buildFilterDto?.OpsUpdateTracker != null && buildFilterDto.OpsUpdateTracker.Any())
            {
                foreach (var item in buildFilterDto?.OpsUpdateTracker)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Opsupdatetracker == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EXNetworks != null && buildFilterDto.EXNetworks.Any())
            {
                foreach (var item in buildFilterDto?.EXNetworks)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Exnetworks == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.NEWOPSRiskEvaluation != null && buildFilterDto.NEWOPSRiskEvaluation.Any())
            {
                var entitieswithdata = _repositoryWrapper.Lcmengineering.FindAll().Include(x => x.Lcmancillarydata).ToList();
                var filteredEntities = entitieswithdata.Where(entity =>
                {
                    foreach (var item in buildFilterDto.NEWOPSRiskEvaluation)
                    {
                        if (entity.Lcmancillarydata.Any(y =>
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
            if (buildFilterDto?.OccurrenceProbability != null && buildFilterDto.OccurrenceProbability.Any())
            {
                foreach (var item in buildFilterDto?.OccurrenceProbability)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Occurenceprobability == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IncidentClass != null && buildFilterDto.IncidentClass.Any())
            {
                foreach (var item in buildFilterDto?.IncidentClass)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Incidentclass == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ProductCode != null && buildFilterDto.ProductCode.Any())
            {
                foreach (var item in buildFilterDto?.ProductCode)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Productcode == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.HandedOverToOperation != null && buildFilterDto.HandedOverToOperation.Any())
            {

                foreach (var item in buildFilterDto?.HandedOverToOperation)
                {
                    if (item == true)
                    {
                        predicateInner.Or(x => !x.Lcmancillarydata.Any());
                        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Handedovertooperation
                    == item);
                    }
                    else
                        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Handedovertooperation
                     == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ContractRenewalPlan != null && buildFilterDto.ContractRenewalPlan.Any())
            {
                foreach (var item in buildFilterDto?.ContractRenewalPlan)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Contractrenewalplan == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.DataSourceLcm != null && buildFilterDto.DataSourceLcm.Any())
            {
                foreach (var item in buildFilterDto?.DataSourceLcm)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Datasource == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ScopeOfSimplification != null && buildFilterDto.ScopeOfSimplification.Any())
            {
                foreach (var item in buildFilterDto?.ScopeOfSimplification)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Scopeofsimplification == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OriginalHwLcmId != null && buildFilterDto.OriginalHwLcmId.Any())
            {
                foreach (var item in buildFilterDto?.OriginalHwLcmId)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Originalhwlcmid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.VulnerabilityRating != null && buildFilterDto.VulnerabilityRating.Any())
            {
                foreach (var item in buildFilterDto?.VulnerabilityRating)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Vulnerabilityrating == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.CyberRiskRequestId != null && buildFilterDto.CyberRiskRequestId.Any())
            {
                foreach (var item in buildFilterDto?.CyberRiskRequestId)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Cyberriskrequestid == item);
                predicateResult.And(predicateInner);
            }
            #endregion
            #region Ticket 551
            #region // 719 Regualtory fields changes
            if (buildFilterDto?.IsPecn != null && buildFilterDto.IsPecn.Any())
            {
                foreach (var item in buildFilterDto?.IsPecn)
                    predicateInner.Or(x => x.Lcmancillarydata.Any(y => y.Ispecn == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IsPecs != null && buildFilterDto.IsPecs.Any())
            {
                foreach (var item in buildFilterDto?.IsPecs)
                    predicateInner.Or(x => x.Lcmancillarydata.Any(y => y.Ispecs == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IsNof != null && buildFilterDto.IsNof.Any())
            {
                foreach (var item in buildFilterDto?.IsNof)
                    predicateInner.Or(x => x.Lcmancillarydata.Any(y => y.Isnof == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.IsScf != null && buildFilterDto.IsScf.Any())
            {
                foreach (var item in buildFilterDto?.IsScf)
                    predicateInner.Or(x => x.Lcmancillarydata.Any(y => y.Isscf == item));
                predicateResult.And(predicateInner);
            }
            #endregion
            if (buildFilterDto?.ExternalFacingFlag != null && buildFilterDto.ExternalFacingFlag.Any())
            {
                foreach (var item in buildFilterDto?.ExternalFacingFlag)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Externalfacingflag == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.InfrastructureLocation != null && buildFilterDto.InfrastructureLocation.Any())
            {
                foreach (var item in buildFilterDto?.InfrastructureLocation)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Locationinfrastructure == item);
                predicateResult.And(predicateInner);
            }

            #endregion

            #region LCM R11
            //if (buildFilterDto?.AssetStatusFY26 != null && buildFilterDto.AssetStatusFY26.Any())
            //{
            //    foreach (var item in buildFilterDto?.AssetStatusFY26)
            //        predicateInner.Or(x =>  GetAssetStatus(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
            //               .SingleOrDefault(m =>
            //                   m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
            //                   m.Deleted == false).Majorhardware.Endofsupport.Value, 2026) == item);
            //    predicateResult.And(predicateInner);
            //}


            if (buildFilterDto?.RiskComment != null && buildFilterDto.RiskComment.Any())
            {
                foreach (var item in buildFilterDto?.RiskComment)
                    predicateInner.Or(x => x.Lcmancillarydata.Any(y => y.Riskcomment == item));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.QID != null && buildFilterDto.QID.Any())
            {
                foreach (var item in buildFilterDto?.QID)
                    predicateInner.Or(x => x.Lcmancillarydata.Any(y => y.Qid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PhysicalLocation != null && buildFilterDto.PhysicalLocation.Any())
            {
                foreach (var item in buildFilterDto?.PhysicalLocation)
                    predicateInner.Or(x => x.Networkelementsasplanned.Any(y => y.Location.Locationid == Convert.ToInt32(item)));
                predicateResult.And(predicateInner);
            }

            #endregion

            return predicateResult;
        }

        private QueryResultDto<ReportHardwareDtoGrid> FindDisaggregatedWithCondition(ReportHardwareQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyDisaggregatedFilter(buildFilterDto);


            var result = GetDisaggregatedQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetDisaggregatedColumnsMapDB());
            IEnumerable<Networkelementsasplanned> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();

            if (buildFilterDto?.LcmStatus != null && buildFilterDto.LcmStatus.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatus.Contains(x.Lcmengineering.Lcmstatushardware));
            }
            if (buildFilterDto?.LcmStatusEngHardware != null && buildFilterDto.LcmStatusEngHardware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusEngHardware.Contains(x.Lcmengineering.Lcmstatusenghardware));
            }
            if (buildFilterDto?.LCMStatusOpsHardware != null && buildFilterDto.LCMStatusOpsHardware.Any())
            {
                query = query.Where(x => buildFilterDto.LCMStatusOpsHardware.Contains(x.Lcmengineering.Lcmstatusopshardware));
            }
            if (buildFilterDto?.OperationsMaintenanceContractLcm != null && buildFilterDto.OperationsMaintenanceContractLcm.Any())
            {
                query = query.Where(x => buildFilterDto.OperationsMaintenanceContractLcm.Contains(x.Lcmengineering.Outputtolcmhardware));
            }
            if (buildFilterDto?.HardwareModel != null && buildFilterDto.HardwareModel.Any())
            {
                query = query.Where(x => buildFilterDto.HardwareModel.Contains(x.Designcomponent.Systemtype.toLcmDbExportHardwareName()));
            }
            if (buildFilterDto?.ENGKPI2 != null && buildFilterDto.ENGKPI2.Any())
            {
                query = query.Where(x => buildFilterDto.ENGKPI2.Contains(GetEngKpi2(x.Lcmengineering.Lcmstatusenghardware, x.Lcmengineering.Outputtolcmhardware)));
            }
            if (buildFilterDto?.ExpLCMstatusatendofFY24 != null && buildFilterDto.ExpLCMstatusatendofFY24.Any())
            {
                query = query.Where(x => buildFilterDto.ExpLCMstatusatendofFY24
                .Contains(GetExpLCMstatusatendofFY24(x.Lcmengineering.Lcmstatushardware,
                                                    x.Lcmengineering.Outputtolcmhardware,
                                                    x.Lcmengineering.Hardwareendofsupportcontract != null ? x.Lcmengineering.Hardwareendofsupportcontract : x.Lcmengineering.Softwareendofwarrantydate,
                                                    x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower()))
                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Plannedcompletion,
                                                    x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower()))
                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Projectstatus
                                                    )));
            }


            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Budgettrackingid)));
            }

            if (buildFilterDto?.AssetServiceFunctionality != null && buildFilterDto.AssetServiceFunctionality.Any())
            {

                query = query.Where(x => x?.Designcomponent?.Designcomponentfamily?.Designaspects?.
                                      FirstOrDefault() != null).Where(x => buildFilterDto.AssetServiceFunctionality
                                 .Contains(string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Designaspects?.
                                      FirstOrDefault()?.Designaspectssupportedsvr?.Select(x => x?.Service?.Description)?.Distinct())));
            }
            if (buildFilterDto?.IdentifiedAction != null && buildFilterDto.IdentifiedAction.Any())
            {
                query = query.Where(x => x.Lcmengineering.Archived != true && buildFilterDto.IdentifiedAction.Contains(GetIdentificationActionForLcmExport(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofmaintenance, _repositoryWrapper)));
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
                    lcmIdList.Add(LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(query, null, 1, item).Where(x => x.Value != "0").Select(x => x.Text).ToList());

                if (lcmIdList != null && lcmIdList.Count() > 0)
                    query = query.Where(x => lcmIdList.SelectMany(y => y).Contains(x.Lcmengineeringid.ToString()));

            }
            #endregion

            var totalCount = query.Count();

            if (buildFilterDto.SortBy == ConstantValueFilter.productImportance)
            { }

            query = query.DistinctBy(x => x.Hwresourcekey);

            if (!isExport)
            {
                query = query.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            else
            {
                query = query.ToList();
            }

            var data = query;

            var allOperationalContracts = _repositoryWrapper.LCMOperationalContracts.FindAll().AsNoTracking().AsSplitQuery().Include(x => x.Operationalcontract).ToList();

            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != null).Select(x => (long)x.Lcmengineeringid)?.Distinct()?.ToList();

            var AssetIdAndOpcoId = query?.ToList()?.Where(x => x.Networkelementasplannedid != 0)?.DistinctBy(x => x?.Networkelementasplannedid)
                .ToDictionary(x => x.Networkelementasplannedid, x => (long)x.Opcoid);

            var allSubDomain = _commonManager.GetCalculatedAssetSubDomainSpocEntityforReport(AssetIdAndOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedAssetEduSpocEntityforReport(AssetIdAndOpcoId).ToList();

            var allOperationalContract = _commonManager.GetCalculateLCMOperationalContractsForReport(lcmengineeringsEntityId);


            var reportLastUpdateDate = _repositoryWrapper.LcmDBExportUpdateHistory.FindAll().FirstOrDefault();
            bool isLcmDBExportUpdated = false;
            if (reportLastUpdateDate != null && reportLastUpdateDate.Lasthwupdatedate.Date == DateTime.Now.Date)
            {
                isLcmDBExportUpdated = true;
            }

            var reports = data.Select(x =>
            {
                var grid = new ReportHardwareDtoGrid();
                var dcSubnetwork = x.Designcomponent?.Subnetworkboundary;

                var systemType = x.Designcomponent?.Systemtype;
                var hardWare = systemType?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware;
                var Dcf = x.Designcomponent?.Designcomponentfamily;
                var plannedActivityHW = x.Lcmengineering.PlannedactivitiesLcmengineering.GetPlannedActivityFilteredDB(ConstantValueFilter.Hardware.ToUpper()).Plannedactivityresourceid == null ? null : x.Lcmengineering.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Hardware.ToUpper());

                if (!isLcmDBExportUpdated)
                {
                    x.Lcmengineering.Outputtolcmhardware = x.Lcmengineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareOutput).Result;
                    x.Lcmengineering.Lcmstatushardware = x.Lcmengineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatus).Result;
                    x.Lcmengineering.Lcmstatusopshardware = x.Lcmengineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatusOps).Result;
                    x.Lcmengineering.Lcmstatusenghardware = x.Lcmengineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatusEng).Result;
                    _repositoryWrapper.Lcmengineering.Update(x.Lcmengineering);
                    _repositoryWrapper.Save();
                    _repositoryWrapper.ClearTracker();

                }

                #region daily updated report

                grid.OperationsMaintenanceContractLcm = x.Lcmengineering.Outputtolcmhardware;
                ///grid.LcmStatus = x.Lcmengineering.Lcmstatushardware;
                grid.LcmStatusOpsHardware = x.Lcmengineering.Lcmstatusopshardware;
                grid.LcmStatusEngHardware = x.Lcmengineering.Lcmstatusenghardware;

                #endregion

                grid.ReportId = $"{x.Lcmengineering.Resourcekey}-{x.Hwresourcekey}-0";
                //grid.OriginalLCMSpreadsheetID = x.Previoushwresourcekey;

                grid.LcmEngineeringId = x.Lcmengineering.Lcmengineeringid;
                grid.DesignComponentIndex = x.Designcomponentid;
                grid.LocalMarket = x.Opco?.Opco;

                #region code optimize org Table

                grid.VerticalEngineeringTeam =
                 string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.VerticalDic != null && x.Deleted == false)
                 .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList());

                grid.VerticalSubDomain = string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.SubdomainresponsiblesDic != null && x.Deleted == false)
            .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList());

                grid.EngineeringContactPoint = _commonManager.GetEngContactPointFromEduAndSubDomainSpoc(allEdu?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && x.Deleted == false).Select(t => t?.ContactEmail).ToList(),
                                allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && x.Deleted == false).Select(t => t?.ContactEmail).ToList());

                grid.OperationsContactPoint = string.Join(" | ",
                    allOperationalContract?.Where(m => m.LcmengineeringId == x.Lcmengineeringid).Select(x => x?.OperationDescription).Distinct());

                #endregion

                grid.AssetCategory = systemType?.Assetcategory?.Assetcategory;

                grid.AssetClass = hardWare?.Hardwaresolution;

                grid.AssetType = GetAssetType(hardWare?.Buildconstruction?.Buildconstruction);

                grid.AssetDescription = Dcf.Description;

                grid.ProductImportance = x.Lcmengineering.Productimportance?.Productimportance;
                grid.Vendor = hardWare?.Orgeqpmanufacturer
                    ?.Originalequipmentmanufacturer;
                grid.HardwareModel = systemType?.toLcmDbExportHardwareName();
                grid.NumberOfNodesLcm = 1;

                grid.EOMStatus = hardWare != null
                           ? (EOMEnum)hardWare.Eomstatus : EOMEnum.NotSpecified;

              // grid.VendorEndOfMaintenanceDate = hardWare?.Endofmaintenance;

                #region Ticket 465 - LCM export: RAG status field - filter is not working fine
                grid.RagStatus = LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(null,
                  data.Where(y => y.Lcmengineering.Lcmengineeringid == x.Lcmengineeringid).ToList(), 1
                  ).FirstOrDefault().Text.ToString();
                #endregion

                //grid.VendorEndOfMaintenanceDateValue = hardWare?.Endofmaintenance != null ? hardWare?.Endofmaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : grid.EOMStatus == EOMEnum.NotAnnounced ? ConstantValueFilter.NotAnnounced : null;

                grid.TrackingNumberProjectNameLcm = x.Lcmengineering.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());
                grid.Notes = plannedActivityHW?.Notes;
                grid.SystemTypeId = x.Designcomponent.Systemtypeid;
                grid.DesignComponentId = x.Designcomponentid;
                grid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
                grid.MajorHardwareBuildId = hardWare?.Majorhardwareid;
                grid.BundleBudget = GetBundleBudget(plannedActivityHW?.Budgettrackingid);
                grid.BundleId = GetBundleBudget(plannedActivityHW?.Budgettrackingid) == ConstantValueFilter.Yes ? plannedActivityHW.Budgettrackingid.Substring(2) : null;
                grid.AssetServiceFunctionality = x?.Designcomponent?.Designcomponentfamily?.Designaspects?.FirstOrDefault()?.Designaspectssupportedsvr?.Count() > 0 ? string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Designaspects?.FirstOrDefault()?.Designaspectssupportedsvr.Select(x => x?.Service?.Description).Distinct()) : string.Empty;

                grid.Platform = hardWare?.Hardwaresolution;

                grid.OpsMaintenanceConractEnd = x.Lcmengineering.Hardwareendofsupportcontract != null ? x.Lcmengineering.Hardwareendofsupportcontract : x.Lcmengineering.Softwareendofwarrantydate;
                grid.OpsMaintenanceConractEndValueLcm = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

                grid.OpsRiskEvaluation = GetRiskValue(plannedActivityHW
                    ?.Operationalrisk?.Description);

                grid.OverallRiskEvaluationLcm = plannedActivityHW
                    ?.Overallriskevaluation;

                grid.EngRiskEvaluationNotes = plannedActivityHW
                    ?.Riskengineeringnotes;

                grid.EngRiskEvaluation = GetRiskValue(plannedActivityHW
                  ?.Engineeringrisk?.Description);

                grid.OpsRiskEvaluationNotes = plannedActivityHW
                    ?.Riskoperationalnotes;
                grid.DescriptionOfPlannedAction = plannedActivityHW
                    ?.Plannedactivityresource?.Plannedactivityresource;

                grid.PlannedHardwareModel = plannedActivityHW?.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.HardwareUpgrade
                ? plannedActivityHW?.Designcomponent?.Systemtype.toLcmDbExportHardwareName() : string.Empty;

                grid.ProjectStatus = plannedActivityHW?.Projectstatus;

                grid.ProjectEndDate = plannedActivityHW?.Plannedcompletion
                    ?.Date;

                grid.ProjectEndDateValue = plannedActivityHW?.Plannedcompletion
                   ?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                grid.PlannedActivityId = plannedActivityHW?.Plannedactivityid;



                grid.GdprRelevant = dcSubnetwork.Gdprrelevant != null ? dcSubnetwork.Gdprrelevant == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper() : null;

                grid.BudgetEstimated = x.Lcmengineering.PlannedactivitiesLcmengineering?.GetPlannedActivityBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());

                // grid.AssetOutofScopeForReportingPurposes = x.Lcmengineering.GetAssetOutOfScop(_repositoryWrapper, grid.ProjectStatus, grid.NumberOfNodesLcm, plannedActivityHW).Result;

                grid.ManagedByGdc = ConstantValueFilter.No;

                grid.DesignComponentFamilyId = x.Designcomponent?.Designcomponentfamilyid;
                grid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";

                grid.Archived = x.Lcmengineering.Archived;

                #region LCM-R8-Phase1

                grid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper);//systemType?.VodafonenameNavigation?.Description;
                grid.Criticality = dcSubnetwork?.Criticality;



                grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngHardware, grid.OperationsMaintenanceContractLcm);
                grid.HwIsExtendedSupportOfferedByVendor = x.Lcmengineering.Hwisextendedsupportofferedbyvendor.HasValue && x.Lcmengineering.Hwisextendedsupportofferedbyvendor.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();
                //grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus,
                //                                                          grid.OperationsMaintenanceContractLcm,
                //                                                          grid.OpsMaintenanceConractEnd,
                //                                                          grid.ProjectEndDate,
                //                                                          grid.ProjectStatus);

                var ipIdentities = x.Identitiesasis?.Where(p => p.Interfacetype == ConstantValueFilter.Management && p?.Category?.Description?.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress);
                grid.IpAddress = ipIdentities != null ? string.Join(" | ", ipIdentities?.Select(fx => fx.Value).Distinct()) : string.Empty;

                grid.Hostname = Convert.ToString(x.Elementname).Trim();

                grid.DeliveryPlanAvailable = plannedActivityHW?.Deliveryplanavailable != null && plannedActivityHW.Deliveryplanavailable ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();

                grid.WbsCode = GetWbsCode(plannedActivityHW?.Deliveryprojectname);
                grid.BptID = plannedActivityHW?.Budgettrackingid;
                grid.PpmID = plannedActivityHW?.Deliveryprojectid;
                grid.MainOrganization = ConstantValueFilter.Nse.ToUpper();
                grid.SerialNumber = x.Hwresourcekey;


                #endregion

                #region LCM R9 part - 1

                var lcmAuditAttributes = x.Lcmengineering.Lcmancillarydata.FirstOrDefault();
                grid.SecurityRiskPotential = x.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames.Where(x => x.Riskcluster != null).Select(x => x.Riskcluster?.Risklevel).FirstOrDefault();
                grid.SecurityRiskOverallLcm = GetSecurityRiskOverAllValue(lcmAuditAttributes != null ? lcmAuditAttributes.Securityriskeffective : string.Empty, grid.SecurityRiskPotential);
                if (lcmAuditAttributes != null)
                {
                    //grid.Custom = lcmAuditAttributes.Custom;
                    //grid.Custom1 = lcmAuditAttributes.Custom1;
                    //grid.Custom2 = lcmAuditAttributes.Custom2;
                    //grid.KpiStatusService = lcmAuditAttributes.Kpistatusservice;
                    grid.ReasonfornoPlan = lcmAuditAttributes.Reasonfornoplan;
                    grid.CommentonProjectStatus = lcmAuditAttributes.Commentonprojectstatus;
                    grid.SecurityRiskEffective = lcmAuditAttributes.Securityriskeffective;
                    grid.SecurityMitigation = lcmAuditAttributes.Securitymitigation;//calculated filesd

                    grid.IncludedinSecurityScanning = lcmAuditAttributes.Includedinsecurityscanning;
                    grid.RaId = lcmAuditAttributes.Raid;
                    grid.RequestIDLcm = lcmAuditAttributes.Requestid;
                    //grid.Id_New = lcmAuditAttributes.Idnew;
                    //grid.ProductImportanceHistory2 = lcmAuditAttributes.Productimportancehistory2;
                    //grid.LcmStatusJune2021 = lcmAuditAttributes.Lcmstatus;
                    grid.LastScanDate = lcmAuditAttributes.Lastscandate?.Date;
                    grid.LastScanDateValue = lcmAuditAttributes.Lastscandate;//?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    grid.AssetOutofScopeForReportingPurposes =  lcmAuditAttributes.Assetoutofscope ;
                    grid.LastUpgradeDate = lcmAuditAttributes.Lastupgradedate?.Date;
                    grid.LastUpgradeDateValue = lcmAuditAttributes.Lastupgradedate;//?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    grid.EomControl = lcmAuditAttributes.Eomcontrol;
                    grid.EngUpdateTracker = lcmAuditAttributes.Engupdatetracker;
                    grid.OpsUpdateTracker = lcmAuditAttributes.Opsupdatetracker;
                    grid.ExNetworks = lcmAuditAttributes.Exnetworks;
                    grid.NewopsRiskEvaluation = GetNewOpsRiskEvaluationValue(lcmAuditAttributes.Incidentclass, lcmAuditAttributes.Occurenceprobability);
                    grid.OccurrenceProbability = lcmAuditAttributes.Occurenceprobability;
                    grid.IncidentClass = lcmAuditAttributes.Incidentclass;
                    grid.ProductCode = lcmAuditAttributes.Productcode;
                    //grid.HandedOverToOperation = GetHandedOverToOperation(x.Lcmancillarydata?.FirstOrDefault());
                    grid.ContractRenewalPlan = lcmAuditAttributes.Contractrenewalplan;
                    grid.DataSourceLcm = lcmAuditAttributes.Datasource;
                    grid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;
                    grid.OriginalHwLcmId = lcmAuditAttributes.Originalhwlcmid;
                    #region Ticket 551
                    #region //719 Regulatory Fields Changes
                    grid.IsPecn = lcmAuditAttributes.Ispecn;
                    grid.IsPecs = lcmAuditAttributes.Ispecs;
                    grid.IsScf = lcmAuditAttributes.Isscf;
                    grid.IsNof = lcmAuditAttributes.Isnof;
                    #endregion
                    //grid.ExposedEdgeFlag = (lcmAuditAttributes.Exposededgeflag != null) ?
                    //lcmAuditAttributes.Exposededgeflag.Value.ToString() : "";
                    grid.ExternalFacingFlag = lcmAuditAttributes.Externalfacingflag != null ?
                    lcmAuditAttributes.Externalfacingflag.Value.ToString() : "";
                    grid.InfrastructureLocation = lcmAuditAttributes.Locationinfrastructure;
                    grid.ExposedEdgeFlag = GetExposedEdgeValue(lcmAuditAttributes.Isexposededge);
                    #endregion
                }

                #region Apr 03 requirement
                grid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
? lcmAuditAttributes.Handedovertooperation : true;
                grid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
               ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;
                #endregion

                #endregion
                grid.IdentifiedAction = x.Lcmengineering.Archived != true ? GetIdentificationActionForLcmExport(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), hardWare.Endofmaintenance, _repositoryWrapper) : "";

                grid.ProgramLcm = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.ProgramNavigation?.Programdescription;

                grid.ProjectOwner = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Projectowner;

                return grid;
            }).ToList();

            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lasthwupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
            }


            var rtn = new QueryResultDto<ReportHardwareDtoGrid>(new GenerateRenderForGrid<ReportHardwareDtoGrid>(_columnManager))
            {
                TotalItems = totalCount
            };

            rtn.Items = reports.ToArray();

            return rtn;
        }

        private IQueryable<Networkelementsasplanned> GetDisaggregatedQuery(ExpressionStarter<Networkelementsasplanned> predicateResult)
        {
            var lcmDeploymentstatusInServiceId = _commonManager.GetLcmDeploymentStatusId("in-service");
            var environmentId = _commonManager.GetEnvironmentId("production");
            return _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult).AsNoTracking().AsSplitQuery()
                .Where(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                            .SingleOrDefault(m =>
                                m.Ismain &&
                                m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                                m.Deleted == false).Majorhardware.Buildconstruction.Iscloudasset == false &&
                                x.Deploymentstatus.Deploymentstatusid == lcmDeploymentstatusInServiceId)
                                .Where(x => x.Designcomponent.Systemtype.Deleted == false &&
                                 x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(s => s.Deleted == false) &&
                                 x.Lcmengineeringid != null &&
                                 x.Environment.Environmentid == environmentId)

                                .Include(x => x.Opco)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                                .Include(x => x.Lcmengineering.Productimportance)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmancillarydata)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation).ThenInclude(x => x.Riskclustervodafonenames).ThenInclude(x => x.Riskcluster)
                                //.Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Verticalresponsible)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetclass)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                                //.Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Subdomainresponsible)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designaspects).ThenInclude(x => x.Designaspectssupportedsvr)
                                        .ThenInclude(x => x.Service)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverytrackings)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Activitystatus)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Engineeringrisk)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Operationalrisk)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.ProgramNavigation)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Identitiesasis).ThenInclude(x => x.Category);
        }

        public ExpressionStarter<Networkelementsasplanned> ApplyDisaggregatedFilter(ReportHardwareQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);

            var predicateInner = PredicateBuilder.New<Networkelementsasplanned>(true);
            if (buildFilterDto?.BptID != null && buildFilterDto.BptID.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.BptID)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PpmID != null && buildFilterDto.PpmID.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.PpmID)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SerialNumber != null && buildFilterDto.SerialNumber.Any())
            {
                foreach (var item in buildFilterDto?.SerialNumber)
                    predicateInner.Or(x => x.Hwresourcekey == item);
                predicateResult.And(predicateInner);
            }
            #region Ticket 465 - LCM export: RAG status field - filter is not working fine
            //if (buildFilterDto?.RagStatus != null && buildFilterDto.RagStatus.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
            //    foreach (var item in buildFilterDto?.RagStatus)
            //        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere("HARDWARE"))
            //           .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliverytrackings.FirstOrDefault().Ms2status.ToString() == item);
            //    predicateResult.And(predicateInner);
            //}
            #endregion
            if (buildFilterDto?.ProgramLcm != null && buildFilterDto.ProgramLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ProgramLcm)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectOwner != null && buildFilterDto.ProjectOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ProjectOwner)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
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
                    predicateInner.Or(x => x.Identitiesasis.Any(p => p.Interfacetype == ConstantValueFilter.Management && p.Category.Description.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress && p.Value == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.TypeOfNetworkElement != null && buildFilterDto.TypeOfNetworkElement.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.TypeOfNetworkElement)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Lcmpolicy == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ReportId != null && buildFilterDto.ReportId.Any())
            {
                foreach (var item in buildFilterDto?.ReportId)
                    predicateInner.Or(x => x.Lcmengineering.Resourcekey + "-" + x.Hwresourcekey + "0" == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PreviousReportId != null && buildFilterDto.PreviousReportId.Any())
            {
                foreach (var item in buildFilterDto?.PreviousReportId)
                    predicateInner.Or(x => x.Lcmengineering.Previousresourcekey == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OriginalLCMSpreadsheetID != null && buildFilterDto.OriginalLCMSpreadsheetID.Any())
            {
                foreach (var item in buildFilterDto?.OriginalLCMSpreadsheetID)
                    predicateInner.Or(x => x.Previoushwresourcekey == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.DesignComponentIndex != null && buildFilterDto.DesignComponentIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.DesignComponentIndex)
                    predicateInner.Or(x => x.Designcomponentid.ToString() == item);
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
                    (m => m.Organisation.Vertical.Verticalresponsibleid.ToString() == item && m.Deleted == false/* && m.Opcoid == x.Opcoid*/)));
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
                    && x.Networkelementasplannedsubdomainspoc.Any(d => d.Subdomainspoc.Subdomainresponsibleid.ToString() == item));
                    }
                if (count > 0) predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngineeringContactPoint != null && buildFilterDto.EngineeringContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.EngineeringContactPoint)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => !x.Networkelementasplannededuspoc.Any() && !x.Networkelementasplannedsubdomainspoc.Any()
                        );
                    }
                    else
                    {
                        predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(d => d.Subdomainspoc.Id.ToString() == item) ||
                                            x.Networkelementasplannededuspoc.Any(d => d.Eduspoc.Id.ToString() == item));
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
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware.Hardwaresolution == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.AssetType != null && buildFilterDto.AssetType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.AssetType)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware.Buildconstruction.Buildconstruction == item);
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
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m =>
                        m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                        m.Deleted == false && m.Deletiondate == null).Majorhardware.Orgeqpmanufacturer
                    .Originalequipmentmanufacturer == item);
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
            if (buildFilterDto?.DescriptionOfPlannedAction != null && buildFilterDto.DescriptionOfPlannedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.DescriptionOfPlannedAction)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmhardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PlannedHardwareModel != null && buildFilterDto.PlannedHardwareModel.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.PlannedHardwareModel)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(a => a.Designcomponentid != null && a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmhardware && a.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.HardwareUpgrade)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Designcomponent.Systemtype.toLcmDbExportHardwareName() == item);
                predicateResult.And(predicateInner);
            }
            //LCM R8 Phase 1  
            if (buildFilterDto?.ProjectStatus != null && buildFilterDto.ProjectStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ProjectStatus)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.TrackingNumberProjectNameLcm != null && buildFilterDto.TrackingNumberProjectNameLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.TrackingNumberProjectNameLcm)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Notes != null && buildFilterDto.Notes.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Notes)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VendorEndOfMaintenanceDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.VendorEndOfMaintenanceDateValue.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Endofmaintenance >= buildFilterDto.VendorEndOfMaintenanceDateValue.StartDate);
                if (buildFilterDto.VendorEndOfMaintenanceDateValue.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Endofmaintenance <= buildFilterDto.VendorEndOfMaintenanceDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ExtendedSupportOptionOfferedByVendor != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.VendorEndOfMaintenanceDateValue.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Lasttimebuyexpansions >= buildFilterDto.VendorEndOfMaintenanceDateValue.StartDate);
                if (buildFilterDto.VendorEndOfMaintenanceDateValue.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Lasttimebuyexpansions <= buildFilterDto.VendorEndOfMaintenanceDateValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.ProjectEndDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.ProjectEndDateValue.StartDate != null)
                    predicateInner.And(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion >= buildFilterDto.ProjectEndDateValue.StartDate);
                if (buildFilterDto?.ProjectEndDateValue.EndDate != null)
                    predicateInner.And(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion <= buildFilterDto.ProjectEndDateValue.EndDate);
                predicateResult.And(predicateInner);
            }



            if (buildFilterDto?.OpsMaintenanceConractEndValueLcm != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto.OpsMaintenanceConractEndValueLcm.StartDate != null)
                    predicateInner.And(x => x.Lcmengineering.Hardwareendofsupportcontract >= buildFilterDto.OpsMaintenanceConractEndValueLcm.StartDate);

                if (buildFilterDto.OpsMaintenanceConractEndValueLcm.EndDate != null)
                    predicateInner.And(x => x.Lcmengineering.Hardwareendofsupportcontract <= buildFilterDto.OpsMaintenanceConractEndValueLcm.EndDate);
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
                         .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                         .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgetvalue == decimalValue);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                            .OrderBy(x => x.Plannedcompletion).Select(s => new { s.Budgetvalue, s.Currency }).AsQueryable().Select(x => x.Budgetvalue.ToString() + x.Currency).FirstOrDefault() == item);
                    }
                }

                predicateResult.And(predicateInner);

            }

            if (buildFilterDto?.BundleId != null && buildFilterDto.BundleId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.BundleId)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid.Substring(2) == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Platform != null && buildFilterDto.Platform.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Platform)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                   .SingleOrDefault(m =>
                       m.Ismain &&
                       m.Deleted == false).Majorhardware.Hardwaresolution == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.EngRiskEvaluation != null && buildFilterDto.EngRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.EngRiskEvaluation)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngRiskEvaluationNotes != null && buildFilterDto.EngRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.EngRiskEvaluationNotes)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluation != null && buildFilterDto.OpsRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluation)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluationNotes != null && buildFilterDto.OpsRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluationNotes)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OverallRiskEvaluation != null && buildFilterDto.OverallRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OverallRiskEvaluation)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DeliveryPlanAvailable != null && buildFilterDto.DeliveryPlanAvailable.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.DeliveryPlanAvailable)
                {
                    if (item == ConstantValueFilter.Yes.ToUpper())
                    {
                        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                       .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == false);
                    }
                }
                predicateResult.And(predicateInner);
            }
            #region LCM R8 Phase 1  
            if (buildFilterDto.GdprRelevant != null && buildFilterDto.GdprRelevant.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.GdprRelevant)
                {
                    if (item == ConstantValueFilter.Yes.ToUpper())
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

            if (buildFilterDto?.OutputToLcmHardware != null && buildFilterDto.OutputToLcmHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OutputToLcmHardware)
                    predicateInner.Or(x => x.Lcmengineering.Lcmstatushardware == item);
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

            #endregion

            if (buildFilterDto.HwIsExtendedSupportOfferedByVendor != null && buildFilterDto.HwIsExtendedSupportOfferedByVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.HwIsExtendedSupportOfferedByVendor)
                {
                    if (item.ToLower() == ConstantValueFilter.Yes.ToLower())
                    {
                        predicateInner.Or(x => x.Lcmengineering.Hwisextendedsupportofferedbyvendor == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.Hwisextendedsupportofferedbyvendor == false || !x.Lcmengineering.Hwisextendedsupportofferedbyvendor.HasValue);
                    }
                }
                predicateResult.And(predicateInner);
            }
            #region LCM R9 Part - 1
            //if (buildFilterDto?.Custom != null && buildFilterDto.Custom.Any())
            //{
            //    foreach (var item in buildFilterDto?.Custom)
            //        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Custom == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.Custom1 != null && buildFilterDto.Custom1.Any())
            //{
            //    foreach (var item in buildFilterDto?.Custom1)
            //        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Custom1 == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.Custom2 != null && buildFilterDto.Custom2.Any())
            //{
            //    foreach (var item in buildFilterDto?.Custom2)
            //        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Custom2 == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.KpiStatusService != null && buildFilterDto.KpiStatusService.Any())
            //{
            //    foreach (var item in buildFilterDto?.KpiStatusService)
            //        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Kpistatusservice == item));
            //    predicateResult.And(predicateInner);
            //}
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
                 entity.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames.Select(x => x.Riskcluster.Risklevel).FirstOrDefault() : string.Empty) == item))
                        {
                            return true;
                        }
                    }
                    return false;
                });
                var filteredEntityIds = filteredEntities.Select(entity => entity.Lcmengineeringid).ToList();
                predicateResult.And(x => filteredEntityIds.Contains(x.Lcmengineeringid));

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
            //if (buildFilterDto?.Id_New != null && buildFilterDto.Id_New.Any())
            //{
            //    foreach (var item in buildFilterDto?.Id_New)
            //        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Idnew == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.ProductImportanceHistory2 != null && buildFilterDto.ProductImportanceHistory2.Any())
            //{
            //    foreach (var item in buildFilterDto?.ProductImportanceHistory2)
            //        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Productimportancehistory2 == item));
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.LcmStatusJune2021 != null && buildFilterDto.LcmStatusJune2021.Any())
            //{
            //    foreach (var item in buildFilterDto?.LcmStatusJune2021)
            //        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Lcmstatus == item));
            //    predicateResult.And(predicateInner);
            //}
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
                        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Handedovertooperation == item);
                    }
                    else
                        predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Handedovertooperation == item);
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
            if (buildFilterDto?.OriginalHwLcmId != null && buildFilterDto.OriginalHwLcmId.Any())
            {
                foreach (var item in buildFilterDto?.OriginalHwLcmId)
                    predicateInner.Or(x => x.Lcmengineering.Lcmancillarydata.Any(y => y.Originalhwlcmid == item));
                predicateResult.And(predicateInner);
            }
            #region ticket 551
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
            #endregion
            return predicateResult;
        }

        private Dictionary<string, Expression<Func<Lcmengineering, object>>[]> GetColumnsMapDB()
        {
            return new Dictionary<string, Expression<Func<Lcmengineering, object>>[]>
            {
                ["hardwareSheetIndex"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Hardwaresheetindex },
                ["localMarket"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Opco.Opco },
                ["LocalMarketId"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Opco.Opcoid },
                //["verticalEngineeringTeam"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Verticalresponsible.Verticalresponsible },
                //["verticalEngineeringTeamId"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Verticalresponsible.Verticalresponsibleid },

                //["verticalSubDomain"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Subdomainresponsible.Subdomainresponsible },
                // ["operationsContactPoint"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Operationalcontact },
                ["assetCategory"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Assetcategory.Assetcategory },
                ["assetClass"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.toAssetClassDescription(_repositoryWrapper) }, //.AssetClassIdNavigation.AssetClassDescription },
                ["assetType"] = new Expression<Func<Lcmengineering, object>>[] { x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype.Assettype.Assettype : x.Designcomponent.Systemtype.VodafonenameNavigation != null ? x.Designcomponent.Systemtype.VodafonenameNavigation.Description : "" }, // .AssetTypeIdNavigation.AssetTypeDescription },
                ["assetDescription"] = new Expression<Func<Lcmengineering, object>>[] { p => !string.IsNullOrEmpty(p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description) ? p.Designcomponent.Designcomponentfamily.Description : p.Designcomponent.Designcomponentfamily.Description },
                ["productImportance"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Productimportance.Productimportance },
                ["vendor"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer },
                ["hardwareModel"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Single(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Platform, p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Hardwaretype },
                ["numberOfNodes"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Numberofnodes },
                ["vendorEndOfMaintenanceDate"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance },
                //["lcmStatus"] = new Expression<Func<Lcmengineering, object>>[] { p => p.DesignComponent.SystemType.ConstraintLcm },
                ["plannedAction"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Onhardware, p => p.Onsoftware },
                ["descriptionOfPlannedAction"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource },
                ["projectStatus"] = new Expression<Func<Lcmengineering, object>>[] { x =>x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus },
                ["projectEndDate"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion },
                ["trackingNumberProjectName"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname },
                ["notes"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes },
                ["identifiedAction"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityid },
                ["softwareRelease"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion },
                //["cloudVersion"] = new Expression<Func<Lcmengineering, object>>[] { p => "cloud version" },//TODO: cloud version?
                ["bundleBudget"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid},

                ["bundleId"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid },
                ["assetServiceFunctionality"] = new Expression<Func<Lcmengineering, object>>[] { p => !string.IsNullOrEmpty(p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias) ? p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias : p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description },
                ["platform"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Assetclass.Assetclass },
                ["opsMaintenanceConractEnd"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Hardwareendofsupportcontract },
                ["engRiskEvaluation"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description },
                ["engRiskEvaluationNotes"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes },
                ["oPSRiskEvaluation"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description },
                ["oPSRiskEvaluationNotes"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes },
                ["VendorEndOfVulnerabilitySecuritySupportDate"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport },
                ["overallRiskEvaluation"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation },
                ["outputToLcmHardware"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmstatushardware },
                // ["lcmStatus"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmstatushardware },
                ["operationsMaintenanceContract"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Outputtolcmhardware },
                ["lcmStatusEngHardware"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmstatusenghardware },
                ["lcmStatusOpsHardware"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmstatusopshardware },
                //["engineeringContactPoint"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmengineeringsubdomainspoc.FirstOrDefault().Subdomainspoc.Subdomainspoc, p => p.Lcmengineeringeduspoc.FirstOrDefault().Subdomainspoc.Subdomainspoc },
                ["operationsContact"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmoperationalcontracts.FirstOrDefault().Operationalcontract.Description, p => p.Lcmoperationalcontracts.FirstOrDefault().Operationalcontract.Description },
                ["riskCluster"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Vodafonename != null ? p.Designcomponent.Systemtype.VodafonenameNavigation.Description : null },
                ["criticality"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Subnetworkboundary.Description },
                ["isExtendedSupportofferedByVendor"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Hwisextendedsupportofferedbyvendor },
                ["deliveryPlanAvailable"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable },
                #region LCM R9 Part-1
                //["custom "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Custom },
                //["custom1 "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Custom1 },
                //["custom2 "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Custom2 },
                //["kpiStatusService "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Kpistatusservice },
                ["reasonfornoPlan "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Reasonfornoplan },
                ["commentonProjectStatus "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Commentonprojectstatus },
                //["securityRiskPotential "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Securityriskpotential },
                ["securityRiskEffective "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Securityriskeffective },
                ["securityMitigation "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Securitymitigation },
                ["securityRiskOverall "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Securitymitigation },
                ["includedinSecurityScanning "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Includedinsecurityscanning },
                ["raId "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Raid },
                ["requestID "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Requestid },
                //["id_New "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Idnew },
                //["productImportanceHistory2 "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Productimportancehistory2 },
                //["lcmStatusJune2021 "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Lcmstatus },
                ["lastScanDate "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Lastscandate },
                ["assetOutofScopeForReportingPurposes "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Assetoutofscope },
                ["lastUpgradeDate "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Lastupgradedate },
                ["eomControl "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Eomcontrol },
                ["engUpdateTracker "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Engupdatetracker },
                ["opsUpdateTracker "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Opsupdatetracker },
                ["exNetworks "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Exnetworks },
                ["nEWOPSRiskEvaluation "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Occurenceprobability },
                ["occurrenceProbability "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Occurenceprobability },
                ["incidentClass "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Incidentclass },
                ["productCode "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Productcode },
                ["handedOverToOperation "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Handedovertooperation },
                ["contractRenewalPlan "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Contractrenewalplan },
                ["dataSource "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Datasource },
                ["scopeOfSimplification "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Scopeofsimplification },
                // ["cloudVersion"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Cloudversion },
                // ["certifiedSWReleaseforNFVIbundle"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Certifiedswrealesefornfvibundle },
                ["labSWRelease"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Scopeofsimplification },
                #endregion
                #region ticket 551
                //["regulatoryFields"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Regulatoryfields },
                //["exposedEdgeFlag"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Exposededgeflag },
                ["externalFacingFlag"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Externalfacingflag },
                #region // Regulatory fields changes
                ["ispecn"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Ispecn },
                ["isPecs"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Ispecs },
                ["isScf"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Isscf },
                ["isNof"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Isnof },
                #endregion
                #endregion
            };
        }

        private Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]> GetDisaggregatedColumnsMapDB()
        {
            return new Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]>
            {
                ["hardwareSheetIndex"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Hardwaresheetindex },
                ["localMarket"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Opco.Opco },
                //["verticalEngineeringTeam"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Verticalresponsible.Verticalresponsible },
                //["verticalEngineeringTeamId"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Verticalresponsible.Verticalresponsibleid },

                //["verticalSubDomain"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Subdomainresponsible.Subdomainresponsible },
                ["assetCategory"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Assetcategory.Assetcategory },
                ["assetClass"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.toAssetClassDescription(_repositoryWrapper) },
                ["assetType"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype.Assettype.Assettype : x.Designcomponent.Systemtype.VodafonenameNavigation != null ? x.Designcomponent.Systemtype.VodafonenameNavigation.Description : "" },
                ["assetDescription"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => !string.IsNullOrEmpty(p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description) ? p.Designcomponent.Designcomponentfamily.Description : p.Designcomponent.Designcomponentfamily.Description },
                ["productImportance"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Productimportance.Productimportance },
                ["vendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer },
                ["hardwareModel"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                            .Single(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Platform, p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                    .Single(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Hardwaretype },
                ["numberOfNodes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => 1 },
                ["vendorEndOfMaintenanceDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance },
                ["plannedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Onhardware, p => p.Lcmengineering.Onsoftware },
                ["descriptionOfPlannedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource },
                ["projectEndDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion },
                ["trackingNumberProjectName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname },
                ["notes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes },
                ["identifiedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityid },
                ["softwareRelease"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion },
                // ["cloudVersion"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => "cloud version" },//TODO: cloud version?
                ["bundleBudget"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid },

                ["bundleId"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid },
                ["assetServiceFunctionality"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => !string.IsNullOrEmpty(p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias) ? p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias : p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description },
                ["platform"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Assetclass.Assetclass },
                ["opsMaintenanceConractEnd"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Hardwareendofsupportcontract },
                ["engRiskEvaluation"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description },
                ["engRiskEvaluationNotes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes },
                ["oPSRiskEvaluation"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description },
                ["oPSRiskEvaluationNotes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes },
                ["VendorEndOfVulnerabilitySecuritySupportDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport },
                ["overallRiskEvaluation"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation },
                ["outputToLcmHardware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatushardware },
                ["lcmStatus"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatushardware },
                ["operationsMaintenanceContract"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Outputtolcmhardware },
                ["lcmStatusEngHardware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatusenghardware },
                ["lcmStatusOpsHardware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatusopshardware },
                //["engineeringContactPoint"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Networkelementasplannedsubdomainspoc.FirstOrDefault().Subdomainspoc.Subdomainspoc, p => p.Networkelementasplannededuspoc.FirstOrDefault().Subdomainspoc.Subdomainspoc },
                ["operationsContact"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmoperationalcontracts.FirstOrDefault().Operationalcontract.Description, p => p.Lcmengineering.Lcmoperationalcontracts.FirstOrDefault().Operationalcontract.Description },
                ["riskCluster"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Vodafonename != null ? p.Designcomponent.Systemtype.VodafonenameNavigation.Description : null },
                ["criticality"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Subnetworkboundary.Description },
                ["isExtendedSupportofferedByVendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Hwisextendedsupportofferedbyvendor },
                ["deliveryPlanAvailable"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToUpper()))
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable },

                #region LCM R9 Part-1
                //["custom "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Custom },
                //["custom1 "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Custom1 },
                //["custom2 "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Custom2 },
                //["kpiStatusService "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Kpistatusservice },
                ["reasonfornoPlan "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Reasonfornoplan },
                ["commentonProjectStatus "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Commentonprojectstatus },
                //["securityRiskPotential "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Securityriskpotential },
                ["securityRiskEffective "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Securityriskeffective },
                ["securityMitigation "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Securitymitigation },
                ["securityRiskOverall "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Securitymitigation },
                ["includedinSecurityScanning "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Includedinsecurityscanning },
                ["raId "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Raid },
                ["requestID "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Requestid },
                // ["id_New "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Idnew },
                // ["productImportanceHistory2 "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Productimportancehistory2 },
                // ["lcmStatusJune2021 "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Lcmstatus },
                ["lastScanDate "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Lastscandate },
                ["assetOutofScopeForReportingPurposes "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Assetoutofscope },
                ["lastUpgradeDate "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Lastupgradedate },
                ["eomControl "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Eomcontrol },
                ["engUpdateTracker "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Engupdatetracker },
                ["opsUpdateTracker "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Opsupdatetracker },
                ["exNetworks "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Exnetworks },
                ["nEWOPSRiskEvaluation "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Occurenceprobability },
                ["occurrenceProbability "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Occurenceprobability },
                ["incidentClass "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Incidentclass },
                ["productCode "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Productcode },
                ["handedOverToOperation "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Handedovertooperation },
                ["contractRenewalPlan "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Contractrenewalplan },
                ["dataSource "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Datasource },
                ["scopeOfSimplification "] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Scopeofsimplification },
                //["cloudVersion"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Cloudversion },
                //["certifiedSWReleaseforNFVIbundle"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Certifiedswrealesefornfvibundle },
                ["labSWRelease"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Scopeofsimplification },
                #endregion

                #region ticket 551
                //["regulatoryFields"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Regulatoryfields },
                //["exposedEdgeFlag"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmancillarydata.FirstOrDefault().Exposededgeflag },
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

        //LCM R8 Phase 1  
        private static string GetRiskValue(string risk)
        {

            var riskEvaluationArr = risk?.ToLower().Replace(" ", "").Split("-");

            if (riskEvaluationArr != null && riskEvaluationArr.Count() > 0)
            {
                if (riskEvaluationArr[0] == ConstantValueFilter.High.ToLower())
                {
                    risk = ConstantValueFilter.High;
                }
                else if (riskEvaluationArr[0] == ConstantValueFilter.Moderate.ToLower())
                {
                    risk = ConstantValueFilter.Moderate;
                }
                else if (riskEvaluationArr[0] == ConstantValueFilter.Low.ToLower())
                {
                    risk = ConstantValueFilter.Low;
                }
                else if (riskEvaluationArr[0] == ConstantValueFilter.Extreme.ToLower())
                {
                    risk = ConstantValueFilter.Extreme;
                }
            }

            return risk;
        }

        private static string GetAssetType(string build)
        {
            var assetType = string.Empty;
            if (build.ToLower().Replace(" ", "") == ConstantValueFilter.BluprintNfvi.ToLower() || build.ToLower().Replace(" ", "") == ConstantValueFilter.OtherNfvi.ToLower())
            {
                return assetType = "Virtual";
            }
            else if (build.ToLower().Replace(" ", "") == ConstantValueFilter.BluprintNfci.ToLower() || build.ToLower().Replace(" ", "") == ConstantValueFilter.OtherNfci.ToLower())
            {
                return assetType = ConstantValueFilter.Container;
            }
            else
            {
                return assetType = build;
            }
        }

        #region Ticket 465 - LCM export: RAG status field - filter is not working fine
        public IEnumerable<FilterValueDto> aggregatedRagStatusFilterRecord_1(IEnumerable<Lcmengineering> lcm, List<Lcmengineering> singleLcm, string filterSearchValue = "")
        {

            FilterValueDto ragDropDownFilterValueDto = new FilterValueDto();  // -- Return RagStatus DropDown Record
            FilterValueDto ragStatusBasedLcmIdFilterValueDto = new FilterValueDto();  //- Return list fo LcmId to filter based on RagStatus

            if (singleLcm != null && singleLcm.Count() > 0)
                lcm = singleLcm;  // Get RagStatus value for Grid row by row record


            return lcm.Select(x =>
            {
                var paRecord = x.PlannedactivitiesLcmengineering.GetPlannedActivityFilteredDB(ConstantValueFilter.Hardware.ToUpper()) != null ?
                x.PlannedactivitiesLcmengineering.GetPlannedActivityFilteredDB(ConstantValueFilter.Hardware.ToUpper())?.Deliverytrackings : null;


                ragDropDownFilterValueDto = new FilterValueDto();
                ragStatusBasedLcmIdFilterValueDto = new FilterValueDto();

                string unAssignedRagStatusValue = "---";

                var hardWare = x.Designcomponent.Systemtype?.Systemtypesmajorhardwarebuilds?.SingleOrDefault
                (m => m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid && m.Deleted == false)?.Majorhardware;

                DateTime? VendorEndOfMaintenanceDate = hardWare?.Endofmaintenance;

                EOMEnum EOMStatus = hardWare != null ? (EOMEnum)hardWare.Eomstatus
                      : EOMEnum.NotSpecified;

                #region Filter Text
                ragDropDownFilterValueDto.Text = string.IsNullOrEmpty(filterSearchValue) ?


            x.Archived == true ?
                LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3) :
                  hardWare != null ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate)
            EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?
            unAssignedRagStatusValue :

              //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null)

              VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null ?
              !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) ? unAssignedRagStatusValue : LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate ?
             !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) ? unAssignedRagStatusValue : LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

            : unAssignedRagStatusValue
            : unAssignedRagStatusValue
            : unAssignedRagStatusValue;

                #endregion Filter Text

                #region filterValue - Value
                ragDropDownFilterValueDto.Value = string.IsNullOrEmpty(filterSearchValue) ?

            x.Archived == true ?
                LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3) :
                  hardWare != null ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?
            unAssignedRagStatusValue :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate|| x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?

            VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null ?
            !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) ? unAssignedRagStatusValue :
               LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate ?
              !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) ? unAssignedRagStatusValue :
              LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

            : unAssignedRagStatusValue
            : unAssignedRagStatusValue
            : unAssignedRagStatusValue;

                #endregion filterValue - Value



                #region fetch LcmEngineeringId

                ragStatusBasedLcmIdFilterValueDto.Value = !string.IsNullOrEmpty(filterSearchValue) ?

                #region Completed Status
           filterSearchValue.ToString().ToLower() == ConstantValueFilter.Completed.ToLower() ?
            x.Archived == true ?
                 x.Lcmengineeringid.ToString() :
                  hardWare != null ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?
            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?
            VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null ?
             LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status).ToLower() == ConstantValueFilter.Completed.ToLower() ? x.Lcmengineeringid.ToString() : "0" :

             //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
             fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate ?
             !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) &&
             LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == ConstantValueFilter.Completed.ToLower() ? x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion Completed Status
        :
                #region On Track && On Track Status
          filterSearchValue.ToString().ToLower().Replace(" ", "") == ConstantValueFilter.OnTrack.ToLower() || filterSearchValue.ToString().ToLower() == ConstantValueFilter.Delayed.ToLower() ?
            x.Archived == true ?
                 "0" :
                  x.Designcomponent?.Systemtype?.Majorsoftwarebuilds != null ?

           // (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
           EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?

            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null && paRecord.FirstOrDefault()?.Ms2status != null ?

             !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) &&
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status).ToLower().Replace(" ", "") == ConstantValueFilter.OnTrack.ToLower() ||
             LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status).ToLower() == ConstantValueFilter.Delayed.ToLower()) ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate && paRecord.FirstOrDefault()?.Ms2status != null ?
              !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) && (Convert.ToInt16(paRecord.FirstOrDefault()?.Ms2status) == 1 ||
              Convert.ToInt16(paRecord.FirstOrDefault()?.Ms2status) == 2) ?
               x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion Completed Status
        :
                #region --- Records
         filterSearchValue.ToString() == unAssignedRagStatusValue ?
           x.Archived == true ?
                "0" :
                  hardWare != null ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?
             x.Lcmengineeringid.ToString() :

             //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?

             VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null ?
             LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "" ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate ?
            LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "" ?
             x.Lcmengineeringid.ToString() : "0"
           : x.Lcmengineeringid.ToString()
            : x.Lcmengineeringid.ToString()

           : "0"
                #endregion --- Records
         : "";

                ragStatusBasedLcmIdFilterValueDto.Text = ragStatusBasedLcmIdFilterValueDto.Value;
                #endregion

                return !string.IsNullOrEmpty(filterSearchValue) ? ragStatusBasedLcmIdFilterValueDto : ragDropDownFilterValueDto;

            }
            );

        }


        public static IEnumerable<FilterValueDto> disAggregatedRagStatusFilterRecord_1(IEnumerable<Networkelementsasplanned> lcm, List<Networkelementsasplanned> singleLcm, string filterSearchValue = "")
        {

            FilterValueDto ragDropDownFilterValueDto = new FilterValueDto();  // -- Return RagStatus DropDown Record
            FilterValueDto ragStatusBasedLcmIdFilterValueDto = new FilterValueDto();  //- Return list fo LcmId to filter based on RagStatus

            if (singleLcm != null && singleLcm.Count() > 0)
                lcm = singleLcm;  // Get RagStatus value for Grid row by row record


            return lcm.Select(x =>
            {
                var paRecord = x.Lcmengineering.PlannedactivitiesLcmengineering.GetPlannedActivityFilteredDB(ConstantValueFilter.Hardware.ToUpper()) != null ?
                x.Lcmengineering.PlannedactivitiesLcmengineering.GetPlannedActivityFilteredDB(ConstantValueFilter.Hardware.ToUpper())?.Deliverytrackings : null;

                ragDropDownFilterValueDto = new FilterValueDto();
                ragStatusBasedLcmIdFilterValueDto = new FilterValueDto();

                string unAssignedRagStatusValue = "---";

                var hardWare = x.Designcomponent.Systemtype?.Systemtypesmajorhardwarebuilds?.SingleOrDefault
                (m => m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid && m.Deleted == false)?.Majorhardware;



                DateTime? VendorEndOfMaintenanceDate = hardWare?.Endofmaintenance;

                EOMEnum EOMStatus = hardWare != null ? (EOMEnum)hardWare.Eomstatus
                      : EOMEnum.NotSpecified;

                #region Filter Text
                ragDropDownFilterValueDto.Text = string.IsNullOrEmpty(filterSearchValue) ?


            x.Lcmengineering.Archived == true ?
                LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3) :
                  hardWare != null ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate)
            EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?
            unAssignedRagStatusValue :

              //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null)

              VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null ?
              !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) ? unAssignedRagStatusValue : LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate ?
             !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) ? unAssignedRagStatusValue : LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

            : unAssignedRagStatusValue
            : unAssignedRagStatusValue
            : unAssignedRagStatusValue;

                #endregion Filter Text

                #region filterValue - Value
                ragDropDownFilterValueDto.Value = string.IsNullOrEmpty(filterSearchValue) ?

            x.Lcmengineering.Archived == true ?
                LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3) :
                  hardWare != null ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?
            unAssignedRagStatusValue :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate|| x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?

            VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null ?
            !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) ? unAssignedRagStatusValue :
               LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate ?
              !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) ? unAssignedRagStatusValue :
              LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status)

            : unAssignedRagStatusValue
            : unAssignedRagStatusValue
            : unAssignedRagStatusValue;

                #endregion filterValue - Value



                #region fetch LcmEngineeringId

                ragStatusBasedLcmIdFilterValueDto.Value = !string.IsNullOrEmpty(filterSearchValue) ?

                #region Completed Status
           filterSearchValue.ToString().ToLower() == ConstantValueFilter.Completed.ToLower() ?
            x.Lcmengineering.Archived == true ?
                 x.Lcmengineeringid.ToString() :
                  hardWare != null ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?
            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?
            VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null ?
             LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status).ToLower() == ConstantValueFilter.Completed.ToLower() ? x.Lcmengineeringid.ToString() : "0" :

             //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
             fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate ?
             !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) &&
             LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status).ToLower() == ConstantValueFilter.Completed.ToLower() ? x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion Completed Status
        :
                #region On Track && On Track Status
          filterSearchValue.ToString().ToLower().Replace(" ", "") == ConstantValueFilter.OnTrack.ToLower() || filterSearchValue.ToString().ToLower() == ConstantValueFilter.Delayed.ToLower() ?
            x.Lcmengineering.Archived == true ?
                 "0" :
                  x.Designcomponent?.Systemtype?.Majorsoftwarebuilds != null ?

           // (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
           EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?

            "0" :

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null && paRecord.FirstOrDefault()?.Ms2status != null ?

             !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) &&
             (LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status).ToLower().Replace(" ", "") == ConstantValueFilter.OnTrack.ToLower() ||
             LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status).ToLower() == ConstantValueFilter.Delayed.ToLower()) ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate && (paRecord.FirstOrDefault()?.Ms2status != null)) ?
            fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate && paRecord.FirstOrDefault()?.Ms2status != null ?
              !string.IsNullOrEmpty(Convert.ToString(paRecord.FirstOrDefault()?.Ms2status)) && (Convert.ToInt16(paRecord.FirstOrDefault()?.Ms2status) == 1 ||
              Convert.ToInt16(paRecord.FirstOrDefault()?.Ms2status) == 2) ?
               x.Lcmengineeringid.ToString() : "0"
           : "0"
            : "0"
                #endregion Completed Status
        :
                #region --- Records
         filterSearchValue.ToString() == unAssignedRagStatusValue ?
           x.Lcmengineering.Archived == true ?
                "0" :
                  hardWare != null ?

            //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > targetDate) ?
            EOMStatus == (short)EOMEnum.NotAnnounced || VendorEndOfMaintenanceDate > targetDate ?
             x.Lcmengineeringid.ToString() :

             //(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < fronzenDate || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance == null) ?

             VendorEndOfMaintenanceDate < fronzenDate || VendorEndOfMaintenanceDate == null ?
             LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "" ?
             x.Lcmengineeringid.ToString() : "0"
           :
            //(fronzenDate <= x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= targetDate) ?
            fronzenDate <= VendorEndOfMaintenanceDate || VendorEndOfMaintenanceDate <= targetDate ?
            LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(paRecord.FirstOrDefault()?.Ms2status) == "" ?
             x.Lcmengineeringid.ToString() : "0"
           : x.Lcmengineeringid.ToString()
            : x.Lcmengineeringid.ToString()

           : "0"
                #endregion --- Records
         : "";

                ragStatusBasedLcmIdFilterValueDto.Text = ragStatusBasedLcmIdFilterValueDto.Value;
                #endregion

                return !string.IsNullOrEmpty(filterSearchValue) ? ragStatusBasedLcmIdFilterValueDto : ragDropDownFilterValueDto;

            }
            );

        }
        #endregion
    }
}


