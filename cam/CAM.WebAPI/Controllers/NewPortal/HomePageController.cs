using CAM.BusinessManager.AbstractionLayer;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.QueryDto.NewPortal;
using CAM.Exports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.Dapper
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class HomePageController : CamControllerBase
    {
        private readonly AchivementLinkDapperManager _achivementLinkDapperManager;
        private readonly HomePageManager _homePagemanager;
      
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<short> _opcoList;
        private readonly List<int> _verticalList;
        private readonly bool _adminRoleCheck = false;

        public HomePageController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager,
                        AbstractionLayerDapperManager manager, HomePageManager homePagemanager, AchivementLinkDapperManager achivementLinkDapperManager) : base(logger, contextAccessor)
        {
            _achivementLinkDapperManager = achivementLinkDapperManager;
            _homePagemanager = homePagemanager;
            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            adminRoleId = _currentUserService.adminRoleId;
            sessionUserId = _currentUserService.UserId;
          
        }
        [HttpPost("achievementRecords")]
        public async Task<ResultDto> GetAchievementRecordsAsync(AchivementSectionQueryDto dto)
        {
            try
            {
                return await _achivementLinkDapperManager.GetAchievementRecordsAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, "Error retrieving achievement records for UserId: {UserId}", Convert.ToString(dto?.UserId));
                throw;
            }
        }        

        [HttpGet("ExpiredEomEosActionsForDesignContactAsync")]
        public async Task<ResultDto> GetEomEosOpenActionsForDesignContactAsync(long userId, long PortalRoleId)
        {
            try
            {
                return await _homePagemanager.FetchExpiredRecordForOpenLink(userId, PortalRoleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, "Error retrieving Open and Upcoming EOM and EOS records for UserId: {UserId}", Convert.ToString(userId));
                throw;
            }
        }

        [HttpGet("UpcomingEomEosActionsForDesignContactAsync")]
        public async Task<ResultDto> GetUpcomingEomEosActionsForDesignContactAsync(long userId, long PortalRoleId)
        {
            try
            {
                return await _homePagemanager.FetchFutureExpiredRecordForUpcomingLink(userId, PortalRoleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, "Error retrieving Open and Upcoming EOM and EOS records for UserId: {UserId}", Convert.ToString(userId));
                throw;
            }
        }

        [HttpGet("ProductComplaince")]
        public async Task<ResultDto> FindByCondtionForProductComplaince( long PortalRoleId)
        {
            try
            {
                return await _homePagemanager.FetchProductGraphComplaince(this.sessionUserId, PortalRoleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        #region Get count for Open and Upcoming link

        [HttpGet("GetOpenLinkRecordCount")]
        public async Task<ResultDto> GetOpenLinkRecordCount(long userId, long PortalRoleId)
        {
            try
            {
                return await _homePagemanager.GetcountForOpenLink(userId, PortalRoleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, "Error retrieving Open and Upcoming EOM and EOS records for UserId: {UserId}", Convert.ToString(userId));
                throw;
            }
        }

        [HttpGet("GetUpcomingLinkRecordCount")]
        public async Task<ResultDto> GetUpcomingLinkRecordCount(long userId, long PortalRoleId)
        {
            try
            {
                return await _homePagemanager.GetcountForUpcomingLink(userId, PortalRoleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, "Error retrieving Open and Upcoming EOM and EOS records for UserId: {UserId}", Convert.ToString(userId));
                throw;
            }
        }

        #endregion

    }
}
