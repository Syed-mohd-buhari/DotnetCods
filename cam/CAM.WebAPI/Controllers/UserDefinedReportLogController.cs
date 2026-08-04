using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AuditLog;
using CAM.DataTransferObjects.Entita.Organisation;
using CAM.DataTransferObjects.Entita.UserDefinedReportLog;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class UserDefinedReportLogController : CamControllerBase 
    {
        private readonly UserDefinedReportLogManager _manager;
        private readonly IExportService _exportService;
        public UserDefinedReportLogController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, UserDefinedReportLogManager manager) 
            : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
        }

        [HttpPost("Get")]
        public QueryResultDto<UserDefinedReportLogDto> GetUserDefinedReportLogHistory([FromBody] UserDefinedReportLogQueryDto userDefinedReportLogQueryDto)
        {
            try
            {
                return _manager.FindWithCondition(userDefinedReportLogQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] UserDefinedReportLogQueryDto userDefinedReportLogQueryDto)
        {
            try
            {
                var data = _manager.GetFilter(propertyName, propertyFilter, userDefinedReportLogQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpPost]
        public async Task<ResultDto> Create([FromBody] UserDefinedReportLogCreateDto dto)
        {
            try
            {
                return await _manager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
           
        }
        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] UserDefinedReportLogQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = _manager.FindWithCondition(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "UserDefinedReportLog_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
                return null;
            }

        }
    }
}
