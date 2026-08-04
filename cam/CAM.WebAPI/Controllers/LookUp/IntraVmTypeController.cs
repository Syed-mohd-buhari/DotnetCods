using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.IntraVMType;
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
    public class IntraVmTypeController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IntraVmTypeManager _intraVmTypeManager;
        private readonly ILoggerManager _loggerManager;
        public IntraVmTypeController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IntraVmTypeManager intraVmTypeManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _intraVmTypeManager = intraVmTypeManager;
            _loggerManager = logger;
        }

        [HttpPost("Get")]
        public async Task<ResultDto> GetProductNames([FromBody] IntraVmTypeQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _intraVmTypeManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] IntraVmTypeQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _intraVmTypeManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<IntraVmTypeUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _intraVmTypeManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public IntraVmTypeCreateDto GetCreatePage()
        {
            try
            {
                return _intraVmTypeManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] IntraVmTypeCreateDto dto)
        {
            try
            {
                return await _intraVmTypeManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(IntraVmTypeUpdateDto dto)
        {
            try
            {
                return await _intraVmTypeManager.Update(dto);
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
                return await _intraVmTypeManager.Delete(id);
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
                return await _intraVmTypeManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
