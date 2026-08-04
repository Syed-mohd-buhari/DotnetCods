using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignAspects;
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
    public class DesignAspectController : CamControllerBase
    {
        private readonly DesignAspectManager _designAspectManager;
        private readonly IExportService _exportService;



        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<long> _opcoList;
        private readonly List<long> _verticalList;
        private readonly bool _adminRoleCheck=false;

        public DesignAspectController(DesignAspectManager designAspectManager, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, IExportService exportService, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _designAspectManager = designAspectManager;
            _exportService = exportService;

            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt64(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToInt64(x)).Distinct().ToList() : null;

        }
        [HttpGet(template: "Create")]
        public DesignAspectDtoModel GetCreate(long? dcfId = null)
        {
            try
            {
                return _designAspectManager.GetCreatedPage(_opcoList, dcfId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "Update")]
        public async Task<DesignAspectDtoModel> GetUpdate(long id)
        {
            try
            {
                return await _designAspectManager.GetUpdatedPage(id, _opcoList, _verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "GetDCFName")]
        public async Task<ResultDto> GetDCFName(long dcfId)
        {
            try
            {
                return await _designAspectManager.GetDCFName(dcfId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetDCFsPerOpco")]
        public Dictionary<long, string> GetDCFs(int opcoId)
        {
            try
            {
                return _designAspectManager.GetDCFPerOpco(opcoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "GetServicesAndNetworks")]
        public DesignAspectDCFModel GetServicesAndNetworks(long dcfId)
        {
            try
            {
                return _designAspectManager.GetServicesAndNetworks(dcfId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }


        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] DesignAspectQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                if ((dto.OpCoId != null && dto.OpCoId.Count == 0))
                    dto.OpCoId = _opcoList;

                if ((dto.VerticalName == null) ||(dto.VerticalName != null && dto.VerticalName.Count == 0))
                    dto.VerticalName = _verticalList != null ? _verticalList.Select(x => Convert.ToString(x)).ToList() : null;

                var data = _designAspectManager.FindWithCondition(dto);
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()

                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);
               
                string sheetName = dto.Archived == true ? "Archived-Design-Aspect" : "Design-Aspect";
              
                var result = _exportService.GetExcelFrom(reportSheets,
                    sheetName + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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
        [HttpPost]
        public async Task<ResultDto> Create([FromBody] DesignAspectDtoModel dto)
        {
            try
            {
                var result = await _designAspectManager.Add(dto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpPut]
        public async Task<ResultDto> Put([FromBody] DesignAspectDtoModel dto)
        {
            try
            {
                var result = await _designAspectManager.Edit(dto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpGet(template: "Reset")]
        public async Task<ResultDto> Reset(int id)
        {
            try
            {

                var result = await _designAspectManager.Reset(id);
                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("Get")]
        public QueryResultDto<DesignAspectDtoGrid> GetDesignAspect(
           [FromBody] DesignAspectQueryDto designAspectQueryDto)
        {
            try
            {
                if ((designAspectQueryDto.OpCoId != null && designAspectQueryDto.OpCoId.Count == 0))
                    designAspectQueryDto.OpCoId = _opcoList;

                if ((designAspectQueryDto.VerticalName ==null) ||(designAspectQueryDto.VerticalName != null && designAspectQueryDto.VerticalName.Count == 0))
                    designAspectQueryDto.VerticalName = _verticalList != null ? _verticalList.Select(x => Convert.ToString(x)).ToList() : null;

                var result = _designAspectManager.FindWithCondition(designAspectQueryDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] DesignAspectQueryDto designAspectQueryDto)
        {
            try
            {
                if ((designAspectQueryDto.OpCoId != null && designAspectQueryDto.OpCoId.Count == 0))
                    designAspectQueryDto.OpCoId = _opcoList;

                if ((designAspectQueryDto.VerticalName == null) ||(designAspectQueryDto.VerticalName != null && designAspectQueryDto.VerticalName.Count == 0))
                    designAspectQueryDto.VerticalName = _verticalList != null ? _verticalList.Select(x => Convert.ToString(x)).ToList() : null;

                var data = _designAspectManager.GetFilter(propertyName, propertyFilter, designAspectQueryDto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "GetRelatedRecords")]
        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            try
            {
                return await _designAspectManager.GetRelatedRecords((int)id);
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
                return await _designAspectManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(long id, bool onlyPlannedActivities)
        {
            try
            {
                return await _designAspectManager.DeleteDeep(id, onlyPlannedActivities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
