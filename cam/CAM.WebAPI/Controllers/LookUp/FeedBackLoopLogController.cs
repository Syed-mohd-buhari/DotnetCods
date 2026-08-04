using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CAM.DataTransferObjects.Entita.FeedBackLoopLog;
using CAM.Exports;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //#if DEBUG
    //    [Authorize]
    //#else
    //    [Authorize]
    //#endif
    public class FeedBackLoopLogController : CamControllerBase
    {
        private readonly FeedBackLoopLogManager _manager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;
        public FeedBackLoopLogController(FeedBackLoopLogManager manager, ILoggerManager logger,IExportService exportService, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _manager = manager;
            _currentUserService = currentUserService;
            _exportService = exportService;
        }


        [HttpPost("Get")]
        public QueryResultDto<FeedBackLoopAuditGridDto> GetFeedBackLoopLogDetails([FromBody] FeedBackLoopAuditQueryDto dto)
        {
            try
            {
                return _manager.FindWithCondition(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] FeedBackLoopAuditQueryDto dto)
        {
            try
            {
                var data = _manager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] FeedBackLoopAuditQueryDto dto)
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
                     "FeedBackLoop_Log_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
