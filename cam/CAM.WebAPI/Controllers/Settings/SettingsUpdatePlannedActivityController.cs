using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.BusinessManager.Settings;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.Settings.SettingsUpdatePlannedActivity;
using CAM.Exports;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CAM.Infrastucture.QueryResult;
using CAM.BusinessManager.Entity;

namespace CAM.WebAPI.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class SettingsUpdatePlannedActivityController : CamControllerBase
    {
        private readonly SettingsUpdatePlannedActivityManager _settingsUpdatePlannedActivityManager;
        private readonly IExportService _exportService;
        public SettingsUpdatePlannedActivityController(SettingsUpdatePlannedActivityManager settingsUpdatePlannedActivityManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService) : base(logger, contextAccessor)
        {
            _settingsUpdatePlannedActivityManager = settingsUpdatePlannedActivityManager;
            _exportService = exportService;
        }

        [HttpGet]
        public Task<QueryResultDto<SettingsUpdatePlannedActivityDtoGrid>> GetSettingsUpdatePlannedActivity([FromQuery] SettingsUpdatePlannedActivityQueryDto dto)
        {
            try
            {
                return _settingsUpdatePlannedActivityManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPut(template: "ChangeGridOrderSettingsUpdatePlannedActivity")]
        public async Task<ResultDto> ChangeGridOrderSettingsUpdatePlannedActivity([FromBody] List<ChangeGridOrderDto> lista)
        {
            try
            {
                return await _settingsUpdatePlannedActivityManager.ChangeGridOrderSettingsUpdatePlannedActivity(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] SettingsUpdatePlannedActivityDtoCreate dto)
        {
            try
            {
                return await _settingsUpdatePlannedActivityManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(SettingsUpdatePlannedActivityDtoUpdate dto)
        {
            try
            {
                return await _settingsUpdatePlannedActivityManager.Update(dto);
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
                return await _settingsUpdatePlannedActivityManager.Delete(id);
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
                return await _settingsUpdatePlannedActivityManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            try
            {
                return await _settingsUpdatePlannedActivityManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public SettingsUpdatePlannedActivityDtoCreate GetCreateResourceSettingsUpdatePlannedActivity()
        {
            try
            {
                return _settingsUpdatePlannedActivityManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "GetCrossSettings")]
        public IDictionary<short, string> GetCrossSettings(short plannedActivityFor, int plannedActivityTypeId, int currentDeliveryStatus)
        {
            try
            {
                return _settingsUpdatePlannedActivityManager.GetCrossSettings(plannedActivityFor, plannedActivityTypeId, currentDeliveryStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "Update{id}")]
        public SettingsUpdatePlannedActivityDtoUpdate GetUpdateResourceSettingsUpdatePlannedActivity(short id)
        {
            try
            {
                return _settingsUpdatePlannedActivityManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }
        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] SettingsUpdatePlannedActivityQueryDto filterDto)
        {
            try
            {
                var data = _settingsUpdatePlannedActivityManager.GetFilter(propertyName, propertyFilter, filterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] SettingsUpdatePlannedActivityQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = await _settingsUpdatePlannedActivityManager.GetEnityGrid(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                foreach (var item in tabs)
                {
                    reportSheets.Add(item);
                }


                var result = _exportService.GetExcelFrom(reportSheets,
                    "Setting_Update_Planned_Activity" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        [HttpGet(template: "GetPATypeAndDeploymentStatus")]
        public Task<ResultDto> GetPATypeAndDeploymentStatus(short plannedActivityTypeFor)
        {
            try
            {
                var data = _settingsUpdatePlannedActivityManager.GetPATypeAndDeploymentStatus(plannedActivityTypeFor);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }
    }
}
