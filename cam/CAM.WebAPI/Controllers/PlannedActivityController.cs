using CAM.BusinessManager;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Settings;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.Entita.DaMigrationStatus;
using CAM.DataTransferObjects.Entita.PlannedActivity;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public partial class PlannedActivityController : CamControllerBase
    {
        private readonly PlannedActivityManager _plannedActivityManager;
        private readonly UpdatePlannedActivityManager _updatePlannedActivityManager;
        private readonly IExportService _exportService;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _adminRoleCheck;
        private readonly List<short> _opcoList;
        private readonly List<int> _verticalList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        

        public PlannedActivityController(PlannedActivityManager plannedActivityManager, ILoggerManager logger, 
            IHttpContextAccessor contextAccessor, IExportService exportService,
            UpdatePlannedActivityManager updatePlannedActivityManager, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _plannedActivityManager = plannedActivityManager;
            _exportService = exportService;
            _updatePlannedActivityManager = updatePlannedActivityManager;
            _currentUserService = currentUserService;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails : null;
        }

        //Ticket 805 LCM -PA: Planned Design components dropdown should be grouped based on DCF  
        [HttpGet(template: "Create")]//{DcId}
        public async Task<PlannedActivityDtoCreate> GetCreateResourcePlannedActivity(long DcId, long DcfId = 0)
        {
            try
            {
                return await _plannedActivityManager.GetCreatePage(_opcoList, _verticalList, DcId, DcfId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "Update{id}")]
        public async Task<PlannedActivityDtoUpdate> GetUpdateResourcePlannedActivity(long id)
        {
            try
            {
                return await _plannedActivityManager.GetUpdatePage(id, _opcoList, _verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
           

        [HttpPost("Get")]
        public async Task<QueryResultDto<PlannedActivityDtoGrid>> GetPlannedActivity(
           [FromBody] PlannedActivityQueryDto plannedActivityFilterDto)
        {
            try
            {
                if (plannedActivityFilterDto.OpCo != null && plannedActivityFilterDto.OpCo.Count == 0)
                {
                    plannedActivityFilterDto.OpCo = _opcoList;
                }
                if ((plannedActivityFilterDto.VerticalName == null) || plannedActivityFilterDto.VerticalName != null && plannedActivityFilterDto.VerticalName.Count == 0)
                {
                    plannedActivityFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                return await _plannedActivityManager.FindWithCondition(plannedActivityFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost("GetArchivedPlannedActivity")]
        public QueryResultDto<ArchivedPlannedActivityDtoGrid> GetArchivedPlannedActivity(
            [FromBody] ArchivedPlannedActivityQueryDto plannedActivityFilterDto)
        {
            try
            {
                if (plannedActivityFilterDto.OpCo != null && plannedActivityFilterDto.OpCo.Count == 0)
                {
                    plannedActivityFilterDto.OpCo = _opcoList;
                }
                if ((plannedActivityFilterDto.VerticalName == null) || plannedActivityFilterDto.VerticalName != null && plannedActivityFilterDto.VerticalName.Count == 0)
                {
                    plannedActivityFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                return _plannedActivityManager.FindArchivedWithCondition(plannedActivityFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ArchivedPlannedActivityFilter")]
        public List<FilterValueDto> ArchivedPlannedActivityFilter([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ArchivedPlannedActivityQueryDto designComponentFilterDto)
        {
            try
            {
                if (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0)
                {
                    designComponentFilterDto.OpCo = _opcoList;
                }
                if ((designComponentFilterDto.VerticalName == null) || designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0)
                {
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                var data = _plannedActivityManager.ArchivedPlannedActivityFilter(propertyName, propertyFilter, designComponentFilterDto, _adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost(template: "ExportArchivedPlannedActivitiesReport")]
        public FileContentResult ExportArchivedPlannedActivitiesReport([FromBody] ArchivedPlannedActivityQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if (dto.OpCo != null &&
              dto.OpCo.Count() == 0)
                     dto.OpCo = _opcoList;

                if ((dto.VerticalName == null) || (dto.VerticalName != null &&
                                            dto.VerticalName.Count() == 0))
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;

                var data = _plannedActivityManager.FindArchivedWithCondition(dto);
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };


                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Archived-Planned-Activities" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] PlannedActivityQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if (dto.OpCo != null &&
              dto.OpCo.Count() == 0)
                      dto.OpCo = _opcoList;

                if ((dto.VerticalName == null) || dto.VerticalName != null &&
                                            dto.VerticalName.Count() == 0)
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;

                var data = await _plannedActivityManager.FindWithCondition(dto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Planned-Activities" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                return await _plannedActivityManager.Delete(id);
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
                return await _plannedActivityManager.DeleteDeep(id);
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
                return await _plannedActivityManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] PlannedActivityDtoCreate dto)
        {
            try
            {
                return await _plannedActivityManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
 
        [HttpGet("GetPlannedActivityRelatedDeliveryStatus")]
        public List<PlannedActivityDeliveryStatusDropDownDto> GetPlannedActivityRelatedDeliveryStatus(short id, short plannedActivityTypeFor)
        {
            try
            {
                return _plannedActivityManager.GetPlannedActivityRelatedDeliveryStatus(id, plannedActivityTypeFor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }


        }


        [HttpPut]
        public async Task<ResultDto> Put(PlannedActivityDtoUpdate dto)
        {
            try
            {
                return await _plannedActivityManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] PlannedActivityQueryDto plannedActivityFilterDto)
        {
            try
            {
                if (plannedActivityFilterDto.OpCo != null &&
                plannedActivityFilterDto.OpCo.Count() == 0)
                    plannedActivityFilterDto.OpCo = _opcoList;

                if ((plannedActivityFilterDto.VerticalName == null) || plannedActivityFilterDto.VerticalName != null &&
                  plannedActivityFilterDto.VerticalName.Count() == 0)
                    plannedActivityFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                var data = await _plannedActivityManager.GetFilter(propertyName, propertyFilter, plannedActivityFilterDto,false,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "GetConfrontoHardwareType")]
        public bool GetConfrontoHardwareTypeFromDesignComponent(long dc, long pdc)
        {
            
                return _plannedActivityManager.GetConfrontoHardwareTypeFromDesignComponent(dc, pdc);
        }
        [HttpGet(template: "CheckFiscalYear")]
        public bool CheckFiscalYear(int year)
        {
           
                return _plannedActivityManager.CheckFiscalYear(year);
        }

        

        [HttpPost(template: "ArchivePAWithDaMigration")]
        public async Task<ResultDto> ArchivePAWithDaMigration([FromBody] DaMigrationStatusAddUpdateDto dto, long PaId, short DeliveyStatusId)
        {
            try
            {

                var result = await _plannedActivityManager.ArchivePAWithDaMigration(dto, PaId, DeliveyStatusId);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ArchivePaWhenDaAssetMigrationComplete")]
        public async Task<ResultDto> ArchivePaWhenDaAssetMigrationComplete([FromBody] DaAssetMigrationAddUpdateDto dto, long PaId, short DeliveyStatusId)
        {
            try
            {

                var result = await _plannedActivityManager.ArchivePaWhenDaAssetMigrationComplete(dto, PaId, DeliveyStatusId);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

    }
}

