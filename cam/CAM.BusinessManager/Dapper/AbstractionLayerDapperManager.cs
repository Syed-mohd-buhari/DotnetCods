using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Repository;
using CAM.Repository.Helpers;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.AbstractionLayer
{
    public class AbstractionLayerDapperManager : BaseManager
    {
        IRepositoryWrapper _repositoryWrapper;
        AuthorizedRoleManager _authorizedRoleManager;
        private readonly LCMPADapperQueries _lCMPADapperQueries;
        private readonly CommonDapperRepository _commonDapperRepository;
        private readonly CommonManager _commonManager;
        private DateTime currentDate = System.DateTime.Now.Date;
        private static DateTime today = DateTime.Today;      
        private DateTime? preferenceDate = null;    
        private DateTime? messagingDate = null;
        private readonly ILoggerManager _loggerManager;
        private readonly string dapperDatabaseMode = "normal";
        private readonly DapperCommonManager _dapperCommonManager;

        public AbstractionLayerDapperManager(IEnumerable<IRepositoryWrapper> wrappers, AuthorizedRoleManager authorizedRoleManager, ILoggerManager loggerManager,
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonDapperRepository commonDapperRepository, LCMPADapperQueries lCMPADapperQueries,
           CommonManager commonManager, DapperCommonManager dapperCommonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _authorizedRoleManager = authorizedRoleManager;
            _lCMPADapperQueries = lCMPADapperQueries;
            _loggerManager = loggerManager;
            _commonDapperRepository = commonDapperRepository;
            _commonManager = commonManager;
            dapperDatabaseMode = GlobalDbMode.DbMode.ContainsKey(CurrentLogLevelConfig._UserName) ? GlobalDbMode.DbMode[CurrentLogLevelConfig._UserName] : "normal";
            _dapperCommonManager = dapperCommonManager;
        }
        private async Task<(string Query,DateTime preferenceDate)> ApplyFilterForSwPARecords(string sqlQuery, long userId,
                List<short> opCoId,
                List<int> ruleId,
                int calculateMonthRange = 0,
                bool isArchive = false,
                OrganisatioinSpocsDto orgDto = null,
                List<int> swPlannedActivityRulesId = null,
                List<int> verticalId = null, bool isDesignContactCheck = false
                )
        {
            try
            {
                var today = DateTime.UtcNow.Date;

                var preferenceDate = new DateTime(
                    today.Year,
                    today.Month,
                    1).AddMonths(calculateMonthRange);

                var sql = new StringBuilder();
 
                sql.Append(sqlQuery);
                await Task.Run(() =>
                {

                    /*---------------------------------- User Base FILTER  ----------------------------------*/

                    sql.Append(@"WHERE
                            (
                                 le.deleted = 0     AND LOWER(op.OPCO) <> 'zzz'
                         ");


                    /*---------------------------------- BASE FILTER  ----------------------------------*/

                    if (!isArchive)
                    {
                        // sql.Append(@" AND le.ARCHIVED = 0 "); // we are using single method to use all api
                    }

                    /*----------------------------------  VERTICAL FILTER  ----------------------------------*/
                    if (verticalId?.Any() == true)
                    {
                        sql.Append($"  AND EXISTS  ( SELECT 1  FROM LCMENGINEERINGSUBDOMAINSPOC lsp  INNER JOIN ASPNETUSERVERTICALS auv   ON auv.USERID = lsp.SUBDOMAINSPOCID   INNER JOIN ORGANISATION org  ON org.ORGANISATIONID = auv.ORGANISATIONID                                     WHERE lsp.LCMENGINEERINGID = le.LCMENGINEERINGID   AND auv.DELETED = 0    AND org.VERTICALID IN ({string.Join(",", verticalId.Select((x, i) => $"{x}"))})                                ) ");
 
                    }

                    /*----------------------------------  SUBDOMAIN + EDU SPOC FILTER  ----------------------------------*/
                    if (orgDto?.IsSubdomainSpoc == true &&
                        orgDto?.IsEduSpoc == true)
                    {
                        sql.Append(@"

                            AND
                            (
                                EXISTS
                                (
                                    SELECT 1
                                    FROM LCMENGINEERINGSUBDOMAINSPOC sd
                                    WHERE sd.LCMENGINEERINGID = le.LCMENGINEERINGID
                                      AND sd.SUBDOMAINSPOCID = :UserId
                                )

                                OR

                                EXISTS
                                (
                                    SELECT 1
                                    FROM LCMENGINEERINGEDUSPOC edu
                                    WHERE edu.LCMENGINEERINGID = le.LCMENGINEERINGID
                                      AND edu.EDUSPOCID = :UserId
                                )
                            )

                            ");
 
                    }
                    else if (orgDto?.IsSubdomainSpoc == true)
                    {
                        sql.Append(@"

                            AND EXISTS
                            (
                                SELECT 1
                                FROM LCMENGINEERINGSUBDOMAINSPOC sd
                                WHERE sd.LCMENGINEERINGID = le.LCMENGINEERINGID
                                  AND sd.SUBDOMAINSPOCID = :UserId
                            )

                            ");

                        
                    }
                    else if (orgDto?.IsEduSpoc == true)
                    {
                        sql.Append(@"

                                AND EXISTS
                                (
                                    SELECT 1
                                    FROM LCMENGINEERINGEDUSPOC edu
                                    WHERE edu.LCMENGINEERINGID = le.LCMENGINEERINGID
                                      AND edu.EDUSPOCID = :UserId
                                )

                                ");
 
                    }

                    /*----------------------------------   OPCO FILTER     ----------------------------------*/
                    if (opCoId?.Any() == true &&
                        ruleId?.Any(x => x != 1) == true)
                    {

                        sql.Append($" AND le.OPCOID IN ({string.Join(",", opCoId.Select((x, i) => $"{x}"))})");
 
                    }

                    /*----------------------------------   ARCHIVE DATE FILTER  ----------------------------------*/
                    if (isArchive)
                    {
                        //sql.Append(@"

                        //    AND EXISTS
                        //    (
                        //        SELECT 1
                        //        FROM PLANNEDACTIVITIES pa2
                        //        WHERE pa2.LCMENGINEERINGID = le.LCMENGINEERINGID
                        //      AND pa2.PLANNEDCOMPLETION < TO_TIMESTAMP(:Today, 'MM/DD/YYYY')
                        //        AND pa2.PLANNEDCOMPLETION > TO_TIMESTAMP(:PreferenceDate, 'MM/DD/YYYY')
                        //    )

                        //    ");
 
                    }

                    /*----------------------------------   Software planned activity records only  FILTER      ----------------------------------*/
                    if (swPlannedActivityRulesId?.Any() == true)
                    {
                        sql.Append($@"
                                AND EXISTS
                                (
                                    SELECT 1
                                    FROM PLANNEDACTIVITIES pa3
                                    WHERE pa3.LCMENGINEERINGID = le.LCMENGINEERINGID
                                      AND pa3.PLANNEDACTIVITYRESOURCEID IN ({string.Join(",", swPlannedActivityRulesId)})
                                )
                            ");                        
                        
                    }

                    if (userId != 0 && !isDesignContactCheck)
                    {
                        sql.Append(@" 
                            
                                OR (EXISTS (
                                    SELECT 1
                                    FROM LCMENGINEERINGSUBDOMAINSPOC sd
                                    WHERE sd.LCMENGINEERINGID = le.LCMENGINEERINGID
                                      AND sd.SUBDOMAINSPOCID = :userId
                                )

                                OR EXISTS (
                                    SELECT 1
                                    FROM LCMENGINEERINGEDUSPOC edu
                                    WHERE edu.LCMENGINEERINGID = le.LCMENGINEERINGID
                                      AND edu.EDUSPOCID = :userId ))
                              
                        ");
                    }
                    else if (userId != 0 && isDesignContactCheck)
                    {
                        sql.Append(@" 
                            
                                or (EXISTS (
                                     SELECT 1
                                    FROM designcomponents dc
                                         JOIN SystemTypes st
                                             ON st.SystemTypeId = dc.SystemTypeId
                                         JOIN MajorSoftwareBuilds msb
                                             ON msb.MAJORSOFTWAREBUILDSID = st.MAJORSOFTWAREBUILDSID
                                         JOIN MajorSwBuildsDesignContacts msbdc
                                             ON msbdc.MAJORSOFTWAREBUILDSID = msb.MAJORSOFTWAREBUILDSID
                                    WHERE msbdc.DesignContactId = :userId
                                ) )
                              
                        ");
                    }
                    /*----------------------------------  ORDER BY   ----------------------------------*/
                    sql.Append(@"  
                            )
                            ");
                });
                return (sql.ToString(),  preferenceDate);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }

        }

        
        #region  // Achievements
        public async Task<ResultDto> GetAchievementRecordsAsync(long userId)
        {
            var result = new List<PlanndActivitySoftwareUpgradDetailsDtoGrid>();
            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId,true);
                var achivementResult = new List<PlanndActivitySoftwareUpgradDetailsDtoGrid>();
                if (userDetailsList?.RoleRecords?.Any(x =>
                              string.Equals(x.RoleName, "SW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = new
                        {
                            AchivementRecords = achivementResult =  null,
                            
                        }
                    };
                }


                var fsiDeliveryStatusTask =
                             _commonDapperRepository.QueryFirstOrDefaultAsync<int>(
                                 dapperDatabaseMode,
                                 @"
                                        SELECT Deliverystatusid
                                        FROM deliverystatuses
                                        WHERE LOWER(REPLACE(Deliverystatus, ' ', '')) = :FSIAchieved
                                        FETCH FIRST 1 ROW ONLY",
                                 new
                                 {
                                     FSIAchieved = ConstantValueFilter.FSIAchieved
                                         .ToLower()
                                         .Replace(" ", "")
                                 });

                var swPlannedActivityRulesTask = _dapperCommonManager.GetSwPlannedActivityRulesAsync();

                var getGridCustomColumns = _dapperCommonManager.GetGridCustomColumnsAsync(userId);                                   

                await Task.WhenAll(
                    fsiDeliveryStatusTask,
                    swPlannedActivityRulesTask,
                    getGridCustomColumns);

                var fsiDeliveryStatusId = await fsiDeliveryStatusTask;
                var swPlannedActivityRulesIds = (await swPlannedActivityRulesTask).ToList();

                var messagingDateRangeTask = getGridCustomColumns.Result.Where(x => x.Messagingdate != null).FirstOrDefault();                 
                var messagingDateRange =  messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;
                messagingDate = new DateTime(today.Year, today.Month, 1).AddMonths((int)-messagingDateRange);

                var verticalIds = userDetailsList.VerticalDetails != null && userDetailsList.VerticalDetails.Count > 0 ?
                    userDetailsList.VerticalDetails : new List<int>();

                var opCoIds = userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                userDetailsList.OpcoDetails : new List<short>();

                var baseSqlQuery = _lCMPADapperQueries.GetLcmPaQueryForAbstraction();

                var (query, preferenceDate) = await ApplyFilterForSwPARecords(baseSqlQuery, 0, opCoIds,
                userDetailsList.UserRoleId, (int)-messagingDateRange, true, null, swPlannedActivityRulesIds, verticalIds);

                var lcmPaEntities = await _commonDapperRepository.QueryAsync<LcmPlannedActivityDapperDto>(
                               dapperDatabaseMode, query, new
                               {
                                   userId  = 0,//Sw owner 
                                   VerticalIds = verticalIds?.ToArray(),
                                   OpcoIds = opCoIds?.ToArray(),
                                   SwPlannedActivityRulesId = swPlannedActivityRulesIds?.ToArray(),
                                   Today = today,
                                   PreferenceDate = preferenceDate,
                                   ProprietaryHW = (int)BuildconstructionRuleEnum.ProprietaryHW,
                                   CotsHW = (int)BuildconstructionRuleEnum.CotsHW,
                               });


                var archivePaEntity = lcmPaEntities
                    .Where(x =>   x.PlannedActivityId != null   &&
                        x.PlannedCompletion < today &&
                        x.PlannedCompletion > messagingDate &&
                        swPlannedActivityRulesIds.Contains(
                            x.PlannedActivityResourceId ?? 0) &&
                        (
                            x.PaArchived == true ||
                            x.PaDeliveryStatusId == fsiDeliveryStatusId
                        ))
                    .Distinct()
                    .ToList();


                result = archivePaEntity
                    .GroupBy(pa => pa.PaProductId)
                         .Select(g => new PlanndActivitySoftwareUpgradDetailsDtoGrid
                         {
                             Oem = g.FirstOrDefault()?.PaOem,

                             Product = g.FirstOrDefault()
                                  ?.PaProductName,
                             PlannedAction = g?.OrderBy(pa => pa.PlannedCompletion == null).ThenBy(pa => pa.PlannedCompletion).Select(pa => new PaPlannedActionDto
                             {
                                 Lcmengineeringid = pa.LcmEngineeringId,
                                 PaId = (long)pa.PlannedActivityId,
                                 Plannedcompletion = (pa.PaDeliveryStatusId == fsiDeliveryStatusId) ? pa.PlannedCompletion : null,
                                 ActionDetail =
                                                                    $"{pa.Opco} | " +
                                                                    $"{pa.PlannedCompletion:MMM} - " +
                                                                    $"{pa.PlannedCompletion:yyyy} | " +
                                                                    $"{pa.PlannedActivityResource} | " +
                                                                    $"{pa.PaDeliveryStatus}",

                             }).ToList()

                         })
                         .ToList();

                var preferenceDetails = getGridCustomColumns.Result.FirstOrDefault();// await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName).FirstOrDefaultAsync();
               
                return new ResultDto
                {
                    Data = new
                    {
                        AchivementRecords = result,
                        WhatsGoingOnDate = preferenceDetails != null && preferenceDetails?.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                        MessagingDate = preferenceDetails != null && preferenceDetails?.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,


                    }
                };

            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        #endregion
        #region  //SignPostRecords
        public async Task<ResultDto> GetSignPostRecordsAsync(long userId)
        {
            var result = new List<PlanndActivitySoftwareUpgradDetailsDtoGrid>();
            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId);


                var swPlannedActivityRulesTask = _dapperCommonManager.GetSwPlannedActivityRulesAsync();
                var getGridCustomColumns = _dapperCommonManager.GetGridCustomColumnsAsync(userId);

                await Task.WhenAll(
                    swPlannedActivityRulesTask,
                    getGridCustomColumns);

                var swPlannedActivityRulesIds = (await swPlannedActivityRulesTask).ToList();

                var messagingDateRangeTask = getGridCustomColumns.Result.Where(x => x.Messagingdate != null).FirstOrDefault();
                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate; 

                var startDate = today.AddMonths((int)messagingDateRange);
                startDate = new DateTime(startDate.Year, startDate.Month, 1);
                messagingDate = startDate.AddMonths(1).AddDays(-1);

                var verticalIds = userDetailsList.VerticalDetails != null && userDetailsList.VerticalDetails.Count > 0 ?
                    userDetailsList.VerticalDetails : new List<int>();
                var opCoIds = userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                userDetailsList.OpcoDetails : new List<short>();

                var baseSqlQuery = _lCMPADapperQueries.GetLcmPaQueryForAbstractionName();

                var (query, PreferenceDate) = await ApplyFilterForSwPARecords(baseSqlQuery, userId, opCoIds,
                userDetailsList.UserRoleId, (int)messagingDateRange, false, null, swPlannedActivityRulesIds, null);

                var lcmPaEntities = await _commonDapperRepository.QueryAsync<LcmPlannedActivityDapperDto>(
                               dapperDatabaseMode, query, new
                               {
                                   userId,
                                   VerticalIds = verticalIds?.ToArray(),
                                   OpcoIds = opCoIds?.ToArray(),
                                   SwPlannedActivityRulesId = swPlannedActivityRulesIds?.ToArray(),
                                   Today = today,
                                   PreferenceDate = preferenceDate,
                                   ProprietaryHW = (int)BuildconstructionRuleEnum.ProprietaryHW,
                                   CotsHW = (int)BuildconstructionRuleEnum.CotsHW,

                               });


                result = lcmPaEntities?.Where(x => x.LcmArchived == false && x.PaArchived == false && x.PlannedActivityId != null && x.PlannedCompletion > today && x.PlannedCompletion < messagingDate)
                 .GroupBy(pa => pa.PaProductId)
                 .Select(g => new PlanndActivitySoftwareUpgradDetailsDtoGrid
                 {
                     Oem = g.FirstOrDefault()?.PaOem,

                     Product = g.FirstOrDefault()
                                ?.PaProductName,

                     PlannedAction = g?.OrderBy(pa => pa?.PlannedCompletion == null).ThenBy(pa => pa?.PlannedCompletion).Select(pa => new PaPlannedActionDto
                     {
                         Lcmengineeringid = pa?.LcmEngineeringId,
                         PaId = (long)(pa?.PlannedActivityId),
                         Plannedcompletion = pa?.PlannedCompletion,
                         ActionDetail =
                                     $"{pa.Opco} | " +
                                     $"{pa?.PlannedCompletion:MMM} - " +
                                     $"{pa?.PlannedCompletion:yyyy} | " +
                                     $"{pa?.PlannedActivityResource} | " +
                                     $"{pa?.PaDeliveryStatus}",
                         PlannedDcName = pa.PaDcName,
                         CurrentDcName = pa.LcmDcName,
                     }).ToList()
                 })
                 .ToList();

                var preferenceDetails = getGridCustomColumns.Result.FirstOrDefault();// await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName).FirstOrDefaultAsync();
 
                return new ResultDto
                {
                    Data = new
                    {
                        DoingSectioinForPA = result,
                        WhatsGoingOnDate = preferenceDetails != null && preferenceDetails?.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                        MessagingDate = preferenceDetails != null && preferenceDetails?.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                    }
                };

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
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId);

                var swPlannedActivityRulesTask = _dapperCommonManager.GetSwPlannedActivityRulesAsync();
                var getGridCustomColumns = _dapperCommonManager.GetGridCustomColumnsAsync(userId);

                //Messagingdate
                await Task.WhenAll(
                    swPlannedActivityRulesTask,
                    getGridCustomColumns);

                var messagingDateRangeTask = getGridCustomColumns.Result.Where(x => x.Messagingdate != null).FirstOrDefault();


                var swPlannedActivityRulesIds = (await swPlannedActivityRulesTask).ToList();

                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate; 
                var startDate = today.AddMonths((int)-messagingDateRange);
                startDate = new DateTime(startDate.Year, startDate.Month, 1);
                messagingDate = startDate;

                var userOrgDetails = await _authorizedRoleManager.GetOrganisationSpocsDetails(userId);

                if ((userOrgDetails.IsEduSpoc == true || userOrgDetails.IsSubdomainSpoc == true) && userOrgDetails.IsUserExistInOrg == true
                    || userDetailsList?.UserRoleId.Any(f => f == 1) == true)
                {

                    //#1913 - Display all the records for Admin
                    if (userDetailsList.UserRoleId.Any(f => f == 1) == true) userOrgDetails = null;


                    var baseSqlQuery = _lCMPADapperQueries.GetLcmPaQueryForAbstractionName();

                    var verticalIds = userDetailsList.VerticalDetails != null && userDetailsList.VerticalDetails.Count > 0 ?
                    userDetailsList.VerticalDetails : new List<int>();
                    var opCoIds = userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                    userDetailsList.OpcoDetails : new List<short>();


                    var (query,  PreferenceDate) = await ApplyFilterForSwPARecords(baseSqlQuery, userOrgDetails == null ? 0 : userId, opCoIds,
                     userDetailsList.UserRoleId, 0, false, userOrgDetails, swPlannedActivityRulesIds, null);

                    var lcmPaEntities = await _commonDapperRepository.QueryAsync<LcmPlannedActivityDapperDto>(
                             dapperDatabaseMode, query, new
                             {
                                 userId,
                                 VerticalIds = verticalIds?.ToArray(),
                                 OpcoIds = opCoIds?.ToArray(),
                                 SwPlannedActivityRulesId = swPlannedActivityRulesIds?.ToArray(),
                                 Today = today,
                                 PreferenceDate = preferenceDate,
                                 ProprietaryHW = (int)BuildconstructionRuleEnum.ProprietaryHW,
                                 CotsHW = (int)BuildconstructionRuleEnum.CotsHW,

                             });

                    var ressult = lcmPaEntities.Where(x => x.LcmArchived == false && x.PaArchived == false && x.PlannedActivityId != null &&
                 x.PlannedCompletion > messagingDate && x.PlannedCompletion < today).ToList();


                    result = lcmPaEntities.Where(x => x.LcmArchived == false && x.PaArchived == false && x.PlannedActivityId != null &&
                    x.PlannedCompletion > messagingDate && x.PlannedCompletion < today).GroupBy(g => g.PaProductId)
                           .Select(g =>
                           {
                               return new DoingSectioinForPA
                               {
                                   Oem = g.FirstOrDefault()?.PaOem,

                                   Product = g.FirstOrDefault()
                                                               ?.PaProductName,

                                   PlannedAction = g?.OrderBy(o => o.PlannedCompletion == null).ThenBy(to => to.PlannedCompletion).Select(s => new PaPlannedActionDto
                                   {
                                       Lcmengineeringid = s.LcmEngineeringId,
                                       PaId = (long)s.PlannedActivityId,
                                       Plannedcompletion = s.PlannedCompletion,
                                       ActionDetail =
                                                            $"{s.Opco} | " +
                                                            $"{s.PlannedCompletion:MMM} - " +
                                                            $"{s.PlannedCompletion:yyyy} | " +
                                                            $"{s.PlannedActivityResource} | " +
                                                            $"{s.PaDeliveryStatus}",
                                       PlannedDcName = s.PaDcName,
                                       CurrentDcName = s.LcmDcName,
                                   }).ToList(),

                               };
                           })?.OrderBy(x => x.Oem)
                            .ToList();

                }

                var preferenceDetails = getGridCustomColumns.Result.FirstOrDefault();// await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName).FirstOrDefaultAsync();

                return new ResultDto
                {
                    Data = new
                    {
                        DoingSectioinForPA = result,
                        WhatsGoingOnDate = preferenceDetails != null && preferenceDetails?.Preferencedate != null ? preferenceDetails.Preferencedate : ConstantValueFilter.NewPortal_WhatsGoingOnDate,
                        MessagingDate = preferenceDetails != null && preferenceDetails?.Messagingdate != null ? preferenceDetails.Messagingdate : ConstantValueFilter.NewPortal_MessagingDate,
                    }
                };
            }
            catch (Exception ex)
            {
                _loggerManager.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<List<DoingSectionForEomAnsEos>> FindByConditionForDoingSectionForEosAndEom(int userId)
        {
            try
            {
                var result = new List<DoingSectionForEomAnsEos>();

                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId);
                var getGridCustomColumns = _dapperCommonManager.GetGridCustomColumnsAsync(userId);

                //Messagingdate
                await Task.WhenAll(                    
                    getGridCustomColumns);

                var messagingDateRangeTask = getGridCustomColumns.Result.Where(x => x.Messagingdate != null).FirstOrDefault();
                var messagingDateRange = messagingDateRangeTask?.Messagingdate ?? ConstantValueFilter.NewPortal_MessagingDate;
                messagingDate = new DateTime(today.Year, today.Month, 1).AddMonths((int)-messagingDateRange);

                var userOrgDetails = await _authorizedRoleManager.GetOrganisationSpocsDetails(userId);

                if ((userOrgDetails.IsEduSpoc == true || userOrgDetails.IsSubdomainSpoc == true) && userOrgDetails.IsUserExistInOrg == true)
                {

                    var lcmProductImportanaceRecord =
                                  _commonDapperRepository.QueryFirstOrDefaultAsync<int?>(
                                      dapperDatabaseMode,
                                      @"
                                            SELECT Productimportanceid
                                            FROM productimportances
                                             WHERE LOWER(REPLACE(Productimportance, ' ', '')) = :Productimportance
                                            FETCH FIRST 1 ROW ONLY",
                                      new
                                      {
                                          Productimportance = ConstantValueFilter.ProdImportanceStrategic
                                      });

                    // Product Importance - Strategic and Non-Strategic #1913 Aprl 01 2026 
                    var lcmProductImportanaceId = await lcmProductImportanaceRecord;


                    //#1913 - Display all the records for Admin
                    if (userDetailsList?.UserRoleId.Any(f => f == 1) == true) userOrgDetails = null;

                    var baseSqlQuery = _lCMPADapperQueries.GetLcmQueryFoEomAndEos();

                    var verticalIds = userDetailsList.VerticalDetails != null && userDetailsList.VerticalDetails.Count > 0 ?
                       userDetailsList.VerticalDetails : new List<int>();
                    var opCoIds = userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                    userDetailsList.OpcoDetails : new List<short>();

                    int designContactId = userOrgDetails == null ? 0 : userId;

                    var (query,  PreferenceDate) = await ApplyFilterForSwPARecords(baseSqlQuery, designContactId, opCoIds,
                userDetailsList.UserRoleId, 0, false, userOrgDetails, null, null,true);

                    string groupByQuery =@"GROUP BY le.LCMENGINEERINGID, 
                                                  op.OPCO, le.deleted, le.archived, lpn.DESCRIPTION, 
                                                        loem.ORIGINALEQUIPMENTMANUFACTURER, lmsw.SOFTWAREVERSION,
                                                    lmsw.ENDOFMAINTENANCE, lmsw.ENDOFSUPPORT, lmsw.MAJORSOFTWAREBUILDSID, le.PRODUCTIMPORTANCEID";

                    query = query + groupByQuery;
                    var lcmEomAndEosEntities = await _commonDapperRepository.QueryAsync<LcmBasedEomEosDapperDto>(
                             dapperDatabaseMode, query, new
                             {
                                 userId,
                                 VerticalIds = verticalIds?.ToArray(),
                                 OpcoIds = opCoIds?.ToArray(),
                                 Today = today,
                                 PreferenceDate = preferenceDate,
                                 ProprietaryHW = (int)BuildconstructionRuleEnum.ProprietaryHW,
                                 CotsHW = (int)BuildconstructionRuleEnum.CotsHW,
                                 ProdImportanceStrategic = ConstantValueFilter.ProdImportanceStrategic,
                                 
                             });


                    result = lcmEomAndEosEntities.Where(x => x.LcmArchived == false && ((x.EndOfMaintenance > messagingDate && x.EndOfMaintenance < today)
     ||
    (x.EndOfSupport > messagingDate && x.EndOfSupport < today)))
                          .GroupBy(x => new { x.Product, x.OriginalEquipmentManufacturer })
                          .Select(productGroup => new DoingSectionForEomAnsEos
                          {
                              Product = $"{productGroup.Key.Product}-{productGroup.Key.OriginalEquipmentManufacturer}",

                              Versions = productGroup
                                  .GroupBy(v => v.SoftwareVersion)   // Use the actual property name
                                  .Select(versionGroup => new
                                  {
                                      VersionGroup = versionGroup,
                                      Eom = versionGroup.Select(x => x.EndOfMaintenance).FirstOrDefault(),
                                      Eos = versionGroup.Select(x => x.EndOfSupport).FirstOrDefault()
                                  })
                                  .OrderBy(x => x.Eom == null)
                                  .ThenBy(x => x.Eom)
                                  .Select(x => new VersionDto
                                  {
                                      Version = x.VersionGroup.Key,
                                      MajorSwId = x.VersionGroup
                                          .Select(v => v.MajorSoftwareBuildId)
                                          .FirstOrDefault(),

                                      OpCo = string.Join(",",
                                          x.VersionGroup
                                           .Select(v => v.Opco)
                                           .Distinct()),

                                      EomDate = x.Eom?.ToString("dd-MM-yyyy"),
                                      EosDate = x.Eos?.ToString("dd-MM-yyyy")
                                  })
                                  .ToList()
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

        #region Product Complaince
        public IQueryable<MajorSoftwareBuild> GetQueryForProduct(ExpressionStarter<Majorsoftwarebuilds> predicateResult)
        {
            var query = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(predicateResult, false)
                            .Include(x => x.Productname)
                            .Include(x => x.Majorswbuildsdesigncontacts)
                            .AsEnumerable().Select(p => MajorSoftwareBuildMapper.GetProductAndDesignContactMapper(p)).AsQueryable(); 

            return query;
        }

        public ExpressionStarter<Majorsoftwarebuilds> ApplyFilter(ProductComplainceQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Majorsoftwarebuilds>();
            var predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();

            if (buildFilterDto.MajorSoftwareBuildId != null && buildFilterDto.MajorSoftwareBuildId.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.MajorSoftwareBuildId)
                    predicateInner.Or(x => x.Majorsoftwarebuildsid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignContactIds != null && buildFilterDto.DesignContactIds.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.DesignContactIds)
                    predicateInner.Or(x => x.Majorswbuildsdesigncontacts.Any(c => c.Designcontactid == item));
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public string GetProductCompliance(DateTime? eos, DateTime? eom, EOMEnum eomStatus)
        {
            if (eos == null && eom == null)
            {
                return eomStatus == EOMEnum.NotAnnounced ? ConstantValueFilter.Green : ConstantValueFilter.Red;
            }

            if ((eos.HasValue && eos.Value >= currentDate) || (eom.HasValue && eom.Value >= currentDate))
            {
                return ConstantValueFilter.Green;
            }

            return ConstantValueFilter.Red;
        }

        public Task<IEnumerable<ProductComplainceDto>> GetBaseSoftwareRecords(List<MajorSoftwareBuild> SoftwareEntity)
        {
            IEnumerable<ProductComplainceDto> baseSoftwareEntity = SoftwareEntity.Select(x => new ProductComplainceDto()
            {
                ProductId = x.ProductName.Id,
                ProductName = x.ProductName.Description,
                EndOfMaintenance = x.EndOfMaintenance,
                EndOfSupport = x.EndOfsupport,
                EOMValue = x.EOMStatus == (short)Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified",
                CompatibilityColorCode = GetProductCompliance(x.EndOfsupport, x.EndOfMaintenance, x.EOMStatus),
            });

            return Task.FromResult(baseSoftwareEntity);
        }

        public async Task<ResultDto> FindByCondtionForProductComplaince(long userId)
        {
            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);

                var verticalIds = userDetailsList.VerticalDetails != null && userDetailsList.VerticalDetails.Count > 0 ?
                                  userDetailsList.VerticalDetails : new List<int>();

                var opCoIds = userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                              userDetailsList.OpcoDetails : new List<short>();
                if (userDetailsList?.RoleRecords?.Any(x =>
                     string.Equals(x.RoleName, "SW Product Owner", StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return new ResultDto
                    {
                        Data = null
                    };
                }
                ProductComplainceQueryDto queryDto = new ProductComplainceQueryDto();

                var designContactIds = _commonManager.GetDesignContactIdsFromOpcoAndVerticalIds(opCoIds, verticalIds);
                if (designContactIds != null && designContactIds.Count > 0) 
                    queryDto.DesignContactIds = designContactIds;



                var predicateResult = ApplyFilter(queryDto);

                var queryList = GetQueryForProduct(predicateResult).ToList();
                if (queryList.Count == 0)
                {
                    return new ResultDto();
                }

                var BaseProductRecords = await GetBaseSoftwareRecords(queryList);
                var productWiseCompliance = await GetProductWisePercentage(BaseProductRecords);

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = new
                    {
                        productWiseCompliance
                    }
                };
            }
            catch (Exception ex)
            {
                
                _loggerManager.LogError(ex, "FindByCondtionForProductComplaince failed");
                throw;
            }
        }

        public Task<ProductComplianceSummaryDto> GetProductWisePercentage(IEnumerable<ProductComplainceDto> productEntities)
        {
            var entityList = productEntities as IList<ProductComplainceDto> ?? productEntities.ToList();

            var products = entityList
                .GroupBy(x => x.ProductName)
                .Select(g =>
                {
                    var greenCount = g.Count(x => x.CompatibilityColorCode == ConstantValueFilter.Green);
                    var redCount = g.Count(x => x.CompatibilityColorCode == ConstantValueFilter.Red);
                    var totalCount = g.Count();

                    return new ProductComplainceDto
                    {
                        ProductId = g.First().ProductId,
                        ProductName = g.Key,
                        CompliantProductCount = greenCount.ToString(),
                        NonCompliantProductCount = redCount.ToString(),
                        TotalProductCount = totalCount.ToString()
                    };
                })
                .OrderByDescending(x => x.ProductName).ToList();

            var distinctProducts = entityList.ToList();
            var totalGreenCount = distinctProducts.Count(x => x.CompatibilityColorCode == ConstantValueFilter.Green);
            var totalRedCount = distinctProducts.Count(x => x.CompatibilityColorCode == ConstantValueFilter.Red);
            var overallProductCount = totalGreenCount + totalRedCount;

            string CalculateProductPercentage(int count, int total) =>
                total > 0 && count > 0 ? ((decimal)count / total * 100).ToString("0.#") : "0";

            return Task.FromResult(new ProductComplianceSummaryDto
            {
                Products = products,
                OverallProductCount = overallProductCount,
                OverallComplaintProductCount = totalGreenCount,
                OverallNonComplaintProductCount = totalRedCount,
                OverallGreenPercentage = CalculateProductPercentage(totalGreenCount, overallProductCount),
                OverallRedPercentage = CalculateProductPercentage(totalRedCount, overallProductCount)
            });
        }

        #endregion


        #region new portal based filter

        private ExpressionStarter<Lcmengineering> ApplyFilterForEomAndEos(long userId, List<short> opCoId, List<int> ruleId, bool isAchive = false,
             List<short> SwPlannedActivityRulesId = null, List<int> veticalId = null)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);
            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);

            predicateInner.Or(x => x.Deleted == false &&
                           x.Opco.Opco.ToLower() != "zzz" &&
                              (isAchive || x.Archived == isAchive));

            predicateResult.And(predicateInner);

            if (veticalId != null && veticalId.Count > 0)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();

                foreach (var item in veticalId)
                {
                    _ = predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser
                                               .Any(m => m.Organisation.Verticalid == item && m.Deleted == false
                                               )));

                }
                _ = predicateResult.And(predicateInner);

            }

            if (opCoId?.Any() == true && ruleId.Any(f => f != 1))
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in opCoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (SwPlannedActivityRulesId != null && SwPlannedActivityRulesId.Count > 0)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                predicateInner.Or(x => x.PlannedactivitiesLcmengineering.All(p => SwPlannedActivityRulesId.Contains((short)p.Plannedactivityresourceid)));
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }


        #endregion      
        #region  Expired EOM and EOS  Record
         
        #endregion  

    }

}
