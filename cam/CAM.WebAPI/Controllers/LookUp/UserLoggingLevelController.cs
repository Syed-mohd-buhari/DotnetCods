using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using CAM.DataTransferObjects.LookUp.UserLogginLevels;
using CAM.BusinessManager.LookUp;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoggingLevelController : CamControllerBase
    {
        private readonly IUserLoggingLevelManager _userLoggingLevelManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IConfiguration _configuration;

        public UserLoggingLevelController(IUserLoggingLevelManager userLoggingLevelManager,
            ILoggerManager logger, IHttpContextAccessor contextAccessor,ICurrentUserService currentUserService, IConfiguration configuration) : base(logger, contextAccessor)
        {
            _userLoggingLevelManager = userLoggingLevelManager;
            _currentUserService = currentUserService;
            _configuration = configuration;

        }
        [HttpGet(template: "GetCreatePageForLoggingLevels")]
        public UsersLoggingLevelsDtoGrid GetCreatePageForLoggingLevels()
        {
            try
            {
                return _userLoggingLevelManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetCurrentLogLevel")]
        public async Task<ResultDto> GetCurrentLogLevel()
        {
            try
            {
                return new ResultDto { Data = await _userLoggingLevelManager.GetCurrentLogLevel() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<ResultDto> Create([FromBody] UserLoggingLevelDto dto)
        {
            try
            {
                return await _userLoggingLevelManager.AddOrEdit(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpGet(template: "DownloadLogs")]
        public FileContentResult DownloadLogs()
        {
            try
            {
                string filePath = "";
                string fileName = "";
                byte[] fileBytes;
                var logsFilePath = string.Empty;
                try
                {
                    logsFilePath = _configuration.GetValue<string>("MySettings:LogsPath");
                    filePath = logsFilePath;
                    fileName = "TEMSLogs.log";
                    fileBytes = System.IO.File.ReadAllBytes(filePath);
                }
                catch
                {
                    logsFilePath = "/var/SP/CAMSDATA/LOGS/TEMSLogs.log";
                    filePath = logsFilePath;
                    fileName = "TEMSLogs.log";
                    fileBytes = System.IO.File.ReadAllBytes(filePath);
                }


                if (!HttpContext.Response.HasStarted)
                {
                    HttpContext.Response.ContentType = "text/plain";

                    HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                    Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");
                }
                var fileContentResult = new FileContentResult(fileBytes, "text/plain")
                {
                    FileDownloadName = fileName,
                };
                return fileContentResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

    }
}
