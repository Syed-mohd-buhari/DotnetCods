using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.PassThroughData;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AssetPassThrough;
using CAM.DataTransferObjects.Entita.BPT;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Imports;
using CAM.Infrastucture;
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

namespace CAM.WebAPI.Controllers.AssetPassThrough
{
    [Route("api/PassThrough")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class PassThroughController : CamControllerBase 
    {
        private readonly AssetPassThroughManager _manager;
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
        public PassThroughController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, AssetPassThroughManager manager, CommonManager commonManager,
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

        [HttpPost("GetPassThroughReport")]
        public QueryResultDto<PassThroughDtoGrid> GetPassThroughReport([FromBody] PassThroughQueryDto passThroughQueryDto)
        {
            try
            {
                if ((passThroughQueryDto.CountryWhereAssetIsLocated == null) || (passThroughQueryDto.CountryWhereAssetIsLocated != null && passThroughQueryDto.CountryWhereAssetIsLocated.Count == 0))
                    passThroughQueryDto.CountryWhereAssetIsLocated = _opcoDescriptionList;

                if ((passThroughQueryDto.NonTemsVertical == null) || (passThroughQueryDto.NonTemsVertical != null && passThroughQueryDto.NonTemsVertical.Count == 0))
                    passThroughQueryDto.NonTemsVertical = _verticalList;
                return _manager.FindWithCondition(passThroughQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] PassThroughQueryDto passThroughQueryDto)
        {
            try
            {
                if ((passThroughQueryDto.CountryWhereAssetIsLocated == null) || (passThroughQueryDto.CountryWhereAssetIsLocated != null && passThroughQueryDto.CountryWhereAssetIsLocated.Count == 0))
                    passThroughQueryDto.CountryWhereAssetIsLocated = _opcoDescriptionList;

                if ((passThroughQueryDto.NonTemsVertical == null) || (passThroughQueryDto.NonTemsVertical != null && passThroughQueryDto.NonTemsVertical.Count == 0))
                    passThroughQueryDto.NonTemsVertical = _verticalList;
                var data = _manager.GetFilter(propertyName, propertyFilter, passThroughQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] PassThroughQueryDto dto)
        {
            try
            {
                if ((dto.CountryWhereAssetIsLocated == null) || (dto.CountryWhereAssetIsLocated != null && dto.CountryWhereAssetIsLocated.Count == 0))
                    dto.CountryWhereAssetIsLocated = _opcoDescriptionList;

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
                var excelConfiguration = _commonManager.GetExcelConfiguration(ConstantValueFilter.passThroughExcelProcessName).Result;

                excelConfiguration = excelConfiguration.Where(x => x.Isexportfield == true && x.Processname == ConstantValueFilter.passThroughExcelProcessName).ToList();
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                var templateFileFolder = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                _logger.LogError(templateFileFolder);
                string templateFileName = excelConfiguration.FirstOrDefault(x => x.Templatefilename != null)?.Templatefilename?.Trim();
                _logger.LogError(templateFileName);

                var filePath = Path.Combine(templateFileFolder, ConstantValueFilter.passThroughExcelProcessName, templateFileName);
                _logger.LogError(filePath);


                if (templateFileName == null)
                {
                    throw new Exception("AssetPassThrough details is missing in Excel template configuration");
                }

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);


                var result = _exportService.ExportExcelBasedOnConfiguration(reportSheets,
                                 "PassThrough_" + domainName + "_" + DateTime.Now.ToShortDateString() + ".xlsx", filePath, data.Items.ToList(),
                                 excelConfiguration.ToList());

                HttpContext.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                var fileContentResult = new FileContentResult(result.FileInByteArray, "application/vnd.ms-excel")
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
                var excelConfigurationEntity = _commonManager.GetExcelConfiguration(ConstantValueFilter.passThroughExcelProcessName).Result;
                using var stream = file.OpenReadStream();

                return await _importService.ImportDataBasedOnTemplateConfigurationForXL(file, ConstantValueFilter.passThroughExcelProcessName, excelConfigurationEntity, nonTemsVertical);

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

        [HttpGet("GetOtherDomainNames")]
        public async Task<Dictionary<long, string>> GetOtherDomainNames()
        {
            try
            {
                return await _manager.GetOtherDomainNames();
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
                var verticalList= _verticalList!=null && _verticalList.Count> 0 ? _verticalList.Select(x=>Convert.ToInt32(x)).ToList():null;
                return await _dropdownDataServiceManager.GetAllVerticalResponsible(verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
    }
}
