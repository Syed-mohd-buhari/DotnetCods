using CAM.BusinessManager.Entity.OMC;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.OMC.AssetAsisSdiSwitchInfo;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.OMC;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.OMC
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetAsisSdiSwitchInfoController : CamControllerBase
    {
        private readonly AssetAsisSdiSwitchInfoManager _manager;
        private readonly IExportService _exportService;
        public AssetAsisSdiSwitchInfoController(AssetAsisSdiSwitchInfoManager manager, IExportService exportService, ILoggerManager logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
        {
            _manager = manager;
            _exportService = exportService;
        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<AssetAsIsSdiSwitchInfoGridDto>> GetAssetAsisSdiInfo([FromBody] AssetAsisSdiSwitchInfoQueryDto assetAsisFilterDto)
        {
            try
            {
                return await _manager.FindWithCondition(assetAsisFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] AssetAsisSdiSwitchInfoQueryDto dto)
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
        public async Task<FileContentResult> ExportReport([FromBody] AssetAsisSdiSwitchInfoQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = await _manager.FindWithCondition(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "CHASIS_INFO_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
