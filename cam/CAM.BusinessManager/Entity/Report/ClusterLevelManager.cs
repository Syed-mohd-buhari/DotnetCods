using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.Report;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.ClusterLevelPA;
using CAM.Entities.Model.ClusterLevelPA;
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

namespace CAM.BusinessManager.Entity.Report
{
    public class ClusterLevelManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        public ClusterLevelManager(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper, IMapper mapper, GridCustomColumnManager customColumnManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;
            _logger = logger;
            _commonManager = commonManager;
        }
        private IQueryable<Infraclusterasplanned> GetInfraClusterAsPlanRecord(ExpressionStarter<Infraclusterasplanned> predicateResult)
        {
            var query = _repositoryWrapper.InfraClusterAsPlannedRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                .Include(x => x.Clustertype).ThenInclude(x => x.Productname)
                .Include(x => x.Deploymentstatus)
                .Include(x => x.HardwaretypeNavigation)
                .Include(x => x.HardwaretypeNavigation).ThenInclude(x => x.Orgeqpmanufacturer)
                  .Include(x => x.HardwaretypeNavigation).ThenInclude(x => x.Platform)
                  .Include(x => x.Location)
                   .Include(x => x.Opco)
                    .Include(x => x.Platform)
                              .Include(x => x.Verticalresponsible)
                    .Include(x => x.Networkelementclusterasplanned)
                    .Include(x => x.Networkelementclusterasplanned).ThenInclude(x => x.Application)
                    .Include(x => x.Networkelementclusterasplanned).ThenInclude(x => x.Deploymentstatus)
                    .Include(x => x.Networkelementclusterasplanned).ThenInclude(x => x.ModificationuserNavigation)
                .AsQueryable();

            return query;
        }
        public async Task<QueryResultDto<ClusterLevelDtoGrid>> FindWithConditionAsync(ClusterLevelQueryDto filterDto)
        {
            var rtn = new QueryResultDto<ClusterLevelDtoGrid>(new GenerateRenderForGrid<ClusterLevelDtoGrid>(_customColumnManager))
            {

            };
            try
            {
                var predicateResult = ApplyFilter(filterDto);
                var infraClusterPlanResult = await Task.Run(() => GetInfraClusterAsPlanRecord(predicateResult).AsEnumerable()
                      .Select(p => InfraClusterAsPlannedMapper.GetInfraClusterAsPlanned(p)).AsQueryable());
                
                var paginatedRecords = await Task.Run(() => infraClusterPlanResult.ApplyOrdering(filterDto, GetColumnsMap()));

                //IEnumerable<ClusterLevelDtoGrid> infraClusterAsPlannedResult;


                var clusterLevelReportDtoGrids = paginatedRecords.SelectMany(infracluster => infracluster.NetworkElementClusterAsPlanned.DefaultIfEmpty(),
                   (infracluster, networkelement) => new ClusterLevelDtoGrid
                    {
                       InfraClusterAsPlannedId = infracluster.InfraClusterAsPlannedId, 
                       OpCoId = infracluster.OpCoId != null ? infracluster.OpCoId : 0,
                       OpCoValue =infracluster.OpCoValue != null ? infracluster.OpCoValue :string.Empty ,
                       LocationId = infracluster.LocationId != null ? infracluster.LocationId : 0  ,
                       LocationValue = infracluster.LocationValue != null ? infracluster.LocationValue : string.Empty ,
                       Site = infracluster.Site != null ? infracluster.Site:string.Empty,
                       PlatformId = infracluster.PlatformId != null ? infracluster.PlatformId : 0,
                       PlatformValue = infracluster.PlatformValue != null ? infracluster.PlatformValue : string.Empty,
                       ClustertypeId = infracluster.ClustertypeId != null ? infracluster.ClustertypeId : 0,
                       ClustertypeValue = infracluster.ClustertypeValue != null ? infracluster.ClustertypeValue : string.Empty,
                       ClusterName = infracluster.ClusterName != null ? infracluster.ClusterName : string.Empty,
                       DeploymentStatusId = networkelement!=null? networkelement.DeploymentStatusId: 0,
                       DeploymentStatusValue = networkelement != null ? networkelement.DeploymentStatus.DeploymentStatusDescription : string.Empty,
                       InfraClusterAsPlannedDeploymentStatus = infracluster.DeploymentStatusValue != null ? infracluster.DeploymentStatusValue : string.Empty,
                       ApplicationId = networkelement != null ? Convert.ToInt64(networkelement.ApplicationId) : 0,
                       ApplicationName = (networkelement !=null && networkelement.ApplicationName!=null) ? networkelement.ApplicationName : string.Empty,
                       AppClusterName = networkelement != null ? networkelement.AppClusterName : string.Empty,
                       HardwaretypeId = infracluster.HardwaretypeId != null ? infracluster.HardwaretypeId : 0,
                       HardwaretypeValue = infracluster.HardwaretypeValue != null ? infracluster.HardwaretypeValue : string.Empty,
                       VerticalResponsibleId = infracluster.VerticalResponsibleId != null ? infracluster.VerticalResponsibleId : 0,
                       VerticalResponsibleValue = infracluster.VerticalResponsibleValue != null ? infracluster.VerticalResponsibleValue : string.Empty,
                       LastModified = infracluster.ModificationDate,
                       LastModifiedBy = infracluster.ModificationuserNavigation.Email,
                       NetworkElementClusterdLastModified = networkelement != null ? networkelement.ModificationDate : null,
                       NetworkElementClusterdLastModifiedBy = networkelement != null ? networkelement.ModificationUserEntity.Email : " ",

                   }).ToList();
                
                rtn.TotalItems = clusterLevelReportDtoGrids.Count();               
                var data= clusterLevelReportDtoGrids.AsQueryable().ApplyPaging(filterDto);
                rtn.Items = data.ToArray();
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return rtn;
           

        }
        public ExpressionStarter<Infraclusterasplanned> ApplyFilter(ClusterLevelQueryDto filterDto)
        {
            var predicateResult = PredicateBuilder.New<Infraclusterasplanned>(true);
            var predicateInner = PredicateBuilder.New<Infraclusterasplanned>(true);
            try
            {
                #region  
                if (filterDto.InfraClusterAsPlannedId != null && filterDto.InfraClusterAsPlannedId?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var id in filterDto.InfraClusterAsPlannedId)
                        predicateInner.Or(x => x.Infraclusterasplannedid == id);

                    predicateResult.And(predicateInner);
                }
                if (filterDto.OpCoValue != null && filterDto.OpCoValue?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var id in filterDto.OpCoValue)
                        predicateInner.Or(x => x.Opcoid.ToString() == id);

                    predicateResult.And(predicateInner);
                }
                if (filterDto.OpCoId != null && filterDto.OpCoId?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var id in filterDto.OpCoId)
                        predicateInner.Or(x => x.Opcoid == id);

                    predicateResult.And(predicateInner);
                }
                if (filterDto.Site != null && filterDto.Site?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.Site)
                        predicateInner.Or(x => x.Site == item);

                    predicateResult.And(predicateInner);
                }
                if (filterDto.LocationValue != null && filterDto.LocationValue?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.LocationValue)
                        predicateInner.Or(x => x.Locationid.ToString() == item);

                    predicateResult.And(predicateInner);
                }
                if (filterDto.PlatformValue != null && filterDto.PlatformValue?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.PlatformValue)
                        predicateInner.Or(x => x.Platformid.ToString() == item);

                    predicateResult.And(predicateInner);
                }

                if (filterDto.ClusterName != null && filterDto.ClusterName?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.ClusterName)
                        predicateInner.Or(x => x.Clustername == item);

                    predicateResult.And(predicateInner);
                }

                if (filterDto.ClustertypeValue != null && filterDto.ClustertypeValue?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.ClustertypeValue)
                        predicateInner.Or(x => x.Clustertypeid.ToString() == item);

                    predicateResult.And(predicateInner);
                }

                if (filterDto.HardwareTypeValue != null && filterDto.HardwareTypeValue?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.HardwareTypeValue)
                        predicateInner.Or(x => x.Hardwaretype.ToString() == item);

                    predicateResult.And(predicateInner);
                }

                if (filterDto.InfraClusterAsPlannedDeploymentStatus != null && filterDto.InfraClusterAsPlannedDeploymentStatus?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.InfraClusterAsPlannedDeploymentStatus)
                        predicateInner.Or(x => x.Deploymentstatusid.ToString() == item);

                    predicateResult.And(predicateInner);
                }
                if (filterDto.DeploymentStatusValue != null && filterDto.DeploymentStatusValue?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.DeploymentStatusValue)
                        predicateInner.Or(x => x.Networkelementclusterasplanned.Any(x =>x.Deploymentstatusid.ToString() == item));

                    predicateResult.And(predicateInner);
                }

                if (filterDto.VerticalResponsibleValue != null && filterDto.VerticalResponsibleValue?.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.VerticalResponsibleValue)
                        predicateInner.Or(x => x.Verticalresponsibleid.ToString() == item);

                    predicateResult.And(predicateInner);
                }

                if (filterDto.ApplicationName != null && filterDto.ApplicationName.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.ApplicationName)
                        predicateInner.Or(x => x.Networkelementclusterasplanned.Any(x =>x.Applicationid.ToString() == item));

                    predicateResult.And(predicateInner);
                }
                if (filterDto.AppClusterName != null &&filterDto.AppClusterName.Any() == true)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.AppClusterName)
                        predicateInner.Or(x => x.Networkelementclusterasplanned.Any(x => x.Appclustername == item));

                    predicateResult.And(predicateInner);
                }

                if (filterDto.LastModifiedBy != null && filterDto.LastModifiedBy.Any())
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.LastModifiedBy)
                        predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                    predicateResult.And(predicateInner);
                }
                if (filterDto.LastModified != null)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    if (filterDto.LastModified.StartDate != null)
                        predicateInner.And(x => x.Modificationdate.Date >= filterDto.LastModified.StartDate);
                    if (filterDto.LastModified.EndDate != null)
                        predicateInner.And(x => x.Modificationdate.Date <= filterDto.LastModified.EndDate);
                    predicateResult.And(predicateInner);
                }
                if (filterDto.LastModifiedValue != null)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    if (filterDto.LastModifiedValue.StartDate != null)
                        predicateInner.And(x => x.Modificationdate.Date >= filterDto.LastModifiedValue.StartDate);
                    if (filterDto.LastModifiedValue.EndDate != null)
                        predicateInner.And(x => x.Modificationdate.Date <= filterDto.LastModifiedValue.EndDate);
                    predicateResult.And(predicateInner);

                }
                if (filterDto.NetworkElementClusterdLastModifiedBy != null && filterDto.NetworkElementClusterdLastModifiedBy.Any())
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.NetworkElementClusterdLastModifiedBy)
                        predicateInner.Or(x => x.Networkelementclusterasplanned.Any(x =>x.ModificationuserNavigation.Email == item) );
                    predicateResult.And(predicateInner);
                }
                if (filterDto.NetworkElementClusterdLastModified != null)
                {
                    predicateInner = PredicateBuilder.New<Infraclusterasplanned>();
                    if (filterDto.NetworkElementClusterdLastModified.StartDate != null)
                        predicateInner.And(x => x.Networkelementclusterasplanned.Any(x => x.Modificationdate >= filterDto.NetworkElementClusterdLastModified.StartDate));
                    if (filterDto.NetworkElementClusterdLastModified.EndDate != null)
                        predicateInner.And(x => x.Networkelementclusterasplanned.Any(x => x.Modificationdate <= filterDto.NetworkElementClusterdLastModified.EndDate));
                    predicateResult.And(predicateInner);

                }

                #endregion

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return predicateResult;
        }
        private Dictionary<string, Expression<Func<InfraClusterAsPlanned, object>>[]> GetColumnsMap()
        {
            var returnInfoDict = new Dictionary<string, Expression<Func<InfraClusterAsPlanned, object>>[]>
            {
                ["infraClusterAsPlannedId"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.InfraClusterAsPlannedId },
                ["opCoValue"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.OpCoValue },
                ["locationId"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.LocationId },
                ["locationValue"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.LocationValue },
                ["site"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.Site },
                ["platformValue"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.PlatformValue },
                ["clustertypeValue"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.ClustertypeValue },
                ["clusterName"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.ClusterName },
                ["hardwaretypeValue"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.HardwaretypeValue },
                ["deploymentStatusValue"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.NetworkElementClusterAsPlanned.Select(x => x.DeploymentStatus.DeploymentStatusDescription).FirstOrDefault()},
                ["infraClusterAsPlannedDeploymentStatus"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.DeploymentStatusValue },
                ["verticalResponsibleValue"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.VerticalResponsibleValue },
                ["applicationName"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.NetworkElementClusterAsPlanned.Select(x => x.ApplicationName).FirstOrDefault() },
                ["appClusterName"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.NetworkElementClusterAsPlanned.Select(x => x.AppClusterName).FirstOrDefault() },
                ["lastModified"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.ModificationuserNavigation.Email },
            };

            return returnInfoDict;
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, ClusterLevelQueryDto request)
        {
            var predicateResult = ApplyFilter(request);
            var filteredQuery = await Task.Run(() => GetInfraClusterAsPlanRecord(predicateResult));
            var result = propertyName switch
            {

                "infraClusterAsPlannedId" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Infraclusterasplannedid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Infraclusterasplannedid))
                    .Distinct()
                    .ToList(),

                "opCoValue" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Opco.Opco.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto
                    { Text = x.Opco.Opco,
                        Value = x.Opcoid.ToString() })
                    .Distinct()
                    .ToList(),
                "locationValue" => filteredQuery
                                   .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Location.Location.ToString().Contains(propertyFilter))
                                   .Select(x => new FilterValueDto { Text = x.Location.Location, Value = x.Locationid.ToString() })
                                   .Distinct()
                                   .ToList(),


                "site" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Site.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto(x.Site))
               .Distinct()
               .ToList(),

                "platformValue" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Platform.Platform.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Text = x.Platform.Platform, Value = x.Platformid.ToString() })
               .Distinct()
               .ToList(),



                "clusterName" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Clustername.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto{Text = x.Clustername ,Value = x.Clustername})
                .Distinct()
                .ToList(),

                "deploymentStatusId" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Deploymentstatus.Deploymentstatusid.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.Deploymentstatus.Deploymentstatusid))
                .Distinct()
                .ToList(),

                "deploymentStatusValue" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Networkelementclusterasplanned.Any(x =>x.Deploymentstatus.Deploymentstatus.Contains(propertyFilter)))
                .SelectMany(y => y.Networkelementclusterasplanned)
                .Select(y1 => new FilterValueDto 
                { Text = y1.Deploymentstatus.Deploymentstatus,
                  Value = y1.Deploymentstatusid.ToString() })
                .Distinct()
                .ToList(),

                "infraClusterAsPlannedDeploymentStatus" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Deploymentstatus.Deploymentstatus.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Text = x.Deploymentstatus.Deploymentstatus, Value = x.Deploymentstatusid.ToString() })
                .Distinct()
                .ToList(),


                "applicationName" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Networkelementclusterasplanned.Any(x => x.Application.Description.Contains(propertyFilter)))
                .SelectMany( y => y.Networkelementclusterasplanned)
                .Select(y1 => new FilterValueDto
                {
                    Value = y1.Applicationid.ToString(),
                    Text =y1.Application.Description
                })

                .Distinct()
                .ToList(),

                "appClusterName" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Networkelementclusterasplanned.Any(x => x.Appclustername.Contains(propertyFilter)))
                .SelectMany(y => y.Networkelementclusterasplanned)
                .Select(y1 => new FilterValueDto
                {
                    Value = y1.Appclustername,
                    Text = y1.Appclustername
                })

                .Distinct()
                .ToList(),              
                "clustertypeValue" => filteredQuery
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || (x.Clustertype.Productname.Description + "-" + x.Clustertype.Softwareversion).Contains(propertyFilter))
                 .Select(x => new FilterValueDto
                 { Text = x.Clustertype.Productname.Description + "-" + x.Clustertype.Softwareversion,
                   Value = x.Clustertypeid.ToString()
                 })
                 .Distinct()
                 .ToList(),
                "hardwaretypeValue" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || (x.HardwaretypeNavigation.Orgeqpmanufacturer.Originalequipmentmanufacturer + "-"
                + x.HardwaretypeNavigation.Platform.Platform + "-" + x.HardwaretypeNavigation.Hardwaretype).Contains(propertyFilter))
                .Select(x => new FilterValueDto 
                { Text = x.HardwaretypeNavigation.Orgeqpmanufacturer.Originalequipmentmanufacturer + "-" + x.HardwaretypeNavigation.Platform.Platform + "-" + x.HardwaretypeNavigation.Hardwaretype ,
                  Value = x.Hardwaretype.ToString()
                })
                .Distinct()
                .ToList(),
                "verticalResponsibleValue" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Verticalresponsible.Verticalresponsible.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Text = x.Verticalresponsible.Verticalresponsible, Value = x.Verticalresponsibleid.ToString() })
                .Distinct()
                .ToList(),
                "lastModifiedBy" => filteredQuery
                   .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationuserNavigation.Email.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.ModificationuserNavigation.Email))
                   .Distinct()
                   .ToList(),
                "networkElementClusterdLastModifiedBy" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Networkelementclusterasplanned.Any(x =>x.ModificationuserNavigation.Email.ToString().Contains(propertyFilter)))
                 .Select(x => new FilterValueDto(x.ModificationuserNavigation.Email))
                .Distinct()
                .ToList(),
                _ => new List<FilterValueDto>()
            };
            return result;
        }
    }
}
