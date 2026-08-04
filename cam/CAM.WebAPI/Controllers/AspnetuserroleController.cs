using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Aspnetuserrole;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.DataTransferObjects.Entita.Organisation;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class AspnetuserroleController : CamControllerBase
    {
        private readonly AspnetuserroleManager _manager;
        private readonly IExportService _exportService;
        public AspnetuserroleController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, AspnetuserroleManager manager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
        }


        [HttpPost("Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] AspnetuserroleQueryDto aspNetUserRoleDto)
        {
            try
            {
                return await _manager.GetFilter(propertyName, propertyFilter, aspNetUserRoleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet("GetCreate")]
        public async Task<AspNetUserRoleCreateOrUpdateDto> GetCreate()
        {
            try
            {
                return await _manager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpDelete("Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                return await _manager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut("UpdateOrCreate")]
        public async Task<ResultDto> UpdateOrCreate([FromBody] AspNetUserRBAMCreateAndUpdateDto aspNetUserRoleDto)
        {
            try
            {
                return await _manager.UpdateOrCreate(aspNetUserRoleDto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost("GetROV")]

        public async Task<ResultDto> GetUserRoles([FromBody] AspnetuserroleQueryDto aspNetUserRoleDto)
        {
            try
            {
                return await _manager.GetUserRoles(aspNetUserRoleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        
    }
}
