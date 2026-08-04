using AutoMapper;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NfviSoftwareCompatibility;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Imports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NfviSoftwareCompatibilityController : CamControllerBase
    {
        private readonly NfviSoftwareCompatibilityManager _nfviSoftwareCompatibilityManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly IImportService _importService;
        private readonly NfviSoftwareCompatibilityImport _nfviImport;


        public NfviSoftwareCompatibilityController(NfviSoftwareCompatibilityManager nfviSoftwareCompatibilityManager, IMapper mapper, ILoggerManager logger, 
            IHttpContextAccessor contextAccessor, IExportService exportService,IImportService importService, NfviSoftwareCompatibilityImport nfviImport) : base(logger, contextAccessor)
        {
            _nfviSoftwareCompatibilityManager = nfviSoftwareCompatibilityManager;
            _exportService = exportService;
            _mapper = mapper;
            _importService = importService;
            _nfviImport = nfviImport;
        }

        [HttpPost("GetNfviSoftwareCompatibility")]
        public async Task<QueryResultDto<NfviSoftwareCompatibilityDtoGrid>> GetNfviSoftwareCompatibilityAsync([FromBody] NfviSoftwareCompatibilityQueryDto filterDto)

        {
            try
            {
                return await _nfviSoftwareCompatibilityManager.FindWithCondition(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter,
          [FromBody] NfviSoftwareCompatibilityQueryDto filterDto)
        {
            try
            {
                var data = await _nfviSoftwareCompatibilityManager.GetFilteredValuesAsync(propertyName, propertyFilter, filterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "CreateNfviSoftwareCompatibility")]
        public async Task<NfviSoftwareCompatibilityDto> CreatePageNfviDetial()
        {
            try
            {
                var result = await _nfviSoftwareCompatibilityManager.CreatePageNfviDetialAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "EditNfviSoftwareCompatibility{nfviSwCompatId}")]
        public async Task<NfviSoftwareCompatibilityDto> EditPageNfviDetial(long nfviSwCompatId)
        {
            try
            {
                var result = await _nfviSoftwareCompatibilityManager.EditPageNfviDetialAsync(nfviSwCompatId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpPost(template: "CreateOrUpdateNfviSoftwareCompat")]
        public async Task<ResultDto> CreateOrUpdateNfviSw([FromBody] NfviSoftwareCompatibilityCreateEditPageDto dto)
        {
            try
            {

                var result = await _nfviSoftwareCompatibilityManager.AddOrUpdateNfviSoftwareCompatAsync(dto);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "DeleteNfviSoftware")]
        public async Task<ResultDto> DeleteDeep(long Id)
        {
            try
            {
                var result = await _nfviSoftwareCompatibilityManager.DeleteDeep(Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] NfviSoftwareCompatibilityQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                var data = await _nfviSoftwareCompatibilityManager.FindWithCondition(dto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);
                string IdColumn = _nfviImport.IdColumn;
                List<string> EditableColumnList = _nfviImport.EditableColumnList;

                var result = _exportService.GetExcelFrom(reportSheets,
                    "NfviSofwareCompatibility_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender,IdColumn,EditableColumnList);

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
        [HttpPost(template: "ImportReport")]
        public async Task<ResultDto> ImportReports()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                IFormFile File = httpRequest.Form.Files.FirstOrDefault();
                return await _importService.Processimportexcel(File, "nfvisoftwarecompatibility");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
  