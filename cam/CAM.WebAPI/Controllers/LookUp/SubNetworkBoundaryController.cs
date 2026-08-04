using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.LookUp.SubNetworkBoundary;
using CAM.Infrastucture.QueryResult;
using CAM.BusinessManager.Entity;
using CAM.Exports;
using System.Linq;
using NLog;
using DocumentFormat.OpenXml.InkML;
using System.Drawing.Drawing2D;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubNetworkBoundaryController : CamControllerBase
    {
        private readonly SubNetworkBoundaryManager _subNetworkBoundaryManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;

        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public SubNetworkBoundaryController(SubNetworkBoundaryManager subNetworkBoundaryManager, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService, IExportService exportService) : base(logger, contextAccessor)
        {
            _subNetworkBoundaryManager = subNetworkBoundaryManager;
            _currentUserService = currentUserService;
            _exportService = exportService;
        }

        [HttpGet(template: "getLogLevel")]
        public async Task<IEnumerable<string>> Get()
        
        {
            try
            {

                var requestLogLevel = Request.Headers.SingleOrDefault(x => x.Key == "loglevel");
                LogLevel logLevel = LogLevel.Error;
                switch (requestLogLevel.Value.ToString().ToLower())
                {
                    case "trace":
                        logLevel = LogLevel.Trace;
                        break;
                    case "debug":
                        logLevel = LogLevel.Debug;
                        break;
                    case "info":
                        logLevel = LogLevel.Info;
                        break;
                    case "warn":
                    case "warning":
                        logLevel = LogLevel.Warn;
                        break;
                    case "error":
                        logLevel = LogLevel.Error;
                        break;
                    case "fatal":
                        logLevel = LogLevel.Fatal;
                        break;
                }
                SetMinLogLevel(logLevel);

                //_log.Trace("Some logs.");

                return new string[] { "value1", "value2" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        public static void SetMinLogLevel(LogLevel NewLogLevel)
        {
            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                rule.EnableLoggingForLevel(NewLogLevel);
            }

            //Call to update existing Loggers created with GetLogger() or 
            //GetCurrentClassLogger()
            LogManager.ReconfigExistingLoggers();
        }


        [HttpGet]
        public  Task<QueryResultDto<SubNetworkBoundaryGridDto>> GetSubNetworkBoundaryGetSubNetworkBoundary([FromQuery] SubNetworkBoundaryQueryDto subnetworkBoundaryFilterDto)
        {
            try
            {
                // Adding default subnetwork boundry on opnening subnetwork boudry table from design component
                if (subnetworkBoundaryFilterDto.VodafoneName != null)
                {
                    _subNetworkBoundaryManager.AddDefaultSubnetwork(subnetworkBoundaryFilterDto.VodafoneName.FirstOrDefault());
                }

                return _subNetworkBoundaryManager.GetEnityGrid(subnetworkBoundaryFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] SubNetworkBoundaryQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = _subNetworkBoundaryManager.GetEnityGrid(dto);
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Result.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Subnetwork-Boundary" + DateTime.Now.ToShortDateString() + ".xlsx", data.Result.GridRender);

                HttpContext.Response.ContentType = result.ContentType;
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                var fileContentResult = new FileContentResult(result.FileInByteArray, result.ContentType)
                {
                    FileDownloadName = result.FileName
                };

                return fileContentResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] SubNetworkBoundaryQueryDto subnetworkBoundaryFilterDto)
        {
            try
            {
                var data = _subNetworkBoundaryManager.GetFilter(propertyName, propertyFilter, subnetworkBoundaryFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] SubnetworkBoundaryDtoCreate subNetworkBoundaryDto)
        {
            try
            {
                return await _subNetworkBoundaryManager.Add(subNetworkBoundaryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(SubnetworkBoundaryDtoCreate subNetworkBoundaryDto)
        {


            try
            {
                return await _subNetworkBoundaryManager.Update(subNetworkBoundaryDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }



        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {

            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return await _subNetworkBoundaryManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _subNetworkBoundaryManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _subNetworkBoundaryManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public SubNetworkBoundaryGridDto GetCreateResourceSubNetworkBoundary()
        {

            try
            {
                return _subNetworkBoundaryManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public SubNetworkBoundaryGridDto GetUpdateResourceSubNetworkBoundary(short id)
        {
            try
            {
                return _subNetworkBoundaryManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "GetServicesOfSubNetworkBoundaries")]
        public Dictionary<int, string> GetServicesOfSubNetworkBoundaries(List<int> subNetworkBoundaryIds)
        {
            try
            {
                return _subNetworkBoundaryManager.GetServicesOfSubNetworkBoundaries(subNetworkBoundaryIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetAllSWApplicationType")]
        public Dictionary<string, string> GetAllSWApplicationType()
        {
            try
            {
                return _subNetworkBoundaryManager.GetAllProductNames();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetAllVodafoneNames")]
        public Dictionary<int, string> GetAllVodafoneNames()
        {
            try
            {
                return _subNetworkBoundaryManager.GetAllVodafoneNames();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }


        [HttpGet(template: "GetNewSubnetworkDescription")]
        public ResultDto GetNewSubnetworkDescription(int? vodafoneNameId)
        {
            try
            {
                return new ResultDto { Info = ResultMessages.GetInfoSuccess, Data = _subNetworkBoundaryManager.GetNewSubnetworkDescription(vodafoneNameId) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }


    }
}
