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

    [Authorize]
    public class BudgetAvailabilityController : CamControllerBase
    {
        private readonly BudgetAvailabilityManager _budgetAvaibilityManager;
        private readonly ICurrentUserService _currentUserService;
        public BudgetAvailabilityController(BudgetAvailabilityManager activityStatusesManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _budgetAvaibilityManager = activityStatusesManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public  Task<QueryResultDto<TipologicaGridDtoCombinationRule>> GetBudgetAvailability([FromQuery] TipologicaQueryDtoCombinationRule activityStatusFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _budgetAvaibilityManager.GetEnityGrid(activityStatusFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }



        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDtoCombinationRule activityStatusFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {

                var data = _budgetAvaibilityManager.GetFilter(propertyName, propertyFilter, activityStatusFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDtoCombinationRule activityStatusDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _budgetAvaibilityManager.Add(activityStatusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDtoCombinationRule activityStatusDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _budgetAvaibilityManager.Update(activityStatusDto);

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
                return await _budgetAvaibilityManager.Delete(id);
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
                return await _budgetAvaibilityManager.DeleteDeep(id);
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
                return await _budgetAvaibilityManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDtoCombinationRule GetCreateResource()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _budgetAvaibilityManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDtoCombinationRule GetUpdateResource(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _budgetAvaibilityManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
