
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class SecurityManagerController : CamControllerBase
    {
        private readonly SecurityManagerManager _securityManagerManager;
        private readonly ICurrentUserService _currentUserService;
        public SecurityManagerController(SecurityManagerManager securityManagerManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _securityManagerManager = securityManagerManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<TipologicaGridDto>> GetSecurityManager([FromQuery] TipologicaQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _securityManagerManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }


        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/


                var data = _securityManagerManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return await _securityManagerManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return await _securityManagerManager.Update(dto);
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
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _securityManagerManager.Delete(id);
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
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _securityManagerManager.DeleteDeep(id);
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
                return await _securityManagerManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateSecurityManager()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _securityManagerManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateSecurityManager(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _securityManagerManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
