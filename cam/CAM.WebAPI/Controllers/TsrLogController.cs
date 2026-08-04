using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.AuditLog;
using CAM.DataTransferObjects.Entita.TsrLog;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAM.WebAPI.Controllers
{
    [Route("api/TsrLog")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class TsrLogController : CamControllerBase 
    {
        private readonly TsrLogManager _manager;
        private readonly IExportService _exportService;
        public TsrLogController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, TsrLogManager manager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
        }

        [HttpPost("Get")]
        public QueryResultDto<TsrLogDtoGrid> GetAuditHistory([FromBody] TsrLogQueryDto tsrLogQueryDto)
        {
            try
            {
                return _manager.FindWithCondition(tsrLogQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] TsrLogQueryDto tsrLogQueryDto)
        {
            try
            {
                var data = _manager.GetFilter(propertyName, propertyFilter, tsrLogQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] TsrLogQueryDto dto)
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
                     "AuditLog_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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

        [HttpGet("GetLatestRefreshStatus")]
        public TsrLogDtoGrid GetLatestRefreshStatus(int filterId ,string opcoId="")
        {
            try
            {
                return _manager.GetRefreshStatus(filterId, opcoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpPost("GetAllOpcoWiseLatestRefreshStatus")]
        public QueryResultDto<TsrLogDtoGrid> GetAllOpcoWiseLatestRefreshStatus([FromBody] TsrLogQueryDto tsrLogQueryDto)
        {
            try
            {
                return _manager.GetAllOpcoWiseLatestRefreshStatus(tsrLogQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

    }
}
