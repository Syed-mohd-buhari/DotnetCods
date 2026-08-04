using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.VNFName;
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
    public class VnfNameController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly VnfNameManager _vnfNameManager;
        private readonly ILoggerManager _loggerManager;
        public VnfNameController(ILoggerManager logger, IHttpContextAccessor contextAccessor, VnfNameManager vnfNameManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _vnfNameManager = vnfNameManager;
            _loggerManager = logger;
        }

        [HttpPost("Get")]
        public Task<ResultDto> GetProductNames([FromBody] VnfNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _vnfNameManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] VnfNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _vnfNameManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<VnfNameUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _vnfNameManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public async Task<VnfNameCreateDto> GetCreatePage()
        {
            try
            {
                return await _vnfNameManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] VnfNameCreateDto dto)
        {
            try
            {
                return await _vnfNameManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(VnfNameUpdateDto dto)
        {
            try
            {
                return await _vnfNameManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(int id)
        {          
            try
            {
                return await _vnfNameManager.Delete(id);
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
                return await _vnfNameManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
