using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Rules;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Common;
using CAM.DataTransferObjects.Entita.Report;
using CAM.Enum;
using Microsoft.AspNetCore.Http;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static CAM.BusinessManager.Rules.LCMEngineeringRulesExtension;

namespace CAM.BusinessManager.Entity.Report
{
    public class ReportExtensionManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;
        private CommonManager _commonManager;

        private readonly ICurrentUserService _currentUserService;

        private readonly string _engUpdateTracker = "To be started";
        bool isLcmDBExportUpdated = false;
        List<NetWorkElementAsPlannedSubDpomainSpoc> allSubDomain = new List<NetWorkElementAsPlannedSubDpomainSpoc>();
        List<NetWorkElementAsPlannedEduSpoc> allEdu = new List<NetWorkElementAsPlannedEduSpoc>();
        List<LcmOperationalContracts> allOperationalContract = new List<LcmOperationalContracts>();
        string paTypeFilterToHwOrSw = ConstantValueFilter.Hardware;

        public ReportExtensionManager(IEnumerable<IRepositoryWrapper> wrappers,
        GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, CommonManager commonManager,
        IRepositoryWrapper repositoryWrapper
        ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
            _commonManager = commonManager;
        }

        #region
       
        public ReportHardwareDtoGrid MapToHardWareReportGrid(Networkelementsasplanned x, List<NetWorkElementAsPlannedSubDpomainSpoc> subDomainSpoc
          ,List<NetWorkElementAsPlannedEduSpoc> eduSpoc, List<LcmOperationalContracts> operationalContract,bool isLcmDBUpdated, string paFilterToHwOrSw)
        {
            var grid = new ReportHardwareDtoGrid();
            paTypeFilterToHwOrSw = paFilterToHwOrSw;

            var lcmEngineering = x.Lcmengineering;
            var designComponent = x.Designcomponent;
            var plannedActivity = lcmEngineering.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(paTypeFilterToHwOrSw.ToUpper()); 
            var ipIdentities = x.Identitiesasis?.Where(p => p.Interfacetype == ConstantValueFilter.Management && p?.Category?.Description?.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress);
           
            allSubDomain = subDomainSpoc;
            allEdu = eduSpoc;
            allOperationalContract = operationalContract;
            isLcmDBExportUpdated = isLcmDBUpdated;

            grid.IpAddress = ipIdentities != null ? string.Join(" | ", ipIdentities?.Select(fx => fx.Value).Distinct()) : string.Empty;
            grid.Hostname = Convert.ToString(x.Elementname).Trim();
            grid.SerialNumber = x.Hwresourcekey;
            grid.DesignComponentIndex = x.Designcomponentid;
            grid.LocalMarket = x.Opco?.Opco;
            grid.ReportId = $"{lcmEngineering.Resourcekey}-{x.Hwresourcekey}-0";
            grid.Hostname = x.Elementname?.Trim();            
            grid.AssetServiceFunctionality = designComponent?.Designcomponentfamily?.Designaspects?.FirstOrDefault()?
                .Designaspectssupportedsvr?.Count() > 0 ? string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Designaspects?
                .FirstOrDefault()?.Designaspectssupportedsvr.Select(x => x?.Service?.Description).Distinct()) : string.Empty;

            #region Ticket 465 - LCM export: RAG status field - filter is not working fine
            var asset = new List<Networkelementsasplanned> { x };
            grid.RagStatus = LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(null,
                asset, 1).FirstOrDefault().Text.ToString();

            #endregion
            //****** Don't changes calling method details - grid column assigned to other grid column
            
            MapHardWareLibraryDetails(grid, designComponent, plannedActivity, lcmEngineering);
            MapHardWareLCMDetails(grid, lcmEngineering);
            MapRiskDetails(grid,null, plannedActivity);
            MapDesignContactPoints(grid,null, x);
            MapProjectDetails(grid, plannedActivity, lcmEngineering);
            MapHardwareLcmAncillaryData(grid, lcmEngineering);

          
            return grid;
        }

        private void MapHardWareLibraryDetails(ReportHardwareDtoGrid grid , Designcomponents designComponent, Plannedactivities plannedActivity, Lcmengineering lcmEngineering)
        {
            var systemType = designComponent?.Systemtype;
            var designcomponentfamily = designComponent?.Designcomponentfamily;
            var hardware = systemType?.Systemtypesmajorhardwarebuilds?.FirstOrDefault(m => m.Ismain == true && m.Systemtypeid
            == systemType.Systemtypeid && m.Deleted == false)?.Majorhardware;
            var dcSubnetwork = designComponent?.Subnetworkboundary;

            var majorSW = systemType?.Majorsoftwarebuilds;
 
            //System Type
            grid.AssetCategory = systemType?.Assetcategory?.Assetcategory;
            grid.HardwareModel = systemType?.toLcmDbExportHardwareName();
            grid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
            grid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper); //systemType?.VodafonenameNavigation?.Description;


            // HardWare
            grid.AssetClass = hardware?.Hardwaresolution;
            grid.AssetType = GetAssetType(hardware?.Buildconstruction?.Buildconstruction);
            grid.Vendor = hardware?.Orgeqpmanufacturer?.Originalequipmentmanufacturer;
            grid.EOMStatus = hardware != null ? (EOMEnum)hardware.Eomstatus : EOMEnum.NotSpecified;
            //grid.VendorEndOfMaintenanceDateValue = hardware?.Endofmaintenance?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            grid.MajorHardwareBuildId = hardware?.Majorhardwareid;
            grid.Platform = hardware?.Hardwaresolution;
            grid.IdentifiedAction = lcmEngineering.Archived != true ? GetIdentificationActionForLcmExport(lcmEngineering.PlannedactivitiesLcmengineering.AsQueryable()
                       .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault(), hardware.Endofmaintenance, _repositoryWrapper) : "";


            //DesignComponentFamily
            grid.AssetDescription = designcomponentfamily?.Description;

            //Designcomponent
            grid.SystemTypeId = designComponent.Systemtypeid;
            grid.DesignComponentId = designComponent.Designcomponentid;

            //Subnetwork 
            grid.GdprRelevant = dcSubnetwork.Gdprrelevant != null ? dcSubnetwork.Gdprrelevant == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper() : null;
            grid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";
            grid.Criticality = dcSubnetwork?.Criticality;
           
        }
        private void MapHardWareLCMDetails(ReportHardwareDtoGrid grid, Lcmengineering lcmEngineering )
        {
            grid.LcmEngineeringId = lcmEngineering.Lcmengineeringid;
            grid.ProductImportance = lcmEngineering.Productimportance?.Productimportance;
            grid.NumberOfNodesLcm = 1;
            grid.OpsMaintenanceConractEnd = lcmEngineering.Hardwareendofsupportcontract != null ? lcmEngineering.Hardwareendofsupportcontract : lcmEngineering.Softwareendofwarrantydate;
            grid.OpsMaintenanceConractEndValueLcm = grid.OpsMaintenanceConractEnd != null ? grid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

            grid.Archived = lcmEngineering.Archived;
            grid.HwIsExtendedSupportOfferedByVendor = lcmEngineering.Hwisextendedsupportofferedbyvendor.HasValue
                && lcmEngineering.Hwisextendedsupportofferedbyvendor.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();

            if (!isLcmDBExportUpdated)
            {
                var lcmU = lcmEngineering;
 
                    lcmU.Outputtolcmhardware = lcmEngineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareOutput).Result;
                    lcmU.Lcmstatushardware = lcmEngineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatus).Result;
                    lcmU.Lcmstatusopshardware = lcmEngineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatusOps).Result;
                    lcmU.Lcmstatusenghardware = lcmEngineering.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatusEng).Result;
                _repositoryWrapper.Lcmengineering.Update(lcmU);
                _repositoryWrapper.Save();
                _repositoryWrapper.ClearTracker();

            }
            grid.OperationsMaintenanceContractLcm = lcmEngineering.Outputtolcmhardware;
            grid.LcmStatusOpsHardware = lcmEngineering.Lcmstatusopshardware;
            grid.LcmStatusEngHardware = lcmEngineering.Lcmstatusenghardware;
            grid.EngKpi2 = GetEngKpi2(grid.LcmStatusEngHardware, grid.OperationsMaintenanceContractLcm);
            //grid.LcmStatus = lcmEngineering.Lcmstatushardware;

        } 
        private void MapDesignContactPoints(ReportHardwareDtoGrid grid,ReportSoftwareDtoGrid swGrid, Networkelementsasplanned x)
        {
            var networkElementId = x.Networkelementasplannedid;

           string VerticalEngineeringTeam = string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == networkElementId && m.VerticalDic != null && x.Deleted == false).SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct());

            string VerticalSubDomain = string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == networkElementId && m.SubdomainresponsiblesDic != null && x.Deleted == false).SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct());

            string EngineeringContactPoint = _commonManager.GetEngContactPointFromEduAndSubDomainSpoc(
            allEdu?.Where(m => m.NetWorkElementAsPlannedId == networkElementId && x.Deleted == false).Select(t => t.ContactEmail).ToList(),
            allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == networkElementId && x.Deleted == false).Select(t => t.ContactEmail).ToList());

            string OperationsContactPoint = string.Join(" | ", allOperationalContract?.Where(m => m.LcmengineeringId == x.Lcmengineeringid).Select(contract => contract?.OperationDescription).Distinct());


            if (paTypeFilterToHwOrSw == ConstantValueFilter.Hardware)
            {
                grid.VerticalEngineeringTeam = VerticalEngineeringTeam;
                grid.VerticalSubDomain = VerticalSubDomain;
                grid.EngineeringContactPoint = EngineeringContactPoint;
                grid.OperationsContactPoint = OperationsContactPoint;

            }
            else if (paTypeFilterToHwOrSw == ConstantValueFilter.Software)
            {
                swGrid.VerticalEngineeringTeam = VerticalEngineeringTeam;
                swGrid.VerticalSubDomain = VerticalSubDomain;
                swGrid.EngineeringContactPoint = EngineeringContactPoint;
                swGrid.OperationsContactPoint = OperationsContactPoint;

            }
        }

        private void MapProjectDetails(ReportHardwareDtoGrid grid, Plannedactivities plannedActivity, Lcmengineering lcmEngineering)
        {
            grid.ProjectStatus = plannedActivity?.Projectstatus;
            grid.ProjectEndDateValue = plannedActivity?.Plannedcompletion?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);          
            grid.Notes = plannedActivity?.Notes;
            grid.DescriptionOfPlannedAction = plannedActivity?.Plannedactivityresource?.Plannedactivityresource;
            grid.PlannedHardwareModel = plannedActivity?.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.HardwareUpgrade ? plannedActivity?.Designcomponent?.Systemtype.toLcmDbExportHardwareName() : string.Empty;
            grid.ProjectEndDate = plannedActivity?.Plannedcompletion?.Date;

            grid.PlannedActivityId = plannedActivity?.Plannedactivityid;
            //grid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(grid.LcmStatus, grid.OperationsMaintenanceContract, grid.OpsMaintenanceConractEnd, grid.ProjectEndDate, grid.ProjectStatus);
            grid.DeliveryPlanAvailable = plannedActivity?.Deliveryplanavailable != null && plannedActivity.Deliveryplanavailable ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();

            grid.WbsCode = GetWbsCode(plannedActivity?.Deliveryprojectname);
            grid.BptID = plannedActivity?.Budgettrackingid;
            grid.PpmID = plannedActivity?.Deliveryprojectid;
            grid.MainOrganization = ConstantValueFilter.Nse.ToUpper();
            grid.BundleBudget = GetBundleBudget(plannedActivity?.Budgettrackingid);
            grid.BundleId = GetBundleBudget(plannedActivity?.Budgettrackingid) == ConstantValueFilter.Yes ? plannedActivity.Budgettrackingid.Substring(2) : null;
            grid.ManagedByGdc = ConstantValueFilter.No;

            grid.TrackingNumberProjectNameLcm = lcmEngineering.PlannedactivitiesLcmengineering?.GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());
            grid.ProgramLcm = lcmEngineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                        ?.ProgramNavigation?.Programdescription;

            grid.ProjectOwner = lcmEngineering.PlannedactivitiesLcmengineering.AsQueryable()
               .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Hardware.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
               ?.Projectowner;
           
            grid.BudgetEstimated = lcmEngineering.PlannedactivitiesLcmengineering?.GetPlannedActivityBudgetEstimated(ConstantValueFilter.Hardware.ToUpper());
        }

        private void MapHardwareLcmAncillaryData(ReportHardwareDtoGrid grid, Lcmengineering lcmEngineering)
        {
            var lcmAuditAttributes = lcmEngineering.Lcmancillarydata?.FirstOrDefault();

            grid.SecurityRiskPotential = lcmEngineering.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames?.Where(x => x.Riskcluster != null)?.Select(x => x.Riskcluster?.Risklevel)?.FirstOrDefault();

            grid.SecurityRiskOverallLcm = GetSecurityRiskOverAllValue(
            lcmAuditAttributes?.Securityriskeffective ?? string.Empty, grid.SecurityRiskPotential);

            if (lcmAuditAttributes != null)
            {
                grid.ReasonfornoPlan = lcmAuditAttributes.Reasonfornoplan;
                grid.CommentonProjectStatus = lcmAuditAttributes.Commentonprojectstatus;
                grid.SecurityRiskEffective = lcmAuditAttributes.Securityriskeffective;
                grid.SecurityMitigation = lcmAuditAttributes.Securitymitigation;
                grid.IncludedinSecurityScanning = lcmAuditAttributes.Includedinsecurityscanning;
                grid.LastScanDateValue = lcmAuditAttributes.Lastscandate; //?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                grid.RaId = lcmAuditAttributes.Raid;
                grid.RequestIDLcm = lcmAuditAttributes.Requestid;

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
                grid.DataSourceLcm = lcmAuditAttributes.Datasource;
                grid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;
                grid.OriginalHwLcmId = lcmAuditAttributes.Originalhwlcmid;
                #region Ticket 551
                //719 Regulatory Fields Changes
                grid.IsPecn = lcmAuditAttributes.Ispecn;
                grid.IsPecs = lcmAuditAttributes.Ispecs;
                grid.IsScf = lcmAuditAttributes.Isscf;
                grid.IsNof = lcmAuditAttributes.Isnof;
                #endregion

                grid.ExternalFacingFlag = lcmAuditAttributes.Externalfacingflag != null ?
                lcmAuditAttributes.Externalfacingflag.Value.ToString() : "";
                grid.InfrastructureLocation = lcmAuditAttributes.Locationinfrastructure;
                grid.ExposedEdgeFlag = GetExposedEdgeValue(lcmAuditAttributes.Isexposededge);
            }

            #region Apr 03 requirement
            grid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
        ? lcmAuditAttributes.Handedovertooperation : true;
            grid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
           ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;
            #endregion

        }
        #endregion

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

        #region Software
        //-------------------------------- Software -----------------------------------------------/
        public ReportSoftwareDtoGrid MapToSoftWareReportGrid(Networkelementsasplanned x, List<NetWorkElementAsPlannedSubDpomainSpoc> subDomainSpoc
               , List<NetWorkElementAsPlannedEduSpoc> eduSpoc, List<LcmOperationalContracts> operationalContract, bool isLcmDBUpdated, string paFilterToHwOrSw)
        {
                 var swGrid = new ReportSoftwareDtoGrid();
            paTypeFilterToHwOrSw = paFilterToHwOrSw;

            var lcmEngineering = x.Lcmengineering;
            var designComponent = x.Designcomponent;
            var plannedActivity = lcmEngineering.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(paTypeFilterToHwOrSw.ToUpper());
            var ipIdentities = x.Identitiesasis?.Where(p => p.Interfacetype == ConstantValueFilter.Management && p?.Category?.Description?.ToLower().Replace(" ", "") == ConstantValueFilter.IPAddress);

            allSubDomain = subDomainSpoc;
            allEdu = eduSpoc;
            allOperationalContract = operationalContract;
            isLcmDBExportUpdated = isLcmDBUpdated;

            swGrid.IpAddress = ipIdentities != null ? string.Join(" | ", ipIdentities?.Select(fx => fx.Value).Distinct()) : string.Empty;
            swGrid.Hostname = Convert.ToString(x.Elementname).Trim();
            swGrid.SerialNumber = x.Hwresourcekey;
            swGrid.DesignComponentIndex = x.Designcomponentid;
            swGrid.LocalMarket = x.Opco?.Opco;
            swGrid.ReportId = $"{lcmEngineering.Resourcekey}-{x.Swresourcekey}-0";

            swGrid.Hostname = x.Elementname?.Trim();            
            swGrid.AssetServiceFunctionality = designComponent?.Designcomponentfamily?.Designaspects?.FirstOrDefault()?
                .Designaspectssupportedsvr?.Count() > 0 ? string.Join(" | ", x?.Designcomponent?.Designcomponentfamily?.Designaspects?
                .FirstOrDefault()?.Designaspectssupportedsvr.Select(x => x?.Service?.Description).Distinct()) : string.Empty;

            #region Ticket 465 - LCM export: RAG status field - filter is not working fine
            var asset = new List<Networkelementsasplanned> { x };
            swGrid.RagStatus = LcmEngineeringExtensionMethod.disAggregatedRagStatusFilterRecord(null,
                asset, 2).FirstOrDefault().Text.ToString();

            #endregion
            //****** Don't changes calling method details - grid column assigned to other grid column

            MapSoftwareLibraryDetails(swGrid, designComponent, plannedActivity, lcmEngineering);
            MapSoftWareLCMDetails(swGrid, lcmEngineering);
            MapRiskDetails(null, swGrid, plannedActivity);
            MapDesignContactPoints(null, swGrid, x);
            MapSoftwareProjectDetails(swGrid, plannedActivity, lcmEngineering);
            MapSoftwareLcmAncillaryData(swGrid, lcmEngineering);


            return swGrid;
        }

        private void MapSoftWareLCMDetails(ReportSoftwareDtoGrid swGrid, Lcmengineering lcmEngineering )
        {
            swGrid.LcmEngineeringId = lcmEngineering.Lcmengineeringid;
            swGrid.ProductImportance = lcmEngineering.Productimportance?.Productimportance;
            swGrid.NumberOfNodesLcm = 1;
            swGrid.OpsMaintenanceConractEnd = lcmEngineering.Softwareendofwarrantydate != null ? lcmEngineering.Softwareendofwarrantydate : lcmEngineering.Softwareendofsupportcontract;
            swGrid.OpsMaintenanceConractEndValueLcm = swGrid.OpsMaintenanceConractEnd != null ? swGrid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;
            swGrid.VendorEndOfVulnerabilitySecuritySupportDateValueLcm = swGrid.OpsMaintenanceConractEnd != null ? swGrid.OpsMaintenanceConractEnd.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;

            swGrid.Archived = lcmEngineering.Archived;
            swGrid.IsExtendedSupportOfferedByVendor = lcmEngineering.Isextendedsupportofferedbyvendor.HasValue && lcmEngineering.Isextendedsupportofferedbyvendor.Value ? ConstantValueFilter.YES : ConstantValueFilter.NO;


            if (!isLcmDBExportUpdated)
            {
                var lcmU = lcmEngineering;
 
                    lcmU.Outputtolcmsoftware = lcmEngineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareOutput).Result;
                    lcmU.Lcmstatussoftware = lcmEngineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatus).Result;
                    lcmU.Lcmstatusopssoftware = lcmEngineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusOps).Result;
                    lcmU.Lcmstatusengsoftware = lcmEngineering.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusEng).Result;
                                
                _repositoryWrapper.Lcmengineering.Update(lcmU);
                _repositoryWrapper.Save();
                _repositoryWrapper.ClearTracker();

            }
            swGrid.LcmStatus = lcmEngineering.Lcmstatussoftware;
            swGrid.OperationsMaintenanceContractLcm = lcmEngineering.Outputtolcmsoftware;
            swGrid.LcmStatusOpsSoftware = lcmEngineering.Lcmstatusopssoftware;
            swGrid.LcmStatusEngSoftware = lcmEngineering.Lcmstatusengsoftware;
            swGrid.LcmStatus = lcmEngineering.Lcmstatushardware;
            swGrid.EngKpi2 = GetEngKpi2(swGrid.LcmStatusEngSoftware, swGrid.OperationsMaintenanceContractLcm);

        }
        private void MapSoftwareLibraryDetails(  ReportSoftwareDtoGrid swGrid, Designcomponents designComponent, Plannedactivities plannedActivity, Lcmengineering lcmEngineering)
        {
            var systemType = designComponent?.Systemtype;
            var designcomponentfamily = designComponent?.Designcomponentfamily;
            var hardware = systemType?.Systemtypesmajorhardwarebuilds?.FirstOrDefault(m => m.Ismain == true && m.Systemtypeid
            == systemType.Systemtypeid && m.Deleted == false)?.Majorhardware;
            var dcSubnetwork = designComponent?.Subnetworkboundary;
            var majorSW = systemType?.Majorsoftwarebuilds;

            //System Type
            swGrid.AssetCategory = systemType?.Assetcategory?.Assetcategory;
            swGrid.HardwareModel = hardware.Buildconstruction?.Rule == (int)BuildconstructionRuleEnum.VirtualHW ? ConstantValueFilter.Virtualized
                : systemType?.toLcmDbExportHardwareName();
            swGrid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
            swGrid.RiskCluster = GetRiskCluster(systemType?.VodafonenameNavigation?.Id, _repositoryWrapper); 


            // Software and HardWare
            swGrid.AssetClass = majorSW.Productname != null ? majorSW.Productname?.Description : "";
            swGrid.AssetType = majorSW.Criticalassettype?.Description;
            swGrid.Vendor = majorSW.Orgeqpmanufacturer ?.Originalequipmentmanufacturer;
            swGrid.EOMStatus = majorSW != null ? (EOMEnum)majorSW.Eomstatus : EOMEnum.NotSpecified;
            swGrid.VendorEndOfMaintenanceDate = majorSW.Endofmaintenance != null  ? majorSW.Endofmaintenance?.Date : null;
            swGrid.VendorEndOfMaintenanceDateValue = majorSW.Endofmaintenance != null
               ? majorSW.Endofmaintenance?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : swGrid.EOMStatus == EOMEnum.NotAnnounced ? "Not Announced" : null;
            swGrid.SoftwareVersion = majorSW?.Softwareversion;

            swGrid.MajorSoftwareBuildId = systemType?.Majorsoftwarebuildsid;
            swGrid.Platform = majorSW.Productname != null ?  majorSW.Productname?.Description : "";

            var Endofmaintenance = majorSW?.Endofmaintenance;
            swGrid.IdentifiedAction = lcmEngineering.Archived != true ? GetIdentificationActionForLcmExport(lcmEngineering.PlannedactivitiesLcmengineering.AsQueryable()
                       .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software.ToLower()))
                       .OrderBy(x => x.Plannedcompletion).FirstOrDefault(), Endofmaintenance, _repositoryWrapper) : "";
            swGrid.MajorHardwareBuildId = hardware?.Majorhardwareid;
            swGrid.AssetVirtualized =
                hardware?.Buildconstruction?.Buildconstruction.ToLower().Replace(" ", "") == ConstantValueFilter.BluprintNfvi
                || hardware?.Buildconstruction?.Buildconstruction.ToLower().Replace(" ", "") == ConstantValueFilter.BluprintNfci
                || hardware?.Buildconstruction?.Buildconstruction.ToLower().Replace(" ", "") == ConstantValueFilter.OtherNfvi
                || hardware?.Buildconstruction?.Buildconstruction.ToLower().Replace(" ", "") == ConstantValueFilter.OtherNfci
                ? ConstantValueFilter.YES
                : ConstantValueFilter.NO;
            //DesignComponentFamily
            swGrid.AssetDescription = designcomponentfamily?.Description;
            swGrid.DesignComponentFamilyId = designcomponentfamily?.Designcomponentfamilyid;
            //Designcomponent
            swGrid.SystemTypeId = designComponent.Systemtypeid;
            swGrid.DesignComponentId = designComponent.Designcomponentid;

            //Subnetwork 
            swGrid.GdprRelevant = dcSubnetwork.Gdprrelevant != null ? dcSubnetwork.Gdprrelevant == true ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper() : null;
            swGrid.TypeOfNetworkElement = dcSubnetwork?.Lcmpolicy != null ? ((LCMPolicy)(dcSubnetwork?.Lcmpolicy.Value)).ToString() : "";
            swGrid.Criticality = dcSubnetwork?.Criticality;

        }
        
        private void MapRiskDetails(ReportHardwareDtoGrid grid,ReportSoftwareDtoGrid swGrid, Plannedactivities plannedActivity)
        {
            if(paTypeFilterToHwOrSw == ConstantValueFilter.Hardware)
            {
                grid.OpsRiskEvaluation = GetRiskValue(plannedActivity?.Operationalrisk?.Description);
                grid.OverallRiskEvaluationLcm = plannedActivity?.Overallriskevaluation;
                grid.EngRiskEvaluation = GetRiskValue(plannedActivity?.Engineeringrisk?.Description);
                grid.OpsRiskEvaluationNotes = plannedActivity?.Riskoperationalnotes;
                grid.EngRiskEvaluationNotes = plannedActivity?.Riskengineeringnotes;
            }
            else if (paTypeFilterToHwOrSw == ConstantValueFilter.Software)
            {
                swGrid.OpsRiskEvaluation = GetRiskValue(plannedActivity?.Operationalrisk?.Description);
                swGrid.OverallRiskEvaluationLcm = plannedActivity?.Overallriskevaluation;
                swGrid.EngRiskEvaluation = GetRiskValue(plannedActivity?.Engineeringrisk?.Description);
                swGrid.OpsRiskEvaluationNotes = plannedActivity?.Riskoperationalnotes;
                swGrid.EngRiskEvaluationNotes = plannedActivity?.Riskengineeringnotes;
            }
           

        }

        private void MapSoftwareProjectDetails(ReportSoftwareDtoGrid swGrid, Plannedactivities plannedActivity, Lcmengineering lcmEngineering)
        {
            swGrid.ProjectStatus = plannedActivity?.Projectstatus;
            swGrid.ProjectEndDateValue = plannedActivity?.Plannedcompletion?.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            swGrid.Notes = plannedActivity?.Notes;
            swGrid.DescriptionOfPlannedAction = plannedActivity?.Plannedactivityresource?.Plannedactivityresource;
            swGrid.PlannedSoftwareVersion = plannedActivity?.Activitydetails ?? ConstantValueFilter.Na;
            swGrid.ProjectEndDate = plannedActivity?.Plannedcompletion?.Date;

            swGrid.PlannedActivityId = plannedActivity?.Plannedactivityid;
            swGrid.ExpLCMstatusatendofFY24 = GetExpLCMstatusatendofFY24(swGrid.LcmStatus, swGrid.OperationsMaintenanceContractLcm, swGrid.OpsMaintenanceConractEnd,
                swGrid.ProjectEndDate, swGrid.ProjectStatus);            
            swGrid.DeliveryPlanAvailable = plannedActivity?.Deliveryplanavailable != null && plannedActivity.Deliveryplanavailable ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper();

            swGrid.WbsCode = GetWbsCode(plannedActivity?.Deliveryprojectname);
            swGrid.BptID = plannedActivity?.Budgettrackingid;
            swGrid.PpmID = plannedActivity?.Deliveryprojectid;
            swGrid.MainOrganization = ConstantValueFilter.Nse;
            swGrid.BundleBudget = GetBundleBudget(plannedActivity?.Budgettrackingid);
            swGrid.BundleId = GetBundleBudget(plannedActivity?.Budgettrackingid) == ConstantValueFilter.Yes ? plannedActivity.Budgettrackingid.Substring(2) : null;
            swGrid.ManagedByGdc = ConstantValueFilter.No;

            swGrid.TrackingNumberProjectNameLcm = lcmEngineering.PlannedactivitiesLcmengineering?
                .GetPlannedActivityTrakingNumberProjectNameBudgetEstimated(ConstantValueFilter.Software.ToUpper());
            swGrid.ProgramLcm = lcmEngineering.PlannedactivitiesLcmengineering.AsQueryable()
                        .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
                        ?.ProgramNavigation?.Programdescription;

            swGrid.ProjectOwner = lcmEngineering.PlannedactivitiesLcmengineering.AsQueryable()
               .Where(PlannedActivitiesExtensionMethod.FiltroPlannedPerWhere(ConstantValueFilter.Software.ToLower())).OrderBy(x => x.Plannedcompletion).FirstOrDefault()
               ?.Projectowner;

            swGrid.BudgetEstimated = lcmEngineering.PlannedactivitiesLcmengineering?.GetPlannedActivityBudgetEstimated(ConstantValueFilter.Software.ToUpper());
        }

        private void MapSoftwareLcmAncillaryData(ReportSoftwareDtoGrid swGrid, Lcmengineering lcmEngineering)
        {
            var lcmAuditAttributes = lcmEngineering.Lcmancillarydata?.FirstOrDefault();

            swGrid.SecurityRiskPotential = lcmEngineering.Designcomponent?.Systemtype?.VodafonenameNavigation?.Riskclustervodafonenames?.Where(x => x.Riskcluster != null)?.Select(x => x.Riskcluster?.Risklevel)?.FirstOrDefault();
            swGrid.SecurityRiskOverallLcm = GetSecurityRiskOverAllValue(
            lcmAuditAttributes?.Securityriskeffective ?? string.Empty, swGrid.SecurityRiskPotential);

            if (lcmAuditAttributes != null)
            {
                swGrid.ReasonfornoPlan = lcmAuditAttributes.Reasonfornoplan;
                swGrid.CommentonProjectStatus = lcmAuditAttributes.Commentonprojectstatus;
                swGrid.SecurityRiskEffective = lcmAuditAttributes.Securityriskeffective;
                swGrid.SecurityMitigation = lcmAuditAttributes.Securitymitigation;
                swGrid.IncludedinSecurityScanning = lcmAuditAttributes.Includedinsecurityscanning;
                swGrid.RaId = lcmAuditAttributes.Raid;
                swGrid.RequestIDLcm = lcmAuditAttributes.Requestid;
                swGrid.LastScanDate = lcmAuditAttributes.Lastscandate?.Date;
                swGrid.LastScanDateValue = lcmAuditAttributes.Lastscandate;
                swGrid.AssetOutofScopeForReportingPurposes = lcmAuditAttributes.Assetoutofscope;//calculated filesd
                swGrid.LastUpgradeDate = lcmAuditAttributes.Lastupgradedate?.Date;
                swGrid.LastUpgradeDateValue = lcmAuditAttributes.Lastupgradedate;
                swGrid.EomControl = lcmAuditAttributes.Eomcontrol;
                swGrid.EngUpdateTracker = lcmAuditAttributes.Engupdatetracker;
                swGrid.OpsUpdateTracker = lcmAuditAttributes.Opsupdatetracker;
                swGrid.ExNetworks = lcmAuditAttributes.Exnetworks;
                swGrid.NewopsRiskEvaluation = GetNewOpsRiskEvaluationValue(lcmAuditAttributes.Incidentclass, lcmAuditAttributes.Occurenceprobability);
                swGrid.OccurrenceProbability = lcmAuditAttributes.Occurenceprobability;
                swGrid.IncidentClass = lcmAuditAttributes.Incidentclass;
                swGrid.ProductCode = lcmAuditAttributes.Productcode;
                swGrid.HandedOverToOperation = lcmAuditAttributes.Handedovertooperation;
                swGrid.ContractRenewalPlan = lcmAuditAttributes.Contractrenewalplan;
                swGrid.DataSourceLcm = lcmAuditAttributes.Datasource;
                swGrid.ScopeOfSimplification = lcmAuditAttributes.Scopeofsimplification;
                swGrid.OriginalSwLcmId = lcmAuditAttributes.Originalswlcmid;
                swGrid.LabSWRelease = "";
                #region Ticket 551
                #region //719 Regulatory Fields Changes
                swGrid.IsPecn = lcmAuditAttributes.Ispecn;
                swGrid.IsPecs = lcmAuditAttributes.Ispecs;
                swGrid.IsScf = lcmAuditAttributes.Isscf;
                swGrid.IsNof = lcmAuditAttributes.Isnof;
                #endregion               
                swGrid.ExternalFacingFlag = lcmAuditAttributes.Externalfacingflag != null ?
                lcmAuditAttributes.Externalfacingflag.Value.ToString() : "";
                #endregion
            }

            #region Apr 03 requirement
            swGrid.HandedOverToOperation = lcmAuditAttributes?.Handedovertooperation != null
        ? lcmAuditAttributes.Handedovertooperation : true;
            swGrid.EngUpdateTracker = lcmAuditAttributes?.Engupdatetracker != null
           ? lcmAuditAttributes.Engupdatetracker : _engUpdateTracker;
            #endregion

        }
        #endregion

    }
}