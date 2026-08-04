using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.BPT;
using CAM.BusinessManager.Entity.ClusterLevelPA;
using CAM.BusinessManager.Entity.ComponentSoftware;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ClusterLevelPA;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.BPT;
using CAM.DataTransferObjects.QueryDto.ClusterLevelPA;
using CAM.DataTransferObjects.QueryDto.ComponentSoftware;
using CAM.Exports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.ClusterLevelPA
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClusterLevelPAController : CamControllerBase
    {
        private readonly InfraClusterAsPlannedManager _infraClusterManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly BPTManager _bPTManager;
        private readonly CommonManager _commonManager;
        public ClusterLevelPAController(InfraClusterAsPlannedManager infraClusterManager, IMapper mapper, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, IExportService exportService, IWebHostEnvironment env, BPTManager bPTManager
            , CommonManager commonManager) : base(logger, contextAccessor)
        {
            _infraClusterManager = infraClusterManager;
            _exportService = exportService;
            _mapper = mapper;
            _env = env;
            _bPTManager= bPTManager;
            _commonManager = commonManager;

        }

        [HttpPost("GetInfraClusterPaLevel")]
        public async Task<QueryResultDto<InfraClusterAsPlannedDtoGrid>> GetPlannedActivitiesByConditionAsync([FromBody] InfraClusterAsPlannedQueryDto filterDto)
        {
            try
            {
                return await _infraClusterManager.GetPlannedActivitiesByConditionAsync(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("GetSitelevelClusters")]
        public async Task<List<SiteLevelClusterDto>> GetSitelevelClusters([FromBody] InfraClusterAsPlannedQueryDto filterDto)
        {
            try
            {
                return await _infraClusterManager.GetSitelevelClusters(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }


        [HttpGet(template: "CreateInfraCluster/{opCoId}/{DcId}")]
        public async Task<InfraClusterClusterUpgradeUpsertDto> CreateInfraCluster(  short opCoId, short DcId)
        {
            try
            {
                return await _infraClusterManager.GetAddPage( opCoId, DcId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "EditInfraClusterClusterUpgradeResource/{infrClusterId}/{opCoId}/{DcId}")]
        public async Task<InfraClusterClusterUpgradeUpsertDto> GetEditInfra(long infrClusterId, short opCoId, short DcId)
        {
            try
            {
                return await _infraClusterManager.GetUpdatePage(infrClusterId,   opCoId,   DcId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

       
        [HttpPost(template: "CreateInfraClusterClusterUpgradeUpsert")]
        public async Task<ResultDto> Create([FromBody] InfraClusterClusterUpgradeUpsertDto dtoInfraClusterRecords, long paId)
        {
            try
            { 

                var result = await _infraClusterManager.AddOrUpdateInfraClusterAsync(dtoInfraClusterRecords, paId);
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
            [FromBody] InfraClusterAsPlannedQueryDto buildBagFilterDto)
        {
            try
            {
                var data = await _infraClusterManager.GetFilteredValuesAsync(propertyName, propertyFilter, buildBagFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "GetPlannedHardwareTypes/{opCoId}/{dcId}")]
        public List<KeyValuePairDto> GetPlannedHardwareTypes(short opCoId, short dcId)
        {
            try
            {
                var data = _infraClusterManager.GetAddPage(opCoId, dcId).Result.HardwareMhwResource;
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("GetInfraClusterListBasedonOpco")]
        public async Task<List<NWElementClusterAsPlannedDto>> GetInfraClusterListBasedonOpco([FromBody] InfraClusterAsPlannedQueryDto filterDto)
        {
            try
            {
                return await _infraClusterManager.GetInfraClusterListBasedonOpco(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

    }
}
  