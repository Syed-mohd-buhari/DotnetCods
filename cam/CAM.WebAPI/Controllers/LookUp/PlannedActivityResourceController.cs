using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.DataTransferObjects.LookUp.PlannedActivityResourceDto;
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
    public class PlannedActivityResourceController : CamControllerBase
    {
        private readonly PlannedActivityResourceManager _plannedActivityResourceManager;
        private readonly ICurrentUserService _currentUserService;

        public PlannedActivityResourceController(PlannedActivityResourceManager plannedActivityResourceManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _plannedActivityResourceManager = plannedActivityResourceManager;
            _currentUserService = currentUserService;
        }



        [HttpGet(template: "ForDropdown")]
        public async Task<ResultDto<IDictionary<short, PlannedActivityResourceDto>>> GetPlannedActivityResourceForDropdown()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return new ResultDto<IDictionary<short, PlannedActivityResourceDto>>
                {
                    Warning = false,
                    Info = ResultMessages.GetInfoSuccess,
                    Data = await _plannedActivityResourceManager.GetResourceForDropdown()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto<IDictionary<short, PlannedActivityResourceDto>>
                {
                    Info = ResultMessages.SystemError,
                    Warning = true
                };
            }

        }
        [HttpGet]
        public Task<QueryResultDto<PlannedActivityResourceDtoGrid>> GetPlannedActivityResource([FromQuery] PlannedActivityResourceQuery filterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _plannedActivityResourceManager.GetEnityGrid(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] PlannedActivityResourceQuery filterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return _plannedActivityResourceManager.GetFilter(propertyName, propertyFilter, filterDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] PlannedActivityResourceDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _plannedActivityResourceManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(PlannedActivityResourceDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _plannedActivityResourceManager.Update(dto);

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
                return await _plannedActivityResourceManager.Delete(id);
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
                return await _plannedActivityResourceManager.DeleteDeep(id);
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
                return await _plannedActivityResourceManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public PlannedActivityResourceDto GetCreateResourcePlannedActivityResource()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _plannedActivityResourceManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Update{id}")]
        public PlannedActivityResourceDto GetUpdateResourcePlannedActivityResource(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _plannedActivityResourceManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
