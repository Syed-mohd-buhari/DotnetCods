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
    //#if DEBUG
    //    [Authorize]
    //#else
    //    [Authorize]
    //#endif
    public class ActivityDetailsController : CamControllerBase
    {
        private readonly ActivityDetailsManager _ActivityDetailsManager;
        private readonly ICurrentUserService _currentUserService;
        public ActivityDetailsController(ActivityDetailsManager ActivityDetailsManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _ActivityDetailsManager = ActivityDetailsManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<TipologicaGridDtoForVirtualized>> GetActivityDetails([FromQuery] TipologicaQueryDtoForVirtualized dto)
        {
            try
            {
                
                /*_currentUserService.UserInRole("Admin");*/

                return _ActivityDetailsManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while fetching ActivityDetails data - \n Error Message :{ex}");
                return null;

            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDtoForVirtualized dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                var data = _ActivityDetailsManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while filtering ActivityDetails data - \n Error Message :{ex}");
                return null;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDtoForVirtualized dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _ActivityDetailsManager.Add(dto);
            }
            catch (Exception ex)
            {

                _logger.LogError($"Issue happen while Creating ActivityDetails data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDtoForVirtualized dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _ActivityDetailsManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating ActivityDetails data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpDelete(template:"Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            

            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _ActivityDetailsManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting ActivityDetails data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
           

            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _ActivityDetailsManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while Deleting ActivityDetails data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try 
            {
                return await _ActivityDetailsManager.GetRelatedRecords(id);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Issue happen while getting related records on ActivityDetails data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
            
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDtoForVirtualized GetCreateResourceActivityDetails()
        {
           
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _ActivityDetailsManager.GetCreatePage();
            }
            catch(Exception ex)
            {

                _logger.LogError($"Issue happen while creating ActivityDetails data - \n Error Message :{ex}");
                return null;
            }
           
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDtoForVirtualized GetUpdateResourceActivityDetails(short id)
        {
           
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _ActivityDetailsManager.GetUpdatePage(id);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Issue happen while creating ActivityDetails data - \n Error Message :{ex}");
                return null;
            }
           
        }
    }
}
