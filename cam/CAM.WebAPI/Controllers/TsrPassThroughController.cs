using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.GenericReports;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.TsrPassThrough;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.GenericReportDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Imports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Presentation;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/TsrPassThrough")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class TsrPassThroughController : CamControllerBase
    {
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<string> _opcoDescriptionList;
        private readonly bool _adminRoleCheck = false;
        private readonly CommonManager _commonManager;
        private readonly TsrPassThroughManager _manager;
        private readonly IExportService _exportService;
        private readonly IImportService _importService;
        private CustomGridRender<ExportService> render;
        private ExportSheet temsSheet;
        private ExportSheet nonTemsSheet;
        private List<ExportSheet> reportSheets;
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
        public TsrPassThroughController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, TsrPassThroughManager manager, IImportService importService, CommonManager commonManager
            , ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
            _importService = importService;
            _commonManager = commonManager;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoDescriptionList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDescription != null && _roleOpcoList.OpcoDescription.Any() == true ?
                _roleOpcoList.OpcoDescription : null;
        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<TsrPassThroughDtoGrid>> GetTSRPassThroughReport([FromBody] TsrPassThroughQueryDto tsrPassThroughQueryDto)
        {
            try
            {
                if ((tsrPassThroughQueryDto.CountryWhereAssetIsLocated == null) || (tsrPassThroughQueryDto.CountryWhereAssetIsLocated != null && tsrPassThroughQueryDto.CountryWhereAssetIsLocated.Count == 0))
                    tsrPassThroughQueryDto.CountryWhereAssetIsLocated = _opcoDescriptionList; 
                return await _manager.FindWithCondition(tsrPassThroughQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] TsrPassThroughQueryDto tsrPassThroughQueryDto)
        {
            try
            {
                if ((tsrPassThroughQueryDto.CountryWhereAssetIsLocated == null) || (tsrPassThroughQueryDto.CountryWhereAssetIsLocated != null && tsrPassThroughQueryDto.CountryWhereAssetIsLocated.Count == 0))
                    tsrPassThroughQueryDto.CountryWhereAssetIsLocated = _opcoDescriptionList; 
                var data = await _manager.GetFilter(propertyName, propertyFilter, tsrPassThroughQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] TsrPassThroughQueryDto dto)
        {
            try
            {
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

                if ((dto.CountryWhereAssetIsLocated == null) || (dto.CountryWhereAssetIsLocated != null && dto.CountryWhereAssetIsLocated.Count ==0))
                    dto.CountryWhereAssetIsLocated = _opcoDescriptionList; 
                if (dto != null && dto.RecordClassifier.Contains(1))
                {
                    var temsdata = _manager.FindWithCondition(dto).Result;
                    temsSheet = new ExportSheet()
                    {
                        Data = temsdata.Items.Cast<object>().ToList(),
                        TabName = "TEMS"
                    };
                    render.Render.AddRange(temsdata.GridRender.Render);
                    reportSheets = new List<ExportSheet>();
                    reportSheets.Add(temsSheet);

                }
                if (dto != null && dto.RecordClassifier.Contains(2))
                {
                    var nonTemsdata =
                            _manager.FindWithCondition(dto).Result;

                    nonTemsSheet = new ExportSheet()
                    {
                        Data = nonTemsdata.Items.Cast<object>().ToList(),
                        TabName = "NON-TEMS"
                    };


                    render.Render.AddRange(nonTemsdata.GridRender.Render);
                    reportSheets = new List<ExportSheet>();
                    reportSheets.Add(nonTemsSheet);

                }

                if (dto != null && dto.ActiveTab.ToLower() == "both")
                {
                    var data = _manager.FindWithCondition(dto).Result;
                    dto.RecordClassifier.Add(1);

                    var temsData = data.Items.ToList().Where(x => x.RecordClassifier == 1);

                    dto.RecordClassifier = new List<int>
                {
                    2
                };
                    var nonTemsData = data.Items.ToList().Where(x => x.RecordClassifier == 2);

                    ExportSheet temsDataSheet = new ExportSheet()
                    {
                        Data = temsData.Cast<object>().ToList(),
                        TabName = "TEMS"
                    };

                    ExportSheet nonTemsDataSheet = new ExportSheet()
                    {
                        Data = nonTemsData.Cast<object>().ToList(),
                        TabName = "NON-TEMS"
                    };
                    render.Render.AddRange(data.GridRender.Render);

                    reportSheets.Add(temsDataSheet);
                    reportSheets.Add(nonTemsDataSheet);

                }

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
                return null;
            }
        }

        [HttpPost("ImportExcel")]
        public async Task<ResultDto> Import(IFormFile file, [FromQuery] long recordClassifier, [FromQuery] long nonTemsVertical)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.NoFile
                    };
                }
                else
                {

                    return await _importService.BulkImport(file, recordClassifier, nonTemsVertical);
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost("TSRDataLoads")]
        public async Task<ResultDto> TSRDataLoads([FromBody] TsrPassThroughQueryDto dto)
        {
            try
            {
                return await _manager.TSRDataRefresh(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportDynamicReportCSV")]
        public FileContentResult ExportDynamicReportCSV([FromBody] TsrPassThroughQueryDto dto)
        {
            try
            {
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

                if ((dto.CountryWhereAssetIsLocated == null) || (dto.CountryWhereAssetIsLocated != null && dto.CountryWhereAssetIsLocated.Count > 0))
                    dto.CountryWhereAssetIsLocated = _opcoDescriptionList;
                if (dto != null && dto.RecordClassifier.Contains(1))
                {
                    var temsdata = _manager.FindWithCondition(dto).Result;
                    temsSheet = new ExportSheet()
                    {
                        Data = temsdata.Items.Cast<object>().ToList(),
                        TabName = "TEMS"
                    };
                    render.Render.AddRange(temsdata.GridRender.Render);
                    reportSheets.Add(temsSheet);
                }
                if (dto != null && dto.RecordClassifier.Contains(2))
                {
                    var nonTemsdata =
                            _manager.FindWithCondition(dto).Result;

                    nonTemsSheet = new ExportSheet()
                    {
                        Data = nonTemsdata.Items.Cast<object>().ToList(),
                        TabName = "NON-TEMS"
                    };
                    
                    render.Render.AddRange(nonTemsdata.GridRender.Render);
                   
                    reportSheets.Add(nonTemsSheet);
                }

                if (dto != null && dto.ActiveTab.ToLower() == "both")
                {
                    var data = _manager.FindWithCondition(dto).Result;

                    var tsrData = data.Items.ToList();

                    ExportSheet tsrDataSheet = new ExportSheet()
                    {
                        Data = tsrData.Cast<object>().ToList(),
                        TabName = "TEMS"
                    };

                    render.Render.AddRange(data.GridRender.Render);

                    reportSheets.Add(tsrDataSheet);
                }

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

        #region GET SCHEDULER CONFIG DETAILS
        [HttpGet("GetReportSchedulerDetails")]
        public async Task<IActionResult> GetReportSchedulerDetails([FromQuery]string reportName)
        {
            try
            {
                var result = await _commonManager.GetReportSchedulerDetails(reportName);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
          
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        #endregion

    }
}
