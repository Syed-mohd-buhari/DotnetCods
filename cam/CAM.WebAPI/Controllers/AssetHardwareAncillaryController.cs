using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.ExodusProgram;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AssetHardwareConfig;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.Entita.LcmAncillaryData;
using CAM.DataTransferObjects.Entita.NetworkElement;
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
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class AssetHardwareAncillaryController : CamControllerBase 
    {
        private readonly AssetHardwareAncillaryManager _assetHardwareAncillaryManager;
        private readonly IMapper _mapper;
        private CustomGridRender<ExportService> render;
        private readonly CommonManager _commonManager;
        public AssetHardwareAncillaryController(AssetHardwareAncillaryManager assetHardwareAncillaryManager, IMapper mapper, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, CommonManager commonManager) : base(logger, contextAccessor)
        {
            _assetHardwareAncillaryManager = assetHardwareAncillaryManager;
            _mapper = mapper;
            _commonManager = commonManager;

        }
        #region  Get Grid, Filter 
        
        [HttpPost("GetAssetAncillaries")]
        public async Task<QueryResultDto<AssetHardwareAncillaryGridDto>> FindWithConditionAsync(AssetHardwareAncillaryQueryDto filterDto)

        {
            try
            {
                return await _assetHardwareAncillaryManager.FindWithConditionAsync(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        #endregion



        #region CREATE,EDIT ,DELETE ,UPDATE



        [HttpPost(template: "AddUpdateAssetAncillary")]
        public async Task<ResultDto> AddUpdateAssetAncillary([FromBody] AssetHardwareAncillaryAddUpdateDto dto)
        {
            try
            {

                var result = await _assetHardwareAncillaryManager.AddOrUpdateAssetAncillary(dto);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }



        #endregion
        #region Create /Edit Page
        [HttpPost("CreateAssetAncillaryPage")]
        public async Task<AssetHardwareAncillaryAddUpdateDto> CreateAssetHardwareAncillary([FromBody] AssetAncillaryCreateEditDto dto)
        {
            try
            {
                return await _assetHardwareAncillaryManager.CreatePageAssetHardwareAncillary(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("EditAssetAncillaryPage")]
        public async Task<AssetHardwareAncillaryAddUpdateDto> UpdateAssetHardwareAncillary([FromBody] AssetAncillaryCreateEditDto dto)
        {
            try
            {
                return await _assetHardwareAncillaryManager.EditPageAssetHardwareAncillary(dto);
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