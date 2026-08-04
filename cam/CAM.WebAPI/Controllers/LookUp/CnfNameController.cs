using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.CnfName;
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
    public class CnfNameController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly CnfNameManager _cnfNameManager;
        private readonly ILoggerManager _loggerManager;
        public CnfNameController(ILoggerManager logger, IHttpContextAccessor contextAccessor, CnfNameManager cnfNameManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _cnfNameManager = cnfNameManager;
            _loggerManager = logger;
        }

        [HttpPost("Get")]
        public async Task<ResultDto> GetProductNames([FromBody] CnfNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _cnfNameManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] CnfNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _cnfNameManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }

        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<CnfNameUpdateDto> GetUpdatedPage(int id)
        {
            try
            {
                return await _cnfNameManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }

        }
        [HttpGet(template: "GetCreatepage")]
        public CnfNameCreateDto GetCreatePage()
        {
            try
            {
                return _cnfNameManager.GetCreatePage();
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] CnfNameCreateDto dto)
        {
            try
            {
                return await _cnfNameManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(CnfNameUpdateDto dto)
        {
            try
            {
                return await _cnfNameManager.Update(dto);
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
                return await _cnfNameManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
