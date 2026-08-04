using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.TeamManagement;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.TeamManagement;
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

    public class TeamMembersController : CamControllerBase
    {
        private readonly TeamMembersManager _manager;
        
        public TeamMembersController(ILoggerManager logger, IHttpContextAccessor contextAccessor, TeamMembersManager manager) : base(logger, contextAccessor)
        {
            _manager = manager;   
        }

        [HttpPost("Get")]

        public async Task<QueryResultDto<TeamMemberGridDto>> Get([FromBody] TeamMemberQueryDto dto)
        {
            try
            {
                return await _manager.FindWithCondition(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpPost("Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] TeamMemberQueryDto dto)
        {
            try
            {
                return await _manager.GetFilter(propertyName, propertyFilter, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet("Create")]
        public async Task<TeamMemberCreateAndUpdateDto> Add()
        {
            try
            {
                return await _manager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        [HttpGet("Edit")]
        public async Task<TeamMemberCreateAndUpdateDto> Edit(int id)
        {
            try
            {
                return await _manager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        //[HttpPost("CreateOrUpdateTeamMembers")]
        //public async Task<ResultDto> CreateOrUpdate([FromBody] TeamMemberCreateAndUpdateDto dto)
        //{
        //    try
        //    {
        //        return await _manager.CreateOrUpdateTeams(dto);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.StackTrace);
        //        return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
        //    }
        //}
        
        [HttpDelete("DeepDelete")]
        public async Task<ResultDto> DeepDelete(int id)
        {
            try
            {
                return await _manager.DeepDelete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
