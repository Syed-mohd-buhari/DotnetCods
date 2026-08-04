#nullable enable
using CAM.BusinessManager;
using CAM.Contracts;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.Report;
using CAM.DataTransferObjects.VIA;
using CAM.Infrastucture.QueryResult;
using CAM.BusinessManager.Entity.Report;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExportController : CamControllerBase
    {

        private readonly IExportService _exportService;
        private readonly ReportHardwareManager _reportHardwareManager;
        private readonly ReportSoftwareManager _reportSoftwareManager;
        private readonly ViaExportHardwareManager _reportViaHardwareManager;
        private readonly ViaExportSoftwareManager _reportViaSoftwareManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private QueryResultDto<ReportHardwareDtoGrid> hdData;
        private QueryResultDto<ReportSoftwareDtoGrid> swData;
        private ExportSheet hardwareSheet;
        private ExportSheet softwareSheet;
        private List<ExportSheet> reportSheets;
        private CustomGridRender<ExportService> render;
        ExportResult result;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly bool _adminRoleCheck = false;
        private readonly List<short> _opcoList;
        private readonly List<string> _verticalList;

        public ExportController(IExportService exportService, ICurrentUserService currentUserService, IIdentityService identityService, ReportSoftwareManager reportSoftwareManager, ReportHardwareManager reportHardwareManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ViaExportHardwareManager reportViaHardwareManager, ViaExportSoftwareManager reportViaSoftwareManager
            , AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _currentUserService = currentUserService;
            _identityService = identityService;
            _reportSoftwareManager = reportSoftwareManager;
            _reportHardwareManager = reportHardwareManager;
            _reportViaHardwareManager = reportViaHardwareManager;
            _reportViaSoftwareManager = reportViaSoftwareManager;

            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToString(x)).ToList() : null;
        }


        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] ReportQueryAllDto? reportQuery = null)
        {
            try
            {
                List<ExportSheet> reportSheets = new List<ExportSheet>();
                render = new CustomGridRender<ExportService>()
                {
                    Render = new List<RenderDetail>()
                };

                if (reportQuery != null && reportQuery.ActiveTab == "software")
                {
                    if (reportQuery.QuerySoftware != null)
                    {
                        reportQuery.QuerySoftware.Page = 0;
                        reportQuery.QuerySoftware.PageSize = 0;
                    }
                    if ((reportQuery.QuerySoftware.LocalMarket==null) || reportQuery.QuerySoftware.LocalMarket != null && reportQuery.QuerySoftware.LocalMarket.Count() == 0)
                        reportQuery.QuerySoftware.LocalMarketId = _opcoList;

                    if ((reportQuery.QuerySoftware.VerticalEngineeringTeam == null) || reportQuery.QuerySoftware.VerticalEngineeringTeam != null && reportQuery.QuerySoftware.VerticalEngineeringTeam.Count() == 0)
                        reportQuery.QuerySoftware.VerticalEngineeringTeam = _verticalList;

                    swData = _reportSoftwareManager.FindWithCondition(reportQuery?.QuerySoftware ?? new ReportSoftwareQueryDto(), true);
                    softwareSheet = new ExportSheet()
                    {
                        Data = swData.Items.Cast<object>().ToList(),
                        TabName = "Software"
                    };

                    foreach (var nome in swData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");
                    }

                    foreach (var detail in swData.GridRender.Render)
                    {
                        detail.Tab = "Software";
                    }
                    render.Render.AddRange(swData.GridRender.Render);
                    reportSheets.Add(softwareSheet);
                  
                }
                if (reportQuery != null && reportQuery.ActiveTab == "hardware")
                {
                    if (reportQuery.QueryHardware != null)
                    {
                        reportQuery.QueryHardware.Page = 0;
                        reportQuery.QueryHardware.PageSize = 0;
                    }
                    hdData =
                    _reportHardwareManager.FindWithCondition(reportQuery?.QueryHardware ?? new ReportHardwareQueryDto(), true);

                    hardwareSheet = new ExportSheet()
                    {
                        Data = hdData.Items.Cast<object>().ToList(),
                        TabName = "Hardware"
                    };

                    foreach (var nome in hdData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");
                       
                    }
                   
                    foreach (var detail in hdData.GridRender.Render)
                    {
                        detail.Tab = "Hardware";
                    }
                    render.Render.AddRange(hdData.GridRender.Render);
                    reportSheets = new List<ExportSheet>();
                    reportSheets.Add(hardwareSheet);
             
                }
                if (reportQuery != null && reportQuery.ActiveTab == "both")
                {
                    if (reportQuery.QuerySoftware != null)
                    {
                        reportQuery.QuerySoftware.Page = 0;
                        reportQuery.QuerySoftware.PageSize = 0;
                    }
                    if (reportQuery.QueryHardware != null)
                    {
                        reportQuery.QueryHardware.Page = 0;
                        reportQuery.QueryHardware.PageSize = 0;
                    }
                    var hdData =
                   _reportHardwareManager.FindWithCondition(reportQuery?.QueryHardware ?? new ReportHardwareQueryDto(), true);
                    var swData =
                        _reportSoftwareManager.FindWithCondition(reportQuery?.QuerySoftware ?? new ReportSoftwareQueryDto(), true);


                    ExportSheet hardwareSheet = new ExportSheet()
                    {
                        Data = hdData.Items.Cast<object>().ToList(),
                        TabName = "Hardware"
                    };

                    foreach (var nome in hdData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");
                       
                    }


                    ExportSheet softwareSheet = new ExportSheet()
                    {
                        Data = swData.Items.Cast<object>().ToList(),
                        TabName = "Software"
                    };

                    foreach (var nome in swData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");
                    }

                    foreach (var detail in hdData.GridRender.Render)
                    {
                        detail.Tab = "Hardware";
                    }
                    foreach (var detail in swData.GridRender.Render)
                    {
                        detail.Tab = "Software";
                    }
                    render.Render.AddRange(hdData.GridRender.Render);
                    render.Render.AddRange(swData.GridRender.Render);

                    reportSheets.Add(hardwareSheet);
                    reportSheets.Add(softwareSheet);

                }

                var fileName = (reportQuery?.LcmExportDescription ?? "LCM DB Export") + "_" + DateTime.Now.ToShortDateString() + ".xlsx";
                result = _exportService.GetExcelFrom(reportSheets, fileName , render);

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

        [HttpPost(template: "ExportViaReport")]
        public FileContentResult ExportViaReport([FromBody] ReportViaQueryAllDto? reportQuery = null)
        {
            try
            {
                if (reportQuery != null)
                {
                    if (reportQuery.QuerySoftware != null)
                    {
                        reportQuery.QuerySoftware.Page = 0;
                        reportQuery.QuerySoftware.PageSize = 0;
                    }
                    if (reportQuery.QueryHardware != null)
                    {
                        reportQuery.QueryHardware.Page = 0;
                        reportQuery.QueryHardware.PageSize = 0;
                    }
                }

                var hdData =
                    _reportViaHardwareManager.FindWithCondition(reportQuery?.QueryHardware ?? new ViaExportQuery());
                var swData =
                    _reportViaSoftwareManager.FindWithCondition(reportQuery?.QuerySoftware ?? new ViaExportQuery());


                ExportSheet hardwareSheet = new ExportSheet()
                {
                    Data = hdData.Items.Cast<object>().ToList(),
                    TabName = "Hardware"
                };


                ExportSheet softwareSheet = new ExportSheet()
                {
                    Data = swData.Items.Cast<object>().ToList(),
                    TabName = "Software"
                };

                CustomGridRender<ExportService> render = new CustomGridRender<ExportService>()
                {
                    Render = new List<RenderDetail>()
                };

                foreach (var detail in hdData.GridRender.Render)
                {
                    detail.Tab = "Hardware";
                }
                foreach (var detail in swData.GridRender.Render)
                {
                    detail.Tab = "Software";
                }
                render.Render.AddRange(hdData.GridRender.Render);
                render.Render.AddRange(swData.GridRender.Render);

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(hardwareSheet);
                reportSheets.Add(softwareSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "VAI-Export" + DateTime.Now.ToShortDateString() + ".xlsx", render);

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
