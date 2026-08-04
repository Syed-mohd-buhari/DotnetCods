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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CAM.BusinessManager.Rules.LCMEngineeringRulesExtension;

namespace CAM.BusinessManager.Entity.Report
{
    public class ReportSubnetWorkManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private CommonManager _commonManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly string _engUpdateTracker = "To be started";
        private static int currenYear = DateTime.Now.Year;
        private static readonly DateTime fronzenDate = new DateTime(currenYear, 6, 1);
        private static readonly DateTime targetDate = new DateTime(currenYear + 1, 6, 1);

        private static Expression<Func<Plannedactivities, bool>> PlannedActivitySoftware = PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software);
        private static Expression<Func<Plannedactivities, bool>> PlannedActivityHardware = PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware);


        public ReportSubnetWorkManager(IEnumerable<IRepositoryWrapper> wrappers,
        GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, CommonManager commonManager,
        IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _commonManager = commonManager;

        }

        #region AggregatedSoftWare

        public List<FilterValueDto> GetAggregatedSoftwareFilter(string propertyName, string propertyFilter,
            ReportSubnetWorkQueryDto buildFilterDto,bool isAdmin=false)
        {
            var str = PlannedActivitySoftware;

            ExpressionStarter<Lcmengineering> predicateResult = ApplyFilter(buildFilterDto);
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
                "wbsCode" => query.Select(p => new FilterValueDto
                     (GetWbsCode(p.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitySoftware)
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectname))).Distinct().ToList(),
                "bptID" => query
                .Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivitySoftware)
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Projectstatus != null ?
                 p.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivitySoftware)
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Budgettrackingid : string.Empty)).Distinct().ToList(),
                "ppmID" => query
                    .Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitySoftware)
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Projectstatus != null ?
                     p.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivitySoftware)
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectid : string.Empty)).Distinct().ToList(),
                "mainOrganization" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Nse) },
                "serialNumber" => query.Select(p => new FilterValueDto(p.Resourcekey)).Distinct().ToList(),

                "ragStatus" => LcmEngineeringExtensionMethod.aggregatedRagStatusFilterRecord(query, null, 2).Distinct().ToList(),
                "ipAddress" => query.SelectMany(x => x.Networkelementsasplanned)?.SelectMany(x => x.Identitiesasis)?.Where(p => p.Interfacetype == ConstantValueFilter.Management && p.Category.Description.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress)
                        .Select(p => new FilterValueDto(p.Value)).Distinct().ToList(),
                "engKpi2" => query.Select(p => new FilterValueDto(GetEngKpi2(p.Lcmstatusengsoftware, p.Outputtolcmsoftware))).ToList().Distinct().ToList(),
                "expLCMstatusatendofFY24" => query.Select(p => new FilterValueDto(
                                            GetExpLCMstatusatendofFY24(p.Lcmstatussoftware,
                                                                        p.Outputtolcmsoftware,
                                                                        p.Softwareendofwarrantydate != null ? p.Softwareendofwarrantydate : p.Softwareendofsupportcontract,
                                                                        p.PlannedactivitiesLcmengineering.AsQueryable()
                                                                       .Where(PlannedActivitySoftware)
                                                                      .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion,
                                                                         p.PlannedactivitiesLcmengineering.AsQueryable()
                                                                       .Where(PlannedActivitySoftware)
                                                                      .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus
                                                                        ))).ToList().Distinct().ToList(),
                "reportId" => query.Select(p => new FilterValueDto(p.Resourcekey)).Distinct().ToList(),
                "previousReportId" => query.Select(p => new FilterValueDto(p.Previousresourcekey)).Distinct().ToList(),
                "identifiedAction" => query.Select(x => new FilterValueDto(GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                                               .Where(PlannedActivitySoftware)
                                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance, _repositoryWrapper))).ToList().Distinct().ToList(),
                "originalLCMSpreadsheetID" => query.Select(p => new FilterValueDto(p.Previousresourcekey)).Distinct().ToList(),
                "localMarket" => query.Select(p => new FilterValueDto(p.Opco.Opco)).Distinct().ToList(),
                "designComponentIndex" => query.Select(p => new FilterValueDto(p.Designcomponentid.ToString())).Distinct().ToList(),
                "operationsContactPoint" => query.SelectMany(x => x.Lcmoperationalcontracts)
                                 .Select(p => new FilterValueDto(p.Operationalcontract.Description)).Distinct().ToList(),
                "assetCategory" => query.AsEnumerable().Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Assetcategory.Assetcategory)).Distinct().ToList(),
                "operationsMaintenanceContract" => query.ToList().Select(p => new FilterValueDto(p.Outputtolcmsoftware)).Distinct().ToList(),
                "assetClass" => query.Where(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null).ToList().Select(p => new FilterValueDto(
                    p.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? p.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description : "")
                   ).Distinct().ToList(),
                "assetType" => query.Where(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Criticalassettype != null).ToList()
                   .Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Criticalassettype.Description)).Distinct().ToList(),
                "productImportance" => _repositoryWrapper.ProductImportance.FindAll().Select(x => new FilterValueDto(x.Productimportance)).ToList(),
                "vendor" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer)).Distinct().ToList(),
                "hardwareModel" => query.AsEnumerable()
                        .Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                   .SingleOrDefault(m =>
                       m.Ismain &&
                       m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                       m.Deleted == false)?.Majorhardware?.Buildconstruction?.Rule == (int)BuildconstructionRuleEnum.VirtualHW ? ConstantValueFilter.Virtualized : x.Designcomponent.Systemtype.toLcmDbExportHardwareName())).Distinct().ToList(),
                "numberOfNodes" => query.Select(p => new FilterValueDto(p.Numberofnodes.ToString())).Distinct().ToList(),
                "plannedAction" => new List<FilterValueDto>() { new FilterValueDto() { Text = ConstantValueFilter.OnHardware, Value = "1" }, new FilterValueDto() { Text = ConstantValueFilter.OnSoftware, Value = "2" } },
                "descriptionOfPlannedAction" => query.Select(p => new FilterValueDto
                            (p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource)).Distinct().ToList(),
                "plannedSoftwareVersion" => query.Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Activitydetails)).ToList().Append(new FilterValueDto(ConstantValueFilter.Na)).Distinct().ToList(),
                "typeOfNetworkElement" => query.Where(x => x.Designcomponent.Subnetworkboundary.Lcmpolicy != null).Select(x => new FilterValueDto
                {
                    Text = ((LCMPolicy)x.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString(),
                    Value = x.Designcomponent.Subnetworkboundary.Lcmpolicy.ToString()
                }).Distinct().ToList(),
                "trackingNumberProjectName" => query.Select(x => new FilterValueDto
                        (x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname)).Distinct().ToList(),
                "notes" => query.Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes)).Distinct().ToList(),
                "assetVirtualized" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.YES), new FilterValueDto(ConstantValueFilter.NO) },
                "softwareVersion" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion)).Distinct().ToList(),
                "budgetEstimated" => query.Select(s => s.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                                                       .FirstOrDefault())?.AsEnumerable()
                                                       ?.Select(p => new FilterValueDto(p?.Currency == null ? p?.Budgetvalue?.ToString() : p?.Budgetvalue?.ToString() + p?.Currency))?.Distinct()?.ToList(),
                "bundleBudget" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.yes), new FilterValueDto(ConstantValueFilter.no) },
                "bundleId" => query.AsEnumerable().Where(x => GetBundleBudget(x.PlannedactivitiesLcmengineering.AsQueryable()
                                            .Where(PlannedActivityHardware)
                                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Budgettrackingid) == ConstantValueFilter.yes)
                                               .Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                                            .Where(PlannedActivityHardware)
                                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid.Substring(2))).Distinct().ToList(),
                "assetServiceFunctionality" => query.Select(p => new FilterValueDto(string.Join(" | ", p.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Select(x => x.Service.Description)))).ToList().Distinct().ToList(),
                "platform" => query.Where(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null)
                            .Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description,
                            x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description)).Distinct().ToList(),
                #region LCM R8 Phase 1  
                "riskCluster" => query.Where(x => x.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames != null).Select(p => new FilterValueDto(p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                .Select(x => x.Riskcluster.Riskclusterid).FirstOrDefault(),
                 p.Designcomponent.Systemtype.VodafonenameNavigation.Riskclustervodafonenames
                .Select(x => x.Riskcluster.Description).FirstOrDefault())).Distinct().ToList(),
                "criticality" => query.Select(p => new FilterValueDto(p.Designcomponent.Subnetworkboundary.Criticality)).Distinct().ToList(),
                "gdprRelevant" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.YES), new FilterValueDto(ConstantValueFilter.NO) },
                "engRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = _commonManager.GetRiskValue(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description),
                    Value = x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid.ToString()
                }).Distinct().ToList(),
                #endregion
                "overallRiskEvaluation" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                                         .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation)).Distinct().ToList(),
                "engRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes)).Distinct().ToList(),
                "opsRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = _commonManager.GetRiskValue(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description),
                    Value = x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid.ToString()
                }
                ).Distinct().ToList(),
                "opsRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                      .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "outputToLcmSoftware" => query.ToList().Select(p => new FilterValueDto(p.Lcmstatussoftware)).Distinct().ToList(),
                "lcmStatusOpsSoftware" => query.ToList().Select(p => new FilterValueDto(p.Lcmstatusopssoftware)).Distinct().ToList(),
                "lcmStatus" => query.ToList().Select(p => new FilterValueDto(p.Lcmstatussoftware)).Distinct().ToList(),
                "lcmStatusEngSoftware" => query.ToList().Select(p => new FilterValueDto(p.Lcmstatusengsoftware)).Distinct().ToList(),
                "isExtendedSupportOfferedByVendor" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.yes), new FilterValueDto(ConstantValueFilter.no) },
                "deliveryPlanAvailable" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.yes), new FilterValueDto(ConstantValueFilter.no) },

                "assetStatus" => query.ToList().Where(x => x.Lcmancillarydata.Any()).Select(x => new FilterValueDto(
                    GetAssetStatusBasedOnOriginalHwAndSw(
                      x.Lcmancillarydata.FirstOrDefault() != null ?
                      x.Lcmancillarydata.FirstOrDefault().Originalswlcmid : string.Empty

                        )))
                .Distinct().ToList(),
                "program" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                           .Where(PlannedActivitySoftware)
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription)).ToList().Distinct().ToList(),
                "projectOwner" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                               .Where(PlannedActivitySoftware)
                               .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectowner)).ToList().Distinct().ToList(),
                "projectStatus" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivitySoftware)
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
                    Text = p.Lcmancillarydata.FirstOrDefault()?.Includedinsecurityscanning == true ? ConstantValueFilter.yes : ConstantValueFilter.no,
                    Value = p.Lcmancillarydata.FirstOrDefault()?.Includedinsecurityscanning.ToString()
                }).Distinct().ToList(),
                "raId" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Raid)).Distinct().ToList(),
                "requestID" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Requestid)).Distinct().ToList(),
                // "id_New" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Idnew)).Distinct().ToList(),
                //"productImportanceHistory2" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Productimportancehistory2)).Distinct().ToList(),
                // "lcmStatusJune2021" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Lcmstatus)).Distinct().ToList(),
                "lastScanDate" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Lastscandate)).Distinct().ToList(),
                "assetOutofScopeForReportingPurposes" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Assetoutofscope)).Distinct().ToList(),
                "lastUpgradeDate" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Lastupgradedate)).Distinct().ToList(),
                "eomControl" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Eomcontrol)).Distinct().ToList(),

                "engUpdateTracker" => query.ToList().Select(p => new FilterValueDto(
                     p.Lcmancillarydata.FirstOrDefault()?.Engupdatetracker)).Distinct().ToList(),
                //.Append(new FilterValueDto(new FilterValueDto
                //{
                //    Text = _engUpdateTracker,
                //    Value = _engUpdateTracker
                //}))



                "opsUpdateTracker" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Opsupdatetracker)).Distinct().ToList(),
                "exNetworks" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Exnetworks)).Distinct().ToList(),
                "newopsRiskEvaluation" => query.ToList().Select(p => new FilterValueDto(GetNewOpsRiskEvaluationValue(p.Lcmancillarydata.FirstOrDefault()?.Incidentclass, p.Lcmancillarydata.FirstOrDefault()?.Occurenceprobability))).Distinct().ToList(),
                "occurrenceProbability" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Occurenceprobability)).Distinct().ToList(),
                "incidentClass" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Incidentclass)).Distinct().ToList(),
                "productCode" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Productcode)).Distinct().ToList(),
                "handedOverToOperation" => query.ToList().Select(p => new FilterValueDto { Text = p.Lcmancillarydata.FirstOrDefault()?.Handedovertooperation == true ? ConstantValueFilter.yes : ConstantValueFilter.no, Value = p.Lcmancillarydata.FirstOrDefault()?.Handedovertooperation.ToString() }).Distinct().ToList(),


                "contractRenewalPlan" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Contractrenewalplan)).Distinct().ToList(),
                "dataSource" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Datasource)).Distinct().ToList(),
                "scopeOfSimplification" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Scopeofsimplification)).Distinct().ToList(),
                //"cloudVersion" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Cloudversion)).Distinct().ToList(),
                //"certifiedSWReleaseforNFVIbundle" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Certifiedswrealesefornfvibundle)).Distinct().ToList(),
                "originalSwLcmId" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Originalswlcmid)).Distinct().ToList(),
                "labSWRelease" => query.ToList().Select(p => new FilterValueDto(p.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Softwareversion)).Distinct().ToList(),
                #endregion
                #region Ticket 551
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

                "exposedEdgeFlag" => query.ToList().Select(p => new FilterValueDto(GetExposedEdgeValue(p.Lcmancillarydata.FirstOrDefault()?.Isexposededge))).Distinct().ToList(),

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
                                 .Concat(query.Where(x => x.Lcmengineeringsubdomainspoc != null && x.Lcmengineeringsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   )).Distinct().ToList(),
                "verticalSubDomain" => string.IsNullOrEmpty(propertyFilter)
                                ? query.SelectMany(x => x.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid))
                                .Select(p => new FilterValueDto
                                {
                                    Text = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsible,
                                    Value = p.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString()
                                })).Distinct().ToList()
                                 .Concat(query.Where(x => x.Lcmengineeringsubdomainspoc != null && x.Lcmengineeringsubdomainspoc.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "yes",
                                      }
                                   )).Distinct().ToList()
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

                "designComponentFamily" => query.ToList().Select(p => new FilterValueDto
                {
                    Text = p.Designcomponent.toDesignComponentFamily(),
                    Value = p.Designcomponent.Designcomponentfamilyid.ToString()
                }).Distinct().ToList(),
                "supportedService" => query.ToList().Select(p => new FilterValueDto
                {
                    Text = p.Designcomponent?.Subnetworkboundary?.Subnetworksupportedsvr?.Select(x => x.Service?.Description).FirstOrDefault(),
                    Value = p.Designcomponent?.Subnetworkboundary?.Subnetworksupportedsvr?.Select(x => x.Serviceid).FirstOrDefault().ToString()
                }).Distinct().ToList(),

                "hostname" => query.ToList().Where(x => x.Networkelementsasplanned != null)
            .SelectMany(p => p.Networkelementsasplanned).Where(y => y.Elementname != null)
            .Select(y => new FilterValueDto { Text = y.Elementname, Value = Convert.ToString(y.Networkelementasplannedid) }).Distinct().ToList(),

                "components" => string.IsNullOrEmpty(propertyFilter) ?
                                query.ToList().Where(a => a.Buildbag?.Componentsoftwarebuildbags != null)
                                .SelectMany(p => p.Buildbag?.Componentsoftwarebuildbags?
                                .Select(y => new FilterValueDto
                                {
                                    Text = ComponentBagExtensionMethod.GetComponentDescription(y?.Componentsoftwarebuild),
                                    Value = y?.Componentsoftwarebuildid.ToString()
                                })).Distinct().ToList()
                                : query.ToList()
                                .Where(a => a.Buildbag?.Componentsoftwarebuildbags != null && a.Buildbag.Componentsoftwarebuildbags.
                                Any(a1 => a1.Componentsoftwarebuildid.ToString().Contains(propertyFilter)))
                                .SelectMany(p => p.Buildbag?.Componentsoftwarebuildbags?
                                .Select(y => new FilterValueDto
                                {
                                    Text = ComponentBagExtensionMethod.GetComponentDescription(y?.Componentsoftwarebuild),
                                    Value = y?.Componentsoftwarebuildid.ToString()
                                })).Distinct().ToList(),

                _ => new List<FilterValueDto>(),
            };
            if (!isAdmin && (buildFilterDto.VerticalEngineeringTeam != null && buildFilterDto.VerticalEngineeringTeam.Count > 0) && propertyName == "verticalEngineeringTeam")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalEngineeringTeam.Contains(x.Value.ToString())).ToList();
            }
            return rtn;
        }
        public QueryResultDto<ReportSubnetWorkSoftWareGrid> FindAggregatedSoftwareWithCondition(ReportSubnetWorkQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Lcmengineering> predicateResult = ApplyFilter(buildFilterDto);


            var result = GetQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetColumnsMapDB(), "Modificationdate");
            IEnumerable<Lcmengineering> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();

            if (buildFilterDto?.OutputToLcmSoftware != null && buildFilterDto.OutputToLcmSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.OutputToLcmSoftware.Contains(x.Lcmstatussoftware));
            }
            if (buildFilterDto?.LcmStatusEngSoftware != null && buildFilterDto.LcmStatusEngSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusEngSoftware.Contains(x.Lcmstatusengsoftware));
            }
            if (buildFilterDto?.LcmStatusOpsSoftware != null && buildFilterDto.LcmStatusOpsSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusOpsSoftware.Contains(x.Lcmstatusopssoftware));
            }
            if (buildFilterDto?.OperationsMaintenanceContractLcm != null && buildFilterDto.OperationsMaintenanceContractLcm.Any())
            {
                query = query.Where(x => buildFilterDto.OperationsMaintenanceContractLcm.Contains(x.Outputtolcmsoftware));
            }

            if (buildFilterDto?.ENGKPI2 != null && buildFilterDto.ENGKPI2.Any())
            {
                query = query.Where(x => buildFilterDto.ENGKPI2.Contains(GetEngKpi2(x.Lcmstatusengsoftware, x.Outputtolcmsoftware)));
            }
            if (buildFilterDto?.ExpLCMstatusatendofFY24 != null && buildFilterDto.ExpLCMstatusatendofFY24.Any())
            {
                query = query.Where(x => buildFilterDto.ExpLCMstatusatendofFY24.Contains(
                        GetExpLCMstatusatendofFY24(x.Lcmstatussoftware,
                                                   x.Outputtolcmsoftware,
                                                   x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract,
                                                   x.PlannedactivitiesLcmengineering.AsQueryable()
                                                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                                                   ?.Plannedcompletion,
                                                   x.PlannedactivitiesLcmengineering.AsQueryable()
                                                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                                                   ?.Projectstatus
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

                query = query.Where(x => x.Archived != true && buildFilterDto.IdentifiedAction.Contains(GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(),
                   x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofmaintenance, _repositoryWrapper)));
            }
            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Budgettrackingid)));
            }

            if (buildFilterDto?.AssetServiceFunctionality != null && buildFilterDto.AssetServiceFunctionality.Any())
            {

                query = query.Where(x => x?.Designcomponent?.Designcomponentfamily?.Subnetworkboundary != null).Where(x => buildFilterDto.AssetServiceFunctionality
                                 .Contains(string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Subnetworkboundary?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description)?.Distinct())));
            }
            if (buildFilterDto?.AssetStatus != null && buildFilterDto.AssetStatus.Any())
            {
                query = query.ToList().Where(x => x.Lcmancillarydata.Any()).Where(x => buildFilterDto.AssetStatus.Contains(
                    GetAssetStatusBasedOnOriginalHwAndSw(
                        x.Lcmancillarydata?.FirstOrDefault() != null ?
                        x.Lcmancillarydata?.FirstOrDefault()?.Originalswlcmid : string.Empty

                        )

                    ));
            }
            if (buildFilterDto?.ExposedEdgeFlag != null && buildFilterDto.ExposedEdgeFlag.Any())
            {
                query = query.ToList().Where(x => x.Lcmancillarydata.Any())
                    .Where(x => buildFilterDto.ExposedEdgeFlag.Contains(GetExposedEdgeValue(x.Lcmancillarydata.FirstOrDefault().Isexposededge)));
            }

            #region Ticket 465 - LCM export: RAG status field - filter is not working fine
            if (buildFilterDto?.RagStatus != null && buildFilterDto.RagStatus.Any())
            {
                List<List<string>> lcmIdList = new List<List<string>>();
                foreach (var item in buildFilterDto.RagStatus)
                    lcmIdList.Add(
                        LcmEngineeringExtensionMethod.aggregatedRagStatusFilterRecord(query, null, 2, item).Where(x => x.Value != "0")
                        .Select(x => x.Text).ToList()
                        );

                if (lcmIdList != null && lcmIdList.Count() > 0)
                    query = query.Where(x => lcmIdList.SelectMany(y => y).Distinct().Contains(x.Lcmengineeringid.ToString()));

            }
            #endregion

            var totalCount = query.Count();
            if (!isExport)
            {
                query = query.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            else
            {
                query = query.ToList();
            }

            var allOperationalContracts = _repositoryWrapper.LCMOperationalContracts.FindAll().Include(x => x.Operationalcontract).ToList();
            #region Code Optimization

            var allVerticalFilterDto = _repositoryWrapper.VerticalResponsible.FindAll().Select(x => new FilterValueDto
            {
                Value = x.Verticalresponsibleid.ToString(),
                Text = x.Verticalresponsible
            }).ToList().DistinctBy(t => t.Value);
            string allVerticalForAdminRole = string.Join(",", allVerticalFilterDto.Select(x => x.Text).ToList()).ToString();
            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != null).Select(x => x.Lcmengineeringid)?.Distinct()?.ToList();
            var lcmIdAndLcmOpcoId = query?.ToList()?.Where(x => x.Lcmengineeringid != null)?.DistinctBy(x => x?.Lcmengineeringid)
                .ToDictionary(x => x.Lcmengineeringid, x => (long)x.Opcoid);
            var allSubDomain = _commonManager.GetCalculatedLcmSubDomainSpocEntityForReport(lcmIdAndLcmOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedLcmEduSpocEntityForReport(lcmIdAndLcmOpcoId).ToList();
            var allOperationalContract = _commonManager.GetCalculateLCMOperationalContractsForReport(lcmengineeringsEntityId);

            #endregion
            var reportLastUpdateDate = _repositoryWrapper.LcmDBExportUpdateHistory.FindAll().FirstOrDefault();
            bool isLcmDBExportUpdated = false;
            if (reportLastUpdateDate != null && reportLastUpdateDate.Lastswupdatedate.Date == DateTime.Now.Date)
            {
                isLcmDBExportUpdated = true;
            }

            var reports = query.Select(x =>
            {
                var grid = new ReportSubnetWorkSoftWareGrid();
                var systemType = x.Designcomponent?.Systemtype;
                var majorSW = systemType?.Majorsoftwarebuilds;
                var plannedActivitySW = x.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Software).Plannedactivityresourceid == null ? null : x.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Software);
                var dcSubnetwork = x.Designcomponent?.Subnetworkboundary;
                var Dcf = x.Designcomponent?.Designcomponentfamily;

                if (!isLcmDBExportUpdated)
                {
                    x.Outputtolcmsoftware = x.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareOutput).Result;
                    x.Lcmstatussoftware = x.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatus).Result;
                    x.Lcmstatusopssoftware = x.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusOps).Result;
                    x.Lcmstatusengsoftware = x.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusEng).Result;
                    _repositoryWrapper.Lcmengineering.Update(x);
                    _repositoryWrapper.Save();
                    _repositoryWrapper.ClearTracker();

                }

                grid.DesignComponentFamily = x.Designcomponent?.toDesignComponentFamily();
                grid.SupportedService = string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct() ?? new List<string>());


                //Lcm Status
                grid.LcmStatus = x.Lcmstatussoftware;
                grid.OperationsMaintenanceContract = x.Outputtolcmsoftware;
                grid.LcmStatusOpsSoftware = x.Lcmstatusopssoftware;
                grid.LcmStatusEngSoftware = x.Lcmstatusengsoftware;

                grid.ReportId = x.Resourcekey;

                grid.LcmEngineeringId = x.Lcmengineeringid;
                grid.LocalMarket = x.Opco?.Opco;
                grid.DesignComponentIndex = x.Designcomponentid;

                #region code optimize org Table
                ;

                grid.VerticalEngineeringTeam =
       string.Join(",", allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && m.VerticalDic != null && x.Deleted == false)
         .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList())
                 ;

                grid.VerticalSubDomain = string.Join(",", allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && m.SubdomainresponsiblesDic != null && x.Deleted == false)
            .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList());

                grid.EngineeringContactPoint = _commonManager.GetEngContactPointFromEduAndSubDomainSpoc(allEdu?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && x.Deleted == false).Select(t => t?.ContactEmail).ToList(),
                    allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && x.Deleted == false).Select(t => t?.ContactEmail).ToList());

                grid.OperationsContactPoint = string.Join(" | ",
                    allOperationalContract?.Where(m => m.LcmengineeringId == x.Lcmengineeringid).Select(x => x?.OperationDescription).Distinct());

                #endregion
                grid.AssetStatus = GetAssetStatusBasedOnOriginalHwAndSw(
                   x.Lcmancillarydata?.FirstOrDefault() != null ? x.Lcmancillarydata?.FirstOrDefault()?.Originalswlcmid
                    : string.Empty
                    );

                grid.AssetCategory = systemType?.Assetcategory?.Assetcategory;

                grid.AssetClass = majorSW.Productname != null ? majorSW.Productname?.Description : "";

                grid.AssetType = majorSW.Criticalassettype?.Description;

                grid.ProductImportance = x.Productimportance?.Productimportance;

                grid.Vendor = majorSW.Orgeqpmanufacturer
                    ?.Originalequipmentmanufacturer;

                var hw = systemType?.Systemtypesmajorhardwarebuilds
                   ?.SingleOrDefault(m =>
                       m.Ismain &&
                       m.Systemtypeid == systemType?.Systemtypeid &&
                       m.Deleted == false)?.Majorhardware;

                grid.HardwareModel = hw.Buildconstruction?.Rule == (int)BuildconstructionRuleEnum.VirtualHW ? ConstantValueFilter.Virtualized : systemType?.toLcmDbExportHardwareName();


                int numberOfNodesCount = x.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result;
                grid.NumberOfNodes = numberOfNodesCount;



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


                grid.TrackingNumberProjectName = x.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Software);
                grid.Notes = plannedActivitySW?.Notes;

                grid.SoftwareVersion = majorSW?.Softwareversion;



                grid.SystemTypeId = x.Designcomponent?.Systemtypeid ?? 0;

                grid.DesignComponentId = x.Designcomponentid;
                grid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
                grid.MajorHardwareBuildId = x.Designcomponent.Systemtype?.Systemtypesmajorhardwarebuilds
                    ?.SingleOrDefault(m =>
                        m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                        m.Deleted == false)?.Majorhardwareid;

                grid.AssetVirtualized = hw?.Buildconstruction?.Iscloudasset == true ? ConstantValueFilter.YES : ConstantValueFilter.NO;

                grid.BundleBudget = GetBundleBudget(plannedActivitySW?.Budgettrackingid);

                grid.BundleId = GetBundleBudget(plannedActivitySW?.Budgettrackingid) == ConstantValueFilter.yes ? plannedActivitySW.Budgettrackingid.Substring(2) : null;

                grid.AssetServiceFunctionality = dcSubnetwork?.Subnetworksupportedsvr?.Count() > 0 ? string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct()) : string.Empty;

                grid.Platform = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description : "";

                grid.OpsMaintenanceConractEnd = x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract;
                grid.OpsMaintenanceConractEndValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

                grid.VendorEndOfVulnerabilitySecuritySupportDateValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

                grid.EngRiskEvaluationNotes = plannedActivitySW
                  ?.Riskengineeringnotes;


                grid.EngRiskEvaluation = _commonManager.GetRiskValue(plannedActivitySW
                   ?.Engineeringrisk?.Description);

                grid.OpsRiskEvaluation = _commonManager.GetRiskValue(plannedActivitySW
                    ?.Operationalrisk?.Description);
                grid.OpsRiskEvaluationNotes = plannedActivitySW?.Riskoperationalnotes;


                grid.OverallRiskEvaluation = plannedActivitySW?.Overallriskevaluation;
                grid.PlannedActivityId = plannedActivitySW?.Plannedactivityid;

                grid.BudgetEstimated = x.PlannedactivitiesLcmengineering.GetPlannedActivityBudgetEstimated(ConstantValueFilter.Software);
                grid.ManagedByGdc = ConstantValueFilter.no;

                grid.DesignComponentFamilyId = x.Designcomponent?.Designcomponentfamilyid;
                grid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";

                grid.Archived = x.Archived;


                #region LCM-R8-Phase1

                grid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper);//systemType?.VodafonenameNavigation?.Description;
                grid.Criticality = dcSubnetwork?.Criticality;
                grid.GdprRelevant = dcSubnetwork.Gdprrelevant != null ? dcSubnetwork.Gdprrelevant == true ? ConstantValueFilter.YES : ConstantValueFilter.NO : null;
                grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngSoftware, grid.OperationsMaintenanceContract);
                grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus, grid.OperationsMaintenanceContract, grid.OpsMaintenanceConractEnd, grid.ProjectEndDate, grid.ProjectStatus);
                grid.IsExtendedSupportOfferedByVendor = x.Isextendedsupportofferedbyvendor.HasValue && x.Isextendedsupportofferedbyvendor.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO;

                var ipIdentities = x?.Networkelementsasplanned.SelectMany(p => p?.Identitiesasis).Where(p => p.Interfacetype == ConstantValueFilter.Management && p?.Category?.Description.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress);
                grid.IpAddress = ipIdentities != null ? string.Join(" | ", ipIdentities?.Select(fx => fx.Value).Distinct()) : string.Empty;

                grid.Hostname = x.Networkelementsasplanned != null ? string.Join(" | ", x.Networkelementsasplanned.Select(fx => fx.Elementname).Distinct()) : string.Empty;
                                             //var plannedActivityHWForRagStatus = x.PlannedactivitiesLcmengineering
                                             //                                    .GetPlannedActivityFilteredDB(ConstantValueFilter.Software, true).Plannedactivityresourceid == null ? null
                                             //                                    : x.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Software, true);

                #region Ticket 465 - LCM export: RAG status field - filter is not working fine
                grid.RagStatus = LcmEngineeringExtensionMethod.aggregatedRagStatusFilterRecord(null,
                  query.Where(y => y.Lcmengineeringid == x.Lcmengineeringid).ToList(), 2
                  ).FirstOrDefault().Text.ToString();
                //if (x.Archived == true)
                //{
                //    grid.RagStatus = LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3);
                //}
                //else if (grid.EOMStatus == EOMEnum.NotAnnounced)
                //{
                //    grid.RagStatus = string.Empty;
                //}
                //else if (grid.VendorEndOfMaintenanceDate > targetDate)
                //{
                //    grid.RagStatus = string.Empty;
                //}
                //else if (grid.VendorEndOfMaintenanceDate < fronzenDate)
                //{
                //    grid.RagStatus = plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status.ToString() != null ?
                //   LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status) : "";
                //}
                //else if (grid.VendorEndOfMaintenanceDate == null)
                //{
                //    grid.RagStatus = plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status.ToString() != null ?
                //    LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status) : "";
                //}
                //else if (fronzenDate <= grid.VendorEndOfMaintenanceDate || grid.VendorEndOfMaintenanceDate <= targetDate)
                //{
                //    grid.RagStatus = plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status.ToString() != null ?
                //    LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status) : "";
                //};
                //grid.RagStatus = grid.RagStatus  + "--" + gdRag +"-"+ x.Lcmengineeringid;
                #endregion
                grid.DeliveryPlanAvailable = plannedActivitySW?.Deliveryplanavailable != null && plannedActivitySW.Deliveryplanavailable ? ConstantValueFilter.YES : ConstantValueFilter.NO;

                grid.WbsCode = GetWbsCode(plannedActivitySW?.Deliveryprojectname);
                grid.BptID = plannedActivitySW?.Budgettrackingid;
                grid.PpmID = plannedActivitySW?.Deliveryprojectid;
                grid.MainOrganization = ConstantValueFilter.Nse;
                grid.SerialNumber = x.Resourcekey;


                #endregion

                #region LCM R9 part-1
                var lcmAuditAttributes = x.Lcmancillarydata.FirstOrDefault();
                grid.SecurityRiskPotential = x.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames.Where(x => x.Riskcluster != null).Select(x => x.Riskcluster?.Risklevel).FirstOrDefault();
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
                    grid.OccurrenceProbability = lcmAuditAttributes.Occurenceprobability;
                    grid.IncidentClass = lcmAuditAttributes.Incidentclass;
                    grid.NewopsRiskEvaluation = GetNewOpsRiskEvaluationValue(grid.IncidentClass, grid.OccurrenceProbability);

                    grid.ProductCode = lcmAuditAttributes.Productcode;
                    grid.HandedOverToOperation = lcmAuditAttributes.Handedovertooperation;
                    grid.ContractRenewalPlan = lcmAuditAttributes.Contractrenewalplan;
                    grid.DataSource = lcmAuditAttributes.Datasource;
                    grid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;
                    grid.OriginalSwLcmId = lcmAuditAttributes.Originalswlcmid;

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
                }
                grid.LabSWRelease = x.Numberofnodesinlab > 0 ?
                                    majorSW?.Softwareversion : "";

                grid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
                                              ? lcmAuditAttributes.Handedovertooperation : true;
                grid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
               ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;

                #endregion
                var Endofmaintenance = majorSW?.Endofmaintenance;
                grid.IdentifiedAction = x.Archived != true ? GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                  .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), Endofmaintenance, _repositoryWrapper) : "";


                grid.Program = x.PlannedactivitiesLcmengineering.AsQueryable()
                  .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                  ?.ProgramNavigation?.Programdescription;

                grid.ProjectOwner = x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Projectowner;

                var componentName = x.Buildbag?.Componentsoftwarebuildbags?
                    .Where(x1=>x1.Componentsoftwarebuild != null)
                    .SelectMany(y => new List<string> { ComponentBagExtensionMethod.GetComponentDescription(y?.Componentsoftwarebuild) })
                    .ToList();
                grid.Components = componentName != null && componentName.Count() > 0 ? string.Join(";", componentName.Select(z => z).Distinct()) : string.Empty;

                return grid;
            });

            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lastswupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
            }

            var rtn = new QueryResultDto<ReportSubnetWorkSoftWareGrid>(new GenerateRenderForGrid<ReportSubnetWorkSoftWareGrid>(_columnManager))
            {
                TotalItems = totalCount
            };
            rtn.Items = reports.ToArray();

            return rtn;
        }

        public IQueryable<Lcmengineering> GetQuery(ExpressionStarter<Lcmengineering> predicateResult)
        {
            var result = _repositoryWrapper.Lcmengineering.FindByCondition(predicateResult)
                                .Where(x =>
                                    x.Designcomponent.Systemtype.Deleted == false &&
                                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(s => s.Deleted == false))
                                .Include(x => x.Lcmdeploymentstatus)
                                .Include(x => x.Opco)
                                .Include(x => x.Networkelementsasplanned)
                                .Include(x => x.Productimportance)
                                .Include(x => x.Lcmancillarydata)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Criticalassettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetclass)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation).ThenInclude(x => x.Riskclustervodafonenames).ThenInclude(x => x.Riskcluster)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                                //.Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                                //.Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                                 .ThenInclude(x => x.Subnetworksupportedsvr).ThenInclude(x => x.Service)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Activitystatus)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Engineeringrisk)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Operationalrisk)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverytrackings)
                                .Include(x => x.Networkelementsasplanned).ThenInclude(x => x.Identitiesasis).ThenInclude(x => x.Category)
                                .Include(x => x.Buildbag).ThenInclude(x => x.Componentsoftwarebuildbags).ThenInclude(x=>x.Componentsoftwarebuild).ThenInclude(x=>x.Componentmanufacturer);

            return result;
        }


        public ExpressionStarter<Lcmengineering> ApplyFilter(ReportSubnetWorkQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);

            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);
            #region LCM R8 Phase 1 
            if (buildFilterDto?.Hostname != null && buildFilterDto.Hostname.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Hostname)
                    predicateInner.Or(x => x.Networkelementsasplanned.Any(d => d.Networkelementasplannedid == Convert.ToInt64(item)));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.BptID != null && buildFilterDto.BptID.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.BptID)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PpmID != null && buildFilterDto.PpmID.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.PpmID)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SerialNumber != null && buildFilterDto.SerialNumber.Any())
            {
                foreach (var item in buildFilterDto?.SerialNumber)
                    predicateInner.Or(x => x.Resourcekey == item);
                predicateResult.And(predicateInner);
            }
            //if (buildFilterDto?.RagStatus != null && buildFilterDto.RagStatus.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Lcmengineering>();
            //    foreach (var item in buildFilterDto?.RagStatus)
            //        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
            //           .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliverytrackings.FirstOrDefault().Ms2status.ToString() == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.Hostname != null && buildFilterDto.Hostname.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Lcmengineering>();
            //    foreach (var item in buildFilterDto?.Hostname)
            //        predicateInner.Or(x => x.Networkelementsasplanned.Any(d => d.Elementname == item));
            //    predicateResult.And(predicateInner);
            //}
            if (buildFilterDto?.IpAddress != null && buildFilterDto.IpAddress.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.IpAddress)
                    predicateInner.Or(x => x.Networkelementsasplanned.SelectMany(p => p.Identitiesasis).Any(p => p.Interfacetype == ConstantValueFilter.Management && p.Category.Description.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress && p.Value == item));
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

            if (buildFilterDto.GdprRelevant != null && buildFilterDto.GdprRelevant.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
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
            #endregion
            if (buildFilterDto?.TypeOfNetworkElement != null && buildFilterDto.TypeOfNetworkElement.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.TypeOfNetworkElement)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Lcmpolicy == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DeliveryPlanAvailable != null && buildFilterDto.DeliveryPlanAvailable.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.DeliveryPlanAvailable)
                {
                    if (item == ConstantValueFilter.YES)
                    {
                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmsoftware)
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmsoftware)
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == false);
                    }
                }
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
            if (buildFilterDto?.DesignComponentIndex != null && buildFilterDto.DesignComponentIndex.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.DesignComponentIndex)
                    predicateInner.Or(x => x.Designcomponentid.ToString() == item);
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
                        predicateInner.Or(x => x.Lcmengineeringsubdomainspoc != null && !x.Lcmengineeringsubdomainspoc.Any());
                    }
                    else
                    {
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
                     && x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString() == item));
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
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description == item : false);

                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.AssetType != null && buildFilterDto.AssetType.Any())
            {

                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.AssetType)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Criticalassettype != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Criticalassettype.Description == item : false);

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
                    predicateInner.Or(x => x.Productimportance.Productimportance == item);

                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Vendor != null && buildFilterDto.Vendor.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Vendor)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer == item);
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
            if (buildFilterDto?.OverallRiskEvaluation != null && buildFilterDto.OverallRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OverallRiskEvaluation)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.DescriptionOfPlannedAction != null && buildFilterDto.DescriptionOfPlannedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.DescriptionOfPlannedAction)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                     .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmsoftware)
                     .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PlannedSoftwareVersion != null && buildFilterDto.PlannedSoftwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.PlannedSoftwareVersion)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                     .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmsoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Activitydetails == item || item == ConstantValueFilter.Na && x.PlannedactivitiesLcmengineering.AsQueryable()
                     .Where(a => a.Plannedactivityresourceid != null && a.Plannedactivityresource.Exportable && a.Deleted == false && a.Plannedactivityresource.Lcmsoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Activitydetails == null);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.ProjectStatus != null && buildFilterDto.ProjectStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.ProjectStatus)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProgramLcm != null && buildFilterDto.ProgramLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.ProgramLcm)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectOwner != null && buildFilterDto.ProjectOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.ProjectOwner)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectowner == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.TrackingNumberProjectNameLcm != null && buildFilterDto.TrackingNumberProjectNameLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.TrackingNumberProjectNameLcm)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Notes != null && buildFilterDto.Notes.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Notes)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.VendorEndOfMaintenanceDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto?.VendorEndOfMaintenanceDateValue.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance >= buildFilterDto.VendorEndOfMaintenanceDateValue.StartDate);
                if (buildFilterDto?.VendorEndOfMaintenanceDateValue.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= buildFilterDto.VendorEndOfMaintenanceDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ExtendedSupportOptionOfferedByVendor != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto?.VendorEndOfMaintenanceDateValue.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Lasttimebuyexpansions >= buildFilterDto.VendorEndOfMaintenanceDateValue.StartDate);
                if (buildFilterDto?.VendorEndOfMaintenanceDateValue.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Lasttimebuyexpansions <= buildFilterDto.VendorEndOfMaintenanceDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectEndDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto?.ProjectEndDateValue.StartDate != null)
                    predicateInner.And(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion >= buildFilterDto.ProjectEndDateValue.StartDate);
                if (buildFilterDto?.ProjectEndDateValue.EndDate != null)
                    predicateInner.And(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion <= buildFilterDto.ProjectEndDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.VendorEndOfVulnerabilitySecuritySupportDateValueLcm != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDateValueLcm.StartDate != null)
                    predicateInner.And(x => (x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract) >= buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDateValueLcm.StartDate);

                if (buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDateValueLcm.EndDate != null)
                    predicateInner.And(x => (x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract) <= buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDateValueLcm.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.AssetVirtualized != null && buildFilterDto.AssetVirtualized.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.AssetVirtualized)
                {
                    if (item == ConstantValueFilter.YES)
                    {
                        predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Buildconstruction.Iscloudasset == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                            m.Deleted == false).Majorhardware.Buildconstruction.Iscloudasset == false);
                    }
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SoftwareVersion != null && buildFilterDto.SoftwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.SoftwareVersion)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OpsMaintenanceConractEndValueLcm != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.OpsMaintenanceConractEndValueLcm.StartDate != null)
                    predicateInner.And(x => (x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract) >= buildFilterDto.OpsMaintenanceConractEndValueLcm.StartDate);

                if (buildFilterDto.OpsMaintenanceConractEndValueLcm.EndDate != null)
                    predicateInner.And(x => (x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract) <= buildFilterDto.OpsMaintenanceConractEndValueLcm.EndDate);
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
                         .Where(PlannedActivitySoftware)
                         .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgetvalue == decimalValue);
                    }
                    else
                    {
                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivitySoftware)
                            .OrderBy(x => x.Plannedcompletion).Select(s => new { s.Budgetvalue, s.Currency }).AsQueryable().Select(x => x.Budgetvalue.ToString() + x.Currency).FirstOrDefault() == item);
                    }
                }

                predicateResult.And(predicateInner);

            }

            if (buildFilterDto?.BundleId != null && buildFilterDto.BundleId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.BundleId)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid.Substring(2) == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Platform != null && buildFilterDto.Platform.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Platform)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null && x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.EngRiskEvaluation != null && buildFilterDto.EngRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.EngRiskEvaluation)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngRiskEvaluationNotes != null && buildFilterDto.EngRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.EngRiskEvaluationNotes)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluation != null && buildFilterDto.OpsRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluation)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluationNotes != null && buildFilterDto.OpsRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluationNotes)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OverallRiskEvaluation != null && buildFilterDto.OverallRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OverallRiskEvaluation)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.LcmStatus != null && buildFilterDto.LcmStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.LcmStatus)
                    predicateInner.Or(x => x.Lcmstatussoftware == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.IsExtendedSupportOfferedByVendor != null && buildFilterDto.IsExtendedSupportOfferedByVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.IsExtendedSupportOfferedByVendor)
                {
                    if (item.ToLower() == ConstantValueFilter.Yes)
                    {
                        predicateInner.Or(x => x.Isextendedsupportofferedbyvendor == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Isextendedsupportofferedbyvendor == false || !x.Isextendedsupportofferedbyvendor.HasValue);
                    }
                }
                predicateResult.And(predicateInner);
            }

            #region LCM R9 Part - 1
            //if (buildFilterDto?.Custom != null && buildFilterDto.Custom.Any())
            //{
            //    foreach (var item in buildFilterDto?.Custom)
            //        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Custom == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.Custom1 != null && buildFilterDto.Custom1.Any())
            //{
            //    foreach (var item in buildFilterDto?.Custom1)
            //        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Custom1 == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.Custom2 != null && buildFilterDto.Custom2.Any())
            //{
            //    foreach (var item in buildFilterDto?.Custom2)
            //        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Custom2 == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.KpiStatusService != null && buildFilterDto.KpiStatusService.Any())
            //{
            //    foreach (var item in buildFilterDto?.KpiStatusService)
            //        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Kpistatusservice == item);
            //    predicateResult.And(predicateInner);
            //}
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
            if (buildFilterDto?.SecurityMitigation != null && buildFilterDto.SecurityMitigation.Any())
            {
                foreach (var item in buildFilterDto?.SecurityMitigation)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Securitymitigation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SecurityRiskOverallLcm != null && buildFilterDto.SecurityRiskOverallLcm.Any())
            {
                var entitieswithdata = _repositoryWrapper.Lcmengineering.FindAll().Include(x => x.Lcmancillarydata)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation).ThenInclude(x => x.Riskclustervodafonenames).ThenInclude(x => x.Riskcluster).ToList();
                var filteredEntities = entitieswithdata.Where(entity =>
                {
                    foreach (var item in buildFilterDto.SecurityRiskOverallLcm)
                    {
                        if (entity.Lcmancillarydata.Any(y =>
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
            if (buildFilterDto?.EngUpdateTracker != null && buildFilterDto.EngUpdateTracker.Any())
            {

                foreach (var item in buildFilterDto?.EngUpdateTracker)
                {
                    if (item.ToLower() == _engUpdateTracker.ToLower())
                    {
                        predicateInner.Or(x => !x.Lcmancillarydata.Any());
                        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Engupdatetracker == item);
                    }

                    else
                        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Engupdatetracker == item);
                }

                predicateResult.And(predicateInner);
            }
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
                        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Handedovertooperation == item);
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
            //if (buildFilterDto?.CloudVersion != null && buildFilterDto.CloudVersion.Any())
            //{
            //    foreach (var item in buildFilterDto?.CloudVersion)
            //        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Cloudversion == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto?.CertifiedSWReleaseforNFVIbundle != null && buildFilterDto.CertifiedSWReleaseforNFVIbundle.Any())
            //{
            //    foreach (var item in buildFilterDto?.CertifiedSWReleaseforNFVIbundle)
            //        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Certifiedswrealesefornfvibundle == item);
            //    predicateResult.And(predicateInner);
            //}
            if (buildFilterDto?.LabSWRelease != null && buildFilterDto.LabSWRelease.Any())
            {
                foreach (var item in buildFilterDto?.LabSWRelease)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OriginalSwLcmId != null && buildFilterDto.OriginalSwLcmId.Any())
            {
                foreach (var item in buildFilterDto?.OriginalSwLcmId)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Originalswlcmid == item);
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
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Externalfacingflag.Value == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.InfrastructureLocation != null && buildFilterDto.InfrastructureLocation.Any())
            {
                foreach (var item in buildFilterDto?.InfrastructureLocation)
                    predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Locationinfrastructure == item);
                predicateResult.And(predicateInner);
            }
            #endregion

            if (buildFilterDto?.DesignComponentFamily != null && buildFilterDto.DesignComponentFamily.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.DesignComponentFamily)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.SupportedService != null && buildFilterDto.SupportedService.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.SupportedService)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Any(m => m.Serviceid == item));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Components != null && buildFilterDto.Components.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Components)
                    predicateInner.Or(x => x.Buildbag.Componentsoftwarebuildbags.Any(y => y.Componentsoftwarebuildid == Convert.ToInt64(item)));
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public Dictionary<string, Expression<Func<Lcmengineering, object>>[]> GetColumnsMapDB()
        {
            return new Dictionary<string, Expression<Func<Lcmengineering, object>>[]>
            {
                ["plannedAction"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Onhardware, p => p.Onsoftware },

                ["softwareSheetIndex"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Softwaresheetindex },
                ["localMarket"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Opco.Opco },
                ["assetCategory"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Assetcategory.Assetcategory },
                ["assetClass"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.toAssetClassDescription(_repositoryWrapper) },
                ["assetType"] = new Expression<Func<Lcmengineering, object>>[] { x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype.Assettype.Assettype : x.Designcomponent.Systemtype.VodafonenameNavigation != null ? x.Designcomponent.Systemtype.VodafonenameNavigation.Description : "" }, // .AssetTypeIdNavigation.AssetTypeDescription },
                ["assetDescription"] = new Expression<Func<Lcmengineering, object>>[] { p => !string.IsNullOrEmpty(p.Designcomponent.Designcomponentfamily.Description) ? p.Designcomponent.Subnetworkboundary.Alias : p.Designcomponent.Designcomponentfamily.Description },
                ["productImportance"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Productimportance.Productimportance },
                ["vendor"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer },
                ["hardwareModel"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Single(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Platform, p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Single(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Hardwaretype },
                ["numberOfNodes"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Numberofnodes },
                ["operationsMaintenanceContract"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Outputtolcmsoftware },
                ["opsMaintenanceConractEnd"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Softwareendofsupportcontract },
                ["vendorEndOfMaintenanceDate"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance },
                ["lcmStatus"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Constraintlcm },

                ["descriptionOfPlannedAction"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource },

                ["plannedSoftwareVersion"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Activitydetails },
                ["projectStatus"] = new Expression<Func<Lcmengineering, object>>[] { x =>x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().GetPlannedActivityProjectStatus(_repositoryWrapper).Projectstatus },
                ["projectEndDate"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion },

                ["trackingNumberProjectName"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname },

                ["notes"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes },


                ["assetVirtualized"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.VodafonenameNavigation != null ? p.Designcomponent.Systemtype.VodafonenameNavigation.Description : "" },

                ["SoftwareVersion"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion },
                ["cloudVersion"] = new Expression<Func<Lcmengineering, object>>[] { p => "cloud version" },
                ["bundleBudget"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgetvalue,p=>p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Currency },

                ["bundleId"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid },

                ["assetServiceFunctionality"] = new Expression<Func<Lcmengineering, object>>[] { p => string.IsNullOrEmpty(p.Designcomponent.Subnetworkboundary.Alias) ? p.Designcomponent.Subnetworkboundary.Description : p.Designcomponent.Subnetworkboundary.Alias },

                ["platform"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Assetclass.Assetclass },
                ["lCMStatusENG"] = new Expression<Func<Lcmengineering, object>>[] { p => "cloud version" },
                ["lCMStatusOPS"] = new Expression<Func<Lcmengineering, object>>[] { p => "cloud version" },
                ["engRiskEvaluation"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description },
                ["engRiskEvaluationNotes"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes },
                ["oPSRiskEvaluation"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description },
                ["oPSRiskEvaluationNotes"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes },

                ["VendorEndOfVulnerabilitySecuritySupportDate"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport },
                ["overallRiskEvaluation"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation },

                ["outputToLcmSoftware"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmstatussoftware },
                ["lcmStatusEngSoftware"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmstatusengsoftware },
                ["lcmStatusOpsSoftware"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmstatusopssoftware },
                ["isExtendedSupportofferedByVendor"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Isextendedsupportofferedbyvendor },
                ["deliveryPlanAvailable"] = new Expression<Func<Lcmengineering, object>>[] { p => p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivitySoftware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion },

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
                //["securityRiskOverall "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Securitymitigation },
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
                ["newopsRiskEvaluation "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Occurenceprobability },
                ["occurrenceProbability "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Occurenceprobability },
                ["incidentClass "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Incidentclass },
                ["productCode "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Productcode },
                ["handedOverToOperation "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Handedovertooperation },
                ["contractRenewalPlan "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Contractrenewalplan },
                ["dataSource "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Datasource },
                ["scopeOfSimplification "] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Scopeofsimplification },
                //["cloudVersion"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Cloudversion },
                //["certifiedSWReleaseforNFVIbundle"] = new Expression<Func<Lcmengineering, object>>[] { p => p.Lcmancillarydata.FirstOrDefault().Certifiedswrealesefornfvibundle },
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

        #endregion


        #region AggregatedHardware
        public List<FilterValueDto> GetAggregatedHardWareFilter(string propertyName, string propertyFilter,
        ReportSubnetWorkQueryDto buildFilterDto,bool isAdmin=false)
        {

            ExpressionStarter<Lcmengineering> predicateResult = ApplyAggregatedHardWareFilter(buildFilterDto);
            var query = GetHardWareQuery(predicateResult);

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
                    .Where(PlannedActivityHardware)
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectname))).Distinct().ToList(),
                "bptID" => query
                .Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivityHardware)
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Projectstatus != null ?
                 p.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivityHardware)
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Budgettrackingid : string.Empty)).Distinct().ToList(),
                "ppmID" => query
                    .Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivityHardware)
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Projectstatus != null ?
                     p.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivityHardware)
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
                                                  p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                  .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion,
                                                   p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
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
                "productImportance" => _repositoryWrapper.ProductImportance.FindAll().Select(x => new FilterValueDto(x.Productimportance)).ToList(),
                "vendor" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid &&
                        m.Deleted == false && m.Deletiondate == null).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer)).Distinct().ToList(),
                "hardwareModel" => query.AsEnumerable().Select(x => new FilterValueDto(x.Designcomponent.Systemtype.toLcmDbExportHardwareName())).Distinct().ToList(),
                "numberOfNodes" => query.Select(p => new FilterValueDto(p.Numberofnodes.ToString())).Distinct().ToList(),
                "plannedAction" => new List<FilterValueDto>() { new FilterValueDto() { Text = ConstantValueFilter.OnHardware, Value = "1" }, new FilterValueDto() { Text = ConstantValueFilter.OnSoftware, Value = "2" } },
                "descriptionOfPlannedAction" => query.Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable()
                                .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                                 .Plannedactivityresource.Plannedactivityresource)).Distinct().ToList(),
                "plannedHardwareModel" => query.Where(p => p.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                        .Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.HardwareUpgrade).ToList()
                       .Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Designcomponent?.Systemtype?.toLcmDbExportHardwareName())).Distinct().ToList(),

                "trackingNumberProjectName" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname)).Distinct().ToList(),
                "notes" => query.Select(p => new FilterValueDto(p.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes)).Distinct().ToList(),

                #region Ticket 465 - LCM export: RAG status field - filter is not working fine

                "ragStatus" => LcmEngineeringExtensionMethod.aggregatedRagStatusFilterRecord(query, null, 1).Distinct().ToList(),
                #endregion

                "operationsContactPoint" => query.SelectMany(x => x.Lcmoperationalcontracts)
                                .Select(p => new FilterValueDto(p.Operationalcontract.Description)).Distinct().ToList(),
                "identifiedAction" => query.Select(x => new FilterValueDto(GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                                .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofmaintenance, _repositoryWrapper))).ToList().Distinct().ToList(),
                "softwareRelease" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion)).Distinct().ToList(),
                "budgetEstimated" => query.Select(s => s.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                          .FirstOrDefault())?.AsEnumerable()
                                          ?.Select(p => new FilterValueDto(p?.Currency == null ? p?.Budgetvalue?.ToString() : p?.Budgetvalue?.ToString() + p?.Currency))?.Distinct()?.ToList(),
                "bundleBudget" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No) },
                "bundleId" => query.Where(x => GetBundleBudget(x.PlannedactivitiesLcmengineering.AsQueryable()
                             .Where(PlannedActivityHardware)
                             .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid) == ConstantValueFilter.Yes)
                                .Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                             .Where(PlannedActivityHardware)
                             .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid.Substring(2))).Distinct().ToList(),
                "assetServiceFunctionality" => query.Select(p => new FilterValueDto(string.Join(" | ", p.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Select(x => x.Service.Description)))).ToList().Distinct().ToList(),
                "platform" => query.Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m => m.Ismain && m.Deleted == false).Majorhardware.Hardwaresolution)).Distinct().ToList(),
                //LCM R8 Phase 1  
                "engRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = _commonManager.GetRiskValue(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description),
                    Value = x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid.ToString()
                }).Distinct().ToList(),
                "engRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes)).Distinct().ToList(),
                //LCM R8 Phase 1  
                "opsRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = _commonManager.GetRiskValue(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description),
                    Value = x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid.ToString()
                }).Distinct().ToList(),
                "opsRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "overallRiskEvaluation" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "outputToLcmSoftware" => query.ToList().Select(p => new FilterValueDto(p.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatus).Result)).Distinct().ToList(),
                "operationsMaintenanceContract" => query.ToList().Select(p => new FilterValueDto(p.Outputtolcmhardware)).Distinct().ToList(),
                "typeOfNetworkElement" => query.Where(x => x.Designcomponent.Subnetworkboundary.Lcmpolicy != null)
                                                        .Select(x => new FilterValueDto
                                                        {
                                                            Text = ((LCMPolicy)x.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString(),
                                                            Value = x.Designcomponent.Subnetworkboundary.Lcmpolicy.ToString()
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

                "program" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                                   .Where(PlannedActivityHardware)
                           .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription)).ToList().Distinct().ToList(),
                "projectOwner" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                       .Where(PlannedActivityHardware)
                         .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectowner)).ToList().Distinct().ToList(),
                "projectStatus" => query.Select(x => new FilterValueDto(x.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivityHardware)
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
                "requestID" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Requestid)).Distinct().ToList(),
                //"id_New" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Idnew)).Distinct().ToList(),
                //"productImportanceHistory2" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Productimportancehistory2)).Distinct().ToList(),
                //"lcmStatusJune2021" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Lcmstatus)).Distinct().ToList(),
                "lastScanDate" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Lastscandate)).Distinct().ToList(),
                "assetOutofScopeForReportingPurposes" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Assetoutofscope)).Distinct().ToList(),
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
                "dataSource" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Datasource)).Distinct().ToList(),
                "scopeOfSimplification" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Scopeofsimplification)).Distinct().ToList(),
                //"cloudVersion" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault().Cloudversion)).Distinct().ToList(),
                //"certifiedSWReleaseforNFVIbundle" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault().Certifiedswrealesefornfvibundle)).Distinct().ToList(),
                "labSWRelease" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Productcode)).Distinct().ToList(),
                "originalHwLcmId" => query.ToList().Select(p => new FilterValueDto(p.Lcmancillarydata.FirstOrDefault()?.Originalhwlcmid)).Distinct().ToList(),

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

                //"externalFacingFlag" => (query.Where(x => x.Lcmancillarydata.Count() >0).ToList()
                //.Select(x => x.Lcmancillarydata.Where(x => x.Externalfacingflag != null) )
                //.Select(p => new FilterValueDto
                //{
                //    Text = p.FirstOrDefault()?.Externalfacingflag == true ? "Yes" : "No",
                //    Value =  p.FirstOrDefault()?.Externalfacingflag.ToString()
                //})).Distinct().ToList(),


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
                                ? query.SelectMany(x => x.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuseropcosUser.Any(r => r.Deleted == false && r.Opcoid == x.Opcoid))
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

                "designComponentFamily" => query.ToList().Select(p => new FilterValueDto
                {
                    Text = p.Designcomponent.toDesignComponentFamily(),
                    Value = p.Designcomponent.Designcomponentfamilyid.ToString()
                }).Distinct().ToList(),
                "supportedService" => query.ToList().Select(p => new FilterValueDto
                {
                    Text = p.Designcomponent?.Subnetworkboundary?.Subnetworksupportedsvr?.Select(x => x.Service?.Description).FirstOrDefault(),
                    Value = p.Designcomponent?.Subnetworkboundary?.Subnetworksupportedsvr?.Select(x => x.Serviceid).FirstOrDefault().ToString()
                }).Distinct().ToList(),

                "hostname" => query.ToList().Where(x => x.Networkelementsasplanned != null)
                            .SelectMany(p => p.Networkelementsasplanned).Where(y => y.Elementname != null)
                            .Select(y => new FilterValueDto { Text = y.Elementname, Value = Convert.ToString(y.Networkelementasplannedid) }).Distinct().ToList(),

                "components" => string.IsNullOrEmpty(propertyFilter) ?
                                query.ToList().Where(a => a.Buildbag?.Componentsoftwarebuildbags != null)
                                .SelectMany(p => p.Buildbag?.Componentsoftwarebuildbags?
                                .Select(y => new FilterValueDto
                                {
                                    Text = ComponentBagExtensionMethod.GetComponentDescription(y?.Componentsoftwarebuild),
                                    Value = y?.Componentsoftwarebuildid.ToString()
                                })).Distinct().ToList()
                                : query.ToList()
                                .Where(a => a.Buildbag?.Componentsoftwarebuildbags != null && a.Buildbag.Componentsoftwarebuildbags.
                                Any(a1 => a1.Componentsoftwarebuildid.ToString().Contains(propertyFilter)))
                                .SelectMany(p => p.Buildbag?.Componentsoftwarebuildbags?
                                .Select(y => new FilterValueDto
                                {
                                    Text = ComponentBagExtensionMethod.GetComponentDescription(y?.Componentsoftwarebuild),
                                    Value = y?.Componentsoftwarebuildid.ToString()
                                })).Distinct().ToList(),

                _ => new List<FilterValueDto>(),
                
            };
            if (!isAdmin && (buildFilterDto.VerticalEngineeringTeam != null && buildFilterDto.VerticalEngineeringTeam.Count > 0) && propertyName == "verticalEngineeringTeam")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalEngineeringTeam.Contains(x.Value.ToString())).ToList();
            }
            return rtn;
        }
        public QueryResultDto<ReportSubnetWorkHardWareGrid> FindAggregatedHardWareWithCondition(ReportSubnetWorkQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Lcmengineering> predicateResult = ApplyAggregatedHardWareFilter(buildFilterDto);


            var result = GetHardWareQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetColumnsMapDB(), ConstantValueFilter.Modificationdate);

            IEnumerable<Lcmengineering> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();
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
                                                            x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Plannedcompletion,
                                                            x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Projectstatus
                                                            )));
            }

            if (buildFilterDto?.IdentifiedAction != null && buildFilterDto.IdentifiedAction.Any())
            {
                query = query.Where(x => x.Archived != true && buildFilterDto.IdentifiedAction.Contains(GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofmaintenance, _repositoryWrapper)));
            }

            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
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
                                 .Contains(string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Subnetworkboundary?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description)?.Distinct())));
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
            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != null).Select(x => x.Lcmengineeringid)?.Distinct()?.ToList();

            var lcmIdAndLcmOpcoId = query?.ToList()?.Where(x => x.Lcmengineeringid != null)?.DistinctBy(x => x?.Lcmengineeringid)
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
                var grid = new ReportSubnetWorkHardWareGrid();
                var dcSubnetwork = x.Designcomponent?.Subnetworkboundary;

                var systemType = x.Designcomponent?.Systemtype;
                var hardWare = systemType?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware;
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


                grid.DesignComponentFamily = x.Designcomponent?.toDesignComponentFamily();
                grid.SupportedService = string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct() ?? new List<string>());


                #region daily updated report

                grid.OperationsMaintenanceContract = x.Outputtolcmhardware;
                grid.LcmStatus = x.Lcmstatushardware;
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

                grid.EngineeringContactPoint = _commonManager.GetEngContactPointFromEduAndSubDomainSpoc(allEdu?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && x.Deleted == false ).Select(t => t?.ContactEmail).ToList(),
                                allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && x.Deleted == false).Select(t => t?.ContactEmail).ToList());

                grid.OperationsContactPoint = string.Join(" | ",
                    allOperationalContract?.Where(m => m.LcmengineeringId == x.Lcmengineeringid).Select(x => x?.OperationDescription).Distinct());

                #endregion
                grid.AssetCategory = systemType?.Assetcategory?.Assetcategory;

                grid.AssetClass = hardWare?.Hardwaresolution;

                grid.AssetType = _commonManager.GetAssetType(hardWare?.Buildconstruction?.Buildconstruction);


                grid.ProductImportance = x.Productimportance?.Productimportance;
                grid.Vendor = hardWare?.Orgeqpmanufacturer
                    ?.Originalequipmentmanufacturer;
                grid.HardwareModel = systemType?.toLcmDbExportHardwareName();
                grid.NumberOfNodes = x.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result;

                grid.EOMStatus = hardWare != null
                           ? (EOMEnum)hardWare.Eomstatus : EOMEnum.NotSpecified;

                grid.VendorEndOfMaintenanceDate = hardWare?.Endofmaintenance;

                grid.VendorEndOfMaintenanceDateValue = hardWare?.Endofmaintenance != null ? hardWare?.Endofmaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : grid.EOMStatus == EOMEnum.NotAnnounced ? ConstantValueFilter.NotAnnounced : null;

                grid.TrackingNumberProjectName = x.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());
                grid.Notes = plannedActivityHW?.Notes;
                grid.SystemTypeId = x.Designcomponent.Systemtypeid;
                grid.DesignComponentId = x.Designcomponentid;
                grid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
                grid.MajorHardwareBuildId = hardWare?.Majorhardwareid;
                grid.BundleBudget = GetBundleBudget(plannedActivityHW?.Budgettrackingid);
                grid.BundleId = GetBundleBudget(plannedActivityHW?.Budgettrackingid) == ConstantValueFilter.Yes ? plannedActivityHW.Budgettrackingid.Substring(2) : null;
                grid.AssetServiceFunctionality = dcSubnetwork.Subnetworksupportedsvr?.Count() > 0 ? string.Join(" | ", dcSubnetwork.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct()) : string.Empty;
                grid.Platform = hardWare?.Hardwaresolution;

                grid.OpsMaintenanceConractEnd = x.Hardwareendofsupportcontract != null ? x.Hardwareendofsupportcontract : x.Softwareendofwarrantydate;
                grid.OpsMaintenanceConractEndValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

                grid.OpsRiskEvaluation = _commonManager.GetRiskValue(plannedActivityHW
                    ?.Operationalrisk?.Description);

                grid.OverallRiskEvaluation = plannedActivityHW
                    ?.Overallriskevaluation;

                grid.EngRiskEvaluationNotes = plannedActivityHW
                    ?.Riskengineeringnotes;

                grid.EngRiskEvaluation = _commonManager.GetRiskValue(plannedActivityHW
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
                grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngHardware, grid.OperationsMaintenanceContract);
                grid.HwIsExtendedSupportOfferedByVendor = x.Hwisextendedsupportofferedbyvendor.HasValue && x.Hwisextendedsupportofferedbyvendor.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();
                grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus, grid.OperationsMaintenanceContract, grid.OpsMaintenanceConractEnd, grid.ProjectEndDate, grid.ProjectStatus);




                var ipIdentities = x.Networkelementsasplanned?.SelectMany(p => p?.Identitiesasis).Where(p => p.Interfacetype == ConstantValueFilter.Yes.ToUpper() && p?.Category?.Description?.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress);
                grid.IpAddress = ipIdentities != null ? string.Join(" | ", ipIdentities.Select(fx => fx.Value).Distinct()) : string.Empty;

                grid.Hostname = x.Networkelementsasplanned != null ? string.Join(" | ", x.Networkelementsasplanned.Select(fx => fx.Elementname).Distinct()) : string.Empty;

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
                    grid.ContractRenewalPlan = lcmAuditAttributes.Contractrenewalplan;
                    grid.DataSource = lcmAuditAttributes.Datasource;
                    grid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;
                    grid.OriginalHwLcmId = lcmAuditAttributes.Originalhwlcmid;
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
                }
                #region Apr 03 requirement
                grid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
? lcmAuditAttributes.Handedovertooperation : true;
                grid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
               ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;
                #endregion

                #endregion

                grid.IdentifiedAction = x.Archived != true ? GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), hardWare.Endofmaintenance, _repositoryWrapper) : "";

                grid.Program = x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.ProgramNavigation?.Programdescription;

                grid.ProjectOwner = x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Projectowner;

                var componentName = x.Buildbag?.Componentsoftwarebuildbags?
                                    .Where(x1 => x1.Componentsoftwarebuild != null)
                                    .SelectMany(y => new List<string> { ComponentBagExtensionMethod.GetComponentDescription(y?.Componentsoftwarebuild) })
                                    .ToList();
                grid.Components = componentName != null && componentName.Count()>0? string.Join(" ; ", componentName.Select(z=>z).Distinct()) : string.Empty;

                return grid;
            }).ToList();

            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lasthwupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
            }


            var rtn = new QueryResultDto<ReportSubnetWorkHardWareGrid>(new GenerateRenderForGrid<ReportSubnetWorkHardWareGrid>(_columnManager))
            {
                TotalItems = totalCount
            };

            rtn.Items = reports.ToArray();

            return rtn;
        }

        public IQueryable<Lcmengineering> GetHardWareQuery(ExpressionStarter<Lcmengineering> predicateResult)
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
                                .Include(x => x.Lcmancillarydata)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation).ThenInclude(x => x.Riskclustervodafonenames).ThenInclude(x => x.Riskcluster)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetclass)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetworksupportedsvr).ThenInclude(x => x.Service)
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
                                .Include(x => x.Buildbag).ThenInclude(x => x.Componentsoftwarebuildbags).ThenInclude(x=>x.Componentsoftwarebuild).ThenInclude(x=>x.Componentmanufacturer);
        }

        public ExpressionStarter<Lcmengineering> ApplyAggregatedHardWareFilter(ReportSubnetWorkQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);

            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);
            if (buildFilterDto?.Hostname != null && buildFilterDto.Hostname.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Hostname)
                    predicateInner.Or(x => x.Networkelementsasplanned.Any(d=>d.Networkelementasplannedid== Convert.ToInt64(item)));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.BptID != null && buildFilterDto.BptID.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.BptID)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PpmID != null && buildFilterDto.PpmID.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.PpmID)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
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
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectOwner != null && buildFilterDto.ProjectOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.ProjectOwner)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
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
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Lcmpolicy == item);
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
                    && x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.Subdomainresponsible.Subdomainresponsibleid.ToString() == item));
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

            if (buildFilterDto?.ProductImportance != null && buildFilterDto.ProductImportance.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.ProductImportance)
                    predicateInner.Or(x => x.Productimportance.Productimportance == item);


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
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.TrackingNumberProjectNameLcm != null && buildFilterDto.TrackingNumberProjectNameLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.TrackingNumberProjectNameLcm)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Notes != null && buildFilterDto.Notes.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Notes)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
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
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion >= buildFilterDto.ProjectEndDateValue.StartDate);
                if (buildFilterDto?.ProjectEndDateValue.EndDate != null)
                    predicateInner.And(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
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
                         .Where(PlannedActivityHardware)
                         .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgetvalue == decimalValue);
                    }
                    else
                    {
                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivityHardware)
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
                        .Where(PlannedActivityHardware)
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
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngRiskEvaluationNotes != null && buildFilterDto.EngRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.EngRiskEvaluationNotes)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluation != null && buildFilterDto.OpsRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluation)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluationNotes != null && buildFilterDto.OpsRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluationNotes)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OverallRiskEvaluation != null && buildFilterDto.OverallRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.OverallRiskEvaluation)
                    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
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
                       .Where(PlannedActivityHardware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
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
            if (buildFilterDto?.EngUpdateTracker != null && buildFilterDto.EngUpdateTracker.Any())
            {
                foreach (var item in buildFilterDto?.EngUpdateTracker)
                {
                    if (item.ToLower() == _engUpdateTracker.ToLower())
                    {
                        predicateInner.Or(x => !x.Lcmancillarydata.Any());
                        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Engupdatetracker == item);
                    }

                    else
                        predicateInner.Or(x => x.Lcmancillarydata.FirstOrDefault().Engupdatetracker == item);
                }

                predicateResult.And(predicateInner);
            }
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

            if (buildFilterDto?.DesignComponentFamily != null && buildFilterDto.DesignComponentFamily.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.DesignComponentFamily)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.SupportedService != null && buildFilterDto.SupportedService.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.SupportedService)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Any(m => m.Serviceid == item));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Components != null && buildFilterDto.Components.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto?.Components)
                    predicateInner.Or(x => x.Buildbag.Componentsoftwarebuildbags.Any(y => y.Componentsoftwarebuildid == Convert.ToInt64(item)));
               predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        #endregion

        #region Async Operations
        public async Task<QueryResultDto<ReportSubnetWorkHardWareGrid>> FindAggregatedHardWareWithConditionAsync(ReportSubnetWorkQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Lcmengineering> predicateResult = ApplyAggregatedHardWareFilter(buildFilterDto);


            var result = GetHardWareQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetColumnsMapDB(), ConstantValueFilter.Modificationdate);

            IEnumerable<Lcmengineering> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();
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
                                                            x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Plannedcompletion,
                                                            x.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Projectstatus
                                                            )));
            }

            if (buildFilterDto?.IdentifiedAction != null && buildFilterDto.IdentifiedAction.Any())
            {
                query = query.Where(x => x.Archived != true && buildFilterDto.IdentifiedAction.Contains(GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofmaintenance, _repositoryWrapper)));
            }

            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
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
                                 .Contains(string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Subnetworkboundary?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description)?.Distinct())));
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
            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != null).Select(x => x.Lcmengineeringid)?.Distinct()?.ToList();

            var lcmIdAndLcmOpcoId = query?.ToList()?.Where(x => x.Lcmengineeringid != null)?.DistinctBy(x => x?.Lcmengineeringid)
                .ToDictionary(x => x.Lcmengineeringid, x => (long)x.Opcoid);

            var allSubDomain = _commonManager.GetCalculatedLcmSubDomainSpocEntityForReport(lcmIdAndLcmOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedLcmEduSpocEntityForReport(lcmIdAndLcmOpcoId).ToList();
            var allOperationalContract = _commonManager.GetCalculateLCMOperationalContractsForReport(lcmengineeringsEntityId);

            #endregion

            var reportLastUpdateDate = await _repositoryWrapper.LcmDBExportUpdateHistory.FindAll().FirstOrDefaultAsync();
            bool isLcmDBExportUpdated = false;
            if (reportLastUpdateDate != null && reportLastUpdateDate.Lasthwupdatedate.Date == DateTime.Now.Date)
            {
                isLcmDBExportUpdated = true;
            }

            var reports = data.Select(x =>
            {
                var grid = new ReportSubnetWorkHardWareGrid();
                var dcSubnetwork = x.Designcomponent?.Subnetworkboundary;

                var systemType = x.Designcomponent?.Systemtype;
                var hardWare = systemType?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware;
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


                grid.DesignComponentFamily = x.Designcomponent?.toDesignComponentFamily();
                grid.SupportedService = string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct() ?? new List<string>());


                #region daily updated report

                grid.OperationsMaintenanceContract = x.Outputtolcmhardware;
                grid.LcmStatus = x.Lcmstatushardware;
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

                grid.AssetType = _commonManager.GetAssetType(hardWare?.Buildconstruction?.Buildconstruction);


                grid.ProductImportance = x.Productimportance?.Productimportance;
                grid.Vendor = hardWare?.Orgeqpmanufacturer
                    ?.Originalequipmentmanufacturer;
                grid.HardwareModel = systemType?.toLcmDbExportHardwareName();
                grid.NumberOfNodes = x.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result;

                grid.EOMStatus = hardWare != null
                           ? (EOMEnum)hardWare.Eomstatus : EOMEnum.NotSpecified;

                grid.VendorEndOfMaintenanceDate = hardWare?.Endofmaintenance;

                grid.VendorEndOfMaintenanceDateValue = hardWare?.Endofmaintenance != null ? hardWare?.Endofmaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : grid.EOMStatus == EOMEnum.NotAnnounced ? ConstantValueFilter.NotAnnounced : null;

                grid.TrackingNumberProjectName = x.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());
                grid.Notes = plannedActivityHW?.Notes;
                grid.SystemTypeId = x.Designcomponent.Systemtypeid;
                grid.DesignComponentId = x.Designcomponentid;
                grid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
                grid.MajorHardwareBuildId = hardWare?.Majorhardwareid;
                grid.BundleBudget = GetBundleBudget(plannedActivityHW?.Budgettrackingid);
                grid.BundleId = GetBundleBudget(plannedActivityHW?.Budgettrackingid) == ConstantValueFilter.Yes ? plannedActivityHW.Budgettrackingid.Substring(2) : null;
                grid.AssetServiceFunctionality = dcSubnetwork.Subnetworksupportedsvr?.Count() > 0 ? string.Join(" | ", dcSubnetwork.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct()) : string.Empty;
                grid.Platform = hardWare?.Hardwaresolution;

                grid.OpsMaintenanceConractEnd = x.Hardwareendofsupportcontract != null ? x.Hardwareendofsupportcontract : x.Softwareendofwarrantydate;
                grid.OpsMaintenanceConractEndValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

                grid.OpsRiskEvaluation = _commonManager.GetRiskValue(plannedActivityHW
                    ?.Operationalrisk?.Description);

                grid.OverallRiskEvaluation = plannedActivityHW
                    ?.Overallriskevaluation;

                grid.EngRiskEvaluationNotes = plannedActivityHW
                    ?.Riskengineeringnotes;

                grid.EngRiskEvaluation = _commonManager.GetRiskValue(plannedActivityHW
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
                grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngHardware, grid.OperationsMaintenanceContract);
                grid.HwIsExtendedSupportOfferedByVendor = x.Hwisextendedsupportofferedbyvendor.HasValue && x.Hwisextendedsupportofferedbyvendor.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();
                grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus, grid.OperationsMaintenanceContract, grid.OpsMaintenanceConractEnd, grid.ProjectEndDate, grid.ProjectStatus);




                var ipIdentities = x.Networkelementsasplanned?.SelectMany(p => p?.Identitiesasis).Where(p => p.Interfacetype == ConstantValueFilter.Yes.ToUpper() && p?.Category?.Description?.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress);
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
                    grid.ContractRenewalPlan = lcmAuditAttributes.Contractrenewalplan;
                    grid.DataSource = lcmAuditAttributes.Datasource;
                    grid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;
                    grid.OriginalHwLcmId = lcmAuditAttributes.Originalhwlcmid;
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
                }
                #region Apr 03 requirement
                grid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
                ? lcmAuditAttributes.Handedovertooperation : true;
                grid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
                ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;
                #endregion

                #endregion

                grid.IdentifiedAction = x.Archived != true ? GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), hardWare.Endofmaintenance, _repositoryWrapper) : "";

                grid.Program = x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.ProgramNavigation?.Programdescription;

                grid.ProjectOwner = x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Projectowner;

                return grid;
            }).ToList();

            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lasthwupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
            }


            var rtn = new QueryResultDto<ReportSubnetWorkHardWareGrid>(new GenerateRenderForGrid<ReportSubnetWorkHardWareGrid>(_columnManager))
            {
                TotalItems = totalCount
            };

            rtn.Items = reports.ToArray();

            return rtn;
        }

        public async Task<QueryResultDto<ReportSubnetWorkSoftWareGrid>> FindAggregatedSoftwareWithConditionAsync(ReportSubnetWorkQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Lcmengineering> predicateResult = ApplyFilter(buildFilterDto);


            var result = GetQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetColumnsMapDB(), "Modificationdate");
            IEnumerable<Lcmengineering> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();

            if (buildFilterDto?.OutputToLcmSoftware != null && buildFilterDto.OutputToLcmSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.OutputToLcmSoftware.Contains(x.Lcmstatussoftware));
            }
            if (buildFilterDto?.LcmStatusEngSoftware != null && buildFilterDto.LcmStatusEngSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusEngSoftware.Contains(x.Lcmstatusengsoftware));
            }
            if (buildFilterDto?.LcmStatusOpsSoftware != null && buildFilterDto.LcmStatusOpsSoftware.Any())
            {
                query = query.Where(x => buildFilterDto.LcmStatusOpsSoftware.Contains(x.Lcmstatusopssoftware));
            }
            if (buildFilterDto?.OperationsMaintenanceContractLcm != null && buildFilterDto.OperationsMaintenanceContractLcm.Any())
            {
                query = query.Where(x => buildFilterDto.OperationsMaintenanceContractLcm.Contains(x.Outputtolcmsoftware));
            }

            if (buildFilterDto?.ENGKPI2 != null && buildFilterDto.ENGKPI2.Any())
            {
                query = query.Where(x => buildFilterDto.ENGKPI2.Contains(GetEngKpi2(x.Lcmstatusengsoftware, x.Outputtolcmsoftware)));
            }
            if (buildFilterDto?.ExpLCMstatusatendofFY24 != null && buildFilterDto.ExpLCMstatusatendofFY24.Any())
            {
                query = query.Where(x => buildFilterDto.ExpLCMstatusatendofFY24.Contains(
                        GetExpLCMstatusatendofFY24(x.Lcmstatussoftware,
                                                   x.Outputtolcmsoftware,
                                                   x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract,
                                                   x.PlannedactivitiesLcmengineering.AsQueryable()
                                                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                                                   ?.Plannedcompletion,
                                                   x.PlannedactivitiesLcmengineering.AsQueryable()
                                                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                                                   ?.Projectstatus
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

                query = query.Where(x => x.Archived != true && buildFilterDto.IdentifiedAction.Contains(GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(),
                   x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofmaintenance, _repositoryWrapper)));
            }
            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Budgettrackingid)));
            }

            if (buildFilterDto?.AssetServiceFunctionality != null && buildFilterDto.AssetServiceFunctionality.Any())
            {

                query = query.Where(x => x?.Designcomponent?.Designcomponentfamily?.Subnetworkboundary != null).Where(x => buildFilterDto.AssetServiceFunctionality
                                 .Contains(string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Subnetworkboundary?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description)?.Distinct())));
            }
            if (buildFilterDto?.AssetStatus != null && buildFilterDto.AssetStatus.Any())
            {
                query = query.ToList().Where(x => x.Lcmancillarydata.Any()).Where(x => buildFilterDto.AssetStatus.Contains(
                    GetAssetStatusBasedOnOriginalHwAndSw(
                        x.Lcmancillarydata?.FirstOrDefault() != null ?
                        x.Lcmancillarydata?.FirstOrDefault()?.Originalswlcmid : string.Empty

                        )

                    ));
            }
            if (buildFilterDto?.ExposedEdgeFlag != null && buildFilterDto.ExposedEdgeFlag.Any())
            {
                query = query.ToList().Where(x => x.Lcmancillarydata.Any())
                    .Where(x => buildFilterDto.ExposedEdgeFlag.Contains(GetExposedEdgeValue(x.Lcmancillarydata.FirstOrDefault().Isexposededge)));
            }

            #region Ticket 465 - LCM export: RAG status field - filter is not working fine
            if (buildFilterDto?.RagStatus != null && buildFilterDto.RagStatus.Any())
            {
                List<List<string>> lcmIdList = new List<List<string>>();
                foreach (var item in buildFilterDto.RagStatus)
                    lcmIdList.Add(
                        LcmEngineeringExtensionMethod.aggregatedRagStatusFilterRecord(query, null, 2, item).Where(x => x.Value != "0")
                        .Select(x => x.Text).ToList()
                        );

                if (lcmIdList != null && lcmIdList.Count() > 0)
                    query = query.Where(x => lcmIdList.SelectMany(y => y).Distinct().Contains(x.Lcmengineeringid.ToString()));

            }
            #endregion

            var totalCount = query.Count();
            if (!isExport)
            {
                query = query.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            else
            {
                query = query.ToList();
            }

            var allOperationalContracts = _repositoryWrapper.LCMOperationalContracts.FindAll().Include(x => x.Operationalcontract).ToList();
            #region Code Optimization

            var allVerticalFilterDto = _repositoryWrapper.VerticalResponsible.FindAll().Select(x => new FilterValueDto
            {
                Value = x.Verticalresponsibleid.ToString(),
                Text = x.Verticalresponsible
            }).ToList().DistinctBy(t => t.Value);
            string allVerticalForAdminRole = string.Join(",", allVerticalFilterDto.Select(x => x.Text).ToList()).ToString();
            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != null).Select(x => x.Lcmengineeringid)?.Distinct()?.ToList();
            var lcmIdAndLcmOpcoId = query?.ToList()?.Where(x => x.Lcmengineeringid != null)?.DistinctBy(x => x?.Lcmengineeringid)
                .ToDictionary(x => x.Lcmengineeringid, x => (long)x.Opcoid);
            var allSubDomain = _commonManager.GetCalculatedLcmSubDomainSpocEntityForReport(lcmIdAndLcmOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedLcmEduSpocEntityForReport(lcmIdAndLcmOpcoId).ToList();
            var allOperationalContract = _commonManager.GetCalculateLCMOperationalContractsForReport(lcmengineeringsEntityId);

            #endregion
            var reportLastUpdateDate = await _repositoryWrapper.LcmDBExportUpdateHistory.FindAll().FirstOrDefaultAsync();
            bool isLcmDBExportUpdated = false;
            if (reportLastUpdateDate != null && reportLastUpdateDate.Lastswupdatedate.Date == DateTime.Now.Date)
            {
                isLcmDBExportUpdated = true;
            }

            var reports = query.Select(x =>
            {
                var grid = new ReportSubnetWorkSoftWareGrid();
                var systemType = x.Designcomponent?.Systemtype;
                var majorSW = systemType?.Majorsoftwarebuilds;
                var plannedActivitySW = x.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Software).Plannedactivityresourceid == null ? null : x.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Software);
                var dcSubnetwork = x.Designcomponent?.Subnetworkboundary;
                var Dcf = x.Designcomponent?.Designcomponentfamily;

                if (!isLcmDBExportUpdated)
                {
                    x.Outputtolcmsoftware = x.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareOutput).Result;
                    x.Lcmstatussoftware = x.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatus).Result;
                    x.Lcmstatusopssoftware = x.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusOps).Result;
                    x.Lcmstatusengsoftware = x.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusEng).Result;
                    _repositoryWrapper.Lcmengineering.Update(x);
                    _repositoryWrapper.Save();
                    _repositoryWrapper.ClearTracker();

                }

                grid.DesignComponentFamily = x.Designcomponent?.toDesignComponentFamily();
                grid.SupportedService = string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct() ?? new List<string>());


                //Lcm Status
                grid.LcmStatus = x.Lcmstatussoftware;
                grid.OperationsMaintenanceContract = x.Outputtolcmsoftware;
                grid.LcmStatusOpsSoftware = x.Lcmstatusopssoftware;
                grid.LcmStatusEngSoftware = x.Lcmstatusengsoftware;

                grid.ReportId = x.Resourcekey;

                grid.LcmEngineeringId = x.Lcmengineeringid;
                grid.LocalMarket = x.Opco?.Opco;
                grid.DesignComponentIndex = x.Designcomponentid;

                #region code optimize org Table
                ;

                grid.VerticalEngineeringTeam =
                string.Join(",", allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && m.VerticalDic != null && x.Deleted == false)
                .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList())
                 ;

                grid.VerticalSubDomain = string.Join(",", allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && m.SubdomainresponsiblesDic != null && x.Deleted == false)
                .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList());

                grid.EngineeringContactPoint = _commonManager.GetEngContactPointFromEduAndSubDomainSpoc(allEdu?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && x.Deleted == false).Select(t => t?.ContactEmail).ToList(),
                    allSubDomain?.Where(m => m.Lcmengineeringid == x.Lcmengineeringid && x.Deleted == false).Select(t => t?.ContactEmail).ToList());

                grid.OperationsContactPoint = string.Join(" | ",
                    allOperationalContract?.Where(m => m.LcmengineeringId == x.Lcmengineeringid).Select(x => x?.OperationDescription).Distinct());

                #endregion
                grid.AssetStatus = GetAssetStatusBasedOnOriginalHwAndSw(
                   x.Lcmancillarydata?.FirstOrDefault() != null ? x.Lcmancillarydata?.FirstOrDefault()?.Originalswlcmid
                    : string.Empty
                    );

                grid.AssetCategory = systemType?.Assetcategory?.Assetcategory;

                grid.AssetClass = majorSW.Productname != null ? majorSW.Productname?.Description : "";

                grid.AssetType = majorSW.Criticalassettype?.Description;

                grid.ProductImportance = x.Productimportance?.Productimportance;

                grid.Vendor = majorSW.Orgeqpmanufacturer
                    ?.Originalequipmentmanufacturer;

                var hw = systemType?.Systemtypesmajorhardwarebuilds
                   ?.SingleOrDefault(m =>
                       m.Ismain &&
                       m.Systemtypeid == systemType?.Systemtypeid &&
                       m.Deleted == false)?.Majorhardware;

                grid.HardwareModel = hw.Buildconstruction?.Rule == (int)BuildconstructionRuleEnum.VirtualHW ? ConstantValueFilter.Virtualized : systemType?.toLcmDbExportHardwareName();


                int numberOfNodesCount = x.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result;
                grid.NumberOfNodes = numberOfNodesCount;



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


                grid.TrackingNumberProjectName = x.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Software);
                grid.Notes = plannedActivitySW?.Notes;

                grid.SoftwareVersion = majorSW?.Softwareversion;



                grid.SystemTypeId = x.Designcomponent?.Systemtypeid ?? 0;

                grid.DesignComponentId = x.Designcomponentid;
                grid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
                grid.MajorHardwareBuildId = x.Designcomponent.Systemtype?.Systemtypesmajorhardwarebuilds
                    ?.SingleOrDefault(m =>
                        m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                        m.Deleted == false)?.Majorhardwareid;

                grid.AssetVirtualized =
                 hw?.Buildconstruction?.Iscloudasset == true ? ConstantValueFilter.YES
                 : ConstantValueFilter.NO;

                grid.BundleBudget = GetBundleBudget(plannedActivitySW?.Budgettrackingid);

                grid.BundleId = GetBundleBudget(plannedActivitySW?.Budgettrackingid) == ConstantValueFilter.yes ? plannedActivitySW.Budgettrackingid.Substring(2) : null;

                grid.AssetServiceFunctionality = dcSubnetwork?.Subnetworksupportedsvr?.Count() > 0 ? string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct()) : string.Empty;

                grid.Platform = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname?.Description : "";

                grid.OpsMaintenanceConractEnd = x.Softwareendofwarrantydate != null ? x.Softwareendofwarrantydate : x.Softwareendofsupportcontract;
                grid.OpsMaintenanceConractEndValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

                grid.VendorEndOfVulnerabilitySecuritySupportDateValue = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

                grid.EngRiskEvaluationNotes = plannedActivitySW
                  ?.Riskengineeringnotes;


                grid.EngRiskEvaluation = _commonManager.GetRiskValue(plannedActivitySW
                   ?.Engineeringrisk?.Description);

                grid.OpsRiskEvaluation = _commonManager.GetRiskValue(plannedActivitySW
                    ?.Operationalrisk?.Description);
                grid.OpsRiskEvaluationNotes = plannedActivitySW?.Riskoperationalnotes;


                grid.OverallRiskEvaluation = plannedActivitySW?.Overallriskevaluation;
                grid.PlannedActivityId = plannedActivitySW?.Plannedactivityid;

                grid.BudgetEstimated = x.PlannedactivitiesLcmengineering.GetPlannedActivityBudgetEstimated(ConstantValueFilter.Software);
                grid.ManagedByGdc = ConstantValueFilter.no;

                grid.DesignComponentFamilyId = x.Designcomponent?.Designcomponentfamilyid;
                grid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";

                grid.Archived = x.Archived;


                #region LCM-R8-Phase1

                grid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper);//systemType?.VodafonenameNavigation?.Description;
                grid.Criticality = dcSubnetwork?.Criticality;
                grid.GdprRelevant = dcSubnetwork.Gdprrelevant != null ? dcSubnetwork.Gdprrelevant == true ? ConstantValueFilter.YES : ConstantValueFilter.NO : null;
                grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngSoftware, grid.OperationsMaintenanceContract);
                grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus, grid.OperationsMaintenanceContract, grid.OpsMaintenanceConractEnd, grid.ProjectEndDate, grid.ProjectStatus);
                grid.IsExtendedSupportOfferedByVendor = x.Isextendedsupportofferedbyvendor.HasValue && x.Isextendedsupportofferedbyvendor.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO;

                var ipIdentities = x?.Networkelementsasplanned.SelectMany(p => p?.Identitiesasis).Where(p => p.Interfacetype == ConstantValueFilter.Management && p?.Category?.Description.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress);
                grid.IpAddress = ipIdentities != null ? string.Join(" | ", ipIdentities?.Select(fx => fx.Value).Distinct()) : string.Empty;

                grid.Hostname = string.Empty;//x.Networkelementsasplanned != null ? string.Join(" | ", x.Networkelementsasplanned.Select(fx => fx.Elementname).Distinct()) : string.Empty;
                                             //var plannedActivityHWForRagStatus = x.PlannedactivitiesLcmengineering
                                             //                                    .GetPlannedActivityFilteredDB(ConstantValueFilter.Software, true).Plannedactivityresourceid == null ? null
                                             //                                    : x.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Software, true);

                #region Ticket 465 - LCM export: RAG status field - filter is not working fine
                grid.RagStatus = LcmEngineeringExtensionMethod.aggregatedRagStatusFilterRecord(null,
                  query.Where(y => y.Lcmengineeringid == x.Lcmengineeringid).ToList(), 2
                  ).FirstOrDefault().Text.ToString();
                //if (x.Archived == true)
                //{
                //    grid.RagStatus = LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(3);
                //}
                //else if (grid.EOMStatus == EOMEnum.NotAnnounced)
                //{
                //    grid.RagStatus = string.Empty;
                //}
                //else if (grid.VendorEndOfMaintenanceDate > targetDate)
                //{
                //    grid.RagStatus = string.Empty;
                //}
                //else if (grid.VendorEndOfMaintenanceDate < fronzenDate)
                //{
                //    grid.RagStatus = plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status.ToString() != null ?
                //   LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status) : "";
                //}
                //else if (grid.VendorEndOfMaintenanceDate == null)
                //{
                //    grid.RagStatus = plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status.ToString() != null ?
                //    LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status) : "";
                //}
                //else if (fronzenDate <= grid.VendorEndOfMaintenanceDate || grid.VendorEndOfMaintenanceDate <= targetDate)
                //{
                //    grid.RagStatus = plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status.ToString() != null ?
                //    LcmEngineeringExtensionMethod.ConvertToDeliveryStatusEnum(plannedActivitySW?.Deliverytrackings?.FirstOrDefault()?.Ms2status) : "";
                //};
                //grid.RagStatus = grid.RagStatus  + "--" + gdRag +"-"+ x.Lcmengineeringid;
                #endregion
                grid.DeliveryPlanAvailable = plannedActivitySW?.Deliveryplanavailable != null && plannedActivitySW.Deliveryplanavailable ? ConstantValueFilter.YES : ConstantValueFilter.NO;

                grid.WbsCode = GetWbsCode(plannedActivitySW?.Deliveryprojectname);
                grid.BptID = plannedActivitySW?.Budgettrackingid;
                grid.PpmID = plannedActivitySW?.Deliveryprojectid;
                grid.MainOrganization = ConstantValueFilter.Nse;
                grid.SerialNumber = x.Resourcekey;


                #endregion

                #region LCM R9 part-1
                var lcmAuditAttributes = x.Lcmancillarydata.FirstOrDefault();
                grid.SecurityRiskPotential = x.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames.Where(x => x.Riskcluster != null).Select(x => x.Riskcluster?.Risklevel).FirstOrDefault();
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
                    grid.OccurrenceProbability = lcmAuditAttributes.Occurenceprobability;
                    grid.IncidentClass = lcmAuditAttributes.Incidentclass;
                    grid.NewopsRiskEvaluation = GetNewOpsRiskEvaluationValue(grid.IncidentClass, grid.OccurrenceProbability);

                    grid.ProductCode = lcmAuditAttributes.Productcode;
                    grid.HandedOverToOperation = lcmAuditAttributes.Handedovertooperation;
                    grid.ContractRenewalPlan = lcmAuditAttributes.Contractrenewalplan;
                    grid.DataSource = lcmAuditAttributes.Datasource;
                    grid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;
                    grid.OriginalSwLcmId = lcmAuditAttributes.Originalswlcmid;

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
                }
                grid.LabSWRelease = x.Numberofnodesinlab > 0 ?
                                    majorSW?.Softwareversion : "";

                grid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
                                              ? lcmAuditAttributes.Handedovertooperation : true;
                grid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
               ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;

                #endregion
                var Endofmaintenance = majorSW?.Endofmaintenance;
                grid.IdentifiedAction = x.Archived != true ? GetIdentificationActionForLcmExport(x.PlannedactivitiesLcmengineering.AsQueryable()
                  .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), Endofmaintenance, _repositoryWrapper) : "";


                grid.Program = x.PlannedactivitiesLcmengineering.AsQueryable()
                  .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                  ?.ProgramNavigation?.Programdescription;

                grid.ProjectOwner = x.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivitySoftware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Projectowner;


                return grid;
            });

            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lastswupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
            }

            var rtn = new QueryResultDto<ReportSubnetWorkSoftWareGrid>(new GenerateRenderForGrid<ReportSubnetWorkSoftWareGrid>(_columnManager))
            {
                TotalItems = totalCount
            };
            rtn.Items = reports.ToArray();

            return rtn;
        }

        #endregion

    }
}


