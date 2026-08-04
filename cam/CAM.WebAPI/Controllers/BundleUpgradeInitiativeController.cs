using CAM.BusinessManager;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.BundleUpgradeInitiative;
using CAM.Exports;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BundleUpgradeInitiativeController : CamControllerBase
    {
        private readonly BundleUpgradeInitiativeManager _bundleUpgradeInitiativeManager;
        private readonly IExportService _exportService;

        public BundleUpgradeInitiativeController(BundleUpgradeInitiativeManager bundleUpgradeInitiativeManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService) : base(logger, contextAccessor)
        {
            _bundleUpgradeInitiativeManager = bundleUpgradeInitiativeManager;
            _exportService = exportService;
        }

        [HttpPost("Get")]
        public Task<QueryResultDto<BundleUpgradeInitiativeDtoGrid>> GetBundleUpgradeInitiative([FromBody] BundleUpgradeInitiativeQueryDto filterDto)
        {
            try
            {
                return _bundleUpgradeInitiativeManager.GetEnityGrid(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }
        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] BundleUpgradeInitiativeQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = await _bundleUpgradeInitiativeManager.GetEnityGrid(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                foreach (var item in tabs)
                {
                    reportSheets.Add(item);
                }


                var result = _exportService.GetExcelFrom(reportSheets,
                    "Bundle-Upgrade" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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


        [HttpPut(template: "Restore")]
        public async Task<ResultDto> Restore(long id)
        {
            try
            {
                return await _bundleUpgradeInitiativeManager.Restore(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult([FromQuery]string propertyName, [FromQuery] string propertyFilter, [FromBody] BundleUpgradeInitiativeQueryDto filterDto)
        {
            try
            {
                var data = _bundleUpgradeInitiativeManager.GetFilter(propertyName, propertyFilter, filterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] BundleUpgradeInitiativeDtoCreate dto)
        {
            try
            {
                return await _bundleUpgradeInitiativeManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(BundleUpgradeInitiativeDtoUpdate dto)
        {
            try
            {
                return await _bundleUpgradeInitiativeManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpDelete(template:"Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            try
            {
                return await _bundleUpgradeInitiativeManager.Delete(id);
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
            try
            {
                return await _bundleUpgradeInitiativeManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async  Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _bundleUpgradeInitiativeManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public BundleUpgradeInitiativeDtoCreate GetCreateResourceBundleUpgradeInitiative()
        {
            try
            {
                return _bundleUpgradeInitiativeManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public BundleUpgradeInitiativeDtoUpdate GetUpdateResourceBundleUpgradeInitiative(short id)
        {
            try
            {
                return _bundleUpgradeInitiativeManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
