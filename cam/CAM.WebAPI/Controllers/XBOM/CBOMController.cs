using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.XBom.CBOM;
using CAM.BusinessManager.Entity.XBom.VBom;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.XBom.CBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.XBom.CBom;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Exports;
using CAM.Imports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.LoggerService;
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
    public class CBOMController : CamControllerBase
    {
        private readonly CBomManager _cBomManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly IImportService _importService;
        private CustomGridRender<ExportService> render;
        private readonly IWebHostEnvironment _env;
        private readonly CommonManager _commonManager;
        private readonly ILoggerManager _loggerManager;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly ICurrentUserService _currentUserService;

        private readonly long sessionUserId;
        private readonly List<short> _opcoList;
        private readonly bool _adminRoleCheck = false;
        public CBOMController(AuthorizedRoleManager authorizedRoleManager, ICurrentUserService currentUserService, CBomManager cBomManager, IMapper mapper, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, IExportService exportService, IImportService importService, CommonManager commonManager, IWebHostEnvironment env) : base(logger, contextAccessor)
        {
            _cBomManager = cBomManager;
            _exportService = exportService;
            _mapper = mapper;
            _importService = importService;
            _commonManager = commonManager;
            _env = env;
            _loggerManager = logger;

            _authorizedRoleManager = authorizedRoleManager;
            _currentUserService = currentUserService;
            sessionUserId = _currentUserService.UserId;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
        }
        #region  Get Grid, Filter 
        [HttpPost("GetCbomClusterinfoDetails")]
        public async Task<QueryResultDto<CnfClusterInfoDtoGrid>> CbomClusterinfoDetailsAsync([FromBody] CnfClusterInfoQueryDto filterDto)

        {
            try
            {
                if (filterDto.opcoName == null || filterDto.opcoName != null && filterDto.opcoName.Count == 0)
                {
                    filterDto.opcoName = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
                }
                return await _cBomManager.CbomClusterinfoDetailsAsync(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost("GetCbomPodInfoAndCapacityDetails")]
        public async Task<QueryResultDto<CnfInfoAndCapacityDtoGrid>> GetCbomPodInfoAndCapacityDetailsAsync([FromBody] CnfClusterInfoQueryDto filterDto)

        {
            try
            {
                return await _cBomManager.GetCnfInfoAndCapacityEntities(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("FindWithConditionAsync")]
        public async Task<QueryResultDto<CnfInfoAndCbomExportGridDto>> FindWithConditionAsync([FromBody] CnfClusterInfoQueryDto filterDto)

        {
            try
            {
                return await _cBomManager.FindWithConditionAsync(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter,
       [FromBody] CnfClusterInfoQueryDto filterDto,bool isInstance = false)
        {
            filterDto.Page = 0;
            filterDto.PageSize = 0;
            var data = new List<FilterValueDto>();
            if (filterDto.opcoName == null || filterDto.opcoName != null && filterDto.opcoName.Count == 0)
            {
                filterDto.opcoName = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
            }
            try
            {
                if(isInstance)                 
                    data = await _cBomManager.GetPodLevelFilteredValues(propertyName, propertyFilter, filterDto);                   
             
                else  
                  data = await _cBomManager.GetFilteredValues(propertyName, propertyFilter, filterDto);

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost(template: "PodLevelFilter")]
        public async Task<List<FilterValueDto>> GetPodLevelFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter,
      [FromBody] CnfClusterInfoQueryDto filterDto)
        {
            try
            {
                var data = await _cBomManager.GetPodLevelFilteredValues(propertyName, propertyFilter, filterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        #endregion

        #region // CNF Capacity
        [HttpPost("CnfCapacityFilter")]
        public async Task<List<FilterValueDto>> GetCapacityFilterAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] CnfCapcityQueryDto filterDto)
        {
            filterDto.Page = 0;
            filterDto.PageSize = 0;
            var data = new List<FilterValueDto>();
            try
            {
                return await _cBomManager.GetCapacityFilteredValuesAsync(propertyName, propertyFilter, filterDto);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"{ex}");
                throw;
            }
        }

        #endregion


        #region CREATE,EDIT ,DELETE ,UPDATE
        [HttpGet(template: "CreateCBomPageResource")]
        public async Task<CBomCreatePageEntityDto> GetCBomPageResource()
        {
            try
            {
                return await _cBomManager.GetCBomCreatePageResourceAsync(_opcoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "EditCBomPageResource{id}")]
        public async Task<ResultDto> GetEditCBomPage(long id)
        {
            try
            {
                return await _cBomManager.GetEditCBomPageResourceAsync(id, _opcoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "AddNewCBomResource")]
        public async Task<ResultDto> InsertNewCBomResource([FromBody] CBomInsertUpdateDto dto)
        {
            try
            {

                var result = await _cBomManager.AddCBomDetailAsync(dto, false);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }



        [HttpPost(template: "UpdateExistingCBomResource")]
        public async Task<ResultDto> UpdateCBomResource([FromBody] CBomInsertUpdateDto dto)
        {
            try
            {

                var result = await _cBomManager.AddCBomDetailAsync(dto, true);
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
                var result = await _cBomManager.GetLinkedInfoBasedOnCapacity(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteCbomEntityBasedOnCapacity")]
        public async Task<ResultDto> DeleteDeep(long id)
        {
            try
            {
                var result = await _cBomManager.DeleteCnfCapacityBasedAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "DeleteCbomEntity")]
        public async Task<ResultDto> DeleteDeep(long infoId = 0, long instanceId = 0, long capacityId = 0)
        {
            try
            {
                var result = await _cBomManager.DeleteCbomAsync(infoId, instanceId, capacityId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        #endregion

        #region Export
        [HttpPost(template: "ExportReport")]
        public async Task<IActionResult> ExportReportLegacyFile([FromBody] CnfClusterInfoQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = await _cBomManager.FindWithConditionAsync(dto);
                if (dto.opcoName == null || dto.opcoName != null && dto.opcoName.Count == 0)
                {
                    dto.opcoName = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
                }
                render = new CustomGridRender<ExportService>()
                {
                    Render = new List<RenderDetail>()
                };
                List<ExportSheet> reportSheets = new List<ExportSheet>();
                ExportSheet dataSheet = new ExportSheet();
                if (data.Items.Count > 0)
                {
                    var clusterLocationBasedData = data.Items[0].cbomExport.ToList();
                    var cnfInfoData = data.Items[0].cnfInfoExport.ToList();

                    if (clusterLocationBasedData.Count > 0)
                    {
                        dataSheet = new ExportSheet()
                        {
                            Data = clusterLocationBasedData.Cast<object>().ToList(),
                            TabName = "cBOM"
                        };
                        reportSheets.Add(dataSheet);
                    }
                    if (cnfInfoData.Count > 0)
                    {
                        dataSheet = new ExportSheet()
                        {
                            Data = cnfInfoData.Cast<object>().ToList(),
                            TabName = "CNF_info"
                        };
                        reportSheets.Add(dataSheet);
                    }

                    render.Render.AddRange(data.GridRender.Render);


                    var excelConfigurationCbom = _commonManager.GetExcelConfiguration("cbom").Result;
                    var excelConfigurationForCNFInfo = _commonManager.GetExcelConfiguration("CNFINFO").Result;





                    var templateFileFolder = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                    _logger.LogError(templateFileFolder);
                    string templateFileName = excelConfigurationCbom.FirstOrDefault(x => x.Templatefilename != null)?.Templatefilename?.Trim();
                    _logger.LogError(templateFileName);

                    if (templateFileName == null)
                    {
                        throw new Exception("CBOM details is missing in Excel template configuration");
                    }

                    var filePath = Path.Combine(templateFileFolder, "CBOM", templateFileName);
                    _logger.LogError(filePath);

                    var logoPath = Path.Combine(templateFileFolder, "CBOM", "logo.png");
                    _logger.LogError(logoPath);



                    var result = _exportService.ExportLegacyExcelBasedOnConfigurationForCBOM(reportSheets,
                                    clusterLocationBasedData.Select(x => x.OpcoName).FirstOrDefault() + "_VF_S2R-BOM_ALL NFs_" + clusterLocationBasedData.Select(x => x.Site).FirstOrDefault() + "_SCN-B_"+ clusterLocationBasedData.Select(x => x.HardwareType).FirstOrDefault() + "-Rev " + clusterLocationBasedData.Select(x => x.Revision).FirstOrDefault() + ".xlsx", filePath, logoPath, data.Items[0].cnfInfoExport.ToList(),
                                     excelConfigurationCbom.ToList(), excelConfigurationForCNFInfo.ToList());

                    HttpContext.Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                    var fileContentResult = new FileContentResult(result.FileInByteArray, "application/vnd.ms-excel")
                    {
                        FileDownloadName = result.FileName
                    };
                    return fileContentResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        #endregion

        #region //Import
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
                var excelConfigurationEntity = _commonManager.GetExcelConfiguration("CBOM").Result;
                var excelConfigurationForCNFInfo = _commonManager.GetExcelConfiguration("CNFINFO").Result;
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

                    return await _importService.ImportDataBasedOnTemplateConfigurationForCbom(file, ConstantValueFilter.cbomExcelProcessName, excelConfigurationEntity, excelConfigurationForCNFInfo, fileName);
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

        #endregion

    }
}
