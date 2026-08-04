using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.Grid;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.GraphicalReports;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.GraphicalReports;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.GraphicalReport.Exodus_Graphical_Report
{
    public class ExodusGraphicalLevel1ReportManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _manager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly CommonManager _commonManager;
        private readonly ILoggerManager _logger;
        private Dictionary<int, string> filteredPlannedActivity = new Dictionary<int, string>();
        private bool isTargetDc = false;
        private readonly DateTime complainceDate = new DateTime(2030, 03, 31);
       
        private readonly DapperCommonManager _dapperCommonManager;
        public ExodusGraphicalLevel1ReportManager(IEnumerable<IRepositoryWrapper> wrappers,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager, GridCustomColumnManager manager,
            DropdownDataServiceManager dropdownDataServiceManager, ILoggerManager logger , DapperCommonManager dapperCommonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _commonManager = commonManager;
            _logger = logger;
            _dapperCommonManager = dapperCommonManager;
        }

        private IQueryable<Designaspects> GetQuery(ExpressionStarter<Designaspects> predicateResult, bool includeDeleted)
        {
            var excludeOemList = _dapperCommonManager.GetHardwareOemForExodusFilterAsync().Result;
            var hwOem = excludeOemList?.Select(x => x.Orgeqpmanufacturerid).ToList();

            var excludedOpcoList = _dapperCommonManager.GetRestrictedOpcoAsync().Result;
            var excludedOpcoid = excludedOpcoList?.Select(x => (short?)x.Opcoid).ToList();

            var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result;
            var hwPlatform = excludePlatformList?.Select(X => X.Platformid).ToList();

            var result = isTargetDc ? _repositoryWrapper.DesignAspectRepository.FindByCondition(predicateResult, includeDeleted).Where(x => x.Archived == false)
                        .Include(x => x.Opco)
                        .Include(x => x.Plannedactivities).ThenInclude(p => p.Plannedactivityresource)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Plannedactivities).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform) 
                        : _repositoryWrapper.DesignAspectRepository.FindByCondition(predicateResult, includeDeleted).Where(x => x.Archived == false
                         && !excludedOpcoid.Contains(x.Opcoid)
                         && x.Designcomponentfamily.Designcomponents.Any(f => f.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwOem.Contains(y.Majorhardware.Orgeqpmanufacturerid)))
                         && x.Designcomponentfamily.Designcomponents.Any(f => f.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwPlatform.Contains(y.Majorhardware.Platformid)))
                         )
                        .Include(x => x.Opco)
                        .Include(x => x.Plannedactivities).ThenInclude(p => p.Plannedactivityresource)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Plannedactivities).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform);


            return result.AsQueryable();
        }
        private ExpressionStarter<Designaspects> ApplyFilter(ExodusGraphicalReportQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Designaspects>(true);
            var predicateInner = PredicateBuilder.New<Designaspects>();

            predicateInner.Or(x => x.Archived == false);
            predicateResult.And(predicateInner);


            if (buildFilterDto.OpcoId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.OpcoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.ProductId)
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Majorsoftwarebuilds.Productnameid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VendorId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.VendorId)
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlatformId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.PlatformId)
                {
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Systemtypesmajorhardwarebuilds.Any(mjh => mjh.Majorhardware.Platformid == item)));
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TargetPlatformId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.TargetPlatformId)
                {
                    predicateInner.Or(x =>
                        x.Plannedactivities
                            .SelectMany(pa => pa.Designcomponentfamily.Designcomponents)
                            .Any(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds
                                .Any(mhb => mhb.Majorhardware.Platform.Platformid == item))
                    );
                }
                predicateResult.And(predicateInner);
                isTargetDc = true;
            }

            return predicateResult;

        }
        public async Task<IEnumerable<ExodusGraphicalReportDto>> GetBaseExodusRecords(List<Designaspects> daEntity, DateTime? selectedDate, bool isPageLoad = false)
        {
            try
            {
                var excludeOemList = _dapperCommonManager.GetHardwareOemForExodusFilterAsync().Result;
                var hwOem = excludeOemList?.Select(x => x.Orgeqpmanufacturerid).ToList();

                var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result;
                var hwPlatform = excludePlatformList?.Select(X => X.Platformid).ToList();

                var exodusBaseEntity = (await Task.WhenAll(
                  daEntity.Select(async y =>
                  {
                      var hardwareBuilds = y.Designcomponentfamily.Designcomponents
                          .SelectMany(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds)
                          .Where(h => h.Ismain).Select(t => t.Majorhardware)
                          .FirstOrDefault();

                      bool assetHwOem = hwOem.Any(t => t == hardwareBuilds.Orgeqpmanufacturerid);

                      if (!assetHwOem)
                          return null;

                      bool assetHwPlatform = hwPlatform.Any(t => t == hardwareBuilds.Platformid);

                      if (!assetHwPlatform)
                          return null;

                      return new ExodusGraphicalReportDto
                                      {
                                          OpcoId = (long)y.Opcoid,
                                          OpCoDescrption = y?.Opco?.Opco,
                                          PlannedActivityType = y?.Plannedactivities?.Any() == true
                                              ? y.Plannedactivities.Select(x => x.Plannedactivityresource?.Plannedactivityresource).FirstOrDefault()
                                              : string.Empty,
                                          SourceNodeCount = GetNodeCountBasedOpcoAndDcfCombination((short)y.Opcoid, y.Designcomponentfamilyid),
                                          compatibilityColorCode = y?.Plannedactivities?.Any() == true
                                              ? CalculateCompliance(y.Plannedactivities.ToList(), isTargetDc)
                                              : ConstantValueFilter.Red,
                                          ProductName = y.Designcomponentfamily?.Designcomponents
                                              .Select(x => x.Systemtype.Majorsoftwarebuilds.Productname.Description)
                                              .FirstOrDefault(),
                                          ProductId = (long)(y.Designcomponentfamily?.Designcomponents
                                              .Select(x => x.Systemtype.Majorsoftwarebuilds.Productname.Productnameid)
                                              .FirstOrDefault() ?? 0),
                                          Vendor = y.Designcomponentfamily?.Designcomponents
                                              .Select(x => x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer)
                                              .FirstOrDefault(),
                                          InitialStack = y.Designcomponentfamily?.Designcomponents
                                              .SelectMany(x => x.Systemtype.Systemtypesmajorhardwarebuilds)
                                              .Select(mjh => mjh.Majorhardware.Platform.Platform)
                                              .FirstOrDefault(),
                                          PlatformId = y.Designcomponentfamily?.Designcomponents
                                              .SelectMany(x => x.Systemtype.Systemtypesmajorhardwarebuilds)
                                              .Select(mjh => (long?)mjh.Majorhardware.Platform.Platformid)
                                              .FirstOrDefault() ?? 0,
                                         BuildConstruction = y.Designcomponentfamily?.Designcomponents
                                            .SelectMany(x => x.Systemtype.Systemtypesmajorhardwarebuilds)
                                            .Select(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction)
                                            .FirstOrDefault()
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
                _logger.LogError("The exception occured in the method ---- GetBaseExodusRecords ---- line number : 101 to 140 with following message : " + ex.Message + ex.StackTrace);
                throw;
            }

        }
        public async Task<IEnumerable<ExodusGraphicalReportDto>> GetOpcoWisePercentage(IEnumerable<ExodusGraphicalReportDto> lcmGlanceEntity, int overAllCount, int overAllNodeCount)
        {    
            var overAllOpcoPercentage = await Task.Run(() => lcmGlanceEntity
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

            if (buildFilterDto.VerticalName?.Any() == true)
            {
                daBaseQueryResult = ApplyVerticalFilter(daBaseQueryResult, buildFilterDto);
            }


            var exodusBaseEntity = await GetBaseExodusRecords(daBaseQueryResult.ToList(), null, isPageload);
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
        public IQueryable<Designaspects> ApplyVerticalFilter(IQueryable<Designaspects> designaspects, ExodusGraphicalReportQueryDto buildFilterDto)
        {
            var designContacts = designaspects.AsEnumerable().Select(model => {

                var DesignContactList = model?.Designcomponentfamily?.Designcomponents?.SelectMany(t => t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts
                .Where(x => x.Deleted == false).Select(m => (int?)m.Designcontactid))?.Distinct().ToList();

                var majorHardwareDesignContact = model?.Designcomponentfamily?.Designcomponents?.SelectMany(t => t?.Systemtype?.Systemtypesmajorhardwarebuilds?.SelectMany(m =>
                m.Majorhardware?.Majorhwbuildsdesigncontacts.Where(x => x.Deleted == false).Select(n => (int?)n.Designcontactid))?.Distinct().ToList())?.ToList();

                var dcList = new DesignAspect();

                if (DesignContactList?.Any() == true && majorHardwareDesignContact?.Any() == true)
                {
                    dcList.DesignContactList = DesignContactList.Union(majorHardwareDesignContact).ToList();
                    dcList.Id = model.Id;
                }
                else if (DesignContactList?.Any() == false && majorHardwareDesignContact?.Any() == true)
                {
                    dcList.DesignContactList = majorHardwareDesignContact;
                    dcList.Id = model.Id;
                }
                else if (DesignContactList?.Any() == true && majorHardwareDesignContact?.Any() == false)
                {
                    dcList.DesignContactList = DesignContactList;
                    dcList.Id = model.Id;
                }
                return dcList;
            }
            ).ToList().AsQueryable();

            foreach (var item in designContacts)
            {
                var designContactList = item.DesignContactList;
                if (designContactList?.Any() == true)
                    item.VerticalFilterDto = _commonManager.GetVerticaleNameDynamicFormat(designContactList, 0)
                            .Select(x => new FilterValueDtoKeyValueList
                            {
                                Key = Convert.ToInt16(x?.Value),
                                Value = x?.Text
                            })?.Distinct().ToList();
            }

            var verticalNames = buildFilterDto.VerticalName;

            // null case (when "yes" is included)
            var nullVerticals = designContacts;
            if (verticalNames.Contains("yes"))
                nullVerticals = designContacts.Where(x => x.VerticalFilterDto == null || (x.VerticalFilterDto != null && x.VerticalFilterDto.Count() == 0));



            var otherVerticals = designContacts.Where(x =>
                x.VerticalFilterDto != null &&
                x.VerticalFilterDto.Any(v => verticalNames.Where(t => t != "yes").Contains(v.Key.ToString()))
            );

            // combine both
            if (!(verticalNames.Contains("yes")))
                designContacts = otherVerticals;
            else if (verticalNames.Contains("yes") && verticalNames.Count > 1)
                designContacts = nullVerticals.Union(otherVerticals);
            else designContacts = nullVerticals;

            if(designContacts!=null && designContacts.Count() > 0)
            {
                var designContactIds = designContacts.Select(x => x.Id).ToList();
                designaspects=designaspects.Where(x=>designContactIds.Contains(x.Id));
            }
            else
            {
                designaspects = null;
            }

             return designaspects.AsQueryable();

        }
        public string CalculateCompliance(List<Plannedactivities> plannedactivities,bool targetDc)
        {
            
            if (plannedactivities == null || !plannedactivities.Any())
                return ConstantValueFilter.Red;

            plannedactivities = plannedactivities.Where(x => x.Archived == false && x.Deleted == false).ToList();

            /*This block is used for calculating TargetDc */
            if (targetDc)
            {
                bool isTargetDc = plannedactivities.Any(pa =>
                    pa.Designcomponentfamily.Designcomponents.Any(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds.Any(mj =>
                    ConstantValueFilter.InfraBuildConstructions.Contains(mj.Majorhardware.Platform.Platform))));

                if (isTargetDc)
                    return ConstantValueFilter.Green;
            }///

            //bool hasNoPlannedActivityForDA = plannedactivities.Any(x =>
            //    x.Plannedactivityresource != null &&
            //    x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity_For_DA
            //);

            //if (hasNoPlannedActivityForDA)
            //    return ConstantValueFilter.Amber;

            bool hasPlatformMigration = plannedactivities.Any(x =>
                x.Plannedactivityresource != null &&
                x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration
            );

            if (hasPlatformMigration)
            {
                if (plannedactivities.Any(x =>x.Plannedcompletion <= complainceDate))
                {
                    return ConstantValueFilter.Green;
                }
                else if (plannedactivities.Any(x => x.Plannedcompletion > complainceDate))
                {
                    return ConstantValueFilter.Amber;
                }
            }
            return ConstantValueFilter.Red;
        }
        public string CalculateCompliance(Plannedactivities plannedactivity, bool targetDc)
        {
            if (plannedactivity == null)
                return ConstantValueFilter.Red;

            if (targetDc)
            {
                bool isTargetDc = plannedactivity.Designcomponentfamily.Designcomponents.Any(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds.Any(mj =>
                    ConstantValueFilter.InfraBuildConstructions.Contains(mj.Majorhardware.Platform.Platform)));

                if (isTargetDc)
                    return ConstantValueFilter.Green;
            }

            //bool hasNoPlannedActivityForDA = plannedactivity.Plannedactivityresource != null &&
            //    plannedactivity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity_For_DA;

            //if (hasNoPlannedActivityForDA)
            //    return ConstantValueFilter.Amber;

            bool hasPlatformMigration = plannedactivity.Plannedactivityresource != null &&
                plannedactivity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration;

            if (hasPlatformMigration)
            {
                if (plannedactivity.Plannedcompletion <= complainceDate)
                {
                    return ConstantValueFilter.Green;
                }
                else if(plannedactivity.Plannedcompletion > complainceDate)
                {
                    return ConstantValueFilter.Amber;
                }
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
            var DaEntities = GetQuery(predicateResult, false).ToList();


            var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result.Select(x => x.Platform).ToList();



            if (DaEntities.Count <= 0 || DaEntities == null)
                return new ResultDto();

            var allProducts = DaEntities
                .Where(x => x.Designcomponentfamily != null)
                .SelectMany(x => x.Designcomponentfamily.Designcomponents)
                .Where(dc => dc.Systemtype != null &&
                             dc.Systemtype.Majorsoftwarebuilds != null)
                .GroupBy(dc => dc.Systemtype.Majorsoftwarebuilds.Productnameid)
                .Select(g => new DropdownKeyValueList
                {
                    Key = (short)g.Key,
                    Value = g.First().Systemtype.Majorsoftwarebuilds
                                .Productname.Description
                })
                .ToList();

            var allOpcos = await Task.Run(() => DaEntities.DistinctBy(x => x.Opcoid).Select(x => new DropdownKeyValueList { Key = (short)x.Opcoid, Value = x.Opco.Opco }));
            var allPlatformAndBuildConstructions = DaEntities.Where(x => x.Designcomponentfamily != null)
                                                        .SelectMany(x => x.Designcomponentfamily.Designcomponents)
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
            var allVendors = DaEntities
                .Where(x => x.Designcomponentfamily != null)
                .SelectMany(x => x.Designcomponentfamily.Designcomponents)
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
            var excludeTargetPlatformList = _dapperCommonManager.GetTargetHWPlatformForExodusFilterAsync().Result.Select(x => x.Platform).ToList();

            var targetedPlatformAndBuildConstructions = DaEntities
                                                        .Where(x => x.Plannedactivities != null)
                                                        .SelectMany(x => x.Plannedactivities).Where(x => x.Designcomponentfamily != null)
                                                        .SelectMany(x => x.Designcomponentfamily.Designcomponents)
                                                        .Where(dc => dc.Systemtype != null &&
                                                                     dc.Systemtype.Systemtypesmajorhardwarebuilds != null)
                                                        .SelectMany(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds)
                                                        .Where(x => x.Majorhardware != null && x.Majorhardware.Platform != null
                                                        && excludeTargetPlatformList.Contains(x.Majorhardware.Platform.Platform.Replace(" ", "").ToUpper())
                                                        )
                                                        .GroupBy(mjh => mjh.Majorhardware.Platform.Platformid)
                                                        .Select(pf => new DropdownKeyValueList
                                                        {
                                                            Key = pf.Key,
                                                            Value = pf.FirstOrDefault().Majorhardware.Platform.Platform
                                                        }).ToList();

            //targetedPlatformAndBuildConstructions = targetedPlatformAndBuildConstructions
            //                                        .OrderByDescending(x =>
            //                                            ConstantValueFilter.InfraBuildConstructions.Contains(
            //                                                x.Value.Replace(" ", "").ToUpper()))
            //                                        .ThenBy(x => x.Value)
            //                                        .ToList();

            //var targetedPlatformAndBuildConstructions = _dropdownDataServiceManager.GetPlaftformAndBuildContructionResource(false, false, true, false).Result.Data;


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
                },
            };
        }
        #endregion
    }
}



