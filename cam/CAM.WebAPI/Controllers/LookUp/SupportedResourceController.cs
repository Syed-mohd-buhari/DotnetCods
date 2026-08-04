using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Mvc;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
#if DEBUG
    [Authorize]
#else
        [Authorize]
#endif
    public class SupportedResourceController : CamControllerBase
    {
        private readonly SupportedResourceManager _Manager;
        private readonly ICurrentUserService _currentUserService;

        public SupportedResourceController(SupportedResourceManager dataManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _Manager = dataManager;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public Task<QueryResultDto<TipologicaGridDtoRule>> GetSupportedResource([FromQuery] TipologicaQueryDtoRule dataFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _Manager.GetEnityGrid(dataFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDtoRule dataFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                var data = _Manager.GetFilter(propertyName, propertyFilter, dataFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> CreateSupportedResource([FromBody] TipologicaGridDtoRule dataDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _Manager.Add(dataDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> PutSupportedResource(TipologicaGridDtoRule dataDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _Manager.Update(dataDto);

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
                return await _Manager.Delete(id);
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
                return await _Manager.DeleteDeep(id);
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
                return await _Manager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDtoRule GetCreateResourceSupportedResource()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _Manager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }



        [HttpGet(template: "Update{id}")]
        public TipologicaGridDtoRule GetUpdateResourceSupportedResource(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _Manager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

    }
}