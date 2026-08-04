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
    public class NetworkElementClusterAsPlannedController : CamControllerBase
    {
        private readonly NetworkElementClusterAsPlannedManager _networkElementClusterAsPlannedManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly BPTManager _bPTManager;
        private readonly CommonManager _commonManager;
        public NetworkElementClusterAsPlannedController(NetworkElementClusterAsPlannedManager networkElementClusterAsPlannedManager, IMapper mapper, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, IExportService exportService, IWebHostEnvironment env, BPTManager bPTManager
            , CommonManager commonManager) : base(logger, contextAccessor)
        {
            _networkElementClusterAsPlannedManager = networkElementClusterAsPlannedManager;
            _exportService = exportService;
            _mapper = mapper;
            _env = env;
            _bPTManager= bPTManager;
            _commonManager = commonManager;

        }


        [HttpPost("GetNWElementClusterAsPlanned")]
        public async Task<QueryResultDto<NWElementClusterAsPlannedDtoGrid>> GetNWElementClusterAsPlannedAsync([FromBody] NWElementClusterAsPlannedQueryDto filterDto)
        {
            try
            {
                return await _networkElementClusterAsPlannedManager.FindWithConditionAsync(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }


        [HttpPost(template: "CreateInfraClusterClusterUpgradeUpsert")]
        public async Task<ResultDto> Create([FromBody] List<NWElementClusterAsPlannedUpSertDto> dtoNetworkElementClusterAsPlannedRecords, long InfraClusterAsPlanned)
        {
            try
            { 

                var result = await _networkElementClusterAsPlannedManager.AddOrRemoveNetWorkClusterAsync(dtoNetworkElementClusterAsPlannedRecords, InfraClusterAsPlanned);
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
            [FromBody] NWElementClusterAsPlannedQueryDto nwElementClusterAsPlannedQueryDto)
        {
            try
            {
                var data = await _networkElementClusterAsPlannedManager.GetFilteredValuesAsync(propertyName, propertyFilter, nwElementClusterAsPlannedQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "CreateNetworkElementCluster")]
        public async Task<NWElementClusterAsPlannedUpSertDto> CreateNetworkElementCluster()
        {
            try
            {
                return await _networkElementClusterAsPlannedManager.GetAddPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

    }
}
  