using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CAM.DataTransferObjects.LookUp.VodafoneName;
using System.Linq;
using CAM.BusinessManager.LookUp;
using CAM.DataTransferObjects.LookUp.SubNetworkBoundary;
using CAM.DataTransferObjects.LookUp.Category;
using Microsoft.AspNetCore.Authorization;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : CamControllerBase
    {
        private readonly CategoryManager _categoryManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;

        public CategoryController(CategoryManager categoryManager, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            ICurrentUserService currentUserService, IExportService exportService) : base(logger, contextAccessor)
        {
            _categoryManager = categoryManager;
            _currentUserService = currentUserService;
            _exportService = exportService; 
        }

        [HttpGet]
        public async Task<QueryResultDto<CategoryDtoGrid>> GetCategoriesAsync([FromQuery] CategoryDtoQuery dto)
        {
            try
            {
                return await _categoryManager.GetEnityGrid(dto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
           
        }

        [HttpGet("Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] CategoryDtoQuery dto)
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

        [HttpGet(template: "Update{id}")]
        public CategoryDtoUpdate GetUpdateResourceCategory(short id)
        {
            try
            {
                return _categoryManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("Create")]
        public CategoryDtoCreate GetCreate()
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

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _categoryManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] CategoryDtoCreate dto)
        {
            try
            {
                return await _categoryManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        
        [HttpPut]
        public async Task<ResultDto> Put(CategoryDtoUpdate categoryDtoUpdate)
        {
            try
            {
                return await _categoryManager.Update(categoryDtoUpdate);
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
                return await _categoryManager.Delete(id);
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
