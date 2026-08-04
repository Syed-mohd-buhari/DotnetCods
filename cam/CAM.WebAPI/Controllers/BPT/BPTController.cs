using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.BPT;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.BPT;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.BPT;
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

namespace CAM.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BPTController : CamControllerBase
    {
        private readonly BPTManager _bptManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly CommonManager _commonManager;
        private readonly IWebHostEnvironment _env;
        private readonly IImportService _importService;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly ICurrentUserService _currentUserService;

        private readonly long sessionUserId;
        private readonly List<short> _opcoList;
        private readonly bool _adminRoleCheck = false;
        private readonly List<int> _verticalList;
        public BPTController(AuthorizedRoleManager authorizedRoleManager,ICurrentUserService currentUserService,BPTManager bptManager, IMapper mapper, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            IExportService exportService, CommonManager commonManager, IWebHostEnvironment env, IImportService importService) : base(logger, contextAccessor)
        {
            _bptManager = bptManager;
            _exportService = exportService;
            _mapper = mapper;
            _commonManager = commonManager;
             _env = env;
            _importService = importService;

            _authorizedRoleManager = authorizedRoleManager;
            _currentUserService = currentUserService;
            sessionUserId = _currentUserService.UserId;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails : null;
        }

        [HttpPost("GetBptRecords")]
        public async Task<QueryResultDto<BPTGridDto>> GetBuildBagAsync([FromBody] BPTQueryDto filterDto)
        {
            try
            {
                if (filterDto.Opco == null || filterDto.Opco != null && filterDto.Opco.Count == 0)
                {
                    filterDto.Opco = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
                }
                if ((filterDto.VerticalName == null) || filterDto.VerticalName != null && filterDto.VerticalName.Count == 0)
                {
                    filterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                return await _bptManager.FindWithCondition(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        
        [HttpGet(template: "EditBPTResource{id}")]
        public async Task<BuildBagEditPageDto> GetEditBudgetProjectTracker(long id)
        {
            try
            {
                return await _bptManager.GetEditedBPTPageDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
 
        [HttpPut(template: "UpdateBPTResource")]
        public async Task<ResultDto> UpdateBudgetProjectTrackerResource(BuildBagUpdateDto dto, [FromQuery] bool? forced)
        {
            try
            {
                var result = await _bptManager.UpdateProjectTrackerAsync(dto, forced);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpPost(template: "Filter")]
        public async  Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] BPTQueryDto filterDto)
        {
            try
            {
                if (filterDto.Opco == null || filterDto.Opco != null && filterDto.Opco.Count == 0)
                {
                    filterDto.Opco = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
                }
                if ((filterDto.VerticalName == null) || filterDto.VerticalName != null && filterDto.VerticalName.Count == 0)
                {
                    filterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                var data = _bptManager.GetFilter(propertyName, propertyFilter, filterDto,_adminRoleCheck).Result;
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        [HttpPost(template: "ExportReport")]
        public async Task<IActionResult> ExportReportLegacyFile([FromBody] BPTQueryDto dto)
        {
            try
            {
                if (dto.Opco == null || dto.Opco != null && dto.Opco.Count == 0)
                {
                    dto.Opco = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
                }
                if ((dto.VerticalName == null) || dto.VerticalName != null && dto.VerticalName.Count == 0)
                {
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                dto.Page = 0;
                dto.PageSize = 0;
                var data = await _bptManager.FindWithCondition(dto);

                var excelConfiguration = _commonManager.GetExcelConfiguration("BPT").Result;

                excelConfiguration = excelConfiguration.Where(x => x.Isexportfield == true && x.Processname == "BPT").ToList();
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                var templateFileFolder = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                _logger.LogError(templateFileFolder);
                string templateFileName = excelConfiguration.FirstOrDefault(x => x.Templatefilename != null)?.Templatefilename?.Trim();
                _logger.LogError(templateFileName);

                if (templateFileName == null)
                {
                    throw new Exception("BPT details is missing in Excel template configuration");
                }

                var filePath = Path.Combine(templateFileFolder, "TemplateFiles", templateFileName);
                _logger.LogError(filePath);
                

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);


                foreach (BPTGridDto item in data.Items)
                {
                    item.Activity = item.Activity.Replace("<b class=\"text-lowercase\">", "");
                    item.Activity = item.Activity.Replace("<b class=\"text-lowercase\" >", "");
                    item.Activity = item.Activity.Replace("</b>", "");


                }

                var result = _exportService.ExportLegacyExcelBasedOnConfiguration(reportSheets,
                                 "BPT_" + DateTime.Now.ToShortDateString() + ".xls", filePath, data.Items.ToList(),
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

        [HttpPost("BulkInsertForBpt")]
        public async Task<ResultDto> BulkInsertForBpt()
        {
            try
            {
                return await _bptManager.BPTDataCreation();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpPost("import")]
        public async Task<ResultDto> ImportXls(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0 || !file.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                    return new ResultDto
                    {
                        Info = "Please upload a valid xls file only.",
                        Warning = false
                    };
                try
                {
                    var excelConfigurationEntity = _commonManager.GetExcelConfiguration("BPT").Result;
                    using var stream = file.OpenReadStream();

                    return await _importService.ImportDataBasedOnTemplateConfiguration(file, ConstantValueFilter.bptExcelProcessName, excelConfigurationEntity);

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
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpGet("exporttest")]
        public IActionResult ExportTest()
        {
            try
            {
                var path = Path.Combine(_env.WebRootPath, "TemplateFiles", "test.txt");
                System.IO.File.WriteAllText(path, "This is a test write from the app.");
                return Ok(" File written successfully: " + path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return BadRequest(" Write failed: " + ex.Message);
            }
        }



        [HttpGet("exporttestnew")]
        public IActionResult ExportTestNew()
        {
            try
            {
                var path = Path.Combine(_env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot"), "TemplateFiles", "test.txt");
                System.IO.File.WriteAllText(path, "This is a test write from the app.");
                return Ok(" File written successfully: " + path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return BadRequest(" Write failed: " + ex.Message);
            }
        }
    }
}
