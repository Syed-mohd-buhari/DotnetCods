
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
    public class AuthenicationTypeController : CamControllerBase
    {
        private readonly AuthenicationTypeManager _authenicationTypeManager;
        private readonly ICurrentUserService _currentUserService;
        public AuthenicationTypeController(AuthenicationTypeManager authenicationTypeManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _authenicationTypeManager = authenicationTypeManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<TipologicaGridDto>> GetAuthenicationType([FromQuery] TipologicaQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _authenicationTypeManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while fetching AuthenicationType data - \n Error Message :{ex}");
                return null;
            }


        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _authenicationTypeManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while filtering AuthenicationType data - \n Error Message :{ex}");
                return null;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return await _authenicationTypeManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating AuthenicationType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return await _authenicationTypeManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating AuthenicationType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }


        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _authenicationTypeManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting AuthenicationType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _authenicationTypeManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting AuthenicationType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _authenicationTypeManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while fetching AuthenicationType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateAuthenicationType()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _authenicationTypeManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating AuthenicationType data - \n Error Message :{ex}");
                return null;
            }

        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateAuthenicationType(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _authenicationTypeManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating AuthenicationType data - \n Error Message :{ex}");
                return null;
            }
        }
    }
}
