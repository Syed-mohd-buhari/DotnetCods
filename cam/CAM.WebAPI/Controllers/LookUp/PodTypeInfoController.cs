using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.PodTypeInfo;
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
    public class PodTypeInfoController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly PodTypeInfoManager _podTypeInfoManager;
        private readonly ILoggerManager _loggerManager;
        public PodTypeInfoController(ILoggerManager logger, IHttpContextAccessor contextAccessor, PodTypeInfoManager podTypeInfoManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _podTypeInfoManager = podTypeInfoManager;
            _loggerManager = logger;
        }

        [HttpPost("Get")]
        public async Task<ResultDto> GetProductNames([FromBody] PodTypeInfoQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _podTypeInfoManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] PodTypeInfoQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _podTypeInfoManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<PodTypeInfoUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _podTypeInfoManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public PodTypeInfoCreateDto GetCreatePage()
        {
            try
            {
                return _podTypeInfoManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] PodTypeInfoCreateDto dto)
        {
            try
            {
                return await _podTypeInfoManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(PodTypeInfoUpdateDto dto)
        {
            try
            {
                return await _podTypeInfoManager.Update(dto);
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
                return await _podTypeInfoManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
