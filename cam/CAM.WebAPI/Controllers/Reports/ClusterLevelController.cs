using CAM.BusinessManager.Entity.Report;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.Report;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.Reports
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClusterLevelController : CamControllerBase
    {
        private readonly ClusterLevelManager _manager;
        private readonly IExportService _exportService;

        public ClusterLevelController(ILoggerManager logger, IHttpContextAccessor contextAccessor, ClusterLevelManager manager, IExportService exportService) : base(logger, contextAccessor)
        {
            _manager = manager;
            _exportService = exportService;
        }
        [HttpPost ("GetClusterLevel")]
        public async Task<QueryResultDto<ClusterLevelDtoGrid>> GetClusterLevel([FromBody] ClusterLevelQueryDto filterDto)
        {
            try
            {
                return await _manager.FindWithConditionAsync(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] ClusterLevelQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = await _manager.FindWithConditionAsync(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "ClusterLevel_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] ClusterLevelQueryDto dto)
        {
            try
            {
                return await _manager.GetFilter(propertyName, propertyFilter, dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
    }
