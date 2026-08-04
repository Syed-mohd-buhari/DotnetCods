using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.XBom.VBom;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
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
using static CAM.DataTransferObjects.QueryDto.XBom.VBom.VnfVmCapacityQueryDto;
namespace CAM.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VBOMController : CamControllerBase
    {
        private readonly VBomManager _vBomManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly IImportService _importService;
        private readonly IWebHostEnvironment _env;
        private CustomGridRender<ExportService> render;
        private readonly CommonManager _commonManager;
        ILoggerManager _loggerManager;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly ICurrentUserService _currentUserService;

        private readonly long sessionUserId;
        private readonly List<short> _opcoList;
        private readonly bool _adminRoleCheck = false;
        public VBOMController(AuthorizedRoleManager authorizedRoleManager, ICurrentUserService currentUserService, VBomManager vBomManager, IMapper mapper, ILoggerManager logger, CommonManager commonManager, IWebHostEnvironment env,
        IHttpContextAccessor contextAccessor, IExportService exportService, IImportService importService) : base(logger, contextAccessor)
        {
            _vBomManager = vBomManager;
            _exportService = exportService;
            _mapper = mapper;
            _importService = importService;
            _commonManager = commonManager;
            _env = env;
            _loggerManager = logger;

            _authorizedRoleManager= authorizedRoleManager;
            _currentUserService= currentUserService;
            sessionUserId = _currentUserService.UserId;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
        }
        // #region
        [HttpPost("GetVnfClusterinfoDetails")]
        public async Task<QueryResultDto<VnfClusterInfoDtoGrid>> VnfClusterInfoAsync([FromBody] VnfClusterInfoQueryDto filterDto)

        {
            try
            {
                if (filterDto.OpcoDescritpion == null || filterDto.OpcoDescritpion != null && filterDto.OpcoDescritpion.Count == 0)
                {
                    filterDto.OpcoDescritpion = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
                }
                return await _vBomManager.VnfClusterBasedEntities(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost("GetVnfInfoAndCapacityDetails")]
        public async Task<QueryResultDto<VnfVbomInfoDtoGrid>> VnfVnfInfoCapacitAsync([FromBody] VnfClusterInfoQueryDto filterDto)

        {
            try
            {
                return await _vBomManager.GetVnfInfoAndCapacityEntities(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter,
         [FromBody] VnfClusterInfoQueryDto filterDto, bool isInstance = false)
        {
            filterDto.Page = 0;
            filterDto.PageSize = 0;
            var data = new List<FilterValueDto>();
            if (filterDto.OpcoDescritpion == null || filterDto.OpcoDescritpion != null && filterDto.OpcoDescritpion.Count == 0)
            {
                filterDto.OpcoDescritpion = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
            }
            try
            {
                if (isInstance)
                    data = await _vBomManager.GetInstanceCapacityFilteredValues(propertyName, propertyFilter, filterDto);

                else
                    data = await _vBomManager.GetFilteredValues(propertyName, propertyFilter, filterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        #region CREATE,EDIT ,DELETE ,UPDATE
        [HttpGet(template: "CreateVnfVBomPageResource")]
        public async Task<VBomCreatePageEntityDto> GetVnfVBomPageResource()
        {
            return await _vBomManager.GetCreateVnfVBomPageResourceAsync(_opcoList);
        }

        [HttpPost(template: "AddNewVBomResource")]
        public async Task<ResultDto> InsertNewVBomResource([FromBody] VBomInsertUpdateDto dto)
        {
            try
            {

                var result = await _vBomManager.AddVBomDetailAsync(dto, false);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "EditVnfVBomPageResource{id}")]
        public async Task<ResultDto> GetEditVnfVBomPage(long id)
        {
            return await _vBomManager.GetEditVnfVBomPageResourceAsync(id, _opcoList);
        }

        [HttpPost(template: "UpdateExistingVBomResource")]
        public async Task<ResultDto> UpdateVBomResource([FromBody] VBomInsertUpdateDto dto)
        {
            try
            {

                var result = await _vBomManager.AddVBomDetailAsync(dto, true);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "DeleteVbomEntityBasedOnCapacity")]
        public async Task<ResultDto> DeleteDeep(long id)
        {
            try
            {
                var result = await _vBomManager.DeleteVnfCapacityBasedAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "DeleteVbomEntity")]
        public async Task<ResultDto> DeleteDeep(long infoId = 0, long instanceId = 0, long capacityId = 0)
        {
            try
            {
                var result = await _vBomManager.DeleteVbomAsync(infoId, instanceId, capacityId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpGet(template: "GetLinkedInfoForCapacity")]
        public async Task<ResultDto> GetLinkedInfoBasedOnCapacity(long id)
        {
            try
            {
                var result = await _vBomManager.GetLinkedInfoBasedOnCapacity(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        #endregion


        [HttpPost("GetAllVbomEntities")]
        public async Task<QueryResultDto<VbomExportGridDto>> GetAllVbomEntities([FromBody] VnfClusterInfoQueryDto filterDto)

        {
            try
            {
                return await _vBomManager.FindWithConditionAsync(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ExportReport")]
        public async Task<IActionResult> ExportReportLegacyFile([FromBody] VnfClusterInfoQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if (dto.OpcoDescritpion == null || dto.OpcoDescritpion != null && dto.OpcoDescritpion.Count == 0)
                {
                    dto.OpcoDescritpion = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
                }
                var data = await _vBomManager.FindWithConditionAsync(dto);
               
                render = new CustomGridRender<ExportService>()
                {
                    Render = new List<RenderDetail>()
                };

                if (data.Items.Count == 0)
                    return Content("Failed To Export Data");

                var opCo = data.Items.FirstOrDefault()?.OpCoDescritpion;
                var hardwareType = data.Items.FirstOrDefault()?.HardwareType;

                var revision =Convert.ToString( data.Items.OrderByDescending(x => x.Revision).FirstOrDefault()?.Revision);
                    


                var clusterLocationBasedData = data.Items.ToList().OrderByDescending(x=>x.SiteLocationid).GroupBy(x => new { x.ClusterNameId, x.SiteLocationid });


                var excelConfiguration = _commonManager.GetExcelConfiguration("VBOM").Result;

                excelConfiguration = excelConfiguration.Where(x => x.Isexportfield == true && x.Processname.ToLower().Trim() == "vbom").ToList();
                List<ExportSheet> reportSheets = new List<ExportSheet>();

                foreach (var groupedData in clusterLocationBasedData)
                {

                    ExportSheet dataSheet = new ExportSheet()
                    {
                        Data = groupedData.Cast<object>().ToList(),
                        TabName = groupedData.FirstOrDefault().ClusterName.ToString() + "_" + groupedData.FirstOrDefault().SiteLocation.ToString()
                    };

                    render.Render.AddRange(data.GridRender.Render);

                    reportSheets.Add(dataSheet);

                }

                var templateFileFolder = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                _logger.LogError(templateFileFolder);
                string templateFileName = excelConfiguration.FirstOrDefault(x => x.Templatefilename != null)?.Templatefilename?.Trim();
                _logger.LogError(templateFileName);

                if (templateFileName == null)
                {
                    throw new Exception("VBOM details is missing in Excel template configuration");
                }

                var filePath = Path.Combine(templateFileFolder, "VBOM", templateFileName);
                _logger.LogError(filePath);

                var logoPath = Path.Combine(templateFileFolder, "VBOM", "logo.png");
                _logger.LogError(logoPath);



                var result = _exportService.ExportLegacyExcelBasedOnConfigurationForXBOM(reportSheets,
                                 "vBOM_"+hardwareType+"_"+opCo+"_NFV_S2R_Rev_"+ revision+" .xlsx", filePath, logoPath, data.Items.ToList(),
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

        [HttpPost("import")]
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
                var excelConfigurationEntity = _commonManager.GetExcelConfiguration("VBOM").Result;
                using var stream = file.OpenReadStream();
                var splitFileName = file.FileName.Split('.');
                var fileName = string.Empty;
                if (splitFileName[0].Contains("("))
                {
                    var split = splitFileName[0].Split(' ');
                    fileName = $"{split[0]}{split[1]}";
                }
                else
                {
                    fileName = splitFileName[0];
                }

                var checkTheFileFormat = fileName.Split('_');

                if (checkTheFileFormat.Count() == 7)
                {

                    return await _importService.ImportDataBasedOnTemplateConfigurationForVbom(file, ConstantValueFilter.vbomExcelProcessName, excelConfigurationEntity, fileName);
                }
                else
                {
                    return new ResultDto
                    {
                        Info = "Please choose the proper file name.",
                        Warning = false,
                        //Data = ex.Message
                    };
                }
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

        #region  // VNF VM Capacity
        [HttpPost("VnfCapacityFilter")]
        public async Task<List<FilterValueDto>> GetCapacityFilterAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] VnfClusterInfoQueryDto filterDto)
        {
            filterDto.Page = 0;
            filterDto.PageSize = 0;
            var data = new List<FilterValueDto>();
            try
            {              
                 return  await _vBomManager.GetCapacityFilteredValuesAsync(propertyName, propertyFilter, filterDto);               
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"{ex.StackTrace}");
                throw;
            }
        }
        #endregion
    }
}
