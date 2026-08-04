using CAM.Contracts;
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
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.Entita.DesignComponentFamily;
using CAM.DataTransferObjects;
using CAM.Infrastucture;
using DocumentFormat.OpenXml.EMMA;
using System.Web.Http.Results;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportHardwareSoftwareController : CamControllerBase
    {

        private readonly ReportSubnetWorkManager _reportSubnetWorkManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IExportService _exportService;

        private readonly NetworkElementLevelTwoManager _levelTwoReportSoftwareManager;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly bool _adminRoleCheck=false;
        private readonly List<short> _opcoList;
        private readonly List<string> _verticalList;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private ReportHardwareConfigurationManager _reportHardwareConfigurationManager;
        private QueryResultDto<ReportSubnetWorkHardWareGrid> hdSubnetWorkData;
        private QueryResultDto<ReportSubnetWorkSoftWareGrid> swSubnetWorkData;
        private ExportSheet hardwareSubnetWorkSheet;
        private ExportSheet softwareSubnetWorkSheet;
        private List<ExportSheet> reportSheets;
        private CustomGridRender<ExportService> render;
        private readonly ReportSoftwareManager _reportSoftwareManager;
        ExportResult result;

        public ReportHardwareSoftwareController(ReportSubnetWorkManager reportSubnetWorkManager, NetworkElementLevelTwoManager levelTwoReportSoftwareManager,
            ICurrentUserService currentUserService, IIdentityService identityService, 
            ILoggerManager logger, IExportService exportService, IHttpContextAccessor contextAccessor, AuthorizedRoleManager authorizedRoleManager
            , DropdownDataServiceManager dropdownDataServiceManager, ReportHardwareConfigurationManager reportHardwareConfigurationManager, ReportSoftwareManager reportSoftwareManager) : base(logger, contextAccessor)
        {
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _reportSubnetWorkManager = reportSubnetWorkManager;
            _currentUserService = currentUserService;
            _identityService = identityService;
            _exportService = exportService;

            _levelTwoReportSoftwareManager = levelTwoReportSoftwareManager;

            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x=>Convert.ToString(x)).ToList() : null;
            _reportSubnetWorkManager = reportSubnetWorkManager;
            _reportHardwareConfigurationManager = reportHardwareConfigurationManager;
            _reportSoftwareManager = reportSoftwareManager;
        }


        #region //NetworkLevelTwoReport

        [HttpPost("GetNetworkLevelTwoRecord")]
        public QueryResultDto<NetworkLevelTwoSWReportDtoGrid> GetLevelTwoReport(
          [FromBody] ReportSoftwareQueryDto reportSoftwareFilterDto)
        {
            try
            {
                if ((reportSoftwareFilterDto.LocalMarket == null) || reportSoftwareFilterDto.LocalMarket != null && reportSoftwareFilterDto.LocalMarket.Count() == 0)
                    reportSoftwareFilterDto.LocalMarketId = _opcoList;

                if ((reportSoftwareFilterDto.VerticalEngineeringTeam == null) || reportSoftwareFilterDto.VerticalEngineeringTeam != null && reportSoftwareFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSoftwareFilterDto.VerticalEngineeringTeam = _verticalList;
                return _levelTwoReportSoftwareManager.FindDisaggregatedWithCondition(reportSoftwareFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost(template: "NetworkLevelTwoExport")]
        public FileContentResult ExportReport([FromBody] ReportSoftwareQueryDto reportSoftwareFilterDto)
        {
            try
            {
                reportSoftwareFilterDto.Page = 0;
                reportSoftwareFilterDto.PageSize = 0;
                if ((reportSoftwareFilterDto.LocalMarket == null) || reportSoftwareFilterDto.LocalMarket != null && reportSoftwareFilterDto.LocalMarket.Count() == 0)
                    reportSoftwareFilterDto.LocalMarketId = _opcoList;

                if ((reportSoftwareFilterDto.VerticalEngineeringTeam == null) || reportSoftwareFilterDto.VerticalEngineeringTeam != null && reportSoftwareFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSoftwareFilterDto.VerticalEngineeringTeam = _verticalList;

                var data = _levelTwoReportSoftwareManager.FindDisaggregatedWithCondition(reportSoftwareFilterDto,true);

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


                var result = _exportService.GetExcelFrom(reportSheets, "Network Element - Level2" + "_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        [HttpPost(template: "NetworkLevelTwoFilter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ReportSoftwareQueryDto reportSoftwawreFilterDto)
        {
            try
            {
                if ((reportSoftwawreFilterDto.LocalMarket == null) || reportSoftwawreFilterDto.LocalMarket != null && reportSoftwawreFilterDto.LocalMarket.Count() == 0)
                    reportSoftwawreFilterDto.LocalMarketId = _opcoList;

                if ((reportSoftwawreFilterDto.VerticalEngineeringTeam == null) || reportSoftwawreFilterDto.VerticalEngineeringTeam != null && reportSoftwawreFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSoftwawreFilterDto.VerticalEngineeringTeam = _verticalList;

                return _levelTwoReportSoftwareManager.GetDisaggregatedFilter(propertyName, propertyFilter, reportSoftwawreFilterDto,_adminRoleCheck);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        #endregion

        #region // Subnetwork Software Report
        [HttpPost("GetSubnetWorkSoftwareReport")]
        public QueryResultDto<ReportSubnetWorkSoftWareGrid> GetSubnetWorkSoftwareReport(
           [FromBody] ReportSubnetWorkQueryDto reportSubnetWorkFilterDto)
        {
            try
            {
                if (reportSubnetWorkFilterDto.LocalMarket==null || reportSubnetWorkFilterDto.LocalMarket != null && reportSubnetWorkFilterDto.LocalMarket.Count() == 0)
                    reportSubnetWorkFilterDto.LocalMarketId = _opcoList;

                if ((reportSubnetWorkFilterDto.VerticalEngineeringTeam == null) || reportSubnetWorkFilterDto.VerticalEngineeringTeam != null && reportSubnetWorkFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSubnetWorkFilterDto.VerticalEngineeringTeam = _verticalList;

                return _reportSubnetWorkManager.FindAggregatedSoftwareWithCondition(reportSubnetWorkFilterDto, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost(template: "ExportForSoftwareSubnetworkReport")]
        public FileContentResult ExportReportForSoftware([FromBody] ReportSubnetWorkQueryDto reportSubnetWorkFilterDto)
        {
            try
            {
                reportSubnetWorkFilterDto.Page = 0;
                reportSubnetWorkFilterDto.PageSize = 0;
                if ((reportSubnetWorkFilterDto.LocalMarket == null) || reportSubnetWorkFilterDto.LocalMarket != null && reportSubnetWorkFilterDto.LocalMarket.Count() == 0)
                    reportSubnetWorkFilterDto.LocalMarketId = _opcoList;

                if ((reportSubnetWorkFilterDto.VerticalEngineeringTeam == null) || reportSubnetWorkFilterDto.VerticalEngineeringTeam != null && reportSubnetWorkFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSubnetWorkFilterDto.VerticalEngineeringTeam = _verticalList;

                var data = _reportSubnetWorkManager.FindAggregatedSoftwareWithCondition(reportSubnetWorkFilterDto, true);

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
                    "Subnetwork Software" + "_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        [HttpPost(template: "GetSoftwareSubnetworkFilter")]
        public List<FilterValueDto> GetSoftwareFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ReportSubnetWorkQueryDto reportSubnetWorkFilterDto)
        {
            try
            {
                if (reportSubnetWorkFilterDto.LocalMarket == null || reportSubnetWorkFilterDto.LocalMarket != null && reportSubnetWorkFilterDto.LocalMarket.Count() == 0)
                    reportSubnetWorkFilterDto.LocalMarketId = _opcoList;

                if ((reportSubnetWorkFilterDto.VerticalEngineeringTeam == null) || reportSubnetWorkFilterDto.VerticalEngineeringTeam != null && reportSubnetWorkFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSubnetWorkFilterDto.VerticalEngineeringTeam = _verticalList;

                return _reportSubnetWorkManager.GetAggregatedSoftwareFilter(propertyName, propertyFilter, reportSubnetWorkFilterDto,_adminRoleCheck);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        #endregion

        #region //Subnetwork Hardware Report
        [HttpPost("GetSubnetWorkHardwareReport")]
        public QueryResultDto<ReportSubnetWorkHardWareGrid> GetSubnetWorkHardwareReport(
               [FromBody] ReportSubnetWorkQueryDto reportSubnetWorkFilterDto)
        {
            try
            {
                if (reportSubnetWorkFilterDto.LocalMarket == null  || reportSubnetWorkFilterDto.LocalMarket != null && reportSubnetWorkFilterDto.LocalMarket.Count() == 0)
                    reportSubnetWorkFilterDto.LocalMarketId = _opcoList;

                if ((reportSubnetWorkFilterDto.VerticalEngineeringTeam == null) || reportSubnetWorkFilterDto.VerticalEngineeringTeam != null && reportSubnetWorkFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSubnetWorkFilterDto.VerticalEngineeringTeam = _verticalList;

                return _reportSubnetWorkManager.FindAggregatedHardWareWithCondition(reportSubnetWorkFilterDto, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }
       

        [HttpPost(template: "ExportForHardwareSubnetworkReport")]
        public FileContentResult ExportReportForHardware([FromBody] ReportSubnetWorkQueryDto reportSubnetWorkFilterDto)
        {
            try
            {
                reportSubnetWorkFilterDto.Page = 0;
                reportSubnetWorkFilterDto.PageSize = 0;
                if (reportSubnetWorkFilterDto.LocalMarket != null && reportSubnetWorkFilterDto.LocalMarket.Count() == 0)
                    reportSubnetWorkFilterDto.LocalMarketId = _opcoList;

                if ((reportSubnetWorkFilterDto.VerticalEngineeringTeam == null) || reportSubnetWorkFilterDto.VerticalEngineeringTeam != null && reportSubnetWorkFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSubnetWorkFilterDto.VerticalEngineeringTeam = _verticalList;

                var data = _reportSubnetWorkManager.FindAggregatedSoftwareWithCondition(reportSubnetWorkFilterDto,true);

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


                var result = _exportService.GetExcelFrom(reportSheets, "Subnetwork Hardware" + "_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

       
        [HttpPost(template: "GetHardwareSubnetworkFilter")]
        public List<FilterValueDto> GetHardwareFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ReportSubnetWorkQueryDto reportSubnetWorkFilterDto)
        {

            try
            {
                if (reportSubnetWorkFilterDto.LocalMarket ==null ||reportSubnetWorkFilterDto.LocalMarket != null && reportSubnetWorkFilterDto.LocalMarket.Count() == 0)
                    reportSubnetWorkFilterDto.LocalMarketId = _opcoList;

                if ((reportSubnetWorkFilterDto.VerticalEngineeringTeam == null) || reportSubnetWorkFilterDto.VerticalEngineeringTeam != null && reportSubnetWorkFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportSubnetWorkFilterDto.VerticalEngineeringTeam = _verticalList;

                return _reportSubnetWorkManager.GetAggregatedHardWareFilter(propertyName, propertyFilter, reportSubnetWorkFilterDto ,_adminRoleCheck);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        #endregion

        #region // Hw Configuration
        [HttpPost(template: "GetHwConfigurationFilter")]
        public List<FilterValueDto> GetHwConfigurationFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ReportHardwareConfigurationQueryDto reportHwConfigurationFilterDto)
        {
            try
            {
                if ((reportHwConfigurationFilterDto.LocalMarket==null) || reportHwConfigurationFilterDto.LocalMarket != null && reportHwConfigurationFilterDto.LocalMarket.Count() == 0)
                    reportHwConfigurationFilterDto.LocalMarketId = _opcoList;

                if ((reportHwConfigurationFilterDto.VerticalEngineeringTeam == null) || reportHwConfigurationFilterDto.VerticalEngineeringTeam != null && reportHwConfigurationFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportHwConfigurationFilterDto.VerticalEngineeringTeam = _verticalList;
                return _reportHardwareConfigurationManager.GetDisaggregatedHWConfigurationFilter(propertyName, propertyFilter, reportHwConfigurationFilterDto,_adminRoleCheck);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("GetHwConfigurationReport")]
        public QueryResultDto<ReportHardwareConfigurationDtoGrid> GetHwConfigurationReport(
       [FromBody] ReportHardwareConfigurationQueryDto reportHwConfigurationFilterDto)
        {
            try
            {
                if ((reportHwConfigurationFilterDto.LocalMarket == null) || reportHwConfigurationFilterDto.LocalMarket != null && reportHwConfigurationFilterDto.LocalMarket.Count() == 0)
                    reportHwConfigurationFilterDto.LocalMarketId = _opcoList;

                if ((reportHwConfigurationFilterDto.VerticalEngineeringTeam == null) || reportHwConfigurationFilterDto.VerticalEngineeringTeam != null && reportHwConfigurationFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportHwConfigurationFilterDto.VerticalEngineeringTeam = _verticalList;

                return _reportHardwareConfigurationManager.FindDisaggregatedHWConfigurationWithCondition(reportHwConfigurationFilterDto, false);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost(template: "ExportHwConfigurationReport")]
        public FileContentResult ExportHwConfigurationReport([FromBody] ReportHardwareConfigurationQueryDto reportHwConfigurationFilterDto)
        {
            try
            {
                reportHwConfigurationFilterDto.Page = 0;
                reportHwConfigurationFilterDto.PageSize = 0;
                if ((reportHwConfigurationFilterDto.LocalMarket == null) || reportHwConfigurationFilterDto.LocalMarket != null && reportHwConfigurationFilterDto.LocalMarket.Count() == 0)
                    reportHwConfigurationFilterDto.LocalMarketId = _opcoList;

                if ((reportHwConfigurationFilterDto.VerticalEngineeringTeam == null) || reportHwConfigurationFilterDto.VerticalEngineeringTeam != null && reportHwConfigurationFilterDto.VerticalEngineeringTeam.Count() == 0)
                    reportHwConfigurationFilterDto.VerticalEngineeringTeam = _verticalList;

                var data = _reportHardwareConfigurationManager.FindDisaggregatedHWConfigurationWithCondition(reportHwConfigurationFilterDto ,true);

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


                var result = _exportService.GetExcelFrom(reportSheets, "Hardware" + "_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        #region // Subnetwork HW,SW and Full Export
        [HttpPost(template: "SubnetworkExportReport")]
        public FileContentResult ExportReport([FromBody] ReportQueryAllDto? reportQuery = null)
        {
            try
            {
                List<ExportSheet> reportSheets = new List<ExportSheet>();
                render = new CustomGridRender<ExportService>()
                {
                    Render = new List<RenderDetail>()
                };
                if (reportQuery.QuerySubnetworkHardware.LocalMarket==null ||reportQuery.QuerySubnetworkHardware.LocalMarket != null && reportQuery.QuerySubnetworkHardware.LocalMarket.Count() == 0)
                    reportQuery.QuerySubnetworkHardware.LocalMarketId = _opcoList;

                if ((reportQuery.QuerySubnetworkHardware.VerticalEngineeringTeam == null) || reportQuery.QuerySubnetworkHardware.VerticalEngineeringTeam != null && reportQuery.QuerySubnetworkHardware.VerticalEngineeringTeam.Count() == 0)
                    reportQuery.QuerySubnetworkHardware.VerticalEngineeringTeam = _verticalList;

                if (reportQuery.QuerySubnetworkSoftware.LocalMarket==null ||reportQuery.QuerySubnetworkSoftware.LocalMarket != null && reportQuery.QuerySubnetworkSoftware.LocalMarket.Count() == 0)
                    reportQuery.QuerySubnetworkSoftware.LocalMarketId = _opcoList;

                if ((reportQuery.QuerySubnetworkSoftware.VerticalEngineeringTeam == null) || reportQuery.QuerySubnetworkSoftware.VerticalEngineeringTeam != null && reportQuery.QuerySubnetworkSoftware.VerticalEngineeringTeam.Count() == 0)
                    reportQuery.QuerySubnetworkSoftware.VerticalEngineeringTeam = _verticalList;
                if (reportQuery != null && reportQuery.ActiveTab == "software")
                {
                    if (reportQuery.QuerySoftware != null)
                    {
                        reportQuery.QuerySoftware.Page = 0;
                        reportQuery.QuerySoftware.PageSize = 0;
                    }
                    swSubnetWorkData = _reportSubnetWorkManager.FindAggregatedSoftwareWithCondition(reportQuery?.QuerySubnetworkSoftware ?? new ReportSubnetWorkQueryDto(), true);
                    softwareSubnetWorkSheet = new ExportSheet()
                    {
                        Data = swSubnetWorkData.Items.Cast<object>().ToList(),
                        TabName = "Software"
                    };

                    foreach (var nome in swSubnetWorkData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");
                    }

                    foreach (var detail in swSubnetWorkData.GridRender.Render)
                    {
                        detail.Tab = "Software";
                    }
                    render.Render.AddRange(swSubnetWorkData.GridRender.Render);
                    reportSheets.Add(softwareSubnetWorkSheet);

                }
                if (reportQuery != null && reportQuery.ActiveTab == "hardware")
                {
                    if (reportQuery.QueryHardware != null)
                    {
                        reportQuery.QueryHardware.Page = 0;
                        reportQuery.QueryHardware.PageSize = 0;
                    }
                    hdSubnetWorkData =
                    _reportSubnetWorkManager.FindAggregatedHardWareWithCondition(reportQuery?.QuerySubnetworkHardware ?? new ReportSubnetWorkQueryDto(), true);

                    hardwareSubnetWorkSheet = new ExportSheet()
                    {
                        Data = hdSubnetWorkData.Items.Cast<object>().ToList(),
                        TabName = "Hardware"
                    };

                    foreach (var nome in hdSubnetWorkData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");

                    }

                    foreach (var detail in hdSubnetWorkData.GridRender.Render)
                    {
                        detail.Tab = "Hardware";
                    }
                    render.Render.AddRange(hdSubnetWorkData.GridRender.Render);
                    reportSheets = new List<ExportSheet>();
                    reportSheets.Add(hardwareSubnetWorkSheet);

                }
                if (reportQuery != null && reportQuery.ActiveTab == "both")
                {
                    if (reportQuery.QuerySubnetworkHardware != null)
                    {
                        reportQuery.QuerySubnetworkHardware.Page = 0;
                        reportQuery.QuerySubnetworkHardware.PageSize = 0;
                    }
                    if (reportQuery.QuerySubnetworkSoftware != null)
                    {
                        reportQuery.QuerySubnetworkSoftware.Page = 0;
                        reportQuery.QuerySubnetworkSoftware.PageSize = 0;
                    }
                    var hdSubnetworkData =
                        _reportSubnetWorkManager.FindAggregatedHardWareWithCondition(reportQuery?.QuerySubnetworkHardware ?? new ReportSubnetWorkQueryDto(), true);
                    var swSubnetworkData =
                        _reportSubnetWorkManager.FindAggregatedSoftwareWithCondition(reportQuery?.QuerySubnetworkSoftware ?? new ReportSubnetWorkQueryDto(), true);


                    ExportSheet hardwareSheet = new ExportSheet()
                    {
                        Data = hdSubnetworkData.Items.Cast<object>().ToList(),
                        TabName = "Hardware"
                    };

                    foreach (var nome in hdSubnetworkData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");

                    }


                    ExportSheet softwareSheet = new ExportSheet()
                    {
                        Data = swSubnetworkData.Items.Cast<object>().ToList(),
                        TabName = "Software"
                    };

                    foreach (var nome in swSubnetworkData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");
                    }

                    foreach (var detail in hdSubnetworkData.GridRender.Render)
                    {
                        detail.Tab = "Hardware";
                    }
                    foreach (var detail in swSubnetworkData.GridRender.Render)
                    {
                        detail.Tab = "Software";
                    }
                    render.Render.AddRange(hdSubnetworkData.GridRender.Render);
                    render.Render.AddRange(swSubnetworkData.GridRender.Render);

                    reportSheets.Add(hardwareSheet);
                    reportSheets.Add(softwareSheet);

                }

                var fileName = "SubnetworkExportReport" + "_" + DateTime.Now.ToShortDateString() + ".xlsx";
                result = _exportService.GetExcelFrom(reportSheets, fileName, render);

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

        [HttpPost(template: "ExportAllReport")]
        public FileContentResult ExportAllReport([FromBody] ReportQueryAllDto? reportQuery = null)
        {
            try
            {
                List<ExportSheet> reportSheets = new List<ExportSheet>();
                render = new CustomGridRender<ExportService>()
                {
                    Render = new List<RenderDetail>()
                };

                    if (reportQuery.QuerySubnetworkHardware != null)
                    {
                        reportQuery.QuerySubnetworkHardware.Page = 0;
                        reportQuery.QuerySubnetworkHardware.PageSize = 0;
                    }
                    if (reportQuery.QuerySubnetworkSoftware != null)
                    {
                        reportQuery.QuerySubnetworkSoftware.Page = 0;
                        reportQuery.QuerySubnetworkSoftware.PageSize = 0;
                    }
                    if (reportQuery.QueryHardwareConfiguration != null)
                    {
                        reportQuery.QueryHardwareConfiguration.Page = 0;
                        reportQuery.QueryHardwareConfiguration.PageSize = 0;
                    }
                    if (reportQuery.QuerySoftware != null)
                    {
                        reportQuery.QuerySoftware.Page = 0;
                        reportQuery.QuerySoftware.PageSize = 0;
                        reportQuery.QuerySoftware.ViewMode = Enum.ReportViewMode.Disaggregated;
                    }
                    if (reportQuery.QuerySoftwareLevelTwo != null)
                    {
                        reportQuery.QuerySoftwareLevelTwo.Page = 0;
                        reportQuery.QuerySoftwareLevelTwo.PageSize = 0;
                    }
                    var hdSubnetworkData =
                        _reportSubnetWorkManager.FindAggregatedHardWareWithConditionAsync(reportQuery?.QuerySubnetworkHardware ?? new ReportSubnetWorkQueryDto(), true).Result;
                    var swSubnetworkData =
                        _reportSubnetWorkManager.FindAggregatedSoftwareWithConditionAsync(reportQuery?.QuerySubnetworkSoftware ?? new ReportSubnetWorkQueryDto(), true).Result;
                    var hwConfigurationData =
                          _reportHardwareConfigurationManager.FindDisaggregatedHWConfigurationWithConditionAsync(reportQuery?.QueryHardwareConfiguration ?? new ReportHardwareConfigurationQueryDto(), true).Result;
                    var levelTwoData =
                        _levelTwoReportSoftwareManager.FindDisaggregatedWithConditionAsync(reportQuery?.QuerySoftwareLevelTwo ?? new ReportSoftwareQueryDto(), true).Result;
                    var levelOneData =
                        _reportSoftwareManager.FindWithCondition(reportQuery?.QuerySoftware ?? new ReportSoftwareQueryDto(), true);

                    ExportSheet hardwareSheet = new ExportSheet()
                    {
                        Data = hdSubnetworkData.Items.Cast<object>().ToList(),
                        TabName = "SubnetworkHardware"
                    };

                    ExportSheet softwareSheet = new ExportSheet()
                    {
                        Data = swSubnetworkData.Items.Cast<object>().ToList(),
                        TabName = "SubnetworkSoftware"
                    };

                    ExportSheet hwConfigurationSheet = new ExportSheet()
                    {
                        Data = hwConfigurationData.Items.Cast<object>().ToList(),
                        TabName = "Hardware"
                    };

                    ExportSheet levelTwoSheet = new ExportSheet()
                    {
                        Data = levelTwoData.Items.Cast<object>().ToList(),
                        TabName = "Network Element - Level2"
                    };
                    ExportSheet levelOneSheet = new ExportSheet()
                    {
                        Data = levelOneData.Items.Cast<object>().ToList(),
                        TabName = "Network Element - Level1"
                    };



                    foreach (var nome in swSubnetworkData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");
                    }

                    foreach (var nome in hdSubnetworkData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");

                    }

                    foreach (var nome in hwConfigurationData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");

                    }

                    foreach (var nome in levelTwoData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");

                    }

                    foreach (var nome in levelOneData.Items)
                    {
                        nome.AssetClass = nome.AssetClass?.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass?.Replace("</b>", "");

                    }


                    foreach (var detail in hdSubnetworkData.GridRender.Render)
                    {
                        detail.Tab = "SubnetworkHardware";
                    }
                    foreach (var detail in swSubnetworkData.GridRender.Render)
                    {
                        detail.Tab = "SubnetworkSoftware";
                    }
                    foreach (var detail in hwConfigurationData.GridRender.Render)
                    {
                        detail.Tab = "Hardware";
                    }
                    foreach (var detail in levelTwoData.GridRender.Render)
                    {
                        detail.Tab = "Network Element - Level2";
                    }
                    foreach (var detail in levelOneData.GridRender.Render)
                    {
                        detail.Tab = "Network Element - Level1";
                    }

                    render.Render.AddRange(hdSubnetworkData.GridRender.Render);
                    render.Render.AddRange(swSubnetworkData.GridRender.Render);
                    render.Render.AddRange(hwConfigurationData.GridRender.Render);
                    render.Render.AddRange(levelTwoData.GridRender.Render);
                    render.Render.AddRange(levelOneData.GridRender.Render);

                    reportSheets.Add(hardwareSheet);
                    reportSheets.Add(softwareSheet);
                    reportSheets.Add(hwConfigurationSheet);
                    reportSheets.Add(levelTwoSheet);
                    reportSheets.Add(levelOneSheet);
                    

                var fileName = "ExportAllReport" + "_" + DateTime.Now.ToShortDateString() + ".xlsx";
                result = _exportService.GetExcelFrom(reportSheets, fileName, render);

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

        #region // Drop down

        [HttpPost("GetDesignComponentFamilyNames")]
        public ResultDto GetDesignComponentFamilyNames([FromBody] DesignComponentFamilyQueryDto buildFilterDto)
        {
            try
            {
                var designComponentFammiyNames = _dropdownDataServiceManager.GetAllDesignComponentFamilyName(buildFilterDto);

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = designComponentFammiyNames.Result.Data
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoNoFound
                };

            }
        }

        [HttpPost("GetSupportedServices")]
        public ResultDto GetSupportedServices()
        {
            try
            {
                var supportedServices = _dropdownDataServiceManager.GetAllSupportedServices();

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = supportedServices.Result.Data
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoNoFound,
                };

            }
        }
       
        #endregion

    }

}


