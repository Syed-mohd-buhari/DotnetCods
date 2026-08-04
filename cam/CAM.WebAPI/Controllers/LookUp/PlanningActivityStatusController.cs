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
    public class PlanningActivityStatusController : CamControllerBase
    {
        private readonly PlanningActivityStatusManager _plannedActivityStatusManager;
        private readonly ICurrentUserService _currentUserService;

        public PlanningActivityStatusController(PlanningActivityStatusManager plannedActivityStatusManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _plannedActivityStatusManager = plannedActivityStatusManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<TipologicaGridDtoProjectStatusCombinationRule>> GetPlanningActivityStatus([FromQuery] TipologicaQueryDtoProjectStatusCombinationRule filterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _plannedActivityStatusManager.GetEnityGrid(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDtoProjectStatusCombinationRule filterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return _plannedActivityStatusManager.GetFilter(propertyName, propertyFilter, filterDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDtoProjectStatusCombinationRule dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _plannedActivityStatusManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDtoProjectStatusCombinationRule dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _plannedActivityStatusManager.Update(dto);

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
                return await _plannedActivityStatusManager.Delete(id);
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
                return await _plannedActivityStatusManager.DeleteDeep(id);
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
                return await _plannedActivityStatusManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDtoProjectStatusCombinationRule GetCreateResourcePlannedActivityStatus()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _plannedActivityStatusManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDtoProjectStatusCombinationRule GetUpdateResourcePlannedActivityStatus(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _plannedActivityStatusManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }
    }
}
