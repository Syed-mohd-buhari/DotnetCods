using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.ExodusAssetLevelReport;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.ExodusAssetLevelReport;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.ExodusAssetLevelReport;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.Reports.ExodusAssetLevelReport
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExodusAssetLevelReportController : CamControllerBase
    {

        private readonly IExportService _exportService;
        private readonly ExodusAssetLevelReportManager _exodusAssetLevelReport;
        private readonly CommonManager _commonManager;
        private readonly IWebHostEnvironment _env;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<long> _opcoList;
        private readonly List<int> _verticalList;
        private readonly bool _adminRoleCheck=false;

        public ExodusAssetLevelReportController(ICurrentUserService currentUserService,AuthorizedRoleManager authorizedRoleManager,ILoggerManager logger, IHttpContextAccessor contextAccessor,
            IExportService exportService, IWebHostEnvironment env, CommonManager commonManager, ExodusAssetLevelReportManager exodusAssetLevelReport) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _exodusAssetLevelReport = exodusAssetLevelReport;
            _env = env;
            _commonManager = commonManager;

            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            sessionUserId = _currentUserService.UserId;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x=>Convert.ToInt64(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
               _roleOpcoList.VerticalDetails : null;
        }


        #region // UI Member
        [HttpPost("Get")]
        public async Task<QueryResultDto<ExodusAssetLevelReportDtoGrid>> GetExodusReport(
           [FromBody] ExodusAssetLevelReportQueryDto querydto)
        {
            try
            {
                if ((querydto.OpCo == null) || (querydto.OpCo != null && querydto.OpCo.Count == 0))
                    querydto.OpCo = _opcoList;
                if ((querydto.VerticalName == null) || (querydto.VerticalName != null && querydto.VerticalName.Count == 0))
                {
                    querydto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                return await _exodusAssetLevelReport.GetExodusReportLevelReport(querydto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter,
           [FromBody] ExodusAssetLevelReportQueryDto filterDto, bool isInstance = false)
        {
            filterDto.Page = 0;
            filterDto.PageSize = 0;
            if (filterDto.OpCo == null || (filterDto.OpCo != null && filterDto.OpCo.Count == 0))
                filterDto.OpCo = _opcoList;
            if ((filterDto.VerticalName == null) || (filterDto.VerticalName != null && filterDto.VerticalName.Count == 0))
            {
                filterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
            }
            try
            {
                var data = await _exodusAssetLevelReport.GetFilteredValues(propertyName, propertyFilter, filterDto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ExportReport")]
        public async Task<IActionResult> ExportReportLegacyFile([FromBody] ExodusAssetLevelReportQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if (dto.OpCo == null || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoList;
                if ((dto.VerticalName == null) || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                {
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                var data = await _exodusAssetLevelReport.GetExodusReportLevelReport(dto, true);

                var excelConfiguration = _commonManager.GetExcelConfiguration("EXODUS").Result;

                excelConfiguration = excelConfiguration.Where(x => x.Isexportfield == true && x.Processname == "EXODUS").ToList();
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                var templateFileFolder = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                _logger.LogError(templateFileFolder);
                string templateFileName = excelConfiguration.FirstOrDefault(x => x.Templatefilename != null)?.Templatefilename?.Trim();
                _logger.LogError(templateFileName);

                if(templateFileName == null)
                {
                    throw new Exception("Exodus details is missing in Excel template configuration");
                }

                var filePath = Path.Combine(templateFileFolder, "EXODUS", templateFileName);
                _logger.LogError(filePath);


                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);


                var result = _exportService.ExportExcelBasedOnConfiguration(reportSheets,
                                 "Exodus_AssetLevelReport_" + DateTime.Now.ToShortDateString() + ".xlsx", filePath, data.Items.ToList(),
                                 excelConfiguration.ToList());

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
        #endregion

    }
}