using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DaMigrationStatus;
using CAM.DataTransferObjects.FunctionalityDto;
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

namespace CAM.BusinessManager.Entity.ExodusProgram
{
    public class DaMigrationStatusManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;

        public DaMigrationStatusManager(IEnumerable<IRepositoryWrapper> wrappers,
            GridCustomColumnManager customColumnManager, DropdownDataServiceManager dropdownDataServiceManager,
            IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
            CommonManager commonManager
            ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _customColumnManager = customColumnManager;
            _logger = logger;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
        }

        private IQueryable<Damigrationstatus> GetDaMigrationEntities(ExpressionStarter<Damigrationstatus> predicateResult)
        {
            var query = _repositoryWrapper.DaMigrationStatusRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                .Include(x => x.Opco)
                .Include(x => x.Location)
                .Include(x => x.Plannedactivity)
                .AsQueryable();

            return query;
        }

        public async Task<QueryResultDto<DaMigrationStatusDtoGrid>> FindWithConditionAsync(DaMigrationStatusQueryDto filterDto)
        {
            var predicateResult = ApplyFilter(filterDto);

            var rtn = new QueryResultDto<DaMigrationStatusDtoGrid>(new GenerateRenderForGrid<DaMigrationStatusDtoGrid>(_customColumnManager))
            {

            };

            var damigrationStatusResult = await Task.Run(() => GetDaMigrationEntities(predicateResult).AsEnumerable()
                       .Select(p => DaMigrationStatusMapper.GetDaMigrationStatus(p)).AsQueryable());

            rtn.TotalItems = damigrationStatusResult.Count();
            var paginatedRecords = await Task.Run(() => damigrationStatusResult.ApplyOrdering(filterDto, GetColumnsMap())
               .ApplyPaging(filterDto));

            paginatedRecords = paginatedRecords.ApplyPaging(filterDto);

            var data = paginatedRecords.Select(x => new DaMigrationStatusDtoGrid
            {
                DaMigrationStatusId = x.DaMigrationStatuId,
                PlannedActivityId = x.PlannedActivityId,
                OpcoId = x.OpcoId,
                Opco = x.OpcoName,
                LocationId = x.LocationId,
                Location = x.LocationName,
                StatusId = x.StatusId,
                Status = ConstantValueFilter.daMigrationStatusCode.Where(t => t.Key.ToString() == x.StatusId.ToString()).FirstOrDefault().Value,
                LastModifiedBy = x.ModificationUserEntity.Email,
                LastModified = x.ModificationDate,
                MigratonStatus = ConstantValueFilter.daMigrationStatusCode
            });
            rtn.Items = data.ToList();
            return rtn;

        }
        public ExpressionStarter<Damigrationstatus> ApplyFilter(DaMigrationStatusQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Damigrationstatus>(true);

            var paArchivedPredicate = PredicateBuilder.New<Damigrationstatus>();
            paArchivedPredicate.Or(x => x.Plannedactivity.Archived == false);
            mainPredicate.And(paArchivedPredicate);

            #region DaMigrationStatus
            if (filterDto.DaMigrationStatusId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Damigrationstatus>();
                foreach (var id in filterDto.DaMigrationStatusId)
                    componentIdPredicate.Or(x => x.Damigrationstatuid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.PlannedActivityId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Damigrationstatus>();
                foreach (var id in filterDto.PlannedActivityId)
                    componentIdPredicate.Or(x => x.Plannedactivityid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.Opco?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Damigrationstatus>();
                foreach (var item in filterDto.Opco)
                    descriptionPredicate.Or(x => x.Opcoid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Location?.Any() == true)
            {
                var podTypeNamePredicate = PredicateBuilder.New<Damigrationstatus>();
                foreach (var item in filterDto.Location)
                    podTypeNamePredicate.Or(x => x.Locationid.ToString() == item);

                mainPredicate.And(podTypeNamePredicate);
            }
            if (filterDto.Status?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Damigrationstatus>();
                foreach (var item in filterDto.Status)
                    descriptionPredicate.Or(x => x.Statusid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.LastModifiedBy?.Any() == true)
            {
                var modifiedByPredicate = PredicateBuilder.New<Damigrationstatus>();
                foreach (var email in filterDto.LastModifiedBy)
                    modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);

                mainPredicate.And(modifiedByPredicate);
            }
            if (filterDto.LastModified != null)
            {
                var LastModifiedValuePredicate = PredicateBuilder.New<Damigrationstatus>();
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
                var componentIdPredicate = PredicateBuilder.New<Damigrationstatus>();
                if (filterDto.LastModifiedValue.StartDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date >= filterDto.LastModifiedValue.StartDate);
                if (filterDto.LastModifiedValue.EndDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date <= filterDto.LastModifiedValue.EndDate);
                mainPredicate.And(componentIdPredicate);
            }


            #endregion


            return mainPredicate;
        }
        private Dictionary<string, Expression<Func<DaMigrationStatus, object>>[]> GetColumnsMap()
        {
            var returnCnfInfoDict = new Dictionary<string, Expression<Func<DaMigrationStatus, object>>[]>
            {
                ["daMigrationStatuId"] = new Expression<Func<DaMigrationStatus, object>>[] { p => p.DaMigrationStatuId },
                ["opcoId"] = new Expression<Func<DaMigrationStatus, object>>[] { p => p.OpcoId },
                ["locationId"] = new Expression<Func<DaMigrationStatus, object>>[] { p => p.LocationId },
                ["statusId"] = new Expression<Func<DaMigrationStatus, object>>[] { p => p.StatusId },

                ["lastModified"] = new Expression<Func<DaMigrationStatus, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<DaMigrationStatus, object>>[] { p => p.ModificationUserEntity.Email },
            };

            return returnCnfInfoDict;
        }

        #region Get Filter       
        public async Task<List<FilterValueDto>> GetFilters(string propertyName, string propertyFilter, DaMigrationStatusQueryDto filterDto)
        {
            var filterCriteria = ApplyFilter(filterDto);

            var vnfinfoQueryResult = await Task.Run(() => GetDaMigrationEntities(filterCriteria));

            var filteredQuery = vnfinfoQueryResult;
            var result = propertyName switch
            {
                #region VnfInfo Fields
                "daMigrationStatusId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Damigrationstatuid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Damigrationstatuid))
                    .Distinct()
                    .ToList(),

                "opco" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Opcoid.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto { Text = x.Opco.Opco, Value = x.Opcoid.ToString() })
                    .Distinct()
                    .ToList(),

                "location" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Locationid.ToString().Contains(propertyFilter))
                 .Select(x => new FilterValueDto { Text = x.Location.Location, Value = x.Locationid.ToString() })
               .Distinct()
               .ToList(),

                "plannedActivityId" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Locationid.ToString().Contains(propertyFilter))
                 .Select(x => new FilterValueDto(x.Plannedactivityid))
               .Distinct()
               .ToList(),

                "status" => filteredQuery
                             .Where(x => string.IsNullOrEmpty(propertyFilter)
                                         || x.Statusid.ToString().Contains(propertyFilter))
                             .Select(x => new FilterValueDto
                             {
                                 Value = ConstantValueFilter.daMigrationStatusCode.Where(t => t.Key.ToString() == x.Statusid.ToString()).FirstOrDefault().Value,
                                 Text = x.Statusid.ToString()
                             })
                             .Distinct()
                             .ToList(),


                #endregion
                _ => new List<FilterValueDto>()
            };

            return result;
        }
        #endregion
        #region Add / Update
        public async Task<DaMigrationStatusAddUpdateDto> GetUpdatePage(long PaId)
        {
            try
            {
                DaMigrationStatusAddUpdateDto returnDaMigration = new DaMigrationStatusAddUpdateDto();

                DaMigrationStatusQueryDto daMigrationFilterDto = new DaMigrationStatusQueryDto
                {
                    PlannedActivityId = new List<long> { PaId }
                };

                var opcoId = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == PaId).FirstOrDefault()?.Opcoid;

                var existingDaMigartion = await Task.Run(() => FindWithConditionAsync(daMigrationFilterDto).Result.Items.ToList());
                returnDaMigration.daMigrationStatusDtoGrids = existingDaMigartion;
                returnDaMigration.LocationResource = await _dropdownDataServiceManager.GetLocationDropDown(opcoId);

                returnDaMigration.statusKeyPairValue = ConstantValueFilter.daMigrationStatusCode;

                return returnDaMigration;

            }
            catch(Exception ex) 
            {
                _logger.LogError($"Issue is happed while get update page for DaMigrationStatus entity : {ex.Message} ");
                return null;
            }
        }
        public async Task<ResultDto> AddOrUpdateLocationAsync(DaMigrationStatusAddUpdateDto dtoDaMigrateRecords)
        {

            try
            {
                if(dtoDaMigrateRecords.daMigrationStatusDtoGrids == null || dtoDaMigrateRecords.daMigrationStatusDtoGrids.Count ==0)
                {
                    return new ResultDto {
                        //Info = ResultMessages.EntryNotFound
                        };
                }
                var addUpdateDto = dtoDaMigrateRecords.daMigrationStatusDtoGrids.ToList();
                var paId = addUpdateDto.FirstOrDefault().PlannedActivityId;

                var migratedLocationEntities = _repositoryWrapper.DaMigrationStatusRepository
                    .FindByCondition(x => x.Plannedactivityid == paId)
                    .ToList();

                #region Add New DaMigrationStatus

                foreach (var item in addUpdateDto)
                {
                    Damigrationstatus damigrationstatus = new Damigrationstatus();
                    if (item.DaMigrationStatusId == 0)
                    {

                        _repositoryWrapper.DaMigrationStatusRepository.Create(damigrationstatus);
                    }
                    else if (item.DaMigrationStatusId != 0)
                    {
                        var existingDaMigrationEntity = migratedLocationEntities.Where(x => x.Opcoid == item.OpcoId
                        && x.Locationid == item.LocationId && x.Damigrationstatuid == item.DaMigrationStatusId).FirstOrDefault();
                        if (existingDaMigrationEntity != null)
                        {
                            damigrationstatus = existingDaMigrationEntity;
                        }
                    }
                    damigrationstatus.Opcoid = item.OpcoId;
                    damigrationstatus.Locationid = item.LocationId;
                    damigrationstatus.Plannedactivityid = item.PlannedActivityId;
                    damigrationstatus.Statusid = item.StatusId;

                    if (damigrationstatus.Damigrationstatuid != 0)
                        _repositoryWrapper.DaMigrationStatusRepository.Update(damigrationstatus);

                    await _repositoryWrapper.SaveAsync();
                }


                await _repositoryWrapper.ClearTracker();
                #endregion

                #region //Delete Damigrationstatus

                var deleteDaMigrationStatus = migratedLocationEntities.ToList().Where(x =>
                                addUpdateDto.ToList().Select(x => x.DaMigrationStatusId).Contains(x.Damigrationstatuid) == false).ToList(); 
                foreach (var deleteItem in deleteDaMigrationStatus)
                {
                    _repositoryWrapper.DaMigrationStatusRepository.DeleteDeep(deleteItem);
                }
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();
                #endregion

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue is happed while Add/Update  DaMigrationStatus entity : {ex.Message} ");
                return new ResultDto();
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }


        #endregion
    }
}
