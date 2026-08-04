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
using System;
using System.Linq;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.Report;
using CAM.BusinessManager.Entity.Report;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportSoftwareController : CamControllerBase
    {

        private readonly ReportSoftwareManager _reportSoftwareManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IExportService _exportService;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly bool _adminRoleCheck=false;
        private readonly List<short> _opcoList;
        private readonly List<string> _verticalList;

        public ReportSoftwareController(ReportSoftwareManager reportSoftwareManager,
            ICurrentUserService currentUserService, IIdentityService identityService, 
            ILoggerManager logger, IExportService exportService, IHttpContextAccessor contextAccessor, AuthorizedRoleManager authorizedRoleManager  ) : base(logger, contextAccessor)
        {
            _reportSoftwareManager = reportSoftwareManager;
            _currentUserService = currentUserService;
            _identityService = identityService;
            _exportService = exportService;

            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x=>Convert.ToString(x)).Distinct().ToList() : null;

        }
        [HttpPost("Get")]

        public QueryResultDto<ReportSoftwareDtoGrid> GetReport(
            [FromBody] ReportSoftwareQueryDto reportSoftwareFilterDto)
        {
            try
            {
                if ((reportSoftwareFilterDto.LocalMarket == null) || reportSoftwareFilterDto.LocalMarket != null && reportSoftwareFilterDto.LocalMarket.Count() == 0)
                    reportSoftwareFilterDto.LocalMarketId = _opcoList;

                if ((reportSoftwareFilterDto.VerticalEngineeringTeam == null) || reportSoftwareFilterDto.VerticalEngineeringTeam != null && reportSoftwareFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSoftwareFilterDto.VerticalEngineeringTeam = _verticalList;

                return _reportSoftwareManager.FindWithCondition(reportSoftwareFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] ReportSoftwareQueryDto reportSoftwareFilterDto)
        {
            try
            {
                reportSoftwareFilterDto.Page = 0;
                if ((reportSoftwareFilterDto.LocalMarket == null) || reportSoftwareFilterDto.LocalMarket != null && reportSoftwareFilterDto.LocalMarket.Count() == 0)
                    reportSoftwareFilterDto.LocalMarketId = _opcoList;

                if ((reportSoftwareFilterDto.VerticalEngineeringTeam == null) || reportSoftwareFilterDto.VerticalEngineeringTeam != null && reportSoftwareFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSoftwareFilterDto.VerticalEngineeringTeam = _verticalList;

                var data = _reportSoftwareManager.FindWithCondition(reportSoftwareFilterDto);
                
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
                    reportSoftwareFilterDto.LcmExportDescription?? "LCM DB Export" +"_"+ DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName,[FromQuery] string propertyFilter, [FromBody] ReportSoftwareQueryDto reportSoftwawreFilterDto)
        {
            try
            {
                if ((reportSoftwawreFilterDto.LocalMarket == null) || reportSoftwawreFilterDto.LocalMarket != null && reportSoftwawreFilterDto.LocalMarket.Count() == 0)
                    reportSoftwawreFilterDto.LocalMarketId = _opcoList;

                if ((reportSoftwawreFilterDto.VerticalEngineeringTeam == null) || reportSoftwawreFilterDto.VerticalEngineeringTeam != null && reportSoftwawreFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSoftwawreFilterDto.VerticalEngineeringTeam = _verticalList;

                return _reportSoftwareManager.GetFilter(propertyName, propertyFilter, reportSoftwawreFilterDto,_adminRoleCheck);
            }
           
             catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
    }
}


