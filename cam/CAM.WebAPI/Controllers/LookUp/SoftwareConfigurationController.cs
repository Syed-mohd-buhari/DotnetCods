using Microsoft.AspNetCore.Authorization;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.Idenitty;
using CAM.DataTransferObjects.Entita.NetworkElement;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CAM.BusinessManager.LookUp;
using CAM.DataTransferObjects.LookUp.Component;
using CAM.DataTransferObjects.Entita.SoftwareConfiguration;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/SoftwareConfiguration")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class SoftwareConfigurationController : CamControllerBase 
    {
        private readonly SoftwareConfigurationManager _manager;
        private readonly IExportService _exportService;
        public SoftwareConfigurationController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, BusinessManager.LookUp.SoftwareConfigurationManager manager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
        }

        [HttpPost("Get")]

        public QueryResultDto<SoftwareConfigurationDtoGrid> GetSubFunctionArea([FromBody] SoftwareConfigurationQueryDto componentQueryDto)
        {
            try
            {
                return _manager.FindWithCondition(componentQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] SoftwareConfigurationQueryDto designComponentFilterDto)
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
        public FileContentResult ExportReport([FromBody] SoftwareConfigurationQueryDto dto)
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
                     "SoftwareConfiguration_Raw_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
