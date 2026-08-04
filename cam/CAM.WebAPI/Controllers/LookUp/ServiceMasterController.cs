using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.ServiceMaster;
using CAM.DataTransferObjects.QueryDto;
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
    public class ServiceMasterController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ServiceMasterManager _serviceManager;
        public ServiceMasterController(ILoggerManager logger, IHttpContextAccessor contextAccessor, ServiceMasterManager serviceManager , ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _serviceManager = serviceManager;
        }

        [HttpPost("Get")]
        public async Task<ResultDto> GetServices([FromBody] ServiceMasterQueryDto dto)
        {
            try
            {
                return await _serviceManager.FindWithCondition(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] ServiceMasterQueryDto dto)
        {
            try
            {
                var data = _serviceManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "GetUpdatedPage")]
        public async Task<ServiceMasterDtoGrid> GetUpdatedPage(int id)
        {
            try
            {
                return await _serviceManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public ServiceMasterDtoGrid GetCreatePage()
        {
            try
            {
                return _serviceManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] ServiceMasterCreateDto dto)
        {
            try
            {
                return await _serviceManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(ServiceMasterUpdateDto dto)
        {
            try
            {
                return await _serviceManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }        

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(int id)
        {           

            try
            {
                return await _serviceManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
