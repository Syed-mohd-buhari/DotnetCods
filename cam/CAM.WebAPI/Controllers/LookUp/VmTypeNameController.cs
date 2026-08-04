using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.VMTypeName;
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
    public class VmTypeNameController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly VmTypeNameManager _vmTypeNameManager;
        private readonly ILoggerManager _loggerManager;
        public VmTypeNameController(ILoggerManager logger, IHttpContextAccessor contextAccessor, VmTypeNameManager vmTypeNameManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _vmTypeNameManager = vmTypeNameManager;
            _loggerManager = logger;
        }

        [HttpPost("Get")]
        public Task<ResultDto> GetProductNames([FromBody] VmTypeNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _vmTypeNameManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] VmTypeNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _vmTypeNameManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<VmTypeNameUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _vmTypeNameManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }
        [HttpGet(template: "GetCreatepage")]
        public VmTypeNameCreateDto GetCreatePage()
        {
            try
            {
                return _vmTypeNameManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] VmTypeNameCreateDto dto)
        {
            try
            {
                return await _vmTypeNameManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(VmTypeNameUpdateDto dto)
        {
            try
            {
                return await _vmTypeNameManager.Update(dto);
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
                return await _vmTypeNameManager.Delete(id);
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
                return await _vmTypeNameManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
