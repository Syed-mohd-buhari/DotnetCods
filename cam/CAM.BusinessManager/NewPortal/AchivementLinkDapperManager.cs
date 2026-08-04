using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.DataTransferObjects.QueryDto.NewPortal;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.Repository;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.AbstractionLayer
{
    public class AchivementLinkDapperManager : BaseManager
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
        private GridCustomColumnManager _gridCustomColumnmanager;
        public AchivementLinkDapperManager(IEnumerable<IRepositoryWrapper> wrappers, AuthorizedRoleManager authorizedRoleManager, ILoggerManager loggerManager,
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonDapperRepository commonDapperRepository, LCMPADapperQueries lCMPADapperQueries,
           CommonManager commonManager, DapperCommonManager dapperCommonManager, GridCustomColumnManager  gridCustomColumnmanager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _authorizedRoleManager = authorizedRoleManager;
            _lCMPADapperQueries = lCMPADapperQueries;
            _loggerManager = loggerManager;
            _commonDapperRepository = commonDapperRepository;
            _commonManager = commonManager;
            dapperDatabaseMode = GlobalDbMode.DbMode.ContainsKey(CurrentLogLevelConfig._UserName) ? GlobalDbMode.DbMode[CurrentLogLevelConfig._UserName] : "normal";
            _dapperCommonManager = dapperCommonManager;
            _gridCustomColumnmanager = gridCustomColumnmanager;
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
        public async Task<ResultDto> GetAchievementRecordsAsync(AchivementSectionQueryDto dto)
        {
             
            var achievementRecords = new QueryResultDto<PlanndActivitySoftwareUpgradDetailsDtoGrid>(new GenerateRenderForGrid<PlanndActivitySoftwareUpgradDetailsDtoGrid>(_gridCustomColumnmanager))
            {
            };
            try
            {
                var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(dto.UserId,true);
                 
                var configuredRole = userDetailsList?.RoleRecords?.FirstOrDefault(x => x.PortalRoleId == dto.PortalRoleId);
                if (configuredRole == null)
                {
                    return new ResultDto
                    {
                        Data = null,
                        Info = "Role not exists for logged user"
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

                var getGridCustomColumns = _dapperCommonManager.GetGridCustomColumnsAsync(dto.UserId);                                   

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
                achievementRecords.TotalItems = archivePaEntity.Count();
                var paginatedRecords = archivePaEntity.AsQueryable().ApplyPaging(dto);
               var result = paginatedRecords?.ToList()
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

                         }).ToList();

               
                achievementRecords.Items = result.ToList(); 

               

                var preferenceDetails = getGridCustomColumns.Result.FirstOrDefault();// await _repositoryWrapper.GridCustomColumnRepository.FindByCondition(f => f.Userid == userId && f.Classname == ConstantValueFilter.UserPreferenceName).FirstOrDefaultAsync();
               
                return new ResultDto
                {
                    Data = new
                    {
                        AchivementRecords = achievementRecords,
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
        
    }

}
