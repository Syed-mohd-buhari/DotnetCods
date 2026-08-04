using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.DataTransferObjects.LookUp.Asset;
using CAM.Infrastucture.QueryResult;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class AssetTypeController : CamControllerBase
    {
        private readonly AssetTypeManager _AssetTypeManager;
        private readonly ICurrentUserService _currentUserService;
        public AssetTypeController(AssetTypeManager AssetTypeManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _AssetTypeManager = AssetTypeManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<AssetTypeDtoGrid>> GetAssetType([FromQuery] AssetTypeDtoQuery AssetTypeFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _AssetTypeManager.GetEnityGrid(AssetTypeFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while fetching AssetType data - \n Error Message :{ex}");
                return null;
            }


        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] AssetTypeDtoQuery AssetTypeFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                var data = _AssetTypeManager.GetFilter(propertyName, propertyFilter, AssetTypeFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while filtering AssetType data - \n Error Message :{ex}");
                return null;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] AssetTypeDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return await _AssetTypeManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating AssetType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(AssetTypeDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return await _AssetTypeManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating AssetType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }




        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _AssetTypeManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting AssetType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _AssetTypeManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting AssetType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _AssetTypeManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while fetching related records of AssetType data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpGet(template: "Create")]
        public AssetTypeDto GetCreateResourceAssetType()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _AssetTypeManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating AssetType data - \n Error Message :{ex}");
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public AssetTypeDto GetUpdateResourceAssetType(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _AssetTypeManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating AssetType data - \n Error Message :{ex}");
                return null;
            }
        }
    }
}
