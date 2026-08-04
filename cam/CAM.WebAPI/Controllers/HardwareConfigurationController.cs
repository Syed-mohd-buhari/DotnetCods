using CAM.BusinessManager.Entity;
using CAM.Contracts;
using Microsoft.AspNetCore.Authorization;
using CAM.DataTransferObjects.Entita.Idenitty;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAM.WebAPI.Controllers
{
    [Route("api/HardwareConfiguration")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class HardwareConfigurationController : CamControllerBase
    {

        private readonly HardwareConfigurationManager _manager;
        private readonly IExportService _exportService;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _adminRoleCheck = false;
        private readonly List<string> _opcoDescriptionList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;

        public HardwareConfigurationController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, HardwareConfigurationManager manager
            , ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoDescriptionList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDescription != null && _roleOpcoList.OpcoDescription.Any() == true ?
                _roleOpcoList.OpcoDescription : null;
        }
        [HttpPost("Get")]
        public QueryResultDto<HardwareConfigurationDtoGrid> GetHardwareConfiguration(
           [FromBody] HardwareConfigurationQueryDto designComponentFilterDto)
        {
            try
            {

                if ((designComponentFilterDto.Opco == null) || (designComponentFilterDto.Opco != null && designComponentFilterDto.Opco.Count == 0))
                    designComponentFilterDto.Opco = _opcoDescriptionList;
                return _manager.FindWithCondition(designComponentFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] HardwareConfigurationQueryDto dto)
        {
            try
            {
                if ((dto.Opco == null) || (dto.Opco != null && dto.Opco.Count == 0))
                    dto.Opco = _opcoDescriptionList;
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
                    "HardwareConfiguration_Raw_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] HardwareConfigurationQueryDto designComponentFilterDto)
        {
            try
            {
                if ((designComponentFilterDto.Opco == null) || (designComponentFilterDto.Opco != null && designComponentFilterDto.Opco.Count == 0))
                    designComponentFilterDto.Opco = _opcoDescriptionList;
                var data = _manager.GetFilter(propertyName, propertyFilter, designComponentFilterDto);
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

