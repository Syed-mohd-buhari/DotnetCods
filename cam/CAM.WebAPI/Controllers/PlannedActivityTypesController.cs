using CAM.Contracts;
using CAM.DataTransferObjects.QueryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using CAM.Infrastucture.QueryResult;
using CAM.DataTransferObjects.Entita.PlannedActivityTypes;
using CAM.DataTransferObjects.FunctionalityDto;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.DataTransferObjects;
using CAM.Infrastucture;
using CAM.BusinessManager.Entity;
using CAM.Exports;
using System.Linq;
using CAM.Imports;

namespace CAM.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlannedActivityTypesController : CamControllerBase
    {
        private readonly PlannedActivityTypesManager _plannedActivityTypesManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;

        public PlannedActivityTypesController(PlannedActivityTypesManager plannedActivityTypesManager, IExportService exportService, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _plannedActivityTypesManager = plannedActivityTypesManager;
            _currentUserService = currentUserService;
            _exportService = exportService;
        }
        [HttpPost("GetPlannedActivity")]

        public QueryResultDto<PlannedActivityTypesGridDto> GetPlannedActivity([FromBody] PlannedActivityTypesQueryDto dto)
        {
            try
            {
                return _plannedActivityTypesManager.GetPlannedActivity(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("GetPlannedDropDownList")]

        public List<PlannedActivityTypesDropDownDto> GetPlannedDropDownList([FromBody] PlannedActivityTypesDropDownDto dto)
        {
            try
            {
                return _plannedActivityTypesManager.GetPlannedDropDownList(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] PlannedActivityTypesQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _plannedActivityTypesManager.GetFilter(propertyName, propertyFilter, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }
        [HttpPost(template: "CreatePlannedRule")]
        public async Task<ResultDto> Create([FromBody] PlannedActivityTypesCreateOrUpdateDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _plannedActivityTypesManager.Add(dto);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpPut(template: "UpdatePlannedRule")]
        public async Task<ResultDto> Put(PlannedActivityTypesCreateOrUpdateDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _plannedActivityTypesManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPut(template: "DeleteRule")]
        public async Task<ResultDto> DeleteRule([FromBody] PlannedActivityTypesCreateOrUpdateDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _plannedActivityTypesManager.DeleteRule(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "EnableDisableLinkedDcRule")]
        public async Task<ResultDto> Post([FromBody] PlannedActivityTypesCreateOrUpdateDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _plannedActivityTypesManager.EnableDisableLinkedDcRule(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] PlannedActivityTypesQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                
                var data = _plannedActivityTypesManager.GetPlannedActivity(dto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Planned-Activitie-Types" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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
    }
}
