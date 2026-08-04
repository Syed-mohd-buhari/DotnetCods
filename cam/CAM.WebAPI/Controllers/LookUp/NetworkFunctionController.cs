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
    public class NetworkFunctionController : CamControllerBase
    {
        private readonly NetworkFunctionManager _networkFunctionManager;
        private readonly ICurrentUserService _currentUserService;
        public NetworkFunctionController(NetworkFunctionManager networkFunctionManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _networkFunctionManager = networkFunctionManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<TipologicaGridDto>> GetNetworkFunction([FromQuery] TipologicaQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _networkFunctionManager.GetEnityGrid(dto);
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


                var data = _networkFunctionManager.GetFilter(propertyName, propertyFilter, dto);
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

                return await _networkFunctionManager.Add(dto);
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

                return await _networkFunctionManager.Update(dto);
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
                return await _networkFunctionManager.Delete(id);
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
                return await _networkFunctionManager.DeleteDeep(id);
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
                return await _networkFunctionManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateNetworkFunction()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _networkFunctionManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateNetworkFunction(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _networkFunctionManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
