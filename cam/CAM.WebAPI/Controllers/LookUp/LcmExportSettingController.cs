using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CAM.DataTransferObjects.LookUp.VodafoneName;
using System.Linq;
using CAM.BusinessManager.LookUp;
using CAM.DataTransferObjects.LookUp.SubNetworkBoundary;
using CAM.DataTransferObjects.LookUp.LcmExportSetting;
using Microsoft.AspNetCore.Authorization;
using CAM.BusinessManager.Grid.QueryResultImplementation;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LcmExportSettingController : CamControllerBase
    {
        private readonly LcmExportSettingManager _LcmExportSettingManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;

        public LcmExportSettingController(LcmExportSettingManager LcmExportSettingManager, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            ICurrentUserService currentUserService, IExportService exportService) : base(logger, contextAccessor)
        {
            _LcmExportSettingManager = LcmExportSettingManager;
            _currentUserService = currentUserService;
            _exportService = exportService; 
        }

        [HttpGet]
        public async Task<QueryResultDto<LcmExportSettingDtoGrid>> GetCategoriesAsync([FromQuery] LcmExportSettingDtoQuery dto)
        {
            try
            {
                //dto.IsHistorical = new List<bool> { false };
                var lcmExportEntities = await _LcmExportSettingManager.GetEnityGrid(dto);

                ///1367 - Default Selection of LCM Label on LCM Export to be fixed - Currently, it should display the label as R10.
                var defaultExportEntities = lcmExportEntities?.Items?.Where(x => x.IsDefault == true)?.ToList();
                var nonDefaultExportEntities = lcmExportEntities?.Items?.Where(x => x.IsDefault == false)?.ToList();

                // Merge default and non-default entities
                defaultExportEntities?.AddRange(nonDefaultExportEntities);
                lcmExportEntities.Items = defaultExportEntities;

                return lcmExportEntities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet("GetHistoricalReports")]
        public async Task<QueryResultDto<LcmExportSettingDtoGrid>> GetHistoricalReports([FromQuery] LcmExportSettingDtoQuery dto)
        {
            try
            {
                dto.IsHistorical = new List<bool> { true };
                var lcmExportEntities = await _LcmExportSettingManager.GetEnityGrid(dto);

                return lcmExportEntities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] LcmExportSettingDtoQuery dto)
        {
            try
            {
                var data = _LcmExportSettingManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public LcmExportSettingDtoUpdate GetUpdateResourceLcmExportSetting(short id)
        {
            try
            {
                return _LcmExportSettingManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("Create")]
        public LcmExportSettingDtoCreate GetCreate()
        {
            try
            {
                return _LcmExportSettingManager.GetCreatePage();
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
                return await _LcmExportSettingManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] LcmExportSettingDtoCreate dto)
        {
            try
            {
                return await _LcmExportSettingManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        
        [HttpPut]
        public async Task<ResultDto> Put(LcmExportSettingDtoUpdate LcmExportSettingDtoUpdate)
        {
            try
            {
                return await _LcmExportSettingManager.Update(LcmExportSettingDtoUpdate);
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
                return await _LcmExportSettingManager.Delete(id);
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
                return await _LcmExportSettingManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
