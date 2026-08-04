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
using CAM.BusinessManager.Entity.Vai;
using CAM.DataTransferObjects.VIA;
using Newtonsoft.Json;
using System.Text;
using System.IO;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ViaExportHardwareController : CamControllerBase
    {
        private readonly ViaExportHardwareManager _ViaExportHardwareManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IExportService _exportService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly bool _adminRoleCheck=false;
        private readonly List<short> _opcoList;
        private readonly List<string> _verticalList;

        public ViaExportHardwareController(ViaExportHardwareManager ViaExportHardwareManager, ICurrentUserService currentUserService,
            IIdentityService identityService, ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService           
            , AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _ViaExportHardwareManager = ViaExportHardwareManager;
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
                _roleOpcoList.VerticalDetails.Select(x=>x.ToString()).ToList() : null;
        }
        [HttpPost("Get")]
        public QueryResultDtoVai GetViaExport(
            [FromBody] ViaExportQuery ViaExportHardwareFilterDto)
        {
            try
            {
                if (ViaExportHardwareFilterDto.OrganisationName == null ||( ViaExportHardwareFilterDto.OrganisationName != null &&
                         ViaExportHardwareFilterDto.OrganisationName.Count() == 0))
                    ViaExportHardwareFilterDto.OrganisationNameId = _opcoList;

                if (ViaExportHardwareFilterDto.VerticalName == null || (ViaExportHardwareFilterDto.VerticalName != null &&
                         ViaExportHardwareFilterDto.VerticalName.Count() == 0))
                    ViaExportHardwareFilterDto.VerticalName = _verticalList;

                return _ViaExportHardwareManager.FindWithCondition(ViaExportHardwareFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "ExportViaExport")]
        public FileContentResult ExportViaExport([FromQuery] ViaExportQuery ViaExportHardwareFilterDto)
        {
            try
            {
                if (ViaExportHardwareFilterDto.OrganisationName == null || (ViaExportHardwareFilterDto.OrganisationName != null &&
       ViaExportHardwareFilterDto.OrganisationName.Count() == 0))
                    ViaExportHardwareFilterDto.OrganisationNameId = _opcoList;

                if (ViaExportHardwareFilterDto.VerticalName == null || (ViaExportHardwareFilterDto.VerticalName != null &&
                     ViaExportHardwareFilterDto.VerticalName.Count() == 0))
                    ViaExportHardwareFilterDto.VerticalName = _verticalList;

                ViaExportHardwareFilterDto.Page = 0;
                var data = _ViaExportHardwareManager.FindWithCondition(ViaExportHardwareFilterDto);

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
        public FileContentResult ExportViaExportCSV([FromQuery] ViaExportQuery ViaExportHardwareFilterDto)
        {
            try
            {

                if ((ViaExportHardwareFilterDto.OrganisationName == null)  || (ViaExportHardwareFilterDto.OrganisationName != null &&
        ViaExportHardwareFilterDto.OrganisationName.Count() == 0))
                    ViaExportHardwareFilterDto.OrganisationNameId = _opcoList;

                if ((ViaExportHardwareFilterDto.VerticalName == null)  ||(ViaExportHardwareFilterDto.VerticalName != null &&
                     ViaExportHardwareFilterDto.VerticalName.Count() == 0))
                    ViaExportHardwareFilterDto.VerticalName = _verticalList;

                ViaExportHardwareFilterDto.Page = 0;
                var data = _ViaExportHardwareManager.FindWithCondition(ViaExportHardwareFilterDto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                var fileName = "VAIExport_HW" + DateTime.Now.ToShortDateString() + ".csv";
                var result = _exportService.GetExcelFromCSV(dataSheet,fileName, data.GridRender);

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
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ViaExportQuery ViaExportHardwareFilterDto)
        {
            try
            {
                if ((ViaExportHardwareFilterDto.OrganisationName == null) ||(ViaExportHardwareFilterDto.OrganisationName != null &&
      ViaExportHardwareFilterDto.OrganisationName.Count() == 0))
                    ViaExportHardwareFilterDto.OrganisationNameId = _opcoList;

                if ((ViaExportHardwareFilterDto.VerticalName == null) ||(ViaExportHardwareFilterDto.VerticalName != null &&
                         ViaExportHardwareFilterDto.VerticalName.Count() == 0))
                    ViaExportHardwareFilterDto.VerticalName = _verticalList;

                return _ViaExportHardwareManager.GetFilter(propertyName, propertyFilter, ViaExportHardwareFilterDto,_adminRoleCheck);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
    }
}


