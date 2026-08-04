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
    public class ResponsibilityPhaseController : CamControllerBase
    {
        private readonly ResponsibilityPhaseManager _responsibilityPhaseManager;
        private readonly ICurrentUserService _currentUserService;

        public ResponsibilityPhaseController(ResponsibilityPhaseManager responsibilityPhaseManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _responsibilityPhaseManager = responsibilityPhaseManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public  Task<QueryResultDto<TipologicaGridDtoRule>> GetResponsibilityPhase([FromQuery] TipologicaQueryDtoRule filterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _responsibilityPhaseManager.GetEnityGrid(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDtoRule filterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return _responsibilityPhaseManager.GetFilter(propertyName, propertyFilter, filterDto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDtoRule dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _responsibilityPhaseManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDtoRule dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _responsibilityPhaseManager.Update(dto);

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
                return await _responsibilityPhaseManager.Delete(id);
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
                return await _responsibilityPhaseManager.DeleteDeep(id);
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
                return await _responsibilityPhaseManager.GetRelatedRecords(id);
            }
           
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDtoRule GetCreateResourceResponsibilityPhase()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _responsibilityPhaseManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDtoRule GetUpdateResourceResponsibilityPhase(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _responsibilityPhaseManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
