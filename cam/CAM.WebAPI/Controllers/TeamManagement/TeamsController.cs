using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.TeamManagement;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.TeamManagement;
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

    public class TeamsController : CamControllerBase
    {
        private readonly TeamsManager _manager;
        private readonly IExportService _exportService;

        public TeamsController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, TeamsManager manager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;            
        }

        [HttpPost("Get")]

        public async Task<QueryResultDto<TeamGridDto>> Get([FromBody] TeamQueryDto dto)
        {
            try
            {                              
                return await _manager.FindWithCondition(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpPost("Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] TeamQueryDto dto)
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

        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] TeamQueryDto dto)
        {
            try
            {                
                dto.Page = 0;
                dto.PageSize = 0;
                var data = await _manager.GetTeamsDetails(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "Team Management_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
        

        [HttpGet("Create")]
        public TeamGridDto Add()
        {
            try
            {
               return _manager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("Edit")]
        public async Task<TeamGridDto> Edit(int id)
        {
            try
            {
                return await _manager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("GetRelatedRecords")]
        public async Task<ResultDto> GetRelatedDetails(int id)
        {
            try
            {
                return await _manager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost("CreateOrUpdateTeams")]
        public async Task<ResultDto> CreateOrUpdate([FromBody] TeamGridDto dto)
        {
            try
            {
                return await _manager.CreateOrUpdateTeams(dto);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpPut("DeactivateTeam")]
        public async Task<ResultDto> Deactivate(TeamGridDto dto)
        {
            try
            {
                return await _manager.Deactivate(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete("Delete")]
        public async Task<ResultDto> Delete(int id)
        {
            try
            {
                return await _manager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete("DeepDelete")]
        public async Task<ResultDto> DeepDelete(int id)
        {
            try
            {
                return await _manager.DeepDelete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
