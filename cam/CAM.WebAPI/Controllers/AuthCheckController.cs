using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthCheckController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserLoggingLevelManager _userLoggingLevelManager;
        private readonly NLog.Logger _logger;
        private readonly string mode;

        public AuthCheckController(ICurrentUserService currentUserService, IUserLoggingLevelManager userLoggingLevelManager,
            ILoggerManager logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _userLoggingLevelManager = userLoggingLevelManager;
            _logger = NLogBuilder.ConfigureNLog($"{Directory.GetCurrentDirectory()}/NLog.config").GetCurrentClassLogger();
            if (string.IsNullOrEmpty(_currentUserService.EMail)) { mode = "normal"; }
            else if (GlobalDbMode.DbMode.TryGetValue(_currentUserService.EMail.ToLower(), out var dbModeValue) && dbModeValue == "training") { mode = "training"; }
            else { mode = "normal"; }
        }

        [HttpGet]
        public async Task<IActionResult> Check()
        {
            try
            {
                var roles = _currentUserService.Rule;
                var _UserId = _currentUserService.UserId.ToString();
                string userLogLevels = "";
                long pageSize = _userLoggingLevelManager.GetPageSize();
                var userPrefrenceDetail = await _userLoggingLevelManager.GetUserPrefrenceDetails(_currentUserService.UserId); // need to change to new logic 
                var RoleDetailsForNewPortal = await _userLoggingLevelManager.GetRoleDetails(_currentUserService.UserId);  
                if (roles != null)
                {
                    userLogLevels = !string.IsNullOrEmpty(_UserId) ? await _userLoggingLevelManager.GetCurrentLogLevel() : "";
                    return Ok(new
                    {
                        role = roles,
                        username = _currentUserService.EMail.ToLower(),
                        mode = mode,
                        userid = Convert.ToInt32(_UserId),
                        pagesize = pageSize,
                        UserPagePrefrenceDetails = userPrefrenceDetail,
                        RoleDetial = RoleDetailsForNewPortal
                    });
                }
                else
                {
                    return Unauthorized("you are not authorize");
                }
            }
            catch (System.Exception exception)
            {
                _logger.Error(exception.Message);
                return Unauthorized("you are not authorize");
            }

        }
    }
}
