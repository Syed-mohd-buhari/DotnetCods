using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.VnfHardwareType;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VnfHardwareTypeController : CamControllerBase
    {
        private readonly VnfHardwareTypeManager _vnfHardwareTypeManager;
        private readonly ILoggerManager _loggerManager;
        public VnfHardwareTypeController(ILoggerManager logger, IHttpContextAccessor contextAccessor, VnfHardwareTypeManager vnfHardwareTypeManager) : base(logger, contextAccessor)
        {
            _vnfHardwareTypeManager = vnfHardwareTypeManager;
            _loggerManager = logger;
        }

        [HttpPost("GetVbomHardware")]
        public async Task<ResultDto> GetVbomHardware([FromBody] VnfHardwareTypeQueryDto dto)
        {
            try
            {
                return await _vnfHardwareTypeManager.FindWithCondition(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] VnfHardwareTypeQueryDto dto)
        {
            try
            {
                var data = _vnfHardwareTypeManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<VnfHardwareTypeUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _vnfHardwareTypeManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public VnfHardwareTypeCreateDto GetCreatePage()
        {
            try
            {
                return _vnfHardwareTypeManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("AddVbomHardware")]
        public async Task<ResultDto> AddVbomHardware([FromBody] VnfHardwareTypeCreateDto dto)
        {
            try
            {
                return await _vnfHardwareTypeManager.AddVbomHardware(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("UpdateVbomHardware")]
        public async Task<ResultDto> UpdateVbomHardware(VnfHardwareTypeUpdateDto dto)
        {
            try
            {
                return await _vnfHardwareTypeManager.UpdateVbomHardware(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteVbomHardware")]
        public async Task<ResultDto> DeleteVbomHardware(int id)
        {

            try
            {
                return await _vnfHardwareTypeManager.DeleteVbomHardware(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


    }
}
