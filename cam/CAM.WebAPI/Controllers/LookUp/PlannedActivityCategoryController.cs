using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.PlannedActivityCategory;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlannedActivityCategoryController : CamControllerBase
    {
        private readonly PlannedActivityCategoryManager _categoryManager;
        private readonly ICurrentUserService _currentUserService;

        public PlannedActivityCategoryController(PlannedActivityCategoryManager categoryManager, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _categoryManager = categoryManager;
            _currentUserService = currentUserService;
        }

        [HttpGet("Get")]
        public async Task<QueryResultDto<PlannedActivityCategoryDtoGrid>> GetBudgetOwneAsync([FromQuery] PlannedActivityCategoryQueryDto dto)
        {
            try
            {
                return await _categoryManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] PlannedActivityCategoryQueryDto dto)
        {
            try
            {
                var data = _categoryManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "UpdatePage{id}")]
        public PlannedActivityCategoryDtoGrid GetUpdateResourceCategory(short id)
        {
            try
            {
                return _categoryManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("CreatePage")]
        public PlannedActivityCategoryDtoGrid GetCreate()
        {
            try
            {
                return _categoryManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        //[HttpGet(template: "GetRelatedRecords{id}")]
        //public Task<ResultDto> GetRelatedRecords(short id)
        //{
        //    return _categoryManager.GetRelatedRecords(id);
        //}

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] PlannedActivityCategoryCreateDto dto)
        {
            try
            {
                return await _categoryManager.Add(dto);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        
        [HttpPut("Update")]
        public async Task<ResultDto> Put(PlannedActivityCategoryUpdateDto categoryDtoUpdate)
        {
            try
            {
                return await _categoryManager.Update(categoryDtoUpdate);
            }
            catch(Exception ex)
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
                return await _categoryManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
