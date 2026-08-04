using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ServicePlan;
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

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServicePlanController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ServicePlanManager _servicePlanManager;
        private readonly IExportService _exportService;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly List<short> _opcoList;
        private readonly bool _adminRoleCheck;
        private readonly List<int> _verticalList;
        public ServicePlanController(AuthorizedRoleManager authorizedRoleManager,ServicePlanManager servicePlanManager, IExportService exportService,ILoggerManager logger, IHttpContextAccessor contextAccessor,ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _authorizedRoleManager = authorizedRoleManager;
            _currentUserService = currentUserService;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;
            _servicePlanManager = servicePlanManager;
            _exportService = exportService;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails : null;
        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<ServicePlanPaGridDto>> GetServices([FromBody] PlannedActivityQueryDto dto)
        {
            try 
            {
                if (dto.OpCo==null ||(dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoList;

                if ((dto.VerticalName == null) || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                {
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }

                return await _servicePlanManager.FindWithCondition(dto);

            }
            catch (Exception ex)
            {
               _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost("GetArchivedServiceLevelPa")]
        public async Task<QueryResultDto<ServicePlanPaGridDto>> GetArchivedServiceLevelPa([FromBody] PlannedActivityQueryDto dto)
        {
            try
            {
                dto.Archived = true;
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoList;
                if ((dto.VerticalName == null) || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                {
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                return await _servicePlanManager.FindWithCondition(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] PlannedActivityQueryDto dto)
        {
            try
            {
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoList;

                if ((dto.VerticalName == null) || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                {
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                var data = _servicePlanManager.GetFilter(propertyName, propertyFilter, dto,_adminRoleCheck);
                return data;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "GetUpdatedPage")]
        public async Task<ServicePlanUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _servicePlanManager.GetUpdatedPage(id, _opcoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public async Task<ServicePlanCreateDto> GetCreatePage()
        {
            try
            {
                return await _servicePlanManager.GetCreatePage(_opcoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] ServicePlanCreateDto dto)
        {
            try
            {
                return await _servicePlanManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(ServicePlanUpdateDto dto)
        {
            try
            {
                return await _servicePlanManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }        

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(int id)
        {           

            try
            {
                return await _servicePlanManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] PlannedActivityQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoList;
                if ((dto.VerticalName == null) || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                {
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                var data = await _servicePlanManager.FindWithCondition(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "ServicePlan_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
