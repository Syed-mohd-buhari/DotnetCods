using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.VmWorkloadType;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
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
    public class VmWorkloadTypeController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly VmWorkloadTypeManager _vmWorkloadTypeManager;
        private readonly ILoggerManager _loggerManager;
        public VmWorkloadTypeController(ILoggerManager logger, IHttpContextAccessor contextAccessor, VmWorkloadTypeManager vmWorkloadTypeManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _vmWorkloadTypeManager = vmWorkloadTypeManager;
            _loggerManager = logger;
        }

        [HttpPost("Get")]
        public async Task<ResultDto> GetProductNames([FromBody] VmWorkloadTypeQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _vmWorkloadTypeManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] VmWorkloadTypeQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _vmWorkloadTypeManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<VmWorkloadTypeUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _vmWorkloadTypeManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public VmWorkloadTypeCreateDto GetCreatePage()
        {
            try
            {
                return _vmWorkloadTypeManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] VmWorkloadTypeCreateDto dto)
        {
            try
            {
                return await _vmWorkloadTypeManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(VmWorkloadTypeUpdateDto dto)
        {
            try
            {
                return await _vmWorkloadTypeManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(int id)
        {          
            try
            {
                return await _vmWorkloadTypeManager.Delete(id);
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
                return await _vmWorkloadTypeManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
