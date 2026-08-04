using CAM.BusinessManager.Entity;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.Enum;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CAM.BusinessManager.AbstractionLayer
{
    public class AbstractionLayerManager : BaseManager
    {
        IRepositoryWrapper _repositoryWrapper;
        AuthorizedRoleManager _authorizedRoleManager;
        private DateTime currentDate = System.DateTime.Now.Date;
        private static DateTime today = DateTime.Today;
        private DateTime whatsGoingOnDefaultDate = new DateTime(today.Year, today.Month, 1).AddMonths(-ConstantValueFilter.WhatsGoingOnDate);
        private DateTime messagingDefaultDate = new DateTime(today.Year, today.Month, 1).AddMonths(-ConstantValueFilter.MessagingDate);
        private int preferenceRange = 0;
        private DateTime? preferenceDate = null;
        private int messagingDateRange = 0;
        private DateTime? messagingDate = null;
        private readonly ILoggerManager _loggerManager;

        public AbstractionLayerManager(IEnumerable<IRepositoryWrapper> wrappers, AuthorizedRoleManager authorizedRoleManager, ILoggerManager loggerManager,
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _authorizedRoleManager = authorizedRoleManager;
            _loggerManager = loggerManager;
        }

        #region // feedbackLoop
        public async Task<FeedbackLoopLatestProcessTimeDtoGrid> FindWithConditionForProcessTimeRecords()
        {
            try
            {
                var query = GetProcessTimeQuery();

                var result = await query
                .Select(x => new FeedbackLoopLatestProcessTimeDtoGrid
                {
                    LatestProcessTime = x.Processendtime
                })
                .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }

        }
        private IQueryable<Feedbackloopaudits> GetProcessTimeQuery()
        {
            try
            {
                var query = _repositoryWrapper.FeedBackLoopAuditRepository.FindAll().AsNoTracking().OrderByDescending(x => x.Processendtime)
                    .Select(r => new Feedbackloopaudits
                    {
                        Processendtime = r.Processendtime
                    });

                return query;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }

        }

        public async Task<List<ReportLatestProcessTimeDto>> GetSchedulerReportProcessTimeQuery()
        {
            try
            {
                var result = await _repositoryWrapper.UserDefinedReportsLogsRepository
                            .FindByCondition(x => x.Reportstatus.ToLower() == "success")
                            .AsNoTracking()
                            .GroupBy(x => x.Reportname)
                            .Select(r => new ReportLatestProcessTimeDto
                            {
                                ReportName = r.Key,
                                Processstarttime = r
                                    .OrderByDescending(x => x.Modificationdate)
                                    .First().Modificationdate
                            })
                            .ToListAsync();


                return result;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region // LCM Complaines
        public async Task<LcmComplianceDto> FindWithConditionForLcmComplianceRecords(int userId)
        {
            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetails(userId);
                var predicateResult = ApplyFilter(userDetailsList.OpcoDetails, userDetailsList.VerticalDetails);

                var query = await GetLcmComplianceQuery(predicateResult);
                var baseLcmComplainceRecords = await GetBaseLcmGlanceRecords(query);
                var overAllAssetPercentage = GetOverAssetWisePercentage(baseLcmComplainceRecords, baseLcmComplainceRecords.Count, (int)baseLcmComplainceRecords.Sum(x => x.NodesCount));

                return overAllAssetPercentage;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        private ExpressionStarter<Lcmengineering> ApplyFilter(List<short> opCoId, List<int> verticalId)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);
            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);

            if (opCoId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in opCoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (verticalId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in verticalId)
                    predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuseropcosUser
                    .Any(m => m.Deleted == false && m.Opcoid.ToString().Contains(x.Opcoid.ToString())))
                    && x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser
                    .Any(m => m.Organisation.Verticalid == item))
                    );
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        private async Task<List<Lcmengineering>> GetLcmComplianceQuery(ExpressionStarter<Lcmengineering> predicateResult)
        {
            try
            {
                var query = await _repositoryWrapper.Lcmengineering.FindByCondition(predicateResult).AsNoTracking()
                    .OrderBy(P => P.Opco.Opco)
                   .Select(r => new Lcmengineering
                   {
                       Opco = new Opcos
                       {
                           Opcoid = r.Opco.Opcoid,
                           Opco = r.Opco.Opco,
                       },
                       Lcmengineeringsubdomainspoc = r.Lcmengineeringsubdomainspoc,
                       Lcmengineeringid = r.Lcmengineeringid,
                       Designcomponent = new Designcomponents
                       {
                           Systemtype = new Systemtypes
                           {
                               Majorsoftwarebuilds = new Majorsoftwarebuilds
                               {
                                   Endofmaintenance = r.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance,
                                   Endofsupport = r.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport,
                                   Orgeqpmanufacturer = new Originalequipmentmanufacturers
                                   {
                                       Orgeqpmanufacturerid = r.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Orgeqpmanufacturerid,
                                       Originalequipmentmanufacturer = r.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                                   },
                                   Productname = new Productname
                                   {
                                       Productnameid = r.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Productnameid,
                                       Description = r.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description,
                                   }

                               }
                           }
                       }
                   }).ToListAsync();

                return query;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        public async Task<List<LcmAtGlanceGridDto>> GetBaseLcmGlanceRecords(List<Lcmengineering> lcmEntity)
        {
            try
            {
                var nodesCount = await GetAssetNodeCountsAsync(lcmEntity.Where(f => f.Lcmengineeringsubdomainspoc.Any()).Select(r => r.Lcmengineeringid).ToList());

                var lcmGlanceEntity = lcmEntity.Where(m => m.Lcmengineeringsubdomainspoc.Any()).Select(y => new LcmAtGlanceGridDto()
                {
                    LcmengineeringId = y.Lcmengineeringid,
                    OpcoId = y.Opco.Opcoid,
                    OpCoDescrption = y.Opco.Opco,
                    NodesCount = nodesCount.GetValueOrDefault(y.Lcmengineeringid),
                    compatibilityColorCode = calculateRagStatusFromSoftware(y),
                }).ToList();

                return lcmGlanceEntity;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        private async Task<Dictionary<long?, int>> GetAssetNodeCountsAsync(List<long> lcmIds)
        {
            try
            {
                var allowedStatusIds = await _repositoryWrapper.DeploymentStatus.FindByCondition(x =>
                        x.Deploymentstatus.ToLower().Trim() == "in-service" ||
                        x.Deploymentstatus.ToLower().Trim() == "in commissioning" ||
                        x.Deploymentstatus.ToLower().Trim() == "decommissioning")
                    .Select(x => x.Deploymentstatusid)
                    .ToListAsync();

                return await _repositoryWrapper.NetworkElementAsPlanned
                    .FindAll()
                    .AsNoTracking()
                    .Where(n =>
                        lcmIds.Contains((long)n.Lcmengineeringid) &&
                        n.Environment.Environment == "production" &&
                        allowedStatusIds.Contains(n.Deploymentstatusid) &&
                        !ConstantValueFilter.excludeDeployementStatus
                            .Contains(n.Deploymentstatus.Deploymentstatus))
                    .GroupBy(n => n.Lcmengineeringid)
                    .Select(g => new
                    {
                        LcmId = g.Key,
                        Count = g.Count()
                    })
                    .ToDictionaryAsync(x => x.LcmId, x => x.Count);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        public string calculateRagStatusFromSoftware(Lcmengineering entity)
        {
            try
            {
                var endOfMaintenance = entity?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofmaintenance;
                var endOfSupport = entity?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofsupport
                                   ?? ((endOfMaintenance == null) ? currentDate : endOfMaintenance)?.AddYears(ConstantValueFilter.eosCalulatedDayCount).Date;

                var ragStatus =
                    endOfMaintenance == null || endOfMaintenance > currentDate
                        ? ConstantValueFilter.Green
                        : (currentDate > endOfMaintenance && endOfSupport > currentDate)
                            ? ConstantValueFilter.Amber
                            : ConstantValueFilter.Red;

                return ragStatus;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        public LcmComplianceDto GetOverAssetWisePercentage(List<LcmAtGlanceGridDto> lcmGlanceEntity, int overAllCount, int overAllNodeCount)
        {
            try
            {
                if (overAllCount == 0 || lcmGlanceEntity == null)
                    return new LcmComplianceDto();

                var colorStats = lcmGlanceEntity
                    .GroupBy(e => e.compatibilityColorCode)
                    .ToDictionary(
                        g => g.Key,
                        g => new
                        {
                            Count = g.Count(),
                            NodeCount = g.Sum(x => x.NodesCount)
                        });

                int GetCount(string color) =>
                    colorStats.TryGetValue(color, out var v) ? v.Count : 0;

                long GetNodeCount(string color) =>
                    colorStats.TryGetValue(color, out var v) ? v.NodeCount : 0;

                string Percent(long value, long total) =>
                    total > 0 ? ((decimal)value / total * 100).ToString("0.##") : "0";

                var greenCount = GetCount(ConstantValueFilter.Green);
                var amberCount = GetCount(ConstantValueFilter.Amber);
                var redCount = GetCount(ConstantValueFilter.Red);

                var greenNodes = GetNodeCount(ConstantValueFilter.Green);
                var amberNodes = GetNodeCount(ConstantValueFilter.Amber);
                var redNodes = GetNodeCount(ConstantValueFilter.Red);

                var totalNodes = greenNodes + amberNodes + redNodes;

                return new LcmComplianceDto
                {
                    // Compatibility %
                    GreenCompatibilityPercentage = Percent(greenCount, overAllCount),
                    AmberCompatibilityPercentage = Percent(amberCount, overAllCount),
                    RedCompatibilityPercentage = Percent(redCount, overAllCount),
                    TotalPercentage = Percent(greenCount + amberCount + redCount, overAllCount),
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region // EOM and EOS  details
        public async Task<List<GetEosAndEomMileStonesDtoGrid>> GetEosAndEomMileStonesFromActiveLcms(int userId)
        {
            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetails(userId);

                var prefrenceDateRange = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName && f.Preferencedate != null)
                    .Select(r => r.Preferencedate).FirstOrDefaultAsync();

                if (prefrenceDateRange != null)
                {
                    preferenceRange = prefrenceDateRange.Value;            

                }
                else
                {
                    preferenceRange = ConstantValueFilter.WhatsGoingOnDate;
                }
                var startDate = today.AddMonths(preferenceRange);
                startDate = new DateTime(startDate.Year, startDate.Month, 1);
                preferenceDate = startDate.AddMonths(1).AddDays(-1);

                bool isAdmin = userDetailsList.UserRoleId.Any(f => f == 1);

                var opcoIds = isAdmin
                    ? new List<short>()
                    : userDetailsList.OpcoDetails.Distinct().ToList();

                var data = await _repositoryWrapper.Lcmengineering
                    .FindByCondition(x =>
                        x.Deleted == false &&
                        x.Archived == false &&
                        (
                            isAdmin ||
                            opcoIds.Contains((short)x.Opcoid)
                        )
                        && (x.Opco.Opco.ToLower() != ("zzz"))
                        //&&
                        //(
                        //    (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > today &&
                        //     x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < preferenceDate)
                        //    ||
                        //    (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport > today &&
                        //     x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport < preferenceDate)
                        //)
                    )
                    .AsNoTracking()
                    .Select(x => new
                    {
                        Opco = x.Opco.Opco,
                        Product = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description,
                        Oem = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer,
                        Version = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion,
                        Eom = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance,
                        Eos = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport,
                        MajorSwId = x.Designcomponent.Systemtype.Majorsoftwarebuildsid
                    })?.Where
                        ( y => 
                            (y.Eom > today &&
                             y.Eom  < preferenceDate)
                            ||
                            (y.Eos > today &&
                             y.Eos < preferenceDate)
                        ).OrderBy(x => x.Eom == null).ThenBy(x => x.Eom)
                    .ToListAsync();



                var result = data
                         .GroupBy(x => x.Product)
                         .Select(productGroup => new GetEosAndEomMileStonesDtoGrid
                         {
                             Product = $"{productGroup.Select(r => r.Oem.Originalequipmentmanufacturer).FirstOrDefault()}-{productGroup.Key}",

                             Versions = productGroup
                                 .GroupBy(v => v.Version)
                                 .Select(versionGroup => new
                                 {
                                     VersionGroup = versionGroup,
                                     Eom = versionGroup.Select(x => x.Eom).FirstOrDefault(),
                                     Eos = versionGroup.Select(x => x.Eos).FirstOrDefault()
                                 })
                                 .OrderBy(x => x.Eom == null)
                                 .ThenBy(x => x.Eom)
                                 .Select(x => new VersionDto
                                 {
                                     Version = x.VersionGroup.Key,
                                     MajorSwId = (long)x.VersionGroup.Select(v => v.MajorSwId).FirstOrDefault(),
                                     OpCo = string.Join(",", x.VersionGroup.Select(v => v.Opco).Distinct()),
                                     EomDate = x.Eom?.ToString("dd-MM-yyyy"),
                                     EosDate = x.Eos?.ToString("dd-MM-yyyy"),
                                 })
                                 .ToList()
                         })
                         .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        #endregion

        #region  // PA SW Upgrade details
        public async Task<ResultDto> FindWithConditionForPlannedActivitySWUpgradeRecords(int userId)
        {

            var result = new List<PlanndActivitySoftwareUpgradDetailsDtoGrid>();


            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetails(userId);

                var prefrenceDateRange = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName && f.Preferencedate != null).Select(r => r.Preferencedate).FirstOrDefaultAsync();

                if (prefrenceDateRange != null)
                {
                    preferenceRange = prefrenceDateRange.Value;
                    
                }
                else
                {
                    preferenceRange = ConstantValueFilter.WhatsGoingOnDate;
                }
                var startDate = today.AddMonths(preferenceRange);
                startDate = new DateTime(startDate.Year, startDate.Month, 1);
                preferenceDate = startDate.AddMonths(1).AddDays(-1);

                var plannedActivityRules = await _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Rulelinkeddc == (int)
                PlannedActivityResourceEnum.SwArchitectureUpgrade_SwMajorRelease).AsNoTracking().Select(x => x.Plannedactivityresourceid).ToListAsync();

                var predicateResult = ApplyFilterForPlannedSWUpgradRecords(userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count >0 ?
                    userDetailsList.OpcoDetails : new List<short>()
                    , userDetailsList.UserRoleId);

                var query = await GetPlannedSWRecors(predicateResult);
   
                query = query.Where(x => x.Archived == false && x.PlannedactivitiesLcmengineering != null && x.PlannedactivitiesLcmengineering.Count > 0  ).ToList();
 
                result = query
                        .SelectMany(lcm => lcm.PlannedactivitiesLcmengineering.Select(x1 => new { lcm, x1 }))
                        .Where(g => g.x1.Deleted == false && g.x1.Archived == false && ( g.x1.Plannedcompletion > today && g.x1.Plannedcompletion < preferenceDate)
                        && (plannedActivityRules.Contains((short)g.x1.Plannedactivityresourceid)))
                        .GroupBy(pa => pa.x1.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Productnameid)
                        .Select(g => new PlanndActivitySoftwareUpgradDetailsDtoGrid
                        {
                            Oem = g.FirstOrDefault()?.x1.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,

                            Product = g.FirstOrDefault()
                                       ?.x1.Designcomponent
                                       .Systemtype
                                       .Majorsoftwarebuilds
                                       .Productname
                                       .Description,

                            PlannedAction = g?.OrderBy(pa => pa.x1.Plannedcompletion == null).ThenBy(pa => pa.x1.Plannedcompletion).Select(pa => new PaPlannedActionDto
                            {
                                PaId = pa.x1.Plannedactivityid,
                                Lcmengineeringid = pa.x1.Lcmengineeringid,
                                ActionDetail =
                                            $"{pa.x1.Opco.Opco} | " +
                                            $"{pa.x1.Plannedcompletion:MMM} - " +
                                            $"{pa.x1.Plannedcompletion:yyyy} | " +
                                            $"{pa.x1.Plannedactivityresource?.Plannedactivityresource} | " +
                                            $"{pa.x1.Deliverystatus?.Deliverystatus}",
                                Plannedcompletion = pa.x1.Plannedcompletion,
                                PlannedDcName = DesignComponentTypeExtensionMethod.ToDesignComponentNameHomePage(pa.x1.Designcomponent, _repositoryWrapper),
                                CurrentDcName = DesignComponentTypeExtensionMethod.ToDesignComponentNameHomePage(pa.lcm.Designcomponent, _repositoryWrapper),
                            }).ToList()
                        })
                        .ToList();

                var preferenceDetails = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName).FirstOrDefaultAsync();


                return new ResultDto
                {
                    Data = new
                    {
                        PlanndActivitySoftwareUpgradDetailsDtoGrid = result,
                        WhatsGoingOnDate = preferenceDetails != null &&  preferenceDetails.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.WhatsGoingOnDate,                       
                        MessagingDate = preferenceDetails != null && preferenceDetails.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.MessagingDate,
                    }
                };

            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        private ExpressionStarter<Lcmengineering> ApplyFilterForPlannedSWUpgradRecords(List<short> opCoId, List<int> ruleId)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);
            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);

            var today = DateTime.Today;
            var nextSixMonths = today.AddMonths(6);


            predicateInner.Or(x => x.Deleted == false && x.Archived == false && x.Opco.Opco.ToLower() != "zzz");
            predicateResult.And(predicateInner);

            if (opCoId?.Any() == true && ruleId.Any( f => f != 1))
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in opCoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
         
            return predicateResult;
        }

        public async Task<List<Lcmengineering>> GetPlannedSWRecors(ExpressionStarter<Lcmengineering> predicateResult,int userId = 0)
        {
            try
            {
              
                  var query = await _repositoryWrapper.Lcmengineering.FindByCondition(predicateResult) .Include(x => x.PlannedactivitiesLcmengineering)
                    .Where(x => userId == 0 || x.Lcmengineeringsubdomainspoc.Any(t => t.Subdomainspocid == userId) || x.Lcmengineeringeduspoc.Any(t => t.Eduspocid == userId))
                    .AsNoTracking()
                    .Select(x => new Lcmengineering
                    {
                        Designcomponent = x.Designcomponent == null ? null : new Designcomponents
                        {
                            Designcomponentid = x.Designcomponentid,
                            Systemtype = x.Designcomponent.Systemtype == null ? null : new Systemtypes
                            {
                                Systemtypeid = x.Designcomponent.Systemtype.Systemtypeid,
                            },
                            Designcomponentfamily = x.Designcomponent.Designcomponentfamily == null ? null : new Designcomponentfamilies
                            {
                                Subnetworkboundary = x.Designcomponent.Designcomponentfamily.Subnetworkboundary == null ? null : new Subnetworkboundaries
                                {
                                    Alias = x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias,
                                    Description = x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description,
                                }
                            }
                        },
                        Archived = x.Archived,
                        Deleted = x.Deleted,
                        Opco = x.Opco == null ? null : new Opcos
                        {
                            Opco = x.Opco.Opco,
                            Opcoid = x.Opco.Opcoid,
                        },
                        PlannedactivitiesLcmengineering = x.PlannedactivitiesLcmengineering != null && x.PlannedactivitiesLcmengineering.Count <= 0 ? 
                        null : x.PlannedactivitiesLcmengineering.Select(f => new Plannedactivities
                        {
                            Deleted = f.Deleted,
                            Lcmengineeringid = f.Lcmengineeringid,
                            Plannedcompletion = f.Plannedcompletion,
                            Startdate = f.Startdate,
                            Plannedactivityresourceid = f.Plannedactivityresourceid,
                            Deliverystatus = f.Deliverystatus,
                            Deliverystatusid = f.Deliverystatusid,
                            Archived = f.Archived,
                            Opco = f.Opco,
                            Plannedactivityresource = f.Plannedactivityresource,
                            Plannedactivityid = f.Plannedactivityid,
                            Designcomponent = f.Designcomponent == null ? null : new Designcomponents
                            {
                                Designcomponentid = f.Designcomponentid.Value,
                                Systemtype = f.Designcomponent.Systemtype == null ? null : new Systemtypes
                                {
                                    Systemtypeid = f.Designcomponent.Systemtype.Systemtypeid,
                                    Majorsoftwarebuilds = f.Designcomponent.Systemtype.Majorsoftwarebuilds == null ? null : new Majorsoftwarebuilds
                                    {
                                        Productname = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname,
                                        Orgeqpmanufacturer = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer,
                                        Endofmaintenance = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance,
                                        Endofsupport = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport,
                                        Softwareversion = f.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion
                                    }
                                },
                                Designcomponentfamily = f.Designcomponent.Designcomponentfamily == null ? null : new Designcomponentfamilies
                                {
                                    Subnetworkboundary = f.Designcomponent.Designcomponentfamily.Subnetworkboundary == null ? null : new Subnetworkboundaries
                                    {
                                        Alias = f.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias,
                                        Description = f.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description,
                                    }
                                }
                            },

                        }).OrderBy(x => x.Plannedcompletion).ToList()


                    }).ToListAsync();

                return query;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        #endregion

        #region // User Prefrenc details    
        public async Task<List<UserPrefrenceDetails>> FindWithConditionForUserPrefrenceRecords(int userId, string className, bool isHomeScreenPreference = false)
        {
            try
            {
                var userPreferenceEntity = await _repositoryWrapper.aspNetUserPreferenceRepository.FindByCondition(x => x.Userid == userId)
                        .Include(x => x.Aspnetmodule)
                        .ToListAsync();
                if (isHomeScreenPreference)
                {
                    userPreferenceEntity = userPreferenceEntity.Where(f => f.Isuserpreference == true).ToList();
                }


                if (userPreferenceEntity != null && userPreferenceEntity.Count > 0)
                {
                    var result = userPreferenceEntity.Select(r => new UserPrefrenceDetails
                    {
                        Id = r.Aspnetmodule?.Aspnetmoduleid.ToString(),
                        Text = r.Aspnetmodule?.Module,
                        Path = r.Aspnetmodule?.Modulepath,
                        Menu = r.Aspnetmodule?.Menu,
                        Order = r.Order,
                        ScreenPermission = r.Permission,

                    }).ToList();

                    return result;
                }
                return new List<UserPrefrenceDetails>();
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion


        #region  // Doing Section for PA
        public async Task<ResultDto> FindWithConditionForDoingSectionPARecords(int userId)
        {
            var result = new List<DoingSectioinForPA>();

            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetails(userId);

                var userOrgDetails = await _authorizedRoleManager.GetOrganisationSpocsDetails(userId);

                if ((userOrgDetails.IsEduSpoc == true || userOrgDetails.IsSubdomainSpoc == true) && userOrgDetails.IsUserExistInOrg == true 
                    || userDetailsList?.UserRoleId.Any(f => f == 1) == true)
                {
                    var SwPlannedActivityRulesId = await _repositoryWrapper.PlannedActivityResourceRepository
              .FindByCondition(x => x.Rulelinkeddc == (int)PlannedActivityResourceEnum.SwArchitectureUpgrade_SwMajorRelease).AsNoTracking().Select(x =>
               x.Plannedactivityresourceid).ToListAsync();

                    //#1913 - Display all the records for Admin
                    if (userDetailsList.UserRoleId.Any(f => f == 1) == true) userOrgDetails = null;

                    var predicateResult = ApplyFilterForSwPARecords(userId, userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                        userDetailsList.OpcoDetails : new List<short>(),
                        userDetailsList.UserRoleId, 0, false, userOrgDetails, SwPlannedActivityRulesId);

                    #region 
                    var MessagingDateRange = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName && f.Messagingdate != null).Select(r => r.Messagingdate).FirstOrDefaultAsync();

                    if (MessagingDateRange != null)
                    {
                        MessagingDateRange = MessagingDateRange.Value;  
                    }
                    else
                    {
                        MessagingDateRange = ConstantValueFilter.MessagingDate;
                    }

                    var startDate = today.AddMonths((int)-MessagingDateRange);
                    startDate = new DateTime(startDate.Year, startDate.Month, 1);
                    messagingDate = startDate; 

                    #endregion  
 

                    var query = await GetPlannedSWRecors(predicateResult, userOrgDetails == null ? 0 : userId);
                   
                   

                    //var ragStatus = CalculateRagStatus(query.SelectMany(x => x.PlannedactivitiesLcmengineering).ToList());

                    result = query.Where(x => x.Archived == false && (x.PlannedactivitiesLcmengineering != null && x.PlannedactivitiesLcmengineering.Count > 0))
                           .SelectMany(lcm => lcm.PlannedactivitiesLcmengineering.Select(pa => new { lcm, pa })).Where(t => t.pa.Deleted == false &&
                           (t.pa.Plannedcompletion > messagingDate && t.pa.Plannedcompletion < today ) && (SwPlannedActivityRulesId.Contains((short)t.pa.Plannedactivityresourceid)))
                           .GroupBy(g => g.pa.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Productnameid)
                           .Select(g =>
                           {
                           //    var colour = g
                           //.Select(r =>
                           //    ragStatus.TryGetValue(r.pa.Plannedactivityid, out var rag)
                           //        ? rag
                           //        : string.Empty)
                           //.FirstOrDefault();

                               return new DoingSectioinForPA
                               {
                                   Oem = g.FirstOrDefault()?.pa.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,

                                   Product = g.FirstOrDefault()
                                                               ?.pa.Designcomponent
                                                               .Systemtype
                                                               .Majorsoftwarebuilds
                                                               .Productname
                                                               .Description,

                                   PlannedAction = g?.OrderBy(o => o.pa.Plannedcompletion == null).ThenBy(to => to.pa.Plannedcompletion).Select(s => new PaPlannedActionDto
                                   {
                                       Lcmengineeringid = s.pa.Lcmengineeringid,
                                       PaId = s.pa.Plannedactivityid,
                                       Plannedcompletion = s.pa.Plannedcompletion,
                                       ActionDetail =
                                                            $"{s.pa.Opco.Opco} | " +
                                                            $"{s.pa.Plannedcompletion:MMM} - " +
                                                            $"{s.pa.Plannedcompletion:yyyy} | " +
                                                            $"{s.pa.Plannedactivityresource?.Plannedactivityresource} | " +
                                                            $"{s.pa.Deliverystatus?.Deliverystatus}",
                                       PlannedDcName = DesignComponentTypeExtensionMethod.ToDesignComponentNameHomePage(s.pa.Designcomponent, _repositoryWrapper),
                                       CurrentDcName = DesignComponentTypeExtensionMethod.ToDesignComponentNameHomePage(s.lcm.Designcomponent, _repositoryWrapper),
                                   }).ToList(),

                                  // colour = colour,
                               };
                           })?.OrderBy(x => x.Oem)
.ToList();

                }

                var preferenceDetails = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName).FirstOrDefaultAsync();

                return new ResultDto
                {
                    Data = new
                    {
                        DoingSectioinForPA = result ,
                        WhatsGoingOnDate = preferenceDetails != null && preferenceDetails?.Preferencedate != null? preferenceDetails.Preferencedate : ConstantValueFilter.WhatsGoingOnDate,
                        MessagingDate = preferenceDetails != null && preferenceDetails?.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.MessagingDate,
                    }
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
         
        private ExpressionStarter<Lcmengineering> ApplyFilterForSwPARecords(long userId, List<short> opCoId, List<int> ruleId, int calculateMonthRange = 0, bool isAchive = false,
        OrganisatioinSpocsDto dto = null, List<short> SwPlannedActivityRulesId = null, List<int> veticalId = null)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);
            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);

            preferenceDate = new DateTime(today.Year, today.Month, 1).AddMonths(calculateMonthRange);



            if (isAchive == true)
            {
                predicateInner.Or(x => x.Deleted == false && x.Opco.Opco.ToLower() != "zzz");                          
            }
            else predicateInner.Or(x => x.Deleted == false && x.Archived == isAchive && x.Opco.Opco.ToLower() != "zzz");

            predicateResult.And(predicateInner);

            if (veticalId != null && veticalId.Count >0)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
               
                foreach (var item in veticalId)
                { 
                _ = predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser
                                           .Any(m => m.Organisation.Verticalid  == item && m.Deleted == false
                                           //&& x.Lcmengineeringsubdomainspoc.Any(a => a.Subdomainspoc.AspnetuseropcosUser.Any(o => o.Opco.Opcoid == x.Opcoid))
                                           )));
                  
                } 
                    _ = predicateResult.And(predicateInner);
                
            }

            if (dto?.IsSubdomainSpoc == true &&  dto?.IsEduSpoc == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(f => f.Subdomainspocid == userId));
                predicateInner.Or(x => x.Lcmengineeringeduspoc.Any(f => f.Eduspocid == userId));
                predicateResult.Or(predicateInner);
            }
            else if (dto?.IsSubdomainSpoc == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(f => f.Subdomainspocid == userId));
                predicateResult.And(predicateInner);
            }
            else if (dto?.IsEduSpoc == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                predicateInner.Or(x => x.Lcmengineeringeduspoc.Any(f => f.Eduspocid == userId));
                predicateResult.And(predicateInner);
            }
            if (opCoId?.Any() == true && ruleId.Any(f => f != 1))
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in opCoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
       
            if (isAchive == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(f => f.Plannedcompletion < today && f.Plannedcompletion > preferenceDate));
                predicateResult.And(predicateInner);
            }
            //if (isAchive == false && calculateMonthRange != 0)
            //{
            //    predicateInner = PredicateBuilder.New<Lcmengineering>();
            //    predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(f => f.Plannedcompletion > today && f.Plannedcompletion < preferenceDate));
            //    predicateResult.And(predicateInner);
            //}
            if (SwPlannedActivityRulesId != null && SwPlannedActivityRulesId.Count > 0)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                predicateInner.Or(x => x.PlannedactivitiesLcmengineering.All(p => SwPlannedActivityRulesId.Contains((short)p.Plannedactivityresourceid)));
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
 
        #endregion

        #region // Doing Section for Eom & Eos

        public async Task<List<DoingSectionForEomAnsEos>> FindByConditionForDoingSectionForEosAndEom(int userId)
        {
            try
            {
                var result = new List<DoingSectionForEomAnsEos>();

                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetails(userId);
                var userOrgDetails = await _authorizedRoleManager.GetOrganisationSpocsDetails(userId);

                if ((userOrgDetails.IsEduSpoc == true || userOrgDetails.IsSubdomainSpoc == true) && userOrgDetails.IsUserExistInOrg == true)
                {

                    //#1913 - Display all the records for Admin
                    if (userDetailsList?.UserRoleId.Any(f => f == 1) == true) userOrgDetails = null;

                        var prdecateResult = ApplyFilterForSwPARecords(userId, userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                            userDetailsList.OpcoDetails : new List<short>(),
                        userDetailsList.UserRoleId, 0, false, userOrgDetails,null);

                    #region no need date calculation
                    var MessagingDateRange = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName && f.Messagingdate != null).Select(r => r.Messagingdate).FirstOrDefaultAsync();

                    if (MessagingDateRange != null)
                    {
                        messagingDateRange = MessagingDateRange.Value;                         
                    }
                    else
                    {
                        messagingDateRange = ConstantValueFilter.MessagingDate;
                    } 
                    messagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(-messagingDateRange);
                    #endregion 
 

                    // Product Importance - Strategic and Non-Strategic #1913 Aprl 01 2026 
                    var lcmProductImportanaceId = _repositoryWrapper.ProductImportance.FindByCondition(x => x.Productimportance.ToLower() == ConstantValueFilter.ProdImportanceStrategic).FirstOrDefault()?.Productimportanceid;
                    int designContactId = userOrgDetails == null ? 0 : userId;
                   var data = await _repositoryWrapper.Lcmengineering
            .FindByCondition(prdecateResult)
                        .Where(x => designContactId == 0 || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any(t => t.Designcontactid == designContactId)
                        /*&&
                (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance > messagingDate && x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance < today)
                ||
               (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport > messagingDate && x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport < today)*/
            ) .AsNoTracking()
            .Select(x => new
            {
                Lcm = x.Lcmengineeringid,
                Opco = x.Opco.Opco,
                Product = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description,
                Oem = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer,
                Version = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion,
                Eom = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance,
                Eos = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport,
                MajorSwId = x.Designcomponent.Systemtype.Majorsoftwarebuildsid,
                MajorSoftware = x.Designcomponent.Systemtype.Majorsoftwarebuilds,
                majordesign = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts,
                IsLcmProductImportanceStrategic = x.Productimportanceid == lcmProductImportanaceId ? true : false,

                //ProductWithOem = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description+"-"+x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer
            })?.Where(y => y.Eom >messagingDate && y.Eom <today || y.Eos > messagingDate && y.Eos<today).OrderBy(x => x.Eom)
            .ToListAsync();

                    //#1913 - Display all the records for Admin
                    if (userDetailsList?.UserRoleId.Any( f => f != 1) == true)
                    data = data.Where(f => f.majordesign.Any(l => l.Designcontactid == userId)).ToList();

                   // var ragStatus = CalculateRagStatus(data.Select(x => x.MajorSoftware).ToList());

                    result = data
                        .GroupBy(x => new {x.Product,x.Oem.Originalequipmentmanufacturer })
                        //.GroupBy(x => x.ProductWithOem)
                       .Select(productGroup =>
                       {
                           //var colour = productGroup
                           //     .Select(pa =>
                           //         ragStatus.TryGetValue(pa.MajorSoftware.Majorsoftwarebuildsid, out var rag)
                           //             ? rag
                           //             : string.Empty)
                           //     .FirstOrDefault();

                           return new DoingSectionForEomAnsEos
                           {
                               //Product = $"{productGroup.Select(r => r.Oem.Originalequipmentmanufacturer).FirstOrDefault()}-{productGroup.Key}",
                               Product = $"{productGroup.Key.Product}-{productGroup.Key.Originalequipmentmanufacturer}",
                               Versions = productGroup
                                    .GroupBy(v => v.Version)
                                    .Select(versionGroup => new
                                    {
                                        VersionGroup = versionGroup,
                                        Eom = versionGroup.Select(x => x.Eom).FirstOrDefault(),
                                        Eos = versionGroup.Select(x => x.Eos).FirstOrDefault()
                                    })
                                    .OrderBy(x => x.Eom == null).ThenBy(x => x.Eom)
                                    .Select(x => new VersionDto
                                    {
                                        Version = x.VersionGroup.Key,
                                        MajorSwId = (long)x.VersionGroup.Select(v => v.MajorSwId).FirstOrDefault(),
                                        OpCo = string.Join(",", x.VersionGroup.Select(v => v.Opco).Distinct()),
                                        EomDate = x.Eom?.ToString("dd-MM-yyyy"),
                                        EosDate = x.Eos?.ToString("dd-MM-yyyy"),
                                    })
                                    .ToList(),
                              // RagStatus = colour,
                           };
                       })
                       .ToList();

                }

                return result;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
 
        #endregion

        #region  // Achievements
       
        public async Task<List<PlanndActivitySoftwareUpgradDetailsDtoGrid>> FindWithConditionForAchievementRecordsOld(int userId)
        {
            var result = new List<PlanndActivitySoftwareUpgradDetailsDtoGrid>();
            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetails(userId);

                var fsi_deliveryStatusId = (short)_repositoryWrapper.DeliveryStatus.FindByCondition(p => p.Deliverystatus.ToLower().Replace(" ", "") == ConstantValueFilter.FSIAchieved
                )?.Select(p => p.Deliverystatusid)?.FirstOrDefault();

                var SwPlannedActivityRulesId = await _repositoryWrapper.PlannedActivityResourceRepository
                 .FindByCondition(x => x.Rulelinkeddc == (int)PlannedActivityResourceEnum.SwArchitectureUpgrade_SwMajorRelease).AsNoTracking().Select(x =>
                  x.Plannedactivityresourceid).ToListAsync();

                var predicateResult = ApplyFilterForSwPARecords(userId, userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                    userDetailsList.OpcoDetails : new List<short>(),
                userDetailsList.UserRoleId, -3, true, null, SwPlannedActivityRulesId);

                var query = await GetPlannedSWRecors(predicateResult);
                var MessagingDateRange = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName && f.Messagingdate != null).Select(r => r.Messagingdate).FirstOrDefaultAsync();

                if (MessagingDateRange != null)
                {
                    messagingDateRange = MessagingDateRange.Value;
                }
                else
                {
                    messagingDateRange = ConstantValueFilter.MessagingDate;
                }

                messagingDate = new DateTime(today.Year, today.Month, 1).AddMonths(-messagingDateRange);

                var archivePaEntity = query.Where(x => x.PlannedactivitiesLcmengineering != null && x.PlannedactivitiesLcmengineering.Count > 0).SelectMany(lcm => lcm.PlannedactivitiesLcmengineering)
                .Where(p => p.Plannedcompletion < today && p.Plannedcompletion > messagingDate && p.Archived == true && SwPlannedActivityRulesId.Contains((short)p.Plannedactivityresourceid))?.ToList();

                var fsiPaEntity = query.Where(x => x.PlannedactivitiesLcmengineering != null && x.PlannedactivitiesLcmengineering.Count > 0).SelectMany(lcm => lcm.PlannedactivitiesLcmengineering)
                .Where(p => p.Plannedcompletion < today && p.Plannedcompletion > messagingDate && p.Deliverystatusid == fsi_deliveryStatusId && SwPlannedActivityRulesId.Contains((short)p.Plannedactivityresourceid))?.ToList(); ;

                if (archivePaEntity?.Any() == true && fsiPaEntity != null && fsiPaEntity.Count > 0)
                {
                    archivePaEntity = archivePaEntity.Union(fsiPaEntity)?.ToList();
                }
                else if (archivePaEntity?.Any() == false && fsiPaEntity != null && fsiPaEntity.Count > 0)
                {
                    archivePaEntity = fsiPaEntity?.ToList();
                }

                result = archivePaEntity
                   .GroupBy(pa => pa.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Productnameid)
                   .Select(g => new PlanndActivitySoftwareUpgradDetailsDtoGrid
                   {
                       Oem = g.FirstOrDefault()?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,

                       Product = g.FirstOrDefault()
                                  ?.Designcomponent
                                  .Systemtype
                                  .Majorsoftwarebuilds
                                  .Productname
                                  .Description,

                       PlannedAction = g?.OrderBy(pa => pa.Plannedcompletion == null).ThenBy(pa => pa.Plannedcompletion).Select(pa => new PaPlannedActionDto
                       {
                           Lcmengineeringid = pa.Lcmengineeringid,
                           PaId = pa.Plannedactivityid,
                           Plannedcompletion = (pa.Deliverystatusid == fsi_deliveryStatusId) ? pa.Plannedcompletion : null,
                           ActionDetail =
                                       $"{pa.Opco.Opco} | " +
                                       $"{pa.Plannedcompletion:MMM} - " +
                                       $"{pa.Plannedcompletion:yyyy} | " +
                                       $"{pa.Plannedactivityresource?.Plannedactivityresource} | " +
                                       $"{pa.Deliverystatus?.Deliverystatus}"
                       }).ToList()
                   })
                   .ToList();

                return result;

            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

        #region // Sign Post 
       
        public async Task<List<PlanndActivitySoftwareUpgradDetailsDtoGrid>> FindWithConditionForSignPostPARecordsOld(int userId)
        {
            var result = new List<PlanndActivitySoftwareUpgradDetailsDtoGrid>();
            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetails(userId);
                var SwPlannedActivityRulesId = await _repositoryWrapper.PlannedActivityResourceRepository
                .FindByCondition(x => x.Rulelinkeddc == (int)PlannedActivityResourceEnum.SwArchitectureUpgrade_SwMajorRelease).AsNoTracking().Select(x =>
                x.Plannedactivityresourceid).ToListAsync();

                var predicateResult = ApplyFilterForSwPARecords(userId, userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                    userDetailsList.OpcoDetails : new List<short>(),
                userDetailsList.UserRoleId, 3, false, null, SwPlannedActivityRulesId);

                var query = await GetPlannedSWRecors(predicateResult);

                var plannedActivityRules = await _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x => x.Rulelinkeddc == (int)
                                          PlannedActivityResourceEnum.SwArchitectureUpgrade_SwMajorRelease).AsNoTracking().Select(x => x.Plannedactivityresourceid).ToListAsync();

                var MessagingDateRange = await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName && f.Messagingdate != null).Select(r => r.Messagingdate).FirstOrDefaultAsync();

                if (MessagingDateRange != null)
                {
                    MessagingDateRange = MessagingDateRange.Value;

                }
                else
                {
                    MessagingDateRange = ConstantValueFilter.MessagingDate;
                }
                var startDate = today.AddMonths((int)MessagingDateRange);
                startDate = new DateTime(startDate.Year, startDate.Month, 1);
                messagingDate = startDate.AddMonths(1).AddDays(-1);

                result = query?.Where(x => x.Archived == false && (x.PlannedactivitiesLcmengineering != null && x.PlannedactivitiesLcmengineering.Count > 0))
                  .SelectMany(lcm => lcm?.PlannedactivitiesLcmengineering.Select(x1 => new { lcm, x1 }))
                  .Where(p => p?.x1?.Plannedcompletion > today && p?.x1?.Plannedcompletion < messagingDate)
                  .GroupBy(pa => pa?.x1?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname?.Productnameid)
                  .Select(g => new PlanndActivitySoftwareUpgradDetailsDtoGrid
                  {
                      Oem = g.FirstOrDefault()?.x1.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,

                      Product = g.FirstOrDefault()
                                 ?.x1.Designcomponent
                                 .Systemtype
                                 .Majorsoftwarebuilds
                                 .Productname
                                 .Description,

                      PlannedAction = g?.OrderBy(pa => pa?.x1?.Plannedcompletion == null).ThenBy(pa => pa?.x1?.Plannedcompletion).Select(pa => new PaPlannedActionDto
                      {
                          Lcmengineeringid = pa?.x1?.Lcmengineeringid,
                          PaId = pa.x1.Plannedactivityid,
                          Plannedcompletion = pa?.x1?.Plannedcompletion,
                          ActionDetail =
                                      $"{pa.x1.Opco.Opco} | " +
                                      $"{pa.x1.Plannedcompletion:MMM} - " +
                                      $"{pa.x1.Plannedcompletion:yyyy} | " +
                                      $"{pa.x1.Plannedactivityresource?.Plannedactivityresource} | " +
                                      $"{pa.x1.Deliverystatus?.Deliverystatus}",           
                          PlannedDcName = DesignComponentTypeExtensionMethod.ToDesignComponentNameHomePage(pa.x1.Designcomponent, _repositoryWrapper),
                          CurrentDcName = DesignComponentTypeExtensionMethod.ToDesignComponentNameHomePage(pa.lcm.Designcomponent, _repositoryWrapper),
                      }).ToList()
                  })
                  .ToList();

                return result;

            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }
        #endregion

    }

    public class ReportLatestProcessTimeDto
    {
        public string ReportName { get; set; }
        public DateTime? Processstarttime { get; set; }
    }
}
