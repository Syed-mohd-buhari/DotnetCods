using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
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
 
    public class OriginalEquipmentManufacturerController : CamControllerBase
    {
        private readonly OriginalEquipmentManufacturerManager _originalEquipmentManufacturerManager;
        private readonly ICurrentUserService _currentUserService;
        public OriginalEquipmentManufacturerController(OriginalEquipmentManufacturerManager originalEquipmentManufacturerManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _originalEquipmentManufacturerManager = originalEquipmentManufacturerManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]

        public Task<QueryResultDto<TipologicaGridDto>> GetOriginalEquipmentManufacturer([FromQuery] TipologicaQueryDto filterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _originalEquipmentManufacturerManager.GetEnityGrid(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TipologicaQueryDto filterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return _originalEquipmentManufacturerManager.GetFilter(propertyName, propertyFilter, filterDto);

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
                return await _originalEquipmentManufacturerManager.Add(dto);
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
                return await _originalEquipmentManufacturerManager.Update(dto);

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
                return await _originalEquipmentManufacturerManager.Delete(id);
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
                return await _originalEquipmentManufacturerManager.DeleteDeep(id);
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
                return await _originalEquipmentManufacturerManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public TipologicaGridDto GetCreateResourceOriginalEquipmentManufacturer()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _originalEquipmentManufacturerManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TipologicaGridDto GetUpdateResourceOriginalEquipmentManufacturer(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _originalEquipmentManufacturerManager.GetUpdatePage(id);
          
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
