using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.BPT;
using CAM.BusinessManager.Entity.ComponentSoftware;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.BPT;
using CAM.DataTransferObjects.QueryDto.ComponentSoftware;
using CAM.Exports;
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
    public class BuildBagController : CamControllerBase
    {
        private readonly BuildBagManager _buildBagManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly BPTManager _bPTManager;
        private readonly CommonManager _commonManager;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly ICurrentUserService _currentUserService;

        private readonly long sessionUserId;
        private readonly List<short> _opcoList;
        private readonly bool _adminRoleCheck = false;

        public BuildBagController(BuildBagManager buildBagManager, IMapper mapper, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, IExportService exportService, IWebHostEnvironment env, BPTManager bPTManager
            , CommonManager commonManager, AuthorizedRoleManager authorizedRoleManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _buildBagManager = buildBagManager;
            _exportService = exportService;
            _mapper = mapper;   
            _env = env;
            _bPTManager= bPTManager;
            _commonManager = commonManager;

            _authorizedRoleManager = authorizedRoleManager;
            _currentUserService = currentUserService;
            sessionUserId = _currentUserService.UserId;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;

        }

        [HttpPost("GetBuildBag")]
        public async Task<QueryResultDto<BuildBagDtoGrid>> GetBuildBagAsync([FromBody] BuildBagQueryDto filterDto)
        {
            try
            {
                if (filterDto.Opco==null || filterDto.Opco != null && filterDto.Opco.Count == 0)
                {
                    filterDto.Opco =  _opcoList;
                }
                return await _buildBagManager.FindWithCondition(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "CreateBuildBagResource")]
        public async Task<BuildBagCreatePageDto> GetCreateBuildBagResource()
        {
            try
            {
                return await _buildBagManager.GetCreateBuildBagPageDetailsAsync(_opcoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "EditBuildBagResource{id}")]
        public async Task<BuildBagEditPageDto> GetEditBuildBag(long id)
        {
            try
            {
                return await _buildBagManager.GetEditedBuildBagPageDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "ViewBagAndComponentDetailsAsync{id}")]
        public async Task<ViewBagandComponenetDto> ViewBagAndComponentDetailsAsync(long id)
        {
            try
            {
                return await _buildBagManager.ViewBagAndComponentDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "CreateNewBuildBag")]
        public async Task<ResultDto> Create([FromBody] BuildBagCreatePageDto dto, [FromQuery] bool? forced)
        {
            try
            { 

                var result = await _buildBagManager.AddBuildBagAsync(dto, forced);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut(template: "UpdateEditedBuildBag")]
        public async Task<ResultDto> UpdateEditedBuildBag(BuildBagUpdateDto dto, [FromQuery] bool? forced)
        {
            try
            {               
                var result = await _buildBagManager.UpdateBuildBagAsync(dto, forced);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "UpgradeBuildComponentSwResource{bagId}")]
        public async Task<BuildBagEditPageDto> UpgradePageDetials(long bagId)
        {
            try
            {
                return await _buildBagManager.GetUpgradeResourceAsync(bagId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }
        [HttpPost(template: "UpgradeBagAndComponent")]
        public async Task<ResultDto> UpgradeComponentSwBuildBag([FromBody] BuildBagCreatePageDto dto)
        {
            try
            {

                var result = await _buildBagManager.UpgradeBagAndComponenetSoftwareBuildBagAsync(dto);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter, 
            [FromBody] BuildBagQueryDto  buildBagFilterDto)
        {
            try
            {
                if (buildBagFilterDto.Opco == null ||buildBagFilterDto.Opco != null && buildBagFilterDto.Opco.Count == 0)
                {
                    buildBagFilterDto.Opco = _opcoList;
                }
                var data = await _buildBagManager.GetFilteredValuesAsync(propertyName, propertyFilter, buildBagFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetReferencedDetailForDelete{id}")]
        public async Task<ResultDto> GetReferencedDetailForDelete(long id)
        {
            try
            {
                return await _buildBagManager.GetReferenceRecordAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "DeleteBuildBag")]
        public async Task<ResultDto> DeleteDeep(long id)
        {
            try
            {
                var result = await _buildBagManager.DeleteComponenetBuildBag(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] BuildBagQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if (dto.Opco==null || dto.Opco != null && dto.Opco.Count == 0)
                {
                    dto.Opco = _opcoList;
                }
                var data = await _buildBagManager.FindWithCondition(dto);
               
                ExportSheet dataSheet = new ExportSheet()
                {
                  Data =   data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Build Bag" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        [HttpPost(template: "GetOpcoDcfBasedBags")]
        public async Task<List<ViewBagandComponenetDto>> GetOpcoDcfBasedBags(long dcId,short opCoId, long lcmBagId)
        {
            try
            {
                var result = await _buildBagManager.GetOpcoDcfBasedBag(dcId, opCoId, lcmBagId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        #region  BPT

        [HttpPost(template: "ExportReportLegacyFile")]
        public async Task<IActionResult> ExportReportLegacyFile([FromBody] BPTQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;


                var data = await _bPTManager.FindWithCondition(dto);

                var excelConfiguration = _commonManager.GetExcelConfiguration("BPT").Result;


                excelConfiguration = excelConfiguration.Where(x => x.Isexportfield == true).ToList();

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                var templateFileFolder = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");

                string templateFileName = excelConfiguration.FirstOrDefault(x => x.Templatefilename != null)?.Templatefilename?.Trim();

                if (templateFileFolder == null)
                {
                    return BadRequest("Export failed: file path not found.");
                }

                var filePath = Path.Combine(templateFileFolder, "TemplateFiles", templateFileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return BadRequest("Export failed: file path not found.");
                }

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.ExportLegacyExcelBasedOnConfiguration(reportSheets,
                                 templateFileName + DateTime.Now.ToShortDateString() + ".xls", filePath, data.Items.Take(10).ToList(),
                                 excelConfiguration.ToList());

                if (result.FileInByteArray == null || result.FileInByteArray.Length == 0)
                {
                    return BadRequest("Export failed: No data found.");
                }

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


        [HttpPost(template: "ExportReportNonLegacyFile")]
        public async Task<FileContentResult> ExportReportNonLegacyFile([FromBody] BuildBagQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                var data = await _buildBagManager.FindWithCondition(dto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                var binFolder = _env.WebRootPath; // e.g., project root
                string templateFileName = ConstantValueFilter.bptTemplateFileName;

                var filePath = Path.Combine(binFolder, "TemplateFiles", templateFileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return null;
                }

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelBasedOnConfiguration(reportSheets,
                    "BPT_NonLegacyFile_" + DateTime.Now.ToShortDateString() + ".xlsx", filePath, data.Items.Take(10).ToList());

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
  