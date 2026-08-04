using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.LookUp.Asset;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.WebAPI.Identity;
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
    //#if DEBUG
    //    [Authorize]
    //#else
    //    [Authorize]
    //#endif
    public class AssetCategoryController : CamControllerBase
    {
        private readonly AssetCategoriesManager _assetCategoriesManager;
        private readonly ICurrentUserService _currentUserService;
        public AssetCategoryController(AssetCategoriesManager assetCategoriesManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _assetCategoriesManager = assetCategoriesManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<AssetCategoryDtoGrid>> GetAssetCategory([FromQuery] AssetCategoryDtoQuery assetCategoryFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _assetCategoriesManager.GetEnityGrid(assetCategoryFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while getting AssetCategory data - \n Error Message :{ex}");
                return null;

            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] AssetCategoryDtoQuery assetCategoryFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                var data = _assetCategoriesManager.GetFilter(propertyName, propertyFilter, assetCategoryFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while filtering AssetCategory data - \n Error Message :{ex}");
                return null;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] AssetCategoryDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _assetCategoriesManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating AssetCategory data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(AssetCategoryDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _assetCategoriesManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating AssetCategory data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpDelete(template:"Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _assetCategoriesManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting AssetCategory data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _assetCategoriesManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting AssetCategory data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _assetCategoriesManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while getting AssetCategory data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
            
        

        [HttpGet(template: "Create")]
        public AssetCategoryDto GetCreateResourceAssetCategories()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _assetCategoriesManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating AssetCategory data - \n Error Message :{ex}");
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public AssetCategoryDto GetUpdateResourceAssetCategories(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _assetCategoriesManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating AssetCategory data - \n Error Message :{ex}");
                return null;
            }

        }
    }
}
