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
    public class OperatingSystemController : CamControllerBase
    {
        private readonly OperatingSystemManager _OperatingSystemesManager;
        private readonly ICurrentUserService _currentUserService;

        public OperatingSystemController(OperatingSystemManager OperatingSystemesManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _OperatingSystemesManager = OperatingSystemesManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public  Task<QueryResultDto<OperatingSystemDto>> GetOperatingSystem([FromQuery] TipologicaQueryDto OperatingSystemFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _OperatingSystemesManager.GetEnityGrid(OperatingSystemFilterDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto OperatingSystemFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                var data = _OperatingSystemesManager.GetFilter(propertyName, propertyFilter, OperatingSystemFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] OperatingSystemDto OperatingSystemDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _OperatingSystemesManager.Add(OperatingSystemDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(OperatingSystemDto OperatingSystemDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _OperatingSystemesManager.Update(OperatingSystemDto);

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
                return await _OperatingSystemesManager.Delete(id);
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
                return await _OperatingSystemesManager.DeleteDeep(id);
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
                return await _OperatingSystemesManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateResourceOperatingSystem()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _OperatingSystemesManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateResourceOperatingSystem(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _OperatingSystemesManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
