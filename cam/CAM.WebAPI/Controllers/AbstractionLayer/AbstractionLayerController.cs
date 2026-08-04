using CAM.BusinessManager.AbstractionLayer;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.AbstractionLayer;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.Exports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.AbstractionLayer
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class AbstractionLayerController : CamControllerBase
    {
        private readonly AbstractionLayerManager _manager;

        public AbstractionLayerController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, AbstractionLayerManager manager) : base(logger, contextAccessor)
        {
            _manager = manager;
        }

        [HttpPost("GetFeedbackLoopLatestProcessTime")]
        public async Task<FeedbackLoopLatestProcessTimeDtoGrid> GetFeedbackLoopLatestProcessTimeRecords()
        {
            try
            {
                return await _manager.FindWithConditionForProcessTimeRecords();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        //[HttpPost("AssetOverviewByMarket")]
        //public async Task<LcmComplianceDto> GetAssetOverviewByMarketRecords()
        //{
        //    return await _manager.FindWithConditionForAssetOverviewByMarketRecords();
        //}

        [HttpPost("LcmCompliance")]
        public async Task<LcmComplianceDto> GetLcmComplianceRecords(int userId)
        {
            try
            {
                return await _manager.FindWithConditionForLcmComplianceRecords(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("GetEosAndEomMileStones")]
        public async Task<List<GetEosAndEomMileStonesDtoGrid>> GetEosAndEomMileStones([FromQuery] int userId)
        {
            try
            {
                return await _manager.GetEosAndEomMileStonesFromActiveLcms(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("PlanndActivitySoftwareUpgradDetails")]
        public async Task<ResultDto> GetPlannedActivitySWUpgradeRecords(int userId)
        {
            try
            {
                return await _manager.FindWithConditionForPlannedActivitySWUpgradeRecords(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("UserPrefrenceDetails")]
        public async Task<List<UserPrefrenceDetails>> GetUserPrefrenceRecords(int userId)
        {
            try
            {
                var className = ConstantValueFilter.UserPreferenceName;
                return await _manager.FindWithConditionForUserPrefrenceRecords(userId, className, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("DoingSectionPADetails")]
        public async Task<ResultDto> GetDoingSectionPARecords(int userId)
        {
            try
            {
                return await _manager.FindWithConditionForDoingSectionPARecords(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("DoingSectionForEomAndEos")]
        public async Task<List<DoingSectionForEomAnsEos>> GetDoingSectionForEosAndEom(int userId)
        {
            try
            {
                return await _manager.FindByConditionForDoingSectionForEosAndEom(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("AchievementsDetails")]
        public async Task<List<PlanndActivitySoftwareUpgradDetailsDtoGrid>> GetAchievementsRecords(int userId)
        {
            try
            {
                return await _manager.FindWithConditionForAchievementRecordsOld(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("SignPostPaDetails")]
        public async Task<List<PlanndActivitySoftwareUpgradDetailsDtoGrid>> GetSignPostPARecords(int userId)
        {
            try
            {
                return await _manager.FindWithConditionForSignPostPARecordsOld(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
    }
}
