using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.LcmAncillaryData;
using CAM.DataTransferObjects.Entita.SystemVerificationProblem;
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
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/SystemVerificationProblem")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class SystemVerificationProblemController : CamControllerBase
    {
        private readonly SystemVerificationProblemManager _manager;
        private readonly IExportService _exportService;
        public SystemVerificationProblemController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, SystemVerificationProblemManager manager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
        }

        [HttpPost("Get")]
        public QueryResultDto<SystemVerificationProblemDtoGrid> GetSystemVerificationProblem([FromBody] SystemVerificationProblemQueryDto SystemVerificationProblemQueryDto)
        {
            try
            {
                return _manager.FindWithCondition(SystemVerificationProblemQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] SystemVerificationProblemQueryDto SystemVerificationProblemQueryDto)
        {
            try
            {
                var data = _manager.GetFilter(propertyName, propertyFilter, SystemVerificationProblemQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] SystemVerificationProblemQueryDto dto)
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
                     "SystemVerificationProblem_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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

        [HttpGet(template: "Create")]
        public SystemVerificationProblemCreateDto GetCreatePage()
        {
            try
            {
                return _manager.GetCreatepage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public SystemVerificationProblemUpdateDto GetUpdatePage(short id)
        {
            try
            {
                return _manager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] SystemVerificationProblemCreateDto dto)
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

        [HttpPut("Update")]
        public async Task<ResultDto> UpdateSystemVerificationProblem([FromBody] SystemVerificationProblemUpdateDto dto)
        {
            try
            {
                return await _manager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                return await _manager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(long id)
        {
            try
            {
                return await _manager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

    }
}
