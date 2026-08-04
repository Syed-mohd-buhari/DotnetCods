using AutoMapper;
using CAM.BusinessManager.Entity.ComponentSoftware;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto.ComponentSoftware;
using CAM.Exports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.WebAPI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ComponentManufacturersController : CamControllerBase
    {
        private readonly ComponentManufacturersManager _componentManufactManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;

        public ComponentManufacturersController(ComponentManufacturersManager componentManufactManager, IMapper mapper, ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService) : base(logger, contextAccessor)
        {
            _componentManufactManager = componentManufactManager;
            _exportService = exportService;
            _mapper = mapper;
        }

        [HttpPost("GetComponentManufacturers")]
        public async Task<QueryResultDto<ComponentManufacturersGridDto>> GetComponentManufacturers([FromBody] ComponentManufacturersQueryDto filterDto)
        {
            try
            {
                return await _componentManufactManager.FindWithCondition(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] ComponentManufacturersGridDto dto)
        {

            try
            {
                return await _componentManufactManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(ComponentManufacturersGridDto dto)
        {
            try
            {
                return await _componentManufactManager.Update(dto);

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
            try
            {
                return await _componentManufactManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "CreatePage")]
        public ComponentManufacturersGridDto GetCreatePage()
        {
            try
            {
                return _componentManufactManager.GetCreatePageDetails();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "UpdatePage{id}")]
        public async Task<ComponentManufacturersGridDto> GetUpdatePage(long id)
        {
            try
            {
                return await _componentManufactManager.GetUpdatePageDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
     
        
        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter, 
            [FromBody] ComponentManufacturersQueryDto FilterDto)
        {
            try
            {
                var data = await _componentManufactManager.GetFilteredValuesAsync(propertyName, propertyFilter, FilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
      
     
       
    }
}
  