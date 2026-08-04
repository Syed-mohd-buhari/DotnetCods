using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.Grid;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.GraphicalReports;
using CAM.DataTransferObjects.QueryDto.GraphicalReports;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.GraphicalReport.Exodus_Graphical_Report
{
    public class ExodusAssetLGraphicalevel1ReportManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _manager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        //private ExodusAssetLevelReportManager _exodusAssetLevelReportManager;
        private readonly CommonManager _commonManager;
        private readonly ILoggerManager _logger;
        private Dictionary<int, string> filteredPlannedActivity = new Dictionary<int, string>();
        private bool isTargetDc = false;
        private readonly DateTime complainceDate = new DateTime(2030, 03, 31);
       
        private readonly DapperCommonManager _dapperCommonManager;
        public ExodusAssetLGraphicalevel1ReportManager(IEnumerable<IRepositoryWrapper> wrappers,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager, GridCustomColumnManager manager,
            DropdownDataServiceManager dropdownDataServiceManager, ILoggerManager logger , DapperCommonManager dapperCommonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _commonManager = commonManager;
            _logger = logger;
            _dapperCommonManager = dapperCommonManager;
           // _exodusAssetLevelReportManager = exodusAssetLevelReportManager;
        }
        private IQueryable<Networkelementsasplanned> GetQuery(ExpressionStarter<Networkelementsasplanned> predicateResult, bool isPageLoad = false)
        {
            var excludedOpcoList = _dapperCommonManager.GetRestrictedOpcoAsync().Result;
            var excludedOpcoid = excludedOpcoList?.Select(x => x.Opcoid).ToList();

            var excludeOemList = _dapperCommonManager.GetHardwareOemForExodusFilterAsync().Result;
            var hwOem = excludeOemList?.Select(x => x.Orgeqpmanufacturerid).ToList();


            var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result;
            var hwPlatform = excludePlatformList?.Select(X => X.Platformid).ToList();

            var query = isTargetDc ? _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult).AsQueryable()
                .Include(x => x.Daassetmigration)
                .Include(x => x.Daassetmigration).ThenInclude(x => x.Platform)
                .Include(x => x.Daassetmigration).ThenInclude(x => x.Productname)
                .Include(x => x.Location)
                .Include(x => x.Environment)
                .Include(x => x.Opco)
                .Include(x => x.Deploymentstatus)
                .Include(x => x.Designcomponentfamily)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Networkelementasplannedsubdomainspoc).ThenInclude(x => x.Subdomainspoc).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation)
                : 
                _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult)
                .Where(f =>
                !excludedOpcoid.Contains(f.Opcoid)
                && f.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwOem.Contains(y.Majorhardware.Orgeqpmanufacturerid))
                && f.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwPlatform.Contains(y.Majorhardware.Platformid))
                && f.Designcomponent.Systemtype.Majorsoftwarebuilds.Isvmware == false &&
                 ConstantValueFilter.assetDeployementStatusForPlatformMigration.Contains(f.Deploymentstatus.Deploymentstatus.ToLower().Trim()))
                .AsQueryable()
                .Include(x => x.Daassetmigration)
                .Include(x => x.Daassetmigration).ThenInclude(x => x.Platform)
                .Include(x => x.Daassetmigration).ThenInclude(x => x.Productname)
                .Include(x => x.Location)
                .Include(x => x.Environment)
                .Include(x => x.Opco)
                .Include(x => x.Deploymentstatus)
                .Include(x => x.Designcomponentfamily)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Networkelementasplannedsubdomainspoc).ThenInclude(x => x.Subdomainspoc).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation);

            return query;
        }
        private ExpressionStarter<Networkelementsasplanned> ApplyFilter(ExodusGraphicalReportQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);
            var predicateInner = PredicateBuilder.New<Networkelementsasplanned>();

            if (buildFilterDto.OpcoId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.OpcoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VerticalName?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.VerticalName)
                    predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(y => y.Subdomainspoc.AspnetuserverticalsUser
                                            .Any(z => z.Organisation.Verticalid.ToString() == item && z.Deleted == false)));

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VerticalId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.VerticalId)
                    predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(y => y.Subdomainspoc.AspnetuserverticalsUser
                                            .Any(z => z.Organisation.Verticalid == item && z.Deleted == false)));

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EnvironmentId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.EnvironmentId)
                    predicateInner.Or(x => x.Environmentid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.ProductId)
                    predicateInner.Or(x =>
                        x.Daassetmigration.Any(mhb => mhb.Productnameid == item)
                    );
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VendorId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.VendorId)
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlatformId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.PlatformId)
                {
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Systemtypesmajorhardwarebuilds.Any(mjh => mjh.Majorhardware.Platformid == item)));
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TargetPlatformId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.TargetPlatformId)
                {
                    predicateInner.Or(x =>
                        x.Daassetmigration.Any(mhb => mhb.Platformid == item)
                    );
                }
                predicateResult.And(predicateInner);
                isTargetDc = true;
            }

            return predicateResult;

        }
        public async Task<IEnumerable<ExodusGraphicalReportDto>> GetBaseExodusRecords(List<Networkelementsasplanned> assetEntities, DateTime? selectedDate, bool isPageLoad = false)
        {
            try
            {

                var exodusBaseEntity = (await Task.WhenAll(
                  assetEntities.Select(async y =>
                  {

                      return new ExodusGraphicalReportDto
                      {
                          OpcoId = (long)y.Opcoid,
                          OpCoDescrption = y?.Opco?.Opco,
                          compatibilityColorCode = y?.Daassetmigration?.Any() == true
                                              ? CalculateCompliance(y.Daassetmigration.ToList(), isTargetDc)
                                              : ConstantValueFilter.Red,
                          SourceNodeCount = y?.Daassetmigration?.Any() == true ? y.Daassetmigration.LongCount() : 1,
                          ProductName = y.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname?.Description,
                          ProductId = Convert.ToInt64(y.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname?.Productnameid),
                          Vendor = y.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,
                          InitialStack = y.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                              .Select(mjh => mjh.Majorhardware?.Platform?.Platform)
                                              .FirstOrDefault(),
                          PlatformId = y.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?
                                              .Select(mjh => (long?)mjh.Majorhardware?.Platform?.Platformid)
                                              .FirstOrDefault() ?? 0,
                          //BuildConstruction = y.Designcomponentfamily?.Designcomponents
                          //   .SelectMany(x => x.Systemtype.Systemtypesmajorhardwarebuilds)
                          //   .Select(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction)
                          //   .FirstOrDefault()
                          VerticalId = _commonManager.GetVerticaleFilterDto(y.Networkelementasplannedsubdomainspoc.Select(x => x.Subdomainspocid).ToList(),
                        0, false, true)?.DistinctBy(d => d.Text)?.ToDictionary(x => x.Text, y => y.Value),
                      };
                  })))
                  .Where(x => x != null)
                  .ToList();

                if (isPageLoad && isTargetDc == false)
                {
                    var filteredExodusEntity = exodusBaseEntity.ToList().Where(x => !ConstantValueFilter.InfraBuildConstructions.Contains(x.InitialStack));
                    return filteredExodusEntity;
                }
                else
                {
                    return exodusBaseEntity;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("The exception occured in the method ---- GetBaseExodusRecords ---- with following message : " + ex.Message + ex.StackTrace);
                throw;
            }

        }
        public async Task<IEnumerable<ExodusGraphicalReportDto>> GetOpcoWisePercentage(IEnumerable<ExodusGraphicalReportDto> exodusGlanceEntity, int overAllCount, int overAllNodeCount)
        {    
            var overAllOpcoPercentage = await Task.Run(() => exodusGlanceEntity
              .AsParallel() // Use parallel processing
              .GroupBy(x => new { x.OpcoId })
              .Select(g =>
              {
                  // Pre-compute totals
                  var totalCount = g.Count();

                  // Pre-compute grouped data for color codes
                  var colorGroups = g
                      .GroupBy(e => e.compatibilityColorCode)
                      .Select(grp => new
                      {
                          ColorCode = grp.Key,
                          Count = grp.Count(),
                          NodeSum = grp.Sum(e => e.SourceNodeCount)
                      })
                      .ToList();

                  // Get counts and sums for each color code
                  var totalGreenCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Green)?.Count ?? 0;
                  var totalRedCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Red)?.Count ?? 0;
                  var totalAmberCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Amber)?.Count ?? 0;

                  var greenNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Green)?.NodeSum ?? 0;
                  var redNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Red)?.NodeSum ?? 0;
                  var amberNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Amber)?.NodeSum ?? 0;

                  return new ExodusGraphicalReportDto()
                  {
                      OpcoId = g.Key.OpcoId,
                      OpCoDescrption = g.FirstOrDefault()?.OpCoDescrption,
                      PlannedActivityType = g.FirstOrDefault()?.PlannedActivityType,
                      ProductName = g.FirstOrDefault()?.ProductName,
                      Vendor = g.FirstOrDefault()?.Vendor,
                      InitialStack = g.FirstOrDefault()?.InitialStack,
                      TargetStack = g.FirstOrDefault()?.TargetStack,
                      SourceNodeCount = (long)g.FirstOrDefault()?.SourceNodeCount,
                      PlatformId = (long)g.FirstOrDefault()?.PlatformId,
                      // Percentage calculations for DA
                      greenCompatibilityPercentage = totalCount > 0 ? ((decimal)totalGreenCount / totalCount * 100).ToString("0.#") : "0",
                      redCompatibilityPercentage = totalCount > 0 ? ((decimal)totalRedCount / totalCount * 100).ToString("0.#") : "0",
                      amberCompatibilityPercentage = totalCount > 0 ? ((decimal)totalAmberCount / totalCount * 100).ToString("0.#") : "0",
                      TotalPercentage = totalCount > 0 ? ((decimal)(totalGreenCount + totalAmberCount + totalRedCount) / totalCount * 100).ToString("0.#") : "0",
                      // Percentage calculations for Asset
                      greenNodePercentage = overAllNodeCount > 0 ? ((decimal)greenNodeSum / overAllNodeCount * 100).ToString("0.#") : "0",
                      amberNodePercentage = overAllNodeCount > 0 ? ((decimal)amberNodeSum / overAllNodeCount * 100).ToString("0.#") : "0",
                      redNodePercentage = overAllNodeCount > 0 ? ((decimal)redNodeSum / overAllNodeCount * 100).ToString("0.#") : "0",

                      greenNodeCount = (short)greenNodeSum,
                      redNodeCount = (short)redNodeSum,
                      amberNodeCount = (short)amberNodeSum,

                  };
              }).OrderBy(x => x.OpCoDescrption).ToList());


            return overAllOpcoPercentage;
        }
        public async Task<ResultDto> FindWithCondition(ExodusGraphicalReportQueryDto buildFilterDto, bool isPageload = false)
        {


            var predicateResult = ApplyFilter(buildFilterDto);

            var daBaseQueryResult = await Task.Run(() => GetQuery(predicateResult, false));

            var exodusBaseEntity = await GetBaseExodusRecords(daBaseQueryResult.ToList(), null, isPageload);
            if (buildFilterDto?.VerticalId?.Any() == true)
            {
                exodusBaseEntity.Where(f => f.VerticalId.Any(a => buildFilterDto.VerticalName.Contains(a.Key)));
            }
            int overAllCount = exodusBaseEntity.Count();
            int overAllNodeCount = (int)exodusBaseEntity.Sum(s => s.SourceNodeCount);
            var opcowisePercentage = await GetOpcoWisePercentage(exodusBaseEntity, overAllCount, overAllNodeCount);
            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    opcowisePercentage
                }
            };

        }
        
        public string CalculateCompliance(List<Daassetmigration> daassetmigrations,bool targetDc)
        {
            
            if (daassetmigrations == null || !daassetmigrations.Any())
                return ConstantValueFilter.Red;

            daassetmigrations = daassetmigrations.Where(x => /*x.Isdecommissioned == false &&*/ x.Deleted == false).ToList();

            /*This block is used for calculating TargetDc */
            if (targetDc)
            {
                bool isTargetDc = daassetmigrations.Any(pa => ConstantValueFilter.InfraBuildConstructions.Contains(pa.Platform.Platform));

                if (isTargetDc)
                    return ConstantValueFilter.Green;
            }

            if (daassetmigrations.Any(x =>x.Migrationcompletiondate <= complainceDate))
            {
                return ConstantValueFilter.Green;
            }
            else if (daassetmigrations.Any(x => x.Migrationcompletiondate > complainceDate))
            {
                return ConstantValueFilter.Amber;
            }
            
            return ConstantValueFilter.Red;
        }
        public string CalculateCompliance(Daassetmigration daassetmigration, bool targetDc)
        {
            if (daassetmigration == null)
                return ConstantValueFilter.Red;

            if (targetDc)
            {
                bool isTargetDc = ConstantValueFilter.InfraBuildConstructions.Contains(daassetmigration.Platform?.Platform);

                if (isTargetDc)
                    return ConstantValueFilter.Green;
            }

            if (daassetmigration.Migrationcompletiondate <= complainceDate)
            {
                return ConstantValueFilter.Green;
            }
            else if(daassetmigration.Migrationcompletiondate > complainceDate)
            {
                return ConstantValueFilter.Amber;
            }
            
            return ConstantValueFilter.Red;
        }

        public int GetNodeCountBasedOpcoAndDcfCombination(short? opcoId, long dcfId)
        {
            try
            {
                var dcIds = _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentfamilyid == dcfId)
                    .Select(x => x.Designcomponentid)
                    .ToList();

                var activeLcm = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x =>
                        x.Archived != true &&
                        x.Opcoid == opcoId &&
                        dcIds.Contains(x.Designcomponentid))
                    .Include(x => x.Networkelementsasplanned)
                        .ThenInclude(x => x.Deploymentstatus);

                var elementCount = activeLcm
                    .SelectMany(x => x.Networkelementsasplanned)
                    .Count(x =>
                        x != null &&
                        x.Deploymentstatus != null &&
                        ConstantValueFilter.assetDeployementStatusForPlatformMigration
                            .Contains(x.Deploymentstatus.Deploymentstatus.ToLower()));

                return elementCount;
            }
            catch (Exception ex)
            {
                _logger.LogError("The exception occured in the method ---- GetNodeCountBasedOpcoAndDcfCombination with following messagae : " + ex.Message + ex.StackTrace);
                throw;
            }
        }

        #region Feedback on Exodus report

        public async Task<ResultDto> GetAllDropdowns(ExodusGraphicalReportQueryDto filterDto,bool pageLoad = false)
        {
            var predicateResult = ApplyFilter(filterDto);
            var assetEntities = GetQuery(predicateResult, false).ToList();

            if(filterDto?.VerticalName?.Any() == true)
            {
                assetEntities = assetEntities.Where(f => f.Networkelementasplannedsubdomainspoc.Any(y => y.Subdomainspoc.AspnetuserverticalsUser
                                            .Any(z => filterDto.VerticalName.Contains(z.Organisation.Verticalid.ToString())))).ToList();
            }

            var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result.Select(x => x.Platform).ToList();
            var excludeTargetPlatformList = _dapperCommonManager.GetTargetHWPlatformForExodusFilterAsync().Result.Select(x => x.Platform).ToList();

            if (assetEntities.Count <= 0 || assetEntities == null)
                return new ResultDto();

            var allProducts = assetEntities.Where(a => a.Daassetmigration != null)
                                                        .SelectMany(a => a.Daassetmigration).Where(x => x.Productname != null)
                                                        .GroupBy(mhb => mhb.Productname.Productnameid)
                                                        .Select(g =>
                                                        {
                                                            var first = g.First();
                                                            return new DropdownKeyValueList
                                                            {
                                                                Key = Convert.ToInt16(g.Key),
                                                                Value = first.Productname.Description
                                                            };
                                                        }).ToList();

            var allOpcos = await Task.Run(() => assetEntities.DistinctBy(x => x.Opcoid).Select(x => new DropdownKeyValueList { Key = (short)x.Opcoid, Value = x.Opco.Opco }));

            var allPlatformAndBuildConstructions = assetEntities.Where(x => x.Designcomponent != null)
                                                        .Select(x => x.Designcomponent)
                                                        .Where(dc => dc.Systemtype != null &&
                                                                     dc.Systemtype.Systemtypesmajorhardwarebuilds != null)
                                                        .SelectMany(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds).Where(x => x.Majorhardware != null && x.Majorhardware.Platform != null
                                                        && excludePlatformList.Contains(x.Majorhardware.Platform.Platform))
                                                        .GroupBy(mjh => mjh.Majorhardware.Platform.Platformid)
                                                        .Select(pf => new DropdownKeyValueList
                                                        {
                                                            Key = pf.Key,
                                                            Value = pf.FirstOrDefault().Majorhardware.Platform.Platform
                                                        }).ToList();

            var allVendors = assetEntities.Where(x => x.Designcomponent != null)
                             .Select(x => x.Designcomponent)
                            .Where(dc => dc.Systemtype != null &&
                                         dc.Systemtype.Majorsoftwarebuilds != null)
                            .GroupBy(dc => dc.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid)
                            .Select(g => new DropdownKeyValueList
                            {
                                Key = (short)g.Key,
                                Value = g.First().Systemtype.Majorsoftwarebuilds
                                            .Orgeqpmanufacturer.Originalequipmentmanufacturer
                            })
                            .ToList();

            var ResourceVertical = assetEntities.Where(f => f.Networkelementasplannedsubdomainspoc != null && f.Networkelementasplannedsubdomainspoc.Count > 0)
                .SelectMany(x => _commonManager.GetVerticaleFilterDto(x.Networkelementasplannedsubdomainspoc.Select(r => r.Subdomainspocid).ToList(), 0, false, true)?.Distinct()
                .Select(x => new DropdownKeyValueList { Key = Convert.ToInt16(x.Value), Value = x.Text })).DistinctBy(d => d.Key);

            var EnvironmentResource = assetEntities.Select(x => new DropdownKeyValueList { Key = x.Environmentid, Value = x.Environment.Environment }).DistinctBy(d => d.Key).ToList();



            var targetedPlatformAndBuildConstructions = assetEntities.Where(a => a.Daassetmigration != null)
                                                        .SelectMany(a => a.Daassetmigration).Where(x => x.Platform != null)
                                                        .Where(x => excludeTargetPlatformList.Contains(x.Platform.Platform.ToUpper().Replace(" ","")))
                                                        .GroupBy(mhb => mhb.Platform.Platformid)
                                                        .Select(g =>
                                                        {
                                                            var first = g.First();
                                                            return new DropdownKeyValueList
                                                            {
                                                                Key = g.Key,
                                                                Value = first.Platform.Platform
                                                            };
                                                        }).ToList();

           // var targetedPlatformAndBuildConstructions = _dropdownDataServiceManager.GetPlaftformAndBuildContructionResource(false, false, true, false).Result.Data;

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    allOpcos,
                    allProducts,
                    allVendors,
                    allPlatformAndBuildConstructions,
                    targetedPlatformAndBuildConstructions,
                    ResourceVertical,
                    EnvironmentResource,
                },
            };
        }
        #endregion
    }
}



