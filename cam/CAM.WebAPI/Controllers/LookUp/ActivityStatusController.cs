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

    //#if DEBUG
    //    [Authorize]
    //#else
    //    [Authorize]
    //#endif
    public class ActivityStatusController : CamControllerBase
    {
        private readonly ActivityStatusesManager _activityStatusesManager;
        private readonly ICurrentUserService _currentUserService;
        public ActivityStatusController(ActivityStatusesManager activityStatusesManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _activityStatusesManager = activityStatusesManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public  Task<QueryResultDto<TipologicaGridDtoCombinationRule>> GetActivityStatus([FromQuery] TipologicaQueryDtoCombinationRule activityStatusFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _activityStatusesManager.GetEnityGrid(activityStatusFilterDto);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Issue happen while getting ActivityStatus data - \n Error Message :{ex}");
                return null;
            }
            
          
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDtoCombinationRule activityStatusFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {

                var data = _activityStatusesManager.GetFilter(propertyName, propertyFilter, activityStatusFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while filtering ActivityStatus data - \n Error Message :{ex}");
                return null;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDtoCombinationRule activityStatusDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _activityStatusesManager.Add(activityStatusDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating ActivityStatus data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDtoCombinationRule activityStatusDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _activityStatusesManager.Update(activityStatusDto);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating ActivityStatus data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }



        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _activityStatusesManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting ActivityStatus data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _activityStatusesManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting ActivityStatus data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _activityStatusesManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while getting related records on ActivityStatus data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpGet(template: "Create")]
        public TipologicaGridDtoRule GetCreateResourceActivityStatus()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _activityStatusesManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating ActivityStatus data - \n Error Message :{ex}");
                return null;
            }


        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDtoRule GetUpdateResourceActivityStatus(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _activityStatusesManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating ActivityStatus data - \n Error Message :{ex}");
                return null;
            }

        }
    }
}
