using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.FNT_Report;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.FNT_Report;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.BPT;
using CAM.DataTransferObjects.QueryDto.FNT;
using CAM.Exports;
using CAM.Imports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.WebAPI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.Reports.FNTReport
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TemsFntReportController :  CamControllerBase
    {
        private readonly TemsFntReportManager _manager;
        private readonly IExportService _exportService;
        private readonly IImportService _importService;
        //private CustomGridRender<ExportService> render;
        private readonly CommonManager _commonManager;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<string> _opcoDescriptionList;
        private readonly List<string> _verticalList;
        private readonly bool _adminRoleCheck = false;

        public TemsFntReportController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, TemsFntReportManager manager, CommonManager commonManager,IImportService importService
            , ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor) 
        {
            _exportService = exportService;
            _importService = importService;
            _manager = manager;
            _commonManager = commonManager;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoDescriptionList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDescription != null && _roleOpcoList.OpcoDescription.Any() == true ?
                _roleOpcoList.OpcoDescription : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x=>Convert.ToString(x)).Distinct().ToList() : null;

        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<TemsFntReportDtoGrid>> GetTemsFntReport([FromBody] TemsFntReportQueryDto fntQueryDto)
        {
            try
            {
                if (((fntQueryDto.LocationOfHardwareAsset == null) || (fntQueryDto.LocationOfHardwareAsset != null && fntQueryDto.LocationOfHardwareAsset.Count == 0))
                   && ((fntQueryDto.LocalMarket == null) || (fntQueryDto.LocalMarket != null && fntQueryDto.LocalMarket.Count == 0))
                   && ((fntQueryDto.Market == null) || (fntQueryDto.Market != null && fntQueryDto.Market.Count == 0)))
                {
                    fntQueryDto.LocationOfHardwareAsset = _opcoDescriptionList;
                    fntQueryDto.LocalMarket = _opcoDescriptionList;
                    fntQueryDto.Market = _opcoDescriptionList;
                }
                if (((fntQueryDto.MeverticalResposible == null) || (fntQueryDto.MeverticalResposible != null && fntQueryDto.MeverticalResposible.Count == 0))
                    && ((fntQueryDto.VerticalEngineeringTeam == null) || (fntQueryDto.VerticalEngineeringTeam != null && fntQueryDto.VerticalEngineeringTeam.Count == 0)))
                {
                    fntQueryDto.MeverticalResposible = _verticalList;
                    fntQueryDto.VerticalEngineeringTeam = _verticalList;
                }
                    
                return await _manager.FindWithCondition(fntQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] TemsFntReportQueryDto fntQueryDto)
        {
            try
            {
                if (((fntQueryDto.LocationOfHardwareAsset == null) || (fntQueryDto.LocationOfHardwareAsset != null && fntQueryDto.LocationOfHardwareAsset.Count == 0))
                    && ((fntQueryDto.LocalMarket == null) || (fntQueryDto.LocalMarket != null && fntQueryDto.LocalMarket.Count == 0))
                    && ((fntQueryDto.Market == null) || (fntQueryDto.Market != null && fntQueryDto.Market.Count == 0)))
                {
                    fntQueryDto.LocationOfHardwareAsset = _opcoDescriptionList;
                    fntQueryDto.LocalMarket = _opcoDescriptionList;
                    fntQueryDto.Market = _opcoDescriptionList;
                }
                if (((fntQueryDto.MeverticalResposible == null) || (fntQueryDto.MeverticalResposible != null && fntQueryDto.MeverticalResposible.Count == 0))
                    && ((fntQueryDto.VerticalEngineeringTeam == null) || (fntQueryDto.VerticalEngineeringTeam != null && fntQueryDto.VerticalEngineeringTeam.Count == 0)))
                {
                    fntQueryDto.MeverticalResposible = _verticalList;
                    fntQueryDto.VerticalEngineeringTeam = _verticalList;
                }
                var data = await _manager.GetFilteredValues(propertyName, propertyFilter, fntQueryDto ,_adminRoleCheck,_verticalList);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] TemsFntReportQueryDto dto)
        {
            try
            {
                if (((dto.LocationOfHardwareAsset == null) || (dto.LocationOfHardwareAsset != null && dto.LocationOfHardwareAsset.Count == 0))
                    && ((dto.LocalMarket == null) || (dto.LocalMarket != null && dto.LocalMarket.Count == 0))
                    && ((dto.Market == null) || (dto.Market != null && dto.Market.Count == 0)))
                {
                    dto.LocationOfHardwareAsset = _opcoDescriptionList;
                    dto.LocalMarket = _opcoDescriptionList;
                    dto.Market = _opcoDescriptionList;
                }
                if (((dto.MeverticalResposible == null) || (dto.MeverticalResposible != null && dto.MeverticalResposible.Count == 0))
                    && ((dto.VerticalEngineeringTeam == null) || (dto.VerticalEngineeringTeam != null && dto.VerticalEngineeringTeam.Count == 0)))
                {
                    dto.MeverticalResposible = _verticalList;
                    dto.VerticalEngineeringTeam = _verticalList;
                }
                dto.Page = 0;
                dto.PageSize = 0;
                var data = await _manager.FindWithCondition(dto, true);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var idColumns = "TEMS_FNT_REPORT_ID";

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "TEMS_FNT_Report_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender, idColumns, ConstantValueFilter.editTemsFntReport);
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

        [HttpPost("TemsFNTDataLoads")]
        public async Task<ResultDto> TemsFNTDataLoads([FromBody] TemsFntReportQueryDto dto)
        {
            try
            {
                return await _manager.TemsFNTDataRefresh(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("Import")]
        public async Task<ResultDto> ImportXls(IFormFile file)
        {
            if (file == null || file.Length == 0 || !file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                return new ResultDto
                {
                    Info = "Please upload a valid xlsx file only.",
                    Warning = false
                };
            try
            {
                var excelConfigurationEntity = _commonManager.GetExcelConfiguration("TEMSFNT").Result;
                using var stream = file.OpenReadStream();

                return await _importService.ImportDataBasedOnTemplateConfigurationForXL(file, ConstantValueFilter.TemsFntReport, excelConfigurationEntity);

            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = "Error importing file.",
                    Warning = false,
                    Data = ex.Message
                };
            }
        }
       

    }
}
