using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.LcmDeploymentStatus;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
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
    
    public class LcmDeploymentStatusController : CamControllerBase
    {
        private readonly ILcmDeploymentStatusManager _lcmDeploymentStatusManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;

        public LcmDeploymentStatusController(ILcmDeploymentStatusManager lcmDeploymentStatusManager, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            ICurrentUserService currentUserService, IExportService exportService) : base(logger, contextAccessor)
        {
            _lcmDeploymentStatusManager = lcmDeploymentStatusManager;
            _currentUserService = currentUserService;
            _exportService = exportService; 
        }

        [HttpGet]
        public QueryResultDto<LcmDeploymentStatusDtoGrid> GetLcmDeploymentStatus([FromQuery] LcmDeploymentStatusDtoQuery dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _lcmDeploymentStatusManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] LcmDeploymentStatusDtoQuery dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _lcmDeploymentStatusManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public LcmDeploymentStatusDtoGrid GetUpdateResourceLcmDeploymentStatus(short id)
        {
            try
            {
                return _lcmDeploymentStatusManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        //[HttpGet(template: "Create")]
        //public LcmDeploymentStatusDtoGrid GetCreateResourceLcmDeploymentStatus()
        //{
        //    return _lcmDeploymentStatusManager.GetCreatePage();
        //}

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _lcmDeploymentStatusManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] LcmDeploymentStatusDtoQuery dto)
        {

            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = _lcmDeploymentStatusManager.GetEnityGrid(dto);
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Lcm Deployment Status" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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
        public async Task<ResultDto> Create([FromBody] LcmDeploymentStatusDto dto)
        {
            try
            {
                return await _lcmDeploymentStatusManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        
        [HttpPut]
        public async Task<ResultDto> Put(LcmDeploymentStatusDtoUpdate vodafoneNameDtoUpdate)
        {
            try
            {
                return await _lcmDeploymentStatusManager.Update(vodafoneNameDtoUpdate);
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
                return await _lcmDeploymentStatusManager.Delete(id);
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
                return await _lcmDeploymentStatusManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}