using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AuditLog;
using CAM.DataTransferObjects.Entita.LcmAncillaryData;
using CAM.DataTransferObjects.Entita.SystemVerificationProblem;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CAM.BusinessManager.Entity
{
    public class SystemVerificationProblemManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public SystemVerificationProblemManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<SystemVerificationProblemDtoGrid> FindWithCondition(SystemVerificationProblemQueryDto SystemVerificationProblemQueryDto)
        {
            var predicateResult = ApplyFilter(SystemVerificationProblemQueryDto);
            var rtn = new QueryResultDto<SystemVerificationProblemDtoGrid>(new GenerateRenderForGrid<SystemVerificationProblemDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.SystemVerificationProblemRepository.Count(predicateResult) : _repositoryWrapper.SystemVerificationProblemRepository.Count(),
            };
            var query = GetQuery(predicateResult, SystemVerificationProblemQueryDto.Deleted ?? false).ApplyOrdering(SystemVerificationProblemQueryDto, GetColumnsMap()).ApplyPaging(SystemVerificationProblemQueryDto);
            var data = query.ToList();
                     
            IEnumerable <SystemVerificationProblemDtoGrid> SystemVerificationProblemDtoGrid;

            SystemVerificationProblemDtoGrid = _mapper.Map<IEnumerable<SystemVerificationProblemDtoGrid>>(data);

            rtn.Items = SystemVerificationProblemDtoGrid.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Systemverificationproblems> ApplyFilter(SystemVerificationProblemQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Systemverificationproblems>();
            var predicateInner = PredicateBuilder.New<Systemverificationproblems>();

            if (buildFilterDto.SystemVerificationProblemId != null && buildFilterDto.SystemVerificationProblemId.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.SystemVerificationProblemId)
                    predicateInner.Or(x => x.Systemverificationproblemid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProblemId != null && buildFilterDto.ProblemId.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.ProblemId)
                    predicateInner.Or(x => x.Problemid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpCoId != null && buildFilterDto.OpCoId.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.OpCoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemTypeId != null && buildFilterDto.SystemTypeId.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.SystemTypeId)
                    predicateInner.Or(x => x.Systemtypeid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EnvironmentId != null && buildFilterDto.EnvironmentId.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.EnvironmentId)
                    predicateInner.Or(x => x.Environmentid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProblemCategoryId != null && buildFilterDto.ProblemCategoryId.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.ProblemCategoryId)
                    predicateInner.Or(x => x.Problemcategoryid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProblemCategory != null && buildFilterDto.ProblemCategory.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.ProblemCategory)
                    predicateInner.Or(x => x.Problemcategory.Problemcategoryid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpCoName != null && buildFilterDto.OpCoName.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.OpCoName)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Environment != null && buildFilterDto.Environment.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.Environment)
                    predicateInner.Or(x => x.Environment.Environmentid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SeverityDescription != null && buildFilterDto.SeverityDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.SeverityDescription)
                    predicateInner.Or(x => x.SeverityNavigation.Severityid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DateFound != null)
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                if (buildFilterDto.DateFound.StartDate != null)
                    predicateInner.And(x => x.Datefound.Value.Date >= buildFilterDto.DateFound.StartDate);
                if (buildFilterDto.DateFound.EndDate != null)
                    predicateInner.And(x => x.Datefound.Value.Date <= buildFilterDto.DateFound.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProblemCategoryId != null && buildFilterDto.ProblemCategoryId.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.ProblemCategoryId)
                    predicateInner.Or(x => x.Problemcategoryid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProblemDescription != null && buildFilterDto.ProblemDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.ProblemDescription)
                    predicateInner.Or(x => x.Problemdescription == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MaintenanceReference != null && buildFilterDto.MaintenanceReference.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.MaintenanceReference)
                    predicateInner.Or(x => x.Maintenancereference == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Severity != null && buildFilterDto.Severity.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.Severity)
                    predicateInner.Or(x => x.Severity == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.StatusUrl != null && buildFilterDto.StatusUrl.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.StatusUrl)
                    predicateInner.Or(x => x.Status == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Mitigation != null && buildFilterDto.Mitigation.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.Mitigation)
                    predicateInner.Or(x => x.Mitigation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SolutionDescription != null && buildFilterDto.SolutionDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.SolutionDescription)
                    predicateInner.Or(x => x.Solutiondescription == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PatchReference != null && buildFilterDto.PatchReference.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.PatchReference)
                    predicateInner.Or(x => x.Patchreference == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductUpgradeReference != null && buildFilterDto.ProductUpgradeReference.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.ProductUpgradeReference)
                    predicateInner.Or(x => x.Productupgradereference == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SuppleMental != null && buildFilterDto.SuppleMental.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.SuppleMental)
                    predicateInner.Or(x => x.Supplemental == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubNetwork != null && buildFilterDto.SubNetwork.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.SubNetwork)
                    predicateInner.Or(x => x.Subnetwork == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VendorCsr != null && buildFilterDto.VendorCsr.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.VendorCsr)
                    predicateInner.Or(x => x.Vendorcsr == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TestReport != null && buildFilterDto.TestReport.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.TestReport)
                    predicateInner.Or(x => x.Testreport == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.StandardNir != null && buildFilterDto.StandardNir.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.StandardNir)
                    predicateInner.Or(x => x.Standardnir == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EricssonSecReport != null && buildFilterDto.EricssonSecReport.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.EricssonSecReport)
                    predicateInner.Or(x => x.Ericssonsecreport == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SwAndStEntries != null && buildFilterDto.SwAndStEntries.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.SwAndStEntries)
                    predicateInner.Or(x => x.Swandstentries == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PenTestingReport != null && buildFilterDto.PenTestingReport.Any())
            {
                predicateInner = PredicateBuilder.New<Systemverificationproblems>();
                foreach (var item in buildFilterDto.PenTestingReport)
                    predicateInner.Or(x => x.Pentestingreport == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<SystemVerificationProblems> GetQuery(ExpressionStarter<Systemverificationproblems> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.SystemVerificationProblemRepository.FindByCondition(predicateResult, includeDeleted)
                .Include(x=>x.Opco)
                .Include(x => x.Environment)
                .Include(x => x.Systemtype)
                .Include(x => x.SeverityNavigation)
                .Include(x => x.Problemcategory)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.SystemVerificationProblemRepository.FindAll()
                 .Include(x => x.Opco)
                .Include(x => x.Environment)
                .Include(x => x.Systemtype)
                .Include(x => x.SeverityNavigation)
                .Include(x => x.Problemcategory)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => SystemVerificationProblemMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<SystemVerificationProblems, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SystemVerificationProblems, object>>[]>
            {
                ["systemVerificationProblemId"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.SystemVerificationProblemId },
                ["problemId"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.ProblemId },
                ["opcoId"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.OpcoId },
                ["systemTypeId"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.SystemTypeId },
                ["environmentId"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.EnvironmentId },
                ["lastModifiedValue"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.CreationDate },
                ["dateFound"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.DateFound },
                ["problemCategoryId"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.ProblemCategoryId },
                ["problemDescription"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.ProblemDescription },
                ["maintenanceReference"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.MaintenanceReference },
                ["severity"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.Severity },
                ["statusUrl"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.StatusUrl },
                ["solutionDescription"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.SolutionDescription },
                ["patchReference"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.PatchReference },
                ["productUpgradeReference"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.ProductUpgradeReference },
                ["suppleMental"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.SuppleMental },
                ["vendorCsr"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.VendorCsr },
                ["subNetwork"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.SubNetwork },
                ["testReport"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.TestReport },
                ["standardNir"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.StandardNir },
                ["ericssonSecReport"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.EricssonSecReport },
                ["swAndStEntries"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.SwAndStEntries },
                ["penTestingReport"] = new Expression<Func<SystemVerificationProblems, object>>[] { p => p.PenTestingReport },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, SystemVerificationProblemQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "systemVerificationProblemId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.SystemVerificationProblemId.ToString(), Value = p.SystemVerificationProblemId.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.SystemVerificationProblemId.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.SystemVerificationProblemId.ToString(), Value = p.SystemVerificationProblemId.ToString() }).Distinct()
                   .ToList(),

                "problemId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.ProblemId, Value = p.ProblemId }).Distinct().ToList()
                    : query
                        .Where(x => x.ProblemId.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.ProblemId, Value = p.ProblemId }).Distinct()
                        .ToList(),
                "opCoId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.OpcoId.ToString(), Value = p.OpcoId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.OpcoId.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.OpcoId.ToString(), Value = p.OpcoId.ToString() }).Distinct()
                        .ToList(),
                "opCoName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.OpCo.OpCoDescription,
                        Value = p.OpcoId.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.OpCo.OpCoDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.OpCo.OpCoDescription,
                                    Value = p.OpcoId.ToString()
                                }).Distinct().ToList(),

                "systemTypeId" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.SystemTypeId.ToString(), Value = p.SystemTypeId.ToString() }).Distinct().ToList()
                   : query
                       .Where(x => x.SystemTypeId.ToString().Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.SystemTypeId.ToString(), Value = p.SystemTypeId.ToString() }).Distinct()
                       .ToList(),

                "environmentId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.EnvironmentId.ToString(), Value = p.EnvironmentId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.EnvironmentId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.EnvironmentId.ToString(), Value = p.EnvironmentId.ToString() }).Distinct().ToList(),

                "environment" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Environment.EnvironmentDescription, Value = p.EnvironmentId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Environment.EnvironmentDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Environment.EnvironmentDescription, Value = p.EnvironmentId.ToString() }).Distinct().ToList(),

                "problemCategoryId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ProblemCategoryId.ToString(), Value = p.ProblemCategoryId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ProblemCategoryId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ProblemCategoryId.ToString(), Value = p.ProblemCategoryId.ToString() }).Distinct().ToList(),

                "problemCategory" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ProblemCategory.ProblemCategoryDescription, Value = p.ProblemCategoryId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ProblemCategory.ProblemCategoryDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ProblemCategory.ProblemCategoryDescription, Value = p.ProblemCategoryId.ToString() }).Distinct().ToList(),


                "problemDescription" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.ProblemDescription, Value = p.ProblemDescription }).Distinct().ToList()
                   : query
                     .Where(x => x.ProblemDescription.Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.ProblemDescription, Value = p.ProblemDescription }).Distinct()
                   .ToList(),

                "maintenanceReference" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.MaintenanceReference, Value = p.MaintenanceReference }).Distinct().ToList()
                    : query
                        .Where(x => x.MaintenanceReference.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.MaintenanceReference, Value = p.MaintenanceReference }).Distinct()
                        .ToList(),
                "severity" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Severity.ToString(), Value = p.Severity.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.Severity.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.Severity.ToString(), Value = p.Severity.ToString() }).Distinct()
                        .ToList(),
                "severityDescription" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.SeverityEntity.SeverityDescription, Value = p.Severity.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.SeverityEntity.SeverityDescription.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.SeverityEntity.SeverityDescription, Value = p.Severity.ToString() }).Distinct()
                        .ToList(),
                "statusUrl" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.StatusUrl, Value = p.StatusUrl }).Distinct().ToList()
                   : query
                       .Where(x => x.StatusUrl.Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.StatusUrl, Value = p.StatusUrl }).Distinct()
                       .ToList(),

                "mitigation" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Mitigation, Value = p.Mitigation }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Mitigation.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Mitigation, Value = p.Mitigation }).Distinct().ToList(),

                "solutionDescription" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.SolutionDescription, Value = p.SolutionDescription }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.SolutionDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.SolutionDescription, Value = p.SolutionDescription }).Distinct().ToList(),

                "patchReference" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.PatchReference, Value = p.PatchReference }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.PatchReference.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.PatchReference, Value = p.PatchReference }).Distinct().ToList(),


                "productUpgradeReference" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.ProductUpgradeReference, Value = p.ProductUpgradeReference }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.ProductUpgradeReference.Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.ProductUpgradeReference, Value = p.ProductUpgradeReference }).Distinct().ToList(),

                "suppleMental" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.SuppleMental, Value = p.SuppleMental }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.SuppleMental.Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.SuppleMental, Value = p.SuppleMental }).Distinct().ToList(),

                "vendorCsr" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.VendorCsr, Value = p.VendorCsr }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.VendorCsr.Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.VendorCsr, Value = p.VendorCsr }).Distinct().ToList(),

                "subNetwork" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.SubNetwork, Value = p.SubNetwork }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.SubNetwork.Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.SubNetwork, Value = p.SubNetwork }).Distinct().ToList(),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email, Value = p.ModificationUserEntity.Email }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.ModificationUserEntity.Email.Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email, Value = p.ModificationUserEntity.Email }).Distinct().ToList(),

                "testReport" => string.IsNullOrEmpty(propertyFilter)
                  ? query.Select(p => new FilterValueDto
                  { Text = p.TestReport, Value = p.TestReport }).Distinct().ToList()
                  : query
                      .Where(x => x.TestReport.Contains(propertyFilter)).Select(p =>
                          new FilterValueDto { Text = p.TestReport, Value = p.TestReport }).Distinct()
                      .ToList(),

                "standardNir" => string.IsNullOrEmpty(propertyFilter)
                 ? query.Select(p => new FilterValueDto
                 { Text = p.StandardNir, Value = p.StandardNir }).Distinct().ToList()
                 : query
                     .Where(x => x.StandardNir.Contains(propertyFilter)).Select(p =>
                         new FilterValueDto { Text = p.StandardNir, Value = p.StandardNir }).Distinct()
                     .ToList(),

                "ericssonSecReport" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.EricssonSecReport, Value = p.EricssonSecReport }).Distinct().ToList()
                : query
                    .Where(x => x.EricssonSecReport.Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.EricssonSecReport, Value = p.EricssonSecReport }).Distinct()
                    .ToList(),

                "swAndStEntries" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.SwAndStEntries, Value = p.SwAndStEntries }).Distinct().ToList()
                : query
                    .Where(x => x.SwAndStEntries.Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.SwAndStEntries, Value = p.SwAndStEntries }).Distinct()
                    .ToList(),

                "penTestingReport" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.PenTestingReport, Value = p.PenTestingReport }).Distinct().ToList()
                   : query
                       .Where(x => x.PenTestingReport.Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.PenTestingReport, Value = p.PenTestingReport }).Distinct()
                       .ToList(),

                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion

        #region CRUD Operations

        public SystemVerificationProblemCreateDto GetCreatepage()
        {
            var severityEntity = _repositoryWrapper.SeverityRepository.FindAll().ToDictionary(x => x.Severityid, x => x.Severitydescription);
            var problemCategoryEntity = _repositoryWrapper.ProblemCategoryRepository.FindAll().ToDictionary(x => x.Problemcategoryid, x => x.Problemcategorydescription);
            var opCoEntity = _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            var environmentEntity = _repositoryWrapper.Environment.FindAll().ToDictionary(x => x.Environmentid, x => x.Environment);
            var systemTypes = _repositoryWrapper.SystemType.FindAll()
                            .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                            .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                            .Include(x => x.Majorsoftwarebuilds.Orgeqpmanufacturer)
                            .Include(x => x.Designcomponents.Where(x => x.Deleted == false))
                            .OrderByDescending(x => x.Modificationdate)
                            .Select(x => new DictionaryList { Key = x.Systemtypeid, Value = x.SystemTypeNameForDC(_repositoryWrapper) })
                            .ToList();
            var result = new SystemVerificationProblemCreateDto()
            {
                OpCos = opCoEntity,
                SeverityTypes = severityEntity,
                ProblemCategoryTypes = problemCategoryEntity,
                EnvironmentTypes = environmentEntity,       
                SystemTypes = systemTypes,
            };
            return result;

        }

        public async Task<ResultDto> Add(SystemVerificationProblemCreateDto dto)
        {
            var entity = _mapper.Map<SystemVerificationProblems>(dto);
            _repositoryWrapper.SystemVerificationProblemRepository.Create(SystemVerificationProblemMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        public SystemVerificationProblemUpdateDto GetUpdatePage(long id)
        {
            var SystemVerificationProblemModel = _repositoryWrapper.SystemVerificationProblemRepository.FindByCondition(x => x.Systemverificationproblemid == id).FirstOrDefault();
            var severityEntity = _repositoryWrapper.SeverityRepository.FindAll().ToDictionary(x => x.Severityid, x => x.Severitydescription);
            var problemCategoryEntity = _repositoryWrapper.ProblemCategoryRepository.FindAll().ToDictionary(x => x.Problemcategoryid, x => x.Problemcategorydescription);
            var opCoEntity = _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);
            var environmentEntity = _repositoryWrapper.Environment.FindAll().ToDictionary(x => x.Environmentid, x => x.Environment);
            var systemTypes = _repositoryWrapper.SystemType.FindAll()
                            .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                            .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                            .Include(x => x.Majorsoftwarebuilds.Orgeqpmanufacturer)
                            .Include(x => x.Designcomponents.Where(x => x.Deleted == false))
                            .OrderByDescending(x => x.Modificationdate)
                            .Select(x => new DictionaryList { Key = x.Systemtypeid, Value = x.SystemTypeNameForDC(_repositoryWrapper) })
                            .ToList();
            var dto = new SystemVerificationProblemUpdateDto();
            if (SystemVerificationProblemModel != null)
            {
                dto.SystemVerificationProblemId = SystemVerificationProblemModel.Systemverificationproblemid;
                dto.ProblemId = SystemVerificationProblemModel.Problemid;
                dto.OpCoId = (short)SystemVerificationProblemModel.Opcoid;
                dto.SystemTypeId = SystemVerificationProblemModel.Systemtypeid;
                dto.EnvironmentId = SystemVerificationProblemModel.Environmentid;
                dto.DateFound = SystemVerificationProblemModel.Datefound;
                dto.ProblemCategoryId = SystemVerificationProblemModel.Problemcategoryid;
                dto.ProblemDescription = SystemVerificationProblemModel.Problemdescription;
                dto.MaintenanceReference = SystemVerificationProblemModel.Maintenancereference;
                dto.Severity = SystemVerificationProblemModel.Severity;
                dto.StatusUrl = SystemVerificationProblemModel.Status;
                dto.Mitigation = SystemVerificationProblemModel.Mitigation;
                dto.SolutionDescription = SystemVerificationProblemModel.Solutiondescription;
                dto.PatchReference = SystemVerificationProblemModel.Patchreference;
                dto.ProductUpgradeReference = SystemVerificationProblemModel.Productupgradereference;
                dto.SuppleMental = SystemVerificationProblemModel.Supplemental;
                dto.SubNetwork = SystemVerificationProblemModel.Subnetwork;
                dto.VendorCsr = SystemVerificationProblemModel.Vendorcsr;
                dto.OpCos = opCoEntity;
                dto.SeverityTypes = severityEntity;
                dto.ProblemCategoryTypes = problemCategoryEntity;
                dto.EnvironmentTypes = environmentEntity;
                dto.SystemTypes = systemTypes;
                dto.TestReport = SystemVerificationProblemModel.Testreport;
                dto.StandardNir = SystemVerificationProblemModel.Standardnir;
                dto.EricssonSecReport = SystemVerificationProblemModel.Ericssonsecreport;
                dto.SwAndStEntries = SystemVerificationProblemModel.Swandstentries;
                dto.PenTestingReport = SystemVerificationProblemModel.Pentestingreport;

            }
            return dto;
        }

        public async Task<ResultDto> Update(SystemVerificationProblemUpdateDto dto)
        {


            var SystemVerificationProblemModel = _repositoryWrapper.SystemVerificationProblemRepository.FindByCondition(x => x.Systemverificationproblemid == dto.SystemVerificationProblemId).FirstOrDefault();
            var SystemVerificationProblemEntity = SystemVerificationProblemMapper.Get(SystemVerificationProblemModel);

            if(SystemVerificationProblemEntity != null)
            {
                SystemVerificationProblemEntity.ProblemId = dto.ProblemId;
                SystemVerificationProblemEntity.OpcoId = dto.OpCoId;
                SystemVerificationProblemEntity.SystemTypeId = dto.SystemTypeId;
                SystemVerificationProblemEntity.EnvironmentId = dto.EnvironmentId;
                SystemVerificationProblemEntity.DateFound = dto.DateFound;
                SystemVerificationProblemEntity.ProblemCategoryId = dto.ProblemCategoryId;
                SystemVerificationProblemEntity.ProblemDescription = dto.ProblemDescription;
                SystemVerificationProblemEntity.MaintenanceReference = dto.MaintenanceReference;
                SystemVerificationProblemEntity.Severity = dto.Severity;
                SystemVerificationProblemEntity.StatusUrl = dto.StatusUrl;
                SystemVerificationProblemEntity.Mitigation = dto.Mitigation;
                SystemVerificationProblemEntity.SolutionDescription = dto.SolutionDescription;
                SystemVerificationProblemEntity.PatchReference = dto.PatchReference;
                SystemVerificationProblemEntity.ProductUpgradeReference = dto.ProductUpgradeReference;
                SystemVerificationProblemEntity.SuppleMental = dto.SuppleMental;
                SystemVerificationProblemEntity.SubNetwork = dto.SubNetwork;
                SystemVerificationProblemEntity.VendorCsr = dto.VendorCsr;
                SystemVerificationProblemEntity.TestReport = dto.TestReport;
                SystemVerificationProblemEntity.StandardNir = dto.StandardNir;
                SystemVerificationProblemEntity.EricssonSecReport = dto.EricssonSecReport;
                SystemVerificationProblemEntity.SwAndStEntries = dto.SwAndStEntries;
                SystemVerificationProblemEntity.PenTestingReport = dto.PenTestingReport;

                var SystemVerificationProblemSetEntity = SystemVerificationProblemMapper.Set(SystemVerificationProblemEntity);
                _repositoryWrapper.SystemVerificationProblemRepository.Update(SystemVerificationProblemSetEntity);
                await _repositoryWrapper.SaveAsync();
            }


            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
                Data = SystemVerificationProblemEntity.SystemVerificationProblemId
            };
        }


        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.SystemVerificationProblemRepository
                .FindByCondition(x => x.Systemverificationproblemid == id).SingleAsync();

            if (entity != null)
            {
                _repositoryWrapper.SystemVerificationProblemRepository.Delete(entity);
                await _repositoryWrapper.SaveAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Systemverificationproblemid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotExists,
                    Data = id
                };
            }
        }
        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.SystemVerificationProblemRepository
               .FindByConditionWithDelete(x => x.Systemverificationproblemid == id).SingleAsync();
            if (entity != null)
            {
                _repositoryWrapper.SystemVerificationProblemRepository.DeleteDeep(entity);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = entity.Systemverificationproblemid
                };
            }
            else
            {
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotExists,
                    Data = id
                };
            }
        }
        #endregion
    }
}
