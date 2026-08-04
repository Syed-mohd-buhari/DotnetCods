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
using CAM.DataTransferObjects.Entita.Report;
using CAM.BusinessManager.Entity.Report;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportHardwareController : CamControllerBase
    {
        private readonly ReportHardwareManager _reportHardwareManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IExportService _exportService;

         private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly bool _adminRoleCheck =false;
        private readonly List<short> _opcoList;
        private readonly List<string> _verticalList;
        
        public ReportHardwareController(ReportHardwareManager reportHardwareManager,
            ICurrentUserService currentUserService, IIdentityService identityService, 
            ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, 
            AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _reportHardwareManager = reportHardwareManager;
            _currentUserService = currentUserService;
            _identityService = identityService;
            _exportService = exportService;
            
            _authorizedRoleManager = authorizedRoleManager;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x=>Convert.ToString(x)).Distinct().ToList() : null;

        }

        [HttpPost("Get")]
        public QueryResultDto<ReportHardwareDtoGrid> GetReport(
            [FromBody] ReportHardwareQueryDto reportHardwareFilterDto)
        {
            try
            {
                if ((reportHardwareFilterDto.LocalMarket == null) || reportHardwareFilterDto.LocalMarket != null && reportHardwareFilterDto.LocalMarket.Count() == 0)
                    reportHardwareFilterDto.LocalMarketId = _opcoList;

                if ((reportHardwareFilterDto.VerticalEngineeringTeam == null) || reportHardwareFilterDto.VerticalEngineeringTeam != null && reportHardwareFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportHardwareFilterDto.VerticalEngineeringTeam = _verticalList;

                return _reportHardwareManager.FindWithCondition(reportHardwareFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] ReportHardwareQueryDto reportHardwareFilterDto)
        {
            try
            {
                reportHardwareFilterDto.Page = 0;

                if ((reportHardwareFilterDto.LocalMarket == null) || reportHardwareFilterDto.LocalMarket != null && reportHardwareFilterDto.LocalMarket.Count() == 0)
                    reportHardwareFilterDto.LocalMarketId = _opcoList;

                if ((reportHardwareFilterDto.VerticalEngineeringTeam == null) ||reportHardwareFilterDto.VerticalEngineeringTeam != null && reportHardwareFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportHardwareFilterDto.VerticalEngineeringTeam = _verticalList;


                var data = _reportHardwareManager.FindWithCondition(reportHardwareFilterDto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                foreach (var nome in data.Items)
                {
                    nome.AssetClass = nome.AssetClass.Replace("<b class=\"text-lowercase\">", "");
                    nome.AssetClass = nome.AssetClass.Replace("</b>", "");
                }
                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    reportHardwareFilterDto.LcmExportDescription ?? "LCM DB Export" + "_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ReportHardwareQueryDto reportHardwareFilterDto)
        {
            try
            {
                if ((reportHardwareFilterDto.LocalMarket == null) || reportHardwareFilterDto.LocalMarket != null && reportHardwareFilterDto.LocalMarket.Count() == 0)
                    reportHardwareFilterDto.LocalMarketId = _opcoList;

                if ((reportHardwareFilterDto.VerticalEngineeringTeam == null) || reportHardwareFilterDto.VerticalEngineeringTeam != null && reportHardwareFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportHardwareFilterDto.VerticalEngineeringTeam = _verticalList;

                return _reportHardwareManager.GetFilter(propertyName, propertyFilter, reportHardwareFilterDto,_adminRoleCheck);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }
    }
}


