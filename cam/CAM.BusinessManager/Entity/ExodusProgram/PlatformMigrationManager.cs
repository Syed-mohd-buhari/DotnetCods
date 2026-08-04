using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DeploymentStatus;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
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
    public class PlatformMigrationManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly DesignComponentFamilyLifeCycleManager _dcfLifeCycleManager;

        private readonly Lazy<LcmEngineeringManager> _lcmEngineeringManager;

        public PlatformMigrationManager(IEnumerable<IRepositoryWrapper> wrappers,
            GridCustomColumnManager customColumnManager, DropdownDataServiceManager dropdownDataServiceManager,
            IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
            CommonManager commonManager, Lazy<LcmEngineeringManager> lcmEngineeringManager, DesignComponentFamilyLifeCycleManager dcfLifeCycleManager

            ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _customColumnManager = customColumnManager;
            _logger = logger;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _dcfLifeCycleManager = dcfLifeCycleManager;
            _lcmEngineeringManager = lcmEngineeringManager;
        }

        #region Grid , Filter 
        private IQueryable<Daassetmigration> GetDaAssetMigrationEntities(ExpressionStarter<Daassetmigration> predicateResult)
        {
            var query = _repositoryWrapper.DaAssetMigrationRepository.FindByCondition(predicateResult)
                 .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation).Include(x => x.Networkelementasplanned).ThenInclude(x => x.Deploymentstatus)
                .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Environment)
                .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Location)
                .Include(x => x.Plannedactivity)
                .Include(x => x.Deploymentstatus)
                .Include(x => x.Environment)
                .Include(x => x.Targetdesigncomponenet)
                .Include(x => x.Location)
                .AsQueryable();

            return query;
        }
        public async Task<QueryResultDto<DaAssetMigrationDtoGrid>> FindWithConditionAsync(DaAssetMigrationQueryDto filterDto)
        {
            var predicateResult = ApplyFilter(filterDto);

            var rtn = new QueryResultDto<DaAssetMigrationDtoGrid>(new GenerateRenderForGrid<DaAssetMigrationDtoGrid>(_customColumnManager))
            {

            };

            var damigrationStatusResult = await Task.Run(() => GetDaAssetMigrationEntities(predicateResult).AsEnumerable()
                       .Select(p => DaAssetMigrationMapper.GetDaAssetMigration(p)).AsQueryable());

            rtn.TotalItems = damigrationStatusResult.Count();
            var paginatedRecords = await Task.Run(() => damigrationStatusResult.ApplyOrdering(filterDto, GetColumnsMap()));

            if (filterDto.IsPagination)
                paginatedRecords = paginatedRecords.ApplyPaging(filterDto);

            var data = paginatedRecords.Select(x => new DaAssetMigrationDtoGrid
            {
                DaAssetMigrationId = x.DaAssetMigrationId,
                NetworkElementAsPlannedId = x.NetworkElementAsPlannedId,
                PlannedActivityId = x.PlannedActivityId,

                OpcoId = x.OpcoId,
                LocationId = x.NewLocationId,
                OldAssetName = x.OldAssetName,
                OldDeploymentStatus = x.OldDeploymentStatus,
                OldDeploymentType = x.OldDeploymentType,
                OldEnvironment = x.OldEnvironment,

                TargetDesignComponenetId = x.TargetDesignComponenetId,
                NewDeploymentStatus = x.NewDeploymentStatus,

                NewEnvironment = x.NewEnvironment,

                NewDeploymentStatusId = x.NewDeploymentStatusId,
                NewelEmentName = x.NewelEmentName,

                NewEnvironmentId = x.NewEnvironmentId,
                OpcoDesc = x.NewOpco,
                Location = x.NewLocation,

                CurrentDesignComponenet = DesignComponentTypeExtensionMethod.ToDesignComponentNameBasedOnDcId((long)(x.CurrentDesignComponenetId), _repositoryWrapper),
                TargetDesignComponenet = DesignComponentTypeExtensionMethod.ToDesignComponentNameBasedOnDcId((long)(x.TargetDesignComponenetId ?? 0), _repositoryWrapper),
                CurrentDcId = x.CurrentDesignComponenetId


,
                RfoDate = getOnlyDate(x.RfoDate),
                RfsDate = getOnlyDate(x.RfsDate),
                MigrationCompletionDate = getOnlyDate(x.MigrationCompletionDate),
                TrafficNodePercentage = x.TrafficNodePercentage,

                LastModifiedBy = x.ModificationUserEntity.Email,
                LastModified = x.ModificationDate,

                BomSubmittedDate = getOnlyDate(x.BomSubmittedDate),
                HwPoRaisedDate = getOnlyDate(x.HwPoRaisedDate),
                HwPoArrivedDate = getOnlyDate(x.HwPoArrivedDate),
                RfaDate = getOnlyDate(x.RfaDate),
                IsDecommissioned=x.IsDecommissioned,
                VecDate = getOnlyDate(x.Vecdate),
                StartOfAppIntegration = getOnlyDate(x.Startofappintegration),
                MigrationStart = getOnlyDate(x.Migrationstart),
            });
            rtn.Items = data.ToList();
            return rtn;

        }
        public DateTime? getOnlyDate(DateTime? dateValue)
        {
            if (dateValue != null)
            {
                var temp = dateValue?.Date;
                return dateValue?.Date;
            }
            return dateValue;

        }
        public ExpressionStarter<Daassetmigration> ApplyFilter(DaAssetMigrationQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Daassetmigration>(true);

            var paArchivedPredicate = PredicateBuilder.New<Daassetmigration>();
            paArchivedPredicate.Or(x => x.Plannedactivity.Archived == false);
            mainPredicate.And(paArchivedPredicate);

            #region Daassetmigration
            if (filterDto.isDecommissioned?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var id in filterDto.isDecommissioned)
                {
                    if(id==ConstantValueFilter.Yes)
                    {
                        componentIdPredicate.Or(x => x.Isdecommissioned==true);
                    }
                    else if(id == ConstantValueFilter.No)
                    {
                        componentIdPredicate.Or(x => x.Isdecommissioned == false);
                    }
                }
                    

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.DaAssetMigrationId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var id in filterDto.DaAssetMigrationId)
                    componentIdPredicate.Or(x => x.Daassetmigrationid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.PlannedActivityId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var id in filterDto.PlannedActivityId)
                    componentIdPredicate.Or(x => x.Plannedactivityid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.OpcoDesc?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.OpcoDesc)
                    descriptionPredicate.Or(x => x.Opcoid == item);

                mainPredicate.And(descriptionPredicate);
            }
            #region  Required for UI auto filter removed for OPco
            if (filterDto.OpcoId?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.OpcoId)
                    descriptionPredicate.Or(x => x.Opcoid == item);

                mainPredicate.And(descriptionPredicate);
            }
            #endregion
            if (filterDto.LocationDesc?.Any() == true)
            {
                var podTypeNamePredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.LocationDesc)
                    podTypeNamePredicate.Or(x => x.Locationid == item);

                mainPredicate.And(podTypeNamePredicate);
            }
            if (filterDto.DeploymentStatusDesc?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.DeploymentStatusDesc)
                    descriptionPredicate.Or(x => x.Deploymentstatusid == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.EnvironmentDesc?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.EnvironmentDesc)
                    descriptionPredicate.Or(x => x.Environmentid == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.TargetDesignComponenet?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.TargetDesignComponenet)
                    descriptionPredicate.Or(x => x.Targetdesigncomponenetid.ToString() == item.ToString());

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.RfoDate != null)
            {
                var rfoDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.RfoDate.StartDate != null)
                {
                    _ = rfoDateValuePredicate.And(x => x.Rfodate >= filterDto.RfoDate.StartDate);
                }

                if (filterDto.RfoDate.EndDate != null)
                {
                    _ = rfoDateValuePredicate.And(x => x.Rfodate <= filterDto.RfoDate.EndDate);
                }

                _ = mainPredicate.And(rfoDateValuePredicate);
            }
            if (filterDto.RfsDate != null)
            {
                var rfsDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.RfsDate.StartDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Rfsdate >= filterDto.RfsDate.StartDate);
                }

                if (filterDto.RfsDate.EndDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Rfsdate <= filterDto.RfsDate.EndDate);
                }

                _ = mainPredicate.And(rfsDateValuePredicate);
            }
            if (filterDto.MigrationCompletionDate != null)
            {
                var migrationDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.MigrationCompletionDate.StartDate != null)
                {
                    _ = migrationDateValuePredicate.And(x => x.Migrationcompletiondate >= filterDto.MigrationCompletionDate.StartDate);
                }

                if (filterDto.MigrationCompletionDate.EndDate != null)
                {
                    _ = migrationDateValuePredicate.And(x => x.Migrationcompletiondate <= filterDto.MigrationCompletionDate.EndDate);
                }

                _ = mainPredicate.And(migrationDateValuePredicate);
            }

            if (filterDto.TrafficNodePercentage?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.TrafficNodePercentage)
                    descriptionPredicate.Or(x => x.Trafficnodepercentage.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.LastModifiedBy?.Any() == true)
            {
                var modifiedByPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var email in filterDto.LastModifiedBy)
                    modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);

                mainPredicate.And(modifiedByPredicate);
            }
            if (filterDto.LastModified != null)
            {
                var LastModifiedValuePredicate = PredicateBuilder.New<Daassetmigration>();
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
                var componentIdPredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.LastModifiedValue.StartDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date >= filterDto.LastModifiedValue.StartDate);
                if (filterDto.LastModifiedValue.EndDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date <= filterDto.LastModifiedValue.EndDate);
                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.OldAssetName != null && filterDto.OldAssetName?.Any() == true)
            {
                var OldAssetNamePredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.OldAssetName)
                    OldAssetNamePredicate.Or(x => x.Networkelementasplannedid == Convert.ToInt16(item));
                mainPredicate.And(OldAssetNamePredicate);
            }
            if (filterDto.NewelEmentName != null && filterDto.NewelEmentName?.Any() == true)
            {
                var NewelEmentNamePredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.NewelEmentName)
                    NewelEmentNamePredicate.Or(x => x.Newelementname == item);

                mainPredicate.And(NewelEmentNamePredicate);
            }
            if (filterDto.NewEnvironment != null && filterDto.NewEnvironment?.Any() == true)
            {
                var NewEnvironmentPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.NewEnvironment)
                    NewEnvironmentPredicate.Or(x => x.Environment.Environmentid == Convert.ToInt16(item));
                mainPredicate.And(NewEnvironmentPredicate);
            }
            if (filterDto.NewDeploymentStatus != null && filterDto.NewDeploymentStatus?.Any() == true)
            {
                var NewDeploymentStatusPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.NewDeploymentStatus)
                    NewDeploymentStatusPredicate.Or(x => x.Deploymentstatus.Deploymentstatusid == Convert.ToInt16(item));
                mainPredicate.And(NewDeploymentStatusPredicate);
            }
            if (filterDto.Location != null && filterDto.Location?.Any() == true)
            {
                var LocationPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.Location)
                    LocationPredicate.Or(x => x.Location.Locationid == Convert.ToInt16(item));
                mainPredicate.And(LocationPredicate);
            }
            if (filterDto.RfaDate != null)
            {
                var rfsDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.RfaDate.StartDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Rfadate >= filterDto.RfaDate.StartDate);
                }

                if (filterDto.RfaDate.EndDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Rfadate <= filterDto.RfaDate.EndDate);
                }

                _ = mainPredicate.And(rfsDateValuePredicate);
            }
            if (filterDto.HwPoRaisedDate != null)
            {
                var rfsDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.HwPoRaisedDate.StartDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Hwporaiseddate >= filterDto.HwPoRaisedDate.StartDate);
                }

                if (filterDto.HwPoRaisedDate.EndDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Hwporaiseddate <= filterDto.HwPoRaisedDate.EndDate);
                }
                _ = mainPredicate.And(rfsDateValuePredicate);
            }
            if (filterDto.HwPoArrivedDate != null)
            {
                var rfsDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.HwPoArrivedDate.StartDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Hwpoarriveddate >= filterDto.HwPoArrivedDate.StartDate);
                }
                if (filterDto.HwPoArrivedDate.EndDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Hwpoarriveddate <= filterDto.HwPoArrivedDate.EndDate);
                }

                _ = mainPredicate.And(rfsDateValuePredicate);
            }
            if (filterDto.BomSubmittedDate != null)
            {
                var rfsDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.BomSubmittedDate.StartDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Bomsubmitteddate >= filterDto.BomSubmittedDate.StartDate);
                }
                if (filterDto.BomSubmittedDate.EndDate != null)
                {
                    _ = rfsDateValuePredicate.And(x => x.Bomsubmitteddate <= filterDto.BomSubmittedDate.EndDate);
                }

                _ = mainPredicate.And(rfsDateValuePredicate);
            }
            if (filterDto.VecDate != null)
            {
                var rfoDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.VecDate.StartDate != null)
                {
                    _ = rfoDateValuePredicate.And(x => x.Vecdate >= filterDto.VecDate.StartDate);
                }

                if (filterDto.VecDate.EndDate != null)
                {
                    _ = rfoDateValuePredicate.And(x => x.Vecdate <= filterDto.VecDate.EndDate);
                }

                _ = mainPredicate.And(rfoDateValuePredicate);
            }
            if (filterDto.StartOfAppIntegration != null)
            {
                var rfoDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.StartOfAppIntegration.StartDate != null)
                {
                    _ = rfoDateValuePredicate.And(x => x.Startofappintegration >= filterDto.StartOfAppIntegration.StartDate);
                }

                if (filterDto.StartOfAppIntegration.EndDate != null)
                {
                    _ = rfoDateValuePredicate.And(x => x.Startofappintegration <= filterDto.StartOfAppIntegration.EndDate);
                }

                _ = mainPredicate.And(rfoDateValuePredicate);
            }
            if (filterDto.MigrationStart != null)
            {
                var rfoDateValuePredicate = PredicateBuilder.New<Daassetmigration>();
                if (filterDto.MigrationStart.StartDate != null)
                {
                    _ = rfoDateValuePredicate.And(x => x.Migrationstart >= filterDto.MigrationStart.StartDate);
                }

                if (filterDto.MigrationStart.EndDate != null)
                {
                    _ = rfoDateValuePredicate.And(x => x.Migrationstart <= filterDto.MigrationStart.EndDate);
                }

                _ = mainPredicate.And(rfoDateValuePredicate);
            }
            #endregion


            return mainPredicate;
        }
        private Dictionary<string, Expression<Func<DaAssetMigration, object>>[]> GetColumnsMap()
        {
            var returnCnfInfoDict = new Dictionary<string, Expression<Func<DaAssetMigration, object>>[]>
            {
                ["daAssetMigrationId"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.DaAssetMigrationId },
                ["opcoId"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.OpcoId },
                ["newLocationId"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.NewLocationId },
                ["networkElementAsPlannedId"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.NetworkElementAsPlannedId },
                ["newelEmentName"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.NewelEmentName },
                ["targetDesignComponenetId"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.TargetDesignComponenetId },
                ["newDeploymentStatusId"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.NewDeploymentStatusId },

                ["trafficNodePercentage"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.TrafficNodePercentage },
                ["rfoDate"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.RfoDate },
                ["rfsDate"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.RfsDate },
                ["migrationCompletionDate"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.MigrationCompletionDate },
                ["lastModified"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.ModificationUserEntity.Email },
                ["rfaDate"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.RfaDate },
                ["hwPoRaisedDate"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.HwPoRaisedDate },
                ["hwPoArrivedDate"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.HwPoArrivedDate },
                ["bomSubmittedDate"] = new Expression<Func<DaAssetMigration, object>>[] { p => p.BomSubmittedDate },
            };

            return returnCnfInfoDict;
        }

        public async Task<List<FilterValueDto>> GetFilteredValuesAsync(string propertyName, string propertyFilter, DaAssetMigrationQueryDto filterDto)
        {

            var assetEntity = await GetAssetsForPlatformMigrationGrid(filterDto);

            var filteredQuery = assetEntity.Items;

            var result = propertyName switch
            {
                "daAssetMigrationId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.DaAssetMigrationId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.DaAssetMigrationId))
                    .Distinct()
                    .ToList(),
                "plannedActivityId" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PlannedActivityId.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.PlannedActivityId))
                .Distinct()
                .ToList(),
                "oldAssetName" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OldAssetName.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.OldAssetName, Value = p.NetworkElementAsPlannedId.ToString() })
                    .Distinct()
                    .ToList(),

                "opcoDesc" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OpcoDesc.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.OpcoDesc, Value = p.OpcoId.ToString() })
                    .Distinct()
                    .ToList(),

                "location" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Location.ToString().Contains(propertyFilter))
                .Select(p => new FilterValueDto { Text = p.Location, Value = p.LocationId.ToString() })
                .Distinct()
                .ToList(),

                "trafficNodePercentage" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.TrafficNodePercentage.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.TrafficNodePercentage))
                .Distinct()
                .ToList(),

                "oldDeploymentType" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OldDeploymentType.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto(x.OldDeploymentType))
               .Distinct()
               .ToList(),


                "oldDeploymentStatus" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OldDeploymentStatus.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto(x.OldDeploymentStatus))
               .Distinct()
               .ToList(),

                "newelEmentName" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NewelEmentName.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto(x.NewelEmentName))
               .Distinct()
               .ToList(),

                "currentDesignComponenet" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CurrentDesignComponenet.ToString().Contains(propertyFilter))
               .Select(p => new FilterValueDto { Text = p.CurrentDesignComponenet, Value = p.CurrentDcId.ToString() })
               .Distinct()
               .ToList(),

                "targetDesignComponenet" => filteredQuery
             .Where(x => string.IsNullOrEmpty(propertyFilter) || x.TargetDesignComponenet.ToString().Contains(propertyFilter))
             .Select(p => new FilterValueDto { Text = p.TargetDesignComponenet, Value = p.TargetDesignComponenetId.ToString() })
             .Distinct()
             .ToList(),
                "newEnvironment" => filteredQuery
             .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NewEnvironment.ToString().Contains(propertyFilter))
             .Select(p => new FilterValueDto { Text = p.NewEnvironment, Value = p.NewEnvironmentId.ToString() })
             .Distinct()
             .ToList(),
                "newDeploymentStatus" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NewDeploymentStatus.ToString().Contains(propertyFilter))
                .Select(p => new FilterValueDto { Text = p.NewDeploymentStatus, Value = p.NewDeploymentStatusId.ToString() })
                .Distinct()
                .ToList(),
                "lastModifiedBy" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.LastModifiedBy.ToString().Contains(propertyFilter))
                .Select(p => new FilterValueDto(p.LastModifiedBy))
                .Distinct()
                .ToList(),
                "isDecommissioned" => filteredQuery
             .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NewEnvironment.ToString().Contains(propertyFilter))
             .Select(p => new FilterValueDto { Text = p.IsDecommissioned == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.IsDecommissioned == true ? ConstantValueFilter.Yes : ConstantValueFilter.No })
             .Distinct()
             .ToList(),
                _ => new List<FilterValueDto>(),
            };

            return result;
        }


        #endregion
        #region Get Asset and Insert Asset 
        public async Task<List<Networkelementsasplanned>> GetListOfAsset(long opCoId, long CurrentDcfId)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);
            var predicateInner = PredicateBuilder.New<Networkelementsasplanned>(true);

            try
            {
                // 1. Filter by OpCo
                predicateInner.Or(x => x.Opcoid == opCoId);
                predicateResult.And(predicateInner);

                // 2. Filter by Deployment Status
                var deploymentStatusResource = _repositoryWrapper.DeploymentStatus
                    .FindByCondition(x => ConstantValueFilter.assetDeployementStatusForPlatformMigration
                        .Contains(x.Deploymentstatus.ToLower()))
                    .Select(x => x.Deploymentstatusid)
                    .ToList();

                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in deploymentStatusResource)
                    predicateInner.Or(x => x.Deploymentstatusid == item);

                predicateResult.And(predicateInner);

                // 3. Filter by Design Component Family Id
                var dcIdEntity = _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentfamilyid == CurrentDcfId)
                    .Select(x => x.Designcomponentid)
                    .ToList();

                if (dcIdEntity != null && dcIdEntity.Count > 0)
                {
                    predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                    foreach (var item in dcIdEntity)
                        predicateInner.Or(x => x.Designcomponentid == item);

                    predicateResult.And(predicateInner);
                }
                // 6. Fetch assets
                var assetEntity = await Task.Run(() =>
                    _repositoryWrapper.NetworkElementAsPlanned
                        .FindByCondition(predicateResult).Include(x => x.Opco)
                        .Include(x => x.Environment)
                        .Include(x => x.Deploymentstatus)
                        .Include(x => x.Deploymenttype)
                        .Include(x => x.Location)
                        .ToList());

                return assetEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Issue occurred while getting list of assets based on Planned DCF for Platform Migration - GetListOfAsset(): {ex.Message}"
                );
                return null;
            }
        }

        public async Task<QueryResultDto<DaAssetMigrationDtoGrid>> GetAssetsForPlatformMigrationGrid(DaAssetMigrationQueryDto filterDto)
        {

            List<long?> existingAssetId = new List<long?>();
            List<DaAssetMigrationDtoGrid> assetPlatformEntity = new List<DaAssetMigrationDtoGrid>();
            try
            {
                // 1. Get existing DA asset migration records for the given Planned Activity
                if (filterDto == null)
                    filterDto = new DaAssetMigrationQueryDto();

                filterDto.IsPagination = false;

                #region ** Need this condition when create 2nd New Pa create automatically get other pa record based on Opco 
                if (filterDto.PlannedActivityId != null && filterDto.PlannedActivityId.Count != 0)
                {

                    var existingDaAssetRecords = await FindWithConditionAsync(filterDto);

                    if (existingDaAssetRecords?.Items != null && existingDaAssetRecords.Items.Count > 0)
                    {
                        assetPlatformEntity = existingDaAssetRecords.Items.ToList();
                        // 2. Collect existing asset IDs 
                        existingAssetId = assetPlatformEntity?
                          .Select(x => x.NetworkElementAsPlannedId)
                          ?.ToList();
                    }

                }
                #endregion
                #region for get list of Asset for Grid and Dropdown               
                // 3. Fetch available asset list for platform migration
                short opCoId = 0;
                long currentDcfId = 0;
                if (filterDto.OpcoDesc != null && filterDto.OpcoDesc.Count > 0)
                    opCoId = (short)(filterDto?.OpcoDesc?.FirstOrDefault());
                else if (filterDto.OpcoId != null && filterDto.OpcoId.Count > 0)
                    opCoId = (short)(filterDto?.OpcoId?.FirstOrDefault());

                if (filterDto.CurrentDcfId != null && filterDto.CurrentDcfId.Count > 0)
                    currentDcfId = (long)(filterDto?.CurrentDcfId?.FirstOrDefault());

                #region dn't filter while after insert
                if ((filterDto.DaAssetMigrationId != null && filterDto.DaAssetMigrationId.Count == 0) && (filterDto.NewelEmentName != null && filterDto.NewelEmentName.Count == 0) &&
                    (filterDto.TargetDesignComponenetId != null && filterDto.TargetDesignComponenetId.Count == 0) &&
                    (filterDto.TrafficNodePercentage != null && filterDto.TrafficNodePercentage.Count == 0) &&
                    filterDto.MigrationCompletionDate == null && filterDto.RfoDate == null && filterDto.RfsDate == null
                    && filterDto.BomSubmittedDate == null && filterDto.HwPoRaisedDate == null && filterDto.HwPoArrivedDate == null 
                    && filterDto.RfaDate == null && filterDto.VecDate == null && filterDto.StartOfAppIntegration == null && filterDto.MigrationStart == null)
                {
                    var assetDtoEntities = await GetListOfAsset(opCoId, currentDcfId);

                    #region  filters when Asset not insert into DB (New PA)
                    if ((filterDto.OldAssetName != null && filterDto.OldAssetName.Count != 0) && (assetDtoEntities != null && assetDtoEntities.Count > 0))
                    {
                        assetDtoEntities = assetDtoEntities?.Where(x => filterDto.OldAssetName.Contains(x.Networkelementasplannedid.ToString())).ToList();
                    }

                    if ((filterDto.NewEnvironment != null && filterDto.NewEnvironment.Count != 0) && (assetDtoEntities != null && assetDtoEntities.Count > 0))
                    {
                        assetDtoEntities = assetDtoEntities?.Where(x => filterDto.NewEnvironment.Contains(x.Environment.Environment.ToString())).ToList();
                    }

                    if ((filterDto.Location != null && filterDto.Location.Count != 0) && (assetDtoEntities != null && assetDtoEntities.Count > 0))
                    {
                        assetDtoEntities = assetDtoEntities?.Where(x => filterDto.Location.Contains(x.Locationid.ToString())).ToList();
                    }
                    #endregion

                    if (existingAssetId != null && existingAssetId.Count > 0)
                    {
                        assetDtoEntities = assetDtoEntities.Where(x => !existingAssetId.Contains(x.Networkelementasplannedid)).ToList();
                    }

                    //4 . Asset Planned Deployment status id 

                    if (assetDtoEntities != null && assetDtoEntities.Count > 0)
                    {
                        foreach (var assetDtoEntity in assetDtoEntities)
                        {
                            assetPlatformEntity.Add(new DaAssetMigrationDtoGrid
                            {
                                NetworkElementAsPlannedId = assetDtoEntity.Networkelementasplannedid,
                                OldAssetName = assetDtoEntity.Elementname,


                                OpcoId = assetDtoEntity.Opcoid,
                                OpcoDesc = assetDtoEntity?.Opco?.Opco,


                                NewEnvironmentId = assetDtoEntity.Environmentid,
                                NewEnvironment = assetDtoEntity?.Environment.Environment,


                                LocationId = assetDtoEntity.Locationid,
                                Location = assetDtoEntity?.Location?.Location,

                                OldDeploymentStatus = assetDtoEntity?.Deploymentstatus?.Deploymentstatus,
                                OldEnvironment = assetDtoEntity?.Environment.Environment,
                                OldDeploymentType = assetDtoEntity?.Deploymenttype?.Deploymenttype,


                                CurrentDesignComponenet = DesignComponentTypeExtensionMethod.ToDesignComponentNameBasedOnDcId
                                ((long)(assetDtoEntity.Designcomponentid), _repositoryWrapper),
                                CurrentDcId = assetDtoEntity.Designcomponentid,


                            });
                        }
                    }
                }
                #endregion
                var rtn = new QueryResultDto<DaAssetMigrationDtoGrid>(new GenerateRenderForGrid<DaAssetMigrationDtoGrid>(_customColumnManager))
                {

                };
                rtn.TotalItems = assetPlatformEntity.Count();

                assetPlatformEntity = assetPlatformEntity.ApplyPaginationList(filterDto);

                int count = 1;
                foreach (var item in assetPlatformEntity)
                {
                    item.UniqueIdForUi = count;
                    count++;

                }
                rtn.Items = assetPlatformEntity;
                #endregion


                // 8. Return result
                return rtn;

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Issue occurred in platform migration - GetListOfAssetForPlatformMigrationGrid(): {ex.Message}"
                );
                return null;

            }
        }

        public async Task<DaAssetMigrationAddUpdateDto> GetAssetPlatformDropdown(long opCoId, long CurrentDcfId, long plannedDcfId)
        {
            var dtoDaAssetMigrateRecords = new DaAssetMigrationAddUpdateDto();

            try
            {

                dtoDaAssetMigrateRecords.daAssetMigrationDtoGrid = new List<DaAssetMigrationDtoGrid>
                {

                                      new DaAssetMigrationDtoGrid {  }

                };

                #region for get list of Asset for Grid and Dropdown
                // 1. Fetch available asset list for platform migration
                var assetDtoEntities = await GetListOfAsset(opCoId, CurrentDcfId);

                //2. Get existing Asset 
                var assetDetails = assetDtoEntities.
                    Select(x => new ExistingAssetDto
                    {
                        key = x.Networkelementasplannedid,
                        value = x.Elementname,
                        EnvironmentId = x.Environmentid,
                        DeploymentStatusId = x.Deploymentstatusid,
                        LocationId = (long)x.Locationid
                    }).ToList();

                dtoDaAssetMigrateRecords.ExistingAssetResource = assetDetails;
                #endregion

                //3.reference data: Environment, Deployment Status, and Location
                var environmentResource = _repositoryWrapper.Environment
                    .FindAll()
                    .ToDictionary(x => (int)x.Environmentid, x => x.Environment);

                var deploymentStatusResource = _repositoryWrapper.DeploymentStatus
                    .FindByCondition(x => ConstantValueFilter.assetDeployementStatusForPlatformMigration
                        .Contains(x.Deploymentstatus.ToLower())) //|| x.Deploymentstatus.ToLower() == ConstantValueFilter.Removed
                    .ToList();


                var locationResource = await _dropdownDataServiceManager.GetLocationDropDown((short?)opCoId);

                //4. DTO resources
                dtoDaAssetMigrateRecords.EnvironmentReosurce = environmentResource;

                dtoDaAssetMigrateRecords.DeploymentStatusReosurce = deploymentStatusResource
                    .ToDictionary(
                        x => x.Deploymentstatusid,
                        dto => new DeploymentStatusDto
                        {
                            DeploymentStatusId = dto.Deploymentstatusid,
                            DeploymentStatusDescription = dto.Deploymentstatus,
                            Rule = (int)dto.Rule,
                            PlannedActivityResourceAllowedId = DeploymentStatusMapper
                                .GetDeploymentStatusPAResourceAllowed(dto)
                                .toPlannedActivityResourceKeyList(),
                            ReadOnlyPlannedActivity = dto.Readonlyplannedactivity,
                            CheckPlannedActivity = dto.Checkplannedactivity
                        });

                dtoDaAssetMigrateRecords.LocationReosurce = locationResource;

                //5. Design Component Resource


                List<Designcomponents> designComponentResource = DesignComponentTypeExtensionMethod.verticalBasedDesignComponentRecord(null, _repositoryWrapper, "all")
                         .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                         .Include(x => x.Designcomponentfamily).
                          ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction)
                         .ThenInclude(x => x.Systemfunction).ToList();

                //var iDictionaryTransientDesignComponentResource =
                //             designComponentResource.Where(x => x.Designcomponentfamilyid == plannedDcfId && x.Visibleflag == false).toDesignComponentResource(_repositoryWrapper)
                //             .Where(
                //   x => !x.Value.ToLower().Contains("unknown")).Select(x => new KeyValuePair<long, string>(x.Key, x.Value)).ToList();

                var iDictionaryDesignComponentResource =
                            designComponentResource.Where(x => x.Designcomponentfamilyid == plannedDcfId).toDesignComponentResource(_repositoryWrapper)
                            .Where(
                  x => !x.Value.ToLower().Contains("unknown")).Select(x => new KeyValuePair<long, string>(x.Key, x.Value)).ToList();

                dtoDaAssetMigrateRecords.TargetDesignComponentResource = iDictionaryDesignComponentResource;
                //dtoDaAssetMigrateRecords.TargetDesignComponentResource.AddRange(iDictionaryTransientDesignComponentResource);


                //6. Return result
                return dtoDaAssetMigrateRecords;


            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Issue occurred in platform migration - GetAssetPlatformDropdown(): {ex.Message}"
                );
                return dtoDaAssetMigrateRecords;

            }
        }

        #endregion
        #region Add / Update
        public async Task<ResultDto> AddOrUpdateAssetMigrationAsync(DaAssetMigrationAddUpdateDto dtoDaAssetMigrateRecords, long paId)
        {
            try
            {
                if (dtoDaAssetMigrateRecords == null) return new ResultDto
                {
                    Info = ResultMessages.NoDaAssetMigration
                };

                // 1. Extract migration DTOs and determine Planned Activity ID
                var addUpdateDto = dtoDaAssetMigrateRecords.daAssetMigrationDtoGrid.ToList();

                if ((addUpdateDto != null && addUpdateDto.Count > 0) && addUpdateDto.Any(x => x.OpcoId != 0) == true)
                {

                    #region merge row - revert
                    //var removeDuplicateRecords = addUpdateDto
                    //       .Where(x => x.NetworkElementAsPlannedId != null || x.NetworkElementAsPlannedId == null)
                    //       .GroupBy(x => new
                    //       {
                    //           x.PlannedActivityId,
                    //           x.NetworkElementAsPlannedId
                    //       })
                    //       .SelectMany(group =>
                    //       {
                    //           bool isRowHasData(DaAssetMigrationDtoGrid x)
                    //           {
                    //               return
                    //                   !string.IsNullOrWhiteSpace(x.NewelEmentName) ||
                    //                   x.RfoDate != null ||
                    //                   x.RfsDate != null ||
                    //                   x.TargetDesignComponenetId != null ||
                    //                   //x.NewEnvironmentId != null ||   // default value assigned form existing asset 
                    //                   x.NewDeploymentStatusId != null ||
                    //                   x.MigrationCompletionDate != null ||
                    //                   !string.IsNullOrWhiteSpace(x.TrafficNodePercentage) ||
                    //                   x.HwPoRaisedDate != null ||
                    //                   x.HwPoArrivedDate != null ||
                    //                   x.BomSubmittedDate != null ||
                    //                   x.RfaDate != null ||
                    //                   x.NewelEmentName != null ||
                    //                   x.IsDecommissioned != null;
                    //               //|| x.LocationId != null // default value assigned form existing asset  

                    //           }

                    //           // Keep only rows having data
                    //           var validRows = group
                    //               .Where(isRowHasData)
                    //               .ToList();

                    //           //   row have data for any one column
                    //           if (validRows.Any())
                    //           {
                    //               return validRows;
                    //           }

                    //           // get only first empty row
                    //           return group.Take(1);
                    //       })
                    //       .ToList();

                    //addUpdateDto = removeDuplicateRecords;
                    #endregion

                    // 2. Get existing migration records for this Planned Activity
                    var migratedLocationEntities = _repositoryWrapper.DaAssetMigrationRepository
                        .FindByCondition(x => x.Plannedactivityid == paId)
                        .ToList();

                    #region Update Asset while edit NewElement
                    var newAssetRecords = migratedLocationEntities.Where(x => x.Plannedactivityid == paId && x.Targetdesigncomponenetid != null && x.Newelementname != null && x.Daassetmigrationid != 0).ToList();
                    foreach (var item in newAssetRecords)
                    {
                        var updateItem = addUpdateDto.Where(x => x.DaAssetMigrationId == item.Daassetmigrationid).FirstOrDefault();
                        if (updateItem != null && (item.Newelementname?.ToLower() != updateItem?.NewelEmentName?.ToLower()))
                        {
                            var existsAsset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == item.Opcoid
                                  && x.Designcomponentid == item.Targetdesigncomponenetid && x.Elementname.ToLower()
                                  == Convert.ToString(item.Newelementname).ToLower()).FirstOrDefault();
                            if (existsAsset != null)
                            {
                                existsAsset.Elementname = updateItem.NewelEmentName;
                                _repositoryWrapper.NetworkElementAsPlanned.Update(existsAsset);
                                _logger.LogDebug($"Asset Updated with new ElementName {existsAsset.Elementname}");

                            }
                        }

                    }
                    await _repositoryWrapper.SaveAsync();

                    #endregion
                    var daEntity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == paId)
                        .Include(x => x.Designaspect)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                        .ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Productname)
                        .FirstOrDefault();
                    #region Add or Update DaAssetMigration Records

                    foreach (var item in addUpdateDto)
                    {
                        var daAssetMigration = new Daassetmigration();

                        // Create new record
                        if (item.DaAssetMigrationId == 0)
                        {
                            _repositoryWrapper.DaAssetMigrationRepository.Create(daAssetMigration);
                            _logger.LogDebug($"DaAssetMigration Entry Created for New Asset {daAssetMigration.Newelementname}");

                        }
                        // Update existing record
                        else
                        {
                            var existingEntity = migratedLocationEntities.FirstOrDefault(x =>
                                 // x.Opcoid == item.OpcoId &&
                                 //x.Networkelementasplannedid == item.NetworkElementAsPlannedId  &&
                                 x.Daassetmigrationid == item.DaAssetMigrationId);

                            if (existingEntity != null)
                                daAssetMigration = existingEntity;
                            else
                            {
                                _repositoryWrapper.DaAssetMigrationRepository.Create(daAssetMigration);
                                _logger.LogDebug($"DaAssetMigration Entry Created for New Asset {daAssetMigration.Newelementname}");

                            }
                        }

                        // Assign properties
                        daAssetMigration.Opcoid = item.OpcoId;
                        if (item.LocationId != 0) daAssetMigration.Locationid = item.LocationId;
                        daAssetMigration.Plannedactivityid = paId;
                        daAssetMigration.Networkelementasplannedid = item.NetworkElementAsPlannedId == 0 ? null : item.NetworkElementAsPlannedId;

                        daAssetMigration.Environmentid = (item.NewEnvironmentId != 0 && item.NewEnvironmentId != null) ? item.NewEnvironmentId : null;
                        daAssetMigration.Deploymentstatusid = (item.NewDeploymentStatusId != 0 && item.NewDeploymentStatusId != null)
                        ? item.NewDeploymentStatusId : null;
                        daAssetMigration.Targetdesigncomponenetid = (item.TargetDesignComponenetId != 0 && item.TargetDesignComponenetId != null) ? item.TargetDesignComponenetId : null;

                        daAssetMigration.Rfodate = item.RfoDate;
                        daAssetMigration.Rfsdate = item.RfsDate;
                        daAssetMigration.Migrationcompletiondate = item.MigrationCompletionDate;
                        daAssetMigration.Trafficnodepercentage = item.TrafficNodePercentage;
                        daAssetMigration.Newelementname = item.NewelEmentName;

                        daAssetMigration.Hwporaiseddate = item.HwPoRaisedDate;
                        daAssetMigration.Hwpoarriveddate = item.HwPoArrivedDate;
                        daAssetMigration.Bomsubmitteddate = item.BomSubmittedDate;
                        daAssetMigration.Rfadate = item.RfaDate;
                        daAssetMigration.Platformid = daEntity?.Designcomponentfamily?.Designcomponents?.SelectMany(x => x.Systemtype?.Systemtypesmajorhardwarebuilds?.Where(x => x.Ismain == true).Select(x => x.Majorhardware?.Platformid)).FirstOrDefault();
                        daAssetMigration.Productnameid = daEntity?.Designcomponentfamily?.Productnameid;
                        daAssetMigration.Isdecommissioned = item.IsDecommissioned;
                        daAssetMigration.Vecdate = item.VecDate;
                        daAssetMigration.Startofappintegration = item.StartOfAppIntegration;
                        daAssetMigration.Migrationstart = item.MigrationStart;

                        if (daAssetMigration.Daassetmigrationid != 0)
                        {
                            _repositoryWrapper.DaAssetMigrationRepository.Update(daAssetMigration);
                            _logger.LogDebug($"DaAssetMigration Entry updated Existing Asset {item.NewelEmentName}");
                        }

                        await _repositoryWrapper.SaveAsync();
                    }

                    await _repositoryWrapper.ClearTracker();

                    #endregion

                    #region Delete Obsolete DaAssetMigration Records             

                    #region delete Asset while delete the DaMigration

                    var deleteDaMigrationStatus = migratedLocationEntities
                        .Where(x => !addUpdateDto.Where(x => x.DaAssetMigrationId != 0)
                            .Select(dto => dto.DaAssetMigrationId)
                            .Contains(x.Daassetmigrationid))
                        .ToList();

                    foreach (var deleteItem in deleteDaMigrationStatus.Where(x => x.Targetdesigncomponenetid != null))
                    {
                        var existsAsset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == deleteItem.Opcoid
                                  && x.Designcomponentid == deleteItem.Targetdesigncomponenetid && x.Elementname.ToLower()
                                  == Convert.ToString(deleteItem.Newelementname).ToLower()).FirstOrDefault();
                        if (existsAsset != null)
                        {
                            await _lcmEngineeringManager.Value.AssetRecordDelete(existsAsset.Networkelementasplannedid);
                            _logger.LogDebug($"Asset Entry deleation for Existing Asset {existsAsset.Elementname}");

                        }

                    }

                    #endregion

                    foreach (var deleteItem in deleteDaMigrationStatus)
                    {
                        _repositoryWrapper.DaAssetMigrationRepository.DeleteDeep(deleteItem);
                        _logger.LogDebug($"DaMigration Entry deleation for Existing DaMigration Entry {deleteItem.Newelementname}");
                    }

                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();


                    #endregion
                    
                    var currendDcfId = (long)daEntity?.Designaspect?.Designcomponentfamilyid;
                    var plannedDcfId = daEntity?.Designcomponentfamilyid;

                    await _lcmEngineeringManager.Value.CreateLcmAndLcmPaForPlatformationMigration(paId, (long)plannedDcfId, currendDcfId, migratedLocationEntities);
                    _logger.LogDebug($"LCM Has been Created for the following OpcoId and currendDcfId{migratedLocationEntities.Select(x => x.Opcoid).FirstOrDefault()} - {currendDcfId} - plannedDcfId {plannedDcfId}");
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddSuccess,
                    Data = 1// This value is used for Archive method in DAPAManager
                };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue occurred while adding/updating DaAssetMigration entity: {ex.Message}");
                _logger.LogError($"Issue occurred while adding/updating DaAssetMigration entity: {ex.StackTrace}");

                return new ResultDto
                {
                    Info = ResultMessages.EntryAddUpdateFailed,
                    Data = 0// This value is used for Archive method in DAPAManager
                };
            }
        }
        #endregion

        #region // Refactor DC
        public async Task<ResultDto> RefactorDcCalculation(DaAssetMigrationAddUpdateDto dtoDaAssetMigrateRecords, long paId)
        {
            try
            {
                var daAssetMigrationEntity = await _repositoryWrapper.DaAssetMigrationRepository.FindByCondition(f => f.Plannedactivityid == paId
                && f.Targetdesigncomponenetid != null).ToListAsync();

                if (daAssetMigrationEntity != null && daAssetMigrationEntity.Count <= 0)
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.DaTargetDCNotExists
                    };
                }
                else
                {
                    var opcoid = daAssetMigrationEntity.Select(r => r.Opcoid).FirstOrDefault();
                    var existingTargetDcIds = daAssetMigrationEntity.Select(r => r.Targetdesigncomponenetid).ToList();
                    var hasOtherAssets = false;

                    // check the Lcm exist for the opco and DC compination, if yes archive the lcm

                    var existingLcmEntities = await _repositoryWrapper.Lcmengineering.FindByCondition(f => f.Opcoid == opcoid
                    && existingTargetDcIds.Contains(f.Designcomponentid) && f.Archived == false)
                    .Include(j => j.Networkelementsasplanned)
                    .Include(j => j.PlannedactivitiesLcmengineering).ToListAsync();

                    if (existingLcmEntities != null && existingLcmEntities.Count > 0)
                    {
                        var allAssets = existingLcmEntities.SelectMany(r => r.Networkelementsasplanned).ToList();
                        var allPlannedActivities = existingLcmEntities.Where(x => x.Archived == false).SelectMany(r => r.PlannedactivitiesLcmengineering).ToList();
                        var migrationNewAssetNames = daAssetMigrationEntity.Select(r => r.Newelementname.ToLower().Trim().Replace(" ", "")).ToList();

                        /* check the lcm asset has other than New Asset, if yes no need to do archive the Lcm but update the new Dc id
                         * to the new Asset and update the LCM id null to that asset 
                        */

                        // Assets not part of migration
                        var otherAssetsExistsInLCM = allAssets.Where(f => !migrationNewAssetNames.Contains(f.Elementname.ToLower().Trim().Replace(" ", ""))).ToList();

                        if (otherAssetsExistsInLCM != null && otherAssetsExistsInLCM.Count > 0) hasOtherAssets = true;

                        // Assets to update
                        var migratedAssetsToUpdate = allAssets.Where(f => migrationNewAssetNames.Contains(f.Elementname.ToLower().Trim().Replace(" ", ""))).ToList();

                        // New target DC mapping
                        //var newTargetDcIds = dtoDaAssetMigrateRecords
                        //    .daAssetMigrationDtoGrid
                        //    .Where(x =>
                        //        !string.IsNullOrWhiteSpace(x.NewelEmentName) &&
                        //        x.TargetDesignComponenetId != null)
                        //    .DistinctBy(x =>  x.NewelEmentName )
                        //    .ToDictionary(
                        //        k =>  k.NewelEmentName ,
                        //        v => v.TargetDesignComponenetId);

                        //migratedAssetsToUpdate.ForEach(r => { 
                        //    r.Lcmengineeringid = null;
                        //    r.Lcmengineering = null;
                        //    r.Designcomponentid = newTargetDcIds.GetValueOrDefault(r.Elementname).Value; 
                        //});

                        if (migratedAssetsToUpdate != null && migratedAssetsToUpdate.Count > 0)
                        {
                            //foreach(var asset in migratedAssetsToUpdate)
                            //{
                            //    await _lcmEngineeringManager.Value.DeleteDeepAssetRecordForPaRefactor(asset.Networkelementasplannedid);
                            //}
                            var assetRemovedStatusID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.Removed).FirstOrDefault().Deploymentstatusid;
                            migratedAssetsToUpdate.ForEach(r =>
                            {
                                r.Deleted = true;
                                r.Deploymentstatusid = assetRemovedStatusID;
                                r.Lcmengineering = null;

                            });
                            _repositoryWrapper.NetworkElementAsPlanned.BulkUpdate(migratedAssetsToUpdate);
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();
                            // need to put an asset entry in DCF lifecycle
                            _dcfLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(migratedAssetsToUpdate, ConstantValueFilter.refactorDcAsset, 0, null);
                        }

                        if (hasOtherAssets == false)
                        {
                            // Removed deployment status
                            var removedDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository
                                .FindByCondition(x => x.Description.ToLower().Replace(" ", "") == ConstantValueFilter.Removed).Select(x => x.Id).FirstOrDefault();
                            existingLcmEntities.ForEach(r =>
                            {
                                r.Archived = true;
                                r.Lcmdeploymentstatusid = removedDeploymentStatusId;
                                r.Numberofnodesinlab = 0;
                                r.Numberofnodes = 0;
                            });

                            foreach (var archivedLcm in existingLcmEntities)
                                await _lcmEngineeringManager.Value.InsertUpdateAncillaryDatas(archivedLcm.Lcmengineeringid, 0, false, null, false, archivedLcm.Lcmdeploymentstatusid.Value, true);

                            // Archive the Planned Activity if the LCM was created through DA Asset Migration.
                            if ((allPlannedActivities != null && allPlannedActivities.Count > 0) && hasOtherAssets == false)
                            {
                                Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();

                                var plannedactivityresourceid = allPlannedActivities.FirstOrDefault()?.Plannedactivityresourceid;

                                var deliveryStatusId = await _repositoryWrapper.SettingsUpdatePlannedActivity
                                        .FindByCondition(x => x.Plannedactivityresourceid == plannedactivityresourceid)
                                        .OrderByDescending(x => x.Order)
                                        .Select(x => x.Deliverystatusid)
                                        .FirstOrDefaultAsync();

                                allPlannedActivities.ForEach(r =>
                                {
                                    r.Archived = true;
                                    r.Deliverystatusid = deliveryStatusId;
                                    r.Activitystatusid = completedActivityStatus.Activitystatusid;
                                });

                                _repositoryWrapper.PlannedActivity.BulkUpdate(allPlannedActivities);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();

                                foreach (var plannedActivityEntity in allPlannedActivities)
                                {
                                    await _commonManager.setArchiveStatusForBpt(plannedActivityEntity);
                                }

                            }
                        }

                        else
                        {
                            existingLcmEntities.ForEach(r =>
                            {
                                r.Numberofnodes = LCMEngineeringMapper.GetLcmEngineeringMapper(r).CountNetworkElementReleated(false, _repositoryWrapper);
                                r.Numberofnodesinlab = LCMEngineeringMapper.GetLcmEngineeringMapper(r).CountNetworkElementReleated(true, _repositoryWrapper);
                            });

                        }
                        _repositoryWrapper.Lcmengineering.BulkUpdate(existingLcmEntities);
                        await _repositoryWrapper.SaveAsync();

                        await _repositoryWrapper.ClearTracker();
 

                       



                    }

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryUpdateSuccess,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion


        #region get PlannedDCF for Exodus - Platform Migration

        public async Task<List<KeyValuePair<long, string>>> GetPlannedDcfResourcesForPlatformMigration(PlatformPlannedDcfDto _platformPlannedDcdDto)
        {
            List<KeyValuePair<long, string>> plannedDcfResource = new List<KeyValuePair<long, string>>();
            try
            {
                if (_platformPlannedDcdDto == null) return null;
                string compareValue = _platformPlannedDcdDto?.compareValue ?? string.Empty;
                if (string.IsNullOrEmpty(compareValue)) return null;

                compareValue = compareValue.Replace("<b class=\"text-lowercase\">", "")
                                      .Replace("<b class=\"text-lowercase\" >", "")
                                      .Replace("</b>", "");
                string GetOnBefore(string s) => s.Split(" on ")[0].Replace(" ", "");
                string GetForAfter(string s)
                {
                    var parts = s.Split(" on ");
                    return parts.Length > 1 ? parts[1].Substring(parts[1].IndexOf(" for "))?.Replace(" ", "") : "";
                }

                // find match
                var beforeInput = GetOnBefore(compareValue);
                var afterInput = GetForAfter(compareValue);
                var match = await Task.Run(() =>
                               //_platformPlannedDcdDto.dcfResource.ToList().Where(x => x.Value.StartsWith(" NICE DUNE")).ToList().Where(x =>
                               _platformPlannedDcdDto.dcfResource.ToList().Where(x =>
                               {
                                   var cleanValue = x.Value
                                       .Replace("<b class=\"text-lowercase\">", "")
                                       .Replace("<b class=\"text-lowercase\" >", "")
                                       .Replace("</b>", "");
                                   var prefixCleanValue = GetOnBefore(cleanValue);
                                   var suffixCleanValue = GetForAfter(cleanValue);
                                   return prefixCleanValue.Equals(beforeInput, StringComparison.OrdinalIgnoreCase) &&
                                         suffixCleanValue.Equals(afterInput, StringComparison.OrdinalIgnoreCase) && (_platformPlannedDcdDto.compareValue != x.Value.TrimStart());
                               }).Select(r => new KeyValuePair<long, string>(r.Key, r.Value)).ToList());
                if (match.Any())
                {
                    plannedDcfResource.AddRange(match.OrderBy(x => x.Value));
                }

                var platformMigrationDcfIds = plannedDcfResource
                    .Select(x => x.Key)
                    .ToList();

                if (platformMigrationDcfIds.Any())
                {
                    var otherDcf = _platformPlannedDcdDto?.dcfResource
                        .Where(x => !platformMigrationDcfIds.Contains(x.Key));

                    if (otherDcf?.Any() == true)
                    {
                        plannedDcfResource.AddRange(otherDcf.OrderBy(x => x.Value));
                    }
                }
                else
                {
                    plannedDcfResource.AddRange(
                        _platformPlannedDcdDto.dcfResource.OrderBy(x => x.Value)
                    );
                }

                return plannedDcfResource;
            }
            catch
            {
                return plannedDcfResource;
            }
        }

        #endregion
        #region DeletePltformMigrationPaNewAsset   -- this method used in LCMEnigneering , PA , PlatformMigration Manager  - depencies injection issue  - 
        public async Task<ResultDto> DeletePltformMigrationPaNewAsset(long paId)
        {
            try
            {


                // 2. Get existing migration records for this Planned Activity
                var deleteNewMigratedAssets = _repositoryWrapper.DaAssetMigrationRepository
                    .FindByCondition(x => x.Plannedactivityid == paId && x.Targetdesigncomponenetid != null && x.Newelementname != null)
                    .ToList();

                #region Update Asset while edit NewElement

                foreach (var deleteItem in deleteNewMigratedAssets)
                {
                    var existsAsset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == deleteItem.Opcoid
                              && x.Designcomponentid == deleteItem.Targetdesigncomponenetid && x.Elementname.ToLower()
                              == Convert.ToString(deleteItem.Newelementname).ToLower()).FirstOrDefault();
                    if (existsAsset != null)
                    {
                        await _lcmEngineeringManager.Value.AssetRecordDelete(existsAsset.Networkelementasplannedid);

                    }

                }


                #endregion

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = 1// This value is used for Archive method in DAPAManager
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue occurred while delete NewAsset in Asset Table for DaAssetMigration entity: {ex.Message}");

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotDeleted,
                    Data = 0// This value is used for Archive method in DAPAManager
                };
            }
        }
        #endregion


    }
}
