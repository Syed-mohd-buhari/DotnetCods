using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity.ExodusProgram;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlatformMigrationController : CamControllerBase
    {
        private readonly PlatformMigrationManager _platformMigrationManager;
        private readonly IMapper _mapper;
        private CustomGridRender<ExportService> render;
        private readonly CommonManager _commonManager;
        public PlatformMigrationController(PlatformMigrationManager  platformMigrationManager, IMapper mapper, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, CommonManager commonManager) : base(logger, contextAccessor)
        {
            _platformMigrationManager = platformMigrationManager;
            _mapper = mapper;
            _commonManager = commonManager;

        }
        #region  Get Grid, Filter 
        [HttpPost("GetAssetsForPlatformMigrationGrid")]
        public async Task<QueryResultDto<DaAssetMigrationDtoGrid>> GetAssetsForPlatformMigrationGrid (DaAssetMigrationQueryDto filterDto)

        {
            try
            {
                return await _platformMigrationManager.GetAssetsForPlatformMigrationGrid(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("GetDaAssetMigrationEntity")]
        public async Task<QueryResultDto<DaAssetMigrationDtoGrid>> FindWithConditionAsync(DaAssetMigrationQueryDto filterDto)

        {
            try
            {
                return await _platformMigrationManager.FindWithConditionAsync(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet("GetAssetPlatformDropdown")]
        public async Task<DaAssetMigrationAddUpdateDto> GetAssetPlatformDropdown(long opCoId, long dcfId, long plannedDcfId)

        {
            try
            {
                return await _platformMigrationManager.GetAssetPlatformDropdown(opCoId, dcfId, plannedDcfId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("GetFilterResult")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] DaAssetMigrationQueryDto filterDto)
        {
            try
            {
 

                List<FilterValueDto> data = await _platformMigrationManager.GetFilteredValuesAsync(propertyName, propertyFilter, filterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        #endregion



        #region CREATE,EDIT ,DELETE ,UPDATE



        [HttpPost(template: "AddUpdateDaAssetMigration")]
        public async Task<ResultDto> AddUpdateDaMigrationStatus([FromBody] DaAssetMigrationAddUpdateDto dto,long paId)
        {
            try
            {

                var result = await _platformMigrationManager.AddOrUpdateAssetMigrationAsync(dto, paId);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }



        #endregion
        #region get Planned DCF
        [HttpPost(template: "GetPlatformMigrationPlannedDCF")]
        public async Task<List<KeyValuePair<long, string>>> GetPlatformMigrationPlannedDCF([FromBody]  PlatformPlannedDcfDto _platformPlannedDcdDto)
        {
            try
            {

                var result = await _platformMigrationManager.GetPlannedDcfResourcesForPlatformMigration(_platformPlannedDcdDto);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        #endregion
    }
}
