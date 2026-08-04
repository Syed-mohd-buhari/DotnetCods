using CAM.BusinessManager;
using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CAM.Infrastucture.QueryResult;
using CAM.Exports;
using System.Linq;
using System;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.VIA;
using System.Text;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class ViaExportSoftwareController : CamControllerBase
    {
        private readonly ViaExportSoftwareManager _ViaExportSoftwareManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IExportService _exportService;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly bool _adminRoleCheck =false;
        private readonly List<short> _opcoList;
        private readonly List<string> _verticalList;
        public ViaExportSoftwareController(ViaExportSoftwareManager ViaExportSoftwareManager,
            ICurrentUserService currentUserService, IIdentityService identityService,
            ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService
              , AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _ViaExportSoftwareManager = ViaExportSoftwareManager;
            _currentUserService = currentUserService;
            _identityService = identityService;
            _exportService = exportService;

            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => x.ToString()).ToList() : null;
        }
        [HttpPost("Get")]

        public QueryResultDto<ViaExport> GetViaExport(
            [FromBody] ViaExportQuery ViaExportSoftwareFilterDto)
        {
            try
            {
                if ((ViaExportSoftwareFilterDto.OrganisationName == null) |(ViaExportSoftwareFilterDto.OrganisationName != null &&
                              ViaExportSoftwareFilterDto.OrganisationName.Count() == 0))
                    ViaExportSoftwareFilterDto.OrganisationNameId = _opcoList;

                if ((ViaExportSoftwareFilterDto.VerticalName == null) ||(ViaExportSoftwareFilterDto.VerticalName != null &&
                  ViaExportSoftwareFilterDto.VerticalName.Count() == 0))
                    ViaExportSoftwareFilterDto.VerticalName = _verticalList;

                return _ViaExportSoftwareManager.FindWithCondition(ViaExportSoftwareFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpPost(template: "ExportViaExport")]
        public FileContentResult ExportViaExport([FromQuery] ViaExportQuery ViaExportSoftwareFilterDto)
        {
            try
            {
                if ((ViaExportSoftwareFilterDto.OrganisationName == null) || (ViaExportSoftwareFilterDto.OrganisationName != null &&
              ViaExportSoftwareFilterDto.OrganisationName.Count() == 0))
                    ViaExportSoftwareFilterDto.OrganisationNameId = _opcoList;

                if ((ViaExportSoftwareFilterDto.VerticalName == null) ||( ViaExportSoftwareFilterDto.VerticalName != null &&
             ViaExportSoftwareFilterDto.VerticalName.Count() == 0))
                    ViaExportSoftwareFilterDto.VerticalName = _verticalList;

                ViaExportSoftwareFilterDto.Page = 0;
                var data = _ViaExportSoftwareManager.FindWithCondition(ViaExportSoftwareFilterDto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };


                List<ExportSheet> ViaExportSheets = new List<ExportSheet>();
                ViaExportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(ViaExportSheets,
                    "VAIExport" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        [HttpPost(template: "ExportViaExportCSV")]
        public FileContentResult ExportViaExportCSV([FromQuery] ViaExportQuery ViaExportSoftwareFilterDto)
        {
            try
            {

                if ((ViaExportSoftwareFilterDto.OrganisationName == null) ||(ViaExportSoftwareFilterDto.OrganisationName != null &&
             ViaExportSoftwareFilterDto.OrganisationName.Count() == 0))
                    ViaExportSoftwareFilterDto.OrganisationNameId = _opcoList;

                if ((ViaExportSoftwareFilterDto.VerticalName == null) ||(ViaExportSoftwareFilterDto.VerticalName != null &&
             ViaExportSoftwareFilterDto.VerticalName.Count() == 0))
                    ViaExportSoftwareFilterDto.VerticalName = _verticalList;

                ViaExportSoftwareFilterDto.Page = 0;
                var data = _ViaExportSoftwareManager.FindWithCondition(ViaExportSoftwareFilterDto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                var fileName = "VAIExport_SW" + DateTime.Now.ToShortDateString() + ".csv";
                var result = _exportService.GetExcelFromCSV(dataSheet, fileName, data.GridRender);
                var file = Encoding.ASCII.GetBytes(result.Stringbuilder.ToString());
                // return File(file, "text/csv", fileName);

                HttpContext.Response.ContentType = result.ContentType;
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                var fileContentResult = new FileContentResult(file, "text/csv")
                {
                    FileDownloadName = fileName
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
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ViaExportQuery ViaExportSoftwareFilterDto)
        {
            try
            {
                if ((ViaExportSoftwareFilterDto.OrganisationName == null) ||(ViaExportSoftwareFilterDto.OrganisationName != null &&
                              ViaExportSoftwareFilterDto.OrganisationName.Count() == 0))
                    ViaExportSoftwareFilterDto.OrganisationNameId = _opcoList;

                if ((ViaExportSoftwareFilterDto.VerticalName == null) ||(ViaExportSoftwareFilterDto.VerticalName != null &&
                 ViaExportSoftwareFilterDto.VerticalName.Count() == 0))
                    ViaExportSoftwareFilterDto.VerticalName = _verticalList;

                return _ViaExportSoftwareManager.GetFilter(propertyName, propertyFilter, ViaExportSoftwareFilterDto,_adminRoleCheck);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
    }
}


