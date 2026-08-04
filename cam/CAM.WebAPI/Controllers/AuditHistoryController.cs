using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.Entita.NetworkElement;
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
    [Route("api/Audithistory")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class AuditHistoryController : CamControllerBase 
    {
        private readonly AuditHistoryManager _manager;
        private readonly IExportService _exportService;
        public AuditHistoryController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, AuditHistoryManager manager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
        }

        [HttpPost("Get")]

        public QueryResultDto<AuditHistoryDtoGrid> GetAuditHistory([FromBody] AuditHistoryQueryDto AuditHistoryQueryDto )
        {
            try
            {
                return _manager.FindWithCondition(AuditHistoryQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] AuditHistoryQueryDto designComponentFilterDto)
        {
            try
            {
                var data = _manager.GetFilter(propertyName, propertyFilter, designComponentFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }


        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] AuditHistoryQueryDto dto)
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
                     "AuditHistory_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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

        [HttpPost("Approved")]
        public async Task<ResultDto> ApproveStatus([FromBody] AuditHistoryQueryDto dto)
        {
            try
            {
                return await _manager.Approved(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("Reject")]
        public async Task<ResultDto> RejectStatus([FromBody] AuditHistoryQueryDto dto)
        {
            try
            {
                return await _manager.Reject(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        
        [HttpPost("Override")]
        public async Task<ResultDto> OverrideStatus([FromBody] AuditHistoryQueryDto dto)
        {
            try
            {
                return await _manager.Override(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        }
}
