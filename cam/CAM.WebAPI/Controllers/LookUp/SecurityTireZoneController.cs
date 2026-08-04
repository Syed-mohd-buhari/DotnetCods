using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.DataTransferObjects.LookUp;
using CAM.Infrastucture.QueryResult;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
        [Authorize]
#endif
    public class SecurityTireZoneController : CamControllerBase
    {
        private readonly SecurityTireZoneManager _SecurityTireZoneManager;
        private readonly ICurrentUserService _currentUserService;

        public SecurityTireZoneController(SecurityTireZoneManager SecurityTireZoneManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _SecurityTireZoneManager = SecurityTireZoneManager;
            _currentUserService = currentUserService;
        }

       


        [HttpGet]
        public  Task<QueryResultDto<TipologicaGridDto>>  GetSecurityTireZone([FromQuery] TipologicaQueryDto SecurityTireZoneFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _SecurityTireZoneManager.GetEnityGrid(SecurityTireZoneFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>>  GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto SecurityTireZoneFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                var data = _SecurityTireZoneManager.GetFilter(propertyName, propertyFilter, SecurityTireZoneFilterDto);                   
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }         

        }
              
        
        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDto SecurityTireZoneDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {                
                return await _SecurityTireZoneManager.Add(SecurityTireZoneDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
            
        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDto SecurityTireZoneDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _SecurityTireZoneManager.Update(SecurityTireZoneDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _SecurityTireZoneManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _SecurityTireZoneManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _SecurityTireZoneManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateResourceSecurityTireZone()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _SecurityTireZoneManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateResourceSecurityTireZone(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _SecurityTireZoneManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
