using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AssetPassThrough;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models.PassThroughData;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace CAM.BusinessManager.Entity.PassThroughData
{
    public class SwPassThroughLcmManager : BaseManager
    {
        private readonly ModelContext _modelContext;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        private DropdownDataServiceManager _dropdownmanager;

        public SwPassThroughLcmManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager, DropdownDataServiceManager dropdownmanager,
             IHttpContextAccessor contextAccessor,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers,out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _dropdownmanager = dropdownmanager;
        }


        #region //UiMemberFunctionsForLCMSoftware
        public QueryResultDto<PassThroughLcmSoftwareDtoGrid> FindWithCondition(PassThroughLcmQueryDto passThroughLcmQueryDto)
        {
            var predicateResult = ApplyFilter(passThroughLcmQueryDto);
            var rtn = new QueryResultDto<PassThroughLcmSoftwareDtoGrid>(new GenerateRenderForGrid<PassThroughLcmSoftwareDtoGrid>(_manager))
            {

            };
            var query = GetQuery(predicateResult);
            var data = query.ApplyOrdering(passThroughLcmQueryDto, GetColumnsMap()).ApplyPaging(passThroughLcmQueryDto).ToList();
                     
            IEnumerable <PassThroughLcmSoftwareDtoGrid> passThroughDtoGrid;

            passThroughDtoGrid = _mapper.Map<IEnumerable<PassThroughLcmSoftwareDtoGrid>>(data);

            rtn.Items = passThroughDtoGrid.ToArray();
            rtn.TotalItems = query.Count();
            return rtn;
        }


        public virtual ExpressionStarter<Swpassthroughlcm> ApplyFilter(PassThroughLcmQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Swpassthroughlcm>(true);
            var predicateInner = PredicateBuilder.New<Swpassthroughlcm>(true);
            if (buildFilterDto.NonTemsVertical != null && buildFilterDto.NonTemsVertical.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.NonTemsVertical)
                    predicateInner.Or(x => x.Nontemsvertical == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ReportId != null && buildFilterDto.ReportId.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ReportId)
                    predicateInner.Or(x => x.Reportid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SwResourceKeyPassThrough != null && buildFilterDto.SwResourceKeyPassThrough.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SwResourceKeyPassThrough)
                    predicateInner.Or(x => x.Swresourcekey == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalMarket != null && buildFilterDto.LocalMarket.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.LocalMarket)
                    predicateInner.Or(x => x.Localmarket == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VerticalEngineeringTeam != null && buildFilterDto.VerticalEngineeringTeam.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.VerticalEngineeringTeam)
                    predicateInner.Or(x => x.Verticalengineeringteam == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VerticalSubDomain != null && buildFilterDto.VerticalSubDomain.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.VerticalSubDomain)
                    predicateInner.Or(x => x.Verticalsubdomain == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EngineeringContactPoint != null && buildFilterDto.EngineeringContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EngineeringContactPoint)
                    predicateInner.Or(x => x.Engineeringcontactpoint == item);
                predicateResult.And(predicateInner);
            }
            #region Sofware
            if (buildFilterDto.SWOperationsContactPoint != null && buildFilterDto.SWOperationsContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SWOperationsContactPoint)
                    predicateInner.Or(x => x.Swoperationscontactpoint == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SWVendor != null && buildFilterDto.SWVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SWVendor)
                    predicateInner.Or(x => x.Swvendor == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareVersion != null && buildFilterDto.SoftwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SoftwareVersion)
                    predicateInner.Or(x => x.Softwareversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SWOperationsMaintenanceContract != null && buildFilterDto.SWOperationsMaintenanceContract.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SWOperationsMaintenanceContract)
                    predicateInner.Or(x => x.Swoperationsmaintenancecontract == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDate != null && buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDate.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.VendorEndOfVulnerabilitySecuritySupportDate)
                    predicateInner.Or(x => x.Vendorendofvulnerabilitysecuritysupportdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LcmStatusEngSoftware != null && buildFilterDto.LcmStatusEngSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.LcmStatusEngSoftware)
                    predicateInner.Or(x => x.Lcmstatusengsoftware == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LcmStatusOpsSoftware != null && buildFilterDto.LcmStatusOpsSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.LcmStatusOpsSoftware)
                    predicateInner.Or(x => x.Lcmstatusopssoftware == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SWOpsMaintenanceConractEndDate != null && buildFilterDto.SWOpsMaintenanceConractEndDate.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SWOpsMaintenanceConractEndDate)
                    predicateInner.Or(x => x.Swopsmaintenanceconractenddate == item);
                predicateResult.And(predicateInner);
            }
            #endregion

            if (buildFilterDto.AssetCategory != null && buildFilterDto.AssetCategory.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.AssetCategory)
                    predicateInner.Or(x => x.Assetcategory == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetClass != null && buildFilterDto.AssetClass.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.AssetClass)
                    predicateInner.Or(x => x.Assetclass == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetType != null && buildFilterDto.AssetType.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.AssetType)
                    predicateInner.Or(x => x.Assettype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetDescription != null && buildFilterDto.AssetDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.AssetDescription)
                    predicateInner.Or(x => x.Assetdescription == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetVirtualized != null && buildFilterDto.AssetVirtualized.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.AssetVirtualized)
                    predicateInner.Or(x => x.Assetvirtualized == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductImportance != null && buildFilterDto.ProductImportance.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ProductImportance)
                    predicateInner.Or(x => x.Productimportance == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardwareModel != null && buildFilterDto.HardwareModel.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.HardwareModel)
                    predicateInner.Or(x => x.Hardwaremodel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductCode != null && buildFilterDto.ProductCode.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ProductCode)
                    predicateInner.Or(x => x.Productcode == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NumberOfNodes != null && buildFilterDto.NumberOfNodes.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.NumberOfNodes)
                    predicateInner.Or(x => x.Numberofnodes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HandedOverToOperation != null && buildFilterDto.HandedOverToOperation.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.HandedOverToOperation)
                    predicateInner.Or(x => x.Handedovertooperation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LcmStatus != null && buildFilterDto.LcmStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.LcmStatus)
                    predicateInner.Or(x => x.Lcmstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IdentifiedAction != null && buildFilterDto.IdentifiedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.IdentifiedAction)
                    predicateInner.Or(x => x.Identifiedaction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DescriptionOfPlannedAction != null && buildFilterDto.DescriptionOfPlannedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.DescriptionOfPlannedAction)
                    predicateInner.Or(x => x.Descriptionofplannedaction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedSoftwareVersion != null && buildFilterDto.PlannedSoftwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.PlannedSoftwareVersion)
                    predicateInner.Or(x => x.Plannedsoftwareversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProjectStatus != null && buildFilterDto.ProjectStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ProjectStatus)
                    predicateInner.Or(x => x.Projectstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ReasonfornoPlan != null && buildFilterDto.ReasonfornoPlan.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ReasonfornoPlan)
                    predicateInner.Or(x => x.Reasonfornoplan == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CommentonProjectStatus != null && buildFilterDto.CommentonProjectStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.CommentonProjectStatus)
                    predicateInner.Or(x => x.Commentonprojectstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProjectEndDate != null && buildFilterDto.ProjectEndDate.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ProjectEndDate)
                    predicateInner.Or(x => x.Projectenddate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RagStatus != null && buildFilterDto.RagStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.RagStatus)
                    predicateInner.Or(x => x.Ragstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TrackingNumberProjectName != null && buildFilterDto.TrackingNumberProjectName.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.TrackingNumberProjectName)
                    predicateInner.Or(x => x.Trackingnumberprojectname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Program != null && buildFilterDto.Program.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.Program)
                    predicateInner.Or(x => x.Program == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.WbsCode != null && buildFilterDto.WbsCode.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.WbsCode)
                    predicateInner.Or(x => x.Wbscode == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BptID != null && buildFilterDto.BptID.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.BptID)
                    predicateInner.Or(x => x.Bptid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PpmID != null && buildFilterDto.PpmID.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.PpmID)
                    predicateInner.Or(x => x.Ppmid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ScopeOfSimplification != null && buildFilterDto.ScopeOfSimplification.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ScopeOfSimplification)
                    predicateInner.Or(x => x.Scopeofsimplification == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DataSource != null && buildFilterDto.DataSource.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.DataSource)
                    predicateInner.Or(x => x.Datasource == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProjectOwner != null && buildFilterDto.ProjectOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ProjectOwner)
                    predicateInner.Or(x => x.Projectowner == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BudgetEstimated != null && buildFilterDto.BudgetEstimated.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.BudgetEstimated)
                    predicateInner.Or(x => x.Budgetestimated == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Notes != null && buildFilterDto.Notes.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.Notes)
                    predicateInner.Or(x => x.Notes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BundleBudget != null && buildFilterDto.BundleBudget.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.BundleBudget)
                    predicateInner.Or(x => x.Bundlebudget == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BundleId != null && buildFilterDto.BundleId.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.BundleId)
                    predicateInner.Or(x => x.Bundleid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetServiceFunctionality != null && buildFilterDto.AssetServiceFunctionality.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.AssetServiceFunctionality)
                    predicateInner.Or(x => x.Assetservicefunctionality == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Platform != null && buildFilterDto.Platform.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.Platform)
                    predicateInner.Or(x => x.Platform == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EngRiskEvaluation != null && buildFilterDto.EngRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EngRiskEvaluation)
                    predicateInner.Or(x => x.Engriskevaluation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EngRiskEvaluationNotes != null && buildFilterDto.EngRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EngRiskEvaluationNotes)
                    predicateInner.Or(x => x.Engriskevaluationnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpsRiskEvaluation != null && buildFilterDto.OpsRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.OpsRiskEvaluation)
                    predicateInner.Or(x => x.Opsriskevaluation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpsRiskEvaluationNotes != null && buildFilterDto.OpsRiskEvaluationNotes.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.OpsRiskEvaluationNotes)
                    predicateInner.Or(x => x.Opsriskevaluationnotes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IncidentClass != null && buildFilterDto.IncidentClass.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.IncidentClass)
                    predicateInner.Or(x => x.Incidentclass == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OccurrenceProbability != null && buildFilterDto.OccurrenceProbability.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.OccurrenceProbability)
                    predicateInner.Or(x => x.Occurrenceprobability == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NewopsRiskEvaluation != null && buildFilterDto.NewopsRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.NewopsRiskEvaluation)
                    predicateInner.Or(x => x.Newopsriskevaluation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OverallRiskEvaluation != null && buildFilterDto.OverallRiskEvaluation.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.OverallRiskEvaluation)
                    predicateInner.Or(x => x.Overallriskevaluation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RiskCluster != null && buildFilterDto.RiskCluster.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.RiskCluster)
                    predicateInner.Or(x => x.Riskcluster == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SecurityRiskPotential != null && buildFilterDto.SecurityRiskPotential.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SecurityRiskPotential)
                    predicateInner.Or(x => x.Securityriskpotential == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VulnerabilityScore != null && buildFilterDto.VulnerabilityScore.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.VulnerabilityScore)
                    predicateInner.Or(x => x.Vulnerabilityscore == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Comments != null && buildFilterDto.Comments.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.Comments)
                    predicateInner.Or(x => x.Comments == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.QId != null && buildFilterDto.QId.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.QId)
                    predicateInner.Or(x => x.Qid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RequestID != null && buildFilterDto.RequestID.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.RequestID)
                    predicateInner.Or(x => x.Requestid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VulnerabilityRating != null && buildFilterDto.VulnerabilityRating.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.VulnerabilityRating)
                    predicateInner.Or(x => x.Vulnerabilityrating == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SecurityRiskEffective != null && buildFilterDto.SecurityRiskEffective.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SecurityRiskEffective)
                    predicateInner.Or(x => x.Securityriskeffective == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SecurityMitigation != null && buildFilterDto.SecurityMitigation.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SecurityMitigation)
                    predicateInner.Or(x => x.Securitymitigation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SecurityRiskOverall != null && buildFilterDto.SecurityRiskOverall.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SecurityRiskOverall)
                    predicateInner.Or(x => x.Securityriskoverall == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IncludedinSecurityScanning != null && buildFilterDto.IncludedinSecurityScanning.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.IncludedinSecurityScanning)
                    predicateInner.Or(x => x.Includedinsecurityscanning == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RaId != null && buildFilterDto.RaId.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.RaId)
                    predicateInner.Or(x => x.Raid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LcmCumulativeRiskId != null && buildFilterDto.LcmCumulativeRiskId.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.LcmCumulativeRiskId)
                    predicateInner.Or(x => x.Lcmcumulativeriskid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LcmCumulativeRiskLevel != null && buildFilterDto.LcmCumulativeRiskLevel.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.LcmCumulativeRiskLevel)
                    predicateInner.Or(x => x.Lcmcumulativerisklevel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CyberRiskRequestId != null && buildFilterDto.CyberRiskRequestId.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.CyberRiskRequestId)
                    predicateInner.Or(x => x.Cyberriskrequestid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Criticality != null && buildFilterDto.Criticality.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.Criticality)
                    predicateInner.Or(x => x.Criticality == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetStatus != null && buildFilterDto.AssetStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.AssetStatus)
                    predicateInner.Or(x => x.Assetstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.GdprRelevant != null && buildFilterDto.GdprRelevant.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.GdprRelevant)
                    predicateInner.Or(x => x.Gdprrelevant == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastScanDate != null && buildFilterDto.LastScanDate.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.LastScanDate)
                    predicateInner.Or(x => x.Lastscandate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastUpgradeDate != null && buildFilterDto.LastUpgradeDate.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.LastUpgradeDate)
                    predicateInner.Or(x => x.Lastupgradedate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetOutofScopeForReportingPurposes != null && buildFilterDto.AssetOutofScopeForReportingPurposes.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.AssetOutofScopeForReportingPurposes)
                    predicateInner.Or(x => x.Assetoutofscopeforreportingpurposes == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MainOrganization != null && buildFilterDto.MainOrganization.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.MainOrganization)
                    predicateInner.Or(x => x.Mainorganization == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IsExtendedSupportOfferedByVendor != null && buildFilterDto.IsExtendedSupportOfferedByVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.IsExtendedSupportOfferedByVendor)
                    predicateInner.Or(x => x.Isextendedsupportofferedbyvendor == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EomControl != null && buildFilterDto.EomControl.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EomControl)
                    predicateInner.Or(x => x.Eomcontrol == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EngUpdateTracker != null && buildFilterDto.EngUpdateTracker.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EngUpdateTracker)
                    predicateInner.Or(x => x.Engupdatetracker == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpsUpdateTracker != null && buildFilterDto.OpsUpdateTracker.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.OpsUpdateTracker)
                    predicateInner.Or(x => x.Opsupdatetracker == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TypeOfNetworkElement != null && buildFilterDto.TypeOfNetworkElement.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.TypeOfNetworkElement)
                    predicateInner.Or(x => x.Typeofnetworkelement == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EngKpi2 != null && buildFilterDto.EngKpi2.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EngKpi2)
                    predicateInner.Or(x => x.Engkpi2 == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IpAddress != null && buildFilterDto.IpAddress.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.IpAddress)
                    predicateInner.Or(x => x.Ipaddress == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SerialNumber != null && buildFilterDto.SerialNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.SerialNumber)
                    predicateInner.Or(x => x.Serialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Hostname != null && buildFilterDto.Hostname.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.Hostname)
                    predicateInner.Or(x => x.Hostname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ExNetworks != null && buildFilterDto.ExNetworks.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ExNetworks)
                    predicateInner.Or(x => x.Exnetworks == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OriginalLcmId != null && buildFilterDto.OriginalLcmId.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.OriginalLcmId)
                    predicateInner.Or(x => x.Originallcmid == item);
                predicateResult.And(predicateInner);
            }
            #region Hardware
            //if (buildFilterDto.HWOperationsContactPoint != null && buildFilterDto.HWOperationsContactPoint.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
            //    foreach (var item in buildFilterDto.HWOperationsContactPoint)
            //        predicateInner.Or(x => x.Hwoperationscontactpoint == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.HWVendor != null && buildFilterDto.HWVendor.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
            //    foreach (var item in buildFilterDto.HWVendor)
            //        predicateInner.Or(x => x.Hwvendor == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.HWOperationsMaintenanceContract != null && buildFilterDto.HWOperationsMaintenanceContract.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
            //    foreach (var item in buildFilterDto.HWOperationsMaintenanceContract)
            //        predicateInner.Or(x => x.Hwoperationsmaintenancecontract == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.HWVendorEndOfMaintenanceDate != null && buildFilterDto.HWVendorEndOfMaintenanceDate.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
            //    foreach (var item in buildFilterDto.HWVendorEndOfMaintenanceDate)
            //        predicateInner.Or(x => x.Hwvendorendofmaintenancedate == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.PlannedHWModel != null && buildFilterDto.PlannedHWModel.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
            //    foreach (var item in buildFilterDto.PlannedHWModel)
            //        predicateInner.Or(x => x.Plannedhwmodel == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.HWOPSMaintenanceContractEndDate != null && buildFilterDto.HWOPSMaintenanceContractEndDate.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
            //    foreach (var item in buildFilterDto.HWOPSMaintenanceContractEndDate)
            //        predicateInner.Or(x => x.Hwopsmaintenancecontractenddate == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.LcmStatusEngHardware != null && buildFilterDto.LcmStatusEngHardware.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
            //    foreach (var item in buildFilterDto.LcmStatusEngHardware)
            //        predicateInner.Or(x => x.Lcmstatusengsoftware == item);
            //    predicateResult.And(predicateInner);
            //}
            //if (buildFilterDto.LcmStatusOpsHardware != null && buildFilterDto.LcmStatusOpsHardware.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
            //    foreach (var item in buildFilterDto.LcmStatusOpsHardware)
            //        predicateInner.Or(x => x.Lcmstatusopssoftware == item);
            //    predicateResult.And(predicateInner);
            //}
            #endregion
            if (buildFilterDto.ApplicationOperatingSys != null && buildFilterDto.ApplicationOperatingSys.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ApplicationOperatingSys)
                    predicateInner.Or(x => x.Applicationoperatingsys == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EofsDate != null && buildFilterDto.EofsDate.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EofsDate)
                    predicateInner.Or(x => x.Eofsdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EoslKpidaily != null && buildFilterDto.EoslKpidaily.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EoslKpidaily)
                    predicateInner.Or(x => x.Eoslkpidaily == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EoslKpiFrozen != null && buildFilterDto.EoslKpiFrozen.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EoslKpiFrozen)
                    predicateInner.Or(x => x.Eoslkpifrozen == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EoslKpiForecast != null && buildFilterDto.EoslKpiForecast.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.EoslKpiForecast)
                    predicateInner.Or(x => x.Eoslkpiforecast == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductImportanceHistory2 != null && buildFilterDto.ProductImportanceHistory2.Any())
            {
                predicateInner = PredicateBuilder.New<Swpassthroughlcm>();
                foreach (var item in buildFilterDto.ProductImportanceHistory2)
                    predicateInner.Or(x => x.Productimportancehistory2 == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public virtual IQueryable<SwPassThroughLcm> GetQuery(ExpressionStarter<Swpassthroughlcm> predicateResult)
        {
            var query = _repositoryWrapper.SwPassThroughLcmRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.NontemsverticalNavigation);
            return query.AsEnumerable().Select(x => SwPassThroughLcmMapper.Get(x)).AsQueryable();
        }


        public virtual Dictionary<string, Expression<Func<SwPassThroughLcm, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SwPassThroughLcm, object>>[]>
            {
                ["passThroughLcmId"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.PassThroughLcmId },
                ["nonTemsVertical"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.NonTemsVertical },
                ["reportId"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ReportId },
                ["localMarket"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.LocalMarket },
                ["verticalEngineeringTeam"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.VerticalEngineeringTeam },
                ["verticalSubDomain"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.VerticalSubDomain },
                ["engineeringContactPoint"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EngineeringContactPoint },
                ["swOperationsContactPoint"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SWOperationsContactPoint },
                ["assetCategory"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.AssetCategory },
                ["assetClass"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.AssetClass },
                ["assetType"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.AssetType },
                ["assetDescription"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.AssetDescription },
                ["assetVirtualized"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.AssetVirtualized },
                ["productImportance"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ProductImportance },
                ["swVendor"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SWVendor },
                ["hardwareModel"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.HardwareModel },
                ["productCode"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ProductCode },
                ["softwareVersion"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SoftwareVersion },
                ["numberOfNodes"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.NumberOfNodes },
                ["handedOverToOperation"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.HandedOverToOperation },
                ["swOperationsMaintenanceContract"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SWOperationsMaintenanceContract },
                ["contractRenewalPlan"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ContractRenewalPlan },
                ["swVendorEndOfMaintenanceDate"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SWVendorEndOfMaintenanceDate },
                ["vendorEndOfVulnerabilitySecuritySupportDate"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.VendorEndOfVulnerabilitySecuritySupportDate },
                ["lcmStatus"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.LcmStatus },
                ["identifiedAction"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.IdentifiedAction },
                ["descriptionOfPlannedAction"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.DescriptionOfPlannedAction },
                ["plannedSoftwareVersion"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.PlannedSoftwareVersion },
                ["projectStatus"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ProjectStatus },
                ["reasonfornoPlan"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ReasonfornoPlan },
                ["commentonProjectStatus"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.CommentonProjectStatus },
                ["projectEndDate"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ProjectEndDate },
                ["ragStatus"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.RagStatus },
                ["trackingNumberProjectName"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.TrackingNumberProjectName },
                ["program"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.Program },
                ["wbsCode"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.WbsCode },
                ["bptID"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.BptID },
                ["ppmID"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.PpmID },
                ["scopeOfSimplification"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ScopeOfSimplification },
                ["dataSource"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.DataSource },
                ["projectOwner"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ProjectOwner },
                ["budgetEstimated"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.BudgetEstimated },
                ["notes"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.Notes },
                ["bundleBudget"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.BundleBudget },
                ["bundleId"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.BundleId },
                ["assetServiceFunctionality"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.AssetServiceFunctionality },
                ["platform"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.Platform },
                ["lcmStatusEngSoftware"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.LcmStatusEngSoftware },
                ["lcmStatusOpsSoftware"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.LcmStatusOpsSoftware },
                ["sWOpsMaintenanceConractEndDate"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SWOpsMaintenanceConractEndDate },
                ["engRiskEvaluation"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EngRiskEvaluation },
                ["engRiskEvaluationNotes"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EngRiskEvaluationNotes },
                ["opsRiskEvaluation"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.OpsRiskEvaluation },
                ["opsRiskEvaluationNotes"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.OpsRiskEvaluationNotes },
                ["incidentClass"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.IncidentClass },
                ["occurrenceProbability"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.OccurrenceProbability },
                ["newopsRiskEvaluation"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.NewopsRiskEvaluation },
                ["overallRiskEvaluation"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.OverallRiskEvaluation },
                ["riskCluster"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.RiskCluster },
                ["securityRiskPotential"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SecurityRiskPotential },
                ["vulnerabilityScore"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.VulnerabilityScore },
                ["comments"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.Comments },
                ["qId"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.QId },
                ["requestID"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.RequestID },
                ["vulnerabilityRating"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.VulnerabilityRating },
                ["securityRiskEffective"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SecurityRiskEffective },
                ["securityMitigation"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SecurityMitigation },
                ["securityRiskOverall"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SecurityRiskOverall },
                ["includedinSecurityScanning"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.IncludedinSecurityScanning },
                ["raId"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.RaId },
                ["lcmCumulativeRiskId"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.LcmCumulativeRiskId },
                ["lcmCumulativeRiskLevel"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.LcmCumulativeRiskLevel },
                ["cyberRiskRequestId"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.CyberRiskRequestId },
                ["criticality"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.Criticality },
                ["assetStatus"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.AssetStatus },
                ["gdprRelevant"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.GdprRelevant },
                ["lastScanDate"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.LastScanDate },
                ["lastUpgradeDate"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.LastUpgradeDate },
                ["assetOutofScopeForReportingPurposes"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.AssetOutofScopeForReportingPurposes },
                ["mainOrganization"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.MainOrganization },
                ["isExtendedSupportOfferedByVendor"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.IsExtendedSupportOfferedByVendor },
                ["eomControl"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EomControl },
                ["engUpdateTracker"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EngUpdateTracker },
                ["opsUpdateTracker"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.OpsUpdateTracker },
                ["typeOfNetworkElement"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.TypeOfNetworkElement },
                ["engKpi2"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EngKpi2 },
                ["ipAddress"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.IpAddress },
                ["serialNumber"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SerialNumber },
                ["hostname"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.Hostname },
                ["exNetworks"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ExNetworks },
                ["originalLcmId"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.OriginalLcmId },
                ["hwOperationsContactPoint"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.HWOperationsContactPoint },
                ["hwVendor"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.HWVendor },
                ["hwOperationsMaintenanceContract"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.HWOperationsMaintenanceContract },
                ["hwVendorEndOfMaintenanceDate"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.HWVendorEndOfMaintenanceDate },
                ["plannedHWModel"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.PlannedHWModel },
                ["hwOPSMaintenanceContractEndDate"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.HWOPSMaintenanceContractEndDate },
                ["lcmStatusEngHardware"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.LcmStatusEngHardware },
                ["applicationOperatingSys"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ApplicationOperatingSys },
                ["eofsDate"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EofsDate },
                ["eoslKpiDaily"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EoslKpiDaily },
                ["eoslKpiForecast"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EoslKpiForecast },
                ["eoslKpiFrozen"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.EoslKpiFrozen },
                ["productImportanceHistory2"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.ProductImportanceHistory2 },
                ["swResourceKeyPassThrough"] = new Expression<Func<SwPassThroughLcm, object>>[] { p => p.SwResourceKey },

            };
        }


        public virtual List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, PassThroughLcmQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult);

            var rtn = propertyName switch
            {
                "reportId" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ReportId, Value = p.ReportId }).Distinct().ToList() : query.Where(x => x.ReportId.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ReportId, Value = p.ReportId }).Distinct().ToList(),
                "swResourceKeyPassThrough" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SwResourceKey, Value = p.SwResourceKey }).Distinct().ToList() : query.Where(x => x.SwResourceKey.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SwResourceKey, Value = p.SwResourceKey }).Distinct().ToList(),
                "localMarket" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LocalMarket, Value = p.LocalMarket }).Distinct().ToList() : query.Where(x => x.LocalMarket.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LocalMarket, Value = p.LocalMarket }).Distinct().ToList(),
                "verticalEngineeringTeam" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.VerticalEngineeringTeam, Value = p.VerticalEngineeringTeam }).Distinct().ToList() : query.Where(x => x.VerticalEngineeringTeam.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.VerticalEngineeringTeam, Value = p.VerticalEngineeringTeam }).Distinct().ToList(),
                "verticalSubDomain" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.VerticalSubDomain, Value = p.VerticalSubDomain }).Distinct().ToList() : query.Where(x => x.VerticalSubDomain.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.VerticalSubDomain, Value = p.VerticalSubDomain }).Distinct().ToList(),
                "engineeringContactPoint" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EngineeringContactPoint, Value = p.EngineeringContactPoint }).Distinct().ToList() : query.Where(x => x.EngineeringContactPoint.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EngineeringContactPoint, Value = p.EngineeringContactPoint }).Distinct().ToList(),
                "swOperationsContactPoint" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SWOperationsContactPoint, Value = p.SWOperationsContactPoint }).Distinct().ToList() : query.Where(x => x.SWOperationsContactPoint.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SWOperationsContactPoint, Value = p.SWOperationsContactPoint }).Distinct().ToList(),
                "assetCategory" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.AssetCategory, Value = p.AssetCategory }).Distinct().ToList() : query.Where(x => x.AssetCategory.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.AssetCategory, Value = p.AssetCategory }).Distinct().ToList(),
                "assetClass" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.AssetClass, Value = p.AssetClass }).Distinct().ToList() : query.Where(x => x.AssetClass.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.AssetClass, Value = p.AssetClass }).Distinct().ToList(),
                "assetType" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.AssetType, Value = p.AssetType }).Distinct().ToList() : query.Where(x => x.AssetType.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.AssetType, Value = p.AssetType }).Distinct().ToList(),
                "assetDescription" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.AssetDescription, Value = p.AssetDescription }).Distinct().ToList() : query.Where(x => x.AssetDescription.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.AssetDescription, Value = p.AssetDescription }).Distinct().ToList(),
                "assetVirtualized" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.AssetVirtualized, Value = p.AssetVirtualized }).Distinct().ToList() : query.Where(x => x.AssetVirtualized.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.AssetVirtualized, Value = p.AssetVirtualized }).Distinct().ToList(),
                "productImportance" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ProductImportance, Value = p.ProductImportance }).Distinct().ToList() : query.Where(x => x.ProductImportance.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ProductImportance, Value = p.ProductImportance }).Distinct().ToList(),
                "swVendor" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SWVendor, Value = p.SWVendor }).Distinct().ToList() : query.Where(x => x.SWVendor.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SWVendor, Value = p.SWVendor }).Distinct().ToList(),
                "hardwareModel" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.HardwareModel, Value = p.HardwareModel }).Distinct().ToList() : query.Where(x => x.HardwareModel.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.HardwareModel, Value = p.HardwareModel }).Distinct().ToList(),
                "productCode" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ProductCode, Value = p.ProductCode }).Distinct().ToList() : query.Where(x => x.ProductCode.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ProductCode, Value = p.ProductCode }).Distinct().ToList(),
                "softwareVersion" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SoftwareVersion, Value = p.SoftwareVersion }).Distinct().ToList() : query.Where(x => x.SoftwareVersion.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SoftwareVersion, Value = p.SoftwareVersion }).Distinct().ToList(),
                "numberOfNodes" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.NumberOfNodes, Value = p.NumberOfNodes }).Distinct().ToList() : query.Where(x => x.NumberOfNodes.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.NumberOfNodes, Value = p.NumberOfNodes }).Distinct().ToList(),
                "handedOverToOperation" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.HandedOverToOperation, Value = p.HandedOverToOperation }).Distinct().ToList() : query.Where(x => x.HandedOverToOperation.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.HandedOverToOperation, Value = p.HandedOverToOperation }).Distinct().ToList(),
                "swOperationsMaintenanceContract" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SWOperationsMaintenanceContract, Value = p.SWOperationsMaintenanceContract }).Distinct().ToList() : query.Where(x => x.SWOperationsMaintenanceContract.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SWOperationsMaintenanceContract, Value = p.SWOperationsMaintenanceContract }).Distinct().ToList(),
                "contractRenewalPlan" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ContractRenewalPlan, Value = p.ContractRenewalPlan }).Distinct().ToList() : query.Where(x => x.ContractRenewalPlan.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ContractRenewalPlan, Value = p.ContractRenewalPlan }).Distinct().ToList(),
                "swVendorEndOfMaintenanceDate" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SWVendorEndOfMaintenanceDate, Value = p.SWVendorEndOfMaintenanceDate }).Distinct().ToList() : query.Where(x => x.SWVendorEndOfMaintenanceDate.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SWVendorEndOfMaintenanceDate, Value = p.SWVendorEndOfMaintenanceDate }).Distinct().ToList(),
                "vendorEndOfVulnerabilitySecuritySupportDate" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.VendorEndOfVulnerabilitySecuritySupportDate, Value = p.VendorEndOfVulnerabilitySecuritySupportDate }).Distinct().ToList() : query.Where(x => x.VendorEndOfVulnerabilitySecuritySupportDate.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.VendorEndOfVulnerabilitySecuritySupportDate, Value = p.VendorEndOfVulnerabilitySecuritySupportDate }).Distinct().ToList(),
                "lcmStatus" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LcmStatus, Value = p.LcmStatus }).Distinct().ToList() : query.Where(x => x.LcmStatus.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LcmStatus, Value = p.LcmStatus }).Distinct().ToList(),
                "identifiedAction" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.IdentifiedAction, Value = p.IdentifiedAction }).Distinct().ToList() : query.Where(x => x.IdentifiedAction.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.IdentifiedAction, Value = p.IdentifiedAction }).Distinct().ToList(),
                "descriptionOfPlannedAction" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.DescriptionOfPlannedAction, Value = p.DescriptionOfPlannedAction }).Distinct().ToList() : query.Where(x => x.DescriptionOfPlannedAction.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.DescriptionOfPlannedAction, Value = p.DescriptionOfPlannedAction }).Distinct().ToList(),
                "plannedSoftwareVersion" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.PlannedSoftwareVersion, Value = p.PlannedSoftwareVersion }).Distinct().ToList() : query.Where(x => x.PlannedSoftwareVersion.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.PlannedSoftwareVersion, Value = p.PlannedSoftwareVersion }).Distinct().ToList(),
                "projectStatus" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ProjectStatus, Value = p.ProjectStatus }).Distinct().ToList() : query.Where(x => x.ProjectStatus.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ProjectStatus, Value = p.ProjectStatus }).Distinct().ToList(),
                "reasonfornoPlan" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ReasonfornoPlan, Value = p.ReasonfornoPlan }).Distinct().ToList() : query.Where(x => x.ReasonfornoPlan.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ReasonfornoPlan, Value = p.ReasonfornoPlan }).Distinct().ToList(),
                "commentonProjectStatus" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.CommentonProjectStatus, Value = p.CommentonProjectStatus }).Distinct().ToList() : query.Where(x => x.CommentonProjectStatus.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.CommentonProjectStatus, Value = p.CommentonProjectStatus }).Distinct().ToList(),
                "projectEndDate" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ProjectEndDate, Value = p.ProjectEndDate }).Distinct().ToList() : query.Where(x => x.ProjectEndDate.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ProjectEndDate, Value = p.ProjectEndDate }).Distinct().ToList(),
                "ragStatus" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.RagStatus, Value = p.RagStatus }).Distinct().ToList() : query.Where(x => x.RagStatus.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.RagStatus, Value = p.RagStatus }).Distinct().ToList(),
                "trackingNumberProjectName" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.TrackingNumberProjectName, Value = p.TrackingNumberProjectName }).Distinct().ToList() : query.Where(x => x.TrackingNumberProjectName.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.TrackingNumberProjectName, Value = p.TrackingNumberProjectName }).Distinct().ToList(),
                "program" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.Program, Value = p.Program }).Distinct().ToList() : query.Where(x => x.Program.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.Program, Value = p.Program }).Distinct().ToList(),
                "wbsCode" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.WbsCode, Value = p.WbsCode }).Distinct().ToList() : query.Where(x => x.WbsCode.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.WbsCode, Value = p.WbsCode }).Distinct().ToList(),
                "bptID" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.BptID, Value = p.BptID }).Distinct().ToList() : query.Where(x => x.BptID.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.BptID, Value = p.BptID }).Distinct().ToList(),
                "ppmID" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.PpmID, Value = p.PpmID }).Distinct().ToList() : query.Where(x => x.PpmID.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.PpmID, Value = p.PpmID }).Distinct().ToList(),
                "scopeOfSimplification" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ScopeOfSimplification, Value = p.ScopeOfSimplification }).Distinct().ToList() : query.Where(x => x.ScopeOfSimplification.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ScopeOfSimplification, Value = p.ScopeOfSimplification }).Distinct().ToList(),
                "dataSource" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.DataSource, Value = p.DataSource }).Distinct().ToList() : query.Where(x => x.DataSource.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.DataSource, Value = p.DataSource }).Distinct().ToList(),
                "projectOwner" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ProjectOwner, Value = p.ProjectOwner }).Distinct().ToList() : query.Where(x => x.ProjectOwner.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ProjectOwner, Value = p.ProjectOwner }).Distinct().ToList(),
                "budgetEstimated" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.BudgetEstimated, Value = p.BudgetEstimated }).Distinct().ToList() : query.Where(x => x.BudgetEstimated.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.BudgetEstimated, Value = p.BudgetEstimated }).Distinct().ToList(),
                "notes" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.Notes, Value = p.Notes }).Distinct().ToList() : query.Where(x => x.Notes.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.Notes, Value = p.Notes }).Distinct().ToList(),
                "bundleBudget" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.BundleBudget, Value = p.BundleBudget }).Distinct().ToList() : query.Where(x => x.BundleBudget.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.BundleBudget, Value = p.BundleBudget }).Distinct().ToList(),
                "bundleId" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.BundleId, Value = p.BundleId }).Distinct().ToList() : query.Where(x => x.BundleId.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.BundleId, Value = p.BundleId }).Distinct().ToList(),
                "assetServiceFunctionality" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.AssetServiceFunctionality, Value = p.AssetServiceFunctionality }).Distinct().ToList() : query.Where(x => x.AssetServiceFunctionality.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.AssetServiceFunctionality, Value = p.AssetServiceFunctionality }).Distinct().ToList(),
                "platform" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.Platform, Value = p.Platform }).Distinct().ToList() : query.Where(x => x.Platform.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.Platform, Value = p.Platform }).Distinct().ToList(),
                "lcmStatusEngSoftware" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LcmStatusEngSoftware, Value = p.LcmStatusEngSoftware }).Distinct().ToList() : query.Where(x => x.LcmStatusEngSoftware.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LcmStatusEngSoftware, Value = p.LcmStatusEngSoftware }).Distinct().ToList(),
                "lcmStatusOpsSoftware" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LcmStatusOpsSoftware, Value = p.LcmStatusOpsSoftware }).Distinct().ToList() : query.Where(x => x.LcmStatusOpsSoftware.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LcmStatusOpsSoftware, Value = p.LcmStatusOpsSoftware }).Distinct().ToList(),
                "swOpsMaintenanceConractEndDate" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SWOpsMaintenanceConractEndDate, Value = p.SWOpsMaintenanceConractEndDate }).Distinct().ToList() : query.Where(x => x.SWOpsMaintenanceConractEndDate.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SWOpsMaintenanceConractEndDate, Value = p.SWOpsMaintenanceConractEndDate }).Distinct().ToList(),
                "engRiskEvaluation" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EngRiskEvaluation, Value = p.EngRiskEvaluation }).Distinct().ToList() : query.Where(x => x.EngRiskEvaluation.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EngRiskEvaluation, Value = p.EngRiskEvaluation }).Distinct().ToList(),
                "engRiskEvaluationNotes" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EngRiskEvaluationNotes, Value = p.EngRiskEvaluationNotes }).Distinct().ToList() : query.Where(x => x.EngRiskEvaluationNotes.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EngRiskEvaluationNotes, Value = p.EngRiskEvaluationNotes }).Distinct().ToList(),
                "opsRiskEvaluation" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.OpsRiskEvaluation, Value = p.OpsRiskEvaluation }).Distinct().ToList() : query.Where(x => x.OpsRiskEvaluation.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.OpsRiskEvaluation, Value = p.OpsRiskEvaluation }).Distinct().ToList(),
                "opsRiskEvaluationNotes" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.OpsRiskEvaluationNotes, Value = p.OpsRiskEvaluationNotes }).Distinct().ToList() : query.Where(x => x.OpsRiskEvaluationNotes.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.OpsRiskEvaluationNotes, Value = p.OpsRiskEvaluationNotes }).Distinct().ToList(),
                "incidentClass" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.IncidentClass, Value = p.IncidentClass }).Distinct().ToList() : query.Where(x => x.IncidentClass.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.IncidentClass, Value = p.IncidentClass }).Distinct().ToList(),
                "occurrenceProbability" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.OccurrenceProbability, Value = p.OccurrenceProbability }).Distinct().ToList() : query.Where(x => x.OccurrenceProbability.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.OccurrenceProbability, Value = p.OccurrenceProbability }).Distinct().ToList(),
                "newopsRiskEvaluation" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.NewopsRiskEvaluation, Value = p.NewopsRiskEvaluation }).Distinct().ToList() : query.Where(x => x.NewopsRiskEvaluation.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.NewopsRiskEvaluation, Value = p.NewopsRiskEvaluation }).Distinct().ToList(),
                "overallRiskEvaluation" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.OverallRiskEvaluation, Value = p.OverallRiskEvaluation }).Distinct().ToList() : query.Where(x => x.OverallRiskEvaluation.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.OverallRiskEvaluation, Value = p.OverallRiskEvaluation }).Distinct().ToList(),
                "riskCluster" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.RiskCluster, Value = p.RiskCluster }).Distinct().ToList() : query.Where(x => x.RiskCluster.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.RiskCluster, Value = p.RiskCluster }).Distinct().ToList(),
                "securityRiskPotential" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SecurityRiskPotential, Value = p.SecurityRiskPotential }).Distinct().ToList() : query.Where(x => x.SecurityRiskPotential.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SecurityRiskPotential, Value = p.SecurityRiskPotential }).Distinct().ToList(),
                "vulnerabilityScore" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.VulnerabilityScore, Value = p.VulnerabilityScore }).Distinct().ToList() : query.Where(x => x.VulnerabilityScore.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.VulnerabilityScore, Value = p.VulnerabilityScore }).Distinct().ToList(),
                "comments" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.Comments, Value = p.Comments }).Distinct().ToList() : query.Where(x => x.Comments.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.Comments, Value = p.Comments }).Distinct().ToList(),
                "qId" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.QId, Value = p.QId }).Distinct().ToList() : query.Where(x => x.QId.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.QId, Value = p.QId }).Distinct().ToList(),
                "requestID" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.RequestID, Value = p.RequestID }).Distinct().ToList() : query.Where(x => x.RequestID.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.RequestID, Value = p.RequestID }).Distinct().ToList(),
                "vulnerabilityRating" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.VulnerabilityRating, Value = p.VulnerabilityRating }).Distinct().ToList() : query.Where(x => x.VulnerabilityRating.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.VulnerabilityRating, Value = p.VulnerabilityRating }).Distinct().ToList(),
                "securityRiskEffective" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SecurityRiskEffective, Value = p.SecurityRiskEffective }).Distinct().ToList() : query.Where(x => x.SecurityRiskEffective.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SecurityRiskEffective, Value = p.SecurityRiskEffective }).Distinct().ToList(),
                "securityMitigation" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SecurityMitigation, Value = p.SecurityMitigation }).Distinct().ToList() : query.Where(x => x.SecurityMitigation.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SecurityMitigation, Value = p.SecurityMitigation }).Distinct().ToList(),
                "securityRiskOverall" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SecurityRiskOverall, Value = p.SecurityRiskOverall }).Distinct().ToList() : query.Where(x => x.SecurityRiskOverall.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SecurityRiskOverall, Value = p.SecurityRiskOverall }).Distinct().ToList(),
                "includedinSecurityScanning" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.IncludedinSecurityScanning, Value = p.IncludedinSecurityScanning }).Distinct().ToList() : query.Where(x => x.IncludedinSecurityScanning.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.IncludedinSecurityScanning, Value = p.IncludedinSecurityScanning }).Distinct().ToList(),
                "raId" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.RaId, Value = p.RaId }).Distinct().ToList() : query.Where(x => x.RaId.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.RaId, Value = p.RaId }).Distinct().ToList(),
                "lcmCumulativeRiskId" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LcmCumulativeRiskId, Value = p.LcmCumulativeRiskId }).Distinct().ToList() : query.Where(x => x.LcmCumulativeRiskId.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LcmCumulativeRiskId, Value = p.LcmCumulativeRiskId }).Distinct().ToList(),
                "lcmCumulativeRiskLevel" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LcmCumulativeRiskLevel, Value = p.LcmCumulativeRiskLevel }).Distinct().ToList() : query.Where(x => x.LcmCumulativeRiskLevel.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LcmCumulativeRiskLevel, Value = p.LcmCumulativeRiskLevel }).Distinct().ToList(),
                "cyberRiskRequestId" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.CyberRiskRequestId, Value = p.CyberRiskRequestId }).Distinct().ToList() : query.Where(x => x.CyberRiskRequestId.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.CyberRiskRequestId, Value = p.CyberRiskRequestId }).Distinct().ToList(),
                "criticality" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.Criticality, Value = p.Criticality }).Distinct().ToList() : query.Where(x => x.Criticality.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.Criticality, Value = p.Criticality }).Distinct().ToList(),
                "assetStatus" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.AssetStatus, Value = p.AssetStatus }).Distinct().ToList() : query.Where(x => x.AssetStatus.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.AssetStatus, Value = p.AssetStatus }).Distinct().ToList(),
                "gdprRelevant" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.GdprRelevant, Value = p.GdprRelevant }).Distinct().ToList() : query.Where(x => x.GdprRelevant.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.GdprRelevant, Value = p.GdprRelevant }).Distinct().ToList(),
                "lastScanDate" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LastScanDate, Value = p.LastScanDate }).Distinct().ToList() : query.Where(x => x.LastScanDate.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LastScanDate, Value = p.LastScanDate }).Distinct().ToList(),
                "lastUpgradeDate" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LastUpgradeDate, Value = p.LastUpgradeDate }).Distinct().ToList() : query.Where(x => x.LastUpgradeDate.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LastUpgradeDate, Value = p.LastUpgradeDate }).Distinct().ToList(),
                "assetOutofScopeForReportingPurposes" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.AssetOutofScopeForReportingPurposes, Value = p.AssetOutofScopeForReportingPurposes }).Distinct().ToList() : query.Where(x => x.AssetOutofScopeForReportingPurposes.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.AssetOutofScopeForReportingPurposes, Value = p.AssetOutofScopeForReportingPurposes }).Distinct().ToList(),
                "mainOrganization" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.MainOrganization, Value = p.MainOrganization }).Distinct().ToList() : query.Where(x => x.MainOrganization.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.MainOrganization, Value = p.MainOrganization }).Distinct().ToList(),
                "isExtendedSupportOfferedByVendor" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.IsExtendedSupportOfferedByVendor, Value = p.IsExtendedSupportOfferedByVendor }).Distinct().ToList() : query.Where(x => x.IsExtendedSupportOfferedByVendor.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.IsExtendedSupportOfferedByVendor, Value = p.IsExtendedSupportOfferedByVendor }).Distinct().ToList(),
                "eomControl" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EomControl, Value = p.EomControl }).Distinct().ToList() : query.Where(x => x.EomControl.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EomControl, Value = p.EomControl }).Distinct().ToList(),
                "engUpdateTracker" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EngUpdateTracker, Value = p.EngUpdateTracker }).Distinct().ToList() : query.Where(x => x.EngUpdateTracker.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EngUpdateTracker, Value = p.EngUpdateTracker }).Distinct().ToList(),
                "opsUpdateTracker" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.OpsUpdateTracker, Value = p.OpsUpdateTracker }).Distinct().ToList() : query.Where(x => x.OpsUpdateTracker.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.OpsUpdateTracker, Value = p.OpsUpdateTracker }).Distinct().ToList(),
                "typeOfNetworkElement" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.TypeOfNetworkElement, Value = p.TypeOfNetworkElement }).Distinct().ToList() : query.Where(x => x.TypeOfNetworkElement.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.TypeOfNetworkElement, Value = p.TypeOfNetworkElement }).Distinct().ToList(),
                "engKpi2" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EngKpi2, Value = p.EngKpi2 }).Distinct().ToList() : query.Where(x => x.EngKpi2.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EngKpi2, Value = p.EngKpi2 }).Distinct().ToList(),
                "ipAddress" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.IpAddress, Value = p.IpAddress }).Distinct().ToList() : query.Where(x => x.IpAddress.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.IpAddress, Value = p.IpAddress }).Distinct().ToList(),
                "serialNumber" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.SerialNumber, Value = p.SerialNumber }).Distinct().ToList() : query.Where(x => x.SerialNumber.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.SerialNumber, Value = p.SerialNumber }).Distinct().ToList(),
                "hostname" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.Hostname, Value = p.Hostname }).Distinct().ToList() : query.Where(x => x.Hostname.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.Hostname, Value = p.Hostname }).Distinct().ToList(),
                "exNetworks" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ExNetworks, Value = p.ExNetworks }).Distinct().ToList() : query.Where(x => x.ExNetworks.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ExNetworks, Value = p.ExNetworks }).Distinct().ToList(),
                "originalLcmId" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.OriginalLcmId, Value = p.OriginalLcmId }).Distinct().ToList() : query.Where(x => x.OriginalLcmId.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.OriginalLcmId, Value = p.OriginalLcmId }).Distinct().ToList(),
                "hwOperationsContactPoint" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.HWOperationsContactPoint, Value = p.HWOperationsContactPoint }).Distinct().ToList() : query.Where(x => x.HWOperationsContactPoint.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.HWOperationsContactPoint, Value = p.HWOperationsContactPoint }).Distinct().ToList(),
                "hwVendor" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.HWVendor, Value = p.HWVendor }).Distinct().ToList() : query.Where(x => x.HWVendor.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.HWVendor, Value = p.HWVendor }).Distinct().ToList(),
                "hwOperationsMaintenanceContract" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.HWOperationsMaintenanceContract, Value = p.HWOperationsMaintenanceContract }).Distinct().ToList() : query.Where(x => x.HWOperationsMaintenanceContract.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.HWOperationsMaintenanceContract, Value = p.HWOperationsMaintenanceContract }).Distinct().ToList(),
                "hwVendorEndOfMaintenanceDate" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.HWVendorEndOfMaintenanceDate, Value = p.HWVendorEndOfMaintenanceDate }).Distinct().ToList() : query.Where(x => x.HWVendorEndOfMaintenanceDate.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.HWVendorEndOfMaintenanceDate, Value = p.HWVendorEndOfMaintenanceDate }).Distinct().ToList(),
                "plannedHWModel" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.PlannedHWModel, Value = p.PlannedHWModel }).Distinct().ToList() : query.Where(x => x.PlannedHWModel.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.PlannedHWModel, Value = p.PlannedHWModel }).Distinct().ToList(),
                "hwOPSMaintenanceContractEndDate" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.HWOPSMaintenanceContractEndDate, Value = p.HWOPSMaintenanceContractEndDate }).Distinct().ToList() : query.Where(x => x.HWOPSMaintenanceContractEndDate.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.HWOPSMaintenanceContractEndDate, Value = p.HWOPSMaintenanceContractEndDate }).Distinct().ToList(),
                "lcmStatusEngHardware" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LcmStatusEngHardware, Value = p.LcmStatusEngHardware }).Distinct().ToList() : query.Where(x => x.LcmStatusEngHardware.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LcmStatusEngHardware, Value = p.LcmStatusEngHardware }).Distinct().ToList(),
                "lcmStatusOpsHardware" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.LcmStatusOpsHardware, Value = p.LcmStatusOpsHardware }).Distinct().ToList() : query.Where(x => x.LcmStatusOpsHardware.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.LcmStatusOpsHardware, Value = p.LcmStatusOpsHardware }).Distinct().ToList(),
                "applicationOperatingSys" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ApplicationOperatingSys, Value = p.ApplicationOperatingSys }).Distinct().ToList() : query.Where(x => x.ApplicationOperatingSys.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ApplicationOperatingSys, Value = p.ApplicationOperatingSys }).Distinct().ToList(),
                "eofsDate" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EofsDate, Value = p.EofsDate }).Distinct().ToList() : query.Where(x => x.EofsDate.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EofsDate, Value = p.EofsDate }).Distinct().ToList(),
                "eoslKpiDaily" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EoslKpiDaily, Value = p.EoslKpiDaily }).Distinct().ToList() : query.Where(x => x.EoslKpiDaily.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EoslKpiDaily, Value = p.EoslKpiDaily }).Distinct().ToList(),
                "eoslKpiForecast" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EoslKpiForecast, Value = p.EoslKpiForecast }).Distinct().ToList() : query.Where(x => x.EoslKpiForecast.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EoslKpiForecast, Value = p.EoslKpiForecast }).Distinct().ToList(),
                "eoslKpiFrozen" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.EoslKpiFrozen, Value = p.EoslKpiFrozen }).Distinct().ToList() : query.Where(x => x.EoslKpiFrozen.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.EoslKpiFrozen, Value = p.EoslKpiFrozen }).Distinct().ToList(),
                "productImportanceHistory2" => string.IsNullOrEmpty(propertyFilter) ? query.Select(p => new FilterValueDto { Text = p.ProductImportanceHistory2, Value = p.ProductImportanceHistory2 }).Distinct().ToList() : query.Where(x => x.ProductImportanceHistory2.Contains(propertyFilter)).Select(p => new FilterValueDto { Text = p.ProductImportanceHistory2, Value = p.ProductImportanceHistory2 }).Distinct().ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList(),


                "lastModified" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList()
                : query
                    .Where(x =>
                        x.ModificationDate.ToString().Contains(
                            propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList(),


                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion
    }
}
