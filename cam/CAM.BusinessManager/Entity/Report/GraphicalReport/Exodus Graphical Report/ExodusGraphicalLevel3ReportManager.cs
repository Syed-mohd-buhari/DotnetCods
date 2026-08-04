using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.Grid;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.Record.PivotTable;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.GraphicalReport.Exodus_Graphical_Report
{
    public class ExodusGraphicalLevel3ReportManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _manager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly CommonManager _commonManager;
        private readonly ILoggerManager _logger;
        private Dictionary<int, string> filteredPlannedActivity = new Dictionary<int, string>();
        private bool isTargetDc = false;
        private readonly DapperCommonManager _dapperCommonManager;
        public ExodusGraphicalLevel3ReportManager(DapperCommonManager dapperCommonManager, IEnumerable<IRepositoryWrapper> wrappers,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager, GridCustomColumnManager manager,
            DropdownDataServiceManager dropdownDataServiceManager, ILoggerManager logger) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _commonManager = commonManager;
            _logger = logger;
            _dapperCommonManager = dapperCommonManager;

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
        public ExpressionStarter<Daassetmigration> ApplyFilter(ExodusQueryLevel3ReportQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Daassetmigration>(true);

            var paArchivedPredicate = PredicateBuilder.New<Daassetmigration>();
            paArchivedPredicate.Or(x => x.Plannedactivity.Archived == false);
            mainPredicate.And(paArchivedPredicate);

            if (filterDto.OpcoId?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.OpcoId)
                    descriptionPredicate.Or(x => x.Plannedactivity.Opcoid == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.TargetDesignComponenet?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.TargetDesignComponenet)
                    descriptionPredicate.Or(x => x.Targetdesigncomponenetid.ToString() == item.ToString());

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Location != null && filterDto.Location?.Any() == true)
            {
                var LocationPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.Location)
                    LocationPredicate.Or(x => x.Location.Locationid == Convert.ToInt16(item));
                mainPredicate.And(LocationPredicate);
            }
            if (filterDto.EnvironmentId != null && filterDto.EnvironmentId?.Any() == true)
            {
                var LocationPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.EnvironmentId)
                    LocationPredicate.Or(x => x.Networkelementasplanned.Environmentid == item);
                mainPredicate.And(LocationPredicate);
            }
            if (filterDto.PlannedDcfId != null && filterDto.PlannedDcfId?.Any() == true)
            {
                var LocationPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.PlannedDcfId)
                    LocationPredicate.Or(x => x.Plannedactivity.Designcomponentfamilyid == item);
                mainPredicate.And(LocationPredicate);
            }
            if (filterDto.ProductId != null && filterDto.ProductId?.Any() == true)
            {
                var LocationPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.ProductId)
                    LocationPredicate.Or(x => x.Productnameid == item);
                mainPredicate.And(LocationPredicate);
            }
            if (filterDto.PlatformId != null && filterDto.PlatformId?.Any() == true)
            {
                var LocationPredicate = PredicateBuilder.New<Daassetmigration>();
                foreach (var item in filterDto.PlatformId)
                    LocationPredicate.Or(x => x.Platformid == item);
                mainPredicate.And(LocationPredicate);
            }

            return mainPredicate;
        }


        public async Task<ResultDto> GetOpcoAndDcfDropdown(ExodusQueryLevel3ReportQueryDto filterDto)
        {
            var predicateResult = ApplyFilter(filterDto);
            var daAssetMigrations = GetDaAssetMigrationEntitiesNew(predicateResult).ToList();

            if (daAssetMigrations.Count <= 0 || daAssetMigrations == null)
                return new ResultDto();

            //var plannedDcfDropdown = await Task.Run(() => daAssetMigrations.DistinctBy(x => x.Plannedactivity.Designcomponentfamilyid).Select(x => new KeyValuePairDto { Key = (long)x.Plannedactivity.Designcomponentfamilyid, Text = x.Plannedactivity.Designcomponentfamily.toDesignComponentFamilyName(_repositoryWrapper) }));

            var productDopdown = await Task.Run(() => daAssetMigrations.Where(x => x.Productname != null).DistinctBy(x => x.Productnameid)
                .Select(x => new KeyValuePairDto { Key = Convert.ToInt64(x.Productnameid), Text = x?.Productname?.Description }));

            var platformDropdown = daAssetMigrations.Where(x => x.Platform != null).DistinctBy(x => x.Platformid)
                .Select(x => new KeyValuePairDto { Key = Convert.ToInt64(x.Platformid), Text = x?.Platform?.Platform });

            var OpcoDropdown = daAssetMigrations.DistinctBy(x => x.Opcoid).Select(x => new KeyValuePairDto { Key = (long)x.Opcoid, Text = x.Opco.Opco });

            var EnvironmentDropdown = daAssetMigrations.Where(x => x.Networkelementasplanned != null && x.Networkelementasplanned.Environment != null)
                                    .DistinctBy(x => x.Networkelementasplanned.Environmentid)
                                    .Select(x => new KeyValuePairDto { Key = (long)x.Networkelementasplanned?.Environment?.Environmentid, Text = x.Networkelementasplanned?.Environment?.Environment });


            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    OpcoDropdown,
                    EnvironmentDropdown,
                    platformDropdown,
                    productDopdown
                },
            };
        }

        #region Optimized code 
        private IQueryable<Daassetmigration> GetDaAssetMigrationEntitiesNew(ExpressionStarter<Daassetmigration> predicateResult)
        {
            var excludeOemList = _dapperCommonManager.GetHardwareOemForExodusFilterAsync().Result;
            var hwOem = excludeOemList?.Select(x => x.Orgeqpmanufacturerid).ToList();

            var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result;
            var hwPlatform = excludePlatformList?.Select(X => X.Platformid).ToList();

            var excludedOpcoList = _dapperCommonManager.GetRestrictedOpcoAsync().Result;
            var excludedOpcoid = excludedOpcoList?.Select(x => x.Opcoid).ToList();

            var query = _repositoryWrapper.DaAssetMigrationRepository.FindByCondition(predicateResult)
                .Where(f => !excludedOpcoid.Contains(f.Opcoid)
                            && !string.IsNullOrEmpty(f.Newelementname)
                            && f.Isdecommissioned == false
                            && hwPlatform.Contains((short)f.Plannedactivity.Designaspect.Designcomponentfamily.Platformid)
                            && f.Plannedactivity.Designaspect.Designcomponentfamily.Designcomponents
                             .Any(f => f.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwOem.Contains(y.Majorhardware.Orgeqpmanufacturerid)))
                             )
                .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Environment)
                .Include(x => x.Location)
                .Include(x => x.Opco)
                .Include(x => x.Productname)
                  .Include(x => x.Platform)
                 .AsNoTracking()
                .AsQueryable();

            return query;
        }

        public async Task<ResultDto> FindWithConditionAsyncNew(ExodusQueryLevel3ReportQueryDto filterDto, bool isPageLoad = false)
        {
            try
            {
                var predicateResult = ApplyFilter(filterDto);

                var activities = _repositoryWrapper.ExodusMilestoneAndActivityRepository.FindAll().Where(x => x.Actualcolumnnames != null).ToList();

                var damigrationStatusResult = await Task.Run(() => GetDaAssetMigrationEntitiesNew(predicateResult).AsEnumerable());

                if (damigrationStatusResult == null || damigrationStatusResult.Count() <= 0)
                    return new ResultDto();
                var daAssetMigration = damigrationStatusResult
                                            .Select(model =>
                                            {
                                                var result = new DaAssetMigration
                                                {
                                                    DaAssetMigrationId = model.Daassetmigrationid,
                                                    PlannedActivityId = model.Plannedactivityid,

                                                    NetworkElementAsPlannedId = model.Networkelementasplannedid,
                                                    OldAssetName = model.Networkelementasplanned?.Elementname ?? string.Empty,
                                                    OldDeploymentStatus = model.Networkelementasplanned?.Deploymentstatus?.Deploymentstatus ?? string.Empty,
                                                    OldEnvironment = model.Networkelementasplanned?.Environment?.Environment ?? string.Empty,
                                                    CurrentDesignComponenetId = model.Networkelementasplanned?.Designcomponentid ?? 0,

                                                    NewelEmentName = model.Newelementname,
                                                    TargetDesignComponenetId = model.Targetdesigncomponenetid,

                                                    NewEnvironmentId = model.Environmentid,
                                                    NewDeploymentStatusId = model.Deploymentstatusid,
                                                    OpcoId = model.Opcoid,
                                                    NewLocationId = model.Locationid,

                                                    RfoDate = model.Rfodate,
                                                    RfsDate = model.Rfsdate,
                                                    MigrationCompletionDate = model.Migrationcompletiondate,
                                                    TrafficNodePercentage = model.Trafficnodepercentage,

                                                    NewOpco = model.Opco?.Opco ?? string.Empty,
                                                    NewLocation = model.Location?.Location ?? string.Empty,
                                                    NewDeploymentStatus = model.Deploymentstatus?.Deploymentstatus ?? string.Empty,
                                                    NewEnvironment = model.Environment?.Environment ?? string.Empty,


                                                    HwPoArrivedDate = model.Hwpoarriveddate,
                                                    HwPoRaisedDate = model.Hwporaiseddate,
                                                    BomSubmittedDate = model.Bomsubmitteddate,
                                                    Startofappintegration = model.Startofappintegration,
                                                    Vecdate = model.Vecdate,
                                                    Migrationstart = model.Migrationstart,
                                                    RfaDate = model.Rfadate,
                                                    ProductName = model?.Productname?.Description,
                                                    Platform = model?.Platform?.Platform
                                                };

                                                result.PlannedActivity ??= PlannedActivityMapper.Get(model.Plannedactivity);

                                                result.NetworkElementAsPlanned ??= NetworkElementAsPlannedMapper.GetEnvMapperFromNTWElement(model.Networkelementasplanned);

                                                return result;
                                            })
                                            .ToList();

                var propertyMap = typeof(DaAssetMigration)
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(
                        p => p.Name,
                        p => p,
                        StringComparer.OrdinalIgnoreCase);

                var daAssetMigrationDetails = daAssetMigration
                                .GroupBy(x => new { x.ProductNameId, x.NewLocationId })
                                .Select(loc =>
                                {
                                    var first = loc.First();

                                    return new ExodusAssetLevelReportGrid
                                    {
                                        Location = first.NewLocation,
                                        SelectedOpco = first.OpcoId,

                                        ProductName = first.ProductName,
                                        Platform = first.Platform,

                                        AssetDetails = loc.Select(x => new ExodusAssetLevelGrid
                                        {
                                            DaAssetMigrationId = x.DaAssetMigrationId,
                                            NetworkElementAsPlannedId = x.NetworkElementAsPlannedId,
                                            PlannedActivityId = x.PlannedActivityId,
                                            PlannedDcfId = x.PlannedActivity?.DesignComponentFamilyId,

                                            OpcoId = x.OpcoId,
                                            LocationId = x.NewLocationId,
                                            TargetDesignComponenetId = x.TargetDesignComponenetId,

                                            NewelEmentName = x.NewelEmentName,

                                            OpcoDesc = x.NewOpco,
                                            Location = x.NewLocation,

                                            CurrentDcId = x.CurrentDesignComponenetId,

                                            RfoDate = getOnlyDate(x.RfoDate),
                                            RfsDate = getOnlyDate(x.RfsDate),
                                            MigrationCompletionDate = getOnlyDate(x.MigrationCompletionDate),

                                            BomSubmittedDate = getOnlyDate(x.BomSubmittedDate),
                                            HwPoRaisedDate = getOnlyDate(x.HwPoRaisedDate),
                                            HwPoArrivedDate = getOnlyDate(x.HwPoArrivedDate),
                                            RfaDate = getOnlyDate(x.RfaDate),
                                            VecDate = getOnlyDate(x.Vecdate),
                                            StartOfAppIntegration = getOnlyDate(x.Startofappintegration),
                                            MigrationStart = getOnlyDate(x.Migrationstart),

                                            BomStartDate = x.BomSubmittedDate.HasValue
                                                             ? getOnlyDate(x.BomSubmittedDate.Value.AddMonths(-1))
                                                             : null,

                                            PlannedStartDate = x.PlannedActivity?.StartDate,

                                            ActivityDetails = activities
                                                                .GroupBy(a => new
                                                                {
                                                                    a.Activitiesorder,
                                                                    a.Activities
                                                                })
                                                                .OrderBy(g => g.Key.Activitiesorder)
                                                                .ToList()
                                                                .Select((group, index) =>
                                                                {
                                                                    DateTime? endDate = null;

                                                                    DateTime? startDate = null;

                                                                    var migrationActivity = group.FirstOrDefault(a =>
                                                                        string.Equals(a.Actualcolumnnames?.Trim(), "MigrationStart", StringComparison.OrdinalIgnoreCase));
                                                                    var appIntegrationActivity = group.FirstOrDefault(a =>
                                                                        string.Equals(a.Actualcolumnnames?.Trim(), "Startofappintegration", StringComparison.OrdinalIgnoreCase));

                                                                    if (migrationActivity != null &&
                                                                        propertyMap.TryGetValue(migrationActivity.Actualcolumnnames.Trim(), out var property))
                                                                    {
                                                                        var value = property.GetValue(x);

                                                                        if (value != null)
                                                                        {
                                                                            startDate = Convert.ToDateTime(value);
                                                                        }
                                                                    }
                                                                    if (appIntegrationActivity != null &&
                                                                            propertyMap.TryGetValue(appIntegrationActivity.Actualcolumnnames.Trim(), out var aiproperty))
                                                                    {
                                                                        var value = aiproperty.GetValue(x);

                                                                        if (value != null)
                                                                        {
                                                                            startDate = Convert.ToDateTime(value);
                                                                        }
                                                                    }

                                                                    if (!startDate.HasValue)
                                                                    {
                                                                        startDate = group
                                                                            .Where(a => !string.Equals(a.Actualcolumnnames?.Trim(), "MigrationStart", StringComparison.OrdinalIgnoreCase) || 
                                                                                        !string.Equals(a.Actualcolumnnames?.Trim(), "Startofappintegration", StringComparison.OrdinalIgnoreCase))
                                                                            .Select(activity =>
                                                                            {
                                                                                if (!propertyMap.TryGetValue(activity.Actualcolumnnames?.Trim() ?? "", out var prop))
                                                                                    return (DateTime?)null;

                                                                                var value = prop.GetValue(x);

                                                                                return value != null ? Convert.ToDateTime(value) : (DateTime?)null;
                                                                            })
                                                                            .FirstOrDefault(d => d.HasValue);
                                                                    }

                                                                    var allGroups = activities
                                                                        .GroupBy(a => new
                                                                        {
                                                                            a.Activitiesorder,
                                                                            a.Activities
                                                                        })
                                                                        .OrderBy(g => g.Key.Activitiesorder)
                                                                        .ToList();


                                                                    if (index < allGroups.Count - 1)
                                                                    {
                                                                        var nextGroup = allGroups[index + 1];
                                                                        var nextmigrationActivity = nextGroup.FirstOrDefault(a =>
                                                                            string.Equals(a.Actualcolumnnames?.Trim(), "MigrationStart", StringComparison.OrdinalIgnoreCase));
                                                                        var nextAppIntegrationActivity = nextGroup.FirstOrDefault(a =>
                                                                            string.Equals(a.Actualcolumnnames?.Trim(), "Startofappintegration", StringComparison.OrdinalIgnoreCase));


                                                                        object value = null;

                                                                        if (nextmigrationActivity != null &&
                                                                            propertyMap.TryGetValue(nextmigrationActivity.Actualcolumnnames.Trim(), out var migrationProperty))
                                                                        {
                                                                            value = migrationProperty.GetValue(x);
                                                                        }

                                                                        if (nextAppIntegrationActivity != null &&
                                                                            propertyMap.TryGetValue(nextAppIntegrationActivity.Actualcolumnnames.Trim(), out var aiProperty))
                                                                        {
                                                                            value = aiProperty.GetValue(x);
                                                                        }

                                                                        if (value == null)
                                                                        {
                                                                            foreach (var activity in nextGroup)
                                                                            {
                                                                                if (string.Equals(activity.Actualcolumnnames?.Trim(), "MigrationStart", StringComparison.OrdinalIgnoreCase) || 
                                                                                    string.Equals(activity.Actualcolumnnames?.Trim(), "Startofappintegration", StringComparison.OrdinalIgnoreCase))
                                                                                    continue;

                                                                                if (propertyMap.TryGetValue(activity.Actualcolumnnames?.Trim() ?? "", out var nextproperty))
                                                                                {
                                                                                    value = nextproperty.GetValue(x);

                                                                                    if (value != null)
                                                                                        break;
                                                                                }
                                                                            }
                                                                        }

                                                                        if (value != null)
                                                                        {
                                                                            endDate = Convert.ToDateTime(value).AddDays(-1);
                                                                        }
                                                                    }
                                                                    if (startDate == null || endDate == null)
                                                                    {
                                                                        startDate = null;
                                                                        endDate = null;
                                                                    }


                                                                    return new ExodusMilestoneAndActivityGrid
                                                                    {
                                                                        ExodusMilestoneAndActivityId = group.First().Exodusmilestoneandactivityid,

                                                                        ActivityDescription = group.Key.Activities,
                                                                        ActivityOrder = group.Key.Activitiesorder,
                                                                        MileStoneDescription = group.FirstOrDefault().Milestones,

                                                                        ActualColumnName = string.Join(", ",
                                                                            group.Select(g => g.Actualcolumnnames)),

                                                                        ActivityStartDate = startDate,
                                                                        ActivityEndDate = endDate,
                                                                    };
                                                                })
                                                                .OrderBy(a => a.ActivityOrder)
                                                                .ToList()
                                        }).ToList(),

                                    };
                                })
                                .ToList();

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = new
                    {
                        daAssetMigrationDetails
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Issue Occured in the ExodusGraphicalLevel3ReportManager class --> FindWithConditionAsync method with the following Error Message" + ex.Message + " " + ex.StackTrace);
                throw;
            }



        }
        #endregion

    }
}



