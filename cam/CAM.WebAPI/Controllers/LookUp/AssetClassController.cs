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
    //#if DEBUG
    //    [Authorize]
    //#else
    //        [Authorize]
    //#endif
    public class AssetClassController : CamControllerBase
    {
        private readonly AssetClassManager _AssetClassManager;
        private readonly ICurrentUserService _currentUserService;
        public AssetClassController(AssetClassManager AssetClassManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _AssetClassManager = AssetClassManager;
            _currentUserService = currentUserService;
        }


        [HttpGet]
        public Task<QueryResultDto<AssetClassDtoGrid>> GetAssetClass([FromQuery] AssetClassDtoQuery AssetClassFilterDto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _AssetClassManager.GetEnityGrid(AssetClassFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while getting AssetClass data - \n Error Message :{ex}");
                return null;

            }



        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] AssetClassDtoQuery AssetClassFilterDto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                var data = _AssetClassManager.GetFilter(propertyName, propertyFilter, AssetClassFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while filtering AssetClass data - \n Error Message :{ex}");
                return null;

            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] AssetClassDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return await _AssetClassManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating AssetClass data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(AssetClassDto dto)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return await _AssetClassManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating AssetClass data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _AssetClassManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting AssetClass data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _AssetClassManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting AssetClass data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _AssetClassManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while getting related records of AssetClass data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpGet(template: "Create")]
        public AssetClassDto GetCreateResourceAssetClass()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _AssetClassManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating AssetClass data - \n Error Message :{ex}");
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public AssetClassDto GetUpdateResourceAssetClass(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _AssetClassManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating AssetClass data - \n Error Message :{ex}");
                return null;
            }

        }
    }
}
