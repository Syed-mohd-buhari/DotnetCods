using AutoMapper.Internal;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.ComponentBag;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
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
using System.Threading.Tasks;
using static CAM.BusinessManager.Rules.LCMEngineeringRulesExtension;
namespace CAM.BusinessManager.Entity.Report
{
    public class ReportHardwareConfigurationManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private static int currenYear = DateTime.Now.Year;
        private static readonly DateTime fronzenDate = new DateTime(currenYear, 6, 1);
        private static readonly DateTime targetDate = new DateTime(currenYear + 1, 6, 1);
        private CommonManager _commonManager;

        private static Expression<Func<Plannedactivities, bool>> PlannedActivityHardware = PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware);

        private readonly string _engUpdateTracker = "To be started";
        public ReportHardwareConfigurationManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, CommonManager commonManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper
            ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _commonManager = commonManager;
        }


        public List<FilterValueDto> GetDisaggregatedHWConfigurationFilter(string propertyName, string propertyFilter,
           ReportHardwareConfigurationQueryDto buildFilterDto,bool isAdmin=false)
        {

            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyDisaggregatedHWConfigurationFilter(buildFilterDto);
            var query = GetDisaggregatedHWConfigurationQuery(predicateResult);

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
                    .Where(PlannedActivityHardware)
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Deliveryprojectname))).Distinct().ToList(),
                "bptID" => query
                .Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivityHardware)
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Projectstatus != null ?
                 p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                .Where(PlannedActivityHardware)
                .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                .Budgettrackingid : string.Empty)).Distinct().ToList(),
                "ppmID" => query
                    .Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivityHardware)
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                    .Projectstatus != null ?
                     p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivityHardware)
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
                                                                   p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                                  .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion,
                                                                    p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
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
                      .Where(PlannedActivityHardware)
                      .OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                      .Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.HardwareUpgrade).ToList()
                     .Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                      .Where(PlannedActivityHardware)
                      .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Designcomponent?.Systemtype.toLcmDbExportHardwareName())).Distinct().ToList(),
                "numberOfNodes" => query.Select(p => new FilterValueDto
                { Text = "1", Value = "1" }).Distinct().ToList(),
                "plannedAction" => new List<FilterValueDto>() { new FilterValueDto() { Text = ConstantValueFilter.OnHardware, Value = "1" }, new FilterValueDto() { Text = ConstantValueFilter.OnSoftware, Value = "2" } },
                "descriptionOfPlannedAction" => query.Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                        .Plannedactivityresource.Plannedactivityresource)).Distinct().ToList(),
                "trackingNumberProjectName" => query.Select(x => new FilterValueDto
                    (x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname)).Distinct().ToList(),
                "notes" => query.Select(p => new FilterValueDto(p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes)).Distinct().ToList(),
                #region Ticket 465 - LCM export: RAG status field - filter is not working fine

                "ragStatus" => LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(query, null, 1).Distinct().ToList(),
                #endregion

                "operationsContactPoint" => query.SelectMany(x => x.Lcmengineering.Lcmoperationalcontracts)
                                        .Select(p => new FilterValueDto(p.Operationalcontract.Description)).Distinct().ToList(),
                "identifiedAction" => query.Select(x => new FilterValueDto(GetIdentificationActionForLcmExport(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                                .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                           .SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false).Majorhardware.Endofmaintenance, _repositoryWrapper))).ToList().Distinct().ToList(),
                "softwareRelease" => query.Select(p => new FilterValueDto(p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion)).Distinct().ToList(),
                "cloudVersion" => query.Select(p => new FilterValueDto(ConstantValueFilter.Cloudversion)).Distinct().ToList(),
                "budgetEstimated" => query.Select(s => s.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
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
                    Text = _commonManager.GetRiskValue(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description),
                    Value = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid.ToString()
                }).Distinct().ToList(),
                "engRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes)).Distinct().ToList(),
                //LCM R8 Phase 1  
                "opsRiskEvaluation" => query.Select(x => new FilterValueDto
                {
                    Text = _commonManager.GetRiskValue(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description),
                    Value = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid.ToString()
                }
                ).Distinct().ToList(),
                "opsRiskEvaluationNotes" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "overallRiskEvaluation" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes)).Distinct().ToList(),
                "outputToLcmSoftware" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatus).Result)).Distinct().ToList(),
                "operationsMaintenanceContract" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Outputtolcmhardware)).Distinct().ToList(),
                "typeOfNetworkElement" => query.Where(x => x.Designcomponent.Subnetworkboundary.Lcmpolicy != null)
                  .Select(x => new FilterValueDto { Text = ((LCMPolicy)x.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString(), Value = x.Designcomponent.Subnetworkboundary.Lcmpolicy.ToString() }).Distinct().ToList(),
                "lcmStatusOpsHardware" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatusopshardware)).Distinct().ToList(),
                "lcmStatusEngHardware" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatusenghardware)).Distinct().ToList(),
                "lcmStatus" => query.ToList().Select(p => new FilterValueDto(p.Lcmengineering.Lcmstatushardware)).Distinct().ToList(),
                "bundleId" => query.AsEnumerable().Where(x => GetBundleBudget(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivityHardware)
                            .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Budgettrackingid) == ConstantValueFilter.Yes)
                               .Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivityHardware)
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
                                             .Where(PlannedActivityHardware)
                                     .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription)).ToList().Distinct().ToList(),
                "projectOwner" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                       .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectowner)).ToList().Distinct().ToList(),
                "projectStatus" => query.Select(x => new FilterValueDto(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                    .Where(PlannedActivityHardware)
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

                "designComponentFamily" => query.ToList().Select(y =>
                new FilterValueDto
                {
                    Text = y.Designcomponent.toDesignComponentFamily(),
                    Value = y.Designcomponent.Designcomponentfamilyid.ToString()
                }).Distinct().ToList(),

                "supportedService" => query.Where(x => x.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr != null).Select(y =>
                new FilterValueDto
                {
                    Text = y.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Select(x => x.Service.Description).FirstOrDefault(),
                    Value = y.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Select(x => x.Service.Id.ToString()).FirstOrDefault()
                }).Distinct().ToList(),

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

        public QueryResultDto<ReportHardwareConfigurationDtoGrid> FindDisaggregatedHWConfigurationWithCondition(ReportHardwareConfigurationQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyDisaggregatedHWConfigurationFilter(buildFilterDto);


            var result = GetDisaggregatedHWConfigurationQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetDisaggregatedHWConfigurationColumnsMapDB());
            IEnumerable<Networkelementsasplanned> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();
            #region // Filters
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
                                                    x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Plannedcompletion,
                                                    x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Projectstatus
                                                    )));
            }


            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
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
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofmaintenance, _repositoryWrapper)));
            }
            if (buildFilterDto?.ExposedEdgeFlag != null && buildFilterDto.ExposedEdgeFlag.Any())
            {
                query = query.ToList().Where(x => x.Lcmengineering.Lcmancillarydata.Any())
                    .Where(x => buildFilterDto.ExposedEdgeFlag.Contains(GetExposedEdgeValue(x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Isexposededge)));
            }
            #endregion

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

            var totalCount = query.SelectMany(x=>x.Buildbag?.Componentsoftwarebuildbags?.DefaultIfEmpty()).Count();

            if (buildFilterDto.SortBy == ConstantValueFilter.productImportance)
            { }

            query = query.DistinctBy(x => x.Hwresourcekey);            

            var data = query;

            var allOperationalContracts = _repositoryWrapper.LCMOperationalContracts.FindAll().AsNoTracking().AsSplitQuery().Include(x => x.Operationalcontract).ToList();

            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != null).Select(x => (long)x.Lcmengineeringid)?.Distinct()?.ToList();

            var AssetIdAndOpcoId = query?.ToList()?.Where(x => x.Networkelementasplannedid != 0)?.DistinctBy(x => x?.Networkelementasplannedid)
                .ToDictionary(x => x.Networkelementasplannedid, x => (long)x.Opcoid);

            var allSubDomain = _commonManager.GetCalculatedAssetSubDomainSpocEntityforReport(AssetIdAndOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedAssetEduSpocEntityforReport(AssetIdAndOpcoId).ToList();

            var allOperationalContract = _commonManager.GetCalculateLCMOperationalContractsForReport(lcmengineeringsEntityId);


            var HWConfiguration = _commonManager.GetHardwareConfigurations(query.Select(x => x.Elementname).ToList());



            var reportLastUpdateDate = _repositoryWrapper.LcmDBExportUpdateHistory.FindAll().FirstOrDefault();
            bool isLcmDBExportUpdated = false;
            if (reportLastUpdateDate != null && reportLastUpdateDate.Lasthwupdatedate.Date == DateTime.Now.Date)
            {
                isLcmDBExportUpdated = true;
            }

            var reports = data.SelectMany(x=>x.Buildbag?.Componentsoftwarebuildbags?.DefaultIfEmpty().Select(y =>
            {
                var grid = new ReportHardwareConfigurationDtoGrid();
                var dcSubnetwork = x.Designcomponent?.Subnetworkboundary;

                var systemType = x.Designcomponent?.Systemtype;
                var hardWare = systemType?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware;
                var Dcf = x.Designcomponent?.Designcomponentfamily;
                var Subnetwork = x.Designcomponent?.Designcomponentfamily?.Subnetworkboundary;
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

                grid.OperationsMaintenanceContract = x.Lcmengineering.Outputtolcmhardware;
                grid.LcmStatus = x.Lcmengineering.Lcmstatushardware;
                grid.LcmStatusOpsHardware = x.Lcmengineering.Lcmstatusopshardware;
                grid.LcmStatusEngHardware = x.Lcmengineering.Lcmstatusenghardware;

                #endregion

                #region // Bag detailsx
                grid.BagName = _commonManager.GetBuildBagDescription(x.Buildbag);
                grid.ComponentName = ComponentBagExtensionMethod.GetComponentBagDescription(_repositoryWrapper, y?.Componentsoftwarebuildid);
                grid.ComponentResourceKey = ComponentBagExtensionMethod.GetComponentResourcekey(_repositoryWrapper, x.Opcoid, grid.ComponentName, x.Designcomponent.Designcomponentfamilyid);
                #endregion

                grid.DesignComponentFamily = x.Designcomponent.toDesignComponentFamily();
                grid.SupportedService = string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct() ?? new List<string>());

                grid.ReportId = $"{x.Lcmengineering.Resourcekey}-{x.Hwresourcekey}-0";
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

                grid.AssetType = _commonManager.GetAssetType(hardWare?.Buildconstruction?.Buildconstruction);

                grid.AssetDescription = Subnetwork.Description;

                grid.ProductImportance = x.Lcmengineering.Productimportance?.Productimportance;
                grid.Vendor = hardWare?.Orgeqpmanufacturer
                    ?.Originalequipmentmanufacturer;
                grid.HardwareModel = systemType?.toLcmDbExportHardwareName();
                grid.NumberOfNodes = 1;

                grid.EOMStatus = hardWare != null
                           ? (EOMEnum)hardWare.Eomstatus : EOMEnum.NotSpecified;

                grid.VendorEndOfMaintenanceDate = hardWare?.Endofmaintenance;

                #region Ticket 465 - LCM export: RAG status field - filter is not working fine
                grid.RagStatus = LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(null,
                  data.Where(y => y.Lcmengineering.Lcmengineeringid == x.Lcmengineeringid).ToList(), 1
                  ).FirstOrDefault().Text.ToString();
                #endregion

                grid.VendorEndOfMaintenanceDateValue = hardWare?.Endofmaintenance != null ? hardWare?.Endofmaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : grid.EOMStatus == EOMEnum.NotAnnounced ? ConstantValueFilter.NotAnnounced : null;

                grid.TrackingNumberProjectName = x.Lcmengineering.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());
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


                grid.ManagedByGdc = ConstantValueFilter.No;

                grid.DesignComponentFamilyId = x.Designcomponent?.Designcomponentfamilyid;
                grid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";

                grid.Archived = x.Lcmengineering.Archived;

                #region LCM-R8-Phase1

                grid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper);
                grid.Criticality = dcSubnetwork?.Criticality;



                grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngHardware, grid.OperationsMaintenanceContract);
                grid.HwIsExtendedSupportOfferedByVendor = x.Lcmengineering.Hwisextendedsupportofferedbyvendor.HasValue && x.Lcmengineering.Hwisextendedsupportofferedbyvendor.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();
                grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus,
                                                                          grid.OperationsMaintenanceContract,
                                                                          grid.OpsMaintenanceConractEnd,
                                                                          grid.ProjectEndDate,
                                                                          grid.ProjectStatus);

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
                grid.SecurityRiskOverall = GetSecurityRiskOverAllValue(lcmAuditAttributes != null ? lcmAuditAttributes.Securityriskeffective : string.Empty, grid.SecurityRiskPotential);
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
                    grid.RequestID = lcmAuditAttributes.Requestid;
                    //grid.Id_New = lcmAuditAttributes.Idnew;
                    //grid.ProductImportanceHistory2 = lcmAuditAttributes.Productimportancehistory2;
                    //grid.LcmStatusJune2021 = lcmAuditAttributes.Lcmstatus;
                    grid.LastScanDate = lcmAuditAttributes.Lastscandate?.Date;
                    grid.LastScanDateValue = lcmAuditAttributes.Lastscandate;//?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    grid.AssetOutofScopeForReportingPurposes = lcmAuditAttributes.Assetoutofscope;//calculated filesd
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
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), hardWare.Endofmaintenance, _repositoryWrapper) : "";

                grid.Program = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.ProgramNavigation?.Programdescription;

                grid.ProjectOwner = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Projectowner;

                grid.HardwareProfile = _commonManager.GetHwProfile(grid.Hostname);

                return grid;
            })).ToList();

            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lasthwupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
            }
            if (!isExport)
            {
                reports = reports.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            else
            {
                reports = reports.ToList();
            }

            var rtn = new QueryResultDto<ReportHardwareConfigurationDtoGrid>(new GenerateRenderForGrid<ReportHardwareConfigurationDtoGrid>(_columnManager))
            {
                TotalItems = totalCount
            };

            rtn.Items = reports.ToArray();

            return rtn;
        }

        private IQueryable<Networkelementsasplanned> GetDisaggregatedHWConfigurationQuery(ExpressionStarter<Networkelementsasplanned> predicateResult)
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
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x=>x.Subnetworkboundary)
                                .Include(x => x.Lcmengineering.Productimportance)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmancillarydata)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation).ThenInclude(x => x.Riskclustervodafonenames).ThenInclude(x => x.Riskcluster)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetclass)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x=>x.Subnetworksupportedsvr).ThenInclude(x=>x.Service)
                                //.Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                                //.Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designaspects).ThenInclude(x => x.Designaspectssupportedsvr)
                                //        .ThenInclude(x => x.Service)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverytrackings)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Activitystatus)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Engineeringrisk)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Operationalrisk)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.ProgramNavigation)
                                .Include(x => x.Lcmengineering.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Identitiesasis).ThenInclude(x => x.Category)
                                .Include(x=>x.Buildbag).ThenInclude(x=>x.Componentsoftwarebuildbags);
        }

        public ExpressionStarter<Networkelementsasplanned> ApplyDisaggregatedHWConfigurationFilter(ReportHardwareConfigurationQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);

            var predicateInner = PredicateBuilder.New<Networkelementsasplanned>(true);
            if (buildFilterDto?.BptID != null && buildFilterDto.BptID.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.BptID)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.PpmID != null && buildFilterDto.PpmID.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.PpmID)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SerialNumber != null && buildFilterDto.SerialNumber.Any())
            {
                foreach (var item in buildFilterDto?.SerialNumber)
                    predicateInner.Or(x => x.Hwresourcekey == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProgramLcm != null && buildFilterDto.ProgramLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ProgramLcm)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().ProgramNavigation.Programdescription == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.ProjectOwner != null && buildFilterDto.ProjectOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ProjectOwner)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
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
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Lcmpolicy == item);
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
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Projectstatus == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.TrackingNumberProjectNameLcm != null && buildFilterDto.TrackingNumberProjectNameLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.TrackingNumberProjectNameLcm)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.Notes != null && buildFilterDto.Notes.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Notes)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
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
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion >= buildFilterDto.ProjectEndDateValue.StartDate);
                if (buildFilterDto?.ProjectEndDateValue.EndDate != null)
                    predicateInner.And(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
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
                         .Where(PlannedActivityHardware)
                         .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgetvalue == decimalValue);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                            .Where(PlannedActivityHardware)
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
                        .Where(PlannedActivityHardware)
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
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EngRiskEvaluationNotes != null && buildFilterDto.EngRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.EngRiskEvaluationNotes)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluation != null && buildFilterDto.OpsRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluation)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OpsRiskEvaluationNotes != null && buildFilterDto.OpsRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OpsRiskEvaluationNotes)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OverallRiskEvaluation != null && buildFilterDto.OverallRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OverallRiskEvaluation)
                    predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
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
                       .Where(PlannedActivityHardware)
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryplanavailable == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivityHardware)
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

        private Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]> GetDisaggregatedHWConfigurationColumnsMapDB()
        {
            return new Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]>
            {
                ["hardwareSheetIndex"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Hardwaresheetindex },
                ["localMarket"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Opco.Opco },
                ["assetCategory"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Assetcategory.Assetcategory },
                ["assetClass"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.toAssetClassDescription(_repositoryWrapper) },
                ["assetType"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype.Assettype.Assettype : x.Designcomponent.Systemtype.VodafonenameNavigation != null ? x.Designcomponent.Systemtype.VodafonenameNavigation.Description : "" },
                ["assetDescription"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => !string.IsNullOrEmpty(p.Designcomponent.Subnetworkboundary.Description) ? p.Designcomponent.Designcomponentfamily.Description : p.Designcomponent.Designcomponentfamily.Description },
                ["productImportance"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Productimportance.Productimportance },
                ["vendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer },
                ["hardwareModel"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                            .Single(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Platform, p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                    .Single(m => m.Ismain && m.Systemtypeid == p.Designcomponent.Systemtype.Systemtypeid).Majorhardware.Hardwaretype },
                ["numberOfNodes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => 1 },
                ["vendorEndOfMaintenanceDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance },
                ["plannedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Onhardware, p => p.Lcmengineering.Onsoftware },
                ["descriptionOfPlannedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityresource.Plannedactivityresource },
                ["projectEndDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedcompletion },
                ["trackingNumberProjectName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Deliveryprojectname },
                ["notes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Notes },
                ["identifiedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Plannedactivityid },
                ["softwareRelease"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion },
                ["bundleBudget"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid },

                ["bundleId"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Budgettrackingid },
                ["assetServiceFunctionality"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => !string.IsNullOrEmpty(p.Designcomponent.Subnetworkboundary.Alias) ? p.Designcomponent.Subnetworkboundary.Alias : p.Designcomponent.Subnetworkboundary.Description },
                ["platform"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Assetclass.Assetclass },
                ["opsMaintenanceConractEnd"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Hardwareendofsupportcontract },
                ["engRiskEvaluation"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Engineeringrisk.Description },
                ["engRiskEvaluationNotes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskengineeringnotes },
                ["oPSRiskEvaluation"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Operationalrisk.Description },
                ["oPSRiskEvaluationNotes"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Riskoperationalnotes },
                ["VendorEndOfVulnerabilitySecuritySupportDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport },
                ["overallRiskEvaluation"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                        .OrderBy(x => x.Plannedcompletion).FirstOrDefault().Overallriskevaluation },
                ["outputToLcmHardware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatushardware },
                ["lcmStatus"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatushardware },
                ["operationsMaintenanceContract"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Outputtolcmhardware },
                ["lcmStatusEngHardware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatusenghardware },
                ["lcmStatusOpsHardware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmstatusopshardware },
                ["operationsContact"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Lcmoperationalcontracts.FirstOrDefault().Operationalcontract.Description, p => p.Lcmengineering.Lcmoperationalcontracts.FirstOrDefault().Operationalcontract.Description },
                ["riskCluster"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Vodafonename != null ? p.Designcomponent.Systemtype.VodafonenameNavigation.Description : null },
                ["criticality"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Subnetworkboundary.Description },
                ["isExtendedSupportofferedByVendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.Hwisextendedsupportofferedbyvendor },
                ["deliveryPlanAvailable"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
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


        #region Ticket 465 - LCM export: RAG status field - filter is not working fine
        public static IEnumerable<FilterValueDto> disAggregatedRagStatusFilterRecord_1(IEnumerable<Networkelementsasplanned> lcm, List<Networkelementsasplanned> singleLcm, string filterSearchValue = "")
        {

            FilterValueDto ragDropDownFilterValueDto = new FilterValueDto();  
            FilterValueDto ragStatusBasedLcmIdFilterValueDto = new FilterValueDto();  

            if (singleLcm != null && singleLcm.Count() > 0)
                lcm = singleLcm;  


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

            });

            //return null;

        }
        #endregion

        #region Async Operations
        public async  Task<QueryResultDto<ReportHardwareConfigurationDtoGrid>> FindDisaggregatedHWConfigurationWithConditionAsync(ReportHardwareConfigurationQueryDto buildFilterDto, bool isExport = false)
        {
            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyDisaggregatedHWConfigurationFilter(buildFilterDto);


            var result = GetDisaggregatedHWConfigurationQuery(predicateResult).AsQueryable();
            var orderedData = result.ApplyOrdering(buildFilterDto, GetDisaggregatedHWConfigurationColumnsMapDB());
            IEnumerable<Networkelementsasplanned> query = orderedData.OrderByDescending(p => p.Modificationdate).ToList();
            #region // Filters
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
                                                    x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Plannedcompletion,
                                                    x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable().Where(PlannedActivityHardware)
                                                    .OrderBy(x => x.Plannedcompletion).FirstOrDefault()?.Projectstatus
                                                    )));
            }


            if (buildFilterDto?.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                query = query.Where(x => buildFilterDto.WbsCode.Contains(GetWbsCode(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Deliveryprojectname)));
            }

            if (buildFilterDto?.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                query = query.Where(x => buildFilterDto.BundleBudget.Contains(GetBundleBudget(x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
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
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware?.Endofmaintenance, _repositoryWrapper)));
            }
            if (buildFilterDto?.ExposedEdgeFlag != null && buildFilterDto.ExposedEdgeFlag.Any())
            {
                query = query.ToList().Where(x => x.Lcmengineering.Lcmancillarydata.Any())
                    .Where(x => buildFilterDto.ExposedEdgeFlag.Contains(GetExposedEdgeValue(x.Lcmengineering.Lcmancillarydata.FirstOrDefault().Isexposededge)));
            }
            #endregion
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

            var totalCount = query.SelectMany(x=>x.Buildbag.Componentsoftwarebuildbags.DefaultIfEmpty()).Count();

            if (buildFilterDto.SortBy == ConstantValueFilter.productImportance)
            { }

            query = query.DistinctBy(x => x.Hwresourcekey);

           

            var data = query;

            #region // Org related changes
            var allOperationalContracts = await _repositoryWrapper.LCMOperationalContracts.FindAll().AsNoTracking().AsSplitQuery().Include(x => x.Operationalcontract).ToListAsync();

            var lcmengineeringsEntityId = query?.ToList()?.Where(x => x.Lcmengineeringid != null).Select(x => (long)x.Lcmengineeringid)?.Distinct()?.ToList();

            var AssetIdAndOpcoId = query?.ToList()?.Where(x => x.Networkelementasplannedid != 0)?.DistinctBy(x => x?.Networkelementasplannedid)
                .ToDictionary(x => x.Networkelementasplannedid, x => (long)x.Opcoid);

            var allSubDomain = _commonManager.GetCalculatedAssetSubDomainSpocEntityforReport(AssetIdAndOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedAssetEduSpocEntityforReport(AssetIdAndOpcoId).ToList();

            var allOperationalContract = _commonManager.GetCalculateLCMOperationalContractsForReport(lcmengineeringsEntityId);


            var HWConfiguration = _commonManager.GetHardwareConfigurations(query.Select(x => x.Elementname).ToList());



            var reportLastUpdateDate = _repositoryWrapper.LcmDBExportUpdateHistory.FindAll().FirstOrDefault();
            bool isLcmDBExportUpdated = false;
            if (reportLastUpdateDate != null && reportLastUpdateDate.Lasthwupdatedate.Date == DateTime.Now.Date)
            {
                isLcmDBExportUpdated = true;
            }
            #endregion

            var reports = data.SelectMany(x=>x.Buildbag.Componentsoftwarebuildbags.DefaultIfEmpty().Select(y=>
            {
                var grid = new ReportHardwareConfigurationDtoGrid();
                var dcSubnetwork = x.Designcomponent?.Subnetworkboundary;

                var systemType = x.Designcomponent?.Systemtype;
                var hardWare = systemType?.Systemtypesmajorhardwarebuilds
                           ?.SingleOrDefault(m =>
                               m.Ismain && m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                               m.Deleted == false)?.Majorhardware;
                var Dcf = x.Designcomponent?.Designcomponentfamily;
                var Subnetwork = x.Designcomponent?.Designcomponentfamily?.Subnetworkboundary;
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
                grid.DesignComponentFamily = x.Designcomponent.toDesignComponentFamily();
                grid.SupportedService = string.Join(" | ", dcSubnetwork?.Subnetworksupportedsvr?.Select(x => x?.Service?.Description).Distinct() ?? new List<string>());
                grid.OperationsMaintenanceContract = x.Lcmengineering.Outputtolcmhardware;
                grid.LcmStatus = x.Lcmengineering.Lcmstatushardware;
                grid.LcmStatusOpsHardware = x.Lcmengineering.Lcmstatusopshardware;
                grid.LcmStatusEngHardware = x.Lcmengineering.Lcmstatusenghardware;

                #endregion

                grid.ReportId = $"{x.Lcmengineering.Resourcekey}-{x.Hwresourcekey}-0";
                grid.LcmEngineeringId = x.Lcmengineering.Lcmengineeringid;
                grid.DesignComponentIndex = x.Designcomponentid;
                grid.LocalMarket = x.Opco?.Opco;

                #region // Add Bag Details
                grid.BagName = _commonManager.GetBuildBagDescription(x.Buildbag);
                grid.ComponentName = ComponentBagExtensionMethod.GetComponentBagDescription(_repositoryWrapper, y?.Componentsoftwarebuildid);
                grid.ComponentResourceKey = ComponentBagExtensionMethod.GetComponentResourcekey(_repositoryWrapper, x.Opcoid, grid.ComponentName, x.Designcomponent.Designcomponentfamilyid);
                #endregion

                #region code optimize org Table

                grid.VerticalEngineeringTeam =
                 string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.VerticalDic != null && x.Deleted == false)
                 .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList());

                grid.VerticalSubDomain = string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.SubdomainresponsiblesDic != null && x.Deleted == false)
            .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList());

                grid.EngineeringContactPoint = _commonManager.GetEngContactPointFromEduAndSubDomainSpoc(allEdu?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && x.Deleted == false ).Select(t => t?.ContactEmail).ToList(),
                                allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && x.Deleted == false).Select(t => t?.ContactEmail).ToList());

                grid.OperationsContactPoint = string.Join(" | ",
                    allOperationalContract?.Where(m => m.LcmengineeringId == x.Lcmengineeringid).Select(x => x?.OperationDescription).Distinct());

                #endregion

                grid.AssetCategory = systemType?.Assetcategory?.Assetcategory;

                grid.AssetClass = hardWare?.Hardwaresolution;

                grid.AssetType = _commonManager.GetAssetType(hardWare?.Buildconstruction?.Buildconstruction);

                grid.AssetDescription = Subnetwork.Description;

                grid.ProductImportance = x.Lcmengineering.Productimportance?.Productimportance;
                grid.Vendor = hardWare?.Orgeqpmanufacturer
                    ?.Originalequipmentmanufacturer;
                grid.HardwareModel = systemType?.toLcmDbExportHardwareName();
                grid.NumberOfNodes = 1;

                grid.EOMStatus = hardWare != null
                           ? (EOMEnum)hardWare.Eomstatus : EOMEnum.NotSpecified;

                grid.VendorEndOfMaintenanceDate = hardWare?.Endofmaintenance;

                #region Ticket 465 - LCM export: RAG status field - filter is not working fine
                grid.RagStatus = LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(null,
                  data.Where(y => y.Lcmengineering.Lcmengineeringid == x.Lcmengineeringid).ToList(), 1
                  ).FirstOrDefault().Text.ToString();
                #endregion

                grid.VendorEndOfMaintenanceDateValue = hardWare?.Endofmaintenance != null ? hardWare?.Endofmaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : grid.EOMStatus == EOMEnum.NotAnnounced ? ConstantValueFilter.NotAnnounced : null;

                grid.TrackingNumberProjectName = x.Lcmengineering.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());
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


                grid.ManagedByGdc = ConstantValueFilter.No;

                grid.DesignComponentFamilyId = x.Designcomponent?.Designcomponentfamilyid;
                grid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";

                grid.Archived = x.Lcmengineering.Archived;

                #region LCM-R8-Phase1

                grid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper);
                grid.Criticality = dcSubnetwork?.Criticality;



                grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngHardware, grid.OperationsMaintenanceContract);
                grid.HwIsExtendedSupportOfferedByVendor = x.Lcmengineering.Hwisextendedsupportofferedbyvendor.HasValue && x.Lcmengineering.Hwisextendedsupportofferedbyvendor.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();
                grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus,
                                                                          grid.OperationsMaintenanceContract,
                                                                          grid.OpsMaintenanceConractEnd,
                                                                          grid.ProjectEndDate,
                                                                          grid.ProjectStatus);

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
                grid.SecurityRiskOverall = GetSecurityRiskOverAllValue(lcmAuditAttributes != null ? lcmAuditAttributes.Securityriskeffective : string.Empty, grid.SecurityRiskPotential);
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
                    grid.RequestID = lcmAuditAttributes.Requestid;
                    //grid.Id_New = lcmAuditAttributes.Idnew;
                    //grid.ProductImportanceHistory2 = lcmAuditAttributes.Productimportancehistory2;
                    //grid.LcmStatusJune2021 = lcmAuditAttributes.Lcmstatus;
                    grid.LastScanDate = lcmAuditAttributes.Lastscandate?.Date;
                    grid.LastScanDateValue = lcmAuditAttributes.Lastscandate;//?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    grid.AssetOutofScopeForReportingPurposes = lcmAuditAttributes.Assetoutofscope;//calculated filesd
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
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault(), hardWare.Endofmaintenance, _repositoryWrapper) : "";

                grid.Program = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.ProgramNavigation?.Programdescription;

                grid.ProjectOwner = x.Lcmengineering.PlannedactivitiesLcmengineering.AsQueryable()
                   .Where(PlannedActivityHardware).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                   ?.Projectowner;

                grid.HardwareProfile = _commonManager.GetHwProfile(grid.Hostname);

                return grid;
            })).ToList();

            if (!isLcmDBExportUpdated)
            {
                reportLastUpdateDate.Lasthwupdatedate = DateTime.Now.Date;
                _repositoryWrapper.LcmDBExportUpdateHistory.Update(reportLastUpdateDate);
                _repositoryWrapper.Save();
            }
            if (!isExport)
            {
                reports = reports.Skip((buildFilterDto.Page - 1) * buildFilterDto.PageSize).Take(buildFilterDto.PageSize).ToList();
            }
            else
            {
                reports = reports.ToList();
            }

            var rtn = new QueryResultDto<ReportHardwareConfigurationDtoGrid>(new GenerateRenderForGrid<ReportHardwareConfigurationDtoGrid>(_columnManager))
            {
                TotalItems = totalCount
            };

            rtn.Items = reports.ToArray();

            return rtn;
        }

        #endregion
    }
}


