using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ClusterLevelPA;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.ClusterLevelPA;
using CAM.Entities.Mappers.ClusterLevelPA;
using CAM.Entities.Model.ClusterLevelPA;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.ClusterLevelPA
{
    public class InfraClusterAsPlannedManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly ClusterUpgradeManager _clusterUpgradeManager;
        private readonly NetworkElementClusterAsPlannedManager _networkElementClusterAsPlannedManager;
        public InfraClusterAsPlannedManager(IEnumerable<IRepositoryWrapper> wrappers,
            DesignComponentManager designComponentManager,
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor, CommonManager commonManager
            , DropdownDataServiceManager dropdownDataServiceManager, ClusterUpgradeManager clusterUpgradeManager, NetworkElementClusterAsPlannedManager networkElementClusterAsPlannedManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;
            _logger = logger;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _clusterUpgradeManager = clusterUpgradeManager;
            _networkElementClusterAsPlannedManager = networkElementClusterAsPlannedManager;
        }
        #region Get InfraClusterAsPlan
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
                        .Include(x => x.Networkelementclusterasplanned).ThenInclude(x => x.Deploymentstatus)
                        .Include(x => x.Networkelementclusterasplanned).ThenInclude(x => x.Application)
                .AsQueryable();

            return query;
        }

        public async Task<QueryResultDto<InfraClusterAsPlannedDtoGrid>> FindWithConditionAsync(InfraClusterAsPlannedQueryDto filterDto)
        {


            var rtn = new QueryResultDto<InfraClusterAsPlannedDtoGrid>(new GenerateRenderForGrid<InfraClusterAsPlannedDtoGrid>(_customColumnManager))
            {

            };

            try
            {
                var predicateResult = ApplyFilter(filterDto);
                var infraClusterPlanResult = await Task.Run(() => GetInfraClusterAsPlanRecord(predicateResult).AsEnumerable()
                      .Select(p => InfraClusterAsPlannedMapper.GetInfraClusterAsPlanned(p)).AsQueryable());

                rtn.TotalItems = infraClusterPlanResult.Count();

                var paginatedRecords = await Task.Run(() => infraClusterPlanResult.ApplyOrdering(filterDto, GetColumnsMap())
                   .ApplyPaging(filterDto));

                IEnumerable<InfraClusterAsPlannedDtoGrid> infraClusterAsPlannedResult;

                infraClusterAsPlannedResult = _mapper.Map<IEnumerable<InfraClusterAsPlannedDtoGrid>>(paginatedRecords);

                rtn.Items = infraClusterAsPlannedResult.ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return rtn;

        }
        public ExpressionStarter<Infraclusterasplanned> ApplyFilter(InfraClusterAsPlannedQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Infraclusterasplanned>(true);
            var paArchivedPredicate = PredicateBuilder.New<Infraclusterasplanned>(true);
            try
            {
                #region  
                if (filterDto.InfraClusterAsPlannedId?.Any() == true)
                {
                    var componentIdPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var id in filterDto.InfraClusterAsPlannedId)
                        componentIdPredicate.Or(x => x.Infraclusterasplannedid == id);

                    mainPredicate.And(componentIdPredicate);
                }
                if (filterDto.OpCoValue?.Any() == true)
                {
                    var componentIdPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var id in filterDto.OpCoValue)
                        componentIdPredicate.Or(x => x.Opcoid.ToString() == id);

                    mainPredicate.And(componentIdPredicate);
                }
                if (filterDto.OpCoId?.Any() == true)
                {
                    var componentIdPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var id in filterDto.OpCoId)
                        componentIdPredicate.Or(x => x.Opcoid == id);

                    mainPredicate.And(componentIdPredicate);
                }
                if (filterDto.Site?.Any() == true)
                {
                    var descriptionPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.Site)
                        descriptionPredicate.Or(x => x.Site == item);

                    mainPredicate.And(descriptionPredicate);
                }
                if (filterDto.LocationValue?.Any() == true)
                {
                    var podTypeNamePredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.LocationValue)
                        podTypeNamePredicate.Or(x => x.Locationid.ToString() == item);

                    mainPredicate.And(podTypeNamePredicate);
                }
                if (filterDto.PlatformValue?.Any() == true)
                {
                    var descriptionPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.PlatformValue)
                        descriptionPredicate.Or(x => x.Platformid.ToString() == item);

                    mainPredicate.And(descriptionPredicate);
                }

                if (filterDto.ClusterName?.Any() == true)
                {
                    var descriptionPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.ClusterName)
                        descriptionPredicate.Or(x => x.Clustername == item);

                    mainPredicate.And(descriptionPredicate);
                }

                if (filterDto.ClustertypeValue?.Any() == true)
                {
                    var descriptionPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.ClustertypeValue)
                        descriptionPredicate.Or(x => x.Clustertypeid.ToString() == item);

                    mainPredicate.And(descriptionPredicate);
                }

                if (filterDto.HardwareTypeValue?.Any() == true)
                {
                    var descriptionPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.HardwareTypeValue)
                        descriptionPredicate.Or(x => x.Hardwaretype.ToString() == item);

                    mainPredicate.And(descriptionPredicate);
                }

                if (filterDto.DeploymentStatusValue?.Any() == true)
                {
                    var descriptionPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.DeploymentStatusValue)
                        descriptionPredicate.Or(x => x.Deploymentstatusid.ToString() == item);

                    mainPredicate.And(descriptionPredicate);
                }

                if (filterDto.VerticalResponsibleValue?.Any() == true)
                {
                    var descriptionPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var item in filterDto.VerticalResponsibleValue)
                        descriptionPredicate.Or(x => x.Verticalresponsibleid.ToString() == item);

                    mainPredicate.And(descriptionPredicate);
                }
                if (filterDto.LastModifiedBy?.Any() == true)
                {
                    var modifiedByPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    foreach (var email in filterDto.LastModifiedBy)
                        modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);

                    mainPredicate.And(modifiedByPredicate);
                }
                if (filterDto.LastModified != null)
                {
                    var LastModifiedValuePredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    if (filterDto.LastModified.StartDate != null)
                    {
                        _ = LastModifiedValuePredicate.And(x => x.Modificationdate.Date >= filterDto.LastModified.StartDate);
                    }

                    if (filterDto.LastModified.EndDate != null)
                    {
                        _ = LastModifiedValuePredicate.And(x => x.Modificationdate.Date <= filterDto.LastModified.EndDate);
                    }

                    _ = mainPredicate.And(LastModifiedValuePredicate);
                }
                if (filterDto.LastModifiedValue != null)
                {
                    var componentIdPredicate = PredicateBuilder.New<Infraclusterasplanned>();
                    if (filterDto.LastModifiedValue.StartDate != null)
                        componentIdPredicate.And(x => x.Modificationdate.Date >= filterDto.LastModifiedValue.StartDate);
                    if (filterDto.LastModifiedValue.EndDate != null)
                        componentIdPredicate.And(x => x.Modificationdate.Date <= filterDto.LastModifiedValue.EndDate);
                    mainPredicate.And(componentIdPredicate);
                }


                #endregion

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return mainPredicate;
        }
        private Dictionary<string, Expression<Func<InfraClusterAsPlanned, object>>[]> GetColumnsMap()
        {
            var returnCnfInfoDict = new Dictionary<string, Expression<Func<InfraClusterAsPlanned, object>>[]>
            {
                ["infraClusterAsPlannedId"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.InfraClusterAsPlannedId },
                ["opcoId"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.OpCoId },
                ["locationId"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.LocationId },
                ["Site"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.Site },

                ["platformId"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.PlatformId },
                ["clustertypeId"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.ClustertypeId },
                ["clusterName"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.ClusterName },
                ["hardwareType"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.HardwaretypeId },

                ["deploymentStatusId"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.DeploymentStatusId },
                ["verticalResponsibleId"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.VerticalResponsibleId },


                ["lastModified"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<InfraClusterAsPlanned, object>>[] { p => p.ModificationUserEntity.Email },
            };

            return returnCnfInfoDict;
        }

        #endregion
        #region get LCM PAwise Record
        public async Task<QueryResultDto<InfraClusterAsPlannedDtoGrid>> GetPlannedActivitiesByConditionAsync(InfraClusterAsPlannedQueryDto filterDto)
        {
            var result = new QueryResultDto<InfraClusterAsPlannedDtoGrid>(
               new GenerateRenderForGrid<InfraClusterAsPlannedDtoGrid>(_customColumnManager)
           );
            try
            {
                if (filterDto == null)
                {
                    filterDto = new InfraClusterAsPlannedQueryDto();
                }

                // Get In-Service Deployment Status Id
                var inServiceDeploymentStatusId = _repositoryWrapper.DeploymentStatus
                    .FindByCondition(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.InService.ToLower())
                    .FirstOrDefault()
                    ?.Deploymentstatusid;

                // Assign Deployment Status filter
                filterDto.DeploymentStatusValue = new List<string?>
                    {
                        inServiceDeploymentStatusId.ToString()
                    };

                #region Get PA-wise Cluster Upgrade Data

                var paInfraClusters = await _repositoryWrapper.InfraClusterAsPlannedRepository.FindByCondition(x => x.Clusterupgradestatus.Any(t => t.Plannedactivityid == filterDto.PaId))
                    .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Clustertype).ThenInclude(x => x.Productname)
                    .Include(x => x.Deploymentstatus)
                    .Include(x => x.HardwaretypeNavigation).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.HardwaretypeNavigation).ThenInclude(x => x.Platform)
                      .Include(x => x.Location)
                       .Include(x => x.Opco)
                        .Include(x => x.Platform)
                                  .Include(x => x.Verticalresponsible)
                        .Include(x => x.Networkelementclusterasplanned)
                    .ToListAsync();

                #endregion

                var filterPredicate = ApplyFilter(filterDto);



                //  InfraCluster records
                var infraClusterEntities = GetInfraClusterAsPlanRecord(filterPredicate)?.ToList();

                // Merge PA-wise records  
                if (infraClusterEntities?.Any() == true && paInfraClusters != null && paInfraClusters.Count > 0 )
                {
                    infraClusterEntities = infraClusterEntities.Union(paInfraClusters)?.DistinctBy(x => x.Infraclusterasplannedid)?.ToList();
                }
                else if (infraClusterEntities?.Any() == false && paInfraClusters != null && paInfraClusters.Count > 0 )
                {
                    infraClusterEntities = paInfraClusters?.DistinctBy(x => x.Infraclusterasplannedid)?.ToList();
                }

                result.TotalItems = infraClusterEntities?.Count() ?? 0;

                var orderedQuery = await Task.Run(() =>
                    infraClusterEntities
                        .Select(entity => InfraClusterAsPlannedMapper.GetInfraClusterAsPlanned(entity))
                        .AsQueryable()
                        .ApplyOrdering(filterDto, GetColumnsMap())
                );

                var pagedRecords = orderedQuery.ApplyPaging(filterDto).ToList();
                var mappedResult = _mapper.Map<IEnumerable<InfraClusterAsPlannedDtoGrid>>(pagedRecords);
                result.Items = mappedResult.ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }

        public async Task<List<SiteLevelClusterDto>> GetSitelevelClusters(InfraClusterAsPlannedQueryDto filterDto)
        {
            var result = new List<SiteLevelClusterDto>();
            try
            {
                if (filterDto == null)
                {
                    filterDto = new InfraClusterAsPlannedQueryDto();
                }

                // Get In-Service Deployment Status Id
                var inServiceDeploymentStatusId = _repositoryWrapper.DeploymentStatus
                    .FindByCondition(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.InService.ToLower())
                    .FirstOrDefault()
                    ?.Deploymentstatusid;

                // Assign Deployment Status filter
                filterDto.DeploymentStatusValue = new List<string?>
                    {
                        inServiceDeploymentStatusId.ToString()
                    };

                #region Get PA-wise Cluster Upgrade Data

                var paInfraClusters = await _repositoryWrapper.InfraClusterAsPlannedRepository.FindByCondition(x => x.Clusterupgradestatus.Any(t => t.Plannedactivityid == filterDto.PaId))
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
                    .ToListAsync();

                #endregion
                bool isPaavailble = false;
                if (filterDto.PaId != 0)
                {
                    isPaavailble = true;
                }

                var filterPredicate = isPaavailble ? null : ApplyFilter(filterDto);



                //  InfraCluster records
                var infraClusterEntities = isPaavailble ? null : GetInfraClusterAsPlanRecord(filterPredicate)?.ToList();

                // Merge PA-wise records  
                if (infraClusterEntities?.Any() == true && paInfraClusters != null && paInfraClusters.Count > 0 && !isPaavailble)
                {
                    infraClusterEntities = infraClusterEntities.Union(paInfraClusters)?.DistinctBy(x => x.Infraclusterasplannedid)?.ToList();
                }
                else if (paInfraClusters != null && paInfraClusters.Count > 0 && isPaavailble)
                {
                    infraClusterEntities = paInfraClusters?.DistinctBy(x => x.Infraclusterasplannedid)?.ToList();
                }

                //result.TotalItems = infraClusterEntities?.Count() ?? 0;

                var orderedQuery = await Task.Run(() =>
                                    infraClusterEntities?
                                    .GroupBy(s => s.Site)
                                    .OrderBy(x => x.Key)
                                    .Select(entity => new SiteLevelClusterDto
                                    {
                                        Site = entity.Key,
                                        infraClusterAsPlannedDtoGrid = entity.Select(group => new InfraClusterAsPlannedDtoGrid
                                        {
                                            InfraClusterAsPlannedId = group.Infraclusterasplannedid,
                                            OpCoId = group.Opcoid,
                                            OpCoValue = group.Opco.Opco,
                                            LocationId = group.Locationid,
                                            LocationValue = group.Location.Location,
                                            Site = group.Site,
                                            PlatformId = group.Platformid,
                                            PlatformValue = group.Platform.Platform,
                                            ClustertypeId = group.Clustertypeid,
                                            ClustertypeValue = group.Clustertype?.Productname?.Description + "-" + group.Clustertype?.Softwareversion,
                                            ClusterName = group.Clustername,
                                            HardwaretypeId = group.Hardwaretype,
                                            HardwaretypeValue = group.HardwaretypeNavigation?.Orgeqpmanufacturer?.Originalequipmentmanufacturer + "-" + group.HardwaretypeNavigation?.Platform?.Platform + "-" + group.HardwaretypeNavigation?.Hardwaretype,
                                            DeploymentStatusId = group.Deploymentstatusid,
                                            DeploymentStatusValue = group.Deploymentstatus.Deploymentstatus,
                                            VerticalResponsibleId = group.Verticalresponsibleid,
                                            VerticalResponsibleValue = group.Verticalresponsible.Verticalresponsible
                                        }).ToList()
                                    }).ToList());

                result = orderedQuery;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }

        #endregion

        #region AddEdit InfraCluster
        public async Task<ResultDto> AddOrUpdateInfraClusterAsync(InfraClusterClusterUpgradeUpsertDto dtoInfraClusterRecords, long paId, bool isWaitingForCluster = false, int? ruleLinkedDc = 0)
        {
            try
            {
                if (dtoInfraClusterRecords == null) return new ResultDto
                {
                    Info = ResultMessages.NoInfraClusterRecord
                };

                //var assetDeploymentStatus  = await _repositoryWrapper.DeploymentStatus.FindByCondition(x =>
                //      ConstantValueFilter.assetDeploymentStatusForClusterLevelPa.Contains(x.Deploymentstatus)).Select(x =>  new { x.Deploymentstatusid, x.Deploymentstatus }  )
                //                       .ToListAsync();

                var assetDeploymentStatus = await _repositoryWrapper.DeploymentStatus.FindAll().Select(x => new { x.Deploymentstatusid, x.Deploymentstatus })
                        .ToListAsync();

                var plannedStatusId = assetDeploymentStatus
                        .FirstOrDefault(x => x.Deploymentstatus.Equals(ConstantValueFilter.Planned, StringComparison.OrdinalIgnoreCase))
                        ?.Deploymentstatusid;

                var inServiceStatusId = assetDeploymentStatus
                    .FirstOrDefault(x => x.Deploymentstatus.Equals(ConstantValueFilter.inService, StringComparison.OrdinalIgnoreCase))
                    ?.Deploymentstatusid;

                var inCommissioningStatusId = assetDeploymentStatus
                    .FirstOrDefault(x => x.Deploymentstatus.Equals(ConstantValueFilter.inCommisioning, StringComparison.OrdinalIgnoreCase))
                    ?.Deploymentstatusid;


                if((dtoInfraClusterRecords?.infraClusterAsPlannedDtoGrid == null || dtoInfraClusterRecords?.infraClusterAsPlannedDtoGrid.Count <= 0) && (dtoInfraClusterRecords?.nwElementClusterAsPlannedUpSertDto != null && dtoInfraClusterRecords?.nwElementClusterAsPlannedUpSertDto.Count > 0))
                {
                    dtoInfraClusterRecords.infraClusterAsPlannedDtoGrid.Add(new InfraClusterAsPlannedDtoGrid
                    {
                        InfraClusterAsPlannedId = (long)dtoInfraClusterRecords.nwElementClusterAsPlannedUpSertDto.Select(x => x.InfraClusterAsPlannedId).FirstOrDefault(),
                        ClusterName = dtoInfraClusterRecords.nwElementClusterAsPlannedUpSertDto.Select(x => x.ClusterName).FirstOrDefault(),
                    });
                }

                // 1. Extract infraCluster DTOs and   Planned Activity ID
                var addUpdateDto = dtoInfraClusterRecords.infraClusterAsPlannedDtoGrid.ToList();


                var clusterUpgradeRecords = _repositoryWrapper.ClusterUpGradeStatusRepository.FindByCondition(x => x.Plannedactivityid == paId).ToList();
                if(clusterUpgradeRecords != null && clusterUpgradeRecords.Count > 0 )
                dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto = clusterUpgradeRecords?.Select(x => new ClusterUpgradeAddUpdateDto
                {
                    NetworkElementClusterAsPlannedId = x.Networkelementclusterasplannedid,
                    ClusterUpGradeStatusId = x.Clusterupgradestatusid,
                    PlannedActivityId = x.Plannedactivityid,
                    InfraClusterAsPlannedId = x.Infraclusterasplannedid,
                    StatusId = x.Statusid,
                    PlannedHardwareTypeId = x.Hardwaretype
                }).ToList();

                #region Add or Update InfraCluster Records               

                foreach (var item in addUpdateDto)
                {
                    var infraclusterasplannedEntity = new Infraclusterasplanned();
                    var isNewRecord = false;
                    var isChanged = false;
                    if (dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto == null)
                        dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto = new List<ClusterUpgradeAddUpdateDto>();

                    if (ruleLinkedDc == (int)PlannedActivityResourceEnum.Upgrade_HardwareTypes)
                    {
                        var existingClusterUpGrade = dtoInfraClusterRecords?.ClusterUpgradeAddUpdateDto?.Where(x =>
                                x.InfraClusterAsPlannedId == item.InfraClusterAsPlannedId).FirstOrDefault();

                        if (existingClusterUpGrade != null)
                        {
                            var Statusid = infraclusterasplannedEntity.Deploymentstatusid == plannedStatusId ? (short)1 :
                                    infraclusterasplannedEntity.Deploymentstatusid == inServiceStatusId ? (short)2 :
                                    (short)3;

                            var index = dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto
                                     .FindIndex(x => x.InfraClusterAsPlannedId == item.InfraClusterAsPlannedId);

                            if (index != -1)
                            {
                                dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto[index].StatusId = Statusid;
                                dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto[index].PlannedHardwareTypeId = dtoInfraClusterRecords?.PlannedHardwareTypeId;
                                dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto[index].ClusterUpGradeStatusId = existingClusterUpGrade.ClusterUpGradeStatusId;
                            }


                        }
                        else
                        {
                            dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto.Add(new ClusterUpgradeAddUpdateDto
                            {
                                InfraClusterAsPlannedId = item.InfraClusterAsPlannedId,
                                StatusId = item.DeploymentStatusId == inServiceStatusId ? (short)2 :
                                            (short)1,
                                PlannedHardwareTypeId = dtoInfraClusterRecords?.PlannedHardwareTypeId,

                            });
                        }
                    }
                    else if (ruleLinkedDc == (int)PlannedActivityResourceEnum.Add_Remove_Application_from_Cluster)
                    {
                        var existingClusterUpGrade = dtoInfraClusterRecords?.ClusterUpgradeAddUpdateDto?.Where(x =>
                                                        x.InfraClusterAsPlannedId == item.InfraClusterAsPlannedId).FirstOrDefault();

                        if (existingClusterUpGrade != null)
                        {
                            var Statusid = infraclusterasplannedEntity.Deploymentstatusid == plannedStatusId ? (short)1 :
                                    infraclusterasplannedEntity.Deploymentstatusid == inServiceStatusId ? (short)3 :
                                    (short)2;

                            var index = dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto
                                     .FindIndex(x => x.InfraClusterAsPlannedId == item.InfraClusterAsPlannedId);

                            if (index != -1)
                            {
                                dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto[index].StatusId = Statusid;
                                dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto[index].ClusterUpGradeStatusId = existingClusterUpGrade.ClusterUpGradeStatusId;
                            }


                        }
                        else
                        {
                            dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto.Add(new ClusterUpgradeAddUpdateDto
                            {
                                InfraClusterAsPlannedId = item.InfraClusterAsPlannedId,
                                StatusId = item.DeploymentStatusId == inServiceStatusId ? (short)3 :
                                            (short)2,
                            });
                        }

                       await _networkElementClusterAsPlannedManager.AddOrRemoveNetWorkClusterAsync(dtoInfraClusterRecords.nwElementClusterAsPlannedUpSertDto, item.InfraClusterAsPlannedId);
                    }
                    else
                    {
                        // Create new record
                        if (item.InfraClusterAsPlannedId == 0)
                        {
                            _repositoryWrapper.InfraClusterAsPlannedRepository.Create(infraclusterasplannedEntity);
                            isNewRecord = true;
                        }
                        // Update existing record
                        else
                        {
                            var existingEntity = _repositoryWrapper.InfraClusterAsPlannedRepository.FindByCondition(x =>
                                 x.Infraclusterasplannedid == item.InfraClusterAsPlannedId).FirstOrDefault();

                            if (existingEntity != null)
                                infraclusterasplannedEntity = existingEntity;
                            else
                            {
                                _repositoryWrapper.InfraClusterAsPlannedRepository.Create(infraclusterasplannedEntity);
                                isNewRecord = true;
                            }


                        }

                        // Assign properties
                        if (infraclusterasplannedEntity.Opcoid != item.OpCoId)
                        {
                            infraclusterasplannedEntity.Opcoid = item.OpCoId;
                            isChanged = true;
                        }

                        if (infraclusterasplannedEntity.Platformid != item.PlatformId)
                        {
                            infraclusterasplannedEntity.Platformid = item.PlatformId;
                            isChanged = true;
                        }

                        if (infraclusterasplannedEntity.Verticalresponsibleid != item.VerticalResponsibleId)
                        {
                            infraclusterasplannedEntity.Verticalresponsibleid = item.VerticalResponsibleId;
                            isChanged = true;
                        }

                        if (infraclusterasplannedEntity.Hardwaretype != item.HardwaretypeId)
                        {
                            infraclusterasplannedEntity.Hardwaretype = item.HardwaretypeId;
                            isChanged = true;
                        }

                        if (infraclusterasplannedEntity.Clustertypeid != item.ClustertypeId)
                        {
                            infraclusterasplannedEntity.Clustertypeid = item.ClustertypeId;
                            isChanged = true;
                        }

                        if (infraclusterasplannedEntity.Clustername != item.ClusterName)
                        {
                            infraclusterasplannedEntity.Clustername = item.ClusterName;
                            isChanged = true;
                        }

                        if (infraclusterasplannedEntity.Site != item.Site)
                        {
                            infraclusterasplannedEntity.Site = item.Site;
                            isChanged = true;
                        }

                        if (infraclusterasplannedEntity.Locationid != item.LocationId)
                        {
                            infraclusterasplannedEntity.Locationid = item.LocationId;
                            isChanged = true;
                        }

                        if (infraclusterasplannedEntity.Deploymentstatusid != item.DeploymentStatusId)
                        {
                            infraclusterasplannedEntity.Deploymentstatusid = item.DeploymentStatusId;
                            isChanged = true;
                        }
                        if (isWaitingForCluster && infraclusterasplannedEntity.Deploymentstatusid == plannedStatusId)
                        {
                            infraclusterasplannedEntity.Deploymentstatusid = inCommissioningStatusId;
                            isChanged = true;
                        }

                        if (infraclusterasplannedEntity.Infraclusterasplannedid != 0 && isChanged)
                            _repositoryWrapper.InfraClusterAsPlannedRepository.Update(infraclusterasplannedEntity);

                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        if (isNewRecord)
                        {
                            dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto.Add(new ClusterUpgradeAddUpdateDto
                            {
                                InfraClusterAsPlannedId = infraclusterasplannedEntity.Infraclusterasplannedid,
                                StatusId = infraclusterasplannedEntity.Deploymentstatusid == plannedStatusId ? (short)1 :
                                        (infraclusterasplannedEntity.Deploymentstatusid == inServiceStatusId ||
                                         infraclusterasplannedEntity.Deploymentstatusid == inCommissioningStatusId) ? (short)2 :
                                        (short)3
                            });
                        }
                        else
                        {
                            var existingClusterUpGrade = dtoInfraClusterRecords?.ClusterUpgradeAddUpdateDto?.Where(x =>
                                                            x.InfraClusterAsPlannedId == item.InfraClusterAsPlannedId).FirstOrDefault();


                            if (existingClusterUpGrade != null)
                            {
                                var Statusid = infraclusterasplannedEntity.Deploymentstatusid == plannedStatusId ? (short)1 :
                                        infraclusterasplannedEntity.Deploymentstatusid == inServiceStatusId ? (short)2 :
                                        (short)3;

                                var index = dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto
                                         .FindIndex(x => x.InfraClusterAsPlannedId == item.InfraClusterAsPlannedId);

                                if (index != -1)
                                {
                                    dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto[index].StatusId = Statusid;
                                    dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto[index].ClusterUpGradeStatusId = existingClusterUpGrade.ClusterUpGradeStatusId;
                                }


                            }
                            else
                            {
                                dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto.Add(new ClusterUpgradeAddUpdateDto
                                {
                                    InfraClusterAsPlannedId = infraclusterasplannedEntity.Infraclusterasplannedid,
                                    StatusId = infraclusterasplannedEntity.Deploymentstatusid == plannedStatusId ? (short)1 :
                                   (infraclusterasplannedEntity.Deploymentstatusid == inServiceStatusId ||
                                         infraclusterasplannedEntity.Deploymentstatusid == inCommissioningStatusId) ? (short)2 :
                                   (short)3
                                });
                            }
                        }

                    }

                }
                if(dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto != null && dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto.Count > 0)
                            await _clusterUpgradeManager.AddOrUpdateClusterUpgradeAsync(dtoInfraClusterRecords.ClusterUpgradeAddUpdateDto, paId);

                await _repositoryWrapper.ClearTracker();
                #endregion


                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = 1// This value is used for Archive method in DAPAManager
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue occurred while adding/updating InfraCluster entity: {ex.Message}");

                return new ResultDto
                {
                    Info = ResultMessages.EntryAddUpdateFailed,
                    Data = 0// This value is used for Archive method in DAPAManager
                };
            }
        }


        #endregion

        #region Edit InfraCluster

        public async Task<InfraClusterClusterUpgradeUpsertDto> GetAddPage(short opCoId, short DcId)
        {
            try
            {
                InfraClusterClusterUpgradeUpsertDto returnInfraCluster = new InfraClusterClusterUpgradeUpsertDto();

                returnInfraCluster.LocationReosurce = await _dropdownDataServiceManager.GetAllLocationsBasedOpcos();

                var opCoEntities = await _dropdownDataServiceManager.GetOpcoDetails();

                returnInfraCluster.OpcoReosurce = opCoEntities.Where(x => x.Key == opCoId).ToList();

                var (PlatformId, productNameId) = await Task.Run(() => GetPlatfromSoftwareHardwareDetails(DcId));

                var platformEntities = await _repositoryWrapper.Platform.FindByCondition(x => PlatformId == 0 || x.Platformid == PlatformId).Select(x => new KeyValuePairDto
                {
                    Key = x.Platformid,
                    Text = x.Platform
                })?.Distinct().OrderBy(x => x.Text).ToListAsync();

                returnInfraCluster.PlatfromResource = platformEntities;

                returnInfraCluster.DeploymentStatusReosurce = _repositoryWrapper.DeploymentStatus.FindByCondition(x =>
                    ConstantValueFilter.assetDeploymentStatusForClusterLevelPa.Contains(x.Deploymentstatus.ToLower())).Select(x =>
                                 new DataTransferObjects.Entita.ClusterLevelPA.DropdownKeyValueList { Key = x.Deploymentstatusid, Value = x.Deploymentstatus })
                                    .Distinct().OrderBy(x => x.Value).ToList();

                returnInfraCluster.HardwareMhwResource = await GetMajorHardware(PlatformId);
                returnInfraCluster.ClusterTypeMswResource = await GetMajorSoftware(productNameId);
                returnInfraCluster.VerticalResponsibleResource = await _dropdownDataServiceManager.GetVerticalFromLCMDto();

                return returnInfraCluster;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue is happed while get update page for InfraClusterAsPlanned entity : {ex.Message} ");
                return null;
            }
        }

        public async Task<InfraClusterClusterUpgradeUpsertDto> GetUpdatePage(long infrClusterId, short opCoId, short DcId)
        {
            try
            {
                InfraClusterClusterUpgradeUpsertDto returnInfraCluster = new InfraClusterClusterUpgradeUpsertDto();

                InfraClusterAsPlannedQueryDto filterDto = new InfraClusterAsPlannedQueryDto
                {
                    InfraClusterAsPlannedId = new List<long> { infrClusterId }
                };

                var (PlatformId, productNameId) = await Task.Run(() => GetPlatfromSoftwareHardwareDetails(DcId));

                var existingInfraCluster = await Task.Run(() => FindWithConditionAsync(filterDto).Result.Items.ToList());
                returnInfraCluster.infraClusterAsPlannedDtoGrid = existingInfraCluster;
                if (existingInfraCluster != null && existingInfraCluster.Count() > 0)
                {
                    returnInfraCluster.LocationReosurce = await _dropdownDataServiceManager.GetAllLocationsBasedOpcos();

                    var opCoEntities = await _dropdownDataServiceManager.GetOpcoDetails();

                    returnInfraCluster.OpcoReosurce = opCoEntities.Where(x => x.Key == opCoId).ToList();

                    var platformEntities = await _repositoryWrapper.Platform.FindByCondition(x => x.Platformid == PlatformId).Select(x => new KeyValuePairDto
                    {
                        Key = x.Platformid,
                        Text = x.Platform
                    })?.Distinct().OrderBy(x => x.Text).ToListAsync();
                    returnInfraCluster.PlatfromResource = platformEntities;

                    returnInfraCluster.DeploymentStatusReosurce = await _repositoryWrapper.DeploymentStatus.FindByCondition(x =>
                    ConstantValueFilter.assetDeploymentStatusForClusterLevelPa.Contains(x.Deploymentstatus)).Select(x =>
                                 new DataTransferObjects.Entita.ClusterLevelPA.DropdownKeyValueList { Key = x.Deploymentstatusid, Value = x.Deploymentstatus })
                                    .Distinct().OrderBy(x => x.Value).ToListAsync();

                    returnInfraCluster.HardwareMhwResource = await GetMajorHardware(PlatformId);
                    returnInfraCluster.ClusterTypeMswResource = await GetMajorSoftware(productNameId);
                    returnInfraCluster.VerticalResponsibleResource = await _dropdownDataServiceManager.GetVerticalFromLCMDto();

                    var notExistHardwareId = returnInfraCluster.HardwareMhwResource?.Where(x => x.Key != existingInfraCluster.FirstOrDefault()?.HardwaretypeId).FirstOrDefault() != null ? false : true;
                    if (notExistHardwareId)
                    {
                        var notExistHardware = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == existingInfraCluster.FirstOrDefault().HardwaretypeId).
                       Include(x => x.Orgeqpmanufacturer).Include(x => x.Platform).
                       Select(x => new KeyValuePairDto
                       {
                           Key = x.Majorhardwareid,
                           Text = x.Orgeqpmanufacturer.Originalequipmentmanufacturer + "-" + x.Platform.Platform + "-" + x.Hardwaretype

                       }).FirstOrDefault();
                        if (notExistHardware != null) returnInfraCluster.HardwareMhwResource.Add(notExistHardware);
                    }

                    var notExistClusterTypeId = returnInfraCluster.ClusterTypeMswResource?.Where(x => x.Key != existingInfraCluster.FirstOrDefault()?.ClustertypeId).FirstOrDefault() != null ? false : true;
                    if (notExistClusterTypeId)
                    {
                        var notExistClusterType = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Isvmware == true).Include(x => x.Productname)
                                .Select(x => new KeyValuePairDto
                                {
                                    Key = x.Majorsoftwarebuildsid,
                                    Text = x.Productname.Description + "-" + x.Softwareversion

                                }).FirstOrDefault();
                        if (notExistClusterType != null) returnInfraCluster.ClusterTypeMswResource.Add(notExistClusterType);
                    }
                }

                return returnInfraCluster;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue is happed while get update page for InfraClusterAsPlanned entity : {ex.Message} ");
                return null;
            }
        }
        #endregion

        #region Get Major Software and Hardware 

        public async Task<List<KeyValuePairDto>> GetMajorHardware(long productNameId)
        {
            //(OEM-Platform-HWModel)
            var majorHardwareEntities = await _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Buildconstruction.Buildconstruction.ToLower() == ConstantValueFilter.buildConst_proprietaryHW).
                   Include(x => x.Orgeqpmanufacturer).Include(x => x.Platform).Include(x => x.Buildconstruction).
                   Select(x => new KeyValuePairDto
                   {
                       Key = x.Majorhardwareid,
                       Text = x.Orgeqpmanufacturer.Originalequipmentmanufacturer + "-" + x.Platform.Platform + "-" + x.Hardwaretype

                   })?.Where(
                   x => !x.Text.ToLower().Contains(ConstantValueFilter.Unknown)).ToListAsync();

            return majorHardwareEntities;
        }

        public async Task<List<KeyValuePairDto>> GetMajorSoftware(long productNameId)
        {

            var majorSoftwareEntities = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Isvmware == true).Include(x => x.Productname)
                .Select(x => new KeyValuePairDto
                {
                    Key = x.Majorsoftwarebuildsid,
                    Text = x.Productname.Description + "-" + x.Softwareversion

                })?.Where(
                x => !x.Text.ToLower().Contains(ConstantValueFilter.Unknown) || !x.Text.ToLower().Contains("undefined")).ToListAsync();

            return majorSoftwareEntities;
        }


        public (long platformId, long productNameId) GetPlatfromSoftwareHardwareDetails(long dcId)
        {

            var dcEntity = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == dcId)
                .Include(x => x.Systemtype.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .Include(x => x.Systemtype.Majorsoftwarebuilds).FirstOrDefault();

            if (dcEntity == null) return (0, 0);

            var platFormId = (long)dcEntity?.Systemtype?.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain == true)?.FirstOrDefault()?.Majorhardware.Platformid;
            var productNameId = (long)dcEntity?.Systemtype?.Majorsoftwarebuilds?.Productnameid;


            return (platFormId, productNameId);
        }
        #endregion


        public async Task<ResultDto> ApplySettingUpdateRulesToClusterLevelPA(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, InfraClusterClusterUpgradeUpsertDto infraClusterClusterUpgradeUpsertDto)
        {
            try
            {
                // return await ArchivePaAndClusterSetInService(plannedActivityEntity, settingNew, infraClusterClusterUpgradeUpsertDto);
                var prevDeliveryStatusIdInfraReady = plannedActivityEntity.Deliverystatusid;
                var prevArchivedInfraReady = plannedActivityEntity.Archived;

                if (plannedActivityEntity.Lcmengineeringid != null)
                {

                    var plannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid, true, false)
                        .Include(x => x.Designaspect).FirstOrDefault();

                    plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                    plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;

                    var settingUpdatePlannedList = _repositoryWrapper.SettingsUpdatePlannedActivity.
                              FindByCondition(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.AddNewCluster)
                              .Select(x => x.Order).ToList();

                    var minimumDeliveryOrder = settingUpdatePlannedList.Min();
                    var maxDeliveryOrder = settingUpdatePlannedList.Max();

                    //#1895 -   Feedback -Add a Milestone Information for Cluster Level Planned Activities
                    bool isWaitingForCluster = (settingNew.Order > minimumDeliveryOrder
                             && settingNew.Order == (maxDeliveryOrder-1)) ? true : false;

                    await AddOrUpdateInfraClusterAsync(infraClusterClusterUpgradeUpsertDto, plannedActivityEntity.Plannedactivityid, isWaitingForCluster);


                    if (settingNew.Ruleelementcount != (int)ArchivingRuleEnum.ArchivePlannedActivityAndParentEntry && settingNew.Ruleelementcount != (int)ArchivingRuleEnum.ArchivePlannedActivityOnly)
                    {
                        _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                    }
                    else if ((settingNew.Ruleelementcount == (int)ArchivingRuleEnum.ArchivePlannedActivityOnly))
                    {
                        Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();
                        if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.AddNewCluster)
                        {

                            if (maxDeliveryOrder == settingNew.Order)
                            {

                                var DeploymentStatusReosurce = _repositoryWrapper.DeploymentStatus.FindByCondition(x =>
                  ConstantValueFilter.assetDeploymentStatusForClusterLevelPa.Contains(x.Deploymentstatus.ToLower())).Select(x =>
                               new DataTransferObjects.Entita.ClusterLevelPA.DropdownKeyValueList { Key = x.Deploymentstatusid, Value = x.Deploymentstatus }).ToList();

                                var clusterUpgradeEntities = _repositoryWrapper.ClusterUpGradeStatusRepository.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid)
                                    .Include(x => x.Infraclusterasplanned).ToList();
                                if (clusterUpgradeEntities != null && clusterUpgradeEntities.Count() > 0)
                                {
                                    var assetInserviceId = DeploymentStatusReosurce?.Where(x => x.Value.ToLower() == ConstantValueFilter.InService.ToLower())?.FirstOrDefault()?.Key;
                                    var infraClusterEntity = clusterUpgradeEntities?.Where(x => x.Infraclusterasplanned.Deploymentstatusid != assetInserviceId)
                                        .Select(x => x.Infraclusterasplanned)?.ToList();
                                    if (infraClusterEntity != null && infraClusterEntity.Count > 0)
                                    {
                                        foreach (var item in infraClusterEntity)
                                        {
                                            item.Deploymentstatusid = assetInserviceId;
                                            _repositoryWrapper.InfraClusterAsPlannedRepository.Update(item);

                                        }
                                    }
                                    await _repositoryWrapper.SaveAsync();
                                    foreach (var item in clusterUpgradeEntities?.Where(x => x.Statusid != 3))
                                    {
                                        item.Statusid = 3;
                                        _repositoryWrapper.ClusterUpGradeStatusRepository.Update(item);

                                    }
                                    await _repositoryWrapper.SaveAsync();

                                }

                                plannedActivity.Archived = true;
                                plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                                plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;

                                await _commonManager.setArchiveStatusForBpt(plannedActivity);

                            }
                        }

                        else
                        {
                            plannedActivity.Archived = true;
                            plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                            plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;

                            await _commonManager.setArchiveStatusForBpt(plannedActivity);
                        }
                        _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                    }

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                        Data = plannedActivity
                    };
                }
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }

        }


        public async Task<ResultDto> ArchivePaAndClusterSetInService(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, InfraClusterClusterUpgradeUpsertDto infraClusterClusterUpgradeUpsertDto)
        {
            try
            {
                var prevDeliveryStatusIdInfraReady = plannedActivityEntity.Deliverystatusid;
                var prevArchivedInfraReady = plannedActivityEntity.Archived;

                if (plannedActivityEntity.Lcmengineeringid != null)
                {

                    var plannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid, true, false)
                        .Include(x => x.Designaspect).FirstOrDefault();
                    plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                    plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;

                    await AddOrUpdateInfraClusterAsync(infraClusterClusterUpgradeUpsertDto, plannedActivityEntity.Plannedactivityid);

                    Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();
                    if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.AddNewCluster)
                    {
                        var settingUpdatePlannedList = _repositoryWrapper.SettingsUpdatePlannedActivity.
                            FindByCondition(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.AddNewCluster)
                            .Select(x => x.Order).ToList();

                        var minimumDeliveryOrder = settingUpdatePlannedList.Min();
                        var maxDeliveryOrder = settingUpdatePlannedList.Max();


                        if (maxDeliveryOrder == settingNew.Order)
                        {

                            var DeploymentStatusReosurce = _repositoryWrapper.DeploymentStatus.FindByCondition(x =>
              ConstantValueFilter.assetDeploymentStatusForClusterLevelPa.Contains(x.Deploymentstatus.ToLower())).Select(x =>
                           new DataTransferObjects.Entita.ClusterLevelPA.DropdownKeyValueList { Key = x.Deploymentstatusid, Value = x.Deploymentstatus }).ToList();

                            var clusterUpgradeEntities = _repositoryWrapper.ClusterUpGradeStatusRepository.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid)
                                .Include(x => x.Infraclusterasplanned).ToList();
                            if (clusterUpgradeEntities != null && clusterUpgradeEntities.Count() > 0)
                            {
                                var assetInserviceId = DeploymentStatusReosurce?.Where(x => x.Value.ToLower() == ConstantValueFilter.InService.ToLower())?.FirstOrDefault()?.Key;
                                var infraClusterEntity = clusterUpgradeEntities?.Where(x => x.Infraclusterasplanned.Deploymentstatusid != assetInserviceId)
                                    .Select(x => x.Infraclusterasplanned)?.ToList();
                                if (infraClusterEntity != null && infraClusterEntity.Count > 0)
                                {
                                    foreach (var item in infraClusterEntity)
                                    {
                                        item.Deploymentstatusid = assetInserviceId;
                                        _repositoryWrapper.InfraClusterAsPlannedRepository.Update(item);

                                    }
                                }
                                await _repositoryWrapper.SaveAsync();
                                foreach (var item in clusterUpgradeEntities?.Where(x => x.Statusid != 3))
                                {
                                    item.Statusid = 3;
                                    _repositoryWrapper.ClusterUpGradeStatusRepository.Update(item);

                                }
                                await _repositoryWrapper.SaveAsync();

                            }

                            plannedActivity.Archived = true;
                            plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                            plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;

                            await _commonManager.setArchiveStatusForBpt(plannedActivity);

                        }
                    }

                    _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                        Data = plannedActivity
                    };
                }
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }

        }

        public async Task<ResultDto> ApplySettingUpdateRulesForUpgradeCluster(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, InfraClusterClusterUpgradeUpsertDto clusterUpgradeAddUpdateDto)
        {
            try
            {
                var prevDeliveryStatusIdInfraReady = plannedActivityEntity.Deliverystatusid;
                var prevArchivedInfraReady = plannedActivityEntity.Archived;

                if (plannedActivityEntity.Lcmengineeringid != null)
                {

                    var plannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid, true, false)
                        .Include(x => x.Designaspect).FirstOrDefault();
                    plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                    plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;

                    //if(clusterUpgradeAddUpdateDto.ClusterUpgradeAddUpdateDto.Select(x => x.PlannedHardwareTypeId).Contains(clusterUpgradeAddUpdateDto.PlannedHardwareTypeId))
                    //    foreach(var item in clusterUpgradeAddUpdateDto.ClusterUpgradeAddUpdateDto) { item.PlannedHardwareTypeId = clusterUpgradeAddUpdateDto.PlannedHardwareTypeId; }

                    //await _clusterUpgradeManager.AddOrUpdateClusterUpgradeAsync(clusterUpgradeAddUpdateDto.ClusterUpgradeAddUpdateDto, plannedActivityEntity.Plannedactivityid);
                    await AddOrUpdateInfraClusterAsync(clusterUpgradeAddUpdateDto, plannedActivityEntity.Plannedactivityid, false,(int)PlannedActivityResourceEnum.Upgrade_HardwareTypes);

                    Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();
                    if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Upgrade_HardwareTypes)
                    {
                        var settingUpdatePlannedList = _repositoryWrapper.SettingsUpdatePlannedActivity.
                            FindByCondition(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Upgrade_HardwareTypes)
                            .Select(x => x.Order).ToList();
                        var minimumDeliveryOrder = settingUpdatePlannedList.Min();
                        var maxDeliveryOrder = settingUpdatePlannedList.Max();


                        if (maxDeliveryOrder == settingNew.Order)
                        {

                            var DeploymentStatusReosurce = _repositoryWrapper.DeploymentStatus.FindByCondition(x =>
              ConstantValueFilter.assetDeploymentStatusForClusterLevelPa.Contains(x.Deploymentstatus.ToLower())).Select(x =>
                           new DataTransferObjects.Entita.ClusterLevelPA.DropdownKeyValueList { Key = x.Deploymentstatusid, Value = x.Deploymentstatus }).ToList();

                            var clusterUpgradeEntities = _repositoryWrapper.ClusterUpGradeStatusRepository.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid)
                                .Include(x => x.Infraclusterasplanned).ToList();
                            if (clusterUpgradeEntities != null && clusterUpgradeEntities.Count() > 0)
                            {
                                var assetInserviceId = DeploymentStatusReosurce?.Where(x => x.Value.ToLower() == ConstantValueFilter.InService.ToLower())?.FirstOrDefault()?.Key;
                                var infraClusterEntity = clusterUpgradeEntities?.Where(x => x.Infraclusterasplanned.Deploymentstatusid == assetInserviceId)
                                    .Select(x => x.Infraclusterasplanned)?.ToList();
                                if (infraClusterEntity != null && infraClusterEntity.Count > 0)
                                {
                                    foreach (var item in infraClusterEntity)
                                    {
                                        item.Deploymentstatusid = assetInserviceId;
                                        item.Hardwaretype = clusterUpgradeEntities.Select(x => x.Hardwaretype).FirstOrDefault();
                                        _repositoryWrapper.InfraClusterAsPlannedRepository.Update(item);
                                    }
                                }
                                await _repositoryWrapper.SaveAsync();
                                foreach (var item in clusterUpgradeEntities?.Where(x => x.Statusid != 3))
                                {
                                    item.Statusid = 3;
                                    _repositoryWrapper.ClusterUpGradeStatusRepository.Update(item);

                                }
                                await _repositoryWrapper.SaveAsync();

                            }

                            plannedActivity.Archived = true;
                            plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                            plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;

                            await _commonManager.setArchiveStatusForBpt(plannedActivity);

                        }
                    }

                    _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                        Data = plannedActivity
                    };
                }
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }

        }

        public async Task<ResultDto> ApplySettingUpdateRulesForAddorRemoveCluster(Plannedactivities plannedActivityEntity, Settingsupdateplannedactivity settingNew, InfraClusterClusterUpgradeUpsertDto clusterUpgradeAddUpdateDto)
        {
            try
            {
                var prevDeliveryStatusIdInfraReady = plannedActivityEntity.Deliverystatusid;
                var prevArchivedInfraReady = plannedActivityEntity.Archived;

                if (plannedActivityEntity.Lcmengineeringid != null)
                {

                    var plannedActivity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid, true, false)
                        .Include(x => x.Designaspect).FirstOrDefault();
                    plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                    plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;

                    await AddOrUpdateInfraClusterAsync(clusterUpgradeAddUpdateDto, plannedActivityEntity.Plannedactivityid, false, (int)PlannedActivityResourceEnum.Add_Remove_Application_from_Cluster);

                    Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();
                    if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Add_Remove_Application_from_Cluster)
                    {
                        var settingUpdatePlannedList = _repositoryWrapper.SettingsUpdatePlannedActivity.
                            FindByCondition(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Add_Remove_Application_from_Cluster)
                            .Select(x => x.Order).ToList();
                        var minimumDeliveryOrder = settingUpdatePlannedList.Min();
                        var maxDeliveryOrder = settingUpdatePlannedList.Max();


                        if (maxDeliveryOrder == settingNew.Order)
                        {

                            var DeploymentStatusReosurce = _repositoryWrapper.DeploymentStatus.FindByCondition(x =>
                                                            
                            ConstantValueFilter.assetDeploymentStatusForClusterLevelPa.Contains(x.Deploymentstatus.ToLower())).Select(x => 
                            new DataTransferObjects.Entita.ClusterLevelPA.DropdownKeyValueList 
                            { Key = x.Deploymentstatusid, Value = x.Deploymentstatus })
                            .ToList();

                            var clusterUpgradeEntities = _repositoryWrapper.ClusterUpGradeStatusRepository.FindByCondition(x => x.Plannedactivityid == plannedActivityEntity.Plannedactivityid)
                                                    .Include(x => x.Infraclusterasplanned).AsQueryable().ToList();
                            var infraClusterIds = clusterUpgradeEntities.Select(x => x.Infraclusterasplannedid).ToList();
                            var networkelementClusters = _repositoryWrapper.NetworkElementClusterAsPlannedRepository.FindByCondition(x => infraClusterIds.Contains(x.Infraclusterasplannedid))
                                                        .ToList();
                            if (clusterUpgradeEntities != null && clusterUpgradeEntities.Count() > 0)
                            {
                                var assetInserviceId = DeploymentStatusReosurce?.Where(x => x.Value.ToLower() == ConstantValueFilter.InService.ToLower())?.FirstOrDefault()?.Key;
                                var assetRemoveId = DeploymentStatusReosurce?.Where(x => x.Value.ToLower() == ConstantValueFilter.Removed.ToLower())?.FirstOrDefault()?.Key;
                                var assetTrafficFreeId = DeploymentStatusReosurce?.Where(x => x.Value.ToLower() == ConstantValueFilter.trafficFree.ToLower())?.FirstOrDefault()?.Key;
                                var assetPlannedId = DeploymentStatusReosurce?.Where(x => x.Value.ToLower() == ConstantValueFilter.Planned.ToLower())?.FirstOrDefault()?.Key;
                                var assetInCommisioningId = DeploymentStatusReosurce?.Where(x => x.Value.ToLower() == ConstantValueFilter.inCommisioning.ToLower())?.FirstOrDefault()?.Key;

                                var infraClusterEntity = clusterUpgradeEntities?.Where(x => x.Infraclusterasplanned.Deploymentstatusid == assetInserviceId)
                                    .Select(x => x.Infraclusterasplanned)?.ToList();

                                var nwClusterTrafficFreeEntitries = networkelementClusters?.Where(x => x.Deploymentstatusid == assetTrafficFreeId).ToList();
                                var nwClusterPlannedEntitries = networkelementClusters?.Where(x => x.Deploymentstatusid == assetPlannedId || x.Deploymentstatusid == assetInCommisioningId).ToList();


                                foreach (var item in clusterUpgradeEntities?.Where(x => x.Statusid != 3))
                                {
                                    item.Statusid = 3;
                                    _repositoryWrapper.ClusterUpGradeStatusRepository.Update(item);

                                }
                                await _repositoryWrapper.SaveAsync();

                                if (nwClusterTrafficFreeEntitries != null && nwClusterTrafficFreeEntitries.Count > 0)
                                {
                                    foreach (var item in nwClusterTrafficFreeEntitries)
                                    {
                                        item.Deploymentstatusid = assetRemoveId;
                                        _repositoryWrapper.NetworkElementClusterAsPlannedRepository.Update(item);
                                    }

                                }
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();

                                if (nwClusterPlannedEntitries != null && nwClusterPlannedEntitries.Count > 0)
                                {
                                    foreach (var item in nwClusterPlannedEntitries)
                                    {
                                        item.Deploymentstatusid = assetInserviceId;
                                        _repositoryWrapper.NetworkElementClusterAsPlannedRepository.Update(item);
                                    }

                                }
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();

                            }

                            plannedActivity.Archived = true;
                            plannedActivity.Deliverystatusid = settingNew.Deliverystatusid;
                            plannedActivity.Activitystatusid = completedActivityStatus.Activitystatusid;

                            await _commonManager.setArchiveStatusForBpt(plannedActivity);

                        }
                    }

                    _repositoryWrapper.PlannedActivity.Update(plannedActivity);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                        Data = plannedActivity
                    };
                }
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResultDto
                {
                    Info = "",
                    Data = ""
                };
            }

        }

        #region Filter
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync(
     string propertyName, string propertyFilter, InfraClusterAsPlannedQueryDto filterDto)
        {
            var filteredQuery = GetPlannedActivitiesByConditionAsync(filterDto)?.Result.Items;

            var result = await Task.Run(() => propertyName switch
            {
                "infraClusterAsPlannedId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.InfraClusterAsPlannedId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.InfraClusterAsPlannedId))
                    .Distinct()
                    .ToList(),
                "opCoValue" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OpCoValue.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.OpCoValue, Value = p.OpCoId.ToString() })
                    .Distinct()
                    .ToList(),

                "locationValue" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.LocationValue.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.LocationValue, Value = p.LocationId.ToString() })
                    .Distinct()
                    .ToList(),

                "site" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Site.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Site, Value = p.Site })
                    .Distinct()
                    .ToList(),

                "lastModifiedBy" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.LastModifiedBy.Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.LastModifiedBy))
                    .Distinct()
                    .ToList(),

                "platformValue" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PlatformValue.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.PlatformValue, Value = p.PlatformId.ToString() })
                    .Distinct()
                    .ToList(),

                "clustertypeValue" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ClustertypeValue.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.ClustertypeValue, Value = p.ClustertypeId.ToString() })
                    .Distinct()
                    .ToList(),

                "clusterName" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ClusterName.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.ClusterName, Value = p.ClusterName.ToString() })
                    .Distinct()
                    .ToList(),
                "hardwareTypeValue" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.HardwaretypeValue.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.HardwaretypeValue, Value = p.HardwaretypeId.ToString() })
                    .Distinct()
                    .ToList(),

                "deploymentStatusValue" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.DeploymentStatusValue.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.DeploymentStatusValue, Value = p.DeploymentStatusId.ToString() })
                    .Distinct()
                    .ToList(),
                "verticalResponsibleValue" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VerticalResponsibleValue.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.VerticalResponsibleValue, Value = p.VerticalResponsibleId.ToString() })
                    .Distinct()
                    .ToList(),

                _ => new List<FilterValueDto>(),
            });

            return result;
        }


        #endregion

        public async Task<List<NWElementClusterAsPlannedDto>> GetInfraClusterListBasedonOpco(InfraClusterAsPlannedQueryDto dto)
        {
            if (dto == null)
            {
                dto = new InfraClusterAsPlannedQueryDto();
            }
            var result = new List<NWElementClusterAsPlannedDto>();
            try
            {
                var inServiceDeploymentStatusId = _repositoryWrapper.DeploymentStatus
                    .FindByCondition(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.InService.ToLower())
                    .FirstOrDefault()
                    ?.Deploymentstatusid;
                var removedDeploymentStatusId = _repositoryWrapper.DeploymentStatus
                                                .FindByCondition(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.Removed.ToLower())
                                                .FirstOrDefault()
                                                ?.Deploymentstatusid;

                dto.DeploymentStatusValue = new List<string?>
                {
                    inServiceDeploymentStatusId.ToString()
                };

                var paInfraClusters = await _repositoryWrapper.InfraClusterAsPlannedRepository.FindByCondition(x => x.Clusterupgradestatus.Any(t => t.Plannedactivityid == dto.PaId))
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
                        .Include(x => x.Networkelementclusterasplanned).ThenInclude(x => x.Deploymentstatus)
                        .Include(x => x.Networkelementclusterasplanned).ThenInclude(x => x.Application)
                    .ToListAsync();

                bool isPaavailble = false;
                if (dto.PaId != 0)
                {
                    isPaavailble = true;
                }

                var filterPredicate = isPaavailble ? null : ApplyFilter(dto);
                var infraClusterEntities = isPaavailble ? null : GetInfraClusterAsPlanRecord(filterPredicate)?.ToList();
                if (paInfraClusters != null && paInfraClusters.Count > 0 && isPaavailble)
                {
                    infraClusterEntities = paInfraClusters?.DistinctBy(x => x.Infraclusterasplannedid)?.ToList();
                }

                var orderedQuery = await Task.Run(() =>
                                    infraClusterEntities?.Select(x => new NWElementClusterAsPlannedDto
                                    {
                                        ClusterName = x.Clustername,
                                        InfraClusterAsPlannedId = x.Infraclusterasplannedid,
                                        NWElementClusterAsPlannedUpSertDto = x.Networkelementclusterasplanned.Where(r => r.Deploymentstatusid != removedDeploymentStatusId)
                                        .Select(n => new NWElementClusterAsPlannedUpSertDto
                                        {
                                            InfraClusterAsPlannedId = x.Infraclusterasplannedid,
                                            ClusterName = x.Clustername,
                                            NetworkElementClusterAsPlannedId = n.Networkelementclusterasplannedid,
                                            AppClusterName = n.Appclustername,
                                            DeploymentStatusValue = n.Deploymentstatus?.Deploymentstatus,
                                            ApplicationName = n.Application?.Description,
                                            DeploymentStatusId = n.Deploymentstatusid,
                                            ApplicationId = n.Applicationid,
                                        }).ToList()
                                    }).ToList());

                result = orderedQuery;
            }
            catch (Exception ex)
            {
                
            }
            return result;

        }
    }
}
