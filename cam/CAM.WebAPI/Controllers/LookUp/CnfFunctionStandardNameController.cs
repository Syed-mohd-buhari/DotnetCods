using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.CnfFunctionstandardname;
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
    public class CnfFunctionStandardNameController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly CnfFunctionStandardNameManager _cnfFunctionStandardNameManager;
        private readonly ILoggerManager _loggerManager;
        public CnfFunctionStandardNameController(ILoggerManager logger, IHttpContextAccessor contextAccessor, CnfFunctionStandardNameManager cnfFunctionStandardNameManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _cnfFunctionStandardNameManager = cnfFunctionStandardNameManager;
            _loggerManager = logger;
        }

        [HttpPost("Get")]
        public async Task<ResultDto> GetProductNames([FromBody] CnfFunctionStandardNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _cnfFunctionStandardNameManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] CnfFunctionStandardNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _cnfFunctionStandardNameManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<CnfFunctionStandardNameUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _cnfFunctionStandardNameManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public CnfFunctionStandardNameCreateDto GetCreatePage()
        {
            try
            {
                return _cnfFunctionStandardNameManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] CnfFunctionStandardNameCreateDto dto)
        {
            try
            {
                return await _cnfFunctionStandardNameManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(CnfFunctionStandardNameUpdateDto dto)
        {
            try
            {
                return await _cnfFunctionStandardNameManager.Update(dto);
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
                return await _cnfFunctionStandardNameManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
