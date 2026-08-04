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
    public class FullOrPartialResourceController : CamControllerBase
    {
        private readonly FullOrPartialResourceManager _FullOrPartialResourceManager;
        private readonly ICurrentUserService _currentUserService;

        public FullOrPartialResourceController(FullOrPartialResourceManager FullOrPartialResourceManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _FullOrPartialResourceManager = FullOrPartialResourceManager;
            _currentUserService = currentUserService;
        }

       


        [HttpGet]
        public  Task<QueryResultDto<TipologicaGridDto>>  GetFullOrPartialResource([FromQuery] TipologicaQueryDto FullOrPartialResourceFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _FullOrPartialResourceManager.GetEnityGrid(FullOrPartialResourceFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>>  GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto FullOrPartialResourceFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                var data = _FullOrPartialResourceManager.GetFilter(propertyName, propertyFilter, FullOrPartialResourceFilterDto);                   
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }         

        }
              
        
        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDto FullOrPartialResourceDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {                
                return await _FullOrPartialResourceManager.Add(FullOrPartialResourceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
            
        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDto FullOrPartialResourceDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _FullOrPartialResourceManager.Update(FullOrPartialResourceDto);

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
                return await _FullOrPartialResourceManager.Delete(id);
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
                return await _FullOrPartialResourceManager.DeleteDeep(id);
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
                return await _FullOrPartialResourceManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateResourceFullOrPartialResource()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _FullOrPartialResourceManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateResourceFullOrPartialResource(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _FullOrPartialResourceManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
