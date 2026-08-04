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
    public class NetworkElementClusterAsPlannedManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        public NetworkElementClusterAsPlannedManager(IEnumerable<IRepositoryWrapper> wrappers,
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
        #region Get NetworkElementClusterAsPlanned
        private IQueryable<Networkelementclusterasplanned> GetNWElementClusterAsPlannedRecord(ExpressionStarter<Networkelementclusterasplanned> predicateResult)
        {
            var query = _repositoryWrapper.NetworkElementClusterAsPlannedRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                .Include(x => x.Application)
                .Include(x => x.Deploymentstatus)
                .Include(x => x.Clusterupgradestatus)
                .AsQueryable();

            return query;
        }

        public async Task<QueryResultDto<NWElementClusterAsPlannedDtoGrid>> FindWithConditionAsync(NWElementClusterAsPlannedQueryDto filterDto)
        {
            var predicateResult = ApplyFilter(filterDto);

            var rtn = new QueryResultDto<NWElementClusterAsPlannedDtoGrid>(new GenerateRenderForGrid<NWElementClusterAsPlannedDtoGrid>(_customColumnManager))
            {

            };

            var AppClusterAsPlannedRecordResult = await Task.Run(() => GetNWElementClusterAsPlannedRecord(predicateResult).AsEnumerable()
                       .Select(p => NetworkElementClusterAsPlannedMapper.GetAppClusterAsPlannedMapper(p)).AsQueryable());

            rtn.TotalItems = AppClusterAsPlannedRecordResult.Count();

            var paginatedRecords = await Task.Run(() => AppClusterAsPlannedRecordResult.ApplyOrdering(filterDto, GetColumnsMap())
               .ApplyPaging(filterDto));

            IEnumerable<NWElementClusterAsPlannedDtoGrid> nwElementClusterAsPlannedDtoGridResult;

            nwElementClusterAsPlannedDtoGridResult = _mapper.Map<IEnumerable<NWElementClusterAsPlannedDtoGrid>>(paginatedRecords);

            rtn.Items = nwElementClusterAsPlannedDtoGridResult.ToArray();

            return rtn;

        }
        public ExpressionStarter<Networkelementclusterasplanned> ApplyFilter(NWElementClusterAsPlannedQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Networkelementclusterasplanned>(true);
            var paArchivedPredicate = PredicateBuilder.New<Networkelementclusterasplanned>(true);

            #region  
            if (filterDto.InfraClusterAsPlannedId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Networkelementclusterasplanned>();
                foreach (var id in filterDto.InfraClusterAsPlannedId)
                    componentIdPredicate.Or(x => x.Infraclusterasplannedid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.NetworkElementClusterAsPlannedId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Networkelementclusterasplanned>();
                foreach (var id in filterDto.NetworkElementClusterAsPlannedId)
                    componentIdPredicate.Or(x => x.Networkelementclusterasplannedid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.AppClusterName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Networkelementclusterasplanned>();
                foreach (var item in filterDto.AppClusterName)
                    descriptionPredicate.Or(x => x.Appclustername == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.ApplicationId?.Any() == true)
            {
                var podTypeNamePredicate = PredicateBuilder.New<Networkelementclusterasplanned>();
                foreach (var item in filterDto.ApplicationId)
                    podTypeNamePredicate.Or(x => x.Applicationid == item);

                mainPredicate.And(podTypeNamePredicate);
            }
            if (filterDto.DeploymentStatusId?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Networkelementclusterasplanned>();
                foreach (var item in filterDto.DeploymentStatusId)
                    descriptionPredicate.Or(x => x.Deploymentstatusid == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.LastModifiedBy?.Any() == true)
            {
                var modifiedByPredicate = PredicateBuilder.New<Networkelementclusterasplanned>();
                foreach (var email in filterDto.LastModifiedBy)
                    modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);

                mainPredicate.And(modifiedByPredicate);
            }
            if (filterDto.LastModified != null)
            {
                var LastModifiedValuePredicate = PredicateBuilder.New<Networkelementclusterasplanned>();
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
                var componentIdPredicate = PredicateBuilder.New<Networkelementclusterasplanned>();
                if (filterDto.LastModifiedValue.StartDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date >= filterDto.LastModifiedValue.StartDate);
                if (filterDto.LastModifiedValue.EndDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date <= filterDto.LastModifiedValue.EndDate);
                mainPredicate.And(componentIdPredicate);
            }


            #endregion


            return mainPredicate;
        }
        private Dictionary<string, Expression<Func<NetworkElementClusterAsPlanned, object>>[]> GetColumnsMap()
        {
            var returnCnfInfoDict = new Dictionary<string, Expression<Func<NetworkElementClusterAsPlanned, object>>[]>
            {
                ["infraClusterAsPlannedId"] = new Expression<Func<NetworkElementClusterAsPlanned, object>>[] { p => p.InfraClusterAsPlannedId },
                ["networkElementClusterAsPlannedId"] = new Expression<Func<NetworkElementClusterAsPlanned, object>>[] { p => p.NetworkElementClusterAsPlannedId },
                ["applicationId"] = new Expression<Func<NetworkElementClusterAsPlanned, object>>[] { p => p.ApplicationId },
                ["appClusterName"] = new Expression<Func<NetworkElementClusterAsPlanned, object>>[] { p => p.AppClusterName },
                ["deploymentStatusId"] = new Expression<Func<NetworkElementClusterAsPlanned, object>>[] { p => p.DeploymentStatusId },
                ["lastModified"] = new Expression<Func<NetworkElementClusterAsPlanned, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<NetworkElementClusterAsPlanned, object>>[] { p => p.ModificationUserEntity.Email },
            };

            return returnCnfInfoDict;
        }

        #endregion        

        #region AddEdit InfraCluster
      
        public async Task<ResultDto> AddOrRemoveNetWorkClusterAsync(List<NWElementClusterAsPlannedUpSertDto> dtoNetworkElementClusterAsPlannedRecords, long InfraClusterAsPlannedId)
        {
            try
            {
                if (dtoNetworkElementClusterAsPlannedRecords == null) return new ResultDto
                {
                    Info = ResultMessages.NoClusterUpgradeRecord
                };


                // 2. Get existing migration records for this Planned Activity
                var ExistingAppClusterEntities = _repositoryWrapper.NetworkElementClusterAsPlannedRepository
                    .FindByCondition(x => x.Infraclusterasplannedid == InfraClusterAsPlannedId)
                    .ToList();

                #region Add or Update ClusterUpgrade Records

                var newNetworkElementClusterAsPlannedRecords = dtoNetworkElementClusterAsPlannedRecords;

                foreach (var item in newNetworkElementClusterAsPlannedRecords)
                {
                    var nwElementClusterAsPlannedUpSertDto = new Networkelementclusterasplanned();
                    if (item.InfraClusterAsPlannedId == 0) continue;
                    // Create new record
                    if (item.NetworkElementClusterAsPlannedId == 0)
                    {
                        _repositoryWrapper.NetworkElementClusterAsPlannedRepository.Create(nwElementClusterAsPlannedUpSertDto);
                    }
                    else
                    {
                        var existingEntity = ExistingAppClusterEntities.FirstOrDefault(x =>
                             x.Networkelementclusterasplannedid == item.NetworkElementClusterAsPlannedId);

                        if (existingEntity != null)
                            nwElementClusterAsPlannedUpSertDto = existingEntity;
                        else
                            _repositoryWrapper.NetworkElementClusterAsPlannedRepository.Create(nwElementClusterAsPlannedUpSertDto);

                    }

                    // Assign properties
                    nwElementClusterAsPlannedUpSertDto.Appclustername = item.AppClusterName;
                    nwElementClusterAsPlannedUpSertDto.Infraclusterasplannedid = item.InfraClusterAsPlannedId;
                    nwElementClusterAsPlannedUpSertDto.Applicationid = item.ApplicationId;
                    nwElementClusterAsPlannedUpSertDto.Deploymentstatusid =  item.DeploymentStatusId;

                    if (nwElementClusterAsPlannedUpSertDto.Networkelementclusterasplannedid != 0)
                        _repositoryWrapper.NetworkElementClusterAsPlannedRepository.Update(nwElementClusterAsPlannedUpSertDto);

                    await _repositoryWrapper.SaveAsync();
                }

                await _repositoryWrapper.ClearTracker();

                #endregion

                #region delete AddOrRemoveNetWorkCluster  

                var deleteAppClusterUpgrade = ExistingAppClusterEntities
                    .Where(x => !ExistingAppClusterEntities.Where(x => x.Networkelementclusterasplannedid != 0)
                        .Select(dto => dto.Networkelementclusterasplannedid)
                        .Contains(x.Networkelementclusterasplannedid))
                    .ToList();


                foreach (var deleteItem in deleteAppClusterUpgrade)
                {
                    _repositoryWrapper.NetworkElementClusterAsPlannedRepository.DeleteDeep(deleteItem);
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
     string propertyName, string propertyFilter, NWElementClusterAsPlannedQueryDto filterDto)
        {
            var filteredQuery = FindWithConditionAsync(filterDto)?.Result.Items;

            var result = await Task.Run(() => propertyName switch
            {
                "infraClusterAsPlannedId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.InfraClusterAsPlannedId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.InfraClusterAsPlannedId))
                    .Distinct()
                    .ToList(),
                "networkElementClusterAsPlannedId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NetworkElementClusterAsPlannedId.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.NetworkElementClusterAsPlannedId.ToString(), Value = p.NetworkElementClusterAsPlannedId.ToString() })
                    .Distinct()
                    .ToList(),

                "applicationId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ApplicationId.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.ApplicationId.ToString(), Value = p.ApplicationId.ToString() })
                    .Distinct()
                    .ToList(),

                "appClusterName" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.AppClusterName.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.AppClusterName, Value = p.AppClusterName })
                    .Distinct()
                    .ToList(),

                //"lastModifiedBy" => filteredQuery
                //    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Modi.Contains(propertyFilter))
                //    .Select(p => new FilterValueDto(p.LastModifiedBy))
                //    .Distinct()
                //    .ToList(),
                

                "deploymentStatusId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.DeploymentStatusId.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.DeploymentStatusId.ToString(), Value = p.DeploymentStatusId.ToString() })
                    .Distinct()
                    .ToList(),

                _ => new List<FilterValueDto>(),
            });

            return result;
        }


        #endregion

        #region drop downs
        public async Task<NWElementClusterAsPlannedUpSertDto> GetAddPage()
        {
            var dto = new NWElementClusterAsPlannedUpSertDto();
            try
            {
                var applicationNames = await _repositoryWrapper.ProductNameRepository.FindAll().Select(x => new KeyValuePairDto
                {
                    Key = (long)x.Productnameid,
                    Text = x.Description
                })?.Distinct().OrderBy(x => x.Text).ToListAsync();
                var deploymentStatuses = await _repositoryWrapper.DeploymentStatus.FindAll().Select(x => new KeyValuePairDto
                {
                    Key = x.Deploymentstatusid,
                    Text = x.Deploymentstatus
                })?.Distinct().OrderBy(x => x.Text).ToListAsync();
                dto.ApplicationNames = applicationNames;
                dto.DeploymentStatuesResources = deploymentStatuses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return dto;
        }
        #endregion
    }
}
