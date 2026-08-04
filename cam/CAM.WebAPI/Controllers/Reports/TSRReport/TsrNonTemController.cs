using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.TSR_Report;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.TsrPassThrough;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.Reports.TSRReport
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TsrNonTemController : CamControllerBase
    {
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<string> _opcoDescriptionList;
        private readonly List<int> _verticalList;
        private readonly bool _adminRoleCheck = false;
        private readonly IExportService _exportService;
        private readonly TsrNonTemManager _manager; 
        private CustomGridRender<ExportService> render;
        private ExportSheet temsSheet;
        private ExportSheet nonTemsSheet;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly List<string> _editColumnsLIst = new List<string>
                                                    {
                                                        {"SUPPORT_OWNER"  } ,
                                                        {"SUPPORT_TEAM"  } ,
                                                        {"SUPPORT_DOMAIN" } ,
                                                        {"RISK_ID" } ,
                                                        {"UPSTREAM_DEPENDENCIES"  } ,
                                                        {"CHANGE_DESCRIPTION" } ,
                                                        {"DOWNSTREAM_DEPENDENCIES" } ,
                                                        {"CHANGE_DESCRIPTION" } ,
                                                        {"VIRTUAL_PLATFORM_LOCATION" } ,
                                                        {"FIRMWARE_PATCH_LEVEL" } ,
                                                        {"MAINTENANCE_SUPPORT_SUPPLIER(HW)" } ,
                                                        {"HW_END_OF_LIFE" } ,
                                                        {"OS_PATCH_LEVEL" } ,
                                                        {"MAINTENANCE_SUPPORT_SUPPLIER(SW)" } ,
                                                        {"SW_EEOSL_CONTRACT_DATE" } ,
                                                        {"DEPENDANT_PRODUCT_NAME" } ,
                                                        {"CUSTOMER" } ,
                                                        {"ME_PRIVILEGED_ACCESS_LOGGING" } ,
                                                        {"HW_PART_NUMBER" } ,
                                                        {"LAST_UPGRADE_DATE"  } ,
                                                        {"SERVICE_LEVEL" } ,
                                                        {"NETWORK_OVERSIGHT"} ,
                                                        {"ME_SERIAL_NUMBER"} ,
                                                        {"DEPENDENT_HARDWARE" }
                                                    };
        public TsrNonTemController(ILoggerManager logger, IHttpContextAccessor contextAccessor,TsrNonTemManager manager, IExportService exportService, DropdownDataServiceManager dropdownDataServiceManager
            , ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _manager = manager;
            _exportService = exportService;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoDescriptionList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDescription != null && _roleOpcoList.OpcoDescription.Any() == true ?
                _roleOpcoList.OpcoDescription : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails : null;
        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<TsrPassThroughDtoGrid>> GetTsrNonTem([FromBody] TsrPassThroughQueryDto tsrPassThroughQueryDto)
        {
            try
            {
                if ((tsrPassThroughQueryDto.CountryWhereAssetIsLocated == null) || (tsrPassThroughQueryDto.CountryWhereAssetIsLocated != null && tsrPassThroughQueryDto.CountryWhereAssetIsLocated.Count == 0))
                    tsrPassThroughQueryDto.CountryWhereAssetIsLocated = _opcoDescriptionList;

                if ((tsrPassThroughQueryDto.VerticalName == null) || (tsrPassThroughQueryDto.VerticalName != null && tsrPassThroughQueryDto.VerticalName.Count == 0))
                    tsrPassThroughQueryDto.VerticalName = _verticalList;
                return await _manager.FindWithCondition(tsrPassThroughQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] TsrPassThroughQueryDto tsrPassThroughQueryDto)
        {
            try
            {
                if ((tsrPassThroughQueryDto.CountryWhereAssetIsLocated == null) || (tsrPassThroughQueryDto.CountryWhereAssetIsLocated != null && tsrPassThroughQueryDto.CountryWhereAssetIsLocated.Count == 0))
                    tsrPassThroughQueryDto.CountryWhereAssetIsLocated = _opcoDescriptionList;

                if ((tsrPassThroughQueryDto.VerticalName == null) || (tsrPassThroughQueryDto.VerticalName != null && tsrPassThroughQueryDto.VerticalName.Count == 0))
                    tsrPassThroughQueryDto.VerticalName = _verticalList;
                var data = await _manager.GetFilter(propertyName, propertyFilter, tsrPassThroughQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] TsrPassThroughQueryDto dto)
        {
            try
            {

                if ((dto.CountryWhereAssetIsLocated == null) || (dto.CountryWhereAssetIsLocated != null && dto.CountryWhereAssetIsLocated.Count == 0))
                    dto.CountryWhereAssetIsLocated = _opcoDescriptionList;

                if ((dto.VerticalName == null) || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                    dto.VerticalName = _verticalList;
                List<ExportSheet> reportSheets = new List<ExportSheet>();
                string IdColumn = "TSR Passthorugh Index";

                render = new CustomGridRender<ExportService>()
                {
                    Render = new List<RenderDetail>()
                };
                if (dto != null)
                {
                    dto.Page = 0;
                    dto.PageSize = 0;
                }

                var nonTemsdata = await _manager.FindWithCondition(dto);

                nonTemsSheet = new ExportSheet()
                {
                    Data = nonTemsdata.Items.Cast<object>().ToList(),
                    TabName = "NON-TEMS"
                };

                render.Render.AddRange(nonTemsdata.GridRender.Render);
                reportSheets.Add(nonTemsSheet);

                #region 1657 -TSR - Export the Glossary / Description on separate sheet //var tsrColumnDescription = await _manager.GetTsrColDescription();
                if (dto.IsGlossary)
                {
                    var temsGlossarydata = _manager.GetGlossyTableData().Result;
                    var temsGlossarySheet = new ExportSheet()
                    {
                        Data = temsGlossarydata.Items.Cast<object>().ToList(),
                        TabName = "Glossary"
                    };
                    render.Render.AddRange(temsGlossarydata.GridRender.Render);
                    reportSheets.Add(temsGlossarySheet);
                }
                #endregion


                var result = _exportService.GetExcelFrom(reportSheets,
                    "TSR Report_" + DateTime.Now.ToShortDateString() + ".xlsx", render, IdColumn, _editColumnsLIst, null);

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

        [HttpPost(template: "ExportDynamicReportCSV")]
        public FileContentResult ExportDynamicReportCSV([FromBody] TsrPassThroughQueryDto dto)
        {
            try
            {
                if ((dto.CountryWhereAssetIsLocated == null) || (dto.CountryWhereAssetIsLocated != null && dto.CountryWhereAssetIsLocated.Count > 0))
                    dto.CountryWhereAssetIsLocated = _opcoDescriptionList;
                if ((dto.VerticalName == null) || (dto.VerticalName != null && dto.VerticalName.Count > 0))
                    dto.VerticalName = _verticalList;
                List<ExportSheet> reportSheets = new List<ExportSheet>();
                render = new CustomGridRender<ExportService>()
                {
                    Render = new List<RenderDetail>()
                };
                if (dto != null)
                {
                    dto.Page = 0;
                    dto.PageSize = 0;
                }

                var nonTemsdata =
                            _manager.FindWithCondition(dto).Result;

                nonTemsSheet = new ExportSheet()
                {
                    Data = nonTemsdata.Items.Cast<object>().ToList(),
                    TabName = "NON-TEMS"
                };

                render.Render.AddRange(nonTemsdata.GridRender.Render);

                reportSheets.Add(nonTemsSheet);

                var fileName = "TSR Report_" + DateTime.Now.ToShortDateString() + ".csv";
                var result = _exportService.GetExcelFromCSV(reportSheets, fileName, render);

                var file = Encoding.ASCII.GetBytes(result.Stringbuilder.ToString());
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

        [HttpGet("GetDomainNames")]
        public async Task<List<KeyValuePairDto>> GetDomainNames()
        {
            try
            {
                return await _dropdownDataServiceManager.GetAllVerticalResponsible(_verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

    }
}
