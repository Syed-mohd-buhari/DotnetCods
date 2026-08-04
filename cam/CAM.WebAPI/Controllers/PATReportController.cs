using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Pat;
using CAM.BusinessManager.ExtensionMethod.PAT;
using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.PAT;
using CAM.Exports;
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

    public class PATReportController : CamControllerBase
    {
        private readonly IExportService _exportService;
        private readonly PATManager _PatManager;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly bool _adminRoleCheck=false;
        private readonly List<short> _opcoList;
        private readonly List<int> _verticalList;
        private readonly ICurrentUserService _currentUserService;

        public PATReportController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, PATManager patManager, AuthorizedRoleManager authorizedRoleManager
            , ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _PatManager = patManager;
            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToInt32(x)).Distinct().ToList() : null;

        }
        [HttpPost("GetAll")]
        public QueryResultDto<PTModel> GetAll([FromBody] PATExportQuery paTExportQueryFilterDto)
        {
            try
            {
                if ((paTExportQueryFilterDto.OpCoId == null) || paTExportQueryFilterDto.OpCoId != null && paTExportQueryFilterDto.OpCoId.Count() == 0) paTExportQueryFilterDto.OpCoId = _opcoList;
                if ((paTExportQueryFilterDto.VerticalNameId == null) || paTExportQueryFilterDto.VerticalNameId != null && paTExportQueryFilterDto.VerticalNameId.Count() == 0)
                    paTExportQueryFilterDto.VerticalNameId = _verticalList;

                return _PatManager.FindWithCondition(paTExportQueryFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);

                throw;
            }
        }
        [HttpGet("GetSWOem")]
        public Dictionary<short, string> GetSWOem()
        {
            try
            {
                return _PatManager.GetSWOem();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);

                throw;
            }
        }

        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] PATExportQuery paTExportQueryFilterDto)
        {
            try
            {
                if ((paTExportQueryFilterDto.OpCoId == null) || paTExportQueryFilterDto.OpCoId != null && paTExportQueryFilterDto.OpCoId.Count() == 0)
                    paTExportQueryFilterDto.OpCoId = _opcoList;

                if ((paTExportQueryFilterDto.VerticalNameId == null) || paTExportQueryFilterDto.VerticalNameId != null && paTExportQueryFilterDto.VerticalNameId.Count() == 0)
                    paTExportQueryFilterDto.VerticalNameId = _verticalList;

                return _PatManager.GetFilter(propertyName, propertyFilter, paTExportQueryFilterDto,_adminRoleCheck);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);

                throw;
            }
        }

        [HttpPost(template: "GetVendorsOfPredefinedFilter")]
        public List<short> GetVendorsOfPredefinedFilter(int filterId)
        {
            try
            {
                return _PatManager.GetVendorsOfPredefinedFilter(filterId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);

                throw;
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromQuery] PATExportQuery buildFilterDto)
        {
            try
            {
                if ((buildFilterDto.OpCoId == null) || buildFilterDto.OpCoId != null && buildFilterDto.OpCoId.Count() == 0) buildFilterDto.OpCoId = _opcoList;

                if ((buildFilterDto.VerticalNameId == null) || buildFilterDto.VerticalNameId != null && buildFilterDto.VerticalNameId.Count() == 0)
                    buildFilterDto.VerticalNameId = _verticalList;

                var data = _PatManager.FindWithCondition(buildFilterDto, isExport: true);

                List<ExportSheetCustom> tabs = new List<ExportSheetCustom>();

                List<PTModel> list = data.Items.ToList();
                if (list != null && list.Count != 0)
                {
                    tabs.Add(new ExportSheetCustom
                    {
                        TabName = "PAT",
                        CustomHeaders = list.GetHeaders(),
                        Data = list.GetData()
                    });
                }
                else
                {
                    tabs.Add(new ExportSheetCustom
                    {
                        TabName = "PAT",
                        CustomHeaders = new List<List<ExportSheetCustomHeader>>(),
                        Data = new List<List<ExportSheetCustomCell>>()
                    });
                }

                var result = _exportService.GetExcelFrom(tabs, "ExportReportPAT" + DateTime.Now.ToShortDateString() + ".xlsx" );
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
