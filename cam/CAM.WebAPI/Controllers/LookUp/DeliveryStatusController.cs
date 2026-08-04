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
    public class DeliveryStatusController : CamControllerBase
    {
        private readonly DeliveryStatusManager _deliveryStatusManager;
        private readonly ICurrentUserService _currentUserService;

        public DeliveryStatusController(DeliveryStatusManager deliveryStatusManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _deliveryStatusManager = deliveryStatusManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<TipologicaGridDtoCombinationRule>> GetDeliveryStatus([FromQuery] TipologicaQueryDtoCombinationRule deliveryStatusFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _deliveryStatusManager.GetEnityGrid(deliveryStatusFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDtoCombinationRule deliveryStatusFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                var data = _deliveryStatusManager.GetFilter(propertyName, propertyFilter, deliveryStatusFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TipologicaGridDtoCombinationRule dto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return await _deliveryStatusManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(TipologicaGridDtoCombinationRule dto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return await _deliveryStatusManager.Update(dto);

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
                return await _deliveryStatusManager.Delete(id);
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
                return await _deliveryStatusManager.GetRelatedRecords(id);
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
                return await _deliveryStatusManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpGet(template: "Create")]
        public TipologicaGridDtoCombinationRule GetCreateResourceDeliveryStatus()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _deliveryStatusManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDtoCombinationRule GetUpdateResourceDeliveryStatus(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _deliveryStatusManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }
    }
}
