using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity.ExodusProgram;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DaMigrationStatus;
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
    public class DaMigrationStatusController : CamControllerBase
    {
        private readonly DaMigrationStatusManager _daMigrationStatusManager;     
        private readonly IMapper _mapper;      
        private CustomGridRender<ExportService> render;        
        private readonly CommonManager _commonManager;
        public DaMigrationStatusController(DaMigrationStatusManager  daMigrationStatusManager, IMapper mapper, ILoggerManager logger,
            IHttpContextAccessor contextAccessor,  CommonManager commonManager ) : base(logger, contextAccessor)
        {
            _daMigrationStatusManager = daMigrationStatusManager;           
            _mapper = mapper;           
            _commonManager = commonManager;
            
        }
        #region  Get Grid, Filter 
        [HttpPost("GetDaMigrationStatusDetails")]
        public async Task<QueryResultDto<DaMigrationStatusDtoGrid>> DaMigrationStatusDetailsAsync([FromBody] DaMigrationStatusQueryDto filterDto)

        {
            try
            {
                return await _daMigrationStatusManager.FindWithConditionAsync(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
       

        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter,
       [FromBody] DaMigrationStatusQueryDto filterDto )
        {
            filterDto.Page = 0;
            filterDto.PageSize = 0;
            var data = new List<FilterValueDto>();
            try
            {                
                    data = await _daMigrationStatusManager.GetFilters(propertyName, propertyFilter, filterDto);               
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

        [HttpGet("GetUpdateDaMigrationRecords{id}")]
        public async Task<DaMigrationStatusAddUpdateDto> GetUpdateDaMigrationRecords(long id)

        {
            try
            {
                return await _daMigrationStatusManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        [HttpPost(template: "AddUpdateDaMigrationStatus")]
        public async Task<ResultDto> AddUpdateDaMigrationStatus([FromBody] DaMigrationStatusAddUpdateDto dto)
        {
            try
            {

                var result = await _daMigrationStatusManager.AddOrUpdateLocationAsync(dto);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

       

        #endregion

    }
}
