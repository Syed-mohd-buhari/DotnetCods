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
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.ClusterLevelPA
{
    public class ClusterUpgradeManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        public ClusterUpgradeManager(IEnumerable<IRepositoryWrapper> wrappers,
            DesignComponentManager designComponentManager,
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor, CommonManager commonManager
            , DropdownDataServiceManager dropdownDataServiceManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;
            _logger = logger;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
        }
        #region Get InfraClusterAsPlan
        private IQueryable<Infraclusterasplanned> GetInfraClusterAsPlanRecord(ExpressionStarter<Infraclusterasplanned> predicateResult)
        {
            var query = _repositoryWrapper.InfraClusterAsPlannedRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                .Include(x => x.Clustertype).ThenInclude(x => x.Productname)
                .Include(x => x.Deploymentstatus)
                .Include(x => x.HardwaretypeNavigation).ThenInclude(x => x.Orgeqpmanufacturer)
                  .Include(x => x.Location)
                   .Include(x => x.Opco)
                    .Include(x => x.Platform)
                      .Include(x => x.Verticalresponsible)
                    .Include(x => x.Networkelementclusterasplanned)
                .AsQueryable();

            return query;
        }

        public async Task<QueryResultDto<InfraClusterAsPlannedDtoGrid>> FindWithConditionAsync(InfraClusterAsPlannedQueryDto filterDto)
        {
            var predicateResult = ApplyFilter(filterDto);

            var rtn = new QueryResultDto<InfraClusterAsPlannedDtoGrid>(new GenerateRenderForGrid<InfraClusterAsPlannedDtoGrid>(_customColumnManager))
            {

            };

            var infraClusterPlanResult = await Task.Run(() => GetInfraClusterAsPlanRecord(predicateResult).AsEnumerable()
                       .Select(p => InfraClusterAsPlannedMapper.GetInfraClusterAsPlanned(p)).AsQueryable());

            rtn.TotalItems = infraClusterPlanResult.Count();

            var paginatedRecords = await Task.Run(() => infraClusterPlanResult.ApplyOrdering(filterDto, GetColumnsMap())
               .ApplyPaging(filterDto));

            IEnumerable<InfraClusterAsPlannedDtoGrid> infraClusterAsPlannedResult;

            infraClusterAsPlannedResult = _mapper.Map<IEnumerable<InfraClusterAsPlannedDtoGrid>>(paginatedRecords);

            rtn.Items = infraClusterAsPlannedResult.ToArray();

            return rtn;

        }
        public ExpressionStarter<Infraclusterasplanned> ApplyFilter(InfraClusterAsPlannedQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Infraclusterasplanned>(true);
            var paArchivedPredicate = PredicateBuilder.New<Infraclusterasplanned>(true);

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

        #region AddEdit InfraCluster
      
        public async Task<ResultDto> AddOrUpdateClusterUpgradeAsync(List<ClusterUpgradeAddUpdateDto> dtoClusterUpgradeRecords, long paId)
        {
            try
            {
                if (dtoClusterUpgradeRecords == null) return new ResultDto
                {
                    Info = ResultMessages.NoClusterUpgradeRecord
                };


                // 2. Get existing migration records for this Planned Activity
                var ExistingClusterEntities = _repositoryWrapper.ClusterUpGradeStatusRepository
                    .FindByCondition(x => x.Plannedactivityid == paId)
                    .ToList();

                #region Add or Update ClusterUpgrade Records

                var newClusterUpgradeRecords = dtoClusterUpgradeRecords;

                foreach (var item in newClusterUpgradeRecords)
                {
                    var clusterupgradeDto = new Clusterupgradestatus();
                    if (item.StatusId == 0) continue;
                    // Create new record
                    if (item.ClusterUpGradeStatusId == 0)
                    {
                        _repositoryWrapper.ClusterUpGradeStatusRepository.Create(clusterupgradeDto);
                    }
                    else
                    {
                        var existingEntity = ExistingClusterEntities.FirstOrDefault(x =>
                             x.Infraclusterasplannedid == item.InfraClusterAsPlannedId);

                        if (existingEntity != null)
                            clusterupgradeDto = existingEntity;
                        else
                            _repositoryWrapper.ClusterUpGradeStatusRepository.Create(clusterupgradeDto);

                    }

                    // Assign properties
                    clusterupgradeDto.Statusid = item.StatusId;
                    clusterupgradeDto.Infraclusterasplannedid = item.InfraClusterAsPlannedId;
                    clusterupgradeDto.Plannedactivityid = paId;
                    clusterupgradeDto.Hardwaretype = item.PlannedHardwareTypeId != null || item.PlannedHardwareTypeId != 0 ? item.PlannedHardwareTypeId : null ;

                    if (clusterupgradeDto.Clusterupgradestatusid != 0)
                        _repositoryWrapper.ClusterUpGradeStatusRepository.Update(clusterupgradeDto);

                    await _repositoryWrapper.SaveAsync();
                }

                await _repositoryWrapper.ClearTracker();

                #endregion

                #region delete ClusterUpgrade  

                var deleteClusterUpgrade = ExistingClusterEntities
                    .Where(x => !newClusterUpgradeRecords.Where(x => x.ClusterUpGradeStatusId != 0)
                        .Select(dto => dto.ClusterUpGradeStatusId)
                        .Contains(x.Clusterupgradestatusid))
                    .ToList();


                foreach (var deleteItem in deleteClusterUpgrade)
                {
                    _repositoryWrapper.ClusterUpGradeStatusRepository.DeleteDeep(deleteItem);
                }

                await _repositoryWrapper.SaveAsync();
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
                _logger.LogError($"Issue occurred while adding/updating ClusterUpgrade entity: {ex.Message}");

                return new ResultDto
                {
                    Info = ResultMessages.EntryAddUpdateFailed,
                    Data = 0// This value is used for Archive method in DAPAManager
                };
            }
        }

        #endregion
              

        #region Filter
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync(
     string propertyName, string propertyFilter, InfraClusterAsPlannedQueryDto filterDto)
        {
            var filteredQuery = FindWithConditionAsync(filterDto)?.Result.Items;

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

    }
}
