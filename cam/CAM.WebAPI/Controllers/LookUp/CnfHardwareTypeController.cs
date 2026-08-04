using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.CnfHardwareType;
using CAM.DataTransferObjects.QueryDto.XBom.CBom;
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
    public class CnfHardwareTypeController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly CnfHardwareTypeManager _cnfHardwareTypeManager;
        private readonly ILoggerManager _loggerManager;
        public CnfHardwareTypeController(ILoggerManager logger, IHttpContextAccessor contextAccessor, CnfHardwareTypeManager cnfHardwareTypeyManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _cnfHardwareTypeManager = cnfHardwareTypeyManager;
            _loggerManager = logger;
        }

        [HttpPost("Get")]
        public async Task<ResultDto> GetProductNames([FromBody] CnfHardwareTypeQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _cnfHardwareTypeManager.GetEnityGrid(dto);
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] CnfHardwareTypeQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _cnfHardwareTypeManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<CnfHardwareTypeUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _cnfHardwareTypeManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }
        [HttpGet(template: "GetCreatepage")]
        public CnfHardwareTypeCreateDto GetCreatePage()
        {
            try
            {
                return _cnfHardwareTypeManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] CnfHardwareTypeCreateDto dto)
        {
            try
            {
                return await _cnfHardwareTypeManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(CnfHardwareTypeUpdateDto dto)
        {
            try
            {
                return await _cnfHardwareTypeManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(int id)
        {           

            try
            {
                return await _cnfHardwareTypeManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
