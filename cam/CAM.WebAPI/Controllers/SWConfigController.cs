using Microsoft.AspNetCore.Authorization;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CAM.DataTransferObjects;
using System.Threading.Tasks;
using CAM.DataTransferObjects.Entita.SoftwareConfiguration;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class SWConfigController : CamControllerBase 
    {
        private readonly SWConfigManager _manager;
        private readonly IExportService _exportService;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _adminRoleCheck = false;
        private readonly List<string> _opcoDescriptionList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        public SWConfigController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, SWConfigManager manager
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

        public QueryResultDto<SWConfigGridDto> GetSWConfig([FromBody] SWConfigQueryDto dto )
        {
            try
            {
                var isExport = false;
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoDescriptionList;
                return _manager.FindWithCondition(dto, isExport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] SWConfigQueryDto dto)
        {
            try
            {
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoDescriptionList;
                var data = _manager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] SWConfigQueryDto dto)
        {
            try
            {

                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoDescriptionList;
                dto.Page = 0;
                dto.PageSize = 0;
                var isExport = true;
                var data = _manager.GetDownloadExcel(dto, isExport);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "SoftwareConfiguration_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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

        [HttpGet("SWConfigFilter")]
        public SWConfigurationFilterDto GetSWConfigFilterResult()
        {
            try
            {
                return _manager.GetSWConfigFilter(_opcoDescriptionList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost("GetSubFunction")]
        public QueryResultDto<SWConfigSubFunctionGridDto> GetSWConfigSubFunction([FromBody] SWConfigQueryDto dto)
        {
            try
            {
                var isExport = false;
                return _manager.GetSWConfigSubFunctionDetails(dto, isExport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        
        [HttpPost("SubFunctionFilter")]
        public List<FilterValueDto> GetSubFunctionFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] SWConfigQueryDto dto)
        {
            try
            {
                var data = _manager.GetFilterForSubFunction(propertyName, propertyFilter, dto);
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
