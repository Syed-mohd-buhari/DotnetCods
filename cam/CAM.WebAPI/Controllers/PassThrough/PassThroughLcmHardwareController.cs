using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.PassThroughData;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AssetPassThrough;
using CAM.DataTransferObjects.Entita.BPT;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.BPT;
using CAM.Exports;
using CAM.Imports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.WebAPI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/PassThroughLcmHardware")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class PassThroughLcmHardwareController : CamControllerBase 
    {
        private readonly HwPassThroughLcmManager _manager;
        private readonly IExportService _exportService;
        private readonly CommonManager _commonManager;
        private readonly IWebHostEnvironment _env;
        private readonly IImportService _importService;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<string> _opcoDescriptionList;
        private readonly bool _adminRoleCheck = false;
        private readonly List<long> _verticalList;

        public PassThroughLcmHardwareController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, HwPassThroughLcmManager manager, CommonManager commonManager,
            IWebHostEnvironment env, IImportService importService, DropdownDataServiceManager dropdownDataServiceManager
            , AuthorizedRoleManager authorizedRoleManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
            _commonManager = commonManager;
            _env = env;
            _importService = importService;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoDescriptionList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDescription != null && _roleOpcoList.OpcoDescription.Any() == true ?
                _roleOpcoList.OpcoDescription : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToInt64(x)).Distinct().ToList() : null;

        }

        [HttpPost("GetPassThroughLcmHardwareReport")]
        public QueryResultDto<PassThroughLcmHardwareDtoGrid> GetPassThroughReport([FromBody] PassThroughLcmQueryDto passThroughLcmQueryDto)
        {
            try
            {
                if ((passThroughLcmQueryDto.LocalMarket == null) || (passThroughLcmQueryDto.LocalMarket != null && passThroughLcmQueryDto.LocalMarket.Count == 0))
                    passThroughLcmQueryDto.LocalMarket = _opcoDescriptionList;

                if ((passThroughLcmQueryDto.NonTemsVertical == null) || (passThroughLcmQueryDto.NonTemsVertical != null && passThroughLcmQueryDto.NonTemsVertical.Count == 0))
                    passThroughLcmQueryDto.NonTemsVertical = _verticalList;

                return _manager.FindWithCondition(passThroughLcmQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] PassThroughLcmQueryDto passThroughLcmQueryDto)
        {
            try
            {
                if ((passThroughLcmQueryDto.LocalMarket == null) || (passThroughLcmQueryDto.LocalMarket != null && passThroughLcmQueryDto.LocalMarket.Count == 0))
                    passThroughLcmQueryDto.LocalMarket = _opcoDescriptionList;

                if ((passThroughLcmQueryDto.NonTemsVertical == null) || (passThroughLcmQueryDto.NonTemsVertical != null && passThroughLcmQueryDto.NonTemsVertical.Count == 0))
                    passThroughLcmQueryDto.NonTemsVertical = _verticalList;
                var data = _manager.GetFilter(propertyName, propertyFilter, passThroughLcmQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] PassThroughLcmQueryDto dto)
        {
            try
            {

                if ((dto.LocalMarket == null) || (dto.LocalMarket != null && dto.LocalMarket.Count == 0))
                    dto.LocalMarket = _opcoDescriptionList;

                if ((dto.NonTemsVertical == null) || (dto.NonTemsVertical != null && dto.NonTemsVertical.Count == 0))
                    dto.NonTemsVertical = _verticalList;
                dto.Page = 0;
                dto.PageSize = 0;
                string domainName = string.Empty;
                if (dto.NonTemsVertical != null && dto.NonTemsVertical.Any())
                {
                    foreach (var item in dto.NonTemsVertical)
                        domainName = _dropdownDataServiceManager.GetVerticalResponseName(item).Result;
                }

                var data = _manager.FindWithCondition(dto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelForLcmPassthroughHardware(reportSheets, "PassThrough_LCM_Hardware_" + domainName + "_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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
        [HttpPost("import")]
        public async Task<ResultDto> ImportXls(IFormFile file, [FromQuery] long nonTemsVertical)
        {
            if (file == null || file.Length == 0 || !file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                return new ResultDto
                {
                    Info = "Please upload a valid xlsx file only.",
                    Warning = false
                };
            try
            {
                var excelConfigurationEntity = _commonManager.GetExcelConfiguration(ConstantValueFilter.passThroughLcmHardware).Result;
                using var stream = file.OpenReadStream();

                return await _importService.ImportDataBasedOnTemplateConfigurationForXL(file, ConstantValueFilter.passThroughLcmHardware, excelConfigurationEntity, nonTemsVertical);

            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    Info = "Error importing file.",
                    Warning = false,
                    Data = ex.Message
                };
                // return StatusCode(500, $"Error importing file: {ex.Message}");
            }
        }

    }
}
