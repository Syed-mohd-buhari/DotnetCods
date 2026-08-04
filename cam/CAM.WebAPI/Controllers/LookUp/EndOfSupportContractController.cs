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
using CAM.Infrastucture.QueryResult;
using CAM.BusinessManager.LookUp;
using CAM.DataTransferObjects.LookUp;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]


    [Authorize]
    public class EndOfSupportContractController : CamControllerBase
    {
        private readonly EndOfSupportContractManager _endOfSupportContractManager;
        private readonly ICurrentUserService _currentUserService;
        public EndOfSupportContractController(EndOfSupportContractManager endOfSupportContractManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _endOfSupportContractManager = endOfSupportContractManager;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public Task<QueryResultDto<TipologicaGridDto>> GetEquipmentStatus([FromQuery] TipologicaQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _endOfSupportContractManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {

                var data = _endOfSupportContractManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {

                return await _endOfSupportContractManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _endOfSupportContractManager.Update(dto);

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
                return await _endOfSupportContractManager.Delete(id);
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
                return await _endOfSupportContractManager.DeleteDeep(id);
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
                return await _endOfSupportContractManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateResourceActivityStatus()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _endOfSupportContractManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateResourceActivityStatus(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _endOfSupportContractManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
