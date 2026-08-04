using CAM.BusinessManager.Entity.OMC;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.OMC.AssetAsIsHwAncillaryData;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.OMC;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class AssetAsisHwAncillaryDataController : CamControllerBase
    {
        private readonly AssetAsisHwAncillaryDataManager _manager;
        private readonly IExportService _exportService;
        public AssetAsisHwAncillaryDataController(IExportService exportService,AssetAsisHwAncillaryDataManager manager,ILoggerManager logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
        {
            _manager = manager;
            _exportService = exportService;
        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<AssetAsIsHwAncillaryDataGridDto>> GetAssetAsisHwAncillaryData([FromBody] AssetAsIsHwAncillaryDataQueryDto assetAsisFilterDto)
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

        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] AssetAsIsHwAncillaryDataQueryDto dto)
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
                     "CCD_INFO_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
        public async Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] AssetAsIsHwAncillaryDataQueryDto dto)
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
    