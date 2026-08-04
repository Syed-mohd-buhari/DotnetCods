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
    public class HardwareSolutionResourceController : CamControllerBase
    {
        private readonly HardwareSolutionResourcesManager _HardwareSolutionResourceManager;
        private readonly ICurrentUserService _currentUserService;

        public HardwareSolutionResourceController(HardwareSolutionResourcesManager HardwareSolutionResourceManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _HardwareSolutionResourceManager = HardwareSolutionResourceManager;
            _currentUserService = currentUserService;
        }

       


        [HttpGet]
        public  Task<QueryResultDto<TipologicaGridDto>>  GetHardwareSolutionResource([FromQuery] TipologicaQueryDto HardwareSolutionResourceFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _HardwareSolutionResourceManager.GetEnityGrid(HardwareSolutionResourceFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>>  GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto HardwareSolutionResourceFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                var data = _HardwareSolutionResourceManager.GetFilter(propertyName, propertyFilter, HardwareSolutionResourceFilterDto);                   
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }         

        }
              
        
        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDto HardwareSolutionResourceDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {                
                return await _HardwareSolutionResourceManager.Add(HardwareSolutionResourceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
            
        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDto HardwareSolutionResourceDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _HardwareSolutionResourceManager.Update(HardwareSolutionResourceDto);

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
                return await _HardwareSolutionResourceManager.Delete(id);
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
                return await _HardwareSolutionResourceManager.DeleteDeep(id);
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
                return await _HardwareSolutionResourceManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateResourceHardwareSolutionResource()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _HardwareSolutionResourceManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateResourceHardwareSolutionResource(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _HardwareSolutionResourceManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
