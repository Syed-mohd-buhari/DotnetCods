using CAM.BusinessManager.ILookUp;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.LookUp.SubNetworkBoundary;
using CAM.DataTransferObjects.LookUp.VodafoneName;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    public class VodafoneNameController : CamControllerBase
    {
        private readonly IVodafoneNameManager _vodafoneNameManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;

        public VodafoneNameController(IVodafoneNameManager vodafoneNameManager, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            ICurrentUserService currentUserService, IExportService exportService) : base(logger, contextAccessor)
        {
            _vodafoneNameManager = vodafoneNameManager;
            _currentUserService = currentUserService;
            _exportService = exportService; 
        }

        [HttpGet]
        public QueryResultDto<VodafoneNameDtoGrid> GetVodafoneNames([FromQuery] VodafoneNameDtoQuery dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _vodafoneNameManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] VodafoneNameDtoQuery dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _vodafoneNameManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public VodafoneNameDtoGrid GetUpdateResourceVodafoneName(short id)
        {
            try
            {
                return _vodafoneNameManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Create")]
        public VodafoneNameDtoGrid GetCreateResourceSubNetworkBoundary()
        {
            try
            {
                return _vodafoneNameManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _vodafoneNameManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] VodafoneNameDtoQuery dto)
        {

            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = _vodafoneNameManager.GetEnityGrid(dto);
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Vodafone-Name" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] VodafoneNameDto dto)
        {
            try
            {
                return await _vodafoneNameManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        
        [HttpPut]
        public async Task<ResultDto> Put(VodafoneNameDtoUpdate vodafoneNameDtoUpdate)
        {
            try
            {
                return await _vodafoneNameManager.Update(vodafoneNameDtoUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return await _vodafoneNameManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _vodafoneNameManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
